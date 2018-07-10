module depActivitiesCtrl {
    export interface IScope {
        page: IPage
        searchParams: quizzOverviewsDirective.ISearchParam;
        depInfo: IDependentUserModel;
        quizzer: IQuizzerModel;
        quizzerTmp: IQuizzerModel;
        depSettings: IDepSettingsTab;
        depId: number;
        view: number;
        isEditBtn: IToggleButton;
        saveBtn: IButtonElement;
        cancelBtn: IButtonElement;
        editProfilePage: IPage;
        changed(item: ITrackModify): void;
        profilePixChanged(): void;
        editProfilePixBtn: IButtonElement;
        saveIsEnabled(): boolean;

        userNameMessage: string;
    }

    export interface IStateParams {
        depId: number;
        view: any;
    }

    export interface IDepSettingsTab {
        isReady: boolean;
        init: () => void;
        saveBtn: IButtonElement;
        changed: (item: ITrackModify) => void;
    }
}

(function () {
    l2lControllers.controller("depActivitiesCtrl", depActivitiesCtrl);

    depActivitiesCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "accountSvc", "dependentSvc", "layoutSvc"];
    function depActivitiesCtrl(
        $scope: depActivitiesCtrl.IScope,
        $stateParams: depActivitiesCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        resourceSvc: IResourceSvc,
        $timeout: ng.ITimeoutService,
        dialogSvc: IDialogSvc,
        accountSvc: IAccount,
        dependentSvc: IDependentSvc,
        layoutSvc: ILayoutSvc
    ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 1,
            disabled: false,
        }
        var searchParams: quizzOverviewsDirective.ISearchParam = {
            userId: $stateParams.depId,
            availOnly: 0
        }

        var depInfo: IDependentUserModel = {
            profile: {},
            permissions: {},
            notificationsSubscription: {},
        };
        var depSettings: depActivitiesCtrl.IDepSettingsTab = {
            isReady: false,
            init: function () {

                var idx: number = util.getNumber($stateParams.depId);
                dependentSvc.getDependentInfo(idx,
                    function (data) {
                        depSettings.isReady = true;
                        $scope.depInfo = depInfo = data;
                    }, errorLoad);
            },
            changed: function changed(item: ITrackModify) {
                depSettings.saveBtn.disabled = false;
                item.isModified = true;
            },
            saveBtn: {
                isReady: true,
                disabled: true,
                click: function () {
                    var itemsToWait: number = 0;

                    if (depSettings.saveBtn.disabled == true)
                        return;

                    if (depInfo.permissions.isModified)
                        itemsToWait++;
                    if (depInfo.notificationsSubscription.isModified)
                        itemsToWait++;

                    function updateIsReady() {
                        itemsToWait--;
                        if (itemsToWait == 0) {
                            notificationSvc.success(str.updateSuccess);
                            depInfo.permissions.isModified = false;
                            depInfo.notificationsSubscription.isModified = false;

                            util.enableForm(<IPage>depSettings.saveBtn);
                            depSettings.saveBtn.disabled = true;
                        }
                    }

                    util.disableForm(<IPage>depSettings.saveBtn);
                    if (depInfo.permissions.isModified)
                        dependentSvc.updatePermissions(depInfo.permissions, updateIsReady, errorSave);
                    if (depInfo.notificationsSubscription.isModified)
                        dependentSvc.updateNotifications(depInfo.notificationsSubscription, updateIsReady, errorSave);
                }
            }
        };
        var quizzer: IQuizzerModel = {};
        var quizzerTmp: IQuizzerModel = {
            isModified: false,
            profile: {
                isModified: false
            }
        };
        var editProfilePage: IPage = {
            disabled: false,
            isReady: true,
            numResourceToWait: 0
        }
        var isEditBtn: IToggleButton = {
            value: false,
            click: function () {
                isEditBtn.value = true;
                quizzerTmp.id = depInfo.id;
                quizzerTmp.userName = depInfo.userName;
                quizzerTmp.profile = {
                    id: depInfo.profile.id,
                    birthDate: new Date(depInfo.profile.birthDate.toString()),
                    firstName: depInfo.profile.firstName,
                    lastName: depInfo.profile.lastName,
                    profileImageUrl: depInfo.profile.profileImageUrl
                }
            }
        }
        var saveBtn: IButtonElement = {
            isReady: true,
            click: function () {
                var finalWait: boolean = false;
                function updateIsReady() {
                    editProfilePage.numResourceToWait--;
                    editProfilePage.isReady = editProfilePage.numResourceToWait <= 0;
                    if (editProfilePage.isReady && finalWait) {
                        notificationSvc.success(str.updateSuccess);
                        util.enableForm(editProfilePage);
                        isEditBtn.value = false;
                    }
                }

                function successQuizzerInfo() {
                    updateIsReady();

                    layoutSvc.updateQuizzlings();

                    if (quizzerTmp.profile.isModified) {

                        depInfo.profile.firstName = quizzerTmp.profile.firstName;
                        depInfo.profile.lastName = quizzerTmp.profile.lastName;
                        depInfo.profile.birthDate = new Date(quizzerTmp.profile.birthDate.toString());

                        quizzer.profile.firstName = quizzerTmp.profile.firstName;
                        quizzer.profile.lastName = quizzerTmp.profile.lastName;

                        if (quizzerTmp.isProfilePixModified) {
                            var lastIdx: number = quizzer.profile.profileImageUrl.lastIndexOf("/");
                            quizzer.profile.profileImageUrl = quizzer.profile.profileImageUrl.substr(0, lastIdx + 1) + quizzerTmp.profilePixName;
                        }

                        resetQuizzerTmpModified();
                    }
                }

                function checkAndUpdateProfile() {
                    finalWait = true;
                    if (quizzerTmp.profile.isModified) {
                        editProfilePage.numResourceToWait++;
                        if (quizzerTmp.isProfilePixModified)
                            quizzerTmp.profilePixName = util.guid() + ".jpg";

                        resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzerInfo, quizzerTmp, successQuizzerInfo, errorSave);
                    } else {
                        successQuizzerInfo();
                    }
                }

                function successUserName() {
                    updateIsReady();
                    quizzerTmp.isModified = false;

                    depInfo.userName = quizzerTmp.userName;
                    checkAndUpdateProfile();
                }

                function successCheckUserName(data: any) {
                    if (data.isAvail == true) {
                        editProfilePage.numResourceToWait++;
                        accountSvc.changeUserName(quizzerTmp, successUserName, errorSave);
                    } else {
                        $scope.userNameMessage = "Username already in use";
                        util.enableForm(editProfilePage);
                        editProfilePage.isReady = true;
                        editProfilePage.numResourceToWait = 0;
                    }
                }

                function checkAndUpdateUserName() {
                    if (quizzerTmp.isModified && quizzerTmp.userName !== depInfo.userName)
                        accountSvc.isUserNameAvailable(quizzerTmp.userName, successCheckUserName, errorSave);
                    else
                        checkAndUpdateProfile();
                }

                if (quizzerTmp.profile.isModified || quizzerTmp.isModified) {
                    util.disableForm(editProfilePage);
                    checkAndUpdateUserName();
                }
            },
        }
        var cancelBtn: IButtonElement = {
            click: function () {
                isEditBtn.value = false;
                resetQuizzerTmpModified();
            }
        }
        var editProfilePixBtn: IButtonElement = {
            click: function () {
                $("#edit-profile-pix").click();
            }
        }

        function errorLoad(response) {
            notificationSvc.error(str.errorLoad);
        }

        function errorSave(response) {
            editProfilePage.isReady = true;
            editProfilePage.disabled = false;
            editProfilePage.numResourceToWait = 0;
            depSettings.saveBtn.disabled = true;
            depSettings.saveBtn.isReady = true;
            util.enableForm(<IPage>depSettings.saveBtn);
            notificationSvc.error(str.errorSave);
        }

        function init() {
            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successQuizzer(data: IQuizzModel) {
                updateIsReady();

                if (quizzer.isDependent === false) {
                    page.isNotFoundOrAuthorized = true;
                    return;
                }

                quizzer.profile.birthDate = new Date(quizzer.profile.birthDate.toString());
            }

            $anchorScroll("top");
            depSettings.init();
            var idx: number = util.getNumber($stateParams.view);
            if (idx === -1)
                idx = 0;
            $scope.view = idx;

            idx = util.getNumber($stateParams.depId);
            $scope.depId = idx;
            quizzer = cachedDataSvc.getQuizzerInfo(idx, successQuizzer, errorLoad);
        }

        function changed(item: ITrackModify) {
            item.isModified = true;

            var tmpQuizzerModel: IQuizzerModel = <IQuizzerModel>item;
            if (tmpQuizzerModel.userName !== undefined) {
                $scope.userNameMessage = "";
            }
        }

        function profilePixChanged() {
            $timeout(function () {
                quizzerTmp.isProfilePixModified = true;
                quizzerTmp.profile.isModified = true;

                pixUtil.saveProfilePix("edit-profile-pix", "profile-image-canvas",
                    function (data: string) {
                        quizzerTmp.profilePix = data;
                    });

            }, 100);
        }

        function resetQuizzerTmpModified() {
            quizzerTmp.isModified = false;
            quizzerTmp.profile.isModified = false;
            quizzerTmp.isProfilePixModified = false;
            quizzerTmp.profilePix = "";
        }

        function saveIsEnabled(): boolean {
            if (quizzerTmp.isModified ||
                quizzerTmp.profile.isModified ||
                quizzerTmp.isProfilePixModified) {

                if (quizzerTmp.userName !== undefined &&
                    quizzerTmp.userName.trim().length !== 0 &&
                    quizzerTmp.profile.firstName !== undefined &&
                    quizzerTmp.profile.firstName.trim().length !== 0 &&
                    quizzerTmp.profile.lastName !== undefined &&
                    quizzerTmp.profile.lastName.trim().length !== 0 &&
                    quizzerTmp.profile.birthDate !== undefined &&
                    quizzerTmp.profile.birthDate !== null
                )
                    return true;
            }
            return false;
        }

        init();

        $scope.userNameMessage = "";
        $scope.page = page;
        $scope.quizzer = quizzer;
        $scope.quizzerTmp = quizzerTmp;
        $scope.searchParams = searchParams;
        $scope.depInfo = depInfo;
        $scope.depSettings = depSettings;
        $scope.isEditBtn = isEditBtn;
        $scope.editProfilePage = editProfilePage;
        $scope.saveBtn = saveBtn;
        $scope.cancelBtn = cancelBtn;
        $scope.changed = changed;
        $scope.profilePixChanged = profilePixChanged;
        $scope.editProfilePixBtn = editProfilePixBtn;
        $scope.saveIsEnabled = saveIsEnabled;
    }

})();