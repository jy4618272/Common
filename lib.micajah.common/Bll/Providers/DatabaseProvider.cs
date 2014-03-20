using Micajah.Common.Dal;
using Micajah.Common.Dal.MasterDataSetTableAdapters;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with databases.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class DatabaseProvider
    {
        #region Public Properties

        /// <summary>
        /// Gets the identifier of the database with minimal number of organizations.
        /// </summary>
        public static Guid UnloadedDatabaseId
        {
            get
            {
                using (DatabaseTableAdapter adapter = new DatabaseTableAdapter())
                {
                    MasterDataSet.DatabaseDataTable table = adapter.GetDatabaseByMinimalNumberOfOrganizations();
                    return ((table.Count > 0) ? table[0].DatabaseId : Guid.Empty);
                }
            }
        }

        #endregion

        #region Private Methods

        private static MasterDataSet.DatabaseRow GetDatabaseRowByDatabaseId(Guid databaseId)
        {
            using (DatabaseTableAdapter adapter = new DatabaseTableAdapter())
            {
                MasterDataSet.DatabaseDataTable table = adapter.GetDatabaseByDatabaseId(databaseId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        private static MasterDataSet.DatabaseRow GetDatabaseRowByOrganizationId(Guid organizationId)
        {
            using (DatabaseTableAdapter adapter = new DatabaseTableAdapter())
            {
                MasterDataSet.DatabaseDataTable table = adapter.GetDatabaseByOrganizationId(organizationId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        private static void IncludeAdditionalInfo(ref DataTable table)
        {
            table.Columns.Add("DatabaseServerFullName", typeof(string));
            table.Columns.Add("FullName", typeof(string));
            table.Columns["FullName"].Expression = "'[' + DatabaseServerFullName + '] \\ [' + Name + ']'";

            foreach (DataRow row in table.Rows)
            {
                row["DatabaseServerFullName"] = DatabaseServerProvider.GetDatabaseServerFullName((Guid)row["DatabaseServerId"]);
            }
        }

        private static DataTable GetPublicDatabases(Guid organizationId, bool includeAdditionalInfo)
        {
            DataTable table = new MasterDataSet.DatabaseDataTable();

            foreach (MasterDataSet.DatabaseRow row in GetDatabases())
            {
                if (row.Private)
                {
                    if (organizationId != Guid.Empty)
                    {
                        Organization org = OrganizationProvider.GetOrganization(organizationId);
                        if (org != null)
                        {
                            if (org.DatabaseId == row.DatabaseId)
                                table.ImportRow(row);
                        }
                    }
                }
                else
                    table.ImportRow(row);
            }

            if (includeAdditionalInfo)
                IncludeAdditionalInfo(ref table);

            table.AcceptChanges();

            return table;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Checks if the database exists.
        /// </summary>
        /// <param name="name">The database name.</param>
        /// <param name="userName">The user name to connect to database.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="databaseServerId">The identifier of the the SQL-server where database is placed.</param>
        /// <param name="errorMessage">The error message if an error occured.</param>
        /// <returns>true, if the database exists; otherwise, false.</returns>
        internal static bool DatabaseExists(string name, string userName, string password, Guid databaseServerId, out string errorMessage)
        {
            bool success = false;
            errorMessage = string.Empty;
            MasterDataSet.DatabaseServerRow row = DatabaseServerProvider.GetDatabaseServerRow(databaseServerId);
            if (row != null)
            {
                string connectionString = CreateConnectionString(name, userName, password, row.Name, row.InstanceName, row.Port);
                SqlConnection connection = null;
                try
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();

                    success = true;
                }
                catch (SqlException ex)
                {
                    errorMessage = ex.Message;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Dispose();
                    }
                }
            }
            return success;
        }

        /// <summary>
        /// Returns the name of the specified database.
        /// </summary>
        /// <param name="databaseId">Specifies the database identifier.</param>
        /// <returns>The System.String that represents the name of the specified database.</returns>
        internal static string GetDatabaseName(Guid databaseId)
        {
            MasterDataSet.DatabaseRow row = GetDatabaseRow(databaseId);
            return ((row == null) ? string.Empty : row.Name);
        }

        internal static Guid GetRandomPublicDatabaseId()
        {
            Guid databaseId = Guid.Empty;

            DataTable table = GetPublicDatabases(Guid.Empty, false);
            if (table.Rows.Count > 0)
            {
                int rowIndex = 0;
                if (table.Rows.Count > 1)
                {
                    Random rnd = new Random();
                    rowIndex = Convert.ToInt32(Math.Round((decimal)rnd.Next(0, (table.Rows.Count - 1) * 10) / 10.0m));
                }
                databaseId = (Guid)table.Rows[rowIndex]["DatabaseId"];
            }

            return databaseId;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the connection string to database with specified details.
        /// </summary>
        /// <param name="name">The database name.</param>
        /// <param name="userName">The user name to connect to database.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="serverName">The name of the SQL-server where database is placed.</param>
        /// <param name="serverInstanceName">The instance name of the SQL-server.</param>
        /// <param name="port">The port of the SQL-server.</param>
        /// <returns>A System.String that represents the connection string to database.</returns>
        public static string CreateConnectionString(string name, string userName, string password, string serverName, string serverInstanceName, int port)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Data Source={0}", serverName);

            if (!string.IsNullOrWhiteSpace(serverInstanceName))
            {
                sb.AppendFormat("\\{0}", serverInstanceName);
            }

            if (port > 0)
            {
                sb.AppendFormat(",{0}", port);
            }

            sb.AppendFormat(";Initial Catalog={0};", name);

            if ((!string.IsNullOrWhiteSpace(userName)) && (!string.IsNullOrWhiteSpace(password)))
            {
                sb.AppendFormat("User ID={0};Password={1};", userName, password);
            }
            else
            {
                sb.Append("Integrated Security=SSPI;");
            }

            sb.Append("Persist Security Info=True;");

            return sb.ToString();
        }

        /// <summary>
        /// Gets the databases, excluding marked as deleted.
        /// </summary>
        /// <returns>The table that contains databases.</returns>
        public static MasterDataSet.DatabaseDataTable GetDatabases()
        {
            using (DatabaseTableAdapter adapter = new DatabaseTableAdapter())
            {
                return adapter.GetDatabases();
            }
        }

        /// <summary>
        /// Gets the databases with additional information (for example, SQL-Server full name, etc.).
        /// </summary>
        /// <param name="includeDatabaseServerName">The flag indicating that the additional information is included in result.</param>
        /// <returns>The System.Data.DataTable that contains databases.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetDatabases(bool includeAdditionalInfo)
        {
            if (includeAdditionalInfo)
            {
                DataTable table = GetDatabases();
                IncludeAdditionalInfo(ref table);
                return table;
            }
            else
                return GetDatabases();
        }

        /// <summary>
        /// Gets the public databases with additional information (for example, SQL-Server full name, etc.).
        /// </summary>
        /// <returns>The System.Data.DataTable that contains databases.</returns>
        public static DataTable GetPublicDatabases(Guid organizationId)
        {
            return GetPublicDatabases(organizationId, true);
        }

        /// <summary>
        /// Gets an object populated with information of the specified database.
        /// </summary>
        /// <param name="databaseId">Specifies the database identifier to get information.</param>
        /// <returns>
        /// The object populated with information of the specified database. 
        /// If the database is not found, the method returns null reference.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MasterDataSet.DatabaseRow GetDatabaseRow(Guid databaseId)
        {
            using (DatabaseTableAdapter adapter = new DatabaseTableAdapter())
            {
                MasterDataSet.DatabaseDataTable table = adapter.GetDatabase(databaseId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        /// <summary>
        /// Creates new database with specified details.
        /// </summary>
        /// <param name="name">The name of the database.</param>
        /// <param name="description">The database description.</param>
        /// <param name="userName">The name of the database user.</param>
        /// <param name="password">The password of database user.</param>
        /// <param name="Private">The flag indicated the database is private and can be used only by one organization.</param>
        /// <param name="databaseServerId">The identifier of the SQL-server which the database belong to.</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertDatabase(string name, string description, string userName, string password, bool Private, Guid databaseServerId)
        {
            Guid databaseId = Guid.NewGuid();

            using (DatabaseTableAdapter adapter = new DatabaseTableAdapter())
            {
                adapter.Insert(databaseId, name, description, userName, password, databaseServerId, Private, false);
            }

            return databaseId;
        }

        /// <summary>
        /// Updates the details of specified database.
        /// </summary>
        /// <param name="databaseId">The identifier of the database.</param>
        /// <param name="name">The name of the database.</param>
        /// <param name="description">The database description.</param>
        /// <param name="userName">The name of the database user.</param>
        /// <param name="password">The password of database user.</param>
        /// <param name="Private">The flag indicated the database is private and can be used only by one organization.</param>
        /// <param name="databaseServerId">The identifier of the SQL-server which the database belong to.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateDatabase(Guid databaseId, string name, string description, string userName, string password, bool Private, Guid databaseServerId)
        {
            using (DatabaseTableAdapter adapter = new DatabaseTableAdapter())
            {
                adapter.Update(databaseId, name, description, userName, password, databaseServerId, Private, false);
            }
        }

        /// <summary>
        /// Marks as deleted the specified database.
        /// </summary>
        /// <param name="databaseId">Specifies the database's identifier.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteDatabase(Guid databaseId)
        {
            MasterDataSet.DatabaseRow row = GetDatabaseRow(databaseId);
            if (row == null) return;

            row.Deleted = true;

            using (DatabaseTableAdapter adapter = new DatabaseTableAdapter())
            {
                adapter.Update(row);
            }
        }

        /// <summary>
        /// Returns the connection string to the specified database.
        /// </summary>
        /// <param name="databaseId">The identifier of the database.</param>
        /// <returns>The connection string to the specified database.</returns>
        public static string GetConnectionString(Guid databaseId)
        {
            MasterDataSet.DatabaseRow row = GetDatabaseRowByDatabaseId(databaseId);
            if (row != null)
                return CreateConnectionString(row.Name, row.UserName, row.Password, row["DatabaseServerName"].ToString(), row["InstanceName"].ToString(), (int)row["Port"]);
            return string.Empty;
        }

        public static string GetConnectionStringByOrganizationId(Guid organizationId)
        {
            MasterDataSet.DatabaseRow row = GetDatabaseRowByOrganizationId(organizationId);
            if (row != null)
                return CreateConnectionString(row.Name, row.UserName, row.Password, row["DatabaseServerName"].ToString(), row["InstanceName"].ToString(), (int)row["Port"]);
            return string.Empty;
        }

        #endregion
    }
}
