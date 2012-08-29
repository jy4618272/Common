<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    ValidateRequest="false" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/CustomStyleSheetEdit.ascx" TagName="CustomStyleSheetEdit"
    TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:CustomStyleSheetEdit id="Csse" runat="server" />
</asp:Content>
