<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.SignupOrganizationControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" TagPrefix="telerik" %>
<div id="Step1Panel" runat="server">
    <div class="Logo">
        <asp:Image ID="LogoImage1" runat="server" /></div>
    <div style="padding-top: 20px;">
        <table id="Step1Form" runat="server" align="center">
            <tr>
                <td>
                    <asp:Label ID="OrganizationNameLabel1" runat="server" CssClass="Large"></asp:Label>
                    <small>
                        <asp:Literal ID="OrganizationNameHelpText1" runat="server"></asp:Literal></small>
                </td>
                <td style="width: 450px;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <mits:TextBox ID="OrganizationName1" runat="server" Required="true" ValidationGroup="Step1"
                                CssClass="Large" Width="385px" TabIndex="1" AutoPostBack="true" OnTextChanged="OrganizationName1_TextChanged" />
                            <asp:CustomValidator ID="OrganizationNameValidator1" runat="server" Display="Dynamic"
                                EnableClientScript="false" ValidationGroup="Step1" CssClass="Error" OnServerValidate="OrganizationNameValidator1_ServerValidate" />
                            <asp:Image ID="OrganizationNameTick1" runat="server" Visible="false" ImageAlign="AbsMiddle" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="EmailLabel1" runat="server" CssClass="Large"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <mits:TextBox ID="Email1" runat="server" Required="true" ValidationGroup="Step1"
                                CssClass="Large" Width="385px" TabIndex="2" AutoPostBack="true" OnTextChanged="Email1_TextChanged" />
                            <asp:CustomValidator ID="EmailValidator1" runat="server" Display="Dynamic" EnableClientScript="false"
                                ValidationGroup="Step1" CssClass="Error" OnServerValidate="EmailValidator1_ServerValidate" />
                            <asp:Image ID="EmailTick1" runat="server" Visible="false" ImageAlign="AbsMiddle" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="OrganizationUrlRow" runat="server">
                <td>
                    <asp:Label ID="OrganizationUrlLabel" runat="server" CssClass="Large"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelOrganizationUrl" runat="server" RenderMode="Inline"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="LightBlue">
                                <asp:Literal ID="Schema" runat="server"></asp:Literal>
                                <mits:TextBox ID="OrganizationUrl" runat="server" ValidationGroup="Step1" MaxLength="1024"
                                    Width="150px" CssClass="Large" TabIndex="3" AutoPostBack="true" OnTextChanged="OrganizationUrl_TextChanged" />
                                <asp:Literal ID="PartialCustomUrlRootAddress" runat="server"></asp:Literal>
                            </div>
                            <asp:CustomValidator ID="OrganizationUrlValidator" runat="server" Display="Dynamic"
                                EnableClientScript="false" ValidationGroup="Step1" CssClass="Error" Style="margin-top: 8px;"
                                OnServerValidate="OrganizationUrlValidator_ServerValidate" />
                            <asp:Image ID="OrganizationUrlTick" runat="server" Visible="false" ImageAlign="AbsMiddle"
                                Style="margin-top: 15px;" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class="Button">
                <td>
                </td>
                <td>
                    <asp:Button ID="Step1Button" runat="server" CssClass="Green Large" TabIndex="4" ValidationGroup="Step1"
                        OnClick="Step1Button_Click"></asp:Button>
                </td>
            </tr>
        </table>
    </div>
</div>
<div id="Step2Panel" runat="server">
    <div class="Logo">
        <asp:Image ID="LogoImage2" runat="server" /></div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" RenderMode="Inline" UpdateMode="Always">
        <ContentTemplate>
            <table id="Step2Form" runat="server" align="center" style="width: 850px;">
                <tr class="Group">
                    <td colspan="2">
                        <div>
                            <asp:Literal ID="OrganizationAddressLabel" runat="server"></asp:Literal></div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 270px;">
                        <asp:Label ID="OrganizationNameLabel2" runat="server" CssClass="Large"></asp:Label>
                    </td>
                    <td style="width: 580px;">
                        <mits:TextBox ID="OrganizationName2" runat="server" Required="true" ValidationGroup="Step2"
                            AutoPostBack="true" Width="350px" OnTextChanged="OrganizationName1_TextChanged" />
                        <asp:CustomValidator ID="OrganizationNameValidator2" runat="server" Display="Dynamic"
                            EnableClientScript="false" ValidationGroup="Step2" CssClass="Error" OnServerValidate="OrganizationNameValidator1_ServerValidate" />
                        <asp:Image ID="OrganizationNameTick2" runat="server" Visible="false" ImageAlign="AbsMiddle" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="WebsiteLabel" runat="server" CssClass="Large"></asp:Label>
                    </td>
                    <td>
                        <mits:TextBox ID="Website" runat="server" MaxLength="2048" ValidationType="RegularExpression"
                            ValidationExpression="(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?" ValidationGroup="Step2"
                            Width="350px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="HowYouHearAboutUsLabel" runat="server" CssClass="Large"></asp:Label>
                    </td>
                    <td>
                        <mits:TextBox ID="HowYouHearAboutUs" runat="server" ValidationGroup="Step2" Width="350px" />
                    </td>
                </tr>
                <tr class="Group">
                    <td colspan="2">
                        <div>
                            <asp:Literal ID="PersonalInformationLabel" runat="server"></asp:Literal></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="FirstNameLabel" runat="server" CssClass="Large"></asp:Label>
                    </td>
                    <td>
                        <mits:TextBox ID="FirstName" runat="server" Required="true" ValidationGroup="Step2"
                            MaxLength="255" Width="350px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="LastNameLabel" runat="server" CssClass="Large"></asp:Label>
                    </td>
                    <td>
                        <mits:TextBox ID="LastName" runat="server" Required="true" ValidationGroup="Step2"
                            MaxLength="255" Width="350px" />
                    </td>
                </tr>
                <tr class="Group">
                    <td colspan="2">
                        <div>
                            <asp:Literal ID="LocalSettingsLabel" runat="server"></asp:Literal></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="TimeZoneLabel" runat="server" CssClass="Large"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="TimeZoneList" runat="server" ValidationGroup="Step2" Width="350px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="CurrencyLabel" runat="server" CssClass="Large"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="CurrencyList" runat="server" ValidationGroup="Step2" Width="350px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="Group">
                    <td colspan="2">
                        <div>
                            <asp:Literal ID="EmailAndPasswordLabel" runat="server"></asp:Literal></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="EmailLabel2" runat="server" CssClass="Large"></asp:Label>
                    </td>
                    <td>
                        <mits:TextBox ID="Email2" runat="server" Required="true" ValidationGroup="Step2"
                            AutoPostBack="true" OnTextChanged="Email1_TextChanged" Width="350px" />
                        <asp:CustomValidator ID="EmailValidator2" runat="server" Display="Dynamic" EnableClientScript="false"
                            ValidationGroup="Step2" CssClass="Error" OnServerValidate="EmailValidator1_ServerValidate" />
                        <asp:Image ID="EmailTick2" runat="server" Visible="false" ImageAlign="AbsMiddle" />
                    </td>
                </tr>
                <tr id="PasswordRow" runat="server">
                    <td>
                        <asp:Label ID="PasswordLabel" runat="server" CssClass="Large"></asp:Label>
                    </td>
                    <td>
                        <mits:TextBox ID="Password" runat="server" Required="true" ValidationGroup="Step2"
                            TextMode="Password" Width="350px" /><asp:CustomValidator ID="PasswordValidator" runat="server"
                                Display="Dynamic" CssClass="Error" ValidationGroup="Step2" ValidateEmptyText="true"
                                OnServerValidate="PasswordValidator_ServerValidate" />
                    </td>
                </tr>
                <tr id="ConfirmPasswordRow" runat="server">
                    <td>
                        <asp:Label ID="ConfirmPasswordLabel" runat="server" CssClass="Large"></asp:Label>
                    </td>
                    <td>
                        <mits:TextBox ID="ConfirmPassword" runat="server" Required="true" ValidationGroup="Step2"
                            TextMode="Password" Width="350px" /><asp:CustomValidator ID="PasswordCompareValidator"
                                runat="server" Display="Dynamic" ValidationGroup="Step2" CssClass="Error" ClientValidationFunction="PasswordCompareValidation" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="CaptchaLabel" runat="server" CssClass="Large"></asp:Label>
                    </td>
                    <td>
                        <telerik:RadCaptcha id="Captcha1" runat="server" captchaimage-textlength="4" captchatextboxlabel=""
                            captchatextboxcssclass="" errormessage="" validationgroup="Step2" />
                        <asp:CustomValidator ID="CaptchaValidator" runat="server" Display="Dynamic" EnableClientScript="false"
                            ValidationGroup="Step2" CssClass="Error" OnServerValidate="CaptchaValidator_ServerValidate" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="Step2FormButton" runat="server" align="center" style="width: 850px;">
        <tr class="Button">
            <td colspan="2" style="text-align: center;">
                <hr />
                <asp:ValidationSummary ID="Step2ValidationSummary" runat="server" CssClass="Error Block Summary"
                    DisplayMode="BulletList" ShowSummary="true" ValidationGroup="Step2" />
                <div style="padding-top: 50px;">
                    <asp:Button ID="Step2Button" runat="server" CssClass="Green X-Large" ValidationGroup="Step2"
                        OnClick="Step2Button_Click"></asp:Button>
                </div>
            </td>
        </tr>
    </table>
    <div class="Agree">
        <asp:Literal ID="AgreeLabel" runat="server"></asp:Literal>
    </div>
</div>
<div id="Step3Panel" runat="server" style="margin-top: 100px;">
    <table align="center" cellpadding="0" cellspacing="0" style="width: 575px;">
        <tr>
            <td>
                <div class="Mp_Dm">
                    <h1>
                        <asp:Literal ID="CustomizeLiteral" runat="server"></asp:Literal></h1>
                    <asp:Repeater ID="InstanceList" runat="server" DataSourceID="InstanceListDataSource">
                        <HeaderTemplate>
                            <ul style="width: 575px;">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li onclick="SelectItem(this, '<%# Eval("InstanceId") %>');" class='<%# Convert.ToString(Eval("InstanceId")) == SelectedInstance.Text ? "Cbc" : "Cb" %>'
                                style="cursor: pointer;"><a>
                                    <h2>
                                        <%# Eval("Name") %></h2>
                                </a>
                                <p>
                                    <%# Eval("Description") %></p>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul><br />
                        </FooterTemplate>
                    </asp:Repeater>
                    <table id="Step3Form" runat="server">
                        <tr class="Button">
                            <td>
                                <asp:Button ID="Step3Button" runat="server" CssClass="Green Large" ValidationGroup="Step3"
                                    Style="float: left;" OnClick="Step3Button_Click"></asp:Button>
                                <asp:CustomValidator ID="InstanceRequiredValidator" runat="server" Display="Dynamic"
                                    ValidationGroup="Step3" CssClass="Error Step3Val" ClientValidationFunction="InstanceRequiredValidation" /><asp:CustomValidator
                                        ID="UniqueDataValidator" runat="server" Display="Dynamic" EnableClientScript="false"
                                        ValidationGroup="Step3" CssClass="Error Step3Val" OnServerValidate="UniqueDataValidator_ServerValidate" /><asp:TextBox
                                            ID="SelectedInstance" runat="server" ValidationGroup="Step3" Style="display: none;"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
<div id="ErrorPanel" runat="server">
    <div class="Logo">
        <asp:Image ID="LogoImage3" runat="server" /></div>
    <table align="center">
        <tr>
            <td style="padding-top: 20px;">
                <asp:Label ID="ErrorLabel" runat="server" CssClass="Error Block"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 10px; text-align: center;" class="Agree">
                <p>
                    <asp:Label ID="ErrorContinueLabel" runat="server"></asp:Label></p>
            </td>
        </tr>
        <tr>
            <td style="text-align: center;">
                <asp:HyperLink ID="ErrorContinueLink" runat="server" CssClass="Button Green Large">
                </asp:HyperLink>
            </td>
        </tr>
    </table>
</div>
<asp:ObjectDataSource ID="CountriesDataSource" runat="server" SelectMethod="GetCountriesView"
    TypeName="Micajah.Common.Bll.Providers.CountryProvider"></asp:ObjectDataSource>
<asp:ObjectDataSource ID="InstanceListDataSource" runat="server" SelectMethod="GetTemplateInstances"
    TypeName="Micajah.Common.Bll.Providers.InstanceProvider"></asp:ObjectDataSource>
