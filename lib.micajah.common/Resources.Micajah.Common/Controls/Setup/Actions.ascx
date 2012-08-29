<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SetupControls.ActionsControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<script type="text/javascript">
    //<![CDATA[
    function NodeClicking(sender, eventArgs) {
        if (eventArgs.get_node().get_parent() == sender) eventArgs.set_cancel(true);
    }
    //]]>
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table cellspacing="0" cellpadding="0">
            <tr valign="top">
                <td style="width: 440px;">
                    <mits:TreeView ID="Tree" runat="server" DataFieldID="ActionId" DataValueField="ActionId"
                        DataFieldParentID="ParentActionId" DataTextField="Name" OnClientNodeClicking="NodeClicking"
                        OnNodeClick="Tree_NodeClick" OnNodeDataBound="Tree_NodeDataBound" />
                </td>
                <td style="padding-left: 20px;">
                    <mits:MagicForm ID="EditForm" runat="server" AutoGenerateRows="False" DefaultMode="ReadOnly"
                        DataKeyNames="ActionId" DataSourceID="EntityDataSource" Visible="false" Width="550px">
                        <fields>
                            <mits:TextBoxField DataField="Name" HeaderStyle-Width="120px" />
                            <mits:TextBoxField DataField="Description" />
                            <mits:TextBoxField DataField="NavigateUrl" />
                            <mits:TextBoxField DataField="OrderNumber" />
                            <mits:CheckBoxField DataField="AuthenticationRequired" />
                            <mits:CheckBoxField DataField="InstanceRequired" />
                            <mits:CheckBoxField DataField="Visible" />
                            <mits:TemplateField>
                                <ItemTemplate>
                                    <div id="Div1" runat="server" style="height: 150px; overflow: auto;"></div>
                                </ItemTemplate>
                            </mits:TemplateField>
                            <mits:GroupField />
                            <mits:TextBoxField DataField="SubmenuItemType" />
                            <mits:TextBoxField DataField="SubmenuItemWidth" />
                            <mits:TextBoxField DataField="SubmenuItemImageUrl" />
                            <mits:GroupField />
                            <mits:TextBoxField DataField="IconUrl" />
                            <mits:TextBoxField DataField="LearnMoreUrl" />
                            <mits:CheckBoxField DataField="ShowInDetailMenu" />
                            <mits:CheckBoxField DataField="ShowChildrenInDetailMenu" />
                            <mits:CheckBoxField DataField="ShowDescriptionInDetailMenu" />
                            <mits:CheckBoxField DataField="GroupInDetailMenu" />
                            <mits:CheckBoxField DataField="HighlightInDetailMenu" />
                        </fields>
                    </mits:MagicForm>
                    <asp:Table ID="CommandTable" runat="server" Visible="false" Width="550px">
                        <asp:TableFooterRow>
                            <asp:TableCell Style="border: none 0 !important;">
                                <asp:LinkButton ID="CloseButton" runat="server" CausesValidation="false" OnClick="CloseButton_Click" />
                            </asp:TableCell>
                        </asp:TableFooterRow>
                    </asp:Table>
                </td>
            </tr>
        </table>
        <div style="display: none;">
            <mits:TreeView ID="AlternativeParentsTree" runat="server" CheckBoxes="true" Enabled="False"
                DataFieldID="ActionId" DataValueField="ActionId" DataFieldParentID="ParentActionId"
                DataTextField="Name" OnNodeDataBound="AlternativeParentsTree_NodeDataBound" />
        </div>
        <asp:Label ID="ActionTypeIdLabel" runat="server" Visible="False" />
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" SelectMethod="GetAction"
            TypeName="Micajah.Common.Bll.Providers.ActionProvider">
            <SelectParameters>
                <asp:ControlParameter ControlID="Tree" PropertyName="SelectedNode.Value" Name="actionId"
                    Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
