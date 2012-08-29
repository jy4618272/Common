<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Setup/Databases.ascx" TagName="Databases"
    TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:Databases id="Databases1" runat="server" />
</asp:Content>
