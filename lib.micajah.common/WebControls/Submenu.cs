using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

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
        private ActionCollection m_Items;
        private bool m_ItemsIsLoaded;
        private HtmlGenericControl m_MainContainer;
        private System.Guid? m_SelectedItemId;
        private bool m_ModernTheme;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Submenu control as left submenu.
        /// </summary>
        public Submenu(Micajah.Common.Pages.MasterPage masterPage, IList actionIdList, bool isFrameworkAdmin, bool isAuthenticated)
        {
            m_MasterPage = masterPage;
            m_ActionIdList = actionIdList;
            m_IsFrameworkAdmin = isFrameworkAdmin;
            m_IsAuthenticated = isAuthenticated;
            m_Position = SubmenuPosition.Left;
            m_ModernTheme = (FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == MasterPageTheme.Modern);
        }

        /// <summary>
        /// Initializes a new instance of the Submenu control with specified type.
        /// </summary>
        /// <param name="position">Specifies the position of the submenu on the page. One of the SubmenuPosition enumerations.</param>
        public Submenu(Micajah.Common.Pages.MasterPage masterPage, IList actionIdList, bool isFrameworkAdmin, bool isAuthenticated, SubmenuPosition position)
            : this(masterPage, actionIdList, isFrameworkAdmin, isAuthenticated)
        {
            m_Position = position;
        }

        #endregion

        #region Private Properties

        private System.Guid SelectedItemId
        {
            get
            {
                if (!m_SelectedItemId.HasValue)
                {
                    System.Guid itemId = System.Guid.Empty;
                    if (m_MasterPage != null)
                    {
                        if (m_MasterPage.ActiveAction != null)
                        {
                            switch (m_Position)
                            {
                                case SubmenuPosition.Left:
                                    itemId = m_MasterPage.ActiveAction.ActionId;
                                    break;
                                case SubmenuPosition.Top:
                                    Micajah.Common.Bll.Action item = ActionProvider.PagesAndControls.FindSubmenuActionByActionId(m_MasterPage.ActiveAction.ActionId, this.ParentActionId);
                                    if (item != null)
                                        itemId = item.ActionId;
                                    break;
                            }
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
                        m_Items = m_ParentAction.GetAvailableChildActions(m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated);
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
                    else if (m_MasterPage != null)
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
                m_MainContainer.Controls.Add(ulMain);

                if (!m_ModernTheme)
                    secondLevelItemCssClass = "S";

                bool first = true;
                foreach (Action item in this.Items)
                {
                    liMain = new HtmlGenericControl("li");
                    if (m_ModernTheme)
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
                        if (!m_ModernTheme)
                            lbl.CssClass = "F";
                        lbl.Text = item.CustomName;
                        liMain.Controls.Add(lbl);
                    }
                    else
                        liMain.Controls.Add(CreateLink(item, (m_ModernTheme ? null : "F"), m_ModernTheme));

                    ulMain.Controls.Add(liMain);

                    if (item.IsDetailMenuPage)
                    {
                        ActionCollection availableChildActions = item.GetAvailableChildActions(m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated);
                        if (availableChildActions.Count > 0)
                        {
                            if (!m_ModernTheme)
                                ulSub = new HtmlGenericControl("ul");

                            foreach (Action item2 in availableChildActions)
                            {
                                liSub = new HtmlGenericControl("li");

                                string cssClass = secondLevelItemCssClass;
                                if (item2.HighlightInSubmenu)
                                {
                                    if (!string.IsNullOrEmpty(cssClass)) cssClass += " ";
                                    cssClass += "H";
                                }
                                if (item2.ActionId == this.SelectedItemId)
                                {
                                    if (!string.IsNullOrEmpty(cssClass)) cssClass += " ";
                                    cssClass += "S";
                                }

                                if (!string.IsNullOrEmpty(cssClass))
                                    liSub.Attributes["class"] = cssClass;

                                if (!((item2.SubmenuItemHorizontalAlign == HorizontalAlign.NotSet) || (item2.SubmenuItemHorizontalAlign == HorizontalAlign.Left)))
                                    liSub.Style[HtmlTextWriterStyle.TextAlign] = item2.SubmenuItemHorizontalAlign.ToString().ToLowerInvariant();

                                liSub.Controls.Add(CreateLink(item2, secondLevelItemCssClass, true, m_ModernTheme));

                                if (m_ModernTheme)
                                    ulMain.Controls.Add(liSub);
                                else
                                    ulSub.Controls.Add(liSub);
                            }

                            if (!m_ModernTheme)
                                liMain.Controls.Add(ulSub);
                        }
                    }

                    if (!m_ModernTheme)
                    {
                        liMain = new HtmlGenericControl("li");
                        liMain.Attributes["class"] = "L";
                        lbl = new Label();
                        lbl.Text = Resources.NonBreakingSpace;
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
            HtmlGenericControl container = null;
            HtmlGenericControl innerContainer = null;

            try
            {
                if (m_ModernTheme)
                {
                    ul = new HtmlGenericControl("ul");

                    innerContainer = new HtmlGenericControl("div");
                    innerContainer.Attributes["class"] = "col-sm-12";
                    innerContainer.Controls.Add(ul);

                    container = new HtmlGenericControl("div");
                    container.Attributes["class"] = "container";
                    container.Controls.Add(innerContainer);

                    m_MainContainer.Attributes["class"] = "Mp_Smt";
                    m_MainContainer.Controls.Add(container);

                    foreach (Action item in Items)
                    {
                        using (HtmlGenericControl li = new HtmlGenericControl("li"))
                        {
                            if (item.ActionId == this.SelectedItemId)
                            {
                                li.Attributes["class"] = "active";
                            }

                            li.Controls.Add(CreateLink(item, m_ModernTheme));
                            ul.Controls.Add(li);
                        }
                    }
                }
                else
                {
                    m_MainContainer.Attributes["class"] = "Mp_Sm";

                    foreach (Action item in Items)
                    {
                        if (item.GroupInDetailMenu)
                        {
                            lbl = new Label();
                            lbl.CssClass = "F";
                            lbl.Text = item.CustomName;

                            m_MainContainer.Controls.Add(lbl);
                        }
                        else
                        {
                            m_MainContainer.Controls.Add(CreateLink(item, "F", m_ModernTheme));
                        }

                        m_MainContainer.Controls.Add(new LiteralControl("&nbsp;:&nbsp;"));

                        if (item.IsDetailMenuPage)
                        {
                            ActionCollection availableChildActions = item.GetAvailableChildActions(m_ActionIdList, m_IsFrameworkAdmin, m_IsAuthenticated);
                            if (availableChildActions.Count > 0)
                            {
                                foreach (Action item2 in availableChildActions)
                                {
                                    m_MainContainer.Controls.Add(CreateLink(item2, "S", m_ModernTheme));
                                    m_MainContainer.Controls.Add(new LiteralControl("&nbsp;|&nbsp;"));
                                }

                                m_MainContainer.Controls.Add(new LiteralControl("<br/>"));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (lbl != null)
                {
                    lbl.Dispose();
                }

                if (container != null)
                {
                    container.Dispose();
                }

                if (innerContainer != null)
                {
                    innerContainer.Dispose();
                }
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Creates a new hyperlink from specified action.
        /// </summary>
        /// <param name="action">The actionm to create link from.</param>
        /// <param name="modernTheme">Whether the theme is modern.</param>
        /// <returns>The Link that represents hyperlink from specified action.</returns>
        internal static Control CreateLink(Action action, bool modernTheme)
        {
            return CreateLink(action, null, modernTheme);
        }

        /// <summary>
        /// Creates a new hyperlink with specifies css class from specified action.
        /// </summary>
        /// <param name="action">The action to create link from.</param>
        /// <param name="cssClass">The css class of the link.</param>
        /// <param name="modernTheme">Whether the theme is modern.</param>
        /// <returns>The Link that represents hyperlink with specifies css class from specified action.</returns>
        internal static Control CreateLink(Action action, string cssClass, bool modernTheme)
        {
            return CreateLink(action, cssClass, false, modernTheme);
        }

        /// <summary>
        /// Creates a new hyperlink with specifies css class from specified action.
        /// </summary>
        /// <param name="action">The action to create link from.</param>
        /// <param name="cssClass">The css class of the link.</param>
        /// <param name="showIcon">Whether the icon of the link is shown.</param>
        /// <param name="modernTheme">Whether the theme is modern.</param>
        /// <returns>The Link that represents hyperlink with specifies css class from specified action.</returns>
        internal static Control CreateLink(Action action, string cssClass, bool showIcon, bool modernTheme)
        {
            if (action == null) return null;
            if (cssClass == null) cssClass = string.Empty;

            HtmlInputButton button = null;
            HyperLink link = null;

            try
            {
                if ((action.SubmenuItemType == SubmenuItemType.Button) && (!modernTheme))
                {
                    button = new HtmlInputButton("button");
                    button.Attributes["value"] = action.CustomName;
                    button.Attributes["class"] = cssClass;
                    button.Attributes["onclick"] = string.Format(CultureInfo.InvariantCulture, "location.href=\"{0}\";return false;", action.CustomAbsoluteNavigateUrl);

                    string descr = action.CustomDescription;
                    if (!string.IsNullOrEmpty(descr))
                    {
                        button.Attributes["title"] = descr;
                    }

                    if (action.SubmenuItemWidth > 0)
                    {
                        button.Style[HtmlTextWriterStyle.Width] = action.SubmenuItemWidth.ToString(CultureInfo.InvariantCulture) + "px";
                    }

                    return button;
                }
                else
                {
                    link = new HyperLink();
                    link.CssClass = cssClass;
                    link.NavigateUrl = action.CustomAbsoluteNavigateUrl;
                    link.ToolTip = action.CustomDescription;

                    if (showIcon && (action.SubmenuItemType == SubmenuItemType.ImageButton))
                    {
                        using (Image image = new Image())
                        {
                            image.ImageUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(action.SubmenuItemImageUrl);
                            image.ToolTip = action.CustomDescription;
                            link.Controls.Add(image);
                        }

                        using (LiteralControl literal = new LiteralControl(action.CustomName))
                        {
                            link.Controls.Add(literal);
                        }
                    }
                    else
                    {
                        link.Text = action.CustomName;
                    }

                    return link;
                }
            }
            finally
            {
                if (button != null) button.Dispose();
                if (link != null) link.Dispose();
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
                m_MainContainer = new HtmlGenericControl("div");

                switch (m_Position)
                {
                    case SubmenuPosition.Left:
                        int width = FrameworkConfiguration.Current.WebApplication.MasterPage.LeftArea.Width;
                        int decrement = m_ModernTheme ? 40 : 8;
                        if (width > decrement)
                            width -= decrement;

                        m_MainContainer.Style[HtmlTextWriterStyle.Width] = width.ToString(CultureInfo.InvariantCulture) + "px";
                        m_MainContainer.Attributes["class"] = "Mp_L";

                        if ((this.Items != null) && (m_Items.Count > 0))
                            this.CreateLeftSubmenu();

                        if (m_MainContainer.HasControls())
                        {
                            if (!string.IsNullOrEmpty(FrameworkConfiguration.Current.WebApplication.MasterPage.LeftArea.Html))
                            {
                                m_MainContainer.Controls.Add(new LiteralControl("<br /><br />"));

                                htmlContainer = new HtmlGenericControl("div");
                                htmlContainer.Attributes["class"] = "Mp_Lah";
                                htmlContainer.InnerHtml = FrameworkConfiguration.Current.WebApplication.MasterPage.LeftArea.Html;
                                m_MainContainer.Controls.Add(htmlContainer);
                            }
                        }
                        break;
                    case SubmenuPosition.Top:
                        if ((this.Items != null) && (m_Items.Count > 0))
                            this.CreateTopSubmenu();
                        break;
                }

                if (m_MainContainer.HasControls())
                    this.Controls.Add(m_MainContainer);
            }
            finally
            {
                if (htmlContainer != null) htmlContainer.Dispose();
                if (m_MainContainer != null) m_MainContainer.Dispose();
            }
        }

        #endregion
    }
}
