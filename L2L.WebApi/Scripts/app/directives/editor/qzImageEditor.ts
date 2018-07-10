interface IImageEditorControl {
    imageUrl?: string;
    imageContent?: string;
    newImageFileName?: string;
    isImageChanged?: boolean;

    changedCb?: any;
}

module qzImageEditor {
    export interface IScope {
        id: string;
        item: any;
        size: any;
        imageControl: IImageEditorControl;

        $on: any;
        imageChanged(): void;
        imageClick(): void;
    }
}

(function () {
    l2lApp.directive("qzImageEditor", qzImageEditor);

    function qzImageEditor() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

        function controller(
            $scope: qzImageEditor.IScope,
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
            var size: number;
            var imageControl: IImageEditorControl;

            function init() {

                imageControl = $scope.imageControl;
                imageControl.isImageChanged = false;
                var tmp: number = util.getNumber($scope.size);
                size = tmp !== -1 ? tmp : 100;

                $scope.$on('$destroy', destroy);
            }

            function destroy() {
            }

            function imageChanged() {
                imageControl.isImageChanged = true;
                imageControl.newImageFileName = util.guid() + ".jpg";
                if (imageControl.changedCb !== undefined)
                    imageControl.changedCb($scope.item);

                $timeout(function () {
                    var fileNode = $("#" + $scope.id + " input.qz-image-editor").get(0);
                    var canvasNode = $("#" + $scope.id + " canvas.qz-image-editor").get(0);

                    pixUtil.saveEditorImage(fileNode, canvasNode, size,
                        function (data: string) {
                            imageControl.imageContent = data;
                        });

                }, 100);
            }

            function imageClick() {
                var inputBtn = $("#" + $scope.id + " .editor-action input").first();
                inputBtn.click();
            }

            init();

            $scope.imageChanged = imageChanged;
            $scope.imageClick = imageClick;
        }

        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/editor/qzImageEditor.html",
            replace: true,
            scope: {
                id: "@",
                item: "=?",
                size: "@?",
                imageControl: "="
            },
            controller: controller
        }
    }
})();