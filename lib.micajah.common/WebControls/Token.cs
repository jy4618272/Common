using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.WebControls.SecurityControls;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Displays the token control in a Web Forms page.
    /// </summary>
    [ParseChildren(true)]
    public class Token : Control, INamingContainer
    {
        #region Members

        private TokenControl m_InnerControl;
        private PlaceHolder m_Holder;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the inner control.
        /// </summary>
        [Browsable(false)]
        public TokenControl InnerControl
        {
            get
            {
                if (m_InnerControl == null)
                    m_InnerControl = this.FindControl("Settings") as TokenControl;
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

            m_InnerControl = this.Page.LoadControl(ResourceProvider.TokenControlVirtualPath) as TokenControl;
            m_InnerControl.ID = "Token";
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
