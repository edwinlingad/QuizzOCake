module dependentQuizzroom {
    export interface IScope {
        depId: number;

        page: IPage;
        depName: string;

        myQuizClasses: IQuizzClassModel[];
        enrolledQuizzClasses: IQuizzClassModel[];

        $on: any;
    }
}

(function () {
    l2lApp.directive("dependentQuizzroom", dependentQuizzroom);

    function dependentQuizzroom() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

        function controller(
            $scope: dependentQuizzroom.IScope,
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
                numResourceToWait: 2,
                disabled: false
            };
            var depId: number = 0;
            var myQuizClasses: IQuizzClassModel[] = new Array<IQuizzClassModel>();
            var enrolledQuizzClasses: IQuizzClassModel[] = new Array<IQuizzClassModel>();

            function init() {
                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait <= 0;
                }

                function errorLoad(response) {
                    notificationSvc.error(str.errorLoad);
                }

                function successEnrolledQuizzClass(data: IQuizzClassModel[]) {
                    $scope.enrolledQuizzClasses = enrolledQuizzClasses = data;
                    updateIsReady();

                    var count: number = 0;
                    data.forEach(function (item: IQuizzClassModel) {
                        if (item.member.isNew)
                            count++;
                    });
                }

                function successMyQuizzClass(data: IQuizzClassModel[]) {
                    $scope.myQuizClasses = myQuizClasses = data;
                    updateIsReady();
                }

                $scope.depId = depId = util.getNumber($scope.depId);

                $scope.$on('$destroy', destroy);
                myQuizClasses = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClass, 2, depId, 0, 0, 0, successMyQuizzClass, errorLoad);
                enrolledQuizzClasses = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClass, 4, depId, 0, 0, 0, successEnrolledQuizzClass, errorLoad);
            }

            function destroy() {
            }

            init();

            $scope.page = page;
            $scope.myQuizClasses = myQuizClasses;
            $scope.enrolledQuizzClasses = enrolledQuizzClasses;
        }

        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/dependent/dependentQuizzroom.html",
            replace: true,
            scope: {
                depId: "@",
                depName: "@",
            },
            controller: controller
        }
    }
})();