<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SetupControls.ActivityReportControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:HyperLink runat="server" ID="hlExcelFile" Text="Download Report in Excel file"></asp:HyperLink><br />
        <mits:CommonGridView ID="cgvList" runat="server" AutoGenerateColumns="False">
            <columns>
                <asp:BoundField DataField="InstanceName" SortExpression="1" HeaderText="Instance Name"></asp:BoundField>
                <asp:BoundField DataField="CreationDate" HeaderText="Created" DataFormatString="{0:d-MMM-yyyy}"></asp:BoundField>
                <mits:ButtonField ShowHeader="true" HeaderGroup="Administrator" DataTextField="AdminFullName" HeaderText="Full Name"></mits:ButtonField>
                <mits:ButtonField ShowHeader="true" HeaderGroup="Administrator" DataTextField="AdminEmail" HeaderText="Email"></mits:ButtonField>
                <mits:ButtonField ShowHeader="true" HeaderGroup="Administrator" DataTextField="AdminPhone" HeaderText="Phone"></mits:ButtonField>
            </columns>
        </mits:CommonGridView>
    </ContentTemplate>
</asp:UpdatePanel>
