<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.ChargifySubscribeControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:PlaceHolder ID="PageContent" runat="server">
    <mits:MagicForm ID="mfChargifyCustomer" runat="server" ObjectName="Chargify Customer"
        AutoGenerateInsertButton="True" 
        AutoGenerateEditButton="True" DefaultMode="Edit" AutoGenerateRows="False" 
        ColorScheme="Blue" onaction="mfChargifyCustomer_Action" 
        onitemupdating="mfChargifyCustomer_ItemUpdating" 
        Caption="Edit Chargify Subscription" CellPadding="0" CssClass="Mf_T" 
        GridLines="None">
        <AlternatingRowStyle CssClass="Mf_R"></AlternatingRowStyle>
        <EditRowStyle CssClass="Mf_R"></EditRowStyle>
        <EmptyDataRowStyle CssClass="Mf_R"></EmptyDataRowStyle>
        <Fields>
            <mits:GroupField Text="Personal Info"/>
            <mits:TextBoxField DataField="Email" HeaderText="EMail" Required="true" ValidationType="RegularExpression" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></mits:TextBoxField>
            <mits:TextBoxField DataField="FirstName" HeaderText="First Name" Required="true"></mits:TextBoxField>
            <mits:TextBoxField DataField="LastName" HeaderText="Last Name" Required="true"></mits:TextBoxField>
            <mits:GroupField Text="Credit Card Details"/>
            <mits:TextBoxField DataField="CCNumber" HeaderText="Number" Required="true"></mits:TextBoxField>
            <mits:ComboBoxField DataField="CCExpirationMonth" HeaderText="Expiration Month" Required="true">
                <Items>
                    <telerik:RadComboBoxItem Text="" Value="" />
                    <telerik:RadComboBoxItem Text="January" Value="1" />
                    <telerik:RadComboBoxItem Text="February" Value="2" />
                    <telerik:RadComboBoxItem Text="March" Value="3" />
                    <telerik:RadComboBoxItem Text="April" Value="4" />
                    <telerik:RadComboBoxItem Text="May" Value="5" />
                    <telerik:RadComboBoxItem Text="June" Value="6" />
                    <telerik:RadComboBoxItem Text="July" Value="7" />
                    <telerik:RadComboBoxItem Text="August" Value="8" />
                    <telerik:RadComboBoxItem Text="September" Value="9" />
                    <telerik:RadComboBoxItem Text="October" Value="10" />
                    <telerik:RadComboBoxItem Text="November" Value="11" />
                    <telerik:RadComboBoxItem Text="December" Value="12" />
                </Items>
            </mits:ComboBoxField>
            <mits:ComboBoxField DataField="CCExpirationYear" DataValueField ="Year" DataTextField="Year" AppendDataBoundItems="True" HeaderText="Expiration Year" Required="true">
                <Items>
                    <telerik:RadComboBoxItem Text="" Value="" />
                </Items>
            </mits:ComboBoxField>
        </Fields>
    </mits:MagicForm>
</asp:PlaceHolder>
