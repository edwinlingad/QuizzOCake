interface JQuery {
    literallycanvas: any;
};

interface ILC {
    init: any;
    renderSnapshotToImage: any;
};

var LC: ILC;

// contentType:
// 0 - none
// 1 - image only
// 2 - drawing only
// 3 - drawing with image background
// 4 - embed youtube video

interface IEditorControl extends ITrackModify {
    title?: string;
    textContent?: string;
    addContentType?: number;
    imageUrl?: string;
    externalUrl?: string;
    imageContent?: string;
    drawingContent?: any;

    newImageFileName?: string;
    isImageChanged?: boolean;

    // callbacks
    save?: any;
    cancel?: any;
    close?: any;

    // internals
    callSave?: any;

    // for view
    isOpen?: boolean;
    isViewOpen?: boolean;
    isOpenIf?: boolean;
    isViewOpenIf?: boolean;
}

module qzEditor {
    export interface IScope {
        id: string;
        editorControl: IEditorControl;
        item: any;
        noTitle: any;
        noActions: any;
        noVideo: any;
        textLabel: string;
        
        page: IPage;
        noTitleControl: boolean;
        noVideoControl: boolean;
        noActionControls: boolean;
        vars: any;
        $on: any;

        addType: number;
        changeAddType(aType: number): void;
        changed(): void;
        save(): void;
        cancel(): void;

        tinymceOptions: any;

        // pix
        imageClick(): void;
        imageChanged(): void;
    }
}

(function () {
    l2lApp.directive("qzEditor", qzEditor);

    function qzEditor() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

        function controller(
            $scope: qzEditor.IScope,
            $state: ng.ui.IStateService,
            cachedDataSvc: ICachedDataSvc,
            notificationSvc: INotificationSvc,
            currentUser: ICurrentUser,
            $anchorScroll: ng.IAnchorScrollService,
            $location: ng.ILocationService,
            resourceSvc: IResourceSvc,
            $timeout: ng.ITimeoutService,
            dialogSvc: IDialogSvc
        ) {

            var page: IPage = {
                isReady: true,
                numResourceToWait: 0,
                disabled: false
            };
            $scope.addType = 0;
            var editorControl: IEditorControl;
            var lc: any;
            var lcUnsubscribe: any = undefined;
            var vars: any = {
                externalUrl: ""
            };

            function init() {
                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait <= 0;
                }

                $scope.$on('$destroy', destroy);
                editorControl = $scope.editorControl;
                editorControl.isImageChanged = false;
                editorControl.isModified = false;
                editorControl.callSave = save;
                $scope.addType = editorControl.addContentType;
                vars.externalUrl = editorControl.externalUrl;
                if (editorControl.addContentType === 2) {
                    openDrawing();
                }

                var noTitle: number = util.getNumber($scope.noTitle);
                if (noTitle == 1) {
                    editorControl.title = "x";
                    
                }
                $scope.noTitleControl = noTitle === 1;

                var noActions: number = util.getNumber($scope.noActions);
                $scope.noActionControls = noActions === 1;

                var noVideo: number = util.getNumber($scope.noVideo);
                $scope.noVideoControl = noVideo === 1;

                if ($scope.textLabel === undefined)
                    $scope.textLabel = "Text";
            }

            function destroy() {
                closeDrawing();
            }

            function changed() {
                function convertToYouTubeEmbed() {
                    var http: string = vars.externalUrl.indexOf("https") !== -1 ? "https" : "http";
                    var lastIdx: number = vars.externalUrl.lastIndexOf("v=");
                    if (lastIdx !== -1) {
                        var ampersandIdx: number = vars.externalUrl.indexOf("&");
                        var videoUrl: string = "";
                        if(ampersandIdx === -1)
                            videoUrl = vars.externalUrl.substring(lastIdx + 2);
                        else
                            videoUrl = vars.externalUrl.substring(lastIdx + 2, ampersandIdx);
                        editorControl.externalUrl = http + "://www.youtube.com/embed/" + videoUrl;
                    }
                }

                $timeout(function () {
                    editorControl.isModified = true;

                    if ($scope.addType == 4) {
                        convertToYouTubeEmbed();
                    }
                }, 100);
            }

            function openDrawing() {
                $timeout(function () {
                    var element = $("#" + $scope.id + " div.literally").get(0);
                    lc = LC.init(
                        element,
                        {
                            imageURLPrefix: '/Content/lib/literally-canvas/img',
                            imageSize: {
                                width: 500, height: 400
                            }
                        }
                    );
                    if (editorControl.addContentType == 2) {
                        lc.loadSnapshot(JSON.parse(editorControl.drawingContent));
                    }
                    lcUnsubscribe = lc.on('drawingChange', changed);
                }, 1000);
            }

            function closeDrawing() {
                if (lc !== undefined) {
                    lc.teardown();
                    lc = undefined;
                }

                if (lcUnsubscribe !== undefined) {
                    lcUnsubscribe();
                    lcUnsubscribe = undefined;
                }
            }

            function changeAddType(aType: number) {
                $scope.addType = aType;
                switch (aType) {
                    case 0: // none
                        closeDrawing();
                        break;
                    case 1: // image
                        closeDrawing();
                        break;
                    case 2: // drawing
                        openDrawing();
                        break;
                    case 3: // drawing + image background
                        break;
                }
            }

            function save() {
                function saveImage() {
                }

                function saveDrawing() {
                    editorControl.drawingContent = JSON.stringify(lc.getSnapshot());
                }

                switch ($scope.addType) {
                    case 0:
                        break;
                    case 1:
                        editorControl.drawingContent = "";
                        if (editorControl.isImageChanged) {
                            editorControl.newImageFileName = util.guid() + ".jpg";
                        }
                        saveImage();
                        break;
                    case 2:
                        editorControl.imageUrl = "";
                        editorControl.drawingContent = "";
                        saveDrawing();
                        break;
                    case 3:
                        break;
                }
                editorControl.addContentType = $scope.addType;
                if (editorControl.save !== undefined)
                    editorControl.save($scope.item);
            }

            function cancel() {
                if (editorControl.cancel !== undefined)
                    editorControl.cancel($scope.item);
            }

            function imageClick() {
                var inputBtn = $("#" + $scope.id + " .editor-action input").first();
                inputBtn.click();
            }

            function imageChanged() {
                editorControl.isImageChanged = true;
                editorControl.isModified = true;
                $timeout(function () {
                    var fileNode = $("#" + $scope.id + " input.editor-image").get(0);
                    var canvasNode = $("#" + $scope.id + " canvas.editor-image").get(0);

                    pixUtil.saveEditorImage(fileNode, canvasNode, 500,
                        function (data: string) {
                            editorControl.imageContent = data;
                        });

                }, 100);
            }

            init();

            $scope.page = page;

            $scope.vars = vars;
            $scope.tinymceOptions = util.getTinymceOptions();
            $scope.changed = changed;
            $scope.changeAddType = changeAddType;
            $scope.save = save;
            $scope.cancel = cancel;

            $scope.imageClick = imageClick;
            $scope.imageChanged = imageChanged;
        }

        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/editor/qzEditor.html",
            replace: true,
            scope: {
                id: "@",
                editorControl: "=",
                item: "=?",
                noTitle: "@?",
                noActions: "@?",
                noVideo: "@?",
                textLabel: "@?",
            },
            controller: controller
        }
    }
})();