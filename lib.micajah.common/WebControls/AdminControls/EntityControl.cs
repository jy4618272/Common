using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.AdminControls
{
    public class EntityControl : UserControl
    {
        #region Members

        protected Panel SearchPanel;
        protected ComboBox InstanceList;
        protected TreeView Tree;
        protected Label RestrictErrorLabel;
        protected Label DescriptionLabel;
        protected CommonGridView List;
        protected ObjectDataSource InstancesDataSource;

        private Guid? m_EntityId;
        private Entity m_Entity;
        private EntityNodeTypeCollection m_EntityCustomNodeTypes;
        private Dictionary<Guid, RadTreeViewContextMenu> m_ContextMenus;
        private Micajah.Common.Pages.MasterPage m_MasterPage;

        #endregion

        #region Private Properties

        private bool IsRefreshAllNodesPath
        {
            get
            {
                bool isRefresh;
                if (bool.TryParse(Request.QueryString["refresh"], out isRefresh))
                    return isRefresh;
                else
                    return false;
            }
        }

        private Micajah.Common.Pages.MasterPage MasterPage
        {
            get
            {
                if (m_MasterPage == null) m_MasterPage = Page.Master as Micajah.Common.Pages.MasterPage;
                return m_MasterPage;
            }
        }

        private Guid? SelectedInstanceId
        {
            get
            {
                Guid? instanceId = new Guid?();
                if (InstanceList.Items.Count == 0)
                    InstanceList.DataBind();
                object obj = Support.ConvertStringToType(InstanceList.SelectedValue, typeof(Guid));
                if (obj != null)
                    instanceId = (Guid)obj;
                return instanceId;
            }
        }

        private Guid EntityId
        {
            get
            {
                if (!m_EntityId.HasValue)
                {
                    object obj = Support.ConvertStringToType(this.Request.QueryString["entityid"], typeof(Guid));
                    m_EntityId = ((obj == null) ? Guid.Empty : (Guid)obj);
                }
                return m_EntityId.Value;
            }
        }

        private Entity Entity
        {
            get
            {
                if (m_Entity == null) m_Entity = WebApplication.Entities[this.EntityId.ToString()];
                return m_Entity;
            }
        }

        private EntityNodeTypeCollection EntityCustomNodeTypes
        {
            get
            {
                if (m_EntityCustomNodeTypes == null)
                {
                    if (this.Entity != null)
                    {
                        Guid? instanceId = null;
                        if (this.Entity.HierarchyStartLevel == EntityLevel.Instance)
                            instanceId = this.SelectedInstanceId;
                        m_EntityCustomNodeTypes = this.Entity.GetCustomNodeTypes(UserContext.SelectedOrganizationId, this.SelectedInstanceId);
                    }
                }
                return m_EntityCustomNodeTypes;
            }
        }

        private Dictionary<Guid, RadTreeViewContextMenu> ContextMenus
        {
            get
            {
                if (m_ContextMenus == null)
                {
                    m_ContextMenus = new Dictionary<Guid, RadTreeViewContextMenu>();

                    if (this.Entity != null)
                    {
                        EntityNodeTypeCollection merged = new EntityNodeTypeCollection();

                        foreach (EntityNodeType entityNodeType in this.Entity.NodeTypes)
                            merged.Add(entityNodeType);

                        if (this.Entity.EnableNodeTypesCustomization)
                            foreach (EntityNodeType entityNodeType in this.EntityCustomNodeTypes)
                                merged.Add(entityNodeType);

                        RadTreeViewContextMenu menu = new RadTreeViewContextMenu();
                        menu.ID = "Menu" + Guid.Empty.ToString("N");
                        if (merged.Count > 0)
                            foreach (EntityEvent entityEvent in this.Entity.Events)
                            {
                                RadMenuItem item = new RadMenuItem(entityEvent.Name + " " + merged[0].Name);
                                item.Value = String.Concat(entityEvent.Name, "_", merged[0].Id.ToString("N"));
                                menu.Items.Add(item);
                            }

                        foreach (EntityEvent entityEvent in Entity.CustomEvents)
                        {
                            RadMenuItem item = new RadMenuItem(entityEvent.Name);
                            item.Value = entityEvent.Url;
                            item.ImageUrl = entityEvent.ImageUrl;
                            menu.Items.Add(item);
                        }

                        m_ContextMenus.Add(Guid.Empty, menu);

                        foreach (EntityNodeType entityNodeType in merged)
                        {
                            menu = new RadTreeViewContextMenu();
                            menu.ID = "Menu" + entityNodeType.Id.ToString("N");
                            RadMenuItem item;
                            foreach (EntityEvent entityEvent in entityNodeType.Events)
                            {
                                if (entityEvent.Name.ToUpperInvariant() == "CREATE")
                                {

                                    item = new RadMenuItem(String.Concat(entityEvent.Name, " ", entityNodeType.Name));
                                    item.Value = String.Concat(entityEvent.Name, "_", entityNodeType.Id.ToString("N"));
                                    menu.Items.Add(item);

                                    if (merged.Count > merged.IndexOf(entityNodeType) + 1)
                                    {
                                        EntityNodeType child = merged[merged.IndexOf(entityNodeType) + 1];
                                        if (child != null)
                                        {
                                            item = new RadMenuItem(String.Concat(entityEvent.Name, " ", child.Name));
                                            item.Value = String.Concat(entityEvent.Name, "_", child.Id.ToString("N"));
                                            menu.Items.Add(item);
                                        }
                                    }
                                }
                                else
                                {
                                    item = new RadMenuItem(entityEvent.Name);
                                    item.Value = entityEvent.Name;
                                    menu.Items.Add(item);
                                }
                            }

                            foreach (EntityEvent entityEvent in Entity.CustomEvents)
                            {
                                item = new RadMenuItem(entityEvent.Name);
                                item.Value = entityEvent.Url;
                                item.ImageUrl = entityEvent.ImageUrl;
                                menu.Items.Add(item);
                            }

                            m_ContextMenus.Add(entityNodeType.Id, menu);
                        }
                    }
                    RadTreeViewContextMenu emptyMenu = new RadTreeViewContextMenu();
                    emptyMenu.ID = "EmptyMenu";

                    RadMenuItem menuItem = new RadMenuItem(Resources.EntityControl_AddNodeType);
                    menuItem.Value = Resources.EntityControl_AddNodeType;

                    emptyMenu.Items.Add(menuItem);
                    m_ContextMenus.Add(Guid.NewGuid(), emptyMenu);
                }
                return m_ContextMenus;
            }
        }

        #endregion

        #region Protected Properties

        protected static string DeleteButtonConfirmText
        {
            get { return Support.PreserveDoubleQuote(Resources.AutoGeneratedButtonsField_DeleteButton_ConfirmText); }
        }

        #endregion

        #region Private Methods

        private void RefreshTree()
        {
            this.RefreshTree(true);
        }

        private void RefreshTree(bool saveExpandedState)
        {
            Guid? instanceId = null;
            if (Entity.HierarchyStartLevel == EntityLevel.Instance)
                instanceId = this.SelectedInstanceId;

            Tree.DataSource = EntityNodeProvider.GetEntityNodesTree(UserContext.SelectedOrganizationId, instanceId, this.Entity.Id, this.Entity.Name);
            Tree.Rebind(saveExpandedState);

            if (Tree.Nodes.Count > 0)
                Tree.Nodes[0].Expanded = true;

            Tree.ContextMenus.Clear();
            Tree.ContextMenus.AddRange(this.ContextMenus.Values);
        }

        private SortedList GetDepths(SortedList depths, RadTreeNode rtn, string entityNodeTypeId)
        {
            if (rtn.ParentNode != null)
            {
                depths["Depth"] = Convert.ToInt32(depths["Depth"], CultureInfo.InvariantCulture) + 1;
                if (rtn.ParentNode.Category == entityNodeTypeId)
                    depths["Restrict"] = Convert.ToInt32(depths["Restrict"], CultureInfo.InvariantCulture) + 1;
                GetDepths(depths, rtn.ParentNode, entityNodeTypeId);
            }
            return depths;
        }

        private bool IsRestricted(RadTreeNode rtn, string entityNodeTypeId, int maxRestrict, int maxDepth)
        {
            SortedList depths = new SortedList();
            depths["Restrict"] = 1;
            depths["Depth"] = 1;
            SortedList countDepths = GetDepths(depths, rtn, entityNodeTypeId);
            int restrict = Convert.ToInt32(countDepths["Restrict"], CultureInfo.InvariantCulture);
            int depth = Convert.ToInt32(countDepths["Depth"], CultureInfo.InvariantCulture);
            if ((maxRestrict > 0 && restrict >= maxRestrict) || (maxDepth > 0 && depth >= maxDepth))
                return true;
            else
                return false;
        }

        private void CloneNode(RadTreeNode radTreeNode, RadTreeNode targetNode)
        {
            foreach (RadTreeNode sourceNode in radTreeNode.Nodes)
            {
                Guid sourceId = new Guid(sourceNode.Value);
                OrganizationDataSet.EntityNodeRow source = UserContext.Current.SelectedOrganization.DataSet.EntityNode.FindByEntityNodeId(sourceId);

                RadTreeNode rtn = new RadTreeNode();
                rtn.Text = sourceNode.Text;
                rtn.Value = EntityNodeProvider.InsertEntityNode(UserContext.SelectedOrganizationId, this.SelectedInstanceId, source.Name, source.EntityNodeTypeId, source.EntityId, new Guid(targetNode.Value), this.Entity.HierarchyStartLevel).ToString();
                rtn.Category = sourceNode.Category;
                rtn.ContextMenuID = sourceNode.ContextMenuID;
                targetNode.Nodes.Add(rtn);
                Bll.Providers.EntityNodeProvider.UpdateEntityNodePath(new Guid(rtn.Value), rtn.GetFullPath(" > "));

                if (sourceNode.Nodes.Count > 0)
                    CloneNode(sourceNode, rtn);
            }
        }

        private void UpdateAllNodesPath(RadTreeNode rtn)
        {
            Guid id = new Guid(rtn.Value);
            if (id != Guid.Empty)
                Bll.Providers.EntityNodeProvider.UpdateEntityNodePath(id, rtn.GetFullPath(" > "));
            foreach (RadTreeNode childNode in rtn.Nodes)
                UpdateAllNodesPath(childNode);
        }

        private void StartNodeInEditMode(string nodeValue)
        {
            //find the node by its Value and edit it when page loads            
            StringBuilder sb = new StringBuilder();
            sb.Append("Sys.Application.add_load(editNode); function editNode(){ var tree = $find(\"");
            sb.Append(Tree.ClientID);
            sb.Append("\"); var node = tree.findNodeByValue('");
            sb.Append(nodeValue);
            sb.Append("'); node.Text = ''; if (node) node.startEdit();");
            sb.Append("Sys.Application.remove_load(editNode);};");

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "nodeEdit" + nodeValue, sb.ToString(), true);
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (FrameworkConfiguration.Current.WebApplication.EnableMultipleInstances)
                {
                    if (this.Entity != null)
                        SearchPanel.Visible = (this.Entity.HierarchyStartLevel == EntityLevel.Instance);
                }
                else
                    SearchPanel.Visible = false;

                InstancesDataSource.FilterExpression = (SearchPanel.Visible ? InstanceProvider.InstancesFilterExpression : "InstanceId IS NULL");

                List.Columns[0].HeaderText = Resources.NodeTypeControl_List_NameColumn_HeaderText;
                List.Columns[1].HeaderText = Resources.NodeTypeControl_List_OrderNumberColumn_HeaderText;
                DescriptionLabel.Text = Resources.EntityControl_DescriptionLabel_Text;

                if (this.Entity != null)
                {
                    if (UserContext.Current != null)
                    {
                        this.RefreshTree();
                        this.MasterPage.CustomName = Entity.Name;

                        if (IsRefreshAllNodesPath && Tree.Nodes.Count > 0)
                            UpdateAllNodesPath(Tree.Nodes[0]);
                    }
                }
            }
        }

        protected void ListDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e == null) return;

            e.InputParameters["organizationId"] = UserContext.SelectedOrganizationId;
            Guid? instanceId = null;
            if (Entity.HierarchyStartLevel == EntityLevel.Instance)
                instanceId = this.SelectedInstanceId;
            e.InputParameters["instanceId"] = instanceId;
            e.InputParameters["entityId"] = EntityId;
        }

        protected void List_Action(object sender, CommonGridViewActionEventArgs e)
        {
            if (e == null) return;
            if (!string.IsNullOrEmpty(Tree.SelectedValue) && List.SelectedValue != null)
                Bll.Providers.EntityNodeProvider.UpdateEntityType(new Guid(Tree.SelectedValue), new Guid(List.SelectedValue.ToString()));
            List.Visible = false;
            List.SelectedIndex = -1;
            m_ContextMenus = null;
            RefreshTree();
        }

        protected void Tree_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            if (e == null) return;

            object obj = DataBinder.Eval(e.Node.DataItem, "EntityNodeTypeId");
            if (!Support.IsNullOrDBNull(obj))
            {
                e.Node.Category = ((Guid)obj).ToString("N");
                if (this.EntityCustomNodeTypes[((Guid)obj).ToString("N")] == null)
                    e.Node.ContextMenuID = "EmptyMenu";
                else
                    e.Node.ContextMenuID = "Menu" + e.Node.Category;
            }
            else
                e.Node.ContextMenuID = "Menu" + Guid.Empty.ToString("N");
        }

        protected void Tree_NodeEdit(object sender, RadTreeNodeEditEventArgs e)
        {
            if (e == null) return;

            if (!string.IsNullOrEmpty(e.Text))
            {
                object obj = Support.ConvertStringToType(e.Node.Value, typeof(Guid));
                e.Node.Text = e.Text;
                EntityNodeProvider.UpdateEntityName(((obj == null) ? Guid.Empty : (Guid)obj), e.Text);
                Bll.Providers.EntityNodeProvider.UpdateEntityNodePath(((obj == null) ? Guid.Empty : (Guid)obj), e.Node.GetFullPath(" > "));
            }
        }

        protected void Tree_NodeDrop(object sender, RadTreeNodeDragDropEventArgs e)
        {
            if (e == null) return;

            if (e.SourceDragNode == null || e.DestDragNode == null) return;
            bool IsMerge = e.SourceDragNode.Level == e.DestDragNode.Level;
            if (e.SourceDragNode.Level - 1 != e.DestDragNode.Level && !IsMerge) return;
            if (e.SourceDragNode.Level < 1) return;

            object obj = Support.ConvertStringToType(e.SourceDragNode.Value, typeof(Guid));
            Guid sourceId = ((obj == null) ? Guid.Empty : (Guid)obj);
            obj = Support.ConvertStringToType(e.DestDragNode.Value, typeof(Guid));
            Guid destId = ((obj == null) ? Guid.Empty : (Guid)obj);
            RadTreeNode destNode = Tree.FindNodeByValue(e.DestDragNode.Value);
            RadTreeNode sourceNode = Tree.FindNodeByValue(e.SourceDragNode.Value);

            if (IsMerge)
            {
                foreach (RadTreeNode rtn in sourceNode.Nodes)
                    EntityNodeProvider.ChangeParentEntityNode(new Guid(rtn.Value), destId);
                EntityNodeProvider.MergeEntityNode(sourceId, destId);
                RefreshTree();
                destNode = Tree.FindNodeByValue(destId.ToString());
                UpdateAllNodesPath(destNode);
            }
            else
            {
                bool isCopy = false;
                if (!String.IsNullOrEmpty(Request.Params["CtrlKeyField"]))
                    isCopy = Convert.ToBoolean(Request.Params["CtrlKeyField"], CultureInfo.InvariantCulture);

                if (isCopy)
                {
                    EntityNodeProvider.CopyEntityNode(UserContext.SelectedOrganizationId, this.SelectedInstanceId, sourceId, destId, this.Entity.HierarchyStartLevel);
                    RadTreeNode rtn = new RadTreeNode();
                    rtn.Text = sourceNode.Text;
                    rtn.Value = sourceNode.Value;
                    rtn.Category = sourceNode.Category;
                    rtn.ContextMenuID = sourceNode.ContextMenuID;
                    destNode.Nodes.Add(rtn);
                    Bll.Providers.EntityNodeProvider.UpdateEntityNodePath(new Guid(rtn.Value), rtn.GetFullPath(" > "));
                }
                else
                {
                    EntityNodeProvider.ChangeParentEntityNode(sourceId, destId);
                    destNode.Nodes.Add(sourceNode);
                    Bll.Providers.EntityNodeProvider.UpdateEntityNodePath(new Guid(sourceNode.Value), sourceNode.GetFullPath(" > "));
                }
            }
        }

        protected void Tree_ContextMenuItemClick(object sender, RadTreeViewContextMenuEventArgs e)
        {
            if (e == null) return;

            List.Visible = false;
            if (e.MenuItem.Value.StartsWith("CREATE", StringComparison.OrdinalIgnoreCase))
            {
                string[] menuItemValues = e.MenuItem.Value.Split('_');
                bool maxRestricted = false;
                if (new Guid(e.Node.Value) != Guid.Empty)
                {
                    EntityNodeType ent = Entity.NodeTypes[menuItemValues[1]];
                    if (ent != null && ent.MaxRestrict > 0)
                        maxRestricted = IsRestricted(e.Node, menuItemValues[1], ent.MaxRestrict, this.Entity.HierarchyMaxDepth);
                }

                if (!maxRestricted)
                {
                    Guid entityId = Bll.Providers.EntityNodeProvider.InsertEntityNode(UserContext.SelectedOrganizationId, this.SelectedInstanceId, "new", new Guid(menuItemValues[1]), this.EntityId, new Guid(e.Node.Value), this.Entity.HierarchyStartLevel);
                    RadTreeNode rtn = new RadTreeNode();
                    rtn.Text = "new";
                    rtn.Value = entityId.ToString("N");
                    rtn.Category = menuItemValues[1];
                    rtn.ContextMenuID = "Menu" + menuItemValues[1];
                    e.Node.Nodes.Add(rtn);
                    e.Node.Expanded = true;
                    Bll.Providers.EntityNodeProvider.UpdateEntityNodePath(entityId, rtn.GetFullPath(" > "));
                    StartNodeInEditMode(rtn.Value);
                }
                else
                {
                    RestrictErrorLabel.Text = Resources.EntityControl_RestrictErrorLabel_Text;
                    RestrictErrorLabel.Visible = true;
                }
            }
            else if (e.MenuItem.Value.ToUpperInvariant() == "DELETE")
            {
                EntityNodeProvider.DeleteEntityNode(new Guid(e.Node.Value));
                Tree.FindNodeByValue(e.Node.Value).Remove();
            }
            else if (e.MenuItem.Value.ToUpperInvariant() == "CLONE")
            {
                Guid sourceId = new Guid(e.Node.Value);
                OrganizationDataSet.EntityNodeRow source = UserContext.Current.SelectedOrganization.DataSet.EntityNode.FindByEntityNodeId(sourceId);

                if (source.IsParentEntityNodeIdNull())
                    source.ParentEntityNodeId = Guid.Empty;

                RadTreeNode rtn = new RadTreeNode();
                rtn.Text = e.Node.Text + "_Copy" + (e.Node.ParentNode.Nodes.Count + 1).ToString(CultureInfo.InvariantCulture);
                rtn.Value = EntityNodeProvider.InsertEntityNode(UserContext.SelectedOrganizationId, this.SelectedInstanceId, rtn.Text, source.EntityNodeTypeId, source.EntityId, source.ParentEntityNodeId, this.Entity.HierarchyStartLevel).ToString();
                rtn.Category = e.Node.Category;
                rtn.ContextMenuID = e.Node.ContextMenuID;
                e.Node.ParentNode.Nodes.Add(rtn);
                Bll.Providers.EntityNodeProvider.UpdateEntityNodePath(new Guid(rtn.Value), rtn.GetFullPath(" > "));
                CloneNode(e.Node, rtn);
            }

            else if (e.MenuItem.Value == Resources.EntityControl_AddNodeType || e.MenuItem.Value == Resources.EntityControl_EditNodeType)
            {
                List.DataBind();
                List.Visible = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(e.MenuItem.Value))
                    Response.Redirect(string.Format(CultureInfo.InvariantCulture, e.MenuItem.Value, e.Node.Value));
            }

        }

        protected void InstancesDataSource_Selecting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e != null)
                e.InputParameters["organizationId"] = UserContext.SelectedOrganizationId;
        }

        protected void InstanceList_DataBound(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                if (comboBox.Items.Count == 1)
                    SearchPanel.Visible = InstanceList.Visible = false;
            }
        }

        protected void InstanceList_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            this.RefreshTree(false);
        }

        #endregion
    }
}
