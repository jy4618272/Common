using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Provides a text-based control that allows the user to enter and validate text.
    /// </summary>
    [DefaultProperty("Text")]
    [ControlValueProperty("Text")]
    [DefaultEvent("TextChanged")]
    [ValidationProperty("Text")]
    // TODO: Implement custom validation instead of standard when Required and EmptyText properties are specified both.
    public class TextBox : BaseValidatedControl, ITextBox, IValidated, IEditableTextControl, ITextControl
    {
        #region Members

        private System.Web.UI.WebControls.TextBox m_TextBox;
        private RadMaskedTextBox m_RadMaskedTextBox;
        private RangeValidator m_RangeValidator;
        private RegularExpressionValidator m_RegularExpressionValidator;
        private HtmlGenericControl m_Span;
        private bool m_ValidationTypeChanged;
        private bool m_IsValid;

        #endregion

        #region Overriden Properties

        /// <summary>
        /// Gets the collection of arbitrary attributes (for rendering only) that do not correspond to properties on the control.
        /// </summary>
        public override System.Web.UI.AttributeCollection Attributes
        {
            get { return (Masked ? m_RadMaskedTextBox.Attributes : m_TextBox.Attributes); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is enabled.
        /// </summary>
        public override bool Enabled
        {
            get { return (Masked ? m_RadMaskedTextBox.Enabled : m_TextBox.Enabled); }
            set
            {
                base.Enabled = value;
                m_RadMaskedTextBox.Enabled = value;
                m_TextBox.Enabled = value;
                m_RangeValidator.Enabled = value;
                m_RegularExpressionValidator.Enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the tab index of the control.
        /// The default is 0, which indicates that this property is not set.
        /// </summary>
        public override short TabIndex
        {
            get { return (Masked ? m_RadMaskedTextBox.TabIndex : m_TextBox.TabIndex); }
            set
            {
                EnsureChildControls();
                m_RadMaskedTextBox.TabIndex = value;
                m_TextBox.TabIndex = value;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the control state automatically posts back to the server when clicked.
        /// </summary>
        [Category("Behavior")]
        [Description("Automatically posts back to the server when the control is clicked.")]
        [DefaultValue(false)]
        public bool AutoPostBack
        {
            get { return (Masked ? m_RadMaskedTextBox.AutoPostBack : m_TextBox.AutoPostBack); }
            set
            {
                EnsureChildControls();
                m_RadMaskedTextBox.AutoPostBack = value;
                m_TextBox.AutoPostBack = value;
            }
        }

        /// <summary>
        /// Gets or sets the Cascading Style Sheet (CSS) class rendered by the Web server control on the client.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("The Cascading Style Sheet (CSS) class rendered by the Web server control on the client.")]
        public string CssClass
        {
            get { return (Masked ? m_RadMaskedTextBox.CssClass : m_TextBox.CssClass); }
            set
            {
                EnsureChildControls();
                m_RadMaskedTextBox.CssClass = value;
                m_TextBox.CssClass = value;
            }
        }

        /// <summary>
        /// Gets or sets the unique, hierarchically qualified identifier of the control that causes postback when the ENTER key is pressed.
        /// </summary>
        [Category("Behavior")]
        [Description("The unique, hierarchically qualified identifier of the control that causes postback when the ENTER key is pressed.")]
        [DefaultValue("")]
        public string DefaultButtonUniqueId
        {
            get
            {
                object obj = ViewState["DefaultButtonUniqueId"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["DefaultButtonUniqueId"] = value; }
        }

        /// <summary>
        /// Gets or sets the value which the text box displays when it does not have focus.
        /// </summary>
        [Category("Appearance")]
        [Description("The value which the text box displays when it does not have focus.")]
        [DefaultValue("")]
        public string EmptyText
        {
            get { return (string)ViewState["EmptyText"]; }
            set { ViewState["EmptyText"] = value; }
        }

        /// <summary>
        /// Gets or sets the text content of this control.
        /// </summary>
        [Category("Appearance")]
        [Description("The text content of this control.")]
        [DefaultValue("")]
        public string Text
        {
            get
            {
                string value = null;
                if (Masked)
                {
                    switch (TextType)
                    {
                        case MaskedTextType.Text:
                            value = m_RadMaskedTextBox.Text;
                            break;
                        case MaskedTextType.TextWithLiterals:
                            value = m_RadMaskedTextBox.TextWithLiterals;
                            break;
                        case MaskedTextType.TextWithPrompt:
                            value = m_RadMaskedTextBox.TextWithPrompt;
                            break;
                        case MaskedTextType.TextWithPromptAndLiterals:
                            value = m_RadMaskedTextBox.TextWithPromptAndLiterals;
                            break;
                    }
                }
                else
                {
                    if (string.Compare(m_TextBox.Text, this.EmptyText, StringComparison.Ordinal) != 0)
                        value = m_TextBox.Text;
                }
                return (string.IsNullOrEmpty(value) ? string.Empty : value);
            }
            set
            {
                EnsureChildControls();
                m_RadMaskedTextBox.Text = value;
                m_TextBox.Text = value;
            }
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
                TextBoxMode mode = TextBoxMode.SingleLine;
                if (Masked)
                {
                    if (m_RadMaskedTextBox.TextMode == InputMode.MultiLine) mode = TextBoxMode.MultiLine;
                }
                else mode = m_TextBox.TextMode;
                return mode;
            }
            set
            {
                EnsureChildControls();
                switch (value)
                {
                    case TextBoxMode.MultiLine:
                        m_RadMaskedTextBox.TextMode = InputMode.MultiLine;
                        break;
                    case TextBoxMode.SingleLine:
                    case TextBoxMode.Password:
                    default:
                        m_RadMaskedTextBox.TextMode = InputMode.SingleLine;
                        break;
                }
                m_TextBox.TextMode = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether validation is performed when the control is selected.
        /// </summary>
        [Description("Whether validation is performed when the control is selected.")]
        [DefaultValue(false)]
        [Category("Behavior")]
        [Themeable(false)]
        public bool CausesValidation
        {
            get { return (Masked ? m_RadMaskedTextBox.CausesValidation : m_TextBox.CausesValidation); }
            set
            {
                EnsureChildControls();
                m_RadMaskedTextBox.CausesValidation = value;
                m_TextBox.CausesValidation = value;
            }
        }

        /// <summary>
        /// Gets the inner control.
        /// </summary>
        [Browsable(false)]
        public Control InnerControl
        {
            get { return (Masked ? (Control)m_RadMaskedTextBox : (Control)m_TextBox); }
        }

        /// <summary>
        /// Gets or sets the display width of the text box in characters.
        /// </summary>
        [Category("Appearance")]
        [Description("The display width of the text box in characters.")]
        [DefaultValue(0)]
        public int Columns
        {
            get { return (Masked ? m_RadMaskedTextBox.Columns : m_TextBox.Columns); }
            set
            {
                EnsureChildControls();
                m_RadMaskedTextBox.Columns = value;
                m_TextBox.Columns = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of rows displayed in a multiline text box.
        /// </summary>
        [Category("Behavior")]
        [Description("The number of rows displayed in a multiline text box.")]
        [DefaultValue(0)]
        public int Rows
        {
            get { return (Masked ? m_RadMaskedTextBox.Rows : m_TextBox.Rows); }
            set
            {
                EnsureChildControls();
                m_RadMaskedTextBox.Rows = value;
                m_TextBox.Rows = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters allowed in the text box.
        /// </summary>
        [Category("Behavior")]
        [Description("The maximum number of characters allowed in the text box.")]
        [DefaultValue(0)]
        public int MaxLength
        {
            get { return (Masked ? 0 : m_TextBox.MaxLength); }
            set
            {
                EnsureChildControls();
                m_TextBox.MaxLength = value;
            }
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
        /// Gets or sets a value indicating whether the contents of the text box can be changed.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return (Masked ? m_RadMaskedTextBox.ReadOnly : m_TextBox.ReadOnly); }
            set
            {
                EnsureChildControls();
                m_RadMaskedTextBox.ReadOnly = value;
                m_TextBox.ReadOnly = value;
            }
        }

        /// <summary>
        /// Gets or sets the input mask of the text box control.
        /// </summary>
        [Category("Behavior")]
        [Description("The input mask of the text box control.")]
        public string Mask
        {
            get { return (Masked ? m_RadMaskedTextBox.Mask : string.Empty); }
            set
            {
                EnsureChildControls();
                m_RadMaskedTextBox.Mask = value;
            }
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
        /// Gets or sets the group of controls for which this control causes validation when it posts back to the server.
        /// </summary>
        public override string ValidationGroup
        {
            get { return base.ValidationGroup; }
            set
            {
                base.ValidationGroup = value;
                m_TextBox.ValidationGroup = value;
                m_RangeValidator.ValidationGroup = value;
                m_RegularExpressionValidator.ValidationGroup = value;
            }
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
            set
            {
                object obj = ViewState["ValidationType"];
                if (obj != null)
                    m_ValidationTypeChanged = (value != (CustomValidationDataType)obj);
                ViewState["ValidationType"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the regular expression that determines the pattern used to validate input value.
        /// To use regular expression validation ValidationType property must be set to CustomValidationDataType.RegularExpression.
        /// </summary>
        [Category("Behavior")]
        [Description("Regular expression to determine validity.")]
        [DefaultValue("")]
        [Editor(typeof(System.Web.UI.Design.WebControls.RegexTypeEditor), typeof(UITypeEditor))]
        public string ValidationExpression
        {
            get
            {
                EnsureChildControls();
                return m_RegularExpressionValidator.ValidationExpression;
            }
            set
            {
                EnsureChildControls();
                m_RegularExpressionValidator.ValidationExpression = value;
            }
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
                EnsureChildControls();
                string msg = string.Empty;
                if (RangeValidation)
                    msg = m_RangeValidator.ErrorMessage;
                else if (RegularExpressionValidation)
                    msg = m_RegularExpressionValidator.ErrorMessage;
                return msg;
            }
            set
            {
                EnsureChildControls();
                m_RangeValidator.ErrorMessage = value;
                m_RegularExpressionValidator.ErrorMessage = value;
            }
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
                EnsureChildControls();
                return m_RangeValidator.MaximumValue;
            }
            set
            {
                EnsureChildControls();
                m_RangeValidator.MaximumValue = value;
            }
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
                EnsureChildControls();
                return m_RangeValidator.MinimumValue;
            }
            set
            {
                EnsureChildControls();
                m_RangeValidator.MinimumValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the control.
        /// </summary>
        [DefaultValue(typeof(Unit), "")]
        [Description("The width of the control.")]
        [Category("Layout")]
        public Unit Width
        {
            get
            {
                object obj = ViewState["Width"];
                return ((obj == null) ? Unit.Empty : (Unit)obj);
            }
            set { ViewState["Width"] = value; }
        }

        /// <summary>
        /// Gets or sets the text displayed when the mouse pointer hovers over the control.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue("")]
        [Description("The text displayed when the mouse pointer hovers over the control.")]
        [Localizable(true)]
        public string ToolTip
        {
            get { return (Masked ? m_RadMaskedTextBox.ToolTip : m_TextBox.ToolTip); }
            set
            {
                EnsureChildControls();
                m_RadMaskedTextBox.ToolTip = value;
                m_TextBox.ToolTip = value;
            }
        }

        public bool IsValid
        {
            get { return m_IsValid; }
            set
            {
                m_IsValid = value;
                if (this.Theme == MasterPageTheme.Modern)
                {
                    this.CssClass = this.CssClass.Replace(" Invalid", string.Empty);
                    if (value)
                        this.Attributes.Remove("validatorId");
                    else
                        this.CssClass += " Invalid";
                }
            }
        }

        #endregion

        #region Private Properties

        private bool Masked
        {
            get
            {
                EnsureChildControls();
                return (!string.IsNullOrEmpty(m_RadMaskedTextBox.Mask));
            }
        }

        private bool RangeValidation
        {
            get { return (ValidationType != CustomValidationDataType.RegularExpression); }
        }

        private bool RegularExpressionValidation
        {
            get { return (ValidationType == CustomValidationDataType.RegularExpression); }
        }

        private bool RangeValidationEnabled
        {
            get { return (RangeValidation && Enabled && (!ReadOnly) && (ValidationType != CustomValidationDataType.String)); }
        }

        private bool RegularExpressionValidationEnabled
        {
            get { return (RegularExpressionValidation && Enabled && (!ReadOnly)); }
        }

        private bool MultiLineMode
        {
            get { return (TextMode == TextBoxMode.MultiLine); }
        }

        private bool LengthInfoEnabled
        {
            get { return (LengthInfo && MultiLineMode && Enabled && (!(Masked || ReadOnly))); }
        }

        #endregion

        #region Events

        /// <summary>
        /// The event is raised when the content of the text box changes between posts to the server.
        /// </summary>
        [Category("Action")]
        public event EventHandler TextChanged;

        #endregion

        #region Private Methods

        private void TextBoxOnTextChanged(object sender, EventArgs e)
        {
            if (this.TextChanged != null) this.TextChanged(this, e);
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Sets focus to a control.
        /// </summary>
        public override void Focus()
        {
            if (this.Masked)
                m_RadMaskedTextBox.Focus();
            else
                m_TextBox.Focus();
        }

        public override void Validate()
        {
            string valClientId = null;

            if (!ReadOnly)
            {
                base.Validate();
                IsValid = RequiredFieldValidator.IsValid;
                valClientId = RequiredFieldValidator.ClientID;
            }

            if (RangeValidationEnabled)
            {
                if (IsValid)
                {
                    m_RangeValidator.Validate();
                    IsValid = m_RangeValidator.IsValid;
                    valClientId = m_RangeValidator.ClientID;
                }
            }

            if (RegularExpressionValidationEnabled)
            {
                if (IsValid)
                {
                    m_RegularExpressionValidator.Validate();
                    IsValid = m_RegularExpressionValidator.IsValid;
                    valClientId = m_RegularExpressionValidator.ClientID;
                }
            }

            if (this.Theme == MasterPageTheme.Modern)
            {
                if (!IsValid)
                {
                    this.CssClass += " Invalid";
                    this.Attributes["validatorId"] = valClientId;
                }
            }
        }

        /// <summary>
        /// Creates child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            m_TextBox = new System.Web.UI.WebControls.TextBox();
            m_TextBox.ID = "txt";
            m_TextBox.TextChanged += new EventHandler(TextBoxOnTextChanged);
            Controls.Add(m_TextBox);

            m_RadMaskedTextBox = new RadMaskedTextBox();
            m_RadMaskedTextBox.ID = "rtxt";
            m_RadMaskedTextBox.Skin = "None";
            m_RadMaskedTextBox.PromptChar = " ";
            m_RadMaskedTextBox.ResetCaretOnFocus = true;
            m_RadMaskedTextBox.EnableEmbeddedSkins = false;
            m_RadMaskedTextBox.TextChanged += new EventHandler(TextBoxOnTextChanged);
            Controls.Add(m_RadMaskedTextBox);

            m_Span = new HtmlGenericControl("span");
            m_Span.InnerText = "0";
            Controls.Add(m_Span);

            m_RangeValidator = new RangeValidator();
            m_RangeValidator.ID = "rng";
            m_RangeValidator.Display = ValidatorDisplay.Dynamic;
            m_RangeValidator.ForeColor = System.Drawing.Color.Empty;
            m_RangeValidator.CssClass = "Error";

            m_RegularExpressionValidator = new RegularExpressionValidator();
            m_RegularExpressionValidator.ID = "rgxp";
            m_RegularExpressionValidator.Display = ValidatorDisplay.Dynamic;
            m_RegularExpressionValidator.ForeColor = System.Drawing.Color.Empty;
            m_RegularExpressionValidator.CssClass = "Error";

            base.RequiredFieldValidator.ControlToValidate = m_RangeValidator.ControlToValidate = m_RegularExpressionValidator.ControlToValidate = (Masked ? m_RadMaskedTextBox.ID : m_TextBox.ID);

            Controls.Add(m_RangeValidator);
            Controls.Add(m_RegularExpressionValidator);
        }

        /// <summary>
        /// Sets the control to validate to all validators or removes if not needed.
        /// </summary>
        /// <param name="e">The EventArgs object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            string controlId = (Masked ? m_RadMaskedTextBox.ID : m_TextBox.ID);

            if (RangeValidationEnabled)
                m_RangeValidator.ControlToValidate = controlId;
            else
                Page.Validators.Remove(m_RangeValidator);

            if (RegularExpressionValidationEnabled)
                m_RegularExpressionValidator.ControlToValidate = controlId;
            else
                Page.Validators.Remove(m_RegularExpressionValidator);

            if (Required) base.RequiredFieldValidator.ControlToValidate = controlId;
            RequiredValidationEnabled = (!ReadOnly);

            base.OnLoad(e);
        }

        /// <summary>
        /// Registers client script.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (RangeValidationEnabled)
            {
                string min = string.Empty;
                string max = string.Empty;
                switch (this.ValidationType)
                {
                    case CustomValidationDataType.Integer:
                        m_RangeValidator.Type = ValidationDataType.Integer;
                        min = Int32.MinValue.ToString(CultureInfo.CurrentCulture);
                        max = Int32.MaxValue.ToString(CultureInfo.CurrentCulture);
                        break;
                    case CustomValidationDataType.Double:
                        m_RangeValidator.Type = ValidationDataType.Double;
                        min = Decimal.MinValue.ToString(CultureInfo.CurrentCulture);
                        max = Decimal.MaxValue.ToString(CultureInfo.CurrentCulture);
                        break;
                    case CustomValidationDataType.Currency:
                        m_RangeValidator.Type = ValidationDataType.Currency;
                        min = Decimal.MinValue.ToString(CultureInfo.CurrentCulture);
                        max = Decimal.MaxValue.ToString(CultureInfo.CurrentCulture);
                        break;
                    case CustomValidationDataType.Date:
                        m_RangeValidator.Type = ValidationDataType.Date;
                        min = DateTime.MinValue.ToShortDateString();
                        max = DateTime.MaxValue.ToShortDateString();
                        break;
                }
                if (m_ValidationTypeChanged || string.IsNullOrEmpty(MinimumValue)) m_RangeValidator.MinimumValue = min;
                if (m_ValidationTypeChanged || string.IsNullOrEmpty(MaximumValue)) m_RangeValidator.MaximumValue = max;
                m_RangeValidator.Visible = true;
            }
            else
                m_RangeValidator.Visible = false;

            bool registerTextBoxScripts = false;

            if (this.LengthInfoEnabled)
            {
                string script = string.Concat("TextBox_OnKeyUp('", m_TextBox.ClientID, "', '", m_Span.ClientID, "');");
                m_TextBox.Attributes.Add("onkeyup", script);

                ScriptManager.RegisterStartupScript(this, this.GetType(), ClientID + "_OnKeyUp", script + "\r\n", true);

                registerTextBoxScripts = true;
            }

            if (this.EmptyText != null)
            {
                if (this.Masked)
                {
                    m_RadMaskedTextBox.EmptyMessage = this.EmptyText;
                    m_RadMaskedTextBox.HideOnBlur = true;
                }
                else
                {
                    m_TextBox.Attributes["emptyText"] = HttpUtility.HtmlAttributeEncode(this.EmptyText);
                    m_TextBox.Attributes["onfocus"] = "TextBox_OnFocus(this);";
                    m_TextBox.Attributes["onblur"] = "TextBox_OnBlur(this);";

                    if (string.IsNullOrEmpty(this.Text))
                        m_TextBox.Text = this.EmptyText;

                    registerTextBoxScripts = true;
                }
            }

            if (!string.IsNullOrEmpty(this.DefaultButtonUniqueId))
            {
                if (this.Masked)
                {
                    m_RadMaskedTextBox.Attributes["defaultButtonUniqueId"] = this.DefaultButtonUniqueId;
                    m_RadMaskedTextBox.ClientEvents.OnKeyPress = "TextBox_OnKeyPress";
                }
                else
                {
                    m_TextBox.Attributes["defaultButtonUniqueId"] = this.DefaultButtonUniqueId;
                    m_TextBox.Attributes["onkeypress"] = "TextBox_OnKeyPress(this, event)";
                }

                registerTextBoxScripts = true;
            }

            if (registerTextBoxScripts)
                ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "TextBoxScripts", ResourceProvider.GetResourceUrl("Scripts.TextBox.js", true));

            if (!Masked) m_RadMaskedTextBox.Visible = false;
        }

        /// <summary>
        /// Renders control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (writer == null) return;

            bool renderSecondRow = ((Required || (RangeValidation && ValidationType != CustomValidationDataType.String) || RegularExpressionValidation)
                && Enabled && (!ReadOnly));

            if (this.Theme != MasterPageTheme.Modern)
                BaseValidatedControl.RenderValidatedControlBeginTag(Required && ShowRequired, writer, this.Width);

            if (Masked)
            {
                if (!this.Width.IsEmpty) m_RadMaskedTextBox.Width = ((this.Theme == MasterPageTheme.Modern) ? this.Width : Unit.Percentage(100));
                m_RadMaskedTextBox.RenderControl(writer);
            }
            else
            {
                if (!this.Width.IsEmpty) m_TextBox.Width = ((this.Theme == MasterPageTheme.Modern) ? this.Width : Unit.Percentage(100));
                if (MultiLineMode && (MaxLength > 0)) m_TextBox.Attributes.Add("maxLength", MaxLength.ToString(CultureInfo.CurrentCulture));
                m_TextBox.RenderControl(writer);

                if (LengthInfoEnabled)
                {
                    string lengthInfoStringFormat = ((MaxLength > 0) ? Resources.TextBox_LengthInfoStringFormat2 : Resources.TextBox_LengthInfoStringFormat1);
                    writer.Write("<br />");
                    m_Span.RenderControl(writer);
                    writer.Write(string.Format(CultureInfo.CurrentCulture, " " + lengthInfoStringFormat, MaxLength));
                }
            }

            if (this.Theme != MasterPageTheme.Modern)
                BaseValidatedControl.RenderValidatedControlMiddleTag(renderSecondRow, writer);

            if (Required && Enabled && (!ReadOnly))
            {
                if (!DesignMode && string.IsNullOrEmpty(ErrorMessage))
                    base.RequiredFieldValidator.ErrorMessage = Resources.TextBox_RequiredValidator_ErrorMessage;
                base.RequiredFieldValidator.RenderControl(writer);
            }

            if (RangeValidationEnabled)
            {
                if (!DesignMode && string.IsNullOrEmpty(m_RangeValidator.ErrorMessage))
                    m_RangeValidator.ErrorMessage = Resources.TextBox_RangeValidator_ErrorMessage;
                m_RangeValidator.RenderControl(writer);
            }

            if (RegularExpressionValidationEnabled)
            {
                if (!DesignMode && string.IsNullOrEmpty(m_RegularExpressionValidator.ErrorMessage))
                    m_RegularExpressionValidator.ErrorMessage = Resources.TextBox_RegularExpressionValidator_ErrorMessage;
                m_RegularExpressionValidator.RenderControl(writer);
            }

            if (this.Theme != MasterPageTheme.Modern)
                BaseValidatedControl.RenderValidatedControlEndTag(renderSecondRow, writer);
        }

        #endregion
    }
}
