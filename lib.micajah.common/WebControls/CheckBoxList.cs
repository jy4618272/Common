using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Provides a multi selection check box group that can be dynamically created by binding the control to a data sourceRow.
    /// </summary>
    [ValidationProperty("SelectedValue")]
    [SupportsEventValidation]
    public class CheckBoxList : System.Web.UI.WebControls.CheckBoxList, IValidated
    {
        #region Members

        private CustomValidator m_Validator;

        private bool m_ChildControlsCreated;

        #endregion

        #region Hidden Properties

        [Browsable(false)]
        public override string SkinID { get { return base.SkinID; } }

        [Browsable(false)]
        public override bool EnableTheming { get { return base.EnableTheming; } }

        [Browsable(false)]
        public override Color BackColor { get { return base.BackColor; } set { return; } }

        [Browsable(false)]
        public override Color BorderColor { get { return base.BorderColor; } set { return; } }

        [Browsable(false)]
        public override BorderStyle BorderStyle { get { return base.BorderStyle; } set { return; } }

        [Browsable(false)]
        public override Unit BorderWidth { get { return base.BorderWidth; } set { return; } }

        [Browsable(false)]
        public override string CssClass { get { return base.CssClass; } set { return; } }

        [Browsable(false)]
        public override FontInfo Font { get { return null; } }

        [Browsable(false)]
        public override Color ForeColor { get { return base.ForeColor; } set { return; } }

        [Browsable(false)]
        public override int CellPadding { get { return base.CellPadding; } set { return; } }

        [Browsable(false)]
        public override int CellSpacing { get { return base.CellSpacing; } set { return; } }

        #endregion

        #region Private Properties

        private static string ClientScript
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"function Cbl_Validation(source, arguments) 
{ 
    arguments.IsValid = false;
    var Elem = document.getElementById(source.controltovalidate);
    if (Elem)
    {
        var Elems = Elem.getElementsByTagName('INPUT'); 
        for (var i = 0; i < Elems.length; i++)
        {
            if (Elems[i].type == 'checkbox')
            {
                if (Elems[i].checked)
                {
                    arguments.IsValid = true;
                    break;
                }
            }
        }
    }
}
");
                return sb.ToString();
            }
        }

        #endregion

        #region Public Properties

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
                return m_Validator.ErrorMessage;
            }
            set
            {
                EnsureChildControls();
                m_Validator.ErrorMessage = value;
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

        /// <summary>
        /// Gets or sets the group of controls for which this control causes validation when it posts back to the server.
        /// </summary>
        [Category("Behavior")]
        [Description("The group of controls for which this control causes validation when it posts back to the server.")]
        [DefaultValue("")]
        public override string ValidationGroup
        {
            get { return base.ValidationGroup; }
            set
            {
                base.ValidationGroup = value;
                EnsureChildControls();
                m_Validator.ValidationGroup = value;
            }
        }

        /// <summary>
        /// Gets the values string of selected items separated by comma.
        /// Marks the items as selected, if the item value is presented in specified values string.
        /// </summary>
        [Browsable(false)]
        public override string SelectedValue
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (ListItem Item in this.Items)
                {
                    if (Item.Selected) sb.AppendFormat("{0},", Item.Value);
                }

                return sb.ToString().TrimEnd(new char[] { ',' });
            }
            set
            {
                foreach (ListItem Item in this.Items)
                {
                    Item.Selected = false;
                }

                if (!string.IsNullOrEmpty(value))
                {
                    string[] Values = value.Split(',');
                    foreach (string CurrentValue in Values)
                    {
                        ListItem Item = this.Items.FindByValue(CurrentValue);
                        if (Item != null) Item.Selected = true;
                    }
                }
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

        #region Override Methods

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
            m_Validator = new CustomValidator();
            m_Validator.ID = "req";
            m_Validator.ClientValidationFunction = "Cbl_Validation";
            m_Validator.Display = ValidatorDisplay.Dynamic;
            m_Validator.ValidationGroup = this.ValidationGroup;
            m_Validator.ForeColor = System.Drawing.Color.Empty;
            m_Validator.CssClass = "Error Block";

            Controls.Add(m_Validator);

            m_ChildControlsCreated = true;
        }

        /// <summary>
        /// Raises the PreRender event and registers the client scripts.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            m_Validator.ControlToValidate = this.ClientID;
            if (this.Required && this.Visible && this.Enabled)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Cbl_Validation", ClientScript, true);
            else
                m_Validator.Enabled = m_Validator.Visible = false;
        }

        /// <summary>
        /// Renders the control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = Resources.CheckBoxList_RequiredValidator_ErrorMessage;

            if (this.Theme != MasterPageTheme.Modern)
                BaseValidatedControl.RenderValidatedControlBeginTag(this.Required && this.ShowRequired, writer, this.Width);

            base.Render(writer);

            if (this.Theme != MasterPageTheme.Modern)
                BaseValidatedControl.RenderValidatedControlEndTag(m_Validator, this.Required, this.DesignMode, this.ErrorMessage, writer);
            else
                m_Validator.RenderControl(writer);
        }

        #endregion
    }
}
