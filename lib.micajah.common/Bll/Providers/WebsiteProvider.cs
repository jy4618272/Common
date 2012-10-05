using System;
using System.ComponentModel;
using System.Data;
using Micajah.Common.Application;
using Micajah.Common.Dal;
using System.Collections.Generic;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with web sites.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class WebsiteProvider
    {
        #region Private Methods

        /// <summary>
        /// Updates the URLs list of the web site, that the specified organization is associated with.
        /// </summary>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <param name="operationCode">The code of the operation: 0 - add, 1 - remove.</param>
        /// <param name="urls">The URLs to add or remove.</param>
        private static void UpdateWebsiteUrl(Guid organizationId, int operationCode, params string[] urls)
        {
            CommonDataSet.WebsiteRow row = GetWebsiteRowByOrganizationId(organizationId);
            if (row == null) return;

            string[] existingUrls = row.Url.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> list = new List<string>(existingUrls);
            int decrement = 0;

            foreach (string url in urls)
            {
                if (string.IsNullOrEmpty(url)) continue;

                string correctUrl = url;
                if (!correctUrl.StartsWith(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase))
                    correctUrl = Uri.UriSchemeHttp + Uri.SchemeDelimiter + correctUrl;

                bool urlExists = false;
                int index = 0;
                foreach (string existingUrl in existingUrls)
                {
                    if (string.Compare(correctUrl, existingUrl, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        urlExists = true;
                        break;
                    }
                    index++;
                }
                if (urlExists)
                {
                    if (operationCode == 1)
                    {
                        index -= decrement;
                        if (index < list.Count)
                        {
                            list.RemoveAt(index);
                            decrement++;
                        }
                    }
                }
                else if (operationCode == 0)
                    list.Add(correctUrl);
            }

            if (list.Count != existingUrls.Length)
            {
                row.Url = string.Join("\r\n", list.ToArray());
                WebApplication.CommonDataSetTableAdapters.WebsiteTableAdapter.Update(row);
                WebApplication.RefreshWebsiteId();
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Finds a web site by specified URLs and returns the identifier of the web site, if it's found and not deleted; otherwise, zero.
        /// </summary>
        /// <param name="urls">The URLs to find web site.</param>
        /// <returns>The web site if it's found; otherwise, null reference.</returns>
        internal static CommonDataSet.WebsiteRow GetWebsiteRowByUrl(params string[] urls)
        {
            CommonDataSet.WebsiteRow site = null;
            Organization org = null;            
            CommonDataSet.CustomUrlRow customUrlRow = null;
            try
            {
                foreach (CommonDataSet.WebsiteRow row in WebApplication.CommonDataSet.Website)
                {
                    foreach (string url in urls)
                    {
                        if (row.Url.Contains(url))
                        {
                            site = row;
                            break;
                        }
                    }
                    if (site != null) break;
                }

                if (site == null)
                {
                    if (Micajah.Common.Configuration.FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                    {
                        foreach (string url in urls)
                        {
                            string[] segments = url.Split('.');
                            if (segments.Length > 1)
                            {
                                string segment = segments[0].ToLower().Replace("http://", string.Empty).Replace("https://", string.Empty);
                                if (segment.Contains("-"))
                                {
                                    org = OrganizationProvider.GetOrganizationByPseudoId(segment.Split('-')[0]);

                                    if (org == null)
                                    {
                                        string vanityUrl = string.Format("{0}{1}", segment.Split('-')[0], url.ToLower().Replace(segment, string.Empty).Replace("http://", string.Empty).Replace("https://", string.Empty));
                                        customUrlRow = CustomUrlProvider.GetCustomUrl(vanityUrl.ToLower());
                                        if (customUrlRow != null)
                                            org = OrganizationProvider.GetOrganization(customUrlRow.OrganizationId);
                                    }
                                }
                                else
                                {
                                    org = OrganizationProvider.GetOrganizationByPseudoId(segment);

                                    if (org == null)
                                    {
                                        customUrlRow = CustomUrlProvider.GetCustomUrl(url.ToLower().Replace("http://", string.Empty).Replace("https://", string.Empty));
                                        if (customUrlRow != null)
                                            org = OrganizationProvider.GetOrganization(customUrlRow.OrganizationId);
                                    }
                                }

                                if (org != null)
                                    site = WebsiteProvider.GetWebsiteRowByOrganizationId(org.OrganizationId);

                                if (site != null) break;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (customUrlRow != null) customUrlRow = null;
                if (org != null) org = null;
            }

            return site;
        }

        /// <summary>
        /// Returns the web site that the specified organization is associated with.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <returns>The web site if it's found; otherwise, null reference.</returns>
        internal static CommonDataSet.WebsiteRow GetWebsiteRowByOrganizationId(Guid organizationId)
        {
            CommonDataSet ds = WebApplication.CommonDataSet;
            CommonDataSet.OrganizationRow org = ds.Organization.FindByOrganizationId(organizationId);
            if (org != null)
            {
                if (!org.IsDatabaseIdNull())
                {
                    CommonDataSet.DatabaseRow db = ds.Database.FindByDatabaseId(org.DatabaseId);
                    if (db != null)
                    {
                        CommonDataSet.DatabaseServerRow server = ds.DatabaseServer.FindByDatabaseServerId(db.DatabaseServerId);
                        if (server != null)
                            return ds.Website.FindByWebsiteId(server.WebsiteId);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the identifier of the web site that the specified organization is associated with.
        /// </summary>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <returns>The unique identifier of the web site if it's found; otherwise, System.Guid.Empty.</returns>
        internal static Guid GetWebsiteIdByOrganizationId(Guid organizationId)
        {
            CommonDataSet.WebsiteRow row = GetWebsiteRowByOrganizationId(organizationId);
            return ((row == null) ? Guid.Empty : row.WebsiteId);
        }

        /// <summary>
        /// Adds the specified URLs to the URLs list of the web site, that the specified organization is associated with.
        /// </summary>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <param name="urls">The URLs to add.</param>
        internal static void AddWebsiteUrls(Guid organizationId, params string[] urls)
        {
            UpdateWebsiteUrl(organizationId, 0, urls);
        }

        /// <summary>
        /// Removes the specified URLs from the URLs list of the web site, that the specified organization is associated with.
        /// </summary>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <param name="urls">The URLs to remove.</param>
        internal static void RemoveWebsiteUrls(Guid organizationId, params string[] urls)
        {
            UpdateWebsiteUrl(organizationId, 1, urls);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the web sites, excluding marked as deleted.
        /// </summary>
        /// <returns>The DataTable that contains web sites.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetWebsites()
        {
            return WebApplication.CommonDataSet.Website;
        }

        /// <summary>
        /// Gets an object populated with information of the specified web site.
        /// </summary>
        /// <param name="websiteId">Specifies the web site identifier to get information.</param>
        /// <returns>
        /// The object populated with information of the specified web site. 
        /// If the web site is not found, the method returns null reference.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static CommonDataSet.WebsiteRow GetWebsiteRow(Guid websiteId)
        {
            return WebApplication.CommonDataSet.Website.FindByWebsiteId(websiteId);
        }

        /// <summary>
        /// Creates new web site with specified details.
        /// </summary>
        /// <param name="name">The name of the web site.</param>
        /// <param name="url">The URL of the web site.</param>
        /// <param name="description">The web site description.</param>
        /// <param name="adminContactInfo">The information to contact with administrator of the web site.</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertWebsite(string name, string url, string description, string adminContactInfo)
        {
            CommonDataSet.WebsiteRow row = WebApplication.CommonDataSet.Website.NewWebsiteRow();

            row.WebsiteId = Guid.NewGuid();
            row.Name = name;
            row.Url = url;
            row.Description = description;
            row.AdminContactInfo = adminContactInfo;

            WebApplication.CommonDataSet.Website.AddWebsiteRow(row);
            WebApplication.CommonDataSetTableAdapters.WebsiteTableAdapter.Update(row);
        }

        /// <summary>
        /// Updates the details of specified web site.
        /// </summary>
        /// <param name="websiteId">The identifier of the web site.</param>
        /// <param name="name">The name of the web site.</param>
        /// <param name="url">The URL of the web site.</param>
        /// <param name="description">The web site description.</param>
        /// <param name="adminContactInfo">The information to contact with administrator of the web site.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateWebsite(Guid websiteId, string name, string url, string description, string adminContactInfo)
        {
            CommonDataSet.WebsiteRow row = WebApplication.CommonDataSet.Website.FindByWebsiteId(websiteId);
            if (row == null) return;

            row.Name = name;
            row.Url = url;
            row.Description = description;
            row.AdminContactInfo = adminContactInfo;

            WebApplication.CommonDataSetTableAdapters.WebsiteTableAdapter.Update(row);
        }

        /// <summary>
        /// Marks as deleted the specified web site.
        /// </summary>
        /// <param name="websiteId">Specifies the web site's identifier.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteWebsite(Guid websiteId)
        {
            CommonDataSet.WebsiteRow row = WebApplication.CommonDataSet.Website.FindByWebsiteId(websiteId);
            if (row != null)
            {
                row.Deleted = true;

                WebApplication.CommonDataSetTableAdapters.WebsiteTableAdapter.Update(row);
                WebApplication.CommonDataSet.Website.RemoveWebsiteRow(row);
            }
        }

        /// <summary>
        /// Returns the first URL of the specified web site.
        /// </summary>
        /// <param name="websiteId">The identifier of the web site.</param>
        /// <returns>The first URL of the web site.</returns>
        public static string GetWebsiteUrl(Guid websiteId)
        {
            string url = null;
            CommonDataSet.WebsiteRow row = WebApplication.CommonDataSet.Website.FindByWebsiteId(websiteId);
            if (row != null) url = row.Url.Replace("\r", string.Empty).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)[0];
            return url;
        }

        #endregion
    }
}
