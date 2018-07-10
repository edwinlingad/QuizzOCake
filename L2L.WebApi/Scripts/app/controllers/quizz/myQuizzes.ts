module myQuizzesCtrl {
    export interface IScope {
        page: IPage;
        searchParams: quizzOverviewsDirective.ISearchParam;

        callGoBack: () => void;
    }
} 

(function () {
    l2lControllers.controller("myQuizzesCtrl", myQuizzesCtrl);

    myQuizzesCtrl.$inject = ["$scope", "cachedDataSvc", "currentUser", "notificationSvc"];
    function myQuizzesCtrl(
        $scope: myQuizzesCtrl.IScope,
        cachedDataSvc: ICachedDataSvc,
        currentUser: ICurrentUser,
        notificationSvc: INotificationSvc
        ) {

        var page: IPage = {
            isReady: true,
            numResourceToWait: 0
        }
        var searchParams: quizzOverviewsDirective.ISearchParam = {};

        function init() {
            var user: IUserModel = currentUser.getUserData();
            searchParams.userId = user.id == 0? -1 : user.id;
            searchParams.availOnly = 0;
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