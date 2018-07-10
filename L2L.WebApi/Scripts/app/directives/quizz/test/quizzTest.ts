module quizzTest {
    export interface IScope {
        quizz: IQuizzModel;
        quizzOverview: IQuizzOverviewModel;
        assId: any;

        page: IPage;
        $on: any;
        test: ITestModel;
        user: IUserModel;

        editTestSetting(): void;

        testPermission: IPermission;

        flashCardControl: any;
        questionControl: any;
        takeTestControl: any;
    }
}

(function () {
    l2lApp.directive("quizzTest", quizzTest);

    function quizzTest() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "questionTypeSvc", "questionSvc", "modalSvc", "layoutSvc", "takeTestCtrlSvc", "builtInQuestionsSvc"];

        function controller(
            $scope: quizzTest.IScope,
            $state: ng.ui.IStateService,
            cachedDataSvc: ICachedDataSvc,
            notificationSvc: INotificationSvc,
            currentUser: ICurrentUser,
            $anchorScroll: ng.IAnchorScrollService,
            $location: ng.ILocationService,
            resourceSvc: IResourceSvc,
            $timeout: ng.ITimeoutService,
            dialogSvc: IDialogSvc,
            questionTypeSvc: IQuestionTypeSvc,
            questionSvc: IQuestionSvc,
            modalSvc: IModalSvc,
            layoutSvc: ILayoutSvc,
            takeTestCtrlSvc: ITakeTestCtrlSvc,
            builtInQuestionsSvc: IBuiltInQuestionsSvc
        ) {
            var page: IPage = {
                isReady: false,
                numResourceToWait: 1,
                disabled: false
            };
            var quizz: IQuizzModel = $scope.quizz;
            var quizzOverview: IQuizzOverviewModel = $scope.quizzOverview;
            var test: ITestModel;
            var user: IUserModel = currentUser.getUserData();

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function errorSave(response) {
                notificationSvc.error(str.errorSave);
            }

            function init() {
                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait <= 0;
                }

                function successTest(data: ITestModel) {
                    $scope.test = test = data;
                    updateIsReady();

                    test.questions.forEach(function (item: IQuestionModel) {
                        item.isReady = true;
                    });
                }

                $scope.$on('$destroy', destroy);

                test = cachedDataSvc.getTestById(quizz.testId, successTest, errorLoad);
            }

            function destroy() {
            }

            var flashCardControl = (function () {
                function toggle(item: IQuestionModel) {
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

                function toolTipString(item: IQuestionModel): string {
                    return item.isFlashCard ? "Click to remove from Flash Cards" : "Click to add in Flash Cards";
                }

                return {
                    toggle: toggle,
                    toolTipString: toolTipString
                }
            })();

            var questionControl = (function () {
                function createNewQuestion(qType: string, param: any): void {
                    questionTypeSvc.goToNewQuestion(qType, param);
                    quizzOverview.numQuestions = test.questions.length;
                }

                function editQuestion(question: IQuestionModel): void {
                    questionTypeSvc.goToEditQuestion(question, quizz.id);
                }

                function deleteQuestion(idx: number, question: IQuestionModel): void {
                    dialogSvc.confirm(str.confirmDeleteQuestion, function (result) {
                        if (result == true) {
                            questionSvc.deleteQuestion(question.id, function (data) {
                                test.questions.splice(idx, 1);
                                quizzOverview.numQuestions = test.questions.length;
                                if (quizzOverview.updateQuestions !== undefined)
                                    quizzOverview.updateQuestions();

                                notificationSvc.success(str.deleteSuccess);
                            }, errorSave);
                        }
                    });
                }

                function editMultiChoiceSameChoiceGroup(id: number): void {
                    $state.go("si.editMultiChoiceSameChoiceGroup", { cgId: id, testId: test.id, quizzId: quizz.id });
                }

                function newMultiChoiceQuestion(choiceGroup: IMultiChoiceSameChoiceGroupModel): void {
                    $state.go("si.editMultiChoiceSameQuestion", { qId: -1, testId: test.id, quizzId: quizz.id });
                }

                return {
                    createNewQuestion: createNewQuestion,
                    editQuestion: editQuestion,
                    deleteQuestion: deleteQuestion,
                    editMultiChoiceSameChoiceGroup,
                    newMultiChoiceQuestion
                }
            })();

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

            var takeTestControl = (function () {
                var vars = {
                    isReady: false,
                    isAssignment: false
                };
                var takeTestBtn: IItemToWait = {
                    isReady: true
                };
                var takeTestPreviewBtn: IItemToWait = {
                    isReady: true
                };
                var isPreview: boolean = false;
                var isDailyReward: boolean = false;
                var isAssignment: boolean = false;
                var asssignmentId: number = 0;
                var assignment: IAssignmentModel = undefined;
                var testModel: ITakeTestModel;
                var testSettingsCopy: ITestSettingModel = {};

                function errorStartTest() {
                    vars.isReady = true;
                    takeTestBtn.isReady = true;
                    takeTestPreviewBtn.isReady = true;
                    notificationSvc.error(str.errorLoad);
                }

                function startTest(data: ITakeTestModel) {
                    vars.isReady = true;
                    takeTestBtn.isReady = true;
                    takeTestPreviewBtn.isReady = true;

                    takeTestCtrlSvc.initWithModel(!isPreview, testModel, quizz.id, assignment);
                    takeTestCtrlSvc.setTestSettings(testSettingsCopy);
                    takeTestCtrlSvc.startTest();
                    takeTestCtrlSvc.setDailyRewardValue(isDailyReward, quizzOverview.dailyReward.availablePoints);
                }

                function initAndStartTest() {

                    vars.isReady = false;
                    if (quizz.isBuiltIn) {
                        testModel = builtInQuestionsSvc.getTest(quizz.id, testSettingsCopy.numberOfQuestions, startTest, errorStartTest);
                    } else {
                        testModel = cachedDataSvc.getTakeTestModelById(quizz.testId, startTest, errorStartTest);
                    }
                }

                function init() {

                    function successTest(data: ITakeTestModel) {
                        testModel = data;
                        vars.isReady = true;

                        if (!isAssignment) {
                            util.copy(testSettingsCopy, data.defaultSetting);

                            if (!quizz.isBuiltIn) {
                                if (testSettingsCopy.numberOfQuestions != 0) {
                                    if (testSettingsCopy.numberOfQuestions > data.questions.length)
                                        testSettingsCopy.numberOfQuestions = 0;
                                }
                            }
                        } else {
                            util.copy(testSettingsCopy, assignment.assignmentGroup.testSetting);
                        }
                    }

                    function successAssignment(data: IAssignmentModel) {
                        assignment = data;

                        if (quizz.isBuiltIn)
                            testModel = builtInQuestionsSvc.getTest(quizz.id, 1, successTest, errorLoad);
                        else
                            testModel = cachedDataSvc.getTakeTestModelById(quizz.testId, successTest, errorLoad);
                    }

                    function initAssignment() {
                        asssignmentId = util.getNumber($scope.assId);
                        if (asssignmentId !== -1) {
                            isAssignment = true;
                            vars.isAssignment = true;
                            assignment = resourceSvc.getResource(enums.ResourceTypeEnum.Assignment, asssignmentId, successAssignment, errorLoad);
                        }
                        else
                            successAssignment(undefined);
                    }

                    function initDailyReward() {
                        if (quizzOverview.dailyReward !== null && quizzOverview.dailyReward.isTaken == false)
                            isDailyReward = true;

                        initAssignment();
                    }

                    initDailyReward();
                }

                function takeTest() {
                    layoutSvc.overwriteOngoingTest(
                        function (result: boolean) {
                            if (result) {
                                vars.isReady = false;
                                takeTestBtn.isReady = false;
                                initAndStartTest();
                            }
                        });
                }

                function previewTest() {
                    layoutSvc.removeOngoingTest(
                        function (result: boolean) {
                            if (result) {
                                isPreview = true;
                                vars.isReady = false;
                                takeTestPreviewBtn.isReady= false;
                                initAndStartTest();
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
                                    numAvailQuestions: test.questions.length,
                                    saveToDb: false,
                                    isBuiltIn: quizz.isBuiltIn
                                }
                            },
                            setting: function (): ITestSettingModel {
                                return testSettingsCopy;
                            }
                        }
                    }

                    modalSvc.open(settings, function () {
                        if (testSettingsCopy.numberOfQuestions != test.defaultSetting.numberOfQuestions) {
                            if (test.defaultSetting.numberOfQuestions == 0) {
                                if (testSettingsCopy.numberOfQuestions < test.questions.length) {
                                    isDailyReward = false;
                                    return;
                                }
                            }

                            if (testSettingsCopy.numberOfQuestions != 0) {
                                if (testSettingsCopy.numberOfQuestions < test.defaultSetting.numberOfQuestions) {
                                    isDailyReward = false;
                                    return;
                                }
                            }
                        }

                        isDailyReward = true;
                    });
                }

                return {
                    vars: vars,
                    takeTestBtn: takeTestBtn,
                    takeTestPreviewBtn: takeTestPreviewBtn,
                    init: init,
                    takeTest: takeTest,
                    previewTest: previewTest,
                    settings: testSettingsCopy,
                    editTestSetting: editTestSetting
                }
            })();

            init();
            takeTestControl.init();

            $scope.page = page;
            $scope.flashCardControl = flashCardControl;
            $scope.questionControl = questionControl;
            $scope.editTestSetting = editTestSetting;
            $scope.takeTestControl = takeTestControl;
            $scope.user = user;
        }

        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/quizz/test/quizzTest.html",
            replace: true,
            scope: {
                quizz: "=",
                quizzOverview: "=",
                assId: "@?"
            },
            controller: controller
        }
    }
})();