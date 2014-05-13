using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web.UI.HtmlControls;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Displays a footer in a Web Forms page.
    /// </summary>
    [ToolboxItem(false)]
    internal sealed class Footer : System.Web.UI.Control
    {
        #region Members

        private UserContext m_UserContext;

        #endregion

        #region Constructors

        public Footer(UserContext user)
        {
            m_UserContext = user;
        }

        #endregion

        #region Private Methods

        private static string GetCompanyName(CopyrightElement copyrightSettings)
        {
            string companyName = null;
            if (!string.IsNullOrEmpty(copyrightSettings.CompanyName))
            {
                if (!string.IsNullOrEmpty(copyrightSettings.CompanyWebsiteUrl))
                    companyName = BaseControl.GetHyperlink(copyrightSettings.CompanyWebsiteUrl, copyrightSettings.CompanyName.TrimEnd('.'));
                else
                    companyName = string.Format(CultureInfo.InvariantCulture, " {0}", copyrightSettings.CompanyName.TrimEnd('.'));
            }
            return companyName;
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Creates child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            HtmlGenericControl footerDiv = null;
            HtmlGenericControl leftDiv = null;
            HtmlGenericControl copyrightDiv = null;

            try
            {
                copyrightDiv = new HtmlGenericControl("div");
                footerDiv = new HtmlGenericControl("div");

                WebApplicationElement webAppSettings = FrameworkConfiguration.Current.WebApplication;

                if (m_UserContext != null)
                {
                    leftDiv = new HtmlGenericControl("div");
                    StringBuilder sb = new StringBuilder();

                    if (m_UserContext.OrganizationId != Guid.Empty)
                        sb.Append(m_UserContext.Organization.Name);

                    if (webAppSettings.EnableMultipleInstances && (m_UserContext.InstanceId != Guid.Empty))
                    {
                        if (sb.Length > 0) sb.Append("<br />");
                        sb.Append(m_UserContext.Instance.Name);
                    }

                    string roleName = RoleProvider.GetRoleName(m_UserContext.RoleId);
                    if (!string.IsNullOrEmpty(roleName))
                    {
                        if (sb.Length > 0) sb.Append("<br />");
                        sb.Append(roleName);
                    }

                    leftDiv.Attributes["class"] = "L";
                    leftDiv.InnerHtml = sb.ToString();
                }

                copyrightDiv.InnerHtml = string.Format(CultureInfo.InvariantCulture, Resources.Footer_CopyrightInformationFormatString, DateTime.UtcNow.Year, GetCompanyName(webAppSettings.Copyright));
                if (webAppSettings.MasterPage.Footer.VisibleEngineeredBy)
                    copyrightDiv.InnerHtml += "<br />" + string.Format(CultureInfo.InvariantCulture, Resources.Footer_EngineeredByFormatString, BaseControl.GetHyperlink("http://www.micajah.com", Resources.Footer_MicajahCompanyName));

                if (leftDiv != null) footerDiv.Controls.Add(leftDiv);
                footerDiv.Controls.Add(copyrightDiv);

                footerDiv.Attributes["class"] = "Mp_Ftr";

                this.Controls.Add(footerDiv);
            }
            finally
            {
                if (leftDiv != null) leftDiv.Dispose();
                if (copyrightDiv != null) copyrightDiv.Dispose();
                if (footerDiv != null) footerDiv.Dispose();
            }
        }

        #endregion
    }
}