using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Diagnostics;
using System.Globalization;

using Micajah.Common.Bll.Providers;
using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.LdapAdapter;
using Micajah.Common.Dal;
using Micajah.Common.Properties;

namespace Micajah.Common.Bll.Handlers
{
    public class LdapHandler : IThreadStateProvider
    {
        #region Public Properties

        public ThreadStateType ThreadState { get; set; }
        public Exception ErrorException { get; set; }

        #endregion

        #region Public Methods

        public void Start()
        {
            OrganizationCollection organizationCollection = null;
            DataView dvDomains = null;
            DataView dvGroups = null;

            DataRow drDomain = null;
            DataRow drGroup = null;

            try
            {
                this.ThreadState = ThreadStateType.Running;

                if (FrameworkConfiguration.Current.WebApplication.Integration.Ldap.Enabled)
                {
                    organizationCollection = OrganizationProvider.GetOrganizations(false, false);

                    foreach (Organization org in organizationCollection)
                    {
                        try
                        {
                            if (String.IsNullOrEmpty(org.LdapServerAddress) == true || String.IsNullOrEmpty(org.LdapServerPort) == true || String.IsNullOrEmpty(org.LdapUserName) == true || String.IsNullOrEmpty(org.LdapPassword) == true || String.IsNullOrEmpty(org.LdapDomain) == true || !org.Beta)
                                continue;

                            //Get All Groups
                            dvDomains = LdapInfoProvider.GetDomains(org.OrganizationId);
                            if (dvDomains.Table.Rows.Count > 0)
                            {
                                for (int i = 0; i < dvDomains.Table.Rows.Count; i++)
                                {
                                    drDomain = dvDomains.Table.Rows[i];
                                    dvGroups = LdapInfoProvider.GetGroupsByDomainDistinguishedName(org.OrganizationId, drDomain["DistinguishedName"].ToString());
                                    if (dvGroups.Table.Rows.Count > 0)
                                    {
                                        WebApplication.CommonDataSetTableAdapters.OrganizationsLdapGroupsTableAdapter.Delete(org.OrganizationId, drDomain["DomainName"].ToString());
                                        for (int j = 0; j < dvGroups.Table.Rows.Count; j++)
                                        {
                                            drGroup = dvGroups.Table.Rows[j];
                                            WebApplication.CommonDataSetTableAdapters.OrganizationsLdapGroupsTableAdapter.Insert(Guid.NewGuid(), org.OrganizationId, drDomain["Id"], drDomain["DomainName"].ToString(), drGroup["Id"], drGroup["GroupName"], drGroup["DistinguishedName"], DateTime.UtcNow);
                                        }
                                    }
                                }
                            }

                            RunADReplication(org.OrganizationId, true);
                        }
                        catch (Exception ex)
                        {
                            this.ThreadState = ThreadStateType.Failed;
                            this.ErrorException = ex;
                            try
                            {
                                if (!EventLog.SourceExists("Micajah.Common.Bll.Handlers.LdapHandler"))
                                    EventLog.CreateEventSource("Micajah.Common.Bll.Handlers.LdapHandler", "Application");
                                EventLog.WriteEntry("Micajah.Common.Bll.Handlers.LdapHandler", string.Format(CultureInfo.InvariantCulture, "Error in method Start() for Organization: {0}; Error: {1} [{2}]", org.Name, ex.Message, ex.ToString()), System.Diagnostics.EventLogEntryType.Error);
                            }
                            catch { }
                        }
                    }
                }

                if (this.ThreadState == ThreadStateType.Running)
                    this.ThreadState = ThreadStateType.Finished;
            }
            catch (Exception ex)
            {
                this.ThreadState = ThreadStateType.Failed;
                this.ErrorException = ex;
                try
                {
                    if (!EventLog.SourceExists("Micajah.Common.Bll.Handlers.LdapHandler"))
                        EventLog.CreateEventSource("Micajah.Common.Bll.Handlers.LdapHandler", "Application");
                    EventLog.WriteEntry("Micajah.Common.Bll.Handlers.LdapHandler", string.Format(CultureInfo.InvariantCulture, "Error in method Start() Error: {0} [{1}]", ex.Message, ex.ToString()), System.Diagnostics.EventLogEntryType.Error);
                }
                catch { }
            }
            finally
            {
                organizationCollection = null;
                drDomain = null;
                if (dvDomains != null) dvDomains.Dispose();
                if (dvGroups != null) dvGroups.Dispose();
            }
        }

        public void ImportLdapGroups(Guid organizationId)
        {
            Organization organization = null;
            DataView dvDomains = null;
            DataView dvGroups = null;

            DataRow drDomain = null;
            DataRow drGroup = null;

            try
            {
                this.ThreadState = ThreadStateType.Running;
                if (FrameworkConfiguration.Current.WebApplication.Integration.Ldap.Enabled)
                {
                    organization = OrganizationProvider.GetOrganization(organizationId);

                    if (organization != null)
                    {
                        if (String.IsNullOrEmpty(organization.LdapServerAddress) == true || String.IsNullOrEmpty(organization.LdapServerPort) == true || String.IsNullOrEmpty(organization.LdapUserName) == true || String.IsNullOrEmpty(organization.LdapPassword) == true || String.IsNullOrEmpty(organization.LdapDomain) == true || !organization.Beta)
                            return;

                        dvDomains = LdapInfoProvider.GetDomains(organization.OrganizationId);
                        if (dvDomains.Table.Rows.Count > 0)
                        {
                            for (int i = 0; i < dvDomains.Table.Rows.Count; i++)
                            {
                                drDomain = dvDomains.Table.Rows[i];
                                dvGroups = LdapInfoProvider.GetGroupsByDomainDistinguishedName(organization.OrganizationId, drDomain["DistinguishedName"].ToString());
                                if (dvGroups.Table.Rows.Count > 0)
                                {
                                    WebApplication.CommonDataSetTableAdapters.OrganizationsLdapGroupsTableAdapter.Delete(organization.OrganizationId, drDomain["DomainName"].ToString());
                                    for (int j = 0; j < dvGroups.Table.Rows.Count; j++)
                                    {
                                        drGroup = dvGroups.Table.Rows[j];
                                        WebApplication.CommonDataSetTableAdapters.OrganizationsLdapGroupsTableAdapter.Insert(Guid.NewGuid(), organization.OrganizationId, drDomain["Id"], drDomain["DomainName"].ToString(), drGroup["Id"], drGroup["GroupName"], drGroup["DistinguishedName"], DateTime.UtcNow);
                                    }
                                }
                            }
                        }
                    }
                }

                this.ThreadState = ThreadStateType.Finished;
            }
            catch (Exception ex)
            {
                this.ThreadState = ThreadStateType.Failed;
                this.ErrorException = ex;
                try
                {
                    if (!EventLog.SourceExists("Micajah.Common.Bll.Handlers.LdapHandler"))
                        EventLog.CreateEventSource("Micajah.Common.Bll.Handlers.LdapHandler", "Application");
                    EventLog.WriteEntry("Micajah.Common.Bll.Handlers.LdapHandler", string.Format(CultureInfo.InvariantCulture, "Error in method ImportLdapGroups() for organizationId: {0}; Error: {1} [{2}]", organizationId, ex.Message, ex.ToString()), System.Diagnostics.EventLogEntryType.Error);
                }
                catch { }
            }
            finally
            {
                organization = null;
                drDomain = null;
                if (dvDomains != null) dvDomains.Dispose();
                if (dvGroups != null) dvGroups.Dispose();
            }
        }

        public void RunADReplication(Guid organizationId, bool isRealReplication)
        {
            string processId = null;
            Bll.LdapProcess ldapProcess = null;
            DomainUserCollection users = null;
            DataTable localLogins = null;
            DataTable ldapLogins = null;
            //DataTable localMappedLogins = null;
            DataTable activeLMLogins = null;
            DataTable inactiveLMLogins = null;
            DataTable ldapActiveLogins = null;
            DataColumn newColumn = null;
            DataRow row = null;
            DataRow newRow = null;
            CommonDataSet.OrganizationsLdapGroupsDataTable orgTable = null;
            CommonDataSet.GroupMappingsDataTable groupMappings = null;
            Bll.Handlers.LdapHandler ldapHendler = null;
            try
            {

                if (isRealReplication)
                    processId = string.Format(CultureInfo.InvariantCulture, "RealADReplication_{0}", organizationId);
                else
                    processId = string.Format(CultureInfo.InvariantCulture, "TestADReplication_{0}", organizationId);

                ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                if (ldapProcess != null)
                    LdapInfoProvider.LdapProcesses.Remove(ldapProcess);

                ldapProcess = new Bll.LdapProcess();
                ldapProcess.ProcessId = processId;
                ldapProcess.ThreadStateType = Bll.ThreadStateType.Running;
                ldapProcess.MessageError = string.Empty;
                ldapProcess.MessageCreatedLogins = string.Empty;
                ldapProcess.MessageActivatedLogins = string.Empty;
                ldapProcess.MessageDeactivatedLogins = string.Empty;
                ldapProcess.DataCreatedLogins = null;
                ldapProcess.DataActivatedLogins = null;
                ldapProcess.DataDeactivatedLogins = null;
                ldapProcess.Logs = new List<LdapProcessLog>();
                LdapInfoProvider.LdapProcesses.Add(ldapProcess);

                if (isRealReplication)
                {
                    ldapProcess.MessageDeactivatedLogins = string.Format(CultureInfo.InvariantCulture, Resources.OrganizationLdapSettingsControl_RealDeactivatedLogins_Text, 0);
                    ldapProcess.MessageActivatedLogins = string.Format(CultureInfo.InvariantCulture, Resources.OrganizationLdapSettingsControl_RealActivatedLogins_Text, 0);
                    ldapProcess.MessageCreatedLogins = string.Format(CultureInfo.InvariantCulture, Resources.OrganizationLdapSettingsControl_RealCreatedLogins_Text, 0);
                }
                else
                {
                    ldapProcess.MessageDeactivatedLogins = string.Format(CultureInfo.InvariantCulture, Resources.OrganizationLdapSettingsControl_TestDeactivatedLogins_Text, 0);
                    ldapProcess.MessageActivatedLogins = string.Format(CultureInfo.InvariantCulture, Resources.OrganizationLdapSettingsControl_TestActivatedLogins_Text, 0);
                    ldapProcess.MessageCreatedLogins = string.Format(CultureInfo.InvariantCulture, Resources.OrganizationLdapSettingsControl_TestCreatedLogins_Text, 0);
                }

                ldapProcess.Logs.Add(new LdapProcessLog() { Date = DateTime.UtcNow, Message = string.Format(CultureInfo.CurrentCulture, Resources.LdapProcessLog_ReplicationStarted) });
                DateTime startDate = DateTime.UtcNow;

                // Get all mapped ldap groups users
                users = LdapInfoProvider.GetMappedGroupsUsers(organizationId, ref ldapProcess);

                if (users != null)
                {
                    // Get all local users
                    localLogins = WebApplication.LoginProvider.GetLoginsByOrganizationId(organizationId);
                    localLogins.Columns.Add("Name", typeof(string));
                    foreach (DataRow dataRow in localLogins.Rows)
                        dataRow["Name"] = string.Concat(dataRow["FirstName"], " ", dataRow["LastName"]);

                    ldapLogins = localLogins.Clone();
                    ldapLogins.Columns.Add("Email", typeof(string));
                    foreach (DomainUser user in users)
                    {
                        if (!string.IsNullOrEmpty(user.EmailAddress) || !string.IsNullOrEmpty(user.PrincipalName))
                        {
                            row = ldapLogins.NewRow();
                            row["LoginId"] = Guid.Empty;
                            row["LoginName"] = (string.IsNullOrEmpty(user.EmailAddress)) ? user.PrincipalName : user.EmailAddress;
                            row["Name"] = string.Concat(user.FirstName, " ", user.LastName);
                            row["FirstName"] = user.FirstName;
                            row["LastName"] = user.LastName;
                            row["Email"] = (string.IsNullOrEmpty(user.EmailAddress)) ? ((user.PrincipalName ?? string.Empty).Contains("@") ? user.PrincipalName : string.Empty) : user.EmailAddress;
                            row["LdapUserId"] = user.ObjectGuid;
                            row["Active"] = user.IsActive;
                            ldapLogins.Rows.Add(row);
                        }
                    }

                    //// Get mapped local users
                    //localMappedLogins = localLogins.Clone();
                    ////foreach (DataRow localLogin in localLogins.Select(string.Format(CultureInfo.InvariantCulture, "(LdapUserId IS NOT NULL) AND (LdapUserId <> '{0}')", Guid.Empty)))
                    //foreach (DataRow localLogin in localLogins.Rows)
                    //{
                    //    newRow = localMappedLogins.NewRow();
                    //    newRow.ItemArray = localLogin.ItemArray;
                    //    localMappedLogins.Rows.Add(newRow);
                    //}

                    // Get active mapped local users
                    activeLMLogins = localLogins.Clone();
                    foreach (DataRow activeLM in localLogins.Select("(Active = 1)"))
                    {
                        newRow = activeLMLogins.NewRow();
                        newRow.ItemArray = activeLM.ItemArray;
                        activeLMLogins.Rows.Add(newRow);
                    }

                    LocalUsersDeactivate(activeLMLogins, ldapLogins, organizationId, ref ldapProcess, isRealReplication);

                    // Get inactive mapped local users
                    inactiveLMLogins = localLogins.Clone();
                    foreach (DataRow inactiveLM in localLogins.Select("(Active = 0)"))
                    {
                        newRow = inactiveLMLogins.NewRow();
                        newRow.ItemArray = inactiveLM.ItemArray;
                        inactiveLMLogins.Rows.Add(newRow);
                    }

                    LocalUsersActivate(inactiveLMLogins, ldapLogins, organizationId, ref ldapProcess, isRealReplication);

                    // Get active ldap users
                    ldapActiveLogins = ldapLogins.Clone();
                    foreach (DataRow ldapActive in ldapLogins.Select("(Active = 1)"))
                    {
                        if (!string.IsNullOrEmpty(ldapActive["LoginName"] != null ? ldapActive["LoginName"].ToString() : string.Empty))
                        {
                            newRow = ldapActiveLogins.NewRow();
                            newRow.ItemArray = ldapActive.ItemArray;
                            ldapActiveLogins.Rows.Add(newRow);
                        }
                    }
                    newColumn = new DataColumn("Processed", typeof(bool));
                    newColumn.DefaultValue = false;
                    ldapActiveLogins.Columns.Add(newColumn);

                    orgTable = new CommonDataSet.OrganizationsLdapGroupsDataTable();
                    WebApplication.CommonDataSetTableAdapters.OrganizationsLdapGroupsTableAdapter.Fill(orgTable, 3, organizationId);

                    if (orgTable.Rows.Count == 0)
                    {
                        ldapHendler = new Bll.Handlers.LdapHandler();
                        ldapHendler.ImportLdapGroups(organizationId);
                        orgTable = new CommonDataSet.OrganizationsLdapGroupsDataTable();
                        WebApplication.CommonDataSetTableAdapters.OrganizationsLdapGroupsTableAdapter.Fill(orgTable, 3, organizationId);
                    }

                    groupMappings = new CommonDataSet.GroupMappingsDataTable();
                    WebApplication.CommonDataSetTableAdapters.GroupMappingsTableAdapter.Fill(groupMappings, 0, organizationId);

                    if (isRealReplication)
                    {
                        ldapProcess.Logs.Add(new LdapProcessLog() { Date = DateTime.UtcNow, Message = string.Format(CultureInfo.CurrentCulture, Resources.LdapProcessLog_UpdatingUserAccounts, users.Count) });
                        LocalUsersCheckGroups(organizationId, activeLMLogins, users, orgTable, groupMappings);
                        ldapProcess.Logs.Add(new LdapProcessLog() { Date = DateTime.UtcNow, Message = string.Format(CultureInfo.CurrentCulture, Resources.LdapProcessLog_UpdateFinished) });
                    }

                    LocalUsersCreate(localLogins, ldapActiveLogins, organizationId, ref ldapProcess, users, orgTable, groupMappings, isRealReplication);
                }

                ldapProcess.ThreadStateType = Bll.ThreadStateType.Finished;
                ldapProcess.Logs.Add(new LdapProcessLog() { Date = DateTime.UtcNow, Message = string.Format(CultureInfo.CurrentCulture, Resources.LdapProcessLog_ReplicationFinished, Math.Round((DateTime.UtcNow - startDate).TotalMinutes, 1)) });
            }
            catch (Exception ex)
            {
                ldapProcess = LdapInfoProvider.LdapProcesses.Find(x => x.ProcessId == processId);
                if (ldapProcess != null)
                {
                    ldapProcess.ProcessId = processId;
                    ldapProcess.ThreadStateType = Bll.ThreadStateType.Failed;
                    ldapProcess.MessageError = string.Format(CultureInfo.InvariantCulture, "<br/>{0}", ex.ToString().Replace("\r\n", "<br/>"));
                    ldapProcess.MessageCreatedLogins = string.Empty;
                    ldapProcess.MessageActivatedLogins = string.Empty;
                    ldapProcess.MessageDeactivatedLogins = string.Empty;
                    ldapProcess.DataCreatedLogins = null;
                    ldapProcess.DataActivatedLogins = null;
                    ldapProcess.DataDeactivatedLogins = null;
                }

                try
                {
                    if (!EventLog.SourceExists("Micajah.Common.Bll.Handlers.LdapHandler"))
                        EventLog.CreateEventSource("Micajah.Common.Bll.Handlers.LdapHandler", "Application");
                    EventLog.WriteEntry("Micajah.Common.Bll.Handlers.LdapHandler", string.Format(CultureInfo.InvariantCulture, "Error in method RunADReplication() Error: {0} [{1}]", ex.Message, ex.ToString()), System.Diagnostics.EventLogEntryType.Error);
                }
                catch { }
            }
            finally
            {
                processId = null;
                ldapProcess = null;
                users = null;
                if (localLogins != null) localLogins.Dispose();
                if (ldapLogins != null) ldapLogins.Dispose();
                //if (localMappedLogins != null) localMappedLogins.Dispose();
                if (activeLMLogins != null) activeLMLogins.Dispose();
                if (inactiveLMLogins != null) inactiveLMLogins.Dispose();
                if (ldapActiveLogins != null) ldapActiveLogins.Dispose();
                if (newColumn != null) newColumn.Dispose();
                if (orgTable != null) orgTable.Dispose();
                row = null;
                newRow = null;
                if (groupMappings != null) groupMappings.Dispose();
                ldapHendler = null;
            }
        }

        private void LocalUsersDeactivate(DataTable localUsers, DataTable ldapUsers, Guid organizationId, ref Bll.LdapProcess ldapProcess, bool isRealReplication)
        {
            int count = 0;
            DataTable newTable = null;
            DataRow[] drm = null;
            DataRow newRow = null;
            try
            {
                count = 0;
                newTable = localUsers.Clone();
                foreach (DataRow dr in localUsers.Rows)
                {
                    drm = ldapUsers.Select(string.Concat("(LdapUserId='", dr["LdapUserId"], "')"));
                    if (drm.Length > 0)
                    {
                        drm = ldapUsers.Select(string.Concat("(LdapUserId='", dr["LdapUserId"], "') AND (Active = 1)"));
                        if (drm.Length == 0)
                        {
                            if (isRealReplication)
                                UserProvider.UpdateUserActive((Guid)dr["LoginId"], organizationId, false);

                            newRow = newTable.NewRow();
                            newRow.ItemArray = dr.ItemArray;
                            newTable.Rows.Add(newRow);
                            count++;

                            ldapProcess.MessageDeactivatedLogins = string.Format(CultureInfo.InvariantCulture, isRealReplication ? Resources.OrganizationLdapSettingsControl_RealDeactivatedLogins_Text : Resources.OrganizationLdapSettingsControl_TestDeactivatedLogins_Text, count);
                        }
                    }
                }

                ldapProcess.MessageDeactivatedLogins = string.Format(CultureInfo.InvariantCulture, isRealReplication ? Resources.OrganizationLdapSettingsControl_RealDeactivatedLogins_Text : Resources.OrganizationLdapSettingsControl_TestDeactivatedLogins_Text, count);

                if (count > 0)
                {
                    newTable.DefaultView.Sort = "LoginName";
                    ldapProcess.DataDeactivatedLogins = Micajah.Common.Bll.Support.TrimDataView(newTable.DefaultView, 25);
                }

                ldapProcess.Logs.Add(new LdapProcessLog() { Date = DateTime.UtcNow, Message = string.Format(CultureInfo.CurrentCulture, isRealReplication ? Resources.LdapProcessLog_RealReplicationUsersInactivated : Resources.LdapProcessLog_TestReplicationUsersInactivated, count) });
            }
            finally
            {
                count = 0;
                if (newTable != null) newTable.Dispose();
                drm = null;
                newRow = null;
            }
        }

        private static void LocalUsersActivate(DataTable localUsers, DataTable ldapUsers, Guid organizationId, ref Bll.LdapProcess ldapProcess, bool isRealReplication)
        {
            int count = 0;
            DataTable newTable = null;
            DataRow[] drm = null;
            DataRow newRow = null;
            try
            {
                count = 0;
                newTable = localUsers.Clone();
                foreach (DataRow dr in localUsers.Rows)
                {
                    drm = ldapUsers.Select(string.Concat("(LdapUserId='", dr["LdapUserId"], "')"));
                    if (drm.Length > 0)
                    {
                        drm = ldapUsers.Select(string.Concat("(LdapUserId='", dr["LdapUserId"], "') AND (Active = 1)"));
                        if (drm.Length > 0)
                        {
                            if (isRealReplication)
                                UserProvider.UpdateUserActive((Guid)dr["LoginId"], organizationId, true);

                            newRow = newTable.NewRow();
                            newRow.ItemArray = dr.ItemArray;
                            newTable.Rows.Add(newRow);
                            count++;

                            ldapProcess.MessageActivatedLogins = string.Format(CultureInfo.InvariantCulture, isRealReplication ? Resources.OrganizationLdapSettingsControl_RealActivatedLogins_Text : Resources.OrganizationLdapSettingsControl_TestActivatedLogins_Text, count);
                        }
                    }
                }
                ldapProcess.MessageActivatedLogins = string.Format(CultureInfo.InvariantCulture, isRealReplication ? Resources.OrganizationLdapSettingsControl_RealActivatedLogins_Text : Resources.OrganizationLdapSettingsControl_TestActivatedLogins_Text, count);
                if (count > 0)
                {
                    newTable.DefaultView.Sort = "LoginName";
                    ldapProcess.DataActivatedLogins = Micajah.Common.Bll.Support.TrimDataView(newTable.DefaultView, 25);
                }
                ldapProcess.Logs.Add(new LdapProcessLog() { Date = DateTime.UtcNow, Message = string.Format(CultureInfo.CurrentCulture, isRealReplication ? Resources.LdapProcessLog_RealReplicationUsersActivated : Resources.LdapProcessLog_TestReplicationUsersActivated, count) });
            }
            finally
            {
                if (newTable != null) newTable.Dispose();
                drm = null;
                newRow = null;
            }
        }

        private static void LocalUsersCreate(DataTable localUsers, DataTable ldapUsers, Guid organizationId, ref Bll.LdapProcess ldapProcess, DomainUserCollection users, CommonDataSet.OrganizationsLdapGroupsDataTable orgTable, CommonDataSet.GroupMappingsDataTable groupMappings, bool isRealReplication)
        {
            int count = 0;
            DataTable newTable = null;
            DataRow[] drm = null;
            StringBuilder sb = null;
            DataRow[] drm2 = null;
            Collection<string> altEmails;
            string ldapGroupIds = null;
            string localGroupIds = null;
            string password = null;
            Guid loginId = Guid.Empty;
            IUser user = null;
            DataRow newRow = null;
            DomainUser ldapUser = null;
            string email = null;
            try
            {
                count = 0;
                newTable = ldapUsers.Clone();

                foreach (DataRow dr in ldapUsers.Rows)
                {
                    try
                    {
                        if ((bool)dr["Processed"]) continue;

                        drm = localUsers.Select(string.Format(CultureInfo.InvariantCulture, "(LdapUserId='{0}') OR (LoginName='{1}')", dr["LdapUserId"].ToString().Replace("'", "''"), dr["LoginName"].ToString().Replace("'", "''")));
                        if (drm.Length == 0)
                        {
                            drm2 = null;
                            sb = new StringBuilder();
                            altEmails = new Collection<string>();
                            for (int i = 0; i < users.Count; i++)
                            {
                                ldapUser = users[i];
                                if (ldapUser.ObjectGuid == (Guid)dr["LdapUserId"])
                                {
                                    if (ldapUser.AltEmails != null)
                                    {
                                        foreach (string altEmail in ldapUser.AltEmails)
                                        {
                                            if (altEmail.Contains("@"))
                                            {
                                                email = (altEmail.ToUpper(CultureInfo.InvariantCulture).Contains("SMTP")) ? altEmail.Remove(0, 5).ToString() : altEmail;
                                                if (string.Compare(ldapUser.EmailAddress, email, StringComparison.InvariantCultureIgnoreCase) != 0)
                                                {
                                                    altEmails.Add(email);
                                                    sb.AppendFormat(CultureInfo.InvariantCulture, " OR (LoginName='{0}')", (altEmail.ToUpper(CultureInfo.InvariantCulture).Contains("SMTP")) ? altEmail.Remove(0, 5).ToString().Replace("'", "''") : altEmail.Replace("'", "''"));
                                                }
                                            }
                                        }
                                    }

                                    if (sb.Length > 0)
                                    {
                                        sb.Remove(0, 4);
                                        drm2 = localUsers.Select(sb.ToString());
                                    }

                                    if (drm2 == null || drm2.Length == 0)
                                    {
                                        if (isRealReplication)
                                        {
                                            sb = new StringBuilder();

                                            foreach (string groupDN in ldapUser.MemberOfGroups)
                                            {
                                                for (int j = 0; j < orgTable.Rows.Count; j++)
                                                {
                                                    if (groupDN == orgTable.Rows[j]["DistinguishedName"].ToString())
                                                    {
                                                        sb.AppendFormat(CultureInfo.InvariantCulture, ",{0}", orgTable.Rows[j]["ObjectGUID"]);
                                                    }
                                                }
                                            }

                                            ldapGroupIds = (sb.Length > 0) ? sb.Remove(0, 1).ToString() : string.Empty;
                                            localGroupIds = LdapInfoProvider.GetAppGroupsByLdapGroups(organizationId, ldapGroupIds);
                                            password = "12345";
                                            loginId = UserProvider.AddUserToOrganization((string)dr["LoginName"], ((dr["Email"]).GetType() == typeof(System.DBNull)) ? string.Empty : (string)dr["Email"], ((dr["FirstName"]).GetType() == typeof(System.DBNull)) ? string.Empty : (string)dr["FirstName"], ((dr["LastName"]).GetType() == typeof(System.DBNull)) ? string.Empty : (string)dr["LastName"], string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, localGroupIds, organizationId, password, false, false);
                                            UserProvider.RaiseUserInserted(loginId, organizationId, null, Bll.Support.ConvertStringToGuidList(localGroupIds));

                                            user = LdapInfoProvider.GetLdapUser(organizationId, ldapUser);
                                            if (user != null)
                                            {
                                                WebApplication.LoginProvider.UpdateUserLdapInfo(organizationId, loginId, user.FirstName, user.LastName, user.LdapDomain, user.LdapDomainFull, user.LdapUserAlias, user.LdapUserPrinciple, user.UserSid, user.UserId, user.LdapOUPath);
                                                foreach (string altEmail in altEmails)
                                                {
                                                    if (!EmailProvider.IsEmailExists(altEmail))
                                                        EmailProvider.InsertEmail(altEmail, loginId);
                                                }
                                            }
                                        }

                                        dr["Processed"] = true;

                                        newRow = newTable.NewRow();
                                        newRow.ItemArray = dr.ItemArray;
                                        newTable.Rows.Add(newRow);
                                        count++;

                                        ldapProcess.MessageCreatedLogins = string.Format(CultureInfo.InvariantCulture, isRealReplication ? Resources.OrganizationLdapSettingsControl_RealCreatedLogins_Text : Resources.OrganizationLdapSettingsControl_TestCreatedLogins_Text, count);
                                    }
                                    break;
                                }
                            }
                        }
                        else
                            dr["Processed"] = true;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            dr["Processed"] = true;

                            if (!EventLog.SourceExists("Micajah.Common.Bll.Handlers.LdapHandler"))
                                EventLog.CreateEventSource("Micajah.Common.Bll.Handlers.LdapHandler", "Application");
                            EventLog.WriteEntry("Micajah.Common.Bll.Handlers.LdapHandler", string.Format(CultureInfo.InvariantCulture, "Error in method LocalUsersCreate() Error: {0} [{1}]", ex.Message, ex.ToString()), System.Diagnostics.EventLogEntryType.Error);
                        }
                        catch { }
                    }
                }

                ldapProcess.MessageCreatedLogins = string.Format(CultureInfo.InvariantCulture, isRealReplication ? Resources.OrganizationLdapSettingsControl_RealCreatedLogins_Text : Resources.OrganizationLdapSettingsControl_TestCreatedLogins_Text, count);

                if (count > 0)
                {
                    newTable.DefaultView.Sort = "LoginName";
                    ldapProcess.DataCreatedLogins = Micajah.Common.Bll.Support.TrimDataView(newTable.DefaultView, 25);
                }

                ldapProcess.Logs.Add(new LdapProcessLog() { Date = DateTime.UtcNow, Message = string.Format(CultureInfo.CurrentCulture, isRealReplication ? Resources.LdapProcessLog_RealReplicationUsersCreated : Resources.LdapProcessLog_TestReplicationUsersCreated, count) });
            }
            finally
            {
                if (newTable != null) newTable.Dispose();
                drm = null;
                if (sb != null)
                {
                    sb.Remove(0, sb.Length);
                    sb = null;
                }
                altEmails = null;
                drm2 = null;
                ldapGroupIds = null;
                localGroupIds = null;
                password = null;
                loginId = Guid.Empty;
                user = null;
                newRow = null;
                ldapUser = null;
                email = null;
            }
        }

        private static void LocalUsersCheckGroups(Guid organizationId, DataTable localMappedLogins, DomainUserCollection users, CommonDataSet.OrganizationsLdapGroupsDataTable orgTable, CommonDataSet.GroupMappingsDataTable groupMappings)
        {
            DataView ldapGroups = null;
            OrganizationDataSet.UserRow userRow = null;
            StringBuilder newUserGroups = null;
            User ldapUser = null;
            DomainUser ldapDomainUser = null;
            LoginProvider provider = null;
            string groupId = null;
            Guid ldapUserId = Guid.Empty;
            StringBuilder sb = null;
            try
            {
                for (int i = 0; i < users.Count; i++)
                {
                    ldapDomainUser = users[i];
                    sb = new StringBuilder();
                    sb.Append("(");
                    sb.AppendFormat("(LdapUserId='{0}') ", ldapDomainUser.ObjectGuid);
                    if (!string.IsNullOrEmpty(ldapDomainUser.PrincipalName))
                        sb.AppendFormat(" OR (LoginName = '{0}')", ldapDomainUser.PrincipalName.Replace("'", "''"));
                    if (!string.IsNullOrEmpty(ldapDomainUser.EmailAddress))
                        sb.AppendFormat(" OR (LoginName = '{0}')", ldapDomainUser.EmailAddress.Replace("'", "''"));
                    sb.Append(")");

                    foreach (DataRow dr in localMappedLogins.Select(sb.ToString()))
                    {
                        try
                        {
                            newUserGroups = new StringBuilder(string.Empty);

                            try { ldapUserId = (Guid)dr["LdapUserId"]; }
                            catch { ldapUserId = Guid.Empty; }

                            if (ldapDomainUser.ObjectGuid == ldapUserId ||
                                ldapDomainUser.PrincipalName.ToLower(CultureInfo.InvariantCulture) == ((string)dr["LoginName"]).ToLower(CultureInfo.InvariantCulture) ||
                                (ldapDomainUser.EmailAddress != null && ldapDomainUser.EmailAddress.ToLower(CultureInfo.InvariantCulture) == ((string)dr["LoginName"]).ToLower(CultureInfo.InvariantCulture)))
                            {
                                foreach (string groupDN in ldapDomainUser.MemberOfGroups)
                                {
                                    for (int j = 0; j < orgTable.Rows.Count; j++)
                                    {
                                        if (groupDN == orgTable.Rows[j]["DistinguishedName"].ToString())
                                        {
                                            for (int k = 0; k < groupMappings.Rows.Count; k++)
                                            {
                                                if ((Guid)orgTable.Rows[j]["ObjectGUID"] == (Guid)groupMappings.Rows[k]["LdapGroupId"])
                                                {
                                                    if (!string.Concat(",", newUserGroups.ToString(), ",").Contains(string.Concat(",", (Guid)groupMappings.Rows[k]["GroupId"], ",")))
                                                        newUserGroups.AppendFormat(",{0}", (Guid)groupMappings.Rows[k]["GroupId"]);
                                                }
                                            }
                                        }
                                    }
                                }

                                userRow = UserProvider.GetUserRow((Guid)dr["LoginId"], organizationId, true);
                                if (userRow == null) continue;

                                UserProvider.UpdateUser((Guid)dr["LoginId"], newUserGroups.ToString(), organizationId, false);

                                // Check user email
                                ldapUser = (User)LdapInfoProvider.GetLdapUser(organizationId, ldapDomainUser);
                                if (ldapUser != null)
                                {
                                    WebApplication.LoginProvider.UpdateUserLdapInfo(organizationId, (Guid)dr["LoginId"], ldapUser.FirstName, ldapUser.LastName, ldapUser.LdapDomain, ldapUser.LdapDomainFull, ldapUser.LdapUserAlias, ldapUser.LdapUserPrinciple, ldapUser.UserSid, ldapUser.UserId, ldapUser.LdapOUPath);

                                    if (string.IsNullOrEmpty(ldapUser.EmailAddress) == false && string.Compare(userRow.Email, ldapUser.EmailAddress, StringComparison.CurrentCultureIgnoreCase) != 0)
                                    {
                                        provider = new LoginProvider();

                                        if (!provider.LoginNameExists(ldapUser.EmailAddress))
                                            UserProvider.UpdateUser(userRow.UserId, ldapUser.EmailAddress, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, groupId, organizationId, false);

                                        if (provider != null) provider = null;
                                    }
                                }

                                UserProvider.RaiseUserUpdated(userRow.UserId, organizationId, Bll.Support.ConvertStringToGuidList(newUserGroups.ToString()));
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                if (!EventLog.SourceExists("Micajah.Common.Bll.Handlers.LdapHandler"))
                                    EventLog.CreateEventSource("Micajah.Common.Bll.Handlers.LdapHandler", "Application");
                                EventLog.WriteEntry("Micajah.Common.Bll.Handlers.LdapHandler", string.Format(CultureInfo.InvariantCulture, "Error in method LocalUsersCheckGroups() Error: {0} [{1}]", ex.Message, ex.ToString()), System.Diagnostics.EventLogEntryType.Error);
                            }
                            catch { }
                        }
                    }
                }
            }
            finally
            {
                if (ldapGroups != null) ldapGroups.Dispose();
                if (newUserGroups != null)
                {
                    newUserGroups.Remove(0, newUserGroups.Length);
                    newUserGroups = null;
                }
                if (sb != null)
                {
                    sb.Remove(0, sb.Length);
                    sb = null;
                }
                ldapUser = null;
                userRow = null;
                provider = null;
                groupId = null;
            }
        }

        #endregion
    }

}