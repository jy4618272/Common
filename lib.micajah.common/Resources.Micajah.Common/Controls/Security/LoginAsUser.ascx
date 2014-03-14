<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.LogOnAsUserControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <contenttemplate>
        <asp:Table ID="SearchTable" runat="server" Width="550px">
            <asp:TableHeaderRow TableSection="TableHeader">
                <asp:TableHeaderCell runat="server" ColumnSpan="2">
                    <asp:Literal ID="CaptionLiteral" runat="server"></asp:Literal>
                </asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell Width="120px">
                    <asp:Label ID="OrganizationListLabel" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <mits:ComboBox ID="OrganizationList" runat="server" Required="True" DataSourceID="OrganizationsDataSource"
                        DataTextField="Name" DataValueField="OrganizationId" OnDataBound="ComboBox_DataBound"
                        Width="100%" AutoPostBack="true" CausesValidation="false">
                    </mits:ComboBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="InstanceListRow" runat="server">
                <asp:TableCell>
                    <asp:Label ID="InstanceListLabel" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <mits:ComboBox ID="InstanceList" runat="server" DataSourceID="InstancesDataSource"
                        DataTextField="Name" DataValueField="InstanceId" OnDataBound="ComboBox_DataBound"
                        Width="100%" AutoPostBack="true" CausesValidation="false">
                    </mits:ComboBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="RoleListLabel" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <mits:ComboBox ID="RoleList" runat="server" Required="True" DataSourceID="RolesDataSource"
                        DataTextField="Name" DataValueField="RoleId" Width="100%" AutoPostBack="true"
                        CausesValidation="false" OnSelectedIndexChanged="RoleList_SelectedIndexChanged">
                    </mits:ComboBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                </asp:TableCell>
                <asp:TableCell>
                    <div id="ErrorPanel" runat="server" class="Error Block" enableviewstate="false"
                        visible="false">
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableFooterRow>
                <asp:TableCell ID="FooterCell" runat="server" ColumnSpan="2">
                    <asp:PlaceHolder ID="ButtonsHolder" runat="server">
                        <asp:Button ID="SubmitButton" runat="server" OnClick="SubmitButton_Click" />
                        <asp:PlaceHolder ID="ButtonsSeparator" runat="server" />
                        <asp:LinkButton ID="CancelButton" runat="server" CssClass="Cancel" CausesValidation="false"
                            OnClick="CancelButton_Click" />
                    </asp:PlaceHolder>
                </asp:TableCell>
            </asp:TableFooterRow>
        </asp:Table>
        <br />
        <mits:CommonGridView ID="List" runat="server" DataKeyNames="UserId" Width="700px"
            Visible="False" EnableSelect="True" OnAction="List_Action" AutoGenerateColumns="False" AllowSorting="True"
            DataSourceID="EntityListDataSource" PageSize="50" OnRowDataBound="List_RowDataBound">
            <CaptionControls>
                <asp:LinkButton ID="InjectButton" runat="server" Visible="false" OnClick="InjectButton_Click"></asp:LinkButton>
            </CaptionControls>
            <columns>
                <mits:TextBoxField DataField="Name" SortExpression="Name" />
                <mits:TextBoxField DataField="Email" SortExpression="Email" />
                <mits:TemplateField SortExpression="LastLoginDate" HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Literal ID="LastLoginDateLiteral" runat="server"></asp:Literal>
                    </ItemTemplate>
                </mits:TemplateField>
            </columns>
        </mits:CommonGridView>
        <asp:ObjectDataSource ID="OrganizationsDataSource" runat="server" SelectMethod="GetOrganizations"
            TypeName="Micajah.Common.Bll.Providers.OrganizationProvider">
            <SelectParameters>
                <asp:Parameter Name="includeExpired" Type="Boolean" DefaultValue="false" />
                <asp:Parameter Name="includeInactive" Type="Boolean" DefaultValue="false" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="InstancesDataSource" runat="server" SelectMethod="GetInstances"
            TypeName="Micajah.Common.Bll.Providers.InstanceProvider" OnSelecting="InstancesDataSource_Selecting"
            OnSelected="InstancesDataSource_Selected">
            <SelectParameters>
                <asp:ControlParameter ControlID="OrganizationList" PropertyName="SelectedValue" Name="organizationId"
                    Type="Object" />
                <asp:Parameter Name="includeInactive" Type="Boolean" DefaultValue="false" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="RolesDataSource" runat="server" SelectMethod="GetAvailableRoles"
            TypeName="Micajah.Common.Bll.Providers.RoleProvider">
            <SelectParameters>
                <asp:Parameter Name="includeInstanceAdministratorRole" Type="Boolean" DefaultValue="true" />
                <asp:Parameter Name="includeOrganizationAdministratorRole" Type="Boolean" DefaultValue="true" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" SelectMethod="GetUsersDataView"
            TypeName="Micajah.Common.Bll.Providers.UserProvider" OnSelecting="EntityListDataSource_Selecting"
            OnSelected="EntityListDataSource_Selected">
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:Parameter Name="instanceId" Type="Object" />
                <asp:Parameter Name="roleId" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </contenttemplate>
</asp:UpdatePanel>
