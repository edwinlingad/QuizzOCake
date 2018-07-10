module quizzEditCategoriesCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        quizzCategory: IQuizzCategoryModel;
        quizzCategories: IQuizzCategoryModel[];
        createNewBtn: IButtonElement;
        saveEditBtn: IButtonElement;
        goBack(): void;
        
    }
}

(function () {
    l2lControllers.controller('quizzEditCategoriesCtrl', quizzEditCategoriesCtrl);

    quizzEditCategoriesCtrl.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];
    function quizzEditCategoriesCtrl(
        $scope: quizzEditCategoriesCtrl.IScope,
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
            isReady: true,
            numResourceToWait: 0,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var quizz: IQuizzModel = {
            id: 0
        };
        var quizzCategory: IQuizzCategoryModel = {
            id: 0
        };
        var quizzCategories: IQuizzCategoryModel[] = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzCategories, 0, 0, 0, 0, 0);
        var createNewBtn: IButtonElement = {
            click: function () {
                resourceSvc.createResource(enums.ResourceTypeEnum.QuizzCategories, quizzCategory,
                    function (data: IQuizzCategoryModel) {
                        quizzCategory.title = "";
                        quizzCategory.description = "";
                        quizzCategory.id = 0;
                        quizzCategory.quizzCategoryType = 0;
                        quizzCategory.iconStrValue = "";
                        quizzCategory.iconColor = "#e53d0e";
                        quizzCategory.borderColor = "#f07a18";
                        quizzCategory.textColor = "#FFFFFF";
                        notificationSvc.success(str.createSuccess);
                        quizzCategories.push(data);
                    }, errorSave);
            }
        };
        var saveEditBtn: IButtonElement = {
            click: function (item: IQuizzCategoryModel) {
                resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzCategories, item, function () {
                    notificationSvc.success(str.updateSuccess);
                }, errorSave);
            }
        }
        
        function errorSave() {
            notificationSvc.error(str.errorSave);
        }

        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successQuizz(data: IQuizzModel) {
                updateIsReady();
            }

            $anchorScroll("top");
        }

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.quizzCategory = quizzCategory;
        $scope.quizzCategories = quizzCategories;
        $scope.createNewBtn = createNewBtn;
        $scope.saveEditBtn = saveEditBtn;
        $scope.goBack = goBack;

        
    }
})();