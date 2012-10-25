Type.registerNamespace("Micajah.Common");

Micajah.Common.UpdateProgress = function (a) {
    Micajah.Common.UpdateProgress.initializeBase(this, [a]);
    this._cancelHideElement = false;
    this._changeSuccessTemplateOpacityDelegate = Function.createDelegate(this, this._changeSuccessTemplateOpacity);
    this._hideAfter = 2000;
    this._fadeOutSuccessTemplateDelegate = Function.createDelegate(this, this._fadeOutSuccessTemplate);
    this._failureTemplate = $get(this.get_element().id + '_FailureTemplate');
    this._postBackHasError = false;
    this._intervalTimerCookie = null;
    this._postBackControlId = "";
    this._progressTemplate = $get(this.get_element().id + '_ProgressTemplate');
    this._successTemplate = $get(this.get_element().id + '_SuccessTemplate');
    this._templates = [this._failureTemplate, this._progressTemplate, this._successTemplate];
    this._timeout = -1;
    this._postBackAction = $get(this.get_element().id + '_PostBackAction');
    this._postBackElement = null;
    try {
        if ((window.navigator.appVersion.indexOf('MSIE') > -1) && (this._successTemplate.style.width == ''))
            this._successTemplate.style.width = '100%';
    }
    catch (err) { }
}

Micajah.Common.UpdateProgress.prototype =
{
    get_postBackHasError: function () { return this._postBackHasError; },
    set_postBackHasError: function (a) { this._postBackHasError = a; },
    get_hideAfter: function () { return this._hideAfter; },
    set_hideAfter: function (a) { this._hideAfter = a; },
    get_postBackControlId: function () { return this._postBackControlId; },
    set_postBackControlId: function (a) { this._postBackControlId = a; },
    get_timeout: function () { return this._timeout; },
    set_timeout: function (a) { this._timeout = a; },

    get_showProgress: function () {
        var showProgress = (!this._associatedUpdatePanelId);
        if (this._postBackElement) {
            if (this._postBackControlId.length > 0) {
                showProgress = (this._postBackElement.id === this._postBackControlId);
            }
            else {
                var curElem = $get(this._postBackElement.id);
                while (!showProgress && curElem) {
                    if (curElem.id && (this._associatedUpdatePanelId === curElem.id))
                        showProgress = true;
                    curElem = curElem.parentNode;
                }
            }
        }
        return showProgress;
    },

    _clearIntervalTimerCookie: function () {
        if (this._intervalTimerCookie) {
            window.clearInterval(this._intervalTimerCookie);
            this._intervalTimerCookie = null;
        }
    },

    _clearTimerCookie: function () {
        if (this._timerCookie) {
            window.clearTimeout(this._timerCookie);
            this._timerCookie = null;
        }
    },

    _changeSuccessTemplateOpacity: function () {
        var s = this._successTemplate.style;
        var opacity = (s.opacity ? s.opacity : 1) - 0.05;
        s.opacity = opacity;
        s.MozOpacity = opacity;
        s.KhtmlOpacity = opacity;
        s.filter = 'alpha(opacity=' + (opacity * 100) + ')';
        if (opacity <= 0) {
            s.opacity = 1;
            s.MozOpacity = 1;
            s.KhtmlOpacity = 1;
            s.filter = 'alpha(opacity=100)';
            this._hideElement();
            this._clearIntervalTimerCookie();
        }
    },

    _fadeOutSuccessTemplate: function () {
        if (this._cancelHideElement) return;
        this._intervalTimerCookie = window.setInterval(this._changeSuccessTemplateOpacityDelegate, 50);
        this._clearTimerCookie();
    },

    _hideElement: function () {
        if (this.get_dynamicLayout())
            this.get_element().style.display = 'none';
        else
            this.get_element().style.visibility = 'hidden';
    },

    _showElement: function () {
        if (this.get_dynamicLayout())
            this.get_element().style.display = 'block';
        else
            this.get_element().style.visibility = 'visible';
    },

    _showTemplate: function (a) {
        var child = null;
        for (var x = 0; x < this._templates.length; x++) {
            child = this._templates[x];
            if ((child) && (a))
                child.style.display = ((child.id == a.id) ? 'block' : 'none');
            if ((child) && (!a))
                child.style.display = 'none';
        }
        this._showElement();
    },

    _startRequest: function () {
        this._cancelHideElement = true;
        this._showTemplate(this._progressTemplate);
        Micajah.Common.UpdateProgress.callBaseMethod(this, '_startRequest');
    },

    _handleBeginRequest: function (sender, args) {
        this._postBackElement = args.get_postBackElement();
        if (this.get_showProgress()) {
            if (this._timeout > -1)
                args.get_request().set_timeout(this._timeout);
            Micajah.Common.UpdateProgress.callBaseMethod(this, '_handleBeginRequest', [sender, args]);
        }
    },

    _handleEndRequest: function (sender, args) {
        this._clearTimerCookie();
        this._cancelHideElement = false;
        if (this.get_showProgress()) {
            var error = args.get_error();
            if ((error != null) || this._postBackHasError) {
                args.set_errorHandled(true);
                if (this._failureTemplate != null)
                    this._showTemplate(this._failureTemplate);
            }
            else {
                this._showTemplate(this._successTemplate);
                if (this._hideAfter > -1)
                    this._timerCookie = window.setTimeout(this._fadeOutSuccessTemplateDelegate, this._hideAfter);
            }
        }
    }
}

Micajah.Common.UpdateProgress.registerClass('Micajah.Common.UpdateProgress', Sys.UI._UpdateProgress);