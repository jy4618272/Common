function TreeView_ToggleTreeView(treeViewId) {
    var treeView = document.getElementById(treeViewId);
    treeView.style.display = (treeView.style.display == "none" ? "block" : "none");
}

function TreeView_NodeClicked(sender, eventArgs) {
    var node = eventArgs.get_node();
    var domEvent = eventArgs.get_domEvent();
    var textBox = document.getElementById(sender.get_id() + "_txt");
    var text = textBox.value;
    if (node.get_selected()) {
        if (sender.get_multipleSelect() && domEvent.ctrlKey)
            text += ", " + node.get_text();
        else {
            text = node.get_text();
            sender.get_element().style.display = "none";
        }
    }
    else
        text = text.replace(node.get_text(), "").replace(/, ,/g, ",");
    var index = text.indexOf(", ");
    if (index == 0) text = text.substr(2);
    index = text.lastIndexOf(", ");
    if (index == (text.length - 2)) text = text.substr(0, index);
    textBox.value = text;
    textBox.title = text;
}

function TreeView_DestNodeIsChild(sourceNode, destNodeValue) {
    var result = false;
    if (sourceNode != null) {
        var nodes = sourceNode.get_nodes();
        for (var i = 0; i < nodes.length; i++) {
            result = ((nodes[i].get_value() == destNodeValue) ? true : TreeView_DestNodeIsChild(nodes[i], destNodeValue));
            if (result) break;
        }
    }
    return result;
}

function TreeView_DestNodeIsParent(sourceNode, destNodeValue) {
    if (sourceNode != null) {
        var sourceNodeParent = sourceNode.get_parent();
        if (sourceNodeParent != sourceNode.get_treeView()) {
            return (sourceNodeParent.get_value() == destNodeValue);
        }
    }
    return false;
}

function TreeView_NodeDropping(sender, eventArgs) {
    var sourceNode = eventArgs.get_sourceNode();
    var destNode = eventArgs.get_destNode();
    var destNodeValue = ((destNode != null) ? destNode.get_value() : "");
    if (TreeView_DestNodeIsChild(sourceNode, destNodeValue) || TreeView_DestNodeIsParent(sourceNode, destNodeValue))
        eventArgs.set_cancel(true);
}

if (typeof (Sys) !== "undefined") Sys.Application.notifyScriptLoaded();