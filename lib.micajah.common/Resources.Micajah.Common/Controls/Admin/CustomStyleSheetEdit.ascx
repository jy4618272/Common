<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.CustomStyleSheetEditControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<asp:Table ID="FormTable" runat="server" Width="99%" Height="90%">
    <asp:TableHeaderRow ID="TableHeaderRow1" runat="server" TableSection="TableHeader">
        <asp:TableHeaderCell ID="TableHeaderCell1" runat="server">
            <asp:Literal ID="CaptionLiteral" runat="server"></asp:Literal>
        </asp:TableHeaderCell>
    </asp:TableHeaderRow>
    <asp:TableRow runat="server" Height="100%">
        <asp:TableCell runat="server" Height="100%" Style="padding-bottom: 7px;">
            <asp:TextBox ID="StyleSheetText" runat="server" Height="100%" Width="100%" Rows="20" TextMode="MultiLine"></asp:TextBox>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableFooterRow ID="FooterRow" runat="server">
        <asp:TableCell runat="server">
            <asp:Button ID="UpdateButton" runat="server" OnClick="UpdateButton_Click" />
            <asp:PlaceHolder ID="ButtonsSeparator" runat="server" />
            <asp:HyperLink ID="CancelLink" runat="server" CssClass="Cancel" />
        </asp:TableCell>
    </asp:TableFooterRow>
</asp:Table>
