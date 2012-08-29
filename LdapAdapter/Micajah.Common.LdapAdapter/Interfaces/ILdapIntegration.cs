using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Micajah.Common.LdapAdapter
{
	public interface ILdapIntegration
	{
		IList<IUser> GetUsersByEmail(string email);
        IList<IUser> GetUsersByDomainLogin(string domainName, string userAlias, string firstName, string lastName);
		IList<IUser> GetUsersByUserPrinciple(string userPrinciple);

		Guid CreateLocalUser(IUser user);        
		Guid CreateLocalUser(IUser user, string groupId);
        void CreateUserEmails(System.Collections.ObjectModel.ReadOnlyCollection<string> altEmails, IUser user);
        bool SetLdapInfoDetails(IUser user);
		bool SetUserRole(IUser user);

		DataSet GetSortedListOfRolesGroupsById(Guid organizationId);
		string GetHashedPassword(string password);

		ILdapServer GetLdapServer(Guid serverId);
	}
}
