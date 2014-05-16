<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.CreditCardRegistrationControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<div style="display: none;">
    <div id="credit_card_form">
        <asp:Panel ID="pnlMissingCard" CssClass="header cc-nag" runat="server" Visible="false">
            <h2>Looks like we are<br />
                missing your credit card.</h2>
        </asp:Panel>
        <div class="content">
            <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                <contenttemplate>
                    <mits:NoticeMessageBox runat="server" ID="msgStatus" MessageType="Success" Visible="False"></mits:NoticeMessageBox>
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
                    <br />
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
                </contenttemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
