using System;
using System.Linq;
using System.Text;
using Micajah.Common.LdapAdapter;

namespace ConsoleTestRig
{
	class Program
	{
		static void Main(string[] args)
		{
			string myChoice;

			TestStrategy ts = new TestStrategy();

			do
			{
				myChoice = ts.getChoice();

				switch (myChoice)
				{
					case "1":
						ts.TestFromSqlTable();
						break;
					case "2":
						ts.UserManualTest();
						break;
					case "3":
						ts.LdapAdapterFunctionsTest();
						break;
					case "4":
						ts.GetGroupMembersTest();
						break;
                    case "5":
                        ts.GetUserLdapAttributes();
                        break;
                    case "6":
                        ts.GetGroupLdapAttributes();
                        break;
                    case "7":
                        ts.GetUserAltEmails();
                        break;
                    case "8":
						break;
					default:
						Console.WriteLine("{0} is not a valid choice.", myChoice);
						break;
				}

				if (myChoice != "8")
				{
					Console.WriteLine();
					Console.WriteLine("press Enter key to continue...");
					
					Console.ReadLine();
					Console.WriteLine();
				}
			} while (myChoice != "8");
		}

	}
}
