<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SetupControls.FrameworkControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<asp:Label ID="TitleLabel3" runat="server" Font-Bold="true" />
<div style="padding-top: 7px;">
    <asp:Label ID="DbInfoLabel" runat="server" />
    <asp:PlaceHolder ID="DbNeedUpgradePanel" runat="server">&nbsp;<asp:Label ID="DbNeedUpgradeLabel"
        runat="server" />&nbsp;<asp:Label ID="DbNeedUpgradeNotesLabel" runat="server" /><br />
        <asp:LinkButton ID="UpgradeLink" runat="server" Visible="false" OnClick="UpgradeLink_Click" />
    </asp:PlaceHolder>
</div>
<div style="padding-top: 7px;">
    <mits:CommonGridView ID="AssemblyList" runat="server" AutoGenerateColumns="False"
        Width="960px">
        <columns>
                <mits:TextBoxField DataField="Name" />
                <mits:TextBoxField DataField="Version" HeaderStyle-Width="90px" />
                <mits:TextBoxField DataField="Culture" HeaderStyle-Width="250px" />
                <mits:TextBoxField DataField="PublicKeyToken" HeaderStyle-Width="110px" />
            </columns>
    </mits:CommonGridView>
</div>
