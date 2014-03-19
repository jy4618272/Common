using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Tools.UploadResourcesToFileService.MasterDataSetTableAdapters;
using Micajah.FileService.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace Micajah.Common.Tools.UploadResourcesToFileService
{
    class Program
    {
        private static Instance GetInstance(Guid instanceId)
        {
            Instance instance = null;

            using (Micajah.Common.Dal.MasterDataSet.OrganizationDataTable table = OrganizationProvider.GetOrganizations(true))
            {
                foreach (Micajah.Common.Dal.MasterDataSet.OrganizationRow row in table)
                {
                    instance = InstanceProvider.GetInstance(instanceId, row.OrganizationId);
                    if (instance != null)
                    {
                        break;
                    }
                }
            }

            return instance;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Tool to upload the files from Mc_Resource table to File Service.\r\n");

            int totalCount = 0;
            int successCount = 0;

            ResourceTableAdapter adapter = null;
            MasterDataSet.ResourceDataTable table = null;

            try
            {
                adapter = new ResourceTableAdapter();

                // Read configuration.
                List<string> objectTypesWithPublicAccess = new List<string>(ConfigurationManager.AppSettings["ObjectTypesWithPublicAccess"].Split(','));
                string applicationId = Micajah.FileService.Client.Properties.Settings.Default.ApplicationId.ToString();

                int fileIndex = 0;
                int rowsCount = 0;

                // Get top 1000 files from the database.
                table = adapter.GetResources();
                rowsCount = table.Count;

                while (rowsCount > 0)
                {
                    totalCount += rowsCount;

                    foreach (MasterDataSet.ResourceRow row in table)
                    {
                        int uploadStatus = 0; // 0 - Failed, 1 - Success, NULL - Not processed.
                        fileIndex++;

                        try
                        {
                            // Get the file from database.
                            Console.WriteLine("Resource #{0} \"{1}\" (resourceId = {2:N}).", fileIndex, row.Name, row.ResourceId);
                            Console.Write("Getting from database...");

                            byte[] content = adapter.GetContent(row.ResourceId);

                            Console.Write(" Done. ");
                            Console.Write("Uploading to File Service...");

                            Guid objectId = new Guid(row.LocalObjectId);
                            string objectType = null;
                            string name = null;
                            string connectionString = null;

                            switch (row.LocalObjectType)
                            {
                                case "InstanceLogo":
                                    Instance instance = GetInstance(objectId);
                                    if (instance == null)
                                    {
                                        throw new ApplicationException(string.Format(CultureInfo.InvariantCulture, "Instance with Id = {0:N} not found.", objectId));
                                    }

                                    objectType = "instance-logo";
                                    name = instance.Name;
                                    connectionString = OrganizationProvider.GetConnectionString(instance.OrganizationId);
                                    break;
                                case "OrganizationLogo":
                                    objectType = "organization-logo";
                                    name = OrganizationProvider.GetName(objectId);
                                    connectionString = OrganizationProvider.GetConnectionString(objectId);
                                    break;
                            }

                            bool publicAccess = objectTypesWithPublicAccess.Contains(objectType);

                            // Upload to File Service.
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

                        // Update upload status of the file in the database.
                        adapter.UpdateUploadStatus(row.ResourceId, uploadStatus);
                    }

                    // Get next 1000 files from the database.
                    table = adapter.GetResources();
                    rowsCount = table.Count;
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
                if (adapter != null)
                {
                    adapter.Dispose();
                }

                if (table != null)
                {
                    table.Dispose();
                }
            }

            Console.WriteLine("\r\nPress any key to quit.");

            Console.ReadKey();
        }
    }
}
