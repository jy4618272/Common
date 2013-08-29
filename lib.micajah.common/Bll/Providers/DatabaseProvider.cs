using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.Properties;

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
                Guid databaseId = Guid.Empty;
                int count = 0;
                int previousCount = -1;

                foreach (CommonDataSet.DatabaseRow row in WebApplication.CommonDataSet.Database.Rows)
                {
                    count = row.GetOrganizationRows().Length;
                    if (previousCount == -1)
                        previousCount = count;
                    else if (count >= previousCount)
                        continue;
                    databaseId = row.DatabaseId;
                }

                return databaseId;
            }
        }

        #endregion

        #region Private Methods

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
            DataTable table = WebApplication.CommonDataSet.Database.Clone();

            foreach (CommonDataSet.DatabaseRow row in WebApplication.CommonDataSet.Database)
            {
                if (row.Private)
                {
                    if (organizationId != Guid.Empty)
                    {
                        foreach (CommonDataSet.OrganizationRow orgRow in row.GetOrganizationRows())
                        {
                            if (orgRow.OrganizationId == organizationId)
                            {
                                table.ImportRow(row);
                                break;
                            }
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
        /// Returns the connection string to database with specified details.
        /// </summary>
        /// <param name="name">The database name.</param>
        /// <param name="userName">The user name to connect to database.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="serverName">The name of the SQL-server where database is placed.</param>
        /// <param name="serverInstanceName">The instance name of the SQL-server.</param>
        /// <param name="port">The port of the SQL-server.</param>
        /// <returns>A System.String that represents the connection string to database.</returns>
        internal static string CreateConnectionString(string name, string userName, string password,
            string serverName, string serverInstanceName, int port)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Data Source={0}", serverName);
            if (serverInstanceName.Trim().Length > 0) sb.AppendFormat("\\{0}", serverInstanceName);
            if (port > 0) sb.AppendFormat(",{0}", port);
            sb.AppendFormat(";Initial Catalog={0};", name);
            if (userName.Trim().Length > 0 && password.Trim().Length > 0)
                sb.AppendFormat("User ID={0};Password={1};", userName, password);
            else
                sb.Append("Integrated Security=SSPI;");
            sb.Append("Persist Security Info=True;");

            return sb.ToString();
        }

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
            CommonDataSet.DatabaseServerRow row = WebApplication.CommonDataSet.DatabaseServer.FindByDatabaseServerId(databaseServerId);
            if (row != null)
            {
                string connectionString = CreateConnectionString(name, userName, password, row.Name, row.InstanceName, row.Port);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        success = true;
                        connection.Close();
                    }
                    catch (SqlException ex)
                    {
                        errorMessage = ex.Message;
                    }
                }
            }
            return success;
        }

        /// <summary>
        /// Returns the SQL-server full name, where the specified database is placed.
        /// </summary>
        /// <param name="databaseId">Specifies the database identifier.</param>
        /// <returns>The System.String that represents the SQL-server full name, where the specified database is placed.</returns>
        internal static string GetDatabaseServerFullName(Guid databaseId)
        {
            CommonDataSet.DatabaseRow row = WebApplication.CommonDataSet.Database.FindByDatabaseId(databaseId);
            return ((row == null) ? string.Empty : row.DatabaseServerRow.FullName);
        }

        /// <summary>
        /// Returns the name of the specified database.
        /// </summary>
        /// <param name="databaseId">Specifies the database identifier.</param>
        /// <returns>The System.String that represents the name of the specified database.</returns>
        internal static string GetDatabaseName(Guid databaseId)
        {
            CommonDataSet.DatabaseRow row = WebApplication.CommonDataSet.Database.FindByDatabaseId(databaseId);
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
        /// Gets the databases, excluding marked as deleted.
        /// </summary>
        /// <returns>The System.Data.DataTable that contains databases.</returns>
        public static DataTable GetDatabases()
        {
            return WebApplication.CommonDataSet.Database;
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
                DataTable table = WebApplication.CommonDataSet.Database.Copy();
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
        public static CommonDataSet.DatabaseRow GetDatabaseRow(Guid databaseId)
        {
            return WebApplication.CommonDataSet.Database.FindByDatabaseId(databaseId);
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
        public static void InsertDatabase(string name, string description, string userName, string password, bool Private, Guid databaseServerId)
        {
            CommonDataSet.DatabaseRow row = WebApplication.CommonDataSet.Database.NewDatabaseRow();

            row.DatabaseId = Guid.NewGuid();
            row.Name = name;
            row.Description = description;
            row.UserName = userName;
            row.Password = password;
            row.Private = Private;
            row.DatabaseServerId = databaseServerId;

            WebApplication.CommonDataSet.Database.AddDatabaseRow(row);
            WebApplication.CommonDataSetTableAdapters.DatabaseTableAdapter.Update(row);
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
            CommonDataSet.DatabaseRow row = WebApplication.CommonDataSet.Database.FindByDatabaseId(databaseId);
            if (row == null) return;

            row.Name = name;
            row.Description = description;
            row.UserName = userName;
            row.Password = password;
            row.Private = Private;
            row.DatabaseServerId = databaseServerId;

            WebApplication.CommonDataSetTableAdapters.DatabaseTableAdapter.Update(row);
        }

        /// <summary>
        /// Marks as deleted the specified database.
        /// </summary>
        /// <param name="databaseId">Specifies the database's identifier.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteDatabase(Guid databaseId)
        {
            CommonDataSet.DatabaseRow row = WebApplication.CommonDataSet.Database.FindByDatabaseId(databaseId);
            if (row == null) return;

            row.Deleted = true;

            WebApplication.CommonDataSetTableAdapters.DatabaseTableAdapter.Update(row);
            WebApplication.CommonDataSet.Database.RemoveDatabaseRow(row);
        }

        /// <summary>
        /// Returns the connection string to the specified database.
        /// Generates an System.Data.DataException exception if error occured.
        /// </summary>
        /// <param name="databaseId">The identifier of the database.</param>
        /// <returns>The connection string to the specified database.</returns>
        public static string GetConnectionString(Guid databaseId)
        {
            string connectionString = string.Empty;

            WebApplication.RefreshCommonData();

            CommonDataSet ds = WebApplication.CommonDataSet;
            CommonDataSet.DatabaseRow db = ds.Database.FindByDatabaseId(databaseId);
            if (db != null)
            {
                Guid databaseServerId = db.DatabaseServerId;
                CommonDataSet.DatabaseServerRow server = ds.DatabaseServer.FindByDatabaseServerId(databaseServerId);
                if (server != null)
                    connectionString = DatabaseProvider.CreateConnectionString(db.Name, db.UserName, db.Password, server.Name, server.InstanceName, server.Port);
                else
                    throw new DataException(string.Format(CultureInfo.CurrentCulture, Resources.DatabaseServerProvider_ErrorMessage_NoDatabaseServer, databaseServerId));
            }
            else
                throw new DataException(string.Format(CultureInfo.CurrentCulture, Resources.DatabaseProvider_ErrorMessage_NoDatabase, databaseId));

            return connectionString;
        }

        #endregion
    }
}
