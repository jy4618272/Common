<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="NoticeMessageBox.aspx.cs" Inherits="NoticeMessageBoxTestPage" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <mits:NoticeMessageBox ID="NoticeMessageBox1" runat="server" Size="Small">
    </mits:NoticeMessageBox>
    <h1>
        Manage the NoticeMessageBox control
    </h1>
    <table>
        <tr>
            <td align="right">
                Message&nbsp;
            </td>
            <td>
                <asp:TextBox ID="TextBox1" runat="server" Columns="40">An error occured.</asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" valign="top">
                Description&nbsp;
            </td>
            <td>
                <asp:TextBox ID="TextBox2" runat="server" Columns="40" Rows="5" TextMode="MultiLine">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin vulputate, sapien quis fermentum luctus, libero.</asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                Type of the message&nbsp;
            </td>
            <td>
                <asp:DropDownList ID="DropDownList1" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                Size&nbsp;
            </td>
            <td>
                <asp:DropDownList ID="DropDownList2" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                Width&nbsp;
            </td>
            <td>
                <asp:TextBox ID="TextBox3" runat="server" Columns="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Set" OnClick="Button1_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
