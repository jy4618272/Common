if (typeof (Sys) !== "undefined") {
    //Safari 3 and Google Chrome is considered WebKit
    Sys.Browser.WebKit = {};
    if (navigator.userAgent.indexOf("WebKit/") > -1) {
        Sys.Browser.agent = Sys.Browser.WebKit;
        Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
        Sys.Browser.name = "WebKit";
    }
}

function Mp_SetLeftAreaHeight() {
    var leftArea = document.getElementById("Mp_La");
    if (leftArea != null)
        leftArea.style.height = document.body.clientHeight + "px";
}

if (navigator.userAgent.indexOf("MSIE 6.0") > -1) {
    Mp_SetLeftAreaHeight();
    window.onresize = Mp_SetLeftAreaHeight;
}

function Mp_AttachHoverEvents(containerId, tagNames, cssClasses) {
    if (navigator.userAgent.indexOf("MSIE") == -1) return;
    var Container = document.getElementById(containerId);
    if (Container) {
        var TagNamesArray = tagNames.split(",");
        var CssClasses = ((typeof (cssClasses) == "string") ? "," + cssClasses + "," : "");
        for (var x = 0; x < TagNamesArray.length; x++) {
            var Elems = Container.getElementsByTagName(TagNamesArray[x]);
            for (var y = 0; y < Elems.length; y++) {
                if (CssClasses.length > 0) {
                    var ElemClassNames = Elems[y].className.split(" ");
                    for (var z = 0; z < ElemClassNames.length; z++) {
                        if (CssClasses.indexOf("," + ElemClassNames[z] + ',') > -1) {
                            Elems[y].onmouseover = function () { this.className += " hover"; };
                            Elems[y].onmouseout = function () { this.className = this.className.replace(" hover", ""); };
                            break;
                        }
                    }
                }
                else {
                    Elems[y].onmouseover = function () { this.className += " hover"; };
                    Elems[y].onmouseout = function () { this.className = this.className.replace(" hover", ""); };
                }
            }
        }
    }
}

function Mp_AttachClickEventsToTableCells(tableId, cssClasses) {
    var Table = document.getElementById(tableId);
    if (Table) {
        var isFF = (navigator.userAgent.indexOf("Firefox") > -1);
        var CssClasses = ((typeof (cssClasses) == "string") ? "," + cssClasses + "," : "");
        var Rows = Table.rows;
        for (var x = 0; x < Rows.length; x++) {
            var Cells = Rows[x].cells;
            for (var y = 0; y < Cells.length; y++) {
                if (CssClasses.length > 0) {
                    var ElemClassNames = Cells[y].className.split(" ");
                    for (var z = 0; z < ElemClassNames.length; z++) {
                        if (CssClasses.indexOf(',' + ElemClassNames[z] + ",") > -1) {
                            if (isFF)
                                Cells[y].setAttribute("onclick", "Mp_TableCellClick(this, event);");
                            else
                                Cells[y].onclick = function () { Mp_TableCellClick(this, event); };
                            break;
                        }
                    }
                }
                else {
                    if (isFF)
                        Cells[y].setAttribute("onclick", "Mp_TableCellClick(this, event);");
                    else
                        Cells[y].onclick = function () { Mp_TableCellClick(this, event); };
                }
            }
        }
    }
}

function Mp_TableCellClick(cell, args) {
    var links = cell.getElementsByTagName("A");
    if (links.length > 0) {
        var link = links[0];
        var target = link;
        if (args) {
            target = (args.target ? args.target : args.srcElement);
            if (target.nodeType == 3) target = target.parentNode;
        }
        if (target.tagName != "A") {
            var result = true;
            if (typeof (link.onclick) == "function") result = link.onclick();
            if (result == null) result = true;
            if (result == true) window.location.href = link.href;
        }
    }
}

function Mp_Search(searchTextBoxId) {
    var searchTextBox = document.getElementById(searchTextBoxId);
    if (searchTextBox) {
        if (searchTextBox.value == searchTextBox.getAttribute("EmptyText")) return false;
        return (searchTextBox.value.replace(/^\s+$/gm, "").length > 0);
    }
    return true;
}

function Mp_SearchTextBox_OnBlur(searchTextBox) {
    if (searchTextBox && ((searchTextBox.value.replace(/^\s+$/gm, "").length == 0) || (searchTextBox.value == searchTextBox.getAttribute("EmptyText")))) {
        searchTextBox.value = searchTextBox.getAttribute("EmptyText");
        searchTextBox.style.color = "Gray";
    }
}

function Mp_SearchTextBox_OnFocus(searchTextBox) {
    if (searchTextBox && (searchTextBox.value == searchTextBox.getAttribute("EmptyText"))) {
        searchTextBox.value = "";
        searchTextBox.style.color = "Black";
    }
}

function Mp_Update(id, content, replace) {
    var elem = document.getElementById(id);
    if (elem) elem.innerHTML = content;
}

function Mp_GetPopupPositionX(width) {
    var w = (window.screenLeft != undefined ? window.screenLeft : window.screenX) + document.body.clientWidth;
    return ((w && width) ? (w - width - 1) : 600);
}

function Mp_GetPopupPositionY(event) {
    if (!event) event = window.event;
    var h = event.screenY;
    return (h ? (h - 90) : 100);
}

function Mp_EncodeTextBoxes() {
    var inputs = document.body.getElementsByTagName("input");
    var input = null;
    for (i = 0; i < inputs.length; i++) {
        input = inputs[i];
        if (input.type == "text") {
            input.disabled = true;
            input.value = input.value.replace(new RegExp('&(?!\#?[a-z0-9]+;)', 'g'), '&amp;');
            input.value = input.value.replace(new RegExp('<', 'g'), '&lt;');
            input.value = input.value.replace(new RegExp('>', 'g'), '&gt;');
            input.disabled = false;
        }
    }

    inputs = document.body.getElementsByTagName("textarea");
    for (i = 0; i < inputs.length; i++) {
        input = inputs[i];
        input.value = input.value.replace(new RegExp('&(?!\#?[a-z0-9]+;)', 'g'), '&amp;');
        input.value = input.value.replace(new RegExp('<', 'g'), '&lt;');
        input.value = input.value.replace(new RegExp('>', 'g'), '&gt;');
    }
}

function Mp_EndRequestHandler(sender, args) {
    Mp_HideOverlay();
}

function Mp_ShowOverlay() {
    document.body.style.cursor = 'wait';
    var overlay = document.getElementById("Mp_Overlay");
    if (overlay) {
        overlay.style.display = "block";
    }

    setTimeout(Mp_HideOverlay, 30000);
}

function Mp_HideOverlay() {
    document.body.style.cursor = 'auto';
    var overlay = document.getElementById("Mp_Overlay");
    if (overlay) {
        overlay.style.display = "none";
    }
}

function Mp_AttachEscapeEvents() {
    document.onkeyup = Mp_EscapePressHandler;
}

function Mp_EscapePressHandler(evt) {
    evt = (evt) ? evt : window.event
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode == 27) {
        Mp_HideOverlay();
    }
}

if (typeof (Sys) !== "undefined") {
    if (Sys.WebForms) {
        if (Sys.WebForms.PageRequestManager)
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Mp_EndRequestHandler);
    }
}