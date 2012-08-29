<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.SupportControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<div style="position: absolute; top: 50%; height: 200px; margin-top: -100px; width: 100%;
    text-align: center;">
    <asp:PlaceHolder ID="DescriptionHolder" runat="server" Visible="false">
        <asp:Label ID="DescriptionLabel" runat="server" CssClass="ErrorMessage" />
        <br />
        <br />
    </asp:PlaceHolder>
    <asp:Label ID="TitleLabel" runat="server" CssClass="Caption" />
    <br />
    <br />
    <table id="FormTable" runat="server" align="center" cellspacing="10" style="text-align: left;"
        class="FormTable">
        <tr>
            <td style="text-align: right;">
                <asp:Label ID="EmailLabel" runat="server" />
            </td>
            <td>
                <asp:HyperLink ID="EmailLink" runat="server"></asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td style="text-align: right;">
                <asp:Label ID="PhoneLabel" runat="server" />
            </td>
            <td>
                <asp:Literal ID="PhoneValueLabel" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <br />
                <br />
                <asp:HyperLink ID="ReturnBackLink" runat="server" Visible="false" CssClass="Return" />
            </td>
        </tr>
    </table>
</div>
