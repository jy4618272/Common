using Micajah.Common.Bll.Providers;
using Micajah.Common.Security;
using System;
using System.Collections;

namespace Micajah.Common.Bll
{
    /// <summary>
    /// The collection of bread crumbs.
    /// </summary>
    [Serializable]
    public sealed class BreadcrumbCollection : ActionCollection
    {
        #region Private Methods

        /// <summary>
        /// Adds the Home page action.
        /// </summary>
        private void AddHomePageAction()
        {
            if (this.Count == 0) return;

            Action homeItem = null;
            Action firstItem = this[0];
            UserContext user = UserContext.Current;
            if (user != null)
                homeItem = ActionProvider.FindAction(user.StartPageUrl);
            else
            {
                homeItem = ActionProvider.FindAction(CustomUrlProvider.CreateApplicationAbsoluteUrl("~/default.aspx"));
                if (homeItem != null)
                {
                    if (homeItem.AuthenticationRequired) homeItem = null;
                }
            }

            if (Micajah.Common.Configuration.FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Pages.MasterPageTheme.Modern)
            {
                if (homeItem != null)
                {
                    if ((homeItem != null) && (firstItem.ActionId == homeItem.ActionId))
                        base.Remove(firstItem);
                }
            }
            else
            {
                if ((homeItem != null) && (firstItem.ActionId != homeItem.ActionId))
                    base.Insert(0, homeItem.Clone());
            }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Adds specified item to the collection.
        /// </summary>
        /// <param name="value">The item to add.</param>
        public new void Add(Action value)
        {
            if (value == null) return;
            if (value.GroupInDetailMenu) return;

            bool add = true;
            bool generate = false;
            int count = this.Count;

            if (count > 0)
            {
                Action lastItem = this[count - 1];

                // The item with specified identifier is exists.
                if (FindByActionId(value.ActionId) != null)
                {
                    // The item with specified navigate URL is exists.
                    Action item = ActionProvider.FindAction(value.AbsoluteNavigateUrl);
                    if (item != null)
                    {
                        // The found item is not last item.
                        if (item != lastItem)
                        {
                            int deletedCount = (count - 1 - IndexOf(item));
                            int idx = 0;
                            while (true)
                            {
                                RemoveAt(count - 1);
                                count--;
                                idx++;
                                if (idx >= deletedCount) break;
                            }
                        }

                        return;
                    }
                }

                add = (lastItem.ActionId == value.ParentActionId);
                if (!add)
                {
                    if (value.AlternativeParentActions.FindByActionId(lastItem.ActionId) != null)
                    {
                        add = true;
                        value.ParentAction = lastItem;
                    }
                    else
                    {
                        // The specified item and last item have the same parent.
                        if (value.ParentAction == lastItem.ParentAction)
                        {
                            Action item = lastItem;
                            item.IsCustom = true;
                            while (item.IsCustom && (count > 0))
                            {
                                RemoveAt(count - 1);
                                count--;
                                if (count > 0) item = this[count - 1];
                            }
                            add = true;
                        }
                        else generate = true;
                    }
                }
            }
            else generate = ((value.ParentActionId.GetValueOrDefault(Guid.Empty) != Guid.Empty) && value.ParentActionId != ActionProvider.PagesAndControlsActionId);

            if (generate) // Generates the bread crumbs.
                this.Generate(value, false);
            else if (add) // Simple adding.
                base.Add(value);

            this.AddHomePageAction();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the specified item to the end of the collection.
        /// </summary>
        /// <param name="value">The item to add.</param>
        /// <param name="validate">The flag specifying that the item will be validated before adding.</param>
        public void Add(Action value, bool validate)
        {
            if (value == null) return;

            if (validate)
                this.Add(value);
            else
                base.Add(value);
        }

        /// <summary>
        /// Creates new item with specified details and adds to collection.
        /// </summary>
        /// <param name="name">The item name.</param>
        /// <param name="navigateUrl">The navigate URL of the item.</param>
        public void Add(string name, string navigateUrl)
        {
            this.Add(name, navigateUrl, string.Empty, true);
        }

        /// <summary>
        /// Creates new item with specified details and adds to collection.
        /// </summary>
        /// <param name="name">The item name.</param>
        /// <param name="navigateUrl">The navigate URL of the item.</param>
        /// <param name="validate">The flag specifying that the item will be validated before adding.</param>
        public void Add(string name, string navigateUrl, bool validate)
        {
            this.Add(name, navigateUrl, string.Empty, validate);
        }

        /// <summary>
        /// Creates new item with specified details and adds to collection.
        /// </summary>
        /// <param name="name">The item name.</param>
        /// <param name="navigateUrl">The navigate URL of the item.</param>
        /// <param name="description">The description of the item.</param>
        public void Add(string name, string navigateUrl, string description)
        {
            this.Add(name, navigateUrl, description, true);
        }

        /// <summary>
        /// Creates new item with specified details and adds to collection.
        /// </summary>
        /// <param name="name">The item name.</param>
        /// <param name="navigateUrl">The navigate URL of the item.</param>
        /// <param name="description">The description of the item.</param>
        /// <param name="validate">The flag specifying that the item will be validated before adding.</param>
        public void Add(string name, string navigateUrl, string description, bool validate)
        {
            this.Add(Guid.Empty, name, navigateUrl, description, validate);
        }

        /// <summary>
        /// Creates new item with specified details and adds to collection.
        /// </summary>
        /// <param name="actionId">The unique identifier of the item.</param>
        /// <param name="name">The item name.</param>
        /// <param name="navigateUrl">The navigate URL of the item.</param>
        /// <param name="description">The description of the item.</param>
        /// <param name="validate">The flag specifying that the item will be validated before adding.</param>
        public void Add(Guid actionId, string name, string navigateUrl, string description, bool validate)
        {
            int count = this.Count;
            Action lastItem = null;

            if (count > 0) lastItem = this[count - 1];

            Action item = new Action();
            item.Name = name;
            item.NavigateUrl = navigateUrl;
            item.Description = description;
            item.ActionId = ((actionId == Guid.Empty) ? Guid.NewGuid() : actionId);
            item.ParentAction = lastItem;

            this.Add(item, validate);
        }

        /// <summary>
        /// Generates the bread crumbs.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="addHomepageAction">A value indicating the homepage's action will be added to the collection.</param>
        public void Generate(Action action, bool addHomepageAction)
        {
            this.Clear();

            UserContext user = UserContext.Current;
            IList actionIdList = null;
            bool isFrameworkAdmin = false;
            bool isAuthenticated = false;
            if (user != null)
            {
                actionIdList = user.ActionIdList;
                isAuthenticated = true;
                isFrameworkAdmin = (user.IsFrameworkAdministrator && (user.SelectedOrganizationId == Guid.Empty));
            }

            Action item = action;
            while (item != null && item.ActionId != ActionProvider.PagesAndControlsActionId && item.ActionId != ActionProvider.GlobalNavigationLinksActionId)
            {
                if ((!item.GroupInDetailMenu) && (ActionProvider.ShowAction(item, actionIdList, isFrameworkAdmin, isAuthenticated)))
                    Insert(0, item);
                item = item.ParentAction;
            }

            if (addHomepageAction)
                this.AddHomePageAction();
        }

        /// <summary>
        /// Removes the last element of the collection.
        /// </summary>
        public void RemoveLast()
        {
            int count = this.Count;
            if (count > 0) this.RemoveAt(this.Count - 1);
        }

        #endregion
    }
}
