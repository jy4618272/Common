<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UpdateProgress.aspx.cs" Inherits="UpdateProgressPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:TextBox ID="FreeText" runat="server" TextMode="MultiLine" Columns="50" Rows="5"></asp:TextBox>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="SubmitButton" runat="server" Text="Send Response" OnClick="SubmitButton_Click" />
                    </td>
                    <td>
                        <asp:CheckBox ID="GenerateTimeout" runat="server" Text="Generate time-out" AutoPostBack="true" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="padding: 10px 0 30px 0;">
        <mits:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        </mits:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <b>The last transmitted text is:</b><br />
            <asp:Label ID="FreeTextLabel" runat="server"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="padding-top: 30px;">
        <p style="font-size: large;">
            The test links to the UpdateProgress3 page with the different value of the "p3"
            parameter:</p>
        <a href="UpdateProgress.aspx?p3=1">UpdateProgress.aspx?p3=1</a><br />
        <a href="UpdateProgress.aspx?p3=2">UpdateProgress.aspx?p3=2</a><br />
        <a href="UpdateProgress.aspx?p3=xxx">UpdateProgress.aspx?p3=xxx</a>
    </div>
</asp:Content>
