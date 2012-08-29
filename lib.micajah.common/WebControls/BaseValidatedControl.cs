using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// The base class for validated controls.
    /// </summary>
    [ParseChildren(true)]
    [PersistChildren(false)]
    public abstract class BaseValidatedControl : System.Web.UI.Control, IValidated, INamingContainer
    {
        #region Members

        private RequiredFieldValidator m_RequiredFieldValidator;
        private bool m_ChildControlsCreated;
        private bool m_RequiredValidationEnabled = true;

        #endregion

        #region Protected Properties

        protected RequiredFieldValidator RequiredFieldValidator
        {
            get
            {
                EnsureChildControls();
                return m_RequiredFieldValidator;
            }
        }

        protected bool RequiredValidationEnabled
        {
            get { return (Required && Enabled && m_RequiredValidationEnabled); }
            set { m_RequiredValidationEnabled = value; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the collection of arbitrary attributes (for rendering only) that do not correspond to properties on the control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual System.Web.UI.AttributeCollection Attributes
        {
            get
            {
                EnsureChildControls();
                return m_RequiredFieldValidator.Attributes;
            }
        }

        /// <summary>
        /// Gets or sets the value indicating that the control's value is validated as required.
        /// </summary>
        [Category("Behavior")]
        [Description("Required state of the control.")]
        [DefaultValue(false)]
        public bool Required
        {
            get
            {
                object obj = ViewState["Required"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["Required"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the control is displayed as required.
        /// </summary>
        [Category("Behavior")]
        [Description("Whether the control is displayed as required.")]
        [DefaultValue(true)]
        public bool ShowRequired
        {
            get
            {
                object obj = ViewState["ShowRequired"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { ViewState["ShowRequired"] = value; }
        }

        /// <summary>
        /// Gets or sets the text for the required field's error message when validation fails.
        /// The default value gets from resources.
        /// </summary>
        [Category("Appearance")]
        [Description("The text for the required field's error message when validation fails.")]
        [DefaultValue("")]
        public string ErrorMessage
        {
            get
            {
                EnsureChildControls();
                return m_RequiredFieldValidator.ErrorMessage;
            }
            set
            {
                EnsureChildControls();
                m_RequiredFieldValidator.ErrorMessage = value;
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
                return m_RequiredFieldValidator.ValidationGroup;
            }
            set
            {
                EnsureChildControls();
                m_RequiredFieldValidator.ValidationGroup = value;
            }
        }

        /// <summary>
        /// Gets or sets the initial value of the control. Uses by inner RequiredFieldValidator control.
        /// </summary>
        [Category("Behavior")]
        [Description("The initial value of the control used by required validation.")]
        [DefaultValue("")]
        public string ValidatorInitialValue
        {
            get
            {
                EnsureChildControls();
                return m_RequiredFieldValidator.InitialValue;
            }
            set
            {
                EnsureChildControls();
                m_RequiredFieldValidator.InitialValue = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is enabled.
        /// </summary>
        [Category("Behavior")]
        [Description("Enabled state of the control.")]
        [DefaultValue(true)]
        public virtual bool Enabled
        {
            get
            {
                EnsureChildControls();
                return m_RequiredFieldValidator.Enabled;
            }
            set
            {
                EnsureChildControls();
                m_RequiredFieldValidator.Enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the tab index of the control.
        /// The default is 0, which indicates that this property is not set.
        /// </summary>
        [Category("Accessibility")]
        [Description("The tab order of the control.")]
        [DefaultValue(0)]
        public virtual short TabIndex
        {
            get
            {
                object obj = ViewState["TabIndex"];
                return ((obj == null) ? (short)0 : (short)obj);
            }
            set { ViewState["TabIndex"] = value; }
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

        #region Overriden Methods

        /// <summary>
        /// Determines whether the server control contains child controls. If it does not, it creates child controls.
        /// </summary>
        protected override void EnsureChildControls()
        {
            if (!m_ChildControlsCreated) CreateChildControls();
        }

        /// <summary>
        /// Creates child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            m_RequiredFieldValidator = new RequiredFieldValidator();
            m_RequiredFieldValidator.ID = "req";
            m_RequiredFieldValidator.Display = ValidatorDisplay.Dynamic;
            m_RequiredFieldValidator.ForeColor = System.Drawing.Color.Empty;
            m_RequiredFieldValidator.CssClass = "Error";

            Controls.Add(m_RequiredFieldValidator);

            m_ChildControlsCreated = true;
        }

        /// <summary>
        /// Raises the Load event.
        /// </summary>
        /// <param name="e">The EventArgs object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            if (!RequiredValidationEnabled)
                Page.Validators.Remove(m_RequiredFieldValidator);
        }

        /// <summary>
        /// Renders control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            EnsureChildControls();

            if (this.Theme != MasterPageTheme.Modern)
                BaseValidatedControl.RenderValidatedControlBeginTag(Required && ShowRequired, writer);

            foreach (System.Web.UI.Control ctrl in Controls)
            {
                if (ctrl.Visible && (ctrl.ID != m_RequiredFieldValidator.ID))
                    ctrl.RenderControl(writer);
            }

            if (this.Theme != MasterPageTheme.Modern)
                BaseValidatedControl.RenderValidatedControlMiddleTag(Required, writer);

            if (Required && Enabled)
            {
                if (!DesignMode && string.IsNullOrEmpty(m_RequiredFieldValidator.ErrorMessage))
                    m_RequiredFieldValidator.ErrorMessage = Resources.TextBox_RequiredValidator_ErrorMessage;
                m_RequiredFieldValidator.RenderControl(writer);
            }

            if (this.Theme != MasterPageTheme.Modern)
                BaseValidatedControl.RenderValidatedControlEndTag(Required, writer);
        }

        #endregion

        #region Public Methods

        public virtual void Validate()
        {
            if (Required && Enabled)
                m_RequiredFieldValidator.Validate();
        }

        public static void RenderValidatedControlBeginTag(bool required, HtmlTextWriter writer)
        {
            RenderValidatedControlBeginTag(required, writer, Unit.Empty);
        }

        public static void RenderValidatedControlBeginTag(bool required, HtmlTextWriter writer, Unit width)
        {
            if (writer == null) return;

            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            if (!width.IsEmpty) writer.AddAttribute(HtmlTextWriterAttribute.Width, width.ToString(CultureInfo.InvariantCulture));
            writer.RenderBeginTag(HtmlTextWriterTag.Table); // Table
            writer.RenderBeginTag(HtmlTextWriterTag.Tr); // Tr1
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "3px");
            if (required) writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "Maroon");
            writer.AddStyleAttribute(HtmlTextWriterStyle.WhiteSpace, "nowrap");
            writer.RenderBeginTag(HtmlTextWriterTag.Td); //Td1
            writer.Write("&nbsp;");
            writer.RenderEndTag(); // Td1
            writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "2px");
            writer.RenderBeginTag(HtmlTextWriterTag.Td); //Td2
        }

        public static void RenderValidatedControlEndTag(System.Web.UI.WebControls.BaseValidator validator,
            bool required, bool designMode, string errorMessage, HtmlTextWriter writer)
        {
            if (writer == null) return;

            writer.RenderEndTag(); // Td2
            writer.RenderEndTag(); // Tr1

            if (required)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr); // Tr2
                writer.RenderBeginTag(HtmlTextWriterTag.Td); // Td3
                writer.RenderEndTag(); // Td3
                writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "2px");
                writer.RenderBeginTag(HtmlTextWriterTag.Td); // Td4

                if (validator != null)
                {
                    if (designMode)
                        validator.ErrorMessage = errorMessage;
                    validator.CssClass = "Error";
                    validator.RenderControl(writer);
                }

                writer.RenderEndTag(); // Td4
                writer.RenderEndTag(); // Tr2
            }

            writer.RenderEndTag(); // Table
        }

        public static void RenderValidatedControlMiddleTag(bool renderSecondRow, HtmlTextWriter writer)
        {
            if (writer == null) return;

            writer.RenderEndTag(); // Td2
            writer.RenderEndTag(); // Tr1
            if (renderSecondRow)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr); // Tr2
                writer.RenderBeginTag(HtmlTextWriterTag.Td); // Td3
                writer.RenderEndTag(); // Td3
                writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "2px");
                writer.RenderBeginTag(HtmlTextWriterTag.Td); // Td4
            }
        }

        public static void RenderValidatedControlEndTag(bool renderSecondRow, HtmlTextWriter writer)
        {
            if (writer == null) return;

            if (renderSecondRow)
            {
                writer.RenderEndTag(); // Td4
                writer.RenderEndTag(); // Tr2
            }
            writer.RenderEndTag(); // Table
        }

        #endregion
    }
}
