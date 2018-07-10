module takeTestMultiChoiceSameCtrl {
    export interface IScope {
        questionModel: IQuestionWithActualModel;
        question: IMultiChoiceSameQuestionModel;
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

    export interface IChoice {
        id: number;
        value: string;
        isSelected: boolean;
        isCorrect: boolean;
    }

    export interface ISelectedIndex {
        value: number;
    }
}

(function () {
    l2lControllers.controller("takeTestMultiChoiceSameCtrl", takeTestMultiChoiceSameCtrl);

    takeTestMultiChoiceSameCtrl.$inject = ["$scope", "takeTestCtrlSvc"];
    function takeTestMultiChoiceSameCtrl(
        $scope: takeTestMultiChoiceSameCtrl.IScope,
        takeTestCtrlSvc: ITakeTestCtrlSvc
    ) {

        var test: ITakeTestModel;
        var questionModel: IQuestionWithActualModel;
        var actualQuestion: IMultiChoiceSameQuestionModel;
        var choices: IMultiChoiceAnswer[];
        var isMultiAnswer: boolean;
        var selectedIndex: takeTestMultiChoiceCtrl.ISelectedIndex = { value: -1 };
        var choiceGroup: IMultiChoiceSameChoiceGroupModel;
        var answersIdx: number[] = new Array<number>();

        var submitBtn: IButtonElement = {
            disabled: true,
            isReady: true,
            click: function () {
                if (submitBtn.disabled)
                    return;

                var numCorrectAnswers: number = 0;
                var numWrongAnswers: number = 0;
                var totalNumAnswers: number = 0;
                var isCorrect: boolean = false;
                var points: number = 0;

                questionModel.isSubmitted = true;
                if (isMultiAnswer == false) {
                    
                    // reset
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
                    if (choiceGroup.isMultiplePoints == true) {
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
            function getChoiceGroup() {
                for (var i = 0; i < test.multiChoiceSameChoiceGroups.length; i++) {
                    if (test.multiChoiceSameChoiceGroups[i].id == actualQuestion.choiceGroupId) {
                        choiceGroup = test.multiChoiceSameChoiceGroups[i];
                        break;
                    }
                }
            }

            function getAnswersIdx() {
                var answersInStr = actualQuestion.answers.split(",");
                for (var i = 0; i < answersInStr.length - 1; i++) {
                    var answerIdx: number = parseInt(answersInStr[i]);
                    answersIdx.push(answerIdx);
                }

                isMultiAnswer = answersIdx.length > 1;
            }

            function getChoices() {
                choices = new Array<IMultiChoiceAnswer>();
                var choicesCopy: IMultiChoiceAnswer[] = new Array<IMultiChoiceAnswer>();
                for (var i = 0; i < choiceGroup.choices.length; i++) {
                    choicesCopy.push({
                        id: choiceGroup.choices[i].id,
                        value: choiceGroup.choices[i].value,
                        isSelected: false,
                        isCorrect: false,
                        isAnswer: false
                    });
                }

                for (var i = 0; i < answersIdx.length; i++) {
                    for (var j = 0; j < choicesCopy.length; j++) {
                        if (choicesCopy[j].id == answersIdx[i]) {
                            choicesCopy[j].isAnswer = true;
                        }
                    }
                }

                if (choiceGroup.shuffleChoices == true) {
                    var availChoices: number = choicesCopy.length;
                    while (availChoices > 0) {
                        var idx: number = Math.floor(Math.random() * availChoices);
                        var choice: IMultiChoiceAnswer = choicesCopy[idx];

                        choices.push(choice);
                        choicesCopy.splice(idx, 1);
                        availChoices--;
                    }

                } else {
                    choices = choicesCopy;
                }

                attachEnterPress();
                updateMarkBtnName();
            }

            function createCopyOfChoices() {
                var choicesTmp: IMultiChoiceAnswer[] = takeTestCtrlSvc.getCurrentActualAnswers();
                choices = new Array<IMultiChoiceAnswer>();

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

                updateMarkBtnName();
                attachEnterPress();
            }

            // init Start
            var numCorrectAnswers: number = 0;
            test = takeTestCtrlSvc.getTest();
            questionModel = takeTestCtrlSvc.getCurrentQuestion();
            actualQuestion = questionModel.actualQuestion;

            if (questionModel.isSubmitted) {
                getAnswersIdx();
                createCopyOfChoices();
            } else {
                getAnswersIdx();
                getChoiceGroup();
                getChoices();
            }
        }

        var numChecked: number = 0;
        function changed(item: IMultiChoiceAnswer) {
            if (isMultiAnswer) {
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