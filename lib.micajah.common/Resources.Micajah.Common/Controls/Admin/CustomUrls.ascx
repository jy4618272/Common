<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.CustomUrlsControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<script type="text/javascript">
    //<![CDATA[
    function StringIsEmpty(str) {
        return (str.replace(/^\s+$/gm, '').length == 0);
    }
    //]]>
</script>
<style type="text/css">
    #ctl00_PageBody_Cu_EditForm_PartialCustomUrlTextBox_rgxp
    {
        position: static !important;
    }
    #ctl00_PageBody_Cu_VanityUrlTextBox_rgxp
    {
        position: static !important;
    }
</style>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

        <asp:MultiView ID="CustomUrlsMultiView" runat="server">
            <asp:View ID="SimpleView" runat="server">               
                <h1><asp:Literal ID="SimpleViewTitleLabel" runat="server"/></h1>                
                <table>                    
                    <tr>
                        <td style="width:110px"><mits:TextBox ID="VanityUrlTextBox" runat="server" MaxLength="1024" Columns="16" Width="110px" ValidationGroup="<%# SimpleView.ClientID %>" ValidationType="RegularExpression" ValidationExpression="[\w-]+" /> </td>
                        <td style="padding-top: 7px;" valign="top"><asp:Label ID="VanityUrlDomainLabel" runat="server" Font-Size="14px" /></td>
                    </tr>               
                    <tr>
                        <td colspan="2">
                            <asp:CustomValidator ID="SimpleViewCustomValidator" runat="server" CssClass="Error Block" Display="Dynamic" ValidateEmptyText="true" ValidationGroup="<%# SimpleView.ClientID %>" ClientValidationFunction="ValidateCustomUrls"></asp:CustomValidator>
                            <div id="SimpleErrorPanel" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                        </td>
                    </tr>     
                    <tr>
                        <td colspan="2"><br /><asp:Button ID="SimpleViewSaveButton" runat="server" OnClick="SimpleViewSaveButton_Click" CausesValidation="true"  ValidationGroup="<%# SimpleView.ClientID %>" /></td>
                    </tr>
                </table>                          
            </asp:View>
            <asp:View ID="AdvancedView" runat="server">
                <asp:Panel ID="DnsAddressPanel" runat="server">
                    <p>
                        <asp:Label ID="DnsAddressCaptionLabel" runat="server" Font-Size="11px"></asp:Label>&nbsp;&nbsp;<asp:Label
                            ID="DnsAddressLabel" runat="server" Font-Size="12px" Font-Bold="true"></asp:Label>
                    </p>
                </asp:Panel>
                <mits:CommonGridView ID="List" runat="server" DataKeyNames="CustomUrlId" DataSourceID="EntityListDataSource"
                    Width="700px">
                    <columns>
                        <mits:TextBoxField DataField="Name" SortExpression="Name" />
                        <mits:TextBoxField DataField="FullCustomUrl" SortExpression="FullCustomUrl" />
                        <mits:TextBoxField DataField="PartialCustomUrl" SortExpression="PartialCustomUrl" />
                    </columns>
                </mits:CommonGridView>
                <mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="CustomUrlId"
                    Width="550px" Visible="False" OnDataBound="EditForm_DataBound">
                    <fields>
                        <mits:TemplateField PaddingLeft="false" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <asp:Label ID="NameLabel" runat="server" style="padding-left: 5px;" Visible="false"></asp:Label>
                                <mits:ComboBox ID="InstanceList" runat="server" DataSourceId="InstanceListDataSource" 
                                    DataTextField="Name" DataValueField="InstanceId" Width="350px" OnDataBound="InstanceList_DataBound" />
                            </ItemTemplate>
                        </mits:TemplateField>
                        <mits:TemplateField PaddingLeft="false">
                            <ItemTemplate>
                                <mits:TextBox ID="FullCustomUrlTextBox" runat="server" MaxLength="1024" Columns="50" Width="350px"
                                    Text='<%# Bind("FullCustomUrl") %>' ValidationGroup="<%# EditForm.ClientID %>"
                                    ValidationType="RegularExpression" ValidationExpression="[\w-\.]+\.\w+" />
                            </ItemTemplate>
                        </mits:TemplateField>
                        <mits:TemplateField PaddingLeft="false">
                            <ItemTemplate>
                                <table cellpadding="0" cellspacing="0">
                                    <tr style="vertical-align: top;">
                                        <td><mits:TextBox ID="PartialCustomUrlTextBox" runat="server" MaxLength="1024" Columns="16" Width="110px" ValidationGroup="<%# EditForm.ClientID %>"
                                            ValidationType="RegularExpression" ValidationExpression="[\w-]+" /></td>
                                        <td style="padding: 7px 0 0 5px;">&nbsp;.</td>
                                        <td><mits:ComboBox ID="RootAddressesList" runat="server" Width="230px" /></td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </mits:TemplateField>
                        <mits:TemplateField>
                            <ItemTemplate>
                                <asp:CustomValidator ID="CustomUrlsValidator" runat="server" CssClass="Error Block" Display="Dynamic" ErrorMessage="<%# CustomUrlsValidatorErrorMessage %>"
                                    ValidateEmptyText="true" ValidationGroup="<%# EditForm.ClientID %>" ClientValidationFunction="ValidateCustomUrls"></asp:CustomValidator>
                                <div id="ErrorPanel" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                            </ItemTemplate>
                        </mits:TemplateField>
                    </fields>
                </mits:MagicForm>
                <asp:ObjectDataSource ID="EntityListDataSource" runat="server" DeleteMethod="DeleteCustomUrl"
                    SelectMethod="GetCustomUrls" TypeName="Micajah.Common.Bll.Providers.CustomUrlProvider"
                    OnSelecting="EntityListDataSource_Selecting">
                    <SelectParameters>
                        <asp:Parameter Name="organizationId" Type="Object" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="customUrlId" Type="Object" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="EntityDataSource" runat="server" InsertMethod="InsertCustomUrl"
                    SelectMethod="GetCustomUrl" TypeName="Micajah.Common.Bll.Providers.CustomUrlProvider"
                    UpdateMethod="UpdateCustomUrl" OnInserting="EntityDataSource_Inserting" OnUpdating="EntityDataSource_Updating">
                    <UpdateParameters>
                        <asp:Parameter Name="customUrlId" Type="Object" />
                        <asp:Parameter Name="fullCustomUrl" Type="String" />
                        <asp:Parameter Name="partialCustomUrl" Type="String" />
                    </UpdateParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="customUrlId"
                            Type="Object" />
                    </SelectParameters>
                    <InsertParameters>
                        <asp:Parameter Name="organizationId" Type="Object" />
                        <asp:Parameter Name="instanceId" Type="Object" />
                        <asp:Parameter Name="fullCustomUrl" Type="String" />
                        <asp:Parameter Name="partialCustomUrl" Type="String" />
                    </InsertParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="InstanceListDataSource" runat="server" SelectMethod="GetInstances"
                    TypeName="Micajah.Common.Bll.Providers.InstanceProvider" OnSelecting="InstanceListDataSource_Selecting">
                    <SelectParameters>
                        <asp:Parameter Name="organizationId" Type="Object" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
        </asp:MultiView>
        <br />
        <asp:LinkButton ID="ChangeViewButton" runat="server" OnClick="ChangeViewButton_Click" CausesValidation="false" />
    </ContentTemplate>
</asp:UpdatePanel>
