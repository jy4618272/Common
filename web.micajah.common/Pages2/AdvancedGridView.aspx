<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="AdvancedGridViewTestPage" Codebehind="AdvancedGridView.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <mits:AdvancedGridView ID="AdvancedGrid1" runat="server" AllowFilteringByColumn="True"
        AllowPaging="True" AllowSorting="True" ShowGroupPanel="True" AutoGenerateColumns="False"
        DataSourceID="ObjectDataSource1">
        <PagerStyle AlwaysVisible="true" />
        <MasterTableView DataSourceID="ObjectDataSource1">
            <Columns>
                <telerik:GridBoundColumn DataField="ActionId" HeaderText="ActionId" HeaderStyle-Width="100px">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Name" HeaderText="Name">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Description" HeaderText="Description">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="OrderNumber" HeaderText="Order Number" HeaderStyle-Width="100px">
                </telerik:GridBoundColumn>
                <telerik:GridCheckBoxColumn DataField="Visible" HeaderText="Visible" HeaderStyle-Width="50px">
                </telerik:GridCheckBoxColumn>
            </Columns>
        </MasterTableView>
    </mits:AdvancedGridView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetActions"
        TypeName="Micajah.Common.Bll.Providers.ActionProvider"></asp:ObjectDataSource>
</asp:Content>
