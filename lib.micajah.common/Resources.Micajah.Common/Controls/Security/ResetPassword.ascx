<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.ResetPasswordControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
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
<div id="MainContainer" runat="server" style="position: absolute; top: 50%; height: 220px;
    margin-top: -110px; width: 100%; text-align: center;">
    <div id="ErrorPanel" runat="server" visible="false" enableviewstate="false" class="ErrorMessage"
        style="padding-bottom: 7px;" />
    <asp:Label ID="TitleLabel" runat="server" CssClass="Caption" />
    <br />
    <br />
    <asp:Label ID="DescriptionLabel" runat="server" />
    <br />
    <br />
    <asp:LinkButton ID="LogOnPageButton2" runat="server" Visible="false" CausesValidation="false"
        OnClick="LogOnPageButton_Click" CssClass="Return Bold" />
    <table id="FormTable" runat="server" align="center" cellspacing="10" style="text-align: left;"
        class="FormTable">
        <tr>
            <td style="padding-top: 6px; text-align: right;">
                <asp:Label ID="LoginLabel" runat="server" />
            </td>
            <td>
                <mits:TextBox ID="LoginTextBox" runat="server" Columns="65" ReadOnly="True" CssClass="Medium" />
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
            <td>
            </td>
            <td>
                <asp:Button ID="SubmitButton" runat="server" CssClass="Btn" OnClick="SubmitButton_Click" />
                <span>
                    <asp:PlaceHolder ID="ButtonsSeparator" runat="server" />
                </span>
                <asp:LinkButton ID="LogOnPageButton" runat="server" CssClass="Return Bold" CausesValidation="false"
                    OnClick="LogOnPageButton_Click" />
            </td>
        </tr>
    </table>
</div>
