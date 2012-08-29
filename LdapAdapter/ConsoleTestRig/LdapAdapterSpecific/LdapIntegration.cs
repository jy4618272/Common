using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Micajah.Common.LdapAdapter;
using System.Configuration;
using System.Data.SqlServerCe;

namespace ConsoleTestRig
{
	class LdapIntegration : ILdapIntegration
	{
		ILogger ApplicationLogger;

		public LdapIntegration(ILogger applicationLogger)
		{
			ApplicationLogger = applicationLogger;
		}

		#region Internal Helper Functions

		public IList<IUser> GetUsersByEmail(string email)
		{
			List<IUser> users = new List<IUser>();
			ApplicationLogger.LogInfo("LDAPIntegration", string.Format("Searching local Logins table for email={0}", email));

			DataSet data = getDataFromLocalDB("select l.FirstName, l.LastName, l.Email, l.AccountName, l.Password, l.Note, l.RoleId, il.InstanceId, il.Active, il.LdapDomain, il.LdapPrincipleName, il.LdapObjectGuid, i.OrganizationId, i.Name as iName, o.Name as oName, o.LdapActive, o.LdapServerIp, o.LdapServerPort from Login l inner join InstancesLogin il ON l.Id = il.LoginId inner join Instance i ON il.InstanceId = i.Id inner join Organization o ON i.OrganizationId = o.Id WHERE l.Email = '" + email.ToLower() + "'");

			if (data.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < data.Tables[0].Rows.Count; i++)
				{
					Guid roleGuid = new Guid();
					if (data.Tables[0].Rows[i].IsNull("RoleId") != true)
						roleGuid = data.Tables[0].Rows[i].Field<Guid>("RoleId");
					users.Add(new User()
					{
						FirstName = data.Tables[0].Rows[i].Field<string>("FirstName"),
						LastName = data.Tables[0].Rows[i].Field<string>("LastName"),
						EmailAddress = data.Tables[0].Rows[i].Field<string>("Email"),
						Password = data.Tables[0].Rows[i].Field<string>("Password"),
						OrganizationId = data.Tables[0].Rows[i].Field<Guid>("OrganizationId"),
						InstanceId = data.Tables[0].Rows[i].Field<Guid>("InstanceId"),
						LdapUserPrinciple = data.Tables[0].Rows[i].Field<string>("LdapPrincipleName"),
						LdapDomain = data.Tables[0].Rows[i].Field<string>("LdapDomain"),
						LdapServerAddress = data.Tables[0].Rows[i].Field<string>("LdapServerIp"),
						LdapServerPort = Convert.ToInt32(data.Tables[0].Rows[i].Field<string>("LdapServerPort")),
						LdapServerUserName = data.Tables[0].Rows[i].Field<string>("AccountName"),
						LdapServerPassword = data.Tables[0].Rows[i].Field<string>("Password"),
						RoleId = roleGuid,
						IsActive = data.Tables[0].Rows[i].Field<bool>("Active")
					});
				}
				ApplicationLogger.LogInfo("LDAPIntegration", "result=Found!");
			}
			else
				ApplicationLogger.LogInfo("LDAPIntegration", "result=not found.");

			return users;
		}

		public IList<IUser> GetUsersByDomainLogin(string domainName, string userAlias, string firstName, string lastName)
		{
            string domainLogin = domainName;
            if (domainLogin == null) domainLogin = userAlias;
			List<IUser> users = new List<IUser>();
			DataSet data;

			if (domainLogin.Contains('@'))
			{
				string[] parts = domainLogin.Split('@');
				ApplicationLogger.LogInfo("LDAPIntegration", string.Format("Searching local InstancesLogin/Logins tables for AccountName={0} OR (AccountName={1} + LdapDomain={2})", domainLogin, parts[0], parts[1]));
				data = getDataFromLocalDB("select l.FirstName, l.LastName, l.Email, l.AccountName, l.Password, l.Note, l.RoleId, il.InstanceId, il.Active, il.LdapDomain, il.LdapPrincipleName, il.LdapObjectGuid, i.OrganizationId, i.Name as iName, o.Name as oName, o.LdapActive, o.LdapServerIp, o.LdapServerPort from Login l inner join InstancesLogin il ON l.Id = il.LoginId inner join Instance i ON il.InstanceId = i.Id inner join Organization o ON i.OrganizationId = o.Id WHERE (l.AccountName = '" + domainLogin.ToLower() + "' OR (l.AccountName = '" + parts[0] + "' AND il.LdapDomain = '" + parts[1] + "'))");
				if (data.Tables[0].Rows.Count == 0 && parts[1].Contains('.'))
				{
					ApplicationLogger.LogInfo("LDAPIntegration", "result=not found.");
					string[] parts2 = parts[1].Split('.');
					ApplicationLogger.LogInfo("LDAPIntegration", string.Format("Searching local InstancesLogin/Logins tables for (AccountName={0} + LdapDomain={1})", parts[0], parts2[0]));
					data = getDataFromLocalDB("select l.FirstName, l.LastName, l.Email, l.AccountName, l.Password, l.Note, l.RoleId, il.InstanceId, il.Active, il.LdapDomain, il.LdapPrincipleName, il.LdapObjectGuid, i.OrganizationId, i.Name as iName, o.Name as oName, o.LdapActive, o.LdapServerIp, o.LdapServerPort from Login l inner join InstancesLogin il ON l.Id = il.LoginId inner join Instance i ON il.InstanceId = i.Id inner join Organization o ON i.OrganizationId = o.Id WHERE (l.AccountName = '" + parts[0] + "' AND il.LdapDomain = '" + parts2[0] + "')");
				}
			}
			else if (domainLogin.Contains('\\'))
			{
				string[] parts = domainLogin.Split('\\');
				ApplicationLogger.LogInfo("LDAPIntegration", string.Format("Searching local InstancesLogin/Logins tables for AccountName={0} OR (AccountName={1} + LdapDomain={2})", domainLogin, parts[1], parts[0]));
				data = getDataFromLocalDB("select l.FirstName, l.LastName, l.Email, l.AccountName, l.Password, l.Note, l.RoleId, il.InstanceId, il.Active, il.LdapDomain, il.LdapPrincipleName, il.LdapObjectGuid, i.OrganizationId, i.Name as iName, o.Name as oName, o.LdapActive, o.LdapServerIp, o.LdapServerPort from Login l inner join InstancesLogin il ON l.Id = il.LoginId inner join Instance i ON il.InstanceId = i.Id inner join Organization o ON i.OrganizationId = o.Id WHERE (l.AccountName = '" + domainLogin.ToLower() + "' OR (l.AccountName = '" + parts[1] + "' AND il.LdapDomain = '" + parts[0] + "'))");
			}
			else
			{
				ApplicationLogger.LogInfo("LDAPIntegration", string.Format("Searching local Logins table for AccountName={0}", domainLogin));
				data = getDataFromLocalDB("select l.FirstName, l.LastName, l.Email, l.AccountName, l.Password, l.Note, l.RoleId, il.InstanceId, il.Active, il.LdapDomain, il.LdapPrincipleName, il.LdapObjectGuid, i.OrganizationId, i.Name as iName, o.Name as oName, o.LdapActive, o.LdapServerIp, o.LdapServerPort from Login l inner join InstancesLogin il ON l.Id = il.LoginId inner join Instance i ON il.InstanceId = i.Id inner join Organization o ON i.OrganizationId = o.Id WHERE l.AccountName = '" + domainLogin.ToLower() + "'");
			}


			if (data.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < data.Tables[0].Rows.Count; i++)
				{
					Guid roleGuid = new Guid();
					if (data.Tables[0].Rows[i].IsNull("RoleId") != true)
						roleGuid = data.Tables[0].Rows[i].Field<Guid>("RoleId");
					users.Add(new User()
					{
						FirstName = data.Tables[0].Rows[i].Field<string>("FirstName"),
						LastName = data.Tables[0].Rows[i].Field<string>("LastName"),
						EmailAddress = data.Tables[0].Rows[i].Field<string>("Email"),
						Password = data.Tables[0].Rows[i].Field<string>("Password"),
						OrganizationId = data.Tables[0].Rows[i].Field<Guid>("OrganizationId"),
						InstanceId = data.Tables[0].Rows[i].Field<Guid>("InstanceId"),
						LdapUserPrinciple = data.Tables[0].Rows[i].Field<string>("LdapPrincipleName"),
						LdapDomain = data.Tables[0].Rows[i].Field<string>("LdapDomain"),
						LdapServerAddress = data.Tables[0].Rows[i].Field<string>("LdapServerIp"),
						LdapServerPort = Convert.ToInt32(data.Tables[0].Rows[i].Field<string>("LdapServerPort")),
						LdapServerUserName = data.Tables[0].Rows[i].Field<string>("AccountName"),
						LdapServerPassword = data.Tables[0].Rows[i].Field<string>("Password"),
						RoleId = roleGuid,
						IsActive = data.Tables[0].Rows[i].Field<bool>("Active")
					});
				}
				ApplicationLogger.LogInfo("LDAPIntegration", "result=Found!");
			}
			else
				ApplicationLogger.LogInfo("LDAPIntegration", "result=not found.");

			return users;
		}

		public IList<IUser> GetUsersByUserPrinciple(string UserPrinciple)
		{
			List<IUser> users = new List<IUser>();

			string[] parts = UserPrinciple.Split(' ');
			ApplicationLogger.LogInfo("LDAPIntegration", string.Format("Searching local InstancesLogin/Logins tables for (FirstName={0} + LastName={1}) OR LdapPrincipleName={2}", parts[0], parts[1], UserPrinciple));
			DataSet data = getDataFromLocalDB("select l.FirstName, l.LastName, l.Email, l.AccountName, l.Password, l.Note, l.RoleId, il.InstanceId, il.Active, il.LdapDomain, il.LdapPrincipleName, il.LdapObjectGuid, i.OrganizationId, i.Name as iName, o.Name as oName, o.LdapActive, o.LdapServerIp, o.LdapServerPort from Login l inner join InstancesLogin il ON l.Id = il.LoginId inner join Instance i ON il.InstanceId = i.Id inner join Organization o ON i.OrganizationId = o.Id WHERE (il.LdapPrincipleName = '" + UserPrinciple.ToLower() + "' OR (l.FirstName = '" + parts[0] + "' AND l.LastName = '" + parts[1] + "'))");

			if (data.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < data.Tables[0].Rows.Count; i++)
				{
					Guid roleGuid = new Guid();
					if (data.Tables[0].Rows[i].IsNull("RoleId") != true)
						roleGuid = data.Tables[0].Rows[i].Field<Guid>("RoleId");
					users.Add(new User()
					{
						FirstName = data.Tables[0].Rows[i].Field<string>("FirstName"),
						LastName = data.Tables[0].Rows[i].Field<string>("LastName"),
						EmailAddress = data.Tables[0].Rows[i].Field<string>("Email"),
						Password = data.Tables[0].Rows[i].Field<string>("Password"),
						OrganizationId = data.Tables[0].Rows[i].Field<Guid>("OrganizationId"),
						InstanceId = data.Tables[0].Rows[i].Field<Guid>("InstanceId"),
						LdapUserPrinciple = data.Tables[0].Rows[i].Field<string>("LdapPrincipleName"),
						LdapDomain = data.Tables[0].Rows[i].Field<string>("LdapDomain"),
						LdapServerAddress = data.Tables[0].Rows[i].Field<string>("LdapServerIp"),
						LdapServerPort = Convert.ToInt32(data.Tables[0].Rows[i].Field<string>("LdapServerPort")),
						LdapServerUserName = data.Tables[0].Rows[i].Field<string>("AccountName"),
						LdapServerPassword = data.Tables[0].Rows[i].Field<string>("Password"),
						RoleId = roleGuid,
						IsActive = data.Tables[0].Rows[i].Field<bool>("Active")
					});
				}
				ApplicationLogger.LogInfo("LDAPIntegration", "result=Found!");
			}
			else
				ApplicationLogger.LogInfo("LDAPIntegration", "result=not found.");

			return users;
		}

		#endregion

		#region ILDAPIntegration Members

		// Return hashed via MD5 or HA1 methods passord or plain text password
		public string GetHashedPassword(string password)
		{
			return password;
		}

		public DataSet GetSortedListOfRolesGroupsById(Guid organizationId)
		{
			DataSet data = getDataFromLocalDB("select r.Id as RoleId, r.Name as RoleName, r.Priority, om.GroupName from Role r inner join OrganizationMapping om ON r.Id = om.RoleId WHERE om.OrganizationId = '" + organizationId.ToString("B", CultureInfo.CurrentCulture) + "' ORDER BY r.Priority Desc");

			return data;
		}

		public Guid CreateLocalUser(IUser user)
		{
			return CreateLocalUser(user, String.Empty);
		}

		public Guid CreateLocalUser(IUser user, string groupId)
		{
			SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.LocalAppDBConnectionString);
			SqlCeCommand cmd = connection.CreateCommand();
			connection.Open();

			Guid g1 = new Guid();
			cmd.CommandText = string.Format("INSERT INTO Login (Id, FirstName, LastName, Email, AccountName, Password, Note, RoleId) VALUES ('{0}', '', '', '{1}', '{2}', '{3}', '', '{4}')", g1.ToString("B", CultureInfo.CurrentCulture), user.EmailAddress, user.LdapServerUserName, user.Password, user.RoleId.ToString("B", CultureInfo.CurrentCulture));
			cmd.ExecuteNonQuery();

			Guid g2 = new Guid();
			cmd.CommandText = string.Format("INSERT INTO Instance (Id, OrganizationId, Name) VALUES ('{0}', '{1}', 'Auto inserted instance.')", g2.ToString("B", CultureInfo.CurrentCulture), user.OrganizationId.ToString("B", CultureInfo.CurrentCulture));
			cmd.ExecuteNonQuery();

			Guid g3 = new Guid(); // LdapObjectGuid
			cmd.CommandText = string.Format("INSERT INTO InstancesLogin (Id, LoginId, InstanceId, Active, LdapDomain, LdapPrincipleName, Note) VALUES ('{0}', '{1}', '{2}', 1, '{3}', '', '{4}', '')", g3.ToString("B", CultureInfo.CurrentCulture), g1.ToString("B", CultureInfo.CurrentCulture), g2.ToString("B", CultureInfo.CurrentCulture), user.LdapDomain);
			cmd.ExecuteNonQuery();

			connection.Close();

			ApplicationLogger.LogInfo("LDAPIntegration", "User has been added to the local Logins table.");

			return Guid.Empty;
		}

        public bool SetLdapInfoDetails(IUser user)
        {
            return true;
        }

		public bool SetUserRole(IUser user)
		{
			SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.LocalAppDBConnectionString);
			SqlCeCommand cmd = connection.CreateCommand();
			connection.Open();

			cmd.CommandText = string.Format("UPDATE Login SET RoleId = '{0}'", user.RoleId.ToString("B", CultureInfo.CurrentCulture));
			cmd.ExecuteNonQuery();

			connection.Close();

			ApplicationLogger.LogInfo("LDAPIntegration", "User RoleID was updated in the local Logins table.");

			return true;
		}

		private DataSet getDataFromLocalDB(string query)
		{
			SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.LocalAppDBConnectionString);
			SqlCeDataAdapter adapter = new SqlCeDataAdapter(query, connection);
			DataSet data = new DataSet();
			adapter.Fill(data);
			connection.Close();

			return data;
		}

		#endregion

		#region ILDAPIntegration Members


		public Micajah.Common.LdapAdapter.ILdapServer GetLdapServer(Guid serverId)
		{
			LdapServer server = null;

			SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.LocalAppDBConnectionString);
			SqlCeDataAdapter adapter = new SqlCeDataAdapter("select LdapServerIp, LdapServerPort from Organization WHERE Id = '" + serverId.ToString("B", CultureInfo.CurrentCulture) + "'", connection);
			DataSet data = new DataSet();
			adapter.Fill(data);
			connection.Close();

			server = new LdapServer()
			{
				ServerAddress = data.Tables[0].Rows[0].Field<string>("LdapServerIp"),
				Port = Convert.ToInt32(data.Tables[0].Rows[0].Field<string>("LdapServerPort")),
				SiteDomain = ConfigurationSettings.AppSettings["SiteDomain"],
				UserName = ConfigurationSettings.AppSettings["UserName"],
				Password = ConfigurationSettings.AppSettings["Password"],
				AuthenticationType = ConfigurationSettings.AppSettings["AuthenticationType"]
			};

			return server;
		}

		#endregion
	}
}
