using Micajah.Common.Dal;
using Micajah.Common.Dal.MasterDataSetTableAdapters;
using System;
using System.ComponentModel;
using System.Data;

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
        public static MasterDataSet.EmailSuffixDataTable GetEmailSuffixes(Guid? organizationId, Guid? instanceId, string emailSuffixName)
        {
            using (EmailSuffixTableAdapter adapter = new EmailSuffixTableAdapter())
            {
                return adapter.GetEmailSuffixes(organizationId, instanceId, emailSuffixName);
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
            using (EmailSuffixTableAdapter adapter = new EmailSuffixTableAdapter())
            {
                return adapter.GetEmailSuffixesByOrganizationId(organizationId);
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
            using (EmailSuffixTableAdapter adapter = new EmailSuffixTableAdapter())
            {
                return adapter.GetEmailSuffixesByInstanceId(instanceId);
            }
        }

        public static Guid GetOrganizationId(string emailSuffixName)
        {
            MasterDataSet.EmailSuffixDataTable table = GetEmailSuffixes(null, null, emailSuffixName);

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
            if (!String.IsNullOrEmpty(emailSuffixName))
            {
                emailSuffixName = emailSuffixName.Replace(" ", string.Empty).Replace("@", string.Empty);

                using (EmailSuffixTableAdapter adapter = new EmailSuffixTableAdapter())
                {
                    adapter.Insert(emailSuffixId, organizationId, instanceId, emailSuffixName);
                }
            }
        }

        public static void InsertEmailSuffixName(Guid organizationId, Guid? instanceId, ref string emailSuffixName)
        {
            InsertEmailSuffix(Guid.NewGuid(), organizationId, instanceId, emailSuffixName);
        }

        public static void UpdateEmailSuffixName(Guid organizationId, Guid? instanceId, ref string emailSuffixName)
        {
            DeleteEmailSuffixes(organizationId, instanceId);
            InsertEmailSuffix(Guid.NewGuid(), organizationId, instanceId, emailSuffixName);
        }

        /// <summary>
        /// Deletes the email suffix.
        /// </summary>
        /// <param name="emailSuffixId">Email Suffix Id.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEmailSuffixes(Guid organizationId, Guid? instanceId)
        {
            using (EmailSuffixTableAdapter adapter = new EmailSuffixTableAdapter())
            {
                adapter.Delete(organizationId, instanceId);
            }
        }

        /// <summary>
        /// Parses specified email suffix name and returns the identifiers of the organization and instance.
        /// </summary>
        /// <param name="emailSuffixName">Email suffix name to parse.</param>
        /// <param name="organizationId">The unique identifier of the organization.</param>
        /// <param name="instanceId">The unique identifier of the instance.</param>
        public static void ParseEmailSuffixName(string emailSuffixName, ref Guid organizationId, ref Guid instanceId)
        {
            MasterDataSet.EmailSuffixDataTable table = GetEmailSuffixes(null, null, emailSuffixName);

            if (table.Count > 0)
            {
                MasterDataSet.EmailSuffixRow row = table[0];
                organizationId = row.OrganizationId;
                instanceId = (row.IsInstanceIdNull() ? Guid.Empty : row.InstanceId);
            }
        }

        #endregion
    }
}
