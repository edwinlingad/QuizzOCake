module quizzmates {
    export interface IScope {
        page: IPage;

        quizzerId: any;
        title: any;
        self: any;

        isSelf: boolean;
        quizzmates: IQuizzerModel[];

        control: any;

        $on: any;
    }
}

(function () {
    l2lApp.directive("quizzmates", quizzmates);

    function quizzmates() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

        function controller(
            $scope: quizzmates.IScope,
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
            var quizzerId: number;
            var quizzmates: IQuizzerModel[] = new Array<IQuizzerModel>();

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function init() {
                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait <= 0;
                }

                function successQuizzmates(data: IQuizzerModel[]) {
                    data.forEach(function (item: IQuizzerModel) {
                        quizzmates.push(item);
                    });
                    updateIsReady();
                }

                $scope.title = "Hello";
                $scope.isSelf = util.getNumber($scope.self) === 1;
                quizzerId = util.getNumber($scope.quizzerId);
                $scope.quizzerId = quizzerId = quizzerId == -1 ? 0 : quizzerId;
                if($scope.control !== undefined && $scope.control !== null)
                    $scope.control.reload = reload;
                resourceSvc.getResourceMany(enums.ResourceTypeEnum.Quizzmates, quizzerId, 0, 0, 0, 0, successQuizzmates, errorLoad);

                $scope.$on('$destroy', destroy);
            }

            function getQuizzmates() {
                function successQuizzmates(data: IQuizzerModel[]) {
                    data.forEach(function (item: IQuizzerModel) {
                        quizzmates.push(item);
                    });
                }

                resourceSvc.getResourceMany(enums.ResourceTypeEnum.Quizzmates, quizzerId, 0, 0, 0, 0, successQuizzmates, errorLoad);
            }

            function reload() {
                quizzmates.splice(0, quizzmates.length);
                getQuizzmates();
            }

            function destroy() {
            }

            init();

            $scope.page = page;
            $scope.quizzmates = quizzmates;
        }

        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/relationships/quizzmates.html",
            replace: true,
            scope: {
                quizzerId: "@",
                title: "@",
                self: "@",
                control: "=?"
            },
            controller: controller
        }
    }
})();