using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Micajah.Common.Bll.Providers;
using Micajah.Common.Configuration;
using Micajah.Common.Pages;
using Telerik.Web.UI;

namespace Micajah.Common.WebControls
{
    /// <summary>
    /// The control displays the hierarchical data structure as tree.
    /// </summary>
    [ToolboxData("<{0}:TreeView runat=server></{0}:TreeView>")]
    public class TreeView : RadTreeView, IThemeable
    {
        #region Private Properties

        private string NodeCheckedHandler
        {
            get
            {
                return string.Concat("function ", ClientID, "_NodeChecked(sender, eventArgs) { TreeView_NodeChecked(sender, eventArgs); ", NodeCheckedOriginalHandler, "(sender, eventArgs); }\r\n");
            }
        }

        private string NodeClickedHandler
        {
            get
            {
                return string.Concat("function ", ClientID, "_NodeClicked(sender, eventArgs) { TreeView_NodeClicked(sender, eventArgs); ", NodeClickedOriginalHandler, "(sender, eventArgs); }\r\n");
            }
        }

        private string NodeDroppingHandler
        {
            get
            {
                return string.Concat("function ", ClientID, "_NodeDropping(sender, eventArgs) { TreeView_NodeDropping(sender, eventArgs); if (!eventArgs.get_cancel()) { ", NodeDroppingOriginalHandler, "(sender, eventArgs); } }\r\n");
            }
        }

        private string NodeCheckedOriginalHandler
        {
            get
            {
                object obj = base.ViewState["NodeCheckedOriginalHandler"];
                return (obj == null) ? string.Empty : (string)obj;
            }
            set { base.ViewState["NodeCheckedOriginalHandler"] = value; }
        }

        private string NodeClickedOriginalHandler
        {
            get
            {
                object obj = base.ViewState["NodeClickedOriginalHandler"];
                return (obj == null) ? string.Empty : (string)obj;
            }
            set { base.ViewState["NodeClickedOriginalHandler"] = value; }
        }

        private string NodeDroppingOriginalHandler
        {
            get
            {
                object obj = base.ViewState["NodeDroppingOriginalHandler"];
                return (obj == null) ? string.Empty : (string)obj;
            }
            set { base.ViewState["NodeDroppingOriginalHandler"] = value; }
        }

        private string SelectedValues
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                IList<RadTreeNode> nodes = null;

                if (base.CheckBoxes)
                {
                    nodes = this.CheckedNodes;
                }
                else
                {
                    nodes = this.SelectedNodes;
                }

                foreach (RadTreeNode node in nodes)
                {
                    sb.AppendFormat(", {0}", node.Text);
                }

                if (sb.Length > 1)
                {
                    sb.Remove(0, 2);
                }

                return sb.ToString();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or set the value indicating that control is displayed as treeview combobox.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool ComboBoxMode
        {
            get
            {
                object obj = ViewState["ComboBoxMode"];
                return ((obj == null) ? false : (bool)obj);
            }
            set { ViewState["ComboBoxMode"] = value; }
        }

        public MasterPageTheme Theme
        {
            get
            {
                object obj = ViewState["Theme"];
                return ((obj == null) ? FrameworkConfiguration.Current.WebApplication.MasterPage.Theme : (MasterPageTheme)obj);
            }
            set { ViewState["Theme"] = value; }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Returns a list of client script library dependencies for the control.
        /// </summary>
        /// <returns>The list of client script library dependencies for the control.</returns>
        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            if (this.ComboBoxMode || base.EnableDragAndDrop)
            {
                List<ScriptReference> list = new List<ScriptReference>();
                list.AddRange(base.GetScriptReferences());
                list.Add(new ScriptReference(ResourceProvider.GetResourceUrl("Scripts.TreeView.js", true)));
                return list;
            }
            else
                return base.GetScriptReferences();
        }

        /// <summary>
        /// Raises the PreRender event and registers client scripts.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            StringBuilder sb = new StringBuilder();


            if (this.ComboBoxMode)
            {
                if (base.CheckBoxes)
                {
                    if (!string.IsNullOrEmpty(base.OnClientNodeChecked) && (!base.OnClientNodeChecked.Equals("TreeView_NodeChecked")))
                    {
                        if (!Page.IsPostBack)
                        {
                            NodeCheckedOriginalHandler = base.OnClientNodeChecked;
                        }

                        sb.Append(NodeCheckedHandler);

                        base.OnClientNodeChecked = string.Concat(ClientID, "_NodeChecked");
                    }
                    else
                    {
                        base.OnClientNodeChecked = "TreeView_NodeChecked";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(base.OnClientNodeClicked) && (!base.OnClientNodeClicked.Equals("TreeView_NodeClicked")))
                    {
                        if (!Page.IsPostBack)
                        {
                            NodeClickedOriginalHandler = base.OnClientNodeClicked;
                        }

                        sb.Append(NodeClickedHandler);

                        base.OnClientNodeClicked = string.Concat(ClientID, "_NodeClicked");
                    }
                    else
                    {
                        base.OnClientNodeClicked = "TreeView_NodeClicked";
                    }
                }
            }

            if (base.EnableDragAndDrop)
            {
                if (!string.IsNullOrEmpty(base.OnClientNodeDropping) && (!base.OnClientNodeDropping.Equals("TreeView_NodeDropping")))
                {
                    if (!Page.IsPostBack)
                    {
                        NodeDroppingOriginalHandler = base.OnClientNodeDropping;
                    }

                    sb.Append(NodeDroppingHandler);

                    base.OnClientNodeDropping = string.Concat(ClientID, "_NodeDropping");
                }
                else
                {
                    base.OnClientNodeDropping = "TreeView_NodeDropping";
                }
            }

            if (sb.Length > 0)
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), ClientID, sb.ToString(), true);
        }

        /// <summary>
        /// Renders control.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter to render content to.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (writer == null) return;

            Unit width = Width;

            if (this.ComboBoxMode)
            {
                if ((Width.Type == UnitType.Percentage) || Width.IsEmpty) Width = Unit.Pixel(200);

                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, Width.ToString());
                writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Concat(ClientID, "_container"));
                writer.AddAttribute(HtmlTextWriterAttribute.Name, string.Concat(UniqueID, "$container"));
                writer.RenderBeginTag(HtmlTextWriterTag.Div); // Div1

                if (this.Theme == MasterPageTheme.Modern)
                {
                    writer.AddStyleAttribute("background", string.Format(CultureInfo.InvariantCulture, "url(\"{0}\") no-repeat right 6px", ResourceProvider.GetImageUrl(typeof(TreeView), "Modern.png", true)));
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "31px");
                }
                else
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingRight, "19px");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.WhiteSpace, "nowrap");
                    writer.AddStyleAttribute("border", "1px solid ActiveBorder");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "19px");
                    writer.AddStyleAttribute("*height", "21px");
                    writer.AddStyleAttribute("background", string.Format(CultureInfo.InvariantCulture, "Window url(\"{0}\") no-repeat right top", ResourceProvider.GetImageUrl(typeof(TreeView), "DropArrow.gif", true)));
                }
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Concat("TreeView_ToggleTreeView('", this.ClientID + "');"));
                writer.RenderBeginTag(HtmlTextWriterTag.Div); // Div2

                writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Concat(ClientID, "_txt"));
                writer.AddAttribute(HtmlTextWriterAttribute.Name, string.Concat(UniqueID, "$txt"));
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                writer.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "readonly");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Cursor, "default");
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "Transparent");
                if (this.Theme == MasterPageTheme.Modern)
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingRight, "36px !important");
                }
                else
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.VerticalAlign, "middle");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingTop, "2px");
                    writer.AddStyleAttribute("border", "0 none");
                }

                string value = SelectedValues;
                if (!string.IsNullOrEmpty(value))
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, value);

                writer.RenderBeginTag(HtmlTextWriterTag.Input); // Input
                writer.RenderEndTag(); // Input

                writer.RenderEndTag(); // Div2

                if (this.Theme == MasterPageTheme.Modern)
                {
                    Style.Add(HtmlTextWriterStyle.BorderColor, "#646464");
                    Style.Add(HtmlTextWriterStyle.MarginTop, "-1px");

                    if (Width.Type == UnitType.Pixel) base.Width = Unit.Pixel((int)Width.Value - 2);
                }
                else
                    Style.Add(HtmlTextWriterStyle.BorderColor, "ActiveBorder");
                Style.Add(HtmlTextWriterStyle.Position, "absolute");
                Style.Add(HtmlTextWriterStyle.Display, "none");
                Style.Add(HtmlTextWriterStyle.BackgroundColor, "Window");
                Style.Add(HtmlTextWriterStyle.BorderStyle, "solid");
                Style.Add(HtmlTextWriterStyle.BorderWidth, "1px");
                Style.Add(HtmlTextWriterStyle.Height, "150px");
                Style.Add(HtmlTextWriterStyle.ZIndex, "500");
            }

            base.Render(writer);

            if (ComboBoxMode)
            {
                base.Width = width;
                writer.RenderEndTag(); // Div1
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the details of the node or adds a new item to items collection if the node is not found.
        /// </summary>
        /// <param name="parentNodeValue">The value of the parent node.</param>
        /// <param name="nodeValue">The value to find node.</param>
        /// <param name="nodeText">The node text.</param>
        public void SaveNode(string parentNodeValue, string nodeValue, string nodeText)
        {
            SaveNode(parentNodeValue, nodeValue, nodeText, null);
        }

        /// <summary>
        /// Updates the details of the node or adds a new item to items collection if the node is not found.
        /// </summary>
        /// <param name="parentNodeValue">The value of the parent node.</param>
        /// <param name="nodeValue">The value to find node.</param>
        /// <param name="nodeText">The node text.</param>
        /// <param name="nodeCategory">The node category.</param>
        public void SaveNode(string parentNodeValue, string nodeValue, string nodeText, string nodeCategory)
        {
            if (parentNodeValue == null) parentNodeValue = string.Empty;
            if (nodeCategory == null) nodeCategory = string.Empty;

            RadTreeNode parentNode = this.FindNodeByValue(parentNodeValue);
            RadTreeNode node = this.FindNodeByValue(nodeValue);
            RadTreeNode[] childNodes = null;
            bool added = true;
            string parentNodeValueOld = string.Empty;

            if (node != null)
            {
                added = false;
                if (node.ParentNode != null) parentNodeValueOld = node.ParentNode.Value;
                if (parentNodeValueOld == null) parentNodeValueOld = string.Empty;
                node.Text = nodeText;

                if (string.Compare(parentNodeValue, parentNodeValueOld, true, CultureInfo.CurrentCulture) != 0)
                {
                    childNodes = new RadTreeNode[node.Nodes.Count];
                    node.Nodes.CopyTo(childNodes, 0);
                    node.Remove();
                    node = null;
                    added = true;
                }
            }

            if (node == null) node = new RadTreeNode(nodeText, nodeValue);
            node.Category = nodeCategory;

            if (childNodes != null)
            {
                foreach (RadTreeNode childNode in childNodes)
                {
                    node.Nodes.Add(childNode);
                }
            }

            if (parentNode != null)
            {
                if (string.Compare(parentNodeValue, parentNodeValueOld, true, CultureInfo.CurrentCulture) != 0)
                    parentNode.Nodes.Add(node);
            }
            else if (added) this.Nodes.Add(node);

            node.ExpandParentNodes();
        }

        public void Rebind()
        {
            this.Rebind(false);
        }

        public void Rebind(bool saveExpandedState)
        {
            List<string> list = null;
            if (saveExpandedState)
            {
                list = new List<string>();
                foreach (RadTreeNode node in this.GetAllNodes())
                {
                    if (node.Expanded)
                        list.Add(node.Value);
                }
            }

            base.DataBind();

            if (saveExpandedState)
            {
                foreach (RadTreeNode node in this.GetAllNodes())
                {
                    if (list.Contains(node.Value))
                        node.Expanded = true;
                }
            }
        }

        #endregion
    }
}