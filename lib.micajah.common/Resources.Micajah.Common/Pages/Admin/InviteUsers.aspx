<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Async="true" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/InviteUsers.ascx" TagName="InviteUsers" TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:InviteUsers id="Usr" runat="server" />
</asp:Content>
