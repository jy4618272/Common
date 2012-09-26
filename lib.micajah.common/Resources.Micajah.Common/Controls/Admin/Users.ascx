<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.UsersControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<%@ Register Src="~/Resources.Micajah.Common/Controls/Security/ChangePassword.ascx"
    TagName="ChangePassword" TagPrefix="uc" %>
<script type="text/javascript">
    //<![CDATA[
    function NodeClicking(sender, eventArgs) {
        eventArgs.set_cancel(true);
    }
    //]]>
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:CommonGridView ID="List" runat="server" DataKeyNames="UserId" DataSourceID="EntityListDataSource"
            Width="700px" OnRowDataBound="List_RowDataBound" OnRowDeleting="List_RowDeleting">
            <captioncontrols>
                <asp:HyperLink ID="InviteUsersLink" runat="server" CssClass="Cgv_AddNew" OnInit="InviteUsersLink_Init"></asp:HyperLink>
            </captioncontrols>
            <columns>
                <mits:TextBoxField DataField="Name" SortExpression="Name" />
                <mits:TextBoxField DataField="Email" SortExpression="Email" />
                <mits:TemplateField SortExpression="LastLoginDate" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:Literal ID="LastLoginDateLiteral" runat="server"></asp:Literal>
                    </ItemTemplate>
                </mits:TemplateField>
            </columns>
        </mits:CommonGridView>
        <div id="InvitedUsersDiv" runat="server" style="padding-top: 20px;">
            <mits:CommonGridView ID="InvitedUsersList" runat="server" DataKeyNames="InvitedLoginId"
                DataSourceID="InvitedUsersListDataSource" Width="700px" AutoGenerateColumns="false"
                AllowSorting="true">
                <columns>
                    <mits:TextBoxField DataField="LoginName" SortExpression="LoginName" />
                    <mits:ButtonField CommandName="Delete" HeaderStyle-Width="80px" ItemStyle-Wrap="false"></mits:ButtonField>
                </columns>
            </mits:CommonGridView>
        </div>
        <mits:DetailMenu ID="UserDetailMenu" runat="server" Visible="false" OnItemClick="UserDetailMenu_ItemClick" />
        <mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="UserId"
            Width="550px" Visible="False" OnItemInserting="EditForm_ItemInserting" OnDataBinding="EditForm_DataBinding"
            OnDataBound="EditForm_DataBound">
            <fields>
                <mits:TextBoxField DataField="Email" MaxLength="255" Columns="65" ControlStyle-Width="350px" Required="True"
                    ValidationType="RegularExpression" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" HeaderStyle-Width="120px" />
                <mits:TextBoxField DataField="FirstName" MaxLength="255" Columns="65" ControlStyle-Width="350px" Required="True" Visible="false" HeaderStyle-Wrap="false" />
                <mits:TextBoxField DataField="MiddleName" MaxLength="255" Columns="65" ControlStyle-Width="350px" Visible="false" HeaderStyle-Wrap="false" />
                <mits:TextBoxField DataField="LastName" MaxLength="255" Columns="65" ControlStyle-Width="350px" Required="True" Visible="false" HeaderStyle-Wrap="false" />
                <mits:TextBoxField DataField="Phone" MaxLength="20" Columns="20" Visible="false" />
                <mits:TextBoxField DataField="MobilePhone" MaxLength="20" Columns="20" Visible="false" />
                <mits:TextBoxField DataField="Fax" MaxLength="20" Columns="20" Visible="false" />
                <mits:TextBoxField DataField="Title" MaxLength="30" Columns="20" Visible="false" />
                <mits:TextBoxField DataField="Department" MaxLength="255" Columns="65" ControlStyle-Width="350px" Visible="false" />
                <mits:TextBoxField DataField="Street" MaxLength="255" Columns="65" ControlStyle-Width="350px" Visible="false" />
                <mits:TextBoxField DataField="Street2" MaxLength="255" Columns="65" ControlStyle-Width="350px" Visible="false" />
                <mits:TextBoxField DataField="City" MaxLength="255" Columns="65" ControlStyle-Width="350px" Visible="false" />
                <mits:TextBoxField DataField="State" MaxLength="255" Columns="65" ControlStyle-Width="350px" Visible="false" />
                <mits:TextBoxField DataField="PostalCode" MaxLength="20" Columns="20" Visible="false" />
                <mits:ComboBoxField DataField="Country" DataSourceId="CountriesDataSource" 
                    DataTextField="Name" DataValueField="Name" ControlStyle-Width="250px" AllowCustomText="true" MarkFirstMatch="true"
                    OnControlInit="CountryList_ControlInit" Visible="false" />
                <mits:TextBoxField DataField="SecondaryEmails" MaxLength="255" Columns="65" Rows="3" TextMode="MultiLine" ControlStyle-Width="350px" Visible="false" />
                <mits:CheckBoxListField DataField="GroupId" DataSourceId="GroupDataSource"
                    DataTextField="Name" DataValueField="GroupId" Required="True" Visible="false" />
                <mits:TemplateField>
                    <ItemTemplate>
                        <div id="ErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </ItemTemplate>
                </mits:TemplateField>
            </fields>
        </mits:MagicForm>
        <uc:ChangePassword ID="PasswordForm" runat="server" Visible="False" OnPasswordUpdated="PasswordForm_PasswordUpdated"
            OnPasswordUpdateCanceled="PasswordForm_PasswordUpdated" />
        <mits:MagicForm ID="EditUserGroupsForm" runat="server" DataSourceID="UserGroupsDataSource"
            DataKeyNames="UserId" Width="550px" Visible="False" OnItemUpdated="EditForm_ItemUpdated_Generic"
            OnItemCommand="EditForm_ItemCommand_Generic">
            <fields>                
                <mits:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <mits:TreeView ID="AppGroupTreeView" runat="Server" CheckBoxes="true" Required="True" ShowRequired="False"
                            DataFieldID="GroupId" DataFieldParentID="ParentGroupId" DataTextField="Name" DataValueField="GroupId"
                            DataSourceID="AppGroupDataSource" OnClientNodeClicking="NodeClicking" OnNodeDataBound="AppGroupTreeView_NodeDataBound" />
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <div id="EditUserGroupsFormErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </ItemTemplate>
                </mits:TemplateField>
            </fields>
        </mits:MagicForm>
        <mits:MagicForm ID="EditUserActiveForm" runat="server" DataSourceID="UserActiveDataSource"
            DataKeyNames="UserId" Width="500px" Visible="False" OnDataBound="EditUserActiveForm_DataBound"
            OnItemUpdated="EditForm_ItemUpdated_Generic" OnItemCommand="EditForm_ItemCommand_Generic">
            <fields>
                <mits:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <mits:CheckBoxList ID="InstanceList" runat="server" DataSourceId="InstancesDataSource" DataTextField="Name" 
                            DataValueField="InstanceId" OnDataBound="InstanceList_DataBound"></mits:CheckBoxList>
                    </ItemTemplate>
                </mits:TemplateField>
                <mits:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <div id="EditUserActiveFormErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </ItemTemplate>
                </mits:TemplateField>
            </fields>
        </mits:MagicForm>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" DeleteMethod="RemoveUserFromOrganization"
            SelectMethod="GetUsers" TypeName="Micajah.Common.Bll.Providers.UserProvider">
            <DeleteParameters>
                <asp:Parameter Name="userId" Type="Object" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" SelectMethod="GetUserRowWithSecondaryEmails"
            TypeName="Micajah.Common.Bll.Providers.UserProvider" UpdateMethod="UpdateUser"
            InsertMethod="AddUserToOrganization">
            <SelectParameters>
                <asp:ControlParameter Name="userId" Type="Object" ControlID="List" PropertyName="SelectedValue" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="userId" Type="Object" />
                <asp:Parameter Name="email" Type="String" />
                <asp:Parameter Name="firstName" Type="String" />
                <asp:Parameter Name="lastName" Type="String" />
                <asp:Parameter Name="middleName" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="phone" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="mobilePhone" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="fax" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="title" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="department" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="street" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="street2" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="city" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="state" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="postalCode" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="country" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="secondaryEmails" Type="String" ConvertEmptyStringToNull="false" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="email" Type="String" />
                <asp:Parameter Name="firstName" Type="String" />
                <asp:Parameter Name="lastName" Type="String" />
                <asp:Parameter Name="middleName" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="phone" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="mobilePhone" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="fax" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="title" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="department" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="street" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="street2" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="city" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="state" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="postalCode" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="country" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="groupId" Type="String" ConvertEmptyStringToNull="false" />
                <asp:Parameter Name="secondaryEmails" Type="String" ConvertEmptyStringToNull="false" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="GroupDataSource" runat="server" SelectMethod="GetGroups"
            TypeName="Micajah.Common.Bll.Providers.GroupProvider">
            <SelectParameters>
                <asp:Parameter Name="includeOrganizationAdministratorGroup" Type="Boolean" DefaultValue="true" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="AppGroupDataSource" runat="server" SelectMethod="GetAppGroupsWithInstancesAndRoles"
            TypeName="Micajah.Common.Bll.Providers.LdapInfoProvider" OnSelecting="AppGroupDataSource_Selecting"
            EnableCaching="False">
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="UserGroupsDataSource" runat="server" SelectMethod="GetUserRow"
            TypeName="Micajah.Common.Bll.Providers.UserProvider" UpdateMethod="UpdateUser"
            OnUpdating="UserGroupsDataSource_Updating">
            <SelectParameters>
                <asp:ControlParameter Name="userId" Type="Object" ControlID="List" PropertyName="SelectedValue" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="userId" Type="Object" />
                <asp:Parameter Name="groupId" Type="String" ConvertEmptyStringToNull="false" />
            </UpdateParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="InvitedUsersListDataSource" runat="server" DeleteMethod="CancelInvitation"
            SelectMethod="GetInvitedLoginsByOrganizationId" TypeName="Micajah.Common.Bll.Providers.LoginProvider"
            OnObjectCreating="InvitedUsersListDataSource_ObjectCreating" OnSelecting="InvitedUsersListDataSource_Selecting"
            OnSelected="InvitedUsersListDataSource_Selected">
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="invitedLoginId" Type="Object" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="CountriesDataSource" runat="server" SelectMethod="GetCountriesView"
            TypeName="Micajah.Common.Bll.Providers.CountryProvider"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="InstancesDataSource" runat="server" SelectMethod="GetInstances"
            TypeName="Micajah.Common.Bll.Providers.InstanceProvider"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="UserActiveDataSource" runat="server" SelectMethod="GetUserRow"
            TypeName="Micajah.Common.Bll.Providers.UserProvider" UpdateMethod="UpdateUserActive"
            OnUpdating="UserActiveDataSource_Updating">
            <SelectParameters>
                <asp:ControlParameter Name="userId" Type="Object" ControlID="List" PropertyName="SelectedValue" />
                <asp:Parameter Name="includeGroups" Type="Boolean" DefaultValue="false" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="userId" Type="Object" />
                <asp:Parameter Name="instanceIdListWhereUserIsActive" Type="String" />
                <asp:Parameter Name="organizationId" Type="Object" />
            </UpdateParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
