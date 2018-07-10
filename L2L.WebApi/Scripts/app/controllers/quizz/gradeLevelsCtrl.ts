module gradeLevelsCtrl {
    export interface IScope {
        page: IPage;
        gradeLevels: IGradeLevelModel[];
    }
} 

(function () {
    l2lControllers.controller("gradeLevelsCtrl", gradeLevelsCtrl);

    gradeLevelsCtrl.$inject = ["$scope", "cachedDataSvc", "notificationSvc", "$anchorScroll"];
    function gradeLevelsCtrl(
        $scope: gradeLevelsCtrl.IScope,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        $anchorScroll: ng.IAnchorScrollService
        ) {

        var page: IPage = {
            isReady: true,
            numResourceToWait: 1
        }
        var gradeLevels: IGradeLevelModel[];

        function error(response) {
            commonError($scope, response);
        }

        function init() {
            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait == 0;
            }

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function successGradeLevel(data) {
                gradeLevels = data;
                updateIsReady();
            }

            $anchorScroll("top");
            gradeLevels = cachedDataSvc.getGradeLevels(successGradeLevel, errorLoad);
        }

        init();

        $scope.page = page;
        $scope.gradeLevels = gradeLevels;
    }
})();