module confirmEmailCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        quizz: IQuizzModel;
        goBack(): void;
    }

    export interface IStateParams {
        userId: string;
        code: string;
    }
}

(function () {
    l2lControllers.controller('confirmEmailCtrl', confirmEmailCtrl);

    confirmEmailCtrl.$inject = ["$stateParams", "$state", "accountSvc", "notificationSvc"];

    function confirmEmailCtrl(
        $stateParams: confirmEmailCtrl.IStateParams,
        $state: ng.ui.IStateService,
        accountSvc: IAccount,
        notificationSvc: INotificationSvc
    ) {
        function init() {
            accountSvc.confirmEmail($stateParams.userId, $stateParams.code,
                function () {
                    notificationSvc.success(str.emailConfirmSuccess);
                }, function () {
                    notificationSvc.error(str.emailConfirmFailed);
                });

            $state.go("nsi.index");
        }

        init();
    }
})();