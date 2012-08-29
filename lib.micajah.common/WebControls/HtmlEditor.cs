using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// The HTML editor.
    /// </summary>
    public class HtmlEditor : RadEditor, INamingContainer, IValidated
    {
        #region Members

        private CustomValidator m_Validator;

        #endregion

        #region Overriden Properties

        [Browsable(false)]
        public override string Skin
        {
            get { return "Default"; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the text for the required field's error message when validation fails.
        /// The default value gets from resources.
        /// </summary>
        [Category("Appearance")]
        [Description("The text for the required field's error message when validation fails.")]
        [ResourceDefaultValue("TextBox_RequiredValidator_ErrorMessage")]
        public string ErrorMessage
        {
            get
            {
                string str = (string)ViewState["ErrorMessage"];
                return (string.IsNullOrEmpty(str) ? Resources.TextBox_RequiredValidator_ErrorMessage : str);
            }
            set { ViewState["ErrorMessage"] = value; }
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
        /// Gets or sets the group of controls for which this control causes validation when it posts back to the server.
        /// </summary>
        [Category("Behavior")]
        [Description("The group of controls for which this control causes validation when it posts back to the server.")]
        [DefaultValue("")]
        public string ValidationGroup
        {
            get
            {
                object obj = ViewState["ValidationGroup"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["ValidationGroup"] = value; }
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
                object obj = ViewState["ValidatorInitialValue"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["ValidatorInitialValue"] = value; }
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

        private void EnsureValidator()
        {
            if (m_Validator == null)
            {
                if (this.Required)
                {
                    m_Validator = new CustomValidator();
                    m_Validator.ID = "val";
                    m_Validator.ErrorMessage = this.ErrorMessage;
                    m_Validator.Display = ValidatorDisplay.Dynamic;
                    m_Validator.ValidationGroup = this.ValidationGroup;
                    m_Validator.ClientValidationFunction = "HtmlEditor_Validate";
                    m_Validator.CssClass = "Error Block";
                    m_Validator.ForeColor = System.Drawing.Color.Empty;
                    m_Validator.Attributes["htmlEditorId"] = this.ClientID;
                    if (!string.IsNullOrEmpty(this.ValidatorInitialValue)) m_Validator.Attributes["initialValue"] = this.ValidatorInitialValue;
                    this.Controls.Add(m_Validator);
                }
            }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Returns a list of client script library dependencies for the control.
        /// </summary>
        /// <returns>The list of client script library dependencies for the control.</returns>
        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            if (this.Required)
            {
                List<ScriptReference> list = new List<ScriptReference>();
                list.AddRange(base.GetScriptReferences());
                list.Add(new ScriptReference(ResourceProvider.GetResourceUrl("Scripts.HtmlEditor.js", true)));
                return list;
            }
            else
                return base.GetScriptReferences();
        }

        /// <summary>
        /// Raises the PreRender event.
        /// </summary>
        /// <param name="e">An System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.EnsureValidator();
        }

        /// <summary>
        /// Renders control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            this.EnsureValidator();

            if (this.Theme != MasterPageTheme.Modern)
                BaseValidatedControl.RenderValidatedControlBeginTag(this.Required && this.ShowRequired, writer);

            if (m_Validator != null) m_Validator.Visible = false;
            base.Render(writer);
            if (m_Validator != null) m_Validator.Visible = true;

            if (this.Theme != MasterPageTheme.Modern)
                BaseValidatedControl.RenderValidatedControlEndTag(m_Validator, this.Required, this.DesignMode, this.ErrorMessage, writer);
            else if (m_Validator != null)
                m_Validator.RenderControl(writer);
        }

        #endregion
    }
}
