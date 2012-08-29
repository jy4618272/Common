<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SetupControls.WebsitesControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<script type="text/javascript">
    //<![CDATA[
    function StringIsEmpty(str) {
        return (str.replace(/^\s+$/gm, '').length == 0);
    }

    function StringIsUrlList(str) {
        var Arr = str.split('\n');
        var Re = new RegExp('^http(s)?://([\\w-]+(\\.)?)+[\\w-]+(:\\d{1,9})?(/)?$');
        var ArrLength = Arr.length;
        var Count = 0;
        for (var i = 0; i < ArrLength; i++) {
            if (Re.test(Arr[i].replace(/\r/g, '')))
                Count++;
            else
                break;
        }
        return (ArrLength == Count);
    }
    //]]>
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:CommonGridView ID="List" runat="server" DataKeyNames="WebSiteId" DataSourceID="EntityListDataSource"
            Width="960px">
            <columns>
                <asp:TemplateField SortExpression="Name" ItemStyle-Width="25%">
                    <itemtemplate>
                        <%# GetHyperlink(Micajah.Common.Bll.Providers.WebsiteProvider.GetWebsiteUrl((Guid)Eval("WebSiteId")), Eval("Name").ToString())%>
                    </itemtemplate>
                </asp:TemplateField>
                <mits:TextBoxField DataField="Description" ItemStyle-Width="75%" />
            </columns>
        </mits:CommonGridView>
        <mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="WebSiteId"
            Width="550px" Visible="False" OnDataBound="EditForm_DataBound">
            <fields>
                <mits:TextBoxField DataField="Name" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" Required="True" />
                <mits:TemplateField PaddingLeft="false">
                    <itemtemplate>
                        <mits:TextBox ID="UrlTextBox" runat="server" MaxLength="2048" Columns="65" Width="350px" Required="True"
                            Rows="5" TextMode="MultiLine" Text='<%# Bind("Url") %>' ValidationGroup="<%# EditForm.ClientID %>" />
                        <asp:CustomValidator ID="UrlValidator" runat="server" Display="Dynamic" CssClass="Error" 
                            ClientValidationFunction="UrlValidation" ValidationGroup="<%# EditForm.ClientID %>"
                            ErrorMessage="<%# UrlValidatorErrorMessage %>" />
                    </itemtemplate>
                </mits:TemplateField>
                <mits:TextBoxField DataField="Description" MaxLength="1024" Columns="65" ControlStyle-Width="350px" Rows="3"
                    TextMode="MultiLine" />
                <mits:TextBoxField DataField="AdminContactInfo" MaxLength="2048" Columns="65" ControlStyle-Width="350px"
                    Rows="5" TextMode="MultiLine" />
            </fields>
        </mits:MagicForm>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" DeleteMethod="DeleteWebsite"
            SelectMethod="GetWebsites" TypeName="Micajah.Common.Bll.Providers.WebsiteProvider">
            <DeleteParameters>
                <asp:Parameter Name="websiteId" Type="Object" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" InsertMethod="InsertWebsite"
            SelectMethod="GetWebsiteRow" TypeName="Micajah.Common.Bll.Providers.WebsiteProvider"
            UpdateMethod="UpdateWebsite">
            <UpdateParameters>
                <asp:Parameter Name="websiteId" Type="Object" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="url" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="adminContactInfo" Type="String" ConvertEmptyStringToNull="False" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="websiteId"
                    Type="Object" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="url" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="adminContactInfo" Type="String" ConvertEmptyStringToNull="False" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
