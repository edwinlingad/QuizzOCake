module takeTestMultiChoiceCtrl {
    export interface IScope {
        questionModel: IQuestionWithActualModel;
        question: IMCQuestionModel;
        choices: IMultiChoiceAnswer[];
        isMultiAnswer: boolean;
        selectedIndex: ISelectedIndex;
        changed: (item: IMultiChoiceAnswer) => void;
        submitBtn: IButtonElement;

        toggleMarkBtn: IButtonElement;
    }

    export interface IStateParams {
        testId: any;
        qId: any;
    }

    export interface ISelectedIndex {
        value: number;
    }
}

(function () {
    l2lControllers.controller('takeTestMultiChoiceCtrl', takeTestMultiChoiceCtrl);

    takeTestMultiChoiceCtrl.$inject = ["$scope", "takeTestCtrlSvc"];
    function takeTestMultiChoiceCtrl(
        $scope: takeTestMultiChoiceCtrl.IScope,
        takeTestCtrlSvc: ITakeTestCtrlSvc
        ) {

        var questionModel: IQuestionWithActualModel;
        var actualQuestion: IMCQuestionModel;
        var choices: IMultiChoiceAnswer[];
        var isMultiAnswer: boolean;
        var selectedIndex: takeTestMultiChoiceCtrl.ISelectedIndex = { value: -1 };

        var submitBtn: IButtonElement ={
            disabled: true,
            isReady: true,
            click: function () {
                if (submitBtn.disabled)
                    return;

                submitBtn.isReady = false;
                submitBtn.disabled = true;

                var numCorrectAnswers: number = 0;
                var numWrongAnswers: number = 0;
                var totalNumAnswers: number = 0;
                var isCorrect: boolean = false;
                var points: number = 0;

                questionModel.isSubmitted = true;
                if (isMultiAnswer == false) {
                    choices.forEach(function (item: IMultiChoiceAnswer) {
                        item.isSelected = false;
                    });

                    var answerIdx: number = selectedIndex.value;
                    if (choices[answerIdx].isAnswer == true) {
                        choices[answerIdx].isCorrect = true;
                        isCorrect = true;
                        points = 1;
                    }
                    choices[answerIdx].isSelected = true;

                    takeTestCtrlSvc.submitAnswer(choices, isCorrect, points, 1);
                } else {

                    choices.forEach(function (choice: IMultiChoiceAnswer) {
                        choice.isCorrect = false; // reset

                        if (choice.isAnswer == true) {
                            totalNumAnswers++;
                            if (choice.isSelected == true) {
                                numCorrectAnswers++;
                                choice.isCorrect = true;
                            }
                            else {
                                numWrongAnswers++;
                            }
                        } else {
                            if (choice.isSelected == true)
                                numWrongAnswers++;
                        }
                    });

                    isCorrect = numWrongAnswers == 0 && numCorrectAnswers == totalNumAnswers;
                    var totalPoints: number;
                    if (actualQuestion.isMultiplePoints == true) {
                        points = numCorrectAnswers - numWrongAnswers;
                        totalPoints = totalNumAnswers;
                    } else {
                        points = numCorrectAnswers == totalNumAnswers ? 1 : 0;
                        totalPoints = 1;
                    }

                    takeTestCtrlSvc.submitAnswer(choices, isCorrect, points, totalPoints);
                }

                takeTestCtrlSvc.goToNextQuestion();
            }
        }
        var toggleMarkBtn: IButtonElement = {
            click: function () {
                takeTestCtrlSvc.toggleMarkOfCurrentQuestion();
                updateMarkBtnName();
            }
        }

        function init() {
            var numCorrectAnswers: number = 0;
            questionModel = takeTestCtrlSvc.getCurrentQuestion();
            actualQuestion = questionModel.actualQuestion;

            choices = new Array<IMultiChoiceAnswer>();

            if (questionModel.isSubmitted) {
                var choicesTmp: IMultiChoiceAnswer[] = takeTestCtrlSvc.getCurrentActualAnswers();
                for (var i = 0; i < choicesTmp.length; i++) {
                    if (choicesTmp[i].isAnswer == true)
                        numCorrectAnswers++;
                    choices.push({
                        value: choicesTmp[i].value,
                        isCorrect: false,
                        isSelected: choicesTmp[i].isSelected,
                        isAnswer: choicesTmp[i].isAnswer
                    });
                    if (choicesTmp[i].isSelected)
                        selectedIndex.value = i;
                }
                    
            } else {
                if (actualQuestion.shuffleChoices == true) {
                    var mcChoicesCopy: IMCChoiceModel[] = new Array<IMCChoiceModel>();
                    for (i = 0; i < actualQuestion.choices.length; i++) {
                        mcChoicesCopy.push(actualQuestion.choices[i]);
                    }

                    var availChoices: number = actualQuestion.choices.length;
                    while (availChoices > 0) {
                        var idx: number = Math.floor(Math.random() * availChoices);
                        var mcChoice: IMCChoiceModel = mcChoicesCopy[idx];
                        if (mcChoice.isAnswer == true)
                            numCorrectAnswers++;
                        choices.push({
                            value: mcChoice.value,
                            isCorrect: false,
                            isSelected: false,
                            isAnswer: mcChoice.isAnswer
                        });
                        mcChoicesCopy.splice(idx, 1);
                        availChoices--;
                    }
                } else {
                    for (var i = 0; i < actualQuestion.choices.length; i++) {
                        if (actualQuestion.choices[i].isAnswer == true)
                            numCorrectAnswers++;
                        choices.push({
                            value: actualQuestion.choices[i].value,
                            isCorrect: false,
                            isSelected: false,
                            isAnswer: actualQuestion.choices[i].isAnswer
                        });
                    }
                }
            }

            
            isMultiAnswer = numCorrectAnswers > 1;

            attachEnterPress();
            updateMarkBtnName();
        }

        var numChecked: number = 0;
        function changed(item: IMultiChoiceAnswer) {
           if(isMultiAnswer) {
               if (item.isSelected == true)
                   numChecked++;
               else
                   numChecked--;
               submitBtn.disabled = numChecked == 0 ? true : false;
            } else {
               submitBtn.disabled = false;
            }
        }

        function updateMarkBtnName() {
            toggleMarkBtn.name = questionModel.isMarked ? "marked" : "mark";
        }

        init();

        $scope.questionModel = questionModel;
        $scope.question = actualQuestion;
        $scope.choices = choices;
        $scope.isMultiAnswer = isMultiAnswer;
        $scope.selectedIndex = selectedIndex;
        $scope.changed = changed;
        $scope.submitBtn = submitBtn;
        $scope.toggleMarkBtn = toggleMarkBtn;
    }
})();