using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.SetupControls
{
    /// <summary>
    /// The control to create new or edit specified role.
    /// </summary>
    public class RoleEditControl : UserControl
    {
        #region Members

        protected MagicForm EditForm;
        protected ObjectDataSource EntityDataSource;
        protected TreeView Atv;
        protected Table CommandTable;
        protected LinkButton CloseButton;

        private ArrayList m_ActionIdList;

        #endregion

        #region Private Properties

        private Guid RoleId
        {
            get
            {
                object obj = Support.ConvertStringToType(Request.QueryString["roleid"], typeof(Guid));
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
        }

        private string ClientScripts
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                HtmlGenericControl Div = EditForm.FindControl("Div1") as HtmlGenericControl;
                if (Div != null)
                {
                    sb.AppendFormat("var Elem1 = document.getElementById('{0}'); ", Div.ClientID);
                    sb.AppendFormat("var Elem2 = document.getElementById('{0}'); ", Atv.ClientID);
                    sb.Append("if (Elem1 && Elem2) Elem1.appendChild(Elem2);\r\n");
                }

                return sb.ToString();
            }
        }

        #endregion

        #region Protected Properties

        protected static string StartActionValidatorErrorMessage
        {
            get { return Resources.RoleEditControl_EditForm_StartActionValidator_ErrorMessage; }
        }

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            BaseControl.LoadResources(EditForm, this.GetType().BaseType.Name);

            EditForm.Fields[4].HeaderText = Resources.RoleEditControl_EditForm_ActionField_HeaderText;

            CloseButton.Text = Resources.AutoGeneratedButtonsField_CloseButton_Text;
        }

        private void Atv_DataBind()
        {
            Guid startActionId = Guid.Empty;
            if (this.RoleId != Guid.Empty)
            {
                startActionId = Micajah.Common.Bll.Providers.RoleProvider.GetStartActionId(RoleId);
                m_ActionIdList = Micajah.Common.Bll.Providers.ActionProvider.GetActionIdListByRoleId(RoleId);
            }

            Atv.DataSource = ActionProvider.GetActionsTree();
            Atv.DataBind();

            if (this.RoleId != Guid.Empty)
            {
                RadTreeNode node = Atv.FindNodeByValue(startActionId.ToString());
                if (node != null)
                {
                    node.Selected = true;
                    node.ExpandParentNodes();
                    node.ToolTip = Resources.RoleEditControl_StartAction_ToolTip;
                }
            }
        }

        private void RedirectToRolePage()
        {
            Response.Redirect(ResourceProvider.RolesPageVirtualPath);
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.InitializeSetupPage(this.Page);

            if (!IsPostBack)
            {
                bool editMode = (this.RoleId != Guid.Empty);
                if (RoleProvider.IsBuiltIn(this.RoleId) || (editMode && (RoleProvider.GetRoleRow(RoleId) == null)))
                    this.RedirectToRolePage();

                LoadResources();
                Atv_DataBind();
            }

            MagicForm.ApplyStyle(CommandTable, EditForm.ColorScheme);
        }

        protected void Atv_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            if (e == null) return;

            object obj = Support.ConvertStringToType(e.Node.Value, typeof(Guid));
            Guid actionId = ((obj == null) ? Guid.Empty : (Guid)obj);

            int actionTypeId = Convert.ToInt32(DataBinder.Eval(e.Node.DataItem, "ActionTypeId"), CultureInfo.InvariantCulture);
            bool authenticationIsNotRequired = (!Convert.ToBoolean(DataBinder.Eval(e.Node.DataItem, "AuthenticationRequired"), CultureInfo.InvariantCulture));
            bool builtIn = Convert.ToBoolean(DataBinder.Eval(e.Node.DataItem, "BuiltIn"), CultureInfo.InvariantCulture);

            if (e.Node.ParentNode == null)
            {
                e.Node.Checkable = false;
            }
            else
            {
                if (!e.Node.Visible) return;

                if ((actionId == ActionProvider.MyAccountGlobalNavigationLinkActionId)
                    || (authenticationIsNotRequired && (actionTypeId != (int)ActionType.Control))
                    || builtIn)
                    e.Node.Checkable = false;

                if (m_ActionIdList != null && m_ActionIdList.Contains(actionId))
                {
                    e.Node.Checked = true;
                }

                e.Node.ExpandParentNodes();
            }
        }

        protected void CloseButton_Click(object sender, EventArgs e)
        {
            RedirectToRolePage();
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Raises the PreRender event and registers client scripts.
        /// </summary>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string scripts = ClientScripts;
            if ((!string.IsNullOrEmpty(scripts)) && (!Page.ClientScript.IsStartupScriptRegistered("RoleEditClientScripts")))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "RoleEditClientScripts", scripts, true);
        }

        #endregion
    }
}
