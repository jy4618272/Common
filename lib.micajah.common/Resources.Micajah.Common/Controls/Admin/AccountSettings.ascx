<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.AccountSettingsControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:PlaceHolder ID="PageContent" runat="server">
<div class="planinfo">
    <div class="account-head">
        <div class="account-type">
            <h2><asp:Literal ID="lBillingPlanName" runat="server" Text="FREE"></asp:Literal></h2><small class="plandescsm"><asp:Literal ID="lSumPerMonth" runat="server" Text="per Month"></asp:Literal></small>
        </div>
        <div class="payment-set">
            <div class="payment-update">
                <a id="inline" class="buttons" rel="facebox" href="#credit_card_form">Update credit card</a>
            </div>
            <div class="payment-status">
                <h4><asp:Literal ID="lCCStatus" runat="server" Text="No Credit Card on File."></asp:Literal></h4><small class="plandescsm"><asp:Literal ID="lNextBillDate" runat="server" Text="Next billed on 8 October 2011" Visible="False"></asp:Literal></small>
            </div>
        </div>
    </div>
    <!-- Account Options -->
    <div class="account-heading"><h2>Account Options</h2></div>           
    <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
    </asp:Repeater>
    <!-- Account Usage -->
    <div class="account-heading"><h2>Account Usage</h2></div>
    <asp:Repeater ID="Repeater2" runat="server" OnItemDataBound="Repeater2_ItemDataBound">
        <HeaderTemplate>
            <table class="account-usage">
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td class="feature" id="AccUsageCol" runat="server">
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td class="feature" id="AccUsageCol" runat="server">
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
                </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <div class="account-heading"><h2>Support Options</h2></div>
    <div id="support">
        <div id="emailsupport">
            <div class="email-emailsupport"><asp:HyperLink runat="server" NavigateUrl="~/support" CssClass="buttons" Text="Submit a Ticket"></asp:HyperLink></div>
            <h4>Online Support</h4>
        </div>
        <div id="phonesupport">
            <h4>Phone Support</h4><span id="phone-service"><h4><asp:Literal ID="lPhoneSupport" runat="server" Text="(866) 996-1200"></asp:Literal></h4></span>
            <div class="feature-toggle">
                <mits:CheckBox ID="chkPhoneSupport" runat="server" RenderingMode="OnOffSwitch"/>
            </div>
        </div>
    </div>
    <div class="account-heading"><h2>Training and Consulting</h2></div>
    <div id="training-consulting">
        <p>Choose one of our training packages for a comprehensive set up and training session, including configuring your emails, setting up your business rules (SLA's), and creating your custom searches. Plus we'll help you make basic changes to your customer portal to reflect your brand, as well as walk you through the crucial parts of your SherpaDesk admin.</p>
        <table>
            <tr>
                <td>
                    <div class="training">
                        <p><strong>1 Hour</strong> <span>$175</span></p>
                    </div>
                    <div class="purchase">
                        <asp:Button runat="server" ID="btnPurchase1Hour" CssClass="Green" Text="Purchase"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="training">
                        <p><strong>3 Hours</strong> <span>$475</span></p>
                    </div>
                    <div class="purchase">
                        <asp:Button runat="server" ID="btnPurchase3Hours" CssClass="Green" Text="Purchase"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="training">
                        <p><strong>5 Hours</strong> <span>$675</span></p>
                    </div>
                    <div class="purchase">
                        <asp:Button runat="server" ID="btnPurchase5Hours" CssClass="Green" Text="Purchase"/>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="account-heading"><h2>Cancel Account</h2></div>
    <div id="cancel-account">
        <div style="width: 225px; float: right;">
            <asp:Button runat="server" ID="btnCancelMyAccount" CssClass="Large Red" Text="Cancel My Account"/>
        </div>
        <div>
            Note that you will lose information stored on our servers once you delete your account.
        </div>
    </div>
</div>
</asp:PlaceHolder>
