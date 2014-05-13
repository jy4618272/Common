<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.ActiveOrganizationControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<div id="MainContainer" runat="server" style="position: absolute; width: 100%; text-align: center;
    margin-top: 35px;">
    <div id="ErrorPanel" runat="server" class="ErrorMessage" enableviewstate="false" visible="false" />
    <asp:PlaceHolder ID="OrganizationArea" runat="server">
        <p>
            <asp:Label ID="DescriptionLabel" runat="server" Font-Bold="true" /></p>
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
    </asp:PlaceHolder>
    <div id="SetupLinkContainer" runat="server" visible="false">
        <p>
            <asp:Label ID="OrLabel1" runat="server" Font-Bold="true" />&nbsp;
            <asp:HyperLink ID="SetupLink" runat="server" ForeColor="Red" />
        </p>
    </div>
    <div id="LogOnAsAnotherUserLinkContainer" runat="server" visible="false">
        <p>
            <asp:Label ID="OrLabel2" runat="server" Font-Bold="true" />&nbsp;
            <asp:HyperLink ID="LogOnAsAnotherUserLink" runat="server" ForeColor="Red" />
        </p>
    </div>
    <br />
    <br />
    <p>
        <asp:Label ID="OrLabel3" runat="server" Font-Bold="true" />&nbsp;
        <asp:LinkButton ID="LogOffLink" runat="server" OnClick="LogOffLink_Click" />
    </p>
</div>
