module mSignInCtrl {
    export interface IScope {
        message: string,
        user: ILogin,
        signIn: () => void
        signUp: () => void;
        forgotUserNameOrPassword: () => void;
        page: IPage;
        close: () => void;
        loginBtn: IElement;
        changed(): void;
    }

    export interface IPageExtend extends IPageOld {
        signingIn: boolean;
        isSignInDisabled: () => boolean;
    }
}

(function () {
    l2lApp.controller('mSignInCtrl', mSignInCtrl);

    mSignInCtrl.$inject = ["$scope", "$state", "accountSvc", "currentUser"];

    function mSignInCtrl(
        $scope: mSignInCtrl.IScope,
        $state: ng.ui.IStateService,
        accountSvc: IAccount,
        currentUser: ICurrentUser
    ) {

        var page: IPage = {
            numResourceToWait: 0,
            isReady: true,
            disabled: false,
        }
        var user: ILogin = { userName: "", password: "" };
        var loginBtn: IElement = {
            isEnabled: false,
            isDisabledFn: isSignInDisabled
        };

        function init() {
            user.userName = "edwin";
            user.password = "RanchRush8765";
        }

        function signIn() {

            function errorLogin(response) {
                user.password = "";
                $scope.message = "Invalid username or password";
                util.enableForm(page);
            }

            function successLogin(data) {
                currentUser.setAcccessToken(data.access_token);

                accountSvc.getUser(
                    function (data: IUserModel) {

                        if (data.userType === 0) {
                            if (data.isEmailConfirmed === false) {
                                $scope.message = "Account confirmation required. Please check your email";
                                accountSvc.logOut();
                                util.enableForm(page);
                                return;
                            }
                        }

                        currentUser.setUserData(data);
                        util.enableForm(page);

                        $state.go(util.getDefaultLocation());
                    }, errorLogin);
            }

            util.disableForm(page);
            $scope.user.grant_type = "password";
            accountSvc.loginUser($scope.user, successLogin, errorLogin);
        }

        function isSignInDisabled() {
            return $scope.user.userName.length == 0 || $scope.user.password.length == 0;
        }

        function signUp(): void {
            $state.go("mSignUpStandard");
        }

        function forgotUserNameOrPassword(): void {
        }

        function close(): void {
            
        }

        function changed() {
            $scope.message = "";
        }

        init();

        $scope.page = page;
        $scope.user = user;
        $scope.signIn = signIn;
        $scope.signUp = signUp;
        $scope.close = close;
        $scope.message = "";
        $scope.forgotUserNameOrPassword = forgotUserNameOrPassword;
        $scope.loginBtn = isSignInDisabled;
        $scope.changed = changed;
    }
})();