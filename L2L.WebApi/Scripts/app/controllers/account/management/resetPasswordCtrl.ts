module resetPasswordCtrl {
    export interface IScope {
        page: IPage;
        model: IResetPasswordModel;
        resetPassword: (form: ng.IFormController) => void;
        cancel: () => void;
    }

    export interface IStateParams {
        userName: string;
        code: string;
    }
}

(function () {
    l2lControllers.controller("resetPasswordCtrl", resetPasswordCtrl);

    resetPasswordCtrl.$inject = ["$scope", "$stateParams", "accountSvc", "$state", "notificationSvc"];
    function resetPasswordCtrl(
        $scope: resetPasswordCtrl.IScope,
        $stateParams: resetPasswordCtrl.IStateParams,
        accountSvc: IAccount,
        $state: ng.ui.IStateService,
        notificationSvc: INotificationSvc
        ) {

        var page: IPage = {
            numResourceToWait: 0,
            isReady: true,
            disabled: false,
        }
        var model: IResetPasswordModel = {
            userName: $stateParams.userName,
            code: $stateParams.code
        }

        function init() {
        }

        function resetPassword(form: ng.IFormController): void {
            if (form.$invalid == false) {
                util.disableForm(page);
                accountSvc.resetPassword(model,
                    function () {
                        util.enableForm(page);
                        notificationSvc.success("Password successfully changed");
                        $state.go("nsi.index");
                    },
                    function () {
                        util.enableForm(page);
                        notificationSvc.error("An error has occured while changing the password");
                    });
            }
        }

        function cancel(): void {
            $state.go("nsi.index");
        }

        init();

        $scope.page = page;
        $scope.cancel = cancel;
        $scope.resetPassword = resetPassword;
        $scope.model = model;
    }
})();