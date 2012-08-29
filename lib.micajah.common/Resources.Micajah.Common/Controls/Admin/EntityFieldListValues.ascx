<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.EntityFieldListValuesControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:CommonGridView ID="List" runat="server" DataKeyNames="EntityFieldListValueId"
            DataSourceID="EntityListDataSource" Width="700px">
            <columns>
                <mits:TextBoxField DataField="Name" SortExpression="Name" ItemStyle-Width="45%" />
                <mits:TextBoxField DataField="Value" ItemStyle-Width="50%" />
                <mits:CheckBoxField DataField="Default" SortExpression="Default" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" />
                <mits:CheckBoxField DataField="Active" SortExpression="Active" ItemStyle-HorizontalAlign="Center" />
            </columns>
        </mits:CommonGridView>
        <mits:MagicForm ID="EditForm" runat="server" DataSourceID="EntityDataSource" DataKeyNames="EntityFieldListValueId"
            Width="550px">
            <fields>
                <mits:TextBoxField DataField="Name" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" Required="True" />
                <mits:TextBoxField DataField="Value" MaxLength="512" Columns="65" ControlStyle-Width="350px" Required="True" />
                <mits:CheckBoxField DataField="Default" />
                <mits:CheckBoxField DataField="Active" DefaultChecked="True" />
                <mits:TemplateField>
                    <itemtemplate>
                        <div id="ErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </itemtemplate>
                </mits:TemplateField>
            </fields>
        </mits:MagicForm>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" DeleteMethod="DeleteEntityFieldListValue"
            SelectMethod="GetEntityFieldListValues" TypeName="Micajah.Common.Bll.Providers.EntityFieldProvider"
            OnSelecting="EntityListDataSource_Selecting">
            <DeleteParameters>
                <asp:Parameter Name="entityFieldListValueId" Type="Object" />
            </DeleteParameters>
            <SelectParameters>
                <asp:Parameter Name="entityFieldId" Type="Object" />
                <asp:Parameter Name="active" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" InsertMethod="InsertEntityFieldListValue"
            SelectMethod="GetEntityFieldListValue" TypeName="Micajah.Common.Bll.Providers.EntityFieldProvider"
            UpdateMethod="UpdateEntityFieldListValue" OnInserting="EntityListDataSource_Selecting"
            OnUpdating="EntityListDataSource_Selecting">
            <UpdateParameters>
                <asp:Parameter Name="entityFieldListValueId" Type="Object" />
                <asp:Parameter Name="entityFieldId" Type="Object" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="value" Type="String" />
                <asp:Parameter Name="default" Type="Boolean" />
                <asp:Parameter Name="active" Type="Boolean" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="entityFieldListValueId"
                    Type="Object" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="entityFieldId" Type="Object" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="value" Type="String" />
                <asp:Parameter Name="default" Type="Boolean" />
                <asp:Parameter Name="active" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
