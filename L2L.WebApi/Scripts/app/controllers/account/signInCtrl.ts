enum SignInCtrlReturnEnum {
    LogInSucces,
    SignUp,
    ForgotUsernameOrPassword
}

module signInCtrl {
    export interface IScope {
        message: string,
        user: ILogin,
        signIn: () => void
        signUp: () => void;
        forgotUserNameOrPassword: () => void;
        page: IPage;
        close: () => void;
        loginBtn: IElement;
        showNeedSignInMsg: boolean;
        changed(): void;

        fbLogin(): void;
        googleLogin(): void;
    }

    export interface IPageExtend extends IPageOld {
        signingIn: boolean;
        isSignInDisabled: () => boolean;
    }
}

interface Window {
    fbAsyncInit: any;
}

interface IFB {
    ui: any;
    getLoginStatus: any;
    init: any;
    api: any;
    login: any;
    logout: any;
}

var FB: IFB;
var window: Window;

(function () {
    l2lApp.controller('signInCtrl', signInCtrl);

    signInCtrl.$inject = ["$scope", "accountSvc", "currentUser", "$uibModalInstance", "hubSvc", "isGuest", "showNeedSignInMsg", "oAuthSvc"];

    function signInCtrl(
        $scope: signInCtrl.IScope,
        accountSvc: IAccount,
        currentUser: ICurrentUser,
        $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
        hubSvc: IHubSvc,
        isGuest: boolean, 
        showNeedSignInMsg: boolean,
        oAuthSvc: IOAuthSvc
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

        function errorLogin(response) {
            user.password = "";
            $scope.message = "Invalid username or password";
            util.enableForm(page);
        }

        function init() {

            if (isGuest == true) {
                user.userName = "guest";
                user.password = "guest";
            }
        }

        function signIn() {
            function successLogin(data) {
                currentUser.setAcccessToken(data.access_token);
                //hubSvc.reconnectHub();

                accountSvc.getUser(
                    function (data: IUserModel) {
                        
                        if (data.userType === 0) {
                            if (data.isEmailConfirmed === false) {
                                $scope.message = "Account confirmation required. Please check your email. Note: You may need to check your SPAM folder for this.";
                                accountSvc.logOut();
                                util.enableForm(page);
                                return;
                            }
                        } 

                        currentUser.setUserData(data);
                        util.enableForm(page);
                        $uibModalInstance.close(SignInCtrlReturnEnum.LogInSucces);
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
            $uibModalInstance.close(SignInCtrlReturnEnum.SignUp);
        }

        function forgotUserNameOrPassword(): void {
            $uibModalInstance.close(SignInCtrlReturnEnum.ForgotUsernameOrPassword);
        }

        function changed(): void {
            $scope.message = "";
        }

        function close(): void {
            $uibModalInstance.dismiss();
        }

        function fbLogin() {
            function successLogin(model: IOauthUserInfo) {
                
            }

            function errorLogin(model: IOauthUserInfo) {

            }

            oAuthSvc.fbLogin(successLogin, errorLogin);
        }

        function googleLogin() {
            function successLogin(model: IOauthUserInfo) {
                
            }

            function errorLogin(model: IOauthUserInfo) {

            }

            oAuthSvc.googleLogin(successLogin, errorLogin);
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
        $scope.showNeedSignInMsg = showNeedSignInMsg;
        $scope.changed = changed;
        
        $scope.fbLogin = fbLogin;
        $scope.googleLogin = googleLogin;
        //$scope.fbControl = fbControl;
    }
})();