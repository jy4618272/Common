using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.AdminControls
{
    /// <summary>
    /// The control to manage the group roles and actions in instances.
    /// </summary>
    public class GroupsInstancesRolesControl : MasterControl
    {
        #region Members

        protected CommonGridView List;
        protected MagicForm EditForm;
        protected Table ActionsTable;
        protected TreeView Atv;
        protected Literal CaptionLiteral;
        protected Button SaveButton;
        protected PlaceHolder ButtonsSeparator;
        protected LinkButton CancelButton;
        protected ObjectDataSource EntityListDataSource;
        protected ObjectDataSource EntityDataSource;

        private Guid m_StartActionId;
        private ArrayList m_ActionIdList;
        private HtmlGenericControl m_ErrorDiv;

        #endregion

        #region Private Properties

        private ArrayList CheckedActionIdList
        {
            get
            {
                ArrayList list = new ArrayList();
                object obj = null;
                foreach (RadTreeNode node in Atv.CheckedNodes)
                {
                    obj = Support.ConvertStringToType(node.Value, typeof(Guid));
                    if (obj != null) list.Add(obj);
                }
                return list;
            }
        }

        private Guid GroupId
        {
            get
            {
                object obj = Support.ConvertStringToType(Request.QueryString["groupid"], typeof(Guid));
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
        }

        protected HtmlGenericControl ErrorDiv
        {
            get
            {
                if (m_ErrorDiv == null) m_ErrorDiv = EditForm.FindControl("ErrorDiv") as HtmlGenericControl;
                return m_ErrorDiv;
            }
        }

        private string GroupName
        {
            get
            {
                object obj = this.ViewState["GroupName"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { this.ViewState["GroupName"] = value; }
        }

        #endregion

        #region Protected Properties

        protected static string ActionsLinkText
        {
            get { return Resources.GroupsInstancesRolesControl_List_ActionsLinkColumn_Text; }
        }

        protected static string SettingsLinkText
        {
            get { return Resources.GroupsInstancesRolesControl_List_SettingsLinkColumn_Text; }
        }

        #endregion

        #region Private Methods

        protected virtual void AddBreadcrumbs()
        {
            if (this.MasterPage != null)
            {
                Micajah.Common.Bll.Action item = new Micajah.Common.Bll.Action();
                item.ActionId = Guid.NewGuid();
                item.Name = EditForm.Caption;
                item.ParentAction = this.MasterPage.ActiveAction;
                UserContext.Breadcrumbs.Add(item);

                this.MasterPage.UpdateBreadcrumbs();
            }
        }

        private void AddBreadCrumbs(bool addGroupName)
        {
            if (this.MasterPage != null)
            {
                Micajah.Common.Bll.Action item = null;

                if (addGroupName)
                {
                    item = this.MasterPage.ActiveAction.Clone();
                    item.Name = this.GroupName;
                    item.NavigateUrl += "?GroupId=" + this.GroupId.ToString("N");
                }
                else
                {
                    item = new Micajah.Common.Bll.Action();
                    item.ActionId = Guid.NewGuid();
                    item.Name = Resources.GroupsInstancesRolesControl_ActionsTable_Caption;
                    item.ParentAction = this.MasterPage.ActiveAction;
                }

                UserContext.Breadcrumbs.Add(item);

                this.MasterPage.UpdateBreadcrumbs(false);
            }
        }

        private void Atv_DataBind(Guid groupId, Guid instanceId)
        {
            Guid roleId = GroupProvider.GetGroupInstanceRole(UserContext.Current.OrganizationId, groupId, instanceId);

            m_ActionIdList = GroupProvider.GetActionIdList(groupId, instanceId, roleId);
            m_StartActionId = RoleProvider.GetStartActionId(roleId);

            Atv.DataSource = ActionProvider.GetActionsTree();
            Atv.DataBind();
        }

        private void LoadResources()
        {
            BaseControl.LoadResources(List, this.GetType().BaseType.Name);

            EditForm.ObjectName = Resources.GroupsInstancesRolesControl_EditForm_ObjectName;
            EditForm.Fields[0].HeaderText = Resources.GroupsInstancesRolesControl_EditForm_InstanceListField_HeaderText;
            EditForm.Fields[1].HeaderText = Resources.GroupsInstancesRolesControl_EditForm_RoleListField_HeaderText;

            CaptionLiteral.Text = MagicForm.GetCaption(DetailsViewMode.Edit, Resources.GroupsInstancesRolesControl_ActionsTable_Caption);
            SaveButton.Text = MagicForm.GetUpdateButtonText(DetailsViewMode.Edit, Resources.GroupsInstancesRolesControl_ActionsTable_Caption);
            CancelButton.Text = Resources.AutoGeneratedButtonsField_CancelButton_Text;
        }

        private void ShowEditForm()
        {
            Atv.Nodes.Clear();
            ActionsTable.Visible = List.Visible = false;
            List.SelectedIndex = -1;
            EditForm.Visible = true;
        }

        private void ShowList()
        {
            List.SelectedIndex = -1;
            EditForm.Visible = ActionsTable.Visible = false;
            List.Visible = true;
            this.AddBreadCrumbs(true);
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClientDataSet.GroupRow row = GroupProvider.GetGroupRow(GroupId);
                if (row != null)
                {
                    MasterPage.CustomName = this.GroupName = row.Name;

                    this.LoadResources();

                    BaseControl.Initialize(List);
                    List.AllowSorting = false;
                    List.AutoGenerateEditButton = false;

                    List.Columns[0].Visible = EditForm.Fields[0].Visible = FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances;

                    if ((!FrameworkConfiguration.Current.Actions.EnableOverride) || ActionProvider.OnlyBuiltInActionsAvailable)
                        List.Columns[2].Visible = false;
                }
                else Response.Redirect(ResourceProvider.GroupsPageVirtualPath);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (ActionsTable.Visible) MagicForm.ApplyStyle(ActionsTable);
            AutoGeneratedButtonsField.InsertButtonSeparator(ButtonsSeparator);
            List.ShowAddLink = ((List.Rows.Count == 0) || (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances));
        }

        protected void EntityListDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e == null) return;

            int count = 0;
            DataTable table = (e.ReturnValue as DataTable);
            if (table != null) count = table.Rows.Count;
            List.AllowPaging = (count > List.PageSize);
        }

        protected void Atv_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            if (e == null) return;

            object obj = Support.ConvertStringToType(e.Node.Value, typeof(Guid));
            Guid actionId = ((obj == null) ? Guid.Empty : (Guid)obj);

            int actionTypeId = Convert.ToInt32(DataBinder.Eval(e.Node.DataItem, "ActionTypeId"), CultureInfo.InvariantCulture);
            bool authenticationIsNotRequired = (!Convert.ToBoolean(DataBinder.Eval(e.Node.DataItem, "AuthenticationRequired"), CultureInfo.InvariantCulture));
            bool builtIn = Convert.ToBoolean(DataBinder.Eval(e.Node.DataItem, "BuiltIn"), CultureInfo.InvariantCulture);

            if (actionId == m_StartActionId) e.Node.Category = "NonCheckable";
            if ((actionId == ActionProvider.MyAccountGlobalNavigationLinkActionId)
                || (authenticationIsNotRequired && (actionTypeId != (int)ActionType.Control))
                || builtIn)
                e.Node.Checkable = false;

            if (e.Node.ParentNode == null)
            {
                e.Node.Checkable = false;
                e.Node.Expanded = true;
            }
            else
            {
                if (m_ActionIdList != null)
                    if (m_ActionIdList.Contains(actionId)) e.Node.Checked = true;
            }
        }

        protected void List_Action(object sender, CommonGridViewActionEventArgs e)
        {
            if (e == null) return;

            switch (e.Action)
            {
                case CommandActions.Add:
                    ShowEditForm();
                    this.AddBreadcrumbs();
                    break;
                case CommandActions.Select:
                    Atv_DataBind((Guid)List.DataKeys[e.RowIndex]["GroupId"], (Guid)List.DataKeys[e.RowIndex]["InstanceId"]);
                    ActionsTable.Visible = true;
                    List.Visible = EditForm.Visible = false;
                    this.AddBreadCrumbs(false);
                    break;
            }
        }

        protected void EntityDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
            {
                if (!FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                {
                    Instance inst = InstanceProvider.GetFirstInstance();
                    if (inst != null)
                        e.InputParameters["instanceId"] = inst.InstanceId;
                }
            }
        }

        protected void EditForm_ItemCommand(object sender, CommandEventArgs e)
        {
            if (e != null)
            {
                if (e.CommandName.Equals("Cancel")) this.ShowList();
            }
        }

        protected void EditForm_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (e == null) return;

            e.KeepInInsertMode = true;
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;

                Exception ex = e.Exception.GetBaseException();
                if (ex is ConstraintException)
                    ErrorDiv.InnerHtml = Resources.GroupsInstancesRolesControl_EditForm_InsertErrorMessage;
                else
                    ErrorDiv.InnerHtml = ex.Message;
                ErrorDiv.Visible = true;
            }
            else
            {
                this.ShowList();
                List.DataBind();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            GroupProvider.GroupsInstancesActionsAcceptChanges((Guid)List.SelectedDataKey["GroupId"], (Guid)List.SelectedDataKey["InstanceId"], this.CheckedActionIdList);
            this.ShowList();
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            this.ShowList();
        }

        protected static bool LinkVisible(object value)
        {
            return ((Guid)value != RoleProvider.InstanceAdministratorRoleId);
        }

        protected static string GetSettingsLink(object groupId, object instanceId)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}?GroupId={1:N}&InstanceId={2:N}", CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.GroupSettingsInInstancePageVirtualPath), groupId, instanceId);
        }

        #endregion
    }
}
