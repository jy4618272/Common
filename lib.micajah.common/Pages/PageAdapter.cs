using System;

namespace Micajah.Common.Pages
{
    /// <summary>
    /// Adapts a Web page for a specific browser.
    /// </summary>
    public sealed class PageAdapter : System.Web.UI.Adapters.PageAdapter
    {
        #region Overriden Methods

        /// <summary>
        /// Returns an object that is used by the Web page to maintain the control and view states.
        /// </summary>
        /// <returns>
        /// An Micajah.Common.Pages.SqlServerPageStatePersister object that supports creating
        /// and extracting the combined control and view states for the System.Web.UI.Page.
        /// </returns>
        public override System.Web.UI.PageStatePersister GetStatePersister()
        {
            return new PageStatePersister(Page);
        }

        #endregion
    }
}
