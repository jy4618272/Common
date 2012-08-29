<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/NodeType.ascx" TagName="NodeType"
    TagPrefix="uc" %>
    
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:NodeType id="NodeType1" runat="server" />
</asp:Content>
