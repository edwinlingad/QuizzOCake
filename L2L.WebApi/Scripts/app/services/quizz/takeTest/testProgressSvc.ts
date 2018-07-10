interface ITestProgress {
    numQuestionsTaken: number;
    totalQuestions: number;

    numCorrectAnswers: number;

    score: number;
    totalAvailableScore: number;

    furthestIdx: number;
}

interface ITestProgressSvc {
    start: (setting: ITestSettingModel, totalQuestions: number, questionList: IQuestionWithActualModel[]) => void;
    addFinishedQuestion: (idx: number, question: IQuestionWithActualModel, actualAnswer: any, isCorrect: boolean, point: number, pointsAvailable: number) => void;
    getTestProgress: () => ITestProgress;
    getTestQuestionResults: () => IQuestionTakenModel[];
    getActualAnswer(idx: number): any;
    finishTest(): void;

    save(): ITestProgressSnapshot;
    load(testSnapshot: ITestSnapshot, setting: ITestSettingModel): void;
}

(function () {
    l2lApp.factory('testProgressSvc', testProgressSvc);

    testProgressSvc.$inject = ["questionTypeSvc"];
    function testProgressSvc(
        questionTypeSvc: IQuestionTypeSvc
        ): ITestProgressSvc {

        var testSetting: ITestSettingModel;
        var questionsTaken: IQuestionTakenModel[];
        var testProgress: ITestProgress;

        function start(setting: ITestSettingModel, totalQuestions: number, questionList: IQuestionWithActualModel[]): void {
            testSetting = setting;
            questionsTaken = new Array<IQuestionTakenModel>();
            testProgress = {
                numQuestionsTaken: 0,
                totalQuestions: totalQuestions,
                numCorrectAnswers: 0,
                score: 0,
                totalAvailableScore: 0,
                furthestIdx: 0
            }

            questionList.forEach(function (item: IQuestionWithActualModel) {
                var model: IQuestionTakenModel = {
                    
                    isCorrect: false,
                    score: 0,
                    totalAvailableScore: questionTypeSvc.getTotalAvailableScore(item)
                }

                questionsTaken.push(model);
            });
        }

        function addFinishedQuestion(idx: number, question: IQuestionWithActualModel, actualAnswer: any, isCorrect: boolean, score: number, totalAvailableScore: number): void {
            var model: IQuestionTakenModel = questionsTaken[idx];
            model.question = question;
            model.answer = actualAnswer;
            model.isCorrect = isCorrect;
            model.score = score;
            model.totalAvailableScore = totalAvailableScore;

            if (isCorrect == true)
                testProgress.numCorrectAnswers++;

            testProgress.numQuestionsTaken = idx;
        }

        function getActualAnswer(idx: number): any {
            var model: IQuestionTakenModel = questionsTaken[idx];
            return model.answer;
        }

        function getTestProgress(): ITestProgress {
            return testProgress;
        }

        function getTestQuestionResults(): IQuestionTakenModel[] {
            return questionsTaken;
        }

        function finishTest() {
            testProgress.score = 0;
            testProgress.totalAvailableScore = 0;
            testProgress.numCorrectAnswers = 0;

            questionsTaken.forEach(function (item: IQuestionTakenModel) {
                testProgress.numCorrectAnswers += item.isCorrect ? 1 : 0;
                testProgress.score += item.score;
                testProgress.totalAvailableScore += item.totalAvailableScore;
            });
        }

        function save(): ITestProgressSnapshot {
            var snapshot: ITestProgressSnapshot = {
                testProgress: testProgress,
                questionsTaken: questionsTaken
            }

            return snapshot;
        }

        function load(testSnapshot: ITestSnapshot, setting: ITestSettingModel): void {
            testSetting = setting;
            var snapshot: ITestProgressSnapshot = JSON.parse(testSnapshot.testProgressSnapshot);
            testProgress = snapshot.testProgress;
            questionsTaken = snapshot.questionsTaken;
        }

        return {
            start: start,
            getTestProgress: getTestProgress,
            addFinishedQuestion: addFinishedQuestion,
            getTestQuestionResults: getTestQuestionResults,
            getActualAnswer: getActualAnswer,
            finishTest: finishTest,
            save: save,
            load: load
        }
    }
})();
