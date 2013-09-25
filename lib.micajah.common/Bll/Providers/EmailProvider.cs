using Micajah.Common.Dal;
using Micajah.Common.Dal.MasterDataSetTableAdapters;
using System;
using System.ComponentModel;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with Email.
    /// </summary>
    [DataObjectAttribute(true)]
    public static class EmailProvider
    {
        #region Public Methods

        /// <summary>
        /// Checks if Email is Exist.
        /// </summary>        
        /// <param name="email">Email.</param>        
        public static bool IsEmailExists(string email)
        {
            return (GetEmail(email).Count > 0);
        }


        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <param name="email">Email.</param>        
        /// <returns>The table object that contains the email.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MasterDataSet.EmailDataTable GetEmail(string email)
        {
            using (EmailTableAdapter adapter = new EmailTableAdapter())
            {
                return adapter.GetEmail(email);
            }
        }


        /// <summary>
        /// Gets the list of emails by login id.
        /// </summary>
        /// <param name="loginId">Login Id.</param>        
        /// <returns>The System.Data.DataTable object that contains the emails.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static MasterDataSet.EmailDataTable GetEmails(Guid loginId)
        {
            using (EmailTableAdapter adapter = new EmailTableAdapter())
            {
                return adapter.GetEmails(loginId);
            }
        }

        /// <summary>
        /// Inserts the email.
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="loginId">Login Id</param>        
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void InsertEmail(string email, Guid loginId)
        {
            using (EmailTableAdapter adapter = new EmailTableAdapter())
            {
                adapter.Insert(email, loginId);
            }
        }

        /// <summary>
        /// Deletes all emails for login id or by email if login id is Guid.Empty
        /// </summary>
        /// <param name="loginId">Login Id</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEmails(Guid loginId, String email)
        {
            using (EmailTableAdapter adapter = new EmailTableAdapter())
            {
                adapter.Delete(loginId, email);
            }
        }

        #endregion
    }
}
