<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="SmallerIcons.aspx.cs" Inherits="SmallerIconsTestPage" %>

<%@ Register Src="~/Controls/EmbeddedIcons.ascx" TagName="EmbeddedIcons" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <uc1:EmbeddedIcons ID="EmbeddedIcons1" runat="server" IconSize="Smaller" />
</asp:Content>
