module quizzmatesCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        quizzmatesItem: IItemToWait;
        quizzmates: IQuizzerModel[];
        goBack(): void;
    }

    export interface IStateParams {
    }
}

(function () {
    l2lControllers.controller('quizzmatesCtrl', quizzmatesCtrl);

    quizzmatesCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

    function quizzmatesCtrl(
        $scope: quizzmatesCtrl.IScope,
        $stateParams: quizzmatesCtrl.IStateParams,
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
        var quizzmatesItem: IItemToWait = { isReady: true };
        var quizzmates: IQuizzerModel[] = new Array<IQuizzerModel>();
        var count: number = 0;
        var currPage: number = 1;
        var numPerPage: number = 10;

        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function callGetQuizzmatesResource() {
                resourceSvc.getResourceMany(enums.ResourceTypeEnum.Quizzmates, 0, currPage, numPerPage, 0, 0, getQuizzmates, errorLoad);
            }

            function getQuizzmates(data: IQuizzerModel[]) {
                updateIsReady();
                data.forEach(function (item: IQuizzerModel) {
                    quizzmates.push(item);
                });

                count -= numPerPage;
                currPage++;
                quizzmatesItem.isReady = true;

                if (count > 0) {
                    callGetQuizzmatesResource();
                }
            }

            function successQuizzmateCount(data: any) {
                count = data.count;
                
                if (count > 0) {
                    page.numResourceToWait++;
                    callGetQuizzmatesResource();
                }

                updateIsReady();
            }

            $anchorScroll("top");
            count = resourceSvc.getResource(enums.ResourceTypeEnum.Quizzmates, 0, successQuizzmateCount, errorLoad);
        }

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        init();

        $scope.user = user;
        $scope.page = page;

        $scope.quizzmatesItem = quizzmatesItem;
        $scope.quizzmates = quizzmates;

        $scope.goBack = goBack;
    }
})();