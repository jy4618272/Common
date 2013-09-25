using System;
using System.Globalization;
using System.Web.UI;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Properties;
using Micajah.Common.Security;

namespace Micajah.Common.WebControls.AdminControls
{
    public class EntitiesControl : UserControl
    {
        #region Members

        protected DetailMenu EntitiesMenu;

        #endregion

        #region Public Properties

        public EntityType DisplayedEntityType
        {
            get
            {
                object obj = ViewState["DisplayedEntityType"];
                return ((obj == null) ? EntityType.Default : (EntityType)obj);
            }
            set { ViewState["DisplayedEntityType"] = value; }
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ActionCollection actions = new ActionCollection();
                Micajah.Common.Bll.Action organizationPageAction = null;
                Micajah.Common.Bll.Action action = null;
                string descriptionFormat = null;
                Micajah.Common.Bll.Action organizationNodeTypePageAction = ActionProvider.FindAction(ActionProvider.NodeTypePageActionId);

                if (this.DisplayedEntityType == EntityType.Hierarchical)
                {
                    organizationPageAction = ActionProvider.FindAction(ActionProvider.TreePageActionId);
                    descriptionFormat = Resources.EntitiesControl_ActionDescriptionFormat;
                }
                else
                {
                    organizationPageAction = ActionProvider.FindAction(ActionProvider.EntityFieldsPageActionId);
                    descriptionFormat = Resources.EntitiesControl_EntityFieldsActionDescriptionFormat;
                }

                foreach (Entity entity in EntityFieldProvider.Entities)
                {
                    if (this.DisplayedEntityType == EntityType.Hierarchical)
                    {
                        if (entity.EnableHierarchy)
                        {
                            UserContext user = UserContext.Current;
                            if (user != null)
                            {
                                switch (entity.HierarchyStartLevel)
                                {
                                    case EntityLevel.Instance:
                                        if (!user.IsInstanceAdministrator())
                                            continue;
                                        break;
                                    case EntityLevel.Organization:
                                        if (!user.IsOrganizationAdministrator)
                                            continue;
                                        break;
                                }
                            }
                        }
                        else
                            continue;
                    }
                    else if (entity.EnableHierarchy)
                        continue;

                    action = organizationPageAction.Clone();
                    action.Name = entity.Name;
                    action.Description = string.Format(CultureInfo.CurrentCulture, descriptionFormat, entity.Name);
                    if (string.IsNullOrEmpty(entity.CustomNavigateUrl))
                        action.NavigateUrl = action.NavigateUrl + "?EntityId=" + entity.Id.ToString("N");
                    else
                        action.NavigateUrl = entity.CustomNavigateUrl;
                    actions.Add(action);

                    if (entity.EnableHierarchy && string.IsNullOrEmpty(entity.CustomNavigateUrl))
                    {
                        action = organizationNodeTypePageAction.Clone();
                        action.Name = entity.Name + Resources.EntitiesControl_NodeTypes;
                        action.Description = string.Format(CultureInfo.CurrentCulture, Resources.NodeTypesControl_ActionDescriptionFormat, action.Name);
                        action.NavigateUrl = action.NavigateUrl + "?EntityId=" + entity.Id.ToString("N");
                        actions.Add(action);
                    }
                }

                EntitiesMenu.DataSource = actions;
            }
        }

        #endregion
    }
}
