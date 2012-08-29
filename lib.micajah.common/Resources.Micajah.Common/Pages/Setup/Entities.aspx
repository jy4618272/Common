<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" EnableViewState="false" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Setup/Entities.ascx" TagName="Entities"
    TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:Entities id="Entities1" runat="server" />
</asp:Content>
