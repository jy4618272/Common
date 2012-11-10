using System;
using System.Globalization;
using System.Security.Authentication;
using System.Web;
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
    /// <summary>
    /// Provides user interface (UI) elements for logging in to a Web site.
    /// </summary>
    public class LogOnControl : UserControl
    {
        #region Members

        /// <summary>
        /// The control that contains the title.
        /// </summary>
        protected Control TitleContainer;

        protected Label TitleLabel;

        /// <summary>
        /// The label associated with the text box to input login name.
        /// </summary>
        protected Label LoginLabel;

        /// <summary>
        /// The text box to input login name.
        /// </summary>
        protected TextBox LoginTextBox;

        /// <summary>
        /// The label associated with the text box to input passsword.
        /// </summary>
        protected Label PasswordLabel;

        /// <summary>
        /// The text box to input password.
        /// </summary>
        protected TextBox PasswordTextBox;

        /// <summary>
        /// The login button.
        /// </summary>
        protected IButtonControl LogOnButton;

        /// <summary>
        /// The hyperlink to password recovery page.
        /// </summary>
        protected LinkButton PasswordRecoveryButton;

        /// <summary>
        /// The div to display an error message, if an error occured.
        /// </summary>
        protected HtmlGenericControl ErrorDiv;

        protected Image LogoImage;
        protected HtmlGenericControl MainContainer;
        protected HtmlTable FormTable;
        protected HtmlTable SignupUserTable;
        protected Label SignupUserTitleLabel;
        protected IButtonControl SignupUserButton;
        protected HyperLink HeaderLeftLogoLink;
        protected HyperLink HeaderRightLogoLink;

        private Organization m_Organization;
        private Instance m_Instance;
        private int m_MainContainerHeight;

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the login name from the cookies collection or remembers it in the cookies collection.
        /// </summary>
        internal static string LoginNameInCookie
        {
            get
            {
                string value = null;
                HttpCookie cookie = HttpContext.Current.Request.Cookies["Mc_L"];
                if (cookie != null) value = cookie.Value;
                return value;
            }
            set
            {
                HttpCookieCollection cookies = HttpContext.Current.Response.Cookies;

                HttpCookie cookie = new HttpCookie("Mc_L");
                if (!string.IsNullOrEmpty(value))
                {
                    string loginUrl = WebApplication.LoginProvider.GetLoginUrl();
                    if (!string.IsNullOrEmpty(loginUrl))
                    {
                        string path = WebApplication.CreateApplicationAbsoluteUrl(ResourceProvider.VirtualRootShortPath + "security/");
                        if (loginUrl.IndexOf(path, StringComparison.OrdinalIgnoreCase) > -1)
                            cookie.Path = path;
                        else
                            cookie.Path = WebApplication.CreateApplicationAbsoluteUrl(loginUrl);
                    }
                    cookie.Expires = DateTime.UtcNow.AddYears(1);
                    cookie.HttpOnly = true;
                    cookie.Value = value;
                }
                else
                    cookie.Expires = DateTime.UtcNow.AddYears(-1);

                cookies.Add(cookie);
            }
        }

        #endregion

        #region Public Properties

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
        /// Gets or sets a value indicating whether the embedded stylesheets should be added into page's header.
        /// </summary>
        public bool EnableEmbeddedStyleSheets
        {
            get
            {
                object obj = ViewState["EnableEmbeddedStyleSheets"];
                return ((obj == null) ? true : (bool)obj);
            }
            set { ViewState["EnableEmbeddedStyleSheets"] = value; }
        }

        /// <summary>
        /// Gets or sets the login name.
        /// </summary>
        public string LoginName
        {
            get
            {
                object obj = ViewState["LoginName"];
                if (obj == null)
                {
                    string str = Request.QueryString["l"];
                    if (!string.IsNullOrEmpty(str))
                        obj = str;
                }
                return ((obj == null) ? string.Empty : (string)obj);
            }
            set { ViewState["LoginName"] = value; }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the organization.
        /// </summary>
        public Guid OrganizationId
        {
            get
            {
                object obj = ViewState["OrganizationId"];
                if (obj == null)
                    obj = Support.ConvertStringToType(Request.QueryString["o"], typeof(Guid));
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
            set { ViewState["OrganizationId"] = value; }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the instance.
        /// </summary>
        public Guid InstanceId
        {
            get
            {
                object obj = ViewState["InstanceId"];
                if (obj == null)
                    obj = Support.ConvertStringToType(Request.QueryString["d"], typeof(Guid));
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
            set { ViewState["InstanceId"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL to return after the logging.
        /// </summary>
        public string ReturnUrl
        {
            get
            {
                string returnUrl = null;
                object obj = ViewState["ReturnUrl"];
                if (obj == null)
                    returnUrl = Request.QueryString["returnurl"];
                else
                    returnUrl = (string)obj;
                return ((returnUrl == null) ? string.Empty : returnUrl);
            }
            set { ViewState["ReturnUrl"] = value; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the values of the properties is initialized from current Web request.
        /// </summary>
        public event EventHandler InitParameters;

        #endregion

        #region Private Methods

        private void Authenticate()
        {
            string password = Request.QueryString["p"];
            string isPersistentString = Request.QueryString["cp"];
            string redirectUrl = this.ReturnUrl;
            string loginName = this.LoginName;
            bool isPersistent = true;

            if (isPersistentString != null)
            {
                if (!Boolean.TryParse(isPersistentString, out isPersistent)) isPersistent = false;
            }

            if (!(string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password) || (this.OrganizationId == Guid.Empty)))
            {
                try
                {
                    (new LoginProvider()).SignOut(true, false);

                    if (string.Compare(Request.QueryString["on"], bool.TrueString, StringComparison.OrdinalIgnoreCase) == 0)
                        WebApplication.RefreshCommonDataSet(true);

                    WebApplication.LoginProvider.Authenticate(loginName, Support.Decrypt(password), false, isPersistent, this.OrganizationId, this.InstanceId);

                    ActiveInstanceControl.ValidateRedirectUrl(ref redirectUrl, true);
                    if (string.IsNullOrEmpty(redirectUrl))
                        redirectUrl = "~/";

                    if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
                    {
                        string url = CustomUrlProvider.GetVanityUrl(this.OrganizationId, this.InstanceId);
                        url = url.ToLower().Replace("https://", string.Empty).Replace("http://", string.Empty);

                        if (!string.IsNullOrEmpty(url) && string.Compare(System.Web.HttpContext.Current.Request.Url.Host, url, true) != 0)
                        {
                            Security.UserContext.SelectedOrganizationId = Guid.Empty;
                            Security.UserContext.SelectedInstanceId = Guid.Empty;
                            Security.UserContext.Current = null;

                            if (redirectUrl != "~/")
                                Response.Redirect(string.Format("{0}{1}{2}{3}", Request.Url.Scheme, Uri.SchemeDelimiter, url, redirectUrl));
                            else
                                Response.Redirect(string.Format("{0}{1}{2}{3}", Request.Url.Scheme, Uri.SchemeDelimiter, url, ResolveUrl(redirectUrl)));
                        }
                        else
                        {
                            Security.UserContext.SelectedOrganizationId = this.OrganizationId;
                            Security.UserContext.SelectedInstanceId = this.InstanceId;
                        }
                    }
                    else
                        Response.Redirect(redirectUrl);

                }
                catch (AuthenticationException ex)
                {
                    ShowErrorMessage(ex.Message);
                }
            }
        }

        private void InitializeControls()
        {
            ErrorDiv.EnableViewState = false;
            ErrorDiv.Visible = false;

            LoginTextBox.Required = true;
            LoginTextBox.ShowRequired = false;

            PasswordTextBox.Required = true;
            PasswordTextBox.ShowRequired = false;
            PasswordTextBox.TextMode = TextBoxMode.Password;

            LogOnButton.Click += new EventHandler(LogOnButton_Click);

            PasswordRecoveryButton.CausesValidation = false;
            PasswordRecoveryButton.Click += new EventHandler(PasswordRecoveryButton_Click);

            SignupUserTable.Visible = false;

            SignupUserButton.CausesValidation = false;
            Button btn = SignupUserButton as Button;
            if (btn != null) btn.UseSubmitBehavior = false;
            SignupUserButton.Click += new EventHandler(SignupUserButton_Click);

            if (HeaderLeftLogoLink != null)
            {
                HeaderLeftLogoLink.Target = "_blank";
                HeaderLeftLogoLink.Visible = false;
            }

            if (HeaderRightLogoLink != null)
                HeaderRightLogoLink.Visible = false;
        }

        /// <summary>
        /// Initializes the values of the properties from current Web request.
        /// </summary>
        private void InitializeParameters()
        {
            if (this.InitParameters != null)
                this.InitParameters(this, EventArgs.Empty);
        }

        private void CheckSignupUser()
        {
            if ((this.OrganizationId == Guid.Empty) || (this.InstanceId == Guid.Empty)) return;

            Organization organization = OrganizationProvider.GetOrganization(this.OrganizationId);

            if (organization != null)
            {
                if (organization.Deleted)
                {
                    ShowErrorMessage(Resources.LogOnControl_OrganizationIsDeleted);
                    return;
                }
                else if (!organization.Active)
                {
                    ShowErrorMessage(Resources.LogOnControl_OrganizationIsInactive);
                    return;
                }
            }
            else
            {
                ShowErrorMessage(Resources.LogOnControl_OrganizationIsNotFound);
                return;
            }

            Instance instance = InstanceProvider.GetInstance(this.InstanceId, this.OrganizationId);
            if (instance == null) return;

            if (!instance.Active)
            {
                ShowErrorMessage(Resources.LogOnControl_InstanceIsInactive);
                return;
            }

            this.OrganizationId = instance.OrganizationId;
            m_Organization = organization;
            m_Instance = instance;

            if (!instance.EnableSignupUser) return;

            Guid groupId = GroupProvider.GetGroupIdOfLowestRoleInInstance(instance);
            if (groupId != Guid.Empty)
            {
                m_MainContainerHeight = 300;
                SignupUserTable.Visible = true;
            }
            else
                SignupUserButton.CommandArgument = string.Empty;
        }

        private void LoadLogos()
        {
            if (HeaderLeftLogoLink != null)
            {
                if (m_Organization == null)
                {
                    if (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.Copyright.CompanyLogoImageUrl))
                    {
                        HeaderLeftLogoLink.ImageUrl = WebApplication.CreateApplicationAbsoluteUrl(FrameworkConfiguration.Current.WebApplication.Copyright.CompanyLogoImageUrl);
                        HeaderLeftLogoLink.ToolTip = FrameworkConfiguration.Current.WebApplication.Copyright.CompanyName;
                        HeaderLeftLogoLink.Visible = true;
                    }
                    else
                    {
                        HeaderLeftLogoLink.Text = FrameworkConfiguration.Current.WebApplication.Copyright.CompanyName;
                        HeaderLeftLogoLink.Visible = true;
                    }
                }
                else
                    CreateLogo(m_Organization, m_Instance, ref HeaderLeftLogoLink);
            }

            if (HeaderRightLogoLink != null)
            {
                if (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.LogoImageUrl))
                {
                    HeaderRightLogoLink.ImageUrl = WebApplication.CreateApplicationAbsoluteUrl(FrameworkConfiguration.Current.WebApplication.LogoImageUrl);
                    HeaderRightLogoLink.ToolTip = FrameworkConfiguration.Current.WebApplication.Name;
                }
                else
                    HeaderRightLogoLink.Text = FrameworkConfiguration.Current.WebApplication.Name;
                HeaderRightLogoLink.Visible = true;
            }

            if ((FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != Pages.MasterPageTheme.Modern) || string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.BigLogoImageUrl))
                LogoImage.Visible = false;
            else
            {
                LogoImage.ImageUrl = FrameworkConfiguration.Current.WebApplication.BigLogoImageUrl;
                m_MainContainerHeight += 100;
            }
        }

        private void LoadResources()
        {
            TitleLabel.Text = FrameworkConfiguration.Current.WebApplication.Login.TitleLabelText;
            PasswordLabel.Text = FrameworkConfiguration.Current.WebApplication.Login.PasswordLabelText;
            LogOnButton.Text = FrameworkConfiguration.Current.WebApplication.Login.LoginButtonText;
            PasswordRecoveryButton.Text = Resources.LogOnControl_PasswordRecoveryLink_Text;
            PasswordRecoveryButton.Visible = FrameworkConfiguration.Current.WebApplication.Password.EnablePasswordRetrieval;

            if (FrameworkConfiguration.Current.WebApplication.EnableLdap)
                LoginLabel.Text = Resources.LoginElement_LdapLoginLabelText;
            else
            {
                LoginLabel.Text = FrameworkConfiguration.Current.WebApplication.Login.LoginLabelText;
                LoginTextBox.ValidationExpression = FrameworkConfiguration.Current.WebApplication.Login.LoginValidationExpression;
                LoginTextBox.ValidationType = Micajah.Common.WebControls.CustomValidationDataType.RegularExpression;
            }

            SignupUserTitleLabel.Text = Resources.LogOnControl_SignupUserTitleLabel_Text;
            SignupUserButton.Text = Resources.LogOnControl_SignupUserButton_Text;
        }

        /// <summary>
        /// Renders the control and the logo of the copyright holder company.
        /// </summary>
        /// <param name="writer">The System.Web.UI.HtmlTextWriter to render content to.</param>
        /// <param name="control">The System.Web.UI.Control to render.</param>
        private static void RenderHeader(HtmlTextWriter writer, Control control)
        {
            if (writer == null) return;

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Mp_Hdr");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "A");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            if (control != null) control.RenderControl(writer);
            writer.RenderEndTag();

            writer.AddStyleAttribute("line-height", "normal !important");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "G");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            using (HyperLink link = new HyperLink())
            {
                if (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.LogoImageUrl))
                {
                    link.ImageUrl = WebApplication.CreateApplicationAbsoluteUrl(FrameworkConfiguration.Current.WebApplication.LogoImageUrl);
                    link.ToolTip = FrameworkConfiguration.Current.WebApplication.Name;
                }
                else
                    link.Text = FrameworkConfiguration.Current.WebApplication.Name;
                link.RenderControl(writer);
            }
            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        private static void CreateLogo(Organization org, Instance inst, ref HyperLink link)
        {
            if (org != null)
            {
                string headerLogoImageUrl = string.Empty;
                string headerLogoText = string.Empty;
                string headerLogoNavigateUrl = string.Empty;

                if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                {
                    if (inst != null)
                    {
                        headerLogoImageUrl = inst.LogoImageUrl;

                        if (org.Instances.Count > 1)
                            headerLogoText = inst.Name;
                    }
                }

                if (string.IsNullOrEmpty(headerLogoImageUrl))
                    headerLogoImageUrl = org.LogoImageUrl;

                if (string.IsNullOrEmpty(headerLogoText))
                    headerLogoText = org.Name;
                else
                    headerLogoText = org.Name + Html32TextWriter.SpaceChar + headerLogoText;

                headerLogoNavigateUrl = org.WebsiteUrl;

                if (link == null)
                    link = new HyperLink();

                if (!string.IsNullOrEmpty(headerLogoNavigateUrl))
                {
                    link.NavigateUrl = headerLogoNavigateUrl;
                    if (string.IsNullOrEmpty(headerLogoImageUrl))
                        link.Text = headerLogoText;
                    else
                        link.ImageUrl = headerLogoImageUrl;
                    link.ToolTip = headerLogoText;
                    link.Target = "_blank";
                    link.Visible = true;
                }
                else
                {
                    if (string.IsNullOrEmpty(headerLogoImageUrl))
                    {
                        link.Text = headerLogoText;
                        link.Visible = true;
                    }
                    else
                    {
                        link.ImageUrl = headerLogoImageUrl;
                        link.ToolTip = headerLogoText;
                        link.Visible = true;
                    }
                }
            }
        }

        private void PasswordRecoveryButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(WebApplication.LoginProvider.GetPasswordRecoveryUrl((!string.IsNullOrEmpty(LoginTextBox.Text) && (string.Compare(LoginTextBox.Text, LoginNameInCookie, StringComparison.OrdinalIgnoreCase) != 0)) ? LoginTextBox.Text : null));
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs when the login button is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected void LogOnButton_Click(object sender, EventArgs e)
        {
            try
            {
                string loginName = LoginTextBox.Text;
                if (WebApplication.LoginProvider.Authenticate(loginName, PasswordTextBox.Text, true, true, this.OrganizationId, this.InstanceId))
                {
                    LoginNameInCookie = loginName;

                    string redirectUrl = this.ReturnUrl;
                    ActiveInstanceControl.ValidateRedirectUrl(ref redirectUrl, true);

                    Response.Redirect(string.IsNullOrEmpty(redirectUrl) ? ResourceProvider.ActiveOrganizationPageVirtualPath : redirectUrl);
                }
            }
            catch (AuthenticationException ex)
            {
                this.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Occurs when the sign up user button is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected void SignupUserButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format(CultureInfo.InvariantCulture, "{0}?o={1:N}&d={2:N}", ResourceProvider.SignupUserPageVirtualPath, this.OrganizationId, this.InstanceId));
        }

        #endregion

        #region Overriden Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.IsPostBack)
                this.Authenticate();

            this.InitializeControls();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.InitializeParameters();

            if (!this.IsPostBack)
            {
                Micajah.Common.Pages.MasterPage.SetPageTitle(this.Page, ActionProvider.GlobalNavigationLinks.FindByActionId(ActionProvider.LoginGlobalNavigationLinkActionId));

                if (string.Compare(Request.QueryString["ac"], "1", StringComparison.OrdinalIgnoreCase) == 0)
                    ShowErrorMessage(Resources.LogOnControl_YouAreLoggedFromAnotherComputer);

                m_MainContainerHeight = 220;

                this.CheckSignupUser();
                this.LoadResources();
                this.LoadLogos();

                string loginName = this.LoginName;
                if (string.IsNullOrEmpty(loginName)) this.LoginName = loginName = LoginNameInCookie;
                if (!string.IsNullOrEmpty(loginName)) LoginTextBox.Text = loginName;
            }

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
            {
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnModernStyleSheet, true)));

                MagicForm.ApplyStyle(FormTable);
            }
            else
                this.Page.Header.Controls.Add(Support.CreateStyleSheetLink(ResourceProvider.GetResourceUrl(ResourceProvider.LogOnStyleSheet, true)));

            if (LoginTextBox.Text.Length > 0)
                PasswordTextBox.Focus();
            else
                LoginTextBox.Focus();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.EnableEmbeddedStyleSheets)
                Micajah.Common.Pages.MasterPage.CreatePageHeader(this.Page, this.EnableClientCaching);
            else if (!this.EnableClientCaching)
                Micajah.Common.Pages.MasterPage.DisableClientCaching(this.Page);

            TitleContainer.Visible = ((!string.IsNullOrEmpty(TitleLabel.Text)) || LogoImage.Visible);
            TitleLabel.Visible = (!LogoImage.Visible);

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
                ResourceProvider.RegisterValidatorScriptResource(this.Page);

            if (m_MainContainerHeight > 0 && (MainContainer != null))
            {
                MainContainer.Style[HtmlTextWriterStyle.Height] = m_MainContainerHeight.ToString(CultureInfo.InvariantCulture) + "px";
                MainContainer.Style[HtmlTextWriterStyle.MarginTop] = (-Convert.ToInt32(m_MainContainerHeight / 2)).ToString(CultureInfo.InvariantCulture) + "px";
            }
        }

        #endregion

        #region Public Methods

        public void ShowErrorMessage(string message)
        {
            ErrorDiv.Visible = true;
            ErrorDiv.InnerHtml = message;
        }

        /// <summary>
        /// Renders the application logo and the logo of the copyright holder company.
        /// </summary>
        /// <param name="writer">The System.Web.UI.HtmlTextWriter to render content to.</param>
        public static void RenderHeader(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.Copyright.CompanyLogoImageUrl))
            {
                using (Image img = new Image())
                {
                    img.ImageUrl = WebApplication.CreateApplicationAbsoluteUrl(FrameworkConfiguration.Current.WebApplication.Copyright.CompanyLogoImageUrl);
                    img.ToolTip = FrameworkConfiguration.Current.WebApplication.Copyright.CompanyName;

                    RenderHeader(writer, img);
                }
            }
            else
            {
                using (LiteralControl literal = new LiteralControl(FrameworkConfiguration.Current.WebApplication.Copyright.CompanyName))
                {
                    RenderHeader(writer, literal);
                }
            }
        }

        /// <summary>
        /// Renders the logo of the specified instance or organization and the logo of the copyright holder company.
        /// </summary>
        /// <param name="writer">The System.Web.UI.HtmlTextWriter to render content to.</param>
        /// <param name="organizationId">The identifier of the organization to render logo of.</param>
        /// <param name="organizationId">The identifier of the instance to render logo of.</param>
        public static void RenderHeader(HtmlTextWriter writer, Guid organizationId, Guid instanceId)
        {
            if (organizationId != Guid.Empty)
            {
                Organization org = new Organization();
                if (org.Load(organizationId))
                {
                    Instance inst = null;
                    if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                    {
                        if (instanceId != Guid.Empty)
                        {
                            inst = new Instance();
                            if (!inst.Load(organizationId, instanceId))
                                inst = null;
                        }
                    }

                    HyperLink link = null;

                    CreateLogo(org, inst, ref link);

                    if (link != null)
                        RenderHeader(writer, link);
                }
                else
                    RenderHeader(writer);
            }
            else
                RenderHeader(writer);
        }

        #endregion
    }
}
