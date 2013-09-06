using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// The message box that displays the error, warning, information notes.
    /// </summary>
    [ToolboxData("<{0}:NoticeMessageBox runat=server></{0}:NoticeMessageBox>")]
    public class NoticeMessageBox : Control
    {
        #region Members

        private Micajah.Common.Pages.MasterPage m_MasterPage;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the type of the message to display.
        /// </summary>
        [Category("Appearance")]
        [Description("Type of the message to display.")]
        [DefaultValue(NoticeMessageType.Error)]
        public NoticeMessageType MessageType
        {
            get
            {
                object obj = ViewState["MessageType"];
                return ((obj == null) ? NoticeMessageType.Error : (NoticeMessageType)obj);
            }
            set { ViewState["MessageType"] = value; }
        }

        /// <summary>
        /// Gets or sets the message to display.
        /// </summary>
        [Category("Appearance")]
        [Description("The message to display.")]
        [DefaultValue("")]
        public string Message
        {
            get
            {
                object obj = ViewState["Message"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["Message"] = value; }
        }

        /// <summary>
        /// Gets or sets the message description to display.
        /// </summary>
        [Category("Appearance")]
        [Description("The message description to display.")]
        [DefaultValue("")]
        public string Description
        {
            get
            {
                object obj = ViewState["Description"];
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["Description"] = value; }
        }

        /// <summary>
        /// Gets or sets the size of control.
        /// </summary>
        [Category("Appearance")]
        [Description("The size of the control.")]
        [DefaultValue(NoticeMessageBoxSize.Normal)]
        public NoticeMessageBoxSize Size
        {
            get
            {
                object obj = ViewState["Size"];
                return ((obj == null) ? NoticeMessageBoxSize.Normal : (NoticeMessageBoxSize)obj);
            }
            set { ViewState["Size"] = value; }
        }

        /// <summary>
        /// Gets or sets the width of the control.
        /// </summary>
        [Category("Layout")]
        [Description("The width of the control.")]
        [DefaultValue(typeof(Unit), "")]
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
        /// Gets or sets the horizontal alignment of the message in the control.
        /// </summary>
        [Category("Layout")]
        [Description("The horizontal alignment of the message in the control.")]
        [DefaultValue(HorizontalAlign.NotSet)]
        public HorizontalAlign HorizontalAlign
        {
            get
            {
                object obj = ViewState["HorizontalAlign"];
                return ((obj == null) ? HorizontalAlign.NotSet : (HorizontalAlign)obj);
            }
            set { ViewState["HorizontalAlign"] = value; }
        }

        #endregion

        #region Private Properties

        private Micajah.Common.Pages.MasterPage MasterPage
        {
            get
            {
                if (m_MasterPage == null)
                    m_MasterPage = Micajah.Common.Pages.MasterPage.GetMasterPage(Page);
                return m_MasterPage;
            }
        }

        #endregion

        #region Overriden Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ResourceProvider.RegisterStyleSheetResource(this, ResourceProvider.NoticeMessageBoxStyleSheet, "NoticeMessageBoxStyleSheet", true);

            if (this.MasterPage != null)
                m_MasterPage.EnableJQuery = true;
            else
                ScriptManager.RegisterClientScriptInclude(this.Page, this.Page.GetType(), "JQueryScript", ResourceProvider.JQueryScriptUrl);
        }

        /// <summary>
        /// Renders control.
        /// </summary>
        /// <param name="writer">The System.Web.UI.HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (writer == null) return;

            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Notification " + this.MessageType.ToString() + " " + this.Size.ToString());
            if (!Width.IsEmpty)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, Width.ToString());
            if (this.HorizontalAlign != HorizontalAlign.NotSet)
                writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, this.HorizontalAlign.ToString().ToLowerInvariant());
            writer.RenderBeginTag(HtmlTextWriterTag.Div); // Div

            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Close");
            writer.AddAttribute(HtmlTextWriterAttribute.Title, Resources.NoticeMessageBox_CloseLink_ToolTip);
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "$('#" + this.ClientID + "').fadeTo(400, 0, function () { $(this).slideUp(400); }); return false;");
            writer.RenderBeginTag(HtmlTextWriterTag.A); // A
            writer.Write(HtmlTextWriter.SpaceChar);
            writer.RenderEndTag(); // A

            writer.RenderBeginTag(HtmlTextWriterTag.Div); // Div

            if (this.Size == NoticeMessageBoxSize.Normal)
            {
                if (string.IsNullOrEmpty(this.Description))
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "NoDescr");
                writer.RenderBeginTag(HtmlTextWriterTag.H4); // H4
                writer.RenderBeginTag(HtmlTextWriterTag.Span); // Span
                switch (this.MessageType)
                {
                    case NoticeMessageType.Error:
                        writer.Write(Resources.NoticeMessageBox_ErrorTitle);
                        break;
                    case NoticeMessageType.Success:
                        writer.Write(Resources.NoticeMessageBox_SuccessTitle);
                        break;
                    case NoticeMessageType.Warning:
                        writer.Write(Resources.NoticeMessageBox_WarningTitle);
                        break;
                    case NoticeMessageType.Information:
                        writer.Write(Resources.NoticeMessageBox_InformationTitle);
                        break;
                }
                writer.RenderEndTag(); // Span
                if (!string.IsNullOrEmpty(this.Message))
                    writer.Write(HtmlTextWriter.SpaceChar);
            }
            else
                writer.RenderBeginTag(HtmlTextWriterTag.Strong); // Strong
            if (!string.IsNullOrEmpty(this.Message))
                writer.Write(this.Message);
            else if (this.Size == NoticeMessageBoxSize.Small)
            {
                if (string.IsNullOrEmpty(this.Description))
                    writer.Write("&nbsp;");
            }
            writer.RenderEndTag(); // H4 or Strong

            if (!string.IsNullOrEmpty(this.Description))
            {
                if (this.Size == NoticeMessageBoxSize.Normal)
                    writer.RenderBeginTag(HtmlTextWriterTag.P); // P
                else
                    writer.Write(HtmlTextWriter.SpaceChar);
                writer.Write(this.Description);
                if (this.Size == NoticeMessageBoxSize.Normal)
                    writer.RenderEndTag(); // P
            }

            writer.RenderEndTag(); // Div

            writer.RenderEndTag(); // Div
        }

        #endregion

        #region Public Methods

        public string RenderControl()
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture))
            {
                HtmlTextWriter w = new HtmlTextWriter(sw);
                this.Render(w);
                return sb.ToString();
            }
        }

        #endregion

    }
}
