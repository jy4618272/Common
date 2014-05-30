<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SetupControls.ActivityReportControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <contenttemplate>
        <mits:CommonGridView ID="List" runat="server" AutoGenerateColumns="False" Width="100%">
            <captioncontrols>
                <asp:HyperLink runat="server" ID="ExportLink"></asp:HyperLink>
            </captioncontrols>
            <columns>
                <asp:BoundField DataField="InstanceName" SortExpression="1"></asp:BoundField>
                <asp:BoundField DataField="CreationDate" DataFormatString="{0:d-MMM-yyyy}"  HeaderStyle-Width="100px" ItemStyle-Wrap="false"></asp:BoundField>
                <mits:ButtonField ShowHeader="true" HeaderGroup="Administrator" DataTextField="AdminFullName" HeaderStyle-CssClass="hid" ItemStyle-CssClass="hid"></mits:ButtonField>
                <mits:ButtonField ShowHeader="true" HeaderGroup="Administrator" DataTextField="AdminEmail" HeaderStyle-CssClass="hid" ItemStyle-CssClass="hid"></mits:ButtonField>
                <mits:ButtonField ShowHeader="true" HeaderGroup="Administrator" DataTextField="AdminPhone" HeaderStyle-CssClass="hid" ItemStyle-CssClass="hid"></mits:ButtonField>
            </columns>
        </mits:CommonGridView>
    </contenttemplate>
</asp:UpdatePanel>
