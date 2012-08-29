<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SetupControls.RoleEditControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="RoleId"
    Width="550px" AutoGenerateRows="False" DefaultMode="ReadOnly">
    <fields>
        <mits:TextBoxField DataField="Name" HeaderStyle-Width="120px" />
        <mits:TextBoxField DataField="Description" />
        <mits:TextBoxField DataField="ShortName" />
        <mits:TextBoxField DataField="Rank" />
        <mits:TemplateField PaddingLeft="false">
            <itemtemplate>
                <div id="Div1" runat="server" style="height: 200px; overflow: auto;"></div>
            </itemtemplate>
        </mits:TemplateField>
    </fields>
</mits:MagicForm>
<asp:Table ID="CommandTable" runat="server" Width="550px">
    <asp:TableFooterRow>
        <asp:TableCell Style="border: none 0 !important;">
            <asp:LinkButton ID="CloseButton" runat="server" CausesValidation="false" OnClick="CloseButton_Click" />
        </asp:TableCell>
    </asp:TableFooterRow>
</asp:Table>
<div style="display: none;">
    <mits:TreeView ID="Atv" runat="server" CheckBoxes="true" Enabled="False" DataFieldID="ActionId"
        DataValueField="ActionId" DataFieldParentID="ParentActionId" DataTextField="Name"
        OnNodeDataBound="Atv_NodeDataBound" />
</div>
<asp:ObjectDataSource ID="EntityDataSource" runat="server" SelectMethod="GetRoleRow"
    TypeName="Micajah.Common.Bll.Providers.RoleProvider">
    <SelectParameters>
        <asp:QueryStringParameter QueryStringField="RoleId" Name="roleId" Type="Object" />
    </SelectParameters>
</asp:ObjectDataSource>
