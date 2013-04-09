<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.InstancesControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:commongridview id="List" runat="server" datakeynames="InstanceId" datasourceid="EntityListDataSource"
            width="700px" OnDataBound="List_DataBound" OnRowEditing="List_RowEditing">
            <columns>
                <mits:ButtonField ButtonType="Link" DataTextField="Name" CommandName="Edit" SortExpression="Name" ItemStyle-Width="100%" ShowHeader="true" />
                <asp:CheckBoxField DataField="Active" SortExpression="Active" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
            </columns>
        </mits:commongridview>
        <mits:magicform id="EditForm" runat="server" datasourceid="EntityDataSource" datakeynames="InstanceId"
            width="550px" Visible="False">
            <fields>
                <mits:TextBoxField DataField="Name" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" Required="True" />                
                <mits:TemplateField PaddingLeft="false">
                    <ItemTemplate>
                        <table cellpadding="0" cellspacing="0">
                            <tr style="vertical-align: top;">
                                <td><mits:TextBox ID="PartialCustomUrlTextBox" runat="server" MaxLength="1024" Columns="16" Width="110px" ValidationGroup="<%# EditForm.ClientID %>" ValidationType="RegularExpression" ValidationExpression="[\w-]+" /></td>
                                <td style="padding: 7px 0 0 5px;">&nbsp;.</td>
                                <td style="padding: 7px 0 0 5px;"><%= Micajah.Common.Configuration.FrameworkConfiguration.Current.WebApplication.CustomUrl.PartialCustomUrlRootAddressesFirst%></td>                                        
                            </tr>
                        </table>
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField>
                    <ItemTemplate>
                        <table cellpadding="0" cellspacing="0">
                            <tr style="vertical-align: top;">
                                <td><asp:DropDownList id="TemplateList" runat="server"  DataSourceId="InstanceListDataSource" DataTextField="Name" DataValueField="InstanceId" Width="350px" /></td>
                            </tr>
                        </table>                        
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TextBoxField DataField="Description" MaxLength="1024" Columns="65" ControlStyle-Width="350px" Rows="3" TextMode="MultiLine" />                
                <mits:CheckBoxField DataField="Beta" />
                <mits:CheckBoxField DataField="Active" DefaultChecked="True" />
                <mits:TemplateField PaddingLeft="false" HeaderText="Custom Billing">
                    <ItemTemplate>                    
                        <mits:ComboBox ID="cmbBillingPlan" runat="server" Required="false">
                            <Items>
                                <telerik:RadComboBoxItem runat="server" Text="Enabled" Value="1" />
                                <telerik:RadComboBoxItem runat="server" Text="Disabled" Value="0" />
                            </Items>
                        </mits:ComboBox>
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField>
                    <itemtemplate>
                        <div id="ErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </itemtemplate>
                </mits:TemplateField>
            </fields>
        </mits:magicform>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" SelectMethod="GetInstances"
            TypeName="Micajah.Common.Bll.Providers.InstanceProvider"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" DeleteMethod="DeleteInstance"
            InsertMethod="InsertInstance" SelectMethod="GetInstance" TypeName="Micajah.Common.Bll.Providers.InstanceProvider"
            UpdateMethod="UpdateInstance" OnInserting="EntityDataSource_Inserting">
            <DeleteParameters>
                <asp:Parameter Name="instanceId" Type="Object" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="instanceId" Type="Object" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="active" Type="Boolean" />
                <asp:Parameter Name="beta" Type="Boolean" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="instanceId"
                    Type="Object" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="active" Type="Boolean" />
                <asp:Parameter Name="beta" Type="Boolean" />
                <asp:Parameter Name="vanityUrl" Type="String" />
                <asp:Parameter Name="templateInstanceId" Type="Object" />
            </InsertParameters>
        </asp:ObjectDataSource>

        <asp:ObjectDataSource ID="InstanceListDataSource" runat="server" SelectMethod="GetTemplateInstances" TypeName="Micajah.Common.Bll.Providers.InstanceProvider"/>
    </ContentTemplate>
</asp:UpdatePanel>
