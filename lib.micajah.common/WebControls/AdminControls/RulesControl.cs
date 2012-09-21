using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.AdminControls
{
    public class RulesControl : BaseControl
    {
        #region Members

        protected Panel SearchPanel;
        protected ComboBox InstanceList;
        protected MagicForm FormRuleEngine;
        protected ObjectDataSource InstancesDataSource;

        private UserContext m_UserContext;

        #endregion

        #region Protected Methods

        protected void ButtonUpdateOrder_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in List.Rows)
            {
                TextBox TextBoxOrder = row.FindControl("TextBoxOrder") as TextBox;
                object dataId = List.DataKeys[row.DataItemIndex].Value;
                if (TextBoxOrder != null && dataId != null)
                {
                    int orderNumber = 0;
                    if (int.TryParse(TextBoxOrder.Text, out orderNumber))
                        RuleEngineProvider.UpdateOrderNumber((Guid)dataId, orderNumber);
                }
            }
            List.DataBind();
        }

        protected void InstanceList_DataBound(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                if (m_UserContext.IsOrganizationAdministrator)
                {
                    using (RadComboBoxItem item = new RadComboBoxItem(Resources.RulesControl_EditForm_InstanceList_AllInstancesItem_Text, string.Empty))
                    {
                        comboBox.Items.Insert(0, item);
                    }
                }
                else if (comboBox.Items.Count == 1)
                    SearchPanel.Visible = InstanceList.Visible = false;
            }
        }

        protected void InstancesDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;

            e.InputParameters["organizationId"] = UserContext.SelectedOrganizationId;
        }

        protected void EntityDataSource_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;

            e.InputParameters["organizationId"] = UserContext.SelectedOrganizationId;
            e.InputParameters["active"] = true;
        }

        protected void ButtonUpdateOrder_Init(object sender, EventArgs e)
        {
            LinkButton link = sender as LinkButton;
            if (link != null)
                link.Text = Resources.RulesControl_ButtonUpdateOrder_Text;
        }

        protected void List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e == null) return;

            object obj = DataBinder.Eval(e.Row.DataItem, "LastUsedDate");
            if (!Support.IsNullOrDBNull(obj))
            {
                Literal lit = (Literal)e.Row.FindControl("LastUsedDateLiteral");
                lit.Text = Support.GetDisplayDateTime((DateTime)obj, m_UserContext.UtcOffset, m_UserContext.DateFormat, true);
            }

            obj = DataBinder.Eval(e.Row.DataItem, "CreatedDate");
            if (!Support.IsNullOrDBNull(obj))
            {
                Literal lit = (Literal)e.Row.FindControl("CreatedDateLiteral");
                lit.Text = Support.GetDisplayDateTime((DateTime)obj, m_UserContext.UtcOffset, m_UserContext.DateFormat, true);
            }
        }

        #endregion

        #region Overriden Methods

        protected override void LoadResources()
        {
            base.LoadResources();

            TemplateField tempField = (List.Columns[0] as TemplateField);
            if (tempField != null)
                tempField.HeaderText = Resources.RulesControl_List_OrderNumberColumn_HeaderText;

            HyperLinkField linkField = (List.Columns[1] as HyperLinkField);
            if (linkField != null)
            {
                linkField.HeaderText = Resources.RulesControl_List_InputParametersLinkColumns_HeaderText;
                linkField.Text = Resources.RulesControl_List_InputParametersLinkColumns_Text;
                linkField.DataNavigateUrlFormatString = WebApplication.CreateApplicationAbsoluteUrl(ResourceProvider.VirtualRootShortPath + "admin/ruleparameters.aspx?RuleId={0:N}");
                linkField.HeaderGroup = Resources.RulesControl_List_ParametersLinkColumns_HeaderGroup;
            }

            linkField = (List.Columns[2] as HyperLinkField);
            if (linkField != null)
            {
                linkField.HeaderText = Resources.RulesControl_List_OutputParametersLinkColumns_HeaderText;
                linkField.Text = Resources.RulesControl_List_OutputParametersLinkColumns_Text;
                linkField.DataNavigateUrlFormatString = WebApplication.CreateApplicationAbsoluteUrl((string)FormRuleEngine.DataKey["OutputEditPage"]);
                linkField.HeaderGroup = Resources.RulesControl_List_ParametersLinkColumns_HeaderGroup;
            }

            tempField = (List.Columns[6] as TemplateField);
            if (tempField != null)
                tempField.HeaderText = Resources.RulesControl_List_LastUsedUserColumn_HeaderText;

            List.Columns[7].HeaderText = Resources.RulesControl_List_LastUsedDateColumn_HeaderText;

            tempField = (List.Columns[8] as TemplateField);
            if (tempField != null)
                tempField.HeaderText = Resources.RulesControl_List_CreatedByColumn_HeaderText;

            List.Columns[9].HeaderText = Resources.RulesControl_List_CreatedDateColumn_HeaderText;

            Initialize(FormRuleEngine);
            FormRuleEngine.Visible = true;
            FormRuleEngine.AutoGenerateEditButton = FormRuleEngine.AutoGenerateInsertButton = false;

            LoadResources(FormRuleEngine, this.GetType().BaseType.Name);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!this.IsPostBack)
            {
                if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                {
                    SearchPanel.Visible = true;
                    InstancesDataSource.FilterExpression = InstanceProvider.InstancesFilterExpression;
                }
                else
                    SearchPanel.Visible = false;
            }

            m_UserContext = UserContext.Current;
        }

        #endregion
    }
}
