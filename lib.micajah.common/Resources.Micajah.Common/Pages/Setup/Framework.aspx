<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" EnableViewState="false" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Setup/Framework.ascx" TagName="Framework"
    TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:Framework id="Framework1" runat="server" />
</asp:Content>
