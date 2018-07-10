module viewAsFlashCardItemCtrl {
    export interface IScope {
        reviewer: IReviewerFromQuestionModel;
        showAnswerBtn: IButtonElement;
        prevBtn: IButtonElement;
        nextBtn: IButtonElement;
        markBtn: IButtonElement;
        finishBtn: IButtonElement;
        counterBtn: IButtonElement;
        idx: any;
        goToReviewerIdxBtn: IButtonElement;
        cancelGoTo: () => void;

        counter: ICounter;
    }

    export interface IStateParams {
        quizzId: number;
        fcId: any;
    }
}

(function () {
    l2lControllers.controller("viewAsFlashCardItemCtrl", viewAsFlashCardItemCtrl);

    viewAsFlashCardItemCtrl.$inject = ["$scope", "$stateParams", "flashCardReviewerSvc", "$state", "$location"];
    function viewAsFlashCardItemCtrl(
        $scope: viewAsFlashCardItemCtrl.IScope,
        $stateParams: viewAsFlashCardItemCtrl.IStateParams,
        flashCardReviewerSvc: IFlashCardReviewerSvc,
        $state: ng.ui.IStateService,
        $location: ng.ILocationService
        ) {

        var reviewer: IReviewerFromQuestionModel;
        var showAnswerBtn: IButtonElement = {
            visible: false,
            click: function () {
                if (showAnswerBtn.visible)
                    flashCardReviewerSvc.goToNextReviewer();

                showAnswerBtn.visible = true;
            }
        };
        var prevBtn: IButtonElement = {
            disabled: !flashCardReviewerSvc.canGoToPrevReviewer(),
            click: function () {
                flashCardReviewerSvc.goToPrevReviewer();
            }
        };
        var nextBtn: IButtonElement = {
            visible: flashCardReviewerSvc.canGoToNextReviewer(),
            click: function () {
                flashCardReviewerSvc.goToNextReviewer();
            }
        };
        var finishBtn: IButtonElement = {
            visible: !flashCardReviewerSvc.canGoToNextReviewer(),
            click: function () {
                $state.go("si.viewAsFlashCard.viewMarkedflashCards", { quizzId: $stateParams.quizzId });
            }
        };
        var markBtn: IButtonElement = {
            click: function () {
                reviewer.isMarked = !reviewer.isMarked;
                markBtn.name = reviewer.isMarked ? "marked" : "mark";
            }
        }
        var counterBtn: IButtonElement = {
            visible: true,
            click: function () {
                counterBtn.visible = false;
                $scope.idx = $scope.counter.current + 1;
                $("#go-to-input").focus();
            }
        };
        var goToReviewerIdxBtn: IButtonElement = {
            disabled: false,
            click: function () {
                if (goToReviewerIdxBtn.disabled == true)
                    return;
                var idx: number = parseInt($scope.idx);
                if (isNaN(idx))
                    return;

                flashCardReviewerSvc.goToReviewerIdx(idx-1);
            }
        }

        function init() {
            var idx: number = parseInt($stateParams.fcId);
            reviewer = flashCardReviewerSvc.getReviewerAtIdx(idx);

            markBtn.name = reviewer.isMarked ? "marked" : "mark";
            $location.replace();
        }

        function cancelGoTo() {
            counterBtn.visible = true;
        }

        init();

        $scope.reviewer = reviewer;
        $scope.prevBtn = prevBtn;
        $scope.nextBtn = nextBtn;
        $scope.markBtn = markBtn;
        $scope.showAnswerBtn = showAnswerBtn;
        $scope.finishBtn = finishBtn;   
        $scope.counter = flashCardReviewerSvc.counter = flashCardReviewerSvc.counter;
        $scope.counterBtn = counterBtn;
        $scope.goToReviewerIdxBtn = goToReviewerIdxBtn;
        $scope.cancelGoTo = cancelGoTo;
    }
})();