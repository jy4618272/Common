<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.UserLdapInfoControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <contenttemplate>
        <mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="OrganizationId,LoginId" Width="550px" OnDataBound="EditForm_DataBound">
            <fields>
                <mits:TextBoxField DataField="FirstName" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" />
                <mits:TextBoxField DataField="LastName" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
                <mits:TextBoxField DataField="LdapDomain" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
                <mits:TextBoxField DataField="LdapDomainFull" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
                <mits:TextBoxField DataField="LdapUserAlias" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
                <mits:TextBoxField DataField="LdapUPN" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
                <mits:TextBoxField DataField="LdapSecurityId" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
                <mits:TextBoxField DataField="LdapUserId" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
                <mits:TextBoxField DataField="LdapOUPath" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
                <mits:TextBoxField DataField="PrimaryEmail" MaxLength="255" Columns="65" ControlStyle-Width="350px" />
                <mits:TextBoxField DataField="SecondaryEmails" Columns="65" ControlStyle-Width="350px" />
                <mits:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="OrgMembershipCount" runat="server"></asp:Label>
                    </ItemTemplate>
                </mits:TemplateField>                
            </fields>
        </mits:MagicForm>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" SelectMethod="GetUserLdapInfo" TypeName="Micajah.Common.Bll.Providers.LoginProvider" UpdateMethod="UpdateUserLdapInfo" OnSelecting="EntityDataSource_Selecting" OnUpdating="EntityDataSource_Updating">
            <UpdateParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:Parameter Name="loginId" Type="Object" />
                <asp:Parameter Name="firstName" Type="String" />
                <asp:Parameter Name="lastName" Type="String" />
                <asp:Parameter Name="ldapDomain" Type="String" />
                <asp:Parameter Name="ldapDomainFull" Type="String" />
                <asp:Parameter Name="ldapUserAlias" Type="String" />
                <asp:Parameter Name="ldapUPN" Type="String" />
                <asp:Parameter Name="ldapSecurityId" Type="String" />
                <asp:Parameter Name="ldapUserId" Type="Object" />
                <asp:Parameter Name="ldapOUPath" Type="String" />
                <asp:Parameter Name="secondaryEmails" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:Parameter Name="loginId" Type="Object" />
                <asp:Parameter Name="includeEmails" Type="Boolean" DefaultValue="true" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </contenttemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="GetUserGroupsUpdatePanel" runat="server">
    <contenttemplate>
        <br />
        <br />
        <asp:LinkButton ID="GetUserGroupsButton" Font-Size="Medium" runat="server" OnClick="GetUserGroupsButton_Click" />
        <br />
        <br />
        <asp:MultiView ID="GetUserGroupsMultiView" runat="server">
            <asp:View ID="GetUserGroupsViewProcess" runat="server">
                <asp:Image ID="GetUserGroupsViewProcessImage" runat="server" />
                <span style="color: Gray; font-family: Arial; font-size: 18px; font-weight: bold; padding: 3px 8px 3px 8px;">
                    <asp:Literal ID="GetUserGroupsViewProcessLiteral" runat="server" />
                </span>                
            </asp:View>
            <asp:View ID="GetUserGroupsViewError" runat="server">
                <span style="color: #000066; background-color: #FFEFAC; font-family: Arial; font-size: 18px; font-weight: bold; padding: 3px 8px 3px 8px;">
                    <asp:Literal ID="GetUserGroupsViewErrorLiteral" runat="server" Text="Error" />
                </span>
                <br />
            </asp:View>
            <asp:View ID="GetUserGroupsViewResult" runat="server">
                <mits:CommonGridView ID="UserGroupsCommonGridView" runat="server" Width="420px" AllowPaging="False" OnRowDataBound="UserGroupsCommonGridView_RowDataBound" AllowSorting="True" AutoGenerateColumns="false">
                    <columns>
                        <mits:TextBoxField DataField="LdapGroupName"/>
                        <mits:TextBoxField DataField="GroupName"/>
                    </columns>
                </mits:CommonGridView>
                <asp:Label ID="UserGroupsNoteLabel" runat="server" />
            </asp:View>
        </asp:MultiView>
        <asp:Timer ID="GetUserGroupsTimer" runat="server" Interval="15000" OnTick="GetUserGroupsTimer_Tick" Enabled="false" />
    </contenttemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="ClearUserLdapInfoUpdatePanel" runat="server">
    <contenttemplate>
        <br />
        <asp:LinkButton ID="ClearUserLdapInfoButton" Font-Size="Medium" runat="server" OnClick="ClearLdapInfoButton_Click" />
        <br />
        <br />
        <mits:UpdateProgress ID="ClearUserLdapInfoUpdateProgress" runat="server" AssociatedUpdatePanelID="ClearUserLdapInfoUpdatePanel" />
    </contenttemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="ReconnectUserToLdapUpdatePanel" runat="server">
    <contenttemplate>
        <br />
        <asp:LinkButton ID="ReconnectUserToLdapButton" Font-Size="Medium" runat="server" OnClick="ReconnectUserToLdapButton_Click" />
        <br />
        <br />
        <asp:MultiView ID="ReconnectUserToLdapMultiView" runat="server">
            <asp:View ID="ReconnectUserToLdapViewProcess" runat="server">
                <asp:Image ID="ReconnectUserToLdapViewProcessImage" runat="server" />
                <span style="color: Gray; font-family: Arial; font-size: 18px; font-weight: bold; padding: 3px 8px 3px 8px;">
                    <asp:Literal ID="ReconnectUserToLdapViewProcessLiteral" runat="server" />
                </span>                
            </asp:View>
            <asp:View ID="ReconnectUserToLdapViewError" runat="server">
                <span style="color: #000066; background-color: #FFEFAC; font-family: Arial; font-size: 18px; font-weight: bold; padding: 3px 8px 3px 8px;">
                    <asp:Literal ID="ReconnectUserToLdapViewErrorLiteral" runat="server" Text="Error" />
                </span>
                <br />
            </asp:View>
            <asp:View ID="ReconnectUserToLdapViewResult" runat="server">                
            </asp:View>
        </asp:MultiView>
        <asp:Timer ID="ReconnectUserToLdapTimer" runat="server" Interval="15000" OnTick="ReconnectUserToLdapTimer_Tick" Enabled="false" />
    </contenttemplate>
</asp:UpdatePanel>
