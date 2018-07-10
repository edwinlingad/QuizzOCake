module editQandACtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage,
        quizz: IQuizzModel;
        question: IQAQuestionModel;
        addAnswerInput: () => void;
        deleteAnswerInput: (idx: number) => void;
        submit: (form: ng.IFormController) => void;
        goBack: () => void;
        changed: (model: ITrackModify) => void;
        showDelete: boolean;
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
    l2lControllers.controller('editQandACtrl', editQandACtrl);

    editQandACtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "qaQuestionSvc", "qaAnswerSvc", "questionSvc", "loginModalSvc", "$anchorScroll"];
    function editQandACtrl(
        $scope: editQandACtrl.IScope,
        $stateParams: editQandACtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        qaQuestionSvc: IQAQuestionSvc,
        qaAnswerSvc: IQaAnswerSvc,
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
        var user: IUserModel = currentUser.getUserData();
        var quizz: IQuizzModel = {};
        var quizzOverview: IQuizzOverviewModel;
        var isEditMode: boolean;
        var question: IQuestionModel = {};
        var actualQuestion: IQAQuestionModel = {
            answers: new Array<IQAAnswerModel>()
        };
        var deletedAnswers: IQAAnswerModel[] = Array<IQAAnswerModel>();
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

            function successActualQuestion(data: IQAQuestionModel) {
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
                        answersInOrder: false,
                        answers: new Array<IQAAnswerModel>({ answer: "" }),

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
                    actualQuestion = cachedDataSvc.getQandAQuestionById(question.questionId, successActualQuestion, errorLoad);
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

        function addAnswerInput(): void {
            var answer: IQAAnswerModel = { id: 0 };
            actualQuestion.answers.push(answer);
            page.isModified = true;
        }

        function deleteAnswerInput(idx: number): void {
            var answer = actualQuestion.answers[idx];
            if (answer.id != 0)
                deletedAnswers.push(answer);
            actualQuestion.answers.splice(idx, 1);
            page.isModified = true;
        }

        function submit(form: ng.IFormController): void {
            function errorSave(response) {
                util.enableForm(page);
                notificationSvc.error(str.errorSave);
            }

            function createNewQuestion() {
                var model: ICreateQuestionModel = {
                    questionType: QuestionTypeEnum.QandA,
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
                    deletedAnswers.forEach(function (answer) {
                        numItemsToWait++;
                        qaAnswerSvc.deleteQaAnswer(answer.id, success, errorSave);
                    });
                }

                function addNewItems() {
                    var newList: IQAAnswerModel[] = new Array<IQAAnswerModel>();
                    for (var i = actualQuestion.answers.length - 1; i >= 0; i--) {
                        var answer: IQAAnswerModel;
                        answer = actualQuestion.answers[i];
                        if (answer.id == 0) {
                            actualQuestion.answers.splice(i, 1);
                            newList.push(answer);
                        }
                    }

                    newList.forEach(function (answer: IQAAnswerModel) {
                        numItemsToWait++;
                        answer.questionId = actualQuestion.id;
                        qaAnswerSvc.createNewQaAnswer(answer,
                            function (data) {
                                actualQuestion.answers.push(data);
                                success();
                            },
                            errorSave);
                    });
                }

                function updateItems() {
                    actualQuestion.answers.forEach(function (answer) {
                        if (answer.isModified == true) {
                            numItemsToWait++;
                            qaAnswerSvc.updateQaAnswer(answer, success, errorSave);
                        }
                    });
                }

                function updateQuestion() {
                    numItemsToWait++;
                    notifyReady = true;

                    if (actualQuestion.isModified == true || editorControl.isModified === true) {
                        qaQuestionSvc.updateQuestion(actualQuestion, function (data) {
                            question.question = actualQuestion.textContent;
                            success();
                        }, errorSave);
                    } else {
                        success();
                    }
                }

                removeDeletedItems();
                addNewItems();
                updateItems();
                updateQuestion();
            }

            if (user.isGuest) {
                loginModalSvc.openLoginModal(false, true);
                return;
            }

            if (form.$valid) {
                util.disableForm(page);
                editorControl.callSave();
                util.editorLoad(actualQuestion, editorControl);

                if (isEditMode)
                    saveEditQuestion();
                else
                    createNewQuestion();
            }
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
        $scope.addAnswerInput = addAnswerInput;
        $scope.deleteAnswerInput = deleteAnswerInput;
        $scope.submit = submit;
        $scope.goBack = goBack;
        $scope.changed = changed
        $scope.showDelete = false;
        $scope.saveButton = saveButton;

        $scope.tinymceOptions = util.getTinymceOptions();
        $scope.editorControl = editorControl;

        $scope.canSave = canSave;
    }
})();