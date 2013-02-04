<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.LogOnControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<div class="Mp_Hdr">
    <div class="A">
        <asp:HyperLink ID="HeaderLeftLogoLink" runat="server"></asp:HyperLink>
    </div>
    <div class="G">
        <asp:HyperLink ID="HeaderRightLogoLink" runat="server"></asp:HyperLink>
    </div>
</div>
<div id="MainContainer" runat="server" style="position: absolute; top: 50%; height: 220px;
    margin-top: -110px; width: 100%; z-index: 500001;">
    <table id="FormTable" class="FormTable" runat="server" align="center" cellspacing="10">
        <tr id="TitleContainer" runat="server" class="Caption">
            <td colspan="2" style="text-align: center;">
                <asp:Image ID="LogoImage" runat="server" /><asp:Label ID="TitleLabel" runat="server"></asp:Label>
                <div id="ErrorDiv" runat="server" class="ErrorMessage" style="text-align: center;
                    padding: 5px 0 5px 0;" />
            </td>
        </tr>
        <tr>
            <td style="padding-top: 12px; text-align: right; width: 110px;">
                <asp:Label ID="LoginLabel" runat="server" CssClass="Large" />
            </td>
            <td style="*width: 335px;">
                <mits:TextBox id="LoginTextBox" runat="server" columns="32" CssClass="Large" tabindex="1" />
            </td>
        </tr>
        <tr>
            <td style="padding-top: 12px; text-align: right;">
                <asp:Label ID="PasswordLabel" runat="server" CssClass="Large" />
            </td>
            <td>
                <mits:TextBox id="PasswordTextBox" runat="server" columns="32" CssClass="Large" tabindex="2" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="Pl">
                            <asp:Button ID="LogOnButton" runat="server" TabIndex="3" CssClass="Btn" />
                            <asp:HyperLink ID="LogOnViaGoogleLink" runat="server" TabIndex="4" CssClass="Return Pl" />
                        </td>
                        <td align="right" style="vertical-align: middle;">
                            <asp:LinkButton ID="PasswordRecoveryButton" runat="server" TabIndex="5" CssClass="Return"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <table id="SignupUserTable" runat="server" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="Caption" style="padding-top: 50px;">
                            <asp:Label ID="SignupUserTitleLabel" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="SignupUserButton" runat="server" TabIndex="7" CssClass="Large" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div id="LinkEmailPanel" runat="server" style="text-align: center;">
        <p>
            <asp:Label ID="LinkEmailLabel" runat="server" Font-Bold="true" /></p>
        <asp:LinkButton ID="LinkEmailButton" runat="server" CssClass="Large" />&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="CancelLinkEmailButton" runat="server" CssClass="Large" />
        <br />
        <br />
        <p>
            <asp:Label ID="OrLabel1" runat="server" Font-Bold="true" />&nbsp;
            <asp:LinkButton ID="LogOffLink" runat="server" OnClick="LogOffLink_Click" />
        </p>
    </div>
</div>
