<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.RulesControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="SearchPanel" runat="server" Style="padding-bottom: 7px; position: relative;
            left: -5px;" Width="350px">
            <mits:ComboBox ID="InstanceList" runat="server" DataTextField="Name" DataValueField="InstanceId"
                DataSourceId="InstancesDataSource" Width="100%" AutoPostBack="true" OnDataBound="InstanceList_DataBound" />
        </asp:Panel>
        <mits:MagicForm ID="FormRuleEngine" runat="server" CellPadding="0" DataKeyNames="Id,OutputEditPage"
            DataSourceID="ObjectDataSourceRuleEngineView" Width="550px">
            <fields>
                <mits:TextBoxField DataField="Name" HeaderStyle-Width="120px" />
                <mits:TextBoxField DataField="DisplayName" />
            </fields>
        </mits:MagicForm>
        <br />
        <mits:CommonGridView ID="List" runat="server" DataKeyNames="RuleId" DataSourceID="EntityListDataSource"
            Width="700px" OnRowDataBound="List_RowDataBound">
            <captioncontrols>
                <asp:LinkButton ID="ButtonUpdateOrder" runat="server" OnInit="ButtonUpdateOrder_Init" OnClick="ButtonUpdateOrder_Click"></asp:LinkButton>
            </captioncontrols>
            <columns>
                <mits:TemplateField>
                    <ItemTemplate>
                        <mits:TextBox ID="TextBoxOrder" runat="server" Required="true" ValidationType="Integer"
                            MinimumValue="0" MaximumValue="254" MaxLength="3" Columns="2" Text='<%# Eval("OrderNumber") %>'>
                        </mits:TextBox>
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:HyperLinkField DataNavigateUrlFields="RuleId" ItemStyle-HorizontalAlign="Center" />
                <mits:HyperLinkField DataNavigateUrlFields="RuleId" ItemStyle-HorizontalAlign="Center" />
                <mits:TextBoxField DataField="Name" SortExpression="Name" />
                <mits:TextBoxField DataField="DisplayName" SortExpression="DisplayName" />
                <mits:TextBoxField DataField="UsedQty" SortExpression="UsedQty"/>
                <mits:TemplateField SortExpression="LastUsedUser">
                    <ItemTemplate>
                        <%# Eval("LastUsedUser") != DBNull.Value ? Micajah.Common.Bll.Providers.RuleEngineProvider.GetDisplayUserName((Guid)Eval("LastUsedUser"), (Guid)Eval("OrganizationId")) : string.Empty %>
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField SortExpression="LastUsedDate">
                    <ItemTemplate>
                        <asp:Literal ID="LastUsedDateLiteral" runat="server"></asp:Literal>
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField SortExpression="CreatedBy">
                    <ItemTemplate>
                        <%# Micajah.Common.Bll.Providers.RuleEngineProvider.GetDisplayUserName((Guid)Eval("CreatedBy"), (Guid)Eval("OrganizationId"))%>
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField SortExpression="CreatedDate">
                    <ItemTemplate>
                        <asp:Literal ID="CreatedDateLiteral" runat="server"></asp:Literal>
                    </ItemTemplate>
                </mits:TemplateField>
            </columns>
        </mits:CommonGridView>
        <mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="RuleId"
            Width="550px" Visible="False">
            <fields>
                <mits:TextBoxField DataField="Name" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" Required="true" />
                <mits:TextBoxField DataField="DisplayName" MaxLength="255" Columns="65" ControlStyle-Width="350px" Required="true" />
                <mits:TextBoxField DataField="OrderNumber" Required="true" ValidationType="Integer" Columns="10" MinimumValue="0" MaximumValue="254" MaxLength="3" />
                <mits:CheckBoxField DataField="Active" InsertVisible="false" />
                <mits:TemplateField>
                    <itemtemplate>
                        <div id="ErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </itemtemplate>
                </mits:TemplateField>
            </fields>
        </mits:MagicForm>
        <br />
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" DeleteMethod="DeleteRule"
            SelectMethod="GetRules" TypeName="Micajah.Common.Bll.Providers.RuleEngineProvider"
            OnSelecting="InstancesDataSource_Selecting">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="RuleEngineId" Type="Object" Name="ruleEngineId" />
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:ControlParameter Name="instanceId" Type="Object" ControlID="InstanceList" PropertyName="SelectedValue" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="ruleId" Type="Object" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" InsertMethod="InsertRule"
            SelectMethod="GetRuleRow" TypeName="Micajah.Common.Bll.Providers.RuleEngineProvider"
            UpdateMethod="UpdateRule" OnInserting="EntityDataSource_Inserting" OnUpdating="InstancesDataSource_Selecting">
            <UpdateParameters>
                <asp:Parameter Name="ruleId" Type="Object" />
                <asp:QueryStringParameter QueryStringField="RuleEngineId" Name="ruleEngineId" Type="Object" />
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:ControlParameter Name="instanceId" Type="Object" ControlID="InstanceList" PropertyName="SelectedValue" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="displayName" Type="String" />
                <asp:Parameter Name="orderNumber" Type="Int32" />
                <asp:Parameter Name="active" Type="Boolean" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="ruleId"
                    Type="Object" />
            </SelectParameters>
            <InsertParameters>
                <asp:QueryStringParameter QueryStringField="RuleEngineId" Name="ruleEngineId" Type="Object" />
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:ControlParameter Name="instanceId" Type="Object" ControlID="InstanceList" PropertyName="SelectedValue" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="displayName" Type="String" />
                <asp:Parameter Name="orderNumber" Type="Int32" />
                <asp:Parameter Name="active" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSourceRuleEngineView" runat="server" SelectMethod="GetRulesEngine"
            TypeName="Micajah.Common.Bll.Providers.RuleEngineProvider">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="RuleEngineId" Name="ruleEngineId" Type="Object" />
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
