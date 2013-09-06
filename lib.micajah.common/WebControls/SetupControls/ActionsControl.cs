using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.SetupControls
{
    /// <summary>
    /// The control to manage actions.
    /// </summary>
    public class ActionsControl : UserControl
    {
        #region Members

        protected TreeView Tree;
        protected MagicForm EditForm;
        protected TreeView AlternativeParentsTree;
        protected Label ActionTypeIdLabel;
        protected ObjectDataSource EntityDataSource;
        protected Table CommandTable;
        protected LinkButton CloseButton;

        private ArrayList m_AlternativeParentActionsIdList;

        #endregion

        #region Private Properties

        private string StartUpClientScripts
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                HtmlGenericControl Div = EditForm.FindControl("Div1") as HtmlGenericControl;
                if (Div != null)
                {
                    sb.AppendFormat("var elem1 = document.getElementById('{0}'); ", Div.ClientID);
                    sb.AppendFormat("var elem2 = document.getElementById('{0}'); ", AlternativeParentsTree.ClientID);
                    sb.Append("if ((elem1 != null) && (elem2 != null)) elem1.appendChild(elem2);\r\n");
                }

                return sb.ToString();
            }
        }

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            BaseControl.LoadResources(EditForm, this.GetType().BaseType.Name);

            EditForm.Fields[7].HeaderText = Resources.ActionsControl_EditForm_AlternativeParentsField_Text;

            GroupField groupField = EditForm.Fields[8] as GroupField; // Submenu Group
            if (groupField != null) groupField.Text = Resources.ActionsControl_EditForm_SubmenuGroupField_Text;

            groupField = EditForm.Fields[12] as GroupField; // Detail Menu Group
            if (groupField != null) groupField.Text = Resources.ActionsControl_EditForm_DetailMenuGroupField_Text;

            CloseButton.Text = Resources.AutoGeneratedButtonsField_CloseButton_Text;
        }

        private void Tree_DataBind()
        {
            List<string> list = new List<string>();
            foreach (RadTreeNode node in Tree.GetAllNodes())
            {
                if (node.Expanded)
                    list.Add(node.Value);
            }

            CommonDataSet.ActionDataTable table = ActionProvider.GetActionsTree() as CommonDataSet.ActionDataTable;
            table.DefaultView.Sort = string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", table.ParentActionIdColumn.ColumnName, table.OrderNumberColumn.ColumnName, table.NameColumn.ColumnName);

            Tree.DataSource = table;
            Tree.DataBind();

            foreach (RadTreeNode node in Tree.GetAllNodes())
            {
                if (list.Contains(node.Value))
                    node.Expanded = true;
            }
        }

        private void AlternativeParentsTree_DataBind(Guid actionId)
        {
            AlternativeParentsTree.Visible = true;
            AlternativeParentsTree.DataSource = ActionProvider.GetAlternativeParentActionsTree(actionId);
            AlternativeParentsTree.DataBind();
        }

        private static void Tree_SetNodeAttributes(RadTreeNode node)
        {
            if (node == null) return;

            ActionType type = (ActionType)Enum.Parse(typeof(ActionType), node.Category);
            string imageUrl = null;
            Type typeOfThis = typeof(ActionsControl);

            if (node.ParentNode == null)
            {
                imageUrl = ResourceProvider.GetImageUrl(typeOfThis, "Folder.gif");
                node.Expanded = true;
            }

            switch (type)
            {
                case ActionType.GlobalNavigationLink:
                    if (node.ParentNode != null)
                    {
                        if (imageUrl == null) imageUrl = ResourceProvider.GetImageUrl(typeOfThis, "HyperLink.gif");
                    }
                    break;
                case ActionType.Page:
                    if (imageUrl == null) imageUrl = ResourceProvider.GetImageUrl(typeOfThis, "Page.gif");
                    break;
                case ActionType.Control:
                    imageUrl = ResourceProvider.GetImageUrl(typeOfThis, "Control.gif");
                    break;
            }

            node.ImageUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(imageUrl);
        }

        private void EditForm_SetDetailMenuFieldsVisible(bool visible)
        {
            EditForm.Fields[12].Visible = visible; // Detail Menu Group
            EditForm.Fields[13].Visible = visible; // IconUrl
            EditForm.Fields[14].Visible = visible; // LearnMoreUrl
            EditForm.Fields[15].Visible = visible; // ShowInDetailMenu
            EditForm.Fields[16].Visible = visible; // ShowChildrenInDetailMenu
            EditForm.Fields[17].Visible = visible; // ShowDescriptionInDetailMenu
            EditForm.Fields[18].Visible = visible; // GroupInDetailMenu
            EditForm.Fields[19].Visible = visible; // HighlightInDetailMenu
        }

        private void EditForm_SetSubmenuFieldsVisible(bool visible)
        {
            EditForm.Fields[8].Visible = visible; // Submenu Group
            EditForm.Fields[9].Visible = visible; // SubmenuItemType
            EditForm.Fields[10].Visible = visible; // SubmenuItemWidth
            EditForm.Fields[11].Visible = visible; // SubmenuItemImageUrl
        }

        private void EditForm_Reset()
        {
            if (Tree.SelectedNode != null) Tree.SelectedNode.Selected = false;
            ActionTypeIdLabel.Text = string.Empty;

            EditForm.Visible = false;
            CommandTable.Visible = false;
        }

        private void EditForm_ChangeType(RadTreeNode editedNode)
        {
            object obj = Support.ConvertStringToType(editedNode.Value, typeof(Guid));
            EditForm_ChangeType((ActionType)Enum.Parse(typeof(ActionType), editedNode.Category), ((obj == null) ? Guid.Empty : (Guid)obj), null);
        }

        private void EditForm_ChangeType(ActionType type, Guid actionId, Guid? parentActionId)
        {
            EditForm.Fields[2].Visible = true; // NavigateUrl
            EditForm.Fields[3].Visible = true; // OrderNumber
            EditForm.Fields[4].Visible = true; // AuthenticationRequired
            CheckBoxField checkBoxField = EditForm.Fields[5] as CheckBoxField; // InstanceRequired
            if (checkBoxField != null)
                checkBoxField.Visible = FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances;
            EditForm.Fields[6].Visible = true; // Visible
            EditForm.Fields[7].Visible = false; // AlternativeParents

            EditForm_SetSubmenuFieldsVisible(false);
            EditForm_SetDetailMenuFieldsVisible(false);

            AlternativeParentsTree.Visible = false;
            AlternativeParentsTree.DataSource = null;

            switch (type)
            {
                case ActionType.GlobalNavigationLink:
                    EditForm.Fields[5].Visible = false; // InstanceRequired
                    break;
                case ActionType.Control:
                    EditForm.Fields[2].Visible = false; // NavigateUrl
                    EditForm.Fields[3].Visible = false; // OrderNumber
                    EditForm.Fields[4].Visible = false; // AuthenticationRequired
                    EditForm.Fields[5].Visible = false; // InstanceRequired
                    EditForm.Fields[6].Visible = false; // Visible
                    break;
                case ActionType.Page:
                    // AuthenticationRequired
                    if (parentActionId.HasValue) EditForm.Fields[4].Visible = (!ActionProvider.AuthenticationRequired(parentActionId.Value)); // AuthenticationRequired
                    if (actionId != Guid.Empty) EditForm.Fields[4].Visible = (!ActionProvider.AuthenticationRequired(actionId)); // AuthenticationRequired

                    // AlternativeParents
                    m_AlternativeParentActionsIdList = ActionProvider.GetAlternativeParentActionsIdList(actionId);
                    if ((m_AlternativeParentActionsIdList != null) && (m_AlternativeParentActionsIdList.Count > 0))
                    {
                        EditForm.Fields[7].Visible = true; // AlternativeParents
                        AlternativeParentsTree_DataBind(actionId);
                    }
                    else
                        EditForm.Fields[7].Visible = false; // AlternativeParents

                    EditForm_SetSubmenuFieldsVisible(true);
                    EditForm_SetDetailMenuFieldsVisible(true);
                    break;
            }
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.InitializeSetupPage(this.Page);

            if (!IsPostBack)
            {
                LoadResources();
                Tree_DataBind();
            }

            MagicForm.ApplyStyle(CommandTable, EditForm.ColorScheme);
        }

        protected void Tree_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            if (e == null) return;

            DataRowView drv = e.Node.DataItem as DataRowView;
            if (drv == null) return;

            if (e.Node.Visible)
            {
                e.Node.Category = drv["ActionTypeId"].ToString();
                Tree_SetNodeAttributes(e.Node);
            }
        }

        protected void Tree_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            EditForm.Visible = true;
            CommandTable.Visible = true;

            if (e == null) return;

            EditForm_ChangeType(e.Node);
        }

        protected void AlternativeParentsTree_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            if (e == null) return;

            if (e.Node.ParentNode == null)
            {
                e.Node.Checkable = false;
            }
            else
            {
                DataRowView drv = e.Node.DataItem as DataRowView;
                if (drv != null)
                {
                    if (!e.Node.Visible) return;

                    if ((ActionType)Convert.ToInt32(drv["ActionTypeId"], CultureInfo.InvariantCulture) != ActionType.Page)
                        e.Node.Checkable = false;
                }

                object obj = Support.ConvertStringToType(e.Node.Value, typeof(Guid));
                Guid id = ((obj == null) ? Guid.Empty : (Guid)obj);
                string value = Tree.SelectedNode.Value;

                if ((e.Node.Value == value) || (id == ActionProvider.UsersPageActionId))
                {
                    e.Node.Checkable = false;
                }
                else if (m_AlternativeParentActionsIdList != null && m_AlternativeParentActionsIdList.Contains(id))
                {
                    e.Node.Checked = true;
                    e.Node.ExpandParentNodes();
                }
            }
        }

        protected void CloseButton_Click(object sender, EventArgs e)
        {
            EditForm_Reset();
        }

        #endregion

        #region Overriden Methods

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (AlternativeParentsTree.Visible)
            {
                string scripts = StartUpClientScripts;
                if (!string.IsNullOrEmpty(scripts)) ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "StartUpClientScripts", scripts, true);
            }
        }

        #endregion
    }
}
