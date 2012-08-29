<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.UserOrganizationControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:Table ID="MagicFormTable" runat="server" Width="550px">
    <asp:TableRow>
        <asp:TableCell>
            <div style="overflow: auto;">
                <mits:EntityTreeView runat="server" ID="Tree" CheckBoxes="True" EntityId="4cda22f3-4f01-4768-8608-938dc6a06825" />
            </div>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableFooterRow>
        <asp:TableCell>
            <asp:Button ID="SaveButton" runat="server" OnClick="SaveButton_Click" />
            <asp:PlaceHolder ID="ButtonsSeparator" runat="server" />
            <asp:LinkButton ID="CancelButton" runat="server" CssClass="Cancel" OnClick="CancelButton_Click" />
        </asp:TableCell>
    </asp:TableFooterRow>
</asp:Table>
