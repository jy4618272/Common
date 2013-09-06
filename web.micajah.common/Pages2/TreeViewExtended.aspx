<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="TreeViewExtendedTestPage" Codebehind="TreeViewExtended.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <table width="100%">
        <tr valign="top">
            <td>
                Default mode
                <mits:TreeView ID="TreeViewExtended1" runat="Server" CheckBoxes="true" DragAndDrop="true"
                    DataFieldID="ActionId" DataFieldParentID="ParentActionId" DataTextField="Name"
                    DataValueField="ActionId" AutoPostBack="true" DataSourceID="ObjectDataSource1"
                    Height="200px" />
            </td>
            <td>
                <p>
                    ComboBox mode, multiple select is allowed
                    <mits:TreeView ID="TreeViewExtended2" runat="Server" MultipleSelect="true" ComboBoxMode="True"
                        DataFieldID="ActionId" DataFieldParentID="ParentActionId" DataTextField="Name"
                        DataValueField="ActionId" Width="9cm" DataSourceID="ObjectDataSource1" />
                </p>
                <p>
                    ComboBox mode, multiple select is not allowed
                    <mits:TreeView ID="TreeViewExtended3" runat="Server" ComboBoxMode="True" DataFieldID="ActionId"
                        DataFieldParentID="ParentActionId" DataTextField="Name" DataValueField="ActionId"
                        Width="9cm" DataSourceID="ObjectDataSource1" />
                </p>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <p>
                    Heirarchical Entity User Control
                    <mits:EntityTreeView runat="server" ID="EntityTreeView1" 
                        CheckBoxes="True" CustomRootNodeText="teccc" EntityId="4cda22f3-4f01-4768-8608-938dc6a06825" />
                </p>
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetActions"
        TypeName="Micajah.Common.Bll.Providers.ActionProvider"></asp:ObjectDataSource>
</asp:Content>
