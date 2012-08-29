Type.registerNamespace("Micajah.Common");

Micajah.Common.StyleSheetLoader = function(a) {
    Micajah.Common.StyleSheetLoader.initializeBase(this, [a]);
}

Micajah.Common.StyleSheetLoader.prototype =
{
    addStyleSheet: function(url) {
        var elements = document.getElementsByTagName('head');
        if (elements.length > 0) {
            var link = document.createElement("link");
            link.href = url;
            link.rel = "stylesheet";
            link.type = "text/css";
            elements[0].appendChild(link);
        }
    },

    dispose: function() {
        Micajah.Common.StyleSheetLoader.callBaseMethod(this, "dispose");
    },

    initialize: function() {
        Micajah.Common.StyleSheetLoader.callBaseMethod(this, "initialize");
    }
}

Micajah.Common.StyleSheetLoader.registerClass("Micajah.Common.StyleSheetLoader");

Micajah.Common.StyleSheetLoader.getInstance = function() {
    var sl = Micajah.Common.StyleSheetLoader._activeInstance;
    if (!sl)
        sl = Micajah.Common.StyleSheetLoader._activeInstance = new Micajah.Common.StyleSheetLoader();
    return sl;
}

if (typeof (Sys) !== "undefined") Sys.Application.notifyScriptLoaded();