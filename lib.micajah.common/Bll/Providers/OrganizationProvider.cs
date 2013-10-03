using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Dal.MasterDataSetTableAdapters;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with organizations.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class OrganizationProvider
    {
        #region Memebers

        private const int NameMaxLength = 255;
        private const int DescriptionMaxLength = 255;
        private const int WebsiteUrlMaxLength = 2048;
        private const int LdapDomainsMaxLength = 2048;
        private const int LdapServerAddressMaxLength = 255;
        private const int LdapServerPortMaxLength = 50;
        private const int LdapDomainMaxLength = 255;
        private const int LdapUserNameMaxLength = 255;
        private const int LdapPasswordMaxLength = 255;
        private const string DefaultLdapServerPort = "636";

        private const string ConnectionStringKeyFormat = "mc.ConnectionString.{0:N}";
        private const string OrganizationKeyFormat = "mc.Organization.{0:N}";
        private const string OrganizationByPseudoIdKeyFormat = "mc.Organization.{0}";
        private const string WebsiteIdKeyFormat = "mc.WebsiteId.{0:N}";

        #endregion

        #region Events

        /// <summary>
        /// Occurs when an organization is inserted.
        /// </summary>
        public static event EventHandler<OrganizationProviderEventArgs> OrganizationInserted;

        /// <summary>
        /// Occurs when an organization is updated.
        /// </summary>
        public static event EventHandler<OrganizationProviderEventArgs> OrganizationUpdated;

        #endregion

        #region Public Properties

        public static Organization TemplateOrganization
        {
            get { return GetOrganizationByName("Template"); }
        }

        #endregion

        #region Private Methods

        private static MasterDataSet.OrganizationRow GetOrganizationRowByName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                using (OrganizationTableAdapter adapter = new OrganizationTableAdapter())
                {
                    MasterDataSet.OrganizationDataTable table = adapter.GetOrganizationByName(name);
                    return ((table.Count > 0) ? table[0] : null);
                }
            }
            return null;
        }

        /// <summary>
        /// Returns an object populated with information of the specified organization.
        /// </summary>
        /// <param name="pseudoId">The pseudo unique identifier of the organization.</param>
        /// <returns>The object populated with information of the specified organization. If the organization is not found, the method returns null reference.</returns>
        private static MasterDataSet.OrganizationRow GetOrganizationRowByPseudoId(string pseudoId)
        {
            using (OrganizationTableAdapter adapter = new OrganizationTableAdapter())
            {
                MasterDataSet.OrganizationDataTable table = adapter.GetOrganizationByPseudoId(pseudoId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        /// <summary>
        /// Creates new organization with specified details.
        /// </summary>
        /// <param name="name">The name of the organization.</param>
        /// <param name="description">The organization description.</param>
        /// <param name="websiteUrl">The URL of the organization web-site.</param>
        /// <param name="databaseId">The identifier of the organization's database.</param>
        /// <param name="fiscalYearStartMonth">The fiscal year start month of the organization.</param>         
        /// <param name="fiscalYearStartDay">The fiscal year start day of the organization.</param>         
        /// <param name="weekStartsDay">The week starts day of the organization.</param>         
        /// <param name="expirationTime">The expiration date and time of the organization</param>
        /// <param name="graceDays">The grace days number when the users be able to log in to the expired organization.</param>
        /// <param name="active">Whether the organization is active.</param>
        /// <param name="canceledTime">The date and time when the organization is cancelled.</param>
        /// <param name="trial">Whether the organization is trial.</param>
        /// <param name="street">The organization's street.</param>
        /// <param name="street2">The organization's secondary street.</param>
        /// <param name="city">The organization's city.</param>
        /// <param name="state">The organization's state/province.</param>
        /// <param name="postalCode">The organization's postal code.</param>
        /// <param name="country">The organization's country.</param>
        /// <param name="currency">The organization's currency.</param>
        /// <param name="timeZoneId">The time zone identifier.</param>
        /// <param name="templateInstanceId">The identifier of the template instance for the organization's first instance.</param>
        /// <param name="adminEmail">The e-mail address of the organization administrator.</param>
        /// <param name="password">The password of the organization administrator.</param>
        /// <param name="firstName">The first name of the organization administrator.</param>
        /// <param name="lastName">The last name of the organization administrator.</param>
        /// <param name="middleName">The middle name of the organization administrator.</param>
        /// <param name="title">The title of the organization administrator.</param>
        /// <param name="phone">The phone of the organization administrator.</param>
        /// <param name="mobilePhone">The mobile phone of the organization administrator.</param>
        /// <param name="sendNotificationEmail">true to send notification email to administrator; otherwise, false.</param>
        private static Guid InsertOrganization(string name, string description, string websiteUrl, Guid? databaseId
            , int? fiscalYearStartMonth, int? fiscalYearStartDay, int? weekStartsDay
            , DateTime? expirationTime, int graceDays, bool active, DateTime? canceledTime, bool trial
            , string street, string street2, string city, string state, string postalCode, string country, string currency, string howYouHearAboutUs
            , string timeZoneId, Guid? templateInstanceId
            , string adminEmail, string password, string firstName, string lastName, string middleName, string title, string phone, string mobilePhone
            , string partialCustomUrl, HttpRequest request
            , bool sendNotificationEmail, Guid? parentOrganizationId)
        {
            adminEmail = Support.TrimString(adminEmail, UserProvider.EmailMaxLength);
            Support.ValidateEmail(adminEmail);

            if (!string.IsNullOrEmpty(websiteUrl))
                Support.ValidateUrl(websiteUrl);
            else
                websiteUrl = string.Empty;

            MasterDataSet.OrganizationDataTable table = new MasterDataSet.OrganizationDataTable();
            MasterDataSet.OrganizationRow row = table.NewOrganizationRow();
            Guid organizationId = Guid.NewGuid();

            name = Support.TrimString(name, table.NameColumn.MaxLength);
            description = Support.TrimString(description, table.DescriptionColumn.MaxLength);
            websiteUrl = Support.TrimString(websiteUrl, table.WebsiteUrlColumn.MaxLength);

            row.OrganizationId = organizationId;
            row.PseudoId = Support.GeneratePseudoUnique();
            row.Name = name;
            if (description != null) row.Description = description;
            if (websiteUrl != null) row.WebsiteUrl = websiteUrl;
            if (databaseId.HasValue) row.DatabaseId = databaseId.Value;
            if (fiscalYearStartMonth.HasValue) row.FiscalYearStartMonth = fiscalYearStartMonth.Value;
            if (fiscalYearStartDay.HasValue) row.FiscalYearStartDay = fiscalYearStartDay.Value;
            if (weekStartsDay.HasValue) row.WeekStartsDay = weekStartsDay.Value;
            if (expirationTime.HasValue) row.ExpirationTime = expirationTime.Value;
            row.GraceDays = graceDays;
            row.Active = active;
            if (canceledTime.HasValue) row.CanceledTime = canceledTime.Value;
            row.Trial = trial;
            if (street != null) row.Street = street;
            if (street2 != null) row.Street2 = street2;
            if (city != null) row.City = city;
            if (state != null) row.State = state;
            if (postalCode != null) row.PostalCode = postalCode;
            if (country != null) row.Country = country;
            if (currency != null) row.Currency = currency;
            if (howYouHearAboutUs != null) row.HowYouHearAboutUs = howYouHearAboutUs;
            if (parentOrganizationId.HasValue) row.ParentOrganizationId = parentOrganizationId.Value;

            table.AddOrganizationRow(row);

            using (OrganizationTableAdapter adapter = new OrganizationTableAdapter())
            {
                adapter.Update(row);
            }

            Organization org = CreateOrganization(row);

            string domain = null;
            if (GoogleProvider.IsGoogleProviderRequest(request))
                domain = GoogleProvider.GetDomain(request);

            if (string.IsNullOrEmpty(domain))
            {
                string returnUrl = null;
                GoogleProvider.ParseOAuth2AuthorizationRequestState(request, ref domain, ref returnUrl);
            }

            if (!string.IsNullOrEmpty(domain))
                EmailSuffixProvider.InsertEmailSuffixName(organizationId, null, ref domain);

            RaiseOrganizationInserted(org);

            if (!string.IsNullOrEmpty(password))
            {
                UserProvider.AddUserToOrganization(adminEmail, firstName, lastName, middleName
                    , phone, mobilePhone, null, title, null
                    , street, street2, city, state, postalCode, country
                    , Guid.Empty.ToString(), organizationId
                    , password, false, false);
            }
            else
            {
                password = null;
                UserProvider.AddUserToOrganization(adminEmail, firstName, lastName, middleName
                      , phone, mobilePhone, null, title, null
                      , street, street2, city, state, postalCode, country
                      , null, null, null
                      , string.Empty, false
                      , organizationId, true
                      , false
                      , 0, 0, out password);
            }

            Guid instanceId = InstanceProvider.InsertFirstInstance(timeZoneId, templateInstanceId, organizationId
                , adminEmail, password
                , partialCustomUrl
                , sendNotificationEmail);

            Setting setting = SettingProvider.GetSettingByShortName("StartMenuCheckedItems");
            if (setting != null)
            {
                setting.Value = bool.TrueString;
                SettingProvider.UpdateSettingValue(setting, organizationId, ((ActionProvider.StartPageSettingsLevels & SettingLevels.Instance) == SettingLevels.Instance ? new Guid?(instanceId) : null), null);
            }

            if (request != null)
            {
                string providerName = GoogleProvider.GetProviderName(request);
                if (!string.IsNullOrEmpty(providerName))
                {
                    setting = SettingProvider.GetSettingByShortName("ProviderName");
                    if (setting != null)
                    {
                        setting.Value = providerName;
                        SettingProvider.UpdateSettingValue(setting, organizationId, null, null);
                    }
                }
            }

            return organizationId;
        }

        /// <summary>
        /// Updates the details of specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="name">The name of the organization.</param>
        /// <param name="description">The organization description.</param>
        /// <param name="websiteUrl">The URL of the organization web-site.</param>
        /// <param name="databaseId">The identifier of the organization's database.</param>
        /// <param name="fiscalYearStartMonth">The fiscal year start month of the organization.</param>         
        /// <param name="fiscalYearStartDay">The fiscal year start day of the organization.</param>         
        /// <param name="weekStartsDay">The week starts day of the organization.</param>
        /// <param name="expirationTime">The expiration date and time of the organization</param>
        /// <param name="graceDays">The grace days number when the users be able to log in to the expired organization.</param>
        /// <param name="active">Whether the organization is active.</param>
        /// <param name="emailSuffixes">The organization email suffixes.</param>
        /// <param name="ldapDomains">The organization ldap domains.</param>
        private static void UpdateOrganization(Guid organizationId, string name, string description, string websiteUrl, Guid? databaseId
            , int? fiscalYearStartMonth, int? fiscalYearStartDay, int? weekStartsDay
            , DateTime? expirationTime, int? graceDays, bool? active, DateTime? canceledTime, bool? trial, bool? beta, string emailSuffixes, string ldapDomains, Guid? parentOrganizationId)
        {
            if (!string.IsNullOrEmpty(websiteUrl))
                Support.ValidateUrl(websiteUrl);
            else
                websiteUrl = string.Empty;

            MasterDataSet.OrganizationRow row = GetOrganizationRow(organizationId);
            if (row == null) return;

            name = Support.TrimString(name, NameMaxLength);
            description = Support.TrimString(description, DescriptionMaxLength);
            websiteUrl = Support.TrimString(websiteUrl, WebsiteUrlMaxLength);

            row.Name = name;

            if (!row.Deleted)
            {
                if (string.IsNullOrEmpty(row.PseudoId))
                    row.PseudoId = Support.GeneratePseudoUnique();
            }

            row.Description = description;
            row.WebsiteUrl = websiteUrl;

            if (FrameworkConfiguration.Current.WebApplication.Integration.Ldap.Enabled)
            {
                if (ldapDomains != null)
                    row.LdapDomains = ldapDomains;
            }

            if (databaseId.HasValue)
            {
                if (databaseId.Value != Guid.Empty) row.DatabaseId = databaseId.Value;
            }
            else
                row.SetDatabaseIdNull();

            if (fiscalYearStartMonth.HasValue)
                row.FiscalYearStartMonth = fiscalYearStartMonth.Value;
            else
                row.SetFiscalYearStartMonthNull();

            if (fiscalYearStartDay.HasValue)
                row.FiscalYearStartDay = fiscalYearStartDay.Value;
            else
                row.SetFiscalYearStartDayNull();

            if (weekStartsDay.HasValue)
                row.WeekStartsDay = weekStartsDay.Value;
            else
                row.SetWeekStartsDayNull();

            if (expirationTime.HasValue)
            {
                if (expirationTime != DateTime.MinValue)
                    row.ExpirationTime = expirationTime.Value;
            }
            else
                row.SetExpirationTimeNull();

            if (graceDays.HasValue) row.GraceDays = graceDays.Value;
            if (active.HasValue) row.Active = active.Value;
            if (canceledTime.HasValue) row.CanceledTime = canceledTime.Value;
            if (trial.HasValue) row.Trial = trial.Value;
            if (beta.HasValue) row.Beta = beta.Value;

            if (parentOrganizationId.HasValue)
            {
                if (parentOrganizationId.Value != Guid.Empty) row.ParentOrganizationId = parentOrganizationId.Value;
            }
            else
                row.SetParentOrganizationIdNull();

            using (OrganizationTableAdapter adapter = new OrganizationTableAdapter())
            {
                adapter.Update(row);
            }

            Organization organization = CreateOrganization(row);

            if (FrameworkConfiguration.Current.WebApplication.Integration.Ldap.Enabled)
                EmailSuffixProvider.UpdateEmailSuffixName(organizationId, null, ref emailSuffixes);

            RemoveConnectionStringFromCache(organizationId);
            RemoveWebsiteIdFromCache(organizationId);
            PutOrganizationToCache(organization);

            UserContext.RefreshCurrent();

            RaiseOrganizationUpdated(organization);
        }

        private static void UpdateOrganizationRow(MasterDataSet.OrganizationRow row)
        {
            using (OrganizationTableAdapter adapter = new OrganizationTableAdapter())
            {
                adapter.Update(row);
            }

            Organization organization = CreateOrganization(row);

            RemoveConnectionStringFromCache(organization.OrganizationId);
            RemoveWebsiteIdFromCache(organization.OrganizationId);
            PutOrganizationToCache(organization);

            UserContext.RefreshCurrent();

            RaiseOrganizationUpdated(organization);
        }

        /// <summary>
        /// Raises the OrganizationInserted event.
        /// </summary>
        /// <param name="organization">The organization.</param>
        private static void RaiseOrganizationInserted(Organization organization)
        {
            if (OrganizationInserted != null)
                OrganizationInserted(null, new OrganizationProviderEventArgs() { Organization = organization });
        }

        /// <summary>
        /// Raises the OrganizationUpdated event.
        /// </summary>
        /// <param name="organization">The organization.</param>
        private static void RaiseOrganizationUpdated(Organization organization)
        {
            if (OrganizationUpdated != null)
                OrganizationUpdated(null, new OrganizationProviderEventArgs() { Organization = organization });
        }

        #endregion

        #region Internal Methods

        #region Cache Methods

        internal static string GetConnectionStringFromCache(Guid organizationId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, ConnectionStringKeyFormat, organizationId);
            string connectionString = CacheManager.Current.Get(key) as string;

            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = DatabaseProvider.GetConnectionStringByOrganizationId(organizationId);

                if (!string.IsNullOrEmpty(connectionString))
                    CacheManager.Current.PutWithDefaultTimeout(key, connectionString);
            }

            return connectionString;
        }

        internal static Organization GetOrganizationFromCache(Guid organizationId)
        {
            return GetOrganizationFromCache(organizationId, false);
        }

        internal static Organization GetOrganizationFromCache(Guid organizationId, bool putToCacheIfNotExists)
        {
            string key = string.Format(CultureInfo.InvariantCulture, OrganizationKeyFormat, organizationId);
            Organization organization = CacheManager.Current.Get(key) as Organization;

            if (organization == null)
            {
                if (putToCacheIfNotExists)
                {
                    organization = GetOrganization(organizationId);

                    if (organization != null)
                        CacheManager.Current.PutWithDefaultTimeout(key, organization);
                }
            }

            return organization;
        }

        internal static Organization GetOrganizationByPseudoIdFromCache(string pseudoId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, OrganizationByPseudoIdKeyFormat, pseudoId);
            Organization organization = CacheManager.Current.Get(key) as Organization;

            if (organization == null)
            {
                organization = GetOrganizationByPseudoId(pseudoId);

                if (organization != null)
                    CacheManager.Current.PutWithDefaultTimeout(key, organization);
            }

            return organization;
        }

        internal static Guid GetWebsiteIdFromCache(Guid organizationId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, WebsiteIdKeyFormat, organizationId);
            object value = CacheManager.Current.Get(key);

            if (value == null)
            {
                Guid websiteId = WebsiteProvider.GetWebsiteIdByOrganizationId(organizationId);

                if (websiteId != Guid.Empty)
                    CacheManager.Current.PutWithDefaultTimeout(key, websiteId);
            }
            else
                return (Guid)value;

            return Guid.Empty;
        }

        internal static void PutOrganizationToCache(Organization organization)
        {
            string key = string.Format(CultureInfo.InvariantCulture, OrganizationKeyFormat, organization.OrganizationId);
            CacheManager.Current.PutWithDefaultTimeout(key, organization);
        }

        internal static void RemoveOrganizationFromCache(Guid organizationId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, OrganizationKeyFormat, organizationId);
            CacheManager.Current.Remove(key);
        }

        internal static void RemoveConnectionStringFromCache(Guid organizationId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, ConnectionStringKeyFormat, organizationId);
            CacheManager.Current.Remove(key);
        }

        internal static void RemoveWebsiteIdFromCache(Guid organizationId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, WebsiteIdKeyFormat, organizationId);
            CacheManager.Current.Remove(key);
        }

        #endregion

        internal static Organization CreateOrganization(MasterDataSet.OrganizationRow row)
        {
            if (row != null)
            {
                Organization organization = new Organization();

                organization.OrganizationId = row.OrganizationId;
                organization.PseudoId = row.PseudoId;
                if (!row.IsParentOrganizationIdNull()) organization.ParentOrganizationId = new Guid?(row.ParentOrganizationId);
                organization.Name = row.Name;
                organization.Description = row.Description;
                organization.WebsiteUrl = row.WebsiteUrl;
                if (!row.IsDatabaseIdNull()) organization.DatabaseId = new Guid?(row.DatabaseId);
                if (!row.IsFiscalYearStartMonthNull()) organization.FiscalYearStartMonth = new int?(row.FiscalYearStartMonth);
                if (!row.IsFiscalYearStartDayNull()) organization.FiscalYearStartDay = new int?(row.FiscalYearStartDay);
                if (!row.IsWeekStartsDayNull()) organization.WeekStartsDay = new int?(row.WeekStartsDay);
                organization.LdapServerAddress = row.LdapServerAddress;
                organization.LdapServerPort = string.IsNullOrEmpty(row.LdapServerPort) ? DefaultLdapServerPort : row.LdapServerPort;
                organization.LdapDomain = row.LdapDomain;
                organization.LdapUserName = row.LdapUserName;
                organization.LdapPassword = row.LdapPassword;
                if (!string.IsNullOrEmpty(row.LdapDomains)) organization.LdapDomains = row.LdapDomains;
                if (!row.IsExpirationTimeNull()) organization.ExpirationTime = new DateTime?(row.ExpirationTime);
                organization.GraceDays = row.GraceDays;
                if (!string.IsNullOrEmpty(row.ExternalId)) organization.ExternalId = row.ExternalId;
                organization.Active = row.Active;
                if (!row.IsCanceledTimeNull()) organization.CanceledTime = new DateTime?(row.CanceledTime);
                organization.Trial = row.Trial;
                organization.Beta = row.Beta;
                if (!row.IsCreatedTimeNull()) organization.CreatedTime = new DateTime?(row.CreatedTime);
                organization.Deleted = row.Deleted;
                organization.Street = row.Street;
                organization.Street2 = row.Street2;
                organization.City = row.City;
                organization.State = row.State;
                organization.PostalCode = row.PostalCode;
                organization.Country = row.Country;
                organization.Currency = row.Currency;
                organization.HowYouHearAboutUs = row.HowYouHearAboutUs;

                return organization;
            }

            return null;
        }

        /// <summary>
        /// Creates and returns the organizations collection sorted by name from specified data sourceRow.
        /// </summary>
        /// <param name="table">A System.Data.DataTable object that represents the data sourceRow to create a collection from.</param>
        /// <returns>A Micajah.Common.Bll.OrganizationCollection object that contains the organizations.</returns>
        internal static OrganizationCollection CreateOrganizationCollection(MasterDataSet.OrganizationDataTable table)
        {
            OrganizationCollection coll = new OrganizationCollection();
            if (table != null)
            {
                foreach (MasterDataSet.OrganizationRow row in table)
                {
                    coll.Add(CreateOrganization(row));
                }
            }
            return coll;
        }

        internal static MasterDataSet.OrganizationRow GetOrganizationRow(Guid organizationId)
        {
            using (OrganizationTableAdapter adapter = new OrganizationTableAdapter())
            {
                MasterDataSet.OrganizationDataTable table = adapter.GetOrganization(organizationId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        internal static void UpdateOrganizationsPseudoId()
        {
            using (OrganizationTableAdapter adapter = new OrganizationTableAdapter())
            {
                MasterDataSet.OrganizationDataTable table = adapter.GetOrganizations(null);

                foreach (MasterDataSet.OrganizationRow organizationRow in table)
                {
                    if (string.IsNullOrEmpty(organizationRow.PseudoId))
                        organizationRow.PseudoId = Support.GeneratePseudoUnique();

                    InstanceProvider.UpdateInstancesPseudoId(organizationRow.OrganizationId);
                }

                adapter.Update(table);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the organizations, excluding marked as deleted.
        /// </summary>
        /// <returns>The table that contains organizations.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MasterDataSet.OrganizationDataTable GetOrganizations()
        {
            return GetOrganizations(false);
        }

        public static MasterDataSet.OrganizationDataTable GetOrganizations(bool? deleted)
        {
            using (OrganizationTableAdapter adapter = new OrganizationTableAdapter())
            {
                return adapter.GetOrganizations(deleted);
            }
        }

        public static MasterDataSet.OrganizationDataTable GetOrganizationsByLdapDomain(string ldapDomain)
        {
            using (OrganizationTableAdapter adapter = new OrganizationTableAdapter())
            {
                return adapter.GetOrganizationsByLdapDomain(ldapDomain);
            }
        }

        public static MasterDataSet.OrganizationDataTable GetOrganizationsByParentOrganizationId(Guid? organizationId)
        {
            using (OrganizationTableAdapter adapter = new OrganizationTableAdapter())
            {
                return adapter.GetOrganizationsByParentOrganizationId(organizationId);
            }
        }

        public static MasterDataSet.OrganizationDataTable GetOrganizationsByParentOrganizationIdAndDatabaseId(Guid databaseId, Guid? organizationId)
        {
            MasterDataSet.OrganizationDataTable table = GetOrganizationsByParentOrganizationId(null);

            if (organizationId.HasValue)
            {
                MasterDataSet.OrganizationRow row = table.FindByOrganizationId(organizationId.Value);
                if (row != null)
                {
                    row.Delete();
                    table.AcceptChanges();
                }
            }

            for (int x = 0; x < table.Count; x++)
            {
                MasterDataSet.OrganizationRow row = table[x];
                if (!row.IsDatabaseIdNull())
                {
                    if (row.DatabaseId != databaseId)
                        row.Delete();
                }
                else if (databaseId != Guid.Empty)
                    row.Delete();
            }

            table.AcceptChanges();

            return table;
        }

        /// <summary>
        /// Gets the organizations with additional information, such as the full name of the database.
        /// </summary>
        /// <param name="includeAdditionalInfo">The flag indicating that the additional information is included in result.</param>        
        /// <returns>The System.Data.DataTable that contains organizations.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetOrganizations(bool includeAdditionalInfo, string name, int statusId)
        {
            MasterDataSet.OrganizationDataTable table = GetOrganizations(null);

            if ((!string.IsNullOrEmpty(name)) || statusId > 0)
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(name))
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0} LIKE '%{1}%'", table.NameColumn.ColumnName, Support.PreserveSingleQuote(name));

                if (statusId > 0)
                {
                    if (sb.Length > 0) sb.Append(" AND ");
                    switch (statusId)
                    {
                        case 1:
                            sb.AppendFormat(CultureInfo.InvariantCulture, "{0} = 1 AND {1} = 0", table.ActiveColumn.ColumnName, table.DeletedColumn.ColumnName);
                            break;
                        case 2:
                            sb.AppendFormat(CultureInfo.InvariantCulture, "{0} = 0 AND {1} = 0", table.ActiveColumn.ColumnName, table.DeletedColumn.ColumnName);
                            break;
                        case 3:
                            sb.AppendFormat(CultureInfo.InvariantCulture, "{0} = 1", table.DeletedColumn.ColumnName);
                            break;
                    }
                }

                table.DefaultView.RowFilter = sb.ToString();
            }

            if (includeAdditionalInfo)
            {
                table.Columns.Add("DatabaseServerFullName", typeof(string));
                table.Columns.Add("DatabaseName", typeof(string));
                table.Columns.Add("ParentOrganizationName", typeof(string));

                Guid databaseId = Guid.Empty;
                foreach (DataRowView drv in table.DefaultView)
                {
                    if (drv.Row.IsNull("DatabaseId"))
                    {
                        drv["DatabaseServerFullName"] = string.Empty;
                        drv["DatabaseName"] = Resources.OrganizationProvider_MasterDatabaseText;
                    }
                    else
                    {
                        databaseId = (Guid)drv["DatabaseId"];
                        if (DatabaseProvider.GetDatabaseRow(databaseId) == null)
                            drv["DatabaseServerFullName"] = drv["DatabaseName"] = "Error:DatabaseDoesntExistOrInactive";
                        else
                        {
                            drv["DatabaseServerFullName"] = DatabaseServerProvider.GetDatabaseServerFullNameByDatabaseId(databaseId);
                            drv["DatabaseName"] = DatabaseProvider.GetDatabaseName(databaseId);
                        }
                    }

                    if (drv.Row.IsNull("ParentOrganizationId"))
                        drv["ParentOrganizationName"] = string.Empty;
                    else
                        drv["ParentOrganizationName"] = GetName((Guid)drv["ParentOrganizationId"]);
                }
            }

            return table.DefaultView;
        }

        /// <summary>
        /// Returns an Micajah.Common.Bll.Organization object populated with information of the specified organization.
        /// </summary>
        /// <param name="organizationId">Specifies the organization identifier to get information.</param>
        /// <returns>
        /// The Micajah.Common.Bll.Organization object populated with information of the specified organization. 
        /// If the organization is not found, the method returns null reference.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static Organization GetOrganization(Guid organizationId)
        {
            return CreateOrganization(GetOrganizationRow(organizationId));
        }

        /// <summary>
        /// Returns an first organization in which the specified setting with specified value exists.
        /// </summary>
        /// <param name="shortName">The short name of the setting to search for.</param>
        /// <param name="value">The value of the setting to search for.</param>
        /// <returns>The Micajah.Common.Bll.Organization object that represents the first organization in which the specified setting with specified value exists;</returns>
        public static Organization GetOrganizationBySettingValue(string shortName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                foreach (MasterDataSet.OrganizationRow row in GetOrganizations())
                {
                    SettingCollection settings = SettingProvider.GetOrganizationSettings(row.OrganizationId);
                    Setting setting = settings.FindByShortName(shortName);

                    if ((setting != null) && (string.Compare(value, setting.Value, StringComparison.Ordinal) == 0))
                        return CreateOrganization(row);
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the organizations in which the specified setting with specified value exists.
        /// </summary>
        /// <param name="shortName">The short name of the setting to search for.</param>
        /// <param name="value">The value of the setting to search for.</param>
        /// <returns>The Micajah.Common.Bll.OrganizationCollection object that represents the collection of the organizations in which the specified setting with specified value exists.</returns>
        public static OrganizationCollection GetOrganizationsBySettingValue(string shortName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                OrganizationCollection coll = new OrganizationCollection();

                foreach (MasterDataSet.OrganizationRow row in GetOrganizations())
                {
                    SettingCollection settings = SettingProvider.GetOrganizationSettings(row.OrganizationId);
                    Setting setting = settings.FindByShortName(shortName);

                    if ((setting != null) && (string.Compare(value, setting.Value, StringComparison.Ordinal) == 0))
                        coll.Add(CreateOrganization(row));
                }

                return coll;
            }
            return null;
        }

        /// <summary>
        /// Gets the organizations, excluding marked as deleted.
        /// </summary>
        /// <param name="includeExpired">The flag indicating that the expired organizations are included in result.</param>
        /// <param name="includeInactive">The flag indicating that the inacive organizations are included in result.</param>
        /// <returns>The organizations collection.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static OrganizationCollection GetOrganizations(bool includeExpired, bool includeInactive)
        {
            OrganizationCollection orgs = CreateOrganizationCollection(GetOrganizations());

            if ((!includeExpired) || (!includeInactive))
            {
                OrganizationCollection coll = new OrganizationCollection();

                foreach (Organization org in orgs)
                {
                    if (!includeExpired)
                    {
                        if (org.Expired && (org.GraceDaysRemaining == 0))
                            continue;
                    }

                    if (!includeInactive)
                    {
                        if (!org.Active)
                            continue;
                    }

                    coll.Add(org);
                }

                orgs = coll;
            }

            return orgs;
        }

        /// <summary>
        /// Returns the child organizations collection for the specific organization.
        /// Generates an System.Data.DataException exception if error occured.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization to get child organizations.</param>
        /// <param name="includeInactive">The flag indicating that the inacive organizations are included in result.</param>
        /// <returns>The organizations collection.</returns>
        public static OrganizationCollection GetChildOrganizations(Guid organizationId, bool includeInactive)
        {
            OrganizationCollection coll = new OrganizationCollection();

            MasterDataSet.OrganizationDataTable table = GetOrganizationsByParentOrganizationId(organizationId);

            foreach (MasterDataSet.OrganizationRow row in table)
            {
                if (!includeInactive && !row.Active)
                    continue;

                coll.Add(CreateOrganization(row));
            }

            return coll;
        }

        public static OrganizationCollection GetOrganizationsByLoginId(Guid loginId)
        {
            using (OrganizationTableAdapter adapter = new OrganizationTableAdapter())
            {
                return CreateOrganizationCollection(adapter.GetOrganizationsByLoginId(loginId));
            }
        }

        /// <summary>
        /// Returns the connection string to organization database.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization to get connection string.</param>
        /// <returns>The connection string to organization database.</returns>
        public static string GetConnectionString(Guid organizationId)
        {
            return GetConnectionStringFromCache(organizationId);
        }

        /// <summary>
        /// Returns the name of the specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization to get name.</param>
        /// <returns>The name of the specified organization.</returns>
        public static string GetName(Guid organizationId)
        {
            MasterDataSet.OrganizationRow row = GetOrganizationRow(organizationId);
            if (row != null) return row.Name;
            return string.Empty;
        }

        /// <summary>
        /// Returns the identifier of the specified organization.
        /// </summary>
        /// <param name="name">The name of the organization.</param>
        /// <returns>The identifier of the specified organization.</returns>
        public static Guid GetOrganizationIdByName(string name)
        {
            MasterDataSet.OrganizationRow row = GetOrganizationRowByName(name);
            if ((row != null) && (!row.Deleted))
                return row.OrganizationId;
            return Guid.Empty;
        }

        /// <summary>
        /// Returns the organization by specified name.
        /// </summary>
        /// <param name="name">The name of the organization.</param>
        /// <returns>The organization.</returns>
        public static Organization GetOrganizationByName(string name)
        {
            MasterDataSet.OrganizationRow row = GetOrganizationRowByName(name);
            if ((row != null) && (!row.Deleted))
                return CreateOrganization(row);
            return null;
        }

        /// <summary>
        /// Returns an Micajah.Common.Bll.Organization object populated with information of the specified organization.
        /// </summary>
        /// <param name="pseudoId">The pseudo unique identifier of the organization.</param>
        /// <returns>The Micajah.Common.Bll.Organization object populated with information of the specified organization. If the organization is not found, the method returns null reference.</returns>
        public static Organization GetOrganizationByPseudoId(string pseudoId)
        {
            if (!string.IsNullOrEmpty(pseudoId))
            {
                MasterDataSet.OrganizationRow row = GetOrganizationRowByPseudoId(pseudoId);
                if ((row != null) && (!row.Deleted))
                    return CreateOrganization(row);
            }
            return null;
        }

        /// <summary>
        /// Returns the true if the organization exists; otherwise, false.
        /// </summary>
        /// <param name="name">The name of the organization.</param>
        /// <returns>true if the organization exists; otherwise, false.</returns>
        public static bool OrganizationExists(string name)
        {
            return (GetOrganizationIdByName(name) != Guid.Empty);
        }

        /// <summary>
        /// Returns the database identifier where the specified organization is placed.
        /// </summary>
        /// <param name="name">The name of the organization.</param>
        /// <returns>The database identifier or 0 (zero) if the specified organization is not found.</returns>
        public static Guid GetOrganizationDatabaseId(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                MasterDataSet.OrganizationRow row = GetOrganizationRowByName(name);
                if (row != null)
                {
                    if (!row.IsDatabaseIdNull())
                        return row.DatabaseId;
                }
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Creates new organization with specified details and send a notification e-mail to administrator.
        /// </summary>
        /// <param name="name">The name of the organization.</param>
        /// <param name="description">The organization description.</param>
        /// <param name="websiteUrl">The URL of the organization web-site.</param>
        /// <param name="databaseId">The identifier of the organization's database.</param>
        /// <param name="adminEmail">The e-mail address of the organization administrator.</param>
        /// <returns>The unique identifier of the newly created organization.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertOrganization(string name, string description, string websiteUrl, Guid? databaseId, string adminEmail)
        {
            return InsertOrganization(name, description, websiteUrl, databaseId
                , null, null, null
                , null, 0, true, null, false
                , null, null, null, null, null, null, null, null
                , null, null
                , adminEmail, null, null, null, null, null, null, null, null, null
                , true, null);
        }

        /// <summary>
        /// Creates new organization with specified details
        /// and refreshes all cached data by application after the action has been performed.
        /// </summary>
        /// <param name="name">The name of the organization.</param>
        /// <param name="description">The organization description.</param>
        /// <param name="websiteUrl">The URL of the organization web-site.</param>
        /// <param name="databaseId">The identifier of the organization's database.</param>      
        /// <param name="adminEmail">The e-mail address of the organization administrator.</param>
        /// <param name="firstName">The first name of the organization administrator.</param>
        /// <param name="lastName">The last name of the organization administrator.</param>
        /// <param name="middleName">The middle name of the organization administrator.</param>
        /// <param name="sendNotificationEmail">true to send notification email to administrator; otherwise, false.</param>
        /// <returns>The unique identifier of the newly created organization.</returns>
        public static Guid InsertOrganization(string name, string description, string websiteUrl, Guid? databaseId
            , string adminEmail, string firstName, string lastName, string middleName, bool sendNotificationEmail)
        {
            return InsertOrganization(name, description, websiteUrl, databaseId
                , null, null, null
                , null, 0, true, null, false
                , null, null, null, null, null, null, null, null
                , null, null
                , adminEmail, null, firstName, lastName, middleName, null, null, null, null, null
                , sendNotificationEmail, null);
        }

        /// <summary>
        /// Creates new organization with specified details and send a notification e-mail to administrator.
        /// </summary>
        /// <param name="name">The name of the organization.</param>
        /// <param name="description">The organization description.</param>
        /// <param name="websiteUrl">The URL of the organization web-site.</param>
        /// <param name="databaseId">The identifier of the organization's database.</param>
        /// <param name="adminEmail">The e-mail address of the organization administrator.</param>
        /// <param name="expirationTime">The expiration date and time of the organization</param>
        /// <param name="graceDays">The grace days number when the users be able to log in to the expired organization.</param>
        /// <param name="active">Whether the organization is active.</param>
        /// <returns>The unique identifier of the newly created organization.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertOrganization(string name, string description, string websiteUrl, Guid? databaseId, string adminEmail
            , DateTime? expirationTime, int graceDays, bool active, DateTime? canceledTime, bool trial, Guid? parentOrganizationId)
        {
            return InsertOrganization(name, description, websiteUrl, databaseId
                , null, null, null
                , expirationTime, graceDays, active, canceledTime, trial
                , null, null, null, null, null, null, null, null
                , null, null
                , adminEmail, null, null, null, null, null, null, null, null, null
                , true, parentOrganizationId);
        }

        /// <summary>
        /// Creates new organization with specified details and send a notification e-mail to administrator.
        /// </summary>
        /// <param name="name">The name of the organization.</param>
        /// <param name="description">The organization description.</param>
        /// <param name="websiteUrl">The URL of the organization web-site.</param>
        /// <param name="street">The organization's street.</param>
        /// <param name="street2">The organization's secondary street.</param>
        /// <param name="city">The organization's city.</param>
        /// <param name="state">The organization's state/province.</param>
        /// <param name="postalCode">The organization's postal code.</param>
        /// <param name="country">The organization's country.</param>
        /// <param name="currency">The organization's currency.</param>
        /// <param name="timeZoneId">The time zone identifier.</param>
        /// <param name="templateInstanceId">The identifier of the template instance for the organization's first instance.</param>
        /// <param name="adminEmail">The e-mail address of the organization administrator.</param>
        /// <param name="password">The password of the organization administrator.</param>
        /// <param name="firstName">The first name of the organization administrator.</param>
        /// <param name="lastName">The last name of the organization administrator.</param>
        /// <param name="title">The title of the organization administrator.</param>
        /// <param name="phone">The phone of the organization administrator.</param>
        /// <param name="mobilePhone">The mobile phone of the organization administrator.</param>
        /// <param name="sendNotificationEmail">true to send notification email to administrator; otherwise, false.</param>
        /// <returns>The unique identifier of the newly created organization.</returns>
        public static Guid InsertOrganization(string name, string description, string websiteUrl
            , string street, string street2, string city, string state, string postalCode, string country, string currency, string howYouHearAboutUs
            , string timeZoneId, Guid? templateInstanceId
            , string adminEmail, string password, string firstName, string lastName, string title, string phone, string mobilePhone
            , string partialCustomUrl, HttpRequest request
            , bool sendNotificationEmail)
        {
            return InsertOrganization(name, description, websiteUrl, DatabaseProvider.GetRandomPublicDatabaseId()
                , null, null, null
                , null, 0, true, null, true
                , street, street2, city, state, postalCode, country, currency, howYouHearAboutUs
                , timeZoneId, templateInstanceId
                , adminEmail, password, firstName, lastName, null, title, phone, mobilePhone
                , partialCustomUrl, request
                , sendNotificationEmail, null);
        }

        /// <summary>
        /// Updates the details of specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="name">The name of the organization.</param>
        /// <param name="description">The organization description.</param>
        /// <param name="websiteUrl">The URL of the organization web-site.</param>
        /// <param name="fiscalYearStartMonth">The fiscal year start month of the organization.</param>         
        /// <param name="fiscalYearStartDay">The fiscal year start day of the organization.</param>         
        /// <param name="weekStartsDay">The week starts day of the organization.</param>         
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateOrganization(Guid organizationId, string name, string description, string websiteUrl
            , int? fiscalYearStartMonth, int? fiscalYearStartDay, int? weekStartsDay)
        {
            UpdateOrganization(organizationId, name, description, websiteUrl, Guid.Empty, fiscalYearStartMonth, fiscalYearStartDay, weekStartsDay, DateTime.MinValue, null, null, null, null, null, null, null, null);
        }

        /// <summary>
        /// Updates the details of specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="name">The name of the organization.</param>
        /// <param name="description">The organization description.</param>
        /// <param name="websiteUrl">The URL of the organization web-site.</param>
        /// <param name="fiscalYearStartMonth">The fiscal year start month of the organization.</param>         
        /// <param name="fiscalYearStartDay">The fiscal year start day of the organization.</param>         
        /// <param name="weekStartsDay">The week starts day of the organization.</param>         
        /// <param name="emailSuffixes">The organization email suffixes.</param>
        /// <param name="ldapDomains">The organization ldap domains.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateOrganization(Guid organizationId, string name, string description, string websiteUrl
            , int? fiscalYearStartMonth, int? fiscalYearStartDay, int? weekStartsDay, string emailSuffixes, string ldapDomains, bool? beta)
        {
            UpdateOrganization(organizationId, name, description, websiteUrl, Guid.Empty, fiscalYearStartMonth, fiscalYearStartDay, weekStartsDay, DateTime.MinValue, null, null, null, null, beta, emailSuffixes, ldapDomains, null);
        }

        /// <summary>
        /// Updates the details of specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="name">The name of the organization.</param>
        /// <param name="description">The organization description.</param>
        /// <param name="websiteUrl">The URL of the organization web-site.</param>
        /// <param name="databaseId">The identifier of the organization's database.</param>
        /// <param name="fiscalYearStartMonth">The fiscal year start month of the organization.</param>         
        /// <param name="fiscalYearStartDay">The fiscal year start day of the organization.</param>         
        /// <param name="weekStartsDay">The week starts day of the organization.</param>
        /// <param name="expirationTime">The expiration date and time of the organization</param>
        /// <param name="graceDays">The grace days number when the users be able to log in to the expired organization.</param>
        /// <param name="active">Whether the organization is active.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateOrganization(Guid organizationId, string name, string description, string websiteUrl, Guid? databaseId
            , int? fiscalYearStartMonth, int? fiscalYearStartDay, int? weekStartsDay
            , DateTime? expirationTime, int graceDays, bool active, DateTime? canceledTime, bool trial, Guid? parentOrganizationId)
        {
            UpdateOrganization(organizationId, name, description, websiteUrl, databaseId, fiscalYearStartMonth, fiscalYearStartDay, weekStartsDay, expirationTime, new int?(graceDays), new bool?(active), canceledTime, trial, null, null, null, parentOrganizationId);
        }

        /// <summary>
        /// Updates the details of specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="LdapServerAddress">The Ldap Server Address of the organization.</param>
        /// <param name="LdapServerPort">The organization Ldap Server Port.</param>
        /// <param name="LdapDomain">The organization Ldap Domain.</param>
        /// <param name="LdapUserName">The Ldap User Name of the organization.</param>
        /// <param name="LdapPassword">The Ldap Password of the organization.</param>         
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateOrganization(Guid organizationId, string ldapServerAddress, string ldapServerPort, string ldapDomain, string ldapUserName, string ldapUpdatePassword, string ldapConfirmNewPassword)
        {
            MasterDataSet.OrganizationRow row = GetOrganizationRow(organizationId);
            if (row != null)
            {
                row.LdapServerAddress = Support.TrimString(ldapServerAddress, LdapServerAddressMaxLength);
                row.LdapServerPort = Support.TrimString(ldapServerPort, LdapServerPortMaxLength);
                row.LdapDomain = Support.TrimString(ldapDomain, LdapDomainMaxLength);
                row.LdapUserName = Support.TrimString(ldapUserName, LdapUserNameMaxLength);
                if (String.IsNullOrEmpty(ldapUpdatePassword) == false && String.IsNullOrEmpty(ldapConfirmNewPassword) == false && ldapUpdatePassword == ldapConfirmNewPassword)
                    row.LdapPassword = Support.TrimString(ldapUpdatePassword, LdapPasswordMaxLength);

                UpdateOrganizationRow(row);
            }
        }

        /// <summary>
        /// Updates the details of specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="LdapDomains">The organization Ldap Domains.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateOrganization(Guid organizationId, string ldapDomains)
        {
            MasterDataSet.OrganizationRow row = GetOrganizationRow(organizationId);
            if (row != null)
            {
                row.LdapDomains = Support.TrimString(ldapDomains, LdapDomainsMaxLength);

                UpdateOrganizationRow(row);
            }
        }

        /// <summary>
        /// Updates the details of specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="expirationTime">The organization Expiration Time.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateOrganization(Guid organizationId, DateTime expirationTime)
        {
            MasterDataSet.OrganizationRow row = GetOrganizationRow(organizationId);
            if (row != null)
            {
                row.ExpirationTime = expirationTime;

                UpdateOrganizationRow(row);
            }
        }

        /// <summary>
        /// Updates the active flag of the specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="active">true, if the organization is active; otherwise, false.</param>
        public static void UpdateOrganizationActive(Guid organizationId, bool active)
        {
            MasterDataSet.OrganizationRow row = GetOrganizationRow(organizationId);
            if (row != null)
            {
                row.Active = active;

                UpdateOrganizationRow(row);
            }
        }

        /// <summary>
        /// Sets the deleted flag to true of the specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>        
        public static void UndeleteOrganization(Guid organizationId)
        {
            MasterDataSet.OrganizationRow row = GetOrganizationRow(organizationId);
            if (row != null)
            {
                row.Deleted = false;

                UpdateOrganizationRow(row);
            }
        }

        /// <summary>
        /// Marks as deleted the specified organization.
        /// </summary>
        /// <param name="organizationId">Specifies the organization's identifier.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteOrganization(Guid organizationId)
        {
            MasterDataSet.OrganizationRow row = GetOrganizationRow(organizationId);
            if (row != null)
            {
                row.Deleted = true;

                using (OrganizationTableAdapter adapter = new OrganizationTableAdapter())
                {
                    adapter.Update(row);
                }

                RemoveConnectionStringFromCache(organizationId);
                RemoveWebsiteIdFromCache(organizationId);
                RemoveOrganizationFromCache(organizationId);
                SettingProvider.RemoveOrganizationSettingsValuesFromCache(organizationId);
                SettingProvider.RemoveOrganizationEmailSettingsFromCache(organizationId);
                CustomUrlProvider.RemoveOrganizationCustomUrlFromCache(organizationId);
                ResourceProvider.RemoveOrganizationLogoImageUrlFromCache(organizationId);
            }
        }

        #endregion
    }

    /// <summary>
    /// The class containing the data for the events of Micajah.Common.Bll.Providers.OrganizationProvider class.
    /// </summary>
    public class OrganizationProviderEventArgs : EventArgs
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        public Organization Organization { get; set; }

        #endregion
    }
}
