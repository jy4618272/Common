<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="BwdUserSide.aspx.cs" Inherits="BwdUserSidePage" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>

<%@ MasterType TypeName="Micajah.Common.Pages.MasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <mits:Hello ID="Hello1" runat="server">
    </mits:Hello>
    <mits:DetailMenu ID="DetailMenu1" runat="server" Theme="Reflective">
    </mits:DetailMenu>
</asp:Content>
