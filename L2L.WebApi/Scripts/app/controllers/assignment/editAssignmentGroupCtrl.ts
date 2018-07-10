module editAssignmentGroupCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage,
        quizz: IQuizzOverviewModel;
        assignmentGroup: IAssignmentGroupModel;
        dependents: IAssignmentInfo[];
        changed(item: ITrackModify): void;
        editTestSetting(): void;

        saveBtn: IButtonElement;
        goBack(): void;
        cancel(): void;

        dateOptions: any;
    }

    export interface IStateParams {
        assGId: any;
        quizzId: number;
    }
}

(function () {
    l2lControllers.controller('editAssignmentGroupCtrl', editAssignmentGroupCtrl);

    editAssignmentGroupCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "loginModalSvc", "modalSvc", "layoutSvc"];
    function editAssignmentGroupCtrl(
        $scope: editAssignmentGroupCtrl.IScope,
        $stateParams: editAssignmentGroupCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        resourceSvc: IResourceSvc,
        $timeout: ng.ITimeoutService,
        dialogSvc: IDialogSvc,

        loginModalSvc: ILoginModalSvc,
        modalSvc: IModalSvc,
        layoutSvc: ILayoutSvc
        ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 3,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var quizz: IQuizzOverviewModel = {};
        var test: ITestModel = {};
        var editMode: boolean;
        var assignmentGroup: IAssignmentGroupModel = {
            id: 0,
            message: "",
            targetScore: 100,
            isDefaultTestSetting: true,
            assignedById: user.id,
            assignments: new Array<IAssignmentInfo>()
        };
        var dependents: IAssignmentInfo[] = new Array<IAssignmentInfo>();
        var saveBtn: IButtonElement = {
            isReady: true,
            disabled: true,
            click: function () {
                function successSave() {
                    util.enableForm(page);
                    saveBtn.isReady = true;
                    goBack();
                }

                function successSaveNew(data: IAssignmentGroupModel) {
                    var layoutAssG: ILayoutAssignmentGroupModel = {
                        id: data.id,
                        quizzName: quizz.title,
                        category: quizz.category
                    }
                    //layoutSvc.addAssignmentGiven(layoutAssG);
                    layoutSvc.updateAssignmentsGiven();
                    successSave();
                }

                function errorSave() {
                    saveBtn.isReady = true;
                    util.enableForm(page);
                }

                function SaveEdit() {
                    var numItemsToWait: number = 0;
                    var notifyReady: boolean = false;

                    function success() {
                        numItemsToWait--;
                        if (notifyReady && numItemsToWait == 0) {
                            notificationSvc.success(str.updateSuccess);
                            successSave();
                            goBack();
                        }
                    }

                    function updateDependents() {
                        dependents.forEach(function (item: IAssignmentInfo) {
                            if (item.isModified) {
                                var matchedDependent: IAssignmentInfo = undefined;
                                assignmentGroup.assignments.forEach(function (dependent: IAssignmentInfo) {
                                    if (dependent.dependentId == item.dependentId)
                                        matchedDependent = dependent;
                                });

                                if (matchedDependent !== undefined) {
                                    if (item.isChosen == matchedDependent.isChosen)
                                        return;
                                    else {
                                        numItemsToWait++;
                                        resourceSvc.deleteResource(enums.ResourceTypeEnum.Assignment, matchedDependent.id, success, errorSave);
                                    }
                                } else {
                                    numItemsToWait++;
                                    var assignment: IAssignmentModel = {
                                        id: 0,
                                        isCompleted: false,
                                        dependentId: item.dependentId,
                                        assignmentGroupId: assignmentGroup.id
                                    }
                                    resourceSvc.createResource(enums.ResourceTypeEnum.Assignment, assignment, success, errorSave);
                                }
                            }
                        });
                    }

                    function updateAssignmentGroup() {
                        notifyReady = true;
                        if (assignmentGroup.isModified) {
                            numItemsToWait++;
                            resourceSvc.updateResource(enums.ResourceTypeEnum.AssignmentGroup, assignmentGroup, success, errorSave);
                        }
                    }

                    updateDependents();
                    updateAssignmentGroup();
                }

                function SaveNew() {
                    dependents.forEach(function (item: IAssignmentInfo) {
                        if (item.isChosen)
                            assignmentGroup.assignments.push(item);
                    });

                    resourceSvc.createResource(enums.ResourceTypeEnum.AssignmentGroup, assignmentGroup, successSaveNew, errorSave);
                }

                function checkIfValid(): boolean {
                    var isValid: boolean = false;
                    dependents.forEach(function (item: IAssignmentInfo) {
                        if (item.isChosen)
                            isValid = true;
                    });

                    return isValid;
                }

                if (checkIfValid() == false) {
                    dialogSvc.alert(str.atleastOneDependentChosen);
                    return;
                }

                if (util.showLoginIfGuest(user, loginModalSvc))
                    return;

                saveBtn.isReady = false;
                util.disableForm(page);
                if (editMode)
                    SaveEdit();
                else {
                    SaveNew();
                }
            }
        };

        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successAssignmentGroupModel(data: IAssignmentGroupModel) {
                $scope.assignmentGroup = assignmentGroup = data;
                updateIsReady();

                setInitialDate();

                assignmentGroup.targetDate = new Date(data.targetDate);

                assignmentGroup.assignments.forEach(function (item: IAssignmentInfo) {
                    dependents.forEach(function (dependent: IAssignmentInfo) {
                        if (dependent.dependentId == item.dependentId) {
                            dependent.isChosen = true;
                            dependent.isCompleted = item.isCompleted;
                        }
                    });
                });

                if (dependents.length == 1) {
                    dependents[0].isChosen = true;
                }

                updateNumQuestions();
            }

            function setInitialDate() {
                if (assignmentGroup.noDueDate) {
                    var today: Date = new Date();
                    assignmentGroup.targetDate = new Date();
                    assignmentGroup.targetDate.setDate(today.getDate() + 5);
                }
            }

            function successTest(data: ITestModel) {
                test = data;
                updateIsReady();

                var idx = util.getNumber($stateParams.assGId);
                if (idx == -1) {
                    editMode = false;
                    page.title = "Create Assignment";
                    saveBtn.name = "Create";

                    assignmentGroup.quizzId = quizz.id;
                    assignmentGroup.testSettingId = test.defaultSettingId;

                    assignmentGroup.noDueDate = true;
                    setInitialDate();

                    var testSettings: ITestSettingModel = {};
                    util.copy(testSettings, test.defaultSetting);
                    testSettings.id = 0;

                    assignmentGroup.testSetting = testSettings;

                    successAssignmentGroupModel(assignmentGroup);
                } else {
                    editMode = true;
                    page.title = "Edit Assignment";
                    saveBtn.name = "Save";

                    var idx: number = util.getNumber($stateParams.assGId);
                    assignmentGroup = cachedDataSvc.getAssignmentGroupById(idx, successAssignmentGroupModel, errorLoad);
                }
            }

            function successQuizz(data: IQuizzOverviewModel) {
                $scope.quizz = quizz = data;
                updateIsReady();

                test = cachedDataSvc.getTestById(quizz.testId, successTest, errorLoad);
            }

            function createDependents() {
                user.asUserDependents.forEach(function (item: IDependentModel) {
                    var assignmentInfo: IAssignmentInfo = {
                        id: 0,
                        dependentFullName: item.dependentFullName,
                        isCompleted: false,
                        dependentId: item.childId,
                        isChosen: false
                    };
                    dependents.push(assignmentInfo);
                });
            }

            $anchorScroll("top");
            createDependents();
            quizz = cachedDataSvc.getQuizzOverviewById($stateParams.quizzId, successQuizz, errorLoad);
        }

        function editTestSetting(): void {
            var settings: ng.ui.bootstrap.IModalSettings = {
                templateUrl: "scripts/templates/quizz/test/testSetting/EditTestSetting.html",
                controller: "editTestSettingCtrl",
                resolve: {
                    modalSetting: function (): editTestSettingCtrl.IModalSetting {
                        return {
                            numAvailQuestions: test.questions.length,
                            //saveToDb: editMode,
                            saveToDb: false,
                            isBuiltIn: quizz.isBuiltIn
                        }
                    },
                    setting: function (): ITestSettingModel {
                        return assignmentGroup.testSetting;
                    }
                }
            }

            modalSvc.open(settings, function () {
                updateNumQuestions();
                assignmentGroup.isModified = true;
            });
        }

        function updateNumQuestions() {
            if (assignmentGroup.testSetting.numberOfQuestions === 0)
                assignmentGroup.numQuestions = quizz.numQuestions;
            else
                assignmentGroup.numQuestions = assignmentGroup.testSetting.numberOfQuestions;
        }

        function changed(item: ITrackModify) {
            item.isModified = true;
            saveBtn.disabled = false;
        }

        function cancel() {
            if (editMode && assignmentGroup.isModified) {
                var idx: number = util.getNumber($stateParams.assGId);
                cachedDataSvc.deleteAssignmentGroupdById(idx);
            }

            goBack();
        }

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
            //history.back();
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.changed = changed;
        $scope.assignmentGroup = assignmentGroup;
        $scope.dependents = dependents;
        $scope.saveBtn = saveBtn;

        $scope.editTestSetting = editTestSetting;
        $scope.goBack = goBack;
    }
})(); 