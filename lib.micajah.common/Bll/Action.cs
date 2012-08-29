using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.WebControls;

namespace Micajah.Common.Bll
{
    /// <summary>
    /// The action.
    /// </summary>
    [Serializable]
    public sealed class Action : ICloneable, IComparable<Action>
    {
        #region Members

        private Guid m_ActionId;
        private ActionType m_ActionType;
        private Guid? m_ParentActionId;
        private string m_Name;
        private string m_Description;
        private string m_IconUrl;
        private SubmenuItemType m_SubmenuItemType;
        private string m_SubmenuItemImageUrl;
        private Unit m_SubmenuItemWidth;
        private HorizontalAlign m_SubmenuItemHorizontalAlign;
        private string m_NavigateUrl;
        private string m_LearnMoreUrl;
        private string m_VideoUrl;
        private int m_OrderNumber;
        private bool m_AuthenticationRequired;
        private bool m_InstanceRequired;
        private bool m_Visible;
        private bool m_ShowInDetailMenu;
        private bool m_ShowChildrenInDetailMenu;
        private bool m_ShowDescriptionInDetailMenu;
        private bool m_GroupInDetailMenu;
        private bool m_HighlightInDetailMenu;
        private DetailMenuTheme? m_DetailMenuTheme;
        private IconSize? m_IconSize;
        private bool m_BuiltIn;

        private bool m_IsCustom;
        private Action m_ParentAction;
        private Action m_ParentMainMenuAction;
        private ActionCollection m_ChildActions;
        private ActionCollection m_ChildControls;
        private ActionCollection m_AlternativeParentActions;

        #endregion

        #region Internal Properties

        internal bool BuiltIn
        {
            get { return m_BuiltIn; }
            set { m_BuiltIn = value; }
        }

        internal bool IsCustom
        {
            get { return m_IsCustom; }
            set { m_IsCustom = value; }
        }

        internal bool IsDetailMenuPage
        {
            get { return ((this.ActionType == ActionType.Page) && (string.Compare(m_NavigateUrl, string.Empty, StringComparison.OrdinalIgnoreCase) == 0)); }
        }

        /// <summary>
        /// Gets a value indicating that the action is a member of the main menu.
        /// </summary>
        internal bool IsMainMenuRoot
        {
            get { return (this.ParentActionId == ActionProvider.PagesAndControlsActionId); }
        }

        /// <summary>
        /// Gets a value indicating that the action is a member of the global navigation links.
        /// </summary>
        internal bool IsGlobalNavigationLinksRoot
        {
            get { return (this.ParentActionId == ActionProvider.GlobalNavigationLinksActionId); }
        }

        /// <summary>
        /// Gets a collection of the child controls of this Action.
        /// </summary>
        internal ActionCollection ChildControls
        {
            get
            {
                if (m_ChildControls == null) m_ChildControls = new ActionCollection();
                return m_ChildControls;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the identifier of the action.
        /// </summary>
        public Guid ActionId
        {
            get { return m_ActionId; }
            set { m_ActionId = value; }
        }

        /// <summary>
        /// Gets or sets the identifier of the parent action.
        /// </summary>
        public Guid? ParentActionId
        {
            get { return m_ParentActionId; }
            set
            {
                m_ParentActionId = value;
                m_ParentAction = null;
            }
        }

        /// <summary>
        /// Gets or sets the action name.
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// Gets or sets the description of the action.
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        /// <summary>
        /// Gets or sets the URL to icon of the action.
        /// </summary>
        public string IconUrl
        {
            get { return m_IconUrl; }
            set { m_IconUrl = value; }
        }

        /// <summary>
        /// Gets or sets the URL to the image for the action that is rendered as image in the submenu.
        /// </summary>
        public string SubmenuItemImageUrl
        {
            get { return m_SubmenuItemImageUrl; }
            set { m_SubmenuItemImageUrl = value; }
        }

        /// <summary>
        /// Gets or sets the type of the action in the submenu.
        /// </summary>
        public SubmenuItemType SubmenuItemType
        {
            get { return m_SubmenuItemType; }
            set { m_SubmenuItemType = value; }
        }

        /// <summary>
        /// Gets or sets the width for the action that is rendered in the submenu.
        /// </summary>
        public Unit SubmenuItemWidth
        {
            get { return m_SubmenuItemWidth; }
            set { m_SubmenuItemWidth = value; }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment of the action in the submenu.
        /// </summary>
        public HorizontalAlign SubmenuItemHorizontalAlign
        {
            get { return m_SubmenuItemHorizontalAlign; }
            set { m_SubmenuItemHorizontalAlign = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the action to navigate.
        /// </summary>
        public string NavigateUrl
        {
            get { return m_NavigateUrl; }
            set { m_NavigateUrl = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the page to learn more.
        /// </summary>
        public string LearnMoreUrl
        {
            get { return m_LearnMoreUrl; }
            set { m_LearnMoreUrl = value; }
        }

        /// <summary>
        /// Gets or sets the URL of the video.
        /// </summary>
        public string VideoUrl
        {
            get { return m_VideoUrl; }
            set { m_VideoUrl = value; }
        }

        /// <summary>
        /// Gets or sets the order number of the action.
        /// </summary>
        public int OrderNumber
        {
            get { return m_OrderNumber; }
            set { m_OrderNumber = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the action requires the authentication.
        /// </summary>
        public bool AuthenticationRequired
        {
            get { return m_AuthenticationRequired; }
            set { m_AuthenticationRequired = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the action requires the selected instance.
        /// </summary>
        public bool InstanceRequired
        {
            get { return m_InstanceRequired; }
            set { m_InstanceRequired = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the action is visible.
        /// </summary>
        public bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the action is rendered in the detail menu.
        /// </summary>
        public bool ShowInDetailMenu
        {
            get { return m_ShowInDetailMenu; }
            set { m_ShowInDetailMenu = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the child actions is rendered in the detail menu.
        /// </summary>
        public bool ShowChildrenInDetailMenu
        {
            get { return m_ShowChildrenInDetailMenu; }
            set { m_ShowChildrenInDetailMenu = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the description is rendered in the detail menu.
        /// </summary>
        public bool ShowDescriptionInDetailMenu
        {
            get { return m_ShowDescriptionInDetailMenu; }
            set { m_ShowDescriptionInDetailMenu = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the action is rendered as group in the detail menu.
        /// </summary>
        public bool GroupInDetailMenu
        {
            get { return m_GroupInDetailMenu; }
            set { m_GroupInDetailMenu = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the action is highlighted in the detail menu.
        /// </summary>
        public bool HighlightInDetailMenu
        {
            get { return m_HighlightInDetailMenu; }
            set { m_HighlightInDetailMenu = value; }
        }

        /// <summary>
        /// Gets or sets the theme of the detail menu action.
        /// </summary>
        public DetailMenuTheme DetailMenuTheme
        {
            get { return (m_DetailMenuTheme.HasValue ? m_DetailMenuTheme.Value : FrameworkConfiguration.Current.WebApplication.MasterPage.DetailMenu.Theme); }
            set { m_DetailMenuTheme = value; }
        }

        /// <summary>
        /// Gets or sets the size of icon of the action in the detail menu.
        /// </summary>
        public IconSize IconSize
        {
            get { return (m_IconSize.HasValue ? m_IconSize.Value : FrameworkConfiguration.Current.WebApplication.MasterPage.DetailMenu.IconSize); }
            set { m_IconSize = value; }
        }

        /// <summary>
        /// Gets an application absolute navigate URL of the action.
        /// </summary>
        public string AbsoluteNavigateUrl
        {
            get
            {
                return this.IsDetailMenuPage ? ResourceProvider.GetDetailMenuPageUrl(this.ActionId) : WebApplication.CreateApplicationAbsoluteUrl(m_NavigateUrl);
            }
        }

        /// <summary>
        /// Gets or sets the parent action.
        /// </summary>
        public Action ParentAction
        {
            get
            {
                if (m_ParentAction == null && m_ParentActionId.HasValue)
                    m_ParentAction = ActionProvider.GetAction(m_ParentActionId.Value);
                return m_ParentAction;
            }
            set
            {
                m_ParentAction = value;
                m_ParentActionId = ((value == null) ? null : new Guid?(value.ActionId));
            }
        }

        /// <summary>
        /// Gets or sets the action type.
        /// </summary>
        public ActionType ActionType
        {
            get { return m_ActionType; }
            set { m_ActionType = value; }
        }

        /// <summary>
        /// Gets the child actions.
        /// </summary>
        public ActionCollection ChildActions
        {
            get
            {
                if (m_ChildActions == null) m_ChildActions = new ActionCollection();
                return m_ChildActions;
            }
        }

        /// <summary>
        /// Gets the collection of the alternative parent actions.
        /// </summary>
        public ActionCollection AlternativeParentActions
        {
            get
            {
                if (m_AlternativeParentActions == null) m_AlternativeParentActions = ActionProvider.GetAlternativeParentActions(m_ActionId);
                return m_AlternativeParentActions;
            }
        }

        /// <summary>
        /// Gets the parent main menu action.
        /// </summary>
        public Action ParentMainMenuAction
        {
            get
            {
                if (m_ParentMainMenuAction == null)
                {
                    if (this.IsMainMenuRoot)
                        m_ParentMainMenuAction = this;
                    else
                    {
                        m_ParentMainMenuAction = this.ParentAction;
                        while ((m_ParentMainMenuAction != null) && (!m_ParentMainMenuAction.IsMainMenuRoot))
                        {
                            m_ParentMainMenuAction = m_ParentMainMenuAction.ParentAction;
                        }
                    }
                }
                return m_ParentMainMenuAction;
            }
        }

        /// <summary>
        /// Gets or sets the value indicating the action should be handled.
        /// </summary>
        public bool Handle
        {
            get;
            set;
        }

        public string CustomDescription
        {
            get
            {
                return (this.Handle
                    ? (this.BuiltIn ? Handlers.ActionHandler.Instance.GetDescription(this)
                    : Handlers.ActionHandler.Current.GetDescription(this)) : this.Description);
            }
        }

        public string CustomName
        {
            get
            {
                return (this.Handle
                    ? (this.BuiltIn ? Handlers.ActionHandler.Instance.GetName(this) : Handlers.ActionHandler.Current.GetName(this))
                    : this.Name);
            }
        }

        public string CustomAbsoluteNavigateUrl
        {
            get
            {
                return (this.Handle
                    ? WebApplication.CreateApplicationAbsoluteUrl(this.BuiltIn ? Handlers.ActionHandler.Instance.GetNavigateUrl(this) : Handlers.ActionHandler.Current.GetNavigateUrl(this))
                    : this.AbsoluteNavigateUrl);
            }
        }

        #endregion

        #region Operators

        public static bool operator ==(Action action1, Action action2)
        {
            if (object.ReferenceEquals(action1, action2))
                return true;

            if (((object)action1 == null) || ((object)action2 == null))
                return false;

            return action1.Equals(action2);
        }

        public static bool operator !=(Action action1, Action action2)
        {
            return (!(action1 == action2));
        }

        public static bool operator <(Action action1, Action action2)
        {
            return (action1.CompareTo(action2) < 0);
        }

        public static bool operator >(Action action1, Action action2)
        {
            return (action1.CompareTo(action2) > 0);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Sets the specified collection as child actions of this Action.
        /// </summary>
        /// <param name="items">The ActionCollection object to set.</param>
        internal void SetChildActions(ActionCollection items)
        {
            m_ChildActions = items;
        }

        /// <summary>
        /// Sets the specified collection as child controls of this Action.
        /// </summary>
        /// <param name="items">The ActionCollection object to set.</param>
        internal void SetChildControls(ActionCollection items)
        {
            m_ChildControls = items;
        }

        /// <summary>
        /// Returns the value indicating that the access to the action is denied.
        /// </summary>
        /// <returns>true, if the access to the action is denied; otherwise, false.</returns>
        internal bool AccessDenied()
        {
            return (this.Handle
                ? (this.BuiltIn ? Handlers.ActionHandler.Instance.AccessDenied(this) : Handlers.ActionHandler.Current.AccessDenied(this))
                : false);
        }

        /// <summary>
        /// Gets a collection of the visible child actions, also taking into consideration the access rights.
        /// </summary>
        internal ActionCollection GetAvailableChildActions(bool userIsAuthenticated, bool userIsFrameworkAdmin, IList actionIdList)
        {
            ActionCollection coll = new ActionCollection();
            foreach (Action item in this.ChildActions)
            {
                if (item.Visible)
                {
                    if (!ActionProvider.ShowAction(item, userIsFrameworkAdmin, userIsAuthenticated, actionIdList))
                        continue;

                    if (item.AccessDenied())
                        continue;

                    coll.Add(item);
                }
            }
            return coll;
        }

        #endregion

        #region Overriden Methods

        public override bool Equals(object obj)
        {
            Action action = obj as Action;
            if ((object)action == null)
                return false;
            return (this.CompareTo(action) == 0);
        }

        public override int GetHashCode()
        {
            return this.ActionId.GetHashCode();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Copies the data for this Action.
        /// </summary>
        /// <returns>A new Action with the same data as this Action.</returns>>
        public Action Clone()
        {
            Action item = new Action();
            item.ActionId = this.ActionId;
            item.ParentAction = this.ParentAction;
            item.ActionType = this.ActionType;
            item.Name = this.Name;
            item.Description = this.Description;
            item.IconUrl = this.IconUrl;
            item.SubmenuItemImageUrl = this.SubmenuItemImageUrl;
            item.SubmenuItemType = this.SubmenuItemType;
            item.SubmenuItemHorizontalAlign = this.SubmenuItemHorizontalAlign;
            item.SubmenuItemWidth = this.SubmenuItemWidth;
            item.NavigateUrl = this.NavigateUrl;
            item.LearnMoreUrl = this.LearnMoreUrl;
            item.VideoUrl = this.VideoUrl;
            item.OrderNumber = this.OrderNumber;
            item.AuthenticationRequired = this.AuthenticationRequired;
            item.InstanceRequired = this.InstanceRequired;
            item.Visible = this.Visible;
            item.ShowInDetailMenu = this.ShowInDetailMenu;
            item.ShowChildrenInDetailMenu = this.ShowChildrenInDetailMenu;
            item.ShowDescriptionInDetailMenu = this.ShowDescriptionInDetailMenu;
            item.GroupInDetailMenu = this.GroupInDetailMenu;
            item.HighlightInDetailMenu = this.HighlightInDetailMenu;
            item.m_DetailMenuTheme = this.m_DetailMenuTheme;
            item.m_IconSize = this.m_IconSize;
            item.BuiltIn = this.BuiltIn;
            item.Handle = this.Handle;
            item.SetChildActions(this.ChildActions);
            item.SetChildControls(this.ChildControls);
            return item;
        }

        /// <summary>
        /// Copies the data for this Action.
        /// </summary>
        /// <returns>A new Action with the same data as this Action.</returns>>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Compares the current action with another.
        /// </summary>
        /// <param name="other">An action to compare with this action.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the actions being compared.</returns>
        public int CompareTo(Action other)
        {
            int result = 0;
            if ((object)other == null)
                result = 1;
            else
                result = this.ActionId.CompareTo(other.ActionId);
            return result;
        }

        #endregion
    }

    /// <summary>
    /// The collection of actions.
    /// </summary>
    [Serializable]
    public class ActionCollection : List<Action>
    {
        #region Private Methods

        private static int CompareByOrderNumberAndName(Action x, Action y)
        {
            int result = 0;

            if (x == null)
            {
                result = ((y == null) ? 0 : -1);
            }
            else
            {
                if (y == null)
                    result = 1;
                else
                {
                    result = (x.OrderNumber - y.OrderNumber);
                    if (result == 0) result += string.Compare(x.Name, y.Name, StringComparison.CurrentCultureIgnoreCase);
                }
            }
            return result;
        }

        private void ReadFromCommonDataSet(Action parent, DataRow[] actionRows)
        {
            foreach (CommonDataSet.ActionRow row in actionRows)
            {
                ActionType type = (ActionType)row.ActionTypeId;
                if (type == ActionType.Page || type == ActionType.Control)
                {
                    Action item = ActionProvider.CreateAction(row, parent);

                    DataRow[] childActionRows = row.GetActionRows();
                    if (childActionRows.Length > 0) ReadFromCommonDataSet(item, childActionRows);

                    if (type == ActionType.Page)
                    {
                        if (parent != null) parent.ChildActions.Add(item);
                        this.Add(item);
                    }
                    else if (type == ActionType.Control && parent != null)
                        parent.ChildControls.Add(item);
                }
            }

            if (parent != null)
            {
                if (parent.ChildActions.Count > 1) parent.ChildActions.Sort();
            }
            else if (this.Count > 1)
            {
                Sort(CompareByOrderNumberAndName);
            }
        }

        private Action GetParentAction(Action item)
        {
            if (item != null && (!item.IsMainMenuRoot))
                return GetParentAction(item.ParentAction);
            return item;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Finds the action by specified identifier.
        /// </summary>
        /// <param name="actionId">The identifier of the action.</param>
        /// <returns>The Micajah.Common.Bll.Action object if the Action is found; otherwise a null reference.</returns>
        public Action FindByActionId(Guid actionId)
        {
            lock (((ICollection)this).SyncRoot)
            {
                return this.Find(
                    delegate(Action action)
                    {
                        return action.ActionId == actionId;
                    });
            }
        }

        /// <summary>
        /// Finds the action by specified navigate URL.
        /// </summary>
        /// <param name="navigateUrl">The navigate URL of the action.</param>
        /// <returns>The Micajah.Common.Bll.Action object if the action is found; otherwise a null reference.</returns>
        public Action FindByNavigateUrl(string navigateUrl)
        {
            return FindByNavigateUrl(navigateUrl, false);
        }

        /// <summary>
        /// Finds the action by specified navigate URL.
        /// </summary>
        /// <param name="navigateUrl">The navigate URL of the action.</param>
        /// <param name="isAbsoluteNavigateUrl">The flag specifying that the navigate URL is an application absolute URL.</param>
        /// <returns>The Micajah.Common.Bll.Action object if the action is found; otherwise a null reference.</returns>
        public Action FindByNavigateUrl(string navigateUrl, bool isAbsoluteNavigateUrl)
        {
            lock (((ICollection)this).SyncRoot)
            {
                return this.Find(
                    delegate(Action action)
                    {
                        return (string.Compare((isAbsoluteNavigateUrl ? action.AbsoluteNavigateUrl : WebApplication.CreateApplicationRelativeUrl(action.NavigateUrl)), navigateUrl, StringComparison.OrdinalIgnoreCase) == 0);
                    });
            }
        }

        /// <summary>
        /// Finds the action by specified navigate URL.
        /// </summary>
        /// <param name="navigateUrl">The navigate URL of the action.</param>
        /// <param name="isAbsoluteNavigateUrl">The flag specifying that the navigate URL is an application absolute URL.</param>
        /// <param name="fullMatch">Whether the comparision mode is full match.</param>
        /// <returns>The Micajah.Common.Bll.Action object if the action is found; otherwise a null reference.</returns>
        public Action FindByNavigateUrlPathAndQuery(string navigateUrl, bool isAbsoluteNavigateUrl, bool fullMatch)
        {
            lock (((ICollection)this).SyncRoot)
            {
                return this.Find(
                    delegate(Action action)
                    {
                        if (!(string.IsNullOrEmpty(action.NavigateUrl) || string.IsNullOrEmpty(navigateUrl) || action.IsDetailMenuPage))
                        {
                            string[] p1 = (isAbsoluteNavigateUrl ? action.AbsoluteNavigateUrl : WebApplication.CreateApplicationRelativeUrl(action.NavigateUrl)).Split('?');
                            string[] p2 = navigateUrl.Split('?');

                            if (((string.Compare(p1[0], p2[0], StringComparison.OrdinalIgnoreCase) == 0)) && (p1.Length > 1) && (p2.Length > 1))
                            {
                                NameValueCollection c1 = HttpUtility.ParseQueryString(p1[1]);
                                NameValueCollection c2 = HttpUtility.ParseQueryString(p2[1]);
                                int equalsCount = 0;
                                int totalCount = 0;

                                foreach (string k1 in c1.AllKeys)
                                {
                                    if (c2[k1] != null)
                                    {
                                        totalCount++;
                                        if ((string.Compare(c1[k1], c2[k1], StringComparison.OrdinalIgnoreCase) == 0)
                                            || ((c1[k1] == string.Empty) && (c2[k1] != null)))
                                        {
                                            equalsCount++;
                                        }
                                    }
                                }

                                return ((((!fullMatch) && (totalCount > 0)) || (fullMatch && (totalCount == c1.AllKeys.Length))) && (equalsCount == totalCount));
                            }
                        }

                        return false;
                    });
            }
        }

        /// <summary>
        /// Retrieves all the actions for which the authentication is not required.
        /// </summary>
        /// <returns>The collection containing all the actions for which the authentication is not required.</returns>
        public ReadOnlyCollection<Action> FindAllPublic()
        {
            lock (((ICollection)this).SyncRoot)
            {
                return this.FindAll(
                    delegate(Action action)
                    {
                        return (!action.AuthenticationRequired);
                    }).AsReadOnly();
            }
        }

        public new void Sort()
        {
            lock (((ICollection)this).SyncRoot)
            {
                Sort(CompareByOrderNumberAndName);
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Finds the parent first level action which the specified action belong to.
        /// </summary>
        /// <param name="actionId">The identifier of the action.</param>
        /// <returns>The Action object that represents parent first level action or the current action if it is a first level action.</returns>
        internal Action FindMainMenuActionByActionId(Guid actionId)
        {
            lock (((ICollection)this).SyncRoot)
            {
                Action item = this.Find(
                    delegate(Action action)
                    {
                        return action.ActionId == actionId;
                    });
                return ((item == null) ? null : GetParentAction(item));
            }
        }

        /// <summary>
        /// Gets the visible sibling actions of specified action, also taking into consideration the access rights.
        /// </summary>
        /// <param name="item">The action to get sibling actions.</param>
        /// <returns>The collection of the sibling actions.</returns>
        internal ActionCollection GetAvailableSiblingActions(Action item)
        {
            ActionCollection items = new ActionCollection();
            if (item != null)
            {
                if (item.IsMainMenuRoot)
                {
                    lock (((ICollection)this).SyncRoot)
                    {
                        foreach (Action menuItem in this)
                        {
                            if (menuItem.IsMainMenuRoot && menuItem.Visible && (menuItem.ActionId != item.ActionId) && (!menuItem.AccessDenied()))
                                items.Add(menuItem);
                        }
                    }
                }
            }
            return items;
        }

        /// <summary>
        /// Loads the collection from CommonDataSet.
        /// </summary>
        internal void LoadFromCommonDataSet()
        {
            lock (((ICollection)this).SyncRoot)
            {
                ReadFromCommonDataSet(null, ActionProvider.GetRootActionRows());
            }
        }

        #endregion
    }
}
