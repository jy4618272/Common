using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.WebControls.AdminControls;

namespace Micajah.Common.WebControls.AdminControls
{
    /// <summary>
    /// Displays a Custom urls control in a Web Forms page.
    /// </summary>
    [ParseChildren(true)]
    public class CustomUrls : Control, INamingContainer
    {
        #region Members

        private CustomUrlsControl m_InnerControl;
        private PlaceHolder m_Holder;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the inner control.
        /// </summary>
        [Browsable(false)]
        public CustomUrlsControl InnerControl
        {
            get
            {
                if (m_InnerControl == null)
                    m_InnerControl = this.FindControl("CustomUrls") as CustomUrlsControl;
                return m_InnerControl;
            }
        }

        #endregion

        #region Overriden Methods

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            m_Holder = new PlaceHolder();
            m_Holder.ID = "Holder";
            this.Controls.Add(m_Holder);

            m_InnerControl = this.Page.LoadControl(ResourceProvider.CustomUrlsControlVirtualPath) as CustomUrlsControl;
            m_InnerControl.ID = "CustomUrls";
            m_Holder.Controls.Add(m_InnerControl);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (writer == null) return;

            if (this.DesignMode)
                writer.Write(this.ID);
            else
                base.Render(writer);
        }

        #endregion
    }
}
