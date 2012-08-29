using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Micajah.Common.LdapAdapter
{
	public interface ILdapServer
	{
			string ServerAddress {get;}
			Int32 Port { get; }
			string UserName { get; }
			string Password { get; }
			string SiteDomain { get; }
			string AuthenticationType { get; }
	}
}
