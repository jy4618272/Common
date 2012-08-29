using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with Email Suffixes.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class EmailSuffixProvider
    {
        #region Public Methods

        /// <summary>
        /// Check if EmailSuffix table is Exist.
        /// </summary>
        public static bool IsEmailSuffixExist()
        {
            SqlConnection m_Connection = null;
            using (m_Connection = new SqlConnection(FrameworkConfiguration.Current.WebApplication.ConnectionString))
            {
                if (!Convert.ToBoolean(Support.ExecuteScalar("select case when EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_EmailSuffix]') AND type in (N'U')) then 1 else 0 end as IsExist", m_Connection), CultureInfo.CurrentCulture))
                    return false;

                if (!Convert.ToBoolean(Support.ExecuteScalar("select case when EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEmailSuffixesByOrganizationId]') AND type in (N'P', N'PC')) then 1 else 0 end as IsExist", m_Connection), CultureInfo.CurrentCulture))
                    return false;

                if (!Convert.ToBoolean(Support.ExecuteScalar("select case when EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Mc_GetEmailSuffixes]') AND type in (N'P', N'PC')) then 1 else 0 end as IsExist", m_Connection), CultureInfo.CurrentCulture))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the list of email suffixes.
        /// </summary>
        /// <param name="organizationId">Organization Id.</param>
        /// <param name="instanceId">Instance Id.</param>
        /// <param name="emailSuffixName">Email Suffix Name.</param>
        /// <returns>The System.Data.DataTable object that contains the email suffixes.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetEmailSuffixes(Guid? organizationId, Guid? instanceId, string emailSuffixName)
        {
            CommonDataSet.EmailSuffixDataTable table = null;
            try
            {
                table = new CommonDataSet.EmailSuffixDataTable();
                WebApplication.CommonDataSetTableAdapters.EmailSuffixTableAdapter.Fill(table, 2, organizationId, instanceId, emailSuffixName);

                table.DefaultView.Sort = "[EmailSuffixName] ASC";

                return table;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Gets the list of email suffixes by organization Id.
        /// </summary>
        /// <param name="organizationId">Organization Id.</param>
        /// <returns>The System.Data.DataTable object that contains the email suffixes.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetEmailSuffixes(Guid organizationId)
        {
            CommonDataSet.EmailSuffixDataTable table = null;
            try
            {
                table = new CommonDataSet.EmailSuffixDataTable();
                WebApplication.CommonDataSetTableAdapters.EmailSuffixTableAdapter.Fill(table, 0, organizationId);

                return table;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Gets the list of email suffixes by instance Id.
        /// </summary>
        /// <param name="instanceId">Instance Id.</param>
        /// <returns>The System.Data.DataTable object that contains the email suffixes.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetEmailSuffixesByInstanceId(Guid instanceId)
        {
            CommonDataSet.EmailSuffixDataTable table = null;
            try
            {
                table = new CommonDataSet.EmailSuffixDataTable();
                WebApplication.CommonDataSetTableAdapters.EmailSuffixTableAdapter.Fill(table, 3, instanceId);

                return table;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Inserts the email suffix.
        /// </summary>
        /// <param name="emailSuffixId">Email Suffix Id.</param>
        /// <param name="organizationId">Organization Id.</param>
        /// <param name="instanceId">Instance Id.</param>
        /// <param name="emailSuffixName">Email Suffix Name.</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertEmailSuffix(Guid emailSuffixId, Guid organizationId, Guid? instanceId, string emailSuffixName)
        {
            WebApplication.CommonDataSetTableAdapters.EmailSuffixTableAdapter.Insert(emailSuffixId, organizationId, instanceId, emailSuffixName);
        }

        /// <summary>
        /// Deletes the email suffix.
        /// </summary>
        /// <param name="emailSuffixId">Email Suffix Id.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEmailSuffixes(Guid organizationId, Guid? instanceId)
        {
            WebApplication.CommonDataSetTableAdapters.EmailSuffixTableAdapter.Delete(organizationId, instanceId);
        }

        #endregion
    }
}
