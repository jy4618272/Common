using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web.UI.HtmlControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls.SetupControls;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Displays a footer in a Web Forms page.
    /// </summary>
    [ToolboxItem(false)]
    internal sealed class Footer : System.Web.UI.Control
    {
        #region Members

        private Micajah.Common.Pages.MasterPage m_MasterPage;

        #endregion

        #region Private Properties

        private static string CompanyName
        {
            get
            {
                string companyName = null;
                if (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.Copyright.CompanyName))
                {
                    if (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.Copyright.CompanyWebsiteUrl))
                        companyName = BaseControl.GetHyperlink(FrameworkConfiguration.Current.WebApplication.Copyright.CompanyWebsiteUrl, FrameworkConfiguration.Current.WebApplication.Copyright.CompanyName.TrimEnd('.'));
                    else
                        companyName = string.Format(CultureInfo.InvariantCulture, " {0}", FrameworkConfiguration.Current.WebApplication.Copyright.CompanyName.TrimEnd('.'));
                }
                return companyName;
            }
        }

        #region Private Properties

        private Micajah.Common.Pages.MasterPage MasterPage
        {
            get
            {
                if (m_MasterPage == null) m_MasterPage = (this.Page.Master as Micajah.Common.Pages.MasterPage);
                return m_MasterPage;
            }
        }

        #endregion

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
            HtmlGenericControl ul1 = null;
            HtmlGenericControl ul2 = null;
            HtmlGenericControl li = null;

            try
            {
                copyrightDiv = new HtmlGenericControl("div");
                footerDiv = new HtmlGenericControl("div");

                if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern)
                {
                    UserContext user = UserContext.Current;
                    if (user != null)
                    {
                        leftDiv = new HtmlGenericControl("div");
                        ul1 = new HtmlGenericControl("ul");

                        if (user.SelectedOrganization != null)
                        {
                            li = new HtmlGenericControl("li");
                            li.InnerHtml = user.SelectedOrganization.Name;
                            ul1.Controls.Add(li);
                        }

                        if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances && (user.SelectedInstance != null))
                        {
                            li = new HtmlGenericControl("li");
                            li.InnerHtml = "|";
                            ul1.Controls.Add(li);

                            li = new HtmlGenericControl("li");
                            li.InnerHtml = user.SelectedInstance.Name;
                            ul1.Controls.Add(li);
                        }

                        string roleName = RoleProvider.GetRoleName(user.RoleId);
                        if (!string.IsNullOrEmpty(roleName))
                        {
                            li = new HtmlGenericControl("li");
                            li.InnerHtml = "|";
                            ul1.Controls.Add(li);

                            li = new HtmlGenericControl("li");
                            li.InnerHtml = roleName;
                            ul1.Controls.Add(li);
                        }

                        leftDiv.Controls.Add(ul1);
                    }

                    if (leftDiv != null) leftDiv.Attributes["class"] = "L";
                    copyrightDiv.Attributes["class"] = "R";

                    ul2 = new HtmlGenericControl("ul");

                    if (this.MasterPage.VisibleApplicationLogo)
                    {
                        li = new HtmlGenericControl("li");
                        li.Controls.Add(MasterPage.ApplicationLogo);
                        ul2.Controls.Add(li);
                    }

                    li = new HtmlGenericControl("li");
                    li.InnerHtml = string.Format(CultureInfo.InvariantCulture, Resources.Footer_CopyrightInformationFormatString, DateTime.Today.Year, CompanyName);
                    ul2.Controls.Add(li);

                    if (FrameworkConfiguration.Current.WebApplication.MasterPage.Footer.VisibleEngineeredBy)
                    {
                        li = new HtmlGenericControl("li");
                        li.Attributes["class"] = "Cpy";
                        li.InnerHtml = string.Format(CultureInfo.InvariantCulture, Resources.Footer_EngineeredByFormatString, BaseControl.GetHyperlink("http://www.micajah.com", Resources.Footer_MicajahCompanyName));
                        ul2.Controls.Add(li);
                    }

                    copyrightDiv.Controls.Add(ul2);

                    if (leftDiv != null) footerDiv.Controls.Add(leftDiv);
                    footerDiv.Controls.Add(copyrightDiv);
                }
                else
                {
                    UserContext user = UserContext.Current;
                    if (user != null)
                    {
                        leftDiv = new HtmlGenericControl("div");
                        StringBuilder sb = new StringBuilder();

                        if (user.SelectedOrganization != null)
                            sb.Append(user.SelectedOrganization.Name);

                        if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances && (user.SelectedInstance != null))
                        {
                            if (sb.Length > 0) sb.Append("<br />");
                            sb.Append(user.SelectedInstance.Name);
                        }

                        string roleName = RoleProvider.GetRoleName(user.RoleId);
                        if (!string.IsNullOrEmpty(roleName))
                        {
                            if (sb.Length > 0) sb.Append("<br />");
                            sb.Append(roleName);
                        }

                        leftDiv.Attributes["class"] = "L";
                        leftDiv.InnerHtml = sb.ToString();
                    }

                    copyrightDiv.InnerHtml = string.Format(CultureInfo.InvariantCulture, Resources.Footer_CopyrightInformationFormatString, DateTime.Today.Year, CompanyName);
                    if (FrameworkConfiguration.Current.WebApplication.MasterPage.Footer.VisibleEngineeredBy)
                        copyrightDiv.InnerHtml += "<br />" + string.Format(CultureInfo.InvariantCulture, Resources.Footer_EngineeredByFormatString, BaseControl.GetHyperlink("http://www.micajah.com", Resources.Footer_MicajahCompanyName));

                    if (leftDiv != null) footerDiv.Controls.Add(leftDiv);
                    footerDiv.Controls.Add(copyrightDiv);
                }

                footerDiv.Attributes["class"] = "Mp_Ftr";

                this.Controls.Add(footerDiv);
            }
            finally
            {
                if (li != null) li.Dispose();
                if (ul1 != null) ul1.Dispose();
                if (ul2 != null) ul2.Dispose();
                if (leftDiv != null) leftDiv.Dispose();
                if (copyrightDiv != null) copyrightDiv.Dispose();
                if (footerDiv != null) footerDiv.Dispose();
            }
        }

        #endregion
    }
}