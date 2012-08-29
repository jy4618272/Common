using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls
{
    public class TextBoxField : BaseValidatedField, ITextBox
    {
        #region Public Properies

        /// <summary>
        /// Gets or sets a value indicating whether the control state automatically posts back to the server when clicked.
        /// </summary>
        [Category("Behavior")]
        [Description("Automatically posts back to the server when the control is clicked.")]
        [DefaultValue(false)]
        public bool AutoPostBack
        {
            get
            {
                object obj = ViewState["AutoPostBack"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["AutoPostBack"] = value; }
        }

        /// <summary>
        /// Gets or sets the behavior mode (single-line, multiline, or password) of the text box.
        /// </summary>
        [Category("Behavior")]
        [Description("The behavior mode (single-line, multiline, or password) of the text box.")]
        [DefaultValue(TextBoxMode.SingleLine)]
        public TextBoxMode TextMode
        {
            get
            {
                object obj = base.ViewState["TextMode"];
                return (obj == null) ? TextBoxMode.SingleLine : (TextBoxMode)obj;
            }
            set { base.ViewState["TextMode"] = value; }
        }

        /// <summary>
        /// Gets or sets the display width of the text box in characters.
        /// </summary>
        [Category("Appearance")]
        [Description("The display width of the text box in characters.")]
        [DefaultValue(35)]
        public int Columns
        {
            get
            {
                object obj = base.ViewState["Columns"];
                return (obj == null) ? 35 : (int)obj;
            }
            set { base.ViewState["Columns"] = value; }
        }

        /// <summary>
        /// Gets or sets the number of rows displayed in a multiline text box.
        /// </summary>
        [Category("Behavior")]
        [Description("The number of rows displayed in a multiline text box.")]
        [DefaultValue(0)]
        public int Rows
        {
            get
            {
                object obj = base.ViewState["Rows"];
                return (obj == null) ? 0 : (int)obj;
            }
            set { base.ViewState["Rows"] = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters allowed in the text box.
        /// </summary>
        [Category("Behavior")]
        [Description("The maximum number of characters allowed in the text box.")]
        [DefaultValue(0)]
        public int MaxLength
        {
            get
            {
                object obj = base.ViewState["MaxLength"];
                return (obj == null) ? 0 : (int)obj;
            }
            set { base.ViewState["MaxLength"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating that the information about current characters length and max length of text box is shown.
        /// Only for MultiLine text mode.
        /// </summary>
        [Category("Behavior")]
        [Description("Whether the information about current characters length and max length of text box is shown.")]
        [DefaultValue(false)]
        public bool LengthInfo
        {
            get
            {
                object obj = ViewState["LengthInfo"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["LengthInfo"] = value; }
        }

        /// <summary>
        /// Gets or sets the input mask of the text box control.
        /// </summary>
        [Category("Behavior")]
        [Description("The input mask of the text box control.")]
        [DefaultValue("")]
        public string Mask
        {
            get
            {
                object obj = ViewState["Mask"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["Mask"] = value; }
        }

        /// <summary>
        /// Gets or sets the type indicating what text will be returned as value of masked text box.
        /// </summary>
        [Category("Behavior")]
        [Description("The type indicating what text will be returned as value of masked text box.")]
        [DefaultValue(MaskedTextType.Text)]
        public MaskedTextType TextType
        {
            get
            {
                object obj = ViewState["TextType"];
                return ((obj == null) ? MaskedTextType.Text : (MaskedTextType)obj);
            }
            set { ViewState["TextType"] = value; }
        }

        /// <summary>
        /// Gets or sets the data type of values.
        /// One of the Micajah.Common.WebControls.CustomValidationDataType enumeration values. The default value is String.
        /// </summary>
        [Category("Behavior")]
        [Description("Data type of values.")]
        [DefaultValue(CustomValidationDataType.String)]
        public CustomValidationDataType ValidationType
        {
            get
            {
                object obj = ViewState["ValidationType"];
                return ((obj == null) ? CustomValidationDataType.String : (CustomValidationDataType)obj);
            }
            set { ViewState["ValidationType"] = value; }
        }

        /// <summary>
        /// Gets or sets the regular expression that determines the pattern used to validate input value.
        /// To use regular expression validation ValidationType property must be set to CustomValidationDataType.RegularExpression.
        /// </summary>
        [Category("Behavior")]
        [Description("Regular expression to determine validity.")]
        [DefaultValue("")]
        [Editor(typeof(System.Web.UI.Design.WebControls.RegexTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ValidationExpression
        {
            get
            {
                object obj = ViewState["ValidationExpression"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["ValidationExpression"] = value; }
        }

        /// <summary>
        /// Gets or sets the text for the error message when validation of input value fails.
        /// The default value gets from resources.
        /// </summary>
        [Category("Behavior")]
        [Description("The text for the error message when validation of input value fails.")]
        [DefaultValue("")]
        public string ValidationErrorMessage
        {
            get
            {
                object obj = ViewState["ValidationErrorMessage"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["ValidationErrorMessage"] = value; }
        }

        /// <summary>
        /// Gets or sets the maximum value for the validation range. The default value is System.String.Empty.
        /// </summary>
        [Category("Behavior")]
        [Description("The maximum value for the validation range.")]
        [DefaultValue("")]
        public string MaximumValue
        {
            get
            {
                object obj = ViewState["MaximumValue"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["MaximumValue"] = value; }
        }

        /// <summary>
        /// Gets or sets the minimum value for the validation range. The default value is System.String.Empty.
        /// </summary>
        [Category("Behavior")]
        [Description("The minimum value for the validation range.")]
        [DefaultValue("")]
        public string MinimumValue
        {
            get
            {
                object obj = ViewState["MinimumValue"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["MinimumValue"] = value; }
        }

        /// <summary>
        /// Gets or sets a default text of the control which is displayed in insert mode.
        /// </summary>
        [Category("Behavior")]
        [Description("The default text of the control which is displayed in insert mode.")]
        [DefaultValue("")]
        public string DefaultText
        {
            get
            {
                object obj = ViewState["DefaultText"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["DefaultText"] = value; }
        }

        /// <summary>
        /// Gets or sets the value which the text box displays when it does not have focus.
        /// </summary>
        [Category("Appearance")]
        [Description("The value which the text box displays when it does not have focus.")]
        [DefaultValue("")]
        public string EmptyText
        {
            get
            {
                object obj = ViewState["EmptyText"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["EmptyText"] = value; }
        }

        #endregion

        #region Private Methods

        private void CopyProperties(TextBox control)
        {
            CopyProperties(this, control);

            control.Width = base.ControlStyle.Width;
            control.AutoPostBack = this.AutoPostBack;
            control.TextMode = this.TextMode;
            control.Columns = this.Columns;
            control.Rows = this.Rows;
            control.MaxLength = this.MaxLength;
            control.LengthInfo = this.LengthInfo;
            control.ReadOnly = this.ReadOnly;
            control.Mask = this.Mask;
            control.TextType = this.TextType;
            control.ValidationType = this.ValidationType;
            control.ValidationExpression = this.ValidationExpression;
            control.ValidationErrorMessage = this.ValidationErrorMessage;
            control.MaximumValue = this.MaximumValue;
            control.MinimumValue = this.MinimumValue;
            control.EmptyText = this.EmptyText;
        }

        private void OnBindingField(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (!this.InsertMode) textBox.Text = this.LookupStringValue(textBox, true);
            }
            else
            {
                TableCell cell = sender as TableCell;
                if (cell != null) cell.Text = this.LookupStringValue(cell, true);
            }
        }

        #endregion

        #region Overriden Methods

        protected override DataControlField CreateField()
        {
            return new TextBoxField();
        }

        protected override void CopyProperties(DataControlField newField)
        {
            base.CopyProperties(newField);

            TextBoxField field = newField as TextBoxField;
            if (field != null)
            {
                field.ControlStyle.Width = this.ControlStyle.Width;
                field.AutoPostBack = this.AutoPostBack;
                field.TextMode = this.TextMode;
                field.Columns = this.Columns;
                field.Rows = this.Rows;
                field.MaxLength = this.MaxLength;
                field.LengthInfo = this.LengthInfo;
                field.Mask = this.Mask;
                field.TextType = this.TextType;
                field.ValidationType = this.ValidationType;
                field.ValidationExpression = this.ValidationExpression;
                field.ValidationErrorMessage = this.ValidationErrorMessage;
                field.MaximumValue = this.MaximumValue;
                field.MinimumValue = this.MinimumValue;
                field.DefaultText = this.DefaultText;
                field.EmptyText = this.EmptyText;
            }
        }

        protected override object ExtractControlValue(Control control)
        {
            TextBox textBox = control as TextBox;
            return ((textBox == null) ? string.Empty : textBox.Text);
        }

        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            Control ctrl = null;

            if (this.EditMode || this.InsertMode)
            {
                using (TextBox textBox = new TextBox())
                {
                    CopyProperties(textBox);
                    textBox.Init += OnControlInit;
                    if (this.InsertMode) textBox.Text = DefaultText;
                    if (!base.Visible) textBox.Required = false;
                    if (cell != null)
                        cell.Controls.Add(textBox);

                    ctrl = textBox;
                }
            }
            else
            {
                ctrl = cell;
            }

            if (ctrl != null && base.Visible) ctrl.DataBinding += new EventHandler(this.OnBindingField);
        }

        #endregion
    }
}
