using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Security;

namespace Micajah.Common.WebControls.AdminControls
{
    public partial class StartControl : System.Web.UI.UserControl
    {
        #region Members

        protected DetailMenu StartMenu;
        protected LinkButton HideLink;

        private Micajah.Common.Pages.MasterPage m_MasterPage;
        private List<Guid> m_StartMenuCheckedItems;

        #endregion

        #region Private Properties

        private Micajah.Common.Pages.MasterPage MasterPage
        {
            get
            {
                if (m_MasterPage == null) m_MasterPage = (this.Page.Master as Micajah.Common.Pages.MasterPage);
                return m_MasterPage;
            }
        }

        #endregion

        #region Private Methods

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
                        settings.UpdateValues(user.SelectedOrganization.OrganizationId, user.SelectedInstance.InstanceId);
                    else
                        settings.UpdateValues(user.SelectedOrganization.OrganizationId);
                }
            }
        }

        private void Redirect()
        {
            string redirectUrl = Request.QueryString["returnurl"];
            Micajah.Common.WebControls.SecurityControls.ActiveInstanceControl.ValidateRedirectUrl(ref redirectUrl, false);
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
            Micajah.Common.Pages.MasterPage.AddGlobalStyleSheet(this.Page);

            this.MasterPage.VisibleLeftArea = false;
            this.MasterPage.EnableJQuery = true;
            this.MasterPage.EnableFancyBox = true;

            this.LoadResources();

            if (!this.IsPostBack)
                this.RegisterFancyBoxInitScript();
        }

        protected void StartMenu_ItemDataBound(object sender, CommandEventArgs e)
        {
            if (e == null) return;

            HtmlGenericControl li = sender as HtmlGenericControl;
            if (li != null)
            {
                Micajah.Common.Bll.Action action = (Micajah.Common.Bll.Action)e.CommandArgument;

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