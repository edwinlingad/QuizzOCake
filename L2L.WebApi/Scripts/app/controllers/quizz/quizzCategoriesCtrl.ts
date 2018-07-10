
module quizzCategoriesCtrl {
    export interface IScope {
        page: IPage,
        message: string;
        categories: IQuizzCategoryModel[];
        goBack(): void;

        search: any;
        appState: IAppState;
        showSearchBtn: IButtonElement;
        goToCategory(item: IQuizzCategoryModel);

    }
}

(function () {
    l2lControllers.controller('quizzCategoriesCtrl', quizzCategoriesCtrl);

    //quizzCategoriesCtrl.$inject = ["$scope", "$state", "quizzCategorySvc", "cachedDataSvc", "$anchorScroll", "$timeout", "$location", "$ionicNavBarDelegate"];
    quizzCategoriesCtrl.$inject = ["$scope", "$state", "quizzCategorySvc", "cachedDataSvc", "$anchorScroll", "$timeout", "$location"];
    function quizzCategoriesCtrl(
        $scope: quizzCategoriesCtrl.IScope,
        $state: ng.ui.IStateService,
        quizzCategorySvc: IQuizzCategorySvc,
        cachedDataSvc: ICachedDataSvc,
        $anchorScroll: ng.IAnchorScrollService,
        $timeout: ng.ITimeoutService,
        $location: ng.ILocationService
        //$ionicNavBarDelegate: ionic.navigation.IonicNavBarDelegate
    ) {
        var appState = $scope.appState = setting.appState;
        var page: IPage = {
            isReady: false,
            disabled: false,
            numResourceToWait: 1,
            title: "Categories"
        }
        var categories: IQuizzCategoryModel[] = new Array<IQuizzCategoryModel>();
        var showSearchBtn: IButtonElement = {
            visible: false,
            click: function () {
                showSearchBtn.visible = !showSearchBtn.visible;

                if (showSearchBtn.visible) {
                    //$ionicNavBarDelegate.title("");
                    $timeout(function () {
                        $("#search-input").focus();
                    }, 200);
                }
                else
                    setPageTitle();
            }
        };

        function error(response) {
            commonError($scope, response);
        }

        function init() {
            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait == 0;
            }

            function successCategories(data) {
                $scope.categories = categories = data;
                updateIsReady();
            }

            $anchorScroll("top");
            setPageTitle();
            categories = cachedDataSvc.getQuizzzCategories(successCategories, error);
        }

        function resetPageState() {
            showSearchBtn.visible = false;
            setPageTitle();
        }

        function setPageTitle() {
            $timeout(function () {
                //$ionicNavBarDelegate.title(page.title);
            }, 200);
        }

        function initAppState() {
            setting.resetMobileAppState();
            appState.hasHeaderTitle = true;
            appState.headerTitle = "Categories";
            appState.hasSearch = true;
        }

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        function goToCategory(item: IQuizzCategoryModel) {
            $state.go("si.quizzCategory", { category: item.quizzCategoryType });
        }

        init();
        initAppState();

        $scope.page = page;
        $scope.categories = categories;

        $scope.goBack = goBack;
        $scope.search = {};

        $scope.showSearchBtn = showSearchBtn;
        $scope.goToCategory = goToCategory;
    }
})();