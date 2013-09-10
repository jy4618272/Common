using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
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
        #region Internal Methods

        /// <summary>
        /// Returns the full name of the specified SQL-server, incuding instance name and port.
        /// </summary>
        /// <param name="databaseServerId">Specifies the SQL-server's identifier.</param>
        /// <returns>The System.String that represents the full name of the specified SQL-server, incuding instance name and port.</returns>
        internal static string GetDatabaseServerFullName(Guid databaseServerId)
        {
            CommonDataSet.DatabaseServerRow row = WebApplication.CommonDataSet.DatabaseServer.FindByDatabaseServerId(databaseServerId);
            return ((row == null) ? string.Empty : row.FullName);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the SQL-servers, excluding marked as deleted.
        /// </summary>
        /// <returns>The DataTable that contains SQL-servers.</returns>
        public static DataTable GetDatabaseServers()
        {
            return WebApplication.CommonDataSet.DatabaseServer;
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
        public static CommonDataSet.DatabaseServerRow GetDatabaseServerRow(Guid databaseServerId)
        {
            return WebApplication.CommonDataSet.DatabaseServer.FindByDatabaseServerId(databaseServerId);
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
        public static void InsertDatabaseServer(string name, string instanceName, int port, string description, Guid websiteId)
        {
            CommonDataSet.DatabaseServerRow row = WebApplication.CommonDataSet.DatabaseServer.NewDatabaseServerRow();

            row.DatabaseServerId = Guid.NewGuid();
            row.Name = name;
            row.InstanceName = instanceName;
            row.Port = port;
            row.Description = description;
            row.WebsiteId = websiteId;

            WebApplication.CommonDataSet.DatabaseServer.AddDatabaseServerRow(row);
            MasterTableAdapters.Current.DatabaseServerTableAdapter.Update(row);
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
            CommonDataSet.DatabaseServerRow row = WebApplication.CommonDataSet.DatabaseServer.FindByDatabaseServerId(databaseServerId);
            if (row == null) return;

            row.Name = name;
            row.InstanceName = instanceName;
            row.Port = port;
            row.Description = description;
            row.WebsiteId = websiteId;

            MasterTableAdapters.Current.DatabaseServerTableAdapter.Update(row);
        }

        /// <summary>
        /// Marks as deleted the specified SQL-server.
        /// </summary>
        /// <param name="databaseServerId">Specifies the SQL-server's identifier.</param>
        public static void DeleteDatabaseServer(Guid databaseServerId)
        {
            CommonDataSet.DatabaseServerRow row = WebApplication.CommonDataSet.DatabaseServer.FindByDatabaseServerId(databaseServerId);
            if (row == null) return;

            row.Deleted = true;

            MasterTableAdapters.Current.DatabaseServerTableAdapter.Update(row);
            WebApplication.CommonDataSet.DatabaseServer.RemoveDatabaseServerRow(row);
        }

        #endregion
    }
}
