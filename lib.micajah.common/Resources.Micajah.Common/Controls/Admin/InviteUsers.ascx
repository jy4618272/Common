<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.InviteUsersControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<script type="text/javascript">
    //<![CDATA[
    function StringIsEmpty(str) {
        return (str.replace(/^\s+$/gm, '').length == 0);
    }

    function Trim(str) {
        return str.replace(/(^\s+)|(\s+$)/g, "");
    }

    function ValidateEmailList(str) {
        var Arr = str.split(',');
        var Re = new RegExp('^\\w+([-+.\u0027]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$');
        var ArrLength = Arr.length;
        var Count = 0;
        for (var i = 0; i < ArrLength; i++) {
            if (Re.test(Trim(Arr[i])))
                Count++;
            else
                break;
        }
        return ((ArrLength == Count) && (Count <= <%= MaximumEmailsPerSend %>));
    }

    function EmailValidation(source, arguments) {
        arguments.IsValid = true;
        var Elem = document.getElementById('<%= this.EmailTextBox.ClientID %>_txt');
        if (Elem) {
            var ElemValue = Elem.value;
            if (!StringIsEmpty(ElemValue))
                arguments.IsValid = ValidateEmailList(ElemValue);
        }
    }
    //]]>
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Table ID="FormTable" runat="server" Width="550px">
            <asp:TableHeaderRow runat="server" TableSection="TableHeader">
                <asp:TableHeaderCell runat="server">
                    <asp:Literal ID="CaptionLiteral" runat="server"></asp:Literal>
                </asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">
                    <div style="padding-left: 5px;">
                        <asp:Literal ID="DescriptionLabel" runat="server"></asp:Literal>
                    </div>
                    <div style="padding: 20px 0 0 5px;">
                        <asp:Label ID="EmailLabel" runat="server" Font-Bold="true" Style="display: inline;"></asp:Label>&nbsp;
                        <asp:Literal ID="EmailHintLabel" runat="server"></asp:Literal>
                    </div>
                    <mits:TextBox ID="EmailTextBox" runat="server" Columns="93" Width="475px" Required="True"
                        ShowRequired="False" Rows="6" TextMode="MultiLine" />
                    <asp:CustomValidator ID="EmailValidator" runat="server" Display="Dynamic" CssClass="Error"
                        ClientValidationFunction="EmailValidation" Style="padding-left: 5px;" />
                    <div style="padding: 20px 0 0 5px;">
                        <asp:Label ID="GroupsLabel" runat="server" Font-Bold="true" Style="display: inline;"></asp:Label>
                    </div>
                    <mits:CheckBoxList ID="GroupList" runat="server" DataSourceId="GroupDataSource" DataTextField="Name"
                        DataValueField="GroupId" Required="True" ShowRequired="False" />
                    <div style="padding: 20px 0 0 5px;">
                        <asp:Label ID="MessageLabel" runat="server" Font-Bold="true" Style="display: inline;"></asp:Label>&nbsp;
                        <asp:Literal ID="MessageHintLabel" runat="server"></asp:Literal>
                    </div>
                    <mits:TextBox ID="MessageTextBox" runat="server" Columns="93" Width="475px" Rows="6"
                        TextMode="MultiLine" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableFooterRow ID="FooterRow" runat="server">
                <asp:TableCell runat="server">
                    <asp:Button ID="SendButton" runat="server" OnClick="SendButton_Click" />
                    <asp:PlaceHolder ID="ButtonsSeparator" runat="server" />
                    <asp:HyperLink ID="CancelLink" runat="server" CssClass="Cancel" />
                </asp:TableCell>
            </asp:TableFooterRow>
        </asp:Table>
        <asp:ObjectDataSource ID="GroupDataSource" runat="server" SelectMethod="GetGroups"
            TypeName="Micajah.Common.Bll.Providers.GroupProvider">
            <SelectParameters>
                <asp:Parameter Name="includeOrganizationAdministratorGroup" Type="Boolean" DefaultValue="true" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
