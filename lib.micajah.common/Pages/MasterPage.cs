using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Micajah.Common.WebControls;
using System;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Micajah.Common.Pages
{
    /// <summary>
    /// The common master page.
    /// </summary>
    public class MasterPage : System.Web.UI.MasterPage
    {
        #region Members

        private string m_BreadcrumbsCenterHtml;
        private string m_BreadcrumbsRightHtml;
        private string m_HeaderLogoImageUrl;
        private string m_HeaderLogoText;
        private string m_HeaderLogoNavigateUrl;
        private string m_HeaderLogoTarget;
        private string m_LeftAreaHtml;
        private string m_SearchButtonText;
        private string m_SearchButtonToolTip;
        private int m_SearchTextBoxColumns;
        private string m_SearchTextBoxEmptyText;
        private int m_SearchTextBoxMaxLength;
        private SubmenuPosition? m_SubmenuPosition;
        private bool? m_VisibleHeader;
        private bool? m_VisibleHeaderLinks;
        private bool? m_VisibleSearchControl;
        private bool? m_VisibleMainMenu;
        private bool? m_VisibleBreadcrumbs;
        private bool? m_VisibleHelpLink;
        private bool? m_VisibleLeftArea;
        private bool? m_VisibleSubmenu;
        private bool? m_VisibleApplicationLogo;
        private bool? m_VisibleHeaderLogo;
        private bool? m_VisibleFooter;

        private Header m_Header;
        private MainMenu m_MainMenu;
        private Submenu m_TopSubmenu;
        private Submenu m_LeftSubmenu;
        private Breadcrumbs m_BreadCrumbs;
        private DetailMenu m_DetailMenu;
        private HtmlGenericControl m_SoftwareLogo;
        private Footer m_Footer;
        private Control[] m_DefaultPageContent;
        private ContentPlaceHolder m_ContentPlaceHolder;
        private NoticeMessageBox m_NoticeMessageBox;
        private NoticeMessageBox m_HeaderMessageBox;

        private string m_PageTitle;
        private string m_CustomName;
        private string m_CustomNavigateUrl;
        private string m_CustomDescription;

        private UserContext m_UserContext;
        private IList m_ActionIdList;
        private bool m_IsFrameworkAdmin;
        private bool m_IsAuthenticated;
        private Guid m_OrganizationId;
        private Guid m_InstanceId;
        private Guid? m_ActiveActionId;
        private Micajah.Common.Bll.Action m_ActiveAction;

        private bool? m_IsDetailMenuPage;
        private bool? m_IsSetupPage;
        private bool m_LogoIsLoaded;
        private bool? m_ShowLeftArea;
        private bool m_UpdateBreadcrumbs;
        private bool m_GenerateBreadcrumbs;

        #endregion

        #region Constructors

        public MasterPage()
        {
            m_GenerateBreadcrumbs = true;
            m_OrganizationId = Guid.Empty;
            m_InstanceId = Guid.Empty;
            this.VisibleHeaderMessage = true;
        }

        #endregion

        #region Private Properties

        private bool ShowLeftArea
        {
            get
            {
                if (!m_ShowLeftArea.HasValue)
                {
                    m_ShowLeftArea = false;
                    if (this.VisibleLeftArea && (m_LeftSubmenu != null))
                    {
                        if (m_LeftSubmenu.HasControls()) m_ShowLeftArea = true;
                    }
                }
                return m_ShowLeftArea.Value;
            }
        }

        /// <summary>
        /// Gets the identifier of the active action from current request if it is detail menu's page.
        /// </summary>
        private Guid ActiveActionId
        {
            get
            {
                if (!m_ActiveActionId.HasValue)
                {
                    m_ActiveActionId = Guid.Empty;
                    if (this.IsDetailMenuPage)
                    {
                        object obj = Support.ConvertStringToType(Request["pageid"], typeof(Guid));
                        if (obj != null) m_ActiveActionId = (Guid)obj;
                    }
                }
                return m_ActiveActionId.Value;
            }
        }

        /// <summary>
        /// Gets the application absolute navigate URL of the current page.
        /// </summary>
        private string ApplicationAbsoluteNavigateUrl
        {
            get { return CustomUrlProvider.CreateApplicationAbsoluteUrl(Request.Url.PathAndQuery); }
        }

        /// <summary>
        /// Gets or sets the title to be shown on the page.
        /// </summary>
        private string PageTitle
        {
            get { return m_PageTitle; }
            set { m_PageTitle = value; }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the selected organization.
        /// </summary>
        private Guid OrganizationId
        {
            get
            {
                object obj = this.ViewState["OrganizationId"];
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
            set { this.ViewState["OrganizationId"] = value; }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the selected instance.
        /// </summary>
        private Guid InstanceId
        {
            get
            {
                object obj = this.ViewState["InstanceId"];
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
            set { this.ViewState["InstanceId"] = value; }
        }

        private bool IsDetailMenuPage
        {
            get
            {
                if (!m_IsDetailMenuPage.HasValue)
                    m_IsDetailMenuPage = ResourceProvider.IsDetailMenuPageUrl(Request.AppRelativeCurrentExecutionFilePath);
                return m_IsDetailMenuPage.Value;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the current postback is being executed in partial-rendering mode.
        /// </summary>
        private bool IsInAsyncPostBack
        {
            get
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                if (scriptManager != null)
                    return scriptManager.IsInAsyncPostBack;
                return false;
            }
        }

        #endregion

        #region Internal Properties

        internal static Control ApplicationLogo
        {
            get
            {
                Control ctl = null;

                string url = FrameworkConfiguration.Current.WebApplication.MasterPage.Footer.LogoImageUrl;
                if (string.IsNullOrEmpty(url))
                    url = FrameworkConfiguration.Current.WebApplication.LogoImageUrl;

                if (!string.IsNullOrEmpty(url))
                {
                    using (Image img = new Image())
                    {
                        img.ToolTip = FrameworkConfiguration.Current.WebApplication.Name;
                        img.ImageUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(url);
                        ctl = img;
                    }
                }
                else if (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.Name))
                {
                    using (LiteralControl logoLiteral = new LiteralControl(FrameworkConfiguration.Current.WebApplication.Name))
                    {
                        ctl = logoLiteral;
                    }
                }

                return ctl;
            }
        }

        internal string HelpLinkOnClick
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture
                    , "window.open(\"{0}\", '_blank', 'location=0,menubar=0,resizable=1,scrollbars=1,status=0,titlebar=0,toolbar=0,top=' + Mp_GetPopupPositionY(event) + ',left=' + Mp_GetPopupPositionX({1})  + ',width={1},height={2}');return false;"
                    , Support.PreserveDoubleQuote(string.Format(CultureInfo.InvariantCulture, FrameworkConfiguration.Current.WebApplication.MasterPage.HelpLink.UrlFormat, HttpUtility.UrlEncode(Page.Request.Url.PathAndQuery)))
                    , FrameworkConfiguration.Current.WebApplication.MasterPage.HelpLink.WindowWidth
                    , FrameworkConfiguration.Current.WebApplication.MasterPage.HelpLink.WindowHeight);
            }
        }

        internal string SearchText
        {
            get
            {
                string target = Request.Form["__EVENTTARGET"];
                string argument = Request.Form["__EVENTARGUMENT"];

                if (target != null && argument != null)
                {
                    if (target.Equals(ClientID, StringComparison.Ordinal) && argument.Equals(Header.SearchButtonId, StringComparison.Ordinal))
                    {
                        string searchText = Request.Form[string.Concat(ClientID, "$", Header.SearchTextBoxId)];
                        if (string.Compare(searchText, this.SearchTextBoxEmptyText, StringComparison.Ordinal) == 0) searchText = null;
                        return searchText;
                    }
                }

                return null;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the embedded global stylesheet should be added into page's header.
        /// </summary>
        public bool EnableGlobalStyleSheet
        {
            get
            {
                object obj = ViewState["EnableGlobalStyleSheet"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { ViewState["EnableGlobalStyleSheet"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the client caching of the page should be enabled or not.
        /// </summary>
        public bool EnableClientCaching
        {
            get
            {
                object obj = ViewState["EnableClientCaching"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["EnableClientCaching"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the encoding for the text inputs should be enabled or not on client.
        /// </summary>
        public bool EnableClientEncoding
        {
            get
            {
                object obj = ViewState["EnableClientEncoding"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { ViewState["EnableClientEncoding"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the overlay should be shown on client after form submitting.
        /// </summary>
        public bool EnableOverlay
        {
            get
            {
                object obj = ViewState["EnableOverlay"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { ViewState["EnableOverlay"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the FancyBox jQuery Plugin should be registered on the page.
        /// </summary>
        public bool EnableFancyBox
        {
            get
            {
                object obj = ViewState["EnableFancyBox"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["EnableFancyBox"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the jQuery JavaScript Library should be registered on the page.
        /// </summary>
        public bool EnableJQuery
        {
            get
            {
                object obj = ViewState["EnableJQuery"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["EnableJQuery"] = value; }
        }

        /// <summary>
        /// Gets the action for the current page.
        /// </summary>
        public Micajah.Common.Bll.Action ActiveAction
        {
            get
            {
                if (m_ActiveAction == null)
                    m_ActiveAction = ActionProvider.FindAction(ActiveActionId, ApplicationAbsoluteNavigateUrl);
                return m_ActiveAction;
            }
            set
            {
                m_ActiveAction = value;
            }
        }

        /// <summary>
        /// Gets or sets the image URL of the logo link in the header.
        /// </summary>
        public string HeaderLogoImageUrl
        {
            get
            {
                this.EnsureLogoIsLoaded();
                return m_HeaderLogoImageUrl;
            }
            set { m_HeaderLogoImageUrl = value; }
        }

        /// <summary>
        /// Gets or sets the text of the logo link in the header.
        /// </summary>
        public string HeaderLogoText
        {
            get
            {
                this.EnsureLogoIsLoaded();
                return m_HeaderLogoText;
            }
            set { m_HeaderLogoText = value; }
        }

        /// <summary>
        /// Gets or sets the URL to navigate of the logo link in the header.
        /// </summary>
        public string HeaderLogoNavigateUrl
        {
            get
            {
                this.EnsureLogoIsLoaded();
                return m_HeaderLogoNavigateUrl;
            }
            set { m_HeaderLogoNavigateUrl = value; }
        }

        /// <summary>
        /// Gets or sets the target window or frame in which to display the URL when logo link in the header is clicked.
        /// </summary>
        public string HeaderLogoTarget
        {
            get
            {
                this.EnsureLogoIsLoaded();
                return m_HeaderLogoTarget;
            }
            set { m_HeaderLogoTarget = value; }
        }

        /// <summary>
        /// Gets or sets to display credit card registration modal window on page load
        /// </summary>
        public bool ShowCreditCardRegistrationWindow { get; set; }

        /// <summary>
        /// Gets or sets the message to be shown on the page at the header.
        /// </summary>
        public string HeaderMessage { get; set; }

        /// <summary>
        /// Gets or sets the type of the message to be shown on the page at the header.
        /// </summary>
        public NoticeMessageType HeaderMessageType { get; set; }

        /// <summary>
        /// Gets or sets the message description to be shown on the page at the header.
        /// </summary>
        public string HeaderMessageDescription { get; set; }

        /// <summary>
        /// The HTML to be shown at the left area below of the submenu.
        /// </summary>
        public string LeftAreaHtml
        {
            get { return (string.IsNullOrEmpty(m_LeftAreaHtml) ? FrameworkConfiguration.Current.WebApplication.MasterPage.LeftArea.Html : m_LeftAreaHtml); }
            set { m_LeftAreaHtml = value; }
        }

        /// <summary>
        /// The HTML to be shown at the center side of the breadcrumbs area
        /// </summary>
        public string BreadcrumbsCenterHtml
        {
            get { return (string.IsNullOrEmpty(m_BreadcrumbsCenterHtml) ? FrameworkConfiguration.Current.WebApplication.MasterPage.Breadcrumbs.CenterHtml : m_BreadcrumbsCenterHtml); }
            set { m_BreadcrumbsCenterHtml = value; }
        }

        /// <summary>
        /// The HTML to be shown at the right side of the breadcrumbs area
        /// </summary>
        public string BreadcrumbsRightHtml
        {
            get { return (string.IsNullOrEmpty(m_BreadcrumbsRightHtml) ? FrameworkConfiguration.Current.WebApplication.MasterPage.Breadcrumbs.RightHtml : m_BreadcrumbsRightHtml); }
            set { m_BreadcrumbsRightHtml = value; }
        }

        /// <summary>
        /// Gets or sets the text caption displayed in the search button.
        /// </summary>
        public string SearchButtonText
        {
            get { return ((m_SearchButtonText == null) ? Resources.Header_SearchButton_Text : m_SearchButtonText); }
            set { m_SearchButtonText = value; }
        }

        /// <summary>
        /// Gets or sets the text displayed when the mouse pointer hovers over the search button.
        /// </summary>
        public string SearchButtonToolTip
        {
            get { return ((m_SearchButtonToolTip == null) ? Resources.Header_SearchButton_ToolTip : m_SearchButtonToolTip); }
            set { m_SearchButtonToolTip = value; }
        }

        /// <summary>
        /// Gets or sets the display width of the search text box in characters.
        /// </summary>
        public int SearchTextBoxColumns
        {
            get { return ((m_SearchTextBoxColumns > 0) ? m_SearchTextBoxColumns : 30); }
            set { m_SearchTextBoxColumns = value; }
        }

        /// <summary>
        /// Gets or sets the value which the search text box displays when it does not have focus.
        /// </summary>
        public string SearchTextBoxEmptyText
        {
            get { return ((m_SearchTextBoxEmptyText == null) ? Resources.Header_SearchTextBox_EmptyText : m_SearchTextBoxEmptyText); }
            set { m_SearchTextBoxEmptyText = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters allowed in the search text box.
        /// </summary>
        public int SearchTextBoxMaxLength
        {
            get { return m_SearchTextBoxMaxLength; }
            set { m_SearchTextBoxMaxLength = value; }
        }

        /// <summary>
        /// Gets or sets the position of the submenu.
        /// </summary>
        public SubmenuPosition SubmenuPosition
        {
            get { return (m_SubmenuPosition.HasValue ? m_SubmenuPosition.Value : FrameworkConfiguration.Current.WebApplication.MasterPage.Submenu.Position); }
            set { m_SubmenuPosition = value; }
        }

        /// <summary>
        /// Gets or sets the identifier of the action the child actions of which are displayed in the submenu.
        /// </summary>
        public Guid SubmenuParentActionId
        {
            get
            {
                object obj = ViewState["SubmenuParentActionId"];
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
            set { ViewState["SubmenuParentActionId"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the header is visible and rendered.
        /// </summary>
        public bool VisibleHeader
        {
            get { return (m_VisibleHeader.HasValue ? m_VisibleHeader.Value : FrameworkConfiguration.Current.WebApplication.MasterPage.Header.Visible); }
            set { m_VisibleHeader = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the header links are visible and rendered.
        /// </summary>
        public bool VisibleHeaderLinks
        {
            get { return (m_VisibleHeaderLinks.HasValue ? m_VisibleHeaderLinks.Value : FrameworkConfiguration.Current.WebApplication.MasterPage.Header.VisibleLinks); }
            set { m_VisibleHeaderLinks = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the help link is visible and rendered.
        /// </summary>
        public bool VisibleHelpLink
        {
            get { return ((m_VisibleHelpLink.HasValue ? m_VisibleHelpLink.Value : FrameworkConfiguration.Current.WebApplication.MasterPage.HelpLink.Visible)); }
            set { m_VisibleHelpLink = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the search control are visible and rendered.
        /// </summary>
        public bool VisibleSearchControl
        {
            get { return (m_VisibleSearchControl.HasValue ? m_VisibleSearchControl.Value : FrameworkConfiguration.Current.WebApplication.MasterPage.Header.VisibleSearch); }
            set { m_VisibleSearchControl = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the main menu is visible and rendered.
        /// </summary>
        public bool VisibleMainMenu
        {
            get { return (m_VisibleMainMenu.HasValue ? m_VisibleMainMenu.Value : FrameworkConfiguration.Current.WebApplication.MasterPage.MainMenu.Visible); }
            set { m_VisibleMainMenu = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the bread crumbs on the page
        /// </summary>
        public bool VisibleBreadcrumbs
        {
            get { return (m_VisibleBreadcrumbs.HasValue ? m_VisibleBreadcrumbs.Value : FrameworkConfiguration.Current.WebApplication.MasterPage.Breadcrumbs.Visible); }
            set { m_VisibleBreadcrumbs = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the left area is visible and rendered.
        /// </summary>
        public bool VisibleLeftArea
        {
            get { return (m_VisibleLeftArea.HasValue ? m_VisibleLeftArea.Value : FrameworkConfiguration.Current.WebApplication.MasterPage.LeftArea.Visible); }
            set { m_VisibleLeftArea = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the submenu is visible and rendered.
        /// </summary>
        public bool VisibleSubmenu
        {
            get { return (m_VisibleSubmenu.HasValue ? m_VisibleSubmenu.Value : FrameworkConfiguration.Current.WebApplication.MasterPage.Submenu.Visible); }
            set { m_VisibleSubmenu = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the application logo's image is visible and rendered.
        /// </summary>
        public bool VisibleApplicationLogo
        {
            get
            {
                return (m_VisibleApplicationLogo.HasValue ? m_VisibleApplicationLogo.Value : FrameworkConfiguration.Current.WebApplication.MasterPage.Footer.VisibleApplicationLogo);
            }
            set
            {
                m_VisibleApplicationLogo = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the logo's image is visible and rendered in the header.
        /// </summary>
        public bool VisibleHeaderLogo
        {
            get { return (m_VisibleHeaderLogo.HasValue ? m_VisibleHeaderLogo.Value : FrameworkConfiguration.Current.WebApplication.MasterPage.Header.VisibleLogo); }
            set { m_VisibleHeaderLogo = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the footer is visible and rendered.
        /// </summary>
        public bool VisibleFooter
        {
            get
            {
                return (m_VisibleFooter.HasValue ? m_VisibleFooter.Value : FrameworkConfiguration.Current.WebApplication.MasterPage.Footer.Visible);
            }
            set
            {
                m_VisibleFooter = value;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the message at the header of the page is visible and rendered.
        /// </summary>
        public bool VisibleHeaderMessage { get; set; }

        /// <summary>
        /// Gets or sets custom title of the page to use it in bread crumbs as name.
        /// </summary>
        public string CustomName
        {
            get { return m_CustomName; }
            set { m_CustomName = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the page to use it in the bread crumbs as navigate URL.
        /// </summary>
        public string CustomNavigateUrl
        {
            get { return m_CustomNavigateUrl; }
            set { m_CustomNavigateUrl = value; }
        }

        /// <summary>
        /// Gets or sets the description of the page to be shown in the bread crumbs as tool tip.
        /// </summary>
        public string CustomDescription
        {
            get { return m_CustomDescription; }
            set { m_CustomDescription = value; }
        }

        /// <summary>
        /// Gets or sets the message to be shown on the page.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        public NoticeMessageType MessageType { get; set; }

        /// <summary>
        /// Gets or sets the message description.
        /// </summary>
        public string MessageDescription { get; set; }

        /// <summary>
        /// Gets or sets the value indicating that the bread crumbs is autogenerated or not.
        /// </summary>
        public bool AutoGenerateBreadcrumbs
        {
            get
            {
                object obj = ViewState["AutoGenerateBreadcrumbs"];
                return ((obj == null) ? true : (bool)obj);
            }
            set
            {
                ViewState["AutoGenerateBreadcrumbs"] = value;
                if (!value) UserContext.Breadcrumbs.Clear();
            }
        }

        /// <summary>
        /// Gets a value that indicates the current page is home page for current user.
        /// </summary>
        public bool IsHomepage
        {
            get
            {
                if (this.ActiveAction != null)
                {
                    if (m_UserContext != null)
                        return this.ActiveAction.AbsoluteNavigateUrl.Equals(m_UserContext.StartPageUrl, StringComparison.OrdinalIgnoreCase);
                    else if (string.Compare(this.ActiveAction.AbsoluteNavigateUrl, CustomUrlProvider.CreateApplicationAbsoluteUrl("/default.aspx"), StringComparison.OrdinalIgnoreCase) == 0)
                        return (!this.ActiveAction.AuthenticationRequired);
                }
                return false;
            }
        }

        /// <summary>
        /// Gets a value that indicates the current page is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                bool isEmpty = true;
                if (m_ContentPlaceHolder != null)
                {
                    if (m_ContentPlaceHolder.HasControls())
                    {
                        if (m_ContentPlaceHolder.Controls.Count == 1)
                        {
                            LiteralControl literal = m_ContentPlaceHolder.Controls[0] as LiteralControl;
                            if (literal != null) isEmpty = Support.StringIsNullOrEmpty(literal.Text);
                        }
                        else
                            isEmpty = false;
                    }
                }
                return isEmpty;
            }
        }

        /// <summary>
        /// Gets a value that indicates the current page is admin page.
        /// </summary>
        public virtual bool IsAdminPage
        {
            get
            {
                return ((Request.AppRelativeCurrentExecutionFilePath.IndexOf(ResourceProvider.AdminVirtualRootShortPath, StringComparison.OrdinalIgnoreCase) > -1)
                    && ((this.ActiveAction != null) && (m_ActiveAction.ActionId != ActionProvider.StartPageActionId)))
                    || ((this.ActiveAction != null) && (m_ActiveAction.ActionId == ActionProvider.ConfigurationPageActionId));
            }
        }

        /// <summary>
        /// Gets a value that indicates the current page is setup page.
        /// </summary>
        public bool IsSetupPage
        {
            get
            {
                if (!m_IsSetupPage.HasValue)
                    m_IsSetupPage = ((this.ActiveAction != null) && ActionProvider.IsSetupPage(m_ActiveAction));
                return m_IsSetupPage.Value;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the submit button of the search control is clicked.
        /// </summary>
        public event CommandEventHandler SearchButtonClick;

        #endregion

        #region Private Methods

        private void CreateApplicationLogo()
        {
            if (this.VisibleApplicationLogo)
            {
                Control ctl = ApplicationLogo;
                if (ctl != null)
                {
                    HtmlGenericControl div1 = null;
                    HtmlGenericControl div2 = null;

                    try
                    {
                        div2 = new HtmlGenericControl("div");
                        div2.Attributes["class"] = ((ctl is Image) ? "Mp_SlgImg" : "Mp_SlgTxt");
                        div2.Controls.Add(ctl);

                        div1 = new HtmlGenericControl("div");
                        div1.Attributes["class"] = "Mp_Slg";
                        div1.Controls.Add(div2);

                        m_SoftwareLogo = new HtmlGenericControl("div");
                        m_SoftwareLogo.Attributes["class"] = "Mp_SlgC";
                        m_SoftwareLogo.Controls.Add(div1);

                        this.Controls.Add(m_SoftwareLogo);
                    }
                    finally
                    {
                        if (div1 != null) div1.Dispose();
                        if (div2 != null) div2.Dispose();
                    }
                }
            }
        }

        private void CreateBreadCrumbs()
        {
            if ((ActiveAction != null) && AutoGenerateBreadcrumbs)
            {
                if (this.IsPostBack)
                {
                    if (m_GenerateBreadcrumbs)
                    {
                        BreadcrumbCollection coll = UserContext.Breadcrumbs;
                        Micajah.Common.Bll.Action lastItem = null;
                        if (coll.Count > 0)
                            lastItem = coll[coll.Count - 1];
                        if (lastItem != ActiveAction)
                        {
                            coll.Generate(ActiveAction.Clone(), true);
                        }
                    }
                }
                else
                {
                    Micajah.Common.Bll.Action item = ActiveAction.Clone();
                    item.NavigateUrl = CustomUrlProvider.CreateApplicationRelativeUrl(Request.Url.PathAndQuery);

                    if (!string.IsNullOrEmpty(CustomName))
                    {
                        item.IsCustom = true;
                        item.Name = CustomName;
                        if (!string.IsNullOrEmpty(CustomNavigateUrl)) item.NavigateUrl = CustomUrlProvider.CreateApplicationRelativeUrl(CustomNavigateUrl);
                        if (!string.IsNullOrEmpty(CustomDescription)) item.Description = CustomDescription;
                    }

                    UserContext.Breadcrumbs.Add(item);
                }
            }

            bool showBreadCrumbs = this.VisibleBreadcrumbs;
            if ((ActiveAction != null) && (m_ActiveAction.IsMainMenuRoot || m_ActiveAction.IsGlobalNavigationLinksRoot))
            {
                showBreadCrumbs = false;
                if (m_VisibleBreadcrumbs.HasValue && m_VisibleBreadcrumbs.Value)
                    showBreadCrumbs = true;
            }
            m_BreadCrumbs = new Breadcrumbs(this);
            m_BreadCrumbs.ShowBreadcrumbs = showBreadCrumbs;
            Controls.Add(m_BreadCrumbs);
        }

        private void CreateCreditCardRegistrationWindow()
        {
            if (!ShowCreditCardRegistrationWindow || !FrameworkConfiguration.Current.WebApplication.Integration.Chargify.Enabled) return;
            if (Request.Path.Contains(ResourceProvider.AccountSettingsVirtualPath.TrimStart('~'))) return;
            CreditCardRegistrationControl ccrControl = (CreditCardRegistrationControl)Page.LoadControl("~/Resources.Micajah.Common/Controls/CreditCardRegistrationControl.ascx");
            HtmlAnchor a = new HtmlAnchor();
            a.ID = "ccr_hidden_link";
            a.HRef = "#credit_card_form";
            a.Attributes.Add("rel", "facebox1");
            a.Style.Add(HtmlTextWriterStyle.Display, "none");
            Page.Form.Controls.Add(a);
            ccrControl.FancyboxHyperlinkRel = "facebox1";
            ccrControl.ShowMissingCardTitle = true;
            Page.Form.Controls.Add(ccrControl);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "CreditCardRegistrationLaunchScript", "$(document).ready(function() {$(\"#" + a.ClientID + "\").trigger('click');});", true);
        }

        private void CreateHeaderMessageBox()
        {
            if (this.VisibleHeaderMessage)
            {
                if (!string.IsNullOrEmpty(this.HeaderMessage))
                {
                    m_HeaderMessageBox = new NoticeMessageBox();
                    m_HeaderMessageBox.Width = Unit.Percentage(100);
                    m_HeaderMessageBox.Size = NoticeMessageBoxSize.Small;
                    m_HeaderMessageBox.HorizontalAlign = HorizontalAlign.Center;
                    m_HeaderMessageBox.Visible = false;
                    m_HeaderMessageBox.Message = this.HeaderMessage;
                    m_HeaderMessageBox.MessageType = this.HeaderMessageType;
                    m_HeaderMessageBox.Description = this.HeaderMessageDescription;
                    Controls.Add(m_HeaderMessageBox);
                }
            }
        }

        private void CreateNoticeMessageBox()
        {
            m_NoticeMessageBox = new NoticeMessageBox();
            m_NoticeMessageBox.Width = Unit.Percentage(100);
            m_NoticeMessageBox.Visible = false;
            if (!string.IsNullOrEmpty(this.Message))
            {
                m_NoticeMessageBox.Message = this.Message;
                m_NoticeMessageBox.MessageType = this.MessageType;
                m_NoticeMessageBox.Description = this.MessageDescription;
            }
            Controls.Add(m_NoticeMessageBox);
        }

        private void CreateMenus()
        {
            if (this.VisibleMainMenu)
            {
                m_MainMenu = new MainMenu(this, m_UserContext, m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated);
                Controls.Add(m_MainMenu);
            }

            if (this.IsAdminPage)
            {
                if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern)
                {
                    this.SubmenuParentActionId = ActionProvider.ConfigurationPageActionId;
                    this.SubmenuPosition = WebControls.SubmenuPosition.Left;
                    this.VisibleLeftArea = this.VisibleSubmenu = true;
                }
                else
                {
                    this.VisibleLeftArea = this.VisibleSubmenu = false;
                }
            }

            if ((this.SubmenuPosition == SubmenuPosition.Top) && VisibleSubmenu)
            {
                m_TopSubmenu = new Submenu(this, m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated, SubmenuPosition.Top);
                m_TopSubmenu.ParentActionId = this.SubmenuParentActionId;
                Controls.Add(m_TopSubmenu);
            }
            else if ((this.SubmenuPosition == SubmenuPosition.Left) && VisibleLeftArea && VisibleSubmenu)
            {
                m_LeftSubmenu = new Submenu(this, m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated);
                m_LeftSubmenu.ParentActionId = this.SubmenuParentActionId;
                Controls.Add(m_LeftSubmenu);
            }
        }

        private void CreateDefaultPageContent()
        {
            if ((this.ActiveActionId != Guid.Empty) || this.IsEmpty)
            {
                m_DetailMenu = new DetailMenu(this, m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated);
                m_DefaultPageContent = new Control[] { m_DetailMenu };
            }

            if (m_DefaultPageContent != null)
            {
                Control ctl = null;
                for (int idx = 0; idx < m_DefaultPageContent.Length; idx++)
                {
                    ctl = m_DefaultPageContent[idx];
                    if (ctl != null)
                    {
                        ctl.ID = string.Concat("ctl", idx);
                        Controls.Add(ctl);
                    }
                }
            }
        }

        private void CreatePageHeader()
        {
            bool modernTheme = (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern);

            CreatePageHeader(this.Page, this.EnableClientCaching, this.EnableClientEncoding, this.EnableGlobalStyleSheet, (this.EnableJQuery || modernTheme), modernTheme, this.EnableFancyBox);

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.EnableCustomStyleSheet)
            {
                if (this.Page.Header != null)
                {
                    if ((m_UserContext != null) && (m_UserContext.OrganizationId != Guid.Empty))
                    {
                        if (!string.IsNullOrEmpty(SettingProvider.GetCustomStyleSheet(m_UserContext.OrganizationId)))
                        {
                            this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.CustomStyleSheet + "?" + m_UserContext.OrganizationId.ToString("N"), true)));
                        }
                    }
                }
            }
        }

        private void EnsureLogoIsLoaded()
        {
            if (m_LogoIsLoaded) return;

            if (m_UserContext != null)
            {
                if (string.IsNullOrEmpty(m_HeaderLogoNavigateUrl))
                {
                    m_HeaderLogoNavigateUrl = m_UserContext.StartPageUrl;
                    m_HeaderLogoTarget = null;
                }
                else if (string.IsNullOrEmpty(m_HeaderLogoTarget))
                    m_HeaderLogoTarget = "_blank";

                if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances && (m_UserContext.InstanceId != Guid.Empty))
                {
                    if (string.IsNullOrEmpty(m_HeaderLogoImageUrl))
                        m_HeaderLogoImageUrl = InstanceProvider.GetInstanceLogoImageUrlFromCache(m_UserContext.InstanceId, m_UserContext.OrganizationId);

                    if (string.IsNullOrEmpty(m_HeaderLogoText))
                        m_HeaderLogoText = m_UserContext.Instance.Name;
                }

                if (m_UserContext.OrganizationId != Guid.Empty)
                {
                    if (string.IsNullOrEmpty(m_HeaderLogoImageUrl))
                        m_HeaderLogoImageUrl = OrganizationProvider.GetOrganizationLogoImageUrlFromCache(m_UserContext.OrganizationId);
                    if (string.IsNullOrEmpty(m_HeaderLogoText))
                        m_HeaderLogoText = m_UserContext.Organization.Name;
                    else
                        m_HeaderLogoText = m_UserContext.Organization.Name + " " + m_HeaderLogoText;
                }
                else if (this.IsSetupPage || ((this.ActiveAction != null) && (this.ActiveAction.ActionId == ActionProvider.LoginAsUserGlobalNavigationLinkActionId)))
                {
                    Micajah.Common.Bll.Action action = ActionProvider.FindAction(ActionProvider.SetupPageActionId);
                    m_HeaderLogoNavigateUrl = action.AbsoluteNavigateUrl;
                }
            }
            else
            {
                m_HeaderLogoNavigateUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl("~/");
            }

            m_LogoIsLoaded = true;
        }

        /// <summary>
        /// Renders the control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        /// <param name="control">The control to render.</param>
        private static void RenderControl(HtmlTextWriter writer, Control control)
        {
            if (writer == null || control == null) return;

            control.RenderControl(writer);
            control.Visible = false;
        }

        /// <summary>
        /// Renders the help hyperlink.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        private void RenderHelpLink(HtmlTextWriter writer)
        {
            if (writer == null || (!this.VisibleHelpLink)) return;

            string cssClass = "Mp_H";
            if (this.VisibleMainMenu)
            {
                if (this.VisibleHeader)
                    cssClass += " Mp_HMmOnHdrOn";
                else
                    cssClass += " Mp_HMmOn";
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, this.HelpLinkOnClick);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(Resources.MasterPage_HelpLink_Text1);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        /// <summary>
        /// Renders the left area.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        private void RenderLeftAreaLayer(HtmlTextWriter writer)
        {
            if (writer == null || (!this.ShowLeftArea)) return;

            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, FrameworkConfiguration.Current.WebApplication.MasterPage.LeftArea.Width.ToString(CultureInfo.InvariantCulture) + "px");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "Mp_La");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Mp_La");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write("&nbsp;");
            writer.RenderEndTag();
        }

        /// <summary>
        /// Renders the notice message box.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        private void RenderNoticeMessageBox(HtmlTextWriter writer)
        {
            if (writer == null) return;

            writer.AddAttribute(HtmlTextWriterAttribute.Id, "Mp_Mbx");
            writer.RenderBeginTag(HtmlTextWriterTag.Div); // Container

            if (!string.IsNullOrEmpty(this.Message))
            {
                m_NoticeMessageBox.Visible = true;
                if (this.IsInAsyncPostBack)
                    this.RegisterUpdateScript("Mp_Mbx", m_NoticeMessageBox.RenderControl());
                else
                {
                    RenderControl(writer, m_NoticeMessageBox);
                }
            }

            writer.RenderEndTag(); // Container
        }

        /// <summary>
        /// Renders the page content's cell.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        private void RenderContent(HtmlTextWriter writer, Control container)
        {
            if (writer == null) return;

            if (this.ShowLeftArea)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.MarginLeft, FrameworkConfiguration.Current.WebApplication.MasterPage.LeftArea.Width.ToString(CultureInfo.InvariantCulture) + "px");
            }

            MasterPageTheme theme = FrameworkConfiguration.Current.WebApplication.MasterPage.Theme;

            if (theme != MasterPageTheme.Modern)
            {
                string cssClass = "Mp_Pc";
                if (this.VisibleFooter || this.VisibleApplicationLogo)
                {
                    cssClass += " Mp_PcSlgOnFtrOn";
                }
                else
                {
                    cssClass += " Mp_PcSlgOn";
                }

                writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Div); // Page's content

            if (theme != MasterPageTheme.Modern)
            {
                this.RenderHelpLink(writer);
            }

            if (m_UpdateBreadcrumbs)
            {
                if (this.IsInAsyncPostBack)
                {
                    this.RegisterUpdateScript("Mp_B", m_BreadCrumbs.RenderContent());
                }
            }
            else
            {
                m_BreadCrumbs.RenderControl(writer);
            }
            m_BreadCrumbs.Visible = false;

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Mp_Pmc");
            writer.RenderBeginTag(HtmlTextWriterTag.Div); // Page's main content

            this.RenderNoticeMessageBox(writer);

            if ((m_DefaultPageContent != null) && (m_DefaultPageContent.Length > 0))
            {
                for (int idx = 0; idx < m_DefaultPageContent.Length; idx++)
                {
                    RenderControl(writer, m_DefaultPageContent[idx]);
                }
            }
            else if (container != null)
            {
                foreach (Control control in container.Controls)
                {
                    control.RenderControl(writer);
                }
            }

            writer.RenderEndTag(); // Page's main content
            writer.RenderEndTag(); // Page's content
        }

        /// <summary>
        /// Renders the standard structure of the page and the content of the ContentPlaceHolder.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        /// <param name="container">The Control to render.</param>
        private void RenderContentPlaceHolder(HtmlTextWriter writer, Control container)
        {
            if (writer == null) return;

            MasterPageTheme theme = FrameworkConfiguration.Current.WebApplication.MasterPage.Theme;
            bool renderHeaderTag = false;
            bool renderTopSubmenu = false;

            if (m_TopSubmenu != null)
            {
                if ((m_TopSubmenu.Items != null) && (m_TopSubmenu.Items.Count > 0))
                {
                    renderTopSubmenu = true;
                }
            }

            if (theme != MasterPageTheme.Modern)
            {
                this.RenderLeftAreaLayer(writer);
            }

            if (m_HeaderMessageBox != null)
            {
                m_HeaderMessageBox.Visible = true;
                RenderControl(writer, m_HeaderMessageBox);
            }

            if (theme == MasterPageTheme.Modern)
            {
                renderHeaderTag = ((m_Header != null) || (m_MainMenu != null) || renderTopSubmenu);

                if (renderHeaderTag)
                {
                    writer.RenderBeginTag("header"); // Header tag
                }
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "Mp_NnFtr"); // Non-footer
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
            }

            if (m_Header != null)
            {
                RenderControl(writer, m_Header);
            }
            else if (this.VisibleMainMenu)
            {
                if (theme != MasterPageTheme.Modern)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "Mp_HdrOff"); // Top padding
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.RenderEndTag(); // Top padding
                }
            }

            RenderControl(writer, m_MainMenu);

            if (renderTopSubmenu)
            {
                RenderControl(writer, m_TopSubmenu);
            }

            if (renderHeaderTag)
            {
                writer.RenderEndTag(); // Header tag
            }

            if (theme == MasterPageTheme.Modern)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "container");
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "Mp_Pm");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Div); // Middle part of the page

            if (this.ShowLeftArea)
            {
                RenderControl(writer, m_LeftSubmenu);
            }

            this.RenderContent(writer, container);

            writer.RenderEndTag(); // Middle part of the page

            if (theme != MasterPageTheme.Modern)
            {
                writer.RenderEndTag(); // Non-footer
            }

            RenderControl(writer, m_SoftwareLogo);
            RenderControl(writer, m_Footer);
        }

        /// <summary>
        /// Checks the access to current page depending on current user's groups roles.
        /// </summary>
        private void CheckAccessToPage()
        {
            CheckAccessToPage(Context, m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated, m_OrganizationId, m_InstanceId, this.ActiveAction);
        }

        private void SetAccessToControls(Micajah.Common.Bll.Action item, Control control)
        {
            Control ctl = null;
            foreach (Micajah.Common.Bll.Action child in item.ChildControls)
            {
                ctl = control.FindControl(child.Name);
                if (ctl != null && ctl.Visible)
                {
                    if (child.AccessDenied() || (!m_ActionIdList.Contains(child.ActionId)))
                        ctl.Visible = false;
                    else
                        SetAccessToControls(child, ctl);
                }
            }
        }

        /// <summary>
        /// Handles the search event.
        /// </summary>
        private void HandleSearchEvent()
        {
            string searchText = this.SearchText;
            if ((searchText != null) && (SearchButtonClick != null)) SearchButtonClick(this, new CommandEventArgs("Search", searchText));
        }

        private void SetPageTitle()
        {
            if (!string.IsNullOrEmpty(CustomName))
                PageTitle = CustomName;

            SetPageTitle(this.Page, PageTitle);
        }

        private void RegisterClientScripts()
        {
            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern)
                ResourceProvider.RegisterValidatorScriptResource(this.Page);
        }

        private void RegisterUpdateScript(string clientId, string content)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Mp_Update_" + clientId, "Mp_Update('" + clientId + "', '" + PrepareHtml(content) + "');\r\n", true);
        }

        private static string PrepareHtml(string value)
        {
            string str = value.Replace("'", "&#039;").Replace("\r\n", string.Empty);
            Regex rgx = new Regex(">\\s+<", RegexOptions.Multiline);
            str = rgx.Replace(str, "><");
            return str;
        }

        #endregion

        #region Internal Methods

        internal static void CheckAccessToPage(HttpContext http, IList actionIdList, bool isFrameworkAdmin, bool isAuthenticated, Guid organizationId, Guid instanceId, Micajah.Common.Bll.Action action)
        {
            if (action != null)
            {
                if (action.GroupInDetailMenu)
                    throw new HttpException(404, Resources.Error_404);
                else if (!action.AuthenticationRequired)
                    return;
            }
            else
            {
                if (http.SkipAuthorization)
                    return;
                else
                    throw new HttpException(404, Resources.Error_404_ActionNotFound);
            }

            bool accessDenied = false;
            string redirectUrl = null;
            string returnUrl = http.Request.Url.PathAndQuery;

            if (isAuthenticated)
            {
                if (action.OrganizationRequired && organizationId == Guid.Empty)
                    redirectUrl = ResourceProvider.GetActiveOrganizationUrl(returnUrl);
                else if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances && action.InstanceRequired && (instanceId == Guid.Empty))
                    redirectUrl = ResourceProvider.GetActiveInstanceUrl(returnUrl);
                else if (action.ActionType == ActionType.Page || action.ActionType == ActionType.GlobalNavigationLink)
                {
                    accessDenied = action.AccessDenied();
                    if (!accessDenied)
                        accessDenied = (!ActionProvider.ShowAction(action, actionIdList, isFrameworkAdmin, isAuthenticated));
                }
            }
            else
                accessDenied = true;

            if (accessDenied)
            {
                if (isAuthenticated)
                    throw new HttpException(403, Resources.Error_403);
                else
                    redirectUrl = LoginProvider.Current.GetLoginUrl(false) + "?returnurl=" + HttpUtility.UrlEncode(returnUrl);
            }

            if (!string.IsNullOrEmpty(redirectUrl))
                http.Response.Redirect(redirectUrl);
        }

        internal static Micajah.Common.Pages.MasterPage GetMasterPage(Page page)
        {
            Micajah.Common.Pages.MasterPage masterPage = null;
            System.Web.UI.MasterPage m = page.Master;
            while (m != null)
            {
                masterPage = (m as Micajah.Common.Pages.MasterPage);
                if (masterPage != null)
                    break;
                m = m.Master;
            }
            return masterPage;
        }

        internal static void RegisterGlobalStyleSheet(Page page, MasterPageTheme theme)
        {
            if (page != null)
            {
                if (page.Header != null)
                    page.Header.Controls.AddAt(0, Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl((theme == MasterPageTheme.Modern) ? ResourceProvider.GlobalModernStyleSheet : ResourceProvider.GlobalStyleSheet, true)));
            }
        }

        /// <summary>
        /// Stops all server caching for the current response.
        /// </summary>
        internal static void DisableServerCaching()
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx != null)
            {
                ctx.Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-1));
                ctx.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                ctx.Response.Cache.SetNoStore();
                ctx.Response.Cache.SetNoServerCaching();
                ctx.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            }
        }

        /// <summary>
        /// Stops all client caching for the specified page.
        /// </summary>
        internal static void DisableClientCaching(Page page)
        {
            if (page != null && page.Header != null)
            {
                using (HtmlMeta expiresMeta = new HtmlMeta())
                {
                    expiresMeta.HttpEquiv = "Expires";
                    expiresMeta.Content = "0";
                    page.Header.Controls.AddAt(0, expiresMeta);
                }

                using (HtmlMeta cacheControlMeta = new HtmlMeta())
                {
                    cacheControlMeta.HttpEquiv = "Cache-control";
                    cacheControlMeta.Content = "no-cache";
                    page.Header.Controls.AddAt(0, cacheControlMeta);
                }

                using (HtmlMeta pragmaMeta = new HtmlMeta())
                {
                    pragmaMeta.HttpEquiv = "Pragma";
                    pragmaMeta.Content = "no-cache";
                    page.Header.Controls.AddAt(0, pragmaMeta);
                }
            }
        }

        /// <summary>
        /// Inserts the common stylesheets and meta tags that disable page caching on the client side into page's header.
        /// </summary>
        /// <param name="page">The page to create header for.</param>
        /// <param name="enableClientCaching">The value indicating whether the client caching of the page should be enabled or not.</param>
        internal static void CreatePageHeader(Page page, bool enableClientCaching, bool enableClientEncoding)
        {
            bool modernTheme = (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern);

            CreatePageHeader(page, enableClientCaching, enableClientEncoding, true, modernTheme, modernTheme, false);
        }

        /// <summary>
        /// Inserts the common stylesheets and meta tags that disable page caching on the client side into page's header.
        /// </summary>
        /// <param name="page">The page to create header for.</param>
        /// <param name="enableClientCaching">The value indicating whether the client caching of the page should be enabled or not.</param>
        /// <param name="enableGlobalStyleSheet">The value indicating whether the embedded global stylesheet should be added into page's header.</param>
        /// <param name="enableJQuery">The value indicating whether the jQuery JavaScript Library should be added into page's header.</param>
        /// <param name="enableFancyBox">The value indicating whether the FancyBox jQuery Plugin should be added into page's header.</param>
        /// <param name="enableFancyBox">The value indicating whether the encoding for the text inputs should be enabled or not on client.</param>
        internal static void CreatePageHeader(Page page, bool enableClientCaching, bool enableClientEncoding, bool enableGlobalStyleSheet, bool enableJQuery, bool enableBootstrap, bool enableFancyBox)
        {
            if (page != null && page.Header != null)
            {
                MasterPageTheme theme = FrameworkConfiguration.Current.WebApplication.MasterPage.Theme;
                MasterPageThemeColor themeColor = FrameworkConfiguration.Current.WebApplication.MasterPage.ThemeColor;

                if (!enableClientCaching)
                {
                    DisableClientCaching(page);
                }

                // Registers scripts.
                if (enableClientEncoding)
                {
                    RegisterClientEncodingScript(page);
                }

                if (enableFancyBox)
                {
                    page.Header.Controls.AddAt(0, Support.CreateJavaScriptLiteral(ResourceProvider.FancyBoxScriptUrl, "FancyBoxScript"));
                }

                if (enableBootstrap)
                {
                    page.Header.Controls.AddAt(0, Support.CreateJavaScriptLiteral(ResourceProvider.BootstrapScriptUrl, "BootstrapScript"));
                }

                if (enableJQuery)
                {
                    page.Header.Controls.AddAt(0, Support.CreateJavaScriptLiteral(ResourceProvider.JQueryScriptUrl, "JQueryScript"));
                }

                // Registers style sheets.
                if (theme != MasterPageTheme.Modern)
                {
                    page.Header.Controls.AddAt(0, Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.GetMasterPageThemeColorStyleSheet(theme, themeColor), true)));
                }

                page.Header.Controls.AddAt(0, Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.GetMasterPageThemeBaseStyleSheet(theme), true)));

                if (enableGlobalStyleSheet)
                {
                    RegisterGlobalStyleSheet(page, theme);
                }

                if (enableFancyBox)
                {
                    page.Header.Controls.AddAt(0, Support.CreateStyleSheetLink(ResourceProvider.FancyBoxStyleSheetUrl));
                }

                if (enableBootstrap)
                {
                    page.Header.Controls.AddAt(0, Support.CreateStyleSheetLink(ResourceProvider.BootstrapStyleSheetUrl));
                }
            }
        }

        internal static void InitializeSetupPage(Page page)
        {
            MasterPage master = GetMasterPage(page);
            if (master != null)
            {
                master.LeftAreaHtml = null;
                master.BreadcrumbsCenterHtml = null;
                master.BreadcrumbsRightHtml = null;

                master.VisibleHeader
                    = master.VisibleHeaderLinks
                    = master.VisibleBreadcrumbs
                    = master.VisibleFooter
                    = true;

                master.VisibleSearchControl
                    = master.VisibleMainMenu
                    = master.VisibleLeftArea
                    = master.VisibleSubmenu
                    = false;
            }
        }

        internal static void SetPageTitle(Page page, Micajah.Common.Bll.Action action)
        {
            SetPageTitle(page, ((action == null) ? null : action.Name));
        }

        internal static void SetPageTitle(Page page, string title)
        {
            if (FrameworkConfiguration.Current.WebApplication.MasterPage.TitlePrefix)
                page.Title = FrameworkConfiguration.Current.WebApplication.Name;
            else
                page.Title = string.Empty;

            if (!string.IsNullOrEmpty(title))
            {
                if (!string.IsNullOrEmpty(page.Title))
                    page.Title += Resources.MasterPage_Title_Separator;
                page.Title += title;
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Raises the System.Web.UI.Control.Init event.
        /// </summary>
        /// <param name="e">The System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            m_UserContext = UserContext.Current;
            if (m_UserContext != null)
            {
                m_ActionIdList = m_UserContext.ActionIdList;
                m_IsAuthenticated = true;
                m_IsFrameworkAdmin = (m_UserContext.IsFrameworkAdministrator && (m_UserContext.OrganizationId == Guid.Empty));
                if (m_UserContext.OrganizationId != Guid.Empty)
                    m_OrganizationId = m_UserContext.OrganizationId;
                if (m_UserContext.InstanceId != Guid.Empty)
                    m_InstanceId = m_UserContext.InstanceId;
            }

            if (!IsPostBack)
                this.CheckAccessToPage();

            base.OnInit(e);
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            if (this.ActiveAction != null)
            {
                if (m_ActiveAction.AuthenticationRequired)
                {
                    if (this.OrganizationId != m_OrganizationId)
                        Response.Redirect(ResourceProvider.GetActiveOrganizationUrl(Request.Url.PathAndQuery, true));
                    else if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances && m_ActiveAction.InstanceRequired && (this.InstanceId != m_InstanceId))
                        Response.Redirect(ResourceProvider.GetActiveInstanceUrl(Request.Url.PathAndQuery, true));
                }
            }
        }

        protected override object SaveViewState()
        {
            if (!this.IsPostBack)
            {
                this.OrganizationId = m_OrganizationId;
                this.InstanceId = m_InstanceId;
            }

            return base.SaveViewState();
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.Load event.
        /// </summary>
        /// <param name="e">The System.EventArgs object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.CreateCreditCardRegistrationWindow();

            if (this.IsPostBack)
                this.HandleSearchEvent();
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Finds the content place holder on the current master page.
            if (ContentPlaceHolders.Count > 0)
            {
                m_ContentPlaceHolder = FindControl("PageBody") as ContentPlaceHolder;
            }

            if (m_ContentPlaceHolder != null)
            {
                m_ContentPlaceHolder.SetRenderMethodDelegate(this.RenderContentPlaceHolder);
            }

            this.CreateDefaultPageContent();

            if (ActiveAction != null)
            {
                PageTitle = m_ActiveAction.CustomName;

                if (m_ContentPlaceHolder != null)
                {
                    SetAccessToControls(m_ActiveAction, m_ContentPlaceHolder);
                }
            }

            SetPageTitle();

            this.CreateHeaderMessageBox();

            if (this.VisibleHeader)
            {
                m_Header = new Header(this, m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated, m_UserContext);
                Controls.Add(m_Header);
            }

            this.CreateMenus();
            this.CreateBreadCrumbs();
            this.CreateNoticeMessageBox();

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != MasterPageTheme.Modern)
            {
                this.CreateApplicationLogo();

                if (this.VisibleFooter)
                {
                    m_Footer = new Footer(m_UserContext);
                    Controls.Add(m_Footer);
                }
            }
            else
            {
                CustomValidator val = new CustomValidator();
                val.ID = "ValidatorScriptFix";
                val.Display = ValidatorDisplay.Dynamic;
                val.Enabled = false;
                val.ForeColor = System.Drawing.Color.Empty;
                this.Page.Form.Controls.Add(val);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            DisableServerCaching();
            this.CreatePageHeader();
            this.RegisterClientScripts();

            if (this.EnableOverlay)
            {
                this.Page.Form.Attributes["onsubmit"] += " Mp_ShowOverlay();";
                using (HtmlGenericControl div = new HtmlGenericControl("div"))
                {
                    div.Attributes["style"] = string.Format(CultureInfo.InvariantCulture, "position: fixed; display: none; left: 0; top: 0; width: 100%; height: 100%; background: url({0}) repeat; z-index: 999998;", ResourceProvider.GetImageUrl(typeof(MasterPage), "Overlay.gif", true));
                    div.Attributes["id"] = "Mp_Overlay";
                    this.Page.Form.Controls.Add(div);
                }
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "AttachEscapeEvents", "Mp_AttachEscapeEvents();\r\n", true);
            }

            this.Page.Form.Attributes["onsubmit"] += " return true;";

            base.Render(writer);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers the common stylesheet on the page.
        /// </summary>
        /// <param name="page">The System.Web.UI.Page object to register the stylesheet.</param>
        public static void RegisterGlobalStyleSheet(Page page)
        {
            RegisterGlobalStyleSheet(page, FrameworkConfiguration.Current.WebApplication.MasterPage.Theme);
        }

        /// <summary>
        /// Registers the encoding client script ont the page.
        /// </summary>
        /// <param name="page">The System.Web.UI.Page object to register the encoding client script.</param>
        public static void RegisterClientEncodingScript(Page page)
        {
            string script = page.Form.Attributes["onsubmit"];
            if (string.IsNullOrEmpty(script))
                script = string.Empty;

            if (script.IndexOf("Mp_EncodeTextBoxes", StringComparison.OrdinalIgnoreCase) == -1)
                page.Form.Attributes["onsubmit"] += " Mp_EncodeTextBoxes();";

            ScriptManager.RegisterClientScriptInclude(page, page.GetType(), "MasterPageScripts", ResourceProvider.GetResourceUrl("Scripts.MasterPage.js", true));
        }

        public void UpdateBreadcrumbs()
        {
            this.UpdateBreadcrumbs(true);
        }

        public void UpdateBreadcrumbs(bool generate)
        {
            m_UpdateBreadcrumbs = true;
            m_GenerateBreadcrumbs = generate;
        }

        #endregion
    }
}
