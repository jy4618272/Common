using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Micajah.Common.LdapAdapter;

namespace Micajah.Common.Security
{
    /// <summary>
    /// The class of LdapServer structure.
    /// </summary>
	internal class LdapServer : ILdapServer
	{
		public string ServerAddress
		{
			get;
			set;
		}

		public int Port
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public string SiteDomain
		{
			get;
			set;
		}

		public string AuthenticationType
		{
			get;
			set;
		}
	}
}
