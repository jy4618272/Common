using Micajah.Common.Dal;
using Micajah.Common.Dal.MasterDataSetTableAdapters;
using System;
using System.ComponentModel;
using System.Data;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with SQL-servers.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class DatabaseServerProvider
    {
        #region Private Methods

        private static MasterDataSet.DatabaseServerRow GetDatabaseServerRowByDatabaseId(Guid databaseId)
        {
            using (DatabaseServerTableAdapter adapter = new DatabaseServerTableAdapter())
            {
                MasterDataSet.DatabaseServerDataTable table = adapter.GetDatabaseServerByDatabaseId(databaseId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns the full name of the specified SQL-server, incuding instance name and port.
        /// </summary>
        /// <param name="databaseServerId">Specifies the SQL-server's identifier.</param>
        /// <returns>The System.String that represents the full name of the specified SQL-server, incuding instance name and port.</returns>
        internal static string GetDatabaseServerFullName(Guid databaseServerId)
        {
            MasterDataSet.DatabaseServerRow row = GetDatabaseServerRow(databaseServerId);
            return ((row == null) ? string.Empty : row.FullName);
        }


        /// <summary>
        /// Returns the SQL-server full name, where the specified database is placed.
        /// </summary>
        /// <param name="databaseId">Specifies the database identifier.</param>
        /// <returns>The System.String that represents the SQL-server full name, where the specified database is placed.</returns>
        internal static string GetDatabaseServerFullNameByDatabaseId(Guid databaseId)
        {
            MasterDataSet.DatabaseServerRow row = GetDatabaseServerRowByDatabaseId(databaseId);
            return ((row == null) ? string.Empty : row.FullName);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the SQL-servers, excluding marked as deleted.
        /// </summary>
        /// <returns>The DataTable that contains SQL-servers.</returns>
        public static MasterDataSet.DatabaseServerDataTable GetDatabaseServers()
        {
            using (DatabaseServerTableAdapter adapter = new DatabaseServerTableAdapter())
            {
                return adapter.GetDatabaseServers();
            }
        }

        /// <summary>
        /// Gets an object populated with information of the specified SQL-server.
        /// </summary>
        /// <param name="databaseServerId">Specifies the SQL-server identifier to get information.</param>
        /// <returns>
        /// The object populated with information of the specified SQL-server. 
        /// If the SQL-server is not found, the method returns null reference.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MasterDataSet.DatabaseServerRow GetDatabaseServerRow(Guid databaseServerId)
        {
            using (DatabaseServerTableAdapter adapter = new DatabaseServerTableAdapter())
            {
                MasterDataSet.DatabaseServerDataTable table = adapter.GetDatabaseServer(databaseServerId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        /// <summary>
        /// Creates new SQL-server with specified details.
        /// </summary>
        /// <param name="name">The name of the SQL-server.</param>
        /// <param name="instanceName">The instance name of the SQL-server.</param>
        /// <param name="port">The port of the SQL-server.</param>
        /// <param name="description">The SQL-server description.</param>
        /// <param name="websiteId">The identifier of a web site where the SQL-server is placed.</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertDatabaseServer(string name, string instanceName, int port, string description, Guid websiteId)
        {
            Guid databaseServerId = Guid.NewGuid();

            using (DatabaseServerTableAdapter adapter = new DatabaseServerTableAdapter())
            {
                adapter.Insert(databaseServerId, name, instanceName, port, description, websiteId, false);
            }

            return databaseServerId;
        }

        /// <summary>
        /// Updates the details of specified SQL-server.
        /// </summary>
        /// <param name="databaseServerId">The identifier of the SQL-server.</param>
        /// <param name="name">The name of the SQL-server.</param>
        /// <param name="instanceName">The instance name of the SQL-server.</param>
        /// <param name="port">The port of the SQL-server.</param>
        /// <param name="description">The SQL-server description.</param>
        /// <param name="websiteId">The identifier of a web site where the SQL-server is placed.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateDatabaseServer(Guid databaseServerId, string name, string instanceName, int port, string description, Guid websiteId)
        {
            using (DatabaseServerTableAdapter adapter = new DatabaseServerTableAdapter())
            {
                adapter.Update(databaseServerId, name, instanceName, port, description, websiteId, false);
            }
        }

        /// <summary>
        /// Marks as deleted the specified SQL-server.
        /// </summary>
        /// <param name="databaseServerId">Specifies the SQL-server's identifier.</param>
        public static void DeleteDatabaseServer(Guid databaseServerId)
        {
            MasterDataSet.DatabaseServerRow row = GetDatabaseServerRow(databaseServerId);
            if (row == null) return;

            row.Deleted = true;

            using (DatabaseServerTableAdapter adapter = new DatabaseServerTableAdapter())
            {
                adapter.Update(row);
            }
        }

        #endregion
    }
}
