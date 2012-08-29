<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Security/LoginAsUser.ascx"
    TagName="LoginAsUser" TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:LoginAsUser id="LoginAsUser1" runat="server" />
</asp:Content>
