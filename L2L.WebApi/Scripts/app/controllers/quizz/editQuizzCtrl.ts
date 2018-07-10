module editQuizzCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPageExtend;
        ctrlState: ICtrlState;
        message: string;
        quizzGradeLevelList: IQuizzGradeLevelModel[];

        quizz: IQuizzModel;
        categories: IQuizzCategoryModel[];
        submit: (form: ng.IFormController) => void;
        cancel: () => void;
        changed: (model: ITrackModify) => void;
        gradeLevelChanged: (value: string) => void;

        saveButton: IElement;

        goBack(): void;
    }

    export interface IStateParams {
        quizzId: any;
    }

    export interface IPageExtend extends IPage {
        isEditMode: boolean;
        gradeLevelMin: string;
        gradeLevelMax: string;
    }
}

(function () {
    l2lControllers.controller('editQuizzCtrl', editQuizzCtrl);

    editQuizzCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "quizzSvc", "quizzCategorySvc", "broadcastSvc", "loginModalSvc", "layoutSvc"];
    function editQuizzCtrl(
        $scope: editQuizzCtrl.IScope,
        $stateParams: editQuizzCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        quizzSvc: IQuizzSvc,
        quizzCategorySvc: IQuizzCategorySvc,
        broadcastSvc: IBroadcastSvc,
        loginModalSvc: ILoginModalSvc,
        layoutSvc: ILayoutSvc
        ) {

        var quizz: IQuizzModel;
        var categories: IQuizzCategoryModel[] = new Array<IQuizzCategoryModel>();
        var page: editQuizzCtrl.IPageExtend = {
            isReady: false,
            numResourceToWait: 2,
            isEditMode: false,
            disabled: false,
            gradeLevelMax: "0",
            gradeLevelMin: "0"
        };
        var user: IUserModel = currentUser.getUserData();
        var saveButton: IElement = {
            isEnabled: true,
            click: submit
        }

        function error(response) {
            util.enableForm(page);
            commonError($scope, response);
            notificationSvc.error("Something went wrong!");
        }

        function init() {
            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait == 0;
            }

            function quizzSuccess(data: IQuizzModel) {
                quizz = data;
                updateIsReady();
                page.gradeLevelMin = data.gradeLevelMin.toString();
                page.gradeLevelMax = data.gradeLevelMax.toString();
                data.isModified = false;
            }

            function setAsCreateMode() {
                page.title = "New Quizz";
                page.isEditMode = false;
                saveButton.name = "Create";
            }

            function setAsEditMode() {
                page.title = "Edit Quizz";
                page.isEditMode = true;
                saveButton.name = "Save";
            }

            function successCategories(data) {
                $scope.categories = categories = data;
                updateIsReady();

                if ($stateParams.quizzId === undefined || $stateParams.quizzId === "-1" || $stateParams.quizzId === -1) {
                    setAsCreateMode();
                    quizz = {
                        category: enums.QuizzCategoryEnum.Math,
                        isBuiltIn: false,
                        gradeLevelMin: enums.QuizzGradeLevelEnum.Unassigned,
                        gradeLevelMax: enums.QuizzGradeLevelEnum.Unassigned
                    };
                    quizzSuccess(quizz);
                }
                else {
                    setAsEditMode();
                    quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, quizzSuccess, error);
                }
            }

            categories = cachedDataSvc.getQuizzzCategories(successCategories, error);
        }

        function submit(form: ng.IFormController) {
            function notifyChanged(eventType: bcEventTypeEnum, data) {
                var bcData: IBroadcastData = {
                    eventType: eventType,
                    model: data
                }
                //broadcastSvc.postMyQuizzEvent(bcData);
            }

            function saveEditQuizz() {
                if (quizz.isModified == true)
                    quizzSvc.updateQuizz(quizz,
                        function (data) {
                            cachedDataSvc.deleteQuizzOverviewById(data.id);
                            util.enableForm(page);
                            //notifyChanged(bcEventTypeEnum.modifyItem, quizz);
                            notificationSvc.success("Modifications Saved");
                            layoutSvc.updateMyQuizzes();
                            
                            goBack();
                        }, error);
            }

            function createNewQuizz() {
                quizzSvc.createNewQuizz($scope.quizz,
                    function (data) {
                        util.enableForm(page);
                        //notifyChanged(bcEventTypeEnum.newItem, data);
                        notificationSvc.success("New Quizz added successfully");
                        layoutSvc.updateMyQuizzes();

                        $state.go("si.quizzDetail", { quizzId: data.id, view: 3 });
                    }, error);
            }

            if (util.showLoginIfGuest(user, loginModalSvc))
                return;

            quizz.gradeLevelMin = parseInt(page.gradeLevelMin);
            quizz.gradeLevelMax = parseInt(page.gradeLevelMax);

            if (form.$valid) {
                util.disableForm(page);
                if (page.isEditMode)
                    saveEditQuizz();
                else
                    createNewQuizz();
            }
        }

        function changed(model: ITrackModify): void {
            model.isModified = true;
        }

        function cancel(): void {
            goBack();
        }

        function goBack(): void {
            history.back();
        }

        function gradeLevelChanged(value: string): void {
            quizz.isModified = true;
            var min = parseInt(page.gradeLevelMin);
            var max = parseInt(page.gradeLevelMax);
            if (max < min) {
                if (value == "min")
                    page.gradeLevelMax = page.gradeLevelMin;
                else
                    page.gradeLevelMin = page.gradeLevelMax;
            }
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.quizz = quizz;
        $scope.categories = categories;
        $scope.submit = submit;
        $scope.changed = changed;
        $scope.quizzGradeLevelList = list.quizzGradeLevels;
        $scope.gradeLevelChanged = gradeLevelChanged;

        $scope.cancel = cancel;
        $scope.saveButton = saveButton;

        $scope.goBack = goBack;
    }
})();