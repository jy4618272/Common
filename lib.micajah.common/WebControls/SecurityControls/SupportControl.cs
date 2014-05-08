using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;

namespace Micajah.Common.WebControls.SecurityControls
{
    public class SupportControl : UserControl
    {
        #region Members

        protected PlaceHolder DescriptionHolder;
        protected Label DescriptionLabel;
        protected HtmlTable FormTable;
        protected Label TitleLabel;
        protected Label EmailLabel;
        protected HyperLink EmailLink;
        protected Label PhoneLabel;
        protected Literal PhoneValueLabel;
        protected HyperLink ReturnBackLink;

        #endregion

        #region Private Properties

        private Guid OrganizationId
        {
            get
            {
                object obj = Support.ConvertStringToType(Request.QueryString["o"], typeof(Guid));
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
        }

        #endregion

        #region Private Methods

        private void LoadResources()
        {
            Organization org = OrganizationProvider.GetOrganization(this.OrganizationId);
            if (org != null)
            {
                if (org.Expired)
                {
                    DescriptionLabel.Text = string.Format(CultureInfo.InvariantCulture, Resources.SupportControl_OrganizationHasExpiredText, org.Name);
                    int days = org.GraceDaysRemaining;
                    if (days > 0)
                        DescriptionLabel.Text += "<br />" + string.Format(CultureInfo.InvariantCulture, Resources.SupportControl_GracePeriodText, days);
                    DescriptionHolder.Visible = true;
                }
            }

            string copyrightCompany = null;
            if (string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.Copyright.CompanyWebsiteUrl))
                copyrightCompany = FrameworkConfiguration.Current.WebApplication.Copyright.CompanyName;
            else
                copyrightCompany = Micajah.Common.WebControls.SetupControls.BaseControl.GetHyperlink(FrameworkConfiguration.Current.WebApplication.Copyright.CompanyWebsiteUrl, FrameworkConfiguration.Current.WebApplication.Copyright.CompanyName);
            TitleLabel.Text = string.Format(CultureInfo.InvariantCulture, Resources.SupportControl_TitleLabel_Text, copyrightCompany);
            EmailLabel.Text = Resources.SupportControl_EmailLabel_Text;
            PhoneLabel.Text = Resources.SupportControl_PhoneLabel_Text;

            EmailLink.Text = FrameworkConfiguration.Current.WebApplication.Support.Email;
            EmailLink.NavigateUrl = "mailto:" + EmailLink.Text;
            PhoneValueLabel.Text = FrameworkConfiguration.Current.WebApplication.Support.Phone;

            ReturnBackLink.Text = Resources.SupportControl_ReturnBackLink_Text;
            if (Request.UrlReferrer != null)
            {
                ReturnBackLink.NavigateUrl = Request.UrlReferrer.ToString();
                ReturnBackLink.Visible = true;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs when the page is being loaded.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.CreatePageHeader(this.Page, false);

            Micajah.Common.Pages.MasterPage.SetPageTitle(this.Page, ActionProvider.FindAction(CustomUrlProvider.CreateApplicationAbsoluteUrl(Request.Url.PathAndQuery)));

            this.LoadResources();

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
            {
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnModernStyleSheet, true)));

                MagicForm.ApplyStyle(FormTable);
            }
            else
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnStyleSheet, true)));
        }

        protected void LogOnPageButton_Click(object sender, EventArgs e)
        {
            LoginProvider.Current.SignOut();
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Renders the page.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            LogOnControl.RenderHeader(writer);
            base.Render(writer);
        }

        #endregion
    }
}
