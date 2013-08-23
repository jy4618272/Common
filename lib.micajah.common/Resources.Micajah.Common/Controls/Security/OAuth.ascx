<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.OAuthControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<div style="position: absolute; top: 50%; height: 250px; margin-top: -125px; width: 90%; text-align: center;">
    <div class="Caption" style="text-align: center; padding-bottom: 15px;">
        <asp:Literal ID="TitleLiteral" runat="server"></asp:Literal>
    </div>
    <asp:MultiView ID="MainMultiView" runat="server" ActiveViewIndex="0">
        <asp:View runat="server">
            <p style="font-weight: bold;">
                <asp:Literal ID="ConsumerLiteral" runat="server" />
            </p>
            <div style="padding-top: 15px;">
                <asp:LinkButton ID="DenyAccessButton" runat="server" CssClass="Large Button" OnClick="DenyAccessButton_Click" />&nbsp;&nbsp;&nbsp;
                <asp:LinkButton ID="AllowAccessButton" runat="server" CssClass="Large Button Green" OnClick="AllowAccessButton_Click" />
                <br />
                <br />
                <p style="max-width: 600px; margin: 0 auto; padding-top: 15px;">
                    <asp:Literal ID="RevokeLiteral" runat="server" />
                </p>
            </div>
            <asp:HiddenField runat="server" ID="OAuthAuthorizationSecToken" EnableViewState="false" />
        </asp:View>
        <asp:View runat="server">
            <p style="font-weight: bold;">
                <asp:Literal ID="AuthorizationGrantedLiteral" runat="server" />
            </p>
            <asp:MultiView runat="server" ID="VerifierMultiView" ActiveViewIndex="0">
                <asp:View runat="server">
                    <p>
                        <asp:Literal ID="VerificationCodeLiteral" runat="server" />
                    </p>
                </asp:View>
                <asp:View runat="server">
                    <p>
                        <asp:Literal ID="CloseLiteral" runat="server" />
                    </p>
                </asp:View>
            </asp:MultiView>
        </asp:View>
        <asp:View runat="server">
            <p style="font-weight: bold;">
                <asp:Literal ID="AuthorizationDeniedLiteral" runat="server" />
            </p>
        </asp:View>
    </asp:MultiView>
</div>
