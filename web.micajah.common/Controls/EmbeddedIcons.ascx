<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_EmbeddedIcons" Codebehind="EmbeddedIcons.ascx.cs" %>
<style type="text/css">
    .IconsList img
    {
        width: <%= (int)this.IconSize %>px;
        height: <%= (int)this.IconSize %>px;
    }
</style>

<script type="text/javascript">
    //<![CDATA[
    function ContextMenuItemClicked(sender, eventArgs) {
        var item = eventArgs.get_item();
        var itemValue = item.get_value();
        var itemText = item.get_text();
        var textToCopy = "";
        if (itemValue == "CopyFilename")
            textToCopy = eventArgs.get_targetElement().title;
        else if (itemValue == "CopyCode")
            textToCopy = "Micajah.Common.Bll.Providers.ResourceProvider.GetIconImageUrl(\"" + eventArgs.get_targetElement().title + "\", Micajah.Common.WebControls.IconSize.<%= this.IconSize %>, true)";
        sender.close();
        window.setTimeout(function() { window.prompt(itemText, textToCopy); }, 500);
    }

    function preventBrowserContextMenu(args) {
        var clickType = 0;
        if (parseInt(navigator.appVersion) > 3)
            clickType = ((navigator.appName == "Netscape") ? args.which : args.button);
        if (clickType != 1) {
            var target = (args.target ? args.target : args.srcElement);
            if (target) {
                if (target.tagName != "IMG") {
                    args.cancelBubble = true;
                    args.returnValue = false;
                    return false;
                }
            }
        }
        return true;
    }
    //]]>
</script>

<asp:DataList ID="IconsList" runat="server" CssClass="IconsList" CellSpacing="3"
    CellPadding="3" RepeatColumns="29" RepeatDirection="Horizontal" OnItemDataBound="IconsList_ItemDataBound"
    oncontextmenu="return preventBrowserContextMenu(event);">
    <ItemTemplate>
        <asp:Image ID="Icon" runat="server" />
    </ItemTemplate>
</asp:DataList>
<telerik:RadContextMenu ID="IconMenu" runat="server" OnClientItemClicked="ContextMenuItemClicked">
    <items>
       <telerik:RadMenuItem Value="CopyFilename" Text="Copy Icon file name" />
       <telerik:RadMenuItem Value="CopyCode" Text="Copy C# code to get the icon URL" />
    </items>
    <targets>
       <telerik:ContextMenuTagNameTarget TagName="img" />
    </targets>
</telerik:RadContextMenu>