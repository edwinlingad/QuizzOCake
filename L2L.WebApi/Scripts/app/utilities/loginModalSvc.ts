interface ILoginModalSvc {
    openLoginModal(isGuest: boolean, showNeedSignInMsg?: boolean, success?: Function): ng.ui.bootstrap.IModalServiceInstance;
    openSignUpModal: () => void;
}

(function () {
    l2lApp.service("loginModalSvc", loginModalSvc);

    loginModalSvc.$inject = ["$state", "modalSvc", "broadcastSvc", "dialogSvc", "$location"];

    function loginModalSvc(
        $state: ng.ui.IStateService,
        modalSvc: IModalSvc,
        broadcastSvc: IBroadcastSvc,
        dialogSvc: IDialogSvc,
        $location: ng.ILocationService
    ): ILoginModalSvc {

        function openLoginModal(isGuest: boolean, showNeedSignInMsg?: boolean, success?: Function): ng.ui.bootstrap.IModalServiceInstance {
            var settings: ng.ui.bootstrap.IModalSettings = {
                templateUrl: "scripts/templates/account/SignIn.html",
                controller: "signInCtrl",
                size: "sm",
                resolve: {
                    isGuest: function () {
                        return isGuest;
                    },
                    showNeedSignInMsg: function () {
                        if (showNeedSignInMsg != undefined)
                            return showNeedSignInMsg;
                        return false;
                    }
                }
            }

            return modalSvc.open(settings, function (result: SignInCtrlReturnEnum) {
                if (success != undefined)
                    success();
                switch (result) {
                    case SignInCtrlReturnEnum.LogInSucces:
                        broadcastSvc.postUpdateLayoutEvent();

                        var path = $location.path();
                        if (isGuest || path.indexOf("n/index") !== -1)
                            $state.go(util.getDefaultLocation());
                        else
                            $state.reload();

                        break;
                    case SignInCtrlReturnEnum.SignUp:
                        openSignUpModal();
                        break;
                    case SignInCtrlReturnEnum.ForgotUsernameOrPassword:
                        openForgotUsernameOrPasswordModal();
                        break;
                }
            });
        }

        function openSignUpModal(): void {
            function openSignUpStandard() {
                var settings: ng.ui.bootstrap.IModalSettings = {
                    templateUrl: "scripts/templates/account/SignUpStandard.html",
                    controller: "registerUserCtrl",
                    //size: "lg",
                }

                modalSvc.open(settings, function (result: boolean) {
                    if (result == true) {
                        // $state.go('si.welcomeStandard');
                        dialogSvc.alert("Please check your email to confirm your accout. You may need to check your SPAM folder for this.");
                    }
                });
            }

            openSignUpStandard();

            //var settings: ng.ui.bootstrap.IModalSettings = {
            //    templateUrl: "scripts/templates/account/SignUp.html",
            //    controller: "signUpCtrl",
            //}

            //modalSvc.open(settings, function (userType: UserTypeEnum) {
            //    if (userType == UserTypeEnum.Standard)
            //        openSignUpStandard();
            //});
        }

        function openForgotUsernameOrPasswordModal(): void {
            var settings: ng.ui.bootstrap.IModalSettings = {
                templateUrl: "scripts/templates/account/management/ForgotPassword.html",
                controller: "forgotPasswordCrtl",
                size: "sm"
            }

            modalSvc.open(settings, function () {
            });
        }

        return {
            openLoginModal: openLoginModal,
            openSignUpModal: openSignUpModal
        }
    }

})();