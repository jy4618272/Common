<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" EnableViewState="false" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Setup/RoleEdit.ascx" TagName="RoleEdit"
    TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:RoleEdit id="RoleEdit1" runat="server" />
</asp:Content>
