<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RadioButtonList.aspx.cs" Inherits="RadioButtonListTestPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <mits:RadioButtonList ID="ValidatedRadioButtonList1" runat="server" Required="True"
        ErrorMessage="You must select an item" DataSourceID="ObjectDataSource1" DataTextField="Name"
        DataValueField="RoleId">
    </mits:RadioButtonList>
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Button1_Click" />
    <br />
    <br />
    <b>Selected item:</b>&nbsp;<asp:Label ID="Label1" runat="server" Text="None" />
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetRoles"
        TypeName="Micajah.Common.Bll.Providers.RoleProvider"></asp:ObjectDataSource>
</asp:Content>
