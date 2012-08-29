<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.LogOffControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<div id="MainContainer" runat="server" style="position: absolute; width: 100%; text-align: center;
    margin-top: 35px;">
    <div id="ErrorDiv" runat="server" class="ErrorMessage" enableviewstate="false" visible="false" />
    <p>
        <asp:Label ID="TitleLabel" runat="server" Font-Bold="true" /></p>
    <asp:Button ID="LogOffLink" runat="server" CssClass="Btn" OnClick="LogOffLink_Click" />
    <asp:PlaceHolder ID="InstanceArea" runat="server">
        <p>
            <asp:Label ID="InstanceDescriptionLabel" runat="server" Font-Bold="true" /></p>
        <div id="InstanceListContainer" runat="server">
            <asp:DataList ID="InstanceList" runat="server" OnItemCommand="InstanceList_ItemCommand"
                CellSpacing="-1" CellPadding="5" HorizontalAlign="Center" Style="text-align: left;">
                <ItemTemplate>
                    <asp:LinkButton ID="Lnk" runat="server" CommandName="Select" CssClass="Large" Text='<%# Bind("Name") %>'
                        CommandArgument='<%# Bind("InstanceId") %>' />
                </ItemTemplate>
            </asp:DataList>
        </div>
        <br />
        <br />
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="OrganizationArea" runat="server">
        <p>
            <asp:Label ID="OrganizationLabel" runat="server" Font-Bold="true" /></p>
        <div id="OrganizationListContainer" runat="server">
            <asp:DataList ID="OrganizationList" runat="server" OnItemDataBound="OrganizationList_ItemDataBound"
                OnItemCommand="OrganizationList_ItemCommand" CellSpacing="-1" CellPadding="5"
                HorizontalAlign="Center" Style="text-align: left;">
                <ItemTemplate>
                    <asp:LinkButton ID="OrgButton" runat="server" CommandName="Select" CssClass="Large"
                        Text='<%# Bind("Name") %>' CommandArgument='<%# Bind("OrganizationId") %>' />
                    <asp:HyperLink ID="OrgLink" runat="server" Visible="false" CssClass="Large" Text='<%# Bind("Name") %>'></asp:HyperLink>
                    &nbsp;
                    <asp:HyperLink ID="ExpirationLink" runat="server" Visible="false" ForeColor="Gray"></asp:HyperLink>
                    <asp:Label ID="ExpiredLabel" runat="server" Visible="false" ForeColor="Gray"></asp:Label>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <br />
        <br />
    </asp:PlaceHolder>
</div>
