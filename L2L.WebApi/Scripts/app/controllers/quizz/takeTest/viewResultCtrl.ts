module viewResultCtrl {
    export interface IScope {
        page: IPage;
        user: IUserModel;
        testLog: ITestLogModel;
        quizz: IQuizzOverviewModel;
        testResult: IQuestionTakenModel[];
        testResultScore: ITestProgress;
        goBack: () => void;
        filter: (item: IQuestionTakenModel) => boolean;
        showOnlyErrorsBtn: IToggleButton;
        showOnlyError: boolean;
        isOngoingTest: boolean;

        //rating: IRating;

        retakeControl: any;
        shareControl: any;
    }

    export interface IStateParams {
        testLogId: number;
        quizzId: number;
        testId: number;
    }

    export interface IRating {
        isReady: boolean;
        disabled: boolean;
        uiValue: number;
        currentValue: number;
        init: Function;
        onLeave: Function;
    }
}

(function () {
    l2lControllers.controller("viewResultCtrl", viewResultCtrl);

    viewResultCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "resourceSvc", "testProgressSvc", "quizzUserRatingSvc", "$location", "takeTestCtrlSvc", "facebookSvc"];
    function viewResultCtrl(
        $scope: viewResultCtrl.IScope,
        $stateParams: viewResultCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        resourceSvc: IResourceSvc,
        testProgressSvc: ITestProgressSvc,
        quizzUserRatingSvc: IQuizzUserRatingSvc,
        $location: ng.ILocationService,
        takeTestCtrlSvc: ITakeTestCtrlSvc,
        facebookSvc: IFacebookSvc
    ) {

        var page: IPage = {
            isReady: false,
            numResourceToWait: 2,
            disabled: false
        }
        var quizz: IQuizzOverviewModel;
        var user: IUserModel = currentUser.getUserData();
        var testResult: IQuestionTakenModel[];
        var testResultScore: ITestProgress;
        var testLog: ITestLogModel;
        var showOnlyErrorsBtn: IToggleButton = {
            name: "Show Errors Only",
            value: false,
            click: function (): void {
                showOnlyErrorsBtn.value = !showOnlyErrorsBtn.value;
                if (showOnlyErrorsBtn.value)
                    showOnlyErrorsBtn.name = "Show All";
                else
                    showOnlyErrorsBtn.name = "Show Errors Only";
            }
        }
        var isDbLoad: boolean = false;
        //var rating: viewResultCtrl.IRating = {
        //    isReady: false,
        //    disabled: false,
        //    uiValue: 0,
        //    currentValue: 0,
        //    init: function () {
        //        var quizzCurrentUserRating: IQuizzCurrentUserRatingModel;

        //        quizzCurrentUserRating = resourceSvc.getResource(enums.ResourceTypeEnum.QuizzCurrentUserRating, quizz.id,
        //            function (data: IQuizzCurrentUserRatingModel) {
        //                rating.currentValue = rating.uiValue = data.rating;
        //                rating.isReady = true;
        //            },
        //            function () {
        //                rating.isReady = true;
        //            });
        //    },
        //    onLeave: function () {

        //        if (user.isGuest)
        //            return;

        //        if (rating.uiValue == 0)
        //            rating.uiValue = rating.currentValue;

        //        if (rating.currentValue != rating.uiValue) {
        //            var model: IQuizzUserRatingUpdateRateModel = {
        //                quizzRatingId: quizz.id,
        //                rating: rating.uiValue
        //            };

        //            rating.disabled = true;
        //            resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzCurrentUserRating, model,
        //                function () {
        //                    rating.currentValue = rating.uiValue;
        //                    rating.disabled = false;
        //                },
        //                function () {
        //                    rating.disabled = false;
        //                    notificationSvc.error(str.errorSave);
        //                });
        //        }
        //    },
        //}

        function init() {
            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function successTestLog(data: ITestLogModel) {
                $scope.testLog = testLog = data;
                updateIsReady();

                testLog.average = (testLog.score / testLog.total) / 100;

                var testLogData: ITestLogDataModel = JSON.parse(data.resultBlob);
                $scope.testResult = testResult = testLogData.testResult;
                $scope.testResultScore = testResultScore = testLogData.testResultScore;

                testResult.forEach(function (item: IQuestionTakenModel) {
                    if (item.question !== undefined) {
                        if (item.question.questionViewType === undefined)
                            item.question.questionViewType = 0;

                        if (item.question.actualQuestion !== undefined &&
                            item.question.actualQuestion.question !== undefined &&
                            item.question.actualQuestion.question !== null &&
                            item.question.actualQuestion.question.length !== 0) {

                            item.question.actualQuestion.textContent = item.question.actualQuestion.question;
                        }
                    }
                });

                retakeControl.init();
            }

            function successQuizz(data: IQuizzOverviewModel) {
                $scope.quizz = quizz = data;
                updateIsReady();

                //rating.init();
                if ($stateParams.testLogId == -1) {
                    updateIsReady();

                    $location.replace();
                    $scope.testResult = testResult = testProgressSvc.getTestQuestionResults();
                    $scope.testResultScore = testResultScore = testProgressSvc.getTestProgress();
                    $scope.testLog = testLog = {
                        quizzId: quizz.id,
                        quizzName: quizz.title,
                        dateTaken: new Date(),
                        average: (testResultScore.score / testResultScore.totalAvailableScore) * 100
                    }

                    if (takeTestCtrlSvc.isAssignment()) {
                        var assignment: IAssignmentModel = takeTestCtrlSvc.getAssignment();
                        if (assignment !== undefined && assignment.id !== 0) {
                            testLog.assignmentId = assignment.id;
                            testLog.isPassed = assignment.isCompleted;
                        }

                        retakeControl.init();
                    }
                }
                else {
                    isDbLoad = true;
                    testResultScore = {
                        numCorrectAnswers: 0,
                        numQuestionsTaken: 0,
                        totalAvailableScore: 0,
                        totalQuestions: 0,
                        score: 0,
                        furthestIdx: 0
                    };
                    testResult = new Array<IQuestionTakenModel>();
                    testLog = cachedDataSvc.getTestLogById($stateParams.testLogId, successTestLog, errorLoad);
                }
            }

            $anchorScroll("top");
            $scope.showOnlyError = false;

            var idx: number = util.getNumber($stateParams.testLogId);
            if (idx !== -1) {
                idx = util.getNumber($stateParams.quizzId);
                var ongoingQuizzId: number = takeTestCtrlSvc.getCurrentOngoingQuizzId();
                if (idx == ongoingQuizzId) {
                    $scope.isOngoingTest = true;
                    page.numResourceToWait = 0;
                    updateIsReady();
                    return;
                }
            }
            $scope.isOngoingTest = false;
            quizz = cachedDataSvc.getQuizzOverviewById($stateParams.quizzId, successQuizz, errorLoad);
        }

        function filter(item: IQuestionTakenModel): boolean {
            if ($scope.showOnlyErrorsBtn.value) {
                return !item.isCorrect;
            }
            return true;
        }

        var retakeControl = (function () {
            var vars = {
                name: "retake",
                assId: -1
            };

            function init() {
                if (testLog.assignmentId !== 0 && testLog.isPassed == false) {
                    vars.name = "retake assignment";
                    vars.assId = testLog.assignmentId;
                }
            }

            return {
                vars: vars,
                init: init
            }
        })();

        var shareControl = (function () {
            function share(where: string) {
                if (where === 'facebook') {
                    var message: string = "Got a score of " + Math.round((testResultScore.numCorrectAnswers / testResultScore.totalQuestions) * 100).toString() + "% ( " + testResultScore.numCorrectAnswers + "/" + testResultScore.totalQuestions + " ) on this Quizz : " + quizz.title;
                    var meta: IOGMeta = {
                        type: 0,
                        url: "https://quizzocake.com/#/quizz/quizz-detail/" + quizz.id + "?view=3"
                    }
                    facebookSvc.share(message, meta);
                    return;
                }
            }

            return {
                share: share
            }
        })();

        function goBack() {
            if (isDbLoad == true)
                history.back();
            else
                $state.go("si.quizzCategories");
        }

        init();

        $scope.page = page;
        $scope.user = user;
        $scope.testLog = testLog;
        $scope.quizz = quizz;
        $scope.testResult = testResult;
        $scope.testResultScore = testResultScore;
        $scope.goBack = goBack;
        $scope.filter = filter;
        $scope.showOnlyErrorsBtn = showOnlyErrorsBtn;
        //$scope.rating = rating;
        $scope.retakeControl = retakeControl;
        $scope.shareControl = shareControl;
    }

})();