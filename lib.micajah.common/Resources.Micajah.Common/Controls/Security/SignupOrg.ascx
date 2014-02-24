<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.SignupOrganizationControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:UpdatePanel ID="UpdatePanel" runat="server">
    <contenttemplate>
        <div class="Logo">
            <asp:Image ID="LogoImage" runat="server" />
        </div>
        <div style="padding-top: 20px;">
            <div id="StepForm">
                <div class="row">
                    <asp:Label ID="OrganizationNameLabel" runat="server" CssClass="Large Title"></asp:Label>
                    <small>
                        <asp:Literal ID="OrganizationNameHelpText" runat="server"></asp:Literal></small>
                        <mits:TextBox ID="OrganizationName" runat="server" Required="true" ValidationGroup="MainForm" CssClass="Large" TabIndex="1" AutoPostBack="true" OnTextChanged="OrganizationName_TextChanged" />
                        <asp:Image ID="OrganizationNameTick" runat="server" Visible="false" ImageAlign="AbsMiddle" />
                </div>
                <div class="clear"></div>
                <div class="row">
                    <asp:Label ID="EmailLabel" runat="server" CssClass="Large Title"></asp:Label>                
                    <mits:TextBox ID="Email" runat="server" Required="true" ValidationGroup="MainForm" CssClass="Large" TabIndex="2" AutoPostBack="true" OnTextChanged="Email_TextChanged" />
                    <asp:Image ID="EmailTick" runat="server" Visible="false" ImageAlign="AbsMiddle" />
                    <asp:CustomValidator ID="EmailValidator" runat="server" Display="Dynamic" EnableClientScript="false" ValidationGroup="MainForm" CssClass="Error" OnServerValidate="EmailValidator_ServerValidate" />
                </div>
                <div class="clear"></div>
                <div id="OrganizationUrlRow" runat="server" class="row">
                    <asp:Label ID="OrganizationUrlLabel" runat="server" CssClass="Large Title"></asp:Label>
                    <div class="LightBlue">
                        <div class="Left">
                            <asp:Literal ID="Schema" runat="server"></asp:Literal>
                        </div>
                        <div class="Right">
                            <asp:Literal ID="PartialCustomUrlRootAddress" runat="server"></asp:Literal>
                        </div>
                        <div class="OrgUrl">
                            <mits:TextBox ID="OrganizationUrl" runat="server" ValidationGroup="MainForm" MaxLength="1024" CssClass="Large" TabIndex="3" AutoPostBack="true" OnTextChanged="OrganizationUrl_TextChanged" />
                        </div>
                    </div>
                    <asp:Image ID="OrganizationUrlTick" runat="server" Visible="false" ImageAlign="AbsMiddle" CssClass="LightBlueImg" />
                    <asp:CustomValidator ID="OrganizationUrlValidator" runat="server" Display="Dynamic" EnableClientScript="false" ValidationGroup="MainForm" CssClass="Error" Style="margin-top: 8px;" OnServerValidate="OrganizationUrlValidator_ServerValidate" />
                </div>
                <div class="clear"></div>
                <div class="row">
                    <asp:Label ID="CaptchaLabel" runat="server" CssClass="Large Title"></asp:Label>
                    <telerik:RadCaptcha id="Captcha" runat="server" ImageStorageLocation="Session" captchaimage-textlength="4" captchatextboxlabel="" captchatextboxcssclass="" ErrorMessage="" ValidationGroup="MainForm" />
                    <asp:CustomValidator ID="CaptchaValidator" runat="server" Display="Dynamic" EnableClientScript="false" ValidationGroup="MainForm" CssClass="Error" OnServerValidate="CaptchaValidator_ServerValidate" />
                </div>
                <div class="clear"></div>
                <div class="row">
                    <asp:Button ID="CreateMyAccountButton" runat="server" CssClass="Green Large" TabIndex="4" ValidationGroup="MainForm" OnClick="CreateMyAccountButton_Click"></asp:Button>
                </div>
            </div>
            <div class="Agree">
                <asp:Literal ID="AgreeLabel" runat="server"></asp:Literal>
            </div>
        </div>
        <div id="ModalWindow" class="modal-window" style="display: none;">
            <div id="ModalWindowHeader" runat="server" class="header">
                <h2>
                    <asp:Literal ID="ModalTitleLiteral" runat="server"></asp:Literal></h2>
                <p>
                    <asp:Literal ID="ModalMessageLiteral" runat="server"></asp:Literal>
                </p>
            </div>
            <p>
                <asp:Literal ID="ModalSelectActionLiteral" runat="server"></asp:Literal>
            </p>
            <p>
                <asp:HyperLink ID="ModalLoginLink" runat="server" CssClass="Button Green Large" Target="_parent"></asp:HyperLink>
            </p>
            <p>
                <asp:Literal ID="ModalSelectActionSeparatorLiteral" runat="server"></asp:Literal>
            </p>
            <p>
                <asp:Button ID="CreateMyAccountModalButton" runat="server" CssClass="Green Large" ValidationGroup="ModalWindow" OnClick="CreateMyAccountModalButton_Click"></asp:Button>
            </p>
        </div>
        <div id="ErrorPanel" runat="server">
            <div class="Logo">
                <asp:Image ID="LogoImage3" runat="server" />
            </div>
            <div>
                <div style="padding-top: 20px;">
                    <asp:Label ID="ErrorLabel" runat="server" CssClass="Error Block"></asp:Label>
                </div>
                <div style="padding-top: 10px; text-align: center;" class="Agree">
                    <p>
                        <asp:Label ID="ErrorContinueLabel" runat="server"></asp:Label>
                    </p>
                </div>
                <div style="text-align: center;">
                    <asp:HyperLink ID="ErrorContinueLink" runat="server" CssClass="Button Green Large">
                    </asp:HyperLink>
                </div>
            </div>
        </div>
    </contenttemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="CreateMyAccountButton" />
    </Triggers>
</asp:UpdatePanel>