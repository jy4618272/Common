<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.Page" %>

<%@ Register Src="~/Resources.Micajah.Common/Controls/Security/SignupOrg.ascx" TagName="SignupOrg" TagPrefix="uc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
    <title></title>
</head>
<body>
    <div class="content">
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <uc:SignupOrg id="SignupOrg1" runat="server" />
        </form>
    </div>
</body>
</html>
