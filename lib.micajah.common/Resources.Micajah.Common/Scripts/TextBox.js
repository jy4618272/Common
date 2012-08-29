function TextBox_OnBlur(textBox) {
    if (textBox && ((textBox.value.replace(/^\s+$/gm, "").length == 0) || (textBox.value == (textBox.getAttribute("emptyText") || "")))) {
        textBox.value = textBox.getAttribute("emptyText") || "";
    }
}

function TextBox_OnFocus(textBox) {
    if (textBox && (textBox.value == (textBox.getAttribute("emptyText") || ""))) {
        textBox.value = "";
    }
}

function TextBox_OnKeyPress(sender, eventArgs) {
    var senderIsInput = (sender.tagName && (sender.tagName == "INPUT"));
    if ((senderIsInput ? eventArgs.keyCode : eventArgs.get_keyCode()) == 13) {
        var defaultButtonUniqueId = (senderIsInput ? sender.getAttribute("defaultButtonUniqueId") : document.getElementById(sender.get_id() + "_text").getAttribute("defaultButtonUniqueId"));
        __doPostBack(defaultButtonUniqueId, "");
        return true;
    }
    return false;
}

function TextBox_OnKeyUp(textBoxId, infoElementId) {
    var textBox = document.getElementById(textBoxId);
    var infoElement = document.getElementById(infoElementId);
    if ((textBox != null) && (infoElement != null)) {
        var currentLength = textBox.value.length;
        var maxLength = parseInt(textBox.getAttribute("maxLength"));
        if (isNaN(maxLength)) maxLength = 0;
        infoElement.style.color = ((currentLength > maxLength && maxLength > 0) ? "Red" : "");
        infoElement.innerHTML = currentLength;
    }
}

if (typeof (Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
