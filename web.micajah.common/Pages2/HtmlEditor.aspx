<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="HtmlEditorTestPage" Codebehind="HtmlEditor.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <p>
        <b>The HTML editor</b></p>
    <mits:HtmlEditor ID="HtmlEditor1" runat="server" Required="true" ValidatorInitialValue="initial value"
        ErrorMessage="Please enter another value">
        <Content>initial value</Content>
    </mits:HtmlEditor>
    <p>
        <b>Lite text editor</b></p>
    <mits:TextEditor ID="TextEditor1" runat="server">
    </mits:TextEditor>
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
</asp:Content>
