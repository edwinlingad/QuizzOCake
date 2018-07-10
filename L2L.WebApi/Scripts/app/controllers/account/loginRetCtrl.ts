module loginRetCtrl {
    export interface IScope {
    }

    export interface IStateParams {
        acccess_token: string;
    }
}

(function () {
    l2lControllers.controller('loginRetCtrl', loginRetCtrl);

    loginRetCtrl.$inject = ["$scope", "$stateParams", "$state", "currentUser", "$location", "resourceSvc", "accountSvc", "hubSvc", "broadcastSvc", "oAuthSvc", "dialogSvc"];

    function loginRetCtrl(
        $scope: loginRetCtrl.IScope,
        $stateParams: loginRetCtrl.IStateParams,
        $state: ng.ui.IStateService,
        currentUser: ICurrentUser,
        $location: ng.ILocationService,
        resourceSvc: IResourceSvc,
        accountSvc: IAccount,
        hubSvc: IHubSvc,
        broadcastSvc: IBroadcastSvc,
        oAuthSvc: IOAuthSvc,
        dialogSvc: IDialogSvc
    ) {
        function error() {
            //alert("error!");
        }

        function successExternalLogin() {
            //hubSvc.reconnectHub();

            accountSvc.getUser(
                function (data: IUserModel) {
                    currentUser.setUserData(data);

                    broadcastSvc.postUpdateLayoutEvent();

                    var path = $location.path();
                    $state.go(util.getDefaultLocation());
                }, error);
        }

        function successUserInfo(data: IUserInfoViewModel) {
            function successRegisterExternal() {
                if (data.loginProvider.toLowerCase() === "facebook")
                    oAuthSvc.fbLogin(successExternalLogin, error);
                else if (data.loginProvider.toLowerCase() === "google")
                    oAuthSvc.googleLogin(successExternalLogin, error);
            }

            if (data.hasRegistered == false) {

                if (data.isEmailUsed) {
                    dialogSvc.alert(str.alertAccountAssociated);
                    $state.go(util.getDefaultLocation());
                    return;
                }

                var model: IRegisterExternalBindingModel = {
                    email: data.email // not really used
                };
                accountSvc.registerExternal(model, successRegisterExternal, error);
            }
            else {
                successExternalLogin();
            }
        }

        function init() {

            var idx = $stateParams.acccess_token.indexOf("=");

            if (idx === -1)
                return;

            var accessToken = $stateParams.acccess_token.substr(idx + 1);
            idx = accessToken.indexOf("&");
            accessToken = accessToken.substr(0, idx);

            currentUser.setAcccessToken(accessToken);

            accountSvc.userInfo(successUserInfo, error);
        }

        init();
    }
})();