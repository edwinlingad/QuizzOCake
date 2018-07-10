interface IDialogSvc {
    alert: (message: string, callback?: () => void) => void;
    confirm: (message: string, callback: (result: boolean) => void) => void;
}

(function () {
    l2lApp.service('dialogSvc', dialogSvc);

    dialogSvc.$inject = ["modalSvc"];

    function dialogSvc(
        modalSvc: IModalSvc
    ): IDialogSvc {
        function alert(message: string, callback?: () => void): void {
            //bootbox.alert(message, callback);

            var settings: ng.ui.bootstrap.IModalSettings = {
                templateUrl: "scripts/templates/service/AlertModal.html",
                controller: "alertDialogCtrl",
                size: "sm",
                resolve: {
                    message: function () {
                        return message;
                    }
                }
            }

            modalSvc.open(settings, function () {
                callback();
            });
        }

        function confirm(message: string, callback: (result: boolean) => void): void {
            var settings: ng.ui.bootstrap.IModalSettings = {
                templateUrl: "scripts/templates/service/ConfirmModal.html",
                controller: "confirmDialogCtrl",
                size: "sm",
                resolve: {
                    message: function () {
                        return message;
                    }
                }
            }

            modalSvc.open(settings, function (result: boolean) {
                callback(result);
            });
        }

        return {
            alert: alert,
            confirm: confirm
        }
    }
})();

module alertDialogCtrl {
    export interface IScope {
        message: string;
        confirm(): void;
    }
}

(function () {
    l2lControllers.controller('alertDialogCtrl', alertDialogCtrl);

    alertDialogCtrl.$inject = ["$scope", "$uibModalInstance", "message"];

    function alertDialogCtrl(
        $scope: confirmDialogCtrl.IScope,
        $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
        message: string
    ) {

        function confirm(): void {
            $uibModalInstance.close();
        }

        $scope.message = message;
        $scope.confirm = confirm;
    }
})();

module confirmDialogCtrl {
    export interface IScope {
        message: string;
        confirm(answer: string): void;
    }
}

(function () {
    l2lControllers.controller('confirmDialogCtrl', confirmDialogCtrl);

    confirmDialogCtrl.$inject = ["$scope", "$uibModalInstance", "message"];

    function confirmDialogCtrl(
        $scope: confirmDialogCtrl.IScope,
        $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
        message: string
    ) {

        function confirm(answer: string): void {

            var isYes = answer === 'yes';
            $uibModalInstance.close(isYes);
        }

        $scope.message = message;
        $scope.confirm = confirm;
    }
})();


