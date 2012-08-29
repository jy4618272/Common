<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ValidatedComboBox.aspx.cs" Inherits="ValidatedComboBoxTestPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <mits:ComboBox ID="ValidatedComboBox1" runat="server" Required="True" ErrorMessage="You must select an item"
        ValidatorInitialValue="0" DataSourceID="ObjectDataSource1" DataTextField="Name"
        DataValueField="RoleId" OnDataBound="ValidatedComboBox1_DataBound" Width="250px">
    </mits:ComboBox>
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
    <br />
    <br />
    <b>Selected item:</b>&nbsp;<asp:Label ID="Label1" runat="server" Text="None" />
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetRoles"
        TypeName="Micajah.Common.Bll.Providers.RoleProvider"></asp:ObjectDataSource>
</asp:Content>
