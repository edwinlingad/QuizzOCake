/// <reference path="../../../models/quizz/quizzmodels.ts" />
module takeTestQandACtrl {
    export interface IScope {
        questionModel: IQuestionWithActualModel;
        question: IQAQuestionModel;
        answers: IActualAnswer[];
        //submit: () => void;
        changed: () => void;
        submitBtn: IButtonElement;

        toggleMarkBtn: IButtonElement;
    }

    export interface IStateParams {
        testId: any;
        qId: any;
        quizzId: number;
    }
}

(function () {
    l2lControllers.controller('takeTestQandACtrl', takeTestQandACtrl);

    takeTestQandACtrl.$inject = ["$scope", "takeTestCtrlSvc"];
    function takeTestQandACtrl(
        $scope: takeTestQandACtrl.IScope,
        takeTestCtrlSvc: ITakeTestCtrlSvc
        ) {

        var questionModel: IQuestionWithActualModel;
        var actualQuestion: IQAQuestionModel;
        var answers: IActualAnswer[];
        var submitBtn: IButtonElement = {
            disabled: true,
            isReady: true,
            click: function() {
                if (submitBtn.disabled)
                    return;

                submitBtn.isReady = false;
                submitBtn.disabled = true;

                questionModel.isSubmitted = true;
                if (actualQuestion.answersInOrder == true) {
                    checkAnswersInOrder();
                } else {
                    checkAnswersNotInOrder();
                }

                takeTestCtrlSvc.goToNextQuestion();
            }
        }
        var toggleMarkBtn: IButtonElement = {
            click: function() {
                takeTestCtrlSvc.toggleMarkOfCurrentQuestion();
                updateMarkBtnName();
            }
        }

        function init() {
            questionModel = takeTestCtrlSvc.getCurrentQuestion();
            actualQuestion = questionModel.actualQuestion;

            answers = new Array<IActualAnswer>();
            if (questionModel.isSubmitted) {
                var answersTmp: IActualAnswer[] = takeTestCtrlSvc.getCurrentActualAnswers();
                for (var i = 0; i < actualQuestion.answers.length; i++) {
                    answers.push({ value: answersTmp[i].value, isCorrect: false });
                }
            } else {
                for (var i = 0; i < actualQuestion.answers.length; i++) {
                    answers.push({ value: "", isCorrect: false });
                }
            }

            updateMarkBtnName();
            attachEnterPress();          
        }

        function checkAnswersInOrder(): void {
            var numCorrectAnswers: number = 0;
            var totalNumAnswers: number = 0;
            for (var i = 0; i < answers.length; i++) {
                if (answers[i].value.toLowerCase().trim() === actualQuestion.answers[i].answer.toLowerCase().trim()) {
                    answers[i].isCorrect = true;
                    numCorrectAnswers++;
                }
                totalNumAnswers++;
            }

            submitAnswer(numCorrectAnswers, totalNumAnswers);
        }

        function submitAnswer(numCorrectAnswer: number, totalNumAnswers: number) {
            
            var isCorrect: boolean = numCorrectAnswer == totalNumAnswers;
            if (actualQuestion.isMultiplePoints) {
                takeTestCtrlSvc.submitAnswer(answers, isCorrect, numCorrectAnswer, totalNumAnswers);
            } else {
                var points: number = isCorrect ? 1 : 0;
                takeTestCtrlSvc.submitAnswer(answers, isCorrect, points, 1);
            }
        }

        function checkAnswersNotInOrder(): void {
            var numCorrectAnswer: number = 0;
            var totalNumAnswers: number = 0;

            var correctAnswersCopy: string[] = new Array<string>();
            for (var i = 0; i < actualQuestion.answers.length; i++) {
                correctAnswersCopy.push(actualQuestion.answers[i].answer);
            }

            for (var i = 0; i < answers.length; i++) {
                for (var j = 0; j < correctAnswersCopy.length; j++) {
                    if (answers[i].value.toLowerCase() === correctAnswersCopy[j].toLowerCase()) {
                        answers[i].isCorrect = true;
                        numCorrectAnswer++;
                        correctAnswersCopy.splice(j, 1);
                        break;
                    }
                }
                totalNumAnswers++;
            }

            submitAnswer(numCorrectAnswer, totalNumAnswers);
        }

        function changed() {
            submitBtn.disabled = true;
            answers.forEach(function (item) {
                if (item.value.trim().length != 0)
                    submitBtn.disabled = false;
            });
        }

        function updateMarkBtnName() {
            toggleMarkBtn.name = questionModel.isMarked ? "marked" : "mark";
        }

        init();

        //$scope.submit = submit;
        $scope.questionModel = questionModel;
        $scope.question = actualQuestion;
        $scope.answers = answers;
        $scope.changed = changed;
        $scope.submitBtn = submitBtn;
        $scope.toggleMarkBtn = toggleMarkBtn;
    }
})();

