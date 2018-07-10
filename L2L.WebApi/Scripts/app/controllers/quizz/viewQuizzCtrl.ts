module viewQuizzCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        quizzId: number;
        goBack: () => void;

        help: number;
        tipControl: any;
    }

    export interface IStateParams {
        quizzId: number;
        help: number;
    }
}

(function () {
    l2lControllers.controller('viewQuizzCtrl', viewQuizzCtrl);

    viewQuizzCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "currentUser", "$anchorScroll", "quizzSvc"];
    function viewQuizzCtrl(
        $scope: viewQuizzCtrl.IScope,
        $stateParams: viewQuizzCtrl.IStateParams,        
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        quizzSvc: IQuizzSvc
        ) {

        var user: IUserModel;
        var page: IPage = {
            numResourceToWait: 0,
            isReady: true,
            disabled: false
        }
        var quizz: IQuizzModel = {};
        var quizzOverview: IQuizzOverviewModel = {};
        var test: ITestModel;

        function init() {

            function processHelp(stateParam: any, scope: any) {
                var idx: number = util.getNumber(stateParam.help);
                scope.help = idx;
            }

            $anchorScroll("top");           
            user = currentUser.getUserData();

            processHelp($stateParams, $scope);
        }

        function goBack(): void {
            history.back();
        }

        init();

        $scope.quizzId = $stateParams.quizzId;
        $scope.user = user;
        $scope.page = page;
        $scope.goBack = goBack;
        $scope.tipControl = {};
    }
})();