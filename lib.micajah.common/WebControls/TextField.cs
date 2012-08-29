using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;

namespace Micajah.Common.WebControls
{
    public class TextField : BaseValidatedField, IDisposable
    {
        #region Members

        TextBox m_TextBox;
        DatePicker m_DatePicker;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public TextField()
        {
            m_TextBox = new TextBox();
            m_DatePicker = new DatePicker();
        }

        #endregion

        #region Public Properties

        [DefaultValue(TextFieldType.Text)]
        public TextFieldType TextType
        {
            get
            {
                object obj = base.ViewState["TextType"];
                return (obj == null) ? TextFieldType.Text : (TextFieldType)obj;
            }
            set { base.ViewState["TextType"] = value; }
        }

        [DefaultValue(0)]
        public int Columns
        {
            get
            {
                int value = 0;
                switch (this.TextType)
                {
                    case TextFieldType.Date:
                    case TextFieldType.DateTime:
                    case TextFieldType.Time:
                    case TextFieldType.DatePicker:
                    case TextFieldType.DateTimePicker:
                    case TextFieldType.TimePicker:
                        //value = m_DatePicker.Columns;
                        break;
                    default:
                        value = m_TextBox.Columns;
                        break;
                }
                return value;
            }
            set
            {
                m_TextBox.Columns = value;
                //m_DatePicker.Columns = value;
            }
        }

        [DefaultValue(0)]
        public int Rows
        {
            get { return m_TextBox.Rows; }
            set { m_TextBox.Rows = value; }
        }

        [DefaultValue(0)]
        public int MaxLength
        {
            get { return m_TextBox.MaxLength; }
            set { m_TextBox.MaxLength = value; }
        }

        [DefaultValue(false)]
        public bool LengthInfo
        {
            get { return m_TextBox.LengthInfo; }
            set { m_TextBox.LengthInfo = value; }
        }

        #endregion

        #region Private Methods

        private void ChangeTextType()
        {
            switch (this.TextType)
            {
                case TextFieldType.Text:
                    break;
                case TextFieldType.Integer:
                    m_TextBox.ValidationType = CustomValidationDataType.Integer;
                    break;
                case TextFieldType.Double:
                    m_TextBox.ValidationType = CustomValidationDataType.Double;
                    break;
                case TextFieldType.Currency:
                    m_TextBox.ValidationType = CustomValidationDataType.Currency;
                    break;
                case TextFieldType.Date:
                    m_DatePicker.Type = DatePickerType.Date;
                    break;
                case TextFieldType.DateTime:
                    m_DatePicker.Type = DatePickerType.DateTime;
                    break;
                case TextFieldType.Time:
                    m_DatePicker.Type = DatePickerType.Time;
                    break;
                case TextFieldType.DatePicker:
                    m_DatePicker.Type = DatePickerType.DatePicker;
                    break;
                case TextFieldType.DateTimePicker:
                    m_DatePicker.Type = DatePickerType.DateTimePicker;
                    break;
                case TextFieldType.TimePicker:
                    m_DatePicker.Type = DatePickerType.TimePicker;
                    break;
                case TextFieldType.Email:
                    m_TextBox.ValidationType = CustomValidationDataType.RegularExpression;
                    m_TextBox.ValidationExpression = Support.EmailRegularExpression;
                    m_TextBox.MaxLength = 0;
                    break;
                case TextFieldType.PhoneNumber:
                    m_TextBox.ValidationType = CustomValidationDataType.RegularExpression;
                    m_TextBox.ValidationExpression = @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}";
                    break;
                case TextFieldType.ZipCode:
                    m_TextBox.ValidationType = CustomValidationDataType.RegularExpression;
                    m_TextBox.ValidationExpression = @"\d{5}(-\d{4})?";
                    break;
                case TextFieldType.InternetUrl:
                    m_TextBox.ValidationType = CustomValidationDataType.RegularExpression;
                    m_TextBox.ValidationExpression = Support.UrlRegularExpression;
                    m_TextBox.MaxLength = 0;
                    break;
                case TextFieldType.Password:
                    m_TextBox.TextMode = TextBoxMode.Password;
                    break;
                case TextFieldType.TextArea:
                    m_TextBox.TextMode = TextBoxMode.MultiLine;
                    break;
            }
        }

        private void OnBindingField(object sender, EventArgs e)
        {
            m_TextBox = sender as TextBox;
            if (m_TextBox != null)
                m_TextBox.Text = LookupStringValue(m_TextBox, true);
            else
            {
                m_DatePicker = sender as DatePicker;
                if (m_DatePicker != null)
                {
                    if ((!DesignMode) && (!this.InsertMode))
                    {
                        DateTime value = LookupDateTimeValue(m_DatePicker);
                        if (value > DateTime.MinValue) m_DatePicker.SelectedDate = value;
                    }
                }
                else
                {
                    TableCell cell = sender as TableCell;
                    if (cell != null) cell.Text = LookupStringValue(cell, true);
                }
            }
        }

        #endregion

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_TextBox != null) m_TextBox.Dispose();
                if (m_DatePicker != null) m_DatePicker.Dispose();
            }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Creates an empty Micajah.Common.WebControls.TextField object.
        /// </summary>
        /// <returns>An empty Micajah.Common.WebControls.TextField.</returns>
        protected override DataControlField CreateField()
        {
            return new TextField();
        }

        /// <summary>
        /// Copies the properties of the current Micajah.Common.WebControls.TextField object to the specified System.Web.UI.WebControls.DataControlField object.
        /// </summary>
        /// <param name="newField">The System.Web.UI.WebControls.DataControlField to copy the properties of the current Micajah.Common.WebControls.TextField to.</param>
        protected override void CopyProperties(DataControlField newField)
        {
            base.CopyProperties(newField);

            TextField field = newField as TextField;
            if (field != null)
            {
                field.TextType = this.TextType;
                field.Columns = this.Columns;
                field.Rows = this.Rows;
                field.MaxLength = this.MaxLength;
                field.LengthInfo = this.LengthInfo;
            }
        }

        protected override object ExtractControlValue(Control control)
        {
            object value = null;

            switch (this.TextType)
            {
                case TextFieldType.Date:
                case TextFieldType.DateTime:
                case TextFieldType.Time:
                case TextFieldType.DatePicker:
                case TextFieldType.DateTimePicker:
                case TextFieldType.TimePicker:
                    m_DatePicker = control as DatePicker;
                    if (m_DatePicker != null) value = m_DatePicker.SelectedDate;
                    break;
                default:
                    m_TextBox = control as TextBox;
                    if (m_TextBox != null) value = m_TextBox.Text;
                    break;
            }

            if (value != null)
            {
                if (this.ConvertEmptyStringToNull && string.IsNullOrEmpty(value as string)) value = null;
            }

            return value;
        }

        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            Control control = null;

            if (this.EditMode || this.InsertMode)
            {
                ChangeTextType();
                switch (this.TextType)
                {
                    case TextFieldType.Date:
                    case TextFieldType.DateTime:
                    case TextFieldType.Time:
                    case TextFieldType.DatePicker:
                    case TextFieldType.DateTimePicker:
                    case TextFieldType.TimePicker:
                        m_DatePicker.ReadOnly = ReadOnly;
                        control = m_DatePicker;
                        break;
                    default:
                        m_TextBox.ReadOnly = ReadOnly;
                        control = m_TextBox;
                        break;
                }

                control.Visible = Visible;

                IValidated ctl = control as IValidated;
                if (ctl != null)
                {
                    ctl.Enabled = Enabled;
                    ctl.ErrorMessage = ErrorMessage;
                    ctl.Required = Required;
                    ctl.ValidationGroup = ValidationGroup;
                    ctl.ValidatorInitialValue = ValidatorInitialValue;
                    ctl.TabIndex = TabIndex;
                }

                if (cell != null)
                    cell.Controls.Add(control);
            }
            else
                control = cell;

            if (control != null && Visible) control.DataBinding += new EventHandler(OnBindingField);
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
