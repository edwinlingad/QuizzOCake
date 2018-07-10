module viewReviewerCtrl {
    export interface IScope {
        page: IPage;
        quizz: IQuizzModel;
        reviewers: IReviewerFromQuestionModel[];
        toggleShowAnswer: (item: IReviewerFromQuestionModel) => void;

        openAllAnswers: () => void;
        closeAllAnswers: () => void;

        openFlashCards: (fType: string) => void;

        goBack: () => void;
    }

    export interface IStateParams {
        quizzId: number;
    }
}

(function () {
    l2lControllers.controller("viewReviewerCtrl", viewReviewerCtrl);

    viewReviewerCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "$anchorScroll", "flashCardSvc", "flashCardReviewerSvc", "builtInQuestionsSvc"];
    function viewReviewerCtrl(
        $scope: viewReviewerCtrl.IScope,
        $stateParams: viewReviewerCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        $anchorScroll: ng.IAnchorScrollService,
        flashCardSvc: IFlashCardSvc,        
        flashCardReviewerSvc: IFlashCardReviewerSvc,        
        builtInQuestionsSvc: IBuiltInQuestionsSvc
        ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 2,
            disabled: false
        };
        var quizz: IQuizzModel = {};
        var reviewers: IReviewerFromQuestionModel[] = new Array<IReviewerFromQuestionModel>();

        function init() {
            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successReviewer(data: IReviewerFromQuestionModel[]) {
                $scope.reviewers = reviewers = data;
                updateIsReady();

                reviewers.forEach(function (item: IReviewerFromQuestionModel) {
                    item.isAnswerShown = false;
                });
            }

            function successQuizz(data: IQuizzModel) {
                $scope.quizz = quizz = data;
                updateIsReady();

                if (quizz.isBuiltIn)
                    reviewers = builtInQuestionsSvc.getFlashCards(quizz.id, 20, successReviewer, errorLoad);
                else
                    reviewers = flashCardSvc.getReviewerFromQuestions(quizz.testId, successReviewer, errorLoad);
            }

            $anchorScroll("top");
            quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, successQuizz, errorLoad);
        }

        function toggleShowAnswer(item: IReviewerFromQuestionModel): void {
            item.isAnswerShown = !item.isAnswerShown;
        }

        function openAllAnswers(): void {
            reviewers.forEach(function (item: IReviewerFromQuestionModel) {
                item.isAnswerShown = true;
            });
        }

        function closeAllAnswers(): void {
            reviewers.forEach(function (item: IReviewerFromQuestionModel) {
                item.isAnswerShown = false;
            });
        }

        function openFlashCards(fType: string): void {
            var isRandomized: boolean = false;
            flashCardReviewerSvc.init($stateParams.quizzId, reviewers);

            if (fType === "random") {
                isRandomized = true;
            }
            flashCardReviewerSvc.start(isRandomized);

            $state.go("si.viewAsFlashCard.flashCardItem", { quizzId: $stateParams.quizzId, fcId: 0 });
        }

        function goBack() {
            $state.go("si.quizzDetail", { quizzId: $stateParams.quizzId, view: 1 });
        }

        init();

        $scope.page = page;
        $scope.quizz = quizz;
        $scope.reviewers = reviewers;
        $scope.toggleShowAnswer = toggleShowAnswer;
        $scope.openAllAnswers = openAllAnswers;
        $scope.closeAllAnswers = closeAllAnswers;
        $scope.goBack = goBack;
        $scope.openFlashCards = openFlashCards;
    }
})();