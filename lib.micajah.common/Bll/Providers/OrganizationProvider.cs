using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;
using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using Micajah.Common.Security;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with organizations.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class OrganizationProvider
    {
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

        /// <summary>
        /// Returns an object populated with information of the specified organization.
        /// </summary>
        /// <param name="pseudoId">The pseudo unique identifier of the organization.</param>
        /// <returns>The object populated with information of the specified organization. If the organization is not found, the method returns null reference.</returns>
        private static CommonDataSet.OrganizationRow GetOrganizationRowByPseudoId(string pseudoId)
        {
            CommonDataSet.OrganizationDataTable table = WebApplication.CommonDataSet.Organization;
            CommonDataSet.OrganizationRow[] rows = table.Select(string.Concat(table.PseudoIdColumn.ColumnName, " = '", pseudoId, "' AND ", table.DeletedColumn.ColumnName, " = 0")) as CommonDataSet.OrganizationRow[];
            if (rows.Length > 0)
                return rows[0] as CommonDataSet.OrganizationRow;
            return null;
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
        /// <param name="refreshAllData">true to refresh all cached data by application.</param>
        private static Guid InsertOrganization(string name, string description, string websiteUrl, Guid? databaseId
            , int? fiscalYearStartMonth, int? fiscalYearStartDay, int? weekStartsDay
            , DateTime? expirationTime, int graceDays, bool active, DateTime? canceledTime, bool trial
            , string street, string street2, string city, string state, string postalCode, string country, string currency
            , string timeZoneId, Guid? templateInstanceId
            , string adminEmail, string password, string firstName, string lastName, string middleName, string title, string phone, string mobilePhone
            , string partialCustomUrl
            , bool sendNotificationEmail, bool refreshAllData)
        {
            adminEmail = Support.TrimString(adminEmail, UserProvider.EmailMaxLength);
            Support.ValidateEmail(adminEmail);

            if (!string.IsNullOrEmpty(websiteUrl))
                Support.ValidateUrl(websiteUrl);
            else
                websiteUrl = string.Empty;

            CommonDataSet.OrganizationDataTable table = WebApplication.CommonDataSet.Organization;
            CommonDataSet.OrganizationRow row = table.NewOrganizationRow();
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

            try
            {
                table.AddOrganizationRow(row);
            }
            catch (ConstraintException ex)
            {
                throw new ConstraintException(string.Format(CultureInfo.CurrentCulture, Resources.OrganizationProvider_ErrorMessage_OrganizationAlreadyExists, name), ex);
            }

            WebApplication.CommonDataSetTableAdapters.OrganizationTableAdapter.Update(row);

            Organization org = new Organization();
            org.Load(row);

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
                      , false, false
                      , 0, 0, out password);
            }

            InstanceProvider.InsertFirstInstance(timeZoneId, templateInstanceId, organizationId
                , adminEmail, password
                , partialCustomUrl
                , sendNotificationEmail, refreshAllData);

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
            , DateTime? expirationTime, int? graceDays, bool? active, DateTime? canceledTime, bool? trial, bool? beta, string emailSuffixes, string ldapDomains)
        {
            if (!string.IsNullOrEmpty(websiteUrl))
                Support.ValidateUrl(websiteUrl);
            else
                websiteUrl = string.Empty;

            CommonDataSet.OrganizationDataTable table = WebApplication.CommonDataSet.Organization;
            CommonDataSet.OrganizationRow row = table.FindByOrganizationId(organizationId);
            if (row == null) return;

            name = Support.TrimString(name, table.NameColumn.MaxLength);
            description = Support.TrimString(description, table.DescriptionColumn.MaxLength);
            websiteUrl = Support.TrimString(websiteUrl, table.WebsiteUrlColumn.MaxLength);

            try
            {
                row.Name = name;
            }
            catch (ConstraintException ex)
            {
                throw new ConstraintException(string.Format(CultureInfo.CurrentCulture, Resources.OrganizationProvider_ErrorMessage_OrganizationAlreadyExists, name), ex);
            }

            if (!row.Deleted)
            {
                if (string.IsNullOrEmpty(row.PseudoId))
                    row.PseudoId = Support.GeneratePseudoUnique();
            }

            row.Description = description;
            row.WebsiteUrl = websiteUrl;

            if (FrameworkConfiguration.Current.WebApplication.EnableLdap)
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

            WebApplication.CommonDataSetTableAdapters.OrganizationTableAdapter.Update(row);

            Organization org = new Organization();
            org.Load(row);

            if (FrameworkConfiguration.Current.WebApplication.EnableLdap)
            {
                EmailSuffixProvider.DeleteEmailSuffixes(organizationId, null);
                if (!string.IsNullOrEmpty(emailSuffixes))
                {
                    emailSuffixes = emailSuffixes.Replace(" ", string.Empty);
                    EmailSuffixProvider.InsertEmailSuffix(Guid.NewGuid(), organizationId, null, emailSuffixes);

                    org.EmailSuffixes = emailSuffixes;
                }
            }

            UserContext.RefreshCurrent();

            RaiseOrganizationUpdated(org);
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

        /// <summary>
        /// Creates and returns the organizations collection sorted by name from specified data source.
        /// </summary>
        /// <param name="table">A System.Data.DataTable object that represents the data source to create a collection from.</param>
        /// <returns>A Micajah.Common.Bll.OrganizationCollection object that contains the organizations.</returns>
        internal static OrganizationCollection CreateOrganizationCollection(DataTable table)
        {
            OrganizationCollection coll = new OrganizationCollection();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    Organization org = new Organization();
                    org.Load(row);
                    coll.Add(org);
                }
                coll.Sort();
            }
            return coll;
        }

        internal static void UpdateOrganizationsPseudoId()
        {
            CommonDataSet.OrganizationDataTable table = WebApplication.CommonDataSet.Organization;

            foreach (CommonDataSet.OrganizationRow organizationRow in table)
            {
                if (string.IsNullOrEmpty(organizationRow.PseudoId))
                    organizationRow.PseudoId = Support.GeneratePseudoUnique();

                InstanceProvider.UpdateInstancesPseudoId(organizationRow.OrganizationId);
            }

            WebApplication.CommonDataSetTableAdapters.OrganizationTableAdapter.Update(table);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the organizations, excluding marked as deleted.
        /// </summary>
        /// <returns>The System.Data.DataTable that contains organizations.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetOrganizations()
        {
            CommonDataSet.OrganizationDataTable table = WebApplication.CommonDataSet.Organization;
            DataView dv = table.DefaultView;
            dv.RowFilter = string.Format(CultureInfo.InvariantCulture, "{0} = 0", table.DeletedColumn.ColumnName);
            return dv.ToTable();
        }

        /// <summary>
        /// Gets the organizations with additional information, such as the full name of the database.
        /// </summary>
        /// <param name="includeAdditionalInfo">The flag indicating that the additional information is included in result.</param>        
        /// <returns>The System.Data.DataTable that contains organizations.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetOrganizations(bool includeAdditionalInfo, string name, int statusId)
        {
            CommonDataSet.OrganizationDataTable table = WebApplication.CommonDataSet.Organization.Copy() as CommonDataSet.OrganizationDataTable;

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
                            drv["DatabaseServerFullName"] = DatabaseProvider.GetDatabaseServerFullName(databaseId);
                            drv["DatabaseName"] = DatabaseProvider.GetDatabaseName(databaseId);
                        }
                    }
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
            Organization org = new Organization();
            return (org.Load(organizationId) ? org : null);
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
        /// Returns the connection string to organization database.
        /// Generates an System.Data.DataException exception if error occured.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization to get connection string.</param>
        /// <returns>The connection string to organization database.</returns>
        public static string GetConnectionString(Guid organizationId)
        {
            return GetConnectionString(organizationId, true);
        }

        /// <summary>
        /// Returns the connection string to organization database.
        /// Generates an System.Data.DataException exception if error occured.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization to get connection string.</param>
        /// <param name="throwOnError">true to throw an exception if an error occured; false to return an empty string.</param>
        /// <returns>The connection string to organization database.</returns>
        public static string GetConnectionString(Guid organizationId, bool throwOnError)
        {
            string connectionString = string.Empty;
            string errorMessage = string.Empty;

            CommonDataSet ds = WebApplication.CommonDataSet;
            CommonDataSet.OrganizationRow org = ds.Organization.FindByOrganizationId(organizationId);
            if (org != null)
            {
                if (org.IsDatabaseIdNull())
                    connectionString = FrameworkConfiguration.Current.WebApplication.ConnectionString;
                else
                {
                    Guid databaseId = org.DatabaseId;
                    CommonDataSet.DatabaseRow db = ds.Database.FindByDatabaseId(databaseId);
                    if (db != null)
                    {
                        Guid databaseServerId = db.DatabaseServerId;
                        CommonDataSet.DatabaseServerRow server = ds.DatabaseServer.FindByDatabaseServerId(databaseServerId);
                        if (server != null)
                        {
                            Guid websiteId = server.WebsiteId;
                            CommonDataSet.WebsiteRow site = ds.Website.FindByWebsiteId(websiteId);
                            if (site != null)
                                connectionString = DatabaseProvider.CreateConnectionString(db.Name, db.UserName, db.Password, server.Name, server.InstanceName, server.Port);
                            else
                                errorMessage = string.Format(CultureInfo.CurrentCulture, Resources.WebsiteProvider_ErrorMessage_NoWebsite, websiteId);
                        }
                        else
                            errorMessage = string.Format(CultureInfo.CurrentCulture, Resources.DatabaseServerProvider_ErrorMessage_NoDatabaseServer, databaseServerId);
                    }
                    else
                        errorMessage = string.Format(CultureInfo.CurrentCulture, Resources.DatabaseProvider_ErrorMessage_NoDatabase, databaseId);
                }
            }
            else
                errorMessage = string.Format(CultureInfo.CurrentCulture, Resources.OrganizationProvider_ErrorMessage_NoOrganization, organizationId);

            if (throwOnError && (!string.IsNullOrEmpty(errorMessage)))
                throw new DataException(errorMessage);

            return connectionString;
        }

        /// <summary>
        /// Returns the name of the specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization to get name.</param>
        /// <returns>The name of the specified organization.</returns>
        public static string GetName(Guid organizationId)
        {
            string name = string.Empty;
            CommonDataSet.OrganizationRow row = WebApplication.CommonDataSet.Organization.FindByOrganizationId(organizationId);
            if (row != null) name = row.Name;
            return name;
        }

        /// <summary>
        /// Returns the identifier of the specified organization.
        /// </summary>
        /// <param name="name">The name of the organization.</param>
        /// <returns>The identifier of the specified organization.</returns>
        public static Guid GetOrganizationIdByName(string name)
        {
            Guid organizationId = Guid.Empty;
            WebApplication.RefreshCommonData();

            if (!string.IsNullOrEmpty(name))
            {
                CommonDataSet.OrganizationDataTable table = WebApplication.CommonDataSet.Organization;
                CommonDataSet.OrganizationRow[] rows = table.Select(string.Concat(table.NameColumn.ColumnName, " = '", Support.PreserveSingleQuote(name), "' AND ", table.DeletedColumn.ColumnName, " = 0")) as CommonDataSet.OrganizationRow[];
                if (rows.Length > 0) organizationId = rows[0].OrganizationId;
            }
            return organizationId;
        }

        /// <summary>
        /// Returns the organization by specified name.
        /// </summary>
        /// <param name="name">The name of the organization.</param>
        /// <returns>The organization.</returns>
        public static Organization GetOrganizationByName(string name)
        {
            return GetOrganization(GetOrganizationIdByName(name));
        }

        /// <summary>
        /// Returns an Micajah.Common.Bll.Organization object populated with information of the specified organization.
        /// </summary>
        /// <param name="pseudoId">The pseudo unique identifier of the organization.</param>
        /// <returns>The Micajah.Common.Bll.Organization object populated with information of the specified organization. If the organization is not found, the method returns null reference.</returns>
        public static Organization GetOrganizationByPseudoId(string pseudoId)
        {
            Organization org = null;
            if (!string.IsNullOrEmpty(pseudoId))
            {
                CommonDataSet.OrganizationRow row = GetOrganizationRowByPseudoId(pseudoId);
                if (row == null)
                {
                    WebApplication.RefreshCommonData();
                    row = GetOrganizationRowByPseudoId(pseudoId);
                }

                if (row != null)
                {
                    org = new Organization();
                    org.Load(row);
                }
            }
            return org;
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
            Guid databaseId = Guid.Empty;
            WebApplication.RefreshCommonData();

            if (!string.IsNullOrEmpty(name))
            {
                CommonDataSet.OrganizationDataTable table = WebApplication.CommonDataSet.Organization;
                CommonDataSet.OrganizationRow[] rows = table.Select(string.Concat(table.NameColumn.ColumnName, " = '", Support.PreserveSingleQuote(name), "'")) as CommonDataSet.OrganizationRow[];
                if (rows.Length > 0)
                {
                    CommonDataSet.OrganizationRow row = rows[0];
                    if (!row.IsDatabaseIdNull()) databaseId = row.DatabaseId;
                }
            }
            return databaseId;
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
                , null, null, null, null, null, null, null
                , null, null
                , adminEmail, null, null, null, null, null, null, null, null
                , true, true);
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
                , null, null, null, null, null, null, null
                , null, null
                , adminEmail, null, firstName, lastName, middleName, null, null, null, null
                , sendNotificationEmail, true);
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
            , DateTime? expirationTime, int graceDays, bool active, DateTime? canceledTime, bool trial)
        {
            return InsertOrganization(name, description, websiteUrl, databaseId
                , null, null, null
                , expirationTime, graceDays, active, canceledTime, trial
                , null, null, null, null, null, null, null
                , null, null
                , adminEmail, null, null, null, null, null, null, null, null
                , true, true);
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
            , string street, string street2, string city, string state, string postalCode, string country, string currency
            , string timeZoneId, Guid? templateInstanceId
            , string adminEmail, string password, string firstName, string lastName, string title, string phone, string mobilePhone
            , string partialCustomUrl
            , bool sendNotificationEmail)
        {
            return InsertOrganization(name, description, websiteUrl, DatabaseProvider.GetRandomPublicDatabaseId()
                , null, null, null
                , null, 0, true, null, true
                , street, street2, city, state, postalCode, country, currency
                , timeZoneId, templateInstanceId
                , adminEmail, password, firstName, lastName, null, title, phone, mobilePhone
                , partialCustomUrl
                , sendNotificationEmail, false);
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
            UpdateOrganization(organizationId, name, description, websiteUrl, Guid.Empty, fiscalYearStartMonth, fiscalYearStartDay, weekStartsDay, DateTime.MinValue, null, null, null, null, null, null, null);
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
            UpdateOrganization(organizationId, name, description, websiteUrl, Guid.Empty, fiscalYearStartMonth, fiscalYearStartDay, weekStartsDay, DateTime.MinValue, null, null, null, null, beta, emailSuffixes, ldapDomains);
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
            , DateTime? expirationTime, int graceDays, bool active, DateTime? canceledTime, bool trial)
        {
            UpdateOrganization(organizationId, name, description, websiteUrl, databaseId, fiscalYearStartMonth, fiscalYearStartDay, weekStartsDay, expirationTime, new int?(graceDays), new bool?(active), canceledTime, trial, null, null, null);
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
            CommonDataSet.OrganizationDataTable table = WebApplication.CommonDataSet.Organization;
            CommonDataSet.OrganizationRow row = table.FindByOrganizationId(organizationId);
            if (row == null) return;

            row.LdapServerAddress = Support.TrimString(ldapServerAddress, table.LdapServerAddressColumn.MaxLength);
            row.LdapServerPort = Support.TrimString(ldapServerPort, table.LdapServerPortColumn.MaxLength);
            row.LdapDomain = Support.TrimString(ldapDomain, table.LdapDomainColumn.MaxLength);
            row.LdapUserName = Support.TrimString(ldapUserName, table.LdapUserNameColumn.MaxLength);
            if (String.IsNullOrEmpty(ldapUpdatePassword) == false && String.IsNullOrEmpty(ldapConfirmNewPassword) == false && ldapUpdatePassword == ldapConfirmNewPassword)
                row.LdapPassword = Support.TrimString(ldapUpdatePassword, table.LdapPasswordColumn.MaxLength);

            WebApplication.CommonDataSetTableAdapters.OrganizationTableAdapter.Update(row);
            UserContext.RefreshCurrent();

            Organization org = new Organization();
            org.Load(row);

            RaiseOrganizationUpdated(org);
        }

        /// <summary>
        /// Updates the details of specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="LdapDomains">The organization Ldap Domains.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateOrganization(Guid organizationId, string ldapDomains)
        {
            CommonDataSet.OrganizationDataTable table = WebApplication.CommonDataSet.Organization;
            CommonDataSet.OrganizationRow row = table.FindByOrganizationId(organizationId);
            if (row == null) return;

            row.LdapDomains = Support.TrimString(ldapDomains, table.LdapDomainsColumn.MaxLength);

            WebApplication.CommonDataSetTableAdapters.OrganizationTableAdapter.Update(row);
            UserContext.RefreshCurrent();

            Organization org = new Organization();
            org.Load(row);

            RaiseOrganizationUpdated(org);
        }

        /// <summary>
        /// Updates the details of specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="expirationTime">The organization Expiration Time.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateOrganization(Guid organizationId, DateTime expirationTime)
        {
            CommonDataSet.OrganizationDataTable table = WebApplication.CommonDataSet.Organization;
            CommonDataSet.OrganizationRow row = table.FindByOrganizationId(organizationId);

            row.ExpirationTime = expirationTime;

            WebApplication.CommonDataSetTableAdapters.OrganizationTableAdapter.Update(row);
            UserContext.RefreshCurrent();

            Organization org = new Organization();
            org.Load(row);

            RaiseOrganizationUpdated(org);
        }

        /// <summary>
        /// Updates the active flag of the specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="active">true, if the organization is active; otherwise, false.</param>
        public static void UpdateOrganizationActive(Guid organizationId, bool active)
        {
            CommonDataSet.OrganizationDataTable table = WebApplication.CommonDataSet.Organization;
            CommonDataSet.OrganizationRow row = table.FindByOrganizationId(organizationId);
            if (row == null) return;

            row.Active = active;

            WebApplication.CommonDataSetTableAdapters.OrganizationTableAdapter.Update(row);
            UserContext.RefreshCurrent();

            Organization org = new Organization();
            org.Load(row);

            RaiseOrganizationUpdated(org);
        }

        /// <summary>
        /// Sets the deleted flag to true of the specified organization.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>        
        public static void UndeleteOrganization(Guid organizationId)
        {
            CommonDataSet.OrganizationDataTable table = WebApplication.CommonDataSet.Organization;
            CommonDataSet.OrganizationRow row = table.FindByOrganizationId(organizationId);
            if (row == null) return;

            row.Deleted = false;

            WebApplication.CommonDataSetTableAdapters.OrganizationTableAdapter.Update(row);
            UserContext.RefreshCurrent();

            Organization org = new Organization();
            org.Load(row);

            RaiseOrganizationUpdated(org);
        }

        /// <summary>
        /// Marks as deleted the specified organization.
        /// </summary>
        /// <param name="organizationId">Specifies the organization's identifier.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteOrganization(Guid organizationId)
        {
            CommonDataSet.OrganizationRow row = WebApplication.CommonDataSet.Organization.FindByOrganizationId(organizationId);
            if (row == null) return;

            row.Deleted = true;

            WebApplication.CommonDataSetTableAdapters.OrganizationTableAdapter.Update(row);
            WebApplication.RemoveOrganizationDataSetByOrganizationId(organizationId);
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
