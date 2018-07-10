module viewQuickNoteCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage,
        quizz: IQuizzModel;
        goBack: () => void;
    }

    export interface IStateParams {
        quizzId: number;
    }
}

(function () {
    l2lControllers.controller('viewQuickNoteCtrl', viewQuickNoteCtrl);

    viewQuickNoteCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser"];
    function viewQuickNoteCtrl(
        $scope: viewQuickNoteCtrl.IScope,
        $stateParams: viewQuickNoteCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser
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

            var quizzId: number = util.getNumber($stateParams.quizzId);
            quizz = cachedDataSvc.getQuizzById(quizzId, successQuizz, errorLoad);
        }

        function goBack() {
            $state.go("si.viewQuickNotes", { quizzId: $stateParams.quizzId, revId: quizz.reviewerId, edit: false });
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.quizz = quizz;
        $scope.goBack = goBack;
    }
})();