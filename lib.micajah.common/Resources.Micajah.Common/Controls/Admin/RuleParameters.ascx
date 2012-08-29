<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.RuleParametersControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:MagicForm ID="FormRoleView" runat="server" DataKeyNames="RuleId" Width="550px"
            DataSourceID="ObjectDataSourceRuleView" OnDataBound="FormRuleView_DataBound">
            <fields>
                <mits:TextBoxField DataField="Name" HeaderStyle-Width="120px" />
                <mits:TextBoxField DataField="DisplayName" />
                <mits:TextBoxField DataField="UsedQty" />
                <mits:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="LabelUserName" runat="server"></asp:Label>
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="LabelUserDate" runat="server"></asp:Label>
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField>
                    <ItemTemplate>
                        <%# Micajah.Common.Bll.Providers.RuleEngineProvider.GetDisplayUserName((Guid)Eval("CreatedBy"), (Guid)Eval("OrganizationId"))%>
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TextBoxField DataField="CreatedDate" />
                <mits:TextBoxField DataField="Active" />
            </fields>
        </mits:MagicForm>
        <br />
        <mits:CommonGridView ID="GridViewParameters" runat="server" DataKeyNames="RuleParameterId"
            DataSourceID="ObjectDataSourceRuleParameterList" OnAction="GridViewParameters_Action"
            Width="700px">
            <headerstyle wrap="true" />
            <columns>
                <mits:TemplateField HeaderText="Entity Name">
                    <ItemTemplate>
                        <%# Micajah.Common.Bll.Providers.EntityNodeProvider.GetEntityNodeType((Guid)Eval("EntityNodeTypeId")).Name %>
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TextBoxField DataField="FieldName" />
                <mits:TextBoxField DataField="TypeName" />
                <mits:TextBoxField DataField="Term" />
                <mits:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="LabelValue" runat="server" Text='<%# (((string)Eval("TypeName")) == "Lookup" || ((string)Eval("TypeName")) == "Entity") ? Micajah.Common.Bll.Providers.EntityNodeProvider.GetEntityValueAndName((Guid)Eval("EntityNodeTypeId"), (string)Eval("FieldName"), Eval("Value")) : Eval("Value") %>'></asp:Label>
                    </ItemTemplate>
                </mits:TemplateField>
            </columns>
        </mits:CommonGridView>
        <asp:Label ID="LabelRuleEngineId" runat="server" Visible="false"></asp:Label>
        <table cellpadding="0" cellspacing="0" id="TableEditParameter" runat="server" visible="false"
            width="700px">
            <tr>
                <td class="Cgv_Cpt" style="color: #333333;">
                    <asp:Literal ID="RuleParameterCaption" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="Cgv_T" cellspacing="0" cellpadding="0" style="border: 1px solid #333333;
                        border-collapse: collapse;">
                        <tr class="Cgv_H">
                            <th scope="col" style="border: 1px solid #333333;">
                                <asp:Literal ID="literal1" runat="server"></asp:Literal>
                            </th>
                            <th scope="col" style="border: 1px solid #333333;">
                                <asp:Literal ID="literal2" runat="server"></asp:Literal>
                            </th>
                            <th scope="col" style="border: 1px solid #333333;">
                                <asp:Literal ID="literal3" runat="server"></asp:Literal>
                            </th>
                            <th scope="col" style="border: 1px solid #333333;">
                                <asp:Literal ID="literal4" runat="server"></asp:Literal>
                            </th>
                            <th scope="col" style="border: 1px solid #333333;">
                                <asp:Literal ID="literal5" runat="server"></asp:Literal>
                            </th>
                            <th scope="col" style="border: 1px solid #333333; width: 90px;">
                                &nbsp;
                            </th>
                        </tr>
                        <tr>
                            <td class="Cgv_C">
                                <asp:DropDownList ID="DropDownListEntityTypes" runat="server" DataSourceID="ObjectDataSourceEntityTypesList"
                                    DataTextField="Name" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="DropDownListEntityTypes_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="Cgv_C">
                                <asp:DropDownList ID="DropDownListEntityFields" runat="server" DataSourceID="ObjectDataSourceEntityFieldsList"
                                    AutoPostBack="true" DataTextField="Name" DataValueField="Name" OnSelectedIndexChanged="DropDownListEntityFields_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="Cgv_C">
                                <asp:Label ID="LabelFieldType" runat="server"></asp:Label>
                                <asp:Label ID="LabelFieldsTypeFullName" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td class="Cgv_C">
                                <asp:DropDownList ID="DropDownListTerm" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td class="Cgv_C">
                                <mits:TextBox ID="TextBoxEntityValue" runat="server" Required="true" Visible="false" />
                                <asp:DropDownList ID="ComboBoxEntityValue" runat="server" DataTextField="Key" DataValueField="Value"
                                    Required="true" Visible="false" />
                                <mits:DatePicker ID="DatePickerEntityValue" runat="server" Required="true" Visible="false" />
                                <mits:CheckBox ID="CheckBoxEntityValue" runat="server" Visible="false" />
                                <mits:EntityTreeView ID="EntityTreeViewValue" runat="server" ComboBoxMode="true"
                                    EnableContextMenu="false" Visible="false">
                                </mits:EntityTreeView>
                            </td>
                            <td class="Cgv_C">
                                &nbsp;
                                <asp:LinkButton ID="LinkButtonUpdateParameter" runat="server" OnClick="LinkButtonUpdateParameter_Click"></asp:LinkButton>&nbsp;&nbsp;
                                <asp:LinkButton ID="LinkButtonCancelParameter" runat="server" OnClick="LinkButtonCancelParameter_Click"
                                    CausesValidation="false"></asp:LinkButton>&nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:ObjectDataSource ID="ObjectDataSourceRuleView" runat="server" SelectMethod="GetRuleRow"
            TypeName="Micajah.Common.Bll.Providers.RuleEngineProvider">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="RuleId" Type="Object" Name="ruleId" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSourceRuleParameterList" runat="server" DeleteMethod="DeleteRuleParameter"
            SelectMethod="GetRuleParameters" TypeName="Micajah.Common.Bll.Providers.RuleEngineProvider">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="RuleId" Type="Object" Name="ruleId" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSourceRuleParameter" runat="server" InsertMethod="InsertRuleParameter"
            SelectMethod="GetRuleParameterRow" TypeName="Micajah.Common.Bll.Providers.RuleEngineProvider"
            UpdateMethod="UpdateRuleParameter">
            <UpdateParameters>
                <asp:Parameter Type="Object" Name="ruleParameterId" />
                <asp:Parameter Type="Object" Name="ruleId" />
                <asp:Parameter Type="Object" Name="entityNodeId" />
                <asp:Parameter Name="isInputParameter" Type="Boolean" />
                <asp:Parameter Name="isEntity" Type="Boolean" />
                <asp:Parameter Name="fieldName" Type="String" />
                <asp:Parameter Name="fullName" Type="String" />
                <asp:Parameter Name="typeName" Type="String" />
                <asp:Parameter Name="term" Type="String" />
                <asp:Parameter Name="value" Type="Object" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="GridViewParameters" Type="Object" Name="ruleParameterId"
                    PropertyName="SelectedValue" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Type="Object" Name="ruleParameterId" />
                <asp:Parameter Type="Object" Name="ruleId" />
                <asp:Parameter Type="Object" Name="entityNodeId" />
                <asp:Parameter Name="isInputParameter" Type="Boolean" />
                <asp:Parameter Name="isEntity" Type="Boolean" />
                <asp:Parameter Name="fieldName" Type="String" />
                <asp:Parameter Name="fullName" Type="String" />
                <asp:Parameter Name="typeName" Type="String" />
                <asp:Parameter Name="term" Type="String" />
                <asp:Parameter Name="value" Type="Object" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSourceEntityTypesList" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetEntityTypes" TypeName="Micajah.Common.Bll.Providers.RuleEngineProvider">
            <SelectParameters>
                <asp:ControlParameter ControlID="LabelRuleEngineId" Type="Object" Name="rulesEngineId"
                    PropertyName="Text" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSourceEntityFieldsList" runat="server" SelectMethod="GetEntityFields"
            TypeName="Micajah.Common.Bll.Providers.EntityNodeProvider">
            <SelectParameters>
                <asp:ControlParameter ControlID="DropDownListEntityTypes" Type="Object" PropertyName="SelectedValue"
                    Name="entityTypeId" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
