<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.Reports.ActivityReportControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:CommonGridView ID="cgvList" runat="server" AutoGenerateColumns="False">
            <columns>
                <asp:BoundField DataField="InstanceName" SortExpression="1" HeaderText="Instance Name"></asp:BoundField>
                <asp:BoundField DataField="CreationDate" HeaderText="Created" DataFormatString="{0:d-MMM-yyyy}"></asp:BoundField>
                <mits:ButtonField ShowHeader="true" HeaderGroup="Administrator" DataTextField="AdminFullName" HeaderText="Full Name"></mits:ButtonField>
                <mits:ButtonField ShowHeader="true" HeaderGroup="Administrator" DataTextField="AdminEmail" HeaderText="Email"></mits:ButtonField>
                <mits:ButtonField ShowHeader="true" HeaderGroup="Administrator" DataTextField="AdminPhone" HeaderText="Phone"></mits:ButtonField>
                <asp:BoundField DataField="LastActivityDate" HeaderText="Last Activity" DataFormatString="{0:d-MMM-yyyy}"></asp:BoundField>
            </columns>
        </mits:CommonGridView>
    </ContentTemplate>
</asp:UpdatePanel>
