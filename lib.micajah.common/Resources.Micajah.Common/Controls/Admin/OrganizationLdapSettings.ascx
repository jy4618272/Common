<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.OrganizationLdapSettingsControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>

<style type="text/css">
<% if (Micajah.Common.Configuration.FrameworkConfiguration.Current.WebApplication.MasterPage.Theme == Micajah.Common.Pages.MasterPageTheme.Modern)
   {%>
     .RadTabStrip .rtsLevel, .RadTabStrip:after, .RadTabStripVertical:after, .RadTabStrip .rtsLevel:after, .RadTabStripVertical .rtsLevel:after, .RadTabStrip .rtsScroll:after, .RadTabStripVertical .rtsScroll:after, .RadTabStrip .rtsUL:after, .RadTabStripVertical .rtsUL:after, .RadTabStrip .rtsLI:after, .RadTabStripVertical .rtsLI:after {
        clear: none;
     }
<% } %>
     .medium, .medium a, .medium a:hover {
         font-size: medium;
     }
</style>
<script type="text/javascript">
<!--
    var instance = Sys.WebForms.PageRequestManager.getInstance();
    instance.add_beginRequest(BeginRequestHandler);

    function BeginRequestHandler(sender, args) {
        if (args.get_postBackElement().id == "<%= CheckPortButton.ClientID %>") {
            var check = document.getElementById("<%= CheckPortResultLabel.ClientID %>");
            if (check)
                check.style.display = "none";
        }

        if (args.get_postBackElement().id == "<%= PingLdapServerButton.ClientID %>") {
            var ping = document.getElementById("<%= PingLdapServerResultLabel.ClientID %>");
            if (ping)
                ping.style.display = "none";
        }
    }

    function checkLdapServerAddress() {        
        var serverAddress = document.getElementById("<%= (EditForm.Rows[0].Cells[1].Controls[0] as Micajah.Common.WebControls.TextBox).ClientID %>" + "_txt");
        if (serverAddress) {
            if ((0 === serverAddress.value.indexOf("192.168")) ||
                (0 === serverAddress.value.indexOf("255.")) ||
                (0 === serverAddress.value.indexOf("10."))) {
                var message = document.getElementById("<%= CheckLdapServerAddressErrorTextHidden.ClientID %>");
                if (message) {
                    alert(message.value);
                }                
                return false;
            }
        }
        return true;
    }
// -->
</script>
<br />
<asp:Label ID="DescriptionLabel" runat="server" CssClass="medium" />
<br /><br /><br />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="OrganizationId" Width="550px">
            <fields>
                <mits:TextBoxField DataField="LdapServerAddress" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" Required="True" />
                <mits:TextBoxField DataField="LdapServerPort" MaxLength="4" Columns="10" Required="True" />
                <mits:TextBoxField DataField="LdapUserName" MaxLength="255" Columns="65" ControlStyle-Width="350px" Required="True" />
                <mits:TextBoxField DataField="LdapDomain" MaxLength="255" Columns="65" ControlStyle-Width="350px" Required="True" />
                <mits:TemplateField PaddingLeft="false">
                    <ItemTemplate>
                        <mits:TextBox ID="LdapUpdatePassword" runat="server" MaxLength="255" Columns="65" Width="350px" TextMode="Password" Required="False" ValidationGroup="<%# EditForm.ClientID %>" />
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField PaddingLeft="false">
                    <ItemTemplate>
                        <mits:TextBox ID="LdapConfirmNewPassword" runat="server" MaxLength="255" Columns="65" Width="350px" TextMode="Password" Required="False" ValidationGroup="<%# EditForm.ClientID %>" />
                        <asp:CustomValidator ID="PasswordCompareValidator" runat="server" Display="Dynamic" CssClass="Error" 
                            Width="285px" ClientValidationFunction="PasswordCompareValidation" ValidationGroup="<%# EditForm.ClientID %>"
                            ErrorMessage="<%# PasswordCompareErrorMessage %>" />
                        <asp:Label ID="ChangePasswordErrorLabel" runat="server" Width="350px" CssClass="Error" Visible="False" />
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField>
                    <ItemTemplate>
                        <div id="ErrorPanel" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                        <asp:Label id="OldPassword" runat="server" Visible="False" Text='<%# Eval("LdapPassword") %>'></asp:Label>
                    </ItemTemplate>
                </mits:TemplateField>
            </fields>
        </mits:MagicForm>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" SelectMethod="GetOrganization" TypeName="Micajah.Common.Bll.Providers.OrganizationProvider" UpdateMethod="UpdateOrganization" OnSelecting="EntityDataSource_Selecting" OnUpdating="EntityDataSource_Updating">
            <UpdateParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:Parameter Name="ldapServerAddress" Type="String" />
                <asp:Parameter Name="ldapServerPort" Type="String" />
                <asp:Parameter Name="ldapDomain" Type="String" />
                <asp:Parameter Name="ldapUserName" Type="String" />
                <asp:Parameter Name="ldapUpdatePassword" Type="String" />
                <asp:Parameter Name="ldapConfirmNewPassword" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:HiddenField ID="CheckLdapServerAddressErrorTextHidden" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<br /><br />
<table class="Mf_T" style="width:550px;border-collapse:collapse;">
    <tr><td class="Mf_Cpt" style="border-bottom: 1px solid;"><asp:Literal ID="LdapSetupLabel" runat="server"/></td></tr>    
</table>

<asp:UpdatePanel ID="CheckPortUpdatePanel" runat="server" RenderMode="Inline">
    <ContentTemplate>        
        <br />
        <br />
        <b><asp:Label ID="Step1Label" runat="server" Font-Size="Medium"  /></b>&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="CheckPortButton" Font-Size="Medium" runat="server" OnClick="CheckPortButton_Click" />
        <br />
        <br />
        <mits:UpdateProgress ID="CheckPortUpdateProgress" runat="server" AssociatedUpdatePanelID="CheckPortUpdatePanel" />
        <asp:Label ID="CheckPortResultLabel" runat="server" Visible="False" />
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="PingLdapServerUpdatePanel" runat="server" RenderMode="Inline">
    <ContentTemplate>        
        <br />
        <b><asp:Label ID="Step2Label" runat="server" Font-Size="Medium"  /></b>&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="PingLdapServerButton" Font-Size="Medium" runat="server" OnClick="PingLdapServerButton_Click" />
        <br />
        <br />
        <mits:UpdateProgress ID="PingLdapServerUpdateProgress" runat="server" AssociatedUpdatePanelID="PingLdapServerUpdatePanel" />
        <asp:Label ID="PingLdapServerResultLabel" runat="server" Visible="False" />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="GetDomainsUpdatePanel" runat="server" RenderMode="Inline">
    <ContentTemplate>
        <br />
        <b><asp:Label ID="Step3Label" runat="server" Font-Size="Medium"  /></b>&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="GetDomainsButton" Font-Size="Medium" runat="server" OnClick="GetDomainsButton_Click" />
        <br />
        <br />
        <asp:MultiView ID="GetDomainsMultiView" runat="server">
            <asp:View ID="GetDomainsViewProcess" runat="server">
                <asp:Image ID="GetDomainsViewProcessImage" runat="server" />
                <span style="color: Gray; font-family: Arial; font-size: 18px; font-weight: bold; padding: 3px 8px 3px 8px;">
                    <asp:Literal ID="GetDomainsViewProcessLiteral" runat="server" />
                </span>
                <asp:Literal ID="GetDomainsViewProcessResultLiteral" runat="server" />
            </asp:View>
            <asp:View ID="GetDomainsViewError" runat="server">
                <span style="color: #000066; background-color: #FFEFAC; font-family: Arial; font-size: 18px; font-weight: bold; padding: 3px 8px 3px 8px;">
                    <asp:Literal ID="GetDomainsViewErrorLiteral" runat="server" Text="Error" />
                </span>
                <br />
            </asp:View>
            <asp:View ID="GetDomainsViewResult" runat="server">
                <mits:ComboBox ID="DomainsComboBox" runat="server" DataTextField="DomainName" DataValueField="Id" />
            </asp:View>
        </asp:MultiView>
        <asp:Timer ID="GetDomainsTimer" runat="server" Interval="30000" OnTick="GetDomainsTimer_Tick" Enabled="false" />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="GetGroupsUpdatePanel" runat="server" RenderMode="Inline">
    <ContentTemplate>
        <br />
        <b><asp:Label ID="Step4Label" runat="server" Font-Size="Medium"  /></b>&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="GetGroupsButton" Font-Size="Medium" runat="server" OnClick="GetGroupsButton_Click" />
        <br />
        <br />
        <asp:MultiView ID="GetGroupsMultiView" runat="server">
            <asp:View ID="GetGroupsViewProcess" runat="server">
                <asp:Image ID="GetGroupsViewProcessImage" runat="server" />
                <span style="color: Gray; font-family: Arial; font-size: 18px; font-weight: bold; padding: 3px 8px 3px 8px;">
                    <asp:Literal ID="GetGroupsViewProcessLiteral" runat="server" />
                </span>
                <asp:Literal ID="GetGroupsViewProcessResultLiteral" runat="server" />
            </asp:View>
            <asp:View ID="GetGroupsViewError" runat="server">
                <span style="color: #000066; background-color: #FFEFAC; font-family: Arial; font-size: 18px; font-weight: bold; padding: 3px 8px 3px 8px;">
                    <asp:Literal ID="GetGroupsViewErrorLiteral" runat="server" Text="Error" />
                </span>
                <br />
            </asp:View>
            <asp:View ID="GetGroupsViewResult" runat="server">
                <mits:CommonGridView ID="GroupsCommonGridView" runat="server" Width="520px" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="false">
                    <columns>
                        <mits:TextBoxField DataField="GroupName" HeaderText="Group Name" />
                    </columns>
                </mits:CommonGridView>
                <asp:Label ID="GetGroupsLabel" runat="server" Font-Size="Medium" />
            </asp:View>
        </asp:MultiView>
        <asp:Timer ID="GetGroupsTimer" runat="server" Interval="30000" OnTick="GetGroupsTimer_Tick" Enabled="false" />       
        <br />
        <b><asp:Label ID="Step5Label" runat="server" Font-Size="Medium"  /></b>&nbsp;&nbsp;&nbsp;<asp:HyperLink ID="GoToGroupMapprings" Font-Size="Medium" runat="server" />        
        <br />
        <br />
        <mits:UpdateProgress ID="GoToGroupMappringsUpdateProgress" runat="server" AssociatedUpdatePanelID="GetGroupsUpdatePanel" />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="TestADReplicationUpdatePanel" runat="server" RenderMode="Inline">
    <ContentTemplate>        
        <br />
        <b><asp:Label ID="Step6Label" runat="server" Font-Size="Medium"  /></b>&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="GetTestAdReplicationInfo" Font-Size="Medium" runat="server" OnClick="GetTestAdReplicationInfo_Click" />
        <br />
        <br />
        <asp:MultiView ID="TestADReplicationMultiView" runat="server">
            <asp:View ID="TestADReplicationViewProcess" runat="server">
                <asp:Image ID="TestADReplicationViewProcessImage" runat="server" />
                <span style="color:Gray;font-family:Arial;font-size:18px;font-weight:bold;padding:3px 8px 3px 8px;">
                    <asp:Literal ID="TestADReplicationViewProcessLiteral" runat="server"/>                    
                </span>
                <telerik:RadTabStrip Id="rtsTestReplicationProcess" runat="server" MultiPageID="rmpTestReplicationProcess" SelectedIndex="0" Width="535px" Visible="false">
                    <Tabs>
                        <telerik:RadTab Text="Logs"/>                
                        <telerik:RadTab Text="Results" Enabled="false"/>                
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="rmpTestReplicationProcess" runat="server" SelectedIndex="0" Width="535px" Visible="false" style="border: 1px solid #828282;margin-top: -1px;">
                     <telerik:RadPageView runat="server">
                        <div style="padding:5px;">
                            <asp:Label ID="TestADReplicationViewProcessResultLabel" runat="server" />
                        </div>
                     </telerik:RadPageView>
                     <telerik:RadPageView runat="server"/>
                </telerik:RadMultiPage>                
            </asp:View>
            <asp:View ID="TestADReplicationViewError" runat="server">
                <span style="color:#000066;background-color:#FFEFAC;font-family:Arial;font-size:18px;font-weight:bold;padding:3px 8px 3px 8px;">
                    <asp:Literal ID="TestADReplicationViewErrorLiteral" runat="server" Text="Error" />
                </span>
                <br />
            </asp:View>
            <asp:View ID="TestADReplicationViewResult" runat="server">         
                <telerik:RadTabStrip Id="rtsTestReplicationResult" runat="server" MultiPageID="rmpTestReplicationResult" SelectedIndex="0" Width="535px" Visible="false">
                    <Tabs>
                        <telerik:RadTab Text="Logs"/>                
                        <telerik:RadTab Text="Results"/>                
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="rmpTestReplicationResult" runat="server" SelectedIndex="0" Width="535px"  Visible="false" style="border: 1px solid #828282;margin-top: -1px;">
                     <telerik:RadPageView runat="server">
                        <div style="padding:5px;">
                            <asp:Label ID="TestADReplicationViewResultLabel" runat="server" />
                        </div>
                     </telerik:RadPageView>
                     <telerik:RadPageView runat="server">
                        <div style="padding:5px;">
                            <asp:Label ID="DeactivatedLoginsLabel" runat="server" Width="520px" />
                            <br />
                            <mits:CommonGridView ID="TestDeactivatedLoginsCommonGridView" runat="server" Width="520px" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="false" >
                                <columns>
                                <mits:TextBoxField DataField="LoginName" HeaderText="User Name" />
                                <mits:TextBoxField DataField="Name" HeaderText="Name" />
                            </columns>
                            </mits:CommonGridView>
                            <asp:Label ID="TestDeactivatedLoginsLabel" runat="server" Visible="false" Font-Size="Medium" />
                            <br />
                            <asp:Label ID="ActivatedLoginsLabel" runat="server" Width="520px" />
                            <br />
                            <mits:CommonGridView ID="TestActivatedLoginsCommonGridView" runat="server" Width="520px" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="false" >
                                <columns>
                                <mits:TextBoxField DataField="LoginName" HeaderText="User Name" />
                                <mits:TextBoxField DataField="Name" HeaderText="Name" />
                            </columns>
                            </mits:CommonGridView>
                            <asp:Label ID="TestActivatedLoginsLabel" runat="server" Visible="false" Font-Size="Medium" />
                            <br />
                            <asp:Label ID="CreatedLoginsLabel" runat="server" Width="520px" />
                            <br />
                            <mits:CommonGridView ID="TestCreatedLoginsCommonGridView" runat="server" Width="520px" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="false">
                                <columns>
                                <mits:TextBoxField DataField="LoginName" HeaderText="User Name" />
                                <mits:TextBoxField DataField="Name" HeaderText="Name" />
                            </columns>
                            </mits:CommonGridView>
                            <asp:Label ID="TestCreatedLoginsLabel" runat="server" Visible="false" Font-Size="Medium" />
                        </div>
                     </telerik:RadPageView>
                </telerik:RadMultiPage>                       
            </asp:View>
        </asp:MultiView>
        <asp:Timer ID="TestADReplicationTimer" runat="server" Interval="10000" OnTick="TestADReplicationTimer_Tick" Enabled="false" />       
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="RealADReplicationUpdatePanel" runat="server" RenderMode="Inline">
    <ContentTemplate>        
        <br />
        <b><asp:Label ID="Step7Label" runat="server" Font-Size="Medium"  /></b>&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="GetRealAdReplicationInfo" Font-Size="Medium" runat="server" OnClick="GetRealAdReplicationInfo_Click" />
        <br />
        <br />
        <asp:MultiView ID="RealADReplicationMultiView" runat="server">
            <asp:View ID="RealADReplicationViewProcess" runat="server">
                <asp:Image ID="RealADReplicationViewProcessImage" runat="server" />
                <span style="color:Gray;font-family:Arial;font-size:18px;font-weight:bold;padding:3px 8px 3px 8px;">
                    <asp:Literal ID="RealADReplicationViewProcessLiteral" runat="server"/>
                </span>                      
                <telerik:RadTabStrip Id="rtsRealReplicationProcess" runat="server" MultiPageID="rmpRealReplicationProcess" SelectedIndex="0" Width="535px" Visible="false">
                    <Tabs>
                        <telerik:RadTab Text="Logs"/>                
                        <telerik:RadTab Text="Results" Enabled="false"/>                
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="rmpRealReplicationProcess" runat="server" SelectedIndex="0" Width="535px" Visible="false" style="border: 1px solid #828282;margin-top: -1px;">
                     <telerik:RadPageView runat="server">
                        <div style="padding:5px;">
                            <asp:Label ID="RealADReplicationViewProcessResultLabel" runat="server" />
                        </div>
                     </telerik:RadPageView>
                     <telerik:RadPageView runat="server"/>
                </telerik:RadMultiPage>                              
            </asp:View>
            <asp:View ID="RealADReplicationViewError" runat="server">
                <span style="color:#000066;background-color:#FFEFAC;font-family:Arial;font-size:18px;font-weight:bold;padding:3px 8px 3px 8px;">
                    <asp:Literal ID="RealADReplicationViewErrorLiteral" runat="server" Text="Error" />
                </span>
                <br />
            </asp:View>
            <asp:View ID="RealADReplicationViewResult" runat="server">                
                <telerik:RadTabStrip Id="rtsRealReplicationResult" runat="server" MultiPageID="rmpRealReplicationResult" SelectedIndex="0" Width="535px" Visible="false">
                    <Tabs>
                        <telerik:RadTab Text="Logs"/>                
                        <telerik:RadTab Text="Results"/>                
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="rmpRealReplicationResult" runat="server" SelectedIndex="0" Width="535px"  Visible="false" style="border: 1px solid #828282;margin-top: -1px;">
                     <telerik:RadPageView runat="server">
                        <div style="padding:5px;">
                            <asp:Label ID="RealADReplicationViewResultLabel" runat="server" />
                        </div>
                     </telerik:RadPageView>
                     <telerik:RadPageView runat="server">
                        <div style="padding:5px;">
                            <asp:Label ID="DeactivatedLoginsLabel2" runat="server" Width="520px" />
                            <br />
                            <mits:CommonGridView ID="RealDeactivatedLoginsCommonGridView" runat="server" Width="520px" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="false" >
                                <columns>
                                <mits:TextBoxField DataField="LoginName" HeaderText="User Name" />
                                <mits:TextBoxField DataField="Name" HeaderText="Name" />
                            </columns>
                            </mits:CommonGridView>
                            <asp:Label ID="RealDeactivatedLoginsLabel" runat="server" Visible="false" Font-Size="Medium" />
                            <br />
                            <asp:Label ID="ActivatedLoginsLabel2" runat="server" Width="520px"/>
                            <br />
                            <mits:CommonGridView ID="RealActivatedLoginsCommonGridView" runat="server" Width="520px" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="false" >
                                <columns>
                                <mits:TextBoxField DataField="LoginName" HeaderText="User Name" />
                                <mits:TextBoxField DataField="Name" HeaderText="Name" />
                            </columns>
                            </mits:CommonGridView>
                            <asp:Label ID="RealActivatedLoginsLabel" runat="server" Visible="false" Font-Size="Medium" />
                            <br />
                            <asp:Label ID="CreatedLoginsLabel2" runat="server" Width="520px"/>
                            <br />
                            <mits:CommonGridView ID="RealCreatedLoginsCommonGridView" runat="server" Width="520px" AllowPaging="False" AllowSorting="True" AutoGenerateColumns="false" >
                                <columns>
                                <mits:TextBoxField DataField="LoginName" HeaderText="User Name" />
                                <mits:TextBoxField DataField="Name" HeaderText="Name" />
                            </columns>
                            </mits:CommonGridView>
                            <asp:Label ID="RealCreatedLoginsLabel" runat="server" Visible="false" Font-Size="Medium" />                
                        </div>
                     </telerik:RadPageView>
                </telerik:RadMultiPage>
            </asp:View>
        </asp:MultiView>
        <asp:Timer ID="RealADReplicationTimer" runat="server" Interval="10000" OnTick="RealADReplicationTimer_Tick" Enabled="false" /> 
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="GoToLdapLogsUpdatePanel" runat="server" RenderMode="Inline">
    <ContentTemplate>
        <br />
        <b><asp:Label ID="Step8Label" runat="server" Font-Size="Medium"  /></b>&nbsp;&nbsp;&nbsp;<asp:HyperLink ID="GoToLdapLogs" Font-Size="Medium" runat="server" />        
        <br />
        <br />
        <mits:UpdateProgress ID="GoToLdapLogsUpdateProgress" runat="server" AssociatedUpdatePanelID="GoToLdapLogsUpdatePanel" />
    </ContentTemplate>
</asp:UpdatePanel>