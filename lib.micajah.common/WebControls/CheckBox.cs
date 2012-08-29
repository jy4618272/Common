using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Displays a check box that allows the user to select a true or false condition.
    /// </summary>
    public class CheckBox : BaseValidatedControl, ICheckBox, ICheckBoxControl, IValidated
    {
        #region Members

        private System.Web.UI.WebControls.CheckBox m_CheckBox;
        private System.Web.UI.WebControls.TextBox m_TextBox;

        private Micajah.Common.Pages.MasterPage m_MasterPage;

        #endregion

        #region Overriden Properties

        /// <summary>
        /// Gets or sets a value indicating whether the control is enabled.
        /// </summary>
        public override bool Enabled
        {
            get { return m_CheckBox.Enabled; }
            set
            {
                base.Enabled = value;
                m_CheckBox.Enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the tab index of the control.
        /// The default is 0, which indicates that this property is not set.
        /// </summary>
        public override short TabIndex
        {
            get
            {
                EnsureChildControls();
                return m_CheckBox.TabIndex;
            }
            set
            {
                EnsureChildControls();
                m_CheckBox.TabIndex = value;
            }
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
            }
        }

        /// <summary>
        /// Gets the collection of arbitrary attributes (for rendering only) that do not correspond to properties on the control.
        /// </summary>
        public override System.Web.UI.AttributeCollection Attributes
        {
            get
            {
                EnsureChildControls();
                return m_CheckBox.Attributes;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the text label associated with the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The text label associated with the control.")]
        [DefaultValue("")]
        public string Text
        {
            get
            {
                EnsureChildControls();
                return m_CheckBox.Text;
            }
            set
            {
                EnsureChildControls();
                m_CheckBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the alignment of the text label associated with the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The alignment of the text label associated with the control.")]
        [DefaultValue(TextAlign.Right)]
        public TextAlign TextAlign
        {
            get
            {
                EnsureChildControls();
                return m_CheckBox.TextAlign;
            }
            set
            {
                EnsureChildControls();
                m_CheckBox.TextAlign = value;
            }
        }

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
                EnsureChildControls();
                return m_CheckBox.AutoPostBack;
            }
            set
            {
                EnsureChildControls();
                m_CheckBox.AutoPostBack = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is checked.
        /// </summary>
        [Category("Behavior")]
        [Description("Checked state of the control.")]
        [DefaultValue(false)]
        public bool Checked
        {
            get
            {
                EnsureChildControls();
                return m_CheckBox.Checked;
            }
            set
            {
                EnsureChildControls();
                m_CheckBox.Checked = value;
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
            get
            {
                EnsureChildControls();
                return m_CheckBox.CausesValidation;
            }
            set
            {
                EnsureChildControls();
                m_CheckBox.CausesValidation = value;
            }
        }

        /// <summary>
        /// Gets or sets the rendering mode for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The rendering mode.")]
        [DefaultValue(CheckBoxRenderingMode.CheckBox)]
        public CheckBoxRenderingMode RenderingMode
        {
            get
            {
                object obj = this.ViewState["RenderingMode"];
                return ((obj == null) ? CheckBoxRenderingMode.CheckBox : (CheckBoxRenderingMode)obj);
            }
            set { this.ViewState["RenderingMode"] = value; }
        }

        /// <summary>
        /// Gets or sets the text label associated with the control in checked state.
        /// </summary>
        [Category("Appearance")]
        [Description("The text label associated with the control in checked state.")]
        [ResourceDefaultValue("CheckBox_CheckedText")]
        public string CheckedText
        {
            get
            {
                object obj = this.ViewState["CheckedText"];
                return (obj == null) ? Resources.CheckBox_CheckedText : (string)obj;
            }
            set { this.ViewState["CheckedText"] = value; }
        }

        /// <summary>
        /// Gets or sets the text label associated with the control in unchecked state.
        /// </summary>
        [Category("Appearance")]
        [Description("The text label associated with the control in unchecked state.")]
        [ResourceDefaultValue("CheckBox_UncheckedText")]
        public string UncheckedText
        {
            get
            {
                object obj = this.ViewState["UncheckedText"];
                return (obj == null) ? Resources.CheckBox_UncheckedText : (string)obj;
            }
            set { this.ViewState["UncheckedText"] = value; }
        }

        #endregion

        #region Private Properties

        private Micajah.Common.Pages.MasterPage MasterPage
        {
            get
            {
                if (m_MasterPage == null) m_MasterPage = (this.Page.Master as Micajah.Common.Pages.MasterPage);
                return m_MasterPage;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// The CheckedChanged event is raised when the value of the Checked property changes between posts to the server. 
        /// This event does not post the page back to the server unless the AutoPostBack property is set to true.
        /// </summary>
        [Category("Action")]
        public event EventHandler CheckedChanged;

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Sets focus to a control.
        /// </summary>
        public override void Focus()
        {
            EnsureChildControls();
            m_CheckBox.Focus();
        }

        /// <summary>
        /// Creates child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            m_CheckBox = new System.Web.UI.WebControls.CheckBox();
            m_CheckBox.ID = "CheckBox";
            m_CheckBox.CheckedChanged += new EventHandler(OnCheckedChanged);
            base.Controls.Add(m_CheckBox);

            m_TextBox = new System.Web.UI.WebControls.TextBox();
            m_TextBox.ID = "TextBox";
            m_TextBox.Style.Add(HtmlTextWriterStyle.Display, "none");
            m_TextBox.Visible = false;
            Controls.Add(m_TextBox);

            base.RequiredFieldValidator.ControlToValidate = m_TextBox.ID;
        }

        /// <summary>
        /// Register client script.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.Required)
            {
                m_TextBox.Visible = true;

                if (m_CheckBox.Checked) m_TextBox.Text = "checked";
                m_CheckBox.Attributes.Add("onclick", "CheckBox_OnClick(this.checked, '" + m_TextBox.ClientID + "');");

                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "CheckBox_OnClick"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CheckBox_OnClick"
                        , "function CheckBox_OnClick(checkedState, textBoxId) { var TextBox = document.getElementById(textBoxId);  if (TextBox) { TextBox.value = (checkedState ? 'checked' : ''); } }\r\n"
                        , true);
                }

                if (this.RenderingMode == CheckBoxRenderingMode.OnOffSwitch)
                {
                    this.RequiredFieldValidator.Style[HtmlTextWriterStyle.PaddingTop] = "5px !important";
                    this.RequiredFieldValidator.Style["background-position"] = "left 5px !important";
                }
                else
                {
                    this.RequiredFieldValidator.Style[HtmlTextWriterStyle.PaddingTop] = "2px !important";
                    this.RequiredFieldValidator.Style["background-position"] = "left 2px !important";
                }
            }

            if (this.RenderingMode == CheckBoxRenderingMode.OnOffSwitch)
            {
                ResourceProvider.RegisterStyleSheetResource(this, ResourceProvider.OnOffSwitchStyleSheet, "OnOffSwitchStyleSheet", true);

                if (this.MasterPage != null)
                    m_MasterPage.EnableJQuery = true;
                else
                    ScriptManager.RegisterClientScriptInclude(this.Page, this.Page.GetType(), "JQueryScript", ResourceProvider.JQueryScriptUrl);

                ScriptManager.RegisterClientScriptInclude(this.Page, this.Page.GetType(), "OnOffSwitchScript", ResourceProvider.GetResourceUrl("Scripts.OnOffSwitch.js", true));

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ApplyStyle"
                    , string.Format(CultureInfo.InvariantCulture, "$(document).ready(function (){{ $('#{0}').iphoneStyle({{ checkedLabel: '{1}', uncheckedLabel: '{2}'{3} }}); }});\r\n"
                    , this.m_CheckBox.ClientID, this.CheckedText, this.UncheckedText, (this.AutoPostBack ? ", onChange : function (){ " + Page.ClientScript.GetPostBackEventReference(m_CheckBox, null) + "; }" : string.Empty))
                    , true);
            }
        }

        /// <summary>
        /// Renders control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                this.ErrorMessage = Resources.CheckBox_RequiredValidator_ErrorMessage;
            base.Render(writer);
        }

        #endregion

        #region Private Methods

        private void OnCheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckedChanged != null) CheckedChanged(this, e);
        }

        #endregion
    }
}
