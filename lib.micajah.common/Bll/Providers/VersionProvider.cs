using Micajah.Common.Configuration;
using System.Data.SqlClient;

namespace Micajah.Common.Bll.Providers
{
    public static class VersionProvider
    {
        public static int GetVersion()
        {
            using (SqlConnection connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString))
            {
                return (int)Support.ExecuteScalar("SELECT Version FROM dbo.Mc_Version", connection);
            }
        }
    }
}
