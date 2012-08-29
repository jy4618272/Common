using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Micajah.Common.LdapAdapter
{
	public static class Helper
	{
		public static string ConvertDistinguishedNameToLdapPath(string name)
		{
			return name.Substring(name.IndexOf(',') + 1, name.Length - name.IndexOf(',') - 1);
		}

		public static string ConvertObjectGuidToString(Guid objectValue)
		{
			string gid = "";

			foreach (byte b in objectValue.ToByteArray())
			{
				gid += "\\" + b.ToString("X2", CultureInfo.CurrentCulture);
			}

			return gid;
		}
	}
}
