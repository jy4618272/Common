using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using System;
using System.Collections;
using System.Globalization;
using System.Security.Authentication;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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
        protected HtmlGenericControl ErrorPanel;

        protected HtmlGenericControl LogoImagePanel;
        protected Image LogoImage;
        protected HtmlGenericControl MainContainer;
        protected HtmlTable FormTable;
        protected HtmlTable SignupUserTable;
        protected Label SignupUserTitleLabel;
        protected IButtonControl SignupUserButton;
        protected HyperLink HeaderLeftLogoLink;
        protected HyperLink HeaderRightLogoLink;
        protected HyperLink LogOnViaGoogleLink;
        protected HtmlGenericControl LinkEmailPanel;
        protected Label LinkEmailLabel;
        protected LinkButton LinkEmailButton;
        protected LinkButton CancelLinkEmailButton;
        protected Label OrLabel1;
        protected LinkButton LogOffLink;

        private Organization m_Organization;
        private Instance m_Instance;
        private int m_MainContainerHeight;
        private string m_EmailToLink;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the class. 
        /// </summary>
        public LogOnControl()
        {
            EnableCustomHandling = true;
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

        /// <summary>
        /// Gets or sets the password recovery URL
        /// </summary>
        public string PasswordRecoveryUrl
        {
            get
            {
                if (ViewState["PasswordRecoveryUrl"] != null)
                {
                    return ViewState["PasswordRecoveryUrl"].ToString();
                }
                return "";
            }
            set { ViewState["PasswordRecoveryUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the Register New User Account URL
        /// </summary>
        public string SignupUserUrl
        {
            get
            {
                if (ViewState["SignupUserUrl"] != null)
                {
                    return ViewState["SignupUserUrl"].ToString();
                }
                return "";
            }
            set { ViewState["SignupUserUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the custom handling is enabled on the page that contains this control.
        /// </summary>
        public bool EnableCustomHandling { get; set; }

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
            string loginName = this.LoginName;
            bool isPersistent = true;

            if (isPersistentString != null)
            {
                if (!Boolean.TryParse(isPersistentString, out isPersistent)) isPersistent = false;
            }

            Guid organizationId = this.OrganizationId;
            Guid instanceId = this.InstanceId;
            bool isCustomUrlEnabled = FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled;

            if (isCustomUrlEnabled)
            {
                if (organizationId == Guid.Empty)
                {
                    string host = Request.Url.Host;
                    if (!CustomUrlProvider.IsDefaultVanityUrl(host))
                        CustomUrlProvider.ParseHost(host, ref organizationId, ref instanceId);
                }
                else if (!GoogleProvider.IsGoogleProviderRequest(Request))
                {
                    this.VerifyVanityUrl(organizationId, instanceId);
                }
            }

            if (!(string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password) || (organizationId == Guid.Empty)))
            {
                if (!isCustomUrlEnabled)
                {
                    LoginProvider loginProvider = new LoginProvider();
                    loginProvider.SignOut(true, false);
                }

                try
                {
                    LoginProvider.Current.Authenticate(loginName, Support.Decrypt(password), false, isPersistent, organizationId, instanceId);

                    this.RedirectAfterLogOn();
                }
                catch (AuthenticationException ex)
                {
                    ShowErrorMessage(ex.Message);
                }
            }
            else
            {
                if (GoogleProvider.IsGoogleProviderRequest(Request))
                {
                    try
                    {
                        string accessToken = GoogleProvider.ProcessAuthorization(this.Context, ref organizationId, ref instanceId);

                        string firstName = null;
                        string lastName = null;
                        GoogleProvider.GetUserProfile(accessToken, out loginName, out firstName, out lastName);
                    }
                    catch (AuthenticationException ex)
                    {
                        ShowErrorMessage(ex.Message);
                    }

                    if (!string.IsNullOrEmpty(loginName))
                    {
                        m_EmailToLink = loginName;

                        string message = null;

                        try
                        {
                            string domain = GoogleProvider.GetDomain(Request);
                            if (!string.IsNullOrEmpty(domain))
                            {
                                EmailSuffixProvider.ParseEmailSuffixName(domain, ref organizationId, ref instanceId);
                            }

                            if (LoginProvider.Current.Authenticate(loginName, null, false, true, organizationId, instanceId))
                            {
                                this.RedirectAfterLogOn();
                            }
                        }
                        catch (AuthenticationException ex)
                        {
                            message = ex.Message;
                        }

                        if (!string.IsNullOrEmpty(message))
                        {
                            if (LoginProvider.Current.GetLogin(loginName) == null)
                            {
                                message = string.Format(CultureInfo.InvariantCulture, Resources.UserContext_ErrorMessage_YourAccountIsNotFound, loginName);
                            }
                        }

                        if (!string.IsNullOrEmpty(message))
                        {
                            this.ShowErrorMessage(message);

                            this.EnableCustomHandling = false;
                        }
                    }
                }
            }
        }

        private void InitializeControls()
        {
            ErrorPanel.EnableViewState = false;

            LoginTextBox.Required = true;
            LoginTextBox.ShowRequired = false;

            PasswordTextBox.Required = true;
            PasswordTextBox.ShowRequired = false;
            PasswordTextBox.TextMode = TextBoxMode.Password;

            LogOnButton.Click += new EventHandler(LogOnButton_Click);

            LogOnViaGoogleLink.Visible = FrameworkConfiguration.Current.WebApplication.Integration.Google.Enabled;

            PasswordRecoveryButton.CausesValidation = false;
            PasswordRecoveryButton.Click += new EventHandler(PasswordRecoveryButton_Click);

            SignupUserTable.Visible = false;

            SignupUserButton.CausesValidation = false;
            Button btn = SignupUserButton as Button;
            if (btn != null) btn.UseSubmitBehavior = false;
            SignupUserButton.Click += new EventHandler(SignupUserButton_Click);

            LinkEmailPanel.Visible = false;
            LinkEmailButton.Click += new EventHandler(LinkEmailButton_Click);
            CancelLinkEmailButton.Click += new EventHandler(CancelLinkEmailButton_Click);

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

            if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled)
            {
                this.VerifyVanityUrl(this.OrganizationId, this.InstanceId);
            }
        }

        private void CheckSignupUser()
        {
            if ((this.OrganizationId == Guid.Empty) || (this.InstanceId == Guid.Empty))
            {
                Guid orgId = Guid.Empty;
                Guid insId = Guid.Empty;
                if (FrameworkConfiguration.Current.WebApplication.CustomUrl.Enabled && m_Organization == null)
                {
                    if (!CustomUrlProvider.IsDefaultVanityUrl(Request.Url.Host))
                        CustomUrlProvider.ParseHost(Request.Url.Host, ref orgId, ref insId);
                }

                this.OrganizationId = orgId;
                this.InstanceId = insId;

                if ((this.OrganizationId == Guid.Empty) || (this.InstanceId == Guid.Empty)) return;
            }

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

            Guid groupId = GroupProvider.GetGroupIdOfLowestRoleInInstance(instance.OrganizationId, instance.InstanceId);
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
            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != Pages.MasterPageTheme.Modern)
            {
                if (HeaderLeftLogoLink != null)
                {
                    if (m_Organization == null)
                    {
                        if (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.Copyright.CompanyLogoImageUrl))
                        {
                            HeaderLeftLogoLink.ImageUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(FrameworkConfiguration.Current.WebApplication.Copyright.CompanyLogoImageUrl);
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
                        HeaderRightLogoLink.ImageUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(FrameworkConfiguration.Current.WebApplication.LogoImageUrl);
                        HeaderRightLogoLink.ToolTip = FrameworkConfiguration.Current.WebApplication.Name;
                    }
                    else
                        HeaderRightLogoLink.Text = FrameworkConfiguration.Current.WebApplication.Name;
                    HeaderRightLogoLink.Visible = true;
                }
            }

            if ((FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != Pages.MasterPageTheme.Modern) || string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.MobileLogoImageUrl))
            {
                if (LogoImagePanel != null) LogoImagePanel.Visible = false;
            }
            else
            {
                if (LogoImagePanel != null)
                {
                    LogoImagePanel.Visible = true;
                    if (LogoImage != null)
                    {
                        LogoImage.ImageUrl = FrameworkConfiguration.Current.WebApplication.MobileLogoImageUrl;
                        if (FrameworkConfiguration.Current.WebApplication.BigLogoImageHeight > 0)
                            m_MainContainerHeight += FrameworkConfiguration.Current.WebApplication.MobileLogoImageHeight;
                    }
                }
            }
        }

        private void LoadResources()
        {
            if (TitleLabel != null) TitleLabel.Text = FrameworkConfiguration.Current.WebApplication.Login.TitleLabelText;
            if (PasswordLabel != null) PasswordLabel.Text = FrameworkConfiguration.Current.WebApplication.Login.PasswordLabelText;
            LogOnButton.Text = FrameworkConfiguration.Current.WebApplication.Login.LoginButtonText;
            LogOnViaGoogleLink.Text = Resources.LogOnControl_LogOnViaGoogleLink_Text;
            PasswordRecoveryButton.Text = Resources.LogOnControl_PasswordRecoveryLink_Text;
            PasswordRecoveryButton.Visible = FrameworkConfiguration.Current.WebApplication.Password.EnablePasswordRetrieval;

            if (FrameworkConfiguration.Current.WebApplication.Integration.Ldap.Enabled)
            {
                if (LoginLabel != null) LoginLabel.Text = Resources.LoginElement_LdapLoginLabelText;
            }
            else
            {
                if (LoginLabel != null) LoginLabel.Text = FrameworkConfiguration.Current.WebApplication.Login.LoginLabelText;
                LoginTextBox.ValidationExpression = FrameworkConfiguration.Current.WebApplication.Login.LoginValidationExpression;
                LoginTextBox.ValidationType = Micajah.Common.WebControls.CustomValidationDataType.RegularExpression;
            }

            SignupUserTitleLabel.Text = Resources.LogOnControl_SignupUserTitleLabel_Text;
            SignupUserButton.Text = Resources.LogOnControl_SignupUserButton_Text;

            LinkEmailButton.Text = Resources.LogOnControl_LinkEmailButton_Text;
            CancelLinkEmailButton.Text = Resources.LogOnControl_CancelLinkEmailButton_Text;
            OrLabel1.Text = Resources.LogOnControl_OrLabel1_Text;
            LogOffLink.Text = Resources.LogOnControl_LogOffLink_Text;
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
                    link.ImageUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(FrameworkConfiguration.Current.WebApplication.LogoImageUrl);
                    link.ToolTip = FrameworkConfiguration.Current.WebApplication.Name;
                }
                else
                    link.Text = FrameworkConfiguration.Current.WebApplication.Name;
                link.RenderControl(writer);
            }
            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        private void RedirectAfterLogOn()
        {
            string redirectUrl = this.ReturnUrl;
            ActiveInstanceControl.ValidateRedirectUrl(ref redirectUrl, true);

            Response.Redirect(string.IsNullOrEmpty(redirectUrl) ? ResourceProvider.ActiveOrganizationPageVirtualPath : redirectUrl);
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
                        headerLogoImageUrl = InstanceProvider.GetInstanceLogoImageUrlFromCache(inst.InstanceId, inst.OrganizationId);
                        headerLogoText = inst.Name;
                    }
                }

                if (string.IsNullOrEmpty(headerLogoImageUrl))
                    headerLogoImageUrl = OrganizationProvider.GetOrganizationLogoImageUrlFromCache(org.OrganizationId);

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
            if (!string.IsNullOrEmpty(PasswordRecoveryUrl))
            {
                Response.Redirect(PasswordRecoveryUrl);
            }
            else
            {
                string url = LoginProvider.Current.GetPasswordRecoveryUrl(((!string.IsNullOrEmpty(LoginTextBox.Text)) ? LoginTextBox.Text : null), false);
                Response.Redirect(url);
            }
        }

        private void LinkEmailButton_Click(object sender, EventArgs e)
        {
            EmailProvider.InsertEmail(m_EmailToLink, LoginProvider.Current.GetLoginId(LoginTextBox.Text));

            this.RedirectAfterLogOn();
        }

        private void CancelLinkEmailButton_Click(object sender, EventArgs e)
        {
            this.RedirectAfterLogOn();
        }

        private void VerifyVanityUrl(Guid organizationId, Guid instanceId)
        {
            if (organizationId != Guid.Empty)
            {
                string vanityUrl = CustomUrlProvider.GetVanityUrl(organizationId, instanceId);

                if (!string.IsNullOrEmpty(vanityUrl) && (string.Compare(Request.Url.Host, vanityUrl, StringComparison.OrdinalIgnoreCase) != 0))
                {
                    string redirectUrl = this.ReturnUrl;

                    if (string.IsNullOrEmpty(redirectUrl))
                        redirectUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(Request.Url.PathAndQuery);
                    else
                    {
                        redirectUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(Request.Url.PathAndQuery)
                            + ((Request.QueryString["returnurl"] == null)
                                ? ((Request.Url.PathAndQuery.IndexOf("&", StringComparison.OrdinalIgnoreCase) > -1) ? "&" : "?") + "returnurl=" + HttpUtility.UrlEncode(redirectUrl)
                                : string.Empty);
                    }

                    Response.Redirect(string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", Request.Url.Scheme, Uri.SchemeDelimiter, vanityUrl, redirectUrl));
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs when the login button is clicked.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected void LogOnButton_Click(object sender, EventArgs e)
        {
            string loginName = LoginTextBox.Text;

            try
            {
                if (LoginProvider.Current.Authenticate(loginName, PasswordTextBox.Text, true, true, this.OrganizationId, this.InstanceId))
                {
                    if (!string.IsNullOrEmpty(m_EmailToLink))
                    {
                        if (!EmailProvider.IsEmailExists(m_EmailToLink))
                        {
                            LinkEmailLabel.Text = string.Format(CultureInfo.InvariantCulture, Resources.LogOnControl_LinkEmailLabel_Text, loginName, m_EmailToLink);

                            LinkEmailPanel.Visible = true;
                            FormTable.Visible = false;
                            if (LogoImagePanel != null)
                                LogoImagePanel.Visible = false;

                            m_MainContainerHeight = 150;
                        }
                        else
                            this.RedirectAfterLogOn();
                    }
                    else
                        this.RedirectAfterLogOn();
                }
            }
            catch (AuthenticationException ex)
            {
                string message = ex.Message;

                if (FrameworkConfiguration.Current.WebApplication.Integration.Google.Enabled)
                {
                    IList domains = null;

                    if (string.Compare(ex.Message, FrameworkConfiguration.Current.WebApplication.Login.FailureText, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        Organization org = OrganizationProvider.GetOrganization(this.OrganizationId);

                        if (org == null)
                        {
                            OrganizationCollection orgs = LoginProvider.Current.GetOrganizationsByLoginName(loginName);
                            if ((orgs != null) && (orgs.Count > 0))
                                org = orgs[0];
                        }

                        if (org != null)
                        {
                            if (SettingProvider.OrganizationProviderIsGoogle(org.OrganizationId))
                            {
                                domains = EmailSuffixProvider.GetEmailSuffixesList(org.OrganizationId);
                            }
                        }
                    }

                    if (domains == null)
                    {
                        domains = FrameworkConfiguration.Current.WebApplication.Integration.Google.MailDomains;
                    }

                    if (domains != null)
                    {
                        foreach (string domain in domains)
                        {
                            if (loginName.IndexOf(domain, StringComparison.OrdinalIgnoreCase) != -1)
                            {
                                message = Resources.LoginElement_GoogleFailureText;
                                break;
                            }
                        }
                    }
                }

                this.ShowErrorMessage(message);
            }
        }

        /// <summary>
        /// Occurs when the sign up user button is clicked.
        /// </summary>
        /// <param name="sender">The sourceRow of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        protected void SignupUserButton_Click(object sender, EventArgs e)
        {
            string url = ResourceProvider.SignupUserPageVirtualPath;
            if (!string.IsNullOrEmpty(this.SignupUserUrl))
            {
                url = this.SignupUserUrl;
            }

            StringBuilder sb = new StringBuilder("{0}");
            if (url.IndexOf("?", StringComparison.OrdinalIgnoreCase) == -1)
            {
                sb.Append("?");
            }
            else
            {
                sb.Append("&");
            }
            sb.Append("o={1:N}&d={2:N}");
            string format = sb.ToString();

            url = string.Format(CultureInfo.InvariantCulture, format, url, this.OrganizationId, this.InstanceId);

            Response.Redirect(url);
        }

        protected void LogOffLink_Click(object sender, EventArgs e)
        {
            LoginProvider.Current.SignOut();
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

                if (!string.IsNullOrEmpty(this.LoginName))
                    LoginTextBox.Text = this.LoginName;
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

            if (LogOnViaGoogleLink.Visible)
            {
                LogOnViaGoogleLink.NavigateUrl = GoogleProvider.GetLoginUrl(this.OrganizationId, this.InstanceId);
            }

            bool modernTheme = (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern);

            if (this.EnableEmbeddedStyleSheets)
                Micajah.Common.Pages.MasterPage.CreatePageHeader(this.Page, this.EnableClientCaching, true, true, modernTheme, modernTheme, false);
            else if (!this.EnableClientCaching)
                Micajah.Common.Pages.MasterPage.DisableClientCaching(this.Page);

            if (TitleLabel != null)
                TitleContainer.Visible = (!string.IsNullOrEmpty(TitleLabel.Text));
            ErrorPanel.Visible = (!string.IsNullOrEmpty(ErrorPanel.InnerHtml));

            if (modernTheme)
                ResourceProvider.RegisterValidatorScriptResource(this.Page);

            if (m_MainContainerHeight > 0 && (MainContainer != null))
            {
                MainContainer.Style[HtmlTextWriterStyle.Height] = m_MainContainerHeight.ToString(CultureInfo.InvariantCulture) + "px";
                MainContainer.Style[HtmlTextWriterStyle.MarginTop] = (-Convert.ToInt32(m_MainContainerHeight / 2)).ToString(CultureInfo.InvariantCulture) + "px";
            }
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            m_EmailToLink = (string)this.ViewState["EmailToLink"];
        }

        protected override object SaveViewState()
        {
            if (!this.IsPostBack)
            {
                if (!string.IsNullOrEmpty(m_EmailToLink))
                {
                    this.ViewState["EmailToLink"] = m_EmailToLink;
                }
            }

            return base.SaveViewState();
        }

        #endregion

        #region Public Methods

        public void ShowErrorMessage(string message)
        {
            ErrorPanel.InnerHtml = message;
        }

        /// <summary>
        /// Renders the application logo and the logo of the copyright holder company.
        /// </summary>
        /// <param name="writer">The System.Web.UI.HtmlTextWriter to render content to.</param>
        public static void RenderHeader(HtmlTextWriter writer)
        {
            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
                return;

            if (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.Copyright.CompanyLogoImageUrl))
            {
                using (Image img = new Image())
                {
                    img.ImageUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(FrameworkConfiguration.Current.WebApplication.Copyright.CompanyLogoImageUrl);
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
            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
                return;

            if (organizationId != Guid.Empty)
            {
                Organization org = OrganizationProvider.GetOrganization(organizationId);
                if (org != null)
                {
                    Instance inst = null;
                    if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                    {
                        if (instanceId != Guid.Empty)
                            inst = InstanceProvider.GetInstance(instanceId, organizationId);
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
