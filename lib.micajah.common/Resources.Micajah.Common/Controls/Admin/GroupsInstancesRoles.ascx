<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.GroupsInstancesRolesControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<script type="text/javascript">
    //<![CDATA[
    function NodeChecking(sender, eventArgs) {
        if (eventArgs.get_node().get_category() == "NonCheckable") eventArgs.set_cancel(true);
    }
    //]]>
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:CommonGridView ID="List" runat="server" DataKeyNames="GroupId,InstanceId" DataSourceID="EntityListDataSource"
            Width="700px" OnAction="List_Action">
            <columns>
                <mits:TextBoxField DataField="InstanceName" />
                <mits:TextBoxField DataField="RoleName" />
                <mits:TemplateField ItemStyle-Wrap="False" ItemStyle-Width="40px">
                    <itemtemplate>
                        <asp:LinkButton ID="ActionsLink" runat="server" CommandName="Select" Text="<%# ActionsLinkText %>"
                            Visible='<%# LinkVisible(Eval("RoleId")) %>' />
                    </itemtemplate>
                </mits:TemplateField>
                <mits:TemplateField ItemStyle-Wrap="False" ItemStyle-Width="40px">
                    <itemtemplate>
                        <asp:HyperLink ID="SettingsLink" runat="server" Text="<%# SettingsLinkText %>"
                            NavigateUrl='<%# GetSettingsLink(Eval("GroupId"), Eval("InstanceId")) %>'
                            Visible='<%# LinkVisible(Eval("RoleId")) %>' />
                    </itemtemplate>
                </mits:TemplateField>
            </columns>
        </mits:CommonGridView>
        <mits:MagicForm ID="EditForm" runat="server" AutoGenerateInsertButton="True" AutoGenerateRows="False"
            DefaultMode="Insert" DataSourceID="EntityDataSource" Width="550px" Visible="false"
            OnItemInserted="EditForm_ItemInserted" OnItemCommand="EditForm_ItemCommand">
            <fields>
                <mits:TemplateField PaddingLeft="false" HeaderStyle-Width="120px">
                    <itemtemplate>
                        <asp:DropDownList ID="InstanceId" runat="server" DataTextField="Name" DataValueField="InstanceId"
                            DataSourceID="InstanceDataSource" SelectedValue='<%# Bind("InstanceId") %>' Width="350px" />
                    </itemtemplate>
                </mits:TemplateField>
                <mits:TemplateField PaddingLeft="false">
                    <itemtemplate>
                        <asp:DropDownList ID="RoleId" runat="server" DataTextField="Name" DataValueField="RoleId"
                            DataSourceID="RoleDataSource" SelectedValue='<%# Bind("RoleId") %>' Width="350px" />
                    </itemtemplate>
                </mits:TemplateField>
                <mits:TemplateField PaddingLeft="false">
                    <itemtemplate>
                        <div id="ErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </itemtemplate>
                </mits:TemplateField>
            </fields>
        </mits:MagicForm>
        <asp:Table ID="ActionsTable" runat="server" Visible="false" Width="550px">
            <asp:TableHeaderRow runat="server" TableSection="TableHeader">
                <asp:TableHeaderCell runat="server">
                    <asp:Literal ID="CaptionLiteral" runat="server"></asp:Literal>
                </asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell>
                    <div style="height: 200px; overflow: auto;">
                        <mits:TreeView ID="Atv" runat="server" CheckBoxes="true" DataFieldID="ActionId" DataValueField="ActionId"
                            DataFieldParentID="ParentActionId" DataTextField="Name" OnClientNodeChecking="NodeChecking"
                            OnNodeDataBound="Atv_NodeDataBound" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableFooterRow>
                <asp:TableCell>
                    <asp:Button ID="SaveButton" runat="server" OnClick="SaveButton_Click" />
                    <asp:PlaceHolder ID="ButtonsSeparator" runat="server" />
                    <asp:LinkButton ID="CancelButton" runat="server" CssClass="Cancel" OnClick="CancelButton_Click" />
                </asp:TableCell>
            </asp:TableFooterRow>
        </asp:Table>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" DeleteMethod="DeleteGroupInstanceRoles"
            SelectMethod="GetGroupInstancesRoles" TypeName="Micajah.Common.Bll.Providers.GroupProvider"
            OnSelected="EntityListDataSource_Selected">
            <DeleteParameters>
                <asp:Parameter Name="groupId" Type="Object" />
                <asp:Parameter Name="instanceId" Type="Object" />
            </DeleteParameters>
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="GroupId" Name="groupId" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" InsertMethod="InsertGroupInstanceRole"
            TypeName="Micajah.Common.Bll.Providers.GroupProvider" OnInserting="EntityDataSource_Inserting">
            <InsertParameters>
                <asp:QueryStringParameter QueryStringField="GroupId" Name="groupId" Type="Object" />
                <asp:Parameter Name="instanceId" Type="Object" />
                <asp:Parameter Name="roleId" Type="Object" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="InstanceDataSource" runat="server" SelectMethod="GetInstances"
            TypeName="Micajah.Common.Bll.Providers.InstanceProvider" />
        <asp:ObjectDataSource ID="RoleDataSource" runat="server" SelectMethod="GetAvailableRoles"
            TypeName="Micajah.Common.Bll.Providers.RoleProvider">
            <SelectParameters>
                <asp:Parameter Name="includeInstanceAdministratorRole" Type="Boolean" DefaultValue="False" />
                <asp:Parameter Name="sortExpression" Type="string" DefaultValue="Rank" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
