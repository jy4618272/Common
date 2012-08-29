using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using Micajah.Common.Security;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Displays a main menu in a Web Forms page.
    /// </summary>
    [ToolboxItem(false)]
    internal sealed class MainMenu : Control
    {
        #region Members

        private Micajah.Common.Pages.MasterPage m_MasterPage;
        private ActionCollection m_Items;

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

        #region Internal Properties

        /// <summary>
        /// Gets the items of the menu.
        /// </summary>
        internal ActionCollection Items
        {
            get
            {
                if (m_Items == null)
                {
                    IList actionIdList = null;
                    bool isFrameworkAdmin = false;
                    bool isAuthenticated = false;
                    UserContext user = UserContext.Current;
                    if (user != null)
                    {
                        actionIdList = user.ActionIdList;
                        isAuthenticated = true;
                        isFrameworkAdmin = user.IsFrameworkAdministrator;
                    }

                    m_Items = new ActionCollection();
                    Micajah.Common.Bll.Action mainItem = ActionProvider.PagesAndControls.FindByActionId(ActionProvider.PagesAndControlsActionId);
                    if (mainItem != null)
                        m_Items = mainItem.GetAvailableChildActions(isAuthenticated, isFrameworkAdmin, actionIdList);
                }
                return m_Items;
            }
        }

        #endregion

        #region Private Methods

        private static Control CreateLinkAsListItem(Micajah.Common.Bll.Action action, string cssClass)
        {
            using (HtmlGenericControl li = new HtmlGenericControl("li"))
            {
                if (!string.IsNullOrEmpty(cssClass)) li.Attributes["class"] = cssClass;
                li.Controls.Add(Submenu.CreateLink(action));
                return li;
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

            if (this.Items.Count == 0) return;

            HtmlGenericControl div1 = null;
            HtmlGenericControl div2 = null;
            HtmlGenericControl ul = null;
            HtmlGenericControl li = null;
            HyperLink link = null;
            ControlList ctrl = null;
            Control itemCtl = null;
            Guid mainMenuItemId = Guid.Empty;

            if (this.MasterPage != null)
            {
                if (m_MasterPage.ActiveAction != null)
                {
                    Micajah.Common.Bll.Action mainItem = ActionProvider.PagesAndControls.FindMainMenuActionByActionId(m_MasterPage.ActiveAction.ActionId);
                    if (mainItem != null)
                        mainMenuItemId = mainItem.ActionId;
                }
            }

            try
            {
                div1 = new HtmlGenericControl("div");
                div1.Attributes["class"] = "Mp_Mm";
                this.Controls.Add(div1);

                switch (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme)
                {
                    case MasterPageTheme.Gradient:
                    case MasterPageTheme.StandardTabs:
                    case MasterPageTheme.Modern:
                        ul = new HtmlGenericControl("ul");
                        div1.Controls.Add(ul);

                        foreach (Micajah.Common.Bll.Action item in this.Items)
                        {
                            ul.Controls.Add(CreateLinkAsListItem(item, ((item.ActionId == mainMenuItemId) ? "S" : null)));
                        }

                        if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern)
                        {
                            if (this.MasterPage.VisibleHelpLink)
                            {
                                link = new HyperLink();
                                link.Text = Resources.MasterPage_HelpLink_Text2;
                                link.NavigateUrl = "#";
                                link.Attributes["onclick"] = this.MasterPage.HelpLinkOnClick;

                                li = new HtmlGenericControl("li");
                                li.Attributes["class"] = "Ph";
                                li.Controls.Add(link);
                                ul.Controls.Add(li);
                            }
                        }
                        else
                        {
                            div2 = new HtmlGenericControl("div");
                            div2.Attributes["class"] = "Mp_Mmb";
                            this.Controls.Add(div2);
                        }
                        break;
                    case MasterPageTheme.Standard:
                        ctrl = new ControlList();
                        ctrl.Delimiter = " &nbsp; | &nbsp; ";

                        foreach (Micajah.Common.Bll.Action item in this.Items)
                        {
                            itemCtl = Submenu.CreateLink(item, ((item.ActionId == mainMenuItemId) ? "S" : null));
                            ctrl.Add(itemCtl);
                        }

                        div1.Controls.Add(ctrl);
                        break;
                }
            }
            finally
            {
                if (link != null) link.Dispose();
                if (li != null) li.Dispose();
                if (ul != null) ul.Dispose();
                if (div1 != null) div1.Dispose();
                if (div2 != null) div2.Dispose();
                if (ctrl != null) ctrl.Dispose();
                if (itemCtl != null) itemCtl.Dispose();
            }
        }

        /// <summary>
        /// Registers the client scripts.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != MasterPageTheme.Modern)
            {
                Type type = this.GetType();
                if (!Page.ClientScript.IsStartupScriptRegistered(type, "AttachHoverEvents"))
                    Page.ClientScript.RegisterStartupScript(type, "AttachHoverEvents", "Mp_AttachHoverEvents('Mp_Mm', 'LI');\r\n", true);
            }
        }

        #endregion
    }
}
