<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="CustomFieldsFormTestPage" Codebehind="CustomFieldsForm.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    Manage the CustomFieldsForm control
    <table>
        <tr>
            <td align="right">
                Select a entity&nbsp;
            </td>
            <td>
                <mits:ComboBox ID="ComboBox1" runat="server" DataTextField="Name" DataValueField="Id"
                    Required="true" ValidationGroup="g1">
                </mits:ComboBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                LocalEntityId&nbsp;
            </td>
            <td>
                <mits:TextBox ID="TextBox1" runat="server" Columns="40" Required="true" ValidationGroup="g1">
                </mits:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Set" ValidationGroup="g1" OnClick="Button1_Click" />
            </td>
        </tr>
    </table>
    <br />
    <asp:Label ID="Label1" runat="server" Visible="false" Text="There is no custom fields for the selected entity"></asp:Label>
    <mits:CustomFieldsForm ID="CustomFieldsForm1" runat="server" Width="400px" ColorScheme="Green"
        ShowCloseButton="false" OnAction="CustomFieldsForm1_Action"></mits:CustomFieldsForm>
</asp:Content>
