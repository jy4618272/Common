using Micajah.Common.Bll.Providers;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.AdminControls
{
    /// <summary>
    /// The control to manage groups.
    /// </summary>
    public class GroupsControl : BaseControl
    {
        #region Overriden Methods

        protected override void LoadResources()
        {
            base.LoadResources();
            HyperLinkField field = (List.Columns[1] as HyperLinkField);
            if (field != null)
            {
                field.Text = Resources.GroupsControl_List_RolesLinkColumns_Text;
                field.DataNavigateUrlFormatString = CustomUrlProvider.CreateApplicationAbsoluteUrl(string.Concat(ResourceProvider.GroupsInstancesRolesPageVirtualPath, "?GroupId={0:N}"));
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (InstanceProvider.GetInstances(UserContext.Current.OrganizationId, true).Count == 0)
                {
                    MasterPage.Message = Resources.GroupsControl_NoInstanceError_Message;
                    MasterPage.MessageDescription = string.Format(CultureInfo.CurrentCulture, Resources.GroupsControl_NoInstanceError_Description, CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.InstancePageVirtualPath));
                    List.Visible = false;
                }
            }

            base.OnLoad(e);
        }

        #endregion

        #region Protected Methods

        protected void List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e == null) return;

            if ((e.Row != null) && (e.Row.DataItem != null))
            {
                if ((bool)DataBinder.Eval(e.Row.DataItem, "BuiltIn"))
                {
                    int lastIndex = e.Row.Cells.Count - 1;

                    if (this.List.Theme != MasterPageTheme.Modern)
                    {
                        BaseControl.HideControls(e.Row.Cells[0].Controls);
                    }

                    BaseControl.HideControls(e.Row.Cells[lastIndex - 1].Controls);
                    BaseControl.HideControls(e.Row.Cells[lastIndex].Controls);
                }
            }
        }

        protected void LdapGroupMappingsLink_Init(object sender, EventArgs e)
        {
            HyperLink lnk = sender as HyperLink;
            if (lnk != null)
            {
                Micajah.Common.Bll.Action action = ActionProvider.FindAction(ActionProvider.LdapGroupMappingsPageActionId);
                if (action != null)
                {
                    if (action.AccessDenied())
                        lnk.Visible = false;
                    else
                    {
                        lnk.Text = action.Name;
                        lnk.NavigateUrl = action.AbsoluteNavigateUrl;
                    }
                }
            }
        }

        #endregion
    }
}
