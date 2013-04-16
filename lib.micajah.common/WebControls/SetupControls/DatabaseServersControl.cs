using System;
using System.Globalization;
using Micajah.Common.Application;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;

namespace Micajah.Common.WebControls.SetupControls
{
    /// <summary>
    /// The control to manage SQL-servers.
    /// </summary>
    public class DatabaseServersControl : BaseControl
    {
        #region Overriden Methods

        protected override void OnLoad(EventArgs e)
        {
            if (WebApplication.CommonDataSet.Website.Rows.Count == 0)
            {
                List.EmptyDataText = string.Format(CultureInfo.CurrentCulture
                    , Resources.DatabaseServersControl_ErrorMessage_NoWebsite
                    , CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.WebsitesPageVirtualPath));
                List.ShowAddLink = EditForm.Visible = false;
            }
            base.OnLoad(e);
        }

        #endregion
    }
}
