<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.RulesEngineControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:CommonGridView ID="List" runat="server" DataKeyNames="Id" Width="700px" DataSourceID="EntityListDataSource">
            <columns>
                <mits:HyperLinkField DataNavigateUrlFields="Id" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                <mits:TextBoxField DataField="Name" SortExpression="Name" />
                <mits:TextBoxField DataField="DisplayName" SortExpression="DisplayName" />
            </columns>
        </mits:CommonGridView>
        <mits:MagicForm ID="EditForm" runat="server">
        </mits:MagicForm>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" SelectMethod="GetRulesEngines"
            TypeName="Micajah.Common.Bll.Providers.RuleEngineProvider"></asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
