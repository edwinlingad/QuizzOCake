module gradeLevelCtrl {
    export interface IScope {
        page: IPage;
        searchParams: quizzOverviewsDirective.ISearchParam;

        callGoBack: () => void;
    }

    export interface IStateParams {
        gradeLevel?: string;
    }
} 

(function () {
    l2lControllers.controller('gradeLevelCtrl', gradeLevelCtrl);

    gradeLevelCtrl.$inject = ["$scope", "$stateParams", "cachedDataSvc", "notificationSvc"];
    function gradeLevelCtrl(
        $scope: gradeLevelCtrl.IScope,
        $stateParams: gradeLevelCtrl.IStateParams,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc
        ) {

        var page: IPage = {
            isReady: false,
            numResourceToWait: 1
        }
        var gradeLevels: IGradeLevelModel[];
        var searchParams: quizzOverviewsDirective.ISearchParam = {};

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
                
                var idx: number = parseInt($stateParams.gradeLevel);
                page.title = gradeLevels[idx].name;
            }

            gradeLevels = cachedDataSvc.getGradeLevels(successGradeLevel, errorLoad);
            searchParams.levelMin = searchParams.levelMax = $stateParams.gradeLevel;
        }

        function callGoBack(): void {
            history.back();
        }

        init();

        $scope.page = page;
        $scope.searchParams = searchParams;
        $scope.callGoBack = callGoBack;
    }
})();