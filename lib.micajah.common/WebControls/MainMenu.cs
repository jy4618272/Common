using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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
        private IList m_ActionIdList;
        private bool m_IsFrameworkAdmin;
        private bool m_IsAuthenticated;
        private ActionCollection m_Items;

        #endregion

        #region Constructors

        public MainMenu(Micajah.Common.Pages.MasterPage masterPage, IList actionIdList, bool isFrameworkAdmin, bool isAuthenticated)
        {
            m_MasterPage = masterPage;
            m_ActionIdList = actionIdList;
            m_IsFrameworkAdmin = isFrameworkAdmin;
            m_IsAuthenticated = isAuthenticated;
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

            HtmlGenericControl div1 = null;
            HtmlGenericControl div2 = null;
            HtmlGenericControl ul = null;
            HtmlGenericControl li = null;
            HyperLink link = null;
            ControlList ctrl = null;
            Control itemCtl = null;
            Guid mainMenuItemId = Guid.Empty;

            if (m_MasterPage != null)
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

                        if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != MasterPageTheme.Modern)
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
                            itemCtl = Submenu.CreateLink(item, ((item.ActionId == mainMenuItemId) ? "S" : null), false);
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
