module dailyRewardsCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        searchParams: quizzOverviewsDirective.ISearchParam;
        goBack(): void;
    }

    export interface IStateParams {
        quizzId: number;
    }
}

(function () {
    l2lControllers.controller('dailyRewardsCtrl', dailyRewardsCtrl);

    dailyRewardsCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

    function dailyRewardsCtrl(
        $scope: dailyRewardsCtrl.IScope,
        $stateParams: dailyRewardsCtrl.IStateParams,
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
        var user: IUserModel = currentUser.getUserData();
        var searchParams: quizzOverviewsDirective.ISearchParam = {};

        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            $anchorScroll("top");
            searchParams.dailyRewardsOnly = 1;
        }

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.goBack = goBack;
        $scope.searchParams = searchParams;
    }
})();