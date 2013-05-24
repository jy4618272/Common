<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.GoogleIntegrationControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<script language="javascript" type="text/javascript">
<!--
    var instance = Sys.WebForms.PageRequestManager.getInstance();
    instance.add_beginRequest(BeginRequestHandler);

    function BeginRequestHandler(sender, args) {
        var id = args.get_postBackElement().id;
        var errorId = id.replace("lbStep", "lblStep") + "Error";
        var error = document.getElementById(errorId);
        if (typeof (error) != 'undefined' && error != null) {
            error.innerHTML = "";
            error.style.display = "none";
            var parent = error.parentNode;
            if (typeof (parent) != 'undefined' && parent != null) {
                parent.innerHTML = "";
                parent.style.display = "none";                
            }
        }
        var progressId = id.replace("lbStep", "upStep") + "Progress_ProgressTemplate";
        var progress = document.getElementById(progressId);
        if (typeof (progress) != 'undefined' && progress != null) {
            progress.style.display = "block"; 
        }
    }
// -->
</script>
<br />
<asp:Image ID="imgGoogleAppsLogo" runat="server" />
<br />
<asp:HyperLink ID="hlGoogleAppsForBusiness" runat="server" NavigateUrl="https://www.google.com/enterprise/apps/business/" Font-Size="Medium" />
<br /><br />
<asp:HyperLink ID="hlAddSherpaDesk" runat="server" NavigateUrl="https://www.google.com/enterprise/marketplace/viewListing?productListingId=23437+1149902435229588599" Font-Size="Medium"/>
<br /><br />
<h1><asp:Literal ID="litCaption" runat="server" /></h1>
<asp:Label ID="lblDescription" runat="server" Font-Size="Medium" />
<br /><br /><br />
<mits:MagicForm ID="EditForm" runat="server" />
<table cellspacing="0" cellpadding="0" style="width: 550px; border-collapse: collapse;" class="Mf_T" id="ctl00_PageBody_Ldap1_EditForm">
    <tbody>
        <tr>
            <td class="Mf_Cpt" colspan="2">
                <asp:Literal ID="lblTitle" runat="server" />                
            </td>
        </tr>
        <tr>
            <td class="Mf_R Mf_Rf" style="width:125px;">
                <label class="Mf_Ht"><asp:Literal ID="lblGoogleDomain" runat="server" /> </label>
            </td>
            <td class="Mf_R Mf_Rf">
                <asp:TextBox ID="txtGoogleDomain" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="Mf_R">
                <label class="Mf_Ht"><asp:Literal ID="lblGoogleEmail" runat="server" /></label>
            </td>
            <td class="Mf_R">
                <asp:TextBox ID="txtGoogleEmail" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="Mf_R">
                <label class="Mf_Ht"><asp:Literal ID="lblGooglePassword" runat="server" /></label>
            </td>
            <td class="Mf_R">
                <asp:TextBox ID="txtGooglePassword" runat="server" TextMode="Password" />
            </td>
        </tr>        
    </tbody>
</table>
<br />
<br />
<table class="Mf_T" style="width: 650px; border-collapse: collapse;">
    <tr>
        <td class="Mf_Cpt" style="border-bottom: 1px solid;">
            <asp:Literal ID="lblGoogleSetup" runat="server" />
        </td>
    </tr>
</table>

<asp:UpdatePanel ID="upStep1" runat="server" RenderMode="Inline">
    <ContentTemplate>
        <br />
        <br />
        <b>
            <asp:Label ID="lblStep1" runat="server" Font-Size="Medium" /></b>&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lbStep1" Font-Size="Medium" runat="server" OnClick="lbStep1_Click" />
        <br />
        <br />
        <mits:UpdateProgress ID="upStep1Progress" runat="server" AssociatedUpdatePanelID="upStep1"/>
        <asp:MultiView ID="mvStep1" runat="server">
            <asp:View ID="vwStep1Result" runat="server">
                <mits:CommonGridView ID="gvStep1Results" runat="server" Width="620px" AllowPaging="False" AllowSorting="False" AutoGenerateColumns="true" />
                <br />
                <asp:LinkButton ID="lbImportUsers" Font-Size="Medium" runat="server" OnClick="lbImportUsers_Click" />                
            </asp:View>
            <asp:View ID="vwStep1Error" runat="server">
                <span style="color: #000066; background-color: #FFEFAC; font-family: Arial; font-size: 18px; font-weight: bold; padding: 3px 8px 3px 8px;">
                    <asp:Label ID="lblStep1Error" runat="server" Text="Error" />
                </span>
                <br />
            </asp:View>
        </asp:MultiView>        
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="upStep2" runat="server" RenderMode="Inline">
    <ContentTemplate>
        <br />
        <br />
        <b>
            <asp:Label ID="lblStep2" runat="server" Font-Size="Medium" /></b>&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lbStep2" Font-Size="Medium" runat="server" OnClick="lbStep2_Click" />
        <br />
        <br />
        <mits:UpdateProgress ID="upStep2Progress" runat="server" AssociatedUpdatePanelID="upStep2"/>
        <asp:MultiView ID="mvStep2" runat="server">
            <asp:View ID="vwStep2Result" runat="server">
                <mits:CommonGridView ID="gvStep2Results" runat="server" Width="620px" AllowPaging="False" AllowSorting="False" AutoGenerateColumns="true" OnRowDataBound="gvStep2Results_RowDataBound" />
            </asp:View>
            <asp:View ID="vwStep2Error" runat="server">
                <span style="color: #000066; background-color: #FFEFAC; font-family: Arial; font-size: 18px; font-weight: bold; padding: 3px 8px 3px 8px;">
                    <asp:Label ID="lblStep2Error" runat="server" Text="Error" />
                </span>
                <br />
            </asp:View>
        </asp:MultiView>        
    </ContentTemplate>
</asp:UpdatePanel>