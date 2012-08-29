<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.ChangePasswordControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<mits:MagicForm ID="EditForm" runat="server" DataKeyNames="LoginId" DataSourceID="PasswordDataSource"
    AutoGenerateRows="False" AutoGenerateEditButton="True" DefaultMode="Edit" Width="550px"
    OnDataBound="EditForm_DataBound" OnItemUpdating="EditForm_ItemUpdating" OnItemUpdated="EditForm_ItemUpdated"
    OnItemCommand="EditForm_ItemCommand">
    <fields>
        <mits:TemplateField PaddingLeft="false" Visible="false" HeaderStyle-Width="120px">
            <ItemTemplate>
                <mits:TextBox ID="CurrentPassword" runat="server" Columns="65" Width="350px" MaxLength="50" TextMode="Password" 
                    ValidationGroup="<%# EditForm.ClientID %>" />
                <asp:Label ID="CurrentPasswordErrorLabel" runat="server" Width="350px" CssClass="Error" Visible="False" EnableViewState="false" />
            </ItemTemplate>
        </mits:TemplateField>
        <mits:TemplateField PaddingLeft="false" HeaderStyle-Width="120px">
            <ItemTemplate>
                <mits:TextBox ID="NewPassword" runat="server" Columns="65" Width="350px" MaxLength="50" TextMode="Password" 
                    ValidationGroup="<%# EditForm.ClientID %>" Text='<%# Bind("Password") %>' />
            </ItemTemplate>
        </mits:TemplateField>
        <mits:TemplateField PaddingLeft="false">
            <ItemTemplate>
                <mits:TextBox ID="ConfirmNewPassword" runat="server" Columns="65" Width="350px" MaxLength="50" TextMode="Password" 
                    ValidationGroup="<%# EditForm.ClientID %>" />
            </ItemTemplate>
        </mits:TemplateField>
        <mits:TemplateField PaddingLeft="false">
            <ItemTemplate>
                <asp:CustomValidator ID="PasswordCompareValidator" runat="server" Display="Dynamic" CssClass="Error Block" 
                    ClientValidationFunction="PasswordCompareValidation" ValidationGroup="<%# EditForm.ClientID %>"
                    ErrorMessage="<%# PasswordCompareErrorMessage %>" />
                <asp:Label ID="ChangePasswordErrorLabel" runat="server" CssClass="Error Block" Visible="False" />
            </ItemTemplate>
        </mits:TemplateField>
    </fields>
</mits:MagicForm>
<asp:Label ID="LoginIdLabel" runat="server" Visible="false" />
<asp:ObjectDataSource ID="PasswordDataSource" runat="server" SelectMethod="GetLogin"
    TypeName="Micajah.Common.Bll.Providers.LoginProvider" UpdateMethod="ChangePassword">
    <SelectParameters>
        <asp:ControlParameter Name="loginId" Type="Object" ControlID="LoginIdLabel" PropertyName="Text" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="loginId" Type="Object" />
        <asp:Parameter Name="password" Type="String" />
    </UpdateParameters>
</asp:ObjectDataSource>
