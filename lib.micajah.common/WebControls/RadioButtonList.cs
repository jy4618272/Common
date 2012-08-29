using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;

namespace Micajah.Common.WebControls
{
    public class RadioButtonList : System.Web.UI.WebControls.RadioButtonList, IValidated
    {
        #region Members

        private RequiredFieldValidator m_Validator;

        #endregion

        #region Contructors
        public RadioButtonList()
        {
            m_Validator = new RequiredFieldValidator();
        }
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

        #region Public Properties

        [Category("Appearance")]
        [DefaultValue("")]
        public string ErrorMessage
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

        [Category("Behavior")]
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

        #region Override Methods

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (Required)
            {
                m_Validator.ID = "req";
                m_Validator.ErrorMessage = this.ErrorMessage;
                m_Validator.Display = ValidatorDisplay.Dynamic;
                m_Validator.ControlToValidate = this.ClientID;
                m_Validator.InitialValue = this.ValidatorInitialValue;
                m_Validator.ValidationGroup = this.ValidationGroup;
                m_Validator.ForeColor = System.Drawing.Color.Empty;
                m_Validator.CssClass = "Error Block";

                Controls.Add(m_Validator);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
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