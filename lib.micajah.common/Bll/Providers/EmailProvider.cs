using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
using System;
using System.ComponentModel;
using System.Data;

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
            return (GetEmail(email).Rows.Count > 0);
        }


        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <param name="email">Email.</param>        
        /// <returns>The System.Data.DataTable object that contains the email.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetEmail(string email)
        {
            MasterDataSet.EmailDataTable table = null;
            try
            {
                table = new MasterDataSet.EmailDataTable();
                MasterDataSetTableAdapters.Current.EmailTableAdapter.Fill(table, 0, email);
                return table;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }


        /// <summary>
        /// Gets the list of emails by login id.
        /// </summary>
        /// <param name="loginId">Login Id.</param>        
        /// <returns>The System.Data.DataTable object that contains the emails.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public static DataTable GetEmails(Guid loginId)
        {
            MasterDataSet.EmailDataTable table = null;
            try
            {
                table = new MasterDataSet.EmailDataTable();
                MasterDataSetTableAdapters.Current.EmailTableAdapter.Fill(table, 1, loginId);
                table.DefaultView.Sort = "[Email] ASC";
                return table;
            }
            finally
            {
                if (table != null) table.Dispose();
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
            MasterDataSetTableAdapters.Current.EmailTableAdapter.Insert(email, loginId);
        }

        /// <summary>
        /// Deletes all emails for login id or by email if login id is Guid.Empty
        /// </summary>
        /// <param name="loginId">Login Id</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void DeleteEmails(Guid loginId, String email)
        {
            MasterDataSetTableAdapters.Current.EmailTableAdapter.Delete(loginId, email);
        }

        #endregion
    }
}
