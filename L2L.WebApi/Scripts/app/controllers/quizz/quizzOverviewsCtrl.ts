module quizzOverviewsCtrl {
    export interface IScope {
        page: IPage;
        searchParams: quizzOverviewsDirective.ISearchParam;
    }

    export interface IStateParams {
        category?: string;
        levelMin: string;
        levelMax: string;
        searchString: string;
        pageNum?: number;
        numPerPage?: number;
        sortBy?: number;
        sortType?: number;
        userId?: any;
        availOnly?: any;
    }
}

(function () {
    l2lControllers.controller('quizzOverviewsCtrl', quizzOverviewsCtrl);

    quizzOverviewsCtrl.$inject = ["$scope", "$stateParams", "$anchorScroll"];
    function quizzOverviewsCtrl(
        $scope: quizzOverviewsCtrl.IScope,
        $stateParams: quizzOverviewsCtrl.IStateParams,
        $anchorScroll: ng.IAnchorScrollService
        ) {
        var appState: IAppState = setting.appState;
        var page: IPage = {
            title: "Quizzes",
            isReady: true,
            numResourceToWait: 0
        }

        function init() {
            $anchorScroll("top");
        }

        init();

        $scope.page = page;
        $scope.searchParams = $stateParams;
    }
})(); 