<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SetupControls.DatabaseServersControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:CommonGridView ID="List" runat="server" DataKeyNames="DatabaseServerId" DataSourceID="EntityListDataSource"
            Width="960px">
            <columns>
                <mits:TextBoxField DataField="FullName" SortExpression="FullName" ItemStyle-Width="25%" />
                <mits:TextBoxField DataField="Description" ItemStyle-Width="75%" />
            </columns>
        </mits:CommonGridView>
        <mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="DatabaseServerId"
            Width="550px" Visible="False">
            <fields>
                <mits:TextBoxField DataField="Name" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" Required="True" />
                <mits:TextBoxField DataField="InstanceName" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
                <mits:TextBoxField DataField="Description" MaxLength="1024" Columns="65" ControlStyle-Width="350px" Rows="3"
                    TextMode="MultiLine" />
                <mits:TextBoxField DataField="Port" MaxLength="10" Columns="10" ValidationType="Integer" 
                    MinimumValue="0" />
                <mits:ComboBoxField DataField="WebSiteId" DataSourceId="WebSiteListDataSource" 
                    DataTextField="Name" DataValueField="WebSiteId" ControlStyle-Width="350px" />
            </fields>
        </mits:MagicForm>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" DeleteMethod="DeleteDatabaseServer"
            SelectMethod="GetDatabaseServers" TypeName="Micajah.Common.Bll.Providers.DatabaseServerProvider">
            <DeleteParameters>
                <asp:Parameter Name="databaseServerId" Type="Object" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" InsertMethod="InsertDatabaseServer"
            SelectMethod="GetDatabaseServerRow" TypeName="Micajah.Common.Bll.Providers.DatabaseServerProvider"
            UpdateMethod="UpdateDatabaseServer">
            <UpdateParameters>
                <asp:Parameter Name="databaseServerId" Type="Object" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="instanceName" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="port" Type="Int32" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="websiteId" Type="Object" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="databaseServerId"
                    Type="Object" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="instanceName" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="port" Type="Int32" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="websiteId" Type="Object" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="WebsiteListDataSource" runat="server" SelectMethod="GetWebsites"
            TypeName="Micajah.Common.Bll.Providers.WebsiteProvider"></asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
