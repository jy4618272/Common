using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Micajah.Common.LdapAdapter
{
	public class Domain
	{
		internal LdapProvider Server;

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
		public Guid Guid
		{
			get;
			set;
		}
		public Byte[] Sid
		{
			get;
			set;
		}

        //public DomainUserGroupCollection Groups
        //{
        //    get { return this.Server.GetGroups(this); }
        //}
	}

	public class DomainCollection : Collection<Domain>
	{
	}
}
