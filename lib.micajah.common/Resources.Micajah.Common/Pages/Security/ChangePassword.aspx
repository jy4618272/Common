<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Security/ChangePassword.ascx"
    TagName="ChangePassword" TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:ChangePassword ID="PwdForm" runat="server" ValidateCurrentPassword="true" />
</asp:Content>
