module testResultsCtrl {
    export interface IScope {
        page: IPage;
        user: IUserModel;
    }
} 

(function () {
    l2lControllers.controller("testResultsCtrl", testResultsCtrl);

    testResultsCtrl.$inject = ["$scope", "currentUser"];
    function testResultsCtrl(
        $scope: testResultsCtrl.IScope,
        currentUser: ICurrentUser
        ) {
        var page: IPage = {
            isReady: true,
            numResourceToWait: 0,
            disabled: false
        }
        var user: IUserModel = currentUser.getUserData();

        $scope.page = page;
        $scope.user = user;
    }
})();