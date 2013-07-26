<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.GrantAccessControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<div style="position: absolute; top: 50%; height: 220px; margin-top: -125px; width: 90%; text-align: center;">
    <div id="ErrorDiv" runat="server" class="ErrorMessage" style="text-align: center;">
        <div id="JavascriptDisabled" style="padding-bottom: 5px;">
            <asp:Label ID="JavascriptDisabledLabel" runat="server" />
        </div>
        <asp:Panel ID="ConsumerWarningPanel" runat="server" Visible="true" Style="padding-bottom: 5px;">
            <asp:Label ID="ConsumerWarningLabel" runat="server" />
        </asp:Panel>
    </div>
    <asp:MultiView ID="MainMultiView" runat="server" ActiveViewIndex="0">
        <asp:View runat="server">
            <p>
                <asp:Label ID="ConsumerLabel" runat="server" Font-Bold="true" />
            </p>
            <div id="ResponseButtonsDiv" style="display: none;">
                <p>
                    <asp:Label ID="AllowLabel" runat="server" Font-Bold="true" />
                </p>
                <asp:LinkButton ID="AllowAccessButton" runat="server" CssClass="Large" Font-Bold="true"
                    OnClick="AllowAccessButton_Click" />&nbsp;&nbsp;&nbsp;
                <asp:LinkButton ID="DenyAccessButton" runat="server" CssClass="Large" Font-Bold="true"
                    OnClick="DenyAccessButton_Click" />
                <br />
                <br />
                <p>
                    <asp:Label ID="RevokeLabel" runat="server" />
                </p>
            </div>
            <asp:HiddenField runat="server" ID="OAuthAuthorizationSecToken" EnableViewState="false" />
        </asp:View>
        <asp:View runat="server">
            <p>
                <asp:Label ID="AuthorizationGrantedLabel" runat="server" Font-Bold="true" />
            </p>
            <asp:MultiView runat="server" ID="VerifierMultiView" ActiveViewIndex="0">
                <asp:View runat="server">
                    <p>
                        <asp:Label ID="VerificationCodeLabel" runat="server" />
                    </p>
                </asp:View>
                <asp:View runat="server">
                    <p>
                        <asp:Label ID="CloseLabel" runat="server" />
                    </p>
                </asp:View>
            </asp:MultiView>
        </asp:View>
        <asp:View runat="server">
            <p>
                <asp:Label ID="AuthorizationDeniedLabel" runat="server" Font-Bold="true" />
            </p>
        </asp:View>
    </asp:MultiView>
    <script type="text/javascript">
        //<![CDATA[
        var elem = document.getElementById('ResponseButtonsDiv')
        if (elem) elem.style.display = 'block';
        elem = document.getElementById('JavascriptDisabled')
        if (elem) elem.style.display = 'none';
        if (document.location !== window.top.location)
            window.top.location = document.location;
        //]]>
    </script>
</div>
