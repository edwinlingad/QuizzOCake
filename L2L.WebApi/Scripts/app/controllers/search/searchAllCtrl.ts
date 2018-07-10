module searchAllCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        goBack(): void;
        searchList: ISearchModel[];
    }

    export interface IStateParams {
        search: string;
    }
}

(function () {
    l2lControllers.controller('searchAllCtrl', searchAllCtrl);

    searchAllCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "searchSvc"];

    function searchAllCtrl(
        $scope: searchAllCtrl.IScope,
        $stateParams: searchAllCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        resourceSvc: IResourceSvc,
        $timeout: ng.ITimeoutService,
        dialogSvc: IDialogSvc,
        searchSvc: ISearchSvc
    ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 1,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var searchList: ISearchModel[] = new Array<ISearchModel>();

        function init() {
            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successSearch(data: ISearchModel[]) {
                updateIsReady();

                data.forEach(function (item: ISearchModel) {
                    searchList.push(item);
                });
            }

            $anchorScroll("top");
            searchSvc.search($stateParams.search, successSearch, errorLoad);
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
        $scope.searchList = searchList;
    }
})();