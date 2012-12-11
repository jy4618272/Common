using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.AdminControls
{
    public class EntityFieldsControl : BaseControl
    {
        #region Members

        protected Panel SearchPanel;
        protected ComboBox InstanceList;
        protected ObjectDataSource InstancesDataSource;

        private Guid? m_EntityId;
        private Entity m_Entity;
        private static string s_EntityFieldListsValuesPageUrlFormat;

        #endregion

        #region Private Properties

        private Guid EntityId
        {
            get
            {
                if (!m_EntityId.HasValue)
                {
                    object obj = Support.ConvertStringToType(this.Request.QueryString["entityid"], typeof(Guid));
                    m_EntityId = ((obj == null) ? Guid.Empty : (Guid)obj);
                }
                return m_EntityId.Value;
            }
        }

        private Entity Entity
        {
            get
            {
                if (m_Entity == null) m_Entity = WebApplication.Entities[this.EntityId.ToString()];
                return m_Entity;
            }
        }

        #endregion

        #region Protected Properties

        protected static string DropDownListImageUrl
        {
            get { return ResourceProvider.GetImageUrl(typeof(CommonGridView), "DropDownList.gif", true); }
        }

        protected static string DropDownListImageToolTip
        {
            get { return Resources.EntityFieldsControl_List_EditValuesButton_ToolTip; }
        }

        protected static string EntityFieldListsValuesPageUrlFormat
        {
            get
            {
                if (string.IsNullOrEmpty(s_EntityFieldListsValuesPageUrlFormat))
                {
                    Micajah.Common.Bll.Action action = ActionProvider.FindAction(ActionProvider.EntityFieldListsValuesPageActionId);
                    if (action != null) s_EntityFieldListsValuesPageUrlFormat = action.AbsoluteNavigateUrl + "?EntityFieldId={0:N}";
                }
                return s_EntityFieldListsValuesPageUrlFormat;
            }
        }

        #endregion

        #region Private Methods

        private void SwitchDataTypeFields(EntityFieldType fieldType, EntityFieldDataType dataType)
        {
            EditForm.Fields[4].Visible = (fieldType == EntityFieldType.Value);
            if (dataType == EntityFieldDataType.NotSet)
            {
                EditForm.Fields[7].Visible = true;
                EditForm.Fields[7].HeaderText = Resources.EntityFieldsControl_EditForm_MaxLengthField_HeaderText;
                EditForm.Fields[8].Visible = true;
                EditForm.Fields[9].Visible = true;
                EditForm.Fields[10].Visible = true;
            }
            else
            {
                EditForm.Fields[7].Visible = ((dataType == EntityFieldDataType.Text) || (dataType == EntityFieldDataType.Numeric));
                if (dataType == EntityFieldDataType.Text)
                    EditForm.Fields[7].HeaderText = Resources.EntityFieldsControl_EditForm_MaxLengthField_HeaderText;
                else if (dataType == EntityFieldDataType.Numeric)
                    EditForm.Fields[7].HeaderText = Resources.EntityFieldsControl_EditForm_LengthField_HeaderText;
                EditForm.Fields[8].Visible = (dataType == EntityFieldDataType.Numeric);
                EditForm.Fields[9].Visible = EditForm.Fields[10].Visible = ((dataType == EntityFieldDataType.DateTime) || (dataType == EntityFieldDataType.Numeric));
            }
        }

        private void SwitchDataTypeRows(EntityFieldDataType dataType)
        {
            EditForm.Rows[7].Style[HtmlTextWriterStyle.Display] = (((dataType == EntityFieldDataType.Text) || (dataType == EntityFieldDataType.Numeric)) ? string.Empty : "none");
            if (dataType == EntityFieldDataType.Text)
                EditForm.Rows[7].Cells[0].Text = Resources.EntityFieldsControl_EditForm_MaxLengthField_HeaderText;
            else if (dataType == EntityFieldDataType.Numeric)
                EditForm.Rows[7].Cells[0].Text = Resources.EntityFieldsControl_EditForm_LengthField_HeaderText;
            EditForm.Rows[8].Style[HtmlTextWriterStyle.Display] = ((dataType == EntityFieldDataType.Numeric) ? string.Empty : "none");
            bool display = ((dataType == EntityFieldDataType.DateTime) || (dataType == EntityFieldDataType.Numeric));
            EditForm.Rows[9].Style[HtmlTextWriterStyle.Display] = (display ? string.Empty : "none");
            EditForm.Rows[10].Style[HtmlTextWriterStyle.Display] = (display ? string.Empty : "none");
            if (display)
                this.ConfigureRangeValidators(dataType, EditForm.Rows[8].Cells[1].Controls[0] as TextBox);
        }

        private void SetDefaultValueTextBoxRequired(bool required)
        {
            TextBox textBox = EditForm.Rows[4].Cells[1].Controls[0] as TextBox;
            if (textBox != null)
                textBox.Required = required;
        }

        private void ConfigureRangeValidators(EntityFieldDataType dataType, TextBox decimalDigitsTextBox)
        {
            TextBox defaultValueTextBox = EditForm.Rows[4].Cells[1].Controls[0] as TextBox;
            TextBox minValueTextBox = EditForm.Rows[9].Cells[1].Controls[0] as TextBox;
            TextBox maxValueTextBox = EditForm.Rows[10].Cells[1].Controls[0] as TextBox;

            int decimalDigits = 0;
            if (decimalDigitsTextBox != null)
            {
                if (int.TryParse(decimalDigitsTextBox.Text, out decimalDigits) && (decimalDigits < 0))
                    decimalDigits = 0;
            }

            ConfigureRangeValidators(dataType, decimalDigits, defaultValueTextBox, minValueTextBox, maxValueTextBox);
        }

        private static void ConfigureRangeValidators(EntityFieldDataType dataType, int decimalDigits
            , ITextBox defaultValueTextBox, ITextBox minValueTextBox, ITextBox maxValueTextBox)
        {
            if (dataType == EntityFieldDataType.DateTime)
            {
                defaultValueTextBox.ValidationType
                    = minValueTextBox.ValidationType
                    = maxValueTextBox.ValidationType
                    = CustomValidationDataType.Date;
            }
            else if (dataType == EntityFieldDataType.Numeric)
            {
                defaultValueTextBox.ValidationType
                    = minValueTextBox.ValidationType
                    = maxValueTextBox.ValidationType
                    = ((decimalDigits > 0) ? CustomValidationDataType.Double : CustomValidationDataType.Integer);
            }
        }

        #endregion

        #region Protected Methods

        protected void EntityListDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
            {
                e.InputParameters["entityId"] = this.EntityId;
                e.InputParameters["organizationId"] = UserContext.Current.SelectedOrganizationId;
            }
        }

        protected void InstancesDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["organizationId"] = UserContext.Current.SelectedOrganizationId;
        }

        protected void EntityDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e == null) return;

            OrganizationDataSet.EntityFieldDataTable table = e.ReturnValue as OrganizationDataSet.EntityFieldDataTable;
            if (table != null)
            {
                if (table.Count > 0)
                {
                    OrganizationDataSet.EntityFieldRow row = table[0];
                    EntityFieldType fieldType = (EntityFieldType)row.EntityFieldTypeId;
                    SwitchDataTypeFields(fieldType, (EntityFieldDataType)row.DataTypeId);
                    (EditForm.Fields[4] as TextBoxField).Required = (!row.AllowDBNull);
                    if (fieldType != EntityFieldType.Value)
                        (EditForm.Fields[5] as CheckBoxField).AutoPostBack = false;
                    ConfigureRangeValidators((EntityFieldDataType)row.DataTypeId, row.DecimalDigits
                        , (EditForm.Fields[4] as TextBoxField), (EditForm.Fields[9] as TextBoxField), (EditForm.Fields[10] as TextBoxField));
                }
            }
        }

        protected void EntityFieldTypeList_ControlInit(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                comboBox.AutoPostBack = true;
                comboBox.CausesValidation = false;
                comboBox.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(EntityFieldTypeList_SelectedIndexChanged);
            }
        }

        protected void EntityFieldTypeList_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                if (comboBox.SelectedValue != "1")
                    SetDefaultValueTextBoxRequired(false);
                EditForm.Rows[4].Style[HtmlTextWriterStyle.Display] = ((comboBox.SelectedValue == "1") ? string.Empty : "none");
            }
        }

        protected void InstanceList_DataBound(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                UserContext user = UserContext.Current;
                if (user.IsOrganizationAdministrator)
                {
                    using (RadComboBoxItem item = new RadComboBoxItem(Resources.EntityFieldsControl_EditForm_InstanceList_AllInstancesItem_Text, string.Empty))
                    {
                        comboBox.Items.Insert(0, item);
                    }
                }
                else if (comboBox.Items.Count == 1)
                    SearchPanel.Visible = comboBox.Visible = false;
            }
        }

        protected void DataTypeList_ControlInit(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                comboBox.AutoPostBack = true;
                comboBox.CausesValidation = false;
                comboBox.SelectedIndexChanged += new RadComboBoxSelectedIndexChangedEventHandler(DataTypeList_SelectedIndexChanged);
            }
        }

        protected void DataTypeList_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
                SwitchDataTypeRows((EntityFieldDataType)Enum.Parse(typeof(EntityFieldDataType), comboBox.SelectedValue));
        }

        protected void AllowDBNullCheckBox_ControlInit(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                checkBox.CausesValidation = false;
                checkBox.CheckedChanged += new EventHandler(AllowDBNullCheckBox_CheckedChanged);
            }
        }

        protected void AllowDBNullCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                if (checkBox.AutoPostBack)
                    SetDefaultValueTextBoxRequired(!checkBox.Checked);
            }
        }

        protected void DecimalDigitsTextBox_ControlInit(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.CausesValidation = false;
                textBox.TextChanged += new EventHandler(DecimalDigitsTextBox_TextChanged);
            }
        }

        protected void DecimalDigitsTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox.AutoPostBack)
                    this.ConfigureRangeValidators(EntityFieldDataType.Numeric, textBox);
            }
        }

        #endregion

        #region Overriden Methods

        protected override void EditFormReset()
        {
            base.EditFormReset();
            SearchPanel.Visible = FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances;
        }

        protected override void LoadResources()
        {
            base.LoadResources();
            List.Columns[3].HeaderText = Resources.EntityFieldsControl_List_DataTypeIdColumn_HeaderText;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.IsPostBack)
            {
                if (this.Entity != null)
                {
                    this.MasterPage.CustomName = this.Entity.Name;
                    this.MasterPage.CustomNavigateUrl = this.Request.Url.PathAndQuery;
                }
                else
                    List.ShowAddLink = false;

                if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                {
                    SearchPanel.Visible = true;
                    InstancesDataSource.FilterExpression = InstanceProvider.InstancesFilterExpression;
                }
                else
                    SearchPanel.Visible = false;
            }
        }

        protected override void List_Action(object sender, CommonGridViewActionEventArgs e)
        {
            base.List_Action(sender, e);

            if (e == null) return;

            switch (e.Action)
            {
                case CommandActions.Add:
                    (EditForm.Fields[0] as ComboBoxField).Enabled = true;
                    (EditForm.Fields[3] as ComboBoxField).Enabled = true;
                    (EditForm.Fields[4] as TextBoxField).Required = false;
                    (EditForm.Fields[5] as CheckBoxField).AutoPostBack = true;
                    (EditForm.Fields[7] as TextBoxField).Enabled = true;
                    (EditForm.Fields[8] as TextBoxField).Enabled = true;
                    SwitchDataTypeFields(EntityFieldType.Value, EntityFieldDataType.NotSet);
                    EditForm.DataBind();
                    SwitchDataTypeRows(EntityFieldDataType.Text);
                    SearchPanel.Visible = false;
                    break;
                case CommandActions.Edit:
                    (EditForm.Fields[0] as ComboBoxField).Enabled = false;
                    (EditForm.Fields[3] as ComboBoxField).Enabled = false;
                    (EditForm.Fields[7] as TextBoxField).Enabled = false;
                    (EditForm.Fields[8] as TextBoxField).Enabled = false;
                    SearchPanel.Visible = false;
                    break;
            }
        }

        #endregion
    }
}
