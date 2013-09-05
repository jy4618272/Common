using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.SetupControls
{
    /// <summary>
    /// The control to manage roles.
    /// </summary>
    public class RolesControl : UserControl
    {
        #region Members

        protected CommonGridView List;
        protected ObjectDataSource EntityListDataSource;

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            BaseControl.LoadResources(List, this.GetType().BaseType.Name);
            List.Columns[4].HeaderText = Resources.RolesControl_List_StartActionColumn_HeaderText;
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.InitializeSetupPage(this.Page);

            if (!IsPostBack)
            {
                this.LoadResources();

                List.Sort("Rank", SortDirection.Ascending);
            }
        }

        protected void EntityListDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e == null) return;

            int count = 0;
            DataTable table = (e.ReturnValue as DataTable);
            if (table != null) count = table.Rows.Count;
            List.AllowPaging = (count > List.PageSize);
        }

        protected void List_Action(object sender, CommonGridViewActionEventArgs e)
        {
            if (e == null) return;

            switch (e.Action)
            {
                case CommandActions.Select:
                    Response.Redirect(string.Concat(ResourceProvider.RoleEditPageVirtualPath, "?RoleId=", ((Guid)List.DataKeys[e.RowIndex]["RoleId"]).ToString("N")));
                    break;
            }
        }

        protected void List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e == null) return;

            if ((e.Row != null) && (e.Row.DataItem != null))
            {
                if (RoleProvider.IsBuiltIn((Guid)DataBinder.Eval(e.Row.DataItem, "RoleId")))
                    e.Row.Attributes["enableSelect"] = bool.FalseString;
                else
                    e.Row.ToolTip = Resources.RolesControl_List_Row_ToolTip;
            }
        }

        protected static string GetUrl(object roleId, object actionId)
        {
            string str = null;
            if (RoleProvider.IsBuiltIn((Guid)roleId))
                str = Resources.RolesControl_BuiltInRoleUrl;
            else
            {
                Micajah.Common.Bll.Action action = ActionProvider.PagesAndControls.FindByActionId((Guid)actionId);
                str = ((action == null) ? string.Empty : CustomUrlProvider.CreateApplicationRelativeUrl(action.AbsoluteNavigateUrl));
            }
            return str;
        }

        #endregion
    }
}
