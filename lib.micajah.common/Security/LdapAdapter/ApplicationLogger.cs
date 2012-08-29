using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Micajah.Common.LdapAdapter;

namespace Micajah.Common.Security
{
    /// <summary>
    /// The class that contains log functions.
    /// </summary>
	internal class ApplicationLogger : ILogger
	{
		#region ILogger Members

		public void LogInfo(string source, string message)
		{
			// havn't implemented yet
		}

		public void LogDebug(string source, string message)
		{
			throw new NotImplementedException();
		}

		public void LogError(string source, string message, Exception exception)
		{
			// havn't implemented yet
		}

		#endregion
	}
}
