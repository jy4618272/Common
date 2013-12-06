using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Dal.ClientDataSetTableAdapters;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with users.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class UserProvider
    {
        #region Members

        internal const int EmailMaxLength = 255;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when an user is inserted.
        /// </summary>
        public static event EventHandler<UserInsertedEventArgs> UserInserted;

        /// <summary>
        /// Occurs when an user is updated.
        /// </summary>
        public static event EventHandler<UserUpdatedEventArgs> UserUpdated;

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the users in specified organization and instance by specified roles.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="instanceId">The identifier of the organization's instance.</param>
        /// <param name="organizationAdministrator">true, if the users should be the organization administrator; otherwise, false.</param>
        /// <param name="active">true, if the users should be the active in the organization or instance; otherwise, false.</param>
        /// <param name="roles">The string that contains the list of the role indetifiers.</param>
        /// <returns>The Micajah.Common.Dal.ClientDataSet.UserDataTable object populated with information of the users.</returns>
        public static ClientDataSet.UserDataTable GetUsers(Guid organizationId, Guid instanceId, bool? organizationAdministrator, bool? active, string roles, string lowerRoles)
        {
            using (UserTableAdapter adapter = new UserTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetUsersByRoles(organizationId, ((instanceId == Guid.Empty) ? null : new Guid?(instanceId)), organizationAdministrator, active
                    , (string.IsNullOrEmpty(roles) ? null : roles.ToUpperInvariant())
                    , (string.IsNullOrEmpty(lowerRoles) ? null : lowerRoles.ToUpperInvariant()));
            }
        }

        private static ClientDataSet.UsersInstancesDataTable GetUsersInstances(Guid userId, Guid organizationId)
        {
            using (UsersInstancesTableAdapter adapter = new UsersInstancesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetUsersInstances(userId, organizationId);
            }
        }

        private static ClientDataSet.UsersGroupsDataTable GetUserGroups(Guid userId, Guid organizationId)
        {
            using (UsersGroupsTableAdapter adapter = new UsersGroupsTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetUsersGroups(userId, organizationId);
            }
        }

        private static ClientDataSet.UsersGroupsDataTable GetUsersGroupsByInstanceId(Guid organizationId, Guid instanceId, Guid userId)
        {
            using (UsersGroupsTableAdapter adapter = new UsersGroupsTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetUsersGroupsByInstanceId(userId, organizationId, instanceId);
            }
        }

        /// <summary>
        /// Returns the value indicating the user is the last organization administrator of the specified organization.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <returns>true, if the user is the last organization administrator of the specified organization; otherwise, false.</returns>
        private static bool UserIsLastAdministrator(Guid userId, Guid organizationId)
        {
            using (UserTableAdapter adapter = new UserTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                ClientDataSet.UserDataTable table = adapter.GetAnotherAdministrator(userId, organizationId);
                return (table.Count == 0);
            }
        }

        private static void InsertUserIntoOrganization(Guid loginId, string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , string timeZoneId, int? timeFormat, int? dateFormat
            , string groupId, Guid organizationId, bool organizationAdministrator, bool sendEmailNotification)
        {
            WebApplication.LoginProvider.ChangeLoginName(loginId, email, sendEmailNotification);

            bool orgAdmin = organizationAdministrator;
            if (groupId != null) orgAdmin = organizationAdministrator || string.Concat(",", groupId, ",").Contains("," + Guid.Empty.ToString() + ",");

            WebApplication.LoginProvider.AddLoginToOrganization(loginId, organizationId, orgAdmin);

            ClientDataSet.UserRow row = GetUserRow(loginId, organizationId);
            if (row == null)
            {
                using (UserTableAdapter adapter = new UserTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    adapter.Insert(loginId, (string.IsNullOrEmpty(email) ? string.Empty : email)
                        , ((firstName == null) ? string.Empty : firstName), ((lastName == null) ? string.Empty : lastName), ((middleName == null) ? string.Empty : middleName)
                       , ((phone == null) ? string.Empty : phone), ((mobilePhone == null) ? string.Empty : mobilePhone), ((fax == null) ? string.Empty : fax)
                       , ((title == null) ? string.Empty : title), ((department == null) ? string.Empty : department)
                       , ((street == null) ? string.Empty : street), ((street2 == null) ? string.Empty : street2), ((city == null) ? string.Empty : city)
                       , ((state == null) ? string.Empty : state), ((postalCode == null) ? string.Empty : postalCode), ((country == null) ? string.Empty : country)
                       , null, false, (string.IsNullOrEmpty(timeZoneId) ? null : timeZoneId), timeFormat, dateFormat);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(email)) row.Email = email;
                if (!string.IsNullOrEmpty(firstName)) row.FirstName = firstName;
                if (!string.IsNullOrEmpty(lastName)) row.LastName = lastName;
                if (!string.IsNullOrEmpty(middleName)) row.MiddleName = middleName;
                if (!string.IsNullOrEmpty(phone)) row.Phone = phone;
                if (!string.IsNullOrEmpty(mobilePhone)) row.MobilePhone = mobilePhone;
                if (!string.IsNullOrEmpty(fax)) row.Fax = fax;
                if (!string.IsNullOrEmpty(title)) row.Title = title;
                if (!string.IsNullOrEmpty(department)) row.Department = department;
                if (!string.IsNullOrEmpty(street)) row.Street = street;
                if (!string.IsNullOrEmpty(street2)) row.Street2 = street2;
                if (!string.IsNullOrEmpty(city)) row.City = city;
                if (!string.IsNullOrEmpty(state)) row.State = state;
                if (!string.IsNullOrEmpty(postalCode)) row.PostalCode = postalCode;
                if (!string.IsNullOrEmpty(country)) row.Country = country;
                if (!string.IsNullOrEmpty(timeZoneId)) row.TimeZoneId = timeZoneId;
                if (timeFormat.HasValue) row.TimeFormat = timeFormat.Value;
                if (dateFormat.HasValue) row.DateFormat = dateFormat.Value;

                using (UserTableAdapter adapter = new UserTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    adapter.Update(row);
                }
            }

            using (OrganizationsUsersTableAdapter adapter = new OrganizationsUsersTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Insert(organizationId, loginId, organizationAdministrator, true);
            }

            if (groupId != null)
                UsersGroupsAcceptChanges(loginId, email, groupId, false, organizationId, false);
        }

        private static void UpdateUser(Guid userId, string email, string groupId, bool addGroup, Guid organizationId, ClientDataSet.UserRow row, bool sendEmailNotification)
        {
            bool isOrganizationAdministratorModified = false;
            bool isOrganizationAdministrator = false;

            if (groupId != null)
            {
                isOrganizationAdministrator = string.Concat(",", groupId, ",").Contains("," + Guid.Empty.ToString() + ",");

                if (addGroup)
                {
                    isOrganizationAdministratorModified = isOrganizationAdministrator;
                }
                else
                {
                    if (row == null)
                        row = GetUserRow(userId, organizationId);

                    isOrganizationAdministratorModified = ((row.IsOrganizationAdministratorNull() ? false : row.OrganizationAdministrator) != isOrganizationAdministrator);

                    if (isOrganizationAdministratorModified && (!isOrganizationAdministrator))
                    {
                        if (UserIsLastAdministrator(userId, organizationId))
                            throw new DataException(Resources.UserProvider_ErrorMessage_YouMustBeOrganizationAdministrator);
                    }
                }
            }

            if (groupId != null)
            {
                if (string.IsNullOrEmpty(email))
                {
                    ClientDataSet.UserRow uRow = GetUserRow(userId, organizationId);
                    if (uRow != null)
                        email = uRow.Email;
                }

                UsersGroupsAcceptChanges(userId, email, groupId, addGroup, organizationId, sendEmailNotification);

                if (isOrganizationAdministratorModified)
                {
                    WebApplication.LoginProvider.UpdateLoginInOrganization(userId, organizationId, isOrganizationAdministrator, null);

                    UpdateUserInOrganization(userId, organizationId, isOrganizationAdministrator, null);
                }
            }
        }

        /// <summary>
        /// Updates the details of specified user in the specified organization.
        /// </summary>
        /// <param name="userId">Specifies the user's identifier.</param>
        /// <param name="email">Specifies the user's e-mail address.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="addGroup">Specifies the groups identifiers should be added only to the user's groups.</param>
        /// <param name="organizationId">Specifies the identifier of the organization.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        private static void UpdateUser(Guid userId, string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , string timeZoneId, int? timeFormat, int? dateFormat
            , string groupId, bool addGroup, Guid organizationId, bool sendEmailNotification)
        {
            ClientDataSet.UserRow row = GetUserRow(userId, organizationId);
            if (row != null)
            {
                WebApplication.LoginProvider.ChangeLoginName(userId, email, sendEmailNotification);
                WebApplication.LoginProvider.UpdateLogin(userId, null, null, firstName, lastName);

                using (UserTableAdapter adapter = new UserTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    adapter.Update(userId, email, firstName, lastName, middleName, phone, mobilePhone, fax, title, department, street, street2, city, state, postalCode, country, null, false, timeZoneId, timeFormat, dateFormat);
                }

                UpdateUser(userId, email, groupId, addGroup, organizationId, row, sendEmailNotification);
            }
        }

        private static void UpdateUserInOrganization(Guid userId, Guid organizationId, bool? isOrganizationAdministrator, bool? active)
        {
            using (OrganizationsUsersTableAdapter adapter = new OrganizationsUsersTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Update(organizationId, userId, isOrganizationAdministrator, active);
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Adds the specified user to the specified organization. If the user is not found creates new user.
        /// Refreshes all cached data by application after the action has been performed.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="addGroup">Specifies the groups identifiers should be added only to the user's groups.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="organizationAdministrator">true, if the user is organization administrator; otherwise, false.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        /// <param name="refreshAllData">true to refresh all cached data by application.</param>
        /// <param name="minRequiredPasswordLength">The minimum length required for a password.</param>
        /// <param name="minRequiredNonAlphanumericCharacters">The minimum number of special characters that must be present in a password.</param>
        /// <param name="password">The generated password for a newly created user or a null reference, if the user exists.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        internal static Guid AddUserToOrganization(string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , string timeZoneId, int? timeFormat, int? dateFormat
            , string groupId, bool addGroup, Guid organizationId, bool organizationAdministrator
            , bool sendEmailNotification
            , int minRequiredPasswordLength, int minRequiredNonAlphanumericCharacters
            , out string password)
        {
            password = null;
            Guid loginId = Guid.Empty;
            DataRowView drv = WebApplication.LoginProvider.GetLogin(email);
            if (drv != null)
            {
                loginId = (Guid)drv["LoginId"];
                if (WebApplication.LoginProvider.LoginInOrganization(loginId, organizationId))
                {
                    UpdateUser(loginId, email, firstName, lastName, middleName
                        , phone, mobilePhone, fax, title, department
                        , street, street2, city, state, postalCode, country
                        , timeZoneId, timeFormat, dateFormat
                        , groupId, addGroup, organizationId, sendEmailNotification);
                }
                else
                {
                    WebApplication.LoginProvider.UpdateLogin(loginId, null, null, firstName, lastName);

                    InsertUserIntoOrganization(loginId, email, firstName, lastName, middleName
                        , phone, mobilePhone, fax, title, department
                        , street, street2, city, state, postalCode, country
                        , timeZoneId, timeFormat, dateFormat
                        , groupId, organizationId, organizationAdministrator, false);

                    if (sendEmailNotification)
                        SendUserEmail(email, string.Empty, organizationId, groupId, false, false, true);
                }
            }
            else
            {
                loginId = InsertUser(email, firstName, lastName, middleName
                      , phone, mobilePhone, fax, title, department
                      , street, street2, city, state, postalCode, country
                      , timeZoneId, timeFormat, dateFormat
                      , groupId, organizationId, organizationAdministrator, sendEmailNotification
                      , minRequiredPasswordLength, minRequiredNonAlphanumericCharacters, out password);
            }

            return loginId;
        }

        /// <summary>
        /// Returns the string that contains the instances identifiers list in which the specified user is active.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <returns>The string that contains the instances identifiers list in which the specified user is active.</returns>
        internal static ArrayList GetInstanceIdListWhereUserIsInactive(Guid userId, Guid organizationId)
        {
            ArrayList list = new ArrayList();

            foreach (ClientDataSet.UsersInstancesRow row in GetUsersInstances(userId, organizationId))
            {
                if (!row.Active)
                    list.Add(row.InstanceId);
            }

            if (!WebApplication.LoginProvider.LoginIsActiveInOrganization(userId, organizationId))
                list.Add(Guid.Empty);

            return list;
        }

        internal static void GetUserGroupsAndRoles(Guid organizationId, Guid instanceId, Guid userId, ref ArrayList groupIdList, ref ArrayList roleIdList)
        {
            foreach (ClientDataSet.UsersGroupsRow row in GetUsersGroupsByInstanceId(organizationId, instanceId, userId))
            {
                if (!groupIdList.Contains(row.GroupId))
                    groupIdList.Add(row.GroupId);

                if (!roleIdList.Contains(row.RoleId))
                {
                    if (RoleProvider.GetRoleRow(row.RoleId) != null)
                        roleIdList.Add(row.RoleId);
                }
            }
        }

        internal static ArrayList GetUserGroupIdList(Guid organizationId, Guid userId, bool organizationAdministrator)
        {
            ArrayList list = new ArrayList();

            foreach (ClientDataSet.UsersGroupsRow row in GetUserGroups(userId, organizationId))
            {
                list.Add(row.GroupId);
            }

            if (organizationAdministrator)
                list.Add(Guid.Empty);

            return list;
        }

        internal static bool UserIsInstanceAdministrator(Guid organizationId, Guid instanceId, Guid userId)
        {
            foreach (ClientDataSet.UsersGroupsRow row in GetUsersGroupsByInstanceId(organizationId, instanceId, userId))
            {
                if (row.RoleId == RoleProvider.InstanceAdministratorRoleId)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Modifies the relations between specified user and groups in the specified organization.
        /// </summary>
        /// <param name="userId">Specifies the identifier of the user.</param>
        /// <param name="groupId">Specifies the identifiers of the groups, separated by comma.</param>
        /// <param name="addGroup">Specifies the groups identifiers should be added only to the user's groups.</param>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <param name="adapters">The tables adapters to update database.</param>
        internal static void UsersGroupsAcceptChanges(Guid userId, string email, string groupId, bool addGroup, Guid organizationId, bool sendEmailNotification)
        {
            ClientDataSet.UsersGroupsDataTable table = null;
            try
            {
                table = GetUserGroups(userId, organizationId);

                List<Guid> groupIdList = new List<Guid>();
                if (groupId == null) groupId = string.Empty;
                groupId = string.Concat(",", groupId, ",").Replace("," + Guid.Empty.ToString() + ",", ",");

                foreach (ClientDataSet.UsersGroupsRow row in table)
                {
                    string currentGroupId = string.Concat(",", row.GroupId, ",");
                    if (groupId.Contains(currentGroupId))
                    {
                        groupId = groupId.Replace(currentGroupId, ",,");
                        if (!groupIdList.Contains(row.GroupId))
                            groupIdList.Add(row.GroupId);
                    }
                    else if (!addGroup)
                        row.Delete();
                }

                if (groupId.Replace(",", string.Empty).Length > 0)
                {
                    foreach (string currentGroupId in groupId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        Guid gid = (Guid)Support.ConvertStringToType(currentGroupId, typeof(Guid));

                        if (table.FindByUserIdGroupId(userId, gid) == null)
                        {
                            ClientDataSet.UsersGroupsRow row = table.NewUsersGroupsRow();
                            row.UserId = userId;
                            row.GroupId = gid;
                            table.AddUsersGroupsRow(row);

                            if (!groupIdList.Contains(row.GroupId))
                                groupIdList.Add(row.GroupId);
                        }
                    }
                }

                int rowsCount = -1;
                using (UsersGroupsTableAdapter adapter = new UsersGroupsTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                {
                    rowsCount = adapter.Update(table);
                }

                if (sendEmailNotification && (rowsCount > 0))
                    SendUserEmail(email, null, organizationId, Support.ConvertListToString(groupIdList), false, false, false);
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        internal static void UpdateLastLoginDate(ClientDataSet.UserRow row, Guid organizationId)
        {
            row.LastLoginDate = DateTime.UtcNow;

            using (UserTableAdapter adapter = new UserTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Update(row);
            }
        }

        /// <summary>
        /// Raises the UserInserted event.
        /// </summary>
        /// <param name="userId">The unique identifier of newly created user.</param>
        /// <param name="organizationId">The unique identifier of the organization whereto the user is added.</param>
        /// <param name="instanceId">The unique identifier of the instance whereto the user is added.</param>
        /// <param name="groupIdList">The list of the unique identifier of the groups in which the user is added.</param>
        internal static void RaiseUserInserted(Guid userId, Guid? organizationId, Guid? instanceId, ICollection<Guid> groupIdList)
        {
            if (UserInserted != null)
                UserInserted(null, new UserInsertedEventArgs(userId, organizationId, instanceId, groupIdList));
        }

        /// <summary>
        /// Raises the UserUpdated event.
        /// </summary>
        /// <param name="userId">The unique identifier of user.</param>
        /// <param name="organizationId">The unique identifier of the organization whereto the user is added.</param>
        /// <param name="groupIdList">The list of the unique identifier of the user groups.</param>
        internal static void RaiseUserUpdated(Guid userId, Guid? organizationId, ICollection<Guid> groupIdList)
        {
            if (UserUpdated != null)
                UserUpdated(null, new UserUpdatedEventArgs(userId, organizationId, groupIdList));
        }

        /// <summary>
        /// Raises the UserUpdated event.
        /// </summary>
        /// <param name="userId">The unique identifier of user.</param>
        /// <param name="organizationId">The unique identifier of the organization whereto the user is added.</param>
        /// <param name="groupIdList">The list of the unique identifier of the user groups.</param>
        /// <param name="email">Email of the user, in case it was changed</param>
        internal static void RaiseUserUpdated(Guid userId, Guid? organizationId, ICollection<Guid> groupIdList, string email)
        {
            if (UserUpdated != null)
                UserUpdated(null, new UserUpdatedEventArgs(userId, organizationId, groupIdList, email));
        }

        /// <summary>
        /// Sends an e-mail notification the the user with specified e-mail when login or email address is changed.
        /// </summary>
        /// <param name="email">The e-mail address of the user.</param>
        /// <param name="organizationId">The identifier of the organization that the user is associated to.</param>
        /// <param name="modifiedByEmail">The email of the person, which modified the login.</param>
        /// <returns>true, if the e-mail was sent successfully; otherwise, false.</returns>
        internal static bool SendChangeLoginEmail(string email, Guid organizationId, string modifiedByEmail)
        {
            Organization org = null;
            bool enableNotification = FrameworkConfiguration.Current.WebApplication.Email.EnableChangeLoginNotification;

            if (organizationId != Guid.Empty)
            {
                org = OrganizationProvider.GetOrganizationFromCache(organizationId);
                if (org == null)
                    org = OrganizationProvider.GetOrganization(organizationId);

                if (org != null)
                {
                    EmailElement settings = SettingProvider.GetOrganizationEmailSettingsFromCache(organizationId);
                    enableNotification = settings.EnableChangeLoginNotification;
                }
            }

            if (!enableNotification)
                return false;

            string subject = string.Empty;
            StringBuilder body = new StringBuilder();

            body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YourLoginOfAccountHasBeenChanged, FrameworkConfiguration.Current.WebApplication.Name);

            subject = body.ToString().TrimEnd('.');

            body.AppendLine();
            body.AppendLine();
            body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YourNewLogin, email);
            body.AppendLine();
            body.AppendLine();
            body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_LoginLink, WebApplication.LoginProvider.GetLoginUrl(email, null, Guid.Empty, Guid.Empty, null, CustomUrlProvider.ApplicationUri));

            return Support.SendEmail(FrameworkConfiguration.Current.WebApplication.Support.Email, modifiedByEmail, email, null, subject, body.ToString(), false
                , new EmailSendingEventArgs()
                {
                    Reason = EmailSendingReason.ChangeLogin,
                    Organization = org
                });
        }

        /// <summary>
        /// Sends an e-mail notification the the user with specified e-mail when password is changed.
        /// </summary>
        /// <param name="email">The e-mail address of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="organizationId">The identifier of the organization that the user is associated to.</param>
        /// <param name="modifiedByEmail">The email of the person, which modified the password.</param>
        /// <returns>true, if the e-mail was sent successfully; otherwise, false.</returns>
        internal static bool SendChangePasswordEmail(string email, string password, Guid organizationId, string modifiedByEmail)
        {
            Organization org = null;

            bool enableNotification = FrameworkConfiguration.Current.WebApplication.Email.EnableChangeLoginNotification;

            if (organizationId != Guid.Empty)
            {
                org = OrganizationProvider.GetOrganizationFromCache(organizationId);
                if (org == null)
                    org = OrganizationProvider.GetOrganization(organizationId);

                if (org != null)
                {
                    EmailElement settings = SettingProvider.GetOrganizationEmailSettingsFromCache(organizationId);
                    enableNotification = settings.EnableChangeLoginNotification;
                }
            }

            if (!enableNotification)
                return false;

            string subject = string.Empty;
            StringBuilder body = new StringBuilder();

            body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YourAccountPasswordHasBeenChanged, FrameworkConfiguration.Current.WebApplication.Name);

            subject = body.ToString().TrimEnd('.');

            body.AppendLine();
            body.AppendLine();
            body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YourLogin, email);
            body.AppendLine();
            body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YourNewPassword, password);
            body.AppendLine();
            body.AppendLine();
            body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_LoginLink, WebApplication.LoginProvider.GetLoginUrl(email, null, Guid.Empty, Guid.Empty, null, CustomUrlProvider.ApplicationUri));

            return Support.SendEmail(FrameworkConfiguration.Current.WebApplication.Support.Email, modifiedByEmail, email, null, subject, body.ToString(), false
                , new EmailSendingEventArgs()
                {
                    Reason = EmailSendingReason.ChangePassword,
                    Organization = org
                });
        }

        /// <summary>
        /// Sends an e-mail notification to the specifed user.
        /// </summary>
        /// <param name="email">The e-mail address of the user.</param>
        /// <param name="password">The password of the new user, if the user exists pass null or empty string.</param>
        /// <param name="organizationId">The identifier of the organization which the user was created in.</param>
        /// <returns>true, if the e-mail was sent successfully; otherwise, false.</returns>
        internal static bool SendUserEmail(string email, string password, Guid organizationId, string groupId, bool newUser, bool newOrg, bool newUserInOrg)
        {
            bool enableNotification = false;
            EmailSendingReason reason = EmailSendingReason.Undefined;

            Organization org = OrganizationProvider.GetOrganizationFromCache(organizationId);
            if (org == null)
                org = OrganizationProvider.GetOrganization(organizationId);

            if (newUser)
            {
                reason = EmailSendingReason.CreateNewLogin;
                enableNotification = FrameworkConfiguration.Current.WebApplication.Email.EnableCreateNewLoginNotification;
                if (org != null)
                {
                    EmailElement settings = SettingProvider.GetOrganizationEmailSettingsFromCache(organizationId);
                    enableNotification = settings.EnableCreateNewLoginNotification;
                }
            }
            else
            {
                reason = EmailSendingReason.ChangeLogin;
                enableNotification = FrameworkConfiguration.Current.WebApplication.Email.EnableChangeLoginNotification;
                if (org != null)
                {
                    EmailElement settings = SettingProvider.GetOrganizationEmailSettingsFromCache(organizationId);
                    enableNotification = settings.EnableChangeLoginNotification;
                }
            }

            if (!enableNotification)
                return false;

            string modifiedByEmail = null;
            if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
            {
                if (!CustomUrlProvider.IsDefaultVanityUrl(HttpContext.Current))
                {
                    UserContext user = UserContext.Current;
                    if (user != null)
                        modifiedByEmail = user.Email;
                }
            }

            string subject = string.Empty;
            string bcc = null;
            StringBuilder body = new StringBuilder();

            if (org != null)
            {
                if (newOrg)
                {
                    bcc = FrameworkConfiguration.Current.WebApplication.Email.SalesTeam;
                    reason = EmailSendingReason.SignupOrganization;
                }

                if (newUser || newOrg || newUserInOrg)
                    body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YourAccountForOrganizationWasCreated, FrameworkConfiguration.Current.WebApplication.Name, org.Name);
                else
                    body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YourAccountForOrganizationWasUpdated, FrameworkConfiguration.Current.WebApplication.Name, org.Name);

                subject = body.ToString().TrimEnd('.');

                if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances && (!string.IsNullOrEmpty(groupId)))
                {
                    List<Guid> instanceIdList = InstanceProvider.GetGroupsInstances(groupId, organizationId);
                    if (instanceIdList.Count > 0)
                    {
                        body.AppendLine();
                        body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YouAreAssociatedToTheFollowingInstances, Support.ConvertListToString(InstanceProvider.GetInstanceNames(instanceIdList, organizationId), ", "));
                        body.AppendLine();
                        body.AppendLine();
                        body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YourLogin, email);
                        body.AppendLine();
                        if (newUser && (!string.IsNullOrEmpty(password)))
                        {
                            body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YourPassword, password);
                            body.AppendLine();
                        }
                        body.AppendLine();
                        body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_LoginLink, WebApplication.LoginProvider.GetLoginUrl(email, null, organizationId, Guid.Empty, null, CustomUrlProvider.ApplicationUri));

                        return Support.SendEmail(FrameworkConfiguration.Current.WebApplication.Support.Email, modifiedByEmail, email, bcc, subject, body.ToString(), false
                            , new EmailSendingEventArgs()
                            {
                                Reason = reason,
                                Organization = org
                            });
                    }
                }

                body.AppendLine();
                body.AppendLine();
                body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YourLogin, email);
                body.AppendLine();
                if (newUser)
                {
                    body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YourPassword, password);
                    body.AppendLine();
                }
                body.AppendLine();
                body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_LoginLink, WebApplication.LoginProvider.GetLoginUrl(email, null, organizationId, Guid.Empty, null, CustomUrlProvider.ApplicationUri));
            }
            else
            {
                body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YourAccountWasCreated, FrameworkConfiguration.Current.WebApplication.Name);

                subject = body.ToString().TrimEnd('.');

                body.AppendLine();
                body.AppendLine();
                body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YourLogin, email);
                body.AppendLine();
                body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_YourPassword, password);
                body.AppendLine();
                body.AppendLine();
                body.AppendFormat(CultureInfo.InvariantCulture, Resources.EmailNotification_LoginLink, WebApplication.LoginProvider.GetLoginUrl(email, null, Guid.Empty, Guid.Empty, null, CustomUrlProvider.ApplicationUri));
            }

            return Support.SendEmail(FrameworkConfiguration.Current.WebApplication.Support.Email, modifiedByEmail, email, bcc, subject, body.ToString(), false
                , new EmailSendingEventArgs()
                {
                    Reason = reason,
                    Organization = org
                });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the users, excluding marked as deleted.
        /// </summary>
        /// <returns>The table pupulated with information of the users.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.UserDataTable GetUsers()
        {
            return GetUsers(UserContext.Current.OrganizationId);
        }

        /// <summary>
        /// Gets the users of the specified organization, excluding marked as deleted.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <returns>The table pupulated with information of the users.</returns>
        public static ClientDataSet.UserDataTable GetUsers(Guid organizationId)
        {
            using (UserTableAdapter adapter = new UserTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                return adapter.GetUsers(organizationId);
            }
        }

        /// <summary>
        /// Gets the active users in specified organization by specified roles.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="roleShortName">The short names of the roles.</param>
        /// <returns>The Micajah.Common.Dal.ClientDataSet.UserDataTable object populated with information of the users.</returns>
        public static ClientDataSet.UserDataTable GetUsers(Guid organizationId, params string[] roleShortName)
        {
            return GetUsers(organizationId, Guid.Empty, roleShortName);
        }

        /// <summary>
        /// Gets the active users in specified organization and instance by specified roles.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="instanceId">The identifier of the organization's instance.</param>
        /// <param name="roleShortName">The short names of the roles.</param>
        /// <returns>The Micajah.Common.Dal.ClientDataSet.UserDataTable object populated with information of the users.</returns>
        public static ClientDataSet.UserDataTable GetUsers(Guid organizationId, Guid instanceId, params string[] roleShortName)
        {
            return GetUsers(organizationId, instanceId, new bool?(), true, ((roleShortName == null) ? null : Support.ConvertListToString(RoleProvider.GetRoleIdListByShortNames(roleShortName))), null);
        }

        public static ClientDataSet.UserDataTable GetUsers(Guid organizationId, Guid instanceId, Guid roleId)
        {
            if (roleId == RoleProvider.OrganizationAdministratorRoleId)
                return GetUsers(organizationId, Guid.Empty, true, true, null, null);
            else if (roleId == RoleProvider.InstanceAdministratorRoleId)
                return GetUsers(organizationId, instanceId, new bool?(), true, roleId.ToString(), null);
            else
                return GetUsers(organizationId, instanceId, new bool?(), true, roleId.ToString(), Support.ConvertListToString(RoleProvider.GetLowerNonBuiltInRoleIdList(roleId)));
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetUsersDataView(Guid organizationId, Guid instanceId, Guid roleId)
        {
            return GetUsers(organizationId, instanceId, roleId).DefaultView;
        }

        /// <summary>
        /// Returns the list of the groups wich have the specified user in the specified instance.
        /// </summary>
        /// <param name="organizationId">The organizations' identifier.</param>
        /// <param name="instanceId">The instance's identifier.</param>
        /// <param name="userId">The user's identifier.</param>
        /// <returns>The list of the groups.</returns>
        public static ArrayList GetUserGroupIdList(Guid organizationId, Guid instanceId, Guid userId)
        {
            ArrayList list = new ArrayList();

            foreach (ClientDataSet.UsersGroupsRow row in GetUsersGroupsByInstanceId(organizationId, instanceId, userId))
            {
                list.Add(row.GroupId);
            }

            return list;
        }

        /// <summary>
        /// Returns a collection that contains the groups identifiers list which the specified user belong to in the specified organization.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <param name="organizationId">The organization's identifier.</param>
        /// <returns>The collection that contains the groups identifiers list which the specified user belong to in the specified organization.</returns>
        public static ArrayList GetUserGroupIdList(Guid organizationId, Guid userId)
        {
            return GetUserGroupIdList(organizationId, userId, WebApplication.LoginProvider.LoginIsOrganizationAdministrator(userId, organizationId));
        }

        /// <summary>
        /// Gets an object populated with information of the specified user.
        /// </summary>
        /// <param name="userId">Specifies the user identifier to get user information.</param>
        /// <returns>
        /// The object populated with information of the specified user. 
        /// If the user is not found, the method returns null reference.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.UserRow GetUserRow(Guid userId)
        {
            return GetUserRow(userId, true);
        }

        /// <summary>
        /// Gets an object populated with information of the specified user.
        /// </summary>
        /// <param name="userId">Specifies the user identifier to get user information.</param>
        /// <returns>
        /// The object populated with information of the specified user. 
        /// If the user is not found, the method returns null reference.
        /// </returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static ClientDataSet.UserRow GetUserRowWithSecondaryEmails(Guid userId)
        {
            ClientDataSet.UserRow dr = GetUserRow(userId, true);
            if (dr != null)
            {
                string secondaryEmail = string.Empty;
                DataTable dt = EmailProvider.GetEmails(userId);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (secondaryEmail.Length > 0)
                            secondaryEmail += ",";
                        secondaryEmail += dt.Rows[i]["Email"];
                    }
                }

                dr.SecondaryEmails = secondaryEmail;
            }
            return dr;
        }

        /// <summary>
        /// Gets an object populated with information of the specified user.
        /// </summary>
        /// <param name="userId">Specifies the user identifier to get user information.</param>
        /// <param name="includeGroups">The flag indicating that the additional information of groups is included in result.</param>
        /// <returns>The object populated with information of the specified user. If the user is not found, the method returns null reference.</returns>
        public static ClientDataSet.UserRow GetUserRow(Guid userId, bool includeGroups)
        {
            return GetUserRow(userId, UserContext.Current.OrganizationId, includeGroups);
        }

        /// <summary>
        /// Gets an object populated with information of the specified user in specified organization.
        /// </summary>
        /// <param name="userId">Specifies the user identifier to get user information.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <returns>The object populated with information of the specified user. If the user is not found, the method returns null reference.</returns>
        public static ClientDataSet.UserRow GetUserRow(Guid userId, Guid organizationId)
        {
            return GetUserRow(userId, organizationId, false);
        }

        /// <summary>
        /// Gets an object populated with information of the specified user in specified organization.
        /// </summary>
        /// <param name="userId">Specifies the user identifier to get user information.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="includeGroups">The flag indicating that the additional information of groups is included in result.</param>
        /// <returns>The object populated with information of the specified user. If the user is not found, the method returns null reference.</returns>
        public static ClientDataSet.UserRow GetUserRow(Guid userId, Guid organizationId, bool includeGroups)
        {
            if (userId == Guid.Empty) return null;

            ClientDataSet.UserRow row = null;

            using (UserTableAdapter adapter = new UserTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                ClientDataSet.UserDataTable table = adapter.GetUser(userId, organizationId);
                row = ((table.Count > 0) ? table[0] : null);
            }

            if (row != null)
            {
                if (includeGroups)
                {
                    ArrayList groupIdList = GetUserGroupIdList(organizationId, userId);
                    row.GroupId = Support.ConvertListToString(groupIdList);
                }
            }

            return row;
        }

        /// <summary>
        /// Gets an object populated with information of the specified user from specified database.
        /// </summary>
        /// <param name="userId">Specifies the user identifier to get user information.</param>
        /// <param name="connectionString">Specifies the connection string to the database to get information from.</param>
        /// <returns>The object populated with information of the specified user from specified database. If the user is not found, the method returns null reference.</returns>
        public static ClientDataSet.UserRow GetUserRowFromDatabase(Guid userId, string connectionString)
        {
            using (UserTableAdapter adapter = new UserTableAdapter(connectionString))
            {
                ClientDataSet.UserDataTable table = adapter.GetUser(userId, null);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        /// <summary>
        /// Gets an object populated with information of the specified user from specified database.
        /// </summary>
        /// <param name="email">Specifies the e-mail address of the user to get the user information.</param>
        /// <param name="connectionString">Specifies the connection string to the database to get information from.</param>
        /// <returns>The object populated with information of the specified user from specified database. If the user is not found, the method returns null reference.</returns>
        public static ClientDataSet.UserRow GetUserRowFromDatabase(string email, string connectionString)
        {
            using (UserTableAdapter adapter = new UserTableAdapter(connectionString))
            {
                ClientDataSet.UserDataTable table = adapter.GetUserByEmail(email, null);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        /// <summary>
        /// Gets an object populated with information of the specified user from specified database.
        /// </summary>
        /// <param name="email">Specifies the e-mail address of the user to get the user information.</param>
        /// <param name="databaseId">Specifies the database identifier to get information from.</param>
        /// <returns>The object populated with information of the specified user from specified database. If the user is not found, the method returns null reference.</returns>
        public static ClientDataSet.UserRow GetUserRowFromDatabase(string email, Guid databaseId)
        {
            if (!string.IsNullOrEmpty(email))
                return GetUserRowFromDatabase(email, DatabaseProvider.GetConnectionString(databaseId));
            return null;
        }

        /// <summary>
        /// Gets an object populated with information of the specified user from first of the organizations that the user is associated to.
        /// </summary>
        /// <param name="email">Specifies the e-mail address of the user to get the user information.</param>
        /// <returns>The object populated with information of the specified user. If the user is not found, the method returns null reference.</returns>
        public static ClientDataSet.UserRow GetUserRow(string email)
        {
            ClientDataSet.UserRow row = null;
            if (!string.IsNullOrEmpty(email))
            {
                foreach (Organization org in WebApplication.LoginProvider.GetOrganizationsByLoginName(email))
                {
                    row = GetUserRow(email, org.OrganizationId);
                    if (row != null)
                        break;
                }
            }
            return row;
        }

        /// <summary>
        /// Gets an object populated with information of the specified user in the specified organization.
        /// </summary>
        /// <param name="email">Specifies the e-mail address of the user to get the user information.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <returns>The object populated with information of the specified user. If the user is not found, the method returns null reference.</returns>
        public static ClientDataSet.UserRow GetUserRow(string email, Guid organizationId)
        {
            using (UserTableAdapter adapter = new UserTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                ClientDataSet.UserDataTable table = adapter.GetUserByEmail(email, organizationId);
                return ((table.Count > 0) ? table[0] : null);
            }
        }

        /// <summary>
        /// Creates new user with specified details, adds the newly created user to user's selected organization and 
        /// sends the e-mail notification.
        /// </summary>
        /// <param name="email">Specifies the user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <returns>The identifier of the newly created user.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid InsertUser(string email, string firstName, string lastName, string middleName, string groupId)
        {
            return InsertUser(email, firstName, lastName, middleName, groupId, UserContext.Current.OrganizationId);
        }

        /// <summary>
        /// Creates new user with specified details, adds the newly created user to specified organization and 
        /// sends the e-mail notification.
        /// </summary>
        /// <param name="email">Specifies the user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <returns>The identifier of the newly created user.</returns>
        public static Guid InsertUser(string email, string firstName, string lastName, string middleName
            , string groupId, Guid organizationId)
        {
            string password = null;
            return InsertUser(email, firstName, lastName, middleName, groupId, organizationId, false, true, 0, 0, out password);
        }

        /// <summary>
        /// Creates new user with specified details, adds the newly created user to specified organization and 
        /// sends the e-mail notification.
        /// </summary>
        /// <param name="email">Specifies the user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>The identifier of the newly created user.</returns>
        public static Guid InsertUser(string email, string firstName, string lastName, string middleName
            , string groupId, Guid organizationId, string password)
        {
            return InsertUser(email, firstName, lastName, middleName, groupId, organizationId, password, true);
        }

        /// <summary>
        /// Creates new user with specified details, adds the newly created user to specified organization and 
        /// sends the e-mail notification.
        /// </summary>
        /// <param name="email">Specifies the user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="validatePassword">true to validate the password; otherwise, false.</param>
        /// <returns>The identifier of the newly created user.</returns>
        public static Guid InsertUser(string email, string firstName, string lastName, string middleName
            , string groupId, Guid organizationId, string password, bool validatePassword)
        {
            return InsertUser(email, firstName, lastName, middleName
                , string.Empty, string.Empty, string.Empty, string.Empty, string.Empty
                , string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty
                , groupId, organizationId
                , password, validatePassword, true);
        }

        /// <summary>
        /// Creates new user with specified details, adds the newly created user to specified organization.
        /// </summary>
        /// <param name="email">Specifies the user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">Specifies the identifier of the organization.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="validatePassword">true to validate the password; otherwise, false.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        /// <returns>The identifier of the newly created user.</returns>
        public static Guid InsertUser(string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , string groupId, Guid organizationId
            , string password, bool validatePassword, bool sendEmailNotification)
        {
            return InsertUser(email, email, firstName, lastName, middleName
                , phone, mobilePhone, fax, title, department
                , street, street2, city, state, postalCode, country
                , groupId, organizationId
                , password, validatePassword, sendEmailNotification);
        }

        /// <summary>
        /// Creates new user with specified details, adds the newly created user to specified organization.
        /// </summary>
        /// <param name="loginName">Specifies the user's login name.</param>
        /// <param name="email">Specifies the user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">Specifies the identifier of the organization.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="validatePassword">true to validate the password; otherwise, false.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        /// <returns>The identifier of the newly created user.</returns>
        public static Guid InsertUser(string loginName, string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , string groupId, Guid organizationId
            , string password, bool validatePassword, bool sendEmailNotification)
        {
            if (validatePassword) WebApplication.LoginProvider.ValidatePassword(password);

            Guid loginId = Guid.Empty;
            DataRow dr = WebApplication.LoginProvider.CreateLogin(loginName, password, firstName, lastName);
            if (dr != null)
            {
                loginId = (Guid)dr["LoginId"];

                InsertUserIntoOrganization(loginId, email, firstName, lastName, middleName
                    , phone, mobilePhone, fax, title, department
                    , street, street2, city, state, postalCode, country
                    , null, null, null
                    , groupId, organizationId, false, false);

                if (sendEmailNotification)
                    SendUserEmail(email, password, organizationId, groupId, true, false, true);
            }
            return loginId;
        }

        /// <summary>
        /// Creates new user with specified details, adds the newly created user to specified organization and 
        /// sends the e-mail notification.
        /// </summary>
        /// <param name="email">Specifies the user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="organizationAdministrator">true, if the user is organization administrator; otherwise, false.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        /// <param name="minRequiredPasswordLength">The minimum length required for a password.</param>
        /// <param name="minRequiredNonAlphanumericCharacters">The minimum number of special characters that must be present in a password.</param>
        /// <param name="password">The generated password for a newly created user.</param>
        /// <returns>The identifier of the newly created user.</returns>
        public static Guid InsertUser(string email, string firstName, string lastName, string middleName
            , string groupId, Guid organizationId, bool organizationAdministrator
            , bool sendEmailNotification
            , int minRequiredPasswordLength, int minRequiredNonAlphanumericCharacters, out string password)
        {
            return InsertUser(email, firstName, lastName, middleName
                , null, null, null, null, null
                , null, null, null, null, null, null
                , null, null, null
                , groupId, organizationId, organizationAdministrator
                , sendEmailNotification
                , minRequiredPasswordLength, minRequiredNonAlphanumericCharacters, out password);
        }

        /// <summary>
        /// Creates new user with specified details, adds the newly created user to specified organization and 
        /// sends the e-mail notification.
        /// </summary>
        /// <param name="email">Specifies the user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="organizationAdministrator">true, if the user is organization administrator; otherwise, false.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        /// <param name="minRequiredPasswordLength">The minimum length required for a password.</param>
        /// <param name="minRequiredNonAlphanumericCharacters">The minimum number of special characters that must be present in a password.</param>
        /// <param name="password">The generated password for a newly created user.</param>
        /// <returns>The identifier of the newly created user.</returns>
        public static Guid InsertUser(string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , string timeZoneId, int? timeFormat, int? dateFormat
            , string groupId, Guid organizationId, bool organizationAdministrator
            , bool sendEmailNotification
            , int minRequiredPasswordLength, int minRequiredNonAlphanumericCharacters, out string password)
        {
            if (minRequiredPasswordLength > 0)
                password = WebApplication.LoginProvider.GeneratePassword(minRequiredPasswordLength, minRequiredNonAlphanumericCharacters);
            else
                password = WebApplication.LoginProvider.GeneratePassword();

            Guid loginId = Guid.Empty;
            DataRow dr = WebApplication.LoginProvider.CreateLogin(email, password, firstName, lastName);
            if (dr != null)
            {
                loginId = (Guid)dr["LoginId"];

                InsertUserIntoOrganization(loginId, email, firstName, lastName, middleName
                    , phone, mobilePhone, fax, title, department
                    , street, street2, city, state, postalCode, country
                    , timeZoneId, timeFormat, dateFormat
                    , groupId, organizationId, organizationAdministrator, false);

                if (sendEmailNotification)
                    SendUserEmail(email, password, organizationId, groupId, true, false, true);
            }
            return loginId;
        }

        /// <summary>
        /// Adds the specified user to current user's selected organization and sends the e-mail notification.
        /// If the user is not found creates new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        public static Guid AddUserToOrganization(string email, string groupId)
        {
            return AddUserToOrganization(email, groupId, UserContext.Current.OrganizationId);
        }

        /// <summary>
        /// Adds the specified user to the specified organization and sends the e-mail notification.
        /// If the user is not found creates new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        public static Guid AddUserToOrganization(string email, string groupId, Guid organizationId)
        {
            return AddUserToOrganization(email, groupId, organizationId, true);
        }

        /// <summary>
        /// Adds the specified user to the specified organization. If the user is not found creates new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        public static Guid AddUserToOrganization(string email, string groupId, Guid organizationId, bool sendEmailNotification)
        {
            string password = null;
            return AddUserToOrganization(email, null, null, null, groupId, organizationId, sendEmailNotification, 0, 0, out password);
        }

        /// <summary>
        /// Adds the specified user to the current organization and sends the e-mail notification.
        /// If the user is not found creates new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid AddUserToOrganization(string email, string firstName, string lastName, string middleName, string groupId)
        {
            return AddUserToOrganization(email, firstName, lastName, middleName, groupId, UserContext.Current.OrganizationId);
        }

        /// <summary>
        /// Adds the specified user to the current organization and sends the e-mail notification.
        /// If the user is not found creates new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid AddUserToOrganization(string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , string timeZoneId, int? timeFormat, int? dateFormat
            , string groupId)
        {
            string password = null;
            return AddUserToOrganization(email, firstName, lastName, middleName
                 , phone, mobilePhone, fax, title, department
                 , street, street2, city, state, postalCode, country
                 , timeZoneId, timeFormat, dateFormat
                 , groupId, false
                 , UserContext.Current.OrganizationId, false
                 , true
                 , 0, 0, out password);
        }

        /// <summary>
        /// Adds the specified user to the current organization and sends the e-mail notification.
        /// If the user is not found creates new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="secondaryEmails">Specifies the user's secondary Emails.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static Guid AddUserToOrganization(string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , string timeZoneId, int? timeFormat, int? dateFormat
            , string groupId, string secondaryEmails)
        {
            string password = null;
            Guid userId = AddUserToOrganization(email, firstName, lastName, middleName
                 , phone, mobilePhone, fax, title, department
                 , street, street2, city, state, postalCode, country
                 , timeZoneId, timeFormat, dateFormat
                 , groupId, false, UserContext.Current.OrganizationId
                 , false, true, 0, 0, out password);

            UpdateUserSecondaryEmails(userId, secondaryEmails);

            return userId;
        }

        /// <summary>
        /// Adds the specified user to the specified organization and sends the e-mail notification.
        /// If the user is not found creates new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        public static Guid AddUserToOrganization(string email, string firstName, string lastName, string middleName
            , string groupId, Guid organizationId)
        {
            string password = null;
            return AddUserToOrganization(email, firstName, lastName, middleName, groupId, organizationId, false, true, 0, 0, out password);
        }

        /// <summary>
        /// Adds the specified user to the specified organization and sends the e-mail notification.
        /// If the user is not found creates new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        public static Guid AddUserToOrganization(string email, string firstName, string lastName, string middleName
            , string groupId, Guid organizationId, string password)
        {
            return AddUserToOrganization(email, firstName, lastName, middleName, groupId, organizationId, password, true);
        }

        /// <summary>
        /// Adds the specified user to the specified organization and sends the e-mail notification.
        /// If the user is not found creates new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="validatePassword">true to validate the password; otherwise, false.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        public static Guid AddUserToOrganization(string email, string firstName, string lastName, string middleName
            , string groupId, Guid organizationId, string password, bool validatePassword)
        {
            return AddUserToOrganization(email, firstName, lastName, middleName, groupId, organizationId, password, validatePassword, true);
        }

        /// <summary>
        /// Adds the specified user to the specified organization. If the user is not found creates new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="validatePassword">true to validate the password; otherwise, false.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        public static Guid AddUserToOrganization(string email, string firstName, string lastName, string middleName
            , string groupId, Guid organizationId, string password, bool validatePassword, bool sendEmailNotification)
        {
            return AddUserToOrganization(email, firstName, lastName, middleName
                , null, null, null, null, null
                , null, null, null, null, null, null
                , groupId, organizationId, password, validatePassword, sendEmailNotification);
        }

        /// <summary>
        /// Adds the specified user to the specified organization. If the user is not found creates new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">Specifies the identifier of the organization.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="validatePassword">true to validate the password; otherwise, false.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        public static Guid AddUserToOrganization(string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , string groupId, Guid organizationId
            , string password, bool validatePassword, bool sendEmailNotification)
        {
            return AddUserToOrganization(email, email, firstName, lastName, middleName
                , phone, mobilePhone, fax, title, department
                , street, street2, city, state, postalCode, country
                , groupId, organizationId
                , password, validatePassword, sendEmailNotification);
        }

        /// <summary>
        /// Adds the specified user to the specified organization. If the user is not found creates new user.
        /// </summary>
        /// <param name="loginName">The user's login name.</param>
        /// <param name="email">The user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">Specifies the identifier of the organization.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="validatePassword">true to validate the password; otherwise, false.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        public static Guid AddUserToOrganization(string loginName, string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , string groupId, Guid organizationId
            , string password, bool validatePassword, bool sendEmailNotification)
        {
            Guid loginId = Guid.Empty;
            DataRowView drv = WebApplication.LoginProvider.GetLogin(loginName);
            if (drv != null)
            {
                loginId = (Guid)drv["LoginId"];
                bool loginInOrganization = WebApplication.LoginProvider.LoginInOrganization(loginId, organizationId);

                if (loginInOrganization)
                {
                    UpdateUser(loginId, email, firstName, lastName, middleName
                        , phone, mobilePhone, fax, title, department
                        , street, street2, city, state, postalCode, country
                        , null, null, null
                        , groupId, false, organizationId, sendEmailNotification);
                }
                else
                {
                    InsertUserIntoOrganization(loginId, email, firstName, lastName, middleName
                        , phone, mobilePhone, fax, title, department, street, street2, city, state, postalCode, country
                        , null, null, null
                        , groupId, organizationId, false, false);

                    if (sendEmailNotification)
                        SendUserEmail(email, string.Empty, organizationId, groupId, false, false, true);
                }

                WebApplication.LoginProvider.ChangePassword(loginId, password, sendEmailNotification, validatePassword);
            }
            else
            {
                loginId = InsertUser(loginName, email, firstName, lastName, middleName
                    , phone, mobilePhone, fax, title, department, street, street2, city, state, postalCode, country
                    , groupId, organizationId
                    , password, validatePassword, sendEmailNotification);
            }

            return loginId;
        }

        /// <summary>
        /// Adds the specified user to the specified organization. If the user is not found creates new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">Specifies the identifier of the organization.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="validatePassword">true to validate the password; otherwise, false.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        public static Guid AddUserToOrganization(string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , IList groupId, Guid organizationId
            , string password, bool validatePassword, bool sendEmailNotification)
        {
            return AddUserToOrganization(email, firstName, lastName, middleName
                 , phone, mobilePhone, fax, title, department
                 , street, street2, city, state, postalCode, country
                 , Support.ConvertListToString(groupId), organizationId
                 , password, validatePassword, sendEmailNotification);
        }

        /// <summary>
        /// Adds the specified user to the specified organization. If the user is not found, creates new user.
        /// Refreshes all cached data by application after the action has been performed.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        /// <param name="minRequiredPasswordLength">The minimum length required for a password.</param>
        /// <param name="minRequiredNonAlphanumericCharacters">The minimum number of special characters that must be present in a password.</param>
        /// <param name="password">The generated password for a newly created user or a null reference, if the user exists.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        public static Guid AddUserToOrganization(string email, string firstName, string lastName, string middleName
            , string groupId, Guid organizationId, bool sendEmailNotification
            , int minRequiredPasswordLength, int minRequiredNonAlphanumericCharacters, out string password)
        {
            return AddUserToOrganization(email, firstName, lastName, middleName
                    , null, null, null, null, null, null
                    , null, null, null, null, null
                    , null, null, null
                    , groupId, false
                    , organizationId, false
                    , sendEmailNotification
                    , minRequiredPasswordLength, minRequiredNonAlphanumericCharacters, out password);
        }

        /// <summary>
        /// Adds the specified user to the specified organization. If the user is not found, creates new user.
        /// Refreshes all cached data by application after the action has been performed.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="organizationAdministrator">true, if the user is organization administrator; otherwise, false.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        /// <param name="minRequiredPasswordLength">The minimum length required for a password.</param>
        /// <param name="minRequiredNonAlphanumericCharacters">The minimum number of special characters that must be present in a password.</param>
        /// <param name="password">The generated password for a newly created user or a null reference, if the user exists.</param>
        /// <returns>The unique identifier of the newly created user or of the updated user if it already exists.</returns>
        public static Guid AddUserToOrganization(string email, string firstName, string lastName, string middleName
            , string groupId, Guid organizationId, bool organizationAdministrator
            , bool sendEmailNotification
            , int minRequiredPasswordLength, int minRequiredNonAlphanumericCharacters, out string password)
        {
            return AddUserToOrganization(email, firstName, lastName, middleName
                   , null, null, null, null, null, null
                   , null, null, null, null, null
                   , null, null, null
                   , groupId, false
                   , organizationId, organizationAdministrator
                   , sendEmailNotification
                   , minRequiredPasswordLength, minRequiredNonAlphanumericCharacters, out password);
        }

        /// <summary>
        /// Updates the details of current user in all databases where the user is placed.
        /// </summary>
        /// <param name="email">Specifies the user's e-mail address.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateCurrentUser(string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country, string timeZoneId, int? timeFormat, int? dateFormat)
        {
            UpdateCurrentUser(Guid.Empty, email, firstName, lastName, middleName, phone, mobilePhone, fax
                , title, department, street, street2, city, state, postalCode, country, timeZoneId, timeFormat, dateFormat);
        }

        /// <summary>
        /// Updates the details of current user in all databases where the user is placed.
        /// </summary>
        /// <param name="userId">Specifies the user's identifier.</param>
        /// <param name="email">Specifies the user's e-mail address.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateCurrentUser(Guid userId, string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country, string timeZoneId, int? timeFormat, int? dateFormat)
        {
            UserContext user = UserContext.Current;
            if (user == null) return;

            if (userId == Guid.Empty) userId = user.UserId;

            LoginProvider loginProvider = WebApplication.LoginProvider;
            bool needEmailUpdate = loginProvider.ChangeLoginName(userId, email);

            ClientDataSet.UserRow row = null;
            ClientDataSet.UserRow lastUpdatedRow = null;
            ArrayList list = new ArrayList();

            using (UserTableAdapter adapter = new UserTableAdapter(OrganizationProvider.GetConnectionString(user.OrganizationId)))
            {
                foreach (Organization organization in loginProvider.GetOrganizationsByLoginId(userId))
                {
                    string connectionString = OrganizationProvider.GetConnectionString(organization.OrganizationId);
                    if (!list.Contains(connectionString))
                    {
                        row = GetUserRowFromDatabase(userId, connectionString);
                        if (row != null)
                        {
                            if (needEmailUpdate) row.Email = email;
                            if (firstName != null) row.FirstName = firstName;
                            if (lastName != null) row.LastName = lastName;
                            if (middleName != null) row.MiddleName = middleName;
                            if (phone != null) row.Phone = phone;
                            if (mobilePhone != null) row.MobilePhone = mobilePhone;
                            if (fax != null) row.Fax = fax;
                            if (title != null) row.Title = title;
                            if (department != null) row.Department = department;
                            if (street != null) row.Street = street;
                            if (street2 != null) row.Street2 = street2;
                            if (city != null) row.City = city;
                            if (state != null) row.State = state;
                            if (postalCode != null) row.PostalCode = postalCode;
                            if (country != null) row.Country = country;
                            if (string.IsNullOrEmpty(timeZoneId))
                                row.SetTimeZoneIdNull();
                            else
                                row.TimeZoneId = timeZoneId;
                            if (timeFormat.HasValue)
                                row.TimeFormat = timeFormat.Value;
                            else
                                row.SetTimeFormatNull();
                            if (dateFormat.HasValue)
                                row.DateFormat = dateFormat.Value;
                            else
                                row.SetDateFormatNull();

                            adapter.Update(row);

                            lastUpdatedRow = row;

                            list.Add(connectionString);
                        }
                    }
                }
            }

            user.RefreshDetails(lastUpdatedRow);
        }

        /// <summary>
        /// Updates the details of specified user.
        /// </summary>
        /// <param name="userId">Specifies the user's identifier.</param>
        /// <param name="email">Specifies the user's e-mail address.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateUser(Guid userId, string email, string firstName, string lastName, string middleName)
        {
            UpdateUser(userId, email, firstName, lastName, middleName, null, null, null, null, null, null, null, null, null, null, null);
        }

        /// <summary>
        /// Updates the details of specified user in current organization.
        /// </summary>
        /// <param name="userId">Specifies the user's identifier.</param>
        /// <param name="email">Specifies the user's e-mail address.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateUser(Guid userId, string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country)
        {
            UpdateUser(userId, email, firstName, lastName, middleName
                , phone, mobilePhone, fax, title, department, street, street2, city, state, postalCode, country
                , null, null, null
                , (string)null, UserContext.Current.OrganizationId, FrameworkConfiguration.Current.WebApplication.Email.EnableChangeLoginNotification);
        }

        /// <summary>
        /// Updates the details of specified user in current organization.
        /// </summary>
        /// <param name="userId">Specifies the user's identifier.</param>
        /// <param name="email">Specifies the user's e-mail address.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="secondaryEmails">Specifies the user's secondary Emails.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateUser(Guid userId, string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , string timeZoneId, int? timeFormat, int? dateFormat
            , string secondaryEmails)
        {
            UpdateUser(userId, email, firstName, lastName, middleName
                , phone, mobilePhone, fax, title, department, street, street2, city, state, postalCode, country
                , timeZoneId, timeFormat, dateFormat
                , (string)null, UserContext.Current.OrganizationId, FrameworkConfiguration.Current.WebApplication.Email.EnableChangeLoginNotification);

            UpdateUserSecondaryEmails(userId, secondaryEmails);
        }

        /// <summary>
        /// Updates secondary emails of specified user in current organization.
        /// </summary>
        /// <param name="userId">Specifies the user's identifier.</param>
        /// <param name="secondaryEmails">Specifies the user's secondary Emails.</param>
        public static void UpdateUserSecondaryEmails(Guid userId, string secondaryEmails)
        {
            EmailProvider.DeleteEmails(userId, string.Empty);

            if (!string.IsNullOrEmpty(secondaryEmails))
            {
                string[] emails = secondaryEmails.Split(',');
                if (emails.Length > 0)
                {
                    foreach (string email in emails)
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(email.Trim(), Support.EmailRegularExpression))
                        {
                            if (!EmailProvider.IsEmailExists(email.Trim()))
                                EmailProvider.InsertEmail(email.Trim(), userId);
                        }
                    }
                }
                else
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(secondaryEmails.Trim(), Support.EmailRegularExpression))
                    {
                        if (!EmailProvider.IsEmailExists(secondaryEmails.Trim()))
                            EmailProvider.InsertEmail(secondaryEmails.Trim(), userId);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the details of specified user in current organization.
        /// </summary>
        /// <param name="userId">Specifies the user's identifier.</param>
        /// <param name="email">Specifies the user's e-mail address.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateUser(Guid userId, string email, string firstName, string lastName, string middleName, string groupId)
        {
            UpdateUser(userId, email, firstName, lastName, middleName, null, null, null, null, null, null, null, null, null, null, null
                , null, null, null
                , groupId, UserContext.Current.OrganizationId, FrameworkConfiguration.Current.WebApplication.Email.EnableChangeLoginNotification);
        }

        /// <summary>
        /// Updates the details of specified user in the specified organization.
        /// </summary>
        /// <param name="userId">Specifies the user's identifier.</param>
        /// <param name="email">Specifies the user's e-mail address.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">Specifies the identifier of the organization.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        public static void UpdateUser(Guid userId, string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , string timeZoneId, int? timeFormat, int? dateFormat
            , string groupId, Guid organizationId, bool sendEmailNotification)
        {
            UpdateUser(userId, email, firstName, lastName, middleName
                , phone, mobilePhone, fax, title, department
                , street, street2, city, state, postalCode, country
                , timeZoneId, timeFormat, dateFormat
                , groupId, false, organizationId, sendEmailNotification);
        }

        /// <summary>
        /// Updates the details of specified user in the specified organization.
        /// </summary>
        /// <param name="userId">Specifies the user's identifier.</param>
        /// <param name="email">Specifies the user's e-mail address.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">Specifies the identifier of the organization.</param>
        public static void UpdateUser(Guid userId, string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , string timeZoneId, int? timeFormat, int? dateFormat
            , IList groupId, Guid organizationId)
        {
            UpdateUser(userId, email, firstName, lastName, middleName, phone, mobilePhone, fax, title, department, street, street2, city, state, postalCode, country, timeZoneId, timeFormat, dateFormat
                , Support.ConvertListToString(groupId), organizationId, FrameworkConfiguration.Current.WebApplication.Email.EnableChangeLoginNotification);
        }

        /// <summary>
        /// Updates the details of specified user in the specified organization.
        /// </summary>
        /// <param name="userId">Specifies the user's identifier.</param>
        /// <param name="email">Specifies the user's e-mail address.</param>
        /// <param name="firstName">Specifies the user's first name.</param>
        /// <param name="lastName">Specifies the user's last name.</param>
        /// <param name="middleName">Specifies the user's middle name.</param>
        /// <param name="phone">Specifies the user's phone.</param>
        /// <param name="mobilePhone">Specifies the user's mobile phone.</param>
        /// <param name="fax">Specifies the user's fax.</param>
        /// <param name="title">Specifies the user's title.</param>
        /// <param name="department">Specifies the department.</param>
        /// <param name="street">Specifies the user's street.</param>
        /// <param name="street2">Specifies the user's secondary street.</param>
        /// <param name="city">Specifies the user's city.</param>
        /// <param name="state">Specifies the user's state/province.</param>
        /// <param name="postalCode">Specifies the user's postal code.</param>
        /// <param name="country">Specifies the user's country.</param>
        /// <param name="timeZoneId">Time zone identifier.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">Specifies the identifier of the organization.</param>
        /// <param name="sendEmailNotification">true to send notification email; otherwise, false.</param>
        public static void UpdateUser(Guid userId, string email, string firstName, string lastName, string middleName
            , string phone, string mobilePhone, string fax, string title, string department
            , string street, string street2, string city, string state, string postalCode, string country
            , string timeZoneId, int? timeFormat, int? dateFormat
            , IList groupId, Guid organizationId, bool sendEmailNotification)
        {
            UpdateUser(userId, email, firstName, lastName, middleName, phone, mobilePhone, fax, title, department, street, street2, city, state, postalCode, country, timeZoneId, timeFormat, dateFormat
                , Support.ConvertListToString(groupId), organizationId, sendEmailNotification);
        }

        /// <summary>
        /// Updates the groups of specified user in the current organization.
        /// </summary>
        /// <param name="userId">Specifies the user's identifier.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateUser(Guid userId, string groupId)
        {
            UserContext user = UserContext.Current;
            if (user != null)
            {
                if (user.OrganizationId != Guid.Empty)
                    UpdateUser(userId, groupId, user.OrganizationId);
            }
        }

        /// <summary>
        /// Updates the groups of specified user in the specified organization.
        /// </summary>
        /// <param name="userId">Specifies the user's identifier.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">Specifies the identifier of the organization.</param>
        public static void UpdateUser(Guid userId, string groupId, Guid organizationId)
        {
            UpdateUser(userId, null, groupId, false, organizationId, null, true);
        }

        /// <summary>
        /// Updates the groups of specified user in the specified organization.
        /// </summary>
        /// <param name="userId">Specifies the user's identifier.</param>
        /// <param name="groupId">Specifies the groups identifiers that the user belong to.</param>
        /// <param name="organizationId">Specifies the identifier of the organization.</param>
        public static void UpdateUser(Guid userId, string groupId, Guid organizationId, bool sendEmailNotification)
        {
            UpdateUser(userId, null, groupId, false, organizationId, null, sendEmailNotification);
        }

        public static bool UserIsActiveInInstance(Guid userId, Guid instanceId, Guid organizationId)
        {
            ClientDataSet.UsersInstancesDataTable table = null;
            try
            {
                table = GetUsersInstances(userId, organizationId);
                ClientDataSet.UsersInstancesRow row = table.FindByUserIdInstanceId(userId, instanceId);
                if (row != null)
                    return row.Active;
                return true;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Updates the active flag of the specified user in the specified organization.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to update.</param>
        /// <param name="organizationId">The unique identifier of the organization to update the user in.</param>
        /// <param name="active">true, if the user is active in the specified organization; otherwise, false.</param>
        public static void UpdateUserActive(Guid userId, Guid organizationId, bool active)
        {
            WebApplication.LoginProvider.UpdateLoginInOrganization(userId, organizationId, null, active);
            UpdateUserInOrganization(userId, organizationId, null, active);
        }

        /// <summary>
        /// Updates the active flag of the specified user in the specified instance.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to update.</param>
        /// <param name="instanceId">The unique identifiers of the instance in which the user should be activated or inactivates.</param>
        /// <param name="active">true, if the user is active in the specified organization; otherwise, false.</param>
        /// <param name="organizationId">The unique identifier of the organization to update the user in.</param>
        public static void UpdateUserActive(Guid userId, Guid instanceId, Guid organizationId, bool active)
        {
            ClientDataSet.UsersInstancesDataTable table = null;
            try
            {
                table = GetUsersInstances(userId, organizationId);
                ClientDataSet.UsersInstancesRow row = table.FindByUserIdInstanceId(userId, instanceId);
                if (row != null)
                {
                    if (active)
                    {
                        using (UsersInstancesTableAdapter adapter = new UsersInstancesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                        {
                            adapter.Delete(userId, instanceId);
                        }

                        UpdateUserActive(userId, organizationId, true);
                    }
                    else
                    {
                        using (UsersInstancesTableAdapter adapter = new UsersInstancesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                        {
                            adapter.Update(userId, instanceId, false);
                        }
                    }
                }
                else if (!active)
                {
                    using (UsersInstancesTableAdapter adapter = new UsersInstancesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
                    {
                        adapter.Insert(userId, instanceId, false);
                    }
                }
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Updates the active flag of the specified user in the specified organization and its instances.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to update.</param>
        /// <param name="instanceIdListWhereUserIsActive">The list of unique identifiers of the instances in which the user is active.</param>
        /// <param name="organizationId">The unique identifier of the organization to update the user in.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateUserActive(Guid userId, string instanceIdListWhereUserIsActive, Guid organizationId)
        {
            ClientDataSet.UsersInstancesDataTable usersInstancesTable = GetUsersInstances(userId, organizationId);

            ICollection<Guid> coll = Support.ConvertStringToGuidList(instanceIdListWhereUserIsActive);

            foreach (ClientDataSet.InstanceRow instanceRow in InstanceProvider.GetInstances(organizationId))
            {
                bool inactivate = true;
                if (coll != null)
                    inactivate = (!coll.Contains(instanceRow.InstanceId));

                ClientDataSet.UsersInstancesRow usersInstancesRow = usersInstancesTable.FindByUserIdInstanceId(userId, instanceRow.InstanceId);

                if (inactivate)
                {
                    if (usersInstancesRow == null)
                    {
                        usersInstancesRow = usersInstancesTable.NewUsersInstancesRow();
                        usersInstancesRow.InstanceId = instanceRow.InstanceId;
                        usersInstancesRow.UserId = userId;
                        usersInstancesRow.Active = false;
                        usersInstancesTable.AddUsersInstancesRow(usersInstancesRow);
                    }
                    else
                        usersInstancesRow.Active = false;
                }
                else if (usersInstancesRow != null)
                    usersInstancesRow.Active = true;
            }

            using (UsersInstancesTableAdapter adapter = new UsersInstancesTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Update(usersInstancesTable);
            }

            if (coll != null)
                UpdateUserActive(userId, organizationId, coll.Contains(Guid.Empty));
        }

        /// <summary>
        /// Removes the specified user from current user's selected organization.
        /// </summary>
        /// <param name="userId">The user's identifier to remove.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void RemoveUserFromOrganization(Guid userId)
        {
            UserContext user = UserContext.Current;
            if (user != null)
                RemoveUserFromOrganization(userId, user.OrganizationId);
        }

        /// <summary>
        /// Removes the specified user from specified organization.
        /// </summary>
        /// <param name="userId">The user's identifier to remove.</param>
        /// <param name="organizationId">The organization's identifier to remove the user from.</param>
        public static void RemoveUserFromOrganization(Guid userId, Guid organizationId)
        {
            using (OrganizationsUsersTableAdapter adapter = new OrganizationsUsersTableAdapter(OrganizationProvider.GetConnectionString(organizationId)))
            {
                adapter.Delete(organizationId, userId);
            }

            WebApplication.LoginProvider.RemoveLoginFromOrganization(userId, organizationId);
        }

        #endregion
    }

    /// <summary>
    /// The class containing the data for the Micajah.Common.Bll.Providers.UserProvider.UserInserted event.
    /// </summary>
    public class UserInsertedEventArgs : EventArgs
    {
        #region Constructors

        private List<Guid> m_GroupIdList;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UserInsertedEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="userId">The unique identifier of newly created user</param>
        /// <param name="organizationId">The unique identifier of the organization whereto the user is added</param>
        /// <param name="instanceId">The unique identifier of the instance whereto the user is added.</param>
        /// <param name="groupIdList">The list of the unique identifier of the groups in which the user is added.</param>
        public UserInsertedEventArgs(Guid userId, Guid? organizationId, Guid? instanceId, ICollection<Guid> groupIdList)
        {
            this.UserId = userId;
            this.OrganizationId = organizationId;
            this.InstanceId = instanceId;
            m_GroupIdList = new List<Guid>(groupIdList);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the unique identifier of newly created user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the organization whereto the user is added.
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the instance whereto the user is added.
        /// </summary>
        public Guid? InstanceId { get; set; }

        /// <summary>
        /// Gets the list of the unique identifier of the groups in which the user is added.
        /// </summary>
        public IList<Guid> GroupIdList
        {
            get
            {
                if (m_GroupIdList == null)
                    m_GroupIdList = new List<Guid>();
                return m_GroupIdList;
            }
        }

        #endregion
    }

    /// <summary>
    /// The class containing the data for the Micajah.Common.Bll.Providers.UserProvider.UserUpdated event.
    /// </summary>
    public class UserUpdatedEventArgs : EventArgs
    {
        #region Constructors

        private List<Guid> m_GroupIdList;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public UserUpdatedEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="userId">The unique identifier of user</param>        
        /// <param name="organizationId">The unique identifier of the user organization</param>
        /// <param name="groupIdList">The list of the unique identifier of the user groups.</param>
        public UserUpdatedEventArgs(Guid userId, Guid? organizationId, ICollection<Guid> groupIdList)
        {
            this.UserId = userId;
            this.OrganizationId = organizationId;
            m_GroupIdList = new List<Guid>(groupIdList);
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="userId">The unique identifier of user</param>        
        /// <param name="organizationId">The unique identifier of the user organization</param>
        /// <param name="groupIdList">The list of the unique identifier of the user groups.</param>
        /// <param name="email">User email.</param>
        public UserUpdatedEventArgs(Guid userId, Guid? organizationId, ICollection<Guid> groupIdList, string email)
        {
            this.UserId = userId;
            this.OrganizationId = organizationId;
            m_GroupIdList = new List<Guid>(groupIdList);
            this.Email = email;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the unique identifier of user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user organization.
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// Gets the list of the unique identifier of the user groups.
        /// </summary>
        public IList<Guid> GroupIdList
        {
            get
            {
                if (m_GroupIdList == null)
                    m_GroupIdList = new List<Guid>();
                return m_GroupIdList;
            }
        }

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public string Email { get; set; }

        #endregion
    }
}
