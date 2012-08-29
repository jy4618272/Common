<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Tickets.aspx.cs" Inherits="TicketsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <mits:SettingList ID="SettingList1" runat="server" />
    <div style="font-size: large;">
        <p>
            The embedded control to manage the settings is above.
        </p>
        <p>
            You can put any control below.</p>
    </div>
</asp:Content>
