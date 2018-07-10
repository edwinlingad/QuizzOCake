module editMultipleChoiceCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        quizz: IQuizzModel;
        question: IMCQuestionModel;
        addChoiceInput: () => void;
        deleteChoiceInput: (idx: number) => void;
        submit: (form: ng.IFormController) => void;
        goBack: () => void;
        changed: (model: ITrackModify) => void;
        saveButton: IElement;

        tinymceOptions: any;

        editorControl: IEditorControl;
        canSave(): void;
    }

    export interface IStateParams {
        qId: any;
        testId: any;
        quizzId: number;
    }
}

(function () {
    l2lControllers.controller('editMultipleChoiceCtrl', editMultipleChoiceCtrl);

    editMultipleChoiceCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "dialogSvc", "mcQuestionSvc", "mcChoiceSvc", "questionSvc", "loginModalSvc", "$anchorScroll"];
    function editMultipleChoiceCtrl(
        $scope: editMultipleChoiceCtrl.IScope,
        $stateParams: editMultipleChoiceCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        dialogSvc: IDialogSvc,
        mcQuestionSvc: IMCQuestionSvc,
        mcChoiceSvc: IMcChoiceSvc,
        questionSvc: IQuestionSvc,
        loginModalSvc: ILoginModalSvc,
        $anchorScroll: ng.IAnchorScrollService
    ) {

        var page: IPage = {
            isReady: false,
            numResourceToWait: 4,
            disabled: false,
            isModified: false
        };
        var test: ITestModel = {
            quizz: {},
            questions: new Array<IQuestionModel>(),
            multiChoiceSameChoiceGroups: new Array<IMultiChoiceSameChoiceGroupModel>(),
            defaultSetting: {}
        };
        var quizz: IQuizzModel = {};
        var quizzOverview: IQuizzOverviewModel;
        var user: IUserModel = currentUser.getUserData();
        var isEditMode: boolean;
        var question: IQuestionModel = {};
        var actualQuestion: IMCQuestionModel = {
            choices: new Array<IMCChoiceModel>()
        };
        var deletedChoices: IMCChoiceModel[] = Array<IMCChoiceModel>();
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

            function successActualQuestion(data: IMCQuestionModel) {
                $scope.question = actualQuestion = data;
                updateIsReady();

                util.editorLoad(editorControl, data);
            }

            function successTest(data: ITestModel) {
                test = data;
                updateIsReady();

                if ($stateParams.qId === undefined || $stateParams.qId === "" || $stateParams.qId === "-1") {
                    isEditMode = false;
                    page.title = "Create New ";
                    saveButton.name = "Create";
                    var tmpActualQuestion = {
                        testId: $stateParams.testId,
                        isMultiplePoints: false,
                        shuffleChoices: true,
                        choices: new Array<IMCChoiceModel>({ isAnswer: true }, { isAnswer: false }),

                        title: "",
                        textContent: "",
                        addContentType: 0,
                        imageUrl: "",
                        imageContent: "",
                        drawingContent: undefined
                    };

                    successActualQuestion(tmpActualQuestion);
                }
                else {
                    isEditMode = true;
                    page.title = "Edit ";
                    saveButton.name = "Save";
                    question = getQuestion($stateParams.qId);
                    $scope.question = actualQuestion = cachedDataSvc.getMultiChoiceQuestionById(question.questionId, successActualQuestion, errorLoad);
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

        function addChoiceInput(): void {
            var choice: IMCChoiceModel = { id: 0, isAnswer: false };
            actualQuestion.choices.push(choice);
            page.isModified = true;
        }

        function deleteChoiceInput(idx: number): void {
            var choice: IMCChoiceModel = actualQuestion.choices[idx];
            if (choice.id != 0)
                deletedChoices.push(choice);
            actualQuestion.choices.splice(idx, 1);
        }

        function submit(form: ng.IFormController): void {
            function errorSave(response) {
                util.enableForm(page);
                notificationSvc.error(str.errorSave);
            }

            function createNewQuestion() {
                var model: ICreateQuestionModel = {
                    questionType: QuestionTypeEnum.MultipleChoice,
                    question: $scope.question
                }
                questionSvc.createQuestion(model, function (data) {
                    util.enableForm(page);
                    test.questions.push(data);
                    quizzOverview.numQuestions = test.questions.length;
                    goBack();
                }, errorSave);
            }

            function saveEditQuestion() {

                var numItemsToWait: number = 0;
                var notifyReady: boolean = false;

                function success() {
                    numItemsToWait--;
                    if (notifyReady && numItemsToWait == 0) {
                        notificationSvc.success(str.updateSuccess);
                        goBack();
                    }
                }

                function removeDeletedItems() {
                    deletedChoices.forEach(function (choice) {
                        numItemsToWait++;
                        mcChoiceSvc.deleteMcChoice(choice.id, success, errorSave);
                    });
                }

                function AddNewItems() {
                    var newList: IMCChoiceModel[] = new Array<IMCChoiceModel>();
                    for (var i = actualQuestion.choices.length - 1; i >= 0; i--) {
                        var choice: IMCChoiceModel;
                        choice = actualQuestion.choices[i];
                        if (choice.id == 0) {
                            actualQuestion.choices.splice(i, 1);
                            newList.push(choice);
                        }
                    }

                    newList.forEach(function (choice: IMCChoiceModel) {
                        numItemsToWait++;
                        choice.questionId = actualQuestion.id;
                        mcChoiceSvc.createNewMcChoice(choice,
                            function (data) {
                                actualQuestion.choices.push(data);
                                success();
                            }, errorSave);
                    });
                }

                function updateItems() {
                    actualQuestion.choices.forEach(function (choice: IMCChoiceModel, idx: number) {
                        if (choice.isModified == true) {
                            numItemsToWait++;
                            mcChoiceSvc.updateMcChoice(choice, success, errorSave);
                        }
                    });
                }

                function updateChoiceGroup() {
                    numItemsToWait++;
                    notifyReady = true;
                    if (actualQuestion.isModified == true || editorControl.isModified === true) {
                        mcQuestionSvc.updateQuestion(actualQuestion,
                            function (data) {
                                question.question = actualQuestion.textContent;
                                success();
                            }, errorSave);
                    } else {
                        success();
                    }
                }

                removeDeletedItems();
                AddNewItems();
                updateItems();
                updateChoiceGroup();
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

        function checkValid(): boolean {
            for (var i = 0; i < actualQuestion.choices.length; i++) {
                if (actualQuestion.choices[i].isAnswer == true)
                    return true;
            }
            return false;
        }

        function goBack() {
            $state.go("si.quizzDetail", { quizzId: quizz.id, view: 3 });
        }

        function changed(model: ITrackModify): void {
            model.isModified = true;
            page.isModified = true;
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
        $scope.addChoiceInput = addChoiceInput;
        $scope.deleteChoiceInput = deleteChoiceInput;
        $scope.submit = submit;
        $scope.goBack = goBack;
        $scope.changed = changed;
        $scope.saveButton = saveButton;

        $scope.tinymceOptions = util.getTinymceOptions();
        $scope.editorControl = editorControl;

        $scope.canSave = canSave;
    }
})();