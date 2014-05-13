<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.SettingsControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<asp:PlaceHolder ID="PageContent" runat="server">
    <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
        <ItemTemplate>
            <div id="ControlHolder1" runat="server" class="Mc_Settings">
            </div>
            <asp:Repeater ID="Repeater2" runat="server" OnItemDataBound="Repeater2_ItemDataBound">
                <HeaderTemplate>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="Mc_Settings">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="Tc2">
                            <asp:PlaceHolder ID="ControlHolder2" runat="server"></asp:PlaceHolder>
                        </td>
                        <td class="Tc2" style="width: 100%;">
                            <asp:Label ID="NameLabel2" runat="server" CssClass="Nm"></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr>
                        <td class="Tc1">
                            <asp:PlaceHolder ID="ControlHolder2" runat="server"></asp:PlaceHolder>
                        </td>
                        <td class="Tc1">
                            <asp:Label ID="NameLabel2" runat="server" CssClass="Nm"></asp:Label>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </ItemTemplate>
    </asp:Repeater>
    <div id="CommandBar" runat="server" class="Mf_B" style="border: none;">
        <asp:Button ID="UpdateButton" runat="server" OnClick="UpdateButton_Click" />
        <asp:PlaceHolder ID="ButtonsSeparator" runat="server" Visible="false" />
        <asp:HyperLink ID="CancelLink" runat="server" CssClass="Mf_Cb" Visible="false" />
    </div>
</asp:PlaceHolder>
<mits:DetailMenu id="DetailMenu1" runat="server" Theme="SideBySide" />