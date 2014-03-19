using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using System;
using System.Collections;
using System.Globalization;
using System.Security.Authentication;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;

namespace Micajah.Common.Security
{
    /// <summary>
    /// The class that contains user information.
    /// Represents a collection of key/value pairs that are organized based on the hash code of the key.
    /// </summary>
    [Serializable]
    public sealed class UserContext : SortedList
    {
        #region Members

        private const string BreadcrumbsKey = "mc.Breadcrumbs";
        private const string EmailKey = "mc.Email";
        private const string FirstNameKey = "mc.FirstName";
        private const string GroupIdListKey = "mc.GroupIdList";
        private const string RoleIdKey = "mc.RoleId";
        private const string LoginNameKey = "mc.LoginName";
        private const string LastNameKey = "mc.LastName";
        private const string MiddleNameKey = "mc.MiddleName";
        private const string PhoneKey = "mc.Phone";
        private const string MobilePhoneKey = "mc.MobilePhone";
        private const string TitleKey = "mc.Title";
        private const string DepartmentKey = "mc.Department";
        private const string StreetKey = "mc.Street";
        private const string Street2Key = "mc.Street2";
        private const string CityKey = "mc.City";
        private const string StateKey = "mc.State";
        private const string PostalCodeKey = "mc.PostalCode";
        private const string CountryKey = "mc.Country";
        private const string TimeZoneIdKey = "mc.TimeZoneId";
        private const string TimeFormatKey = "mc.TimeFormat";
        private const string DateFormatKey = "mc.DateFormat";
        private const string RoleIdListKey = "mc.RoleIdList";
        private const string InstanceIdKey = "mc.InstanceId";
        private const string OrganizationIdKey = "mc.OrganizationId";
        private const string StartPageUrlKey = "mc.StartPageUrl";
        private const string UserContextKey = "mc.UserContext";
        private const string UserIdKey = "mc.UserId";
        private const string VanityUrlKey = "mc.VanityUrl";
        private const string OAuthAuthorizationSecretKey = "mc.OAuthAutSecret";

        private static ArrayList s_ReservedKeys;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UserContext()
        {
            base[EmailKey] = string.Empty;
            base[FirstNameKey] = string.Empty;
            base[GroupIdListKey] = new ArrayList();
            base[LastNameKey] = string.Empty;
            base[MiddleNameKey] = string.Empty;
            base[RoleIdListKey] = new ArrayList();
            base[RoleIdKey] = Guid.Empty;
            base[OrganizationIdKey] = Guid.Empty;
            base[InstanceIdKey] = Guid.Empty;
            base[StartPageUrlKey] = string.Empty;
            base[UserIdKey] = Guid.Empty;
            base[LoginNameKey] = string.Empty;
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets a collection of the actions identifiers that the user has access to.
        /// </summary>
        internal ArrayList ActionIdList
        {
            get
            {
                if (this.InstanceId == Guid.Empty || (!FrameworkConfiguration.Current.Actions.EnableOverride))
                    return ActionProvider.GetActionIdList(this.RoleIdList, this.IsOrganizationAdministrator, true);
                else
                    return GroupProvider.GetActionIdList(this.GroupIdList, this.RoleIdList, this.InstanceId, this.OrganizationId, this.IsOrganizationAdministrator);
            }
        }

        /// <summary>
        /// Gets a collection of user's roles identifiers.
        /// </summary>
        internal ArrayList RoleIdList
        {
            get { return (ArrayList)base[RoleIdListKey]; }
        }

        /// <summary>
        /// Gets a collection of user's groups identifiers.
        /// </summary>
        internal ArrayList GroupIdList
        {
            get { return (ArrayList)base[GroupIdListKey]; }
        }

        /// <summary>
        /// Gets a value indicating whether the user has the access to the Log On As Another User feature.
        /// </summary>
        internal bool CanLogOnAsUser
        {
            get { return LoginProvider.CanLogOnAsUser(this.LoginName); }
        }

        /// <summary>
        /// Gets the unique identifier of the user's role.
        /// </summary>
        internal Guid RoleId
        {
            get { return (Guid)base[RoleIdKey]; }
        }

        /// <summary>
        /// Gets the list of the keys which are reserved by Micajah.Common.UserContext class for internal using.
        /// </summary>
        internal static ArrayList ReservedKeys
        {
            get
            {
                if (s_ReservedKeys == null)
                {
                    s_ReservedKeys = new ArrayList();
                    s_ReservedKeys.Add(BreadcrumbsKey);
                    s_ReservedKeys.Add(EmailKey);
                    s_ReservedKeys.Add(FirstNameKey);
                    s_ReservedKeys.Add(GroupIdListKey);
                    s_ReservedKeys.Add(LastNameKey);
                    s_ReservedKeys.Add(MiddleNameKey);
                    s_ReservedKeys.Add(PhoneKey);
                    s_ReservedKeys.Add(MobilePhoneKey);
                    s_ReservedKeys.Add(TitleKey);
                    s_ReservedKeys.Add(DepartmentKey);
                    s_ReservedKeys.Add(StreetKey);
                    s_ReservedKeys.Add(Street2Key);
                    s_ReservedKeys.Add(CityKey);
                    s_ReservedKeys.Add(StateKey);
                    s_ReservedKeys.Add(PostalCodeKey);
                    s_ReservedKeys.Add(CountryKey);
                    s_ReservedKeys.Add(TimeZoneIdKey);
                    s_ReservedKeys.Add(TimeFormatKey);
                    s_ReservedKeys.Add(DateFormatKey);
                    s_ReservedKeys.Add(RoleIdListKey);
                    s_ReservedKeys.Add(RoleIdKey);
                    s_ReservedKeys.Add(InstanceIdKey);
                    s_ReservedKeys.Add(OrganizationIdKey);
                    s_ReservedKeys.Add(StartPageUrlKey);
                    s_ReservedKeys.Add(UserIdKey);
                    s_ReservedKeys.Add(LoginNameKey);
                }
                return s_ReservedKeys;
            }
        }

        internal static string OAuthAuthorizationSecret
        {
            get
            {
                HttpContext http = HttpContext.Current;
                if (http != null)
                {
                    HttpSessionState session = http.Session;
                    if (session != null)
                    {
                        object obj = session[OAuthAuthorizationSecretKey];
                        if (obj != null)
                            return (string)obj;
                    }
                }
                return null;
            }
            set
            {
                HttpContext http = HttpContext.Current;
                if (http != null)
                {
                    HttpSessionState session = http.Session;
                    if (session != null)
                    {
                        if (value == null)
                            session.Remove(OAuthAuthorizationSecretKey);
                        else
                            session[OAuthAuthorizationSecretKey] = value;
                    }
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the instance of UserContext class from Session object for the current user.
        /// </summary>
        public static UserContext Current
        {
            get
            {
                UserContext user = null;

                HttpContext http = HttpContext.Current;
                if (http != null)
                {
                    HttpSessionState session = http.Session;
                    if (session != null)
                    {
                        user = session[UserContextKey] as UserContext;
                        if (user == null)
                        {
                            try
                            {
                                user = CreateFromAuthCookie();
                                session[UserContextKey] = user;
                            }
                            catch (AuthenticationException)
                            {
                                (new LoginProvider()).SignOut();
                            }
                        }
                        else
                        {
                            if ((http.User != null) && (http.User.Identity != null) && http.User.Identity.IsAuthenticated)
                            {
                                if (string.IsNullOrEmpty(http.User.Identity.Name)) // Checks if the user is logged out from another URL, but the session was not cleaned up.
                                {
                                    if (!LoginProvider.IsLoginUrl(http.Request.Url.ToString())) // Checks if current page is not login page.
                                    {
                                        http.Session.Clear();
                                        user = null;
                                    }
                                }
                            }
                        }
                    }
                }

                return user;
            }
            internal set
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx != null)
                {
                    HttpSessionState session = ctx.Session;
                    if (session != null)
                        session[UserContextKey] = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the login name.
        /// </summary>
        public string LoginName
        {
            get
            {
                string str = (string)base[LoginNameKey];
                if (string.IsNullOrEmpty(str))
                    base[LoginNameKey] = str = LoginProvider.Current.GetLoginName(this.UserId);
                return str;
            }
            set { base[LoginNameKey] = value; }
        }

        /// <summary>
        /// Gets or sets the identifier of an user.
        /// </summary>
        public Guid UserId
        {
            get { return (Guid)base[UserIdKey]; }
            set { base[UserIdKey] = value; }
        }

        /// <summary>
        /// Gets or sets the e-mail address of an user. The default value is empty string.
        /// </summary>
        public string Email
        {
            get { return (string)base[EmailKey]; }
            set { base[EmailKey] = value; }
        }

        /// <summary>
        /// Gets or sets the first name of an user. The default value is empty string.
        /// </summary>
        public string FirstName
        {
            get { return (string)base[FirstNameKey]; }
            set { base[FirstNameKey] = value; }
        }

        /// <summary>
        /// Gets or sets the last name of an user. The default value is empty string.
        /// </summary>
        public string LastName
        {
            get { return (string)base[LastNameKey]; }
            set { base[LastNameKey] = value; }
        }

        /// <summary>
        /// Gets or sets the middle name of an user. The default value is empty string.
        /// </summary>
        public string MiddleName
        {
            get { return (string)base[MiddleNameKey]; }
            set { base[MiddleNameKey] = value; }
        }

        /// <summary>
        /// Gets or sets the phone of an user. The default value is empty string.
        /// </summary>
        public string Phone
        {
            get { return (string)base[PhoneKey]; }
            set { base[PhoneKey] = value; }
        }

        /// <summary>
        /// Gets or sets the mobile phone of an user. The default value is empty string.
        /// </summary>
        public string MobilePhone
        {
            get { return (string)base[MobilePhoneKey]; }
            set { base[MobilePhoneKey] = value; }
        }

        /// <summary>
        /// Gets or sets the title of an user. The default value is empty string.
        /// </summary>
        public string Title
        {
            get { return (string)base[TitleKey]; }
            set { base[TitleKey] = value; }
        }

        /// <summary>
        /// Gets or sets the department. The default value is empty string.
        /// </summary>
        public string Department
        {
            get { return (string)base[DepartmentKey]; }
            set { base[DepartmentKey] = value; }
        }

        /// <summary>
        /// Gets or sets the user's street. The default value is empty string.
        /// </summary>
        public string Street
        {
            get { return (string)base[StreetKey]; }
            set { base[StreetKey] = value; }
        }

        /// <summary>
        /// Gets or sets the user's secondary street. The default value is empty string.
        /// </summary>
        public string Street2
        {
            get { return (string)base[Street2Key]; }
            set { base[Street2Key] = value; }
        }

        /// <summary>
        /// Gets or sets the user's city. The default value is empty string.
        /// </summary>
        public string City
        {
            get { return (string)base[CityKey]; }
            set { base[CityKey] = value; }
        }

        /// <summary>
        /// Gets or sets the user's state/province. The default value is empty string.
        /// </summary>
        public string State
        {
            get { return (string)base[StateKey]; }
            set { base[StateKey] = value; }
        }

        /// <summary>
        /// Gets or sets the user's postal code. The default value is empty string.
        /// </summary>
        public string PostalCode
        {
            get { return (string)base[PostalCodeKey]; }
            set { base[PostalCodeKey] = value; }
        }

        /// <summary>
        /// Gets or sets the user's country. The default value is empty string.
        /// </summary>
        public string Country
        {
            get { return (string)base[CountryKey]; }
            set { base[CountryKey] = value; }
        }

        /// <summary>
        /// Gets or sets the time zone identifier.
        /// </summary>
        public string TimeZoneId
        {
            get
            {
                string value = InstanceProvider.DefaultTimeZoneId;
                if (base[TimeZoneIdKey] == null)
                {
                    Instance inst = this.Instance;
                    if (inst != null)
                        value = inst.TimeZoneId;
                }
                else
                    value = (string)base[TimeZoneIdKey];
                return value;
            }
            set { base[TimeZoneIdKey] = value; }
        }

        /// <summary>
        /// Gets or sets the time format.
        /// </summary>
        public int TimeFormat
        {
            get
            {
                int value = 0;
                if (base[TimeFormatKey] == null)
                {
                    Instance inst = this.Instance;
                    if (inst != null)
                        value = inst.TimeFormat;
                }
                else
                    value = (int)base[TimeFormatKey];
                return value;
            }
            set { base[TimeFormatKey] = value; }
        }

        /// <summary>
        /// Gets or sets the date format.
        /// </summary>
        public int DateFormat
        {
            get
            {
                int value = 0;
                if (base[DateFormatKey] == null)
                {
                    Instance inst = this.Instance;
                    if (inst != null)
                        value = inst.DateFormat;
                }
                else
                    value = (int)base[DateFormatKey];
                return value;
            }
            set { base[DateFormatKey] = value; }
        }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        public TimeZoneInfo TimeZone
        {
            get { return TimeZoneInfo.FindSystemTimeZoneById(this.TimeZoneId); }
        }

        /// <summary>
        /// Gets a value indicating whether the user is administrator of the framework.
        /// </summary>
        public bool IsFrameworkAdministrator
        {
            get { return LoginProvider.IsFrameworkAdministrator(this.LoginName); }
        }

        /// <summary>
        /// Gets the value indicating that the user is administrator of selected organization.
        /// </summary>
        public bool IsOrganizationAdministrator
        {
            get { return RoleIdList.Contains(RoleProvider.OrganizationAdministratorRoleId); }
        }

        /// <summary>
        /// Gets the selected organization.
        /// </summary>
        public Organization Organization
        {
            get { return ((this.OrganizationId == Guid.Empty) ? null : OrganizationProvider.GetOrganizationFromCache(this.OrganizationId, true)); }
        }

        /// <summary>
        /// Gets the unique identifier of the selected organization.
        /// </summary>
        public Guid OrganizationId
        {
            get { return (Guid)base[OrganizationIdKey]; }
        }

        /// <summary>
        /// Gets the selected instance.
        /// </summary>
        public Instance Instance
        {
            get
            {
                if ((this.OrganizationId != Guid.Empty) && (this.InstanceId != Guid.Empty))
                    return InstanceProvider.GetInstanceFromCache(this.InstanceId, this.OrganizationId, true);
                return null;
            }
        }

        /// <summary>
        /// Gets the unique identifier of the selected instance.
        /// </summary>
        public Guid InstanceId
        {
            get { return (Guid)base[InstanceIdKey]; }
        }

        /// <summary>
        /// Gets a URL of start page based on user's groups roles, taking the role's rank into account.
        /// </summary>
        public string StartPageUrl
        {
            get { return (string)base[StartPageUrlKey]; }
        }

        /// <summary>
        /// Gets the collection of the bread crumbs.
        /// </summary>
        public static BreadcrumbCollection Breadcrumbs
        {
            get
            {
                BreadcrumbCollection breadcrumbs = new BreadcrumbCollection();
                HttpContext ctx = HttpContext.Current;
                if (ctx != null)
                {
                    HttpSessionState session = ctx.Session;
                    if (session != null)
                    {
                        breadcrumbs = (session[BreadcrumbsKey] as BreadcrumbCollection);
                        if (breadcrumbs == null)
                        {
                            breadcrumbs = new BreadcrumbCollection();
                            session[BreadcrumbsKey] = breadcrumbs;
                        }
                    }
                }
                return breadcrumbs;
            }
        }

        /// <summary>
        /// Gets the collection of the settings.
        /// </summary>
        public SettingCollection Settings
        {
            get
            {
                SettingCollection settings = null;

                Guid organizationId = this.OrganizationId;
                Guid instanceId = this.InstanceId;

                if (instanceId != Guid.Empty)
                {
                    settings = SettingProvider.GetSettings(SettingLevels.Organization | SettingLevels.Instance | SettingLevels.Group, null);
                    SettingProvider.FillSettingsByGroupValues(ref settings, organizationId, instanceId, this.GroupIdList);
                }
                else if (organizationId != Guid.Empty)
                {
                    settings = SettingProvider.GetSettings(SettingLevels.Organization, null);
                    SettingProvider.FillSettingsByOrganizationValues(ref settings, organizationId);
                }
                else
                    settings = new SettingCollection();
                return settings;
            }
        }

        /// <summary>
        /// Gets or sets the user's vanity url. The default value is empty string.
        /// </summary>
        public static string VanityUrl
        {
            get
            {
                HttpContext http = HttpContext.Current;
                if (http != null)
                {
                    HttpSessionState session = http.Session;
                    if (session != null)
                    {
                        object obj = session[VanityUrlKey];
                        if (obj != null)
                            return (string)obj;
                    }
                }
                return string.Empty;
            }
            internal set
            {
                HttpContext http = HttpContext.Current;
                if (http != null)
                {
                    HttpSessionState session = http.Session;
                    if (session != null)
                    {
                        if (string.IsNullOrEmpty(value))
                            session.Remove(VanityUrlKey);
                        else
                            session[VanityUrlKey] = value;
                    }
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raises when the selected instance is changing.
        /// </summary>
        public static event EventHandler<UserContextInstanceChangingEventArgs> InstanceChanging;

        /// <summary>
        /// Raises when the selected instance is changed.
        /// </summary>
        public static event EventHandler InstanceChanged;

        /// <summary>
        /// Raises when the selected organization is changing.
        /// </summary>
        public static event EventHandler<UserContextOrganizationChangingEventArgs> OrganizationChanging;

        /// <summary>
        /// Raises when the selected organization is changed.
        /// </summary>
        public static event EventHandler OrganizationChanged;

        #endregion

        #region Private Methods

        private void SelectedInstanceChange(Instance newInstance, bool setAuthCookie, bool? isPersistent, bool putToCache)
        {
            ArrayList groupIdList = new ArrayList();
            ArrayList roleIdList = new ArrayList();
            string startUrl = null;

            UserProvider.GetUserGroupsAndRoles(newInstance.OrganizationId, newInstance.InstanceId, this.UserId, ref groupIdList, ref roleIdList);

            Guid roleId = RoleProvider.AssumeRole(this.IsOrganizationAdministrator, ref roleIdList, ref startUrl);

            if (roleIdList.Count == 0)
                throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.UserContext_ErrorMessage_NoGroupsInstanceRoles, newInstance.Name));

            if (putToCache)
                InstanceProvider.PutInstanceToCache(newInstance);

            if (InstanceChanging != null)
            {
                if (string.IsNullOrEmpty(this.Email))
                {
                    ClientDataSet.UserRow userRow = UserProvider.GetUserRow(this.UserId, newInstance.OrganizationId);
                    if (userRow != null)
                        RefreshDetails(this, userRow);
                }
                InstanceChanging(this, new UserContextInstanceChangingEventArgs() { Instance = newInstance });
            }

            base[GroupIdListKey] = groupIdList;
            base[RoleIdListKey] = roleIdList;
            base[RoleIdKey] = roleId;
            base[StartPageUrlKey] = CustomUrlProvider.CreateApplicationAbsoluteUrl(startUrl);
            base[InstanceIdKey] = newInstance.InstanceId;

            if (FrameworkConfiguration.Current.WebApplication.AuthenticationMode == AuthenticationMode.Forms)
            {
                if (setAuthCookie)
                    LoginProvider.SetAuthCookie(this.UserId, newInstance.OrganizationId, newInstance.InstanceId, isPersistent);
            }

            if (InstanceChanged != null)
                InstanceChanged(this, EventArgs.Empty);
        }

        private void SelectedOrganizationChange(Organization newOrganization, bool isOrgAdmin, ArrayList userGroupIdList, bool setAuthCookie, bool? isPersistent, Guid? instanceId, bool putToCache)
        {
            if (this.OrganizationId != newOrganization.OrganizationId)
                CheckWebSite(newOrganization.OrganizationId, instanceId);

            Guid currentInstanceId = instanceId.GetValueOrDefault();

            Breadcrumbs.Clear();

            base[InstanceIdKey] = Guid.Empty;

            ArrayList groupIdList = new ArrayList();
            ArrayList roleIdList = new ArrayList();
            Guid roleId = Guid.Empty;
            string startUrl = null;

            if (userGroupIdList.Count > 0)
            {
                // Looks up roles in all instances.
                foreach (ClientDataSet.GroupsInstancesRolesRow row in GroupProvider.GetGroupsInstancesRolesByGroups(newOrganization.OrganizationId, userGroupIdList))
                {
                    if (!groupIdList.Contains(row.GroupId))
                        groupIdList.Add(row.GroupId);

                    if (!roleIdList.Contains(row.RoleId))
                    {
                        if (RoleProvider.GetRoleRow(row.RoleId) != null) // Validates if this role still defined in the configuration file.
                            roleIdList.Add(row.RoleId);
                    }
                }
            }

            roleId = RoleProvider.AssumeRole(isOrgAdmin, ref roleIdList, ref startUrl);

            if (roleIdList.Count == 0)
                throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.UserContext_ErrorMessage_NoGroupsInstancesRoles, newOrganization.Name));

            if (putToCache)
                OrganizationProvider.PutOrganizationToCache(newOrganization);

            if (OrganizationChanging != null)
                OrganizationChanging(this, new UserContextOrganizationChangingEventArgs() { Organization = newOrganization });

            base[TimeZoneIdKey] = null;
            base[TimeFormatKey] = null;
            base[DateFormatKey] = null;
            base[OrganizationIdKey] = newOrganization.OrganizationId;
            base[GroupIdListKey] = groupIdList;
            base[RoleIdListKey] = roleIdList;
            base[RoleIdKey] = roleId;
            base[StartPageUrlKey] = CustomUrlProvider.CreateApplicationAbsoluteUrl(startUrl);

            if (FrameworkConfiguration.Current.WebApplication.AuthenticationMode == AuthenticationMode.Forms)
            {
                if (setAuthCookie)
                    LoginProvider.SetAuthCookie(this.UserId, newOrganization.OrganizationId, Guid.Empty, isPersistent);
            }

            if (currentInstanceId != Guid.Empty)
                this.SelectInstance(currentInstanceId, isPersistent);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Creates a new instance of the UserContext class that is initialized from the form authentification ticket.
        /// </summary>
        /// <returns>Returns a new instance of the UserContext class, or null reference, if the ticket is not valid.</returns>
        internal static UserContext CreateFromAuthCookie()
        {
            Guid userId = Guid.Empty;
            Guid organizationId = Guid.Empty;
            Guid instanceId = Guid.Empty;

            LoginProvider.ParseUserIdentityName(out userId, out organizationId, out instanceId);

            if (userId != Guid.Empty)
            {
                UserContext user = new UserContext();
                user.Initialize(userId, organizationId, instanceId, false, null);

                return user;
            }

            return null;
        }

        internal void CheckWebSite(Guid organizationId, Guid? instanceId)
        {
            string returnUrl = null;
            HttpContext http = HttpContext.Current;
            if (http != null)
            {
                if (http.Request != null)
                {
                    returnUrl = http.Request.Url.PathAndQuery;
                }
            }

            if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
            {
                if (!CustomUrlProvider.IsDefaultVanityUrl(http))
                {
                    return;
                }
            }

            Guid webSiteId = OrganizationProvider.GetWebsiteIdFromCache(organizationId);
            Guid webSiteIdByUrl = WebsiteProvider.GetWebsiteIdByUrlFromCache(WebApplication.Hosts);
            if (webSiteIdByUrl != webSiteId)
            {
                if (GoogleProvider.IsGoogleProviderRequest(returnUrl))
                {
                    returnUrl = null;
                }

                string url = LoginProvider.Current.GetLoginUrl(this.UserId, organizationId, instanceId.GetValueOrDefault(), returnUrl);

                LoginProvider loginProvider = new LoginProvider();
                loginProvider.SignOut(false, url);
            }
        }

        internal void RefreshDetails(ClientDataSet.UserRow userRow)
        {
            RefreshDetails(this, userRow);
        }

        internal static void RefreshDetails(UserContext user, ClientDataSet.UserRow userRow)
        {
            if ((user != null) && (userRow != null))
            {
                user.Email = userRow.Email;
                user.FirstName = userRow.FirstName;
                user.LastName = userRow.LastName;
                user.MiddleName = userRow.MiddleName;
                user.Phone = userRow.Phone;
                user.MobilePhone = userRow.MobilePhone;
                user.Title = userRow.Title;
                user.Department = userRow.Department;
                user.Street = userRow.Street;
                user.Street2 = userRow.Street2;
                user.City = userRow.City;
                user.State = userRow.State;
                user.PostalCode = userRow.PostalCode;
                user.Country = userRow.Country;
                if (userRow.IsTimeZoneIdNull())
                    ((SortedList)user)[TimeZoneIdKey] = null;
                else
                {
                    if (string.IsNullOrEmpty(userRow.TimeZoneId))
                        ((SortedList)user)[TimeZoneIdKey] = null;
                    else
                        user.TimeZoneId = userRow.TimeZoneId;
                }
                if (userRow.IsTimeFormatNull())
                    ((SortedList)user)[TimeFormatKey] = null;
                else
                    user.TimeFormat = userRow.TimeFormat;
                if (userRow.IsDateFormatNull())
                    ((SortedList)user)[DateFormatKey] = null;
                else
                    user.DateFormat = userRow.DateFormat;
            }
        }

        internal void SelectOrganization(Guid organizationId, bool setAuthCookie, bool? isPersistent, Guid? instanceId)
        {
            if (organizationId == Guid.Empty)
                return;

            bool putToCache = false;
            Organization newOrganization = OrganizationProvider.GetOrganizationFromCache(organizationId);
            if (newOrganization == null)
            {
                newOrganization = OrganizationProvider.GetOrganization(organizationId);
                if (newOrganization == null)
                    throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.OrganizationProvider_ErrorMessage_NoOrganization, organizationId));
                putToCache = true;
            }

            if (newOrganization.Deleted)
                throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.OrganizationProvider_ErrorMessage_OrganizationIsDeleted, newOrganization.Name));
            else if (!newOrganization.Active)
                throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.OrganizationProvider_ErrorMessage_OrganizationIsInactive, newOrganization.Name));
            else if (newOrganization.Expired && (newOrganization.GraceDaysRemaining == 0))
                throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.OrganizationProvider_ErrorMessage_OrganizationHasExpired, newOrganization.Name));

            Guid userId = this.UserId;
            bool active = false;
            bool isOrganizationAdministrator = false;
            bool loginInOrganization = LoginProvider.Current.LoginInOrganization(userId, organizationId, out active, out isOrganizationAdministrator);

            if (!(loginInOrganization && active))
            {
                throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.UserContext_ErrorMessage_YourAccountInOrganizationIsInactivated
                    , newOrganization.Name, CustomUrlProvider.CreateApplicationUri(LoginProvider.Current.GetLoginUrl(false))));
            }

            ArrayList userGroupIdList = null;
            ClientDataSet.UserRow userRow = UserProvider.GetUserRow(userId, organizationId);
            if (userRow != null)
            {
                userGroupIdList = UserProvider.GetUserGroupIdList(organizationId, userId, isOrganizationAdministrator);

                if (!isOrganizationAdministrator)
                {
                    if (userGroupIdList.Count == 0)
                        throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.UserContext_ErrorMessage_NoGroups, newOrganization.Name));
                    else if (InstanceProvider.GetInstances(newOrganization.OrganizationId, false).Count == 0)
                        throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.InstanceProvider_ErrorMessage_NoActiveInstances, newOrganization.Name));
                }
            }
            else
                throw new AuthenticationException(FrameworkConfiguration.Current.WebApplication.Login.FailureText);

            this.SelectedOrganizationChange(newOrganization, isOrganizationAdministrator, userGroupIdList, setAuthCookie, isPersistent, instanceId, putToCache);

            RefreshDetails(this, userRow);

            UserProvider.UpdateLastLoginDate(userRow, newOrganization.OrganizationId);

            if (OrganizationChanged != null)
                OrganizationChanged(this, EventArgs.Empty);
        }

        internal void SelectInstance(Guid instanceId, bool setAuthCookie, bool? isPersistent)
        {
            if (instanceId == Guid.Empty)
                return;

            Guid organizationId = this.OrganizationId;
            if (organizationId != Guid.Empty)
            {
                bool putToCache = false;
                Instance instance = InstanceProvider.GetInstanceFromCache(instanceId, organizationId);

                if (instance == null)
                {
                    instance = InstanceProvider.GetInstance(instanceId, organizationId);
                    if (instance == null)
                        throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.InstanceProvider_ErrorMessage_NoInstance, instanceId, this.Organization.Name));

                    putToCache = true;
                }

                if (!instance.Active)
                    throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.InstanceProvider_ErrorMessage_InstanceIsInactive, instance.Name));
                else if (GroupProvider.GetGroupIdRoleIdList(organizationId, instanceId).Count == 0)
                    throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.UserContext_ErrorMessage_NoGroupsInstanceRoles, instance.Name));
                else
                {
                    if (UserProvider.UserIsActiveInInstance(this.UserId, instanceId, organizationId))
                        this.SelectedInstanceChange(instance, setAuthCookie, isPersistent, putToCache);
                    else
                        throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.UserContext_ErrorMessage_YourAccountInInstanceIsInactivated, instance.Name));
                }
            }
            else
                throw new AuthenticationException(Resources.UserContext_ErrorMessage_NoSelectedOrganization);
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Adds an element with the specified key and value into the UserContext if the key isn't reserved.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null.</param>
        public new void Add(object key, object value)
        {
            if (!KeyIsReserved(key)) base.Add(key, value);
        }

        /// <summary>
        /// Removes all elements from the UserContext except the values of the reserved elements (like as UserId, Email, etc.).
        /// </summary>
        public new void Clear()
        {
            SortedList backup = new SortedList();
            foreach (string key in ReservedKeys)
            {
                backup.Add(key, base[key]);
            }
            base.Clear();
            foreach (string key in ReservedKeys)
            {
                base[key] = backup[key];
            }
        }

        /// <summary>
        /// Removes the element with the specified key from the UserContext if the key isn't reserved.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        public new void Remove(object key)
        {
            if (!KeyIsReserved(key)) base.Remove(key);
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key if the key isn't reserved.
        /// </summary>
        /// <param name="key">The key whose value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        public new object this[object key]
        {
            get { return base[key]; }
            set { if (!KeyIsReserved(key)) base[key] = value; }
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

        public void Initialize(Guid userId, Guid organizationId, Guid instanceId, bool setAuthenticationCookie, bool? isPersistent)
        {
            this.UserId = userId;

            if (FrameworkConfiguration.Current.WebApplication.AuthenticationMode == AuthenticationMode.Forms)
            {
                if (setAuthenticationCookie)
                {
                    if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                        LoginProvider.SetAuthCookie(userId, organizationId, instanceId, isPersistent);
                    else
                        LoginProvider.SetAuthCookie(userId, Guid.Empty, Guid.Empty, isPersistent);
                }
            }

            if (organizationId != Guid.Empty)
            {
                this.SelectOrganization(organizationId, setAuthenticationCookie, isPersistent, instanceId);
                this.SelectInstance(instanceId, setAuthenticationCookie, isPersistent);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified key is reserved by Micajah.Common.UserContext class for internal using.
        /// </summary>
        /// <param name="key">The key of the element to check.</param>
        /// <returns>true, if the specified key is reserved; otherwise, false.</returns>
        public static bool KeyIsReserved(object key)
        {
            return (ReservedKeys.Contains(key));
        }

        /// <summary>
        /// Refreshes the cached data for this user.
        /// </summary>
        public void Refresh()
        {
            ClientDataSet.UserRow userRow = UserProvider.GetUserRow(this.UserId, this.OrganizationId);
            if (userRow != null)
                RefreshDetails(this, userRow);
        }

        /// <summary>
        /// Sets the specified value to the OrganizationId element and gets related data.
        /// If an error occured during getting data process, the value isn't set and the Error property contains the error.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        public void SelectOrganization(Guid organizationId)
        {
            SelectOrganization(organizationId, null);
        }

        /// <summary>
        /// Sets the specified value to the OrganizationId element and gets related data.
        /// If an error occured during getting data process, the value isn't set and the Error property contains the error.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        public void SelectOrganization(Guid organizationId, bool? isPersistent)
        {
            SelectOrganization(organizationId, true, isPersistent, null);
        }

        /// <summary>
        /// Sets the specified value to the InstanceId element and gets related data.
        /// If an error occured during getting data process, the value isn't set and the Error property contains the error.
        /// </summary>
        /// <param name="instanceId">The identifier of the instance.</param>
        public void SelectInstance(Guid instanceId)
        {
            SelectInstance(instanceId, null);
        }

        /// <summary>
        /// Sets the specified value to the InstanceId element and gets related data.
        /// If an error occured during getting data process, the value isn't set and the Error property contains the error.
        /// </summary>
        /// <param name="instanceId">The identifier of the instance.</param>
        public void SelectInstance(Guid instanceId, bool? isPersistent)
        {
            SelectInstance(instanceId, true, isPersistent);
        }

        /// <summary>
        /// Determines whether the current user have either of the specified roles in selected organization.
        /// </summary>
        /// <param name="shortName">An array containing the short names of the roles.</param>
        /// <returns>true if the current user is a member of the specified roles; otherwise, false.</returns>
        public bool IsInRole(params string[] shortName)
        {
            bool exists = false;
            IList list = RoleIdList;
            foreach (Guid roleId in RoleProvider.GetRoleIdListByShortNames(shortName))
            {
                if (list.Contains(roleId))
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }

        /// <summary>
        /// Determines whether the current user have either of the specified roles in specified instance of the selected organization.
        /// </summary>
        /// <param name="instanceId">The identifier of the instance in the selected organization.</param>
        /// <param name="shortName">An array containing the short names of the roles.</param>
        /// <returns>true if the current user is a member of the specified roles in specified instance of the selected organization; otherwise, false.</returns>
        public bool IsInRole(Guid instanceId, params string[] shortName)
        {
            if (this.OrganizationId != Guid.Empty)
            {
                ArrayList roleIdList = RoleProvider.GetRoleIdListByShortNames(shortName);

                return GroupProvider.GroupsInstanceHasRole(this.OrganizationId, this.GroupIdList, instanceId, roleIdList);
            }
            return false;
        }

        /// <summary>
        /// Returns the value indicating that the user is administrator of any instance in selected organization.
        /// </summary>
        /// <returns>true, if the user is administrator of any instance in selected organization; otherwise, false.</returns>
        public bool IsInstanceAdministrator()
        {
            return RoleIdList.Contains(RoleProvider.InstanceAdministratorRoleId);
        }

        /// <summary>
        /// Returns the value indicating that the user is administrator of specified instance in selected organization.
        /// </summary>
        /// <param name="instanceId">The unique identifier of the instance.</param>
        /// <returns>true, if the user is administrator of specified instance in selected organization; otherwise, false.</returns>
        public bool IsInstanceAdministrator(Guid instanceId)
        {
            Guid organizationId = this.OrganizationId;
            if (organizationId != Guid.Empty)
                return UserProvider.UserIsInstanceAdministrator(organizationId, instanceId, this.UserId);
            return false;
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
            foreach (string key in this.Keys)
            {
                string stringValue = null;
                object value = this[key];

                ArrayList list = value as ArrayList;
                if (list == null)
                    stringValue = ((value == null) ? "null" : value.ToString());
                else
                {
                    StringBuilder sb1 = new StringBuilder();
                    foreach (object item in list)
                    {
                        sb1.AppendFormat(",{0}", item);
                    }
                    if (sb1.Length > 0) sb1.Remove(0, 1);

                    stringValue = "[" + sb1.ToString() + "]";
                }

                sb.AppendFormat("{0}{1}={2}", delimiter, key.Replace("mc.", string.Empty), stringValue);
            }
            if (sb.Length > 0) sb.Remove(0, delimiter.Length);

            return sb.ToString();
        }

        #endregion
    }

    public class UserContextInstanceChangingEventArgs : EventArgs
    {
        #region Public Properties

        public Instance Instance { get; set; }

        #endregion
    }

    public class UserContextOrganizationChangingEventArgs : EventArgs
    {
        #region Public Properties

        public Organization Organization { get; set; }

        #endregion
    }
}
