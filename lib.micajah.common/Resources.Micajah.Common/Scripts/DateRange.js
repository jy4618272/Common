function DateRange_OnChange(dateRangeId, dateStartPickerId, dateEndPickerId, validatorId) {
    var dateStartPicker = $find(dateStartPickerId);
    var dateEndPicker = $find(dateEndPickerId);
    var dateRange = $get(dateRangeId);
    if ((dateStartPicker != null) && (dateEndPicker != null)) {
        var isCustomRange = (dateRange.value == 7);
        if (!isCustomRange) {
            dateStartPicker.set_selectedDate(DateStart[dateRange.value]);
            dateEndPicker.set_selectedDate(DateEnd[dateRange.value]);
        }
        dateStartPicker.set_enabled(isCustomRange);
        dateEndPicker.set_enabled(isCustomRange);
    }
    if (typeof (ValidatorValidate) == "function") {
        var val = $get(validatorId);
        if (val != null)
            ValidatorValidate(val);
    }
}

function DateRange_ValidatorUpdateDisplay(ctl, validatorId, isValid) {
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

function DateRange_Validate(source, arguments) {
    var dateStartPicker = $find(source.getAttribute("dateStartPickerId"));
    var dateEndPicker = $find(source.getAttribute("dateEndPickerId"));
    var dateStart = dateStartPicker.get_selectedDate();
    var dateEnd = dateEndPicker.get_selectedDate();
    if (dateStart != null)
        dateStart = new Date(dateStart.getYear(), dateStart.getMonth(), dateStart.getDate(), 0, 0, 0);
    if (dateEnd != null)
        dateEnd = new Date(dateEnd.getYear(), dateEnd.getMonth(), dateEnd.getDate(), 23, 59, 59);
    var msg = "";
    if (dateStartPicker.isEmpty() || dateEndPicker.isEmpty()) {
        if (source.getAttribute("required") == "true") msg = source.getAttribute("requiredErrorMessage");
    }
    else if (dateEnd <= dateStart)
        msg = source.getAttribute("compareErrorMessage");
    arguments.IsValid = (msg.length == 0);
    source.innerHTML = msg;
    DateRange_ValidatorUpdateDisplay(dateStartPicker.get_textBox(), source.id, arguments.IsValid);
    DateRange_ValidatorUpdateDisplay(dateEndPicker.get_textBox(), source.id, arguments.IsValid);
    DateRange_ValidatorUpdateDisplay($get(source.id.replace("_cust", "_dr")), source.id, arguments.IsValid);
}
