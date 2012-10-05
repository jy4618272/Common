using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Properties;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with custom URLs.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class CustomUrlProvider
    {
        #region Private Methods

        /// <summary>
        /// Validates the specified partial custom URL.
        /// </summary>
        /// <param name="partialCustomUrl">The partial custom URL to validate.</param>
        public static void ValidatePartialCustomUrl(string partialCustomUrl)
        {
            if (partialCustomUrl != null)
            {
                foreach (string address in FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlReservedAddresses)
                {
                    if (partialCustomUrl.StartsWith(address, StringComparison.OrdinalIgnoreCase))
                        throw new ConstraintException(Resources.CustomUrlProvider_UrlReserved);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deletes the specified custom URLs.
        /// </summary>
        /// <param name="customUrlId">The unique identifier of the custom URLs.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteCustomUrl(Guid customUrlId)
        {
            CommonDataSet.CustomUrlRow row = GetCustomUrl(customUrlId);
            if (row != null)
            {
                WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Delete(customUrlId);

                WebsiteProvider.RemoveWebsiteUrls(row.OrganizationId, row.FullCustomUrl, row.PartialCustomUrl);
            }
        }

        /// <summary>
        /// Returns the custom URLs for the specified organization and the instances of this organization.
        /// </summary>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <returns>The System.Data.DataView that contains the custom URLs.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetCustomUrls(Guid organizationId)
        {
            CommonDataSet.CustomUrlDataTable table = null;
            try
            {
                table = new CommonDataSet.CustomUrlDataTable();
                WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Fill(table, 0, organizationId);
                table.Columns.Add("Name", typeof(string));
                Organization org = null;
                foreach (CommonDataSet.CustomUrlRow row in table)
                {
                    if (org == null)
                        org = OrganizationProvider.GetOrganization(organizationId);
                    string name = string.Empty;
                    if (row.IsInstanceIdNull())
                        name = org.Name;
                    else
                    {
                        Instance inst = org.Instances.FindByInstanceId(row.InstanceId);
                        if (inst != null)
                            name = inst.Name;
                    }
                    row["Name"] = name;
                }
                table.DefaultView.Sort = string.Format(CultureInfo.InvariantCulture, "{0}, Name", table.InstanceIdColumn.ColumnName);
                return table.DefaultView;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Returns the custom URLs by specified unique identifier.
        /// </summary>
        /// <param name="customUrlId">The unique identifier of the custom URLs.</param>
        /// <returns>The Micajah.Common.Dal.CommonDataSet.CustomUrlRow that pupulated by data of the custom URLs.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static CommonDataSet.CustomUrlRow GetCustomUrl(Guid customUrlId)
        {
            CommonDataSet.CustomUrlDataTable table = null;
            try
            {
                table = new CommonDataSet.CustomUrlDataTable();
                WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Fill(table, 1, customUrlId);
                return ((table.Count > 0) ? table[0] : null);
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Returns the custom URLs by specified host.
        /// </summary>
        /// <param name="host">The host of the URL to find the custom URLs.</param>
        /// <returns>The Micajah.Common.Dal.CommonDataSet.CustomUrlRow that pupulated by data of the custom URLs.</returns>
        public static CommonDataSet.CustomUrlRow GetCustomUrl(string host)
        {
            CommonDataSet.CustomUrlDataTable table = null;
            try
            {
                table = new CommonDataSet.CustomUrlDataTable();
                WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Fill(table, 2, null, null, host, host);
                return ((table.Count > 0) ? table[0] : null);
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }


        /// <summary>
        /// Returns the custom URLs by specified organization id.
        /// </summary>
        /// <param name="host">The host of the URL to find the custom URLs.</param>
        /// <returns>The Micajah.Common.Dal.CommonDataSet.CustomUrlRow that pupulated by data of the custom URLs.</returns>
        public static CommonDataSet.CustomUrlRow GetCustomUrlByOrganizationId(Guid organizationId)
        {
            CommonDataSet.CustomUrlDataTable table = null;
            try
            {
                table = new CommonDataSet.CustomUrlDataTable();
                WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Fill(table, 2, organizationId, null, null, null);
                return ((table.Count > 0) ? table[0] : null);
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }


        /// <summary>
        /// Returns the custom URLs by by specified organization id and instance id.
        /// </summary>
        /// <param name="host">The host of the URL to find the custom URLs.</param>
        /// <returns>The Micajah.Common.Dal.CommonDataSet.CustomUrlRow that pupulated by data of the custom URLs.</returns>
        public static CommonDataSet.CustomUrlRow GetCustomUrl(Guid organizationId, Guid instanceId)
        {
            CommonDataSet.CustomUrlDataTable table = null;
            try
            {
                table = new CommonDataSet.CustomUrlDataTable();
                WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Fill(table, 2, organizationId, instanceId, null, null);
                return ((table.Count > 0) ? table[0] : null);
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Registers new custom URLs for the specified organization or instance.
        /// </summary>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <param name="instanceId">The unique identifier of the instance.</param>
        /// <param name="fullCustomUrl">Full custom URL.</param>
        /// <param name="partialCustomUrl">Partial custom URL.</param>
        /// <returns>The unique idenfifier if the newly registered custom URLs.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertCustomUrl(Guid organizationId, Guid? instanceId, string fullCustomUrl, string partialCustomUrl)
        {
            ValidatePartialCustomUrl(partialCustomUrl);

            CommonDataSet.CustomUrlDataTable table = null;
            try
            {
                table = new CommonDataSet.CustomUrlDataTable();
                WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Fill(table, 2, organizationId, instanceId, fullCustomUrl, partialCustomUrl);
                if (table.Count > 0)
                    throw new ConstraintException(Resources.CustomUrlProvider_CustomUrlAlreadyExists);

                if (!CheckCustomUrl(fullCustomUrl) && !string.IsNullOrEmpty(fullCustomUrl))
                    throw new ConstraintException(Resources.CustomUrlProvider_CustomUrlAlreadyExists);

                if (!CheckCustomUrl(partialCustomUrl) && !string.IsNullOrEmpty(partialCustomUrl))
                    throw new ConstraintException(Resources.CustomUrlProvider_CustomUrlAlreadyExists);

                CommonDataSet.CustomUrlRow row = WebApplication.CommonDataSet.CustomUrl.NewCustomUrlRow();
                row.CustomUrlId = Guid.NewGuid();
                if (!string.IsNullOrEmpty(fullCustomUrl))
                    row.FullCustomUrl = fullCustomUrl.ToLower(CultureInfo.CurrentCulture);
                if (!string.IsNullOrEmpty(partialCustomUrl))
                    row.PartialCustomUrl = partialCustomUrl.ToLower(CultureInfo.CurrentCulture);
                row.OrganizationId = organizationId;
                if (instanceId.HasValue)
                    row.InstanceId = instanceId.Value;

                WebApplication.CommonDataSet.CustomUrl.AddCustomUrlRow(row);
                WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Update(row);

                WebsiteProvider.AddWebsiteUrls(organizationId, row.FullCustomUrl, row.PartialCustomUrl);

                return row.CustomUrlId;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Updates the custom URLs of the specified organization or instance.
        /// </summary>
        /// <param name="customUrlId">The unique identifier of the registered custom URLs.</param>
        /// <param name="fullCustomUrl">Full custom URL.</param>
        /// <param name="partialCustomUrl">Partial custom URL.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateCustomUrl(Guid customUrlId, string fullCustomUrl, string partialCustomUrl)
        {
            if (customUrlId == Guid.Empty) return;

            ValidatePartialCustomUrl(partialCustomUrl);

            CommonDataSet.CustomUrlDataTable table = null;
            try
            {
                CommonDataSet.CustomUrlRow row = null;
                table = new CommonDataSet.CustomUrlDataTable();
                WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Fill(table, 2, null, null, fullCustomUrl, partialCustomUrl);
                if (table.Count > 0)
                {
                    if (table.Count == 1)
                    {
                        if (table[0].CustomUrlId == customUrlId)
                            row = table[0];
                        else
                            throw new ConstraintException(Resources.CustomUrlProvider_CustomUrlAlreadyExists);
                    }
                    else
                        throw new ConstraintException(Resources.CustomUrlProvider_CustomUrlAlreadyExists);
                }

                if (!CheckCustomUrl(fullCustomUrl) && !string.IsNullOrEmpty(fullCustomUrl))
                    throw new ConstraintException(Resources.CustomUrlProvider_CustomUrlAlreadyExists);

                if (!CheckCustomUrl(partialCustomUrl) && !string.IsNullOrEmpty(partialCustomUrl))
                    throw new ConstraintException(Resources.CustomUrlProvider_CustomUrlAlreadyExists);

                if (string.IsNullOrEmpty(fullCustomUrl))
                    fullCustomUrl = string.Empty;
                if (string.IsNullOrEmpty(partialCustomUrl))
                    partialCustomUrl = string.Empty;

                if (row == null)
                    row = GetCustomUrl(customUrlId);

                WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Update(customUrlId, fullCustomUrl, partialCustomUrl);

                if (row != null)
                {
                    WebsiteProvider.RemoveWebsiteUrls(row.OrganizationId, row.FullCustomUrl, row.PartialCustomUrl);
                    WebsiteProvider.AddWebsiteUrls(row.OrganizationId, fullCustomUrl, partialCustomUrl);
                }
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Checks if custom url is exists in system
        /// </summary>
        /// <param name="customUrl">Custom url</param>
        /// <returns></returns>
        public static bool CheckCustomUrl(string customUrl)
        {
            if (!string.IsNullOrEmpty(customUrl))
            {
                CommonDataSet.CustomUrlDataTable table = null;
                Organization org = null;
                string[] segments = null;
                string segment = null;
                try
                {
                    table = new CommonDataSet.CustomUrlDataTable();
                    WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Fill(table, 2, null, null, customUrl, customUrl);

                    if (table.Rows.Count > 0)
                        return false;
                    else
                    {
                        segments = customUrl.Split('.');
                        if (segments.Length > 1)
                        {
                            segment = segments[0].ToLower().Replace("http://", string.Empty).Replace("https://", string.Empty);
                            if (segment.Contains("-"))
                                org = OrganizationProvider.GetOrganizationByPseudoId(segment.Split('-')[0]);
                            else
                                org = OrganizationProvider.GetOrganizationByPseudoId(segment);

                            return (org == null);
                        }
                        else
                            return false;
                    }

                }
                finally
                {
                    if (table != null) table.Dispose();
                    if (org != null) org = null;
                    segment = null;
                    segments = null;
                }
            }
            else
                return false;
        }


        /// <summary>
        /// Initializes the Organization or Instance from custom URL.
        /// </summary>
        public static void InitializeOrganizationOrInstanceFromCustomUrl()
        {
            string url = System.Web.HttpContext.Current.Request.Url.Host;
            Security.UserContext.VanityUrl = System.Web.HttpContext.Current.Request.Url.Host;
            string[] segments = url.Split('.');
            string defaultUrl = FrameworkConfiguration.Current.WebApplication.CustomUrl.DefaultPartialCustomUrl;
            bool found = false;

            foreach (string rootAddresse in FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddresses)
            {
                if (url.IndexOf(rootAddresse, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                if (segments.Length > 1)
                {
                    Organization org = null;
                    Instance instance = null;
                    string instPseudo = string.Empty;
                    string segment = segments[0];
                    found = false;

                    foreach (string reservedAddresse in FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlReservedAddresses)
                    {
                        if (string.Compare(segment, reservedAddresse, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        if (segment.IndexOf("-", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            string[] pseudos = segment.Split('-');
                            org = OrganizationProvider.GetOrganizationByPseudoId(pseudos[0]);
                            instPseudo = pseudos[1];
                            if (org == null)
                            {
                                string vanityUrl = string.Format("{0}{1}", segment.Split('-')[0], url.ToLower().Replace(segment, string.Empty).Replace("http://", string.Empty).Replace("https://", string.Empty));
                                CommonDataSet.CustomUrlRow customUrlRow = CustomUrlProvider.GetCustomUrl(vanityUrl.ToLower());
                                if (customUrlRow != null)
                                    org = OrganizationProvider.GetOrganization(customUrlRow.OrganizationId);

                                if (customUrlRow != null) customUrlRow = null;
                            }
                        }
                        else
                            org = OrganizationProvider.GetOrganizationByPseudoId(segment);

                        if (org != null)
                        {
                            Security.UserContext.SelectedOrganizationId = org.OrganizationId;

                            if (!string.IsNullOrEmpty(instPseudo))
                            {
                                instance = InstanceProvider.GetInstanceByPseudoId(instPseudo, org.OrganizationId);
                                if (instance != null)
                                    Security.UserContext.SelectedInstanceId = instance.InstanceId;                                    
                            }

                            if (org.Instances.Count == 1)
                            {
                                instance = org.Instances[0];
                                Security.UserContext.SelectedInstanceId = instance.InstanceId;   
                            }

                            Security.UserContext uc = Security.UserContext.Current;
                            
                            if (uc != null)
                            {
                                uc.SelectOrganization(org.OrganizationId);
                                if (instance != null)
                                    uc.SelectInstance(instance.InstanceId);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(defaultUrl))
                            {
                                if (url.IndexOf(defaultUrl, StringComparison.OrdinalIgnoreCase) != 0)
                                    System.Web.HttpContext.Current.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString().ToLower(CultureInfo.CurrentCulture).Replace(segment.ToLower(), defaultUrl.ToLower()));
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
