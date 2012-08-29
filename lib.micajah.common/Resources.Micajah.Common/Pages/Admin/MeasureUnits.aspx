<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/MeasureUnits.ascx" TagName="MeasureUnitsControl" TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:MeasureUnitsControl id="MeasureUnits" runat="server" />
</asp:Content>
