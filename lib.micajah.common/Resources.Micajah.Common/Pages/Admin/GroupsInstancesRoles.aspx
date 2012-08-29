<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Admin/GroupsInstancesRoles.ascx" TagName="GroupsInstancesRoles"
    TagPrefix="uc" %>
<asp:Content ID="Ch" ContentPlaceHolderID="PageBody" runat="Server">
    <uc:GroupsInstancesRoles id="GroupsInstancesRoles1" runat="server" />
</asp:Content>
