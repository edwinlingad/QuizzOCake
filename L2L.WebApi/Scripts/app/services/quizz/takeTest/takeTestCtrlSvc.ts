interface ITakeTestCtrlSvc {
    initWithModel: (logResult: boolean, model: ITakeTestModel, quizzId: number, assignment: IAssignmentModel) => void;
    setTestSettings: (testSetting: ITestSettingModel) => void;
    startTest: () => void;
    goToPrevQuestion(): void;
    goToNextQuestion(): void;
    goToQuestion(idx: number): void;
    finishTest(finishBtn: IButtonElement): void;
    submitAnswer: (actualAnswer: any, isCorrect: boolean, point: number, pointsAvailable: number) => void;
    getCurrentActualAnswers(): any;
    getCurrentQuestion: () => IQuestionWithActualModel;
    getTestSettings: () => ITestSettingModel;
    getTest: () => ITakeTestModel;
    getUnfinishedQuestions(): IQuestionWithActualModel[];

    getMarkedQuestions(): IQuestionWithActualModel[];
    toggleMarkOfCurrentQuestion(): void;

    goToFurthestQuestion(): void;
    goToFinishTest(): void;

    save(saveForLateBtn: IButtonElement): void;
    load(testSnapshot: ITestSnapshot): void;
    reloadCurrent(): void;

    isPreviewMode(): boolean;
    isAssignment(): boolean;
    isDailyReward(): boolean;
    setDailyRewardValue(_isDailyReward: boolean, numPoints: number): void;
    getRewardAvailablePoints(): number;

    getCurrentOngoingQuizzId(): number;

    getAssignment(): IAssignmentModel;
}

(function () {
    l2lApp.factory('takeTestCtrlSvc', takeTestCtrlSvc);

    takeTestCtrlSvc.$inject = ["testSvc", "questionTypeSvc", "testProgressSvc", "$state", "testLogSvc", "cachedDataSvc", "currentUser", "notificationSvc", "broadcastSvc", "resourceSvc", "layoutSvc"];
    function takeTestCtrlSvc(
        testSvc: ITestSvc,
        questionTypeSvc: IQuestionTypeSvc,
        testProgressSvc: ITestProgressSvc,
        $state: ng.ui.IStateService,
        testLogSvc: ITestLogSvc,
        cachedDataSvc: ICachedDataSvc,
        currentUser: ICurrentUser,
        notificationSvc: INotificationSvc,
        broadcastSvc: IBroadcastSvc,
        resourceSvc: IResourceSvc,
        layoutSvc: ILayoutSvc
        ): ITakeTestCtrlSvc {

        var isReady: boolean = false;
        var user: IUserModel = currentUser.getUserData();
        var test: ITakeTestModel;
        var setting: ITestSettingModel;
        var availableQuestions: IQuestionModel[];
        var questionList: IQuestionWithActualModel[];
        var curIdx: number;
        var currentQuestion: IQuestionWithActualModel;
        var totalNumQuestions: number;
        var quizzId: number;
        var logResult: boolean;
        var quizz: IQuizzModel;
        var assignment: IAssignmentModel;
        var testProgress: ITestProgress;
        var markedQuestions: IQuestionWithActualModel[] = new Array<IQuestionWithActualModel>();
        var testSnapshot: ITestSnapshot = undefined;
        var isTestOngoing: boolean = false;

        function errorSave(response) {
            notificationSvc.error(str.errorSave);
        }

        function initWithModel(_logResult: boolean, model: ITakeTestModel, _quizzId: number, _assignment: IAssignmentModel) {
            assignment = _assignment;
            logResult = _logResult;
            test = model;
            availableQuestions = new Array<IQuestionModel>();
            model.questions.forEach(function (question) {
                availableQuestions.push(question);
            });
            isReady = true;
            quizzId = _quizzId;
            quizz = cachedDataSvc.getQuizzById(quizzId);
            if (user.isGuest)
                logResult = false;
        }

        function setTestSettings(testSetting: ITestSettingModel): void {
            setting = testSetting;

            if (user.isGuest || user.id === 0) {
                totalNumQuestions = availableQuestions.length >= 3 ? 3 : availableQuestions.length;
            } else {
                totalNumQuestions = setting.numberOfQuestions == 0 ? availableQuestions.length : setting.numberOfQuestions;
            }
        }

        function startTest(): void {
            function createQuestions() {
                questionList = new Array<IQuestionWithActualModel>();
                for (var i = 0; i < totalNumQuestions; i++) {
                    var nextQIndex: number = 0;
                    if (setting.isOrdered == false)
                        nextQIndex = Math.floor(Math.random() * availableQuestions.length);

                    var questionWithActualAnswer: IQuestionWithActualModel = availableQuestions[nextQIndex];
                    questionTypeSvc.getActualQuestion(questionWithActualAnswer, test);
                    questionWithActualAnswer.idx = i;
                    questionWithActualAnswer.isSubmitted = false;
                    questionWithActualAnswer.isMarked = false;

                    availableQuestions.splice(nextQIndex, 1);
                    questionList.push(questionWithActualAnswer);
                }
            }

            curIdx = -1;
            createQuestions();
            testSnapshot = undefined;
            isTestOngoing = true;
            if (!isPreviewMode())
                layoutSvc.changeTestSnapshot(-1);

            markedQuestions.splice(0, markedQuestions.length);
            testProgressSvc.start(setting, totalNumQuestions, questionList);
            testProgress = testProgressSvc.getTestProgress();
            goToNextQuestion();
        }

        function goToPrevQuestion(): void {
            if (curIdx <= 0)
                return;

            currentQuestion = questionList[--curIdx];
            syncCurIdx();
            questionTypeSvc.goToQuestion(currentQuestion, quizzId);
        }

        function goToNextQuestion(): void {
            if (curIdx >= totalNumQuestions - 1) {
                $state.go("si.takeTest.Finish");
                return;
            }

            currentQuestion = questionList[++curIdx];
            syncCurIdx();
            questionTypeSvc.goToQuestion(currentQuestion, quizzId);
        }

        function goToQuestion(idx: number): void {
            if (idx >= 0 && idx <= totalNumQuestions - 1) {
                curIdx = idx;
                currentQuestion = questionList[idx];
                syncCurIdx();
                questionTypeSvc.goToQuestion(currentQuestion, quizzId);
            }
        }

        function syncCurIdx() {
            testProgress.numQuestionsTaken = curIdx;
            if (curIdx > testProgress.furthestIdx)
                testProgress.furthestIdx = curIdx;
        }

        function getUnfinishedQuestions(): IQuestionWithActualModel[] {
            var list: IQuestionWithActualModel[] = new Array<IQuestionWithActualModel>();
            questionList.forEach(function (item: IQuestionWithActualModel) {
                if (item.isSubmitted == false)
                    list.push(item);
            });

            return list;
        }

        function finishTest(finishBtn: IButtonElement): void {
            function successTestSnapshot() {

            }
            function goToViewResult() {
                finishBtn.isReady = true;
                $state.go("si.viewResult", { testLogId: -1, quizzId: quizzId, testId: test.id });
            }

            function saveLogResult() {
                var testLog: ITestLogModel;
                var quizzInfo: IQuizzInfo = cachedDataSvc.getQuizzById(quizzId);
                var testResult: IQuestionTakenModel[] = testProgressSvc.getTestQuestionResults();
                var testResultScore: ITestProgress = testProgressSvc.getTestProgress();

                var log: ITestLogDataModel = {
                    quizzInfo: quizzInfo,
                    testResult: testResult,
                    testResultScore: testResultScore
                }

                testLog = {
                    id: 0,
                    score: testResultScore.score,
                    total: testResultScore.totalAvailableScore,
                    duration: 300,
                    dateTaken: new Date(),
                    comment: "",
                    resultBlob: JSON.stringify(log),
                    userId: currentUser.getUserData().id,
                    testSettingId: 0,
                    testSetting: setting,
                    quizzId: quizzId,
                    quizzName: quizz.title,
                    category: quizz.category
                }

                if (assignment !== undefined)
                    testLog.assignmentId = assignment.id;

                testLogSvc.createNewTestLog(testLog, function (data: ITestLogModel) {
                    var bcData: IBroadcastData = {
                        eventType: bcEventTypeEnum.newItem,
                        model: data
                    }
                    //broadcastSvc.postTestLogEvent(bcData);
                    layoutSvc.updateRecentTestResults();

                    notificationSvc.success("Result saved to server");

                    if (assignment !== undefined)
                        updateAssignment(data);
                    else
                        goToViewResult();

                    currentUser.getUpdatedUserData();
                }, errorSave);
            }

            function updateAssignment(testLog: ITestLogModel) {
                var testResultScore: ITestProgress = testProgressSvc.getTestProgress();
                var score: number = Math.ceil((testResultScore.score / testResultScore.totalAvailableScore) * 100);

                if (score >= assignment.assignmentGroup.targetScore) {
                    assignment.isCompleted = true;
                    assignment.completedScore = score;
                    assignment.testResultId = testLog.id;
                    resourceSvc.updateResource(enums.ResourceTypeEnum.Assignment, assignment,
                        function () {
                            //layoutSvc.removeAssignment(assignment.id);
                            layoutSvc.updateAssignments();
                            notificationSvc.success(str.assignmentComplete);
                        }, errorSave);
                }

                goToViewResult();
            }

            // finishTest Start
            isTestOngoing = false;
            finishBtn.isReady = false;
            testProgressSvc.finishTest();
            if (logResult == true)
                saveLogResult();
            else
                goToViewResult();
            layoutSvc.removeTestSnapshot();
            testSnapshot = undefined;
            //if (testSnapshot !== undefined)
            //    resourceSvc.deleteResource(enums.ResourceTypeEnum.TestSnapshot, testSnapshot.id, successTestSnapshot, errorSave);
        }

        function submitAnswer(actualAnswer: any, isCorrect: boolean, point: number, pointsAvailable: number): void {
            testProgressSvc.addFinishedQuestion(curIdx, currentQuestion, actualAnswer, isCorrect, point, pointsAvailable);
            if (curIdx + 1 >= testProgress.furthestIdx)
                testProgress.furthestIdx = curIdx + 1;
        }

        function getCurrentActualAnswer(): any {
            return testProgressSvc.getActualAnswer(curIdx);
        }

        function getCurrentQuestion(): IQuestionModel {
            return currentQuestion;
        }

        function getTestSettings(): ITestSettingModel {
            return setting;
        }

        function getTest(): ITakeTestModel {
            return test;
        }

        function isPreviewMode(): boolean {
            return !logResult;
        }

        function isAssignment(): boolean {
            return assignment !== undefined;
        }

        function getAssignment(): IAssignmentModel {
            return assignment;
        }

        var numPoints: number = 0;
        var isDailyRewardValue: boolean = false;
        function setDailyRewardValue(_isDailyReward: boolean, _numPoints: number) {
            isDailyRewardValue = _isDailyReward;
            numPoints = _numPoints;
        }

        function getRewardAvailablePoints(): number {
            return numPoints;
        }

        function isDailyReward(): boolean {
            return isDailyRewardValue;
        }

        function getMarkedQuestions(): IQuestionWithActualModel[] {
            return markedQuestions;
        }

        function goToFurthestQuestion(): void {
            goToQuestion(testProgress.furthestIdx);
        }

        function goToFinishTest(): void {
            $state.go("si.takeTest.Finish");
        }

        function toggleMarkOfCurrentQuestion(): void {
            currentQuestion.isMarked = !currentQuestion.isMarked;
            var idx: number = 0;

            for (var i: number = 0; i < markedQuestions.length; i++) {
                if (markedQuestions[i].idx >= currentQuestion.idx)
                    break;
            }
            idx = i;

            if (currentQuestion.isMarked) {
                if (idx == 0)
                    markedQuestions.unshift(currentQuestion);
                else if (idx >= markedQuestions.length)
                    markedQuestions.push(currentQuestion)
                else
                    markedQuestions.splice(idx, 0, currentQuestion);

            } else {
                markedQuestions.splice(idx, 1);
            }
        }

        function save(saveForLateBtn: IButtonElement) {
            function successSaveSnapshot() {
                saveForLateBtn.isReady = true;
                $state.go("si.quizzCategories");
            }

            function errorSaveSnapshot() {
                saveForLateBtn.isReady = true;
            }

            function successSaveNewSnapshot(data: ITestSnapshot) {
                layoutSvc.changeTestSnapshot(data.id);
                successSaveSnapshot();
            }

            var snapshot: ITakeTestCtrlSnapshot = {
                curIdx: curIdx,
                logResult: logResult,
                totalNumQuestions: totalNumQuestions,
                setting: setting,
                questionList: questionList,
                markedQuestions: markedQuestions,
                assignment: assignment
            };
            var testProgressSnapshot: ITestProgressSnapshot = testProgressSvc.save();
            var localTestSnapshot: ITestSnapshot = {
                id: 0,
                ownerId: 0,
                quizzId: quizzId,
                takeTestModelSnapshot: JSON.stringify(test),
                testProgressSnapshot: JSON.stringify(testProgressSnapshot),
                takeTestCtrlSnapshot: JSON.stringify(snapshot)
            }

            saveForLateBtn.isReady = false;
            if (testSnapshot === undefined)
                resourceSvc.createResource(enums.ResourceTypeEnum.TestSnapshot, localTestSnapshot, successSaveNewSnapshot, errorSaveSnapshot);
            else {
                localTestSnapshot.id = testSnapshot.id;
                localTestSnapshot.ownerId = testSnapshot.ownerId;
                resourceSvc.updateResource(enums.ResourceTypeEnum.TestSnapshot, localTestSnapshot, successSaveSnapshot, errorSaveSnapshot);
            }
        }

        function load(_testSnapshot: ITestSnapshot) {
            isTestOngoing = true;
            testSnapshot = _testSnapshot;
            test = JSON.parse(_testSnapshot.takeTestModelSnapshot);
            var snapshot: ITakeTestCtrlSnapshot = JSON.parse(_testSnapshot.takeTestCtrlSnapshot);
            quizzId = _testSnapshot.quizzId;
            quizz = cachedDataSvc.getQuizzById(quizzId);

            curIdx = snapshot.curIdx;
            logResult = snapshot.logResult;
            totalNumQuestions = snapshot.totalNumQuestions;
            setting = snapshot.setting;
            questionList = snapshot.questionList;
            markedQuestions = snapshot.markedQuestions;
            assignment = snapshot.assignment;

            testProgressSvc.load(_testSnapshot, setting);
            testProgress = testProgressSvc.getTestProgress();

            if (!isPreviewMode())
                layoutSvc.changeTestSnapshot(testSnapshot.id);

            goToQuestion(curIdx);
        }

        function reloadCurrent(): void {
            goToQuestion(curIdx);
        }

        function getCurrentOngoingQuizzId(): number {
            if (isTestOngoing)
                return quizzId;
            return 0;
        }

        return {
            initWithModel: initWithModel,
            setTestSettings: setTestSettings,
            startTest: startTest,
            goToPrevQuestion: goToPrevQuestion,
            goToNextQuestion: goToNextQuestion,
            goToQuestion: goToQuestion,
            finishTest: finishTest,
            getUnfinishedQuestions: getUnfinishedQuestions,
            submitAnswer: submitAnswer,
            getCurrentQuestion: getCurrentQuestion,
            getTestSettings: getTestSettings,
            getTest: getTest,

            isPreviewMode: isPreviewMode,
            isAssignment: isAssignment,
            isDailyReward: isDailyReward,
            setDailyRewardValue: setDailyRewardValue,
            getRewardAvailablePoints: getRewardAvailablePoints,

            getCurrentActualAnswers: getCurrentActualAnswer,

            getMarkedQuestions: getMarkedQuestions,
            toggleMarkOfCurrentQuestion: toggleMarkOfCurrentQuestion,

            goToFinishTest: goToFinishTest,
            goToFurthestQuestion: goToFurthestQuestion,

            load: load,
            save: save,
            reloadCurrent: reloadCurrent,

            getCurrentOngoingQuizzId: getCurrentOngoingQuizzId,
            getAssignment: getAssignment,
        }
    }

})();