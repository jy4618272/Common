using System.Web.UI;

namespace Micajah.Common.WebControls.SetupControls
{
    public abstract class MasterControl : UserControl
    {
        #region Members

        private Micajah.Common.Pages.MasterPage m_MasterPage;

        #endregion

        #region Protected Properties

        protected Micajah.Common.Pages.MasterPage MasterPage
        {
            get
            {
                if (m_MasterPage == null)
                    m_MasterPage = Micajah.Common.Pages.MasterPage.GetMasterPage(Page);
                return m_MasterPage;
            }
        }

        #endregion
    }
}
