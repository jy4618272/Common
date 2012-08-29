if (typeof (ValidatorUpdateDisplay) == "function") {
    Micajah_Common_ValidatorUpdateDisplay = ValidatorUpdateDisplay;

    ValidatorUpdateDisplay = function (val) {
        Micajah_Common_ValidatorUpdateDisplay(val);

        if (val.style.display == "inline")
            val.style.display = "inline-block";
        if ((val.clientvalidationfunction == "Cbl_Validation") || (val.clientvalidationfunction == "DateRange_Validate"))
            return;
        var clientId = val.controltovalidate;
        if (typeof (clientId) == "string") {
            if (clientId.indexOf("_rdp") > -1)
                clientId += "_dateInput_text";
        }
        var ctl = document.getElementById(clientId);
        if (ctl == null) {
            clientId = val.getAttribute("controltovalidate2");
            ctl = document.getElementById(clientId);
        }
        if (ctl == null) ctl = document.getElementById(clientId + "_txt");
        if (ctl == null) ctl = document.getElementById(clientId + "_rtxt");
        if (ctl == null) ctl = document.getElementById(clientId + "_TextBox");
        if (ctl != null) {
            if (val.isvalid) {
                if (ctl.getAttribute("validatorId") == val.id) {
                    ctl.className = ctl.className.replace(" Invalid", "");
                    ctl.removeAttribute("validatorId");
                }
            }
            else if (ctl.className.indexOf(" Invalid") == -1) {
                ctl.className += " Invalid";
                ctl.setAttribute("validatorId", val.id);
            }
            else {
                clientId = ctl.getAttribute("validatorId");
                if (clientId != val.id) {
                    var custval = document.getElementById(clientId);
                    if (custval) custval.style.display = "none";
                }
                ctl.setAttribute("validatorId", val.id);
            }
        }
    }
}

if (typeof (Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
