using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
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
            {
                return null;
            }

            HtmlGenericControl rightContainer = null;
            Control links = null;
            HyperLink link = null;
            HtmlGenericControl div = null;
            HtmlGenericControl li = null;

            try
            {
                rightContainer = new HtmlGenericControl("div");

                if (m_MasterPage.VisibleHeaderLinks)
                {
                    links = CreateGlobalNavigation(this.Page.Request.IsSecureConnection);
                }

                if (m_ModernTheme)
                {
                    rightContainer.Controls.Add(links);
                }
                else
                {
                    rightContainer.Attributes["class"] = "G";

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
                    {
                        rightContainer.Controls.Add(CreateSearchControl(paddindTop));
                    }
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
            if (string.IsNullOrEmpty(m_MasterPageSettings.Header.LogoImageUrl))
                return null;

            HtmlGenericControl div = null;
            HtmlGenericControl ul = null;

            try
            {
                div = new HtmlGenericControl("div");
                div.Attributes["class"] = "Al";

                ul = new HtmlGenericControl("ul");
                div.Controls.Add(ul);

                Control li = CreateApplicationLogoListItem(null);
                ul.Controls.Add(li);

                return div;
            }
            finally
            {
                if (div != null) div.Dispose();
                if (ul != null) ul.Dispose();
            }
        }

        private Control CreateApplicationLogoListItem(string cssClass)
        {
            if (string.IsNullOrEmpty(m_MasterPageSettings.Header.LogoImageUrl))
                return null;

            HtmlGenericControl li = null;
            HyperLink link = null;

            try
            {
                li = new HtmlGenericControl("li");
                if (!string.IsNullOrEmpty(cssClass))
                {
                    li.Attributes["class"] = cssClass;
                }

                link = new HyperLink();
                link.ImageUrl = m_MasterPageSettings.Header.LogoImageUrl;
                link.NavigateUrl = m_MasterPage.HeaderLogoNavigateUrl;
                li.Controls.Add(link);

                return li;
            }
            finally
            {
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

            try
            {
                div = new HtmlGenericControl("div");

                string searchButtonOnClientClick = string.Format(CultureInfo.InvariantCulture
                    , "if (Mp_Search('{0}{1}{2}')) {3}; return false;"
                    , m_MasterPage.ClientID, this.ClientIDSeparator, SearchTextBoxId
                    , this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(m_MasterPage, SearchButtonId, string.Empty, false, false, false, true, false, "Mp_Search")));

                txt = new System.Web.UI.WebControls.TextBox();
                txt.ID = SearchTextBoxId;
                txt.Style[HtmlTextWriterStyle.VerticalAlign] = "middle";
                txt.CausesValidation = false;
                txt.ValidationGroup = "Mp_Search";
                txt.Attributes["onkeypress"] = "if (event.keyCode == 13) {" + searchButtonOnClientClick + "}";
                if (m_MasterPage.SearchTextBoxMaxLength > 0)
                {
                    txt.MaxLength = m_MasterPage.SearchTextBoxMaxLength;
                }

                if (m_ModernTheme)
                {
                    txt.CssClass = "form-control";
                    txt.Attributes["placeholder"] = HttpUtility.HtmlAttributeEncode(m_MasterPage.SearchTextBoxEmptyText);

                    string searchText = m_MasterPage.SearchText;
                    if (searchText != null)
                    {
                        txt.Text = searchText;
                    }

                    div.Attributes["class"] = "S";
                }
                else
                {
                    txt.Columns = m_MasterPage.SearchTextBoxColumns;
                    txt.Attributes["onfocus"] = "Mp_SearchTextBox_OnFocus(this);";
                    txt.Attributes["onblur"] = "Mp_SearchTextBox_OnBlur(this);";
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

                    div.Style["clear"] = "both";
                    div.Style[HtmlTextWriterStyle.WhiteSpace] = "nowrap";
                    if (!string.IsNullOrEmpty(paddindTop))
                    {
                        div.Style[HtmlTextWriterStyle.PaddingTop] = paddindTop;
                    }
                }

                div.Controls.Add(txt);
                div.Controls.Add(new LiteralControl("&nbsp;"));

                return div;
            }
            finally
            {
                if (div != null) div.Dispose();
                if (txt != null) txt.Dispose();
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
            HyperLink link = null;
            HyperLink link2 = null;
            HtmlGenericControl ul = null;
            HtmlGenericControl ul2 = null;
            HtmlGenericControl li = null;
            HtmlGenericControl li2 = null;

            try
            {
                if (m_ModernTheme)
                {
                    ul = new HtmlGenericControl("ul");
                    ul.Attributes["class"] = "nav pull-right";
                }
                else
                {
                    links = new ControlList();
                }

                Micajah.Common.Bll.ActionCollection items = ActionProvider.GlobalNavigationLinks.FindByActionId(ActionProvider.GlobalNavigationLinksActionId).GetAvailableChildActions(m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated);

                if (m_ModernTheme)
                {
                    li = (HtmlGenericControl)CreateApplicationLogoListItem("Al");

                    ul.Controls.Add(li);
                }
                else
                {
                    Micajah.Common.Bll.Action item = items.FindByActionId(ActionProvider.MyAccountMenuGlobalNavigationLinkActionId);

                    if (item != null)
                    {
                        Micajah.Common.Bll.ActionCollection items2 = item.GetAvailableChildActions(m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated);

                        items.AddRange(items2);

                        items.Sort();
                    }
                }

                foreach (Micajah.Common.Bll.Action item in items)
                {
                    link = new HyperLink();
                    link.NavigateUrl = item.CustomAbsoluteNavigateUrl;
                    link.ToolTip = item.Description;

                    if (m_ModernTheme)
                    {
                        li = new HtmlGenericControl("li");

                        string iconUrl = null;
                        string text = item.CustomName;
                        string cssClass = "Icon";

                        if (item.ActionId == ActionProvider.MyAccountMenuGlobalNavigationLinkActionId)
                        {
                            iconUrl = string.Format(CultureInfo.InvariantCulture, "{0}{1}www.gravatar.com/avatar/{2}?s=24"
                                , (isSecureConnection ? Uri.UriSchemeHttps : Uri.UriSchemeHttp), Uri.SchemeDelimiter, Support.CalculateMD5Hash(m_UserContext.Email.ToLowerInvariant()));

                            cssClass += " Avtr";
                        }
                        else
                        {
                            if (item.ActionId == ActionProvider.PageHelpGlobalNavigationLinkActionId)
                            {
                                if (!m_MasterPage.VisibleHelpLink)
                                {
                                    continue;
                                }

                                link.Attributes["onclick"] = m_MasterPage.HelpLinkOnClick;
                            }

                            if (!string.IsNullOrEmpty(item.IconUrl))
                            {
                                iconUrl = item.IconUrl;
                                if (iconUrl.IndexOf("glyphicon", StringComparison.OrdinalIgnoreCase) == -1)
                                {
                                    iconUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(iconUrl);
                                }
                                else
                                {
                                    using (Label label = new Label())
                                    {
                                        label.CssClass = "glyphicon " + iconUrl;

                                        if (string.IsNullOrEmpty(text))
                                        {
                                            label.CssClass += " no-margin";
                                        }

                                        link.Controls.Add(label);
                                    }

                                    iconUrl = null;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(iconUrl))
                        {
                            using (Image image = new Image())
                            {
                                image.ImageUrl = iconUrl;
                                image.CssClass = cssClass;

                                link.Controls.Add(image);
                            }
                        }

                        if (!string.IsNullOrEmpty(text))
                        {
                            using (LiteralControl literal = new LiteralControl(text))
                            {
                                link.Controls.Add(literal);
                            }
                        }

                        li.Controls.Add(link);
                        ul.Controls.Add(li);

                        ActionCollection childActions = item.GetAvailableChildActions(m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated);
                        if (childActions.Count > 0)
                        {
                            link.CssClass = "dropdown-toggle";
                            link.Attributes["data-toggle"] = "dropdown";

                            using (Label label = new Label())
                            {
                                label.CssClass = "caret";
                                link.Controls.Add(label);
                            }

                            ul2 = new HtmlGenericControl("ul");
                            ul2.Attributes["class"] = "dropdown-menu blue";

                            foreach (Micajah.Common.Bll.Action item2 in childActions)
                            {
                                li2 = new HtmlGenericControl("li");

                                link2 = new HyperLink();
                                link2.Text = item2.CustomName;
                                link2.NavigateUrl = item2.CustomAbsoluteNavigateUrl;
                                link2.ToolTip = item2.Description;

                                li2.Controls.Add(link2);
                                ul2.Controls.Add(li2);
                            }

                            li.Attributes["class"] = "dropdown";
                            li.Controls.Add(ul2);
                        }
                    }
                    else
                    {
                        if (item.ActionId == ActionProvider.MyAccountMenuGlobalNavigationLinkActionId)
                        {
                            if ((m_UserContext != null) && (m_UserContext.OrganizationId == Guid.Empty))
                            {
                                continue;
                            }
                        }

                        link.Text = item.CustomName;

                        links.Add(link);
                    }
                }

                if (m_ModernTheme)
                {
                    return ul;
                }
                else
                {
                    return ((links.Count > 0) ? links : null);
                }
            }
            finally
            {
                if (ul2 != null) ul2.Dispose();
                if (li2 != null) li2.Dispose();
                if (ul != null) ul.Dispose();
                if (li != null) li.Dispose();
                if (link != null) link.Dispose();
                if (links != null) links.Dispose();
            }
        }

        private void CreateChildControlsStandard()
        {
            HtmlGenericControl container = null;
            Control ctrl = null;

            try
            {
                container = new HtmlGenericControl("div");

                ctrl = this.CreateLogo();
                if (ctrl != null)
                {
                    container.Controls.Add(ctrl);
                }

                ctrl = this.CreateHeaderLinks();
                if (ctrl != null)
                {
                    container.Controls.Add(ctrl);
                }

                if (container.HasControls())
                {
                    container.Attributes["class"] = "Mp_Hdr";

                    this.Controls.Add(container);
                }
            }
            finally
            {
                if (container != null)
                {
                    container.Dispose();
                }

                if (ctrl != null)
                {
                    ctrl.Dispose();
                }
            }
        }

        private void CreateChildControlsModern()
        {
            HtmlGenericControl mainContainer = null;
            HtmlGenericControl container = null;
            HtmlGenericControl leftContainer = null;
            Control ctrl = null;

            try
            {
                container = new HtmlGenericControl("div");
                leftContainer = new HtmlGenericControl("div");

                ctrl = this.CreateApplicationLogo();
                if (ctrl != null)
                {
                    leftContainer.Controls.Add(ctrl);
                }

                ctrl = this.CreateLogo();
                if (ctrl != null)
                {
                    leftContainer.Controls.Add(ctrl);
                }

                if (m_MasterPage.VisibleSearchControl)
                {
                    ctrl = CreateSearchControl(null);

                    leftContainer.Controls.Add(ctrl);
                }

                if (leftContainer.HasControls())
                {
                    container.Controls.Add(leftContainer);
                }

                ctrl = this.CreateHeaderLinks();
                if (ctrl != null)
                {
                    container.Controls.Add(ctrl);
                }

                if (container.HasControls())
                {
                    container.Attributes["class"] = "container";

                    mainContainer = new HtmlGenericControl("div");
                    mainContainer.Attributes["class"] = "Mp_Hdr";
                    mainContainer.Controls.Add(container);

                    this.Controls.Add(mainContainer);
                }
            }
            finally
            {
                if (mainContainer != null)
                {
                    mainContainer.Dispose();
                }

                if (container != null)
                {
                    container.Dispose();
                }

                if (leftContainer != null)
                {
                    leftContainer.Dispose();
                }

                if (ctrl != null)
                {
                    ctrl.Dispose();
                }
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

            if (m_ModernTheme)
            {
                CreateChildControlsModern();
            }
            else
            {
                CreateChildControlsStandard();
            }
        }

        #endregion
    }
}