module quizzClassAllCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;

        imageControl: IImageEditorControl;
        addClassroomCtrl: any;
        myQuizClasses: IQuizzClassModel[];
        enrolledQuizzClasses: IQuizzClassModel[];
        pendingClasses: IQuizzClassModel[];
        pendingInvites: IQuizzClassInviteRequestModel[];
        findQuizzroom: any;
        pendingClassesControl: any;
        pendingInvitesControl: any;

        goBack(): void;

        helpControl: any;
    }

    export interface IStateParams {
    }
}

(function () {
    l2lControllers.controller('quizzClassAllCtrl', quizzClassAllCtrl);

    quizzClassAllCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "topPanelContentSvc"];

    function quizzClassAllCtrl(
        $scope: quizzClassAllCtrl.IScope,
        $stateParams: quizzClassAllCtrl.IStateParams,
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
            numResourceToWait: 4,
            disabled: false
        };
        var imageControl: IImageEditorControl = {
        };
        var user: IUserModel = currentUser.getUserData();
        var myQuizClasses: IQuizzClassModel[] = new Array<IQuizzClassModel>();
        var enrolledQuizzClasses: IQuizzClassModel[] = new Array<IQuizzClassModel>();
        var pendingClasses: IQuizzClassModel[] = new Array<IQuizzClassModel>();
        var pendingInvites: IQuizzClassInviteRequestModel[] = new Array<IQuizzClassInviteRequestModel>();

        myQuizClasses.length

        function errorCreate(response) {
            addClassroomCtrl.vars.disabled = false;
            notificationSvc.error(str.errorCreateChat);
        }

        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successEnrolledQuizzClass(data: IQuizzClassModel[]) {
                $scope.enrolledQuizzClasses = enrolledQuizzClasses = data;
                updateIsReady();

                var count: number = 0;
                data.forEach(function (item: IQuizzClassModel) {
                    if (item.member.isNew)
                        count++;
                });

                topPanelContentSvc.subFromNewQuizzClassCount(count);
            }

            function successMyQuizzClass(data: IQuizzClassModel[]) {
                $scope.myQuizClasses = myQuizClasses = data;
                updateIsReady();
            }

            function successPendingQuizzClass(data: IQuizzClassModel[]) {
                $scope.pendingClasses = pendingClasses = data;
                updateIsReady();

                data.forEach(function (item: IQuizzClassModel) {
                    item.disabled = false;
                });
            }

            function successQuizzClassInvites(data: IQuizzClassInviteRequestModel[]) {
                $scope.pendingInvites = pendingInvites = data;
                updateIsReady();

                data.forEach(function (item: IQuizzClassInviteRequestModel) {
                    item.disabled = false;
                    item.acceptRequestItem = {
                        isReady: true
                    };
                    item.rejectRequestItem = {
                        isReady: true
                    };
                });
            }

            $anchorScroll("top");
            myQuizClasses = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClass, 0, 0, 0, 0, 0, successMyQuizzClass, errorLoad);
            enrolledQuizzClasses = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClass, 1, 0, 0, 0, 0, successEnrolledQuizzClass, errorLoad);
            pendingClasses = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClass, 3, 0, 0, 0, 0, successPendingQuizzClass, errorLoad);
            pendingInvites = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClassInviteRequest, 1, 0, 0, 0, 0, successQuizzClassInvites, errorLoad);
            util.imageEditorClear(imageControl);
        }

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        var addClassroomCtrl = (function () {
            var vars = {
                isOpen: false,
                classroomName: "",
                description: "",
                tags: "",
                membershipNeedsApproval: true,
                isCreateEnabled: false,
                disabled: false
            };

            function open() {
                vars.isOpen = true;
                util.imageEditorClear(imageControl);
                imageControl.imageUrl = "Content/images/classroom/class-default.jpg";
            }

            function close() {
                vars.disabled = false;
                vars.isOpen = false;
            }

            function changed() {
                if (vars.classroomName.trim() === "")
                    vars.isCreateEnabled = false;
                else
                    vars.isCreateEnabled = true;
            }

            function successCreate(model: IQuizzClassModel) {
                vars.isOpen = false;
                vars.disabled = false;
                myQuizClasses.unshift(model);
            }

            function create() {
                vars.disabled = true;

                var model: IQuizzClassModel = {
                    id: 0,
                    membershipNeedsApproval: vars.membershipNeedsApproval,
                    className: vars.classroomName,
                    description: vars.description,
                    tags: vars.tags
                };

                util.imagetEditorLoad(model, imageControl);

                resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClass, model, successCreate, errorCreate);
            }

            return {
                vars: vars,
                open: open,
                close: close,
                changed: changed,
                create: create
            }
        })();

        var findQuizzroom = (function () {
            var vars = {
                isOpen: false,
                message: "",
                isSaveEnabled: false,
                searchString: "",
                disabled: false,
                isJoinOpen: false,
            };

            var selected: ISearchModel = {};
            var classes: ISearchModel[] = new Array<ISearchModel>();

            function open() {
                vars.isOpen = true;
            }

            function close() {
                vars.isOpen = false;
            }

            function selectItem(item: ISearchModel) {
                //selected = item;
                selected.className = item.className;
                selected.description = item.description;
                selected.qcTags = item.qcTags;
                selected.teacherName = item.teacherName;
                selected.quizzClassId = item.quizzClassId;
                selected.imageUrl = item.imageUrl;

                vars.isSaveEnabled = true;
                vars.isOpen = false;
                vars.isJoinOpen = true;
                vars.message = "Hi " + item.teacherName + ", please add me as your learner";
            }

            function submitRequest() {
                function successCreate(model: IQuizzClassModel) {
                    vars.disabled = false;
                    vars.message = "";
                    vars.isOpen = false;
                    vars.isJoinOpen = false;
                    vars.searchString = "";
                    vars.isSaveEnabled = false;
                    
                    model.disabled = false;
                    classes.splice(0, classes.length);

                    pendingClasses.push(model);

                    notificationSvc.success(str.successRequest);
                }

                function errorSave() {
                    vars.disabled = false;
                }

                vars.disabled = true;

                var model: IQuizzClassJoinRequestModel = {
                    id: 0,
                    message: vars.message,
                    quizzClassId: selected.quizzClassId
                };

                resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassJoinRequest, model, successCreate, errorSave);
            }

            function cancelJoinRequest() {
                vars.isJoinOpen = false;
                vars.isOpen = true;
            }

            function changed() {
                if (vars.message.trim() === "")
                    vars.isSaveEnabled = false;
                else
                    vars.isSaveEnabled = true;
            }

            var searchHandle: any = undefined;
            function search() {
                
                function successSearch(data: ISearchModel[]) {
                    classes.splice(0, classes.length);

                    data.forEach(function (item: ISearchModel) {
                        //if (item.isTeacher || item.isMember || item.isRequestSent || item.isInviteSent)
                        //    return;
                        classes.push(item);
                    });
                }

                function errorSearch() {
                    notificationSvc.error(str.errorLoad);
                }

                if (searchHandle !== undefined) {
                    $timeout.cancel(searchHandle);
                    searchHandle = undefined;
                }

                searchHandle = $timeout(function () {
                    resourceSvc.getResourceManyAlt(enums.ResourceTypeEnum.QuizzClass, 1, 0, 0, vars.searchString, "", "", successSearch, errorSearch);
                }, 1000);
            }

            return {
                vars: vars,
                classes: classes,
                selected: selected,
                open: open,
                close: close,
                selectItem: selectItem,
                cancelJoinRequest: cancelJoinRequest,
                submitRequest: submitRequest,
                search: search,
                changed: changed

            }
        })();

        var pendingClassesControl = (function () {

            function errorSave() {
                notificationSvc.error(str.errorSave);
            }
 
            function cancelRequest(idx: number, item: IQuizzClassModel) {

                function successCancel() {
                    pendingClasses.splice(idx, 1);

                    notificationSvc.success(str.successCancelRequest);
                }

                dialogSvc.confirm(str.confirmCancelRequest, function (result: boolean) {
                    if (result) {
                        item.disabled = true;

                        var model: IQuizzClassJoinRequestModel = {
                            quizzClassId: item.id
                        };

                        resourceSvc.deleteResource(enums.ResourceTypeEnum.QuizzClassJoinRequest, item.id, successCancel, errorSave);
                    }
                });
            }


            return {
                cancelRequest: cancelRequest,
            }
        })();

        var pendingInvitesControl = (function () {
            var currItemProcessing: IQuizzClassInviteRequestModel;

            function errorSave() {
                currItemProcessing.disabled = false;
                currItemProcessing.acceptRequestItem.isReady = true;
                currItemProcessing.rejectRequestItem.isReady = true;

                notificationSvc.error(str.errorSave);
            }

            function accept(idx: number, item: IQuizzClassInviteRequestModel) {
                function successAccept(data: IQuizzClassModel) {

                    item.disabled = false;
                    item.acceptRequestItem.isReady = true;

                    pendingInvites.splice(idx, 1);

                    data.disabled = false;
                    enrolledQuizzClasses.unshift(data);

                    topPanelContentSvc.subFromNewQuizzClassCount(1);
                    notificationSvc.success(str.successAcceptRequest);
                }

                item.disabled = true;
                item.acceptRequestItem.isReady = false;

                item.isAccepted = true;
                resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassMemberInvite, item, successAccept, errorSave);
            }

            function reject(idx: number, item: IQuizzClassInviteRequestModel) {
                function successReject() {
                    item.disabled = false;
                    item.rejectRequestItem.isReady = true;

                    pendingInvites.splice(idx, 1);
                    topPanelContentSvc.subFromNewQuizzClassCount(1);

                    notificationSvc.success(str.successRejectRequest);
                }

                dialogSvc.confirm(str.confirmRejectRequest, function (result: boolean) {
                    if (result) {

                        item.disabled = true;
                        item.rejectRequestItem.isReady = false;

                        item.isAccepted = false;
                        resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassMemberInvite, item, successReject, errorSave);
                    }
                });
            }

            return {
                accept: accept,
                reject: reject
            }
        })();

        var helpControl = (function () {
            var vars = {
                isOpen: false
            }

            function open() {
                vars.isOpen = true;
            }

            function close() {
                vars.isOpen = false;
            }

            return {
                vars: vars,
                open: open,
                close: close
            }
        })();

        init();

        $scope.user = user;
        $scope.page = page;

        $scope.addClassroomCtrl = addClassroomCtrl;
        $scope.myQuizClasses = myQuizClasses;
        $scope.enrolledQuizzClasses = enrolledQuizzClasses;
        $scope.pendingClasses = pendingClasses;
        $scope.pendingInvites = pendingInvites;

        $scope.imageControl = imageControl;
        $scope.findQuizzroom = findQuizzroom;
        $scope.pendingClassesControl = pendingClassesControl;
        $scope.pendingInvitesControl = pendingInvitesControl;

        $scope.helpControl = helpControl;

        $scope.goBack = goBack;
    }
})();