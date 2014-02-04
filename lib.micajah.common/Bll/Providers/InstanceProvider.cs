using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Dal.ClientDataSetTableAdapters;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with instances.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class InstanceProvider
    {
        #region Members

        private const int NameMaxLength = 255;
        private const int DescriptionMaxLength = 255;
        private const int ExternalIdMaxLength = 255;

        internal const string DefaultTimeZoneId = "Eastern Standard Time";
        internal const string DefaultWorkingDays = "1111100";

        private const string InstanceKeyFormat = "mc.Instance.{0:N}";
        private const string InstanceByPseudoIdKeyFormat = "mc.Instance.{0}.{1:N}";

        #endregion

        #region Events

        /// <summary>
        /// Occurs when an instance is inserted.
        /// </summary>
        public static event EventHandler<InstanceProviderEventArgs> InstanceInserted;

        /// <summary>
        /// Occurs when an instance is updated.
        /// </summary>
        public static event EventHandler<InstanceProviderEventArgs> InstanceUpdated;

        #endregion

        #region Internal Properties

        internal static string InstancesFilterExpression
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                UserContext user = UserContext.Current;
                if ((user != null) && (user.OrganizationId != Guid.Empty))
                {
                    if (!user.IsOrganizationAdministrator)
                    {
                        foreach (ClientDataSet.InstanceRow instanceRow in GetInstances(user.OrganizationId))
                        {
                            if (!user.IsInstanceAdministrator(instanceRow.InstanceId))
                                sb.AppendFormat(",'{0}'", instanceRow.InstanceId);
                        }

                        if (sb.Length > 0)
                        {
                            sb.Remove(0, 1);
                            sb.Append(")");
                            sb.Insert(0, "CONVERT(InstanceId, 'System.String') NOT IN (");
                        }
                    }
                }
                return sb.ToString();
            }
        }

        #endregion

        #region Private Methods

        private static Instance GetTemplateInstance(Guid? templateInstanceId)
        {
            Instance templateInstance = null;
            if (templateInstanceId.HasValue)
            {
                if (templateInstanceId.Value != Guid.Empty)
                {
                    Organization templateOrg = OrganizationProvider.TemplateOrganization;
                    if (templateOrg != null)
                        templateInstance = GetInstance(templateInstanceId.Value, templateOrg.OrganizationId);
                }
            }
            return templateInstance;
        }

        /// <summary>
        /// Creates new instance with specified details in specified organization.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <param name="description">The instance description.</param>
        /// <param name="enableSignupUser">The value indicating that the sign up user is enabled for the instance.</param>
        /// <param name="externalId">The external id.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="workingDays">The working days.</param>
        /// <param name="active">The active status.</param>
        /// <param name="canceledTime">The canceled time.</param>
        /// <param name="trial">The trial status.</param>
        /// <param name="beta">The beta status.</param>
        /// <param name="emailSuffixes">The instance email suffixes.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="adminEmail">The email of the user to make his the administrator of the instance.</param>
        /// <param name="sendNotificationEmail">true to send notification email to administrator; otherwise, false.</param>
        /// <param name="configure">true to configure the instance.</param>
        /// <returns>The System.Guid that represents the identifier of the newly created instance.</returns>
        private static Guid InsertInstance(string name, string description, bool enableSignupUser
            , string externalId, string timeZoneId, int timeFormat, int dateFormat, string workingDays, bool active, DateTime? canceledTime, bool trial, bool beta, string emailSuffixes
            , Guid organizationId, string adminEmail, string adminPassword, bool sendNotificationEmail, bool configure, bool newOrg
            , Guid? templateInstanceId
            , string partialCustomUrl)
        {
            ClientDataSet.InstanceDataTable table = new ClientDataSet.InstanceDataTable();
            ClientDataSet.InstanceRow row = table.NewInstanceRow();
            Guid instanceId = Guid.NewGuid();

            name = Support.TrimString(name, table.NameColumn.MaxLength);
            description = Support.TrimString(description, table.DescriptionColumn.MaxLength);
            externalId = Support.TrimString(externalId, table.ExternalIdColumn.MaxLength);

            Instance templateInstance = GetTemplateInstance(templateInstanceId);

            row.InstanceId = instanceId;
            row.PseudoId = Support.GeneratePseudoUnique();
            row.Name = name;
            if (description != null) row.Description = description;
            row.OrganizationId = organizationId;
            row.EnableSignUpUser = (templateInstance == null) ? enableSignupUser : templateInstance.EnableSignupUser;
            row.ExternalId = ((externalId == null) ? string.Empty : externalId);
            if (string.IsNullOrEmpty(timeZoneId))
                row.TimeZoneId = DefaultTimeZoneId;
            else
                row.TimeZoneId = timeZoneId;
            if (templateInstance == null)
            {
                row.TimeFormat = timeFormat;
                row.DateFormat = dateFormat;
            }
            else
            {
                row.TimeFormat = templateInstance.TimeFormat;
                row.DateFormat = templateInstance.DateFormat;
            }
            row.WorkingDays = (templateInstance == null) ? string.IsNullOrEmpty(workingDays) ? DefaultWorkingDays : workingDays : templateInstance.WorkingDays;
            row.Active = active;
            if (canceledTime.HasValue) row.CanceledTime = canceledTime.Value;
            row.Trial = (templateInstance == null) ? trial : templateInstance.Trial;
            row.Beta = (templateInstance == null) ? beta : templateInstance.Beta;
            row.Deleted = false;

            try
            {
                table.AddInstanceRow(row);
            }
            catch (ConstraintException ex)
            {
                throw new ConstraintException(string.Format(CultureInfo.CurrentCulture, Resources.InstanceProvider_ErrorMessage_InstanceAlreadyExists, name), ex);
            }

            using (InstanceTableAdapter adapter = new InstanceTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Update(row);
            }

            Instance inst = CreateInstance(row);

            // Creates the built-in group for the Administrators of this Instance.
            Guid groupId = GroupProvider.InsertGroup(string.Format(CultureInfo.InvariantCulture, Resources.GroupProvider_InstanceAdministratorGroup_Name, name), Resources.GroupProvider_InstanceAdministratorGroup_Description, organizationId, true);
            GroupProvider.InsertGroupInstanceRole(groupId, instanceId, RoleProvider.InstanceAdministratorRoleId, organizationId);

            Guid? userId = null;
            string password = null;

            // Makes the user as administrator of the instance.
            if (!string.IsNullOrEmpty(adminEmail))
            {
                userId = UserProvider.AddUserToOrganization(adminEmail, null, null, null
                    , null, null, null, null, null, null
                    , null, null, null, null, null
                    , null, null, null
                    , groupId.ToString(), true
                    , organizationId, false
                    , false
                    , 0, 0, out password);

                if (string.IsNullOrEmpty(password))
                    password = adminPassword;
            }

            if (emailSuffixes != null)
                EmailSuffixProvider.InsertEmailSuffixName(organizationId, instanceId, ref emailSuffixes);

            if (configure) ConfigureInstance(instanceId, organizationId);

            if (!string.IsNullOrEmpty(partialCustomUrl))
                CustomUrlProvider.InsertCustomUrl(organizationId, instanceId, null, partialCustomUrl);

            RaiseInstanceInserted(inst, userId, templateInstance);

            if (sendNotificationEmail && (!string.IsNullOrEmpty(adminEmail)))
                UserProvider.SendUserEmail(adminEmail, password, organizationId, Support.ConvertListToString(UserProvider.GetUserGroupIdList(organizationId, userId.Value, true)), (!string.IsNullOrEmpty(password)), newOrg, (!string.IsNullOrEmpty(password)));

            return instanceId;
        }

        /// <summary>
        /// Raises the InstanceInserted event.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="userId">The unique identifier of the user that is the administrator of the instance.</param>
        /// <param name="templateInstance">The template instance.</param>
        private static void RaiseInstanceInserted(Instance instance, Guid? userId, Instance templateInstance)
        {
            if (InstanceInserted != null)
                InstanceInserted(null, new InstanceProviderEventArgs() { Instance = instance, UserId = userId, TemplateInstance = templateInstance });
        }

        /// <summary>
        /// Raises the InstanceUpdated event.
        /// </summary>
        /// <param name="organization">The organization.</param>
        private static void RaiseInstanceUpdated(Instance instance)
        {
            if (InstanceUpdated != null)
                InstanceUpdated(null, new InstanceProviderEventArgs() { Instance = instance });
        }

        #endregion

        #region Internal Methods

        internal static Instance CreateInstance(ClientDataSet.InstanceRow row)
        {
            if (row != null)
            {
                Instance instance = new Instance();

                instance.InstanceId = row.InstanceId;
                instance.PseudoId = row.PseudoId;
                instance.OrganizationId = row.OrganizationId;
                instance.Name = row.Name;
                instance.Description = row.Description;
                instance.EnableSignupUser = row.EnableSignUpUser;
                instance.ExternalId = row.ExternalId;
                instance.TimeZoneId = row.TimeZoneId;
                instance.TimeFormat = row.TimeFormat;
                instance.DateFormat = row.DateFormat;
                instance.WorkingDays = row.WorkingDays;
                instance.Active = row.Active;
                if (!row.IsCanceledTimeNull()) instance.CanceledTime = new DateTime?(row.CanceledTime);
                instance.Trial = row.Trial;
                instance.Beta = row.Beta;
                if (!row.IsCreatedTimeNull()) instance.CreatedTime = new DateTime?(row.CreatedTime);
                instance.BillingPlan = (BillingPlan)row.BillingPlan;
                instance.CreditCardStatus = (CreditCardStatus)row.CreditCardStatus;

                return instance;
            }

            return null;
        }

        #region Cache Methods

        internal static Instance GetInstanceFromCache(Guid instanceId, Guid organizationId)
        {
            return GetInstanceFromCache(instanceId, organizationId, false);
        }

        internal static Instance GetInstanceFromCache(Guid instanceId, Guid organizationId, bool putToCacheIfNotExists)
        {
            string key = string.Format(CultureInfo.InvariantCulture, InstanceKeyFormat, instanceId);
            Instance instance = CacheManager.Current.Get(key, true) as Instance;

            if (instance == null)
            {
                if (putToCacheIfNotExists)
                {
                    instance = GetInstance(instanceId, organizationId);

                    if (instance != null)
                        CacheManager.Current.PutWithDefaultTimeout(key, instance);
                }
            }

            return instance;
        }

        internal static Instance GetInstanceByPseudoIdFromCache(string pseudoId, Guid organizationId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, InstanceByPseudoIdKeyFormat, pseudoId, organizationId);
            Instance instance = CacheManager.Current.Get(key, true) as Instance;

            if (instance == null)
            {
                instance = GetInstanceByPseudoId(pseudoId, organizationId);

                if (instance != null)
                    CacheManager.Current.PutWithDefaultTimeout(key, instance);
            }

            return instance;
        }

        internal static void PutInstanceToCache(Instance instance)
        {
            string key = string.Format(CultureInfo.InvariantCulture, InstanceKeyFormat, instance.InstanceId);
            CacheManager.Current.PutWithDefaultTimeout(key, instance);
        }

        internal static void RemoveInstanceFromCache(Guid instanceId)
        {
            string key = string.Format(CultureInfo.InvariantCulture, InstanceKeyFormat, instanceId);
            CacheManager.Current.Remove(key);
        }

        #endregion

        internal static Guid InsertFirstInstance(string timeZoneId, Guid? templateInstanceId, Guid organizationId
            , string adminEmail, string adminPassword, string partialCustomUrl
            , bool sendNotificationEmail)
        {
            return InsertInstance(Resources.InstanceProvider_FirstInstance_Name, null, false, null, timeZoneId, 0, 0, null, true, null, false, false, null, organizationId
                , adminEmail, adminPassword, sendNotificationEmail, false, true, templateInstanceId, partialCustomUrl);
        }

        /// <summary>
        /// Creates and returns the instances collection sorted by name from specified data sourceRow.
        /// </summary>
        /// <param name="table">A System.Data.DataTable object that represents the data sourceRow to create a collection from.</param>
        /// <returns>A Micajah.Common.Bll.InstanceCollection object that contains the instances.</returns>
        internal static InstanceCollection CreateInstanceCollection(DataTable table, bool sort)
        {
            InstanceCollection coll = new InstanceCollection();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    coll.Add(CreateInstance(row as ClientDataSet.InstanceRow));
                }

                if (sort)
                    coll.Sort();
            }
            return coll;
        }

        internal static ClientDataSet.InstanceRow GetInstanceRow(Guid organizationId, Guid instanceId)
        {
            using (InstanceTableAdapter adapter = new InstanceTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                ClientDataSet.InstanceDataTable table = adapter.GetInstance(instanceId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        internal static List<Guid> GetGroupsInstances(string groupId, Guid organizationId)
        {
            List<Guid> instanceIdList = new List<Guid>();

            using (InstanceTableAdapter adapter = new InstanceTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                foreach (ClientDataSet.InstanceRow row in adapter.GetInstancesByGroups(organizationId, groupId.ToUpperInvariant()))
                {
                    if (!instanceIdList.Contains(row.InstanceId))
                        instanceIdList.Add(row.InstanceId);
                }
            }

            return instanceIdList;
        }

        /// <summary>
        /// Returns the organization's instances which the specified user have access to.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="organization">The unique identifier of the organization.</param>
        /// <returns>The organization's instances which this user have access to.</returns>
        internal static InstanceCollection GetUserInstances(Guid userId, Guid organizationId, bool isOrgAdmin)
        {
            InstanceCollection coll = null;

            if ((userId != Guid.Empty) && (organizationId != Guid.Empty))
            {
                Guid? roleId = null;
                if (isOrgAdmin)
                {
                    bool isInstanceAdmin = false;
                    roleId = RoleProvider.AssumeRole(isOrgAdmin, null, ref isInstanceAdmin);
                }

                using (InstanceTableAdapter adapter = new InstanceTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    ClientDataSet.InstanceDataTable table = adapter.GetInstancesByUserIdRoleId(organizationId, userId, roleId);
                    coll = CreateInstanceCollection(table, false);
                }
            }
            else
                coll = new InstanceCollection();

            return coll;
        }

        internal static void UpdateInstancesPseudoId(Guid organizationId)
        {
            using (InstanceTableAdapter adapter = new InstanceTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                ClientDataSet.InstanceDataTable table = adapter.GetInstances(organizationId);

                foreach (ClientDataSet.InstanceRow instanceRow in table)
                {
                    if (string.IsNullOrEmpty(instanceRow.PseudoId))
                        instanceRow.PseudoId = Support.GeneratePseudoUnique();
                }

                adapter.Update(table);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Configures the specified instance: creates the groups and roles of these groups in this instance which are based on non built-in roles.
        /// </summary>
        /// <param name="instanceId">The unique identifier of the instance to configure.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        public static void ConfigureInstance(Guid instanceId, Guid organizationId)
        {
            string name = GetInstanceName(instanceId, organizationId);
            if (!string.IsNullOrEmpty(name)) name += " ";

            foreach (ConfigurationDataSet.RoleRow roleRow in ConfigurationDataSet.Current.Role.Rows)
            {
                if (!roleRow.BuiltIn)
                {
                    Guid groupId = GroupProvider.InsertGroup(name + roleRow.Name + "s", string.Empty, organizationId, false);
                    GroupProvider.InsertGroupInstanceRole(groupId, instanceId, roleRow.RoleId, organizationId);
                }
            }
        }

        /// <summary>
        /// Gets the instances, excluding marked as deleted.
        /// </summary>
        /// <returns>The System.Data.DataTable object that contains instances.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetInstances()
        {
            return GetInstances(UserContext.Current.OrganizationId);
        }

        /// <summary>
        /// Gets the instances for the specified organization, excluding marked as deleted.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <returns>The table object that contains instances.</returns>
        public static ClientDataSet.InstanceDataTable GetInstances(Guid organizationId)
        {
            using (InstanceTableAdapter adapter = new InstanceTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetInstances(organizationId);
            }
        }

        /// <summary>
        /// Gets the instances for the specified organization, excluding marked as deleted.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="includeInactive">The flag indicating that the inacive organizations are included in result.</param>
        /// <returns>The instances collection.</returns>
        public static InstanceCollection GetInstances(Guid organizationId, bool includeInactive)
        {
            InstanceCollection instances = new InstanceCollection();
            if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
            {
                instances = CreateInstanceCollection(GetInstances(organizationId), false);

                if (!includeInactive)
                {
                    InstanceCollection coll = new InstanceCollection();

                    foreach (Instance inst in instances)
                    {
                        if (!inst.Active)
                            continue;

                        coll.Add(inst);
                    }

                    instances = coll;
                }

                instances.Sort();
            }
            else
            {
                Instance inst = GetFirstInstance(organizationId);

                if (inst != null)
                    instances.Add(inst);
            }

            return instances;
        }

        /// <summary>
        /// Gets the instances for the template organization, excluding inactive and marked as deleted.
        /// </summary>
        /// <returns>The instances collection.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static InstanceCollection GetTemplateInstances()
        {
            Organization templateOrg = OrganizationProvider.TemplateOrganization;

            InstanceCollection instances = null;
            InstanceCollection coll = new InstanceCollection();

            if (templateOrg != null)
            {
                instances = CreateInstanceCollection(GetInstances(templateOrg.OrganizationId), false);

                foreach (Instance inst in instances)
                {
                    if (!inst.Active)
                        continue;

                    coll.Add(inst);
                }
            }

            instances = coll;
            instances.SortByCreatedTime();

            return instances;
        }

        /// <summary>
        /// Gets a Micajah.Common.Bll.Instance object populated with information of the specified instance.
        /// </summary>
        /// <param name="instanceId">Specifies the instance identifier to get information.</param>
        /// <returns>
        /// The Micajah.Common.Bll.Instance object populated with information of the specified instance. 
        /// If the instance is not found, the method returns null reference.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static Instance GetInstance(Guid instanceId)
        {
            UserContext user = UserContext.Current;
            if (user != null && user.OrganizationId != Guid.Empty)
                return GetInstance(instanceId, user.OrganizationId);
            else
                return null;
        }

        /// <summary>
        /// Returns a Micajah.Common.Bll.Instance object populated with information of the specified instance in specified organization.
        /// </summary>
        /// <param name="instanceId">Specifies the instance identifier to get information.</param>
        /// <param name="organizationId">Specifies the organization identifier.</param>
        /// <returns>
        /// The Micajah.Common.Bll.Instance object populated with information of the specified instance in specified organization. 
        /// If the instance is not found, the method returns null reference.
        /// </returns>
        public static Instance GetInstance(Guid instanceId, Guid organizationId)
        {
            Instance instance = null;

            if (organizationId == Guid.Empty)
            {
                foreach (MasterDataSet.OrganizationRow row in OrganizationProvider.GetOrganizations(null))
                {
                    instance = CreateInstance(GetInstanceRow(row.OrganizationId, instanceId));
                    if (instance != null)
                        break;
                }
            }
            else
                instance = CreateInstance(GetInstanceRow(organizationId, instanceId));

            return instance;
        }

        /// <summary>
        /// Returns a Micajah.Common.Bll.Instance object populated with information of the specified instance in specified organization.
        /// </summary>
        /// <param name="pseudoId">The pseudo unique identifier of the instance.</param>
        /// <param name="organizationId">Specifies the organization identifier.</param>
        /// <returns>The Micajah.Common.Bll.Instance object populated with information of the specified instance in specified organization. If the instance is not found, the method returns null reference.</returns>
        public static Instance GetInstanceByPseudoId(string pseudoId, Guid organizationId)
        {
            if (!string.IsNullOrEmpty(pseudoId))
            {
                using (InstanceTableAdapter adapter = new InstanceTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    ClientDataSet.InstanceDataTable table = adapter.GetInstanceByPseudoId(pseudoId);
                    if (table.Count > 0)
                        return CreateInstance(table[0]);

                }
            }
            return null;
        }

        /// <summary>
        /// Returns an first instance in which the specified setting with specified value exists.
        /// </summary>
        /// <param name="organizationId">The organization's identifier to search the instance in.</param>
        /// <param name="shortName">The short name of the setting to search for.</param>
        /// <param name="value">The value of the setting to search for.</param>
        /// <returns>The Micajah.Common.Bll.Instance object that represents the first instance in which the specified setting with specified value exists.</returns>
        public static Instance GetInstanceBySettingValue(Guid organizationId, string shortName, string value)
        {
            foreach (ClientDataSet.InstanceRow row in GetInstances(organizationId))
            {
                SettingCollection settings = SettingProvider.GetInstanceSettings(organizationId, row.InstanceId);
                Setting setting = settings.FindByShortName(shortName);
                if ((setting != null) && (string.Compare(value, setting.Value, StringComparison.Ordinal) == 0))
                    return CreateInstance(row);
            }

            return null;
        }

        /// <summary>
        /// Returns the identifier of the specified instance in specified organization.
        /// </summary>
        /// <param name="organizationName">The name of the organization.</param>
        /// <param name="instanceName">The name of the instance.</param>
        /// <returns>The identifier of the specified instance in specified organization</returns>
        public static Guid GetInstanceIdByName(string organizationName, string instanceName)
        {
            Guid instanceId = Guid.Empty;
            if (!string.IsNullOrEmpty(instanceName))
            {
                Guid organizationId = OrganizationProvider.GetOrganizationIdByName(organizationName);

                using (InstanceTableAdapter adapter = new InstanceTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    ClientDataSet.InstanceDataTable table = adapter.GetInstanceByName(instanceName);
                    if (table.Count > 0)
                        instanceId = table[0].InstanceId;
                }
            }
            return instanceId;
        }

        /// <summary>
        /// Returns the name of the specified instance in specified organization.
        /// </summary>
        /// <param name="instanceId">Specifies the instance identifier to get the name of.</param>
        /// <param name="organizationId">Specifies the organization identifier.</param>
        /// <returns>The System.String that represents the name of the instance.</returns>
        public static string GetInstanceName(Guid instanceId, Guid organizationId)
        {
            ClientDataSet.InstanceRow instanceRow = GetInstanceRow(organizationId, instanceId);
            return ((instanceRow == null) ? string.Empty : instanceRow.Name);
        }

        public static string[] GetInstanceNames(IList<Guid> instanceIdList, Guid organizationId)
        {
            List<string> names = new List<string>();

            if (instanceIdList != null)
            {
                ClientDataSet.InstanceDataTable table = GetInstances(organizationId);

                foreach (Guid instId in instanceIdList)
                {
                    ClientDataSet.InstanceRow row = table.FindByInstanceId(instId);
                    if (row != null)
                        names.Add(row.Name);
                }

                names.Sort();
            }

            return names.ToArray();
        }

        public static Instance GetFirstInstance(Guid organizationId)
        {
            Instance inst = null;
            DataTable table = GetInstances(organizationId);

            foreach (ClientDataSet.InstanceRow row in table.Rows)
            {
                if (row != null)
                {
                    if (row.Active)
                    {
                        inst = CreateInstance(row);
                        break;
                    }
                }
            }

            return inst;
        }

        public static Guid GetFirstInstanceId(Guid organizationId)
        {
            Instance inst = GetFirstInstance(organizationId);
            return ((inst == null) ? Guid.Empty : inst.InstanceId);
        }

        public static string GetInstanceLogoImageUrl(Guid instanceId)
        {
            return ResourceProvider.GetInstanceLogoImageUrl(instanceId);
        }

        /// <summary>
        /// Creates new instance with specified details in selected organization.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <param name="description">The instance description.</param>
        /// <param name="active">The active status.</param>
        /// <param name="beta">The beta status.</param>
        /// <returns>The System.Guid that represents the identifier of the newly created instance.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertInstance(string name, string description, bool active, bool beta, string vanityUrl, Guid templateInstanceId)
        {
            UserContext user = UserContext.Current;
            Guid instanceId = InsertInstance(name, description, false, null, null, 0, 0, null, active, null, false, beta, null, user.OrganizationId, user.Email, null, true, false, false, templateInstanceId, null);
            if (!string.IsNullOrEmpty(vanityUrl))
                CustomUrlProvider.InsertCustomUrl(user.OrganizationId, instanceId, null, vanityUrl);
            return instanceId;
        }

        /// <summary>
        /// Creates new instance with specified details in selected organization.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <param name="description">The instance description.</param>
        /// <param name="enableSignupUser">The value indicating that the sign up user is enabled for the instance.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="workingDays">The working days.</param>
        /// <param name="active">The active status.</param>
        /// <param name="beta">The beta status.</param>
        /// <param name="emailSuffixes">The instance email suffixes.</param>
        /// <param name="adminEmail">The e-mail address of the instance administrator.</param>
        /// <returns>The System.Guid that represents the identifier of the newly created instance.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertInstance(string name, string description, bool enableSignupUser, string timeZoneId, int timeFormat, int dateFormat, string workingDays, bool active, bool beta, string emailSuffixes, string adminEmail)
        {
            return InsertInstance(name, description, enableSignupUser, null, timeZoneId, timeFormat, dateFormat, workingDays, active, null, false, beta, emailSuffixes, UserContext.Current.OrganizationId, adminEmail, null, true, false, false, null, null);
        }

        /// <summary>
        /// Creates new instance with specified details in specified organization
        /// and refreshes all cached data by application after the action has been performed.
        /// </summary>
        /// <param name="name">The name of the instance.</param>
        /// <param name="description">The instance description.</param>
        /// <param name="enableSignupUser">The value indicating that the sign up user is enabled for the instance.</param>
        /// <param name="externalId">The external id.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="workingDays">The working days.</param>
        /// <param name="active">The active status.</param>
        /// <param name="canceledTime">The canceled time.</param>
        /// <param name="trial">The trial status.</param>
        /// <param name="beta">The beta status.</param>
        /// <param name="emailSuffixes">The instance email suffixes.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <returns>The System.Guid that represents the identifier of the newly created instance.</returns>
        public static Guid InsertInstance(string name, string description, bool enableSignupUser, string externalId, string timeZoneId, int timeFormat, int dateFormat, string workingDays, bool active, DateTime? canceledTime, bool trial, bool beta, string emailSuffixes, Guid organizationId)
        {
            return InsertInstance(name, description, enableSignupUser, externalId, timeZoneId, timeFormat, dateFormat, workingDays, active, canceledTime, trial, beta, emailSuffixes, organizationId, null, null, true, false, false, null, null);
        }

        /// <summary>
        /// Updates the details of specified instance that belong to selected organization.
        /// </summary>
        /// <param name="instanceId">The identifier of the instance.</param>
        /// <param name="name">The name of the instance.</param>
        /// <param name="description">The instance description.</param>
        /// <param name="enableSignupUser">The value indicating that the sign up user is enabled for the instance.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="workingDays">The working days.</param>
        /// <param name="beta">The beta status.</param>
        /// <param name="emailSuffixes">The instance email suffixes.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateInstance(Guid instanceId, string name, string description, bool enableSignupUser, string timeZoneId, int? timeFormat, int? dateFormat, string workingDays, bool? beta, string emailSuffixes)
        {
            UpdateInstance(instanceId, name, description, enableSignupUser, null, timeZoneId, timeFormat, dateFormat, workingDays, null, null, null, beta, emailSuffixes, UserContext.Current.OrganizationId);
        }

        /// <summary>
        /// Updates the details of specified instance that belong to selected organization.
        /// </summary>
        /// <param name="instanceId">The identifier of the instance.</param>
        /// <param name="name">The name of the instance.</param>
        /// <param name="description">The instance description.</param>
        /// <param name="active">The active status.</param>
        /// <param name="beta">The beta status.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateInstance(Guid instanceId, string name, string description, bool active, bool beta)
        {
            UpdateInstance(instanceId, name, description, null, null, null, null, null, active, beta, null);
        }

        /// <summary>
        /// Updates the details of specified instance that belong to selected organization.
        /// </summary>
        /// <param name="instanceId">The identifier of the instance.</param>
        /// <param name="name">The name of the instance.</param>
        /// <param name="description">The instance description.</param>
        /// <param name="enableSignupUser">The value indicating that the sign up user is enabled for the instance.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="workingDays">The working days.</param>
        /// <param name="active">The active status.</param>
        /// <param name="beta">The beta status.</param>
        /// <param name="emailSuffixes">The instance email suffixes.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateInstance(Guid instanceId, string name, string description, bool? enableSignupUser, string timeZoneId, int? timeFormat, int? dateFormat, string workingDays, bool? active, bool? beta, string emailSuffixes)
        {
            UpdateInstance(instanceId, name, description, enableSignupUser, null, timeZoneId, timeFormat, dateFormat, workingDays, active, null, null, beta, emailSuffixes, UserContext.Current.OrganizationId);
        }

        /// <summary>
        /// Updates the details of specified instance.
        /// </summary>
        /// <param name="instanceId">The identifier of the instance.</param>
        /// <param name="name">The name of the instance.</param>
        /// <param name="description">The instance description.</param>
        /// <param name="enableSignupUser">The value indicating that the sign up user is enabled for the instance.</param>
        /// <param name="externalId">The external id.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="workingDays">The working days.</param>
        /// <param name="active">The active status.</param>
        /// <param name="canceledTime">The canceled time.</param>
        /// <param name="trial">The trial status.</param>
        /// <param name="beta">The beta status.</param>
        /// <param name="emailSuffixes">The instance email suffixes.</param>
        /// <param name="organizationId">The identifier of the organization that the instance belong to.</param>
        public static void UpdateInstance(Guid instanceId, string name, string description, bool? enableSignupUser, string externalId, string timeZoneId, int? timeFormat, int? dateFormat, string workingDays, bool? active, DateTime? canceledTime, bool? trial, bool? beta, string emailSuffixes, Guid organizationId)
        {
            UpdateInstance(instanceId, name, description, enableSignupUser, externalId, timeZoneId, timeFormat, dateFormat, workingDays, active, canceledTime, trial, beta, emailSuffixes, organizationId, true, -1, -1);
        }

        /// <summary>
        /// Updates the details of specified instance.
        /// </summary>
        /// <param name="instanceId">The identifier of the instance.</param>
        /// <param name="name">The name of the instance.</param>
        /// <param name="description">The instance description.</param>
        /// <param name="enableSignupUser">The value indicating that the sign up user is enabled for the instance.</param>
        /// <param name="externalId">The external id.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="workingDays">The working days.</param>
        /// <param name="active">The active status.</param>
        /// <param name="canceledTime">The canceled time.</param>
        /// <param name="trial">The trial status.</param>
        /// <param name="beta">The beta status.</param>
        /// <param name="emailSuffixes">The instance email suffixes.</param>
        /// <param name="organizationId">The identifier of the organization that the instance belong to.</param>
        /// <param name="raiseEvent">Whether the Micajah.Common.Bll.Providers.InstanceProvider.InstanceUpdated event will be raised.</param>
        /// <param name="billingPlan"></param>
        /// <param name="creditCardStatus"></param>
        public static void UpdateInstance(Guid instanceId, string name, string description, bool? enableSignupUser, string externalId, string timeZoneId, int? timeFormat, int? dateFormat, string workingDays, bool? active, DateTime? canceledTime, bool? trial, bool? beta, string emailSuffixes, Guid organizationId, bool raiseEvent, int billingPlan, int creditCardStatus)
        {
            ClientDataSet.InstanceRow row = GetInstanceRow(organizationId, instanceId);
            if (row == null) return;

            name = Support.TrimString(name, NameMaxLength);
            description = Support.TrimString(description, DescriptionMaxLength);

            bool nameIsModified = (string.Compare(name, row.Name, StringComparison.Ordinal) != 0);

            try
            {
                row.Name = name;
            }
            catch (ConstraintException ex)
            {
                throw new ConstraintException(string.Format(CultureInfo.CurrentCulture, Resources.InstanceProvider_ErrorMessage_InstanceAlreadyExists, name), ex);
            }

            row.Description = description;
            if (enableSignupUser.HasValue) row.EnableSignUpUser = enableSignupUser.Value;
            if (externalId != null) row.ExternalId = Support.TrimString(externalId, ExternalIdMaxLength);
            if (!string.IsNullOrEmpty(timeZoneId)) row.TimeZoneId = timeZoneId;
            if (timeFormat.HasValue) row.TimeFormat = timeFormat.Value;
            if (dateFormat.HasValue) row.DateFormat = dateFormat.Value;
            if (workingDays != null) row.WorkingDays = workingDays;
            if (active.HasValue) row.Active = active.Value;
            if (canceledTime.HasValue) row.CanceledTime = canceledTime.Value;
            if (trial.HasValue) row.Trial = trial.Value;
            if (beta.HasValue) row.Beta = beta.Value;
            if (billingPlan >= 0) row.BillingPlan = (byte)billingPlan;
            if (creditCardStatus >= 0) row.CreditCardStatus = (byte)creditCardStatus;
            row.Deleted = false;

            using (InstanceTableAdapter adapter = new InstanceTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Update(row);
            }

            Instance instance = CreateInstance(row);

            if (nameIsModified)
                GroupProvider.UpdateInstanceAdministratorGroup(organizationId, instanceId, name);

            if (emailSuffixes != null)
                EmailSuffixProvider.UpdateEmailSuffixName(organizationId, instanceId, ref emailSuffixes);

            PutInstanceToCache(instance);

            if (raiseEvent)
                RaiseInstanceUpdated(instance);
        }

        /// <summary>
        /// Updates the details of specified instance.
        /// </summary>
        /// <param name="instance">The instance to update.</param>
        public static void UpdateInstance(Instance instance)
        {
            UpdateInstance(instance, true);
        }

        /// <summary>
        /// Updates the details of specified instance.
        /// </summary>
        /// <param name="instance">The instance to update.</param>
        /// <param name="raiseEvent">Whether the Micajah.Common.Bll.Providers.InstanceProvider.InstanceUpdated event will be raised.</param>
        public static void UpdateInstance(Instance instance, bool raiseEvent)
        {
            if (instance != null)
                UpdateInstance(instance.InstanceId, instance.Name, instance.Description, instance.EnableSignupUser, instance.ExternalId, instance.TimeZoneId, instance.TimeFormat, instance.DateFormat, instance.WorkingDays, instance.Active, instance.CanceledTime, instance.Trial, instance.Beta, null, instance.OrganizationId, raiseEvent, -1, -1);
        }

        /// <summary>
        /// Updates the details of specified instance.
        /// </summary>
        /// <param name="instance">The instance to update.</param>
        /// <param name="billingPlan"></param>
        public static void UpdateInstance(Instance instance, BillingPlan billingPlan)
        {
            if (instance != null)
                UpdateInstance(instance.InstanceId, instance.Name, instance.Description, instance.EnableSignupUser, instance.ExternalId, instance.TimeZoneId, instance.TimeFormat, instance.DateFormat, instance.WorkingDays, instance.Active, instance.CanceledTime, instance.Trial, instance.Beta, null, instance.OrganizationId, false, (int)billingPlan, -1);
        }

        /// <summary>
        /// Updates the details of specified instance.
        /// </summary>
        /// <param name="instance">The instance to update.</param>
        /// <param name="creditCardStatus"></param>
        public static void UpdateInstance(Instance instance, CreditCardStatus creditCardStatus)
        {
            if (instance != null)
                UpdateInstance(instance.InstanceId, instance.Name, instance.Description, instance.EnableSignupUser, instance.ExternalId, instance.TimeZoneId, instance.TimeFormat, instance.DateFormat, instance.WorkingDays, instance.Active, instance.CanceledTime, instance.Trial, instance.Beta, null, instance.OrganizationId, false, -1, (int)creditCardStatus);
        }

        /// <summary>
        /// Marks as deleted the specified instance.
        /// </summary>
        /// <param name="databaseId">Specifies the instance's identifier.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteInstance(Guid instanceId)
        {
            UserContext user = UserContext.Current;

            ClientDataSet.InstanceRow row = GetInstanceRow(user.OrganizationId, instanceId);
            if (row != null)
            {
                row.Deleted = true;

                using (InstanceTableAdapter adapter = new InstanceTableAdapter(OrganizationProvider.GetConnectionString(user.OrganizationId)))
                {
                    adapter.Update(row);
                }

                GroupProvider.DeleteInstanceAdministratorGroup(user.OrganizationId);

                EmailSuffixProvider.DeleteEmailSuffixes(user.OrganizationId, instanceId);

                RemoveInstanceFromCache(instanceId);
                SettingProvider.RemoveInstanceSettingsValuesFromCache(instanceId);
                CustomUrlProvider.RemoveInstanceCustomUrlFromCache(instanceId);
                ResourceProvider.RemoveInstanceLogoImageUrlFromCache(instanceId);
            }
        }

        /// <summary>
        /// Returns the true if the instance exists; otherwise, false.
        /// </summary>
        /// <param name="organizationName">The name of the organization.</param>
        /// <param name="instanceName">The name of the instance.</param>
        /// <returns>true if the instance exists; otherwise, false.</returns>
        public static bool InstanceExists(string organizationName, string instanceName)
        {
            return (GetInstanceIdByName(organizationName, instanceName) != Guid.Empty);
        }

        #endregion
    }

    /// <summary>
    /// The class containing the data for the events of Micajah.Common.Bll.Providers.InstanceProvider class.
    /// </summary>
    public class InstanceProviderEventArgs : EventArgs
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the template instance.
        /// </summary>
        public Instance TemplateInstance { get; set; }

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        public Instance Instance { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user that is the administrator of the instance.
        /// </summary>
        public Guid? UserId { get; set; }

        #endregion
    }
}
