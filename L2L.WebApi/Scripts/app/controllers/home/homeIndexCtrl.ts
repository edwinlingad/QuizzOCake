module homeIndexCtrl {
    export interface IScope {
        openLoginModal: () => void;
        openSignUpModal: () => void;
    }
}

(function () {
    l2lControllers.controller("homeIndexCtrl", homeIndexCtrl);

    homeIndexCtrl.$inject = ["$scope", "$state", "currentUser", "$location"];
    function homeIndexCtrl(
        $scope: homeIndexCtrl.IScope,
        $state: ng.ui.IStateService,
        currentUser: ICurrentUser,
        $location: ng.ILocationService) {

        function init() {
            if (currentUser !== undefined && currentUser.isLoggedIn())
                $state.go(util.getDefaultLocation());
            //else
            //    $state.go("nsi.index");
            if ($location !== undefined)
                $location.replace();
        }

        init();
    }

    l2lControllers.controller("mobileStartCtrl", mobileStartCtrl);

    homeIndexCtrl.$inject = ["$state", "currentUser"];
    function mobileStartCtrl(
        currentUser: ICurrentUser,
        $state: ng.ui.IStateService) {

        function init() {
            // TODO: check if logged In
            if (currentUser !== undefined && currentUser.isLoggedIn())
                //$state.go(util.getDefaultLocation());
                // EDIT-ME
                $state.go("si.quizzCategory", { category: 4 });
            else
                $state.go("mSignIn");
        }

        init();
    }
})();
