using System;
using System.Web.UI;
using Micajah.Common.Application;
using Micajah.Common.Bll;
using Micajah.Common.Bll.Providers;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls.SetupControls
{
    public class EntitiesControl : UserControl
    {
        #region Members

        protected TreeView Tree;

        #endregion

        #region Private Methods

        private void Tree_DataBind()
        {
            Type typeOfThis = typeof(EntitiesControl);
            string entityImageUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.GetImageUrl(typeOfThis, "Entity.gif"));
            string fieldImageUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.GetImageUrl(typeOfThis, "Field.gif"));
            string eventImageUrl = CustomUrlProvider.CreateApplicationAbsoluteUrl(ResourceProvider.GetImageUrl(typeOfThis, "Event.gif"));

            foreach (Entity entity in WebApplication.Entities)
            {
                RadTreeNode entityNode = new RadTreeNode(entity.Name);
                entityNode.ImageUrl = entityImageUrl;
                Tree.Nodes.Add(entityNode);

                if (entity.EnableHierarchy)
                {
                    RadTreeNode parentNode = entityNode;
                    foreach (EntityNodeType nodeType in entity.NodeTypes)
                    {
                        RadTreeNode nodeTypeNode = new RadTreeNode(nodeType.Name);
                        nodeTypeNode.ImageUrl = entityImageUrl;
                        parentNode.Nodes.Add(nodeTypeNode);

                        foreach (EntityEvent entityEvent in nodeType.Events)
                        {
                            RadTreeNode eventNode = new RadTreeNode(entityEvent.Name);
                            eventNode.ImageUrl = eventImageUrl;
                            nodeTypeNode.Nodes.Add(eventNode);
                        }

                        parentNode = nodeTypeNode;
                    }

                    foreach (EntityEvent entityEvent in entity.CustomEvents)
                    {
                        RadTreeNode eventNode = new RadTreeNode(entityEvent.Name);
                        eventNode.ImageUrl = eventImageUrl;
                        entityNode.Nodes.Add(eventNode);
                    }
                }
                else
                {
                    foreach (EntityField field in entity.Fields)
                    {
                        RadTreeNode fieldNode = new RadTreeNode(field.Name);
                        fieldNode.ImageUrl = fieldImageUrl;
                        entityNode.Nodes.Add(fieldNode);
                    }

                    foreach (EntityEvent entityEvent in entity.Events)
                    {
                        RadTreeNode eventNode = new RadTreeNode(entityEvent.Name);
                        eventNode.ImageUrl = eventImageUrl;
                        entityNode.Nodes.Add(eventNode);
                    }
                }
            }
        }

        #endregion

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack) this.Tree_DataBind();
        }

        #endregion
    }
}
