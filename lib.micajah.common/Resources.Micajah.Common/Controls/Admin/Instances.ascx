<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.AdminControls.InstancesControl" %>
<%@ Register TagPrefix="mits" Namespace="Micajah.Common.WebControls" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <mits:commongridview id="List" runat="server" datakeynames="InstanceId" datasourceid="EntityListDataSource"
            width="700px" OnDataBound="List_DataBound" OnRowEditing="List_RowEditing">
            <columns>
                <mits:ButtonField ButtonType="Link" DataTextField="Name" CommandName="Edit" SortExpression="Name" ItemStyle-Width="100%" ShowHeader="true" />
                <asp:CheckBoxField DataField="Active" SortExpression="Active" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
            </columns>
        </mits:commongridview>
        <mits:magicform id="EditForm" runat="server" datasourceid="EntityDataSource" datakeynames="InstanceId"
            width="550px" Visible="False">
            <fields>
                <mits:TextBoxField DataField="Name" MaxLength="255" Columns="65" ControlStyle-Width="350px" HeaderStyle-Width="120px" Required="True" />
                <mits:TextBoxField DataField="Description" MaxLength="1024" Columns="65" ControlStyle-Width="350px" Rows="3"
                    TextMode="MultiLine" />
                <mits:CheckBoxField DataField="Beta" />
                <mits:CheckBoxField DataField="Active" DefaultChecked="True" />
                <mits:TemplateField>
                    <itemtemplate>
                        <div id="ErrorDiv" runat="server" visible="false" enableviewstate="false" class="Error Block"></div>
                    </itemtemplate>
                </mits:TemplateField>
            </fields>
        </mits:magicform>
        <asp:ObjectDataSource ID="EntityListDataSource" runat="server" SelectMethod="GetInstances"
            TypeName="Micajah.Common.Bll.Providers.InstanceProvider"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="EntityDataSource" runat="server" DeleteMethod="DeleteInstance"
            InsertMethod="InsertInstance" SelectMethod="GetInstance" TypeName="Micajah.Common.Bll.Providers.InstanceProvider"
            UpdateMethod="UpdateInstance">
            <DeleteParameters>
                <asp:Parameter Name="instanceId" Type="Object" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="instanceId" Type="Object" />
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="active" Type="Boolean" />
                <asp:Parameter Name="beta" Type="Boolean" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="List" PropertyName="SelectedValue" Name="instanceId"
                    Type="Object" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="name" Type="String" />
                <asp:Parameter Name="description" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="active" Type="Boolean" />
                <asp:Parameter Name="beta" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
