using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Micajah.Common.LdapAdapter
{
	public class User : IUser
	{
        public Guid LocalLoginId
        {
            get;
            set;
        }

        public Guid UserId
        {
            get;
            set;
        }

        public string UserSid
        {
            get;
            set;
        }

        public string FirstName
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

        public string LoginName
        {
            get;
            set;
        }

		public string EmailAddress
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public Guid OrganizationId
		{
			get;
			set;
		}

		public Guid InstanceId
		{
			get;
			set;
		}

		public string LdapUserAlias
		{
			get;
			set;
		}

        public string LdapUserPrinciple
        {
            get;
            set;
        }

		public string LdapDomain
		{
			get;
			set;
		}

        public string LdapDomainFull
        {
            get;
            set;
        }

        public string LdapOUPath
        {
            get;
            set;
        }

		public string LdapServerAddress
		{
			get;
			set;
		}

		public Int32 LdapServerPort
		{
			get;
			set;
		}

		public string LdapServerUserName
		{
			get;
			set;
		}

		public string LdapServerPassword
		{
			get;
			set;
		}

		public bool IsActive
		{
			get;
			set;
		}

		public Guid RoleId
		{
			get;
			set;
		}

        public Guid LdapUserId
        {
            get;
            set;
        }
	}
}
