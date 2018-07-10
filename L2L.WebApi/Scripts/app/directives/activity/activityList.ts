module activityListDirective {
    export interface IScope {
        isDependent: string;
        userId: string;
        listType: string;
        activities: IActivityModel[];
        openActivity: (item: IActivityModel) => void;
        openActivityCb: (item: IActivityModel) => void;

        notificationItem: IItemToWait;
        showMore: IButtonElement;

        openQuizzer: (item: IActivityModel) => void;
        openQuizz: (item: IActivityModel) => void;
        openTestLog: (item: IActivityModel) => void;

        showDetails: (item: IActivityModel) => void;
    }
}

(function () {
    l2lApp.directive("activityList", function () {

        controller.$inject = ["$scope", "$state", "activitySvc", "notificationSvc", "$anchorScroll" ];
        
        function controller(
            $scope: activityListDirective.IScope,
            $state: ng.ui.IStateService,
            activitySvc: IActivitySvc,
            notificationSvc: INotificationSvc,
            $anchorScroll: ng.IAnchorScrollService
            ) {

            var notificationItem: IItemToWait = { isReady: true };
            var activities: IActivityModel[] = new Array<IActivityModel>();
            var count: number = 0;
            var currPage: number = 1;
            var numPerPage: number = 5;
            var showMore: IButtonElement = {
                isReady: false,
                click: function () {
                    initiateGetActivities();
                }
            };

            function init() {
                $anchorScroll("top");
                initiateGetActivities();
            }

            function initiateGetActivities() {
                function errorLoad() {
                    notificationSvc.error(str.errorLoad);
                }

                function getActivities() {
                    if ($scope.listType === "current-user") {
                        activitySvc.getCurrentUserActivities(currPage, numPerPage, 0, successGetActivities, errorLoad);
                    } else if ($scope.listType === "user") {
                        var id = parseInt($scope.userId);
                        activitySvc.getUserActivities(id, currPage, numPerPage, 0, successGetActivities, errorLoad);
                    } else if ($scope.listType === "quizzmates") {
                        activitySvc.getQuizzmateActivities(currPage, numPerPage, 0, successGetActivities, errorLoad);
                    }
                }

                function successGetActivities(data: IActivityModel[]) {
                    if (data.length == 0) {
                        notificationItem.isReady = true;
                        return;
                    }

                    data.forEach(function (item) {
                        activities.push(item);
                    });

                    count -= numPerPage;
                    currPage++;

                    if (count <= 0) {
                        notificationItem.isReady = true;
                        showMore.visible = true;
                        return;
                    }

                    getActivities();
                }

                count = 50;
                notificationItem.isReady = false;
                showMore.visible = false;
                getActivities();
            }

            function openActivity(item: IActivityModel) {
                if ($scope.openActivityCb != undefined)
                    $scope.openActivityCb(item);
            }

            function openQuizzer(item: IActivityModel) {
                $state.go("si.quizzer", { quizzerId: item.ownerId });
            }

            function openQuizz(item: IActivityModel) {
                $state.go("si.quizzDetail", { quizzId: item.quizzId, view: 3 });
            }

            function openTestLog(item: IActivityModel) {
                $state.go("si.viewResult", { testLogId: item.testLogId, quizzId: item.quizzId });
            }

            function showDetails(item: IActivityModel) {
                item.isDetailsShown = !item.isDetailsShown;
            }

            init();

            $scope.activities = activities;
            $scope.openActivity = openActivity;
            $scope.notificationItem = notificationItem;
            $scope.showMore = showMore;
            $scope.openQuizzer = openQuizzer;
            $scope.openQuizz = openQuizz;
            $scope.openTestLog = openTestLog;

            $scope.showDetails = showDetails;
        }

        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/activity/activityList.html",
            replace: true,
            scope: {
                isDependent: "@",
                userId: "@",
                listType: "@",  // current-user, user, quizzmates
                openActivityCb: "="
            },
            controller: controller
        }
    });
})();