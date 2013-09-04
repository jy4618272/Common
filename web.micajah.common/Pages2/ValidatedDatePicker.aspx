<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="DatePickerTestPage" Codebehind="ValidatedDatePicker.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <mits:DatePicker ID="ValidatedDatePicker1" runat="server" Required="True" Type="DateTimePicker" />
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" Text="Submit" />
    <br />
    <br />
    <b>Selected date:</b>&nbsp;
    <asp:Label ID="Label1" runat="server" />
</asp:Content>
