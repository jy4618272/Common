<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="MessageListTestPage" Codebehind="MessageList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <style type="text/css">
        .hdr
        {
            padding: 2px 2px 2px 2px;
        }
        .sbj
        {
            padding: 2px 2px 2px 2px;
            text-align: center;
        }
        .txt
        {
            padding: 2px 2px 10px 2px;
        }
    </style>
    <div style="padding-bottom: 5px; font-size: 14pt; font-weight: bold;">
        Response Message
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        End Users:
                    </td>
                    <td>
                        <mits:CheckBoxList ID="UserList" runat="server" DataSourceID="UserListDataSource"
                            DataTextField="FullName" DataValueField="UserId" RepeatColumns="10" />
                    </td>
                </tr>
            </table>
            <asp:TextBox ID="MessageTextBox" runat="server" TextMode="MultiLine" Columns="110"
                Rows="8"></asp:TextBox>
            <div style="padding-top: 7px;">
                <asp:Button ID="SubmitButton" runat="server" Text="Send Response" OnClick="SubmitButton_Click"
                    Font-Bold="true" Font-Size="10pt" />
            </div>
            <br />
            <mits:MessageList ID="MessageList1" runat="server" LocalObjectType="Ticket" LocalObjectId="e28c6e7cadd84ccd8b98b2a6031156d4"
                Width="790px" DateTimeHoursOffset="3" DateTimeFormatString="{0:dd\/mm\/yyyy hh:MM} (UTC3)">
                <SubjectStyle BackColor="#3D3D8D" ForeColor="White" Font-Bold="true" CssClass="sbj" />
                <HeaderStyle BackColor="#cccccc" ForeColor="Black" CssClass="hdr" />
                <TextStyle BackColor="White" ForeColor="#003300" CssClass="txt" />
            </mits:MessageList>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="UserListDataSource" runat="server" SelectMethod="GetUsers"
        TypeName="Micajah.Common.Bll.Providers.UserProvider"></asp:ObjectDataSource>
</asp:Content>
