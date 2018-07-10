module qzEditorView {
    export interface IScope {
        id: string;
        editorControl: IEditorControl;
        item: any;
        noTitle: any;

        noTitleControl: boolean;
        noActionControls?: any;

        page: IPage;
        noActions: any;
        $on: any;

        close(): void;
    }
}

(function () {
    l2lApp.directive("qzEditorView", qzEditorView);

    function qzEditorView() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

        function controller(
            $scope: qzEditorView.IScope,
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
            var editorControl: IEditorControl;
            var lc: any;

            function init() {
                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait <= 0;
                }

                $scope.$on('$destroy', destroy);
                editorControl = $scope.editorControl;

                if (editorControl.addContentType === 2) {
                    renderDrawing();
                }

                var noTitle: number = util.getNumber($scope.noTitle);
                if (noTitle == 1)
                    editorControl.title = "x";
                $scope.noTitleControl = noTitle === 1;

                var noActions: number = util.getNumber($scope.noActions);
                $scope.noActionControls = noActions == 1;
            }

            function renderDrawing() {
                $timeout(function () {
                    var data = LC.renderSnapshotToImage(
                        JSON.parse(editorControl.drawingContent),
                        {
                            scale: 1
                        });
                    
                    var canvasContainer = $("#" + $scope.id + " .literally-view").get(0);
                    canvasContainer.appendChild(data);
                }, 1000);
            }

            function destroy() {
            }

            function close() {
                editorControl.close($scope.item);
            }

            init();

            $scope.page = page;
            $scope.close = close;
        }

        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/editor/qzEditorView.html",
            replace: true,
            scope: {
                id: "@",
                editorControl: "=",
                item: "=?",
                noTitle: "@?",
                noActions: "@?"
            },
            controller: controller
        }
    }
})();