<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.LdapIntegrationControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <contenttemplate>
        <mits:DetailMenu ID="IntegrationMenu" runat="server" Visible="True" />
    </contenttemplate>
</asp:UpdatePanel>
