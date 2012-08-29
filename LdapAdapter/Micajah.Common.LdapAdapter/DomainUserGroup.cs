using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Micajah.Common.LdapAdapter
{
	public class DomainUserGroup
	{
		internal Domain Domain;

		public Guid GroupGuid
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		public string DistinguishedName
		{
			get;
			set;
		}
		public string SamAccountName
		{
			get;
			set;
		}
		public Int32 PrimaryGroupToken
		{
			get;
			set;
		}

        public string[] GroupMembers
        {
            get;
            set;
        }

        //public DomainUserCollection Users
        //{
        //    get { return this.Domain.Server.GetUsers(this, new List<string>(), new List<string>()); }
        //}
	}

	public class DomainUserGroupCollection : Collection<DomainUserGroup>
	{
	}
}
