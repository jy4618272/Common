function HtmlEditor_Validate(source, arguments) {
    var isValid = false;
    var editor = $find(source.getAttribute("htmlEditorId"));
    if (editor != null) {
        var text = editor.get_text();
        isValid = (text.replace(/^\s+$/gm, "").length > 0)
        if (isValid) {
            var initialValue = source.getAttribute("initialValue");
            if (initialValue != null)
                isValid = (text != initialValue);
        }
    }
    arguments.IsValid = isValid;
}
