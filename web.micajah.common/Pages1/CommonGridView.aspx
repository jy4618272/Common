<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CommonGridView.aspx.cs" Inherits="CommonGridViewTestPage" %>

<%@ MasterType TypeName="Micajah.Common.Pages.MasterPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageBody" runat="Server">
    <asp:PlaceHolder ID="SelectHolder" runat="server">Select a color scheme&nbsp;
        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
        </asp:DropDownList>
        <br />
        <br />
    </asp:PlaceHolder>
    <h1>
        Hierarchical grid</h1>
    <mits:CommonGridView ID="CommonGridView1" runat="server" PageSize="2" AllowPaging="True"
        AllowSorting="True" AutoGenerateDeleteButton="true" AutoGenerateEditButton="true"
        AutoGenerateColumns="false" EnableSelect="True" DataKeyNames="RoleId" DataSourceID="ObjectDataSource1"
        ShowAddLink="True" AddLinkCaption="Add New Role" Width="100%" ShowHeader="true"
        ChildControl="Panel1" Caption="Caption is not rendered if the search is enabled"
        EnableSearch="true" SearchEmptyText="Search Role" OnAction="CommonGridView1_Action">
        <CaptionControls>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="http://www.google.com"
                Target="_blank" Text="Google search"></asp:HyperLink>
            <asp:LinkButton ID="ShowInactiveButton" runat="server" Text="Show Inactive" OnClick="ShowInactiveButton_Click"></asp:LinkButton>
            <asp:LinkButton ID="ShowActiveButton" runat="server" CausesValidation="false" Text="Show Active"
                OnClick="ShowActiveButton_Click"></asp:LinkButton>
        </CaptionControls>
        <Filter>
            <div style="float: left;">
                <a href="#" class="Selected">My Checked-Out</a><asp:HyperLink ID="MyOwnedLink" runat="server"
                    NavigateUrl="#">My Owned</asp:HyperLink><a href="#">My Location</a><a href="#">Missing</a><asp:LinkButton
                        ID="BulkButton" runat="server" CausesValidation="false">Bulk</asp:LinkButton>
            </div>
            <div style="float: right;">
                <ul>
                    <li class="DropMenu">
                        <asp:HyperLink ID="ColSettingLink" runat="server" NavigateUrl="#">Columns &amp; Orders</asp:HyperLink>
                        <asp:Repeater ID="cmbColSetting" runat="server" DataSourceID="ObjectDataSource5">
                            <HeaderTemplate>
                                <ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li>
                                    <asp:LinkButton ID="SelectButton" runat="server" CausesValidation="false" CommandName="Select"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.Name") %>' CommandArgument='<%# DataBinder.Eval(Container, "DataItem.RoleId") %>'
                                        OnClick="SelectButton_Click"></asp:LinkButton>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul></FooterTemplate>
                        </asp:Repeater>
                    </li>
                </ul>
                <asp:HyperLink ID="ExportLink" runat="server" NavigateUrl="#">Export to Excel</asp:HyperLink>
                <asp:HyperLink ID="PrintLink" runat="server" NavigateUrl="#">Print</asp:HyperLink>
            </div>
        </Filter>
        <Columns>
            <mits:TextBoxField DataField="RoleId" HeaderText="RoleId">
            </mits:TextBoxField>
            <mits:TextBoxField DataField="Name" HeaderText="Name" HeaderGroup="General Info"
                SortExpression="Name">
            </mits:TextBoxField>
            <mits:TextBoxField DataField="Rank" HeaderText="Rank" HeaderGroup="General Info" ItemStyle-CssClass="Number">
            </mits:TextBoxField>
            <mits:TextBoxField DataField="StartActionId" HeaderText="StartActionId">
            </mits:TextBoxField>
            <mits:ButtonField Text="Link1" HeaderText="Link1 Caption">
            </mits:ButtonField>
            <mits:CheckBoxField DataField="BuiltIn" HeaderText="BuiltIn" CreateNewRow="True">
            </mits:CheckBoxField>
            <mits:TextBoxField DataField="Description" HeaderText="Description" ColumnSpan="6"
                CreateNewRow="True">
            </mits:TextBoxField>
            <mits:TextBoxField DataField="ShortName" HeaderText="ShortName" ColumnSpan="2" CreateNewRow="True">
            </mits:TextBoxField>
        </Columns>
    </mits:CommonGridView>
    <asp:Panel ID="Panel1" runat="server">
        <mits:CommonGridView ID="CommonGridView2" runat="server" AutoGenerateColumns="False"
            DataKeyNames="RoleId" DataSourceID="ObjectDataSource2" Width="100%" ColorScheme="Red"
            ChildControl="Panel2" Caption="Child Grid 1" ShowAddLink="true">
            <Columns>
                <mits:TextBoxField DataField="Name" HeaderText="Name">
                </mits:TextBoxField>
                <mits:TextBoxField DataField="Description" HeaderText="Description">
                </mits:TextBoxField>
                <mits:TextBoxField DataField="Rank" HeaderText="Rank">
                </mits:TextBoxField>
            </Columns>
        </mits:CommonGridView>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server">
        <mits:CommonGridView ID="CommonGridView3" runat="server" AutoGenerateColumns="False"
            DataKeyNames="RoleId" DataSourceID="ObjectDataSource3" Width="100%" Caption="Child Grid 2"
            ColorScheme="Green">
            <Columns>
                <mits:TextBoxField DataField="Name" HeaderText="Name" />
                <mits:TextBoxField DataField="Description" HeaderText="Description" />
            </Columns>
        </mits:CommonGridView>
    </asp:Panel>
    <p>
        &nbsp;
    </p>
    <mits:CommonGridView ID="CommonGridView4" runat="server" AutoGenerateColumns="false"
        DataKeyNames="RoleId" Width="100%" Caption="Empty grid">
        <Columns>
            <mits:TextBoxField DataField="RoleId" HeaderText="RoleId">
            </mits:TextBoxField>
            <mits:TextBoxField DataField="Name" HeaderText="Name">
            </mits:TextBoxField>
            <mits:TextBoxField DataField="Description" HeaderText="Description">
            </mits:TextBoxField>
            <mits:TextBoxField DataField="ShortName" HeaderText="ShortName">
            </mits:TextBoxField>
            <mits:TextBoxField DataField="Rank" HeaderText="Rank">
            </mits:TextBoxField>
            <mits:TextBoxField DataField="StartActionId" HeaderText="StartActionId">
            </mits:TextBoxField>
            <mits:CheckBoxField DataField="BuiltIn" HeaderText="BuiltIn">
            </mits:CheckBoxField>
        </Columns>
    </mits:CommonGridView>
    <p>
        &nbsp;
    </p>
    <mits:CommonGridView ID="CommonGridView5" runat="server" Width="100%" DataSourceID="ObjectDataSource4"
        AutoGenerateColumns="False" AllowSorting="true" AllowPaging="true" PageSize="5"
        ColorScheme="Yellow" ShowAddLink="true" Caption="Standard grid" AutoGenerateDeleteButton="true"
        AutoGenerateEditButton="true">
        <Columns>
            <mits:TextBoxField DataField="ActionId" HeaderText="ActionId" SortExpression="ActionId">
            </mits:TextBoxField>
            <mits:TextBoxField DataField="Name" HeaderText="Name">
            </mits:TextBoxField>
            <mits:TextBoxField DataField="Description" HeaderText="Description">
            </mits:TextBoxField>
        </Columns>
    </mits:CommonGridView>
    <p>
        &nbsp;
    </p>
    <asp:GridView ID="GridView1" runat="server" Width="100%" DataSourceID="ObjectDataSource4"
        AutoGenerateColumns="False" AllowSorting="true" AllowPaging="true" PageSize="5"
        Caption="This is GridView control that looks like CommonGridView" AutoGenerateDeleteButton="true"
        AutoGenerateEditButton="true" OnDataBound="GridView1_DataBound">
        <PagerSettings Mode="NumericFirstLast" />
        <Columns>
            <mits:TextBoxField DataField="ActionId" HeaderText="ActionId" SortExpression="ActionId">
            </mits:TextBoxField>
            <mits:TextBoxField DataField="Name" HeaderText="Name">
            </mits:TextBoxField>
            <mits:TextBoxField DataField="Description" HeaderText="Description">
            </mits:TextBoxField>
        </Columns>
    </asp:GridView>
    <p>
        &nbsp;
    </p>
    <table id="Table1" runat="server" style="width: 100%;">
        <tr class="Caption">
            <td colspan="4">
                This is HtmlTable control that looks like CommonGridView
            </td>
        </tr>
        <tr class="Header">
            <th>
                <a href="#">Column 1</a>
            </th>
            <th>
                Column 2
            </th>
            <th>
                Column 3
            </th>
            <th>
                Column 4
            </th>
        </tr>
        <tr>
            <td>
                <a href="#">Row 1 Cell 1</a>
            </td>
            <td>
                Row 1 Cell 2
            </td>
            <td>
                Row 1 Cell 3
            </td>
            <td>
                Row 1 Cell 4
            </td>
        </tr>
        <tr>
            <td>
                <a href="#">Row 2 Cell 1</a>
            </td>
            <td>
                Row 2 Cell 2
            </td>
            <td>
                Row 2 Cell 3
            </td>
            <td>
                Row 2 Cell 4
            </td>
        </tr>
        <tr>
            <td>
                <a href="#">Row 3 Cell 1</a>
            </td>
            <td>
                Row 3 Cell 2
            </td>
            <td>
                Row 3 Cell 3
            </td>
            <td>
                Row 3 Cell 4
            </td>
        </tr>
        <tr>
            <td>
                <a href="#">Row 4 Cell 1</a>
            </td>
            <td>
                Row 4 Cell 2
            </td>
            <td>
                Row 4 Cell 3
            </td>
            <td>
                Row 4 Cell 4
            </td>
        </tr>
    </table>
    <p>
        &nbsp;
    </p>
    <asp:Table ID="Table2" runat="server" Caption="This is Table control that looks like CommonGridView"
        Width="100%">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>
                <a href="#">Column 1</a>
            </asp:TableHeaderCell>
            <asp:TableHeaderCell>
                Column 2
            </asp:TableHeaderCell>
            <asp:TableHeaderCell>
                Column 3
            </asp:TableHeaderCell>
            <asp:TableHeaderCell>
                Column 4
            </asp:TableHeaderCell>
        </asp:TableHeaderRow>
        <asp:TableRow>
            <asp:TableCell>
                <a href="#">Row 1 Cell 1</a>
            </asp:TableCell>
            <asp:TableCell>
                Row 1 Cell 2
            </asp:TableCell>
            <asp:TableCell>
                Row 1 Cell 3
            </asp:TableCell>
            <asp:TableCell>
            Row 1 Cell 4
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <a href="#">Row 2 Cell 1</a>
            </asp:TableCell>
            <asp:TableCell>
                Row 2 Cell 2
            </asp:TableCell>
            <asp:TableCell>
                Row 2 Cell 3
            </asp:TableCell>
            <asp:TableCell>
            Row 2 Cell 4
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <a href="#">Row 3 Cell 1</a>
            </asp:TableCell>
            <asp:TableCell>
                Row 3 Cell 2
            </asp:TableCell>
            <asp:TableCell>
                Row 3 Cell 3
            </asp:TableCell>
            <asp:TableCell>
            Row 3 Cell 4
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <a href="#">Row 4 Cell 1</a>
            </asp:TableCell>
            <asp:TableCell>
                Row 4 Cell 2
            </asp:TableCell>
            <asp:TableCell>
                Row 4 Cell 3
            </asp:TableCell>
            <asp:TableCell>
            Row 4 Cell 4
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetRoles"
        TypeName="Micajah.Common.Bll.Providers.RoleProvider" FilterExpression="Name LIKE '%{0}%'"
        OnFiltering="ObjectDataSource1_Filtering">
        <FilterParameters>
            <asp:Parameter Name="Name" />
        </FilterParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetRoleRow"
        TypeName="Micajah.Common.Bll.Providers.RoleProvider">
        <SelectParameters>
            <asp:ControlParameter Name="roleId" Type="Object" ControlID="CommonGridView1" PropertyName="ExpandedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" SelectMethod="GetRoleRow"
        TypeName="Micajah.Common.Bll.Providers.RoleProvider">
        <SelectParameters>
            <asp:ControlParameter Name="roleId" Type="Object" ControlID="CommonGridView2" PropertyName="ExpandedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSource4" runat="server" SelectMethod="GetActions"
        TypeName="Micajah.Common.Bll.Providers.ActionProvider"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSource5" runat="server" SelectMethod="GetRoles"
        TypeName="Micajah.Common.Bll.Providers.RoleProvider"></asp:ObjectDataSource>
</asp:Content>
