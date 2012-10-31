<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.GroupsControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:CommonGridView ID="List" runat="server" DataKeyNames="GroupId" DataSourceID="EntityListDataSource"
            Width="700px" OnRowDataBound="List_RowDataBound">
            <captioncontrols>
                <asp:HyperLink ID="LdapGroupMappingsLink" runat="server" OnInit="LdapGroupMappingsLink_Init"></asp:HyperLink>
            </captioncontrols>
            <columns>
                <mits:TemplateField HeaderText="Name">
                    <itemtemplate>
                        <asp:Label ID="Name" runat="server" Text='<%# Bind("Name") %>' style="font-size:medium;" ToolTip='<%# Bind("Description") %>'></asp:Label>
                    </itemtemplate>
                </mits:TemplateField>
                <mits:HyperLinkField DataNavigateUrlFields="GroupId" ItemStyle-Width="80px" ItemStyle-Wrap="False" ControlStyle-CssClass="Command" />
            </columns>
        </mits:CommonGridView>
        <mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="GroupId"
            Width="550px" Visible="False">
            <fields>
                <mits:TextBoxField DataField="Name" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" Required="True" />
                <mits:TextBoxField DataField="Description" MaxLength="1024" Columns="65" Rows="3"
                    TextMode="MultiLine" ControlStyle-Width="350px" />
                <mits:TemplateField>
                    <itemtemplate>
                        <div id="ErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </itemtemplate>
                </mits:TemplateField>
            </fields>
        </mits:MagicForm>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" DeleteMethod="DeleteGroup"
            SelectMethod="GetGroups" TypeName="Micajah.Common.Bll.Providers.GroupProvider">
            <DeleteParameters>
                <asp:Parameter Name="groupId" Type="Object" />
            </DeleteParameters>
            <SelectParameters>
                <asp:Parameter Name="includeOrganizationAdministratorGroup" Type="Boolean" DefaultValue="true" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" InsertMethod="InsertGroup"
            SelectMethod="GetGroupRow" TypeName="Micajah.Common.Bll.Providers.GroupProvider"
            UpdateMethod="UpdateGroup">
            <UpdateParameters>
                <asp:Parameter Name="groupId" Type="Object" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="groupId"
                    Type="Object" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
