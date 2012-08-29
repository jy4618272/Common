<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~//MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/Instances.ascx" TagName="Instances"
    TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:Instances id="Instances1" runat="server" />
</asp:Content>
