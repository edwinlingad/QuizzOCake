module myAssignmentsCtrl {
    export interface IScope {
        assId: number;
        userId: any;
    }

    export interface IStateParams {
        assId: number;
    }
}

(function () {
    l2lControllers.controller('myAssignmentsCtrl', myAssignmentsCtrl);

    myAssignmentsCtrl.$inject = ["$scope", "$stateParams", "currentUser"];
    function myAssignmentsCtrl(
        $scope: myAssignmentsCtrl.IScope,
        $stateParams: myAssignmentsCtrl.IStateParams,
        currentUser: ICurrentUser
        ) {

        var user: IUserModel = currentUser.getUserData();
        
        function init() {
            $scope.assId = util.getNumber($stateParams.assId);
            $scope.userId = user.id;
        }

        init();        
    }
})();