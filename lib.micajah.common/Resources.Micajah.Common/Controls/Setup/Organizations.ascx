<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SetupControls.OrganizationsControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="SearchPanel" runat="server" Style="padding-bottom: 7px;">
            <table border="0">
                <tr>
                    <td>
                        <mits:TextBox ID="OrganizationName" runat="server" Columns="40" />
                    </td>
                    <td>
                        <mits:ComboBox ID="ActiveValueList" runat="server" />
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:Button ID="SearchButton" Text="Search" runat="server" OnClick="SearchButton_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <mits:CommonGridView ID="List" runat="server" DataKeyNames="OrganizationId" DataSourceID="EntityListDataSource"
            Width="960px" OnRowDataBound="List_RowDataBound" OnRowCommand="List_RowCommand"
            OnDataBound="List_DataBound">
            <columns>
                <asp:TemplateField SortExpression="Name" ItemStyle-Width="25%">
                    <ItemTemplate>
                        <%# GetHyperlink(Eval("WebSiteUrl").ToString(), Eval("Name").ToString(), Eval("Description").ToString())%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="DatabaseServerFullName" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <%# GetValidatedValue(Eval("DatabaseServerFullName"), Eval("DatabaseId")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="DatabaseName" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <%# GetValidatedValue(Eval("DatabaseName"), Eval("DatabaseId")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <mits:CheckBoxField DataField="Active" SortExpression="Active" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                <mits:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                         <asp:LinkButton ID="UpdateActiveButton" runat="server" CausesValidation="false"></asp:LinkButton>
                    </ItemTemplate>
                </mits:TemplateField>
            </columns>
        </mits:CommonGridView>
        <mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="OrganizationId"
            Width="550px" OnDataBound="EditForm_DataBound">
            <fields>
                <mits:TextBoxField DataField="Name" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" Required="True" />
                <mits:TextBoxField DataField="Description" MaxLength="1024" Columns="65" ControlStyle-Width="350px" Rows="3"
                    TextMode="MultiLine" />
                <mits:TextBoxField DataField="WebSiteUrl" MaxLength="2048" Columns="65" ControlStyle-Width="350px" ValidationType="RegularExpression"
                    ValidationExpression="http(s)?://([\w-]+(\.)?)+[\w-]+(/[\w- ./?%&=]*)?" />
                <mits:TemplateField>
                    <ItemTemplate>
                        <mits:ImageUpload ID="LogoImageUpload" runat="server" LocalObjectType="OrganizationLogo" 
                            LocalObjectId='<%# Eval("OrganizationId", "{0:N}") %>' />
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField PaddingLeft="false">
                    <ItemTemplate>
                        <mits:ComboBox ID="DatabaseList" runat="server" DataSourceID="DatabaseListDataSource" 
                            DataTextField="FullName" DataValueField="DatabaseId" Width="350px" 
                            Required="True" ValidatorInitialValue="x" ValidationGroup="<%# EditForm.ClientID %>">
                        </mits:ComboBox>
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TextBoxField DataField="AdminEmail" MaxLength="255" Columns="65" ControlStyle-Width="350px" Required="True"
                    ValidationType="RegularExpression" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                <mits:DatePickerField DataField="ExpirationTime" />
                <mits:TextBoxField DataField="GraceDays" MaxLength="9" Columns="9" ValidationType="Integer" />
                <mits:DatePickerField DataField="CanceledTime" />
                <mits:CheckBoxField DataField="Active" DefaultChecked="True" />
                <mits:CheckBoxField DataField="Trial" DefaultChecked="False" />
                <mits:TemplateField PaddingLeft="false">
                    <ItemTemplate>
                        <%# Eval("CreatedTime") %>
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField>
                    <ItemTemplate>
                        <div id="ErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </ItemTemplate>
                </mits:TemplateField>
            </fields>
        </mits:MagicForm>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" SelectMethod="GetOrganizations"
            TypeName="Micajah.Common.Bll.Providers.OrganizationProvider">
            <SelectParameters>
                <asp:Parameter Name="includeAdditionalInfo" Type="Boolean" DefaultValue="True" />
                <asp:ControlParameter Name="name" Type="String" ConvertEmptyStringToNull="false"
                    ControlID="OrganizationName" PropertyName="Text" />
                <asp:ControlParameter Name="statusId" Type="Int32" ConvertEmptyStringToNull="true"
                    ControlID="ActiveValueList" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" DeleteMethod="DeleteOrganization"
            InsertMethod="InsertOrganization" SelectMethod="GetOrganization" TypeName="Micajah.Common.Bll.Providers.OrganizationProvider"
            UpdateMethod="UpdateOrganization" OnInserting="EntityDataSource_Inserting" OnUpdating="EntityDataSource_Inserting"
            OnInserted="EntityDataSource_Inserted" OnSelected="EntityDataSource_Selected">
            <DeleteParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="webSiteUrl" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="databaseId" Type="Object" />
                <asp:Parameter Name="fiscalYearStartMonth" Type="Int32" />
                <asp:Parameter Name="fiscalYearStartDay" Type="Int32" />
                <asp:Parameter Name="weekStartsDay" Type="Int32" />
                <asp:Parameter Name="expirationTime" Type="DateTime" />
                <asp:Parameter Name="graceDays" Type="Int32" DefaultValue="0" />
                <asp:Parameter Name="active" Type="Boolean" />
                <asp:Parameter Name="canceledTime" Type="DateTime" />
                <asp:Parameter Name="trial" Type="Boolean" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="organizationId"
                    Type="Object" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="webSiteUrl" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="databaseId" Type="Object" />
                <asp:Parameter Name="adminEmail" Type="String" />
                <asp:Parameter Name="expirationTime" Type="DateTime" />
                <asp:Parameter Name="graceDays" Type="Int32" DefaultValue="0" />
                <asp:Parameter Name="active" Type="Boolean" />
                <asp:Parameter Name="canceledTime" Type="DateTime" />
                <asp:Parameter Name="trial" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="DatabaseListDataSource" runat="server" SelectMethod="GetPublicDatabases"
            TypeName="Micajah.Common.Bll.Providers.DatabaseProvider">
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="organizationId"
                    Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
