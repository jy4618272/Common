using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.AdminControls
{
    /// <summary>
    /// The control to manage the application group roles and actions in instances.
    /// </summary>
    public class LdapGroupMappingsControl : UserControl
    {
        #region Members

        protected Table LdapGroupMappingsTable;
        protected Literal CaptionLiteral;
        protected Label AddGroupLiteral;
        protected Label DomainLiteral;
        protected Label LdapGroupLiteral;
        protected Button SaveButton;
        protected ComboBox DomainList;
        protected ComboBox LdapGroupList;
        protected TableCell ButtonHeaderCell;
        protected TableCell ButtonCell;
        protected CommonGridView CommonGridView1;
        protected TreeView AppGroupTreeView;
        protected UpdateProgress FormUpdateProgress;

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            CaptionLiteral.Text = Resources.LdapGroupMappingsControl_ActionsTable_Caption;
            AddGroupLiteral.Text = Resources.LdapGroupMappingsControl_AddGroupLiteral_Text;
            DomainLiteral.Text = Resources.LdapGroupMappingsControl_DomainLiteral_Text;
            LdapGroupLiteral.Text = Resources.LdapGroupMappingsControl_LdapGroupLiteral_Text;
            SaveButton.Text = Resources.LdapGroupMappingsControl_ActionsTable_Caption;

            FormUpdateProgress.ProgressText = Resources.LdapGroupMappingsControl_UpdateProgress_Text;
            FormUpdateProgress.Timeout = int.MaxValue;
            FormUpdateProgress.HideAfter = -1;
            FormUpdateProgress.ShowSuccessText = false;
            FormUpdateProgress.PostBackControlId = this.SaveButton.ClientID;
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.InitializeAdminPage(this.Page);

            if (!IsPostBack)
            {
                CommonGridView1.DeleteButtonCaption = DeleteButtonCaptionType.Remove;
                this.LoadResources();
                DomainList.CausesValidation = false;
            }
        }

        protected void OrganizationIdDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["organizationId"] = UserContext.Current.SelectedOrganizationId;
        }

        protected void LdapGroupListDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
            {
                RadComboBoxItem item = DomainList.SelectedItem;
                e.InputParameters["organizationId"] = UserContext.Current.SelectedOrganizationId;
                if (item != null)
                    e.InputParameters["domainName"] = item.Text;
                else
                    e.InputParameters["domainName"] = string.Empty;
            }
        }

        protected void DomainList_DataBound(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
                comboBox.Items.Insert(0, new RadComboBoxItem(null, null));
        }

        protected void LdapGroupList_DataBound(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
                comboBox.Items.Insert(0, new RadComboBoxItem(null, null));
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (LdapGroupMappingsTable.Visible)
                MagicForm.ApplyStyle(LdapGroupMappingsTable);

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
            {
                ButtonHeaderCell.Visible = true;
                ButtonCell.ColumnSpan = 0;
            }
            else
            {
                ButtonHeaderCell.Visible = false;
                ButtonCell.ColumnSpan = 2;
            }
        }

        protected void AppGroupTreeView_NodeDataBound(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            if (e.Node.DataItem != null)
            {
                DataRowView dataSourceRow = (DataRowView)e.Node.DataItem;
                if (dataSourceRow != null)
                    e.Node.ToolTip = dataSourceRow["InstancesRoles"].ToString();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (new Guid(DomainList.SelectedItem.Value) != new Guid() && new Guid(LdapGroupList.SelectedItem.Value) != new Guid())
            {
                LdapInfoProvider.InsertGroupMapping(new Guid(AppGroupTreeView.SelectedValue), UserContext.Current.SelectedOrganizationId, AppGroupTreeView.SelectedNode.Text, new Guid(DomainList.SelectedItem.Value), DomainList.SelectedItem.Text, new Guid(LdapGroupList.SelectedItem.Value), LdapGroupList.SelectedItem.Text);
                CommonGridView1.DataBind();
                LdapInfoProvider.UpdateLdapDomains(UserContext.Current.SelectedOrganizationId);

                AppGroupTreeView.UncheckAllNodes();
                AppGroupTreeView.UnselectAllNodes();
                DomainList.SelectedIndex = 0;
                LdapGroupList.SelectedIndex = 0;
            }
        }

        protected void DomainList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
                LdapGroupList.DataBind();
        }

        #endregion
    }
}
