module enrolledQuizzClassCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        depId: number;
        depName: string;
        view: number;

        quizzClass: IQuizzClassModel;

        goBack(): void;
    }

    export interface IStateParams {
        qcId: number;
        depId: number;
        depName: string;
        view: number;
    }
}

(function () {
    l2lControllers.controller('enrolledQuizzClassCtrl', enrolledQuizzClassCtrl);

    enrolledQuizzClassCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "topPanelContentSvc"];

    function enrolledQuizzClassCtrl(
        $scope: enrolledQuizzClassCtrl.IScope,
        $stateParams: enrolledQuizzClassCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        resourceSvc: IResourceSvc,
        $timeout: ng.ITimeoutService,
        dialogSvc: IDialogSvc,
        topPanelContentSvc: ITopPanelContentSvc
    ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 1,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var quizzClass: IQuizzClassModel = {};

        function errorSave(response) {
            notificationSvc.error(str.errorSave);
        }

        function errorLoad(response) {
            notificationSvc.error(str.errorLoad);
        }

        function init() {

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successQuizzClass(data: IQuizzClassModel) {
                $scope.quizzClass = quizzClass = data;
                page.title = quizzClass.className;
                updateIsReady();

                switch ($scope.view) {
                    case 0: // summary
                        break;
                    case 1: // announement
                        topPanelContentSvc.subFromNewQuizzClassCount(quizzClass.member.newAnnouncementCount);
                        break;
                    case 2: // materials
                        break;
                    case 3: // lessons
                        topPanelContentSvc.subFromNewQuizzClassCount(quizzClass.member.newClassLesson);
                        break;
                    case 4: // discussions
                        if ($scope.depId === -1)
                            topPanelContentSvc.subFromNewQuizzClassCount(quizzClass.member.newClassCommentCount);
                        break;
                    case 5: // learners
                        break;
                    case 6: // Quizzes
                        topPanelContentSvc.subFromNewQuizzClassCount(quizzClass.member.newClassQuizzCount);
                        break;
                }
            }

            $anchorScroll("top");
            var qcId: number = util.getNumber($stateParams.qcId);
            $scope.depId = util.getNumber($stateParams.depId);
            $scope.depName = $stateParams.depName;
            $scope.view = util.getNumber($stateParams.view);

            if ($scope.depId === -1)
                quizzClass = resourceSvc.getResource(enums.ResourceTypeEnum.QuizzClass, qcId, successQuizzClass, errorLoad);
            else
                quizzClass = resourceSvc.getResourceAlt(enums.ResourceTypeEnum.QuizzClass, qcId.toString() + "," + $scope.depId.toString(), successQuizzClass, errorLoad);
        }

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.quizzClass = quizzClass;

        $scope.goBack = goBack;
    }
})();