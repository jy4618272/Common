<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.AccountSettingsControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:PlaceHolder ID="PageContent" runat="server">
<div style="overflow: hidden;padding-top:10px;">
    <div style="border-right: 1px solid #417DA6;overflow: hidden;float: left;padding-left: 20px;padding-right: 20px;">
        <center><h2 style="margin: 0;padding: 0;"><asp:Literal ID="lBillingPlanName" runat="server" Text="FREE"></asp:Literal></h2></center><small style="color: #999;"><asp:Literal ID="lSumPerMonth" runat="server" Text="per Month"></asp:Literal></small>
    </div>
    <div id="divCCInfo" runat="server" style="overflow: hidden;float: left;padding-left:20px;padding-right:30px;">
        <h4 style="margin: 0;padding:1px;"><asp:Literal ID="lCCStatus" runat="server" Text="No Credit Card on File."></asp:Literal></h4>
        <small style="color: #999;"><asp:Literal ID="lNextBillDate" runat="server" Text="Next billed on 8 October 2011"></asp:Literal></small>
    </div>
    <div id="Div1" runat="server" class="btn_pnl">
        <asp:Button ID="btnUpdateBillingPlan" runat="server" Text="Upgrade to Paid" OnClick="UpdateBillingPlanButton_Click" />
    </div>
</div>
<div style="margin: 20px 0 20px;padding: 10px 15px 10px 0;border-bottom: 2px solid #417DA6;"><h2 style="margin: 0;padding: 0;line-height: 1em;">Paid Options</h2></div>
    <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
        <HeaderTemplate>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td class="Tc2" style="white-space: nowrap;">
                    <asp:PlaceHolder ID="ControlHoder2" runat="server"></asp:PlaceHolder>
                </td>
                <td class="Tc2" style="white-space: nowrap;">
                    <asp:Label ID="NameLabel2" runat="server" CssClass="Nm"></asp:Label>
                </td>
                <td class="Tc2" style="width: 100%;">
                    <asp:PlaceHolder ID="PriceHolder" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td class="Tc1" style="white-space: nowrap;">
                    <asp:PlaceHolder ID="ControlHoder2" runat="server"></asp:PlaceHolder>
                </td>
                <td class="Tc1" style="white-space: nowrap;">
                    <asp:Label ID="NameLabel2" runat="server" CssClass="Nm"></asp:Label>
                </td>
                <td class="Tc1" style="width: 100%;">
                    <asp:PlaceHolder ID="PriceHolder" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
<div id="Div2" runat="server" class="btn_pnl"><asp:Button ID="UpdateButton" runat="server" OnClick="UpdateButton_Click" /></div>
<div style="margin: 20px 0 20px;padding: 10px 15px 10px 0;border-bottom: 2px solid #417DA6;"><h2 style="margin: 0;padding: 0;line-height: 1em;">Account Usage</h2></div>
    <asp:Repeater ID="Repeater2" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
        <HeaderTemplate>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td class="Tc2" style="white-space: nowrap;">
                    <asp:PlaceHolder ID="ControlHoder2" runat="server"></asp:PlaceHolder>
                </td>
                <td class="Tc2" style="white-space: nowrap;">
                    <asp:Label ID="NameLabel2" runat="server" CssClass="Nm"></asp:Label>
                </td>
                <td class="Tc2" style="width: 100%;">
                    <asp:PlaceHolder ID="PriceHolder" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td class="Tc1" style="white-space: nowrap;">
                    <asp:PlaceHolder ID="ControlHoder2" runat="server"></asp:PlaceHolder>
                </td>
                <td class="Tc1" style="white-space: nowrap;">
                    <asp:Label ID="NameLabel2" runat="server" CssClass="Nm"></asp:Label>
                </td>
                <td class="Tc1" style="width: 100%;">
                    <asp:PlaceHolder ID="PriceHolder" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Label ID="lblPurchase" runat="server" CssClass="Nm" ForeColor="Black">Purchase HelpDesk using to for </asp:Label><asp:Label ID="lblPurchaseSum" runat="server" CssClass="Nm" ForeColor="Green"></asp:Label>
        <div id="CommandBarDiv" runat="server" class="btn_pnl">
        <asp:Button ID="ChargifyPayButton" runat="server" OnClick="ChargifyPayButton_Click" />
        <asp:PlaceHolder ID="ButtonsSeparator" runat="server" Visible="false" />
        <asp:HyperLink ID="CancelLink" runat="server" CssClass="Cmd_Cb" Visible="false" />
    </div>
</asp:PlaceHolder>
