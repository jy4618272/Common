<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.EntityFieldsControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <contenttemplate>
        <asp:Panel ID="SearchPanel" runat="server" Style="padding-bottom: 7px; position: relative;
            left: -5px;" Width="350px">
            <mits:ComboBox ID="InstanceList" runat="server" DataTextField="Name" DataValueField="InstanceId"
                DataSourceId="InstancesDataSource" Width="100%" AutoPostBack="true" OnDataBound="InstanceList_DataBound" />
        </asp:Panel>
        <mits:CommonGridView ID="List" runat="server" DataKeyNames="EntityFieldId" DataSourceID="EntityListDataSource"
            Width="700px">
            <columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="20px" ItemStyle-Wrap="false" HeaderStyle-Width="30px">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ImageUrl='<%# DropDownListImageUrl %>' ToolTip='<%# DropDownListImageToolTip %>'
                            NavigateUrl='<%# string.Format(EntityFieldListsValuesPageUrlFormat, Eval("EntityFieldId")) %>'
                            Visible='<%# (Convert.ToInt32(Eval("EntityFieldTypeId")) != 1) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <mits:TextBoxField DataField="Name" SortExpression="Name" ItemStyle-Width="25%" />
                <mits:TextBoxField DataField="Description" ItemStyle-Width="25%" />
                <asp:TemplateField ItemStyle-Wrap="false" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Micajah.Common.Bll.Providers.EntityFieldProvider.GetDataTypeName((int)Eval("DataTypeId")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <mits:TextBoxField DataField="DefaultValue" ItemStyle-Width="10%" HeaderStyle-Wrap="false" />
                <mits:CheckBoxField DataField="AllowDBNull" SortExpression="AllowDBNull" ItemStyle-HorizontalAlign="Center" />
                <mits:CheckBoxField DataField="Unique" SortExpression="Unique" ItemStyle-HorizontalAlign="Center" />
                <mits:CheckBoxField DataField="Active" SortExpression="Active" ItemStyle-HorizontalAlign="Center" />
            </columns>
        </mits:CommonGridView>
        <mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="EntityFieldId"
            Width="550px" Visible="False">
            <fields>
                <mits:ComboBoxField DataField="EntityFieldTypeId" Required="True" DataSourceId="EntityFieldTypesDataSource"
                    DataTextField="Name" DataValueField="EntityFieldTypeId" OnControlInit="EntityFieldTypeList_ControlInit" HeaderStyle-Width="120px" ControlStyle-Width="200px">
                </mits:ComboBoxField>
                <mits:TextBoxField DataField="Name" MaxLength="255" Columns="65" ControlStyle-Width="350px" Required="True" />
                <mits:TextBoxField DataField="Description" MaxLength="255" Columns="65" Rows="3" ControlStyle-Width="350px" TextMode="MultiLine" />
                <mits:ComboBoxField DataField="DataTypeId" DataTextField="Name" DataValueField="DataTypeId" Required="True" DataSourceId="DataTypesDataSource" ControlStyle-Width="200px"
                    OnControlInit="DataTypeList_ControlInit">
                </mits:ComboBoxField>
                <mits:TextBoxField DataField="DefaultValue" MaxLength="512" Columns="65" ControlStyle-Width="350px" />
                <mits:CheckBoxField DataField="AllowDBNull" DefaultChecked="True" AutoPostBack="true" OnControlInit="AllowDBNullCheckBox_ControlInit" />
                <mits:CheckBoxField DataField="Unique" />
                <mits:TextBoxField DataField="MaxLength" MaxLength="10" Columns="10" ValidationType="Integer" MinimumValue="0" />
                <mits:TextBoxField DataField="DecimalDigits" MaxLength="10" MinimumValue="0" Columns="10" DefaultText="0" AutoPostBack="true" ValidationType="Integer"
                     OnControlInit="DecimalDigitsTextBox_ControlInit" />
                <mits:TextBoxField DataField="MinValue" MaxLength="512" Columns="65" ControlStyle-Width="350px" />
                <mits:TextBoxField DataField="MaxValue" MaxLength="512" Columns="65" ControlStyle-Width="350px" />
                <mits:TextBoxField DataField="OrderNumber" MaxLength="10" Columns="10" ValidationType="Integer" />
                <mits:CheckBoxField DataField="Active" DefaultChecked="True" />
                <mits:TemplateField>
                    <ItemTemplate>
                        <div id="ErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </ItemTemplate>
                </mits:TemplateField>
            </fields>
        </mits:MagicForm>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" DeleteMethod="DeleteEntityField"
            SelectMethod="GetEntityFields" TypeName="Micajah.Common.Bll.Providers.EntityFieldProvider"
            OnSelecting="EntityListDataSource_Selecting">
            <DeleteParameters>
                <asp:Parameter Name="entityFieldId" Type="Object" />
            </DeleteParameters>
            <SelectParameters>
                <asp:Parameter Name="entityId" Type="Object" />
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:ControlParameter Name="instanceId" Type="Object" ControlID="InstanceList" PropertyName="SelectedValue" />
                <asp:Parameter Name="active" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" InsertMethod="InsertEntityField"
            SelectMethod="GetEntityField" TypeName="Micajah.Common.Bll.Providers.EntityFieldProvider"
            UpdateMethod="UpdateEntityField" OnInserting="EntityListDataSource_Selecting"
            OnUpdating="EntityListDataSource_Selecting" OnSelected="EntityDataSource_Selected">
            <UpdateParameters>
                <asp:Parameter Name="entityFieldId" Type="Object" />
                <asp:Parameter Name="entityFieldTypeId" Type="Int32" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="dataTypeId" Type="Int32" />
                <asp:Parameter Name="defaultValue" Type="String" />
                <asp:Parameter Name="allowDBNull" Type="Boolean" />
                <asp:Parameter Name="unique" Type="Boolean" />
                <asp:Parameter Name="maxLength" Type="Int32" />
                <asp:Parameter Name="minValue" Type="String" />
                <asp:Parameter Name="maxValue" Type="String" />
                <asp:Parameter Name="decimalDigits" Type="Int32" />
                <asp:Parameter Name="orderNumber" Type="Int32" />
                <asp:Parameter Name="entityId" Type="Object" />
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:ControlParameter Name="instanceId" Type="Object" ControlID="InstanceList" PropertyName="SelectedValue" />
                <asp:Parameter Name="active" Type="Boolean" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="entityFieldId"
                    Type="Object" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="entityFieldTypeId" Type="Int32" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="dataTypeId" Type="Int32" />
                <asp:Parameter Name="defaultValue" Type="String" />
                <asp:Parameter Name="allowDBNull" Type="Boolean" />
                <asp:Parameter Name="unique" Type="Boolean" />
                <asp:Parameter Name="maxLength" Type="Int32" />
                <asp:Parameter Name="minValue" Type="String" />
                <asp:Parameter Name="maxValue" Type="String" />
                <asp:Parameter Name="decimalDigits" Type="Int32" />
                <asp:Parameter Name="orderNumber" Type="Int32" />
                <asp:Parameter Name="entityId" Type="Object" />
                <asp:Parameter Name="organizationId" Type="Object" />
                <asp:ControlParameter Name="instanceId" Type="Object" ControlID="InstanceList" PropertyName="SelectedValue" />
                <asp:Parameter Name="active" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="DataTypesDataSource" runat="server" SelectMethod="GetDataTypes"
            TypeName="Micajah.Common.Bll.Providers.EntityFieldProvider"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityFieldTypesDataSource" runat="server" SelectMethod="GetEntityFieldTypes"
            TypeName="Micajah.Common.Bll.Providers.EntityFieldProvider"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="InstancesDataSource" runat="server" SelectMethod="GetInstances"
            TypeName="Micajah.Common.Bll.Providers.InstanceProvider" OnSelecting="InstancesDataSource_Selecting">
            <SelectParameters>
                <asp:Parameter Name="organizationId" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </contenttemplate>
</asp:UpdatePanel>
