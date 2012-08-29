<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SetupControls.RolesControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <contenttemplate>
        <mits:CommonGridView ID="List" runat="server" DataKeyNames="RoleId" AutoGenerateColumns="false" EnableSelect="true" 
            DataSourceID="EntityListDataSource" Width="960px" OnAction="List_Action" OnRowDataBound="List_RowDataBound">
            <columns>
                <mits:TextBoxField DataField="Name" SortExpression="Name" ItemStyle-Width="25%" />
                <mits:TextBoxField DataField="Description" ItemStyle-Width="60%" />
                <mits:TextBoxField DataField="ShortName" SortExpression="ShortName" />
                <mits:TextBoxField DataField="Rank" SortExpression="Rank" />
                <mits:TemplateField ItemStyle-Wrap="False">
                    <itemtemplate>
                        <%# GetUrl(Eval("RoleId"), Eval("StartActionId"))%>
                    </itemtemplate>
                </mits:TemplateField>
            </columns>
        </mits:CommonGridView>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" SelectMethod="GetRoles"
            TypeName="Micajah.Common.Bll.Providers.RoleProvider" OnSelected="EntityListDataSource_Selected">
        </asp:ObjectDataSource>
    </contenttemplate>
</asp:UpdatePanel>
