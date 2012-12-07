using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using Micajah.Common.Security;

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

        #endregion

        #region Private Properties

        private Micajah.Common.Pages.MasterPage MasterPage
        {
            get
            {
                if (m_MasterPage == null)
                {
                    System.Web.UI.MasterPage master = this.Page.Master;
                    while (master != null)
                    {
                        if (master is Micajah.Common.Pages.MasterPage)
                        {
                            m_MasterPage = (master as Micajah.Common.Pages.MasterPage);
                            return m_MasterPage;
                        }
                        master = master.Master;
                    }
                }
                return m_MasterPage;
            }
        }

        #endregion

        #region Private Methods

        private Control CreateHeaderLinks()
        {
            if (!(this.MasterPage.VisibleHeaderLinks || this.MasterPage.VisibleSearchControl))
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

                if (MasterPage.VisibleHeaderLinks)
                    links = CreateGlobalNavigation(false, MasterPage.IsSetupPage, this.Page.Request.IsSecureConnection);

                if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern)
                {
                    HtmlGenericControl ul = (HtmlGenericControl)links;

                    if (this.MasterPage.VisibleHelpLink)
                    {
                        link = new HyperLink();
                        link.ToolTip = Resources.MasterPage_HelpLink_Text2;
                        link.ImageUrl = ResourceProvider.GetResourceUrl(ResourceProvider.GetMasterPageThemeColorResource(MasterPageTheme.Modern, MasterPageThemeColor.NotSet, "Help.png"), true);
                        link.Attributes["onclick"] = this.MasterPage.HelpLinkOnClick;

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

                    if (MasterPage.VisibleSearchControl)
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
            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != MasterPageTheme.Modern)
                return null;

            if (string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.MasterPage.Header.LogoImageUrl))
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
                link.ImageUrl = FrameworkConfiguration.Current.WebApplication.MasterPage.Header.LogoImageUrl;
                if (this.MasterPage.IsSetupPage)
                {
                    Micajah.Common.Bll.Action action = ActionProvider.FindAction(ActionProvider.SetupPageActionId);
                    if (action != null)
                        link.NavigateUrl = action.AbsoluteNavigateUrl;
                }
                else
                    link.NavigateUrl = this.MasterPage.HeaderLogoNavigateUrl;
                li.Controls.Add(link);

                return div;
            }
            finally
            {
                if (div != null) ul.Dispose();
                if (ul != null) ul.Dispose();
                if (li != null) li.Dispose();
                if (link != null) link.Dispose();
            }
        }

        private Control CreateLogo()
        {
            if ((!this.MasterPage.VisibleHeaderLogo) || string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.MasterPage.Header.LogoImageUrl))
                return null;

            string text = this.MasterPage.HeaderLogoText;
            string imageUrl = null;

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != MasterPageTheme.Modern)
                imageUrl = this.MasterPage.HeaderLogoImageUrl;

            if (string.IsNullOrEmpty(imageUrl) && string.IsNullOrEmpty(text))
                return null;

            HtmlGenericControl logoContainer = null;
            HyperLink link = null;
            Image image = null;

            try
            {
                string navigateUrl = this.MasterPage.HeaderLogoNavigateUrl;
                logoContainer = new HtmlGenericControl("div");
                logoContainer.Attributes["class"] = "A";

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    if (!string.IsNullOrEmpty(navigateUrl))
                    {
                        link = new HyperLink();
                        if (!string.IsNullOrEmpty(this.MasterPage.HeaderLogoTarget))
                            link.Target = this.MasterPage.HeaderLogoTarget;
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
                        if (!string.IsNullOrEmpty(this.MasterPage.HeaderLogoTarget))
                            link.Target = this.MasterPage.HeaderLogoTarget;
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
                    , this.MasterPage.ClientID, this.ClientIDSeparator, SearchTextBoxId
                    , this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(MasterPage, SearchButtonId, string.Empty, false, false, false, true, false, "Mp_Search")));

                txt = new System.Web.UI.WebControls.TextBox();
                txt.ID = SearchTextBoxId;
                txt.Columns = this.MasterPage.SearchTextBoxColumns;
                if (this.MasterPage.SearchTextBoxMaxLength > 0) txt.MaxLength = this.MasterPage.SearchTextBoxMaxLength;
                txt.Style[HtmlTextWriterStyle.VerticalAlign] = "middle";
                txt.CausesValidation = false;
                txt.ValidationGroup = "Mp_Search";
                txt.Attributes["onfocus"] = "Mp_SearchTextBox_OnFocus(this);";
                txt.Attributes["onblur"] = "Mp_SearchTextBox_OnBlur(this);";
                txt.Attributes["onkeypress"] = "if (event.keyCode == 13) {" + searchButtonOnClientClick + "}";
                txt.Attributes["EmptyText"] = HttpUtility.HtmlAttributeEncode(this.MasterPage.SearchTextBoxEmptyText);

                string searchText = this.MasterPage.SearchText;
                if (searchText == null)
                {
                    txt.Text = HttpUtility.HtmlAttributeEncode(this.MasterPage.SearchTextBoxEmptyText);
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
                btn.Text = this.MasterPage.SearchButtonText;
                btn.ToolTip = this.MasterPage.SearchButtonToolTip;

                if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern)
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

        private static bool ShowNonPublicAction(Micajah.Common.Bll.Action item, bool isFrameworkAdministrator, bool canLogOnAsUser, bool organizationIsSelected, bool builtInOnly, IList actionIdList
            , ref string customAbsoluteNavigateUrl, ref  string customDescription)
        {
            if (item.ActionId == ActionProvider.SetupGlobalNavigationLinkActionId)
            {
                if (!isFrameworkAdministrator)
                    return false;
            }
            else if (item.ActionId == ActionProvider.LoginAsUserGlobalNavigationLinkActionId)
            {
                if (!canLogOnAsUser)
                    return false;
            }
            else if (item.ActionId == ActionProvider.LogOffGlobalNavigationLinkActionId)
            {
                if (!organizationIsSelected)
                    return false;
            }
            else if (item.ActionId == ActionProvider.MyAccountGlobalNavigationLinkActionId)
            {
                if (!organizationIsSelected)
                {
                    if (builtInOnly)
                        return false;

                    customAbsoluteNavigateUrl = customDescription = string.Empty;
                }
            }
            else
            {
                if (builtInOnly)
                    return false;
                else
                {
                    if (!((actionIdList != null) && actionIdList.Contains(item.ActionId)))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Creates the global navigation links.
        /// </summary>
        /// <param name="builtInOnly">The flag specifying that the list contains the built-in global navigation links only.</param>
        /// <param name="activeActionIsSetupPage">The flag specifying that the active action is setup page.</param>
        /// <param name="isSecureConnection">The flag indicating whether the HTTP connection uses secure sockets.</param>
        /// <returns>The control that represents the global navigation links.</returns>
        private static Control CreateGlobalNavigation(bool builtInOnly, bool activeActionIsSetupPage, bool isSecureConnection)
        {
            UserContext user = UserContext.Current;
            IList actionIdList = null;
            bool isFrameworkAdministrator = false;
            bool isAuthenticated = false;
            bool canLogOnAsUser = false;

            if (user != null)
            {
                isFrameworkAdministrator = user.IsFrameworkAdministrator;
                canLogOnAsUser = user.CanLogOnAsUser;
                actionIdList = user.ActionIdList;
                isAuthenticated = true;
            }

            ControlList links = null;
            Link link = null;
            HtmlGenericControl ul = null;
            HtmlGenericControl ul2 = null;
            HtmlGenericControl li = null;

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern)
            {
                ul = new HtmlGenericControl("ul");
                ul.Attributes["class"] = "Mm";
            }
            else
                links = new ControlList();

            try
            {
                foreach (Micajah.Common.Bll.Action item in ActionProvider.GlobalNavigationLinks)
                {
                    bool isConfigActionInModerTheme = ((FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern) && (item.ActionId == ActionProvider.ConfigurationGlobalNavigationLinkActionId));
                    bool accessDenied = ((!item.Visible) || item.AccessDenied());

                    if ((!isConfigActionInModerTheme) && accessDenied)
                        continue;

                    string customAbsoluteNavigateUrl = null;
                    string customDescription = null;

                    if (isAuthenticated)
                    {
                        if (item.AuthenticationRequired)
                        {
                            accessDenied = (!ShowNonPublicAction(item, isFrameworkAdministrator, canLogOnAsUser, (user.SelectedOrganization != null), builtInOnly, actionIdList, ref customAbsoluteNavigateUrl, ref customDescription));
                            if (!isConfigActionInModerTheme)
                            {
                                if (accessDenied)
                                    continue;
                            }
                        }
                        else if (item.ActionId == ActionProvider.LoginGlobalNavigationLinkActionId)
                        {
                            if (user.SelectedOrganization == null)
                                customAbsoluteNavigateUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.ActiveOrganizationPageVirtualPath);
                            else
                                continue;
                        }
                        else if (builtInOnly)
                            continue;
                        else if (activeActionIsSetupPage)
                            continue;
                    }
                    else if (item.AuthenticationRequired)
                        continue;

                    if (!(isConfigActionInModerTheme && accessDenied))
                    {
                        link = new Link(item.CustomName
                            , ((customAbsoluteNavigateUrl == null) ? item.CustomAbsoluteNavigateUrl : customAbsoluteNavigateUrl)
                            , ((customDescription == null) ? item.Description : customDescription));
                    }

                    if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern)
                    {
                        if (item.ActionId == ActionProvider.ConfigurationGlobalNavigationLinkActionId)
                        {
                            li = new HtmlGenericControl("li");
                            li.Attributes["class"] = "Dm";

                            HyperLink usernameLink = new HyperLink();
                            li.Controls.Add(usernameLink);

                            Image avatarImg = new Image();
                            avatarImg.CssClass = "Avtr";
                            avatarImg.ImageUrl = string.Format(CultureInfo.InvariantCulture, "{0}{1}www.gravatar.com/avatar/{2}?s=24"
                                , (isSecureConnection ? Uri.UriSchemeHttps : Uri.UriSchemeHttp), Uri.SchemeDelimiter, Support.CalculateMD5Hash(user.Email.ToLowerInvariant()));
                            usernameLink.Controls.Add(avatarImg);

                            string name = user.FirstName + Html32TextWriter.SpaceChar + user.LastName;

                            LiteralControl literal = new LiteralControl();
                            literal.Text = ((name.Trim().Length > 0) ? name : user.LoginName);
                            usernameLink.Controls.Add(literal);

                            Image dropMenuImg = new Image();
                            dropMenuImg.ImageUrl = ResourceProvider.GetResourceUrl(ResourceProvider.GetMasterPageThemeColorResource(MasterPageTheme.Modern, MasterPageThemeColor.NotSet, "DropMenu.png"), true);
                            usernameLink.Controls.Add(dropMenuImg);

                            ul.Controls.Add(li);

                            ul2 = new HtmlGenericControl("ul");
                            ul2.Attributes["class"] = "Sm";
                        }

                        if (link != null)
                        {
                            li = new HtmlGenericControl("li");
                            li.Controls.Add(link);

                            if (ul2 != null)
                                ul2.Controls.Add(li);
                            else
                                ul.Controls.Add(li);
                        }
                    }
                    else
                        links.Add(link);
                }

                if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern)
                {
                    if (ul2 != null)
                    {
                        if (ul2.HasControls() && ul.HasControls())
                        {
                            li = (HtmlGenericControl)ul.Controls[ul.Controls.Count - 1];
                            li.Controls.Add(ul2);
                        }
                    }

                    return ul;
                }
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

                if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern)
                {
                    if (MasterPage.VisibleSearchControl)
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