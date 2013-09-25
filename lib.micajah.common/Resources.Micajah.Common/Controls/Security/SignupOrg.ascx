<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.SignupOrganizationControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" TagPrefix="telerik" %>

<div id="Step1Panel" runat="server">
    <div class="Logo">
        <asp:Image ID="LogoImage1" runat="server" />
    </div>
    <div style="padding-top: 20px;">
        <div id="Step1Form" runat="server">
            <div class="row">
                <asp:Label ID="OrganizationNameLabel1" runat="server" CssClass="Large Title"></asp:Label>
                <small class="left">
                    <asp:Literal ID="OrganizationNameHelpText1" runat="server"></asp:Literal></small>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <mits:TextBox ID="OrganizationName1" runat="server" Required="true" ValidationGroup="Step1" CssClass="Large" TabIndex="1" AutoPostBack="true" OnTextChanged="OrganizationName1_TextChanged" />
                        <asp:Image ID="OrganizationNameTick1" runat="server" Visible="false" ImageAlign="AbsMiddle" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="clear"></div>
            <div class="row">
                <asp:Label ID="EmailLabel1" runat="server" CssClass="Large Title"></asp:Label>
                <small></small>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <mits:TextBox ID="Email1" runat="server" Required="true" ValidationGroup="Step1" CssClass="Large" TabIndex="2" AutoPostBack="true" OnTextChanged="Email1_TextChanged" />
                        <asp:Image ID="EmailTick1" runat="server" Visible="false" ImageAlign="AbsMiddle" />
                        <asp:CustomValidator ID="EmailValidator1" runat="server" Display="Dynamic" EnableClientScript="false" ValidationGroup="Step1" CssClass="Error" OnServerValidate="EmailValidator1_ServerValidate" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="clear"></div>
            <div id="OrganizationUrlRow" runat="server" class="row orgurl">
                <asp:Label ID="OrganizationUrlLabel" runat="server" CssClass="Large Title"></asp:Label>
                <small></small>
                <asp:UpdatePanel ID="UpdatePanelOrganizationUrl" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="LightBlue">
                            <div style="float: left; line-height: 48px;">
                                <asp:Literal ID="Schema" runat="server"></asp:Literal>
                            </div>
                            <div style="float: right; line-height: 48px;">
                                <asp:Literal ID="PartialCustomUrlRootAddress" runat="server"></asp:Literal>
                            </div>
                            <div style="padding: 0 125px 0 60px;">
                                <mits:TextBox ID="OrganizationUrl" runat="server" ValidationGroup="Step1" MaxLength="1024" CssClass="Large" TabIndex="3" AutoPostBack="true" OnTextChanged="OrganizationUrl_TextChanged" />
                            </div>
                        </div>
                        <asp:Image ID="OrganizationUrlTick" runat="server" Visible="false" ImageAlign="AbsMiddle" Style="margin-top: 15px;" />
                        <asp:CustomValidator ID="OrganizationUrlValidator" runat="server" Display="Dynamic" EnableClientScript="false" ValidationGroup="Step1" CssClass="Error" Style="margin-top: 8px;" OnServerValidate="OrganizationUrlValidator_ServerValidate" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="clear"></div>
            <div class="row">
                <asp:Button ID="Step1Button" runat="server" CssClass="Green Large" TabIndex="4" ValidationGroup="Step1" OnClick="Step1Button_Click"></asp:Button>
            </div>
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
<div id="Step2Panel" runat="server">
    <div class="Logo">
        <asp:Image ID="LogoImage2" runat="server" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" RenderMode="Inline" UpdateMode="Always">
        <ContentTemplate>
            <div id="Step2Form" runat="server">
                <div class="row">
                    <asp:Label ID="OrganizationNameLabel2" runat="server" CssClass="Large Title"></asp:Label>
                    <small></small>
                    <mits:TextBox ID="OrganizationName2" runat="server" Required="true" ValidationGroup="Step2"
                        AutoPostBack="true" OnTextChanged="OrganizationName1_TextChanged" />
                    <asp:Image ID="OrganizationNameTick2" runat="server" Visible="false" ImageAlign="AbsMiddle" />
                </div>
                <div class="row">
                    <asp:Label ID="HowYouHearAboutUsLabel" runat="server" CssClass="Large Title"></asp:Label>
                    <small></small>
                    <mits:TextBox ID="HowYouHearAboutUs" runat="server" ValidationGroup="Step2" MaxLength="255" />
                </div>
                <div class="clear"></div>
                <div class="title">
                    <asp:Literal ID="PersonalInformationLabel" runat="server"></asp:Literal>
                </div>
                <div class="row">
                    <asp:Label ID="FirstNameLabel" runat="server" CssClass="Large Title"></asp:Label>
                    <small></small>
                    <mits:TextBox ID="FirstName" runat="server" Required="true" ValidationGroup="Step2"
                        MaxLength="255" />
                </div>
                <div class="row">
                    <asp:Label ID="LastNameLabel" runat="server" CssClass="Large Title"></asp:Label>
                    <small></small>
                    <mits:TextBox ID="LastName" runat="server" Required="true" ValidationGroup="Step2" MaxLength="255" />
                </div>
                <div class="clear"></div>
                <div class="title">
                    <asp:Literal ID="LocalSettingsLabel" runat="server"></asp:Literal>
                </div>
                <div class="clear"></div>
                <div class="row">
                    <asp:Label ID="TimeZoneLabel" runat="server" CssClass="Large Title"></asp:Label>
                    <small></small>
                    <asp:DropDownList ID="TimeZoneList" runat="server" ValidationGroup="Step2">
                    </asp:DropDownList>
                </div>
                <div class="clear"></div>
                <div class="row">
                    <asp:Label ID="CurrencyLabel" runat="server" CssClass="Large Title"></asp:Label>
                    <small></small>
                    <asp:DropDownList ID="CurrencyList" runat="server" ValidationGroup="Step2">
                    </asp:DropDownList>
                </div>
                <div class="clear"></div>
                <div id="EmailAndPasswordGroupRow" runat="server">
                    <div class="title">
                        <asp:Literal ID="EmailAndPasswordLabel" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="row" id="Email2Row" runat="server">
                    <asp:Label ID="EmailLabel2" runat="server" CssClass="Large Title"></asp:Label>
                    <small></small>
                    <mits:TextBox ID="Email2" runat="server" Required="true" ValidationGroup="Step2" AutoPostBack="true" OnTextChanged="Email1_TextChanged" />
                    <asp:Image ID="EmailTick2" runat="server" Visible="false" ImageAlign="AbsMiddle" />
                    <asp:CustomValidator ID="EmailValidator2" runat="server" Display="Dynamic" EnableClientScript="false" ValidationGroup="Step2" CssClass="Error" OnServerValidate="EmailValidator1_ServerValidate" />
                </div>
                <div class="clear"></div>
                <div class="row" id="PasswordRow" runat="server">
                    <asp:Label ID="PasswordLabel" runat="server" CssClass="Large Title"></asp:Label>
                    <small></small>
                    <mits:TextBox ID="Password" runat="server" Required="true" ValidationGroup="Step2" TextMode="Password" />
                    <asp:CustomValidator ID="PasswordValidator" runat="server" Display="Dynamic" CssClass="Error" ValidationGroup="Step2" ValidateEmptyText="true" OnServerValidate="PasswordValidator_ServerValidate" />
                </div>
                <div class="clear"></div>
                <div class="row" id="ConfirmPasswordRow" runat="server">
                    <asp:Label ID="ConfirmPasswordLabel" runat="server" CssClass="Large Title"></asp:Label>
                    <small></small>
                    <mits:TextBox ID="ConfirmPassword" runat="server" Required="true" ValidationGroup="Step2" TextMode="Password" />
                    <asp:CustomValidator ID="PasswordCompareValidator" runat="server" Display="Dynamic" ValidationGroup="Step2" CssClass="Error" ClientValidationFunction="PasswordCompareValidation" />
                </div>
                <div class="clear"></div>
                <div class="row">
                    <asp:Label ID="CaptchaLabel" runat="server" CssClass="Large Title"></asp:Label>
                    <small></small>
                    <telerik:RadCaptcha id="Captcha1" runat="server" captchaimage-textlength="4" captchatextboxlabel="" captchatextboxcssclass="" errormessage="" validationgroup="Step2" />
                    <asp:CustomValidator ID="CaptchaValidator" runat="server" Display="Dynamic" EnableClientScript="false" ValidationGroup="Step2" CssClass="Error" OnServerValidate="CaptchaValidator_ServerValidate" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="Step2FormButton" runat="server">
        <asp:ValidationSummary ID="Step2ValidationSummary" runat="server" CssClass="Error Block Summary" DisplayMode="BulletList" ShowSummary="true" ValidationGroup="Step2" />
        <div style="padding-top: 15px; width: 93%; clear: both;" align="center">
            <asp:Button ID="Step2Button" runat="server" CssClass="Green X-Large" ValidationGroup="Step2" OnClick="Step2Button_Click"></asp:Button>
        </div>
    </div>
    <div class="Agree" style="width: 93%;">
        <asp:Literal ID="AgreeLabel" runat="server"></asp:Literal>
    </div>
</div>
<div id="Step3Panel" runat="server" style="margin-top: 100px;">
    <div>
        <div class="Mp_Dm">
            <h1>
                <asp:Literal ID="CustomizeLiteral" runat="server"></asp:Literal></h1>
            <asp:Repeater ID="InstanceList" runat="server" DataSourceID="InstanceListDataSource">
                <HeaderTemplate>
                    <ul id="InstanceList">
                </HeaderTemplate>
                <ItemTemplate>
                    <li onclick="SelectItem(this, '<%# Eval("InstanceId") %>');" class='<%# Convert.ToString(Eval("InstanceId")) == SelectedInstance.Text ? "Cbc" : "Cb" %>'
                        style="cursor: pointer;"><a>
                            <h2>
                                <%# Eval("Name") %></h2>
                        </a>
                        <p>
                            <%# Eval("Description") %>
                        </p>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul><br />
                </FooterTemplate>
            </asp:Repeater>
            <div id="Step3Form" runat="server">
                <asp:Button ID="Step3Button" runat="server" CssClass="Green Large" ValidationGroup="Step3"
                    Style="float: left;" OnClick="Step3Button_Click"></asp:Button>
                <asp:CustomValidator ID="InstanceRequiredValidator" runat="server" Display="Dynamic" ValidationGroup="Step3"
                    CssClass="Error Step3Val" ClientValidationFunction="InstanceRequiredValidation" OnServerValidate="InstanceRequiredValidator_ServerValidate" /><asp:CustomValidator
                        ID="UniqueDataValidator" runat="server" Display="Dynamic" EnableClientScript="false"
                        ValidationGroup="Step3" CssClass="Error Step3Val" OnServerValidate="UniqueDataValidator_ServerValidate" /><asp:TextBox
                            ID="SelectedInstance" runat="server" ValidationGroup="Step3" Style="display: none;"></asp:TextBox>
            </div>
        </div>
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
<asp:ObjectDataSource ID="InstanceListDataSource" runat="server" SelectMethod="GetTemplateInstances"
    TypeName="Micajah.Common.Bll.Providers.InstanceProvider"></asp:ObjectDataSource>
