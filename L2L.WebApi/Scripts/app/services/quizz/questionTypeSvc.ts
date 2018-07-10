interface IQuestionTypeSvc {
    goToNewQuestion(questionTypeString: string, param: any): void;
    goToEditQuestion(question: IQuestionModel, quizzId: number): void;
    goToQuestion(question: IQuestionWithActualModel, quizzId: number): void;
    getActualQuestion(question: IQuestionWithActualModel, takeTest: ITakeTestModel): void;
    getTotalAvailableScore(question: IQuestionWithActualModel): number;
}

(function () {
    l2lApp.service('questionTypeSvc', questionTypeSvc);

    questionTypeSvc.$inject = ["$state", "$location"];
    function questionTypeSvc(
        $state: ng.ui.IStateService,
        $location: ng.ILocationService
        ): IQuestionTypeSvc {

        function goToNewQuestion(questionTypeString: string, param: any): void {
            var stateTo: string;
            switch (questionTypeString) {
                case "QandA":
                    stateTo = "si.editQandA";
                    break;
                case "MultipleChoice":
                    stateTo = "si.editMultipleChoice";
                    break;
                case "MultiChoiceSame":
                    stateTo = "si.editMultiChoiceSameQuestion";
                    break;
            }
            $state.go(stateTo, param);
        }

        function goToEditQuestion(question: IQuestionModel, quizzId: number): void {
            var stateTo: string;

            switch (question.questionType) {
                case QuestionTypeEnum.QandA:
                    stateTo = "si.editQandA";
                    break;
                case QuestionTypeEnum.MultipleChoice:
                    stateTo = "si.editMultipleChoice";
                    break;
                case QuestionTypeEnum.MultiChoiceSame:
                    stateTo = "si.editMultiChoiceSameQuestion";
                    break;
            }
            $state.go(stateTo, { qId: question.id, testId: question.testId, quizzId: quizzId });
        }

        function goToQuestion(question: IQuestionWithActualModel, quizzId: number): void {
            var stateTo: string;
            switch (question.questionType) {
                case QuestionTypeEnum.QandA:
                    stateTo = "si.takeTest.QandA";
                    break;
                case QuestionTypeEnum.MultipleChoice:
                    stateTo = "si.takeTest.MultiChoice";
                    break;
                case QuestionTypeEnum.MultiChoiceSame:
                    stateTo = "si.takeTest.MultiChoiceSame";
                    break;
            }
            $state.go(stateTo, { qId: question.id, testId: question.testId, quizzId: quizzId });
            $location.replace();
        }

        function getActualQuestion(question: IQuestionWithActualModel, takeTest: ITakeTestModel): void {
            var qId: number = question.questionId;
            // TODO: optimize
            switch (question.questionType) {
                case QuestionTypeEnum.QandA:
                    var qaQuestions: IQAQuestionModel[] = takeTest.qandAQuestions;
                    for (var i = 0; i < qaQuestions.length; i++) {
                        if (qaQuestions[i].id == qId) {
                            question.actualQuestion = qaQuestions[i];
                            qaQuestions.splice(i, 1);
                            break;
                        }
                    }
                    break;
                case QuestionTypeEnum.MultipleChoice:
                    var mcQuestions: IMCQuestionModel[] = takeTest.multiChoiceQuestions;
                    for (var i = 0; i < mcQuestions.length; i++) {
                        if (mcQuestions[i].id == qId) {
                            question.actualQuestion = mcQuestions[i];
                            mcQuestions.splice(i, 1);
                            break;
                        }
                    }
                    break;
                case QuestionTypeEnum.MultiChoiceSame:
                    var mcsQuestions: IMultiChoiceSameQuestionModel[] = takeTest.multiChoiceSameQuestions;
                    for (var i = 0; i < mcsQuestions.length; i++) {
                        if (mcsQuestions[i].id == qId) {
                            question.actualQuestion = mcsQuestions[i];
                            mcsQuestions.splice(i, 1);
                            break;
                        }
                    }
                    break;
            }
        }

        function getTotalAvailableScore(question: IQuestionWithActualModel): number {
            function getQandATotalScore() {
                var qaQuestion: IQAQuestionModel = question.actualQuestion;
                return qaQuestion.isMultiplePoints ? qaQuestion.answers.length : 1;
            }

            function getMultiChoiceTotalScore(): number {
                var mcQuestion: IMCQuestionModel = question.actualQuestion;
                var totalScore: number = 0;
                if (mcQuestion.isMultiplePoints) {
                    mcQuestion.choices.forEach(function (item: IMCChoiceModel) {
                        if (item.isAnswer)
                            totalScore++;
                    });
                }
                else
                    totalScore = 1;

                return totalScore;
            }

            function getMultiChoiceSameTotalScore(): number {
                var mcsQuestion: IMultiChoiceSameQuestionModel = question.actualQuestion;
                var totalScore: number = 0;

                if (mcsQuestion.isMultiplePoints) {
                    var answersInt: string[] = mcsQuestion.answers.split(",");
                    totalScore = answersInt.length;
                }
                else
                    totalScore = 1;

                return totalScore;                
            }

            var score: number = 1;
            switch (question.questionType) {
                case QuestionTypeEnum.QandA:
                    score = getQandATotalScore();
                    break;
                case QuestionTypeEnum.MultipleChoice:
                    score = getMultiChoiceTotalScore();
                    break;
                case QuestionTypeEnum.MultiChoiceSame:
                    score = getMultiChoiceSameTotalScore();
                    break;
            }

            return score;
        }   

        return {
            goToNewQuestion: goToNewQuestion,
            goToEditQuestion: goToEditQuestion,
            goToQuestion: goToQuestion,
            getActualQuestion: getActualQuestion,
            getTotalAvailableScore: getTotalAvailableScore
        }
    }
})();