﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.LdapGroupMappingsControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<script type="text/javascript">
    //<![CDATA[
    function NodeClicking(sender, eventArgs) {
        if (eventArgs.get_node().get_parent() != sender) eventArgs.set_cancel(true);
    }
    //]]>
</script>
<asp:UpdatePanel ID="FormUpdatePanel" runat="server">
    <ContentTemplate>
        <asp:Table ID="LdapGroupMappingsTable" runat="server" Visible="True" Width="550px">
            <asp:TableHeaderRow runat="server" TableSection="TableHeader">
                <asp:TableHeaderCell ColumnSpan="2" runat="server">
                    <asp:Literal ID="CaptionLiteral" runat="server"></asp:Literal>
                </asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell Width="120px">
                    <asp:Label ID="AddGroupLiteral" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <mits:TreeView ID="AppGroupTreeView" runat="Server" ComboBoxMode="True" DataFieldID="GroupId"
                        DataFieldParentID="ParentGroupId" DataTextField="Name" DataValueField="GroupId"
                        DataSourceID="AppGroupListDataSource" OnClientNodeClicking="NodeClicking" OnNodeDataBound="AppGroupTreeView_NodeDataBound"
                        Width="380px" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="DomainLiteral" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <mits:ComboBox ID="DomainList" runat="server" DataSourceID="DomainListDataSource"
                        DataTextField="DomainName" DataValueField="Id" OnDataBound="DomainList_DataBound"
                        Width="250px" AutoPostBack="true" OnSelectedIndexChanged="DomainList_SelectedIndexChanged"
                        Visible="True" Required="True">
                    </mits:ComboBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="LdapGroupLiteral" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <mits:ComboBox ID="LdapGroupList" runat="server" DataSourceID="LdapGroupListDataSource"
                        DataTextField="GroupName" DataValueField="Id" OnDataBound="LdapGroupList_DataBound"
                        Width="380px" Visible="True" Required="True">
                    </mits:ComboBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableFooterRow>
                <asp:TableCell ID="ButtonHeaderCell" runat="server"></asp:TableCell>
                <asp:TableCell ID="ButtonCell" runat="server">
                    <asp:Button ID="SaveButton" runat="server" OnClick="SaveButton_Click" />
                </asp:TableCell>
            </asp:TableFooterRow>
        </asp:Table>
        <mits:UpdateProgress ID="FormUpdateProgress" runat="server" AssociatedUpdatePanelID="FormUpdatePanel" />
        <asp:ObjectDataSource ID="AppGroupListDataSource" runat="server" SelectMethod="GetAppGroupsWithInstancesAndRoles"
            TypeName="Micajah.Common.Bll.Providers.LdapInfoProvider" OnSelecting="OrganizationIdDataSource_Selecting"
            EnableCaching="False">
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="DomainListDataSource" runat="server" SelectMethod="GetDomainsFromDB"
            TypeName="Micajah.Common.Bll.Providers.LdapInfoProvider" OnSelecting="OrganizationIdDataSource_Selecting"
            EnableCaching="False">
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="LdapGroupListDataSource" runat="server" SelectMethod="GetGroups"
            TypeName="Micajah.Common.Bll.Providers.LdapInfoProvider" OnSelecting="LdapGroupListDataSource_Selecting"
            EnableCaching="False">
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:Parameter Name="domainName" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="GridUpdatePanel" runat="server" RenderMode="Inline">
    <ContentTemplate>
        <mits:CommonGridView ID="CommonGridView1" runat="server" Width="700px" AllowPaging="False"
            AllowSorting="True" AutoGenerateColumns="false" ShowAddLink="False" AutoGenerateDeleteButton="True"
            DataKeyNames="GroupId,LdapDomainId,LdapGroupId" DataSourceID="MappingsListDataSource"
            Visible="True">
            <columns>
                <mits:TextBoxField DataField="GroupName" HeaderText="App Group" SortExpression="GroupName" />
                <mits:TextBoxField DataField="LdapDomainName" HeaderText="Ldap Domain" SortExpression="LdapDomainName" />
                <mits:TextBoxField DataField="LdapGroupName" HeaderText="Ldap Group" SortExpression="LdapGroupName" />
            </columns>
        </mits:CommonGridView>
        <asp:ObjectDataSource ID="MappingsListDataSource" runat="server" DeleteMethod="DeleteGroupMapping"
            SelectMethod="GetGroupMappings" TypeName="Micajah.Common.Bll.Providers.LdapInfoProvider"
            OnSelecting="OrganizationIdDataSource_Selecting" EnableCaching="False">
            <DeleteParameters>
                <asp:Parameter Name="groupId" Type="Object" />
                <asp:Parameter Name="ldapDomainId" Type="Object" />
                <asp:Parameter Name="ldapGroupId" Type="Object" />
            </DeleteParameters>
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
