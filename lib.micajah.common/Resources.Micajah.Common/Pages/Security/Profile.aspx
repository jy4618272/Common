<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Security/Profile.ascx" TagName="Profile"
    TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:Profile id="Prf" runat="server" />
</asp:Content>
