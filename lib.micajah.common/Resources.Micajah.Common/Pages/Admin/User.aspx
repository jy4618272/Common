<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/Users.ascx" TagName="Users" TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:Users id="Usr" runat="server" />
</asp:Content>
