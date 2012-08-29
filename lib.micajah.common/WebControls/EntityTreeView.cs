using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Dal;
using Micajah.Common.Properties;
using Micajah.Common.Security;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls
{
    [ToolboxData("<{0}:EntityTreeView runat=server></{0}:EntityTreeView>")]
    public class EntityTreeView : TreeView
    {
        #region Private Properties

        RadFormDecorator radFormDecorator;
        private Dictionary<Guid, RadTreeViewContextMenu> m_ContextMenus;
        private string customRootNodeText;
        private bool? allowRootNodeSelection = new bool?();
        private Entity m_Entity;
        private Entity Entity
        {
            get
            {
                if (m_Entity == null) m_Entity = WebApplication.Entities[EntityId.ToString()];
                return m_Entity;
            }
        }

        private Dictionary<Guid, RadTreeViewContextMenu> CustomContextMenus
        {
            get
            {
                if (m_ContextMenus == null)
                {
                    m_ContextMenus = new Dictionary<Guid, RadTreeViewContextMenu>();
                    RadTreeViewContextMenu mainContextMenu = new RadTreeViewContextMenu();

                    RadMenuItem menuItem = new RadMenuItem(Resources.EntityTreeView_Select);
                    menuItem.Value = Resources.EntityTreeView_Select;
                    mainContextMenu.Items.Add(menuItem);

                    menuItem = new RadMenuItem(Resources.EntityTreeView_Unselect);
                    menuItem.Value = Resources.EntityTreeView_Unselect;
                    mainContextMenu.Items.Add(menuItem);

                    menuItem = new RadMenuItem(Resources.EntityTreeView_SelectWithAllChildNodes);
                    menuItem.Value = Resources.EntityTreeView_SelectWithAllChildNodes;
                    mainContextMenu.Items.Add(menuItem);

                    menuItem = new RadMenuItem(Resources.EntityTreeView_Block);
                    menuItem.Value = Resources.EntityTreeView_Block;
                    mainContextMenu.Items.Add(menuItem);

                    m_ContextMenus.Add(new Guid("f3c305ee-308b-44bf-b6e2-90d22a9ce878"), mainContextMenu);
                }
                return m_ContextMenus;
            }
        }

        #endregion

        #region Public Properties

        public string CustomRootNodeText
        {
            get
            {
                return customRootNodeText;
            }
            set
            {
                customRootNodeText = value;
            }
        }

        public bool? AllowRootNodeSelection
        {
            get
            {
                return allowRootNodeSelection;
            }
            set
            {
                allowRootNodeSelection = value;
            }
        }

        public new bool TriStateCheckBoxes
        {
            get
            {
                return base.CheckBoxes;
            }
        }

        public new bool CheckBoxes
        {
            get
            {
                return base.CheckBoxes;
            }
            set
            {
                base.CheckBoxes = value;
                base.TriStateCheckBoxes = false;
                base.CheckChildNodes = false;
            }
        }

        [Browsable(true)]
        public Guid EntityId
        {
            get
            {
                object obj = ViewState["EntityId"];
                return (obj == null) ? Guid.Empty : (Guid)obj;
            }
            set
            {
                ViewState["EntityId"] = value;
            }
        }

        [Browsable(true)]
        public Guid EntityNodeId
        {
            get
            {
                object obj = ViewState["NodeId"];
                return (obj == null) ? Guid.Empty : (Guid)obj;
            }
            set
            {
                ViewState["NodeId"] = value;
            }
        }

        public new string DataTextField
        {
            get
            {
                return base.DataTextField;
            }
        }

        public new string DataValueField
        {
            get
            {
                return base.DataValueField;
            }
        }

        #endregion

        #region Events

        public event EventHandler OnLoaded;
        public event EventHandler OnSaved;

        #endregion

        #region Protected Methods

        protected void DisableChildNodes(RadTreeNode node)
        {
            if (node == null) return;

            foreach (RadTreeNode rtNode in node.Nodes)
            {
                rtNode.Enabled = false;
                DisableChildNodes(rtNode);
            }
        }

        protected void SetStateNode(RadTreeNode node)
        {
            if (node == null) return;

            if (!node.Enabled)
            {
                node.ImageUrl = ResourceProvider.GetImageUrl(typeof(EntityTreeView), "spacer.png", true);
                node.Category = string.Empty;
                node.Checkable = true;
                node.Checked = false;
            }
            else if (string.IsNullOrEmpty(node.Category) && !node.Checked)
            {
                node.ImageUrl = ResourceProvider.GetImageUrl(typeof(EntityTreeView), "unchecked.png", true);
            }
            else if (string.IsNullOrEmpty(node.Category) && node.Checked)
            {
                node.ImageUrl = ResourceProvider.GetImageUrl(typeof(EntityTreeView), "checked.png", true);
            }
            else if (node.Category == "1")
            {
                node.ImageUrl = ResourceProvider.GetIconImageUrl("add.png", IconSize.Smaller, true);
                node.CheckChildNodes();

            }
            else if (node.Category == "2")
            {
                node.ImageUrl = ResourceProvider.GetIconImageUrl("cancel.png", IconSize.Smaller, true);
                DisableChildNodes(node);
            }

            node.Checkable = false;
        }

        protected void ProcessNodes(RadTreeNode node)
        {
            if (node == null) return;

            foreach (RadTreeNode rtNode in node.Nodes)
            {
                SetStateNode(rtNode);
                ProcessNodes(rtNode);
            }
        }

        #endregion

        #region Overriden Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        /// <summary>
        /// Raises the PreRender event and registers client scripts.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            radFormDecorator = new RadFormDecorator();
            radFormDecorator.DecoratedControls = FormDecoratorDecoratedControls.CheckBoxes;
            Controls.Add(radFormDecorator);

            string script = @"

               function EnableAllChildren(nodes)
               {
                   var i;
                   for (i=0; i<nodes.get_count(); i++)
                   {
                       nodes.getNode(i).set_enabled(true);
                       nodes.getNode(i).set_checkable(true);
                       nodes.getNode(i).set_checked(false);
                       nodes.getNode(i).set_checkable(false);
                       nodes.getNode(i).set_imageUrl('" + ResourceProvider.GetImageUrl(typeof(EntityTreeView), "unchecked.png", true) + @"');

                       if (nodes.getNode(i).get_nodes().get_count()> 0)
                       {
                           EnableAllChildren(nodes.getNode(i).get_nodes());
                       }
                   }
               }

               function UpdateAllChildren(nodes, checked)
               {
                   var i;
                   for (i=0; i<nodes.get_count(); i++)
                   {
                       nodes.getNode(i).set_category('');
                       if (checked)
                       {
                           nodes.getNode(i).set_enabled(true);
                           nodes.getNode(i).set_checkable(true);
                           nodes.getNode(i).set_checked(true);
                           nodes.getNode(i).set_checkable(false);
                           nodes.getNode(i).set_imageUrl('" + ResourceProvider.GetImageUrl(typeof(EntityTreeView), "checked.png", true) + @"');
                       }
                       else
                       {                           
                           nodes.getNode(i).set_imageUrl('" + ResourceProvider.GetImageUrl(typeof(EntityTreeView), "spacer.png", true) + @"');    
                           nodes.getNode(i).set_checkable(true);
                           nodes.getNode(i).set_checked(false);
                           nodes.getNode(i).set_checkable(false);
                           nodes.getNode(i).set_enabled(false);
                       }
                       
                       if (nodes.getNode(i).get_nodes().get_count()> 0)
                       {
                           UpdateAllChildren(nodes.getNode(i).get_nodes(), checked);
                       }
                   }
               }

             function onClientNodeClicked(sender, eventArgs) 
             {
               var tree = $find('" + this.ClientID + @"');
               tree.trackChanges();
               var node = eventArgs.get_node();
               var childNodes = eventArgs.get_node().get_nodes();
               if (node.get_enabled())
               {
                   if ((node.get_category() == '' || node.get_category() == null) && !node.get_checked())
                   {                  
                      node.set_checkable(true);
                      node.set_checked(true);
                      node.set_checkable(false);
                      node.set_category(''); 
                      node.set_imageUrl('" + ResourceProvider.GetImageUrl(typeof(EntityTreeView), "checked.png", true) + @"');                                   
                   }
                   else if ((node.get_category() == '' || node.get_category() == null) && node.get_checked())
                   {
                      node.set_imageUrl('" + ResourceProvider.GetIconImageUrl("add.png", IconSize.Smaller, true) + @"');
                      node.set_category('1');
                      node.set_checkable(true);
                      node.set_checked(true);
                      node.set_checkable(false);
                      UpdateAllChildren(childNodes, true);
                   }
                   else if (node.get_category() == '1')
                   {
                      node.set_checkable(true);
                      node.set_checked(true);
                      node.set_checkable(false);
                      UpdateAllChildren(childNodes, false);
                      node.set_category('2');
                      node.set_imageUrl('" + ResourceProvider.GetIconImageUrl("cancel.png", IconSize.Smaller, true) + @"');              
                   }
                   else if (node.get_category() == '2')
                   {                 
                      node.set_checkable(true);
                      node.set_checked(false);
                      node.set_checkable(false);
                      node.set_category('');        
                      node.set_imageUrl('" + ResourceProvider.GetImageUrl(typeof(EntityTreeView), "unchecked.png", true) + @"');
                      EnableAllChildren(childNodes);
                   }
               }
               tree.commitChanges();
            }";

            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "EntityTreeView_onClientNodeClicked", script, true);
        }

        /// <summary>
        /// Renders control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            radFormDecorator.RenderControl(writer);
            base.Render(writer);
        }

        #endregion

        #region Public Methods

        public void LoadTree()
        {
            if (EntityId != Guid.Empty && UserContext.Current != null)
            {
                Entity entity = WebApplication.Entities[EntityId.ToString()];
                Guid? instanceId = new Guid?();
                if (entity.HierarchyStartLevel == EntityLevel.Instance)
                    instanceId = new Guid?(UserContext.Current.SelectedInstance.InstanceId);
                OrganizationDataSet.EntityNodeDataTable dt = new OrganizationDataSet.EntityNodeDataTable();
                if (UserContext.Current != null)
                {
                    this.OnClientNodeClicked = "onClientNodeClicked";
                    //                    this.ContextMenus.AddRange(CustomContextMenus.Values);
                    base.DataFieldID = dt.EntityNodeIdColumn.ColumnName;
                    base.DataFieldParentID = dt.ParentEntityNodeIdColumn.ColumnName;
                    base.DataTextField = dt.NameColumn.ColumnName;
                    base.DataValueField = dt.EntityNodeIdColumn.ColumnName;
                    this.DataSource = EntityNodeProvider.GetEntityNodesTree(UserContext.Current.SelectedOrganization.OrganizationId, instanceId, EntityId, entity.Name);
                    this.DataBind();
                    if (this.Nodes.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(CustomRootNodeText))
                            this.Nodes[0].Text = CustomRootNodeText;

                        if ((AllowRootNodeSelection.HasValue && !AllowRootNodeSelection.Value) || (!AllowRootNodeSelection.HasValue && !Entity.EnableRootNodeSelection))
                        {
                            this.Nodes[0].Checkable = false;
                            this.Nodes[0].Category = "3";
                        }
                        this.Nodes[0].Expanded = true;
                    }
                }
            }

            OrganizationDataSet.EntityNodesRelatedEntityNodesDataTable t = EntityNodesRelatedEntityNodesProvider.GetAllEntityNodesRelatedEntityNodes(UserContext.Current.SelectedOrganization.OrganizationId, EntityNodeId, EntityId);
            RadTreeNode rtn;
            foreach (OrganizationDataSet.EntityNodesRelatedEntityNodesRow row in t.Rows)
            {
                rtn = this.FindNodeByValue(row.RelatedEntityNodeId.ToString());
                if (rtn == null)
                {
                    row.Delete();
                    continue;
                }
                if (rtn.Category != "3")
                {
                    if (row.RelationType == (int)RelationType.Checked)
                    {
                        rtn.Checked = true;
                    }
                    else if (row.RelationType == (int)RelationType.CheckedAndAllChildren)
                    {
                        rtn.Checked = true;
                        rtn.Category = "1";
                    }
                    else if (row.RelationType == (int)RelationType.Blocked)
                    {
                        rtn.Checked = true;
                        rtn.Category = "2";
                    }
                }
            }

            SetStateNode(this.Nodes[0]);
            ProcessNodes(this.Nodes[0]);

            if (OnLoaded != null)
                OnLoaded(this, new EventArgs());
        }

        public void SaveTree()
        {
            EntityNodesRelatedEntityNodesProvider.DeleteAll(UserContext.Current.SelectedOrganization.OrganizationId, EntityNodeId, EntityId);
            RelationType rt;
            foreach (RadTreeNode rtn in this.CheckedNodes)
            {
                if (rtn.Category == "1")
                    rt = RelationType.CheckedAndAllChildren;
                else if (rtn.Category == "2")
                    rt = RelationType.Blocked;
                else
                    rt = RelationType.Checked;

                EntityNodesRelatedEntityNodesProvider.InsertEntityNodesRelatedEntityNodes(EntityNodeId, new Guid(rtn.Value), EntityId, rt, UserContext.Current.SelectedOrganization.OrganizationId);
            }
            if (OnSaved != null)
                OnSaved(this, new EventArgs());
        }

        #endregion
    }
}
