module changePasswordCtrl {
    export interface IScope {
        page: IPageOld
        model: IChangePasswordBindingModel;
        changePassword: (form: ng.IFormController) => void;
        close(): void;
    }
}

(function () {
    l2lControllers.controller("changePasswordCtrl", changePasswordCtrl);

    changePasswordCtrl.$inject = ["$scope", "accountSvc", "$uibModalInstance", "notificationSvc"];
    function changePasswordCtrl(
        $scope: changePasswordCtrl.IScope,
        accountSvc: IAccount,
        $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
        notificationSvc: INotificationSvc
        ) {

        var page: IPageOld = { disabled: false }
        var model: IChangePasswordBindingModel = {}

        function changePassword(form: ng.IFormController): void {
            if (form.$invalid == false) {
                page.disabled = true;
                accountSvc.changePassword(model,
                    function() {
                        page.disabled = false;
                        notificationSvc.success("Password successfully changed");
                        $uibModalInstance.close();
                    },
                    function() {
                        page.disabled = false;
                        notificationSvc.error("An error has occurred while trying to change the password");
                    })
            }
        }

        function close(): void {
            $uibModalInstance.dismiss();
        }

        $scope.page = page;
        $scope.model = model;
        $scope.changePassword = changePassword;
        $scope.close = close;
    }
})();