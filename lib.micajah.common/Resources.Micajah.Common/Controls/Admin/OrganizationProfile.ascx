<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.OrganizationProfileControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:MagicForm ID="EditForm" runat="server" OnDataBound="EditForm_DataBound" DataSourceID="EntityDataSource"
            DataKeyNames="OrganizationId" Width="550px">
            <fields>
                <mits:TextBoxField DataField="Name" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" Required="True" />
                <mits:TextBoxField DataField="Description" MaxLength="1024" Columns="65" ControlStyle-Width="350px" Rows="3"
                    TextMode="MultiLine" />
                <mits:TextBoxField DataField="WebSiteUrl" MaxLength="2048" Columns="65" ControlStyle-Width="350px" ValidationType="RegularExpression"
                    ValidationExpression="http(s)?://([\w-]+(\.)?)+[\w-]+(/[\w- ./?%&=]*)?" />
                <mits:TemplateField PaddingLeft="false">
                    <ItemTemplate>                    
                        <mits:ComboBox ID="MonthList" runat="server" DataTextField="Name" DataValueField="Value" Required="false" />
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField PaddingLeft="false">
                    <ItemTemplate>                    
                        <mits:ComboBox ID="DayList" runat="server" DataTextField="Name" DataValueField="Value" Required="false" />
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField PaddingLeft="false">
                    <ItemTemplate>                    
                        <mits:ComboBox ID="WeekStartsDayList" runat="server" DataTextField="Name" DataValueField="Value" Required="false" />
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TextBoxField DataField="EmailSuffixes" MaxLength="1024" Columns="65" Rows="3" ControlStyle-Width="350px" 
                    TextMode="MultiLine" />
                <mits:TextBoxField DataField="LdapDomains" MaxLength="2048" Columns="65" Rows="3" ControlStyle-Width="350px" 
                    TextMode="MultiLine" />               
                <mits:CheckBoxField DataField="Beta" DefaultChecked="False" />
                <mits:TemplateField>
                    <ItemTemplate>
                        <div id="ErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </ItemTemplate>
                </mits:TemplateField>
            </fields>
        </mits:MagicForm>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" SelectMethod="GetOrganization"
            TypeName="Micajah.Common.Bll.Providers.OrganizationProvider" UpdateMethod="UpdateOrganization"
            OnSelecting="EntityDataSource_Selecting" OnUpdating="EntityDataSource_Updating">
            <UpdateParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="webSiteUrl" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="fiscalYearStartMonth" Type="Int32" />
                <asp:Parameter Name="fiscalYearStartDay" Type="Int32" />
                <asp:Parameter Name="weekStartsDay" Type="Int32" />
                <asp:Parameter Name="emailSuffixes" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="ldapDomains" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="beta" Type="Boolean" />
                <asp:Parameter Name="BillingPlan" Type="Int32" />
                <asp:Parameter Name="CreditCardStatus" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
