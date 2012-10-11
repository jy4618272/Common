using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Displays a text box edit field combined with a ListBox, enabling the user to select items from the list or to enter new text.
    /// </summary>
    [SupportsEventValidation]
    public class ComboBox : RadComboBox, IValidated
    {
        #region Members

        private CustomValidator m_Validator;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the text for the required field's error message when validation fails.
        /// The default value gets from resources.
        /// </summary>
        [Category("Appearance")]
        [Description("The text for the required field's error message when validation fails.")]
        [DefaultValue("")]
        public new string ErrorMessage
        {
            get
            {
                object obj = ViewState["ErrorMessage"];
                return ((obj == null) ? string.Empty : (string)obj);
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

        #region Overriden Methods

        /// <summary>
        /// Returns a list of client script library dependencies for the control.
        /// </summary>
        /// <returns>The list of client script library dependencies for the control.</returns>
        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            if (Required)
            {
                List<ScriptReference> list = new List<ScriptReference>();
                list.AddRange(base.GetScriptReferences());
                list.Add(new ScriptReference(ResourceProvider.GetResourceUrl("Scripts.ComboBox.js", true)));
                return list;
            }
            else
                return base.GetScriptReferences();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.Theme == MasterPageTheme.Modern)
                ApplyStyle(this);
        }

        /// <summary>
        /// Raises the PreRender event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Items.Count > 10) Height = Unit.Pixel(200);

            if (this.Required)
            {
                m_Validator = new CustomValidator();
                m_Validator.ID = "val";
                m_Validator.ErrorMessage = this.ErrorMessage;
                m_Validator.Display = ValidatorDisplay.Dynamic;
                m_Validator.ValidationGroup = this.ValidationGroup;
                m_Validator.ClientValidationFunction = "ComboBox_Validate";
                m_Validator.ForeColor = System.Drawing.Color.Empty;
                m_Validator.CssClass = "Error";
                m_Validator.Attributes["comboBoxId"] = this.ClientID;
                if (!string.IsNullOrEmpty(this.ValidatorInitialValue))
                    m_Validator.Attributes["initialValue"] = this.ValidatorInitialValue;

                this.Controls.Add(m_Validator);
            }
        }

        /// <summary>
        /// Renders control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.Visible) return;

            if ((!DesignMode) && string.IsNullOrEmpty(ErrorMessage) && (m_Validator != null))
                m_Validator.ErrorMessage = (this.AllowCustomText ? Resources.ComboBox_RequiredValidator_ErrorMessage2 : Resources.ComboBox_RequiredValidator_ErrorMessage1);

            if (this.Theme == MasterPageTheme.Modern)
            {
                if (m_Validator != null)
                    base.OnClientSelectedIndexChanged = "ComboBox_SelectedIndexChanged";
            }
            else
                BaseValidatedControl.RenderValidatedControlBeginTag(this.Required && this.ShowRequired, writer, this.Width);

            if (m_Validator != null) m_Validator.Visible = false;

            base.Render(writer);

            if (m_Validator != null) m_Validator.Visible = true;

            if (this.Theme == MasterPageTheme.Modern)
            {
                if (m_Validator != null)
                {
                    m_Validator.Attributes["controltovalidate2"] = this.ClientID + "_Input";
                    m_Validator.Style[HtmlTextWriterStyle.MarginLeft] = "17px !important";
                    m_Validator.RenderControl(writer);
                }
            }
            else
                BaseValidatedControl.RenderValidatedControlEndTag(m_Validator, this.Required, this.DesignMode, this.ErrorMessage, writer);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Applies the modern styles for the combobox.
        /// </summary>
        /// <param name="comboBox">The Telerik.Web.UI.RadComboBox object to apply the styles to.</param>
        public static void ApplyStyle(RadComboBox comboBox)
        {
            comboBox.EnableEmbeddedSkins = false;
            comboBox.Skin = "Modern";

            ResourceProvider.RegisterStyleSheetResource(comboBox, ResourceProvider.ComboBoxModernStyleSheet, "ComboBoxModernStyleSheet", true);
        }

        #endregion
    }
}
