declare var tinymce: any;

var util = (function () {
    function clone(obj: any): any {
        return jQuery.extend({}, obj);
    }

    function copy(destination: any, source: any) {
        for (var p in source)
            destination[p] = source[p];
    }
    var inputSelectors: string = "form input, form button:not('.ignore-remove-disable'), form select, form textarea";
    function disableForm(page: IPage) {
        if (page != undefined) {
            page.disabled = true;
        }

        $(inputSelectors).attr("disabled", "true");
    }

    function enableForm(page: IPage) {
        if (page != undefined) {
            page.disabled = false;
        }

        $(inputSelectors).removeAttr("disabled");
    }

    function getNumber(source: any): number {
        if (source === undefined)
            return -1;

        var num: number = parseInt(source);
        if (isNaN(num) == true)
            return -1;

        return num;
    }

    function isNumberEqual(source: any, num: number): boolean {
        if (source === undefined)
            return false;

        var numTmp: number = parseInt(source);
        if (isNaN(numTmp) == true)
            return false;

        return numTmp === num;
    }

    function isBoolEqual(source: any, value: boolean): boolean {
        if (source === undefined)
            return false;
        if (value == true) {
            if (source === "true" || source === true)
                return true;
        } else {
            if (source === "false" || source === false)
                return true;
        }

        return false;
    }

    function getTinymceOptions(): any {
        var tinymceOptions = {
            menubar: false,
            //toolbar: 'undo redo | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent'
            toolbar: 'undo redo | bold italic forecolor | bullist numlist outdent indent | link',
            statusbar: false,
            plugins: 'autoresize textcolor autolink link',
            width: '100%',
            height: 150,
            autoresize_min_height: 100,
            autoresize_max_height: 400
        };

        return tinymceOptions;
    }

    function getDiffDays(date: Date): number {
        if (date === undefined)
            return -1;
        var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
        var todayTmp = new Date();

        var targetDate = new Date(date.getFullYear(), date.getMonth(), date.getDate());
        var today = new Date(todayTmp.getFullYear(), todayTmp.getMonth(), todayTmp.getDate());

        var diffDays = Math.round((date.getTime() - today.getTime()) / (oneDay));

        return diffDays;
    }

    function showLoginIfGuest(
        user: IUserModel,
        loginModalSvc: ILoginModalSvc,
        success?: Function): boolean {

        if (user.isGuest) {
            loginModalSvc.openLoginModal(false, true, success);
            return true;
        }

        return false;
    }

    function guid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
            s4() + '-' + s4() + s4() + s4();
    }

    function getClientToday(): string {
        var today: Date = new Date();
        var clientToday: string = today.getFullYear() + "," + (today.getMonth() + 1) + "," + today.getDate();

        return clientToday;
    }

    function getDefaultLocation(): string {
        return "si.quizzOverviews";
    }

    function editorLoad(editorControl: IEditorControl, src: any, save?: any, cancel?: any, close?: any) {
        editorControl.title = src.title;
        editorControl.textContent = src.textContent;
        editorControl.addContentType = src.addContentType;
        editorControl.imageUrl = src.imageUrl;
        editorControl.imageContent = src.imageContent;
        editorControl.externalUrl = src.externalUrl;
        editorControl.drawingContent = src.drawingContent;
        editorControl.newImageFileName = src.newImageFileName;
        editorControl.save = save;
        editorControl.cancel = cancel;
        editorControl.close = close;
        editorControl.isImageChanged = src.isImageChanged;
        
    }

    function editorClear(editorControl: IEditorControl) {
        editorControl.title = "";
        editorControl.textContent = "";
        editorControl.addContentType = 0;
        editorControl.imageUrl = "";
        editorControl.imageContent = "";
        editorControl.externalUrl = "";
        editorControl.drawingContent = "";
        editorControl.newImageFileName = "";
        editorControl.save = undefined;
        editorControl.cancel = undefined;
        editorControl.close = undefined;
        editorControl.isImageChanged = false;
    }

    function imagetEditorLoad(imageControl: IImageEditorControl, src: any, cb?: any) {
        imageControl.imageUrl = src.imageUrl;
        imageControl.imageContent = src.imageContent;
        imageControl.newImageFileName = src.newImageFileName;
        imageControl.isImageChanged = src.isImageChanged;
        imageControl.changedCb = cb;
    }

    function imageEditorClear(imageControl: IImageEditorControl) {
        imageControl.imageUrl = "";
        imageControl.imageContent = "";
        imageControl.newImageFileName = "";
        imageControl.isImageChanged = false;
        imageControl.changedCb = undefined;
    }

    return {
        clone: clone,
        copy: copy,
        disableForm: disableForm,
        enableForm: enableForm,
        getNumber: getNumber,
        isNumberEqual: isNumberEqual,
        getTinymceOptions: getTinymceOptions,
        isBoolEqual: isBoolEqual,
        getDiffDays: getDiffDays,
        showLoginIfGuest: showLoginIfGuest,
        guid: guid,
        getClientToday: getClientToday,
        getDefaultLocation: getDefaultLocation,
        editorLoad: editorLoad,
        editorClear: editorClear,
        imageEditorClear: imageEditorClear,
        imagetEditorLoad: imagetEditorLoad
    }
})();
