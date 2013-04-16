<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DateRange.aspx.cs" Inherits="DateRangeTestPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <mits:DateRange ID="DateRange1" runat="server" Required="True" />
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" Text="Submit" />
    <br />
    <br />
    <b>Selected start date:</b>&nbsp;
    <asp:Label ID="Label1" runat="server" />
    <br />
    <b>Selected end date:</b>&nbsp;
    <asp:Label ID="Label2" runat="server" />
</asp:Content>
