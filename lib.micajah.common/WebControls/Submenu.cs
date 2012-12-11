using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Security;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Displays a submenu in a Web Forms page. By default it look like a left submenu.
    /// </summary>
    [ToolboxItem(false)]
    internal sealed class Submenu : Control
    {
        #region Members

        private Action m_ParentAction;
        private Micajah.Common.Pages.MasterPage m_MasterPage;
        private SubmenuPosition m_Position;
        private IList m_ActionIdList;
        private bool m_IsFrameworkAdmin;
        private bool m_IsAuthenticated;
        private UserContext m_UserContext;
        private ActionCollection m_Items;
        private bool m_ItemsIsLoaded;
        private HtmlGenericControl m_Container;
        private System.Guid? m_SelectedItemId;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Submenu control as left submenu.
        /// </summary>
        public Submenu()
        {
            m_Position = SubmenuPosition.Left;
            m_UserContext = UserContext.Current;
            if (m_UserContext != null)
            {
                m_ActionIdList = m_UserContext.ActionIdList;
                m_IsFrameworkAdmin = m_UserContext.IsFrameworkAdministrator;
                m_IsAuthenticated = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Submenu control with specified type.
        /// </summary>
        /// <param name="position">Specifies the position of the submenu on the page. One of the SubmenuPosition enumerations.</param>
        public Submenu(SubmenuPosition position)
            : this()
        {
            m_Position = position;
        }

        #endregion

        #region Private Properties

        private Micajah.Common.Pages.MasterPage MasterPage
        {
            get
            {
                if (m_MasterPage == null)
                {
                    System.Web.UI.MasterPage master = this.Page.Master;
                    while (master != null)
                    {
                        if (master is Micajah.Common.Pages.MasterPage)
                        {
                            m_MasterPage = (master as Micajah.Common.Pages.MasterPage);
                            return m_MasterPage;
                        }
                        master = master.Master;
                    }
                }
                return m_MasterPage;
            }
        }

        private System.Guid SelectedItemId
        {
            get
            {
                if (!m_SelectedItemId.HasValue)
                {
                    System.Guid itemId = System.Guid.Empty;
                    if (this.MasterPage != null)
                    {
                        if (m_MasterPage.ActiveAction != null)
                        {
                            Micajah.Common.Bll.Action item = ActionProvider.PagesAndControls.FindSubmenuActionByActionId(m_MasterPage.ActiveAction.ActionId, this.ParentActionId);
                            if (item != null)
                                itemId = item.ActionId;
                        }
                    }
                    m_SelectedItemId = itemId;
                }
                return m_SelectedItemId.Value;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the items of the menu.
        /// </summary>
        public ActionCollection Items
        {
            get
            {
                if (!m_ItemsIsLoaded)
                {
                    if (this.ParentAction != null)
                    {
                        m_Items = m_ParentAction.GetAvailableChildActions(m_IsAuthenticated, m_IsFrameworkAdmin, m_ActionIdList);
                        m_ItemsIsLoaded = true;
                    }
                }
                return m_Items;
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the action the child actions of which are displayed in the control.
        /// </summary>
        public System.Guid ParentActionId
        {
            get
            {
                object obj = ViewState["ParentActionId"];
                return ((obj == null) ? System.Guid.Empty : (System.Guid)obj);
            }
            set
            {
                ViewState["ParentActionId"] = value;
                if (m_ParentAction != null)
                {
                    if (m_ParentAction.ActionId != value)
                        m_ParentAction = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the action the child actions of which are displayed in the control.
        /// </summary>
        public Action ParentAction
        {
            get
            {
                if (m_ParentAction == null)
                {
                    if (this.ParentActionId != System.Guid.Empty)
                        m_ParentAction = ActionProvider.PagesAndControls.FindByActionId(this.ParentActionId);
                    else if (this.MasterPage != null)
                    {
                        if (m_MasterPage.ActiveAction != null)
                        {
                            m_ParentAction = ActionProvider.PagesAndControls.FindMainMenuActionByActionId(m_MasterPage.ActiveAction.ActionId);
                            this.ParentActionId = ((m_ParentAction == null) ? System.Guid.Empty : m_ParentAction.ActionId);
                        }
                    }
                }
                return m_ParentAction;
            }
            set
            {
                m_ParentAction = value;
                this.ParentActionId = ((value == null) ? System.Guid.Empty : value.ActionId);
            }
        }

        #endregion

        #region Private Methods

        private void CreateLeftSubmenu()
        {
            HtmlGenericControl ulMain = null;
            HtmlGenericControl liMain = null;
            HtmlGenericControl ulSub = null;
            HtmlGenericControl liSub = null;
            Label lbl = null;

            try
            {
                string secondLevelItemCssClass = null;

                ulMain = new HtmlGenericControl("ul");
                ulMain.Attributes["class"] = "Mp_Sm";
                m_Container.Controls.Add(ulMain);

                if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != MasterPageTheme.Modern)
                    secondLevelItemCssClass = "S";

                bool first = true;
                foreach (Action item in Items)
                {
                    liMain = new HtmlGenericControl("li");
                    if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern)
                    {
                        liMain.Attributes["class"] = "F";
                        if (first)
                        {
                            liMain.Attributes["class"] += " Fst";
                            first = false;
                        }
                    }
                    else
                        if (!((item.SubmenuItemHorizontalAlign == HorizontalAlign.NotSet) || (item.SubmenuItemHorizontalAlign == HorizontalAlign.Left)))
                            liMain.Style[HtmlTextWriterStyle.TextAlign] = item.SubmenuItemHorizontalAlign.ToString().ToLowerInvariant();

                    if (item.GroupInDetailMenu)
                    {
                        lbl = new Label();
                        if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != MasterPageTheme.Modern)
                            lbl.CssClass = "F";
                        lbl.Text = item.CustomName;
                        liMain.Controls.Add(lbl);
                    }
                    else
                        liMain.Controls.Add(CreateLink(item, ((FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern) ? null : "F")));

                    ulMain.Controls.Add(liMain);

                    if (item.IsDetailMenuPage)
                    {
                        ActionCollection availableChildActions = item.GetAvailableChildActions(m_IsAuthenticated, m_IsFrameworkAdmin, m_ActionIdList);
                        if (availableChildActions.Count > 0)
                        {
                            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != MasterPageTheme.Modern)
                                ulSub = new HtmlGenericControl("ul");

                            foreach (Action item2 in availableChildActions)
                            {
                                liSub = new HtmlGenericControl("li");
                                if (!string.IsNullOrEmpty(secondLevelItemCssClass))
                                    liSub.Attributes["class"] = secondLevelItemCssClass;
                                if (!((item2.SubmenuItemHorizontalAlign == HorizontalAlign.NotSet) || (item2.SubmenuItemHorizontalAlign == HorizontalAlign.Left)))
                                    liSub.Style[HtmlTextWriterStyle.TextAlign] = item2.SubmenuItemHorizontalAlign.ToString().ToLowerInvariant();

                                liSub.Controls.Add(CreateLink(item2, secondLevelItemCssClass));

                                if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern)
                                    ulMain.Controls.Add(liSub);
                                else
                                    ulSub.Controls.Add(liSub);
                            }

                            if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != MasterPageTheme.Modern)
                                liMain.Controls.Add(ulSub);
                        }
                    }

                    if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != MasterPageTheme.Modern)
                    {
                        liMain = new HtmlGenericControl("li");
                        liMain.Attributes["class"] = "L";
                        lbl = new Label();
                        lbl.Text = "&nbsp;";
                        liMain.Controls.Add(lbl);
                        ulMain.Controls.Add(liMain);
                    }
                }
            }
            finally
            {
                if (ulMain != null) ulMain.Dispose();
                if (liMain != null) liMain.Dispose();
                if (ulSub != null) ulSub.Dispose();
                if (liSub != null) liSub.Dispose();
                if (lbl != null) lbl.Dispose();
            }
        }

        private void CreateTopSubmenu()
        {
            Label lbl = null;
            HtmlGenericControl ul = null;

            try
            {


                if (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern)
                {
                    m_Container.Attributes["class"] = "Mp_Smt";

                    ul = new HtmlGenericControl("ul");

                    m_Container.Controls.Add(ul);

                    foreach (Action item in Items)
                    {
                        using (HtmlGenericControl li = new HtmlGenericControl("li"))
                        {
                            if (item.ActionId == this.SelectedItemId)
                                li.Attributes["class"] = "S";
                            li.Controls.Add(CreateLink(item));
                            ul.Controls.Add(li);
                        }
                    }
                }
                else
                {
                    m_Container.Attributes["class"] = "Mp_Sm";

                    foreach (Action item in Items)
                    {
                        if (item.GroupInDetailMenu)
                        {
                            lbl = new Label();
                            lbl.CssClass = "F";
                            lbl.Text = item.CustomName;
                            m_Container.Controls.Add(lbl);
                        }
                        else
                            m_Container.Controls.Add(CreateLink(item, "F"));

                        m_Container.Controls.Add(new LiteralControl("&nbsp;:&nbsp;"));

                        if (item.IsDetailMenuPage)
                        {
                            ActionCollection availableChildActions = item.GetAvailableChildActions(m_IsAuthenticated, m_IsFrameworkAdmin, m_ActionIdList);
                            if (availableChildActions.Count > 0)
                            {
                                foreach (Action item2 in availableChildActions)
                                {
                                    m_Container.Controls.Add(CreateLink(item2, "S"));
                                    m_Container.Controls.Add(new LiteralControl("&nbsp;|&nbsp;"));
                                }

                                m_Container.Controls.Add(new LiteralControl("<br/>"));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (lbl != null) lbl.Dispose();
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Creates a new hyperlink from specified action.
        /// </summary>
        /// <param name="action">The actionm to create link from.</param>
        /// <returns>The Link that represents hyperlink from specified action.</returns>
        internal static Control CreateLink(Action action)
        {
            return CreateLink(action, null);
        }

        /// <summary>
        /// Creates a new hyperlink with specifies css class from specified action.
        /// </summary>
        /// <param name="action">The action to create link from.</param>
        /// <param name="cssClass">The css class of the link.</param>
        /// <returns>The Link that represents hyperlink with specifies css class from specified action.</returns>
        internal static Control CreateLink(Action action, string cssClass)
        {
            return CreateLink(action, cssClass, false);
        }

        /// <summary>
        /// Creates a new hyperlink with specifies css class from specified action.
        /// </summary>
        /// <param name="action">The action to create link from.</param>
        /// <param name="cssClass">The css class of the link.</param>
        /// <param name="showIcon">Whether the icon of the link is shown.</param>
        /// <returns>The Link that represents hyperlink with specifies css class from specified action.</returns>
        internal static Control CreateLink(Action action, string cssClass, bool showIcon)
        {
            if (action == null) return null;
            if (cssClass == null) cssClass = string.Empty;

            HtmlInputButton btn = null;
            Link hl = null;

            try
            {
                if ((action.SubmenuItemType == SubmenuItemType.Button) && (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme != MasterPageTheme.Modern))
                {
                    btn = new HtmlInputButton("button");
                    btn.Attributes["value"] = action.CustomName;
                    btn.Attributes["class"] = cssClass;
                    string descr = action.CustomDescription;
                    if (!string.IsNullOrEmpty(descr))
                        btn.Attributes["title"] = descr;
                    if (!action.SubmenuItemWidth.IsEmpty)
                        btn.Style[HtmlTextWriterStyle.Width] = action.SubmenuItemWidth.ToString();
                    btn.Attributes["onclick"] = string.Format(CultureInfo.InvariantCulture, "location.href=\"{0}\";return false;", action.CustomAbsoluteNavigateUrl);
                    return btn;
                }
                else
                {
                    hl = new Link();
                    hl.CssClass = cssClass;
                    hl.Text = action.CustomName;
                    hl.NavigateUrl = action.CustomAbsoluteNavigateUrl;
                    hl.ToolTip = action.CustomDescription;
                    if (showIcon && (action.SubmenuItemType == SubmenuItemType.ImageButton))
                        hl.ImageUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(action.SubmenuItemImageUrl);
                    return hl;
                }
            }
            finally
            {
                if (btn != null) btn.Dispose();
                if (hl != null) hl.Dispose();
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

            HtmlGenericControl htmlContainer = null;

            try
            {
                m_Container = new HtmlGenericControl("div");

                switch (m_Position)
                {
                    case SubmenuPosition.Left:
                        int width = FrameworkConfiguration.Current.WebApplication.MasterPage.LeftArea.Width;
                        int decrement = (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern) ? 40 : 8;
                        if (width > decrement)
                            width -= decrement;

                        m_Container.Style[HtmlTextWriterStyle.Width] = width.ToString(CultureInfo.InvariantCulture) + "px";
                        m_Container.Attributes["class"] = "Mp_L";

                        if ((this.Items != null) && (m_Items.Count > 0))
                            this.CreateLeftSubmenu();

                        if (m_Container.HasControls())
                        {
                            if (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.MasterPage.LeftArea.Html))
                            {
                                m_Container.Controls.Add(new LiteralControl("<br /><br />"));

                                htmlContainer = new HtmlGenericControl("div");
                                htmlContainer.Attributes["class"] = "Mp_Lah";
                                htmlContainer.InnerHtml = FrameworkConfiguration.Current.WebApplication.MasterPage.LeftArea.Html;
                                m_Container.Controls.Add(htmlContainer);
                            }
                        }
                        break;
                    case SubmenuPosition.Top:
                        if ((this.Items != null) && (m_Items.Count > 0))
                            this.CreateTopSubmenu();
                        break;
                }

                if (m_Container.HasControls())
                    this.Controls.Add(m_Container);
            }
            finally
            {
                if (htmlContainer != null) htmlContainer.Dispose();
                if (m_Container != null) m_Container.Dispose();
            }
        }

        #endregion
    }
}
