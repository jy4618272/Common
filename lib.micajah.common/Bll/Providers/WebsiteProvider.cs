using Micajah.Common.Dal;
using Micajah.Common.Dal.MasterDataSetTableAdapters;
using System;
using System.ComponentModel;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with web sites.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class WebsiteProvider
    {
        #region Internal Methods

        internal static Guid GetWebsiteIdByUrl(params string[] urls)
        {
            MasterDataSet.WebsiteRow row = GetWebsiteRowByUrl(urls);
            return ((row != null) ? row.WebsiteId : Guid.Empty);
        }

        /// <summary>
        /// Finds a web site by specified URLs and returns the identifier of the web site, if it's found and not deleted; otherwise, zero.
        /// </summary>
        /// <param name="urls">The URLs to find web site.</param>
        /// <returns>The web site if it's found; otherwise, null reference.</returns>
        internal static MasterDataSet.WebsiteRow GetWebsiteRowByUrl(params string[] urls)
        {
            MasterDataSet.WebsiteRow site = null;

            foreach (MasterDataSet.WebsiteRow row in GetWebsites())
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
        internal static MasterDataSet.WebsiteRow GetWebsiteRowByOrganizationId(Guid organizationId)
        {
            using (WebsiteTableAdapter adapter = new WebsiteTableAdapter())
            {
                MasterDataSet.WebsiteDataTable table = adapter.GetWebsiteByOrganizationId(organizationId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        /// <summary>
        /// Returns the identifier of the web site that the specified organization is associated with.
        /// </summary>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <returns>The unique identifier of the web site if it's found; otherwise, System.Guid.Empty.</returns>
        internal static Guid GetWebsiteIdByOrganizationId(Guid organizationId)
        {
            MasterDataSet.WebsiteRow row = GetWebsiteRowByOrganizationId(organizationId);
            return ((row == null) ? Guid.Empty : row.WebsiteId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the web sites, excluding marked as deleted.
        /// </summary>
        /// <returns>The DataTable that contains web sites.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MasterDataSet.WebsiteDataTable GetWebsites()
        {
            using (WebsiteTableAdapter adapter = new WebsiteTableAdapter())
            {
                return adapter.GetWebsites();
            }
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
        public static MasterDataSet.WebsiteRow GetWebsiteRow(Guid websiteId)
        {
            using (WebsiteTableAdapter adapter = new WebsiteTableAdapter())
            {
                MasterDataSet.WebsiteDataTable table = adapter.GetWebsite(websiteId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        /// <summary>
        /// Creates new web site with specified details.
        /// </summary>
        /// <param name="name">The name of the web site.</param>
        /// <param name="url">The URL of the web site.</param>
        /// <param name="description">The web site description.</param>
        /// <param name="adminContactInfo">The information to contact with administrator of the web site.</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertWebsite(string name, string url, string description, string adminContactInfo)
        {
            Guid websiteId = Guid.NewGuid();

            using (WebsiteTableAdapter adapter = new WebsiteTableAdapter())
            {
                adapter.Insert(websiteId, name, url, description, adminContactInfo, false);
            }

            return websiteId;
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
            using (WebsiteTableAdapter adapter = new WebsiteTableAdapter())
            {
                adapter.Update(websiteId, name, url, description, adminContactInfo, false);
            }
        }

        /// <summary>
        /// Marks as deleted the specified web site.
        /// </summary>
        /// <param name="websiteId">Specifies the web site's identifier.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteWebsite(Guid websiteId)
        {
            MasterDataSet.WebsiteRow row = GetWebsiteRow(websiteId);
            if (row != null)
            {
                row.Deleted = true;

                using (WebsiteTableAdapter adapter = new WebsiteTableAdapter())
                {
                    adapter.Update(row);
                }
            }
        }

        /// <summary>
        /// Returns the first URL of the specified web site.
        /// </summary>
        /// <param name="websiteId">The identifier of the web site.</param>
        /// <returns>The first URL of the web site.</returns>
        public static string GetWebsiteUrl(Guid websiteId)
        {
            MasterDataSet.WebsiteRow row = GetWebsiteRow(websiteId);
            return ((row == null) ? null : row.Url.Replace("\r", string.Empty).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)[0]);
        }

        #endregion
    }
}
