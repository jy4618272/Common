using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;

namespace Micajah.Common.Bll
{
    /// <summary>
    /// The organization.
    /// </summary>
    [Serializable]
    public sealed class Organization : IComparable<Organization>
    {
        #region Members

        private Guid m_OrganizationId;
        private string m_PseudoId;
        private string m_Name;
        private string m_Description;
        private string m_WebsiteUrl;
        private Guid? m_LogoImageResourceId;
        private Guid? m_DatabaseId;
        private int? m_FiscalYearStartMonth;
        private int? m_FiscalYearStartDay;
        private int? m_WeekStartsDay;
        private string m_LdapServerAddress;
        private string m_LdapServerPort;
        private string m_LdapDomain;
        private string m_LdapUserName;
        private string m_LdapPassword;
        private string m_LdapDomains;
        private DateTime? m_ExpirationTime;
        private int m_GraceDays;
        private string m_ExternalId;
        private bool m_Active;
        private DateTime? m_CanceledTime;
        private bool m_Trial;
        private bool m_Beta;
        private DateTime? m_CreatedTime;
        private string m_ConnectionString;
        private bool m_LogoImageResourceIdLoaded;
        private SettingCollection m_Settings;
        private InstanceCollection m_Instances;
        private EmailElement m_EmailSettings;
        private string m_EmailSuffixes;
        private Collection<string> m_EmailSuffixesList;
        private string m_CustomStyleSheet;
        private bool m_Deleted;
        private bool m_Visible;
        private BillingPlan m_BillingPlan;
        private string m_Street;
        private string m_Street2;
        private string m_City;
        private string m_State;
        private string m_PostalCode;
        private string m_Country;
        private string m_Currency;

        // The objects which are used to synchronize access to the cached objects.
        private object m_SettingsSyncRoot = new object();
        private object m_EmailSettingsSyncRoot = new object();
        private object m_EmailSuffixSyncRoot = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Micajah.Common.Bll.Organization class.
        /// </summary>
        public Organization()
        {
            m_PseudoId = string.Empty;
            m_Name = string.Empty;
            m_Description = string.Empty;
            m_WebsiteUrl = string.Empty;
            m_LdapServerAddress = string.Empty;
            m_LdapServerPort = string.Empty;
            m_LdapDomain = string.Empty;
            m_LdapUserName = string.Empty;
            m_LdapPassword = string.Empty;
            m_LdapDomains = string.Empty;
            m_ExternalId = string.Empty;
            m_Active = true;
            m_Visible = true;
            m_BillingPlan = BillingPlan.Free;
            m_Street = string.Empty;
            m_Street2 = string.Empty;
            m_City = string.Empty;
            m_State = string.Empty;
            m_PostalCode = string.Empty;
            m_Country = string.Empty;
            m_Currency = string.Empty;
        }

        #endregion

        #region Internal Properties

        internal string CustomStyleSheet
        {
            get
            {
                if (m_CustomStyleSheet == null)
                {
                    m_CustomStyleSheet = string.Empty;
                    Setting setting = this.Settings.FindBySettingId(SettingProvider.MasterPageCustomStyleSheetSettingId);
                    if (setting != null)
                    {
                        if (!Support.StringIsNullOrEmpty(setting.Value))
                            m_CustomStyleSheet = setting.Value;
                    }
                }
                return m_CustomStyleSheet;
            }
            set
            {
                Setting setting = this.Settings.FindBySettingId(SettingProvider.MasterPageCustomStyleSheetSettingId);
                if (setting != null)
                {
                    setting.Value = value;
                    this.Settings.UpdateValues(this.OrganizationId);
                    m_CustomStyleSheet = null;
                }
            }
        }

        /// <summary>
        /// Gets the dataset of the organization.
        /// </summary>
        internal OrganizationDataSet DataSet
        {
            get { return WebApplication.GetOrganizationDataSetByOrganizationId(OrganizationId); }
        }

        /// <summary>
        /// Gets the table adapters of the organization dataset.
        /// </summary>
        internal OrganizationDataSetTableAdapters TableAdapters
        {
            get { return WebApplication.GetOrganizationDataSetTableAdaptersByConnectionString(this.ConnectionString); }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the identifier of the organization.
        /// </summary>
        public Guid OrganizationId
        {
            get { return m_OrganizationId; }
            set { m_OrganizationId = value; }
        }

        /// <summary>
        /// Gets or sets the pseudo unique identifier of the organization.
        /// </summary>
        public string PseudoId
        {
            get { return m_PseudoId; }
            set { m_PseudoId = value; }
        }

        /// <summary>
        /// Gets or sets the name of the organization.
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// Gets or sets the organization description.
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the organization web-site.
        /// </summary>
        public string WebsiteUrl
        {
            get { return m_WebsiteUrl; }
            set { m_WebsiteUrl = value; }
        }

        /// <summary>
        /// Gets or sets the identifier of the logo image.
        /// </summary>
        public Guid? LogoImageResourceId
        {
            get
            {
                if (!m_LogoImageResourceIdLoaded)
                {
                    CommonDataSet.ResourceRow resourceRow = ResourceProvider.GetResourceRow("OrganizationLogo", m_OrganizationId.ToString("N"));
                    if (resourceRow != null) m_LogoImageResourceId = resourceRow.ResourceId;
                    m_LogoImageResourceIdLoaded = true;
                }
                return m_LogoImageResourceId;
            }
            set { m_LogoImageResourceId = value; }
        }

        /// <summary>
        /// Gets the URL of the logo image.
        /// </summary>
        public string LogoImageUrl
        {
            get { return (this.LogoImageResourceId.HasValue ? ResourceProvider.GetResourceUrl(this.LogoImageResourceId.Value, 300, 45, true) : null); }
        }

        /// <summary>
        /// Gets or sets identifier of the organization's database.
        /// </summary>
        public Guid? DatabaseId
        {
            get { return m_DatabaseId; }
            set { m_DatabaseId = value; }
        }

        /// <summary>
        /// Gets or sets the fiscal year start month.
        /// </summary>
        public int? FiscalYearStartMonth
        {
            get { return m_FiscalYearStartMonth; }
            set { m_FiscalYearStartMonth = value; }
        }

        /// <summary>
        /// Gets or sets the fiscal year start day.
        /// </summary>
        public int? FiscalYearStartDay
        {
            get { return m_FiscalYearStartDay; }
            set { m_FiscalYearStartDay = value; }
        }

        /// <summary>
        /// Gets the first day of the week.
        /// </summary>
        public FirstDayOfWeek StartDayOfWeek
        {
            get { return (m_WeekStartsDay.HasValue ? (FirstDayOfWeek)m_WeekStartsDay.Value : FirstDayOfWeek.Monday); }
        }

        /// <summary>
        /// Gets or sets the first day of the week.
        /// </summary>
        public int? WeekStartsDay
        {
            get { return m_WeekStartsDay; }
            set { m_WeekStartsDay = value; }
        }

        /// <summary>
        /// Gets or sets the LDAP server address of the organization.
        /// </summary>
        public string LdapServerAddress
        {
            get { return m_LdapServerAddress; }
            set { m_LdapServerAddress = value; }
        }

        /// <summary>
        /// Gets or sets the LDAP server port of the organization.
        /// </summary>
        public string LdapServerPort
        {
            get { return m_LdapServerPort; }
            set { m_LdapServerPort = value; }
        }

        /// <summary>
        /// Gets or sets the LDAP server domain of the organization.
        /// </summary>
        public string LdapDomain
        {
            get { return m_LdapDomain; }
            set { m_LdapDomain = value; }
        }

        /// <summary>
        /// Gets or sets the LDAP server user name of the organization.
        /// </summary>
        public string LdapUserName
        {
            get { return m_LdapUserName; }
            set { m_LdapUserName = value; }
        }

        /// <summary>
        /// Gets or sets the LDAP server password of the organization.
        /// </summary>
        public string LdapPassword
        {
            get { return m_LdapPassword; }
            set { m_LdapPassword = value; }
        }

        /// <summary>
        /// Gets or sets the LDAP server domains of the organization.
        /// </summary>
        public string LdapDomains
        {
            get { return m_LdapDomains; }
            set { m_LdapDomains = value; }
        }

        /// <summary>
        /// Gets or sets the email suffixes for the organization.
        /// </summary>
        public string EmailSuffixes
        {
            get
            {
                EnsureEmailSuffixesIsLoaded();
                return m_EmailSuffixes;
            }
            set
            {
                m_EmailSuffixes = value;
                m_EmailSuffixesList = null;
            }
        }

        /// <summary>
        /// Gets the list of email suffixes for the organization.
        /// </summary>
        public Collection<string> EmailSuffixesList
        {
            get
            {
                EnsureEmailSuffixesIsLoaded();
                return m_EmailSuffixesList;
            }
        }

        /// <summary>
        /// Gets or sets the expiration date and time of the organization.
        /// </summary>
        public DateTime? ExpirationTime
        {
            get { return m_ExpirationTime; }
            set { m_ExpirationTime = value; }
        }

        /// <summary>
        /// Gets or sets the grace days number when the users be able to log in to the expired organization.
        /// </summary>
        public int GraceDays
        {
            get { return m_GraceDays; }
            set { m_GraceDays = value; }
        }

        /// <summary>
        /// Gets or sets the external id of the organization.
        /// </summary>
        public string ExternalId
        {
            get { return m_ExternalId; }
            set { m_ExternalId = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating the organization is active.
        /// </summary>
        public bool Active
        {
            get { return m_Active; }
            set { m_Active = value; }
        }

        /// <summary>
        /// Gets or sets the canceled time of the organization.
        /// </summary>
        public DateTime? CanceledTime
        {
            get { return m_CanceledTime; }
            set { m_CanceledTime = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating the organization is trial.
        /// </summary>
        public bool Trial
        {
            get { return m_Trial; }
            set { m_Trial = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating that test pages will be shown for the organization.
        /// </summary>
        public bool Beta
        {
            get { return m_Beta; }
            set { m_Beta = value; }
        }

        /// <summary>
        /// Gets or sets the created time of the organization.
        /// </summary>
        public DateTime? CreatedTime
        {
            get { return m_CreatedTime; }
            set { m_CreatedTime = value; }
        }

        /// <summary>
        /// Gets the connection string of the organization's database.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (m_ConnectionString == null) m_ConnectionString = OrganizationProvider.GetConnectionString(OrganizationId);
                return m_ConnectionString;
            }
        }

        /// <summary>
        /// Gets a collection that contains the organization level settings.
        /// </summary>
        public SettingCollection Settings
        {
            get
            {
                if (m_Settings == null)
                {
                    lock (m_SettingsSyncRoot)
                    {
                        if (m_Settings == null)
                            m_Settings = SettingProvider.GetOrganizationSettings(OrganizationId);
                    }
                }
                return m_Settings;
            }
        }

        /// <summary>
        /// Gets a collection that contains the organization instances.
        /// </summary>
        public InstanceCollection Instances
        {
            get
            {
                if (m_Instances == null) m_Instances = InstanceProvider.GetInstances(this.OrganizationId, true);
                return m_Instances;
            }
        }

        public EmailElement EmailSettings
        {
            get
            {
                if (m_EmailSettings == null)
                {
                    lock (m_EmailSettingsSyncRoot)
                    {
                        if (m_EmailSettings == null)
                        {
                            EmailElement settings = new EmailElement();
                            SettingProvider.FillSettingsClass(settings, this.Settings);
                            m_EmailSettings = settings;
                        }
                    }
                }
                return m_EmailSettings;
            }
        }

        /// <summary>
        /// Gets the value indicating the organization is expired.
        /// </summary>
        public bool Expired
        {
            get
            {
                if (ExpirationTime.HasValue)
                {
                    if (m_ExpirationTime.Value.Date < DateTime.UtcNow)
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the remaining grace days number when the users be able to log in to the expired organization.
        /// </summary>
        public int GraceDaysRemaining
        {
            get
            {
                int days = 0;
                if (Expired)
                {
                    days = (int)(m_ExpirationTime.Value.Date.AddDays(m_GraceDays) - DateTime.UtcNow.Date).TotalDays;
                    if (days < 0) days = 0;
                }
                return days;
            }
        }

        /// <summary>
        /// Gets or sets the value indicating the organization is deleted.
        /// </summary>
        public bool Deleted
        {
            get { return m_Deleted; }
            set { m_Deleted = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating the organization is visible for the user on the login pages.
        /// </summary>
        public bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }

        /// <summary>
        /// Gets or sets the organization BillingPlan (Free, Paid, Custom).
        /// </summary>
        public BillingPlan BillingPlan
        {
            get { return m_BillingPlan; }
            set { m_BillingPlan = value; }
        }

        /// <summary>
        /// Gets or sets the street of the organization.
        /// </summary>
        public string Street
        {
            get { return m_Street; }
            set { m_Street = value; }
        }

        /// <summary>
        /// Gets or sets the secondary street of the organization.
        /// </summary>
        public string Street2
        {
            get { return m_Street2; }
            set { m_Street2 = value; }
        }

        /// <summary>
        /// Gets or sets the city of the organization.
        /// </summary>
        public string City
        {
            get { return m_City; }
            set { m_City = value; }
        }

        /// <summary>
        /// Gets or sets the state of the organization.
        /// </summary>
        public string State
        {
            get { return m_State; }
            set { m_State = value; }
        }

        /// <summary>
        /// Gets or sets the postal code of the organization.
        /// </summary>
        public string PostalCode
        {
            get { return m_PostalCode; }
            set { m_PostalCode = value; }
        }

        /// <summary>
        /// Gets or sets the country of the organization.
        /// </summary>
        public string Country
        {
            get { return m_Country; }
            set { m_Country = value; }
        }

        /// <summary>
        /// Gets or sets the 3-letters ISO code of the currency of the organization.
        /// </summary>
        public string Currency
        {
            get { return m_Currency; }
            set { m_Currency = value; }
        }

        #endregion

        #region Operators

        public static bool operator ==(Organization organization1, Organization organization2)
        {
            if (object.ReferenceEquals(organization1, organization2))
                return true;

            if (((object)organization1 == null) || ((object)organization2 == null))
                return false;

            return organization1.Equals(organization2);
        }

        public static bool operator !=(Organization organization1, Organization organization2)
        {
            return (!(organization1 == organization2));
        }

        public static bool operator <(Organization organization1, Organization organization2)
        {
            return (organization1.CompareTo(organization2) < 0);
        }

        public static bool operator >(Organization organization1, Organization organization2)
        {
            return (organization1.CompareTo(organization2) > 0);
        }

        #endregion

        #region Private Methods

        private void EnsureEmailSuffixesIsLoaded()
        {
            if (m_EmailSuffixesList == null)
            {
                lock (m_EmailSuffixSyncRoot)
                {
                    if (m_EmailSuffixesList == null)
                    {
                        m_EmailSuffixesList = new Collection<string>();

                        if (m_EmailSuffixes == null)
                        {
                            DataTable table = EmailSuffixProvider.GetEmailSuffixes(this.OrganizationId);
                            if (table.Rows.Count > 0)
                                m_EmailSuffixes = (string)table.Rows[0]["EmailSuffixName"];
                            else
                                m_EmailSuffixes = string.Empty;
                        }

                        if (!string.IsNullOrEmpty(m_EmailSuffixes))
                        {
                            foreach (string suffix in m_EmailSuffixes.Split(','))
                            {
                                string val = suffix.Trim();
                                if (val.Length > 0) m_EmailSuffixesList.Add(val);
                            }
                        }
                    }
                }
            }
        }

        private void Reset()
        {
            m_ConnectionString = null;
            m_EmailSuffixes = null;
            m_EmailSuffixesList = null;
            m_Instances = null;

            this.Refresh();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Loads the data of specified organization into Micajah.Common.Bll.Organization class from CommonDataSet.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization to load.</param>
        /// <returns>true, if the specified organization is found; otherwise, false.</returns>
        internal bool Load(Guid organizationId)
        {
            CommonDataSet.OrganizationRow orgRow = WebApplication.CommonDataSet.Organization.FindByOrganizationId(organizationId);
            if (orgRow != null)
            {
                Load(orgRow);

                this.Reset();

                return true;
            }
            return false;
        }

        internal void Load(CommonDataSet.OrganizationRow row)
        {
            if (row != null)
            {
                m_OrganizationId = row.OrganizationId;
                m_PseudoId = row.PseudoId;
                m_Name = row.Name;
                m_Description = row.Description;
                m_WebsiteUrl = row.WebsiteUrl;
                if (!row.IsDatabaseIdNull()) m_DatabaseId = new Guid?(row.DatabaseId);
                if (!row.IsFiscalYearStartMonthNull()) m_FiscalYearStartMonth = new int?(row.FiscalYearStartMonth);
                if (!row.IsFiscalYearStartDayNull()) m_FiscalYearStartDay = new int?(row.FiscalYearStartDay);
                if (!row.IsWeekStartsDayNull()) m_WeekStartsDay = new int?(row.WeekStartsDay);
                m_LogoImageResourceIdLoaded = false;
                m_LogoImageResourceId = null;
                m_LdapServerAddress = row.LdapServerAddress;
                m_LdapServerPort = string.IsNullOrEmpty(row.LdapServerPort) ? "636" : row.LdapServerPort;
                m_LdapDomain = row.LdapDomain;
                m_LdapUserName = row.LdapUserName;
                m_LdapPassword = row.LdapPassword;
                m_LdapDomains = (string.IsNullOrEmpty(row.LdapDomains) == true) ? string.Empty : row.LdapDomains;
                if (!row.IsExpirationTimeNull()) m_ExpirationTime = new DateTime?(row.ExpirationTime);
                m_GraceDays = row.GraceDays;
                m_ExternalId = (string.IsNullOrEmpty(row.ExternalId) == true) ? string.Empty : row.ExternalId;
                m_Active = row.Active;
                if (!row.IsCanceledTimeNull()) m_CanceledTime = new DateTime?(row.CanceledTime);
                m_Trial = row.Trial;
                m_Beta = row.Beta;
                if (!row.IsCreatedTimeNull()) m_CreatedTime = new DateTime?(row.CreatedTime);
                m_Deleted = row.Deleted;
                m_BillingPlan = (BillingPlan)row.BillingPlan;
                m_Street = row.Street;
                m_Street2 = row.Street2;
                m_City = row.City;
                m_State = row.State;
                m_PostalCode = row.PostalCode;
                m_Country = row.Country;
                m_Currency = row.Currency;

                this.Reset();
            }
        }

        /// <summary>
        /// Loads the data of specified organization into Micajah.Common.Bll.Organization class from DataRow.
        /// </summary>
        /// <param name="row">The data row of the organization to load.</param>
        internal void Load(DataRow row)
        {
            if (row != null)
            {
                m_OrganizationId = (Guid)row["OrganizationId"];

                if (row.Table.Columns.Contains("PseudoId")) m_PseudoId = (string)row["PseudoId"];

                m_Name = (string)row["Name"];
                m_Description = (string)row["Description"];
                m_WebsiteUrl = (string)row["WebSiteUrl"];
                if (!row.IsNull("DatabaseId")) m_DatabaseId = new Guid?((Guid)row["DatabaseId"]);
                if (!row.IsNull("FiscalYearStartMonth")) m_FiscalYearStartMonth = new int?((int)row["FiscalYearStartMonth"]);
                if (!row.IsNull("FiscalYearStartDay")) m_FiscalYearStartDay = new int?((int)row["FiscalYearStartDay"]);
                if (!row.IsNull("WeekStartsDay")) m_WeekStartsDay = new int?((int)row["WeekStartsDay"]);
                m_LogoImageResourceIdLoaded = false;
                m_LogoImageResourceId = null;

                if (row.Table.Columns.Contains("LdapServerAddress"))
                {
                    m_LdapServerAddress = (string)row["LdapServerAddress"];
                    m_LdapServerPort = (string)row["LdapServerPort"];
                    m_LdapDomain = (string)row["LdapDomain"];
                    m_LdapUserName = (string)row["LdapUserName"];
                    m_LdapPassword = (string)row["LdapPassword"];
                }

                if (row.Table.Columns.Contains("LdapDomains"))
                    m_LdapDomains = (Convert.IsDBNull(row["LdapDomains"])) ? string.Empty : (string)row["LdapDomains"];

                if (row.Table.Columns.Contains("ExpirationTime"))
                {
                    if (!row.IsNull("ExpirationTime")) m_ExpirationTime = new DateTime?((DateTime)row["ExpirationTime"]);
                    m_GraceDays = (int)row["GraceDays"];
                    m_Active = (bool)row["Active"];

                    if (row.Table.Columns.Contains("CanceledTime"))
                    {
                        if (!row.IsNull("CanceledTime")) m_CanceledTime = new DateTime?((DateTime)row["CanceledTime"]);
                        m_Trial = (bool)row["Trial"];
                    }
                }

                if (row.Table.Columns.Contains("ExternalId"))
                    m_ExternalId = (string)row["ExternalId"];

                if (row.Table.Columns.Contains("Beta"))
                    m_Beta = (bool)row["Beta"];

                if (row.Table.Columns.Contains("CreatedTime"))
                {
                    if (!row.IsNull("CreatedTime")) m_CreatedTime = new DateTime?((DateTime)row["CreatedTime"]);
                }

                m_Deleted = (bool)row["Deleted"];
                m_BillingPlan = (BillingPlan)((byte)row["BillingPlan"]);
                m_Street = (string)row["Street"];
                m_Street2 = (string)row["Street2"];
                m_City = (string)row["City"];
                m_State = (string)row["State"];
                m_PostalCode = (string)row["PostalCode"];
                m_Country = (string)row["Country"];
                m_Currency = (string)row["Currency"];

                this.Reset();
            }
        }

        /// <summary>
        /// Refreshes the the organization level settings and the email settings.
        /// </summary>
        internal void Refresh()
        {
            m_Settings = null;
            m_EmailSettings = null;
        }

        #endregion

        #region Overriden Methods

        public override bool Equals(object obj)
        {
            Organization organization = obj as Organization;
            if ((object)organization == null)
                return false;
            return (this.CompareTo(organization) == 0);
        }

        public override int GetHashCode()
        {
            return this.OrganizationId.GetHashCode();
        }

        /// <summary>
        /// Returns a System.String that represents this object.
        /// </summary>
        /// <returns>A System.String that represents this object.</returns>
        public override string ToString()
        {
            return this.ToString(null);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Compares the current organization with another.
        /// </summary>
        /// <param name="other">An organization to compare with this organization.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the organizations being compared.</returns>
        public int CompareTo(Organization other)
        {
            return (((object)other == null) ? 1 : string.Compare(this.Name, other.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Returns a System.String that represents this object.
        /// </summary>
        /// <param name="delimiter">The string that delimit the properties/values in the string.</param>
        /// <returns>A System.String that represents this object.</returns>
        public string ToString(string delimiter)
        {
            if (string.IsNullOrEmpty(delimiter)) delimiter = Environment.NewLine;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("OrganizationId={0}{1}", OrganizationId, delimiter);
            sb.AppendFormat("PseudoId={0}{1}", PseudoId, delimiter);
            sb.AppendFormat("Name={0}{1}", Name, delimiter);
            sb.AppendFormat("DatabaseId={0}{1}", DatabaseId, delimiter);
            sb.AppendFormat("ExternalId={0}{1}", ExternalId, delimiter);
            sb.AppendFormat("Expired={0}{1}", Expired, delimiter);
            sb.AppendFormat("Active={0}", Active);
            return sb.ToString();
        }

        #endregion
    }

    /// <summary>
    /// The collection of the organizations.
    /// </summary>
    [Serializable]
    public sealed class OrganizationCollection : List<Organization>
    {
        #region Constuctors

        /// <summary>
        /// Initializes a new instance of the class that is empty and has the default initial capacity.
        /// </summary>
        public OrganizationCollection() : base() { }

        /// <summary>
        ///  Initializes a new instance of the class that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new collection.</param>
        public OrganizationCollection(IEnumerable<Organization> collection) : base(collection) { }

        #endregion

        #region Private Methods

        private static int CompareByExpiration(Organization x, Organization y)
        {
            int result = 0;

            if (x == null)
            {
                result = ((y == null) ? 0 : -1);
            }
            else
            {
                if (y == null)
                    result = 1;
                else
                {
                    result = ((x.Expired ? 1 : 0) - (y.Expired ? 1 : 0));
                    if (result == 0) result += ((x.GraceDaysRemaining == 0 ? 1 : 0) - (y.GraceDaysRemaining == 0 ? 1 : 0));
                    if (result == 0) result += string.Compare(x.Name, y.Name, StringComparison.CurrentCultureIgnoreCase);
                }
            }
            return result;
        }

        #endregion

        #region Public Methods

        public void SortByExpiration()
        {
            Sort(CompareByExpiration);
        }

        /// <summary>
        /// Retrieves all visible organizations.
        /// </summary>
        /// <returns>The collection containing visible organizations.</returns>
        public OrganizationCollection FindAllVisible()
        {
            lock (((ICollection)this).SyncRoot)
            {
                return new OrganizationCollection(this.FindAll(
                    delegate(Organization org)
                    {
                        return (org.Visible);
                    }));
            }
        }

        #endregion
    }
}
