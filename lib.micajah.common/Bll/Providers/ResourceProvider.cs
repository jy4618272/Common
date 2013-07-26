using Micajah.Common.Application;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Dal.TableAdapters;
using Micajah.Common.Pages;
using Micajah.Common.WebControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security;
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
        internal const string OAuthPageVirtualPath = "~/mc/oauth.aspx";

        #endregion

        #region Setup pages

        internal const string ActionsPageVirtualPath = VirtualRootShortPath + "setup/actions.aspx";
        internal const string DatabasesPageVirtualPath = VirtualRootShortPath + "setup/databases.aspx";
        internal const string FrameworkPageVirtualPath = VirtualRootShortPath + "setup/framework.aspx";
        internal const string OrganizationsPageVirtualPath = VirtualRootShortPath + "setup/organizations.aspx";
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
        internal const string ResourceHandlerVirtualPath = "~/mc.axd";

        internal const string StyleSheetLoader = "Scripts.StyleSheetLoader.js";
        internal const string ComboBoxModernStyleSheet = "Styles.ComboBoxModern.css";
        internal const string CustomStyleSheet = "Styles.Custom.css";
        internal const string FancyBoxStyleSheet = "Styles.jquery.fancybox-1.3.4.css";
        internal const string CommonGridViewModernStyleSheet = "Styles.CommonGridViewModern.css";
        internal const string AccountSettingsStyleSheet = "Styles.AccountSettings.css";
        internal const string GlobalModernStyleSheet = "Styles.GlobalModern.css";
        internal const string GlobalStyleSheet = "Styles.Global.css";
        internal const string LogOnStyleSheet = "Styles.LogOn.css";
        internal const string LogOnModernStyleSheet = "Styles.LogOnModern.css";
        internal const string MagicFormModernStyleSheet = "Styles.MagicFormModern.css";
        internal const string OnOffSwitchStyleSheet = "Styles.OnOffSwitch.css";
        internal const string NoticeMessageBoxStyleSheet = "Styles.NoticeMessageBox.css";

        internal const string DetailMenuPageVirtualPath = VirtualRootShortPath + "detailmenu.aspx";
        internal const string ImageUploadPageVirtualPath = VirtualRootShortPath + "imageupload.aspx";
        internal const string SupportPageVirtualPath = VirtualRootShortPath + "support.aspx";

        #endregion

        #region Public Properties

        public static string FancyBoxStyleSheetUrl
        {
            get { return GetResourceUrl(FancyBoxStyleSheet, true); }
        }

        public static string FancyBoxScriptUrl
        {
            get { return GetResourceUrl("Scripts.jquery.fancybox-1.3.4.pack.js", true); }
        }

        public static string JQueryScriptUrl
        {
            get { return GetResourceUrl("Scripts.jquery-1.7.2.min.js", true); }
        }

        #endregion

        #region Private Methods

        private static Bitmap DrawImage(Image image, int x, int y, int width, int height, int thumbnailWidth, int thumbnailHeight)
        {
            Graphics graphics = null;

            try
            {
                Bitmap bitmap = new Bitmap(thumbnailWidth, thumbnailHeight);
                graphics = Graphics.FromImage(bitmap);
                graphics.Clear(Color.White);
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, x, y, width, height);
                graphics.Flush();

                return bitmap;
            }
            finally
            {
                if (graphics != null) graphics.Dispose();
            }
        }

        private static void GetAlignPosition(int align, int maxWidth, int maxHeight, int localSizeX, int localSizeY, ref int left, ref int top)
        {
            left = 0;
            top = 0;

            switch (align)
            {
                case 1:
                    left = (maxWidth - localSizeX) / 2;
                    top = (maxHeight - localSizeY) / 2;
                    break;
                case 2:
                    left = 0;
                    top = 0;
                    break;
                case 3:
                    left = (maxWidth - localSizeX) / 2;
                    top = 0;
                    break;
                case 4:
                    left = maxWidth - localSizeX;
                    top = 0;
                    break;
                case 5:
                    left = maxWidth - localSizeX;
                    top = (maxHeight - localSizeY) / 2;
                    break;
                case 6:
                    left = maxWidth - localSizeX;
                    top = maxHeight - localSizeY;
                    break;
                case 7:
                    left = (maxWidth - localSizeX) / 2;
                    top = maxHeight - localSizeY;
                    break;
                case 8:
                    left = 0;
                    top = maxHeight - localSizeY;
                    break;
                case 9:
                    left = 0;
                    top = (maxHeight - localSizeY) / 2;
                    break;
            }
        }

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
                Setting setting = null;
                if (parts.Length > 0)
                {
                    object obj = Support.ConvertStringToType(parts[1], typeof(Guid));
                    if (obj != null)
                        setting = SettingProvider.GetOrganizationSetting((Guid)obj, SettingProvider.MasterPageCustomStyleSheetSettingId);
                }
                if (setting != null && (!Support.StringIsNullOrEmpty(setting.Value)))
                    content = UnicodeEncoding.UTF8.GetBytes(ProcessStyleSheet(setting.Value, FrameworkConfiguration.Current.WebApplication.MasterPage.Theme, FrameworkConfiguration.Current.WebApplication.MasterPage.ThemeColor));
                else
                    content = new byte[] { };
            }
            else if (resourceName.EndsWith(ComboBoxModernStyleSheet, StringComparison.OrdinalIgnoreCase))
                content = UnicodeEncoding.UTF8.GetBytes(ProcessComboBoxModernStyleSheet(GetManifestResourceString(resourceName)));
            else if (IsMasterPageThemeColorStyleSheet(resourceName, out masterPageTheme, out masterPageThemeColor))
                content = UnicodeEncoding.UTF8.GetBytes(ProcessStyleSheet(GetManifestResourceString(resourceName), masterPageTheme, masterPageThemeColor));
            else if (IsDetailMenuThemeStyleSheet(resourceName, out detailMenuTheme))
                content = UnicodeEncoding.UTF8.GetBytes(ProcessStyleSheet(GetManifestResourceString(resourceName), detailMenuTheme));
            else if (resourceName.EndsWith(FancyBoxStyleSheet, StringComparison.OrdinalIgnoreCase))
                content = UnicodeEncoding.UTF8.GetBytes(ProcessFancyBoxStyleSheet(GetManifestResourceString(resourceName)));
            else if (resourceName.EndsWith(GlobalModernStyleSheet, StringComparison.OrdinalIgnoreCase))
                content = UnicodeEncoding.UTF8.GetBytes(ProcessStyleSheet(GetManifestResourceString(resourceName), MasterPageTheme.Modern, MasterPageThemeColor.NotSet));
            else if (resourceName.EndsWith(CommonGridViewModernStyleSheet, StringComparison.OrdinalIgnoreCase))
                content = UnicodeEncoding.UTF8.GetBytes(ProcessCommonGridViewModernStyleSheet(GetManifestResourceString(resourceName)));
            else if (resourceName.EndsWith(AccountSettingsStyleSheet, StringComparison.OrdinalIgnoreCase))
                content = UnicodeEncoding.UTF8.GetBytes(ProcessAccountSettingsStyleSheet(GetManifestResourceString(resourceName)));
            else if (resourceName.EndsWith(OnOffSwitchStyleSheet, StringComparison.OrdinalIgnoreCase))
                content = UnicodeEncoding.UTF8.GetBytes(ProcessOnOffSwitchStyleSheet(GetManifestResourceString(resourceName)));
            else if (resourceName.EndsWith(NoticeMessageBoxStyleSheet, StringComparison.OrdinalIgnoreCase))
                content = UnicodeEncoding.UTF8.GetBytes(ProcessNoticeMessageBoxStyleSheet(GetManifestResourceString(resourceName)));
            else
                content = GetManifestResourceBytes(resourceName);
        }

        private static void GetProportionalSize(int originalWidth, int originalHeight, ref int width, ref int height)
        {
            double widthPercent = ((originalWidth > 0) ? 100 * width / originalWidth : 100);
            double heightPercent = ((originalHeight > 0) ? 100 * height / originalHeight : 100);

            double currentPercent = ((widthPercent > heightPercent) ? heightPercent : widthPercent);
            if (currentPercent == 0)
            {
                if (heightPercent != 0)
                    currentPercent = heightPercent;
                else
                    currentPercent = widthPercent;
            }
            currentPercent = currentPercent / 100;

            width = Convert.ToInt32(originalWidth * currentPercent, CultureInfo.InvariantCulture);
            height = Convert.ToInt32(originalHeight * currentPercent, CultureInfo.InvariantCulture);
        }

        private static string GetResourceName(string resourceName)
        {
            return HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}|{1}", resourceName, Assembly.GetExecutingAssembly().GetName().Version)));
        }

        private static string GetResourceName(string resourceName, int width, int height, int align)
        {
            return HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}|{3}", resourceName, width, height, align)));
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

        private static void ParseResourceName(string resourceName, ref string decodedResourceName, ref Guid resourceId, ref int width, ref int height, ref int align)
        {
            byte[] decodedResourceNameBytes = null;
            try { decodedResourceNameBytes = HttpServerUtility.UrlTokenDecode(resourceName); }
            catch (FormatException) { }

            if (decodedResourceNameBytes == null) return;

            string[] parts = Encoding.UTF8.GetString(decodedResourceNameBytes).Split('|');

            object obj = Support.ConvertStringToType(parts[0], typeof(Guid));
            if (obj == null)
                decodedResourceName = parts[0];
            else
            {
                resourceId = (Guid)obj;

                if (parts.Length > 1)
                {
                    int val = 0;
                    if (int.TryParse(parts[1], out val))
                        width = val;

                    if (parts.Length > 2)
                    {
                        if (int.TryParse(parts[2], out val))
                            height = val;

                        if (parts.Length > 3)
                        {
                            if (int.TryParse(parts[3], out val))
                                align = val;
                        }
                    }
                }
            }
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
                keyNames = new string[] { "NotificationCross.png", "AddNew.png", "DropMenu.png" };
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

        private static string ProcessFancyBoxStyleSheet(string styleSheetContent)
        {
            return ProcessStyleSheet(styleSheetContent
                , new string[] { "blank.gif", "fancy_close.png", "fancy_loading.png", "fancy_nav_left.png" , "fancy_nav_right.png"
                    , "fancy_shadow_e.png", "fancy_shadow_n.png", "fancy_shadow_ne.png", "fancy_shadow_nw.png", "fancy_shadow_s.png", "fancy_shadow_se.png", "fancy_shadow_sw.png", "fancy_shadow_w.png"
                    , "fancy_title_left.png", "fancy_title_main.png", "fancy_title_over.png", "fancy_title_right.png"
                    , "fancybox.png", "fancybox-x.png", "fancybox-y.png"}
                , "Images.FancyBox.{0}");
        }

        private static string ProcessOnOffSwitchStyleSheet(string styleSheetContent)
        {
            return ProcessStyleSheet(styleSheetContent, new string[] { "On.png", "Off.png", "SliderLeft.png", "SliderRight.png", "SliderCenter.png" }, "Images.Micajah.Common.WebControls.CheckBox.{0}");
        }

        private static string ProcessNoticeMessageBoxStyleSheet(string styleSheetContent)
        {
            return ProcessStyleSheet(styleSheetContent
                , new string[] { "Close.png", "Error.png", "Information.png", "Success.png", "Warning.png", "ErrorSmall.png", "InformationSmall.png", "SuccessSmall.png", "WarningSmall.png" }
                , "Images.Micajah.Common.WebControls.NoticeMessageBox.{0}");
        }

        private static string ProcessCommonGridViewModernStyleSheet(string styleSheetContent)
        {
            return ProcessStyleSheet(styleSheetContent, new string[] { "Search.png", "DropMenu.png", "Gear.png", "Cross.png" }, "Images.Micajah.Common.WebControls.CommonGridView.{0}");
        }

        private static string ProcessAccountSettingsStyleSheet(string styleSheetContent)
        {
            return ProcessStyleSheet(styleSheetContent, new string[] { "amex.png", "assets.png", "billing.png", "credit_card.png", "discover.png", "email.png", "help.png", "ldap.png", "mastercard.png", "paypal.png", "phone.png", "remote.png", "ssl.png", "visa.png", "fancy_close.png" }, "Images.Micajah.Common.WebControls.AdminControls.AccountSettings.{0}");
        }

        private static string ProcessComboBoxModernStyleSheet(string styleSheetContent)
        {
            return ProcessStyleSheet(styleSheetContent, new string[] { "Modern.png" }, "Images.Micajah.Common.WebControls.ComboBox.{0}");
        }

        #endregion

        #region Internal Methods

        internal static string GetActiveOrganizationUrl(string returnUrl, bool anotherOrganizationIsRequired)
        {
            if (string.IsNullOrEmpty(returnUrl))
                return ResourceProvider.ActiveOrganizationPageVirtualPath;
            return string.Concat(ResourceProvider.ActiveOrganizationPageVirtualPath, "?returnurl=", HttpUtility.UrlEncodeUnicode(returnUrl), (anotherOrganizationIsRequired ? "&ao=1" : string.Empty));
        }

        internal static string GetActiveInstanceUrl(string returnUrl, bool anotherInstanceIsRequired)
        {
            if (string.IsNullOrEmpty(returnUrl))
                return ResourceProvider.ActiveInstancePageVirtualPath;
            return string.Concat(ResourceProvider.ActiveInstancePageVirtualPath, "?returnurl=", HttpUtility.UrlEncodeUnicode(returnUrl), (anotherInstanceIsRequired ? "&ai=1" : string.Empty));
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

        internal static void GetResource(string resourceName, ref byte[] content, ref string contentType, ref string name, ref bool cacheable)
        {
            string decodedResourceName = null;
            Guid resourceId = Guid.Empty;
            int height = 0;
            int width = 0;
            int align = 0;

            ParseResourceName(resourceName, ref decodedResourceName, ref resourceId, ref width, ref height, ref align);
            if (resourceId != Guid.Empty)
            {
                CommonDataSet.ResourceRow row = GetResourceRow(resourceId, width, height, align, true);
                if (row != null)
                {
                    if (!row.IsContentTypeNull()) contentType = row.ContentType;
                    content = row.Content;
                    if (!row.IsNameNull()) name = row.Name;
                    cacheable = true;
                }
            }
            else
                GetEmbeddedResource(decodedResourceName, ref content, ref contentType, ref name, ref  cacheable);
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

        internal static string GetSqlScript(int version, string dbType)
        {
            return GetManifestResourceString(string.Concat(ResourceProvider.ManifestResourceNamePrefix, ".SqlScripts.", dbType, ".v", version, ".sql"));
        }

        internal static string GetSqlScript(string sqlScriptName, string dbType)
        {
            return GetManifestResourceString(string.Concat(ResourceProvider.ManifestResourceNamePrefix, ".SqlScripts.", dbType, ".", sqlScriptName, ".sql"));
        }

        internal static string GetJavaScript(string src)
        {
            return "<script type=\"text/javascript\" src=\"" + src + "\"></script>";
        }

        internal static bool IsDetailMenuPageUrl(string virtualPath)
        {
            return (string.Compare(CustomUrlProvider.CreateApplicationRelativeUrl(virtualPath), DetailMenuPageVirtualPath.Remove(0, 1), StringComparison.OrdinalIgnoreCase) == 0);
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
            return GetResourceUrl(string.Format(CultureInfo.InvariantCulture, "Images.{0}.{1}", type.FullName, name), createApplicationAbsoluteUrl);
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
            string resourceUrl = ResourceProvider.GetResourceUrl(resourceName, true);

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

        public static byte[] CreateThumbnail(byte[] content, int width, int height, int align)
        {
            MemoryStream originalStream = null;
            Image originalImage = null;
            Bitmap scaledImage = null;
            Bitmap outputImage = null;
            MemoryStream outputStream = null;

            try
            {
                originalStream = new MemoryStream(content);
                originalImage = Image.FromStream(originalStream);
                originalStream.Position = 0;

                outputStream = new MemoryStream();

                int outputWidth = width;
                int outputHeight = height;
                GetProportionalSize(originalImage.Width, originalImage.Height, ref outputWidth, ref outputHeight);

                scaledImage = DrawImage(originalImage, 0, 0, outputWidth, outputHeight, outputWidth, outputHeight);

                if (align > 0)
                {
                    if (width == 0) width = outputWidth;
                    if (height == 0) height = outputHeight;
                    int maxWidth = ((outputWidth > width) ? outputWidth : width);
                    int maxHeight = ((outputHeight > height) ? outputHeight : height);

                    int x = 0;
                    int y = 0;
                    GetAlignPosition(align, maxWidth, maxHeight, outputWidth, outputHeight, ref x, ref y);

                    outputImage = DrawImage((Image)scaledImage, x, y, outputWidth, outputHeight, maxWidth, maxHeight);

                    outputImage.Save(outputStream, ImageFormat.Png);
                }
                else
                    scaledImage.Save(outputStream, ImageFormat.Png);

                return outputStream.ToArray();
            }
            finally
            {
                if (scaledImage != null) scaledImage.Dispose();
                if (outputImage != null) outputImage.Dispose();
                if (outputStream != null) outputStream.Dispose();
                if (originalImage != null) originalImage.Dispose();
                if (originalStream != null) originalStream.Dispose();
            }
        }

        public static void DeleteResources(params string[] resourceId)
        {
            if (resourceId == null) return;

            foreach (string str in resourceId)
            {
                object obj = Support.ConvertStringToType(str, typeof(Guid));
                if (obj != null)
                    WebApplication.CommonDataSetTableAdapters.ResourceTableAdapter.Delete((Guid)obj);
            }
        }

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

        public static string GetResourceUrl(Guid resourceId)
        {
            return GetResourceUrl(resourceId, false);
        }

        public static string GetResourceUrl(Guid resourceId, bool createApplicationAbsoluteUrl)
        {
            return GetResourceUrl(resourceId, 0, 0, 0, createApplicationAbsoluteUrl);
        }

        public static string GetResourceUrl(Guid resourceId, int width, int height)
        {
            return GetResourceUrl(resourceId, width, height, 0, false);
        }

        public static string GetResourceUrl(Guid resourceId, int width, int height, int align)
        {
            return GetResourceUrl(resourceId, width, height, align, false);
        }

        public static string GetResourceUrl(Guid resourceId, int width, int height, bool createApplicationAbsoluteUrl)
        {
            return GetResourceUrl(resourceId, width, height, 0, createApplicationAbsoluteUrl);
        }

        public static string GetResourceUrl(Guid resourceId, int width, int height, int align, bool createApplicationAbsoluteUrl)
        {
            return string.Format(CultureInfo.InvariantCulture, GetResourceUrlFormat(createApplicationAbsoluteUrl), GetResourceName(resourceId.ToString("N"), width, height, align));
        }

        public static IList<string> GetIconImageFileNameList(IconSize iconSize)
        {
            List<string> list = new List<string>();
            foreach (string resourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (Micajah.Common.Bll.Providers.ResourceProvider.IsIconImageResource(resourceName, iconSize))
                    list.Add(ResourceProvider.GetResourceFileName(resourceName));
            }
            list.Sort();
            return list;
        }

        /// <summary>
        /// Gets an object populated with information of the specified resource.
        /// </summary>
        /// <param name="resourceId">The unique identifier of the resource.</param>
        /// <returns>The object populated with information of the resource or null reference, if the resources is not found.</returns>
        public static CommonDataSet.ResourceRow GetResourceRow(Guid resourceId)
        {
            return GetResourceRow(resourceId, 0, 0, 0);
        }

        public static CommonDataSet.ResourceRow GetResourceRow(Guid resourceId, int width, int height, int align)
        {
            return GetResourceRow(resourceId, width, height, align, false);
        }

        public static CommonDataSet.ResourceRow GetResourceRow(Guid resourceId, int width, int height, int align, bool createThumbnailIfNotExists)
        {
            int? w = ((width > 0) ? new int?(width) : null);
            int? h = ((height > 0) ? new int?(height) : null);
            int? a = ((align > 0) ? new int?(align) : null);
            CommonDataSet.ResourceDataTable table = null;

            try
            {
                ITableAdapter resourceTableAdapter = WebApplication.CommonDataSetTableAdapters.ResourceTableAdapter.Clone() as ITableAdapter;

                table = new CommonDataSet.ResourceDataTable();
                resourceTableAdapter.Fill(table, 0, resourceId, w, h, a);
                CommonDataSet.ResourceRow row = ((table.Count > 0) ? table[0] : null);

                if (row != null)
                {
                    if ((height > 0) || (width > 0))
                    {
                        if (row.ResourceId == resourceId)
                        {
                            if (createThumbnailIfNotExists)
                            {
                                byte[] content = CreateThumbnail(row.Content, width, height, align);
                                string name = (row.IsNameNull() ? null : row.Name);
                                bool temporary = row.Temporary;
                                string localObjectType = row.LocalObjectType;
                                string localObjectId = row.LocalObjectId;

                                row = null;
                                if (content != null)
                                {
                                    row = table.NewResourceRow();
                                    row.ResourceId = Guid.NewGuid();
                                    row.ParentResourceId = resourceId;
                                    row.LocalObjectType = localObjectType;
                                    row.LocalObjectId = localObjectId;
                                    row.Content = content;
                                    row.ContentType = MimeType.Png;
                                    if (!string.IsNullOrEmpty(name)) row.Name = name.Split('.')[0] + ".png";
                                    if (w.HasValue) row.Width = w.Value;
                                    if (h.HasValue) row.Height = h.Value;
                                    if (a.HasValue) row.Align = a.Value;
                                    row.Temporary = temporary;
                                    row.CreatedTime = DateTime.UtcNow;

                                    table.AddResourceRow(row);
                                    resourceTableAdapter.Update(row);
                                }
                            }
                            else
                                row = null;
                        }
                    }
                }

                return row;
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        public static CommonDataSet.ResourceRow GetResourceRow(string localObjectType, string localObjectId)
        {
            CommonDataSet.ResourceDataTable table = null;
            try
            {
                table = new CommonDataSet.ResourceDataTable();
                WebApplication.CommonDataSetTableAdapters.ResourceTableAdapter.Fill(table, 1, localObjectType, localObjectId);
                return ((table.Count > 0) ? table[0] : null);
            }
            finally
            {
                if (table != null) table.Dispose();
            }
        }

        /// <summary>
        /// Downloads the resource with the specified URI and stores the downloaded data in the table.
        /// </summary>
        /// <param name="fileName">The URI from which to download data or path to the file.</param>
        /// <param name="localObjectType">The type of the object which the resource is associated with.</param>
        /// <param name="localObjectId">The unique identifier of the object which the resource is associated with.</param>
        /// <param name="temporary">The value indicating whether the resource is temporary.</param>
        /// <param name="maxSize">The maximal size for the data in bytes.</param>
        /// <returns>The unique identifier of the resource.</returns>
        public static Guid? InsertResource(string fileName, string localObjectType, string localObjectId, bool temporary, int maxSize)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                byte[] bytes = null;
                string contentType = null;
                string name = null;

                if (Support.ValidateUrl(fileName, false))
                {
                    WebClient webClient = null;
                    try
                    {
                        Uri uri = new Uri(fileName);
                        webClient = new WebClient();
                        bytes = webClient.DownloadData(uri);

                        name = Support.GetLastPartOfString(uri.PathAndQuery.Split('?')[0], "/");
                        contentType = MimeType.GetMimeType(Support.GetLastPartOfString(name, ".", true));
                    }
                    catch (UriFormatException) { }
                    catch (WebException) { }
                    finally
                    {
                        if (webClient != null) webClient.Dispose();
                    }
                }
                else
                {
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            bytes = File.ReadAllBytes(fileName);

                            FileInfo fi = new FileInfo(fileName);
                            name = fi.Name;
                            contentType = MimeType.GetMimeType(fi.Extension);
                        }
                        catch (PathTooLongException) { }
                        catch (DirectoryNotFoundException) { }
                        catch (IOException) { }
                        catch (UnauthorizedAccessException) { }
                        catch (NotSupportedException) { }
                        catch (SecurityException) { }
                    }
                }

                if (bytes != null)
                {
                    if ((maxSize > 0) && (bytes.Length > maxSize))
                        throw new InvalidDataException();

                    return InsertResource(null, localObjectType, localObjectId, bytes, contentType, name, 0, 0, 0, temporary);
                }
            }

            return null;
        }

        /// <summary>
        /// Stores the binary resource in the table.
        /// </summary>
        /// <param name="parentResourceId">The unique identifier of the parent resource.</param>
        /// <param name="localObjectType">The type of the object which the resource is associated with.</param>
        /// <param name="localObjectId">The unique identifier of the object which the resource is associated with.</param>
        /// <param name="content">A System.Byte array containing the resource</param>
        /// <param name="contentType">The MIME types of a resource.</param>
        /// <param name="name">The name of a resource.</param>
        /// <param name="width">The width of the image resource.</param>
        /// <param name="height">The height of the image resource.</param>
        /// <param name="align">The align of the image resource.</param>
        /// <param name="temporary">The value indicating whether the resource is temporary.</param>
        /// <returns>The unique identifier of the resource.</returns>
        public static Guid? InsertResource(Guid? parentResourceId, string localObjectType, string localObjectId, byte[] content, string contentType, string name, int width, int height, int align, bool temporary)
        {
            if ((content != null) && (content.Length > 0))
            {
                int? w = ((width > 0) ? new int?(width) : null);
                int? h = ((height > 0) ? new int?(height) : null);
                int? a = ((align > 0) ? new int?(align) : null);

                CommonDataSet.ResourceRow row = WebApplication.CommonDataSet.Resource.NewResourceRow();
                row.ResourceId = Guid.NewGuid();
                if (parentResourceId.HasValue && (parentResourceId.Value != Guid.Empty)) row.ParentResourceId = parentResourceId.Value;
                row.LocalObjectType = localObjectType;
                row.LocalObjectId = localObjectId;
                row.Content = content;
                if (!string.IsNullOrEmpty(contentType)) row.ContentType = contentType;
                if (!string.IsNullOrEmpty(name)) row.Name = name;
                if (w.HasValue) row.Width = w.Value;
                if (h.HasValue) row.Height = h.Value;
                if (a.HasValue) row.Align = a.Value;
                row.Temporary = temporary;
                row.CreatedTime = DateTime.UtcNow;

                WebApplication.CommonDataSet.Resource.AddResourceRow(row);
                WebApplication.CommonDataSetTableAdapters.ResourceTableAdapter.Update(row);

                return row.ResourceId;
            }
            return null;
        }

        public static void UpdateResource(Guid resourceId, string localObjectType, string localObjectId, bool temporary)
        {
            if (resourceId != Guid.Empty)
                WebApplication.CommonDataSetTableAdapters.ResourceTableAdapter.Update(resourceId, localObjectType, localObjectId, temporary);
        }

        #endregion
    }
}
