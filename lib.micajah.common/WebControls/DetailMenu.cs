using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Pages;
using Micajah.Common.Properties;
using Micajah.Common.Security;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// Displays a detailed menu in a Web Forms page.
    /// </summary>
    [ToolboxData("<{0}:DetailMenu runat=server></{0}:DetailMenu>")]
    [ParseChildren(true)]
    public sealed class DetailMenu : WebControl, IPostBackEventHandler
    {
        #region Members

        /// <summary>
        /// The maximal number of the items per column.
        /// </summary>
        private const int MaxItemsInColumn = 8;

        /// <summary>
        /// The maximal number of the child items to be shown.
        /// </summary>
        private const int MaxChildItems = 4;

        private Micajah.Common.Bll.Action m_ParentAction;
        private object m_DataSource;
        private ActionCollection m_PrimaryMenuItems;
        private ActionCollection m_OtherMenuItems;
        private int m_RepeatColumnsInternal;
        private Micajah.Common.Pages.MasterPage m_MasterPage;
        private bool m_IsFrameworkAdmin;
        private bool m_IsAuthenticated;
        private ArrayList m_ActionIdList;
        private UserContext m_UserContext;
        private bool? m_ShowDescriptionAsToolTip;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DetailMenu class.
        /// </summary>
        public DetailMenu()
        {
            m_PrimaryMenuItems = new ActionCollection();
            m_OtherMenuItems = new ActionCollection();

            m_UserContext = UserContext.Current;
            if (m_UserContext != null)
            {
                m_ActionIdList = m_UserContext.ActionIdList;
                m_IsFrameworkAdmin = m_UserContext.IsFrameworkAdministrator;
                m_IsAuthenticated = true;
            }
        }

        #endregion

        #region Events

        public event EventHandler<CommandEventArgs> ItemDataBound;
        public event EventHandler<CommandEventArgs> ItemClick;

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

        private int RepeatColumnsInternal
        {
            get
            {
                if (m_RepeatColumnsInternal == 0)
                {
                    if ((this.Theme != DetailMenuTheme.SideBySide) && (this.Theme != DetailMenuTheme.Modern))
                    {
                        object obj = ViewState["RepeatColumns"];
                        if (obj == null)
                        {
                            int realItemsCount = m_PrimaryMenuItems.Count;
                            if (realItemsCount <= MaxItemsInColumn)
                            {
                                foreach (Micajah.Common.Bll.Action item in m_PrimaryMenuItems)
                                {
                                    if (item.GroupInDetailMenu)
                                    {
                                        realItemsCount += item.GetAvailableChildActions(m_IsAuthenticated, m_IsFrameworkAdmin, m_ActionIdList).Count;
                                        if (realItemsCount > MaxItemsInColumn)
                                        {
                                            m_RepeatColumnsInternal = 2;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                                m_RepeatColumnsInternal = 2;
                        }
                        else
                            m_RepeatColumnsInternal = (int)obj;
                    }
                    if (m_RepeatColumnsInternal == 0)
                        m_RepeatColumnsInternal = 1;
                }
                return m_RepeatColumnsInternal;
            }
        }

        #endregion

        #region Internal Properties

        internal bool? ShowDescriptionAsToolTip
        {
            get { return m_ShowDescriptionAsToolTip; }
            set { m_ShowDescriptionAsToolTip = value; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the object from which the control retrieves its list of items.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object DataSource
        {
            get { return m_DataSource; }
            set { m_DataSource = value; }
        }

        /// <summary>
        /// Gets or sets the type of the object which the control is associated with.
        /// </summary>
        [Category("Data")]
        [Description("The type of the object which the control is associated with.")]
        [DefaultValue("")]
        public string ObjectType
        {
            get { return (string)this.ViewState["ObjectType"]; }
            set { this.ViewState["ObjectType"] = value; }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the object which the control is associated with.
        /// </summary>
        [Category("Data")]
        [Description("The unique identifier of the object which the control is associated with.")]
        [DefaultValue("")]
        public string ObjectId
        {
            get { return (string)this.ViewState["ObjectId"]; }
            set { this.ViewState["ObjectId"] = value; }
        }

        /// <summary>
        /// Gets or sets the identifier of the action the child actions of which are displayed in the control.
        /// </summary>
        [Category("Data")]
        [Description("The identifier of the action the child actions of which are displayed in the control.")]
        [DefaultValue(typeof(Guid), "00000000-0000-0000-0000-000000000000")]
        public Guid ParentActionId
        {
            get
            {
                object obj = ViewState["ParentActionId"];
                return ((obj == null) ? Guid.Empty : (Guid)obj);
            }
            set { ViewState["ParentActionId"] = value; }
        }

        /// <summary>
        /// Gets or sets the action the child actions of which are displayed in the control.
        /// </summary>
        [Browsable(false)]
        public Micajah.Common.Bll.Action ParentAction
        {
            get
            {
                if (m_ParentAction == null)
                {
                    if (this.ParentActionId != Guid.Empty)
                        m_ParentAction = ActionProvider.PagesAndControls.FindByActionId(this.ParentActionId);
                    else if (this.MasterPage != null)
                        m_ParentAction = m_MasterPage.ActiveAction;
                }
                return m_ParentAction;
            }
            set
            {
                m_ParentAction = value;
                this.ParentActionId = ((value == null) ? Guid.Empty : value.ActionId);
            }
        }

        /// <summary>
        /// Gets or sets the number of columns to display in the control.
        /// </summary>
        [Category("Layout")]
        [Description("The number of columns to display in the control.")]
        [DefaultValue(1)]
        public int RepeatColumns
        {
            get
            {
                object obj = ViewState["RepeatColumns"];
                return ((obj == null) ? 1 : (int)obj);
            }
            set
            {
                if (value < 1)
                    ViewState.Remove("RepeatColumns");
                else
                    ViewState["RepeatColumns"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the theme of the detail menu.
        /// </summary>
        [Category("Appearance")]
        [Description("The theme of the detail menu.")]
        [DefaultValue(DetailMenuTheme.Standard)]
        public DetailMenuTheme Theme
        {
            get
            {
                if (!this.DesignMode)
                {
                    object obj = this.ViewState["Theme"];
                    if (obj == null)
                    {
                        if (this.ParentAction != null)
                            return m_ParentAction.DetailMenuTheme;
                    }
                    else
                        return (DetailMenuTheme)obj;
                }
                return DetailMenuTheme.Standard;
            }
            set { this.ViewState["Theme"] = value; }
        }

        /// <summary>
        /// Gets or sets the icon size of detail menu.
        /// </summary>
        [Category("Appearance")]
        [Description("The icon size of detail menu.")]
        [DefaultValue(IconSize.Normal)]
        public IconSize IconSize
        {
            get
            {
                if (!this.DesignMode)
                {
                    object obj = this.ViewState["IconSize"];
                    if (obj == null)
                    {
                        if (this.ParentAction != null)
                            return m_ParentAction.IconSize;
                    }
                    else
                        return (IconSize)obj;
                }
                return IconSize.Normal;
            }
            set { this.ViewState["IconSize"] = value; }
        }

        /// <summary>
        /// Gets or sets the title of the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The title of the control.")]
        [DefaultValue("")]
        public string Title
        {
            get { return (string)this.ViewState["Title"]; }
            set { this.ViewState["Title"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the sibling items are visible and rendered.
        /// </summary>
        [Category("Appearance")]
        [Description("Indicates whether the sibling items are visible and rendered.")]
        [DefaultValue(typeof(bool?), "")]
        public bool? VisibleSiblingItems
        {
            get { return (bool?)ViewState["VisibleSiblingItems"]; }
            set { ViewState["VisibleSiblingItems"] = value; }
        }

        #endregion

        #region Private Methods

        private void BuildMenus()
        {
            ActionCollection allItems = (this.DataSource as ActionCollection);
            if (allItems == null)
            {
                if ((this.ParentAction == null) || (this.ParentAction.ActionType != ActionType.Page)) return;

                allItems = this.ParentAction.GetAvailableChildActions(m_IsAuthenticated, m_IsFrameworkAdmin, m_ActionIdList);

                bool flag = false;
                if (MasterPage != null)
                    flag = (MasterPage.IsHomepage && ((!MasterPage.VisibleMainMenu) || allItems.Count == 0));

                if (flag || (this.VisibleSiblingItems.HasValue && this.VisibleSiblingItems.Value))
                {
                    ActionCollection siblingActions = new ActionCollection();
                    foreach (Micajah.Common.Bll.Action item in ActionProvider.PagesAndControls.GetAvailableSiblingActions(this.ParentAction))
                    {
                        if (ActionProvider.ShowAction(item, m_IsFrameworkAdmin, m_IsAuthenticated, m_ActionIdList))
                        {
                            siblingActions.Add(item);
                        }
                    }
                    allItems.AddRange(siblingActions);
                }
            }

            if (allItems != null)
            {
                foreach (Micajah.Common.Bll.Action item in allItems)
                {
                    if (item.ShowInDetailMenu)
                        m_PrimaryMenuItems.Add(item);
                    else
                        m_OtherMenuItems.Add(item);
                }
            }
        }

        private string GetItemLinkNavigateUrl(Micajah.Common.Bll.Action item)
        {
            if (item.NavigateUrl != null)
            {
                string url = item.CustomAbsoluteNavigateUrl;
                if (!string.IsNullOrEmpty(this.ObjectId))
                {
                    if (url.Contains("?"))
                        url += "&ObjectId=" + HttpUtility.UrlEncodeUnicode(this.ObjectId);
                    else
                        url += "?ObjectId=" + HttpUtility.UrlEncodeUnicode(this.ObjectId);
                }
                return url;
            }
            return this.Page.ClientScript.GetPostBackClientHyperlink(this, item.ActionId.ToString("N"), false);
        }

        private static Control CreateItemDescription(string description)
        {
            using (HtmlGenericControl div = new HtmlGenericControl("div"))
            {
                div.InnerHtml = description;
                return div;
            }
        }

        private HyperLink CreateItemLink(Micajah.Common.Bll.Action item)
        {
            return CreateItemLink(item, null, true);
        }

        private HyperLink CreateItemLink(Micajah.Common.Bll.Action item, string text, bool showToolTip)
        {
            return this.CreateItemLink(item, text, showToolTip, null, true);
        }

        private HyperLink CreateItemLink(Micajah.Common.Bll.Action item, string text, bool showToolTip, Control innerControl, bool raiseItemDataBound)
        {
            using (HyperLink link = new HyperLink())
            {
                if (innerControl != null)
                    link.Controls.Add(innerControl);
                else
                    link.Text = (text == null) ? item.CustomName : text;

                link.NavigateUrl = this.GetItemLinkNavigateUrl(item);

                if (showToolTip)
                {
                    string customDescription = item.CustomDescription;
                    if (!string.IsNullOrEmpty(customDescription))
                    {
                        link.ToolTip = customDescription;
                    }
                }

                if (raiseItemDataBound)
                {
                    if (this.ItemDataBound != null)
                        this.ItemDataBound(link, new CommandEventArgs("ItemDataBound", item));
                }

                return link;
            }
        }

        private static Image CreateItemIcon(Micajah.Common.Bll.Action item, IconSize iconSize)
        {
            Image img = null;

            try
            {
                img = new Image();
                img.ImageAlign = ImageAlign.AbsMiddle;
                img.ImageUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(item.IconUrl);
                if (iconSize != IconSize.NotSet)
                    img.Height = img.Width = Unit.Pixel((int)iconSize);
                string descr = item.CustomDescription;
                if (!string.IsNullOrEmpty(descr)) img.ToolTip = descr;

                return img;
            }
            finally
            {
                if (img != null) img.Dispose();
            }
        }

        private HyperLink CreateItemIconLink(Micajah.Common.Bll.Action item)
        {
            using (HyperLink link = new HyperLink())
            {
                link.NavigateUrl = this.GetItemLinkNavigateUrl(item);

                string customDescription = item.CustomDescription;
                if (!string.IsNullOrEmpty(customDescription)) link.ToolTip = customDescription;

                link.Controls.Add(CreateItemIcon(item, this.IconSize));

                return link;
            }
        }

        private Control CreateChildItems(Micajah.Common.Bll.Action item)
        {
            HtmlGenericControl div = null;
            if (item.ShowChildrenInDetailMenu)
            {
                ActionCollection availableChildActions = item.GetAvailableChildActions(m_IsAuthenticated, m_IsFrameworkAdmin, m_ActionIdList);
                if (availableChildActions.Count > 0)
                {
                    ControlList list = null;
                    try
                    {
                        div = new HtmlGenericControl("div");
                        div.Attributes["class"] = "C";
                        list = new ControlList();
                        list.Delimiter = ", ";

                        foreach (Micajah.Common.Bll.Action childItem in availableChildActions)
                        {
                            if (list.Count < MaxChildItems)
                                list.Add(CreateItemLink(childItem));
                            else
                            {
                                list.Add(CreateItemLink(item, Resources.DetailMenu_Ellipsis, false));
                                break;
                            }
                        }

                        div.Controls.Add(list);
                    }
                    finally
                    {
                        if (div != null) div.Dispose();
                        if (list != null) list.Dispose();
                    }
                }
            }
            return div;
        }

        private Control CreateItem(Micajah.Common.Bll.Action item, bool root)
        {
            HtmlGenericControl div = null;
            HtmlTable table = null;
            HtmlTableRow tr = null;
            HtmlTableCell td = null;
            HyperLink lnk = null;
            Label lbl = null;

            try
            {
                bool rootAndGroup = (root && item.GroupInDetailMenu);

                if ((!string.IsNullOrEmpty(item.IconUrl)) || (this.Theme == DetailMenuTheme.SideBySide))
                {
                    div = new HtmlGenericControl("div");
                    div.Attributes["class"] = "P";

                    table = new HtmlTable();
                    table.CellPadding = table.CellSpacing = 0;

                    tr = new HtmlTableRow();
                    td = new HtmlTableCell();

                    if (!string.IsNullOrEmpty(item.IconUrl))
                    {
                        td.Attributes["class"] = "P";
                        if (rootAndGroup)
                            td.Controls.Add(CreateItemIcon(item, this.IconSize));
                        else
                            td.Controls.Add(CreateItemIconLink(item));
                        tr.Cells.Add(td);

                        td = new HtmlTableCell();
                    }

                    if (rootAndGroup)
                    {
                        lbl = new Label();
                        lbl.Text = item.CustomName;
                        td.Controls.Add(lbl);
                    }
                    else
                    {
                        lnk = CreateItemLink(item, null, ((this.Theme != DetailMenuTheme.SideBySide) || this.ShowDescriptionAsToolTip.GetValueOrDefault(false)));
                        td.Controls.Add(lnk);
                    }

                    if (item.ShowDescriptionInDetailMenu && (this.Theme != DetailMenuTheme.SideBySide))
                    {
                        if (!item.GroupInDetailMenu)
                        {
                            string descr = item.CustomDescription;
                            if (!string.IsNullOrEmpty(descr))
                                td.Controls.Add(CreateItemDescription(descr));
                        }
                    }

                    Control ctrl = CreateChildItems(item);
                    if (ctrl != null) td.Controls.Add(ctrl);

                    tr.Cells.Add(td);

                    if (this.Theme == DetailMenuTheme.SideBySide)
                    {
                        if (!this.ShowDescriptionAsToolTip.GetValueOrDefault(false))
                        {
                            if (!(root && item.GroupInDetailMenu) && item.ShowDescriptionInDetailMenu)
                            {
                                td.Width = "35%";

                                td = new HtmlTableCell();
                                td.Attributes["class"] = "D";
                                td.Width = "65%";
                                td.InnerHtml = item.CustomDescription;
                                tr.Cells.Add(td);
                            }
                        }
                    }

                    table.Rows.Add(tr);
                    div.Controls.Add(table);
                }
                else
                {
                    div = CreateSimpleItem(item, rootAndGroup);
                }

                return div;
            }
            finally
            {
                if (div != null) div.Dispose();
                if (table != null) table.Dispose();
                if (tr != null) tr.Dispose();
                if (td != null) td.Dispose();
                if (lnk != null) lnk.Dispose();
                if (lbl != null) lbl.Dispose();
            }
        }

        private HtmlGenericControl CreateSimpleItem(Micajah.Common.Bll.Action item, bool rootAndGroup)
        {
            HtmlGenericControl div = null;
            Label lbl = null;

            try
            {
                div = new HtmlGenericControl("div");

                if (rootAndGroup)
                {
                    lbl = new Label();
                    lbl.Text = item.CustomName;
                    div.Controls.Add(lbl);
                }
                else
                    div.Controls.Add(CreateItemLink(item));

                if ((!item.GroupInDetailMenu) && item.ShowDescriptionInDetailMenu && (this.Theme != DetailMenuTheme.Decorated))
                {
                    string descr = item.CustomDescription;
                    if (!string.IsNullOrEmpty(descr))
                        div.Controls.Add(CreateItemDescription(descr));
                }

                Control ctrl = CreateChildItems(item);
                if (ctrl != null)
                    div.Controls.Add(ctrl);

                return div;
            }
            finally
            {
                if (div != null) div.Dispose();
                if (lbl != null) lbl.Dispose();
            }
        }

        private Table CreateOtherMenusAsTable()
        {
            if (m_OtherMenuItems.Count > 0)
            {
                Table table = null;
                TableRow tr = null;
                TableCell td = null;

                try
                {
                    table = new Table();
                    table.CssClass = "Mp_Om";

                    foreach (Micajah.Common.Bll.Action item in m_OtherMenuItems)
                    {
                        tr = new TableRow();
                        td = new TableCell();
                        td.Controls.Add(CreateItemLink(item));
                        tr.Cells.Add(td);
                        table.Rows.Add(tr);
                    }
                }
                finally
                {
                    if (table != null) table.Dispose();
                    if (tr != null) tr.Dispose();
                    if (td != null) td.Dispose();
                }

                return table;
            }
            return null;
        }

        private static string GetItemCssClass(bool isAlternatedItem, bool highlighted, bool group)
        {
            string cssClass = "I";
            if (isAlternatedItem) cssClass = "A";
            if (highlighted)
            {
                if (!string.IsNullOrEmpty(cssClass))
                    cssClass += " H";
                else
                    cssClass = "H";
            }
            if (group) cssClass = "G";
            return cssClass;
        }

        private TableRow[] CreateDetailMenuRows(ActionCollection items, bool root)
        {
            List<TableRow> list = new List<TableRow>();
            TableRow tr = null;
            TableCell td = null;
            int cellIndex = 0;
            bool isAlternatedRow = false;

            try
            {
                foreach (Micajah.Common.Bll.Action item in items)
                {
                    bool rootAndGroup = (root && item.GroupInDetailMenu);
                    if ((cellIndex == 0) || rootAndGroup)
                    {
                        if (rootAndGroup)
                        {
                            if (tr != null)
                            {
                                cellIndex = 0;
                                isAlternatedRow = false;
                            }
                        }
                        else
                            isAlternatedRow = (!isAlternatedRow);

                        tr = new TableRow();
                        list.Add(tr);
                    }

                    td = new TableCell();
                    string cssClass = GetItemCssClass(isAlternatedRow, item.HighlightInDetailMenu, rootAndGroup);
                    if (!string.IsNullOrEmpty(cssClass)) td.CssClass = cssClass;
                    td.Controls.Add(CreateItem(item, root));
                    tr.Cells.Add(td);

                    if (rootAndGroup)
                    {
                        if (this.RepeatColumnsInternal > 1) td.ColumnSpan = this.RepeatColumnsInternal;
                        list.AddRange(CreateDetailMenuRows(item.GetAvailableChildActions(m_IsAuthenticated, m_IsFrameworkAdmin, m_ActionIdList), false));
                    }
                    else
                    {
                        cellIndex++;
                        if (cellIndex == this.RepeatColumnsInternal) cellIndex = 0;
                    }
                }

                ProcessDetailMenuRows(list);
            }
            finally
            {
                if (tr != null) tr.Dispose();
                if (td != null) td.Dispose();
            }

            return list.ToArray();
        }

        private void ProcessDetailMenuRows(List<TableRow> list)
        {
            if (this.RepeatColumnsInternal > 1)
            {
                int columnWidth = Convert.ToInt32(Math.Floor(((double)100 / (double)this.RepeatColumnsInternal)), CultureInfo.InvariantCulture);
                bool widthAssigned = false;
                foreach (TableRow row in list)
                {
                    int cellsCount = 0;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (cell.ColumnSpan > 1)
                            cellsCount += cell.ColumnSpan;
                        else
                            cellsCount++;
                    }

                    for (var idx = 0; idx < (this.RepeatColumnsInternal - cellsCount); idx++)
                    {
                        TableCell td = new TableCell();
                        td.Text = "&nbsp;";
                        row.Cells.Add(td);
                    }

                    if ((!widthAssigned) && (row.Cells.Count == this.RepeatColumnsInternal))
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            cell.Width = Unit.Percentage(columnWidth);
                        }
                        widthAssigned = true;
                    }
                }
            }
        }

        private List<HtmlGenericControl> CreateDetailMenuItems(ActionCollection items, bool root)
        {
            List<HtmlGenericControl> list = new List<HtmlGenericControl>();
            HtmlGenericControl h = null;
            HtmlGenericControl li = null;
            HtmlGenericControl p = null;
            HtmlGenericControl span = null;
            HyperLink link = null;

            try
            {
                foreach (Micajah.Common.Bll.Action action in items)
                {
                    li = new HtmlGenericControl("li");

                    if (root && action.GroupInDetailMenu)
                    {
                        li.Attributes["class"] = "G";
                        h = new HtmlGenericControl("h1");
                        li.Controls.Add(h);
                    }
                    else
                    {
                        h = new HtmlGenericControl("h2");
                        li.Controls.Add(this.CreateItemLink(action, null, false, h, false));
                    }

                    h.InnerHtml = action.CustomName;
                    list.Add(li);

                    if (root && action.GroupInDetailMenu)
                        list.AddRange(this.CreateDetailMenuItems(action.GetAvailableChildActions(this.m_IsAuthenticated, this.m_IsFrameworkAdmin, this.m_ActionIdList), false));

                    if (!string.IsNullOrEmpty(action.VideoUrl))
                    {
                        link = new HyperLink();
                        link.NavigateUrl = action.VideoUrl;
                        link.Target = "_blank";
                        link.Text = Resources.DetailMenu_VideoLink_Text;

                        span = new HtmlGenericControl("span");
                        span.Controls.Add(link);

                        li.Controls.Add(span);
                    }

                    if (!string.IsNullOrEmpty(action.LearnMoreUrl))
                    {
                        link = new HyperLink();
                        link.NavigateUrl = action.LearnMoreUrl;
                        link.Target = "_blank";
                        link.Text = Resources.DetailMenu_LearnMoreLink_Text;

                        span = new HtmlGenericControl("span");
                        span.Controls.Add(link);

                        li.Controls.Add(span);
                    }

                    string customDescription = action.CustomDescription;
                    if (!string.IsNullOrEmpty(customDescription))
                    {
                        p = new HtmlGenericControl("p");
                        p.InnerHtml = customDescription;
                        li.Controls.Add(p);
                    }

                    if (this.ItemDataBound != null)
                        this.ItemDataBound(li, new CommandEventArgs("ItemDataBound", action));
                }
            }
            finally
            {
                if (h != null) h.Dispose();
                if (li != null) li.Dispose();
                if (p != null) p.Dispose();
                if (span != null) span.Dispose();
            }

            return list;
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Creates child controls.
        /// </summary>
        protected override void CreateChildControls()
        {
            if (this.DesignMode) return;

            this.Controls.Clear();
            this.BuildMenus();

            if ((m_PrimaryMenuItems.Count == 0) && (m_OtherMenuItems.Count == 0)) return;

            if (this.Theme == DetailMenuTheme.Modern)
            {
                this.CssClass = "Mp_Dm";

                HtmlGenericControl ul = null;

                try
                {
                    foreach (HtmlGenericControl ctl in this.CreateDetailMenuItems(this.m_PrimaryMenuItems, true))
                    {
                        if (ctl.TagName != "li")
                        {
                            ul = null;
                            this.Controls.Add(ctl);
                        }
                        else
                        {
                            if (ul == null)
                            {
                                ul = new HtmlGenericControl("ul");
                                this.Controls.Add(ul);
                            }
                            ul.Controls.Add(ctl);
                        }
                    }
                }
                finally
                {
                    if (ul != null) ul.Dispose();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.Title))
                {
                    using (HtmlGenericControl p = new HtmlGenericControl("p"))
                    {
                        p.Attributes["class"] = "Mp_Dm_T";
                        p.InnerHtml = this.Title;
                        this.Controls.Add(p);
                    }
                }

                Table table = null;
                TableRow tr = null;
                TableCell td = null;
                Table otherMenusTable = null;

                try
                {
                    table = new Table();
                    table.Attributes["id"] = "Mp_Dm";
                    table.CssClass = "Mp_Dm";
                    table.CellPadding = 0;
                    switch (this.Theme)
                    {
                        case DetailMenuTheme.SideBySide:
                            table.CellSpacing = 2;
                            break;
                        case DetailMenuTheme.Decorated:
                            table.CellSpacing = 0;
                            break;
                        default:
                            table.CellSpacing = 5;
                            break;
                    }
                    table.Width = ((this.RepeatColumnsInternal == 1) ? Unit.Pixel(600) : Unit.Percentage(100));
                    if (!this.Height.IsEmpty) table.Height = Height;
                    table.Rows.AddRange(this.CreateDetailMenuRows(m_PrimaryMenuItems, true));

                    otherMenusTable = this.CreateOtherMenusAsTable();
                    if (otherMenusTable != null)
                    {
                        tr = new TableRow();
                        tr.VerticalAlign = VerticalAlign.Bottom;

                        td = new TableCell();
                        td.Height = Unit.Percentage(100);
                        td.ColumnSpan = 2;

                        if (m_PrimaryMenuItems.Count > 0) td.Controls.Add(new LiteralControl("<hr />"));
                        td.Controls.Add(new LiteralControl(Resources.DetailMenu_OtherLinks_Caption));
                        td.Controls.Add(otherMenusTable);

                        tr.Cells.Add(td);
                        table.Rows.Add(tr);

                        if (this.Height.IsEmpty) table.Height = Unit.Percentage(100);
                    }

                    if (table.Rows.Count > 0) this.Controls.Add(table);
                }
                finally
                {
                    if (table != null) table.Dispose();
                    if (tr != null) tr.Dispose();
                    if (td != null) td.Dispose();
                    if (otherMenusTable != null) otherMenusTable.Dispose();
                }
            }
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.Load event.
        /// </summary>
        /// <param name="e">The System.EventArgs object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ResourceProvider.RegisterStyleSheetResource(this, ResourceProvider.GetDetailMenuThemeStyleSheet(this.Theme), this.Theme.ToString());
        }

        /// <summary>
        /// Registers the client scripts.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.Theme == DetailMenuTheme.Reflective)
            {
                Type type = this.GetType();
                ScriptManager.RegisterStartupScript(this, type, "AttachHoverEvents", "Mp_AttachHoverEvents('Mp_Dm', 'TD', 'A,I,H');\r\n", true);
                ScriptManager.RegisterStartupScript(this, type, "AttachClickEventsToTableCells", "Mp_AttachClickEventsToTableCells('Mp_Dm', 'A,I,H');\r\n", true);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (writer == null) return;

            if (this.DesignMode)
                writer.Write(this.ID);
            else
                base.Render(writer);
        }

        #endregion

        #region Public Methods

        public void RaisePostBackEvent(string eventArgument)
        {
            if (this.ItemClick != null)
            {
                Guid actionId = Guid.Empty;
                Micajah.Common.Bll.Action action = null;
                object obj = Support.ConvertStringToType(eventArgument, typeof(Guid));
                if (obj != null) actionId = (Guid)obj;
                if (actionId != Guid.Empty)
                {
                    action = ActionProvider.PagesAndControls.FindByActionId(actionId);
                    if (action != null) action = action.Clone();
                }
                this.ItemClick(this, new CommandEventArgs("ItemClick", action));
            }
        }

        #endregion
    }
}