<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.NodeTypeControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="SearchPanel" runat="server" Style="padding-bottom: 7px; position: relative;
            left: -5px;" Width="350px">
            <mits:ComboBox ID="InstanceList" runat="server" DataTextField="Name" DataValueField="InstanceId"
                DataSourceId="InstancesDataSource" Width="100%" AutoPostBack="true" OnDataBound="InstanceList_DataBound"
                OnSelectedIndexChanged="InstanceList_SelectedIndexChanged" />
        </asp:Panel>
        <mits:CommonGridView ID="List" runat="server" DataSourceID="EntityListDataSource"
            DataKeyNames="EntityNodeTypeId" Width="700px">
            <columns>
                <mits:TextBoxField DataField="Name"  />
                <mits:TextBoxField DataField="OrderNumber" ItemStyle-Width="100px" />
            </columns>
        </mits:CommonGridView>
        <mits:MagicForm ID="EditForm" runat="server" DataKeyNames="EntityNodeTypeId" DataSourceID="EntityDataSource"
            Width="550px" Visible="False">
            <fields>
                <mits:TextBoxField DataField="Name" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" Required="True" />
                <mits:TextBoxField DataField="OrderNumber" MaxLength="10" Columns="10" ValidationType="Integer" Required="True" />
                <mits:TemplateField>
                    <itemtemplate>
                        <div id="ErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </itemtemplate>
                </mits:TemplateField>
            </fields>
        </mits:MagicForm>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" TypeName="Micajah.Common.Bll.Providers.EntityNodeProvider"
            SelectMethod="GetEntityNodeTypesByEntityId" OnDeleted="EntityListDataSource_Deleted"
            DeleteMethod="DeleteEntityNodeType" OnSelecting="EntityListDataSource_Selecting">
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:Parameter Name="instanceId" Type="Object" />
                <asp:Parameter Name="entityId" Type="Object" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="entityNodeTypeId" Type="Object" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" TypeName="Micajah.Common.Bll.Providers.EntityNodeProvider"
            SelectMethod="GetCustomEntityNodeType" InsertMethod="InsertEntityNodeType" UpdateMethod="UpdateEntityNodeType"
            OnInserting="EntityDataSource_Inserting" OnUpdating="EntityDataSource_Updating">
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="entityNodeTypeId"
                    Type="Object" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:Parameter Name="instanceId" Type="Object" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="entityId" Type="Object" />
                <asp:Parameter Name="orderNumber" Type="Int32" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:Parameter Name="entityNodeTypeId" Type="Object" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="orderNumber" Type="Int32" />
            </UpdateParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="InstancesDataSource" runat="server" SelectMethod="GetInstances"
            TypeName="Micajah.Common.Bll.Providers.InstanceProvider" OnSelecting="InstancesDataSource_Selecting">
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
