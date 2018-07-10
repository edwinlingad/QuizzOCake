module patternDirective {
    export interface IScope {
        page: IPage;

        $on: any;
    }
}

(function () {
    l2lApp.directive("patternDirective", patternDirective);

    function patternDirective() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

        function controller(
            $scope: patternDirective.IScope,
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

            function init() {
                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait <= 0;
                }

                $scope.$on('$destroy', destroy);
            }

            function destroy() {
            }

            init();

            $scope.page = page;
        }

        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/add-guest-message.html",
            replace: true,
            scope: {
                msg: "@",
                isGuest: "="
            },
            controller: controller
        }
    }
})();