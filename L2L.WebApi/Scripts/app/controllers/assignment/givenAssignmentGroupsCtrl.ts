/// <reference path="../../utilities/helper.ts" />
module givenAssignmentGroupsCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage,
        assignmentGroups: IAssignmentGroupModel[];

        goToQuizz(item: IAssignmentGroupModel): void;
        goToEdit(item: IAssignmentGroupModel): void;
        goToDependent(id: number): void;
        goToQuizzer(id: number): void;
        goToTestLog(item: IAssignmentInfo, assG: IAssignmentGroupModel): void;
        deleteAssignmentGroup(idx: number, item: IAssignmentGroupModel): void;

        isSingle: boolean;
        toggleDetailedViewBtn: IToggleButton;
        showMoreBtn: IButtonElement;
        showCompletedBtn: IToggleButton;

        goBack(): void;
    }

    export interface IStateParams {
        assGId: any;
    }
}

(function () {
    l2lControllers.controller('givenAssignmentGroupsCtrl', givenAssignmentGroupsCtrl);

    givenAssignmentGroupsCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "loginModalSvc", "layoutSvc"];
    function givenAssignmentGroupsCtrl(
        $scope: givenAssignmentGroupsCtrl.IScope,
        $stateParams: givenAssignmentGroupsCtrl.IStateParams,
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
        layoutSvc: ILayoutSvc
        ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 1,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var assignmentGroups: IAssignmentGroupModel[] = new Array<IAssignmentGroupModel>();
        var pageNumCurrentAssGs: number = 1;
        var showMoreAvailableCurrentAssGs: boolean = true;
        var currentAssGs: IAssignmentGroupModel[] = new Array<IAssignmentGroupModel>();
        var pageNumCompletedAssGs: number = 1;
        var showMoreAvailableCompletedAssGs: boolean = true;
        var completedAssGs: IAssignmentGroupModel[] = undefined;
        var toggleDetailedViewBtn: IToggleButton = {
            value: false,
            name: "expand",
            click: function () {
                toggleDetailedViewBtn.value = !toggleDetailedViewBtn.value;

                if (toggleDetailedViewBtn.value)
                    toggleDetailedViewBtn.name = "collapse";
                else
                    toggleDetailedViewBtn.name = "expand";
            }
        }
        var showMoreBtn: IButtonElement = {
            visible: false,
            isReady: true,
            click: function () {
                function getMoreCurrentAssGs() {
                    function successAssignmentGroups(data: IAssignmentGroupModel[]) {
                        data.forEach(function (item: IAssignmentGroupModel) {
                            updateNumQuestions(item);
                            updateNumDaysDue(item);
                            currentAssGs.push(item);
                            assignmentGroups.push(item);
                        });

                        if (data.length < consts.numPerPage) {
                            showMoreAvailableCurrentAssGs = false;
                            showMoreBtn.visible = false;
                        }

                        showMoreBtn.isReady = true;
                    }

                    cachedDataSvc.getAssignmentGroups(pageNumCurrentAssGs++, false, successAssignmentGroups, errorLoad);
                }

                function getMoreCompletedAssGs() {
                    function successAssignmentGroups(data: IAssignmentGroupModel[]) {
                        data.forEach(function (item: IAssignmentGroupModel) {
                            updateNumQuestions(item);
                            updateNumDaysDue(item);
                            completedAssGs.push(item);
                            assignmentGroups.push(item);
                        });

                        if (data.length < consts.numPerPage) {
                            showMoreAvailableCompletedAssGs = false;
                            showMoreBtn.visible = false;
                        }

                        showMoreBtn.isReady = true;
                    }

                    cachedDataSvc.getAssignmentGroups(pageNumCompletedAssGs++, true, successAssignmentGroups, errorLoad);
                }

                showMoreBtn.isReady = false;
                if (showCompleted.value)
                    getMoreCurrentAssGs();
                else
                    getMoreCompletedAssGs();
            }
        }
        var showCompleted: IToggleButton = {
            isReady: true,
            value: false,
            name: "show completed",
            click: function () {
                showCompleted.value = !showCompleted.value;
                if (showCompleted.value) {
                    showCompleted.name = "show unfinished",
                    updateWithCompletedAssGs();
                } else {
                    showCompleted.name = "show completed",
                    updateWithCurrentAssGs();
                }
            }
        }

        function errorLoad(response) {
            notificationSvc.error(str.errorLoad);
        }

        function init() {

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successAssignmentGroups(data: IAssignmentGroupModel[]) {
                currentAssGs = data;
                updateIsReady();

                data.forEach(function (item: IAssignmentGroupModel) {
                    updateNumQuestions(item);
                    updateNumDaysDue(item);
                });

                updateWithCurrentAssGs();

                if (data.length < consts.numPerPage) {
                    showMoreAvailableCurrentAssGs = false;
                    showMoreBtn.visible = false;
                }
            }

            function successAssignmentGroup(data: IAssignmentGroupModel) {
                updateIsReady();
                assignmentGroups.push(data);

                updateNumQuestions(data);
                updateNumDaysDue(data);
            }

            $anchorScroll("top");
            var idx: number = util.getNumber($stateParams.assGId);
            if (idx != -1) {
                $scope.isSingle = true;
                cachedDataSvc.getAssignmentGroupById(idx, successAssignmentGroup, errorLoad);
            } else {
                $scope.isSingle = false;
                currentAssGs = cachedDataSvc.getAssignmentGroups(pageNumCurrentAssGs++, false, successAssignmentGroups, errorLoad);
            }
        }

        function goToQuizz(item: IAssignmentGroupModel): void {
            $state.go("si.quizzDetail", { quizzId: item.quizzId, view: 3 });
        }

        function goToEdit(item: IAssignmentGroupModel): void {
            $state.go("si.editAssignmentGroup", { quizzId: item.quizzId, assGId: item.id });
        }

        function deleteAssignmentGroup(idx: number, item: IAssignmentGroupModel): void {
            function errorSave(response) {
                notificationSvc.error(str.errorSave);
            }

            if (util.showLoginIfGuest(user, loginModalSvc))
                return;

            dialogSvc.confirm(str.confirmDelete, function (result) {
                if (result == true) {
                    resourceSvc.deleteResource(enums.ResourceTypeEnum.AssignmentGroup, item.id,
                        function (data) {
                            assignmentGroups.splice(idx, 1);
                            notificationSvc.success(str.deleteSuccess);
                            //layoutSvc.removeAssignmentGiven(item.id);

                            layoutSvc.updateAssignmentsGiven();
                        }, errorSave);
                }
            });
        }

        function goToDependent(id: number): void {
            $state.go("si.depActivities", { depId: id });
        }

        function goToQuizzer(id: number): void {
            $state.go("si.quizzer", { quizzerId: id });
        }

        function goToTestLog(item: IAssignmentInfo, assG: IAssignmentGroupModel): void {
            $state.go("si.viewResult", { quizzId: assG.quizzId, testLogId: item.testResultId });
        }

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        function updateWithCurrentAssGs() {
            assignmentGroups.splice(0, assignmentGroups.length);
            currentAssGs.forEach(function (item: IAssignmentGroupModel) {
                assignmentGroups.push(item);
            });
            showMoreBtn.visible = showMoreAvailableCurrentAssGs;
        }

        function updateWithCompletedAssGs() {
            function update() {
                completedAssGs.forEach(function (item: IAssignmentGroupModel) {
                    assignmentGroups.push(item);
                });
                showMoreBtn.visible = showMoreAvailableCompletedAssGs;
            }

            function successCompletedAssGs(data: IAssignmentGroupModel[]) {
                completedAssGs = data;

                data.forEach(function (item: IAssignmentGroupModel) {
                    updateNumQuestions(item);
                    updateNumDaysDue(item);
                });

                if (data.length < consts.numPerPage)
                    showMoreAvailableCompletedAssGs = false;
                update();
            }

            assignmentGroups.splice(0, assignmentGroups.length);
            if (completedAssGs == undefined)
                completedAssGs = cachedDataSvc.getAssignmentGroups(pageNumCompletedAssGs++, true, successCompletedAssGs, errorLoad);
            else
                successCompletedAssGs(completedAssGs);
        }

        function updateNumQuestions(item: IAssignmentGroupModel) {
            if (item.testSetting.numberOfQuestions === 0)
                item.numQuestions = item.quizz.numQuestions;
            else
                item.numQuestions = item.testSetting.numberOfQuestions;
        }

        function updateNumDaysDue(item: IAssignmentGroupModel) {
            if (item.isCompleted || item.noDueDate) {
                item.numDaysDue = 30;
            } else {
                item.numDaysDue = util.getDiffDays(new Date(item.targetDate));
            }
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.assignmentGroups = assignmentGroups;
        $scope.goToQuizz = goToQuizz;
        $scope.goToEdit = goToEdit;
        $scope.deleteAssignmentGroup = deleteAssignmentGroup;
        $scope.goToDependent = goToDependent;
        $scope.goToTestLog = goToTestLog;
        $scope.goBack = goBack;
        $scope.toggleDetailedViewBtn = toggleDetailedViewBtn;
        $scope.showCompletedBtn = showCompleted;
        $scope.showMoreBtn = showMoreBtn;
    }
})();