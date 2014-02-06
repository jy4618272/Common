<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.SignupOrganizationControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" TagPrefix="telerik" %>

<div id="Step1Panel">
    <div class="Logo">
        <asp:Image ID="LogoImage1" runat="server" />
    </div>
    <div style="padding-top: 20px;">
        <div id="Step1Form">
            <div class="row">
                <asp:Label ID="OrganizationNameLabel1" runat="server" CssClass="Large Title"></asp:Label>
                <small>
                    <asp:Literal ID="OrganizationNameHelpText1" runat="server"></asp:Literal></small>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <contenttemplate>
                        <mits:TextBox ID="OrganizationName1" runat="server" Required="true" ValidationGroup="Step1" CssClass="Large" TabIndex="1" AutoPostBack="true" OnTextChanged="OrganizationName1_TextChanged" />
                        <asp:Image ID="OrganizationNameTick1" runat="server" Visible="false" ImageAlign="AbsMiddle" />
                    </contenttemplate>
                </asp:UpdatePanel>
            </div>
            <div class="clear"></div>
            <div class="row">
                <asp:Label ID="EmailLabel1" runat="server" CssClass="Large Title"></asp:Label>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <contenttemplate>
                        <mits:TextBox ID="Email1" runat="server" Required="true" ValidationGroup="Step1" CssClass="Large" TabIndex="2" AutoPostBack="true" OnTextChanged="Email1_TextChanged" />
                        <asp:Image ID="EmailTick1" runat="server" Visible="false" ImageAlign="AbsMiddle" />
                        <asp:CustomValidator ID="EmailValidator1" runat="server" Display="Dynamic" EnableClientScript="false" ValidationGroup="Step1" CssClass="Error" OnServerValidate="EmailValidator1_ServerValidate" />
                    </contenttemplate>
                </asp:UpdatePanel>
            </div>
            <div class="clear"></div>
            <div id="OrganizationUrlRow" runat="server" class="row">
                <asp:Label ID="OrganizationUrlLabel" runat="server" CssClass="Large Title"></asp:Label>
                <asp:UpdatePanel ID="UpdatePanelOrganizationUrl" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <contenttemplate>
                        <div class="LightBlue">
                            <div class="Left">
                                <asp:Literal ID="Schema" runat="server"></asp:Literal>
                            </div>
                            <div class="Right">
                                <asp:Literal ID="PartialCustomUrlRootAddress" runat="server"></asp:Literal>
                            </div>
                            <div class="OrgUrl">
                                <mits:TextBox ID="OrganizationUrl" runat="server" ValidationGroup="Step1" MaxLength="1024" CssClass="Large" TabIndex="3" AutoPostBack="true" OnTextChanged="OrganizationUrl_TextChanged" />
                            </div>
                        </div>
                        <asp:Image ID="OrganizationUrlTick" runat="server" Visible="false" ImageAlign="AbsMiddle" CssClass="LightBlueImg" />
                        <asp:CustomValidator ID="OrganizationUrlValidator" runat="server" Display="Dynamic" EnableClientScript="false" ValidationGroup="Step1" CssClass="Error" Style="margin-top: 8px;" OnServerValidate="OrganizationUrlValidator_ServerValidate" />
                    </contenttemplate>
                </asp:UpdatePanel>
            </div>
            <div class="clear"></div>
            <div class="row">
                <asp:Label ID="CaptchaLabel" runat="server" CssClass="Large Title"></asp:Label>
                <telerik:RadCaptcha id="Captcha1" runat="server" ImageStorageLocation="Session" MinTimeout="15" CaptchaMaxTimeout="30" captchaimage-textlength="4" captchatextboxlabel="" captchatextboxcssclass="" ErrorMessage="" ValidationGroup="Step1" />
                <asp:CustomValidator ID="CaptchaValidator" runat="server" Display="Dynamic" EnableClientScript="false" ValidationGroup="Step1" CssClass="Error" OnServerValidate="CaptchaValidator_ServerValidate" />
            </div>
            <div class="clear"></div>
            <div class="row">
                <asp:Button ID="Step1Button" runat="server" CssClass="Green Large" TabIndex="4" CausesValidation="true" ValidationGroup="Step1" OnClick="Step1Button_Click"></asp:Button>
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
            <asp:Button ID="ModalStep1Button" runat="server" CssClass="Green Large" ValidationGroup="ModalWindow" OnClick="ModalStep1Button_Click"></asp:Button>
        </p>
    </div>
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
