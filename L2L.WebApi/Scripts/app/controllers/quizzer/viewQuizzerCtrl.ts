interface pixUtilInterface {
    saveProfilePix(elemId: string, canvasElemId: string, success: Function);
    renderToCanvas(canvas: any, dataUrl: any);
    saveEditorImage(fileNode, canvasNode, size, success);
}

declare var pixUtil: pixUtilInterface;

module viewQuizzerCtrl {
    export interface IScope {
        userId: number;
        view: number;
        quizzer: IQuizzerModel;
        quizzerTmp: IQuizzerModel;
        badgeDescriptions: string[];
        user: IUserModel;
        page: IPage;
        goBack(): void;
        searchParams: quizzOverviewsDirective.ISearchParam;
        editProfilePage: IPage;
        profilePix: string;
        profilePixChanged(): void;
        editProfilePixBtn: IButtonElement;

        userNameMessage: string;
        quizzmateControl: viewQuizzerCtrl.IQuizzmateControl;

        classes: IQuizzClassModel[];
        classesControl: any;

        editProfileControl: any;
    }

    export interface IStateParams {
        quizzerId?: any;
        view?: any;
    }

    export interface IQuizzmateControlVars {
        state: number;
        message: string;
    }

    export interface IQuizzmateControl {
        vars: IQuizzmateControlVars;
        page: IPage;
        init(): void;
        quizzmateBtn: IButtonElement;
        sendBtn: IButtonElement;
        cancelBtn: IButtonElement;
    }
}

(function () {
    l2lControllers.controller('viewQuizzerCtrl', viewQuizzerCtrl);

    viewQuizzerCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "accountSvc"];
    function viewQuizzerCtrl(
        $scope: viewQuizzerCtrl.IScope,
        $stateParams: viewQuizzerCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        resourceSvc: IResourceSvc,
        $timeout: ng.ITimeoutService,
        dialogSvc: IDialogSvc,
        accountSvc: IAccount
    ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 2,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var view: number = 0;
        var quizzer: IQuizzerModel = {};
        var quizzerTmp: IQuizzerModel = {
            isProfilePixModified: false,
            isModified: false,
            profile: {
                isModified: false
            }
        };
        var badgeDescriptions: string[] = new Array<string>();
        var searchParams: quizzOverviewsDirective.ISearchParam = {
            userId: $stateParams.quizzerId,
            availOnly: true
        };
        var editProfilePage: IPage = {
            disabled: false,
            isReady: true,
            numResourceToWait: 0
        }
        var editProfilePixBtn: IButtonElement = {
            click: function () {
                $("#edit-profile-pix").click();
            }
        }
        var classes: IQuizzClassModel[] = new Array<IQuizzClassModel>();
        var quizzmateControl: viewQuizzerCtrl.IQuizzmateControl = (function (): viewQuizzerCtrl.IQuizzmateControl {
            var vars: viewQuizzerCtrl.IQuizzmateControlVars = {
                state: 0,
                message: ""
            };
            var page: IPage = {
                isReady: true,
                numResourceToWait: 0,
                disabled: false
            };

            function updateButtonName() {
                var name = "";
                switch (vars.state) {
                    case 0:
                        name = "Request Quizzmate";
                        break;
                    case 1:
                        name = "Resend Quizzmate Request";
                        break;
                    case 2:
                        name = "Accept Quizzmate Request";
                        break;
                    case 3:
                        name = "Remove as Quizzmate";
                        break;
                }
                quizzmateBtn.name = name;
            }

            function init() {
                function initButtonName() {
                    var name: string = "";
                    if (quizzer.isQuizzmate)
                        vars.state = 3;
                    else if (quizzer.isFriendRequestPending)
                        vars.state = 2;
                    else if (quizzer.isFriendRequestSent)
                        vars.state = 1;
                    else
                        vars.state = 0;

                    updateButtonName();
                }

                initButtonName();
                vars.message = "Hi " + quizzer.userName + ", please add me as your Quizzmate.";
            }

            var quizzmateBtn: IButtonElement = {
                showSubControl: false,
                disabled: false,
                click: function () {
                    switch (vars.state) {
                        case 0:
                            quizzmateBtn.disabled = true;
                            quizzmateBtn.showSubControl = true;
                            break;
                        case 1: // Resend State
                            quizzmateBtn.disabled = true;
                            quizzmateBtn.showSubControl = true;
                            break;
                        case 2: // Accept Request
                            quizzer.relationshipNotification.response = 0;  // accept
                            resourceSvc.updateResource(enums.ResourceTypeEnum.RelationshipNotification, quizzer.relationshipNotification,
                                function () {
                                    notificationSvc.success(str.relationshipRequestAcceptSuccess);
                                    vars.state = 3;
                                    updateButtonName();
                                }, errorSave);
                            break;
                        case 3: // Unfriend
                            dialogSvc.confirm(str.unQuizzmateConfirm, function (result) {
                                if (result == true) {
                                    resourceSvc.deleteResource(enums.ResourceTypeEnum.Quizzmates, quizzer.id,
                                        function () {
                                            notificationSvc.success(str.unQuizzmateSuccess);
                                            vars.state = 0;
                                            updateButtonName();
                                            $state.reload();
                                        }, errorSave);
                                }
                            });
                            break;
                    }

                }
            }

            var sendBtn: IButtonElement = {
                click: function () {
                    function successQuizzmateRequest(data: IFriendRequestModel) {
                        util.enableForm(page);
                        quizzmateControl.quizzmateBtn.showSubControl = false;
                        vars.state = 1; // Resend State
                        updateButtonName();
                        notificationSvc.success(str.quizzmateRequestSendSuccess);
                    }

                    util.disableForm(page);

                    if (vars.state == 0) {
                        var model: IFriendRequestModel = {
                            requestToId: quizzer.id,
                            message: vars.message
                        }
                        resourceSvc.createResource(enums.ResourceTypeEnum.QuizzmateRequest, model, successQuizzmateRequest, errorSave);
                    }
                    else {
                        quizzer.relationshipNotification.response = 2;  // resend
                        quizzer.relationshipNotification.quizzmateRequestMessage = vars.message;
                        resourceSvc.updateResource(enums.ResourceTypeEnum.RelationshipNotification, quizzer.relationshipNotification, successQuizzmateRequest, errorSave);
                    }
                }
            }

            var cancelBtn: IButtonElement = {
                click: function () {
                    quizzmateBtn.disabled = false;
                    quizzmateBtn.showSubControl = false;
                }
            }

            return {
                vars: vars,
                page: page,
                init: init,
                quizzmateBtn: quizzmateBtn,
                sendBtn: sendBtn,
                cancelBtn: cancelBtn
            }

        })();

        function errorSave() {
            editProfilePage.isReady = true;
            editProfilePage.disabled = false;
            editProfilePage.numResourceToWait = 0;
            util.enableForm(editProfilePage);
            util.enableForm(quizzmateControl.page);
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

            function successBadgeDescriptions(data: string[]) {
                $scope.badgeDescriptions = badgeDescriptions = data;
                updateIsReady();
            }

            function successQuizzer(data: IQuizzModel) {
                updateIsReady();

                quizzer.profile.birthDate = new Date(quizzer.profile.birthDate.toString());
                quizzmateControl.init();
                badgeDescriptions = cachedDataSvc.getBadgeDescriptions(successBadgeDescriptions, errorLoad);
            }

            function successClasses(data: IQuizzClassModel[]) {
                $scope.classes = classes = data;
                classes.forEach(function (item: IQuizzClassModel) {
                    item.isOpen = false;
                    item.disabled = false;
                    item.resendRequestItem = {
                        isReady: true
                    };
                    item.cancelRequestItem = {
                        isReady: true
                    };
                });
                updateIsReady();
            }

            $anchorScroll("top");
            var quizzerId: number = util.getNumber($stateParams.quizzerId);
            quizzer = cachedDataSvc.getQuizzerInfo(quizzerId, successQuizzer, errorLoad);
            var idx = util.getNumber($stateParams.view);
            view = idx !== -1 ? idx : 0;

            if (view === 3) {
                page.numResourceToWait++;
                classes = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClass, 2, quizzerId, 0, 0, 0, successClasses, errorLoad);
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

        var classesControl = (function () {
            var vars = {
                message: "",
                isSaveEnabled: false,
                disableAll: false,
            };

            function open(item: IQuizzClassModel) {
                item.isOpen = true;
                vars.disableAll = true;
                vars.isSaveEnabled = true;
                vars.message = "Hi " + item.teacherName + ", please add me as your learner";
            }

            function close(item: IQuizzClassModel) {
                item.isOpen = false;
                vars.disableAll = false;
            }

            function changed() {
                if (vars.message.trim() === "")
                    vars.isSaveEnabled = false;
                else
                    vars.isSaveEnabled = true;
            }

            function submitRequest(item: IQuizzClassModel) {

                function successCreate(model: IQuizzClassJoinRequestModel) {
                    item.isRequestSent = true;
                    item.isOpen = false;
                    item.disabled = false;
                    vars.disableAll = false;

                    vars.message = "";

                    notificationSvc.success(str.successRequest);
                }

                vars.disableAll = true;
                item.disabled = true;

                var model: IQuizzClassJoinRequestModel = {
                    id: 0,
                    message: vars.message,
                    quizzClassId: item.id
                };

                resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassJoinRequest, model, successCreate, errorSave);
            }

            function resendRequest(item: IQuizzClassModel) {

                function successUpdate(model: IQuizzClassJoinRequestModel) {
                    item.disabled = false;
                    item.resendRequestItem.isReady = true;

                    notificationSvc.success(str.successResendRequest);
                }

                item.disabled = true;
                item.resendRequestItem.isReady = false;

                var model: IQuizzClassJoinRequestModel = {
                    quizzClassId: item.id
                };

                resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzClassJoinRequest, model, successUpdate, errorSave);
            }

            function cancelRequest(item: IQuizzClassModel) {

                function successCancel() {
                    item.disabled = false;
                    item.cancelRequestItem.isReady = true;
                    item.isRequestSent = false;

                    notificationSvc.success(str.successCancelRequest);
                }

                dialogSvc.confirm(str.confirmCancelRequest, function (result: boolean) {
                    if (result) {
                        item.disabled = true;
                        item.cancelRequestItem.isReady = false;

                        var model: IQuizzClassJoinRequestModel = {
                            quizzClassId: item.id
                        };

                        resourceSvc.deleteResource(enums.ResourceTypeEnum.QuizzClassJoinRequest, item.id, successCancel, errorSave);
                    }
                });
            }

            function leaveClass(idx: number, item: IQuizzClassModel) {

                function successLeave() {
                    item.disabled = false;
                    item.isMember = false;
                    item.isRequestSent = false;
                    vars.disableAll = false;

                    notificationSvc.success(str.successRequest);
                }

                dialogSvc.confirm(str.confirmDropOut, function (result: boolean) {
                    if (result) {
                        item.disabled = true;
                        vars.disableAll = true;

                        // this is actually a delete API
                        resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzClassMember, item, successLeave, errorSave);
                    }
                });


            }

            function edit(item: IQuizzClassAnnouncementModel) {
                vars.disableAll = true;
                item.isOpen = true;

                vars.message = item.announcement;
            }

            return {
                vars: vars,
                open: open,
                close: close,
                changed: changed,
                submitRequest: submitRequest,
                resendRequest: resendRequest,
                cancelRequest: cancelRequest,
                leaveClass: leaveClass,
            }
        })();

        var editProfileControl = (function () {
            var vars = {
                isOpen: false
            }

            function saveIsEnabled() {
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

            function resetQuizzerTmpModified() {
                quizzerTmp.isModified = false;
                quizzerTmp.profile.isModified = false;
                quizzerTmp.isProfilePixModified = false;
                quizzerTmp.profilePix = "";
            }

            function open() {
                vars.isOpen = true;
                quizzerTmp.id = quizzer.id;
                quizzerTmp.userName = quizzer.userName;
                $scope.profilePix = "";
                quizzerTmp.profile = {
                    id: quizzer.profile.id,
                    birthDate: new Date(quizzer.profile.birthDate.toString()),
                    firstName: quizzer.profile.firstName,
                    lastName: quizzer.profile.lastName,
                    profileImageUrl: quizzer.profile.profileImageUrl
                }
            }

            function close() {
                vars.isOpen = false;
                resetQuizzerTmpModified();
            }

            function changed(item: ITrackModify): void {
                item.isModified = true;

                var tmpQuizzerModel: IQuizzerModel = <IQuizzerModel>item;
                if (tmpQuizzerModel.userName !== undefined) {
                    $scope.userNameMessage = "";
                }
            }

            function save() {
                var finalWait: boolean = false;
                function updateIsReady() {
                    editProfilePage.numResourceToWait--;
                    editProfilePage.isReady = editProfilePage.numResourceToWait <= 0;
                    if (editProfilePage.isReady && finalWait) {
                        notificationSvc.success(str.updateSuccess);
                        util.enableForm(editProfilePage);
                        vars.isOpen = false;
                        currentUser.getUpdatedUserData();
                    }
                }

                function successQuizzerInfo() {
                    updateIsReady();

                    if (quizzerTmp.profile.isModified) {
                        quizzer.profile.firstName = quizzerTmp.profile.firstName;
                        quizzer.profile.lastName = quizzerTmp.profile.lastName;
                        quizzer.profile.birthDate = new Date(quizzerTmp.profile.birthDate.toString());

                        if (quizzerTmp.isProfilePixModified) {
                            var lastIdx: number = quizzer.profile.profileImageUrl.lastIndexOf("/");
                            quizzer.profile.profileImageUrl =
                                quizzer.profile.profileImageUrl.substr(0, lastIdx + 1) + quizzerTmp.profilePixName;
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

                    quizzer.userName = quizzerTmp.userName;
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
                    if (quizzerTmp.isModified && quizzerTmp.userName !== quizzer.userName)
                        accountSvc.isUserNameAvailable(quizzerTmp.userName, successCheckUserName, errorSave);
                    else
                        checkAndUpdateProfile();
                }

                if (quizzerTmp.profile.isModified || quizzerTmp.isModified) {
                    util.disableForm(editProfilePage);
                    checkAndUpdateUserName();
                }
            }

            return {
                vars: vars,
                open: open,
                close: close,
                changed: changed,
                save: save,
                saveIsEnabled: saveIsEnabled
            }
        })();

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        init();

        $scope.userNameMessage = "";
        $scope.userId = $stateParams.quizzerId;
        $scope.view = view;
        $scope.quizzer = quizzer;
        $scope.quizzerTmp = quizzerTmp;
        $scope.badgeDescriptions = badgeDescriptions;
        $scope.user = user;
        $scope.page = page;
        $scope.goBack = goBack;
        $scope.searchParams = searchParams;
        $scope.editProfilePage = editProfilePage;
        $scope.profilePixChanged = profilePixChanged;
        $scope.editProfilePixBtn = editProfilePixBtn;
        $scope.quizzmateControl = quizzmateControl;

        $scope.classes = classes;
        $scope.classesControl = classesControl;
        $scope.editProfileControl = editProfileControl;
    }
})();

