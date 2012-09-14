using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Micajah.Common.LdapAdapter;
using System.Data.SqlServerCe;
using System.Data;

namespace ConsoleTestRig
{
	class TestStrategy
	{

		public string getChoice()
		{
			string choice;

			Console.WriteLine("");
			Console.WriteLine("Test Application Menu:");
			Console.WriteLine("[1] - Full automated test from TestScenario table.");
			Console.WriteLine("[2] - Custom user test.");
			Console.WriteLine("[3] - Application functions test.");
			Console.WriteLine("[4] - Get group members (enter group name and retrieve all the members from the AD)");
            Console.WriteLine("[5] - User Ldap Attributes.");
            Console.WriteLine("[6] - Group Ldap Attributes.");
            Console.WriteLine("[7] - User Alt Emails.");
            Console.WriteLine("[8] - EXIT.");
            Console.WriteLine("");
			Console.Write("Press a key followed by ENTER: ");
			choice = Console.ReadLine();
			Console.WriteLine("");

			return choice;
		}

		public void UserManualTest()
		{
			string myLogin;
			string myPass;
			ApplicationLogger log = new ApplicationLogger();
			LdapIntegration ldi = new LdapIntegration(log);
			LdapProvider server = new LdapProvider(new Guid("{a1fb1d06-325e-4bc6-933a-1dc48cc375c2}"), ldi, log);
			server.Initialize();

			Console.Write("Enter login followed by ENTER: ");
			myLogin = Console.ReadLine();
			Console.Write("Enter password followed by ENTER: ");
			myPass = Console.ReadLine();
			Console.WriteLine("");

			Response<AuthenticationStatus> isAuth = server.AuthenticateUser(myLogin, myPass, true, Guid.NewGuid());
			if (isAuth.ResponseValue == AuthenticationStatus.Authenticated)
			{
				Console.WriteLine("{0} - User is Authenticated!", myLogin);
				Console.WriteLine("User Group List:");
				foreach(Guid k in isAuth.GroupList.Keys)
					Console.WriteLine("{0}", isAuth.GroupList[k].ToString());
			}
			else
				Console.WriteLine("{0} - User is NOT Authenticated!", myLogin);
			Console.WriteLine();
		}

        public void GetUserLdapAttributes()
        {
            string account;
            ApplicationLogger log = new ApplicationLogger();
            LdapIntegration ldi = new LdapIntegration(log);
            LdapProvider server = new LdapProvider(new Guid("{a1fb1d06-325e-4bc6-933a-1dc48cc375c2}"), ldi, log);
            server.Initialize();

            Console.Write("Enter user account name followed by ENTER: ");
            account = Console.ReadLine();
            Console.WriteLine("");

            server.GetUserByAccountName(account);

            Console.WriteLine();
        }

        public void GetGroupLdapAttributes()
        {
            string account;
            ApplicationLogger log = new ApplicationLogger();
            LdapIntegration ldi = new LdapIntegration(log);
            LdapProvider server = new LdapProvider(new Guid("{a1fb1d06-325e-4bc6-933a-1dc48cc375c2}"), ldi, log);
            server.Initialize();

            Console.Write("Enter group name followed by ENTER: ");
            account = Console.ReadLine();
            Console.WriteLine("");

            server.GetGroupByName(account);

            Console.WriteLine();
        }

        public void GetUserAltEmails()
        {
            string guid;
            ApplicationLogger log = new ApplicationLogger();
            LdapIntegration ldi = new LdapIntegration(log);
            LdapProvider server = new LdapProvider(new Guid("{a1fb1d06-325e-4bc6-933a-1dc48cc375c2}"), ldi, log);
            server.Initialize();

            Console.Write("Enter user Guid followed by ENTER: ");
            guid = Console.ReadLine();
            Console.WriteLine("");

            ReadOnlyCollection<string> altEmails = server.GetUserAltEmails(new Guid(guid));
            foreach (string email in altEmails)
                Console.WriteLine(email);

            Console.WriteLine();
        }

		public void TestFromSqlTable()
		{
			ApplicationLogger log = new ApplicationLogger();
			LdapIntegration ldi = new LdapIntegration(log);
			LdapProvider server = new LdapProvider(new Guid("{a1fb1d06-325e-4bc6-933a-1dc48cc375c2}"), ldi, log);
			server.Initialize();

			Console.WriteLine("Login test scenarios from SQL Table:");
			Console.WriteLine("====================================");

			// Get test scenarios from the table
			SqlCeConnection connection = new SqlCeConnection(Properties.Settings.Default.LocalAppDBConnectionString);
			SqlCeDataAdapter adapter = new SqlCeDataAdapter("select * from TestScenario", connection);
			DataSet data = new DataSet();
			adapter.Fill(data);
			connection.Close();

			if (data.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < data.Tables[0].Rows.Count; i++)
				{
					Console.WriteLine(string.Format("{0}. {1} | {2}", (i + 1).ToString(), data.Tables[0].Rows[i].Field<string>("LoginInput"), data.Tables[0].Rows[i].Field<string>("PasswordInput")));
					Console.WriteLine(string.Format("({0})", data.Tables[0].Rows[i].Field<string>("ExpectedOutput")));
					Console.WriteLine();

					Response<AuthenticationStatus> isAuth = server.AuthenticateUser(data.Tables[0].Rows[i].Field<string>("LoginInput"), data.Tables[0].Rows[i].Field<string>("PasswordInput"), true, Guid.NewGuid());
					if (isAuth.ResponseValue == AuthenticationStatus.Authenticated)
					{
						Console.WriteLine("{0} - User is Authenticated!", data.Tables[0].Rows[i].Field<string>("LoginInput"));
						Console.WriteLine("User Group List:");
						foreach (Guid k in isAuth.GroupList.Keys)
							Console.WriteLine("{0}", isAuth.GroupList[k].ToString());
					}
					else
						Console.WriteLine("{0} - User is NOT Authenticated!", data.Tables[0].Rows[i].Field<string>("LoginInput"));
					Console.WriteLine();
				}
			}
			else
				Console.WriteLine("There is no scenarios in the local table!");

			Console.WriteLine("");
		}

		public void GetGroupMembersTest()
		{
			string groupCN;

			Console.Write("Enter Group CN followed by ENTER: ");
			groupCN = Console.ReadLine();
			Console.WriteLine("");

			ApplicationLogger log = new ApplicationLogger();
			LdapIntegration ldi = new LdapIntegration(log);
			using (var server = new LdapProvider(new Guid(), ldi, log))
			{
				server.Initialize();
				var group = server.GetGroupByCN(groupCN);

				if (group != null)
				{
					var users = group.DistinguishedName;
					if (users.Count > 0)
					{
						Console.WriteLine("List of users:");
						users.ToList<DomainUser>().ForEach(
							delegate(DomainUser user)
							{
								Console.WriteLine(string.Format("USER: {0}{1}Details:{3}{1}{2}",
									user.DistinguishedName,
									Environment.NewLine,
									"-------------------------------------------------------",
									user.IsActive ? "ACTIVE" : "INACTIVE"));
							}
							);
						Console.WriteLine();
					}
					else
						Console.WriteLine(string.Format("{0} doesn't contain any users.", groupCN));
				}
				else
					Console.WriteLine(string.Format("{0} group is NOT found in AD.", groupCN));
			}
		}

		public void LdapAdapterFunctionsTest()
		{
			ApplicationLogger log = new ApplicationLogger();
			LdapIntegration ldi = new LdapIntegration(log);
			using (var server = new LdapProvider(new Guid(), ldi, log))
			{
				server.Initialize();
				var domains = server.GetDomains();
				if (domains != null)
				{
					Console.WriteLine("List of Domains:");
					domains.ToList<Domain>().ForEach(delegate(Domain domain) { Console.WriteLine(domain.DistinguishedName + " " + domain.Guid.ToString("D")); });
					Console.WriteLine("");
					Console.WriteLine("");

					foreach (Domain domain in domains)
					{
						var groups = server.GetGroups(domain);
						if (groups != null)
						{
							Console.WriteLine(domain.DistinguishedName + " domain groups list:");
							groups.ToList<DomainUserGroup>().ForEach(delegate(DomainUserGroup group) { Console.WriteLine(group.Name); });
							Console.WriteLine("");

							Console.WriteLine(string.Format("'{0}' Group Users List:", groups[59].Name));
							var users = server.GetUsers(groups[59], new List<string>(), new List<string>());
							if (users.Count > 0)
							{
								users.ToList<DomainUser>().ForEach(delegate(DomainUser user) { Console.WriteLine(user.CommonName); });
								Console.WriteLine("");
								foreach (DomainUser user in users)
								{
									var groupList = user.MemberOf();
									if (groupList != null)
									{
										Console.WriteLine("MemberOf Groups List:");
										groupList.ToList<DomainUserGroup>().ForEach(delegate(DomainUserGroup group) { Console.WriteLine(group.DistinguishedName); });
										Console.WriteLine("");
									}

									DomainUser newUser = server.GetUser(user.ObjectGuid);
									if (newUser != null)
									{
										Console.WriteLine(user.FirstName + " User Details:");
										Console.WriteLine(newUser.EmailAddress);
										Console.WriteLine(newUser.Mobile);
										Console.WriteLine(newUser.OULdapPath);
										Console.WriteLine(newUser.OUPath);
										Console.WriteLine("");
									}

									DomainUser newUser2 = server.GetUser(Helper.ConvertDistinguishedNameToLdapPath(user.DistinguishedName), user.CommonName);
									if (newUser2 != null)
									{
										Console.WriteLine(newUser2.FirstName + " User Details (Overloaded Method):");
										Console.WriteLine(newUser2.EmailAddress);
										Console.WriteLine(newUser2.Mobile);
										Console.WriteLine(newUser2.OULdapPath);
										Console.WriteLine(newUser.OUPath);
										Console.WriteLine("");
									}

									DomainUser newUser3 = server.GetUserByEmail(user.EmailAddress);
									if (newUser3 != null)
									{
										Console.WriteLine(newUser3.FirstName + " User Details (By Email):");
										Console.WriteLine(newUser3.EmailAddress);
										Console.WriteLine(newUser3.Mobile);
										Console.WriteLine(newUser3.OULdapPath);
										Console.WriteLine(newUser.OUPath);
										Console.WriteLine("");
									}
								}
							}
						}
					}
				}
			}
		}

	}
}
