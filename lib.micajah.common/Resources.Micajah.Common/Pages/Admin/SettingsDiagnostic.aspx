<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/SettingsDiagnostic.ascx" TagName="SettingsDiagnostic"
    TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:SettingsDiagnostic id="Stgs" runat="server" />
</asp:Content>
