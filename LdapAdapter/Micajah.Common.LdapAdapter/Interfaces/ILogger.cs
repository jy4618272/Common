using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Micajah.Common.LdapAdapter
{
	public interface ILogger
	{
		void LogInfo(string source, string message);
		void LogDebug(string source, string message);
		void LogError(string source, string message, Exception exception);
	}
}
