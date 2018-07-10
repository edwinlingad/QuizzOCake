module patternCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        quizz: IQuizzModel;
        goBack(): void;
    }

    export interface IStateParams {
        quizzId: number;
    }
} 

(function () {
    l2lControllers.controller('patternCtrl', patternCtrl);

    patternCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

    function patternCtrl(
        $scope: patternCtrl.IScope,
        $stateParams: patternCtrl.IStateParams,       
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
        var user: IUserModel = currentUser.getUserData();
        var quizz: IQuizzModel = {};
    
        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }
            
            function successQuizz(data: IQuizzModel) {
                $scope.quizz = quizz = data;
                updateIsReady();
            }

            $anchorScroll("top");
            quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, successQuizz, errorLoad);
        }

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.quizz = quizz;
        $scope.goBack = goBack;
    }
})();