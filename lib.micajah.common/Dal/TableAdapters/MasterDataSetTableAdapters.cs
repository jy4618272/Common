using Micajah.Common.Bll.Providers;
using System.Collections;

namespace Micajah.Common.Dal.TableAdapters
{
    /// <summary>
    /// The container class for the tables adapters of the common and master data sets.
    /// </summary>
    public sealed class MasterDataSetTableAdapters : TableAdaptersHolder
    {
        #region Members

        private static MasterDataSetTableAdapters s_Current;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the current instance of the class.
        /// </summary>
        public static MasterDataSetTableAdapters Current
        {
            get
            {
                if (s_Current == null)
                    s_Current = new MasterDataSetTableAdapters();
                return s_Current;
            }
            set
            {
                s_Current = value;
                Micajah.Common.Application.WebApplication.RefreshCommonData();
            }
        }

        public ITableAdapter DatabaseTableAdapter { get { return this.TableAdapters[TableName.Database]; } }

        public ITableAdapter DatabaseServerTableAdapter { get { return this.TableAdapters[TableName.DatabaseServer]; } }

        public ITableAdapter CountryTableAdapter { get { return this.TableAdapters[TableName.Country]; } }

        public ITableAdapter CustomUrlTableAdapter { get { return this.TableAdapters[TableName.CustomUrl]; } }

        public ITableAdapter EmailTableAdapter { get { return this.TableAdapters[TableName.Email]; } }

        public ITableAdapter EmailSuffixTableAdapter { get { return this.TableAdapters[TableName.EmailSuffix]; } }

        public ITableAdapter InvitedLoginTableAdapter { get { return this.TableAdapters[TableName.InvitedLogin]; } }

        public ITableAdapter GroupMappingsTableAdapter { get { return this.TableAdapters[TableName.GroupMappings]; } }

        public ITableAdapter OrganizationTableAdapter { get { return this.TableAdapters[TableName.Organization]; } }

        public ITableAdapter OrganizationsLdapGroupsTableAdapter { get { return this.TableAdapters[TableName.OrganizationsLdapGroups]; } }

        public ITableAdapter ResetPasswordRequestTableAdapter { get { return this.TableAdapters[TableName.ResetPasswordRequest]; } }

        public ITableAdapter ResourceTableAdapter { get { return this.TableAdapters[TableName.Resource]; } }

        public ITableAdapter UnitsOfMeasureAdapter { get { return this.TableAdapters[TableName.UnitsOfMeasure]; } }

        public ITableAdapter UnitsOfMeasureConversionAdapter { get { return this.TableAdapters[TableName.UnitsOfMeasureConversion]; } }

        public ITableAdapter WebsiteTableAdapter { get { return this.TableAdapters[TableName.Website]; } }

        public ITableAdapter NonceTableAdapter { get { return this.TableAdapters[TableName.Nonce]; } }

        public ITableAdapter OAuthTokenTableAdapter { get { return this.TableAdapters[TableName.OAuthToken]; } }

        public ITableAdapter OAuthConsumerTableAdapter { get { return this.TableAdapters[TableName.OAuthConsumer]; } }

        #endregion

        #region Constructors

        public MasterDataSetTableAdapters()
        {
            this.Initialize();
        }

        public MasterDataSetTableAdapters(ICollection adapters)
            : base(adapters)
        {
            this.Initialize();
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            this.AddAdapter(TableName.Database, typeof(DatabaseTableAdapter));
            this.AddAdapter(TableName.DatabaseServer, typeof(DatabaseServerTableAdapter));
            this.AddAdapter(TableName.Country, typeof(CountryTableAdapter));
            this.AddAdapter(TableName.CustomUrl, typeof(CustomUrlTableAdapter));
            this.AddAdapter(TableName.Email, typeof(EmailTableAdapter));
            this.AddAdapter(TableName.EmailSuffix, typeof(EmailSuffixTableAdapter));
            this.AddAdapter(TableName.InvitedLogin, typeof(InvitedLoginTableAdapter));
            this.AddAdapter(TableName.GroupMappings, typeof(GroupMappingsTableAdapter));
            this.AddAdapter(TableName.Organization, typeof(OrganizationTableAdapter));
            this.AddAdapter(TableName.OrganizationsLdapGroups, typeof(OrganizationsLdapGroupsTableAdapter));
            this.AddAdapter(TableName.ResetPasswordRequest, typeof(ResetPasswordRequestTableAdapter));
            this.AddAdapter(TableName.Resource, typeof(ResourceTableAdapter));
            this.AddAdapter(TableName.UnitsOfMeasure, typeof(UnitsOfMeasureAdapter));
            this.AddAdapter(TableName.UnitsOfMeasureConversion, typeof(UnitsOfMeasureConversionAdapter));
            this.AddAdapter(TableName.Website, typeof(WebsiteTableAdapter));
            this.AddAdapter(TableName.Nonce, typeof(NonceTableAdapter));
            this.AddAdapter(TableName.OAuthToken, typeof(OAuthTokenTableAdapter));
            this.AddAdapter(TableName.OAuthConsumer, typeof(OAuthConsumerTableAdapter));
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Fills the tables of specified CommonDataSet.
        /// </summary>
        /// <param name="dataSet">The CommonDataSet to fill.</param>
        internal void Fill(CommonDataSet dataSet)
        {
            SettingProvider.Fill(dataSet);
            RoleProvider.Fill(dataSet);
            ActionProvider.Fill(dataSet);
            WebsiteTableAdapter.Fill(dataSet.Website);
            DatabaseServerTableAdapter.Fill(dataSet.DatabaseServer);
            DatabaseTableAdapter.Fill(dataSet.Database);
            OrganizationTableAdapter.Fill(dataSet.Organization);
        }

        #endregion
    }
}
