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
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static CommonDataSet.CustomUrlRow GetCustomUrlByOrganizationId(Guid organizationId)
        {
            CommonDataSet.CustomUrlDataTable table = null;
            CommonDataSet.CustomUrlRow row = null;
            try
            {
                table = new CommonDataSet.CustomUrlDataTable();
                WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Fill(table, 2, organizationId, null, null, null);
                row = ((table.Count > 0) ? table[0] : null);
                if (row != null)
                {
                    try
                    {
                        if (row.InstanceId == Guid.Empty)
                            row.InstanceId = Guid.Empty;
                    }
                    catch { row.InstanceId = Guid.Empty; }
                }
                return row;
            }
            finally
            {
                if (table != null) table.Dispose();
                if (row != null) row = null;
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

                if (!FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                    WebsiteProvider.AddWebsiteUrls(organizationId, row.FullCustomUrl, row.PartialCustomUrl);
                else
                    WebsiteProvider.AddWebsiteUrls(organizationId, row.FullCustomUrl, string.Format("{0}.{1}", row.PartialCustomUrl, FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddressesFirst));

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
        /// <param name="partialCustomUrl">Partial Custom Url with out domain</param>
        /// <returns></returns>
        public static bool CheckCustomUrl(string partialCustomUrl)
        {
            if (!string.IsNullOrEmpty(partialCustomUrl))
            {
                CommonDataSet.CustomUrlDataTable table = null;
                Organization org = null;
                string segment = null;
                try
                {
                    table = new CommonDataSet.CustomUrlDataTable();
                    WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Fill(table, 2, null, null, partialCustomUrl, partialCustomUrl);

                    if (table.Rows.Count > 0)
                        return false;
                    else
                    {
                        segment = partialCustomUrl.ToLower().Replace("http://", string.Empty).Replace("https://", string.Empty);
                        if (segment.Contains("-"))
                            org = OrganizationProvider.GetOrganizationByPseudoId(segment.Split('-')[0]);
                        else
                            org = OrganizationProvider.GetOrganizationByPseudoId(segment);

                        if (org == null)
                        {
                            CommonDataSet.CustomUrlRow row = CustomUrlProvider.GetCustomUrl(segment);
                            if (row != null)
                                org = OrganizationProvider.GetOrganization(row.OrganizationId);
                        }

                        return (org == null);
                    }
                }
                finally
                {
                    if (table != null) table.Dispose();
                    if (org != null) org = null;
                    segment = null;
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
            string defaultUrl = FrameworkConfiguration.Current.WebApplication.CustomUrl.DefaultPartialCustomUrl;
            Organization org = null;
            Instance instance = null;

            if (url.IndexOf(defaultUrl, StringComparison.OrdinalIgnoreCase) != 0)
            {
                InitializeFromCustomUrl(ref org, ref instance);
                Security.UserContext.VanityUrl = (org != null) ? url : string.Empty;
            }

            if (org != null)
            {
                Security.UserContext.SelectedOrganizationId = org.OrganizationId;
                Security.UserContext uc = Security.UserContext.Current;

                if (instance != null)
                    Security.UserContext.SelectedInstanceId = instance.InstanceId;
                else if (Security.UserContext.SelectedInstanceId != Guid.Empty)
                    Security.UserContext.SelectedInstanceId = Guid.Empty;

                if (uc != null)
                {
                    uc.SelectOrganization(org.OrganizationId);
                    if (instance != null)
                        uc.SelectInstance(instance.InstanceId);
                }
                else
                    Security.UserContext.VanityUrl = string.Empty;
            }
            else
            {
                if (!string.IsNullOrEmpty(defaultUrl))
                {
                    if (url.IndexOf(defaultUrl, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        System.Web.HttpRequest request = System.Web.HttpContext.Current.Request;
                        System.Web.HttpContext.Current.Response.Redirect(request.Url.ToString().Replace(request.Url.Host, string.Format("{0}.{1}", FrameworkConfiguration.Current.WebApplication.CustomUrl.DefaultPartialCustomUrl, FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddressesFirst)));
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the Organization or Instance from System.Web.HttpContext.Current.Request.
        /// </summary>        
        /// <param name="organization">Organization</param>
        /// <param name="instance">Instance</param>
        public static void InitializeFromCustomUrl(ref Organization organization, ref Instance instance)
        {
            string[] segments = null;
            string instPseudo = null;
            string segment = null;
            string[] pseudos = null;
            CommonDataSet.CustomUrlRow customUrlRow = null;
            InstanceCollection coll = null;
            string customUrl = System.Web.HttpContext.Current.Request.Url.Host.ToLower();

            organization = null;
            instance = null;

            try
            {
                segments = customUrl.Split('.');

                if (segments.Length > 1)
                {
                    segment = segments[0];
                    if (string.Compare(segment, FrameworkConfiguration.Current.WebApplication.CustomUrl.AuthenticationTicketDomain) != 0)
                    {
                        if (segment.IndexOf("-", StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            pseudos = segment.Split('-');
                            organization = OrganizationProvider.GetOrganizationByPseudoId(pseudos[0]);
                            instPseudo = pseudos[1];
                            if (organization == null)
                            {
                                customUrlRow = CustomUrlProvider.GetCustomUrl(segment.Split('-')[0].ToLower());
                                if (customUrlRow != null)
                                    organization = OrganizationProvider.GetOrganization(customUrlRow.OrganizationId);
                            }
                        }
                        else
                            organization = OrganizationProvider.GetOrganizationByPseudoId(segment);

                        if (organization == null)
                        {
                            customUrlRow = CustomUrlProvider.GetCustomUrl(segment.ToLower());
                            if (customUrlRow != null)
                            {
                                organization = OrganizationProvider.GetOrganization(customUrlRow.OrganizationId);
                                if (!customUrlRow.IsInstanceIdNull())
                                    instance = InstanceProvider.GetInstance(customUrlRow.InstanceId, customUrlRow.OrganizationId);
                            }
                        }

                        if (organization != null)
                        {
                            Security.UserContext.SelectedOrganizationId = organization.OrganizationId;
                            Security.UserContext uc = Security.UserContext.Current;

                            if (!string.IsNullOrEmpty(instPseudo))
                                instance = InstanceProvider.GetInstanceByPseudoId(instPseudo, organization.OrganizationId);

                            if (uc != null)
                            {
                                if (instance == null)
                                {
                                    coll = WebApplication.LoginProvider.GetLoginInstances(uc.UserId, organization.OrganizationId);
                                    if (coll.Count == 1)
                                        instance = coll[0];
                                }
                            }

                            if (instance == null)
                            {
                                coll = InstanceProvider.GetInstances(organization.OrganizationId, false);
                                if (coll.Count == 1)
                                    instance = coll[0];
                            }

                            if (instance != null)
                                Security.UserContext.SelectedInstanceId = instance.InstanceId;
                        }
                    }
                }
            }
            finally
            {
                if (segments != null) segments = null;
                if (instPseudo != null) instPseudo = null;
                if (segment != null) segment = null;
                if (pseudos != null) pseudos = null;
                if (customUrlRow != null) customUrlRow = null;
                if (customUrl != null) customUrl = null;
                if (coll != null) coll = null;
            }
        }

        /// <summary>
        /// Gets Custom url by organizationId and instanceId. If need only by organization use instanceId == Guid.Empty
        /// </summary>
        /// <param name="organizationId">Organization Id</param>
        /// <param name="instanceId">Instance Id</param>
        /// <returns>Custom Url</returns>
        public static string GetVanityUrl(Guid organizationId, Guid instanceId)
        {
            string customUrl = string.Empty;
            Organization org = null;
            Instance inst = null;
            CommonDataSet.CustomUrlRow row = null;
            System.Data.DataView table = null;
            bool oneInstance = true;
            InstanceCollection coll = null;
            try
            {
                org = OrganizationProvider.GetOrganization(organizationId);
                if (org != null)
                {
                    if (instanceId != Guid.Empty)
                        row = CustomUrlProvider.GetCustomUrl(organizationId, instanceId);

                    if (row == null)
                        row = CustomUrlProvider.GetCustomUrlByOrganizationId(organizationId);

                    if (row == null)
                    {
                        table = CustomUrlProvider.GetCustomUrls(organizationId);
                        if (table != null && table.Table.Rows.Count > 0)
                            row = table.Table.Rows[0] as CommonDataSet.CustomUrlRow;
                    }

                    Security.UserContext ctx = Security.UserContext.Current;
                    if (ctx != null)
                    {
                        coll = WebApplication.LoginProvider.GetLoginInstances(ctx.UserId, organizationId);
                        oneInstance = (coll != null && coll.Count == 1);
                    }

                    if (instanceId != Guid.Empty)
                        inst = InstanceProvider.GetInstance(instanceId, organizationId);

                    if (row != null)
                    {
                        customUrl = !string.IsNullOrEmpty(row.FullCustomUrl) ? row.FullCustomUrl : string.Format("{0}.{1}", row.PartialCustomUrl, FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddressesFirst);

                        if (row.InstanceId == Guid.Empty && inst != null && !oneInstance)
                            customUrl = string.Format(CultureInfo.InvariantCulture, "{0}-{1}.{2}", row.PartialCustomUrl, inst.PseudoId, FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddressesFirst);
                    }
                    else
                    {
                        customUrl = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", org.PseudoId, FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddressesFirst);

                        if (inst != null && !oneInstance)
                            customUrl = string.Format(CultureInfo.InvariantCulture, "{0}-{1}.{2}", org.PseudoId, inst.PseudoId, FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddressesFirst);
                    }
                }

                return customUrl;
            }
            finally
            {
                if (org != null) org = null;
                if (inst != null) inst = null;
                if (!string.IsNullOrEmpty(customUrl)) customUrl = null;
                if (row != null) row = null;
                if (table != null) table = null;
                if (coll != null) coll = null;
            }
        }

        #endregion
    }
}
