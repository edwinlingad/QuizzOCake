module editMultiChoiceSameQuestionCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        quizz: IQuizzModel;
        question: IMultiChoiceSameQuestionModel;
        choices: IChoice[];
        submit: (form: ng.IFormController) => void;
        goBack: () => void;
        changed: (model: ITrackModify) => void;
        saveButton: IElement;

        tinymceOptions: any;

        editorControl: IEditorControl;
        canSave(): void;
    }

    export interface IStateParams {
        qId: number;
        testId: number;
        quizzId: number;
        cgId: number;
    }

    export interface IChoice {
        id: number;
        value: string;
        isSelected: boolean;
    }
}

(function () {
    l2lControllers.controller("editMultiChoiceSameQuestionCtrl", editMultiChoiceSameQuestionCtrl);

    editMultiChoiceSameQuestionCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "multiChoiceSameQuestionSvc", "questionSvc", "dialogSvc", "loginModalSvc", "$anchorScroll"];
    function editMultiChoiceSameQuestionCtrl(
        $scope: editMultiChoiceSameQuestionCtrl.IScope,
        $stateParams: editMultiChoiceSameQuestionCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        multiChoiceSameQuestionSvc: IMultiChoiceSameQuestionSvc,
        questionSvc: IQuestionSvc,
        dialogSvc: IDialogSvc,
        loginModalSvc: ILoginModalSvc,
        $anchorScroll: ng.IAnchorScrollService
    ) {

        var page: IPage = {
            isReady: false,
            numResourceToWait: 4,
            disabled: false
        }
        var user: IUserModel = currentUser.getUserData();
        var test: ITestModel;
        var quizz: IQuizzModel;
        var quizzOverview: IQuizzOverviewModel;
        var isEditMode: boolean;
        var question: IQuestionModel;
        var actualQuestion: IMultiChoiceSameQuestionModel = { choiceGroupId: $stateParams.cgId };
        var choices: editMultiChoiceSameQuestionCtrl.IChoice[] = new Array<editMultiChoiceSameQuestionCtrl.IChoice>();
        var choiceGroup: IMultiChoiceSameChoiceGroupModel;
        var saveButton: IElement = {
            click: submit
        }
        var editorControl: IEditorControl = {
            title: "",
            textContent: "",
            addContentType: 0,
            imageUrl: "",
            imageContent: "",
            drawingContent: undefined
        };

        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function getQuestion(id: number): IQuestionModel {
                for (var i = 0; i < test.questions.length; i++) {
                    if (test.questions[i].id == id) {
                        return test.questions[i];
                    }
                }
            }

            function mapChoices() {
                choiceGroup.choices.forEach(function (item) {
                    var choice: editMultiChoiceSameQuestionCtrl.IChoice = {
                        id: item.id,
                        value: item.value,
                        isSelected: false
                    }

                    choices.push(choice);
                });

                if (isEditMode == true) {
                    var list = actualQuestion.answers.split(",");
                    list.forEach(function (item) {
                        var id: number = parseInt(item);
                        for (var i = 0; i < choices.length; i++) {
                            if (choices[i].id == id)
                                choices[i].isSelected = true;
                        }
                    });
                }
            }

            function successMultiChoiceSameQuestion(data: IMultiChoiceSameQuestionModel) {
                $scope.question = actualQuestion = data;
                updateIsReady();

                util.editorLoad(editorControl, data);

                var cgId = actualQuestion.choiceGroupId;
                for (var i = 0; i < test.multiChoiceSameChoiceGroups.length; i++) {
                    if (test.multiChoiceSameChoiceGroups[i].id == cgId) {
                        choiceGroup = test.multiChoiceSameChoiceGroups[i];
                        break;
                    }
                }

                page.title = page.title + choiceGroup.name + " Question";

                mapChoices();
            }

            function successTest(data: ITestModel) {
                test = data;
                updateIsReady();

                if ($stateParams.qId == undefined || $stateParams.qId == -1) {
                    isEditMode = false;
                    page.title = "Create New ";
                    saveButton.name = "Create";
                    var tmpActualQuestion: IMultiChoiceSameQuestionModel = {
                        id: 0,
                        question: "",
                        answers: "",
                        isMultiplePoints: false,

                        testId: $stateParams.testId,
                        choiceGroupId: $stateParams.cgId,

                        title: "",
                        textContent: "",
                        addContentType: 0,
                        imageUrl: "",
                        imageContent: "",
                        drawingContent: undefined
                    };

                    successMultiChoiceSameQuestion(tmpActualQuestion);
                } else {
                    isEditMode = true;
                    page.title = "Edit ";
                    saveButton.name = "Save";
                    question = getQuestion($stateParams.qId);
                    actualQuestion = cachedDataSvc.getMultiChoiceSameQuestionById(question.questionId, successMultiChoiceSameQuestion, errorLoad);
                }
            }

            function successQuizzOvierview(data: IQuizzOverviewModel) {
                quizzOverview = data;
                updateIsReady();

                test = cachedDataSvc.getTestById($stateParams.testId, successTest, errorLoad);
            }

            function successQuizz(data: IQuizzModel) {
                $scope.quizz = quizz = data;
                updateIsReady();

                quizzOverview = cachedDataSvc.getQuizzOverviewById($stateParams.quizzId, successQuizzOvierview, errorLoad);
            }

            $anchorScroll("top");
            quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, successQuizz, errorLoad);
        }

        function submit(form: ng.IFormController): void {

            function checkValid(): boolean {
                for (var i = 0; i < choices.length; i++) {
                    if (choices[i].isSelected == true)
                        return true;
                }
                return false;
            }

            function errorSave(response) {
                util.enableForm(page);
                notificationSvc.error(str.errorSave);
            }

            function createNewQuestion() {
                actualQuestion.answers = getAnswersString();
                actualQuestion.choiceGroupId = choiceGroup.id;
                actualQuestion.testId = test.id;
                var model: ICreateQuestionModel = {
                    questionType: QuestionTypeEnum.MultiChoiceSame,
                    question: actualQuestion
                }
                questionSvc.createQuestion(model,
                    function (data) {
                        util.enableForm(page);
                        test.questions.push(data);
                        quizzOverview.numQuestions = test.questions.length;
                        goBack();
                    }, errorSave);
            }

            function saveEditQuestion() {
                if (actualQuestion.isModified == true || editorControl.isModified === true) {
                    actualQuestion.answers = getAnswersString();
                    multiChoiceSameQuestionSvc.updateQuestion(actualQuestion,
                        function (data) {
                            util.enableForm(page);
                            question.question = actualQuestion.textContent;
                            goBack();
                        }, errorSave);
                } else {
                    goBack();
                }
            }

            if (user.isGuest) {
                loginModalSvc.openLoginModal(false, true);
                return;
            }

            if (form.$valid) {
                util.disableForm(page);
                editorControl.callSave();
                util.editorLoad(actualQuestion, editorControl);

                if (checkValid() == false) {
                    util.enableForm(page);
                    dialogSvc.alert(str.multiChoiceAtleastOneItemChoice);
                    return;
                }

                if (isEditMode)
                    saveEditQuestion();
                else
                    createNewQuestion();
            }
        }

        function getAnswersString(): string {
            var answerStrList: string = "";
            choices.forEach(function (choice) {
                if (choice.isSelected == true) {
                    answerStrList = answerStrList + choice.id.toString() + ",";
                }
            });

            return answerStrList;
        }

        function goBack() {
            $state.go("si.quizzDetail", { quizzId: quizz.id, view: 3 });
        }

        function changed(model: ITrackModify): void {
            model.isModified = true;
        }

        function canSave() {
            if (editorControl.textContent === undefined)
                return false;
            if (editorControl.textContent.trim().length === 0)
                return false;

            if (page.isModified || editorControl.isModified)
                return true;

            return false;
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.quizz = quizz;
        $scope.question = actualQuestion;
        $scope.choices = choices;
        $scope.submit = submit;
        $scope.goBack = goBack;
        $scope.changed = changed;
        $scope.saveButton = saveButton;

        $scope.tinymceOptions = util.getTinymceOptions();
        $scope.editorControl = editorControl;

        $scope.canSave = canSave;
    }
})();