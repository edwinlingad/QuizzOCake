module viewMarkedFlashCardsCtrl {
    export interface IScope {
        reviewers: IReviewerFromQuestionModel[];
        showAnswer(item: IReviewerFromQuestionModel): void;
    }

    export interface IStateParams {
        quizzId: number;
    }
}
 
(function () {
    l2lControllers.controller("viewMarkedFlashCardsCtrl", viewMarkedFlashCardsCtrl);

    viewMarkedFlashCardsCtrl.$inject = ["$scope", "flashCardReviewerSvc", "$state", "$location", "$stateParams"];
    function viewMarkedFlashCardsCtrl(
        $scope: viewMarkedFlashCardsCtrl.IScope,
        flashCardReviewerSvc: IFlashCardReviewerSvc,
        $state: ng.ui.IStateService,
        $location: ng.ILocationService,
        $stateParams: viewMarkedFlashCardsCtrl.IStateParams
        ) {

        var reviewers: IReviewerFromQuestionModel[];

        function init() {
            reviewers = flashCardReviewerSvc.getMarkedReviewers();
            if(reviewers.length == 0) {
                $state.go("si.quizzDetail", { quizzId: $stateParams.quizzId, view: 2 });
                return;
            }
            $location.replace();
        }

        function showAnswer(item: IReviewerFromQuestionModel) {
            item.isAnswerShown = true;
        }

        init();

        $scope.reviewers = reviewers;
        $scope.showAnswer = showAnswer;
    }
})();