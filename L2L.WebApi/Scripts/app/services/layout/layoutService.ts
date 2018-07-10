interface ILayoutSvc {
    getLayoutModel: (success?: Function, error?: Function) => void;
    // testSnapshots
    getTestSnapshots(): ILayoutTestSnapshot[];
    overwriteOngoingTest(callback: (result: boolean) => void): void;
    removeOngoingTest(callback: (result: boolean) => void): void;
    changeTestSnapshot(idx: number): void;
    removeTestSnapshot(): void;
    // Quizzlings
    updateQuizzlings();
    // Assignments given
    updateAssignmentsGiven();
    // Assignements
    updateAssignments();
    // Recent Test Results
    updateRecentTestResults();
    // My Quizzes
    updateMyQuizzes();

    topPanelControl: any;
}

module layoutSvc {
    export interface IResource {
        layout: any;
    }
}

(function () {
    l2lApp.factory('layoutSvc', layoutSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser: ICurrentUser): layoutSvc.IResource {

        var layoutResource = $resource(setting.serverUrl() + '/api/layout', null,
            {
                'get': { headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        return {
            layout: layoutResource
        }
    }

    layoutSvc.$inject = ["$resource", "currentUser", "resourceSvc", "dialogSvc"];
    function layoutSvc(
        $resource,
        currentUser: ICurrentUser,
        resourceSvc: IResourceSvc,
        dialogSvc: IDialogSvc
        ): ILayoutSvc {
        function getLayoutModel(success?: Function, error?: Function): void {
            resource($resource, currentUser).layout.get(null, function (data: ILayoutModel) {
                layout = data;
                testSnapshots = layout.leftSideBar.testSnapshots;
                updateTestSnapshots();
                assignmentGroups = layout.leftSideBar.assignmentsGiven;
                assignments = layout.leftSideBar.assignments;

                if (success !== undefined)
                    success(data);
            }, error);
        }

        var layout: ILayoutModel = undefined;
        var testSnapshots: ILayoutTestSnapshot[];
        var assignmentGroups: ILayoutAssignmentGroupModel[];
        var assignments: ILayoutAssignmentModel[];

        function errorLoad() {
        }

        // testSnapshots
        function updateTestSnapshots() {
            testSnapshots.forEach(function (item: ILayoutTestSnapshot) {
                item.origId = item.id;
            });
        }

        function getTestSnapshots(): ILayoutTestSnapshot[] {
            return testSnapshots;
        }

        function overwriteOngoingTest(callback: (result: boolean) => void): void {
            var ret: boolean = true;
            if (testSnapshots !== undefined && testSnapshots.length != 0) {
                dialogSvc.confirm(str.confirmChangeTestSnapshot, function (result: boolean) {
                    testSnapshots.forEach(function (item: ILayoutTestSnapshot) {
                        function successDelete() {
                        }

                        function errorDelete() {
                        }

                        resourceSvc.deleteResource(enums.ResourceTypeEnum.TestSnapshot, item.origId, successDelete, errorDelete);
                    });
                    callback(result);
                });
            } else
                callback(true);
        }

        function changeTestSnapshot(idx: number) {
            function updateTestSnapShot() {
                testSnapshots.splice(0, testSnapshots.length);
                var item: ILayoutTestSnapshot = {
                    id: -1,
                    origId: idx
                };
                testSnapshots.push(item);
            }

            updateTestSnapShot()
        }

        function removeOngoingTest(callback: (result: boolean) => void): void {
            var ret: boolean = true;
            if (testSnapshots.length != 0) {
                dialogSvc.confirm(str.confirmRemoveTestSnapshot, function (result: boolean) {
                    testSnapshots.forEach(function (item: ILayoutTestSnapshot) {
                        function successDelete() {
                        }

                        function errorDelete() {
                        }

                        resourceSvc.deleteResource(enums.ResourceTypeEnum.TestSnapshot, item.origId, successDelete, errorDelete);
                    });
                    removeTestSnapshot();
                    callback(result);
                });
            } else
                callback(true);
        }

        function removeTestSnapshot() {
            testSnapshots.forEach(function (item: ILayoutTestSnapshot) {
                resourceSvc.deleteResource(enums.ResourceTypeEnum.TestSnapshot, item.origId);
            });
            testSnapshots.splice(0, testSnapshots.length);
        }

        // Quizzling
        function updateQuizzlings() {
            resourceSvc.getResourceMany(enums.ResourceTypeEnum.Layout, 1, 0, 0, 0, 0,
                function (data: ILayoutDependentModel[]) {
                    layout.leftSideBar.dependents.splice(0, layout.leftSideBar.dependents.length);

                    data.forEach(function (item: ILayoutDependentModel) {
                        layout.leftSideBar.dependents.push(item);
                    });
                },
                errorLoad);
        }

       
        // Assignment Given
        function updateAssignmentsGiven() {
            resourceSvc.getResourceMany(enums.ResourceTypeEnum.Layout, 3, 0, 0, 0, 0,
                function (data: ILayoutAssignmentGroupModel[]) {
                    layout.leftSideBar.assignmentsGiven.splice(0, layout.leftSideBar.assignmentsGiven.length);

                    data.forEach(function (item: ILayoutAssignmentGroupModel) {
                        layout.leftSideBar.assignmentsGiven.push(item);
                    });
                },
                errorLoad);
        }

        // Assignments
        function updateAssignments() {
            resourceSvc.getResourceMany(enums.ResourceTypeEnum.Layout, 3, 0, 0, 0, 0,
                function (data: ILayoutAssignmentModel[]) {
                    layout.leftSideBar.assignments.splice(0, layout.leftSideBar.assignments.length);

                    data.forEach(function (item: ILayoutAssignmentModel) {
                        layout.leftSideBar.assignments.push(item);
                    });
                },
                errorLoad);
        }

        // Recent Test Results
        function updateRecentTestResults() {
            resourceSvc.getResourceMany(enums.ResourceTypeEnum.Layout, 2, 0, 0, 0, 0,
                function (data: ILayoutRecentQuizzModel[]) {
                    layout.leftSideBar.recentQuizzes.splice(0, layout.leftSideBar.recentQuizzes.length);

                    data.forEach(function (item: ILayoutRecentQuizzModel) {
                        layout.leftSideBar.recentQuizzes.push(item);
                    });
                },
                errorLoad);
        }


        // My Quizzes
        function updateMyQuizzes() {
            resourceSvc.getResourceMany(enums.ResourceTypeEnum.Layout, 0, 0, 0, 0, 0,
                function (data: ILayoutQuizzModel[]) {
                    layout.leftSideBar.myQuizzes.splice(0, layout.leftSideBar.myQuizzes.length);

                    data.forEach(function (item: ILayoutQuizzModel) {
                        layout.leftSideBar.myQuizzes.push(item);
                    });
                },
                errorLoad);
        }

        var topPanelControl = (function () {
            var vars = {
                newQuizzClassNotificationCount: 0,
                newFriendRequestCount: 0,
                newMessageCount: 0,
                newNotificationCount: 0
            };

            function addToNewQuizzClassCount(count: number) {
                vars.newQuizzClassNotificationCount += count;
            }

            function subFromNewQuizzClassCount(count: number) {
                vars.newQuizzClassNotificationCount -= count;
            }

            function addToNewFriendRequestCount(count: number) {
                vars.newFriendRequestCount += count;
            }

            function subFromNewFriendRequestCount(count: number) {
                vars.newFriendRequestCount -= count;
            }

            function addToNewMessageCount(count: number) {
                vars.newMessageCount += count;
            }

            function subFromNewMessageCount(count: number) {
                vars.newMessageCount -= count;
            }

            function addToNewNotificationCount(count: number) {
                vars.newNotificationCount += count;
            }

            function subFromNewNotificationCount(count: number) {
                vars.newNotificationCount -= count;
            }

            return {
                vars: vars,
                addToNewQuizzClassCount: addToNewQuizzClassCount,
                subFromNewQuizzClassCount: subFromNewQuizzClassCount,
                addToNewFriendRequestCount: addToNewFriendRequestCount,
                subFromNewFriendRequestCount: subFromNewFriendRequestCount,
                addToNewMessageCount: addToNewMessageCount,
                subFromNewMessageCount: subFromNewMessageCount,
                addToNewNotificationCount: addToNewNotificationCount,
                subFromNewNotificationCount: subFromNewNotificationCount
            }
        })();

        return {
            getLayoutModel: getLayoutModel,
            // TestSnapshots
            getTestSnapshots: getTestSnapshots,
            overwriteOngoingTest: overwriteOngoingTest,
            removeOngoingTest: removeOngoingTest,
            changeTestSnapshot: changeTestSnapshot,
            removeTestSnapshot: removeTestSnapshot,
            // Assignments Given
            updateAssignmentsGiven: updateAssignmentsGiven,
            // Assignments
            updateAssignments: updateAssignments,
            // Quizzlings
            updateQuizzlings: updateQuizzlings,
            // Recent Test Results
            updateRecentTestResults: updateRecentTestResults,
            // My Quizzes
            updateMyQuizzes: updateMyQuizzes,
            // topPanel
            topPanelControl: topPanelControl
        }
    }
})();