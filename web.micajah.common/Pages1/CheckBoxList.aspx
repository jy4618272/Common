<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="CheckBoxListTestPage" Codebehind="CheckBoxList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <mits:CheckBoxList ID="ValidatedCheckBoxList1" runat="server" ErrorMessage="You must select an item"
        Required="True" DataSourceID="ObjectDataSource1" DataTextField="Name" DataValueField="RoleId" />
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
    <br />
    <br />
    <b>Selected items:</b>&nbsp;<asp:Label ID="Label1" runat="server" Text="None" />
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetRoles"
        TypeName="Micajah.Common.Bll.Providers.RoleProvider"></asp:ObjectDataSource>
    <br />
    <br />
    <mits:CheckBox ID="CheckBox1" runat="server" RenderingMode="OnOffSwitch" />
    <br />
    <br />
    <mits:CheckBox ID="CheckBox2" runat="server" RenderingMode="OnOffSwitch" />
</asp:Content>
