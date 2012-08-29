<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/OrganizationProfile.ascx"
    TagName="OrganizationProfile" TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:OrganizationProfile id="Op" runat="server" />
</asp:Content>
