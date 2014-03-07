using Micajah.Common.Application;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Dal;
using Micajah.Common.Dal.MasterDataSetTableAdapters;
using Micajah.Common.LdapAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Micajah.Common.Security
{
    /// <summary>
    /// The class that works with local application login tables.
    /// </summary>
    public class LdapIntegration : ILdapIntegration
    {
        #region Members

        ILogger ApplicationLogger;

        #endregion

        #region Public Methods

        public LdapIntegration(ILogger applicationLogger)
        {
            ApplicationLogger = applicationLogger;
        }

        #endregion

        #region Internal Helper Functions

        public IList<IUser> GetUsersByEmail(string email)
        {
            ApplicationLogger.LogInfo("LDAPIntegration", string.Format(CultureInfo.CurrentCulture, "Searching local Logins table for email={0}", email));

            DataTable table = new DataTable();
            table.Locale = CultureInfo.CurrentCulture;

            using (OrganizationsLoginsTableAdapter adapter = new OrganizationsLoginsTableAdapter())
            {
                table = adapter.GetLoginByEmail(email);
            }

            List<IUser> users = new List<IUser>();

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    users.Add(createNewUser(table.Rows[i]));
                }
                ApplicationLogger.LogInfo("LDAPIntegration", "result=Found!");
            }
            else
                ApplicationLogger.LogInfo("LDAPIntegration", "result=not found.");

            return users;
        }

        public IList<IUser> GetUsersByDomainLogin(string domainName, string userAlias, string firstName, string lastName)
        {
            ApplicationLogger.LogInfo("LDAPIntegration", string.Format(CultureInfo.CurrentCulture, "Searching local Logins tables for DomainName={0} AND UserAlias={1} AND FirstName={2} AND LastName={3}", domainName, userAlias, firstName, lastName));

            DataTable table = new DataTable();
            table.Locale = CultureInfo.CurrentCulture;

            using (OrganizationsLoginsTableAdapter adapter = new OrganizationsLoginsTableAdapter())
            {
                table = adapter.GetLoginByDomainName(
                    (string.IsNullOrEmpty(domainName) == false) ? domainName : null
                    , (string.IsNullOrEmpty(userAlias) == false) ? userAlias : null
                    , (string.IsNullOrEmpty(firstName) == false) ? firstName : null
                    , (string.IsNullOrEmpty(lastName) == false) ? lastName : null);
            }

            List<IUser> users = new List<IUser>();

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    users.Add(createNewUser(table.Rows[i]));
                }
                ApplicationLogger.LogInfo("LDAPIntegration", "result=Found!");
            }
            else
                ApplicationLogger.LogInfo("LDAPIntegration", "result=not found.");

            return users;
        }

        public IList<IUser> GetUsersByUserPrinciple(string uPrinciple)
        {
            return GetUsersByDomainLogin(null, uPrinciple, null, null);
        }

        private static IUser createNewUser(DataRow row)
        {
            /* Guid roleGuid = new Guid();
            if (data.Tables[0].Rows[i].IsNull("RoleId") != true)
                roleGuid = data.Tables[0].Rows[i].Field<Guid>("RoleId"); */
            string ldapDomainName = string.Empty;
            int ldapPort = 0;
            if (string.IsNullOrEmpty(Convert.ToString(row["LdapDomain"], CultureInfo.CurrentCulture)) == false)
                ldapDomainName = (string)row["LdapDomain"];
            else
                ldapDomainName = (string)row["OrganizationLdapDomain"];
            if (string.IsNullOrEmpty((string)row["LdapServerPort"]) == false)
                ldapPort = Convert.ToInt32((string)row["LdapServerPort"], CultureInfo.CurrentCulture);

            return new User()
            {
                FirstName = (string.IsNullOrEmpty(Convert.ToString(row["FirstName"], CultureInfo.CurrentCulture)) == true) ? string.Empty : (string)row["FirstName"],
                LastName = (string.IsNullOrEmpty(Convert.ToString(row["LastName"], CultureInfo.CurrentCulture)) == true) ? string.Empty : (string)row["LastName"],
                LocalLoginId = (Guid)row["LoginId"],
                EmailAddress = (string)row["LoginName"],
                Password = (string)row["Password"],
                OrganizationId = (Guid)row["OrganizationId"],
                //InstanceId = row.Field<Guid>("InstanceId"),
                LdapUserAlias = (string.IsNullOrEmpty(Convert.ToString(row["LdapUserAlias"], CultureInfo.CurrentCulture)) == true) ? string.Empty : (string)row["LdapUserAlias"],
                LdapUserPrinciple = (string.IsNullOrEmpty(Convert.ToString(row["LdapUPN"], CultureInfo.CurrentCulture)) == true) ? string.Empty : (string)row["LdapUPN"],
                LdapDomain = (string.IsNullOrEmpty(ldapDomainName) == true) ? string.Empty : ldapDomainName,
                LdapDomainFull = (string.IsNullOrEmpty(Convert.ToString(row["LdapDomainFull"], CultureInfo.CurrentCulture)) == true) ? string.Empty : (string)row["LdapDomainFull"],
                LdapServerAddress = (string.IsNullOrEmpty(Convert.ToString(row["LdapServerAddress"], CultureInfo.CurrentCulture)) == true) ? string.Empty : (string)row["LdapServerAddress"],
                LdapServerPort = ldapPort,
                LdapServerUserName = (string.IsNullOrEmpty(Convert.ToString(row["LdapUserName"], CultureInfo.CurrentCulture)) == true) ? string.Empty : (string)row["LdapUserName"],
                LdapServerPassword = (string.IsNullOrEmpty(Convert.ToString(row["LdapPassword"], CultureInfo.CurrentCulture)) == true) ? string.Empty : (string)row["LdapPassword"],
                //RoleId = roleGuid,
                IsActive = (bool)row["Active"],
                LdapUserId = (string.IsNullOrEmpty(Convert.ToString(row["LdapUserId"], CultureInfo.CurrentCulture)) == true) ? Guid.Empty : (Guid)row["LdapUserId"],
            };
        }

        #endregion

        #region ILDAPIntegration Members

        // Return hashed via MD5 or HA1 methods passord or plain text password
        public string GetHashedPassword(string password)
        {
            return WebApplication.LoginProvider.EncryptPassword(password);
        }

        public DataSet GetSortedListOfRolesGroupsById(Guid organizationId)
        {
            ClientDataSet.GroupsInstancesRolesDataTable table = GroupProvider.GetGroupsInstancesRoles(organizationId);
            table.Columns.Add("RoleName", typeof(string));
            table.Columns.Add("GroupName", typeof(string));

            foreach (ClientDataSet.GroupsInstancesRolesRow row in table.Rows)
            {
                ConfigurationDataSet.RoleRow roleRow = RoleProvider.GetRoleRow((Guid)row["RoleId"]);
                if (row != null) row["RoleName"] = roleRow.Name;

                MasterDataSet.GroupMappingsRow[] groups = LdapInfoProvider.GetGroupMappings(organizationId).Select(string.Format(CultureInfo.InvariantCulture, "GroupId = '{0}' AND OrganizationId = '{1}'", row["GroupId"], organizationId)) as MasterDataSet.GroupMappingsRow[];
                if (groups.Length > 0) row["GroupName"] = groups[0].LdapGroupName;
            }

            return table.DataSet;
        }

        public Guid CreateLocalUser(IUser user)
        {
            return CreateLocalUser(user, string.Empty);
        }

        public Guid CreateLocalUser(IUser user, string groupId)
        {
            Guid loginId = Guid.Empty;
            if (user != null)
            {
                if (!string.IsNullOrEmpty((string.IsNullOrEmpty(user.EmailAddress) == true) ? user.LdapUserPrinciple : user.EmailAddress))
                {
                    string localGroupId = LdapInfoProvider.GetAppGroupsByLdapGroups(user.OrganizationId, groupId);
                    loginId = UserProvider.AddUserToOrganization((string.IsNullOrEmpty(user.EmailAddress) == true) ? user.LdapUserPrinciple : user.EmailAddress, user.EmailAddress, user.FirstName, user.LastName, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, localGroupId, user.OrganizationId, user.Password, false, false);
                    WebApplication.LoginProvider.UpdateUserLdapInfo(user.OrganizationId, loginId, user.FirstName, user.LastName, user.LdapDomain, user.LdapDomainFull, user.LdapUserAlias, user.LdapUserPrinciple, user.UserSid, user.UserId, user.LdapOUPath);
                }
            }
            return loginId;
        }

        public bool SetLdapInfoDetails(IUser user)
        {
            DataRowView drv = null;

            if (user != null)
            {
                if (user.LocalLoginId != Guid.Empty)
                    drv = WebApplication.LoginProvider.GetLogin(user.LocalLoginId);
                else if (!string.IsNullOrEmpty(user.EmailAddress))
                    drv = WebApplication.LoginProvider.GetLogin(user.EmailAddress);
                else if (!string.IsNullOrEmpty(user.LdapUserPrinciple))
                    drv = WebApplication.LoginProvider.GetLogin(user.LdapUserPrinciple);

                if (drv != null)
                    WebApplication.LoginProvider.UpdateUserLdapInfo(user.OrganizationId, (Guid)drv["LoginId"], user.FirstName, user.LastName, user.LdapDomain, user.LdapDomainFull, user.LdapUserAlias, user.LdapUserPrinciple, user.UserSid, user.UserId, user.LdapOUPath);
            }

            return true;
        }

        public bool SetUserRole(IUser user)
        {
            /*SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.LocalAppDBConnectionString);
            SqlCeCommand cmd = connection.CreateCommand();
            connection.Open();

            cmd.CommandText = string.Format("UPDATE Login SET RoleId = '{0}'", user.RoleId.ToString("B", CultureInfo.CurrentCulture));
            cmd.ExecuteNonQuery();

            connection.Close();

            ApplicationLogger.LogInfo("LDAPIntegration", "User RoleID was updated in the local Logins table.");
            */
            return true;
        }

        public Micajah.Common.LdapAdapter.ILdapServer GetLdapServer(Guid serverId)
        {
            LdapServer server = null;

            MasterDataSet.OrganizationRow row = OrganizationProvider.GetOrganizationRow(serverId);

            if (row != null)
            {
                if ((!row.Deleted) && row.Active)
                {
                    int ldapPort = 0;
                    if (!string.IsNullOrEmpty(row.LdapServerPort))
                        ldapPort = Convert.ToInt32(row.LdapServerPort);

                    server = new LdapServer()
                    {
                        ServerAddress = row.LdapServerAddress,
                        Port = ldapPort,
                        SiteDomain = row.LdapDomain,
                        UserName = row.LdapUserName,
                        Password = row.LdapPassword,
                        AuthenticationType = "Default"
                    };
                }
            }

            return server;
        }

        public void CreateUserEmails(System.Collections.ObjectModel.ReadOnlyCollection<string> altEmails, IUser user)
        {
            DataRowView drv = null;
            if (user != null && altEmails.Count > 0)
            {
                if (user.LocalLoginId != Guid.Empty)
                    drv = WebApplication.LoginProvider.GetLogin(user.LocalLoginId);
                else if (!string.IsNullOrEmpty(user.EmailAddress))
                    drv = WebApplication.LoginProvider.GetLogin(user.EmailAddress);
                else if (!string.IsNullOrEmpty(user.LdapUserPrinciple))
                    drv = WebApplication.LoginProvider.GetLogin(user.LdapUserPrinciple);

                if (drv != null)
                {
                    foreach (string email in altEmails)
                    {
                        if (!EmailProvider.IsEmailExists(email))
                            EmailProvider.InsertEmail(email, (Guid)drv["LoginId"]);
                    }
                }
            }
        }

        #endregion
    }
}
