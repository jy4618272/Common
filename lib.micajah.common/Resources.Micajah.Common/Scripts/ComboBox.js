function ComboBox_ValidatorUpdateDisplay(ctl, validatorId, isValid) {
    if (isValid) {
        if (ctl.getAttribute("validatorId") == validatorId) {
            ctl.className = ctl.className.replace(" Invalid", "");
            ctl.removeAttribute("validatorId");
        }
    }
    else if (ctl.className.indexOf(" Invalid") == -1) {
        ctl.className += " Invalid";
        ctl.setAttribute("validatorId", validatorId);
    }
    else {
        clientId = ctl.getAttribute("validatorId");
        if (clientId != validatorId) {
            var custval = document.getElementById(clientId);
            if (custval) custval.style.display = "none";
        }
        ctl.setAttribute("validatorId", validatorId);
    }
}

function ComboBox_Validate(source, arguments) {
    var isValid = false;
    var comboBox = $find(source.getAttribute("comboBoxId"));
    if (comboBox != null) {
        var value = null;
        if (comboBox.get_allowCustomText() == true)
            value = comboBox.get_text();
        else {
            var item = comboBox.get_selectedItem();
            if (item != null) value = item.get_value();
        }
        if (value != null) {
            var initialValue = source.getAttribute("initialValue");
            if (initialValue != null)
                isValid = (value != initialValue);
            else
                isValid = (value.replace(/^\s+$/gm, "").length > 0)
        }
        ComboBox_ValidatorUpdateDisplay(comboBox.get_element(), source.id, isValid);
    }
    arguments.IsValid = isValid;
}

function ComboBox_SelectedIndexChanged(sender, eventArgs) {
    if (typeof (ValidatorValidate) == "function") {
        var val = $get(sender.get_id() + "_val");
        if (val != null)
            ValidatorValidate(val);
    }
}

if (typeof (Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
