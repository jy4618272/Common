using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Micajah.Common.LdapAdapter
{
	public interface IUser
	{
        Guid LocalLoginId { get; }
        Guid UserId { get; }
        string UserSid { get; }
        string FirstName { get; }
		string LastName { get; }
        string LoginName { get; }
        string EmailAddress { get; }
		string Password { get; }
		Guid OrganizationId { get; }
		Guid InstanceId { get; }
		string LdapUserAlias { get; }
        string LdapUserPrinciple { get; }
        string LdapDomain { get; }
        string LdapDomainFull { get; }
        string LdapOUPath { get; }
        string LdapServerAddress { get; }
		Int32 LdapServerPort { get; }
		string LdapServerUserName { get; }
		string LdapServerPassword { get; }
		bool IsActive { get; }
		Guid RoleId { get; }
        Guid LdapUserId { get; }
	}
}
