<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Setup/DatabaseServers.ascx" TagName="DatabaseServers"
    TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:DatabaseServers id="DatabaseServers1" runat="server" />
</asp:Content>
