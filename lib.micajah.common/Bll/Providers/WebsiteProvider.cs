using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
using System;
using System.ComponentModel;
using System.Data;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with web sites.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class WebsiteProvider
    {
        #region Internal Methods

        /// <summary>
        /// Finds a web site by specified URLs and returns the identifier of the web site, if it's found and not deleted; otherwise, zero.
        /// </summary>
        /// <param name="urls">The URLs to find web site.</param>
        /// <returns>The web site if it's found; otherwise, null reference.</returns>
        internal static CommonDataSet.WebsiteRow GetWebsiteRowByUrl(params string[] urls)
        {
            CommonDataSet.WebsiteRow site = null;

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
                    Instance inst = new Instance();

                    foreach (string url in urls)
                    {
                        Organization org = null;

                        CustomUrlProvider.ParseHost(CustomUrlProvider.RemoveSchemeFormUri(url), ref org, ref inst);

                        if (org != null)
                            site = WebsiteProvider.GetWebsiteRowByOrganizationId(org.OrganizationId);

                        if (site != null) break;
                    }
                }
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
            MasterTableAdapters.Current.WebsiteTableAdapter.Update(row);
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

            MasterTableAdapters.Current.WebsiteTableAdapter.Update(row);
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

                MasterTableAdapters.Current.WebsiteTableAdapter.Update(row);
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
            CommonDataSet.WebsiteRow row = WebApplication.CommonDataSet.Website.FindByWebsiteId(websiteId);
            return ((row == null) ? null : row.Url.Replace("\r", string.Empty).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)[0]);
        }

        #endregion
    }
}
