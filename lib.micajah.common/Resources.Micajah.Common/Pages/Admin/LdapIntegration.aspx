<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>
<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/LdapIntegration.ascx" TagName="LdapIntegrationMenu" TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:LdapIntegrationMenu id="Ldap1" runat="server" />
</asp:Content>
