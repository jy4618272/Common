using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Micajah.Common.LdapAdapter;

namespace ConsoleTestRig
{
	internal class ApplicationLogger : ILogger
	{
		#region ILogger Members

		public void LogInfo(string source, string message)
		{
			if (String.IsNullOrEmpty(source) == true)
				Console.WriteLine("{0} [INFO] - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), message);
			else
				Console.WriteLine("{0} [INFO] - [{1}]: {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), source, message);
		}

		public void LogDebug(string source, string message)
		{
			throw new NotImplementedException();
		}

		public void LogError(string source, string message, Exception exception)
		{
			if (String.IsNullOrEmpty(source) == true)
				Console.WriteLine("{0} [ERROR] - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), message);
			else
				Console.WriteLine("{0} [ERROR] - [{1}]: {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), source, message);
			Console.WriteLine(exception.Message.ToString());
		}

		#endregion
	}
}
