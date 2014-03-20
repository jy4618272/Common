using Micajah.Common.Tools.UploadResourcesToFileService.MasterDataSetTableAdapters;
using Micajah.FileService.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;

namespace Micajah.Common.Tools.UploadResourcesToFileService
{
    class Program
    {
        #region Memebers

        private static List<string> s_ConnectionStrings;

        #endregion

        #region Private Properties

        private static List<string> ConnectionStrings
        {
            get
            {
                if (s_ConnectionStrings == null)
                {
                    // Gets the connection strings to the databases.
                    s_ConnectionStrings = new List<string>();

                    DatabaseTableAdapter adapter = null;
                    MasterDataSet.DatabaseDataTable table = null;

                    try
                    {
                        adapter = new DatabaseTableAdapter();

                        table = adapter.GetDatabases();

                        foreach (MasterDataSet.DatabaseRow row in table)
                        {
                            string connStr = CreateConnectionString(row.DatabaseName, row.UserName, row.Password, row.ServerName, row.ServerInstanceName, row.Port);
                            if (!s_ConnectionStrings.Contains(connStr))
                            {
                                s_ConnectionStrings.Add(connStr);
                            }
                        }
                    }
                    finally
                    {
                        if (adapter != null)
                        {
                            adapter.Dispose();
                        }

                        if (table != null)
                        {
                            table.Dispose();
                        }
                    }

                }
                return s_ConnectionStrings;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the connection string to database with specified details.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        /// <param name="userName">The user name to connect to database.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="serverName">The name of the SQL-server where database is placed.</param>
        /// <param name="serverInstanceName">The instance name of the SQL-server.</param>
        /// <param name="port">The port of the SQL-server.</param>
        /// <returns>A System.String that represents the connection string to database.</returns>
        private static string CreateConnectionString(string databaseName, string userName, string password, string serverName, string serverInstanceName, int port)
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

            sb.AppendFormat(";Initial Catalog={0};", databaseName);

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

        // Looks for the instance in all organization through all databases.
        private static MasterDataSet.InstanceRow GetInstance(Guid instanceId)
        {
            MasterDataSet.InstanceDataTable table = null;
            MasterDataSet.InstanceRow row = null;
            InstanceTableAdapter adapter = null;

            foreach (string connStr in ConnectionStrings)
            {
                try
                {
                    adapter = new InstanceTableAdapter(connStr);

                    table = adapter.GetInstance(instanceId);
                    if (table.Count > 0)
                    {
                        row = table[0];
                        break;
                    }
                }
                catch
                {
                }
                finally
                {
                    if (adapter != null)
                    {
                        adapter.Dispose();
                    }

                    if (table != null)
                    {
                        table.Dispose();
                    }
                }
            }

            return row;
        }

        private static string GetConnectionString(Guid organizationId)
        {
            string connectionString = null;
            DatabaseTableAdapter adapter = null;
            MasterDataSet.DatabaseDataTable table = null;
            MasterDataSet.DatabaseRow row = null;

            try
            {
                adapter = new DatabaseTableAdapter();

                table = adapter.GetDatabaseByOrganizationId(organizationId);
                row = table[0];

                connectionString = CreateConnectionString(row.DatabaseName, row.UserName, row.Password, row.ServerName, row.ServerInstanceName, row.Port);
            }
            finally
            {
                if (adapter != null)
                {
                    adapter.Dispose();
                }

                if (table != null)
                {
                    table.Dispose();
                }
            }

            return connectionString;
        }

        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("Tool to upload the files from Mc_Resource table to File Service.\r\n");

            int totalCount = 0;
            int successCount = 0;

            QueriesTableAdapter queryAdapter = null;
            ResourceTableAdapter resourceAdapter = null;
            MasterDataSet.ResourceDataTable resourceTable = null;

            try
            {
                queryAdapter = new QueriesTableAdapter();
                resourceAdapter = new ResourceTableAdapter();

                // Reads configuration.
                List<string> objectTypesWithPublicAccess = new List<string>(ConfigurationManager.AppSettings["ObjectTypesWithPublicAccess"].Split(','));
                string applicationId = Micajah.FileService.Client.Properties.Settings.Default.ApplicationId.ToString();

                // Gets top 1000 files from the database.
                resourceTable = resourceAdapter.GetResources();
                int rowsCount = resourceTable.Count;
                int fileIndex = 0;

                while (rowsCount > 0)
                {
                    totalCount += rowsCount;

                    foreach (MasterDataSet.ResourceRow row in resourceTable)
                    {
                        int uploadStatus = 0; // 0 - Failed, 1 - Success, NULL - Not processed.
                        fileIndex++;

                        try
                        {
                            // Gets the file from database.
                            Console.WriteLine("Resource #{0} \"{1}\" (resourceId = {2:N}).", fileIndex, row.Name, row.ResourceId);
                            Console.Write("Getting from database...");

                            byte[] content = resourceAdapter.GetContent(row.ResourceId);

                            Console.Write(" Done. ");
                            Console.Write("Uploading to File Service...");

                            Guid objectId = new Guid(row.LocalObjectId);
                            string objectType = null;
                            string name = null;
                            string connectionString = null;

                            switch (row.LocalObjectType)
                            {
                                case "InstanceLogo":
                                    objectType = "instance-logo";

                                    MasterDataSet.InstanceRow instanceRow = GetInstance(objectId);
                                    if (instanceRow == null)
                                    {
                                        throw new ApplicationException(string.Format(CultureInfo.InvariantCulture, "Instance with Id = {0} not found.", objectId));
                                    }

                                    name = instanceRow.Name;
                                    connectionString = GetConnectionString(instanceRow.OrganizationId);
                                    break;
                                case "OrganizationLogo":
                                    objectType = "organization-logo";

                                    name = queryAdapter.GetOrganiationName(objectId);
                                    connectionString = GetConnectionString(objectId);
                                    break;
                            }

                            bool publicAccess = objectTypesWithPublicAccess.Contains(objectType);

                            // Uploads to File Service.
                            string checksum = null;
                            string organizationId = objectId.ToString("N");
                            string departmentId = objectId.ToString("N");

                            string result = Access.PutFileAsByteArray(applicationId, name, ref organizationId, name, ref departmentId, row.Name, ref content, publicAccess, ref checksum
                                , objectId.ToString("N"), objectType, string.Empty, connectionString);

                            if (!Access.StringIsFileUniqueId(result))
                            {
                                throw new ApplicationException(result);
                            }

                            uploadStatus = 1;
                            successCount++;

                            Console.WriteLine(" Done.\r\n");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(" Failed.\r\n{0}\r\n", ex.ToString());
                        }

                        // Updates upload status of the file in the database.
                        resourceAdapter.UpdateUploadStatus(row.ResourceId, uploadStatus);
                    }

                    // Gets next 1000 files from the database.
                    resourceTable = resourceAdapter.GetResources();
                    rowsCount = resourceTable.Count;
                }

                Console.WriteLine(@"
Total files found: {0}
Uploaded: {1}
Failed: {2}"
                    , totalCount, successCount, totalCount - successCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\r\n{0}\r\n", ex.ToString());
            }
            finally
            {
                if (queryAdapter != null)
                {
                    queryAdapter.Dispose();
                }

                if (resourceAdapter != null)
                {
                    resourceAdapter.Dispose();
                }

                if (resourceTable != null)
                {
                    resourceTable.Dispose();
                }
            }

            Console.WriteLine("\r\nPress any key to quit.");

            Console.ReadKey();
        }
    }
}
