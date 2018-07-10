module messages {
    export interface IScope {
        page: IPage;

        depId: any;

        messages: IQuizzConnectMsgThread[];
        $on: any;
    }
}

(function () {
    l2lApp.directive("messages", messages);

    function messages() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

        function controller(
            $scope: messages.IScope,
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
                isReady: false,
                numResourceToWait: 1,
                disabled: false
            };
            var depId: number = 0;
            var messages: IQuizzConnectMsgThread[] = new Array<IQuizzConnectMsgThread>();

            function init() {
                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait <= 0;
                }

                $scope.$on('$destroy', destroy);


                function errorLoad(response) {
                    notificationSvc.error(str.errorLoad);
                }

                function successQuizzConnectMsgThread(data: IQuizzConnectMsgThread[]) {
                    updateIsReady();

                    $scope.messages = messages = data;
                }

                $scope.depId = depId = util.getNumber($scope.depId);
                $anchorScroll("top");
                messages = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzConnectMsgThread, depId, 0, 0, 0, 0, successQuizzConnectMsgThread, errorLoad);
            }

            function destroy() {
            }

            init();

            $scope.page = page;
            $scope.messages = messages;
        }

        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/messages/messages.html",
            replace: true,
            scope: {
                depId: "@"
            },
            controller: controller
        }
    }
})();