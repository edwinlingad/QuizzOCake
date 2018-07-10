module assignments {
    export interface IScope {
        user: IUserModel;
        page: IPage,
        assignments: IAssignmentModel[];

        goToQuizz(item: IAssignmentModel): void;
        goToQuizzer(id: number): void;
        goToViewReviewer(item: IAssignmentModel): void;
        goToViewQuickNotes(item: IAssignmentModel): void;
        goToPrepTest(item: IAssignmentModel): void;
        goToTestLog(item: IAssignmentModel): void;

        isSingle: boolean;
        toggleDetailedViewBtn: IToggleButton;
        showMoreBtn: IButtonElement;
        showCompletedBtn: IToggleButton;

        goBack(): void;
    }
}

(function () {
    l2lApp.directive("assignments", assignments);

    controller.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc" ];

    function controller(
        $scope: assignments.IScope,
        $stateParams: myAssignmentsCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        resourceSvc: IResourceSvc,
        $timeout: ng.ITimeoutService,
        dialogSvc: IDialogSvc
        ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 1,
            disabled: false
        };
        var userId: number;
        var user: IUserModel = currentUser.getUserData();
        var assignments: IAssignmentModel[] = new Array<IAssignmentModel>();
        var pageNumCurrentAss: number = 1;
        var showMoreAvailableCurrentAss: boolean = true;
        var currentAss: IAssignmentModel[] = new Array<IAssignmentModel>();
        var pageNumCompletedAss: number = 1;
        var showMoreAvailableCompletedAss: boolean = true;
        var completedAss: IAssignmentModel[] = undefined;
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
                function getMoreCurrentAss() {
                    function successAssignments(data: IAssignmentModel[]) {
                        data.forEach(function (item: IAssignmentModel) {
                            updateNumQuestions(item);
                            updateNumDaysDue(item);
                            currentAss.push(item);

                            assignments.push(item);
                        });

                        if (data.length < consts.numPerPage) {
                            showMoreAvailableCurrentAss = false;
                            showMoreBtn.visible = false;
                        }

                        showMoreBtn.isReady = true;
                    }

                    cachedDataSvc.getAssignments(user.id, pageNumCurrentAss++, false, successAssignments, errorLoad);
                }

                function getMorecompletedAss() {
                    function successAssignments(data: IAssignmentModel[]) {
                        data.forEach(function (item: IAssignmentModel) {
                            updateNumQuestions(item);
                            updateNumDaysDue(item);
                            completedAss.push(item);
                            assignments.push(item);
                        });

                        if (data.length < consts.numPerPage) {
                            showMoreAvailableCompletedAss = false;
                            showMoreBtn.visible = false;
                        }

                        showMoreBtn.isReady = true;
                    }

                    cachedDataSvc.getAssignments(user.id, pageNumCompletedAss++, true, successAssignments, errorLoad);
                }

                showMoreBtn.isReady = false;
                if (showCompleted.value)
                    getMoreCurrentAss();
                else
                    getMorecompletedAss();
            }
        }
        var showCompleted: IToggleButton = {
            isReady: true,
            value: false,
            name: "show completed",
            tooltip: "click to show completed assignments",
            click: function () {
                showCompleted.value = !showCompleted.value;
                if (showCompleted.value) {
                    showCompleted.name = "show unfinished",
                    updateWithcompletedAss();
                } else {
                    showCompleted.name = "show completed",
                    updateWithCurrentAss();
                }
            }
        }

        function errorLoad(response) {
            notificationSvc.error(str.errorLoad);
        }

        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successAssignments(data: IAssignmentModel[]) {
                data.forEach(function (item: IAssignmentModel) {
                    assignments.push(item);
                    updateNumQuestions(item);
                    updateNumDaysDue(item);
                });
                updateIsReady();

                if (data.length < 25) {
                    showMoreBtn.visible = false;
                }
            }

            function successAssignment(data: IAssignmentModel) {
                updateIsReady();
                assignments.push(data);
                updateNumQuestions(data);
                updateNumDaysDue(data);
            }

            $anchorScroll("top");
            userId = user.id;
            var idx: number = util.getNumber($stateParams.assId);
            if (idx != -1) {
                $scope.isSingle = true;
                cachedDataSvc.getAssignmentById(idx, successAssignment, errorLoad);
            } else {
                $scope.isSingle = false;
                currentAss = cachedDataSvc.getAssignments(userId, pageNumCurrentAss++, false, successAssignments, errorLoad);
            }
        }

        function goToViewReviewer(item: IAssignmentModel) {
            $state.go("si.viewReviewer", { quizzId: item.assignmentGroup.quizzId });
        }

        function goToViewQuickNotes(item: IAssignmentModel) {
            $state.go("si.viewQuickNotes", { quizzId: item.assignmentGroup.quizzId, revId: item.assignmentGroup.quizz.reviewerId, edit: false });
        }

        function goToPrepTest(item: IAssignmentModel) {
            $state.go("si.prepTest", { testId: item.assignmentGroup.quizz.testId, quizzId: item.assignmentGroup.quizzId, isPrev: false, assId: item.id });
        }

        function updateWithCurrentAss() {
            assignments.splice(0, assignments.length);
            currentAss.forEach(function (item: IAssignmentModel) {
                assignments.push(item);
            });
            showMoreBtn.visible = showMoreAvailableCurrentAss;
        }

        function updateWithcompletedAss() {
            function update() {
                completedAss.forEach(function (item: IAssignmentModel) {
                    assignments.push(item);
                });
                showMoreBtn.visible = showMoreAvailableCompletedAss;
            }

            function successCompletedAss(data: IAssignmentModel[]) {
                completedAss = data;

                data.forEach(function (item: IAssignmentModel) {
                    updateNumQuestions(item);
                    updateNumDaysDue(item);
                });

                if (data.length < consts.numPerPage)
                    showMoreAvailableCompletedAss = false;
                update();
            }

            assignments.splice(0, assignments.length);
            if (completedAss == undefined)
                completedAss = cachedDataSvc.getAssignments(userId, pageNumCompletedAss++, true, successCompletedAss, errorLoad);
            else
                successCompletedAss(completedAss);
        }

        function updateNumQuestions(item: IAssignmentModel) {
            var assignmentGroup: IAssignmentGroupModel = item.assignmentGroup;
            if (assignmentGroup.testSetting.numberOfQuestions === 0)
                assignmentGroup.numQuestions = assignmentGroup.quizz.numQuestions;
            else
                assignmentGroup.numQuestions = assignmentGroup.testSetting.numberOfQuestions;
        }

        function updateNumDaysDue(item: IAssignmentModel) {
            if (item.isCompleted || item.assignmentGroup.noDueDate) {
                item.numDaysDue = 30;
            } else {
                item.numDaysDue = util.getDiffDays(new Date(item.assignmentGroup.targetDate));
            }
        }

        function goToQuizz(item: IAssignmentModel): void {
            $state.go("si.quizzDetail", { quizzId: item.assignmentGroup.quizzId, view: 3 });
        }

        function goToQuizzer(id: number): void {
            $state.go("si.quizzer", { quizzerId: id });
        }

        function goToTestLog(item: IAssignmentModel): void {
            $state.go("si.viewResult", { quizzId: item.assignmentGroup.quizzId, testLogId: item.testResultId });
        }

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.assignments = assignments;

        $scope.goToQuizz = goToQuizz;
        $scope.goToQuizzer = goToQuizzer;
        $scope.goToViewReviewer = goToViewReviewer;
        $scope.goToPrepTest = goToPrepTest;
        $scope.goToViewQuickNotes = goToViewQuickNotes;
        $scope.goToTestLog = goToTestLog;
        $scope.toggleDetailedViewBtn = toggleDetailedViewBtn;
        $scope.showCompletedBtn = showCompleted;
        $scope.showMoreBtn = showMoreBtn;

        $scope.goBack = goBack;
    }

    function assignments() {
        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/assignments/assignments.html",
            replace: true,
            scope: {
                assId: "@",
                userId: "@"
            },
            controller: controller
        }
    }
})();