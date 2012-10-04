using System;
using System.Collections;
using System.Globalization;
using System.Security.Authentication;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Properties;

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

        private const string ActionIdListKey = "mc.ActionIdList";
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
        private const string RoleIdListKey = "mc.RoleIdList";
        private const string SelectedInstanceIdKey = "mc.SelectedInstanceId";
        private const string SelectedOrganizationIdKey = "mc.SelectedOrganizationId";
        private const string StartPageUrlKey = "mc.StartPageUrl";
        private const string UserContextKey = "mc.UserContext";
        private const string UserIdKey = "mc.UserId";
        private const string VanityUrlKey = "mc.VanityUrl";

        private static ArrayList s_ReservedKeys;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UserContext()
        {
            base[ActionIdListKey] = new ArrayList();
            base[EmailKey] = string.Empty;
            base[FirstNameKey] = string.Empty;
            base[GroupIdListKey] = new ArrayList();
            base[LastNameKey] = string.Empty;
            base[MiddleNameKey] = string.Empty;
            base[RoleIdListKey] = new ArrayList();
            base[RoleIdKey] = Guid.Empty;
            base[SelectedOrganizationIdKey] = Guid.Empty;
            base[SelectedInstanceIdKey] = Guid.Empty;
            base[StartPageUrlKey] = string.Empty;
            base[UserIdKey] = Guid.Empty;
            base[LoginNameKey] = string.Empty;
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets a collection of the actions identifiers that the user's groups roles are associated with.
        /// </summary>
        internal ArrayList ActionIdList
        {
            get { return (ArrayList)base[ActionIdListKey]; }
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
        /// Gets a value indicating whether the user is administrator of the framework.
        /// </summary>
        internal bool IsFrameworkAdministrator
        {
            get { return (((Guid)base[SelectedOrganizationIdKey] == Guid.Empty) && LoginProvider.IsFrameworkAdministrator(this.LoginName)); }
        }

        /// <summary>
        /// Gets a value indicating whether the user has the access to the Log On As Another User feature.
        /// </summary>
        internal bool CanLogOnAsUser
        {
            get { return (((Guid)base[SelectedOrganizationIdKey] == Guid.Empty) && LoginProvider.CanLogOnAsUser(this.LoginName)); }
        }

        /// <summary>
        /// Gets the unique identifier of the user's role.
        /// </summary>
        internal Guid RoleId
        {
            get { return (Guid)base[RoleIdKey]; }
        }

        /// <summary>
        /// Gets the list of the keys which are reserved by Micajah.Common.Security.UserContext class for internal using.
        /// </summary>
        internal static ArrayList ReservedKeys
        {
            get
            {
                if (s_ReservedKeys == null)
                {
                    s_ReservedKeys = new ArrayList();
                    s_ReservedKeys.Add(ActionIdListKey);
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
                    s_ReservedKeys.Add(RoleIdListKey);
                    s_ReservedKeys.Add(RoleIdKey);
                    s_ReservedKeys.Add(SelectedInstanceIdKey);
                    s_ReservedKeys.Add(SelectedOrganizationIdKey);
                    s_ReservedKeys.Add(StartPageUrlKey);
                    s_ReservedKeys.Add(UserIdKey);
                    s_ReservedKeys.Add(LoginNameKey);
                }
                return s_ReservedKeys;
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

                HttpContext ctx = HttpContext.Current;
                if (ctx != null)
                {
                    HttpSessionState session = ctx.Session;
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
                    base[LoginNameKey] = str = WebApplication.LoginProvider.GetLoginName(this.UserId);
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
        /// Gets the time zone identifier.
        /// </summary>
        public string TimeZoneId
        {
            get
            {
                string value = "Eastern Standard Time";
                if (base[TimeZoneIdKey] == null)
                {
                    Instance inst = this.SelectedInstance;
                    if (inst != null)
                    {
                        if (!string.IsNullOrEmpty(inst.TimeZoneId))
                            base[TimeZoneIdKey] = value = inst.TimeZoneId;
                    }
                }
                else
                    value = (string)base[TimeZoneIdKey];
                return value;
            }
        }

        /// <summary>
        /// Gets the time format.
        /// </summary>
        public int TimeFormat
        {
            get
            {
                int value = 0;
                if (base[TimeFormatKey] == null)
                {
                    Instance inst = this.SelectedInstance;
                    if (inst != null)
                        base[TimeFormatKey] = value = inst.TimeFormat;
                }
                else
                    value = (int)base[TimeFormatKey];
                return value;
            }
        }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        public TimeZoneInfo TimeZone
        {
            get { return TimeZoneInfo.FindSystemTimeZoneById(this.TimeZoneId); }
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
        public Organization SelectedOrganization
        {
            get
            {
                Organization org = null;
                Guid organizationId = Guid.Empty;
                
                if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                    FillSelectedOrganizationIdFromSession(ref organizationId);
                
                if (organizationId == Guid.Empty)
                    organizationId = (Guid)base[SelectedOrganizationIdKey];

                if (organizationId != Guid.Empty)
                    org = OrganizationProvider.GetOrganization(organizationId);
                return org;
            }
        }

        /// <summary>
        /// Gets the unique identifier of the selected organization.
        /// </summary>
        public static Guid SelectedOrganizationId
        {
            get
            {
                Guid organizationId = Guid.Empty;
                UserContext user = Current;
                if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                    FillSelectedOrganizationIdFromSession(ref organizationId);

                if (organizationId == Guid.Empty)
                {
                    if (user != null)
                        organizationId = (Guid)user[SelectedOrganizationIdKey];
                    else
                        FillSelectedOrganizationIdFromSession(ref organizationId);
                }
                return organizationId;
            }
            internal set
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx != null)
                {
                    HttpSessionState session = ctx.Session;
                    if (session != null)
                    {
                        if (value == Guid.Empty)
                            session.Remove(SelectedOrganizationIdKey);
                        else
                            session[SelectedOrganizationIdKey] = value;
                    }
                }
            }
        }
        
        /// <summary>
        /// Fills SelectedOrganizationId from session
        /// </summary>        
        private static void FillSelectedOrganizationIdFromSession(ref Guid organizationId)
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx != null)
            {
                HttpSessionState session = ctx.Session;
                if (session != null)
                {
                    object obj = session[SelectedOrganizationIdKey];
                    if (obj != null)
                        organizationId = (Guid)obj;
                    else
                        UserContext.InitializeOrganizationOrInstanceFromCustomUrl();
                }
            }
        }

        /// <summary>
        /// Gets the selected instance.
        /// </summary>
        public Instance SelectedInstance
        {
            get
            {
                Instance inst = null;
                Guid organizationId = Guid.Empty;
                Guid instanceId = Guid.Empty;
                bool isCorrectInstance = false;
                if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                {
                    FillSelectedOrganizationIdFromSession(ref organizationId);
                    FillSelectedInstanceIdFromSession(ref instanceId);
                }

                if ((organizationId == Guid.Empty) || (instanceId == Guid.Empty))
                {
                    organizationId = (Guid)base[SelectedOrganizationIdKey];
                    instanceId = (Guid)base[SelectedInstanceIdKey];
                    
                    if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                    {
                        if (SelectedOrganization != null && SelectedOrganization.Instances != null)
                        {
                            foreach (Instance instance in SelectedOrganization.Instances)
                            {
                                if (instanceId == instance.InstanceId)
                                    isCorrectInstance = true;
                            }
                        }
                        if (!isCorrectInstance)
                            instanceId = Guid.Empty;
                    }
                }

                if ((organizationId != Guid.Empty) && (instanceId != Guid.Empty))
                    inst = InstanceProvider.GetInstance(instanceId, organizationId);
                return inst;
            }
        }

        /// <summary>
        /// Gets or sets the user's vanity url. The default value is empty string.
        /// </summary>
        public static string VanityUrl
        {
            get
            {
                string vanityUrl = string.Empty;
                UserContext user = Current;
                if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                    FillVanityUrlFromSession(ref vanityUrl);

                if (string.IsNullOrEmpty(vanityUrl))
                {
                    if (user != null)
                        vanityUrl = (string)user[VanityUrlKey];
                    else
                        FillVanityUrlFromSession(ref vanityUrl);
                }
                return vanityUrl;
            }
            internal set
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx != null)
                {
                    HttpSessionState session = ctx.Session;
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

        /// <summary>
        /// Fills VanityUrl from session
        /// </summary>        
        private static void FillVanityUrlFromSession(ref string vanityUrl)
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx != null)
            {
                HttpSessionState session = ctx.Session;
                if (session != null)
                {
                    object obj = session[VanityUrlKey];
                    if (obj != null)
                        vanityUrl = (string)obj;
                }
            }
        }


        /// <summary>
        /// Gets the unique identifier of the selected instance.
        /// </summary>
        public static Guid SelectedInstanceId
        {
            get
            {
                Guid instanceId = Guid.Empty;
                UserContext user = Current;
                if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                    FillSelectedInstanceIdFromSession(ref instanceId);

                if (instanceId == Guid.Empty)
                {
                    if (user != null)
                        instanceId = (Guid)user[SelectedInstanceIdKey];
                    else
                        FillSelectedInstanceIdFromSession(ref instanceId);
                }
                return instanceId;
            }
            internal set
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx != null)
                {
                    HttpSessionState session = ctx.Session;
                    if (session != null)
                    {
                        if (value == Guid.Empty)
                            session.Remove(SelectedInstanceIdKey);
                        else
                            session[SelectedInstanceIdKey] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Fills SelectedInstanceId from session
        /// </summary>        
        private static void FillSelectedInstanceIdFromSession(ref Guid instanceId)
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx != null)
            {
                HttpSessionState session = ctx.Session;
                if (session != null)
                {
                    object obj = session[SelectedInstanceIdKey];
                    if (obj != null)
                        instanceId = (Guid)obj;
                }
            }
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

                Guid organizationId = (Guid)base[SelectedOrganizationIdKey];
                Guid instanceId = (Guid)base[SelectedInstanceIdKey];

                if (instanceId != Guid.Empty)
                {
                    settings = SettingProvider.GetSettings(SettingLevels.Organization | SettingLevels.Instance | SettingLevels.Group, null);
                    SettingProvider.FillSettingsByGroupValues(ref settings, organizationId, instanceId, this.GroupIdList);
                }
                else if ((Guid)base[SelectedOrganizationIdKey] != Guid.Empty)
                {
                    settings = SettingProvider.GetSettings(SettingLevels.Organization, null);
                    SettingProvider.FillSettingsByOrganizationValues(ref settings, organizationId);
                }
                else
                    settings = new SettingCollection();
                return settings;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raises when the selected instance is changing.
        /// </summary>
        public static event EventHandler<UserContextSelectedInstanceChangingEventArgs> SelectedInstanceChanging;

        /// <summary>
        /// Raises when the selected instance is changed.
        /// </summary>
        public static event EventHandler SelectedInstanceChanged;

        /// <summary>
        /// Raises when the selected organization is changing.
        /// </summary>
        public static event EventHandler<UserContextSelectedOrganizationChangingEventArgs> SelectedOrganizationChanging;

        /// <summary>
        /// Raises when the selected organization is changed.
        /// </summary>
        public static event EventHandler SelectedOrganizationChanged;

        #endregion

        #region Private Methods

        private void SelectedInstanceChange(Instance newInstance, bool? isPersistent)
        {
            ArrayList actionIdList = new ArrayList();
            ArrayList groupIdList = new ArrayList();
            ArrayList roleIdList = new ArrayList();
            Guid roleId = Guid.Empty;
            string startUrl = null;

            OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(newInstance.OrganizationId);
            SortedList list = null;
            SortedList list2 = new SortedList();
            OrganizationDataSet.GroupsInstancesRolesDataTable gdrTable = ds.GroupsInstancesRoles;

            // Gets the actions that the user have access to.
            foreach (OrganizationDataSet.UsersGroupsRow ugRow in UserProvider.GetUserGroups(this.UserId, newInstance.OrganizationId))
            {
                Guid groupId = ugRow.GroupId;
                OrganizationDataSet.GroupsInstancesRolesRow gdrRow = gdrTable.FindByGroupIdInstanceId(groupId, newInstance.InstanceId);
                if (gdrRow != null)
                {
                    if (!groupIdList.Contains(groupId)) groupIdList.Add(groupId);

                    roleId = gdrRow.RoleId;
                    if (!roleIdList.Contains(roleId))
                    {
                        if (RoleProvider.GetRoleRow(roleId) != null)
                            roleIdList.Add(roleId);
                    }

                    if (roleId != RoleProvider.InstanceAdministratorRoleId)
                    {
                        list = GroupProvider.GetActionIdListWithEnabledFlag(ds.GroupsInstancesActions, groupId, newInstance.InstanceId, roleId);
                        foreach (Guid actionId in list.Keys)
                        {
                            bool enabled = (bool)list[actionId];
                            if (list2.Contains(actionId))
                                list2[actionId] = enabled;
                            else
                                list2.Add(actionId, enabled);
                        }
                    }
                }
            }

            foreach (Guid actionId in list2.Keys)
            {
                if ((bool)list2[actionId]) actionIdList.Add(actionId);
            }

            roleId = RoleProvider.AssumeRole(this.IsOrganizationAdministrator, ref roleIdList, ref startUrl, ref actionIdList);

            if (roleIdList.Count == 0)
                throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.UserContext_ErrorMessage_NoGroupsInstanceRoles, newInstance.Name));

            if (SelectedInstanceChanging != null)
            {
                if (string.IsNullOrEmpty(this.Email))
                {
                    OrganizationDataSet.UserRow userRow = UserProvider.GetUserRow(this.UserId, newInstance.OrganizationId);
                    if (userRow != null)
                        RefreshDetails(this, userRow);
                }
                SelectedInstanceChanging(this, new UserContextSelectedInstanceChangingEventArgs() { Instance = newInstance });
            }

            base[ActionIdListKey] = actionIdList;
            base[GroupIdListKey] = groupIdList;
            base[RoleIdListKey] = roleIdList;
            base[RoleIdKey] = roleId;
            base[StartPageUrlKey] = WebApplication.CreateApplicationAbsoluteUrl(startUrl);
            SelectedInstanceId = newInstance.InstanceId;
            base[TimeZoneIdKey] = null;
            base[TimeFormatKey] = null;
            base[SelectedInstanceIdKey] = newInstance.InstanceId;

            if (FrameworkConfiguration.Current.WebApplication.AuthenticationMode == AuthenticationMode.Forms)
                LoginProvider.SetAuthCookie(this.UserId, newInstance.OrganizationId, newInstance.InstanceId, isPersistent);

            if (SelectedInstanceChanged != null)
                SelectedInstanceChanged(this, EventArgs.Empty);
        }

        private void SelectedOrganizationChange(Organization newOrganization, bool isOrgAdmin, ArrayList userGroupIdList, bool? isPersistent)
        {            
            if ((Guid)base[SelectedOrganizationIdKey] != newOrganization.OrganizationId)
                CheckWebSite(newOrganization.OrganizationId, null);

            Guid currentInstanceId = Guid.Empty;
            if (this.SelectedInstance != null)
            {
                if (this.SelectedInstance.OrganizationId == newOrganization.OrganizationId)
                    currentInstanceId = this.SelectedInstance.InstanceId;
            }

            Breadcrumbs.Clear();

            base[SelectedInstanceIdKey] = Guid.Empty;

            ArrayList actionIdList = new ArrayList();
            ArrayList groupIdList = new ArrayList();
            ArrayList roleIdList = new ArrayList();
            Guid roleId = Guid.Empty;
            string startUrl = null;

            // Looks up roles in all instances.
            if (userGroupIdList.Count > 0)
            {
                InstanceCollection newOrganizationInstances = InstanceProvider.GetInstances(newOrganization.OrganizationId, false);
                foreach (Guid groupId in userGroupIdList)
                {
                    if (!groupIdList.Contains(groupId))
                        groupIdList.Add(groupId);

                    foreach (Instance instance in newOrganizationInstances)
                    {
                        if (instance.GroupIdRoleIdList.Contains(groupId))
                        {
                            roleId = (Guid)instance.GroupIdRoleIdList[groupId];
                            if (!roleIdList.Contains(roleId))
                            {
                                if (RoleProvider.GetRoleRow(roleId) != null)
                                    roleIdList.Add(roleId);
                            }
                        }
                    }
                }
            }

            roleId = RoleProvider.AssumeRole(isOrgAdmin, ref roleIdList, ref startUrl, ref actionIdList);

            if (roleIdList.Count == 0)
                throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.UserContext_ErrorMessage_NoGroupsInstancesRoles, newOrganization.Name));

            if (SelectedOrganizationChanging != null)
                SelectedOrganizationChanging(this, new UserContextSelectedOrganizationChangingEventArgs() { Organization = newOrganization });

            SelectedOrganizationId = newOrganization.OrganizationId;
            base[TimeZoneIdKey] = null;
            base[TimeFormatKey] = null;
            base[SelectedOrganizationIdKey] = newOrganization.OrganizationId;
            base[ActionIdListKey] = actionIdList;
            base[GroupIdListKey] = groupIdList;
            base[RoleIdListKey] = roleIdList;
            base[RoleIdKey] = roleId;
            base[StartPageUrlKey] = WebApplication.CreateApplicationAbsoluteUrl(startUrl);

            if (FrameworkConfiguration.Current.WebApplication.AuthenticationMode == AuthenticationMode.Forms)
                LoginProvider.SetAuthCookie(this.UserId, newOrganization.OrganizationId, Guid.Empty, isPersistent);

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

            LoginProvider.ParseAuthCookie(out userId, out organizationId, out instanceId);

            if (userId != Guid.Empty)
            {
                UserContext user = new UserContext();
                user.Initialize(userId, organizationId, instanceId, null);

                return user;
            }

            return null;
        }

        /// <summary>
        /// Initializes the Organization or Instance from custom URL.
        /// </summary>
        internal static void InitializeOrganizationOrInstanceFromCustomUrl()
        {
            if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled && (HttpContext.Current != null))
            {
                CommonDataSet.CustomUrlRow row = CustomUrlProvider.GetCustomUrl(HttpContext.Current.Request.Url.Host.ToLower(CultureInfo.CurrentCulture));
                if (row != null)
                {
                    UserContext.SelectedOrganizationId = row.OrganizationId;
                    UserContext.SelectedInstanceId = (row.IsInstanceIdNull() ? Guid.Empty : row.InstanceId);
                }
                else
                    CustomUrlProvider.InitializeOrganizationOrInstanceFromCustomUrl();
            }
        }

        internal void CheckWebSite(Guid? organizationId, string returnUrl)
        {
            if (organizationId.HasValue)
            {
                Guid webSiteId = WebsiteProvider.GetWebsiteIdByOrganizationId(organizationId.Value);
                if (WebApplication.WebsiteId != webSiteId)
                {
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        HttpContext ctx = HttpContext.Current;
                        if (ctx != null)
                        {
                            if (ctx.Request != null)
                                returnUrl = ctx.Request.QueryString["returnurl"];
                        }
                    }
                    (new LoginProvider()).SignOut(true, WebApplication.LoginProvider.GetLoginUrl(this.UserId, organizationId.Value, returnUrl));
                }
            }
        }

        internal void RefreshDetails(OrganizationDataSet.UserRow userRow)
        {
            RefreshDetails(this, userRow);
        }

        internal static void RefreshDetails(UserContext user, OrganizationDataSet.UserRow userRow)
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
            }
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

        public void Initialize(Guid userId, Guid organizationId, Guid instanceId, bool? isPersistent)
        {
            this.UserId = userId;

            if (FrameworkConfiguration.Current.WebApplication.AuthenticationMode == AuthenticationMode.Forms)
                LoginProvider.SetAuthCookie(userId, Guid.Empty, Guid.Empty, isPersistent);

            if (organizationId != Guid.Empty)
            {
                this.SelectOrganization(organizationId, isPersistent);

                if (instanceId != Guid.Empty)
                    this.SelectInstance(instanceId, isPersistent);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified key is reserved by Micajah.Common.Security.UserContext class for internal using.
        /// </summary>
        /// <param name="key">The key of the element to check.</param>
        /// <returns>true, if the specified key is reserved; otherwise, false.</returns>
        public static bool KeyIsReserved(object key)
        {
            return (ReservedKeys.Contains(key));
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
            if (organizationId == Guid.Empty) return;

            Organization newOrganization = new Organization();
            if (!newOrganization.Load(organizationId))
                throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.OrganizationProvider_ErrorMessage_NoOrganization, organizationId));

            if (newOrganization.Deleted)
                throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.OrganizationProvider_ErrorMessage_OrganizationIsDeleted, newOrganization.Name));
            else if (!newOrganization.Active)
                throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.OrganizationProvider_ErrorMessage_OrganizationIsInactive, newOrganization.Name));
            else if (newOrganization.Expired && (newOrganization.GraceDaysRemaining == 0))
                throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.OrganizationProvider_ErrorMessage_OrganizationHasExpired, newOrganization.Name));

            Guid userId = this.UserId;
            if (!WebApplication.LoginProvider.LoginIsActiveInOrganization(userId, organizationId))
                throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.UserContext_ErrorMessage_YourAccountInOrganizationIsInactivated, newOrganization.Name));

            bool isOrganizationAdministrator = false;
            ArrayList userGroupIdList = null;

            OrganizationDataSet.UserRow userRow = UserProvider.GetUserRow(userId, organizationId);
            if (userRow != null)
            {
                isOrganizationAdministrator = WebApplication.LoginProvider.LoginIsOrganizationAdministrator(userId, organizationId);
                userGroupIdList = UserProvider.GetUserGroupIdList(organizationId, userId);

                if (!isOrganizationAdministrator)
                {
                    if (userGroupIdList.Count == 0)
                        throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.UserContext_ErrorMessage_NoGroups, newOrganization.Name));
                    else if (newOrganization.Instances.Count == 0)
                        throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.InstanceProvider_ErrorMessage_NoActiveInstances, newOrganization.Name));
                }
            }
            else
                throw new AuthenticationException(FrameworkConfiguration.Current.WebApplication.Login.FailureText);

            this.SelectedOrganizationChange(newOrganization, isOrganizationAdministrator, userGroupIdList, isPersistent);

            RefreshDetails(this, userRow);

            userRow.LastLoginDate = DateTime.UtcNow;
            newOrganization.TableAdapters.UserTableAdapter.Update(userRow);

            if (SelectedOrganizationChanged != null)
                SelectedOrganizationChanged(this, EventArgs.Empty);
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
            if (instanceId == Guid.Empty) return;

            Guid organizationId = (Guid)base[SelectedOrganizationIdKey];
            if (organizationId != Guid.Empty)
            {
                Instance instance = InstanceProvider.GetInstance(instanceId, organizationId);
                if (instance != null)
                {
                    if (!instance.Active)
                        throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.InstanceProvider_ErrorMessage_InstanceIsInactive, instance.Name));
                    else if (instance.GroupIdRoleIdList.Count == 0)
                        throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.UserContext_ErrorMessage_NoGroupsInstanceRoles, instance.Name));
                    else
                    {
                        if (UserProvider.UserIsActiveInInstance(this.UserId, instanceId, organizationId))
                            this.SelectedInstanceChange(instance, isPersistent);
                        else
                            throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.UserContext_ErrorMessage_YourAccountInInstanceIsInactivated, instance.Name));
                    }
                }
                else
                    throw new AuthenticationException(string.Format(CultureInfo.InvariantCulture, Resources.InstanceProvider_ErrorMessage_NoInstance, instanceId, this.SelectedOrganization.Name));
            }
            else
                throw new AuthenticationException(Resources.UserContext_ErrorMessage_NoSelectedOrganization);
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
            bool exists = false;
            Organization org = this.SelectedOrganization;
            if (org != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Guid groupId in GroupIdList)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, ",'{0}'", groupId.ToString());
                }
                if (sb.Length > 0)
                {
                    sb.Remove(0, 1);

                    ArrayList roleIdList = RoleProvider.GetRoleIdListByShortNames(shortName);
                    OrganizationDataSet.GroupsInstancesRolesDataTable gdrTable = org.DataSet.GroupsInstancesRoles;

                    foreach (OrganizationDataSet.GroupsInstancesRolesRow gdrRow in gdrTable.Select(
                        string.Concat(gdrTable.InstanceIdColumn.ColumnName, " = '", instanceId.ToString(), "' AND ", gdrTable.GroupIdColumn.ColumnName, " IN (", sb.ToString(), ")")))
                    {
                        if (roleIdList.Contains(gdrRow.RoleId))
                        {
                            exists = true;
                            break;
                        }
                    }
                }
            }
            return exists;
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
            Guid organizationId = (Guid)base[SelectedOrganizationIdKey];
            if (organizationId != Guid.Empty)
                return UserProvider.UserIsInstanceAdministrator(organizationId, instanceId, this.UserId);
            return false;
        }

        /// <summary>
        /// Refreshes the cached data for the current user.
        /// </summary>
        public static void RefreshCurrent()
        {
            UserContext ctx = UserContext.Current;
            if (ctx != null) ctx.Refresh();
        }

        /// <summary>
        /// Refreshes the cached data for this user.
        /// </summary>
        public void Refresh()
        {
            Guid organizationId = (Guid)base[SelectedOrganizationIdKey];
            if (organizationId != Guid.Empty)
                this.SelectOrganization(organizationId, null);
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

    public class UserContextSelectedInstanceChangingEventArgs : EventArgs
    {
        #region Public Properties

        public Instance Instance { get; set; }

        #endregion
    }

    public class UserContextSelectedOrganizationChangingEventArgs : EventArgs
    {
        #region Public Properties

        public Organization Organization { get; set; }

        #endregion
    }
}
