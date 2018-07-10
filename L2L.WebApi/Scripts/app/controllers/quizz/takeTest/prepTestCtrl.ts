module prepTestCtrl {
    export interface IScope {
        page: IPage;
        quizz: IQuizzModel;
        quizzOverview: IQuizzOverviewModel;
        test: ITakeTestModel;
        settings: ITestSettingModel;
        isTestSettingsOptionsShown: boolean;
        toggleModifySettings: () => void;
        editTestSetting: () => void;
        goBack: () => void;

        isPreview: boolean;
        isAssignment: boolean;
        isDailyReward: boolean;

        takeTestBtn: IButtonElement;
    }

    export interface IStateParams {
        testId: any;
        quizzId: number;
        isPrev: any;
        assId: any;
    }
}

(function () {
    l2lControllers.controller('prepTestCtrl', prepTestCtrl);

    prepTestCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "$anchorScroll", "resourceSvc", "dialogSvc", "testSvc", "takeTestCtrlSvc", "modalSvc", "builtInQuestionsSvc", "layoutSvc"];
    function prepTestCtrl(
        $scope: prepTestCtrl.IScope,
        $stateParams: prepTestCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        $anchorScroll: ng.IAnchorScrollService,
        resourceSvc: IResourceSvc,
        dialogSvc: IDialogSvc,

        testSvc: ITestSvc,
        takeTestCtrlSvc: ITakeTestCtrlSvc,        
        modalSvc: IModalSvc,
        builtInQuestionsSvc: IBuiltInQuestionsSvc,
        layoutSvc: ILayoutSvc
        
        ) {

        var page: IPage = {
            isReady: false,
            numResourceToWait: 3,
            disabled: false
        };
        var quizz: IQuizzModel = {};;
        var test: ITakeTestModel;
        var testSettingsCopy: ITestSettingModel = {};
        var quizzOverview: IQuizzOverviewModel = {
            dailyReward: {}
        };
        var assignment: IAssignmentModel = undefined;
        var takeTestBtn: IButtonElement = {
            isReady: true,
            click: function () {
                function startTest() {
                    takeTestCtrlSvc.initWithModel(!$scope.isPreview, test, $stateParams.quizzId, assignment);
                    takeTestCtrlSvc.setTestSettings(testSettingsCopy);
                    takeTestCtrlSvc.startTest();
                    takeTestCtrlSvc.setDailyRewardValue($scope.isDailyReward, quizzOverview.dailyReward.availablePoints);
                }

                function initAndStartTest() {
                    takeTestBtn.isReady = false;
                    if (quizz.isBuiltIn) {
                        test = builtInQuestionsSvc.getTest(quizz.id, testSettingsCopy.numberOfQuestions,
                            function (data: ITakeTestModel) {
                                $scope.test = test = data;
                                takeTestBtn.isReady = true;
                                startTest();
                            }, function error() {
                                takeTestBtn.isReady = true;
                                notificationSvc.error(str.errorLoad);
                            });
                    } else {
                        startTest();
                    }
                }

                if ($scope.isPreview) {
                    layoutSvc.removeOngoingTest(
                        function (result: boolean) {
                            if (result)
                                initAndStartTest();
                        });
                } else {
                    layoutSvc.overwriteOngoingTest(
                        function (result: boolean) {
                            if (result) 
                                initAndStartTest();
                        });
                }
            }
        };

        function error(response) {
            commonError($scope, response);
        }

        function init() {
            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successTest(data: ITakeTestModel) {
                test = data;
                updateIsReady();

                if (!$scope.isAssignment) {
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

            function successQuizzOverview(data: IQuizzOverviewModel) {
                $scope.quizzOverview = quizzOverview = data;
                updateIsReady();

                if (quizzOverview.dailyReward !== null && quizzOverview.dailyReward.isTaken == false)
                    $scope.isDailyReward = true;

                if (quizz.isBuiltIn)
                    test = builtInQuestionsSvc.getTest(quizz.id, 1, successTest, errorLoad);
                else
                    test = cachedDataSvc.getTakeTestModelById($stateParams.testId, successTest, errorLoad);
            }

            function successQuizz(data: IQuizzModel) {
                $scope.quizz = quizz = data;
                updateIsReady();
                
                quizzOverview = cachedDataSvc.getQuizzOverviewById($stateParams.quizzId, successQuizzOverview, errorLoad);
            }

            function successAssignment(data: IAssignmentModel) {
                assignment = data;
                updateIsReady();

                quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, successQuizz, errorLoad);
            }

            $anchorScroll("top");
            $scope.isPreview = util.isBoolEqual($stateParams.isPrev, true);
            var idx: number = util.getNumber($stateParams.assId);
            if (idx != -1) {
                page.numResourceToWait++;
                $scope.isAssignment = true;
                assignment = resourceSvc.getResource(enums.ResourceTypeEnum.Assignment, idx, successAssignment, errorLoad);
            }
            else {
                $scope.isAssignment = false;
                quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, successQuizz, errorLoad);
            }
        }

        function toggleModifySettings() {
            $scope.isTestSettingsOptionsShown = !$scope.isTestSettingsOptionsShown;
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
                            $scope.isDailyReward = false;
                            return;
                        }
                    }

                    if (testSettingsCopy.numberOfQuestions != 0) {
                        if (testSettingsCopy.numberOfQuestions < test.defaultSetting.numberOfQuestions) {
                            $scope.isDailyReward = false;
                            return;
                        }
                    }
                }

                $scope.isDailyReward = true;
            });
        }

        function goBack() {
            history.back();
        }

        init();

        $scope.page = page;
        $scope.settings = testSettingsCopy;
        $scope.quizz = quizz;
        $scope.quizzOverview = quizzOverview;
        $scope.isTestSettingsOptionsShown = false;
        $scope.toggleModifySettings = toggleModifySettings;
        $scope.editTestSetting = editTestSetting;
        $scope.goBack = goBack;

        $scope.takeTestBtn = takeTestBtn;
    }
})();