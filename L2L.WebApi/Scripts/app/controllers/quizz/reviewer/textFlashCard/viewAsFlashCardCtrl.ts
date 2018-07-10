module viewAsFlashCardCtrl {
    export interface IScope {
        page: IPage;
        quizz: IQuizzModel;
        finishBtn: IButtonElement;
        goBack: () => void;
    }

    export interface IStateParams {
        quizzId: number;
    }
} 

(function () {
    l2lControllers.controller("viewAsFlashCardCtrl", viewAsFlashCardCtrl);

    viewAsFlashCardCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "$anchorScroll", "flashCardReviewerSvc"];
    function viewAsFlashCardCtrl(
        $scope: viewAsFlashCardCtrl.IScope,
        $stateParams: viewAsFlashCardCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        $anchorScroll: ng.IAnchorScrollService,
        flashCardReviewerSvc: IFlashCardReviewerSvc
        ) {

        var page: IPage = {
            isReady: false,
            numResourceToWait: 1,
            disabled: false
        };
        var quizz: IQuizzModel = {};
        var finishBtnAlreadyPressed: boolean = false;
        var finishBtn: IButtonElement = {
            name: "Finish",
            click: function () {
                if(finishBtnAlreadyPressed) {
                    finishBtn.name = "To Reviewer",
                    goBack();
                    return;
                }

                finishBtnAlreadyPressed = true;
                $state.go("si.viewAsFlashCard.viewMarkedflashCards", { quizzId: $stateParams.quizzId });
            }
        };

        function init() {
            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successQuizz(data: IQuizzModel) {
                $scope.quizz = quizz = data;
                updateIsReady();
            }

            $anchorScroll("top");
            quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, successQuizz, errorLoad);
        }

        function goBack() {
            $state.go("si.quizzDetail", { quizzId: quizz.id, view: 2});
        }

        init();

        $scope.page = page;
        $scope.quizz = quizz;
        $scope.finishBtn = finishBtn;
        $scope.goBack = goBack;
    }
})();