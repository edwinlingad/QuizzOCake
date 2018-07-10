module testLogGroupedDirective {
    export interface IScope {
        page: IPage;
        testLogGrouped: ITestLogGroupedModel[];

        userId: number;
        goToTestLog: (quizz: IQuizzSummaryModel, testLog: ITestLogSummaryModel) => void;
    }
}

(function () {
    l2lApp.directive("testLogGrouped", function () {

        controller.$inject = ["$scope", "notificationSvc", "cachedDataSvc", "$state", "testLogSvc"];

        function controller(
            $scope: testLogGroupedDirective.IScope,
            notificationSvc: INotificationSvc,
            cachedDataSvc: ICachedDataSvc,
            $state: ng.ui.IStateService,
            testLogSvc: ITestLogSvc
            ) {

            var page: IPage = {
                isReady: false,
                numResourceToWait: 1
            }
            var testLogGrouped: ITestLogGroupedModel[] = new Array<ITestLogGroupedModel>();

            function init() {
                function errorLoad() {
                    notificationSvc.error(str.errorLoad);
                }

                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait == 0;
                }

                function successTestLogGrouped(data: ITestLogGroupedModel[]) {
                    $scope.testLogGrouped = testLogGrouped = data;
                    updateIsReady();

                    testLogGrouped.forEach(function (item: ITestLogGroupedModel) {
                        item.isOpen = false;
                        item.average = (item.quizzSummary.scoreSum / item.quizzSummary.totalSum) * 100;

                        item.testLogs.forEach(function (log: ITestLogSummaryModel) {
                            log.average = (log.score / log.total) * 100;
                        });
                    });
                }

                testLogGrouped = cachedDataSvc.getTestLogGroupedById($scope.userId, successTestLogGrouped, errorLoad);
            }

            function goToTestLog(quizz: IQuizzSummaryModel, testLog: ITestLogSummaryModel) {
                $state.go("si.viewResult", {
                    testLogId: testLog.id,
                    quizzId: quizz.id
                });
            }

            init();

            $scope.page = page;
            $scope.testLogGrouped = testLogGrouped;
            $scope.goToTestLog = goToTestLog;
        }

        return {
            restrict: "E",
            templateUrl: 'scripts/templates/directives/quizz/test-log-grouped.html',
            replace: true,
            scope: {
                userId: "@"
            },
            controller: controller
        }
    });
})();