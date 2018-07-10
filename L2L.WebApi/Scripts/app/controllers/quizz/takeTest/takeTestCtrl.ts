module takeTestCtrl {
    export interface IScope {
        page: IPage;
        quizz: IQuizzModel;
        testProgress: ITestProgress

        isPreview: boolean;
        isAssignment: boolean;
        isDailyReward: boolean;
        rewardPoinstAvailable: number;

        idx: any;
        prevBtn: IButtonElement;
        nextBtn: IButtonElement;
        goToQuestionIdxBtn: IButtonElement;
        counterBtn: IButtonElement;
        cancelGoTo(): void;

        prevBtnDisabled(): boolean;
        nextBtnDisabled(): boolean;

        markedQuestions: IQuestionWithActualModel[];

        goToQuestion(item: IQuestionWithActualModel): void;
        goToFurthestQuestion(): void;
        goToFinishTest(): void;
        saveForLaterBtn: IButtonElement;
    }

    export interface IStateParams {
        testId: any;
        quizzId: number;
    }
}

(function () {
    l2lControllers.controller('takeTestCtrl', takeTestCtrl);

    takeTestCtrl.$inject = ["$scope", "$stateParams", "testProgressSvc", "cachedDataSvc", "notificationSvc", "$timeout", "takeTestCtrlSvc"];
    function takeTestCtrl(
        $scope: takeTestCtrl.IScope,
        $stateParams: prepTestCtrl.IStateParams,
        testProgressSvc: ITestProgressSvc,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        $timeout: ng.ITimeoutService,
        takeTestCtrlSvc: ITakeTestCtrlSvc
        ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 1,
            disabled: false
        };
        var quizz: IQuizzModel = {};
        var testProgress: ITestProgress = testProgressSvc.getTestProgress();
        var markedQuestions: IQuestionWithActualModel[];
        var prevBtn: IButtonElement = {
            disabled: false,
            click: function () {
                if (testProgress.numQuestionsTaken <= 0 || prevBtn.disabled == true)
                    return;

                //prevBtn.disabled = true;
                takeTestCtrlSvc.goToPrevQuestion();
                //$timeout(function () {
                //    prevBtn.disabled = false;
                //}, 500);
            }
        };
        var nextBtn: IButtonElement = {
            disabled: false,
            click: function () {
                if (testProgress.numQuestionsTaken >= testProgress.totalQuestions - 1 || nextBtn.disabled == true)
                    return;

                //nextBtn.disabled = true;
                takeTestCtrlSvc.goToNextQuestion();
                //$timeout(function () {
                //    nextBtn.disabled = false;
                //}, 500);
            }
        };
        var counterBtn: IButtonElement = {
            visible: true,
            click: function () {
                counterBtn.visible = false;
                $scope.idx = testProgress.numQuestionsTaken + 1;
                $("#go-to-input").focus();
            }
        };
        var goToQuestionIdxBtn: IButtonElement = {
            disabled: false,
            click: function () {
                if (goToQuestionIdxBtn.disabled == true)
                    return;
                var idx: number = parseInt($scope.idx);
                if (isNaN(idx))
                    return;

                takeTestCtrlSvc.goToQuestion(idx - 1);
                counterBtn.visible = true;
            }
        };
        var saveForLaterBtn: IButtonElement = {
            isReady: true,
            click: function () {
                if (!saveForLaterBtn.isReady)
                    return;
                takeTestCtrlSvc.save(saveForLaterBtn);
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

            function successQuizz(data: IQuizzModel) {
                $scope.quizz = quizz = data;
                updateIsReady();
            }

            testProgress = testProgressSvc.getTestProgress();
            markedQuestions = takeTestCtrlSvc.getMarkedQuestions();
            quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, successQuizz, errorLoad);
            $scope.isPreview = takeTestCtrlSvc.isPreviewMode();
            $scope.isAssignment = takeTestCtrlSvc.isAssignment();
            $scope.idx = testProgress.numQuestionsTaken + 1;

            $scope.isDailyReward = takeTestCtrlSvc.isDailyReward();
            $scope.rewardPoinstAvailable = takeTestCtrlSvc.getRewardAvailablePoints();

        }

        function cancelGoTo() {
            counterBtn.visible = true;
        }

        function prevBtnDisabled(): boolean {
            return testProgress.numQuestionsTaken <= 0 || prevBtn.disabled === true;
        }

        function nextBtnDisabled(): boolean {
            return testProgress.numQuestionsTaken >= testProgress.totalQuestions - 1 || nextBtn.disabled === true;
        }

        function goToFurthestQuestion(): void {
            takeTestCtrlSvc.goToFurthestQuestion();
        }

        function goToFinishTest(): void {
            takeTestCtrlSvc.goToFinishTest();
        }

        function goToQuestion(item: IQuestionWithActualModel): void {
            takeTestCtrlSvc.goToQuestion(item.idx);
        }

        init();

        $scope.page = page;
        $scope.quizz = quizz;
        $scope.testProgress = testProgress;

        $scope.prevBtn = prevBtn;
        $scope.nextBtn = nextBtn;
        $scope.counterBtn = counterBtn;
        $scope.goToQuestionIdxBtn = goToQuestionIdxBtn;
        $scope.cancelGoTo = cancelGoTo;

        $scope.prevBtnDisabled = prevBtnDisabled;
        $scope.nextBtnDisabled = nextBtnDisabled;

        $scope.markedQuestions = markedQuestions;

        $scope.goToFurthestQuestion = goToFurthestQuestion;
        $scope.goToFinishTest = goToFinishTest;
        $scope.goToQuestion = goToQuestion;
        $scope.saveForLaterBtn = saveForLaterBtn;
    }
})();