using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Micajah.Common.WebControls.AdminControls
{
    public partial class StartControl : Micajah.Common.WebControls.SetupControls.MasterControl
    {
        #region Members

        protected HtmlGenericControl VideoPanel;
        protected DetailMenu StartMenu;
        protected LinkButton HideLink;

        private List<Guid> m_StartMenuCheckedItems;

        #endregion

        #region Private Methods

        private static string GetObjectTag(int width, int height, string url)
        {
            string widthAttribute = string.Empty;
            if (width > 0) widthAttribute = " width=\"" + width + "\"";
            string heightAttribute = string.Empty;
            if (height > 0) heightAttribute = " height=\"" + height + "\"";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.InvariantCulture, @"<object classid=""clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"" codebase=""https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=5,0,0,0""{0}{1}>
    <param name=""movie"" value=""{2}"">
    <param name=""quality"" value=""high"">
    <param name=""WMode"" value=""opaque"">
    <embed src=""{2}"" quality=""high"" wmode=""opaque""{0}{1} type=""application/x-shockwave-flash"" pluginspage=""https://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash"">
</object>
"
                , widthAttribute, heightAttribute, url);
            return sb.ToString();
        }

        private void LoadResources()
        {
            HideLink.Text = Resources.StartControl_HideLink_Text;
        }

        private void LoadStartMenuCheckedItems()
        {
            bool redirect = false;
            m_StartMenuCheckedItems = GetStartMenuCheckedItems(UserContext.Current, out redirect);
            if (redirect)
                Redirect();
        }

        private static void SaveStartMenuCheckedItems(string value)
        {
            UserContext user = UserContext.Current;
            if (user != null)
            {
                SettingCollection settings = user.Settings;
                Setting setting = settings["StartMenuCheckedItems"];
                if (setting != null)
                {
                    if (string.IsNullOrEmpty(setting.Value) || (string.Compare(setting.Value, bool.TrueString, StringComparison.OrdinalIgnoreCase) == 0))
                        setting.Value = value;
                    else
                        setting.Value += "," + value;

                    if ((ActionProvider.StartPageSettingsLevels & SettingLevels.Instance) == SettingLevels.Instance)
                        settings.UpdateValues(user.OrganizationId, user.InstanceId);
                    else
                        settings.UpdateValues(user.OrganizationId);
                }
            }
        }

        private void Redirect()
        {
            string redirectUrl = Request.QueryString["returnurl"];
            Micajah.Common.WebControls.SecurityControls.ActiveInstanceControl.ValidateRedirectUrl(ref redirectUrl, true);
            if (!string.IsNullOrEmpty(redirectUrl))
                Response.Redirect(redirectUrl);
        }

        private void RegisterFancyBoxInitScript()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "FancyBoxInitScript", @"$(""a[rel=WatchVideo]"").fancybox({
    'type': 'swf',
    'swf': {
        'allowfullscreen': 'true',
        'allowscriptaccess': 'true'
    },
    'width': '800',
    'height': '450',
    'showNavArrows': false,
    'titlePosition': 'inside',
    'transitionIn': 'none',
    'transitionOut': 'none'
});
"
, true);
        }

        #endregion

        #region Internal Methods

        internal static List<Guid> GetStartMenuCheckedItems(UserContext user, out bool redirect)
        {
            redirect = false;
            if (user != null)
            {
                SettingCollection settings = user.Settings;
                Setting setting = settings["StartMenuCheckedItems"];
                if (setting != null)
                {
                    if (setting.ValueIsDefault)
                        redirect = true;
                    else if (!string.IsNullOrEmpty(setting.Value))
                    {
                        if (setting.Value.EndsWith(bool.FalseString, StringComparison.OrdinalIgnoreCase))
                            redirect = true;
                        else if (string.Compare(setting.Value, bool.TrueString, StringComparison.OrdinalIgnoreCase) == 0)
                            return new List<Guid>();
                        else
                            return Support.ConvertStringToGuidList(setting.Value) as List<Guid>;
                    }
                }
            }
            return null;
        }

        #endregion

        #region Protected Methods

        protected void Page_Init(object sender, EventArgs e)
        {
            this.LoadStartMenuCheckedItems();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Micajah.Common.Pages.MasterPage.RegisterGlobalStyleSheet(this.Page);

            this.MasterPage.VisibleSubmenu = false;
            this.MasterPage.VisibleLeftArea = false;
            this.MasterPage.EnableJQuery = true;
            this.MasterPage.EnableFancyBox = true;

            this.LoadResources();

            if (!this.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.MasterPage.ActiveAction.VideoUrl))
                    VideoPanel.InnerHtml = GetObjectTag(386, 220, this.MasterPage.ActiveAction.VideoUrl);
                else
                    VideoPanel.Visible = false;

                this.RegisterFancyBoxInitScript();
            }
        }

        protected void StartMenu_ItemDataBound(object sender, CommandEventArgs e)
        {
            if (e == null) return;

            HtmlGenericControl li = sender as HtmlGenericControl;
            if (li == null) return;

            Micajah.Common.Bll.Action action = (Micajah.Common.Bll.Action)e.CommandArgument;

            if (string.IsNullOrEmpty(action.Name))
            {
                li.Visible = false;
                return;
            }

            if (action.NavigateUrl == null)
            {
                if ((m_StartMenuCheckedItems != null) && m_StartMenuCheckedItems.Contains(action.ActionId))
                {
                    li.Attributes["class"] = "Cbc";
                    if (li.HasControls())
                        ((HyperLink)li.Controls[0]).NavigateUrl = (string.IsNullOrEmpty(action.AbsoluteNavigateUrl) ? action.CustomAbsoluteNavigateUrl : null);
                }
                else
                    li.Attributes["class"] = "Cb";
            }
            else if (!string.IsNullOrEmpty(action.NavigateUrl))
            {
                li.Attributes["class"] = "G";
                if (string.Compare(action.NavigateUrl, action.VideoUrl, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (li.Controls.Count > 0)
                    {
                        HyperLink link = (HyperLink)li.Controls[0];
                        link.NavigateUrl = string.Empty;

                        if (li.Controls.Count > 1)
                        {
                            HtmlGenericControl span = (HtmlGenericControl)li.Controls[1];
                            if (span.HasControls())
                            {
                                link = (HyperLink)span.Controls[0];
                                link.Text = Resources.StartControl_WatchLink_Text;
                            }
                        }
                    }

                    return;
                }
            }

            if (!string.IsNullOrEmpty(action.VideoUrl))
            {
                if (li.Controls.Count > 1)
                {
                    HtmlGenericControl span = (HtmlGenericControl)li.Controls[1];
                    if (span.HasControls())
                    {
                        HyperLink link = (HyperLink)span.Controls[0];
                        link.Attributes["rel"] = "WatchVideo";
                    }
                }
            }
        }

        protected void StartMenu_ItemClick(object sender, CommandEventArgs e)
        {
            if (e == null) return;

            Micajah.Common.Bll.Action action = (Micajah.Common.Bll.Action)e.CommandArgument;

            if (m_StartMenuCheckedItems == null)
                m_StartMenuCheckedItems = new List<Guid>();

            if (!m_StartMenuCheckedItems.Contains(action.ActionId))
            {
                m_StartMenuCheckedItems.Add(action.ActionId);

                SaveStartMenuCheckedItems(action.ActionId.ToString("N"));

                this.RegisterFancyBoxInitScript();
            }
        }

        protected void HideLink_Click(object sender, EventArgs e)
        {
            SaveStartMenuCheckedItems(bool.FalseString);

            this.Redirect();
        }

        #endregion
    }
}