<%@ Control Language="C#" AutoEventWireup="true" Inherits="Micajah.Common.WebControls.SecurityControls.ActiveInstanceControl" %>
<%@ Register Namespace="Micajah.Common.WebControls" TagPrefix="mits" %>
<div id="MainContainer" runat="server" style="position: absolute; width: 100%; text-align: center;
    margin-top: 35px;">
    <div id="ErrorPanel" runat="server" class="ErrorMessage" enableviewstate="false" visible="false" />
    <asp:PlaceHolder ID="InstanceArea" runat="server">
        <p>
            <asp:Label ID="DescriptionLabel" runat="server" Font-Bold="true" /></p>
        <div id="InstanceListContainer" runat="server">
            <asp:DataList ID="InstanceList" runat="server" OnItemCommand="InstanceList_ItemCommand"
                CellSpacing="-1" CellPadding="5" HorizontalAlign="Center" Style="text-align: left;">
                <ItemTemplate>
                    <asp:LinkButton ID="Lnk" runat="server" CommandName="Select" CssClass="Large" Text='<%# Bind("Name") %>'
                        CommandArgument='<%# Bind("InstanceId") %>' />
                </ItemTemplate>
            </asp:DataList>
        </div>
    </asp:PlaceHolder>
    <br />
    <br />
    <p>
        <asp:Label ID="LogOffDescriptionLabel" runat="server" Font-Bold="true" />&nbsp;
        <asp:HyperLink ID="LogOffLink" runat="server" />
    </p>
</div>
