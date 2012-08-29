<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>
<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/UserLdapInfo.ascx" TagName="UserLdapSettings" TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:UserLdapSettings id="Ldap1" runat="server" />
</asp:Content>

