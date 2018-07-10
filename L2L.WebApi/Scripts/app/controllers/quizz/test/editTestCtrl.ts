/// <reference path="../../../../typings/angular-ui-bootstrap/angular-ui-bootstrap.d.ts" />
module editTestCtrl {
    export interface IScope {
        page: IPage;
        message: string;
        quizz: IQuizzModel;
        test: ITestModel;
        user: IUserModel;

        createNewQuestion: (qType: string, param: any) => void;
        editQuestion: (question: IQuestionModel) => void;
        deleteQuestion: (idx: number, question: IQuestionModel) => void;
        toggleIncludeInFlashCards: (question: IQuestionModel) => void;
        editTestSetting: () => void;

        editMultiChoiceSameChoiceGroup: (id: number) => void;
        newMultiChoiceQuestion: (choiceGroup: IMultiChoiceSameChoiceGroupModel) => void;

        flashBtn: IButtonElement;
        liveBtn: IButtonElement;

        toolTipString: (item: IQuestionModel) => string;

        editQuizz(): void;
        deleteQuizz(): void;

        goBack: () => void;

        testPermission: IPermission;
    }

    export interface IStateParams {
        testId: number;
        quizzId: number;
    }
}

(function () {
    l2lControllers.controller('editTestCtrl', editTestCtrl);

    editTestCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$uibModal", "testSvc", "questionSvc", "dialogSvc", "questionTypeSvc", "modalSvc", "quizzSvc", "loginModalSvc"];
    function editTestCtrl(
        $scope: editTestCtrl.IScope,
        $stateParams: editTestCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $uibModal: ng.ui.bootstrap.IModalService,
        testSvc: ITestSvc,
        questionSvc: IQuestionSvc,
        dialogSvc: IDialogSvc,
        questionTypeSvc: IQuestionTypeSvc,
        modalSvc: IModalSvc,
        quizzSvc: IQuizzSvc,
        loginModalSvc: ILoginModalSvc
    ) {

        var page: IPage = {
            isReady: false,
            numResourceToWait: 3,
            disabled: false
        }
        var test: ITestModel;
        var quizz: IQuizzModel;
        var quizzOverview: IQuizzOverviewModel;
        var user: IUserModel = currentUser.getUserData();
        var testPermission: IPermission = {
            canCreate: true
        };
        var user: IUserModel = currentUser.getUserData();
        var flashBtn: IButtonElement = {
            click: function (item: IQuestionModel) {
                function successIsFlashCard() {
                    item.isFlashCard = !item.isFlashCard;
                    item.isReady = true;
                }

                function errorIsFlashCard() {
                    item.isReady = true;
                }

                item.isReady = false;

                if (item.isFlashCard)
                    questionSvc.excludeInFlashCards(item.id, successIsFlashCard, errorIsFlashCard);
                else
                    questionSvc.includeInFlashCards(item.id, successIsFlashCard, errorIsFlashCard);
            }
        }

        var liveBtn: IButtonElement = {
            isReady: true,
            click: function () {

                if (util.showLoginIfGuest(user, loginModalSvc))
                    return;

                var onClass: string = "is-live";
                var tmpValue: boolean = !quizz.isLive;
                liveBtn.isReady = false;
                quizzSvc.setLive({ id: quizz.id, value: tmpValue },
                    function () {
                        quizz.isLive = tmpValue;
                        cachedDataSvc.deleteQuizzById(quizz.id);
                        updateLiveBtn();
                        liveBtn.isReady = true;
                    },
                    function () {
                        liveBtn.isReady = true;
                        notificationSvc.error(str.errorSave);
                    });
            }
        }

        function errorSave(response) {
            notificationSvc.error(str.errorSave);
        }

        function init() {
            function getTestPermission() {
                if (user.id != quizz.ownerId) {
                    testPermission.canCreate = false;
                }
            }

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait == 0;
            }

            function successTest(data: ITestModel) {
                $scope.test = test = data;
                updateIsReady();

                updateLiveBtn();
                test.questions.forEach(function (item: IQuestionModel) {
                    item.isReady = true;
                });
            }

            function successQuizzOvierview(data: IQuizzOverviewModel) {
                quizzOverview = data;
                updateIsReady();

                getTestPermission();

                test = cachedDataSvc.getTestById($stateParams.testId, successTest, errorLoad);
            }

            function successQuizz(data: IQuizzModel) {
                $scope.quizz = quizz = data;
                updateIsReady();

                quizzOverview = cachedDataSvc.getQuizzOverviewById($stateParams.quizzId, successQuizzOvierview, errorLoad);
            }

            quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, successQuizz, errorLoad);
        }

        function updateLiveBtn() {
            if (test.questions.length < 10) {
                liveBtn.tooltip = "There should at least be 10 questions";
                return;
            }

            liveBtn.tooltip = quizz.isLive ? "Click to hide from other people" : "Click to make other people see this Quizz";
        }

        function createNewQuestion(qType: string, param: any): void {
            questionTypeSvc.goToNewQuestion(qType, param);
        }

        function editQuestion(question: IQuestionModel): void {
            questionTypeSvc.goToEditQuestion(question, $stateParams.quizzId);
        }

        function deleteQuestion(idx: number, question: IQuestionModel): void {
            dialogSvc.confirm(str.confirmDeleteQuestion, function (result) {
                if (result == true) {
                    questionSvc.deleteQuestion(question.id, function (data) {
                        test.questions.splice(idx, 1);
                        quizzOverview.numQuestions = test.questions.length;
                        notificationSvc.success(str.deleteSuccess);
                    }, errorSave);
                }
            });
        }

        function editTestSetting(): void {
            var settings: ng.ui.bootstrap.IModalSettings = {
                templateUrl: "scripts/templates/quizz/test/testSetting/EditTestSetting.html",
                controller: "editTestSettingCtrl",
                resolve: {
                    modalSetting: function (): editTestSettingCtrl.IModalSetting {
                        return {
                            numAvailQuestions: 0,
                            saveToDb: true,
                            isBuiltIn: false
                        }
                    },
                    setting: function (): ITestSettingModel {
                        return test.defaultSetting;
                    }
                }
            }

            modalSvc.open(settings, function (newSetting: ITestSettingModel) {
                notificationSvc.success(str.updateSuccess);
            });
        }

        function editMultiChoiceSameChoiceGroup(id: number): void {
            $state.go("si.editMultiChoiceSameChoiceGroup", { cgId: id, testId: test.id, quizzId: quizz.id });
        }

        function newMultiChoiceQuestion(choiceGroup: IMultiChoiceSameChoiceGroupModel): void {
            $state.go("si.editMultiChoiceSameQuestion", { qId: -1, testId: test.id, quizzId: quizz.id });
        }

        function toolTipString(item: IQuestionModel): string {
            return item.isFlashCard ? "Click to remove from Flash Cards" : "Click to add in Flash Cards";
        }

        function editQuizz(): void {
            $state.go("si.editQuizz", { quizzId: quizz.id });
        }

        function deleteQuizz(): void {

        }

        function goBack(): void {
            history.back();
        }

        init();

        $scope.page = page;
        $scope.quizz = quizz;
        $scope.test = test;
        $scope.user = user;
        $scope.createNewQuestion = createNewQuestion;
        $scope.editQuestion = editQuestion;
        $scope.deleteQuestion = deleteQuestion;
        $scope.editTestSetting = editTestSetting;
        $scope.editMultiChoiceSameChoiceGroup = editMultiChoiceSameChoiceGroup;
        $scope.goBack = goBack;
        $scope.testPermission = testPermission;

        $scope.flashBtn = flashBtn;
        $scope.liveBtn = liveBtn;

        $scope.toolTipString = toolTipString;

        $scope.editQuizz = editQuizz;
        $scope.deleteQuizz = deleteQuizz;
    }
})();