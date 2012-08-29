<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.EntityControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<script type="text/javascript">
    //<![CDATA[
    function ContextMenuItemClicking(sender, eventArgs) {
        var treeNode = eventArgs.get_node();
        var item = eventArgs.get_menuItem();
        if (item.get_value() == "Delete") {
            if (!confirm("<%= DeleteButtonConfirmText %>")) {
                eventArgs.set_cancel(true);
                item.get_menu().hide();
            }
        }
        else if (item.get_value() == "Rename") {
            treeNode.startEdit();
            item.get_menu().hide();
            eventArgs.set_cancel(true);
        }
        else
            eventArgs.get_node().select();
    }

    function onClientNodeClicked(sender, eventArgs) {
        var list = document.getElementById('ctl00_PageBody_Entity1_List');
        if (list != null)
            list.style.display = "none";
        var node = eventArgs.get_node();
        var menu = node.get_contextMenu();
        var domEvent = eventArgs.get_domEvent();
        sender._contextMenuNode = node;
        if (menu != null)
            menu.show(domEvent);
    }

    function ClientNodeDragStart(sender, eventArgs) {
        var node = eventArgs.get_node();
        var Level = node.get_level();
        var NodeValue = node.get_value();
        var nodes = sender.get_allNodes();
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].get_level() > 0 && (nodes[i].get_level() == Level - 1 || nodes[i].get_level() == Level) && nodes[i].get_value() != NodeValue)
                nodes[i].set_allowDrop(true);
            else
                nodes[i].set_allowDrop(false);
        }
    }

    function ChildNodeExist(ParentNode, ChildText) {
        var ChildNodes = ParentNode.get_nodes();
        var ChildCount = ChildNodes.get_count();
        for (var i = 0; i < ChildCount; i++) {
            var node = ChildNodes.getNode(i);
            if (node.get_text() == ChildText) return true;
        }
        return false;
    }

    function ClientNodeDropping(sender, eventArgs) {
        var DestNode = eventArgs.get_destNode();
        var SourceNode = eventArgs.get_sourceNode();
        if (DestNode != null && SourceNode != null) {
            var tree = DestNode.get_treeView();
            var DestLevel = DestNode.get_level();
            var SourceLevel = SourceNode.get_level();
            if (DestLevel == SourceLevel) {
                if (!confirm("Merge '" + SourceNode.get_text() + "' to '" + DestNode.get_text() + "'?")) {
                    eventArgs.set_cancel(true);
                    return;
                }
            }
            else {
                if (DestLevel + 1 == SourceLevel) {
                    if (ChildNodeExist(DestNode, SourceNode.get_text())) {
                        if (eventArgs.get_domEvent().ctrlKey) {
                            alert("Can not copy '" + SourceNode.get_text() + "'! This exist in destination.");
                            eventArgs.set_cancel(true);
                            return;
                        }
                        if (!confirm("'" + SourceNode.get_text() + "' exist in '" + DestNode.get_text() + "'! Merge it ?")) {
                            eventArgs.set_cancel(true);
                            return;
                        }
                    }
                    var hidden = document.getElementById("CtrlKeyField");
                    if (eventArgs.get_domEvent().ctrlKey) {
                        hidden.value = "True";
                        if (!confirm("Copy '" + SourceNode.get_text() + "' to '" + DestNode.get_text() + "'?")) {
                            eventArgs.set_cancel(true);
                            return;
                        }
                    }
                    else
                        hidden.value = "False";
                }
            }
        }
    }

    function OnClientNodeEditStartHandler(sender, eventArgs) {
        var node = eventArgs.get_node();
        var textInput = node.get_inputElement();
        var myLink = document.createElement('a');
        var href = document.createAttribute('href');
        myLink.setAttribute('href', '#');
        myLink.innerText = "[Save]";
        textInput.parentNode.appendChild(myLink);
    }
    //]]>
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="SearchPanel" runat="server" Style="padding-bottom: 7px; position: relative;
            left: -5px;" Width="350px">
            <mits:ComboBox ID="InstanceList" runat="server" DataTextField="Name" DataValueField="InstanceId"
                DataSourceId="InstancesDataSource" Width="100%" AutoPostBack="true" OnDataBound="InstanceList_DataBound" />
        </asp:Panel>
        <table width="100%">
            <tr>
                <td valign="top">
                    <mits:TreeView ID="Tree" runat="server" DataFieldID="EntityNodeId" DataValueField="EntityNodeId"
                        DataFieldParentID="ParentEntityNodeId" DataTextField="Name" EnableDragAndDrop="True"
                        AllowNodeEditing="false" EnableDragAndDropBetweenNodes="true" OnClientNodeDragStart="ClientNodeDragStart"
                        OnClientNodeDropping="ClientNodeDropping" OnClientContextMenuItemClicking="ContextMenuItemClicking"
                        OnNodeDataBound="Tree_NodeDataBound" OnClientNodeClicked="onClientNodeClicked"
                        OnContextMenuItemClick="Tree_ContextMenuItemClick" OnNodeDrop="Tree_NodeDrop"
                        OnNodeEdit="Tree_NodeEdit" OnClientNodeEditStart="OnClientNodeEditStartHandler" />
                </td>
                <td valign="top">
                    <mits:CommonGridView ID="List" runat="server" Visible="false" DataSourceID="ListDataSource"
                        EnableSelect="true" ShowAddLink="false" Width="300px" AutoGenerateColumns="false"
                        OnAction="List_Action" AutoGenerateDeleteButton="false" AutoGenerateEditButton="false"
                        DataKeyNames="EntityNodeTypeId">
                        <columns>
                            <mits:TextBoxField DataField="Name"  />
                            <mits:TextBoxField DataField="OrderNumber" />
                        </columns>
                    </mits:CommonGridView>
                </td>
                <td valign="top" style="width: 150px;">
                    <ul style="color: Gray">
                        <asp:Label ID="DescriptionLabel" runat="server" />
                    </ul>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <input type="hidden" id="CtrlKeyField" name="CtrlKeyField" value="False" />
        <asp:Label ID="RestrictErrorLabel" CssClass="ErrorMessage" runat="server" Visible="False" />
        <asp:ObjectDataSource ID="ListDataSource" runat="server" SelectMethod="GetEntityNodeTypesByEntityId"
            TypeName="Micajah.Common.Bll.Providers.EntityNodeProvider" OnSelecting="ListDataSource_Selecting">
            <SelectParameters>
                <asp:Parameter Name="organizationId" DbType="Guid" />
                <asp:Parameter Name="instanceId" DbType="Guid" />
                <asp:Parameter Name="entityId" DbType="Guid" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="InstancesDataSource" runat="server" SelectMethod="GetInstances"
            TypeName="Micajah.Common.Bll.Providers.InstanceProvider" OnSelecting="InstancesDataSource_Selecting">
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
