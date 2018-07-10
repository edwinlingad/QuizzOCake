module editMultiChoiceSameChoiceGroupCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        quizz: IQuizzModel;
        choiceGroup: IMultiChoiceSameChoiceGroupModel;
        addChoiceInput: () => void;
        deleteChoiceInput: (idx: number) => void;
        submit: (form: ng.IFormController) => void;
        goBack: () => void;
        changed: (model: ITrackModify) => void;
        saveButton: IElement;
    }

    export interface IStateParams {
        cgId: any;
        testId: any;
        quizzId: number;
    }
}

(function () {
    l2lControllers.controller("editMultiChoiceSameChoiceGroupCtrl", editMultiChoiceSameChoiceGroupCtrl);

    editMultiChoiceSameChoiceGroupCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "multiChoiceSameChoiceGroupSvc", "multiChoiceSameChoiceSvc", "dialogSvc", "loginModalSvc", "$anchorScroll"];
    function editMultiChoiceSameChoiceGroupCtrl(
        $scope: editMultiChoiceSameChoiceGroupCtrl.IScope,
        $stateParams: editMultiChoiceSameChoiceGroupCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        multiChoiceSameChoiceGroupSvc: IMultiChoiceSameChoiceGroupSvc,
        multiChoiceSameChoiceSvc: IMultiChoiceSameChoiceSvc,
        dialogSvc: IDialogSvc,
        loginModalSvc: ILoginModalSvc,
        $anchorScroll: ng.IAnchorScrollService
    ) {

        var page: IPage = {
            numResourceToWait: 2,
            isReady: false,
            disabled: false
        }
        var user: IUserModel = currentUser.getUserData();
        var quizz: IQuizzModel;
        var test: ITestModel;
        var isEditMode: boolean;
        var choiceGroup: IMultiChoiceSameChoiceGroupModel = {
            id: 0,
            name: "",
            shuffleChoices: true,
            testId: 0,
            isMultiplePoints: true,
            choices: new Array<IMultiChoiceSameChoiceModel>()
        };
        var deletedChoices: IMultiChoiceSameChoiceModel[] = Array<IMultiChoiceSameChoiceModel>();
        var saveButton: IElement = {
            click: submit
        }

        function error(response) {
            commonError($scope, response);
            page.disabled = false;
        }

        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successTest(data: ITestModel) {
                test = data;
                updateIsReady();

                if ($stateParams.cgId === undefined || $stateParams.cgId === "" || $stateParams.cgId === "-1") {
                    isEditMode = false;
                    page.title = "Create New ";
                    saveButton.name = "Create";
                    choiceGroup.testId = data.id;
                    addChoiceInput();
                    addChoiceInput();
                } else {
                    isEditMode = true;
                    page.title = "Edit ";
                    saveButton.name = "Save";
                    for (var i = 0; i < data.multiChoiceSameChoiceGroups.length; i++) {
                        if (data.multiChoiceSameChoiceGroups[i].id == $stateParams.cgId) {
                            $scope.choiceGroup = choiceGroup = data.multiChoiceSameChoiceGroups[i];
                            break;
                        }
                    }

                    choiceGroup.choices.forEach(function (item: IMultiChoiceSameChoiceModel) {
                        item.noDelete = true;
                    });
                }
            }

            function successQuizz(data: IQuizzModel) {
                $scope.quizz = quizz = data;
                updateIsReady();

                test = cachedDataSvc.getTestById($stateParams.testId, successTest, errorLoad);
            }

            $anchorScroll("top");
            quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, successQuizz, errorLoad);
        }

        function addChoiceInput(): void {
            var choice: IMultiChoiceSameChoiceModel = {
                id: 0,
                value: "",
                choiceGroupId: 0,
                noDelete: false
            };
            choiceGroup.choices.push(choice);
        }

        function deleteChoiceInput(idx: number): void {
            var choice: IMultiChoiceSameChoiceModel = choiceGroup.choices[idx];
            if (choice.id != 0)
                deletedChoices.push(choice);
            choiceGroup.choices.splice(idx, 1);
        }

        function submit(form: ng.IFormController): void {
            function errorSave(response) {
                util.enableForm(page);
                notificationSvc.error(str.errorSave);
            }

            function createNewChoiceGroup() {
                multiChoiceSameChoiceGroupSvc.createChoiceGroup(choiceGroup,
                    function (data) {
                        util.enableForm(page);
                        test.multiChoiceSameChoiceGroups.push(data);
                        notificationSvc.success(str.createSuccess);
                        goBack();
                    }, errorSave);
            }

            function saveEditChoiceGroup() {
                var numItemsToWait: number = 0;
                var notifyReady: boolean = false;

                function success() {
                    numItemsToWait--;
                    if (notifyReady && numItemsToWait == 0) {
                        util.enableForm(page);
                        notificationSvc.success(str.updateSuccess);
                        goBack();
                    }
                }

                function removeDeletedItems() {
                    deletedChoices.forEach(function (choice) {
                        numItemsToWait++;
                        multiChoiceSameChoiceSvc.deleteChoice(choice.id, success, errorSave);
                    });
                }

                function AddNewItems() {
                    var newList: IMultiChoiceSameChoiceModel[] = new Array<IMultiChoiceSameChoiceModel>();
                    for (var i = choiceGroup.choices.length - 1; i >= 0; i--) {
                        var choice: IMultiChoiceSameChoiceModel;
                        choice = choiceGroup.choices[i];
                        if (choice.id == 0) {
                            choiceGroup.choices.splice(i, 1);
                            newList.push(choice);
                        }
                    }

                    newList.forEach(function (choice: IMultiChoiceSameChoiceModel) {
                        numItemsToWait++;
                        choice.choiceGroupId = choiceGroup.id;
                        multiChoiceSameChoiceSvc.createChoice(choice,
                            function (data) {
                                choiceGroup.choices.push(data);
                                success();
                            }, errorSave);
                    });
                }

                function updateItems() {
                    choiceGroup.choices.forEach(function (choice: IMultiChoiceSameChoiceModel, idx: number) {
                        if (choice.isModified == true) {
                            numItemsToWait++;
                            multiChoiceSameChoiceSvc.updateChoice(choice, success, errorSave);
                        }
                    });
                }

                function updateChoiceGroup() {
                    numItemsToWait++;
                    notifyReady = true;
                    if (choiceGroup.isModified == true) {
                        multiChoiceSameChoiceGroupSvc.updateChoiceGroup(choiceGroup, success, errorSave);
                    } else {
                        success();
                    }
                }

                removeDeletedItems();
                AddNewItems();
                updateItems();
                updateChoiceGroup();
            }

            if (user.isGuest) {
                loginModalSvc.openLoginModal(false, true);
                return;
            }

            if (form.$valid) {
                util.disableForm(page);
                if (isEditMode)
                    saveEditChoiceGroup();
                else
                    createNewChoiceGroup();
            }
        }

        function goBack() {
            $state.go("si.quizzDetail", { quizzId: quizz.id, view: 3 });
        }

        function changed(model: ITrackModify): void {
            model.isModified = true;
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.quizz = quizz;
        $scope.choiceGroup = choiceGroup;
        $scope.addChoiceInput = addChoiceInput;
        $scope.deleteChoiceInput = deleteChoiceInput;
        $scope.submit = submit;
        $scope.goBack = goBack;
        $scope.changed = changed;
        $scope.saveButton = saveButton;
    }
})();