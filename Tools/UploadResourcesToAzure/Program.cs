using Micajah.Common.Tools.UploadResourcesToAzure.MasterDataSetTableAdapters;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace Micajah.Common.Tools.UploadResourcesToAzure
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tool to upload the files from Mc_Resource table to Windows Azure Blob Storage.\r\n");

            int totalCount = 0;
            int successCount = 0;

            ResourceTableAdapter adapter = null;
            MasterDataSet.ResourceDataTable table = null;

            try
            {
                adapter = new ResourceTableAdapter();

                // Read configuration.
                string cacheControl = string.Format(CultureInfo.InvariantCulture, "public, max-age={0}", ConfigurationManager.AppSettings["mafs:ClientCacheExpiryTime"]);
                int uploadSpeedLimit = Convert.ToInt32(ConfigurationManager.AppSettings["UploadSpeedLimit"]);
                int parallelOperationThreadCount = Convert.ToInt32(ConfigurationManager.AppSettings["ParallelOperationThreadCount"]);
                string storageConnectionString = ConfigurationManager.AppSettings["mafs:StorageConnectionString"];
                List<string> objectTypesWithPublicAccess = new List<string>(ConfigurationManager.AppSettings["ObjectTypesWithPublicAccess"].Split(','));

                // Initialize and configure the storage and client.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                blobClient.ParallelOperationThreadCount = parallelOperationThreadCount;
                blobClient.SingleBlobUploadThresholdInBytes = uploadSpeedLimit;

                CloudBlobContainer container = null;
                CloudBlobContainer publicContainer = null;
                Guid currentContainerId = Guid.Empty;
                string currentObjectType = null;
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
                            // Download the file from File Service.
                            Console.WriteLine("Resource #{0} \"{1}\" (resourceId = {2:N}).", fileIndex, row.Name, row.ResourceId);
                            Console.Write("Getting from database...");

                            byte[] content = adapter.GetContent(row.ResourceId);

                            Console.Write(" Done. ");
                            Console.Write("Uploading to Azure...");

                            // Get the corresponding container for the file and create it if not exists.
                            Guid objectId = new Guid(row.LocalObjectId);

                            string objectType = null;
                            switch (row.LocalObjectType)
                            {
                                case "InstanceLogo":
                                    objectType = "instance-logo";
                                    break;
                                case "OrganizationLogo":
                                    objectType = "organization-logo";
                                    break;
                            }

                            if ((objectId != currentContainerId) || (string.Compare(objectType, currentObjectType, StringComparison.OrdinalIgnoreCase) != 0))
                            {
                                publicContainer = null;
                                container = null;

                                currentContainerId = objectId;
                                currentObjectType = objectType;
                            }

                            bool publicAccess = objectTypesWithPublicAccess.Contains(objectType);
                            CloudBlobContainer blobContainer = null;

                            if (publicAccess)
                            {
                                if (publicContainer == null)
                                {
                                    string containerName = string.Format(CultureInfo.InvariantCulture, "{0:N}p", objectId);

                                    publicContainer = blobClient.GetContainerReference(containerName);
                                    publicContainer.CreateIfNotExists();

                                    BlobContainerPermissions p = new BlobContainerPermissions()
                                    {
                                        PublicAccess = BlobContainerPublicAccessType.Blob
                                    };
                                    publicContainer.SetPermissions(p);
                                }

                                blobContainer = publicContainer;
                            }
                            else
                            {
                                if (container == null)
                                {
                                    string containerName = objectId.ToString("N");

                                    container = blobClient.GetContainerReference(containerName);
                                    container.CreateIfNotExists();
                                }

                                blobContainer = container;
                            }

                            string blobName = string.Format(CultureInfo.InvariantCulture, "{0}/{1:N}/{2}", objectType, objectId, row.Name);
                            string mimeType = System.Web.MimeMapping.GetMimeMapping(row.Name);

                            // Upload to Azure.
                            using (Stream stream = new MemoryStream(content))
                            {
                                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(blobName);
                                blob.Properties.ContentType = mimeType;
                                blob.Properties.CacheControl = cacheControl;
                                blob.UploadFromStream(stream);
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
