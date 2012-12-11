using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
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

        #region Public Properties

        /// <summary>
        /// Gets the default vanity URL.
        /// </summary>
        /// <returns>The String that represents the default vanity URL.</returns>
        public static string DefaultVanityUrl
        {
            get
            {
                CustomUrlElement customUrlSettings = FrameworkConfiguration.Current.WebApplication.CustomUrl;
                return customUrlSettings.DefaultPartialCustomUrl + "." + customUrlSettings.PartialCustomUrlRootAddressesFirst;
            }
        }

        public static string ApplicationUri
        {
            get { return CreateApplicationUri(null); }
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
                WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Delete(customUrlId);
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
            if (!string.IsNullOrEmpty(host))
                host = host.ToLowerInvariant();
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

                if (!ValidateCustomUrl(fullCustomUrl) && !string.IsNullOrEmpty(fullCustomUrl))
                    throw new ConstraintException(Resources.CustomUrlProvider_CustomUrlAlreadyExists);

                if (!ValidateCustomUrl(partialCustomUrl) && !string.IsNullOrEmpty(partialCustomUrl))
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

                if (row == null)
                    row = GetCustomUrl(customUrlId);

                if (!ValidateCustomUrl(fullCustomUrl) && !string.IsNullOrEmpty(fullCustomUrl) && (row != null && string.Compare(row.FullCustomUrl, fullCustomUrl, true) != 0))
                    throw new ConstraintException(Resources.CustomUrlProvider_CustomUrlAlreadyExists);

                if (!ValidateCustomUrl(partialCustomUrl) && !string.IsNullOrEmpty(partialCustomUrl) && (row != null && string.Compare(row.PartialCustomUrl, partialCustomUrl, true) != 0))
                    throw new ConstraintException(Resources.CustomUrlProvider_CustomUrlAlreadyExists);

                if (string.IsNullOrEmpty(fullCustomUrl))
                    fullCustomUrl = string.Empty;
                if (string.IsNullOrEmpty(partialCustomUrl))
                    partialCustomUrl = string.Empty;

                WebApplication.CommonDataSetTableAdapters.CustomUrlTableAdapter.Update(customUrlId, fullCustomUrl, partialCustomUrl);
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
        public static bool ValidateCustomUrl(string partialCustomUrl)
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
                        segment = RemoveSchemeFormUri(partialCustomUrl);
                        if (segment.Contains("-"))
                            org = OrganizationProvider.GetOrganizationByPseudoId(segment.Split('-')[0]);
                        else
                            org = OrganizationProvider.GetOrganizationByPseudoId(segment);

                        if (org == null)
                        {
                            CommonDataSet.CustomUrlRow row = GetCustomUrl(segment);
                            if (row != null)
                                org = OrganizationProvider.GetOrganization(row.OrganizationId);
                        }

                        return (org == null);
                    }
                }
                finally
                {
                    if (table != null) table.Dispose();
                }
            }

            return false;
        }

        public static bool IsDefaultVanityUrl(string host)
        {
            if (string.IsNullOrEmpty(host))
                return false;

            return string.IsNullOrEmpty(host.ToLowerInvariant().Replace(DefaultVanityUrl, string.Empty));
        }

        public static bool IsDefaultVanityUrl(HttpContext http)
        {
            if (http != null)
            {
                if (http.Request != null)
                    return IsDefaultVanityUrl(http.Request.Url.Host);
            }
            return false;
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

            Organization org = OrganizationProvider.GetOrganization(organizationId);
            if (org != null)
            {
                Instance inst = null;
                CommonDataSet.CustomUrlRow row = null;

                if (instanceId != Guid.Empty)
                    row = GetCustomUrl(organizationId, instanceId);

                if (row == null)
                    row = GetCustomUrlByOrganizationId(organizationId);

                if (instanceId != Guid.Empty)
                    inst = InstanceProvider.GetInstance(instanceId, organizationId);

                CustomUrlElement customUrlSettings = FrameworkConfiguration.Current.WebApplication.CustomUrl;

                if (row != null)
                {
                    if (row.IsInstanceIdNull() && (inst != null))
                        customUrl = row.PartialCustomUrl + "-" + inst.PseudoId + "." + customUrlSettings.PartialCustomUrlRootAddressesFirst;
                    else
                        customUrl = (!string.IsNullOrEmpty(row.FullCustomUrl))
                            ? row.FullCustomUrl
                            : row.PartialCustomUrl + "." + customUrlSettings.PartialCustomUrlRootAddressesFirst;
                }
                else
                {
                    if (inst != null)
                        customUrl = org.PseudoId + "-" + inst.PseudoId + "." + customUrlSettings.PartialCustomUrlRootAddressesFirst;
                    else
                        customUrl = org.PseudoId + "." + customUrlSettings.PartialCustomUrlRootAddressesFirst;
                }
            }

            return customUrl;
        }

        public static string GetVanityUri(Guid organizationId, Guid instanceId)
        {
            return GetVanityUri(organizationId, instanceId, null);
        }

        public static string GetVanityUri(Guid organizationId, Guid instanceId, string pathAndQuery)
        {
            string websiteUrl = string.Empty;

            if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
            {
                websiteUrl = GetVanityUrl(organizationId, instanceId);
                if (string.IsNullOrEmpty(websiteUrl))
                    websiteUrl = DefaultVanityUrl;
            }
            else
            {
                Guid webSiteId = WebsiteProvider.GetWebsiteIdByOrganizationId(organizationId);
                if (webSiteId != Guid.Empty)
                    websiteUrl = WebsiteProvider.GetWebsiteUrl(webSiteId);
            }

            return CreateApplicationUri(websiteUrl, pathAndQuery);
        }

        /// <summary>
        /// Converts the specified path and query to the application relative URL if it is possible.
        /// </summary>
        /// <param name="pathAndQuery">The path and query to convert.</param>
        /// <returns>The string that represents the application relative URL or original path and query, if the conversion is not possible.</returns>
        public static string CreateApplicationRelativeUrl(string pathAndQuery)
        {
            if (!string.IsNullOrEmpty(pathAndQuery))
            {
                pathAndQuery = Regex.Replace(pathAndQuery, "default.aspx", string.Empty, RegexOptions.IgnoreCase);
                if (pathAndQuery.StartsWith("~", StringComparison.OrdinalIgnoreCase)) pathAndQuery = pathAndQuery.Remove(0, 1);
                if (!string.IsNullOrEmpty(WebApplication.RootPath)) pathAndQuery = Regex.Replace(pathAndQuery, WebApplication.RootPath, string.Empty, RegexOptions.IgnoreCase);
            }
            return pathAndQuery;
        }

        /// <summary>
        /// Converts the specified path and query to the application absolute URL if it is possible.
        /// </summary>
        /// <param name="pathAndQuery">The path and query to convert.</param>
        /// <returns>The string that represents the application absolute URL or original path and query, if the conversion is not possible.</returns>
        public static string CreateApplicationAbsoluteUrl(string pathAndQuery)
        {
            if (!string.IsNullOrEmpty(pathAndQuery))
            {
                if (!(pathAndQuery.StartsWith(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
                    || pathAndQuery.StartsWith(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
                    || pathAndQuery.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase)))
                {
                    if (pathAndQuery.StartsWith("~", StringComparison.OrdinalIgnoreCase)) pathAndQuery = pathAndQuery.Remove(0, 1);
                    if (!pathAndQuery.StartsWith("/", StringComparison.OrdinalIgnoreCase)) pathAndQuery = "/" + pathAndQuery;
                    if (!string.IsNullOrEmpty(WebApplication.RootPath))
                    {
                        if (!pathAndQuery.ToUpperInvariant().Contains(WebApplication.RootPath.ToUpperInvariant() + "/"))
                            pathAndQuery = WebApplication.RootPath + pathAndQuery;
                    }
                    pathAndQuery = Regex.Replace(pathAndQuery, "default.aspx", string.Empty, RegexOptions.IgnoreCase);
                }
            }
            return pathAndQuery;
        }

        /// <summary>
        /// Converts the specified path and query to the URI.
        /// </summary>
        /// <param name="pathAndQuery">The path to convert.</param>
        /// <returns>The string that represents the URI.</returns>
        public static string CreateApplicationUri(string pathAndQuery)
        {
            return CreateApplicationUri((FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled ? DefaultVanityUrl : null), pathAndQuery);
        }

        public static string CreateApplicationUri(string host, string pathAndQuery)
        {
            string url = null;

            if (string.IsNullOrEmpty(host))
                url = FrameworkConfiguration.Current.WebApplication.Url;
            else
            {
                HttpRequest request = HttpContext.Current.Request;

                url = request.Url.Scheme + Uri.SchemeDelimiter + RemoveSchemeFormUri(host);

                if (FrameworkConfiguration.Current.WebApplication.AddPort)
                {
                    int port = request.Url.Port;
                    if ((!request.Url.IsDefaultPort) && (port > -1))
                        url += ":" + port;
                }
            }

            if (string.IsNullOrEmpty(pathAndQuery))
                url += CreateApplicationAbsoluteUrl("~/");
            else
                url += CreateApplicationAbsoluteUrl(pathAndQuery);

            return url.TrimEnd('/');
        }

        /// <summary>
        /// Parses specified host and returns the organization and instance.
        /// </summary>
        /// <param name="host">Host component of the URL.</param>
        /// <param name="organization">An organization.</param>
        /// <param name="instance">An instance.</param>
        public static void ParseHost(string host, ref Organization organization, ref Instance instance)
        {
            CommonDataSet.CustomUrlRow row = GetCustomUrl(host);
            if (row != null)
            {
                organization = OrganizationProvider.GetOrganization(row.OrganizationId);

                if (instance == null)
                {
                    if (!row.IsInstanceIdNull())
                        instance = InstanceProvider.GetInstance(row.InstanceId, row.OrganizationId);
                }
            }
            else
            {
                string[] segments = host.ToLowerInvariant().Split('.');

                if (segments.Length < 2) return;

                string segment = segments[0];

                if (string.Compare(segment, FrameworkConfiguration.Current.WebApplication.CustomUrl.AuthenticationTicketDomain, StringComparison.OrdinalIgnoreCase) == 0)
                    return;

                CommonDataSet.CustomUrlRow customUrlRow = null;
                string instPseudoId = null;
                string[] pseudos = segment.Split('-');

                if (pseudos.Length > 1)
                {
                    organization = OrganizationProvider.GetOrganizationByPseudoId(pseudos[0]);
                    instPseudoId = pseudos[1];

                    if (organization == null)
                    {
                        customUrlRow = GetCustomUrl(pseudos[0]);
                        if (customUrlRow != null)
                            organization = OrganizationProvider.GetOrganization(customUrlRow.OrganizationId);
                    }
                }
                else
                    organization = OrganizationProvider.GetOrganizationByPseudoId(segment);

                if (organization == null)
                {
                    customUrlRow = GetCustomUrl(segment);
                    if (customUrlRow != null)
                    {
                        organization = OrganizationProvider.GetOrganization(customUrlRow.OrganizationId);

                        if (instance == null)
                        {
                            if (!customUrlRow.IsInstanceIdNull())
                                instance = InstanceProvider.GetInstance(customUrlRow.InstanceId, customUrlRow.OrganizationId);
                        }
                    }
                }

                if (organization != null)
                {
                    if (instance == null)
                    {
                        if (!string.IsNullOrEmpty(instPseudoId))
                            instance = InstanceProvider.GetInstanceByPseudoId(instPseudoId, organization.OrganizationId);
                    }
                }
            }
        }

        /// <summary>
        /// Parses specified host and returns the identifiers of the organization and instance.
        /// </summary>
        /// <param name="host">Host component of the URL.</param>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <param name="instanceId">The unique identifier of the instance.</param>
        public static void ParseHost(string host, ref Guid organizationId, ref Guid instanceId)
        {
            Organization org = null;
            Instance instance = null;

            ParseHost(host, ref org, ref instance);

            organizationId = ((org == null) ? Guid.Empty : org.OrganizationId);
            instanceId = ((instance == null) ? Guid.Empty : instance.InstanceId);
        }

        public static string RemoveSchemeFormUri(string url)
        {
            if (!string.IsNullOrEmpty(url))
                url = url.ToLowerInvariant().Replace("http://", string.Empty).Replace("https://", string.Empty);
            return url;
        }

        #endregion
    }
}
