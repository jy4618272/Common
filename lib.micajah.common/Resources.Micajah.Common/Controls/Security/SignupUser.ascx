<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.SignupUserControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<script type="text/javascript">
    //<![CDATA[
    function PasswordCompareValidation(source, arguments) {
        arguments.IsValid = true;
        var Elem1 = document.getElementById('<% = PasswordTextBox.ClientID %>_txt');
        var Elem2 = document.getElementById('<% = ConfirmPasswordTextBox.ClientID %>_txt');
        if (Elem1 && Elem2) arguments.IsValid = (Elem2.value == Elem1.value);
    }
    //]]>
</script>
<div id="MainContainer" runat="server" style="position: absolute; top: 50%; height: 170px;
    margin-top: -85px; width: 100%; text-align: center;">
    <div id="ErrorPanel" runat="server" visible="false" enableviewstate="false" class="ErrorMessage"
        style="padding-bottom: 7px;" />
    <asp:Label ID="TitleLabel" runat="server" CssClass="Caption" />
    <br />
    <br />
    <asp:Panel ID="DescriptionPanel" runat="server" Visible="false">
        <asp:Label ID="DescriptionLabel" runat="server" />
        <br />
        <br />
    </asp:Panel>
    <table id="Step1Table" runat="server" align="center" cellspacing="10" style="text-align: left;"
        class="FormTable">
        <tr>
            <td style="padding-top: 12px; text-align: right;">
                <asp:Label ID="LoginLabel" runat="server" CssClass="Large" />
            </td>
            <td>
                <mits:TextBox ID="LoginTextBox" runat="server" MaxLength="255" Columns="32" Required="True"
                    ShowRequired="false" CssClass="Large" ValidationType="RegularExpression" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="Pl">
                <asp:Button ID="SubmitButton1" runat="server" CssClass="Btn" OnClick="SubmitButton1_Click" />
                <span>
                    <asp:PlaceHolder ID="ButtonsSeparator0" runat="server" />
                </span>
                <asp:HyperLink ID="LogOnPageLink1" runat="server" CssClass="Return" />
            </td>
        </tr>
    </table>
    <table id="Step2Table" runat="server" align="center" cellspacing="10" visible="false"
        style="text-align: left;" class="FormTable">
        <tr>
            <td style="text-align: right;">
                <asp:Label ID="LoginLabel1" runat="server" />
            </td>
            <td class="Pl" style="*width: 490px;">
                <b>
                    <asp:Literal ID="LoginTextLabel" runat="server" /></b>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 6px; text-align: right;">
                <asp:Label ID="FirstNameLabel" runat="server" />
            </td>
            <td>
                <mits:TextBox ID="FirstNameTextBox" runat="server" MaxLength="255" Columns="65" Required="True"
                    ShowRequired="false" CssClass="Medium" />
            </td>
        </tr>
        <tr>
            <td style="padding-top: 6px; text-align: right;">
                <asp:Label ID="LastNameLabel" runat="server" />
            </td>
            <td>
                <mits:TextBox ID="LastNameTextBox" runat="server" MaxLength="255" Columns="65" Required="True"
                    ShowRequired="false" CssClass="Medium" />
            </td>
        </tr>
        <tr>
            <td style="padding-top: 6px; text-align: right;">
                <asp:Label ID="PasswordLabel" runat="server" />
            </td>
            <td>
                <mits:TextBox ID="PasswordTextBox" runat="server" Columns="65" MaxLength="50" Required="true"
                    ShowRequired="false" TextMode="Password" CssClass="Medium" />
            </td>
        </tr>
        <tr id="ConfirmPasswordRow" runat="server">
            <td style="padding-top: 6px; text-align: right;">
                <asp:Label ID="ConfirmPasswordLabel" runat="server" />
            </td>
            <td>
                <mits:TextBox ID="ConfirmPasswordTextBox" runat="server" Columns="65" MaxLength="50"
                    TextMode="Password" CssClass="Medium" />
                <asp:CustomValidator ID="PasswordCompareValidator" runat="server" Display="Dynamic"
                    CssClass="Error" ClientValidationFunction="PasswordCompareValidation" />
            </td>
        </tr>
        <tr>
            <td style="padding-top: 6px; text-align: right;">
                <asp:Label ID="CaptchaLabel" runat="server" />
            </td>
            <td>
                <telerik:RadCaptcha ID="CaptchaControl" runat="server" CaptchaImage-TextLength="4"
                    CaptchaTextBoxLabel="" errormessage="" CaptchaTextBoxCssClass="Medium" />
                <asp:CustomValidator ID="CaptchaValidator" runat="server" Display="Dynamic" EnableClientScript="false"
                    CssClass="Error" OnServerValidate="CaptchaValidator_ServerValidate" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="SubmitButton2" runat="server" CssClass="Btn" OnClick="SubmitButton2_Click" />
                <asp:PlaceHolder ID="Step1ButtonHolder" runat="server"><span>
                    <asp:PlaceHolder ID="ButtonsSeparator1" runat="server" />
                </span>
                    <asp:LinkButton ID="Step1Button" runat="server" CausesValidation="false" CssClass="Return"
                        OnClick="Step1Button_Click" />
                </asp:PlaceHolder>
                <span>
                    <asp:PlaceHolder ID="ButtonsSeparator2" runat="server" />
                </span>
                <asp:HyperLink ID="LogOnPageLink2" runat="server" CssClass="Return" />
            </td>
        </tr>
    </table>
    <br />
    <asp:HyperLink ID="LogOnPageLink3" runat="server" Visible="false" CssClass="Large Return" />
</div>
