<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/EntityFieldListValues.ascx" TagName="EntityFieldListValues" TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:EntityFieldListValues id="Values" runat="server" />
</asp:Content>
