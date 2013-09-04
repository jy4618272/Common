<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="ValidatedComboBoxTestPage" Codebehind="ValidatedComboBox.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <h1>
        ComboBox</h1>
    <mits:ComboBox ID="ValidatedComboBox1" runat="server" Required="True" ErrorMessage="You must select an item"
        ValidatorInitialValue="0" DataSourceID="ObjectDataSource1" DataTextField="Name"
        DataValueField="RoleId" OnDataBound="ValidatedComboBox1_DataBound" Width="250px">
    </mits:ComboBox>
    <br />
    <br />
    <b>Selected item:</b>&nbsp;<asp:Label ID="Label1" runat="server" Text="None" />
    <p>
        &nbsp;
    </p>
    <h1>
        Telerik RadComboBox that looks like ComboBox</h1>
    <telerik:RadComboBox ID="RadComboBox1" runat="server" DataSourceID="ObjectDataSource1"
        DataTextField="Name" DataValueField="RoleId">
        <ItemTemplate>
            <asp:CheckBox runat="server" ID="chkToDoListTemplate" Checked="false" />
            <asp:Label runat="server" ID="lblToDoListTemplateName" AssociatedControlID="chkToDoListTemplate"
                Text='<%# Eval("Name") %>'></asp:Label>
        </ItemTemplate>
    </telerik:RadComboBox>
    <p>
        &nbsp;
    </p>
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetRoles"
        TypeName="Micajah.Common.Bll.Providers.RoleProvider"></asp:ObjectDataSource>
</asp:Content>
