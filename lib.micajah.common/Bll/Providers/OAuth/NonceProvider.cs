using DotNetOpenAuth.Messaging.Bindings;
using Micajah.Common.Dal.TableAdapters;
using System;

namespace Micajah.Common.Bll.Providers.OAuth
{
    public class NonceProvider : INonceStore
    {
        #region Members

        private static NonceProvider s_Instance;
        private static NonceProvider s_Current;

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the instance of this class.
        /// </summary>
        internal static NonceProvider Instance
        {
            get
            {
                if (s_Instance == null) s_Instance = new NonceProvider();
                return s_Instance;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the instance of the class.
        /// </summary>
        public static NonceProvider Current
        {
            get { return ((s_Current == null) ? Instance : s_Current); }
            set { s_Current = value; }
        }

        #endregion

        #region Public Methods

        public bool StoreNonce(string context, string nonce, DateTime timestampUtc)
        {
            return (MasterTableAdapters.Current.NonceTableAdapter.Insert(context, nonce, timestampUtc) == 1);
        }

        #endregion
    }
}
