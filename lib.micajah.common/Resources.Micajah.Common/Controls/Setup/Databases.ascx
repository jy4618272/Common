<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SetupControls.DatabasesControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:CommonGridView ID="List" runat="server" DataKeyNames="DatabaseId" DataSourceID="EntityListDataSource"
            Width="960px">
            <columns>
                <mits:TextBoxField DataField="Name" SortExpression="Name" ItemStyle-Width="25%" />
                <mits:TextBoxField DataField="DatabaseServerFullName" SortExpression="DatabaseServerFullName" ItemStyle-Width="25%" />
                <mits:TextBoxField DataField="Description" ItemStyle-Width="50%" />
            </columns>
        </mits:CommonGridView>
        <mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="DatabaseId"
            Width="550px" Visible="False" OnItemInserting="EditForm_ItemInserting" OnItemUpdating="EditForm_ItemUpdating">
            <fields>
                <mits:TextBoxField DataField="Name" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" Required="True" />
                <mits:TextBoxField DataField="Description" MaxLength="1024" Columns="65" ControlStyle-Width="350px" Rows="3"
                    TextMode="MultiLine" />
                <mits:TextBoxField DataField="UserName" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
                <mits:TextBoxField DataField="Password" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
                <mits:ComboBoxField DataField="DatabaseServerId" DataSourceId="DatabaseServerListDataSource" 
                    DataTextField="FullName" DataValueField="DatabaseServerId" ControlStyle-Width="350px" />
                <mits:CheckBoxField DataField="Private" />
                <mits:TemplateField>
                    <itemtemplate>
                        <div id="ErrorPanel" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </itemtemplate>
                </mits:TemplateField>
            </fields>
        </mits:MagicForm>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" DeleteMethod="DeleteDatabase"
            SelectMethod="GetDatabases" TypeName="Micajah.Common.Bll.Providers.DatabaseProvider">
            <SelectParameters>
                <asp:Parameter Name="includeAdditionalInfo" Type="Boolean" DefaultValue="True" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="databaseId" Type="Object" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" InsertMethod="InsertDatabase"
            SelectMethod="GetDatabaseRow" TypeName="Micajah.Common.Bll.Providers.DatabaseProvider"
            UpdateMethod="UpdateDatabase">
            <UpdateParameters>
                <asp:Parameter Name="databaseId" Type="Object" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="userName" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="password" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="Private" Type="Boolean" />
                <asp:Parameter Name="databaseServerId" Type="Object" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="databaseId"
                    Type="Object" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="userName" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="password" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="Private" Type="Boolean" />
                <asp:Parameter Name="databaseServerId" Type="Object" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="DatabaseServerListDataSource" runat="server" SelectMethod="GetDatabaseServers"
            TypeName="Micajah.Common.Bll.Providers.DatabaseServerProvider"></asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
