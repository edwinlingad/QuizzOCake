module notificationListDirective {
    export interface IScope {
        list: INewNotificationModel[];
        openQuizzer(item: INewNotificationModel): void;
        openQuizz(item: INewNotificationModel): void;
        openQuizzCb(item: INewNotificationModel): void;
        openAssignment(item: INewNotificationModel): void;
        openAssignmentGroup(item: INewNotificationModel): void;
    }
}

(function () {
    l2lApp.directive("notificationList", function () {
        controller.$inject = ["$scope", "$state"];

        function controller (
            $scope: notificationListDirective.IScope,
            $state: ng.ui.IStateService
            ) {

            function init() {
            }

            function openQuizz(item: INewNotificationModel) {
                item.isNew = false;

                $state.go("si.quizzDetail", { quizzId: item.quizzId, view: 0 });

                if ($scope.openQuizzCb !== undefined)
                    $scope.openQuizzCb(item);
            }

            function openAssignment(item: INewNotificationModel) {
                $state.go("si.myAssignments", { assId: item.assignmentId });
            }

            function openAssignmentGroup(item: INewNotificationModel) {
                $state.go("si.givenAssignmentGroups", { assGId: item.assignmentGroupId });
            }

            init();

            $scope.openQuizz = openQuizz;
            $scope.openAssignment = openAssignment;
            $scope.openAssignmentGroup = openAssignmentGroup;
        }
        return {
            restrict: 'E',
            templateUrl: 'scripts/templates/directives/notification/notification-list.html',
            replace: true,
            scope: {
                list: '=',
                openQuizzCb: "="
            },
            controller: controller
        }
    });
})();