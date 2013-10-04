using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Pages;
using Micajah.Common.Security;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    public class CustomField : Control, INamingContainer, IThemeable
    {
        #region Members

        private TextBox m_TextBox;
        private CheckBox m_CheckBox;
        private DatePicker m_DatePicker;
        private ComboBox m_ComboBox;
        private CheckBoxList m_CheckBoxList;
        private Control m_Control;

        private Entity m_Entity;
        private EntityField m_EntityField;

        #endregion

        #region Public Properties

        [DefaultValue(typeof(Guid), "00000000-0000-0000-0000-000000000000")]
        public Guid EntityFieldId
        {
            get
            {
                object obj = this.ViewState["EntityFieldId"];
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
            set
            {
                if (value == Guid.Empty)
                    this.ViewState.Remove("EntityFieldId");
                else
                    this.ViewState["EntityFieldId"] = value;
                m_EntityField = null;
            }
        }

        [DefaultValue(typeof(Guid), "00000000-0000-0000-0000-000000000000")]
        public Guid EntityId
        {
            get
            {
                object obj = this.ViewState["EntityId"];
                if ((obj == null) && (!this.DesignMode))
                {
                    ClientDataSet.EntityFieldDataTable table = EntityFieldProvider.GetEntityField(this.EntityFieldId, this.OrganizationId);
                    if (table.Count > 0)
                        obj = table[0].EntityId;
                }
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
            set
            {
                if (value == Guid.Empty)
                    this.ViewState.Remove("EntityId");
                else
                    this.ViewState["EntityId"] = value;
                m_Entity = null;
            }
        }

        [DefaultValue(typeof(Guid), "00000000-0000-0000-0000-000000000000")]
        public Guid OrganizationId
        {
            get
            {
                object obj = this.ViewState["OrganizationId"];
                if (obj == null)
                    obj = UserContext.Current.OrganizationId;
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
            set { this.ViewState["OrganizationId"] = value; }
        }

        [DefaultValue(typeof(Guid?), "")]
        public Guid? InstanceId
        {
            get
            {
                object obj = this.ViewState["InstanceId"];
                return ((obj == null) ? null : (Guid?)obj);
            }
            set { this.ViewState["InstanceId"] = value; }
        }

        [DefaultValue("")]
        public string LocalEntityId
        {
            get
            {
                object obj = this.ViewState["LocalEntityId"];
                return ((obj == null) ? null : (string)obj);
            }
            set { this.ViewState["LocalEntityId"] = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public Entity Entity
        {
            get
            {
                if (m_Entity == null)
                    m_Entity = EntityFieldProvider.Entities[this.EntityId.ToString()];
                return m_Entity;
            }
            set
            {
                this.EntityId = ((value == null) ? Guid.Empty : value.Id);
                m_Entity = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public EntityField EntityField
        {
            get
            {
                if (m_EntityField == null)
                {
                    if (this.Entity != null)
                    {
                        m_Entity.LoadCustomFields(this.OrganizationId, this.InstanceId, this.LocalEntityId);
                        m_EntityField = m_Entity.CustomFields[this.EntityFieldId.ToString()];
                    }
                }
                return m_EntityField;
            }
            set
            {
                this.EntityFieldId = ((value == null) ? Guid.Empty : value.Id);
                m_EntityField = value;
            }
        }

        /// <summary>
        /// Gets or sets the group of controls for which this control causes validation when it posts back to the server.
        /// </summary>
        [Category("Behavior")]
        [Description("The group of controls for which this control causes validation when it posts back to the server.")]
        [DefaultValue("")]
        public virtual string ValidationGroup
        {
            get
            {
                EnsureChildControls();
                return m_TextBox.ValidationGroup;
            }
            set
            {
                EnsureChildControls();
                m_TextBox.ValidationGroup
                    = m_CheckBox.ValidationGroup
                    = m_CheckBoxList.ValidationGroup
                    = m_ComboBox.ValidationGroup
                    = m_DatePicker.ValidationGroup
                    = value;
            }
        }

        public MasterPageTheme Theme
        {
            get
            {
                object obj = ViewState["Theme"];
                return ((obj == null) ? FrameworkConfiguration.Current.WebApplication.MasterPage.Theme : (MasterPageTheme)obj);
            }
            set { ViewState["Theme"] = value; }
        }

        #endregion

        #region Private Methods

        private void ConfigureValueControl()
        {
            switch (m_EntityField.EntityFieldDataType)
            {
                case EntityFieldDataType.Text:
                case EntityFieldDataType.Numeric:
                    m_TextBox.Required = (!m_EntityField.AllowDBNull);
                    switch (m_EntityField.EntityFieldDataType)
                    {
                        case EntityFieldDataType.Numeric:
                            m_TextBox.ValidationType = ((m_EntityField.DecimalDigits > 0) ? CustomValidationDataType.Double : CustomValidationDataType.Integer);
                            if (m_EntityField.MinValue != null) m_TextBox.MinimumValue = Convert.ToString(m_EntityField.MinValue, CultureInfo.CurrentCulture);
                            if (m_EntityField.MaxValue != null) m_TextBox.MaximumValue = Convert.ToString(m_EntityField.MaxValue, CultureInfo.CurrentCulture);
                            break;
                        case EntityFieldDataType.Text:
                            m_TextBox.MaxLength = m_EntityField.MaxLength;
                            break;
                    }

                    this.ResetTextBox();
                    break;
                case EntityFieldDataType.YesNo:
                    m_CheckBox.Required = (!m_EntityField.AllowDBNull);

                    this.ResetCheckBox();
                    break;
                case EntityFieldDataType.DateTime:
                    m_DatePicker.Required = (!m_EntityField.AllowDBNull);
                    if (m_EntityField.MinValue != null) m_DatePicker.MinDate = (DateTime)m_EntityField.MinValue;
                    if (m_EntityField.MaxValue != null) m_DatePicker.MaxDate = (DateTime)m_EntityField.MaxValue;

                    this.ResetDatePicker();
                    break;
            }
        }

        private void ConfigureSingleSelectListControl()
        {
            m_ComboBox.Required = (!m_EntityField.AllowDBNull);
            bool loadDefaultValues = (m_EntityField.SelectedValues.Count == 0);
            object selectedValue = m_EntityField.SelectedValue;
            foreach (string name in m_EntityField.ListValues.Keys)
            {
                object value = m_EntityField.ListValues[name][0];
                string valueStr = Convert.ToString(value, CultureInfo.CurrentCulture);
                if (m_ComboBox.Items.FindItemByValue(valueStr) == null)
                    m_ComboBox.Items.Add(new RadComboBoxItem(name, valueStr));
                if ((loadDefaultValues && (bool)m_EntityField.ListValues[name][1]) || (m_EntityField.SelectedValues.Contains(value)))
                    selectedValue = value;
            }
            if (selectedValue != null)
                m_ComboBox.SelectedValue = Convert.ToString(selectedValue, CultureInfo.CurrentCulture);
        }

        private void ConfigureMultipleSelectListControl()
        {
            m_CheckBoxList.Required = (!m_EntityField.AllowDBNull);
            bool loadDefaultValues = (m_EntityField.SelectedValues.Count == 0);
            foreach (string name in m_EntityField.ListValues.Keys)
            {
                object value = m_EntityField.ListValues[name][0];
                string valueStr = Convert.ToString(value, CultureInfo.CurrentCulture);
                if (m_CheckBoxList.Items.FindByValue(valueStr) == null)
                {
                    ListItem listItem = new ListItem(name, valueStr);
                    m_CheckBoxList.Items.Add(listItem);
                    if ((loadDefaultValues && (bool)m_EntityField.ListValues[name][1]) || (m_EntityField.SelectedValues.Contains(value)))
                        listItem.Selected = true;
                }
            }
        }

        private void ConfigureControl()
        {
            switch (m_EntityField.EntityFieldType)
            {
                case EntityFieldType.Value:
                    this.ConfigureValueControl();
                    break;
                case EntityFieldType.SingleSelectList:
                    this.ConfigureSingleSelectListControl();
                    break;
                case EntityFieldType.MultipleSelectList:
                    this.ConfigureMultipleSelectListControl();
                    break;
            }
        }

        private void EnsureControl()
        {
            if (m_Control != null) return;

            switch (m_EntityField.EntityFieldType)
            {
                case EntityFieldType.Value:
                    switch (m_EntityField.EntityFieldDataType)
                    {
                        case EntityFieldDataType.Text:
                        case EntityFieldDataType.Numeric:
                            m_Control = m_TextBox;
                            break;
                        case EntityFieldDataType.YesNo:
                            m_Control = m_CheckBox;
                            break;
                        case EntityFieldDataType.DateTime:
                            m_Control = m_DatePicker;
                            break;
                    }
                    break;
                case EntityFieldType.SingleSelectList:
                    m_Control = m_ComboBox;
                    break;
                case EntityFieldType.MultipleSelectList:
                    m_Control = m_CheckBoxList;
                    break;
            }
        }

        private void ResetTextBox()
        {
            bool valuesIsEmpty = (m_EntityField.SelectedValues.Count == 0);
            object value = (valuesIsEmpty ? m_EntityField.DefaultValue : m_EntityField.Value);
            m_TextBox.Text = ((value == null) ? null : Convert.ToString(value, CultureInfo.CurrentCulture));
        }

        private void ResetCheckBox()
        {
            if (m_EntityField.Value != null)
                m_CheckBox.Checked = (bool)m_EntityField.Value;
        }

        private void ResetDatePicker()
        {
            bool valuesIsEmpty = (m_EntityField.SelectedValues.Count == 0);
            object value = (valuesIsEmpty ? m_EntityField.DefaultValue : m_EntityField.Value);
            if (value == null)
                m_DatePicker.Clear();
            else
                m_DatePicker.SelectedDate = (DateTime)value;
        }

        private void ResetValueControl()
        {
            switch (m_EntityField.EntityFieldDataType)
            {
                case EntityFieldDataType.Text:
                case EntityFieldDataType.Numeric:
                    this.ResetTextBox();
                    break;
                case EntityFieldDataType.YesNo:
                    this.ResetCheckBox();
                    break;
                case EntityFieldDataType.DateTime:
                    this.ResetDatePicker();
                    break;
            }
        }

        private void ResetSingleSelectListControl()
        {
            bool valuesIsEmpty = (m_EntityField.SelectedValues.Count == 0);
            object selectedValue = m_EntityField.SelectedValue;
            foreach (string name in m_EntityField.ListValues.Keys)
            {
                object value = m_EntityField.ListValues[name][0];
                if ((valuesIsEmpty && (bool)m_EntityField.ListValues[name][1]) || (m_EntityField.SelectedValues.Contains(value)))
                    selectedValue = value;
            }
            if (selectedValue != null)
                m_ComboBox.SelectedValue = Convert.ToString(selectedValue, CultureInfo.CurrentCulture);
        }

        private void ResetMultipleSelectListControl()
        {
            m_CheckBoxList.ClearSelection();
            bool valuesIsEmpty = (m_EntityField.SelectedValues.Count == 0);
            foreach (string name in m_EntityField.ListValues.Keys)
            {
                object value = m_EntityField.ListValues[name][0];
                ListItem listItem = m_CheckBoxList.Items.FindByValue(Convert.ToString(value, CultureInfo.CurrentCulture));
                if (listItem != null)
                {
                    if ((valuesIsEmpty && (bool)m_EntityField.ListValues[name][1]) || (m_EntityField.SelectedValues.Contains(value)))
                        listItem.Selected = true;
                }
            }
        }

        private void ResetControl()
        {
            this.EnsureControl();
            switch (m_EntityField.EntityFieldType)
            {
                case EntityFieldType.Value:
                    this.ResetValueControl();
                    break;
                case EntityFieldType.SingleSelectList:
                    this.ResetSingleSelectListControl();
                    break;
                case EntityFieldType.MultipleSelectList:
                    this.ResetMultipleSelectListControl();
                    break;
            }
        }

        #endregion

        #region Internal Methods

        internal void ExtractControlValue()
        {
            if (this.EntityField == null) return;

            switch (m_EntityField.EntityFieldType)
            {
                case EntityFieldType.Value:
                    switch (m_EntityField.EntityFieldDataType)
                    {
                        case EntityFieldDataType.Text:
                        case EntityFieldDataType.Numeric:
                            m_EntityField.Value = m_TextBox.Text;
                            break;
                        case EntityFieldDataType.YesNo:
                            m_EntityField.Value = m_CheckBox.Checked;
                            break;
                        case EntityFieldDataType.DateTime:
                            if (m_DatePicker.IsEmpty)
                                m_EntityField.Value = null;
                            else
                                m_EntityField.Value = m_DatePicker.SelectedDate;
                            break;
                    }
                    break;
                case EntityFieldType.SingleSelectList:
                    m_EntityField.Value = m_ComboBox.SelectedValue;
                    break;
                case EntityFieldType.MultipleSelectList:
                    m_EntityField.ClearSelectedValues();
                    foreach (ListItem listItem in m_CheckBoxList.Items)
                    {
                        if (listItem.Selected)
                            m_EntityField.AddSelectedValue(listItem.Value);
                    }
                    break;
            }
        }

        #endregion

        #region Overriden Methods

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            m_TextBox = new TextBox();
            m_TextBox.ID = "txt";
            this.Controls.Add(m_TextBox);

            m_CheckBox = new CheckBox();
            m_CheckBox.ID = "chk";
            this.Controls.Add(m_CheckBox);

            m_DatePicker = new DatePicker();
            m_DatePicker.ID = "dp";
            this.Controls.Add(m_DatePicker);

            m_ComboBox = new ComboBox();
            m_ComboBox.ID = "cmb";
            this.Controls.Add(m_ComboBox);

            m_CheckBoxList = new CheckBoxList();
            m_CheckBoxList.ID = "cbl";
            this.Controls.Add(m_CheckBoxList);

            if (this.EntityField != null)
            {
                this.EnsureControl();
                this.ConfigureControl();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            m_TextBox.Theme = m_CheckBox.Theme = m_DatePicker.Theme = m_ComboBox.Theme = m_CheckBoxList.Theme = this.Theme;
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (writer == null) return;

            if (this.DesignMode)
                writer.Write(this.ID);
            else if (m_Control != null)
                m_Control.RenderControl(writer);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Commits all the changes since the last time AcceptChanges was called.
        /// </summary>
        /// <returns>true, if the changes are commited successfully; otherwise, false.</returns>
        public bool AcceptChanges()
        {
            if (this.EntityField == null) return false;

            this.ExtractControlValue();
            EntityFieldProvider.SaveEntityCustomField(m_EntityField, this.OrganizationId, this.LocalEntityId);

            return true;
        }

        /// <summary>
        /// Rolls back all changes that have been made to the control since it was loaded, or the last time AcceptChanges was called.
        /// </summary>
        /// <returns>true, if the changes are rolled back successfully; otherwise, false.</returns>
        public bool RejectChanges()
        {
            if (this.EntityField == null) return false;

            this.ResetControl();

            return true;
        }

        #endregion
    }
}
