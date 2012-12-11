using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;
using Micajah.Common.Application;
using Micajah.Common.Dal;
using Micajah.Common.LdapAdapter;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Net.Sockets;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with ldap.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class LdapInfoProvider
    {
        #region Members
        // The objects which are used to synchronize access to the cached objects.
        private static object s_LdapProcessesSyncRoot = new object();
        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the instance of the Micajah.Common.Dal.CommonDataSet class that contains common data of application.
        /// </summary>
        internal static List<LdapProcess> LdapProcesses
        {
            get
            {
                List<LdapProcess> list = null;
                try
                {
                    list = CacheManager.Current.Get("mc.LdapProcesses") as List<LdapProcess>;
                    if (list == null)
                    {
                        lock (s_LdapProcessesSyncRoot)
                        {
                            list = CacheManager.Current.Get("mc.LdapProcesses") as List<LdapProcess>;
                            if (list == null)
                            {
                                list = new List<LdapProcess>();
                                CacheManager.Current.Add("mc.LdapProcesses", list);
                            }
                        }
                    }
                    return list;
                }
                finally
                {
                    list = null;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Pings connect to ldap server.
        /// </summary>
        /// <param name="ldapServerAddress">Server Address.</param>
        /// <param name="ldapServerPort">Server Port.</param>
        /// <param name="ldapUserName">Ldap User Name.</param>
        /// <param name="ldapPassword">Ldap Password.</param>
        /// <param name="ldapDomain">Server Domain.</param>
        /// <returns>The System.String that contains result of ping.</returns>
        public static string PingLdapServer(string ldapServerAddress, Int32 ldapServerPort, string ldapUserName, string ldapPassword, string ldapDomain)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(ldapServerAddress))
                return Resources.LdapInfoProvider_MissingServerAddress_Text;
            if (ldapServerPort == 0)
                return Resources.LdapInfoProvider_MissingServerPort_Text;
            if (string.IsNullOrEmpty(ldapUserName))
                return Resources.LdapInfoProvider_MissingUserName_Text;
            if (string.IsNullOrEmpty(ldapPassword))
                return Resources.LdapInfoProvider_MissingPassword_Text;
            if (string.IsNullOrEmpty(ldapDomain))
                return Resources.LdapInfoProvider_MissingDomain_Text;

            using (var server = new LdapProvider(Guid.NewGuid(), ldapServerAddress, ldapServerPort, ldapUserName, ldapPassword, ldapDomain, "Default", null, null))
            {
                try
                {
                    server.Ping();
                    result = Resources.LdapInfoProvider_Success_Text;
                }
                catch (LdapException ex)
                {
                    result = string.Format(CultureInfo.InvariantCulture, Resources.LdapInfoProvider_LdapError_Text, ex.ErrorCode, ex.Message);
                }
                catch (DirectoryOperationException ex)
                {
                    result = string.Format(CultureInfo.InvariantCulture, Resources.LdapInfoProvider_DirectoryOperationError_Text, ex.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// Checks if address port can be connected
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool ScanPort(string address, int port)
        {
            System.Net.Sockets.Socket socket = null;
            try
            {
                socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                socket.Connect(address, port);
                if (!socket.Connected)
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (socket != null && socket.Connected) socket.Disconnect(false);
            }
        }



        /// <summary>
        /// Gets list of domains from ldap.
        /// </summary>
        /// <param name="ldapServerAddress">Server Address.</param>
        /// <param name="ldapServerPort">Server Port.</param>
        /// <param name="ldapUserName">Ldap User Name.</param>
        /// <param name="ldapPassword">Ldap Password.</param>
        /// <param name="ldapDomain">Server Domain.</param>
        /// <returns>The System.Data.DataView that contains domains.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetDomains(string ldapServerAddress, Int32 ldapServerPort, string ldapUserName, string ldapPassword, string ldapDomain)
        {
            DataTable table = new DataTable("Domains");
            table.Locale = CultureInfo.CurrentCulture;
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("DomainName", typeof(string));

            if (String.IsNullOrEmpty(ldapServerAddress) == true || ldapServerPort == 0 || String.IsNullOrEmpty(ldapUserName) == true || String.IsNullOrEmpty(ldapPassword) == true || String.IsNullOrEmpty(ldapDomain) == true)
                return table.DefaultView;

            using (var server = new LdapProvider(Guid.NewGuid(), ldapServerAddress, ldapServerPort, ldapUserName, ldapPassword, ldapDomain, "Default", null, null))
            {
                if (server.Initialize())
                {
                    var domains = server.GetDomains();
                    if (domains != null)
                    {
                        // output list of domains
                        for (int i = 0; i < domains.Count; i++)
                        {
                            if (domains[i].Name != "DomainDnsZones" && domains[i].Name != "ForestDnsZones")
                                table.Rows.Add(domains[i].Guid, domains[i].Name);
                        }
                    }
                }
            }
            table.DefaultView.Sort = "[DomainName] asc";

            return table.DefaultView;
        }

        /// <summary>
        /// Gets list of domains from ldap by OrganizationId.
        /// </summary>
        /// <param name="organizationId">organizationId.</param>
        /// <returns>The System.Data.DataView that contains domains.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetDomains(Guid organizationId)
        {
            DataTable table = new DataTable("Domains");
            table.Locale = CultureInfo.CurrentCulture;
            table.Columns.Add("Id", typeof(Guid));
            table.Columns.Add("DomainName", typeof(string));
            table.Columns.Add("DistinguishedName", typeof(string));

            Organization org = OrganizationProvider.GetOrganization(organizationId);

            if (String.IsNullOrEmpty(org.LdapServerAddress) == true || String.IsNullOrEmpty(org.LdapServerPort) == true || String.IsNullOrEmpty(org.LdapUserName) == true || String.IsNullOrEmpty(org.LdapPassword) == true || String.IsNullOrEmpty(org.LdapDomain) == true)
                return table.DefaultView;

            using (var server = new LdapProvider(organizationId, org.LdapServerAddress, Convert.ToInt32(org.LdapServerPort, CultureInfo.InvariantCulture), org.LdapUserName, org.LdapPassword, org.LdapDomain, "Default", null, null))
            {
                if (server.Initialize())
                {
                    var domains = server.GetDomains();
                    if (domains != null)
                    {
                        // output list of domains
                        for (int i = 0; i < domains.Count; i++)
                        {
                            Domain domain = domains[i];
                            if (domain.Name != "DomainDnsZones" && domain.Name != "ForestDnsZones")
                                table.Rows.Add(domain.Guid, domain.Name, domain.DistinguishedName);

                            domain = null;
                        }
                    }
                }
            }
            table.DefaultView.Sort = "[DomainName] asc";

            return table.DefaultView;
        }

        /// <summary>
        /// Gets list of domains from ldap by OrganizationId from local DB.
        /// </summary>
        /// <param name="organizationId">organizationId.</param>
        /// <returns>The System.Data.DataView that contains domains.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetDomainsFromDB(Guid organizationId)
        {
            DataTable table = null;
            CommonDataSet.OrganizationsLdapGroupsDataTable orgTable = null;
            CommonDataSet.OrganizationsLdapGroupsRow dr = null;
            Organization org = null;
            try
            {
                table = new DataTable("Domains");
                table.Locale = CultureInfo.CurrentCulture;
                table.Columns.Add("Id", typeof(Guid));
                table.Columns.Add("DomainName", typeof(string));
                table.Columns.Add("DistinguishedName", typeof(string));

                orgTable = new CommonDataSet.OrganizationsLdapGroupsDataTable();
                WebApplication.CommonDataSetTableAdapters.OrganizationsLdapGroupsTableAdapter.Fill(orgTable, 2, organizationId);

                if (orgTable.Rows.Count > 0)
                {
                    for (int i = 0; i < orgTable.Rows.Count; i++)
                    {
                        dr = orgTable.Rows[i] as CommonDataSet.OrganizationsLdapGroupsRow;
                        if (dr.Domain != "DomainDnsZones" && dr.Domain != "ForestDnsZones")
                            table.Rows.Add(dr.DomainId, dr.Domain, string.Empty);
                    }
                    table.DefaultView.Sort = "[DomainName] asc";

                    return table.DefaultView;
                }
                else
                {
                    org = OrganizationProvider.GetOrganization(organizationId);

                    if (String.IsNullOrEmpty(org.LdapServerAddress) == true || String.IsNullOrEmpty(org.LdapServerPort) == true || String.IsNullOrEmpty(org.LdapUserName) == true || String.IsNullOrEmpty(org.LdapPassword) == true || String.IsNullOrEmpty(org.LdapDomain) == true)
                        return table.DefaultView;

                    using (var server = new LdapProvider(organizationId, org.LdapServerAddress, Convert.ToInt32(org.LdapServerPort, CultureInfo.InvariantCulture), org.LdapUserName, org.LdapPassword, org.LdapDomain, "Default", null, null))
                    {
                        if (server.Initialize())
                        {
                            var domains = server.GetDomains();
                            if (domains != null)
                            {
                                // output list of domains
                                for (int i = 0; i < domains.Count; i++)
                                {
                                    Domain domain = domains[i];
                                    if (domain.Name != "DomainDnsZones" && domain.Name != "ForestDnsZones")
                                        table.Rows.Add(domain.Guid, domain.Name, domain.DistinguishedName);

                                    domain = null;
                                }
                            }
                        }
                    }
                    table.DefaultView.Sort = "[DomainName] asc";
                    return table.DefaultView;
                }
            }
            finally
            {
                if (table != null) table.Dispose();
                if (orgTable != null) orgTable.Dispose();
                dr = null;
            }
        }

        /// <summary>
        /// Gets list of groups for the current domain from ldap.
        /// </summary>
        /// <param name="ldapServerAddress">Server Address.</param>
        /// <param name="ldapServerPort">Server Port.</param>
        /// <param name="ldapUserName">Ldap User Name.</param>
        /// <param name="ldapPassword">Ldap Password.</param>
        /// <param name="ldapDomain">Server Domain.</param>
        /// <returns>The System.Data.DataView that contains groups.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetGroups(string ldapServerAddress, Int32 ldapServerPort, string ldapUserName, string ldapPassword, string ldapDomain, string selectedDomain, Guid organizationId)
        {
            DataTable table = null;
            CommonDataSet.OrganizationsLdapGroupsDataTable orgTable = null;
            CommonDataSet.OrganizationsLdapGroupsRow dr = null;
            try
            {
                table = new DataTable("Groups");
                table.Locale = CultureInfo.CurrentCulture;
                table.Columns.Add("Id", typeof(Guid));
                table.Columns.Add("GroupName", typeof(string));

                orgTable = new CommonDataSet.OrganizationsLdapGroupsDataTable();
                WebApplication.CommonDataSetTableAdapters.OrganizationsLdapGroupsTableAdapter.Fill(orgTable, 1, organizationId, selectedDomain);

                if (orgTable.Rows.Count > 0)
                {
                    for (int i = 0; i < orgTable.Rows.Count; i++)
                    {
                        dr = orgTable.Rows[i] as CommonDataSet.OrganizationsLdapGroupsRow;
                        table.Rows.Add(dr.ObjectGUID, dr.Name);
                    }
                    table.DefaultView.Sort = "[GroupName] asc";

                    return table.DefaultView;
                }
                else
                {
                    if (String.IsNullOrEmpty(ldapServerAddress) == true || ldapServerPort == 0 || String.IsNullOrEmpty(ldapUserName) == true || String.IsNullOrEmpty(ldapPassword) == true || String.IsNullOrEmpty(ldapDomain) == true || String.IsNullOrEmpty(selectedDomain) == true)
                        return table.DefaultView;

                    using (var server = new LdapProvider(Guid.NewGuid(), ldapServerAddress, ldapServerPort, ldapUserName, ldapPassword, ldapDomain, "Default", null, null))
                    {
                        if (server.Initialize())
                        {
                            var domain = server.GetDomainByName(selectedDomain);
                            if (domain != null)
                            {
                                // output list of groups
                                var groups = server.GetGroups(domain, 25);
                                if (groups != null)
                                {
                                    // output list of domains
                                    for (int i = 0; i < groups.Count; i++)
                                    {
                                        table.Rows.Add(groups[i].GroupGuid, groups[i].Name);
                                    }
                                }
                            }
                        }
                    }
                    table.DefaultView.Sort = "[GroupName] asc";

                    return table.DefaultView;
                }
            }
            finally
            {
                if (table != null) table.Dispose();
                if (orgTable != null) orgTable.Dispose();
                dr = null;
            }
        }

        /// <summary>
        /// Gets list of groups for the selected domain from ldap by organization id.
        /// </summary>
        /// <param name="organizationId">OrganizationId.</param>
        /// <param name="domainName">Domain Name.</param>
        /// <returns>The System.Data.DataView that contains groups.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetGroups(Guid organizationId, string domainName)
        {
            DataTable table = null;
            Organization org = null;
            Domain domain = null;
            DomainUserGroupCollection groups = null;
            DomainUserGroup group = null;
            CommonDataSet.OrganizationsLdapGroupsDataTable orgTable = null;
            CommonDataSet.OrganizationsLdapGroupsRow dr = null;
            try
            {
                table = new DataTable("Groups");
                table.Locale = CultureInfo.CurrentCulture;
                table.Columns.Add("Id", typeof(Guid));
                table.Columns.Add("GroupName", typeof(string));

                orgTable = new CommonDataSet.OrganizationsLdapGroupsDataTable();
                WebApplication.CommonDataSetTableAdapters.OrganizationsLdapGroupsTableAdapter.Fill(orgTable, 0, organizationId, domainName);

                if (orgTable.Rows.Count > 0)
                {
                    for (int i = 0; i < orgTable.Rows.Count; i++)
                    {
                        dr = orgTable.Rows[i] as CommonDataSet.OrganizationsLdapGroupsRow;
                        table.Rows.Add(dr.ObjectGUID, dr.Name);
                    }
                    table.DefaultView.Sort = "[GroupName] asc";
                    return table.DefaultView;
                }
                else
                {
                    org = OrganizationProvider.GetOrganization(organizationId);

                    if (String.IsNullOrEmpty(domainName) == true || String.IsNullOrEmpty(org.LdapServerAddress) == true || String.IsNullOrEmpty(org.LdapServerPort) == true || String.IsNullOrEmpty(org.LdapUserName) == true || String.IsNullOrEmpty(org.LdapPassword) == true || String.IsNullOrEmpty(org.LdapDomain) == true)
                    {
                        table.Rows.Add(new Guid(), "No Data (select another domain)");
                        return table.DefaultView;
                    }

                    using (var server = new LdapProvider(organizationId, org.LdapServerAddress, Convert.ToInt32(org.LdapServerPort, CultureInfo.InvariantCulture), org.LdapUserName, org.LdapPassword, org.LdapDomain, "Default", null, null))
                    {
                        if (server.Initialize())
                        {
                            domain = server.GetDomainByName(domainName);
                            if (domain != null)
                            {
                                // output list of groups
                                groups = server.GetGroups(domain);
                                if (groups != null)
                                {
                                    // output list of domains
                                    for (int i = 0; i < groups.Count; i++)
                                    {
                                        group = groups[i];
                                        table.Rows.Add(group.GroupGuid, group.Name);
                                    }
                                }
                            }
                        }
                    }
                    if (table.Rows.Count == 0)
                        table.Rows.Add(new Guid(), "No Data");

                    table.DefaultView.Sort = "[GroupName] asc";

                    return table.DefaultView;
                }
            }
            finally
            {
                if (table != null) table.Dispose();
                if (orgTable != null) orgTable.Dispose();
                org = null;
                domain = null;
                groups = null;
                dr = null;
            }
        }


        /// <summary>
        /// Gets list of groups for the selected domain from ldap by organization id.
        /// </summary>        
        /// <param name="organizationId">OrganizationId.</param>        
        /// <param name="distinguishedName">Domain DistinguishedName.</param>
        /// <returns>The System.Data.DataView that contains groups.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetGroupsByDomainDistinguishedName(Guid organizationId, string distinguishedName)
        {
            DataTable table = null;
            Organization org = null;
            DomainUserGroupCollection groups = null;
            DomainUserGroup group = null;
            try
            {
                table = new DataTable("Groups");
                table.Locale = CultureInfo.CurrentCulture;
                table.Columns.Add("Id", typeof(Guid));
                table.Columns.Add("GroupName", typeof(string));
                table.Columns.Add("DistinguishedName", typeof(string));

                org = OrganizationProvider.GetOrganization(organizationId);

                if (String.IsNullOrEmpty(distinguishedName) == true || String.IsNullOrEmpty(org.LdapServerAddress) == true || String.IsNullOrEmpty(org.LdapServerPort) == true || String.IsNullOrEmpty(org.LdapUserName) == true || String.IsNullOrEmpty(org.LdapPassword) == true || String.IsNullOrEmpty(org.LdapDomain) == true)
                {
                    table.Rows.Add(new Guid(), "No Data (select another domain)", string.Empty);
                    return table.DefaultView;
                }

                using (var server = new LdapProvider(organizationId, org.LdapServerAddress, Convert.ToInt32(org.LdapServerPort, CultureInfo.InvariantCulture), org.LdapUserName, org.LdapPassword, org.LdapDomain, "Default", null, null))
                {
                    if (server.Initialize())
                    {
                        // output list of groups
                        groups = server.GetGroups(distinguishedName);
                        if (groups != null)
                        {
                            for (int i = 0; i < groups.Count; i++)
                            {
                                group = groups[i];
                                table.Rows.Add(group.GroupGuid, group.Name, group.DistinguishedName);
                            }
                        }
                    }
                }
                if (table.Rows.Count == 0)
                    table.Rows.Add(new Guid(), "No Data", string.Empty);

                table.DefaultView.Sort = "[GroupName] asc";

                return table.DefaultView;
            }
            finally
            {
                if (table != null) table.Dispose();
                org = null;
                group = null;
                groups = null;
            }
        }

        /// <summary>
        /// Gets list of mapped groups users for the selected organization.
        /// </summary>
        /// <param name="organizationId">OrganizationId.</param>
        /// <returns>The DomainUserCollection that contains mapped groups users.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DomainUserCollection GetMappedGroupsUsers(Guid organizationId, ref LdapProcess ldapProcess)
        {
            DomainUserCollection users = null;
            Organization org = OrganizationProvider.GetOrganization(organizationId);

            if (String.IsNullOrEmpty(org.LdapServerAddress) == true || String.IsNullOrEmpty(org.LdapServerPort) == true || String.IsNullOrEmpty(org.LdapUserName) == true || String.IsNullOrEmpty(org.LdapPassword) == true || String.IsNullOrEmpty(org.LdapDomain) == true)
                return null;

            DataView dv = GetGroupMappings(organizationId);
            if (dv.Count == 0) return null;

            string[] groupNames = new string[dv.Count];
            int i = 0;
            foreach (DataRowView drv in dv)
            {
                groupNames[i] = (string)drv["LdapGroupName"];
                i++;
            }
            
            ldapProcess.Logs.Add(new LdapProcessLog() { Date = DateTime.UtcNow, Message = string.Format(CultureInfo.CurrentCulture, Resources.LdapProcessLog_FoundMappedGroups, i) });

            using (var server = new LdapProvider(organizationId, org.LdapServerAddress, Convert.ToInt32(org.LdapServerPort, CultureInfo.InvariantCulture), org.LdapUserName, org.LdapPassword, org.LdapDomain, "Default", null, null))
            {
                if (server.Initialize())
                {                    
                    ldapProcess.Logs.Add(new LdapProcessLog() { Date = DateTime.UtcNow, Message = string.Format(CultureInfo.CurrentCulture, Resources.LdapProcessLog_ConnectedToLDAP, i) });
                    ldapProcess.Logs.Add(new LdapProcessLog() { Date = DateTime.UtcNow, Message = string.Format(CultureInfo.CurrentCulture, Resources.LdapProcessLog_GettingUsersFromMappedGroups, i) });
                    users = server.GetUsers(groupNames);
                    ldapProcess.Logs.Add(new LdapProcessLog() { Date = DateTime.UtcNow, Message = string.Format(CultureInfo.CurrentCulture, Resources.LdapProcessLog_FoundUsers, users.Count) });
                }
                else
                    ldapProcess.Logs.Add(new LdapProcessLog() { Date = DateTime.UtcNow, Message = string.Format(CultureInfo.CurrentCulture, Resources.LdapProcessLog_CantConnectToLDAP, i) });
            }

            return users;
        }

        /// <summary>
        /// Gets the group mappings.
        /// </summary>
        /// <param name="organizationId">OrganizationId.</param>
        /// <returns>The System.Data.DataView object that contains the group mappings.</returns>s
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetGroupMappings(Guid organizationId)
        {
            CommonDataSet.GroupMappingsDataTable table = new CommonDataSet.GroupMappingsDataTable();
            WebApplication.CommonDataSetTableAdapters.GroupMappingsTableAdapter.Fill(table, 0, organizationId);

            DataView dv = table.DefaultView;
            dv.Sort = "[GroupName] asc";

            return dv;
        }

        /// <summary>
        /// Inserts the group mappings.
        /// </summary>
        /// <param name="groupId">Application Group Id.</param>
        /// <param name="organizationId">Organization Id.</param>
        /// <param name="groupName">Application Group Name.</param>
        /// <param name="ldapDomainId">Ldap Domain Id.</param>
        /// <param name="ldapDomainName">Ldap Domain Name.</param>
        /// <param name="ldapGroupId">Ldap Group Id.</param>
        /// <param name="ldapGroupName">Ldap Group Name.</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertGroupMapping(Guid groupId, Guid organizationId, string groupName, Guid ldapDomainId, string ldapDomainName, Guid ldapGroupId, string ldapGroupName)
        {
            WebApplication.CommonDataSetTableAdapters.GroupMappingsTableAdapter.Insert(groupId, organizationId, groupName, ldapDomainId, ldapDomainName, ldapGroupId, ldapGroupName);
        }

        /// <summary>
        /// Deletes the group mappings.
        /// </summary>
        /// <param name="groupId">Group Id.</param>
        /// <param name="ldapDomainId">Ldap Domain Id.</param>
        /// <param name="ldapGroupId">Ldap Group Id.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteGroupMapping(Guid groupId, Guid ldapDomainId, Guid ldapGroupId)
        {
            WebApplication.CommonDataSetTableAdapters.GroupMappingsTableAdapter.Delete(groupId, ldapDomainId, ldapGroupId);
        }

        /// <summary>
        /// Gets the list of application groups.
        /// </summary>
        /// <param name="organizationId">OrganizationId.</param>
        /// <returns>The System.Data.DataView object that contains the application groups.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetAppGroups(Guid organizationId)
        {
            DataTable table = null;
            table = GroupProvider.GetGroups(organizationId);
            if (!table.Columns.Contains("SortNumber"))
            {
                table.Columns.Add("SortNumber", typeof(int), "IIF((GroupId = '00000000-0000-0000-0000-000000000000'), 0, 1)");

                DataRow row = table.NewRow();
                row["GroupId"] = Guid.Empty;
                row["OrganizationId"] = UserContext.Current.SelectedOrganizationId;
                row["Name"] = Resources.GroupProvider_OrganizationAdministratorGroupName;
                row["Description"] = Resources.GroupProvider_OrganizationAdministratorGroupDescription;
                row["BuiltIn"] = true;

                table.Rows.Add(row);
                table.AcceptChanges();
            }

            table.DefaultView.Sort = "BuiltIn DESC, SortNumber, Name";

            return table.DefaultView;
        }


        /// <summary>
        /// Gets the list of application groups with its instances/roles.
        /// </summary>
        /// <param name="organizationId">OrganizationId.</param>
        /// <returns>The System.Data.DataView object that contains the application groups with its instances/roles.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataView GetAppGroupsWithInstancesAndRoles(Guid organizationId)
        {
            DataTable table = null;
            table = GroupProvider.GetGroups(organizationId);
            if (!table.Columns.Contains("SortNumber"))
            {
                table.Columns.Add("SortNumber", typeof(int), "IIF((GroupId = '00000000-0000-0000-0000-000000000000'), 0, 1)");

                DataRow row = table.NewRow();
                row["GroupId"] = Guid.Empty;
                row["OrganizationId"] = UserContext.Current.SelectedOrganizationId;
                row["Name"] = Resources.GroupProvider_OrganizationAdministratorGroupName;
                row["Description"] = Resources.GroupProvider_OrganizationAdministratorGroupDescription;
                row["BuiltIn"] = true;

                table.Rows.Add(row);
                table.AcceptChanges();
            }

            table.DefaultView.Sort = "BuiltIn DESC, SortNumber, Name";

            System.Collections.Generic.List<Guid> groupIds = new List<Guid>();

            foreach (DataRow row in table.DefaultView.ToTable().Rows)
            {
                groupIds.Add(new Guid(row["GroupId"].ToString()));
            }

            return (GroupProvider.GetGroupsInstancesRoles(groupIds) ?? new DataTable()).DefaultView;
        }

        /// <summary>
        /// Gets the list of application groups mapping to the given ldap groups.
        /// </summary>
        /// <param name="organizationId">OrganizationId.</param>
        /// <param name="groupId">List of ldap groups separated by comma.</param>
        /// <returns>String that contains the application groups separated by comma.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static string GetAppGroupsByLdapGroups(Guid organizationId, string groupId)
        {
            StringBuilder result = new StringBuilder();

            if (string.IsNullOrEmpty(groupId)) return string.Empty;
            groupId = string.Concat(",", groupId, ",").Replace("," + Guid.Empty.ToString() + ",", ",");

            DataTable table = GetGroupMappings(organizationId).Table;

            if (groupId.Replace(",", string.Empty).Length > 0)
            {
                foreach (string currentGroupId in groupId.Split(','))
                {
                    if (string.IsNullOrEmpty(currentGroupId)) continue;

                    foreach (DataRow row in table.Select(string.Format(CultureInfo.CurrentCulture, "LdapGroupId='{0}'", currentGroupId)))
                        if (!result.ToString().Contains(Convert.ToString((Guid)row["GroupId"], CultureInfo.CurrentCulture)))
                            result.AppendFormat(CultureInfo.CurrentCulture, ",{0}", (Guid)row["GroupId"]);
                }
            }

            return (result.Length > 0) ? result.Remove(0, 1).ToString() : string.Empty;
        }

        /// <summary>
        /// Gets the list of application groups mapping to the given ldap groups.
        /// </summary>
        /// <param name="organizationId">OrganizationId.</param>
        /// <param name="groupId">List of ldap groups.</param>
        /// <returns>String that contains the application groups separated by comma.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static string GetAppGroupsByLdapGroups(Guid organizationId, Dictionary<Guid, GroupListItemValue> groupId)
        {
            StringBuilder result = new StringBuilder();
            if (groupId == null) return string.Empty;

            string groupList = string.Empty;
            foreach (Guid key in groupId.Keys)
                groupList = string.Concat(groupList, key.ToString("D", CultureInfo.CurrentCulture), ",");

            if (string.IsNullOrEmpty(groupList))
                return string.Empty;
            else
                groupList = groupList.Substring(0, groupList.Length - 1);

            groupList = string.Concat(",", groupList, ",").Replace("," + Guid.Empty.ToString() + ",", ",");

            DataTable table = GetGroupMappings(organizationId).Table;

            if (groupList.Replace(",", string.Empty).Length > 0)
            {
                foreach (string currentGroupId in groupList.Split(','))
                {
                    if (string.IsNullOrEmpty(currentGroupId)) continue;

                    foreach (DataRow row in table.Select(string.Format(CultureInfo.CurrentCulture, "LdapGroupId='{0}'", currentGroupId)))
                        if (!result.ToString().Contains(Convert.ToString((Guid)row["GroupId"], CultureInfo.CurrentCulture)))
                            result.AppendFormat(CultureInfo.CurrentCulture, ",{0}", (Guid)row["GroupId"]);
                }
            }

            return (result.Length > 0) ? result.Remove(0, 1).ToString() : string.Empty;
        }

        /// <summary>
        /// Gets the list of application groups and if exists mapped ldap groups for the user.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="userId">The identifier of the user.</param>
        /// <returns>The DataView object populated with information of the user groups.</returns>
        public static DataView GetUserLdapGroups(Guid organizationId, Guid userId)
        {
            DataTable resultTable = null;

            Organization org = OrganizationProvider.GetOrganization(organizationId);

            if (String.IsNullOrEmpty(org.LdapDomain) == true || String.IsNullOrEmpty(org.LdapServerAddress) == true || String.IsNullOrEmpty(org.LdapServerPort) == true || String.IsNullOrEmpty(org.LdapUserName) == true || String.IsNullOrEmpty(org.LdapPassword) == true || String.IsNullOrEmpty(org.LdapDomain) == true)
                return null;

            using (var server = new LdapProvider(organizationId, org.LdapServerAddress, Convert.ToInt32(org.LdapServerPort, CultureInfo.InvariantCulture), org.LdapUserName, org.LdapPassword, org.LdapDomain, "Default", null, null))
            {
                if (server.Initialize())
                {
                    Dictionary<Guid, GroupListItemValue> ldapGroups = null;

                    DataView dv = WebApplication.LoginProvider.GetUserLdapInfo(organizationId, userId);
                    if (dv == null || dv.Table.Rows.Count == 0 || (dv.Table.Rows[0]["LdapUserId"]).GetType() != typeof(Guid) || (Guid)dv.Table.Rows[0]["LdapUserId"] == Guid.Empty)
                    {
                        OrganizationDataSet ds = WebApplication.GetOrganizationDataSetByOrganizationId(organizationId);
                        if (ds == null) return null;

                        OrganizationDataSet.UserDataTable userTable = ds.User;
                        OrganizationDataSet.UserRow userRow = userTable.FindByUserId(userId);
                        if (userRow == null) return null;

                        if (string.IsNullOrEmpty(userRow.Email) == false)
                            ldapGroups = server.GetUserGroupsByEmail(userRow.Email);
                        else
                        {
                            DataRowView drv = WebApplication.LoginProvider.GetLogin(userRow.UserId);
                            ldapGroups = server.GetUserGroupsByPrincipalName((string)drv["LoginName"]);
                        }
                    }
                    else
                    {
                        ldapGroups = server.GetUserGroupsById((Guid)dv.Table.Rows[0]["LdapUserId"]);
                    }

                    DataView groupMappingsView = GetGroupMappings(organizationId);
                    resultTable = groupMappingsView.Table.Clone();
                    resultTable.Columns.Add("IsDirect", typeof(bool));

                    if (ldapGroups != null)
                        foreach (KeyValuePair<Guid, GroupListItemValue> kvp in ldapGroups)
                        {
                            DataRow[] drs = groupMappingsView.Table.Select(string.Concat("LdapGroupId='", kvp.Key, "'"));
                            if (drs.Length > 0)
                            {
                                foreach (DataRow dr in drs)
                                {
                                    DataRow row = resultTable.NewRow();
                                    row["GroupId"] = (Guid)dr["GroupId"];
                                    row["OrganizationId"] = (Guid)dr["OrganizationId"];
                                    row["GroupName"] = (string)dr["GroupName"];
                                    row["LdapDomainId"] = (Guid)dr["LdapDomainId"];
                                    row["LdapDomainName"] = (string)dr["LdapDomainName"];
                                    row["LdapGroupId"] = (Guid)dr["LdapGroupId"];
                                    row["LdapGroupName"] = (string)dr["LdapGroupName"];
                                    row["IsDirect"] = kvp.Value.IsDirect;
                                    resultTable.Rows.Add(row);
                                }
                            }
                            else
                            {
                                DataRow row = resultTable.NewRow();
                                row["GroupId"] = Guid.Empty;
                                row["OrganizationId"] = organizationId;
                                row["GroupName"] = string.Empty;
                                row["LdapDomainId"] = Guid.Empty;
                                row["LdapDomainName"] = string.Empty;
                                row["LdapGroupId"] = kvp.Key;
                                row["LdapGroupName"] = kvp.Value.Name;
                                row["IsDirect"] = kvp.Value.IsDirect;
                                resultTable.Rows.Add(row);
                            }
                        }
                }
            }

            if (resultTable != null)
                resultTable.DefaultView.Sort = "[LdapGroupName]";

            return resultTable != null ? resultTable.DefaultView : new DataView();
        }

        /// <summary>
        /// Gets the list of ldap group ids of ldap user separated by comma.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="userId">The identifier of the ldap user.</param>
        /// <returns>The list of ldap user group ids separated by comma.</returns>
        public static string GetLdapUserGroupIds(Guid organizationId, Guid userId)
        {
            StringBuilder sb = new StringBuilder();

            Organization org = OrganizationProvider.GetOrganization(organizationId);

            if (String.IsNullOrEmpty(org.LdapDomain) == true || String.IsNullOrEmpty(org.LdapServerAddress) == true || String.IsNullOrEmpty(org.LdapServerPort) == true || String.IsNullOrEmpty(org.LdapUserName) == true || String.IsNullOrEmpty(org.LdapPassword) == true || String.IsNullOrEmpty(org.LdapDomain) == true)
                return sb.ToString();

            using (var server = new LdapProvider(organizationId, org.LdapServerAddress, Convert.ToInt32(org.LdapServerPort, CultureInfo.InvariantCulture), org.LdapUserName, org.LdapPassword, org.LdapDomain, "Default", null, null))
            {
                if (server.Initialize())
                {
                    Dictionary<Guid, GroupListItemValue> ldapGroups = server.GetUserGroupsById(userId);

                    if (ldapGroups != null)
                        foreach (Guid key in ldapGroups.Keys)
                            sb.AppendFormat(CultureInfo.CurrentCulture, ",{0}", key);
                }
            }

            return (sb.Length > 0) ? sb.Remove(0, 1).ToString() : string.Empty;
        }

        /// <summary>
        /// Gets the ldap user.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="userId">The identifier of the ldap user.</param>
        /// <returns>The ldap user.</returns>
        public static IUser GetLdapUser(Guid organizationId, Guid userId)
        {
            IUser user = null;

            Organization org = OrganizationProvider.GetOrganization(organizationId);

            if (String.IsNullOrEmpty(org.LdapDomain) == true || String.IsNullOrEmpty(org.LdapServerAddress) == true || String.IsNullOrEmpty(org.LdapServerPort) == true || String.IsNullOrEmpty(org.LdapUserName) == true || String.IsNullOrEmpty(org.LdapPassword) == true || String.IsNullOrEmpty(org.LdapDomain) == true)
                return null;

            using (var server = new LdapProvider(organizationId, org.LdapServerAddress, Convert.ToInt32(org.LdapServerPort, CultureInfo.InvariantCulture), org.LdapUserName, org.LdapPassword, org.LdapDomain, "Default", null, null))
            {
                if (server.Initialize())
                    user = server.GetIUser(userId);
            }

            return user;
        }

        /// <summary>
        /// Gets the ldap user.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="ldapUser">DomainUser.</param>
        /// <returns>The ldap user.</returns>
        public static IUser GetLdapUser(Guid organizationId, DomainUser ldapUser)
        {
            IUser user = null;

            Organization org = OrganizationProvider.GetOrganization(organizationId);

            if (String.IsNullOrEmpty(org.LdapDomain) == true || String.IsNullOrEmpty(org.LdapServerAddress) == true || String.IsNullOrEmpty(org.LdapServerPort) == true || String.IsNullOrEmpty(org.LdapUserName) == true || String.IsNullOrEmpty(org.LdapPassword) == true || String.IsNullOrEmpty(org.LdapDomain) == true)
                return null;

            using (var server = new LdapProvider(organizationId, org.LdapServerAddress, Convert.ToInt32(org.LdapServerPort, CultureInfo.InvariantCulture), org.LdapUserName, org.LdapPassword, org.LdapDomain, "Default", null, null))
            {
                user = server.convertDomainUserToUser(ldapUser);
            }

            return user;
        }

        /// <summary>
        /// Gets the ldap user.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="userName">The user name.</param>
        /// <returns>The ldap user.</returns>
        public static IUser GetLdapUser(Guid organizationId, string userName)
        {
            IUser user = null;

            Organization org = OrganizationProvider.GetOrganization(organizationId);

            if (String.IsNullOrEmpty(org.LdapDomain) == true || String.IsNullOrEmpty(org.LdapServerAddress) == true || String.IsNullOrEmpty(org.LdapServerPort) == true || String.IsNullOrEmpty(org.LdapUserName) == true || String.IsNullOrEmpty(org.LdapPassword) == true || String.IsNullOrEmpty(org.LdapDomain) == true)
                return null;

            ApplicationLogger log = new ApplicationLogger();
            LdapIntegration ldi = new LdapIntegration(log);

            using (var server = new LdapProvider(organizationId, org.LdapServerAddress, Convert.ToInt32(org.LdapServerPort, CultureInfo.InvariantCulture), org.LdapUserName, org.LdapPassword, org.LdapDomain, "Default", ldi, log))
            {
                if (server.Initialize())
                    user = server.GetIUser(userName);
            }

            return user;
        }

        /// <summary>
        /// Gets the ldap user alt emails.
        /// </summary>
        /// <param name="organizationId">The identifier of the organization.</param>
        /// <param name="userId">The identifier of the ldap user.</param>
        /// <returns>The ldap user alt emails.</returns>
        public static ReadOnlyCollection<string> GetLdapUserAltEmails(Guid organizationId, Guid userId, bool includeMainEmail)
        {
            ReadOnlyCollection<string> list = null;

            Organization org = OrganizationProvider.GetOrganization(organizationId);

            if (String.IsNullOrEmpty(org.LdapDomain) == true || String.IsNullOrEmpty(org.LdapServerAddress) == true || String.IsNullOrEmpty(org.LdapServerPort) == true || String.IsNullOrEmpty(org.LdapUserName) == true || String.IsNullOrEmpty(org.LdapPassword) == true || String.IsNullOrEmpty(org.LdapDomain) == true)
                return null;

            using (var server = new LdapProvider(organizationId, org.LdapServerAddress, Convert.ToInt32(org.LdapServerPort, CultureInfo.InvariantCulture), org.LdapUserName, org.LdapPassword, org.LdapDomain, "Default", null, null))
            {
                if (server.Initialize())
                    list = server.GetUserAltEmails(userId, includeMainEmail);
            }

            return list;
        }

        /// <summary>
        /// Updates the organization ldap domains.
        /// </summary>
        /// <param name="organizationId">Organization Id.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void UpdateLdapDomains(Guid organizationId)
        {
            Organization org = OrganizationProvider.GetOrganization(organizationId);

            if (String.IsNullOrEmpty(org.LdapServerAddress) != true && String.IsNullOrEmpty(org.LdapServerPort) != true && String.IsNullOrEmpty(org.LdapUserName) != true && String.IsNullOrEmpty(org.LdapPassword) != true && String.IsNullOrEmpty(org.LdapDomain) != true)
            {
                using (var server = new LdapProvider(organizationId, org.LdapServerAddress, Convert.ToInt32(org.LdapServerPort, CultureInfo.InvariantCulture), org.LdapUserName, org.LdapPassword, org.LdapDomain, "Default", null, null))
                {
                    if (server.Initialize())
                    {
                        string newList = org.LdapDomains;
                        List<string> currentList = new List<string>();
                        foreach (string elem in org.LdapDomains.Split(','))
                            if (elem.Trim().Length > 0) currentList.Add(elem.Trim());

                        var domains = server.GetDomains();
                        if (domains != null)
                        {
                            // output list of domains
                            for (int i = 0; i < domains.Count; i++)
                            {
                                if (domains[i].Name != "DomainDnsZones" && domains[i].Name != "ForestDnsZones")
                                {
                                    if (!currentList.Contains(domains[i].Name)) newList = (newList.Length > 0) ? string.Concat(newList, ",", domains[i].Name) : domains[i].Name;
                                    string fullDomainName = server.GetDomainFullByUserDN(domains[i].DistinguishedName);
                                    if (!currentList.Contains(fullDomainName)) newList = (newList.Length > 0) ? string.Concat(newList, ",", fullDomainName) : fullDomainName;
                                }
                            }

                            OrganizationProvider.UpdateOrganization(organizationId, newList);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
