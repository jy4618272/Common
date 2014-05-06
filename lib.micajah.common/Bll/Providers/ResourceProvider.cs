using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.WebControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Micajah.Common.Bll.Providers
{
    /// <summary>
    /// The class provides the methods to work with resources.
    /// </summary>
    public static class ResourceProvider
    {
        #region Members

        #region Admin Pages

        internal const string GroupsPageVirtualPath = AdminVirtualRootShortPath + "group.aspx";
        internal const string GroupsInstancesRolesPageVirtualPath = AdminVirtualRootShortPath + "groupsinstancesroles.aspx";
        internal const string GroupSettingsInInstancePageVirtualPath = AdminVirtualRootShortPath + "groupsettingsininstance.aspx";
        internal const string InstancePageVirtualPath = AdminVirtualRootShortPath + "instance.aspx";
        internal const string InviteUsersPageVirtualPath = AdminVirtualRootShortPath + "inviteusers.aspx";
        internal const string UsersPageVirtualPath = AdminVirtualRootShortPath + "user.aspx";
        internal const string SettingsPageVirtualPath = AdminVirtualRootShortPath + "settings.aspx";
        internal const string AccountSettingsVirtualPath = AdminVirtualRootShortPath + "accountsettings.aspx";

        #endregion

        #region Security Pages

        internal const string LogOnPageVirtualPath = VirtualRootShortPath + "login.aspx";
        internal const string LogOffPageVirtualPath = VirtualRootShortPath + "logoff.aspx";
        internal const string ActiveOrganizationPageVirtualPath = VirtualRootShortPath + "activeorganization.aspx";
        internal const string ActiveInstancePageVirtualPath = VirtualRootShortPath + "activeinstance.aspx";
        internal const string PasswordRecoveryPageVirtualPath = VirtualRootShortPath + "passwordrecovery.aspx";
        internal const string ResetPasswordPageVirtualPath = VirtualRootShortPath + "resetpassword.aspx";
        internal const string SignupUserPageVirtualPath = VirtualRootShortPath + "signupuser.aspx";
        internal const string OAuthHandlerVirtualPath = "~/mc/oauth.ashx";

        #endregion

        #region Setup pages

        internal const string RolesPageVirtualPath = VirtualRootShortPath + "setup/roles.aspx";
        internal const string RoleEditPageVirtualPath = VirtualRootShortPath + "setup/roleedit.aspx";
        internal const string DatabaseServersPageVirtualPath = VirtualRootShortPath + "setup/databaseservers.aspx";
        internal const string WebsitesPageVirtualPath = VirtualRootShortPath + "setup/websites.aspx";

        #endregion

        #region Controls

        internal const string LogOnControlVirtualPath = VirtualRootPath + "controls/security/login.ascx";
        internal const string SettingsControlVirtualPath = VirtualRootPath + "controls/admin/settings.ascx";
        internal const string RecurringScheduleControlVirtualPath = VirtualRootPath + "controls/recurrenceschedulecontrol.ascx";
        internal const string CustomUrlsControlVirtualPath = VirtualRootPath + "controls/admin/customurls.ascx";
        internal const string TokenControlVirtualPath = VirtualRootPath + "controls/security/token.ascx";

        #endregion

        internal const string ManifestResourceNamePrefix = "Micajah.Common.Resources.Micajah.Common";
        internal const string VirtualRootPath = "~/Resources.Micajah.Common/";
        internal const string VirtualRootShortPath = "~/mc/";
        internal const string AdminVirtualRootShortPath = "~/mc/admin/";
        internal const string SetupVirtualRootShortPath = "~/mc/setup/";
        internal const string ResourceHandlerVirtualPath = "~/mc.axd";

        internal const string StyleSheetLoader = "Scripts.StyleSheetLoader.js";
        internal const string ComboBoxModernStyleSheet = "Styles.ComboBoxModern.css";
        internal const string CustomStyleSheet = "Styles.Custom.css";
        internal const string BootstrapStyleSheet = "Styles.bootstrap.min.css";
        internal const string FancyBoxStyleSheet = "Styles.jquery.fancybox-2.1.5.css";
        internal const string CommonGridViewModernStyleSheet = "Styles.CommonGridViewModern.css";
        internal const string AccountSettingsStyleSheet = "Styles.AccountSettings.css";
        internal const string CreditCardRegistrationStyleSheet = "Styles.CreditCardRegistration.css";
        internal const string GlobalModernStyleSheet = "Styles.GlobalModern.css";
        internal const string GlobalStyleSheet = "Styles.Global.css";
        internal const string LogOnStyleSheet = "Styles.LogOn.css";
        internal const string LogOnModernStyleSheet = "Styles.LogOnModern.css";
        internal const string MagicFormModernStyleSheet = "Styles.MagicFormModern.css";
        internal const string OnOffSwitchStyleSheet = "Styles.OnOffSwitch.css";
        internal const string NoticeMessageBoxStyleSheet = "Styles.NoticeMessageBox.css";

        internal const string DetailMenuPageVirtualPath = VirtualRootShortPath + "detailmenu.aspx";
        internal const string SupportPageVirtualPath = VirtualRootShortPath + "support.aspx";

        #endregion

        #region Public Properties

        public static string BootstrapStyleSheetUrl
        {
            get { return GetResourceUrl(BootstrapStyleSheet, true); }
        }

        public static string FancyBoxStyleSheetUrl
        {
            get { return GetResourceUrl(FancyBoxStyleSheet, true); }
        }

        public static string FancyBoxScriptUrl
        {
            get { return GetResourceUrl("Scripts.jquery.fancybox-2.1.5.pack.js", true); }
        }

        public static string JQueryScriptUrl
        {
            get { return GetResourceUrl("Scripts.jquery-1.11.0.min.js", true); }
        }

        public static string BootstrapScriptUrl
        {
            get { return GetResourceUrl("Scripts.bootstrap.min.js", true); }
        }

        #endregion

        #region Private Methods

        private static void GetEmbeddedResource(string resourceName, ref byte[] content, ref string contentType, ref string name, ref bool cacheable)
        {
            if (string.IsNullOrEmpty(resourceName)) return;

            string[] parts = resourceName.Split('?');
            resourceName = parts[0];

            name = GetResourceFileName(resourceName);
            contentType = MimeType.GetMimeType(Support.GetLastPartOfString(resourceName, ".", true), MimeType.Text);
            resourceName = ManifestResourceNamePrefix + "." + resourceName;
            MasterPageTheme masterPageTheme = MasterPageTheme.Standard;
            MasterPageThemeColor masterPageThemeColor = MasterPageThemeColor.Red;
            DetailMenuTheme detailMenuTheme = DetailMenuTheme.Standard;

            if (resourceName.EndsWith(CustomStyleSheet, StringComparison.OrdinalIgnoreCase))
            {
                cacheable = false;

                string styleSheet = null;

                if (parts.Length > 0)
                {
                    object obj = Support.ConvertStringToType(parts[1], typeof(Guid));
                    if (obj != null)
                        styleSheet = SettingProvider.GetCustomStyleSheet((Guid)obj);
                }

                if (!Support.StringIsNullOrEmpty(styleSheet))
                    content = UnicodeEncoding.UTF8.GetBytes(ProcessStyleSheet(styleSheet, FrameworkConfiguration.Current.WebApplication.MasterPage.Theme, FrameworkConfiguration.Current.WebApplication.MasterPage.ThemeColor));
                else
                    content = new byte[] { };
            }
            else if (resourceName.EndsWith(ComboBoxModernStyleSheet, StringComparison.OrdinalIgnoreCase))
            {
                content = UnicodeEncoding.UTF8.GetBytes(ProcessComboBoxModernStyleSheet(GetManifestResourceString(resourceName)));
            }
            else if (IsMasterPageThemeColorStyleSheet(resourceName, out masterPageTheme, out masterPageThemeColor))
            {
                content = UnicodeEncoding.UTF8.GetBytes(ProcessStyleSheet(GetManifestResourceString(resourceName), masterPageTheme, masterPageThemeColor));
            }
            else if (IsDetailMenuThemeStyleSheet(resourceName, out detailMenuTheme))
            {
                content = UnicodeEncoding.UTF8.GetBytes(ProcessStyleSheet(GetManifestResourceString(resourceName), detailMenuTheme));
            }
            else if (resourceName.EndsWith(BootstrapStyleSheet, StringComparison.OrdinalIgnoreCase))
            {
                content = UnicodeEncoding.UTF8.GetBytes(ProcessBootstrapStyleSheet(GetManifestResourceString(resourceName)));
            }
            else if (resourceName.EndsWith(FancyBoxStyleSheet, StringComparison.OrdinalIgnoreCase))
            {
                content = UnicodeEncoding.UTF8.GetBytes(ProcessFancyBoxStyleSheet(GetManifestResourceString(resourceName)));
            }
            else if (resourceName.EndsWith(GlobalModernStyleSheet, StringComparison.OrdinalIgnoreCase))
            {
                content = UnicodeEncoding.UTF8.GetBytes(ProcessStyleSheet(GetManifestResourceString(resourceName), MasterPageTheme.Modern, MasterPageThemeColor.NotSet));
            }
            else if (resourceName.EndsWith(CommonGridViewModernStyleSheet, StringComparison.OrdinalIgnoreCase))
            {
                content = UnicodeEncoding.UTF8.GetBytes(ProcessCommonGridViewModernStyleSheet(GetManifestResourceString(resourceName)));
            }
            else if (resourceName.EndsWith(AccountSettingsStyleSheet, StringComparison.OrdinalIgnoreCase))
            {
                content = UnicodeEncoding.UTF8.GetBytes(ProcessAccountSettingsStyleSheet(GetManifestResourceString(resourceName)));
            }
            else if (resourceName.EndsWith(CreditCardRegistrationStyleSheet, StringComparison.OrdinalIgnoreCase))
            {
                content = UnicodeEncoding.UTF8.GetBytes(ProcessCreditCardRegistrationStyleSheet(GetManifestResourceString(resourceName)));
            }
            else if (resourceName.EndsWith(OnOffSwitchStyleSheet, StringComparison.OrdinalIgnoreCase))
            {
                content = UnicodeEncoding.UTF8.GetBytes(ProcessOnOffSwitchStyleSheet(GetManifestResourceString(resourceName)));
            }
            else if (resourceName.EndsWith(NoticeMessageBoxStyleSheet, StringComparison.OrdinalIgnoreCase))
            {
                content = UnicodeEncoding.UTF8.GetBytes(ProcessNoticeMessageBoxStyleSheet(GetManifestResourceString(resourceName)));
            }
            else
            {
                content = GetManifestResourceBytes(resourceName);
            }
        }

        private static string GetResourceName(string resourceName)
        {
            return HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}|{1}", resourceName, Assembly.GetExecutingAssembly().GetName().Version)));
        }

        private static string GetResourceUrlFormat(bool createApplicationAbsoluteUrl)
        {
            return ((createApplicationAbsoluteUrl ? CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceHandlerVirtualPath) : ResourceHandlerVirtualPath) + "?d={0}");
        }

        private static byte[] GetManifestResourceBytes(string resourceName)
        {
            byte[] bytes = null;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    int length = (int)stream.Length;
                    bytes = new byte[length];
                    stream.Read(bytes, 0, length);
                }
            }
            return bytes;
        }

        private static string GetManifestResourceString(string resourceName)
        {
            string content = null;
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    content = streamReader.ReadToEnd();
                }
            }
            return content;
        }

        private static string ProcessStyleSheet(string styleSheetContent, string[] keyNames, string resourceNameFormat)
        {
            if ((!string.IsNullOrEmpty(styleSheetContent)) && (keyNames != null))
            {
                StringBuilder sb = new StringBuilder(styleSheetContent);

                foreach (string keyName in keyNames)
                {
                    sb.Replace("$" + keyName + "$", ResourceProvider.GetResourceUrl(string.Format(CultureInfo.InvariantCulture, resourceNameFormat, keyName), true));
                }

                return sb.ToString();
            }
            return styleSheetContent;
        }

        private static string ProcessStyleSheet(string styleSheetContent, MasterPageTheme masterPageTheme, MasterPageThemeColor masterPageThemeColor)
        {
            string[] keyNames = null;
            if (masterPageTheme == MasterPageTheme.Modern)
            {
                keyNames = new string[] { "NotificationCross.png", "AddNew.png", "DropMenu.png", "SearchLense.png" };
            }
            else if (masterPageTheme == MasterPageTheme.Gradient)
            {
                keyNames = new string[] { "MainMenuBottom.gif", "MainMenuItemRight.gif", "MainMenuItemLeft.gif"
                        , "SubmenuButtonItemMiddle.gif", "SubmenuButtonItemLeft.gif", "SubmenuButtonItemRight.gif"
                        , "SubmenuFirstLevelItemMiddle.gif", "SubmenuFirstLevelItemLeft.gif", "SubmenuFirstLevelItemRight.gif"
                        , "SubmenuSecondLevelItemPointer.gif"
                        , "SubmenuLastSecondLevelItemMiddle.gif", "SubmenuLastSecondLevelItemLeft.gif", "SubmenuLastSecondLevelItemRight.gif"};
            }

            return ProcessStyleSheet(styleSheetContent, keyNames, ResourceProvider.GetMasterPageThemeColorResource(masterPageTheme, masterPageThemeColor, "{0}"));
        }

        private static string ProcessStyleSheet(string styleSheetContent, DetailMenuTheme theme)
        {
            if (theme == DetailMenuTheme.Modern)
                return ProcessStyleSheet(styleSheetContent, new string[] { "Arrow.png", "CheckBox.png", "CheckBoxChecked.png" }, ResourceProvider.GetDetailMenuThemeResource(theme, "{0}"));
            return styleSheetContent;
        }

        private static string ProcessBootstrapStyleSheet(string styleSheetContent)
        {
            return ProcessStyleSheet(styleSheetContent
                , new string[] { "glyphicons-halflings-regular.eot", "glyphicons-halflings-regular.svg", "glyphicons-halflings-regular.ttf", "glyphicons-halflings-regular.woff" }
                , "Fonts.{0}");
        }

        private static string ProcessFancyBoxStyleSheet(string styleSheetContent)
        {
            return ProcessStyleSheet(styleSheetContent
                , new string[] { "blank.gif", "fancybox_loading.gif", "fancybox_loading@2x.gif", "fancybox_overlay.png", "fancybox_sprite.png", "fancybox_sprite@2x.png" }
                , "Images.FancyBox.{0}");
        }

        private static string ProcessOnOffSwitchStyleSheet(string styleSheetContent)
        {
            return ProcessStyleSheet(styleSheetContent, new string[] { "On.png", "Off.png", "SliderLeft.png", "SliderRight.png", "SliderCenter.png" }, "Images.CheckBox.{0}");
        }

        private static string ProcessNoticeMessageBoxStyleSheet(string styleSheetContent)
        {
            return ProcessStyleSheet(styleSheetContent
                , new string[] { "Close.png", "Error.png", "Information.png", "Success.png", "Warning.png", "ErrorSmall.png", "InformationSmall.png", "SuccessSmall.png", "WarningSmall.png" }
                , "Images.NoticeMessageBox.{0}");
        }

        private static string ProcessCommonGridViewModernStyleSheet(string styleSheetContent)
        {
            return ProcessStyleSheet(styleSheetContent, new string[] { "Search.png", "DropMenu.png", "Gear.png", "Cross.png" }, "Images.CommonGridView.{0}");
        }

        private static string ProcessAccountSettingsStyleSheet(string styleSheetContent)
        {
            return ProcessStyleSheet(styleSheetContent, new string[] { "assets.png", "billing.png", "credit_card.png", "email.png", "help.png", "ldap.png", "paypal.png", "phone.png", "remote.png", "ssl.png", "fancy_close.png" }, "Images.AdminControls.AccountSettings.{0}");
        }

        private static string ProcessCreditCardRegistrationStyleSheet(string styleSheetContent)
        {
            return ProcessStyleSheet(styleSheetContent, new string[] { "amex.png", "discover.png", "mastercard.png", "visa.png" }, "Images.CreditCardRegistration.{0}");
        }

        private static string ProcessComboBoxModernStyleSheet(string styleSheetContent)
        {
            return ProcessStyleSheet(styleSheetContent, new string[] { "Modern.png" }, "Images.ComboBox.{0}");
        }

        #endregion

        #region Internal Methods

        internal static string GetActiveOrganizationUrl(string returnUrl, bool anotherOrganizationIsRequired)
        {
            if (string.IsNullOrEmpty(returnUrl))
                return ResourceProvider.ActiveOrganizationPageVirtualPath;
            return string.Concat(ResourceProvider.ActiveOrganizationPageVirtualPath, "?returnurl=", HttpUtility.UrlEncode(returnUrl), (anotherOrganizationIsRequired ? "&ao=1" : string.Empty));
        }

        internal static string GetActiveInstanceUrl(string returnUrl, bool anotherInstanceIsRequired)
        {
            if (string.IsNullOrEmpty(returnUrl))
                return ResourceProvider.ActiveInstancePageVirtualPath;
            return string.Concat(ResourceProvider.ActiveInstancePageVirtualPath, "?returnurl=", HttpUtility.UrlEncode(returnUrl), (anotherInstanceIsRequired ? "&ai=1" : string.Empty));
        }

        internal static string GetDetailMenuThemeStyleSheet(DetailMenuTheme theme)
        {
            return string.Format(CultureInfo.InvariantCulture, "Styles.DetailMenuThemes.{0}.css", theme);
        }

        internal static string GetDetailMenuThemeResource(DetailMenuTheme theme, string resourceName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Images.DetailMenuThemes.{0}.{1}", theme, resourceName);
        }

        internal static string GetDetailMenuPageUrl(Guid actionId)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}?pageid={1:N}", CustomUrlProvider.CreateApplicationAbsoluteUrl(DetailMenuPageVirtualPath), actionId);
        }

        internal static string GetMasterPageThemeBaseStyleSheet(MasterPageTheme theme)
        {
            return string.Format(CultureInfo.InvariantCulture, "Styles.MasterPageThemes.{0}.css", theme);
        }

        internal static string GetMasterPageThemeColorStyleSheet(MasterPageTheme theme, MasterPageThemeColor color)
        {
            return string.Format(CultureInfo.InvariantCulture, "Styles.MasterPageThemes.{0}{1}.css", theme, ((color == MasterPageThemeColor.NotSet) ? null : color.ToString()));
        }

        internal static string GetMasterPageThemeColorResource(MasterPageTheme theme, MasterPageThemeColor color, string resourceName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Images.MasterPageThemes.{0}{1}.{2}", theme, ((color == MasterPageThemeColor.NotSet) ? null : color.ToString()), resourceName);
        }

        internal static string GetResourceFileName(string resourceName)
        {
            string[] parts = resourceName.Split('.');
            if (parts.Length < 2)
                return null;
            return string.Join(".", new string[] { parts[parts.Length - 2], parts[parts.Length - 1] });
        }

        internal static string GetResourceUrl(string resourceName, bool createApplicationAbsoluteUrl)
        {
            return string.Format(CultureInfo.InvariantCulture, GetResourceUrlFormat(createApplicationAbsoluteUrl), GetResourceName(resourceName));
        }

        internal static void GetResource(string resourceName, ref byte[] content, ref string contentType, ref string name, ref bool cacheable)
        {
            byte[] decodedResourceNameBytes = null;

            try
            {
                decodedResourceNameBytes = HttpServerUtility.UrlTokenDecode(resourceName);
            }
            catch (FormatException) { }

            if (decodedResourceNameBytes != null)
            {
                string[] parts = Encoding.UTF8.GetString(decodedResourceNameBytes).Split('|');
                string decodedResourceName = parts[0];

                GetEmbeddedResource(decodedResourceName, ref content, ref contentType, ref name, ref  cacheable);
            }
        }

        internal static string GetJavaScript(string src)
        {
            return "<script type=\"text/javascript\" src=\"" + src + "\"></script>";
        }

        internal static bool IsDetailMenuPageUrl(string virtualPath)
        {
            return (string.Compare(CustomUrlProvider.CreateApplicationRelativeUrl(virtualPath), DetailMenuPageVirtualPath.Remove(0, 1), StringComparison.OrdinalIgnoreCase) == 0);
        }

        internal static bool IsSetupPageUrl(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
                return false;
            return (virtualPath.IndexOf(SetupVirtualRootShortPath.Remove(0, 1), StringComparison.OrdinalIgnoreCase) > -1);
        }

        internal static bool IsMasterPageThemeColorStyleSheet(string resourceName, out MasterPageTheme masterPageTheme, out MasterPageThemeColor masterPageThemeColor)
        {
            masterPageTheme = MasterPageTheme.Standard;
            masterPageThemeColor = MasterPageThemeColor.Red;
            foreach (MasterPageTheme theme in Enum.GetValues(typeof(MasterPageTheme)))
            {
                foreach (MasterPageThemeColor themeColor in Enum.GetValues(typeof(MasterPageThemeColor)))
                {
                    if (resourceName.EndsWith(GetMasterPageThemeColorStyleSheet(theme, themeColor), StringComparison.OrdinalIgnoreCase))
                    {
                        masterPageTheme = theme;
                        masterPageThemeColor = themeColor;
                        return true;
                    }
                }
            }
            return false;
        }

        internal static bool IsDetailMenuThemeStyleSheet(string resourceName, out DetailMenuTheme detailMenuTheme)
        {
            detailMenuTheme = DetailMenuTheme.Standard;
            foreach (DetailMenuTheme theme in Enum.GetValues(typeof(DetailMenuTheme)))
            {
                if (resourceName.EndsWith(GetDetailMenuThemeStyleSheet(theme), StringComparison.OrdinalIgnoreCase))
                {
                    detailMenuTheme = theme;
                    return true;
                }
            }
            return false;
        }

        internal static bool IsResourceUrl(string virtualPath)
        {
            return (string.Compare(CustomUrlProvider.CreateApplicationRelativeUrl(virtualPath), ResourceHandlerVirtualPath.Remove(0, 1), StringComparison.OrdinalIgnoreCase) == 0);
        }

        internal static bool IsIconImageResource(string resourceName, IconSize iconSize)
        {
            return resourceName.Contains(string.Format(CultureInfo.InvariantCulture, ".Images.Icons._{0}x{0}.", (int)iconSize));
        }

        internal static string GetImageUrl(Type type, string name)
        {
            return GetImageUrl(type, name, false);
        }

        internal static string GetImageUrl(Type type, string name, bool createApplicationAbsoluteUrl)
        {
            string path = type.FullName.Replace("Micajah.Common.WebControls.", string.Empty).Replace("Micajah.Common.Pages.", string.Empty);
            return GetResourceUrl(string.Format(CultureInfo.InvariantCulture, "Images.{0}.{1}", path, name), createApplicationAbsoluteUrl);
        }

        internal static void RegisterScriptResource(Control ctl, string key, string resourceName)
        {
            ScriptManager.RegisterStartupScript(ctl, ctl.GetType(), key, GetJavaScript(ResourceProvider.GetResourceUrl(resourceName, true)), false);
        }

        internal static void RegisterStyleSheetResource(Control ctl, string resourceName, string id)
        {
            RegisterStyleSheetResource(ctl, resourceName, id, true);
        }

        internal static void RegisterStyleSheetResource(Control ctl, string resourceName, string id, bool registerStyleSheetLoader)
        {
            Page page = ctl.Page;
            bool useStyleSheetLoader = false;
            string resourceUrl = GetResourceUrl(resourceName, true);

            ScriptManager sm = ScriptManager.GetCurrent(ctl.Page);
            if (sm != null)
                useStyleSheetLoader = sm.IsInAsyncPostBack;

            if (!useStyleSheetLoader)
            {
                HtmlHead head = page.Header;
                if (head == null)
                    useStyleSheetLoader = true;
                else if (head.FindControl(id) == null)
                    head.Controls.Add(Support.CreateStyleSheetLink(resourceUrl, id));
            }

            if (useStyleSheetLoader)
            {
                Type pageType = page.GetType();
                if (registerStyleSheetLoader)
                    RegisterScriptResource(page, "Micajah.Common.StyleSheetLoader", StyleSheetLoader);
                ScriptManager.RegisterStartupScript(page, pageType, resourceName, string.Format(CultureInfo.InvariantCulture, "Micajah.Common.StyleSheetLoader.getInstance().addStyleSheet(\"{0}\");\r\n", resourceUrl), true);
            }
        }

        internal static void RegisterValidatorScriptResource(Control ctl)
        {
            RegisterScriptResource(ctl, "ValidatorScript", "Scripts.Validator.js");
        }

        #endregion

        #region Public Methods

        public static string GetIconImageUrl(string name, IconSize size)
        {
            return GetIconImageUrl(name, size, false);
        }

        public static string GetIconImageUrl(string name, IconSize size, bool createApplicationAbsoluteUrl)
        {
            return GetResourceUrl(string.Format(CultureInfo.InvariantCulture, "Images.Icons._{0}x{0}.{1}", (int)size, name), createApplicationAbsoluteUrl);
        }

        public static string GetActiveOrganizationUrl()
        {
            return GetActiveOrganizationUrl(null, false);
        }

        public static string GetActiveOrganizationUrl(string returnUrl)
        {
            return GetActiveOrganizationUrl(returnUrl, false);
        }

        public static string GetActiveInstanceUrl()
        {
            return GetActiveInstanceUrl(null, false);
        }

        public static string GetActiveInstanceUrl(string returnUrl)
        {
            return GetActiveInstanceUrl(returnUrl, false);
        }

        public static IList<string> GetIconImageFileNameList(IconSize iconSize)
        {
            List<string> list = new List<string>();
            foreach (string resourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (IsIconImageResource(resourceName, iconSize))
                {
                    list.Add(GetResourceFileName(resourceName));
                }
            }
            list.Sort();
            return list;
        }

        #endregion
    }
}
