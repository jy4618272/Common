<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.PasswordRecoveryControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<div id="MainContainer" runat="server" style="position: absolute; top: 50%; height: 240px;
    margin-top: -120px; width: 100%; text-align: center;">
    <div id="ErrorDiv" runat="server" class="ErrorMessage" enableviewstate="false" style="text-align: center;" />
    <table id="ResultTable" runat="server" align="center" cellspacing="10" class="FormTable">
        <tr id="SuccessTableRow" runat="server" class="Caption">
            <td colspan="2" style="text-align: center;">
                <asp:Literal ID="TitleLabel2" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="text-align: center; padding-top: 15px;">
                <asp:LinkButton ID="LogOnPageButton2" runat="server" TabIndex="3" CausesValidation="false"
                    OnClick="LogOnPageButton_Click" CssClass="Return" />
            </td>
        </tr>
    </table>
    <table id="FormTable" runat="server" align="center" cellspacing="10" style="text-align: left;"
        class="FormTable">
        <tr class="Caption">
            <td colspan="2" style="text-align: center; padding-bottom: 15px;">
                <asp:Literal ID="TitleLabel" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 12px; text-align: right;">
                <asp:Label ID="LoginLabel" runat="server" CssClass="Large" />
            </td>
            <td>
                <mits:TextBox id="LoginTextBox" runat="server" Columns="33" CssClass="Large" Required="True"
                    ShowRequired="false" TabIndex="1" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="Pl" style="padding-top: 18px;">
                <asp:Button ID="SubmitButton" runat="server" TabIndex="2" CssClass="Btn" OnClick="SubmitButton_Click" />
                <span>
                    <asp:PlaceHolder ID="ButtonsSeparator" runat="server" />
                </span>
                <asp:LinkButton ID="LogOnPageButton" runat="server" TabIndex="3" CausesValidation="false"
                    OnClick="LogOnPageButton_Click" CssClass="Return" />
            </td>
        </tr>
    </table>
</div>
