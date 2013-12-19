<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>
<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/LdapLogs.ascx" TagName="LdapLogs" TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:LdapLogs id="Ldap1" runat="server" />
</asp:Content>

