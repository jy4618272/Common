<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogOn2.aspx.cs" Inherits="LogOn2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="font-family: Arial;">
    <form id="form1" runat="server">
    <div style="margin: 20% 0 0 40%;">
        <asp:Login ID="Login1" runat="server" UserNameLabelText="Email:" UserNameRequiredErrorMessage="Email is required."
            OnAuthenticate="Login1_Authenticate">
        </asp:Login>
    </div>
    </form>
</body>
</html>
