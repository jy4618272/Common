<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.StartControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<style type="text/css">
    .buttonAlt
    {
        color: #28597a;
        padding: 7px;
        font-size: 1.2em;
        text-decoration: none;
        font-size: 12px;
    }
    .buttonAlt:hover, .buttonAlt:focus
    {
        background-color: #28597a;
        color: white;
        font-size: 12px;
    }
</style>
<table cellpadding="0" cellspacing="0">
    <td style="vertical-align: top;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <mits:detailmenu id="StartMenu" runat="server" onitemdatabound="StartMenu_ItemDataBound"
                    onitemclick="StartMenu_ItemClick" Theme="Modern">
                </mits:detailmenu>
                <div style="margin: 16px 0 0 45px;">
                    <asp:LinkButton ID="HideLink" runat="server" CssClass="buttonAlt" OnClick="HideLink_Click"></asp:LinkButton>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </td>
    <td style="padding: 50px 0 0 20px; vertical-align: top;">
        <div id="VideoDiv" runat="server" style="border: 1px solid #e2e2e2; padding: 2px;">
        </div>
    </td>
</table>
