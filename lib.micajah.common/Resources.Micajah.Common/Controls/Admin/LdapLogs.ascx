<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.LdapLogsControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:commongridview id="List" runat="server" datakeynames="LdapLogId" datasourceid="EntityListDataSource" OnDataBound="List_DataBound" width="100%">
            <columns>
                <mits:TextBoxField DataField="CreatedTime" SortExpression="CreatedTime"/>
                <mits:TextBoxField DataField="Message" SortExpression="Message"/>
                <mits:TextBoxField DataField="IsError" SortExpression="IsError"/>
            </columns>
        </mits:commongridview>

        <mits:magicform id="EditForm" runat="server" width="550px" Visible="False" />

        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" SelectMethod="GetLdapLogs" TypeName="Micajah.Common.Bll.Providers.LdapInfoProvider"/>
       
    </ContentTemplate>
</asp:UpdatePanel>
