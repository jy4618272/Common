<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.AccountSettingsControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2011.1.413.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<asp:PlaceHolder ID="PageContent" runat="server">
<script type="text/javascript">

    // Override $.fancybox.init to fix ASP.NET PostBack bug;
    var fancyboxInitOld = $.fancybox.init;
    $.fancybox.init = function () {
        fancyboxInitOld.apply(arguments);
        $("#fancybox-tmp, #fancybox-loading, #fancybox-overlay, #fancybox-wrap").appendTo("form:first");
    };

    // Your code ...

</script>
<div class="planinfo">
    <div class="account-head" id="divAccountHead" runat="server">
        <div class="account-type" id="divAccountType" runat="server">
            <h2><asp:Literal ID="lBillingPlanName" runat="server" Text="FREE"></asp:Literal></h2><small class="plandescsm"><asp:Literal ID="lSumPerMonth" runat="server" Text="per Month"></asp:Literal></small>
        </div>
        <div class="payment-set">
            <div class="payment-update" id="divPaymentUpdate" runat="server">
                <a id="aBtnCCUpdate" class="buttons" rel="facebox" href="#credit_card_form">Update credit card</a>
            </div>
            <div class="payment-status">
                <h4><asp:Literal ID="lCCStatus" runat="server" Text="No Credit Card on File."></asp:Literal></h4><small class="plandescsm" ID="smallNextBillDate" runat="server" Visible="False">Next billed on 8 October 2011</small>
            </div>
        </div>
    </div>
    <!-- Account Options -->
    <div class="account-heading"><h2>Account Options</h2></div>           
    <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
    </asp:Repeater>
    <!-- Account Usage -->
    <div class="account-heading" id="divPaidUsageHeader" runat="server"><h2><asp:Literal runat="server" ID="lAccountUsage">Account Usage</asp:Literal></h2></div>
    <asp:Repeater ID="Repeater2" runat="server">
        <HeaderTemplate>
            <table class="account-usage">
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td class="feature">
                    <div class="featurelabel"><h5><%# DataBinder.Eval(Container.DataItem, "SettingName")%></h5></div>
                    <div class="account-usage-amount"><h4><%# DataBinder.Eval(Container.DataItem, "UsageCount")%></h4></div>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td class="feature">
                    <div class="featurelabel"><h5><%# DataBinder.Eval(Container.DataItem, "SettingName")%></h5></div>
                    <div class="account-usage-amount"><h4><%# DataBinder.Eval(Container.DataItem, "UsageCount")%></h4></div>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
                </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <asp:Repeater ID="Repeater3" runat="server">
        <HeaderTemplate>
            <table class="account-usage">
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td class="feature" id="AccUsageCol" runat="server">
                    <div class="featurelabel"><h5><%# DataBinder.Eval(Container.DataItem, "SettingName")%></h5></div>
                    <div class="account-usage-amount"><h4><%# (int)DataBinder.Eval(Container.DataItem, "UsageCount") == -1 ? "Enabled" : DataBinder.Eval(Container.DataItem, "UsageCount") %></h4></div>
                    <div class="clearfix"></div>
                    <div class="accsettings paid-account">
                        <a class="accsettings tooltip_right tooltip" href="#">
                            <span class="accsettings"><%# (int)DataBinder.Eval(Container.DataItem, "UsageCount") == -1 ? DataBinder.Eval(Container.DataItem, "Price", "{0:c}") + " / month if option is enabled" : DataBinder.Eval(Container.DataItem, "SettingDescription")%></span>
                            <h3><%# DataBinder.Eval(Container.DataItem, "Price", "{0:c}")%></h3>
                        </a>
                    </div>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td class="feature" id="AccUsageCol" runat="server">
                    <div class="featurelabel"><h5><%# DataBinder.Eval(Container.DataItem, "SettingName")%></h5></div>
                    <div class="account-usage-amount"><h4><%# (int)DataBinder.Eval(Container.DataItem, "UsageCount") == -1 ? "Enabled" : DataBinder.Eval(Container.DataItem, "UsageCount") %></h4></div>
                    <div class="clearfix"></div>
                    <div class="accsettings paid-account">
                        <a class="accsettings tooltip_right tooltip" href="#">
                            <span class="accsettings"><%# (int)DataBinder.Eval(Container.DataItem, "UsageCount") == -1 ? DataBinder.Eval(Container.DataItem, "Price", "{0:c}") + " / month if option is enabled" : DataBinder.Eval(Container.DataItem, "SettingDescription")%></span>
                            <h3><%# DataBinder.Eval(Container.DataItem, "Price", "{0:c}")%></h3>
                        </a>                        
                    </div>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
                </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <div class="account-heading" id="divFreeUsageHeader" runat="server" Visible="False"><h2>Free Usage</h2></div>
    <asp:Repeater ID="Repeater4" runat="server" Visible="False">
        <HeaderTemplate>
            <table class="account-usage">
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td class="feature" id="AccUsageCol" runat="server">
                    <div class="featurelabel"><h5><%# DataBinder.Eval(Container.DataItem, "SettingName")%></h5></div>
                    <div class="account-usage-amount"><h4><span class="under"><%# DataBinder.Eval(Container.DataItem, "UsageCount")%></span>&nbsp;of&nbsp;<%# DataBinder.Eval(Container.DataItem, "UsageCountLimit")%></h4></div>
                    <div class="clearfix"></div>
                    <div class="progress">
                        <div class="bar" style='<%# "width:" +  DataBinder.Eval(Container.DataItem, "UsagePersent") + "%;"%>'></div>
                    </div>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td class="feature" id="AccUsageCol" runat="server">
                    <div class="featurelabel"><h5><%# DataBinder.Eval(Container.DataItem, "SettingName")%></h5></div>
                    <div class="account-usage-amount"><h4><span class="under"><%# DataBinder.Eval(Container.DataItem, "UsageCount")%></span>&nbsp;of&nbsp;<%# DataBinder.Eval(Container.DataItem, "UsageCountLimit")%></h4></div>
                    <div class="clearfix"></div>
                    <div class="progress">
                        <div class="bar" style='<%# "width:" +  DataBinder.Eval(Container.DataItem, "UsagePersent") + "%;"%>'></div>
                    </div>
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
        <div id="divPhoneSupport" class="accsettings phonesupport" runat="server">
            <h4 class="accsettings phonesupport-label">Phone Support</h4><span id="phone-service" class="accsettings"><h4 class="accsettings"><asp:Literal ID="lPhoneSupport" runat="server" Text="(866) 996-1200"></asp:Literal></h4></span>
            <div class="feature-toggle" id="divbtPhoneSupportOnOff">
                <mits:CheckBox ID="chkPhoneSupport" runat="server" RenderingMode="OnOffSwitch" AutoPostBack="True" OnCheckedChanged="checkBox_CheckedChanged"/>
                <asp:PlaceHolder runat="server" ID="phPhoneSupportToolTip"></asp:PlaceHolder>
            </div>
        </div>
    </div>
    <div class="account-heading" id="divTrainingHeader" runat="server"><h2>Training and Consulting</h2></div>
    <div id="divTraining" runat="server">
        <p>Choose one of our training packages for a comprehensive set up and training session, including configuring your emails, setting up your business rules (SLA's), and creating your custom searches. Plus we'll help you make basic changes to your customer portal to reflect your brand, as well as walk you through the crucial parts of your SherpaDesk admin.</p>
        <table>
            <tr>
                <td>
                    <div class="training">
                        <p><strong>1 Hour&nbsp;</strong><asp:Label runat="server" ID="lblTraining1HourPrice" Width="150"></asp:Label></p>
                    </div>
                    <div class="purchase">
                        <asp:Button runat="server" ID="btnPurchase1Hour" CssClass="Green" Text="Purchase" OnClick="btnPurchaseHours_Click" OnClientClick="return confirm('Are You sure You want to purchase 1 hour training?');"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="training">
                        <p><strong>3 Hours</strong><asp:Label runat="server" ID="lblTraining3HoursPrice" Width="150"></asp:Label></p>
                    </div>
                    <div class="purchase">
                        <asp:Button runat="server" ID="btnPurchase3Hours" CssClass="Green" Text="Purchase" OnClick="btnPurchaseHours_Click" OnClientClick="return confirm('Are You sure You want to purchase 3 hours training?');"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="training">
                        <p><strong>8 Hours</strong><asp:Label runat="server" ID="lblTraining8HoursPrice" Width="150"></asp:Label></p>
                    </div>
                    <div class="purchase">
                        <asp:Button runat="server" ID="btnPurchase8Hours" CssClass="Green" Text="Purchase" OnClick="btnPurchaseHours_Click" OnClientClick="return confirm('Are You sure You want to purchase 8 hours training?');"/>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divCancelAccountHeader" runat="server" Visible="False" class="account-heading"><h2>Cancel Account</h2></div>
    <div id="divCancelAccount" runat="server" Visible="False">
        <div style="width: 225px; float: right;">
            <asp:Button runat="server" ID="btnCancelMyAccount" CssClass="Large Red" Text="Cancel My Account" OnClick="btnCancelMyAccount_Click" OnClientClick="return confirm('Are You sure to delete your credit card registration?');"/>
        </div>
        <div>
            Note that you will cancel your credit card registration.
        </div>
    </div>
    <div class="account-heading" id="divPaymentHistoryHeader" runat="server" Visible="False"><h2>Payment History</h2></div>
    <mits:CommonGridView ID="cgvTransactList" runat="server" AutoGenerateColumns="False" AllowSorting="False" Width="600px" Visible="False">
        <columns>
            <mits:TextBoxField DataField="CreatedAt" HeaderText="Date" DataFormatString="{0:d-MMM-yyyy}"/>
            <mits:TextBoxField DataField="Memo" HeaderText="Memo"/>
            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <asp:Label ID="lbStatus" runat="server" Text='<%# (bool)Eval("Success") ? "Success" : "Failed" %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <mits:TextBoxField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C}" />
        </columns>
    </mits:CommonGridView>
</div>

<!-- Pop ups from here down -->
<div style="display: none;">
    <div id="credit_card_form">    
    <div class="content">
        <asp:UpdatePanel runat="server" ID="UpdatePanel3">
        <ContentTemplate>
        <mits:NoticeMessageBox runat="server" ID="msgStatus" MessageType="Success" Visible="False"></mits:NoticeMessageBox>
        <asp:HiddenField runat="server" ID="hfPurchaseTrainingHours" Value="0"/>
            <div class="cards_select">  
            <ul class="cards">
                <li>
                <span data-name="visa" title="Visa" class="card visa">Visa</span>
                </li>    
                <li>
                <span data-name="master" title="Mastercard" class="card master">Mastercard</span>
                </li>    
                <li>
                <span data-name="american-express" title="American Express" class="card american-express">American Express</span>
                </li>    
                <li>
                <span data-name="discover" title="Discover" class="card discover">Discover</span>
                </li>
            </ul>
        </div>
            <dl class="form">
            <dt><label>Card Number</label></dt>
            <dd><mits:TextBox runat="server" ID="txtCCNumber" Required="True" Width="300"/></dd>
            </dl>
            <dl class="form expiration">
            <dt><label>Expiration</label></dt>
            <dd>
                <mits:TextBox runat="server" ID="txtCCExpMonth" ValidationType="Integer" MinimumValue="1" MaximumValue="12" ToolTip="Month MM" Columns="2" MaxLength="2" Required="True"/>
                <span>&nbsp;/&nbsp;</span>
                <mits:TextBox runat="server" ID="txtCCExpYear" ValidationType="Integer" MinimumValue="13" MaximumValue="23" ToolTip="Year YY" Columns="2" MaxLength="2" Required="True"/>
            </dd>
            </dl>
            <div class="ccformsubmit">
                <asp:Button runat="server" ID="btnUpdateCC" CssClass="Large Green" Text="Update Credit Card" OnClick="btnUpdateCC_Click"/>
	        </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnUpdateCC" EventName="Click"/>
            <asp:PostBackTrigger ControlID="btnPurchase1Hour"/>
            <asp:PostBackTrigger ControlID="btnPurchase3Hours"/>
            <asp:PostBackTrigger ControlID="btnPurchase8Hours"/>
        </Triggers>
        </asp:UpdatePanel>    
    </div>
    </div>
</div>
</asp:PlaceHolder>
