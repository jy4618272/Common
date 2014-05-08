<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.GoogleIntegrationControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<script type="text/javascript">
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
<asp:HyperLink ID="hlAddApplication" runat="server" Font-Size="Medium"/>
<br /><br /><br />
<h1 style="text-decoration:underline;"><asp:Literal ID="litCaption" runat="server" /></h1>
<asp:Label ID="lblDescription" runat="server" Font-Size="Medium" />
<br />
<mits:MagicForm ID="EditForm" runat="server" />

<br />
<br />
<h1 style="text-decoration:underline;"><asp:Literal ID="lblTitle" runat="server" /></h1>   

    <asp:MultiView ID="mvConnectionSettings" runat="server">
        <asp:View ID="vwForm" runat="server">            
            <table cellspacing="0" cellpadding="0" style="border-collapse: collapse;" class="Mf_T">
                <tbody>                    
                    <tr id="trGoogleDomain" runat="server">
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
                    <tr id="trFormError" visible="false" runat="server">
                        <td colspan="2">
                            <span style="color: #000066; background-color: #FFEFAC; font-family: Arial; font-size: 18px; font-weight: bold; padding: 3px 8px 3px 8px; width:100%;">
                                <asp:Label ID="lblFromError" runat="server" Text="Error" />
                            </span>
                        </td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><asp:Button ID="btnSaveToken" runat="server" OnClick="btnSaveToken_Click" />&nbsp;<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" /></td>
                    </tr>
                </tbody>
            </table>

        </asp:View>
        <asp:View ID="vwToken" runat="server">
            <table cellspacing="0" cellpadding="0" style="border-collapse: collapse;" class="Mf_T">                
                <tr>
                    <td>
                        <asp:Label ID="lblTokenDescription" runat="server"  Font-Size="Medium" />                        
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td>
                        <asp:Button ID="btnChangeToken" runat="server" OnClick="btnChangeToken_Click" />
                    </td>
                </tr>
            </table>
                        
            <br />
            <br />            
            <h1 style="text-decoration:underline;"><asp:Literal ID="lblGoogleSetup" runat="server" /></h1>
            
            <table>
                <tr>
                    <td><b><asp:Label ID="lblYourGoogleDomain" runat="server" Font-Size="Medium" /></b></td>
                    <td><asp:DropDownList ID="ddlDomains" runat="server" /></td>
                </tr>
            </table>
            
            <br />
            <br />
            <asp:UpdatePanel ID="upStep1" runat="server" RenderMode="Inline">
                <ContentTemplate>                    
                    
                    <b><asp:Label ID="lblStep1" runat="server" Font-Size="Medium" /></b>&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lbStep1" Font-Size="Medium" runat="server" OnClick="lbStep1_Click" />
                    <br />
                    <br />
                    <mits:UpdateProgress ID="upStep1Progress" runat="server" AssociatedUpdatePanelID="upStep1"/>
                    <asp:MultiView ID="mvStep1" runat="server">
                        <asp:View ID="vwStep1Result" runat="server">
                            <mits:CommonGridView ID="gvStep1Results" runat="server" Width="620px" AllowPaging="False" AllowSorting="False" AutoGenerateColumns="true" />
                            <br />
                            <asp:LinkButton ID="lbImportUsers" Font-Size="Medium" runat="server" OnClick="lbImportUsers_Click" />                
                            <br />
                        </asp:View>
                        <asp:View ID="vwStep1Error" runat="server">
                            <span style="color: #000066; background-color: #FFEFAC; font-family: Arial; font-size: 18px; font-weight: bold; padding: 3px 8px 3px 8px; width:100%;">
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
                            <span style="color: #000066; background-color: #FFEFAC; font-family: Arial; font-size: 18px; font-weight: bold; padding: 3px 8px 3px 8px; width:100%;">
                                <asp:Label ID="lblStep2Error" runat="server" Text="Error" />
                            </span>
                            <br />
                        </asp:View>
                    </asp:MultiView>        
                </ContentTemplate>
            </asp:UpdatePanel>

        </asp:View>
    </asp:MultiView>
