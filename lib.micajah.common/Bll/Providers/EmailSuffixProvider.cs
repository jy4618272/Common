using System;
using System.ComponentModel;
using System.Data;
using Micajah.Common.Application;
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

                table.DefaultView.Sort = table.EmailSuffixNameColumn.ColumnName;

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

        public static Guid GetOrganizationId(string emailSuffixName)
        {
            CommonDataSet.EmailSuffixDataTable table = (CommonDataSet.EmailSuffixDataTable)GetEmailSuffixes(null, null, emailSuffixName);

            if (table.Count > 0)
                return table[0].OrganizationId;

            return Guid.Empty;
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

        public static void InsertEmailSuffixName(Guid organizationId, Guid? instanceId, ref string emailSuffixName)
        {
            if (!String.IsNullOrEmpty(emailSuffixName))
            {
                emailSuffixName = emailSuffixName.Replace(" ", string.Empty).Replace("@", string.Empty);

                InsertEmailSuffix(Guid.NewGuid(), organizationId, instanceId, emailSuffixName);
            }
        }

        public static void UpdateEmailSuffixName(Guid organizationId, Guid? instanceId, ref string emailSuffixName)
        {
            DeleteEmailSuffixes(organizationId, instanceId);

            if (!String.IsNullOrEmpty(emailSuffixName))
            {
                emailSuffixName = emailSuffixName.Replace(" ", string.Empty).Replace("@", string.Empty);

                InsertEmailSuffix(Guid.NewGuid(), organizationId, instanceId, emailSuffixName);
            }
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

        /// <summary>
        /// Parses specified email suffix name and returns the identifiers of the organization and instance.
        /// </summary>
        /// <param name="emailSuffixName">Email suffix name to parse.</param>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <param name="instanceId">The unique identifier of the instance.</param>
        public static void ParseEmailSuffixName(string emailSuffixName, ref Guid organizationId, ref Guid instanceId)
        {
            CommonDataSet.EmailSuffixDataTable table = (CommonDataSet.EmailSuffixDataTable)GetEmailSuffixes(null, null, emailSuffixName);

            if (table.Count > 0)
            {
                CommonDataSet.EmailSuffixRow row = table[0];
                organizationId = row.OrganizationId;
                instanceId = (row.IsInstanceIdNull() ? Guid.Empty : row.InstanceId);
            }
        }

        #endregion
    }
}
