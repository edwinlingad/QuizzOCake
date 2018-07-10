module forgotPasswordCrtl {
    export interface IScope {
        page: IPage;
        emailOrUserName: string;
        sendResetPasswordRequest(): void;
        close(): void;
    }
}

(function () {
    l2lControllers.controller("forgotPasswordCrtl", forgotPasswordCrtl);

    forgotPasswordCrtl.$inject = ["$scope", "$uibModalInstance", "accountSvc", "dialogSvc"];
    function forgotPasswordCrtl(
        $scope: forgotPasswordCrtl.IScope,
        $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
        accountSvc: IAccount,
        dialogSvc: IDialogSvc
        ) {

        var page: IPage = {
            isReady: true,
            numResourceToWait: 0,
            disabled: false
        }

        function sendResetPasswordRequest(): void {

            util.disableForm(page);
            accountSvc.forgotPassword({ emailOrUserName: $scope.emailOrUserName },
                function () {
                    util.enableForm(page);
                    dialogSvc.alert("Please check you email or ask your parent/guardian to check their email.");
                    $uibModalInstance.close($scope.emailOrUserName);
                },
                function () {
                    util.enableForm(page);
                    dialogSvc.alert(str.errorSave);
                });

        }

        function close(): void {
            $uibModalInstance.dismiss();
        }

        $scope.close = close;
        $scope.page = page;
        $scope.emailOrUserName = "";
        $scope.sendResetPasswordRequest = sendResetPasswordRequest;
    }
})();