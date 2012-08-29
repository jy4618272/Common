using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Micajah.Common.LdapAdapter
{
	public class DomainUser
	{
		internal LdapProvider Server;

		public Guid ObjectGuid
		{
			get;
			set;
		}
        public string ObjectSid
        {
            get;
            set;
        }
        public string AccountName
		{
			get;
			set;
		}
        public string PrincipalName
        {
            get;
            set;
        }
        public string DomainName
        {
            get;
            set;
        }
        public string DomainFullName
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
		public string EmailAddress
		{
			get;
			set;
		}
		public string CommonName
		{
			get;
			set;
		}
		public string Mobile
		{
			get;
			set;
		}
		public string OULdapPath
		{
			get;
			set;
		}
		public string DistinguishedName
		{
			get;
			set;
		}
		public string OUPath
		{
			get;
			set;
		}
		public bool IsActive
		{
			get;
			set;
		}

        public string[] MemberOfGroups
        {
            get;
            set;
        }

        public string PrimaryGroupId
        {
            get;
            set;
        }

        public string[] AltEmails
        {
            get;
            set;
        }

        //public DomainUserGroupCollection MemberOf()
        //{
        //    return this.Server.GetMemberOfGroups(this);
        //}
	}

	public class DomainUserCollection : Collection<DomainUser>
	{
	}
}
