module finishTestCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage,
        goBack(): void;
        finishTestBtn: IButtonElement;

        unfinishedQuestions: IQuestionWithActualModel[];

        goToQueston(item: IQuestionWithActualModel): void;
    }

    export interface IStateParams {
        quizzId: number;
    }
}

(function () {
    l2lControllers.controller('finishTestCtrl', finishTestCtrl);

    finishTestCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc","takeTestCtrlSvc"];
    function finishTestCtrl(
        $scope: finishTestCtrl.IScope,
        $stateParams: finishTestCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        resourceSvc: IResourceSvc,
        $timeout: ng.ITimeoutService,
        dialogSvc: IDialogSvc,
        takeTestCtrlSvc: ITakeTestCtrlSvc
        ) {
        var page: IPage = {
            isReady: true,
            numResourceToWait: 0,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var unfinishedQuestions: IQuestionWithActualModel[];
        var finishTestBtn: IButtonElement = {
            isReady: true,
            click: function () {
                if (finishTestBtn.isReady == false)
                    return;

                if (unfinishedQuestions.length > 0) {
                    dialogSvc.confirm(str.unansweredQuestions, function (result) {
                        if (result == true) {
                            takeTestCtrlSvc.finishTest(finishTestBtn);
                        }
                    });
                } else {
                    takeTestCtrlSvc.finishTest(finishTestBtn);
                }
            }
        }

        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            unfinishedQuestions = takeTestCtrlSvc.getUnfinishedQuestions();
        }

        function goToQueston(item: IQuestionWithActualModel): void {
            takeTestCtrlSvc.goToQuestion(item.idx);
        }

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.finishTestBtn = finishTestBtn;
        $scope.unfinishedQuestions = unfinishedQuestions;
        $scope.goBack = goBack;
        $scope.goToQueston = goToQueston;
    }
})();