<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/EntityFields.ascx" TagName="EntityFields"
    TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:EntityFields ID="EntityFields1" runat="server" DisplayedEntityLevel="Organization" />
</asp:Content>
