<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.SettingsDiagnosticControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/Settings.ascx" TagName="Settings"
    TagPrefix="uc" %>
<asp:Table ID="FormTable" runat="server" Width="550px">
    <asp:TableHeaderRow runat="server" TableSection="TableHeader">
        <asp:TableHeaderCell runat="server" ColumnSpan="2">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="Mf_Cpt">
                        <asp:Literal ID="CaptionLiteral" runat="server"></asp:Literal>
                    </td>
                    <td class="Mf_CptCtrls">
                        <asp:LinkButton ID="SwitchButton" runat="server" CausesValidation="false" OnClick="SwitchButton_Click" />
                    </td>
                </tr>
            </table>
        </asp:TableHeaderCell>
    </asp:TableHeaderRow>
    <asp:TableRow ID="UserListRow" runat="server">
        <asp:TableCell runat="server" Width="120px">
            <asp:Label ID="UserListLabel" runat="server" />
        </asp:TableCell>
        <asp:TableCell runat="server" Width="430px">
            <div style="padding-left: 5px;">
                <asp:DropDownList ID="UserList" runat="server" DataSourceID="UserListDataSource"
                    DataTextField="Email" DataValueField="UserId" Width="100%" />
            </div>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="GroupListRow" runat="server">
        <asp:TableCell runat="server" Width="120px">
            <asp:Label ID="GroupListLabel" runat="server" />
        </asp:TableCell>
        <asp:TableCell runat="server" Width="430px">
            <mits:CheckBoxList id="GroupList" runat="server" Required="True" ShowRequired="False" DataSourceID="GroupListDataSource"
                DataTextField="Name" DataValueField="GroupId" />
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="InstanceListRow" runat="server">
        <asp:TableCell runat="server" Width="120px">
            <asp:Label ID="InstanceListLabel" runat="server" />
        </asp:TableCell>
        <asp:TableCell runat="server" Width="430px">
            <div style="padding-left: 5px;">
                <asp:DropDownList ID="InstanceList" runat="server" DataSourceID="InstanceListDataSource"
                    DataTextField="Name" DataValueField="InstanceId" Width="100%" OnDataBound="InstanceList_DataBound" />
            </div>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableFooterRow runat="server">
        <asp:TableCell ID="ButtonHeaderCell" runat="server"></asp:TableCell>
        <asp:TableCell ID="ButtonCell" runat="server">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:Button ID="SubmitButton" runat="server" OnClick="SubmitButton_Click" />
                        <asp:PlaceHolder ID="ButtonsSeparator" runat="server" />
                        <asp:HyperLink ID="CancelLink" runat="server" CssClass="Cancel" />
                    </td>
                    <td id="RequiredCell" runat="server" align="right" style="padding-left: 20px;">
                    </td>
                </tr>
            </table>
        </asp:TableCell>
    </asp:TableFooterRow>
</asp:Table>
<br />
<uc:Settings ID="Settings" runat="server" DisplayedSettingLevel="Group" DiagnoseConflictingSettings="true"
    Visible="false" />
<asp:ObjectDataSource ID="UserListDataSource" runat="server" SelectMethod="GetUsers"
    TypeName="Micajah.Common.Bll.Providers.UserProvider"></asp:ObjectDataSource>
<asp:ObjectDataSource ID="GroupListDataSource" runat="server" SelectMethod="GetGroups"
    TypeName="Micajah.Common.Bll.Providers.GroupProvider"></asp:ObjectDataSource>
<asp:ObjectDataSource ID="InstanceListDataSource" runat="server" SelectMethod="GetInstances"
    TypeName="Micajah.Common.Bll.Providers.InstanceProvider"></asp:ObjectDataSource>
