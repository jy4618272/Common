<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.TokenControl" %>
<asp:TextBox ID="TokenTextBox" runat="server" ReadOnly="true" Columns="36"></asp:TextBox>&nbsp;
<asp:LinkButton ID="ResetTokenButton" runat="server" OnClick="ResetTokenButton_Click"
    CausesValidation="false"></asp:LinkButton>