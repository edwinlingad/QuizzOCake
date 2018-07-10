module quizzCategoryCtrl {
    export interface IScope {
        page: IPage;
        searchParams: quizzOverviewsDirective.ISearchParam;

        callGoBack:() => void;
    }

    export interface IStateParams {
        category?: string;
    }
} 

(function () {
    l2lControllers.controller('quizzCategoryCtrl', quizzCategoryCtrl);

    quizzCategoryCtrl.$inject = ["$scope", "$stateParams", "cachedDataSvc", "notificationSvc"];
    function quizzCategoryCtrl(
        $scope: quizzCategoryCtrl.IScope,
        $stateParams: quizzCategoryCtrl.IStateParams,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc
        ) {
        var appState: IAppState = setting.appState;
        var page: IPage = {
            isReady: false,
            numResourceToWait: 1
        }
        var categories: IQuizzCategoryModel[];

        function init() {
            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait == 0;
            }

            function error(response) {
                notificationSvc.error(str.errorLoad);
            }

            function successCategories(data) {
                categories = data;
                updateIsReady();

                var catIdx: number = util.getNumber($stateParams.category);
                categories.forEach(function (item: IQuizzCategoryModel) {
                    if (item.quizzCategoryType === catIdx) {

                        page.title = item.title;
                    }
                });
                //page.title = categories[catIdx].title;
            }

            categories = cachedDataSvc.getQuizzzCategories(successCategories, error); 
        }

        function callGoBack(): void {
            history.back();
        }

        function initAppState() {
            setting.resetMobileAppState();
            appState.hasHeaderTitle = true;
            appState.headerTitle = page.title
            appState.hasBackButton = true;
            appState.hasSearch = true;
            appState.hasTabs = true;
        }

        init();
        initAppState();

        $scope.page = page;
        $scope.searchParams = $stateParams;
        $scope.callGoBack = callGoBack;
    }
})();