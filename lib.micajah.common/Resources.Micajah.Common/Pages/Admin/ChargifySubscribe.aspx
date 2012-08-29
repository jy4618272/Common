<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/ChargifySubscribe.ascx" TagName="ChargifySubscribe" TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:ChargifySubscribe id="ucChargifySubscribe" runat="server" />
</asp:Content>
