using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// A control that displays a link to another Web page.
    /// </summary>
    [ToolboxData("<{0}:Link runat=server>HyperLink</{0}:Link>")]
    [ParseChildren(false)]
    [DefaultProperty("Text")]
    public class Link : System.Web.UI.Control
    {
        #region Public Properties

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("The text to be shown for the link.")]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor))]
        [Description("The URL of the image to be shown.")]
        public string ImageUrl
        {
            get
            {
                String s = (String)ViewState["ImageUrl"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["ImageUrl"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(64)]
        public Unit ImageWidth
        {
            get
            {
                object s = ViewState["ImageWidth"];
                return ((s == null) ? Unit.Empty : (Unit)s);
            }

            set
            {
                ViewState["ImageWidth"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(64)]
        public Unit ImageHeight
        {
            get
            {
                object s = ViewState["ImageHeight"];
                return ((s == null) ? Unit.Empty : (Unit)s);
            }

            set
            {
                ViewState["ImageHeight"] = value;
            }
        }

        [Bindable(true)]
        [Category("Navigation")]
        [DefaultValue("")]
        [Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(UITypeEditor))]
        [Description("The URL to navigate to.")]
        public string NavigateUrl
        {
            get
            {
                String s = (String)ViewState["NavigateUrl"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["NavigateUrl"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the target window or frame in which to display the Web page content linked to when the control is clicked.
        /// </summary>
        [Category("Navigation")]
        [TypeConverter(typeof(TargetConverter))]
        [DefaultValue("")]
        [Description("The target frame for the NavigateUrl.")]
        public string Target
        {
            get
            {
                object obj = base.ViewState["Target"];
                return (obj == null) ? string.Empty : (string)obj;
            }
            set { base.ViewState["Target"] = value; }
        }


        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("")]
        [Description("The tooltip displayed when the mouse is over the control.")]
        public string ToolTip
        {
            get
            {
                String s = (String)ViewState["ToolTip"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["ToolTip"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string CssClass
        {
            get
            {
                String s = (String)ViewState["CssClass"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["CssClass"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is enabled.
        /// </summary>
        [Bindable(true)]
        [Category("Misc")]
        [DefaultValue(true)]
        [Description("Enabled state of the control.")]
        public bool Enabled
        {
            get { return ((ViewState["Enabled"] == null) ? true : (bool)ViewState["Enabled"]); }
            set { ViewState["Enabled"] = value; }
        }

        #endregion

        #region Constructors

        public Link()
        {
        }

        public Link(string text, string navigateUrl)
        {
            this.Text = text;
            this.NavigateUrl = navigateUrl;
        }

        public Link(string text, string navigateUrl, string toolTip)
        {
            this.Text = text;
            this.NavigateUrl = navigateUrl;
            this.ToolTip = toolTip;
        }

        public Link(string text, string navigateUrl, string toolTip, bool visible)
        {
            this.Text = text;
            this.NavigateUrl = navigateUrl;
            this.ToolTip = toolTip;
            this.Visible = visible;
        }

        #endregion

        #region Overriden Methods

        public override void RenderControl(HtmlTextWriter writer)
        {
            System.Web.UI.WebControls.HyperLink link = null;

            try
            {
                link = new System.Web.UI.WebControls.HyperLink();
                link.ID = this.ClientID;
                link.NavigateUrl = this.NavigateUrl;
                link.Target = Target;
                link.ToolTip = this.ToolTip;
                link.CssClass = this.CssClass;
                link.Enabled = Enabled;
                link.Visible = Visible;

                if (this.ImageUrl.Trim().Length > 0)
                {
                    using (Image image = new Image())
                    {
                        image.ImageUrl = this.ImageUrl;
                        image.ImageAlign = ImageAlign.AbsMiddle;
                        image.ToolTip = this.ToolTip;
                        image.Height = this.ImageHeight;
                        image.Width = this.ImageWidth;
                        link.Controls.Add(image);
                    }

                    using (LiteralControl literal = new LiteralControl(this.Text))
                    {
                        link.Controls.Add(literal);
                    }
                }
                else
                {
                    link.Text = this.Text;
                }

                link.RenderControl(writer);
            }
            finally
            {
                if (link != null) link.Dispose();
            }
        }

        #endregion
    }
}
