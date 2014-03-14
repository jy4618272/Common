using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;

namespace Micajah.Common.WebControls.SetupControls
{
    /// <summary>
    /// The control to manage databases.
    /// </summary>
    public class DatabasesControl : BaseControl
    {
        #region Private Methods

        private bool DatabaseExists(IOrderedDictionary values)
        {
            string errorMessage = string.Empty;
            if (!DatabaseProvider.DatabaseExists(values["Name"].ToString(), values["UserName"].ToString(), values["Password"].ToString(), (Guid)Support.ConvertStringToType(values["DatabaseServerId"].ToString(), typeof(Guid)), out errorMessage))
            {
                this.ErrorPanel.InnerHtml = errorMessage;
                this.ErrorPanel.Visible = true;

                return false;
            }
            return true;
        }

        #endregion

        #region Overriden Methods

        protected override void OnLoad(EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.InitializeSetupPage(this.Page);

            if (DatabaseServerProvider.GetDatabaseServers().Count == 0)
            {
                List.EmptyDataText = string.Format(CultureInfo.CurrentCulture
                    , Resources.DatabasesControl_ErrorMessage_NoDatabaseServer
                    , CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.DatabaseServersPageVirtualPath));
                List.ShowAddLink = EditForm.Visible = false;
            }

            base.OnLoad(e);
        }

        protected override void LoadResources()
        {
            base.LoadResources();
            (EditForm.Fields[5] as CheckBoxField).Text = Resources.DatabasesControl_EditForm_PrivateField_Text;
        }

        protected void EditForm_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            if (e != null) e.Cancel = (!DatabaseExists(e.Values));
        }

        protected void EditForm_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            if (e != null) e.Cancel = (!DatabaseExists(e.NewValues));
        }

        #endregion
    }
}
