using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Displays a header in a Web Forms page.
    /// </summary>
    [ToolboxItem(false)]
    internal sealed class Header : Control
    {
        #region Members

        internal const string SearchTextBoxId = "SearchTextBox";
        internal const string SearchButtonId = "SearchButton";

        private Micajah.Common.Pages.MasterPage m_MasterPage;
        private UserContext m_UserContext;
        private IList m_ActionIdList;
        private bool m_IsFrameworkAdmin;
        private bool m_IsAuthenticated;
        private bool m_ModernTheme;
        private MasterPageElement m_MasterPageSettings;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Header(Micajah.Common.Pages.MasterPage masterPage, IList actionIdList, bool isFrameworkAdmin, bool isAuthenticated, UserContext user)
        {
            m_MasterPage = masterPage;
            m_ActionIdList = actionIdList;
            m_IsFrameworkAdmin = isFrameworkAdmin;
            m_IsAuthenticated = isAuthenticated;
            m_UserContext = user;
            m_MasterPageSettings = FrameworkConfiguration.Current.WebApplication.MasterPage;
            m_ModernTheme = (m_MasterPageSettings.Theme == MasterPageTheme.Modern);
        }

        #endregion

        #region Private Methods

        private Control CreateHeaderLinks()
        {
            if (!(m_MasterPage.VisibleHeaderLinks || m_MasterPage.VisibleSearchControl))
                return null;

            HtmlGenericControl rightContainer = null;
            Control links = null;
            HyperLink link = null;
            HtmlGenericControl div = null;
            HtmlGenericControl li = null;

            try
            {
                rightContainer = new HtmlGenericControl("div");
                rightContainer.Attributes["class"] = "G";

                if (m_MasterPage.VisibleHeaderLinks)
                    links = CreateGlobalNavigation(this.Page.Request.IsSecureConnection);

                if (m_ModernTheme)
                {
                    HtmlGenericControl ul = (HtmlGenericControl)links;

                    if (m_MasterPage.VisibleHelpLink)
                    {
                        link = new HyperLink();
                        link.ToolTip = Resources.MasterPage_HelpLink_Text2;
                        link.ImageUrl = ResourceProvider.GetResourceUrl(ResourceProvider.GetMasterPageThemeColorResource(MasterPageTheme.Modern, MasterPageThemeColor.NotSet, "Help.png"), true);
                        link.Attributes["onclick"] = m_MasterPage.HelpLinkOnClick;

                        li = new HtmlGenericControl("li");
                        li.Controls.Add(link);
                        ul.Controls.Add(li);
                    }

                    rightContainer.Controls.Add(links);
                }
                else
                {
                    string paddindTop = null;

                    if (links != null)
                    {
                        div = new HtmlGenericControl("div");
                        div.Style["clear"] = "both";
                        div.Controls.Add(links);
                        rightContainer.Controls.Add(div);

                        paddindTop = "8px";
                    }

                    if (m_MasterPage.VisibleSearchControl)
                        rightContainer.Controls.Add(CreateSearchControl(paddindTop));
                }

                return rightContainer;
            }
            finally
            {
                if (rightContainer != null) rightContainer.Dispose();
                if (links != null) links.Dispose();
                if (div != null) div.Dispose();
                if (link != null) link.Dispose();
                if (li != null) li.Dispose();
            }
        }

        private Control CreateApplicationLogo()
        {
            if (!m_ModernTheme)
                return null;

            if (string.IsNullOrEmpty(m_MasterPageSettings.Header.LogoImageUrl))
                return null;

            HtmlGenericControl div = null;
            HtmlGenericControl ul = null;
            HtmlGenericControl li = null;
            HyperLink link = null;

            try
            {
                div = new HtmlGenericControl("div");
                div.Attributes["class"] = "Al";

                ul = new HtmlGenericControl("ul");
                ul.Attributes["class"] = "Mm";
                div.Controls.Add(ul);

                li = new HtmlGenericControl("li");
                ul.Controls.Add(li);

                link = new HyperLink();
                link.ImageUrl = m_MasterPageSettings.Header.LogoImageUrl;
                link.NavigateUrl = m_MasterPage.HeaderLogoNavigateUrl;
                li.Controls.Add(link);

                return div;
            }
            finally
            {
                if (div != null) div.Dispose();
                if (ul != null) ul.Dispose();
                if (li != null) li.Dispose();
                if (link != null) link.Dispose();
            }
        }

        private Control CreateLogo()
        {
            if ((!m_MasterPage.VisibleHeaderLogo) || string.IsNullOrEmpty(m_MasterPageSettings.Header.LogoImageUrl))
                return null;

            string text = m_MasterPage.HeaderLogoText;
            string imageUrl = null;

            if (!m_ModernTheme)
                imageUrl = m_MasterPage.HeaderLogoImageUrl;

            if (string.IsNullOrEmpty(imageUrl) && string.IsNullOrEmpty(text))
                return null;

            HtmlGenericControl logoContainer = null;
            HyperLink link = null;
            Image image = null;

            try
            {
                string navigateUrl = m_MasterPage.HeaderLogoNavigateUrl;
                logoContainer = new HtmlGenericControl("div");
                logoContainer.Attributes["class"] = "A";

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    if (!string.IsNullOrEmpty(navigateUrl))
                    {
                        link = new HyperLink();
                        if (!string.IsNullOrEmpty(m_MasterPage.HeaderLogoTarget))
                            link.Target = m_MasterPage.HeaderLogoTarget;
                        link.NavigateUrl = navigateUrl;
                        link.ImageUrl = imageUrl;
                        link.ToolTip = text;
                        logoContainer.Controls.Add(link);
                    }
                    else
                    {
                        image = new Image();
                        image.ImageAlign = ImageAlign.AbsMiddle;
                        image.ImageUrl = imageUrl;
                        image.ToolTip = text;
                        logoContainer.Controls.Add(image);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(navigateUrl))
                    {
                        link = new HyperLink();
                        if (!string.IsNullOrEmpty(m_MasterPage.HeaderLogoTarget))
                            link.Target = m_MasterPage.HeaderLogoTarget;
                        link.NavigateUrl = navigateUrl;
                        link.ToolTip = text;
                        link.Text = text;
                        logoContainer.Controls.Add(link);
                    }
                    else
                    {
                        logoContainer.InnerHtml = text;
                    }
                }

                return logoContainer;
            }
            finally
            {
                if (logoContainer != null) logoContainer.Dispose();
                if (link != null) link.Dispose();
                if (image != null) image.Dispose();
            }
        }

        private Control CreateSearchControl(string paddindTop)
        {
            HtmlGenericControl div = null;
            System.Web.UI.WebControls.TextBox txt = null;
            Button btn = null;

            try
            {
                div = new HtmlGenericControl("div");

                string searchButtonOnClientClick = string.Format(CultureInfo.InvariantCulture
                    , "if (Mp_Search('{0}{1}{2}')) {3}; return false;"
                    , m_MasterPage.ClientID, this.ClientIDSeparator, SearchTextBoxId
                    , this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(m_MasterPage, SearchButtonId, string.Empty, false, false, false, true, false, "Mp_Search")));

                txt = new System.Web.UI.WebControls.TextBox();
                txt.ID = SearchTextBoxId;
                txt.Columns = m_MasterPage.SearchTextBoxColumns;
                if (m_MasterPage.SearchTextBoxMaxLength > 0) txt.MaxLength = m_MasterPage.SearchTextBoxMaxLength;
                txt.Style[HtmlTextWriterStyle.VerticalAlign] = "middle";
                txt.CausesValidation = false;
                txt.ValidationGroup = "Mp_Search";
                txt.Attributes["onfocus"] = "Mp_SearchTextBox_OnFocus(this);";
                txt.Attributes["onblur"] = "Mp_SearchTextBox_OnBlur(this);";
                txt.Attributes["onkeypress"] = "if (event.keyCode == 13) {" + searchButtonOnClientClick + "}";
                txt.Attributes["EmptyText"] = HttpUtility.HtmlAttributeEncode(m_MasterPage.SearchTextBoxEmptyText);

                string searchText = m_MasterPage.SearchText;
                if (searchText == null)
                {
                    txt.Text = HttpUtility.HtmlAttributeEncode(m_MasterPage.SearchTextBoxEmptyText);
                    txt.Style[HtmlTextWriterStyle.Color] = "Gray";
                }
                else
                {
                    txt.Text = searchText;
                    txt.Style[HtmlTextWriterStyle.Color] = "Black";
                }

                div.Controls.Add(txt);
                div.Controls.Add(new LiteralControl("&nbsp;"));

                btn = new Button();
                btn.ID = SearchButtonId;
                btn.OnClientClick = searchButtonOnClientClick;
                btn.CausesValidation = false;
                btn.ValidationGroup = "Mp_Search";
                btn.Text = m_MasterPage.SearchButtonText;
                btn.ToolTip = m_MasterPage.SearchButtonToolTip;

                if (m_ModernTheme)
                {
                    div.Attributes["class"] = "S";
                    btn.CssClass = "Green";
                }
                else
                {
                    div.Style["clear"] = "both";
                    div.Style[HtmlTextWriterStyle.WhiteSpace] = "nowrap";
                    if (!string.IsNullOrEmpty(paddindTop)) div.Style[HtmlTextWriterStyle.PaddingTop] = paddindTop;

                    btn.Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
                }

                div.Controls.Add(btn);

                return div;
            }
            finally
            {
                if (div != null) div.Dispose();
                if (txt != null) txt.Dispose();
                if (btn != null) btn.Dispose();
            }
        }

        /// <summary>
        /// Creates the global navigation links.
        /// </summary>
        /// <param name="isSecureConnection">The flag indicating whether the HTTP connection uses secure sockets.</param>
        /// <returns>The control that represents the global navigation links.</returns>
        private Control CreateGlobalNavigation(bool isSecureConnection)
        {
            ControlList links = null;
            Link link = null;
            Link link2 = null;
            HtmlGenericControl ul = null;
            HtmlGenericControl ul2 = null;
            HtmlGenericControl li = null;
            HtmlGenericControl li2 = null;
            bool modernTheme = (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern);

            if (modernTheme)
            {
                ul = new HtmlGenericControl("ul");
                ul.Attributes["class"] = "Mm";
            }
            else
                links = new ControlList();

            try
            {
                foreach (Micajah.Common.Bll.Action item in ActionProvider.GlobalNavigationLinks.FindByActionId(ActionProvider.GlobalNavigationLinksActionId).GetAvailableChildActions(m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated))
                {
                    link = new Link(item.CustomName, item.CustomAbsoluteNavigateUrl, item.Description);

                    if (modernTheme)
                    {
                        li = new HtmlGenericControl("li");

                        if (item.ActionId == ActionProvider.MyAccountMenuGlobalNavigationLinkActionId)
                        {
                            HyperLink usernameLink = new HyperLink();
                            li.Controls.Add(usernameLink);

                            Image avatarImg = new Image();
                            avatarImg.CssClass = "Avtr";
                            avatarImg.ImageUrl = string.Format(CultureInfo.InvariantCulture, "{0}{1}www.gravatar.com/avatar/{2}?s=24"
                                , (isSecureConnection ? Uri.UriSchemeHttps : Uri.UriSchemeHttp), Uri.SchemeDelimiter, Support.CalculateMD5Hash(m_UserContext.Email.ToLowerInvariant()));
                            usernameLink.Controls.Add(avatarImg);

                            string name = m_UserContext.FirstName + Html32TextWriter.SpaceChar + m_UserContext.LastName;

                            LiteralControl literal = new LiteralControl();
                            literal.Text = ((name.Trim().Length > 0) ? name : m_UserContext.LoginName);
                            usernameLink.Controls.Add(literal);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(item.IconUrl))
                                link.ImageUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(item.IconUrl);
                            li.Controls.Add(link);
                        }

                        ul.Controls.Add(li);

                        ActionCollection childActions = item.GetAvailableChildActions(m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated);
                        if (childActions.Count > 0)
                        {
                            li.Attributes["class"] = "Dm";

                            ul2 = new HtmlGenericControl("ul");
                            ul2.Attributes["class"] = "Sm";

                            foreach (Micajah.Common.Bll.Action item2 in childActions)
                            {
                                li2 = new HtmlGenericControl("li");
                                link2 = new Link(item2.CustomName, item2.CustomAbsoluteNavigateUrl, item2.Description);
                                li2.Controls.Add(link2);
                                ul2.Controls.Add(li2);
                            }

                            li.Controls.Add(ul2);
                        }
                    }
                    else
                    {
                        if (item.ActionId == ActionProvider.MyAccountMenuGlobalNavigationLinkActionId)
                        {
                            foreach (Micajah.Common.Bll.Action item2 in item.GetAvailableChildActions(m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated))
                            {
                                link2 = new Link(item2.CustomName, item2.CustomAbsoluteNavigateUrl, item2.Description);
                                links.Add(link2);
                            }
                        }
                        else
                            links.Add(link);
                    }
                }

                if (modernTheme)
                    return ul;
                else
                    return ((links.Count > 0) ? links : null);
            }
            finally
            {
                if (ul2 != null) ul2.Dispose();
                if (ul != null) ul.Dispose();
                if (link != null) link.Dispose();
                if (links != null) links.Dispose();
            }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Creates child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            HtmlGenericControl headerContainer = null;
            Control ctrl = null;

            try
            {
                headerContainer = new HtmlGenericControl("div");
                headerContainer.Attributes["class"] = "Mp_Hdr";

                ctrl = this.CreateApplicationLogo();
                if (ctrl != null)
                    headerContainer.Controls.Add(ctrl);

                ctrl = this.CreateLogo();
                if (ctrl != null)
                    headerContainer.Controls.Add(ctrl);

                if (m_ModernTheme)
                {
                    if (m_MasterPage.VisibleSearchControl)
                        headerContainer.Controls.Add(CreateSearchControl(null));
                }

                ctrl = this.CreateHeaderLinks();
                if (ctrl != null)
                    headerContainer.Controls.Add(ctrl);

                if (headerContainer.HasControls())
                    this.Controls.Add(headerContainer);
            }
            finally
            {
                if (headerContainer != null) headerContainer.Dispose();
                if (ctrl != null) ctrl.Dispose();
            }
        }

        #endregion
    }
}