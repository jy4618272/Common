using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;

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
        private UserContext m_UserContext;
        private IList m_ActionIdList;
        private bool m_IsFrameworkAdmin;
        private bool m_IsAuthenticated;
        private ActionCollection m_Items;

        #endregion

        #region Constructors

        public MainMenu(Micajah.Common.Pages.MasterPage masterPage, UserContext user, IList actionIdList, bool isFrameworkAdmin, bool isAuthenticated)
        {
            m_MasterPage = masterPage;
            m_UserContext = user;
            m_ActionIdList = actionIdList;
            m_IsFrameworkAdmin = isFrameworkAdmin;
            m_IsAuthenticated = isAuthenticated;
        }

        #endregion

        #region Private Properties

        private Guid SelectedActionId
        {
            get
            {
                Guid mainMenuItemId = Guid.Empty;

                if (m_MasterPage != null)
                {
                    if (m_MasterPage.ActiveAction != null)
                    {
                        Micajah.Common.Bll.Action mainItem = ActionProvider.PagesAndControls.FindMainMenuActionByActionId(m_MasterPage.ActiveAction.ActionId);
                        if (mainItem != null)
                        {
                            mainMenuItemId = mainItem.ActionId;
                        }
                    }
                }

                return mainMenuItemId;
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
                    m_Items = new ActionCollection();
                    Micajah.Common.Bll.Action mainItem = ActionProvider.PagesAndControls.FindByActionId(ActionProvider.PagesAndControlsActionId);
                    if (mainItem != null)
                        m_Items = mainItem.GetAvailableChildActions(m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated);
                }
                return m_Items;
            }
        }

        #endregion

        #region Private Methods

        private static Control CreateLinkAsListItem(Micajah.Common.Bll.Action action, string cssClass)
        {
            bool moderTheme = (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern);
            using (HtmlGenericControl li = new HtmlGenericControl("li"))
            {
                if (!string.IsNullOrEmpty(cssClass)) li.Attributes["class"] = cssClass;
                li.Controls.Add(Submenu.CreateLink(action, moderTheme));
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

            HtmlGenericControl mainContainer = null;
            HtmlGenericControl container = null;
            HtmlGenericControl innerContainer = null;
            HtmlGenericControl bar = null;
            HtmlGenericControl ul = null;
            HtmlGenericControl p = null;
            ControlList controlList = null;
            Control itemLink = null;
            Guid mainMenuItemId = this.SelectedActionId;

            try
            {
                mainContainer = new HtmlGenericControl("div");
                mainContainer.Attributes["class"] = "Mp_Mm";
                this.Controls.Add(mainContainer);

                MasterPageTheme theme = FrameworkConfiguration.Current.WebApplication.MasterPage.Theme;

                switch (theme)
                {
                    case MasterPageTheme.Gradient:
                    case MasterPageTheme.StandardTabs:
                    case MasterPageTheme.Modern:
                        ul = new HtmlGenericControl("ul");

                        string cssClass = null;

                        if (theme == MasterPageTheme.Modern)
                        {
                            p = new HtmlGenericControl("p");
                            p.Attributes["class"] = "mobile-nav";
                            p.InnerHtml = Resources.MainMenu_MenuLink_Text;

                            innerContainer = new HtmlGenericControl("div");
                            innerContainer.Attributes["class"] = "col-sm-12";
                            innerContainer.Controls.Add(p);
                            innerContainer.Controls.Add(ul);

                            container = new HtmlGenericControl("div");
                            container.Attributes["class"] = "container";
                            container.Controls.Add(innerContainer);

                            mainContainer.Controls.Add(container);

                            cssClass = "active";
                        }
                        else
                        {
                            mainContainer.Controls.Add(ul);

                            cssClass = "S";
                        }

                        foreach (Micajah.Common.Bll.Action item in this.Items)
                        {
                            if (item.ActionId == ActionProvider.StartPageActionId)
                            {
                                bool redirect = false;

                                Micajah.Common.WebControls.AdminControls.StartControl.GetStartMenuCheckedItems(m_UserContext, out redirect);

                                if (redirect)
                                {
                                    continue;
                                }
                            }

                            ul.Controls.Add(CreateLinkAsListItem(item, ((item.ActionId == mainMenuItemId) ? cssClass : null)));
                        }

                        if (theme != MasterPageTheme.Modern)
                        {
                            bar = new HtmlGenericControl("div");
                            bar.Attributes["class"] = "Mp_Mmb";

                            this.Controls.Add(bar);
                        }
                        break;
                    case MasterPageTheme.Standard:
                        controlList = new ControlList();
                        controlList.Delimiter = " &nbsp; | &nbsp; ";

                        foreach (Micajah.Common.Bll.Action item in this.Items)
                        {
                            if (item.ActionId == ActionProvider.StartPageActionId)
                            {
                                bool redirect = false;

                                Micajah.Common.WebControls.AdminControls.StartControl.GetStartMenuCheckedItems(m_UserContext, out redirect);

                                if (redirect)
                                {
                                    continue;
                                }
                            }

                            itemLink = Submenu.CreateLink(item, ((item.ActionId == mainMenuItemId) ? "S" : null), false);

                            controlList.Add(itemLink);
                        }

                        mainContainer.Controls.Add(controlList);
                        break;
                }
            }
            finally
            {
                if (ul != null)
                {
                    ul.Dispose();
                }

                if (mainContainer != null)
                {
                    mainContainer.Dispose();
                }

                if (bar != null)
                {
                    bar.Dispose();
                }

                if (container != null)
                {
                    container.Dispose();
                }

                if (innerContainer != null)
                {
                    innerContainer.Dispose();
                }

                if (p != null)
                {
                    p.Dispose();
                }

                if (controlList != null)
                {
                    controlList.Dispose();
                }

                if (itemLink != null)
                {
                    itemLink.Dispose();
                }
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
