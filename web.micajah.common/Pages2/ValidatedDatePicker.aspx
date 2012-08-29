<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ValidatedDatePicker.aspx.cs" Inherits="DatePickerTestPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <mits:DatePicker ID="ValidatedDatePicker1" runat="server" Required="True" />
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" Text="Submit" />
    <br />
    <br />
    <b>Selected date:</b>&nbsp;
    <asp:Label ID="Label1" runat="server" />
</asp:Content>
