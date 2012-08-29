<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>
<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/LdapGroupMappings.ascx" TagName="LdapGroupMappings" TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:LdapGroupMappings id="Ldap1" runat="server" />
</asp:Content>

