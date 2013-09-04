<%@ Page Language="C#" AutoEventWireup="true" Inherits="Pages1_Schedule"
    MasterPageFile="~/MasterPage.master" Codebehind="Schedule.aspx.cs" %>

<%@ MasterType TypeName="Micajah.Common.Pages.MasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    
    <asp:Label ID="Label1" runat="server" Text="None"></asp:Label><br />
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Rule Execute" />
</asp:Content>
