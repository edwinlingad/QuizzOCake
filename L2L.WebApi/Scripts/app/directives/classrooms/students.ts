module quizzClassStudents {
    export interface IScope {
        // directive parameter
        quizzClass: IQuizzClassModel;
        isTeacher: number;

        page: IPage;
        isOwner: boolean;
        pendingInvites: IQuizzClassInviteRequestModel[];
        pendingRequests: IQuizzClassJoinRequestModel[];
        pendingRequestControl: any;

        students: IQuizzClassMemberModel[];
        studentControl: any;

        findLearnersControl: any;
        pendingInviteControl: any;
    }
}

(function () {
    l2lApp.directive("quizzClassStudents", quizzClassStudents);

    function quizzClassStudents() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "topPanelContentSvc"];

        function controller(
            $scope: quizzClassStudents.IScope,
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
                numResourceToWait: 1,
                disabled: false
            };
            var qcId: number = 0;
            var quizzClass: IQuizzClassModel;
            var isOwner: boolean = false;
            var pendingInvites: IQuizzClassInviteRequestModel[] = new Array<IQuizzClassInviteRequestModel>();
            var pendingRequests: IQuizzClassJoinRequestModel[] = new Array<IQuizzClassJoinRequestModel>();
            var students: IQuizzClassMemberModel[] = new Array<IQuizzClassMemberModel>();

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function init() {
                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait <= 0;
                }

                function successStudents(data: IQuizzClassMemberModel[]) {
                    $scope.students = students = data;
                    data.forEach(function (item: IQuizzClassMemberModel) {
                        item.disabled = false;
                        item.dropStudentItem = {
                            isReady: true
                        };
                    });
                    updateIsReady();
                }

                function successJoinRequest(data: IQuizzClassJoinRequestModel[]) {
                    $scope.pendingRequests = pendingRequests = data;
                    data.forEach(function (item: IQuizzClassJoinRequestModel) {
                        item.disabled = false;
                        item.acceptRequestItem = {
                            isReady: true
                        };
                        item.rejectRequestItem = {
                            isReady: true
                        };
                    });
                    updateIsReady();
                }

                function successInvites(data: IQuizzClassJoinRequestModel[]) {
                    $scope.pendingInvites = pendingInvites = data;
                    data.forEach(function (item: IQuizzClassJoinRequestModel) {
                        item.disabled = false;
                    });
                    updateIsReady();
                }

                quizzClass = $scope.quizzClass;

                var idx: number = util.getNumber($scope.isTeacher);
                $scope.isOwner = isOwner = idx == 1;
                qcId = $scope.quizzClass.id;

                if (isOwner) {
                    page.numResourceToWait += 2;
                    pendingRequests = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClassJoinRequest, qcId, 0, 0, 0, 0, successJoinRequest, errorLoad);
                    pendingInvites = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClassInviteRequest, 0, qcId, 0, 0, 0, successInvites, errorLoad);

                    topPanelContentSvc.subFromNewQuizzClassCount(quizzClass.numNewInviteAccepts);
                }

                var qType = isOwner ? 1 : 0;
                students = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClassMember, qcId, qType, 0, 0, 0, successStudents, errorLoad);
            }

            var pendingRequestControl = (function () {
                var currItemProcessing: IQuizzClassJoinRequestModel;

                function errorSave() {
                    currItemProcessing.disabled = false;
                    currItemProcessing.acceptRequestItem.isReady = true;
                    currItemProcessing.rejectRequestItem.isReady = true;

                    notificationSvc.error(str.errorSave);
                }

                function accept(idx: number, item: IQuizzClassJoinRequestModel) {
                    function successAccept(data: IQuizzClassMemberModel) {

                        quizzClass.numStudentJoinRequests--;

                        item.disabled = false;
                        item.acceptRequestItem.isReady = true;

                        pendingRequests.splice(idx, 1);

                        data.dropStudentItem = {
                            isReady: true
                        };
                        data.disabled = false;
                        students.unshift(data);
                        topPanelContentSvc.subFromNewQuizzClassCount(1);
                        
                        notificationSvc.success(str.successAcceptRequest);
                    }

                    currItemProcessing = item;
                    item.disabled = true;
                    item.acceptRequestItem.isReady = false;

                    item.isAccepted = true;
                    resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassMember, item, successAccept, errorSave);
                }

                function reject(idx: number, item: IQuizzClassJoinRequestModel) {
                    function successReject() {
                        quizzClass.numStudentJoinRequests--;

                        item.disabled = false;
                        item.rejectRequestItem.isReady = true;

                        pendingRequests.splice(idx, 1);
                        topPanelContentSvc.subFromNewQuizzClassCount(1);

                        notificationSvc.success(str.successRejectRequest);
                    }

                    dialogSvc.confirm(str.confirmRejectRequest, function (result: boolean) {
                        if (result) {
                            
                            item.disabled = true;
                            item.rejectRequestItem.isReady = false;

                            item.isAccepted = false;
                            resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassMember, item, successReject, errorSave);
                        }
                    });
                }

                return {
                    accept: accept,
                    reject: reject
                }
            })();

            var studentControl = (function () {
                var vars = {
                    disableAll: false
                }
                var curItem: IQuizzClassMemberModel;

                function errorSave() {
                    curItem.disabled = false;
                    curItem.dropStudentItem.isReady = true;
                    vars.disableAll = false;

                    notificationSvc.error(str.errorSave);
                }

                function dropStudent(idx: number, item: IQuizzClassMemberModel) {
                    function successDropStudent() {
                        curItem.disabled = false;
                        curItem.dropStudentItem.isReady = true;
                        vars.disableAll = false;

                        students.splice(idx, 1);

                        notificationSvc.success(str.successDropStudent);
                    }

                    dialogSvc.confirm(str.confirmDropStudent, function (result: boolean) {

                        if (result) {
                            curItem = item;
                            item.disabled = true;
                            item.dropStudentItem.isReady = false;
                            vars.disableAll = true;

                            resourceSvc.deleteResource(enums.ResourceTypeEnum.QuizzClassMember, item.id, successDropStudent, errorSave);
                        }
                    });
                }

                return {
                    vars: vars,
                    dropStudent: dropStudent
                }
            })();

            var findLearnersControl = (function () {
                var vars = {
                    isOpen: false,
                    message: "",
                    isSaveEnabled: false,
                    searchString: "",
                    disabled: false,
                    isJoinOpen: false,
                };

                var selected: ISearchModel = {};
                var learners: ISearchModel[] = new Array<ISearchModel>();

                function open() {
                    vars.isOpen = true;
                }

                function close() {
                    vars.isOpen = false;
                }

                function selectItem(item: ISearchModel) {
                    //selected = item;
                    selected.userId = item.userId;
                    selected.userDisplayName = item.userDisplayName;
                    selected.profileImageUrl = item.profileImageUrl;

                    vars.isSaveEnabled = true;
                    vars.isOpen = false;
                    vars.isJoinOpen = true;
                    vars.message = "Hi " + item.userDisplayName + ", please join my Quizzroom";
                }

                function submitRequest() {
                    function successCreate(model: IQuizzClassInviteRequestModel) {
                        vars.disabled = false;
                        vars.message = "";
                        vars.isOpen = false;
                        vars.isJoinOpen = false;
                        vars.searchString = "";
                        vars.isSaveEnabled = false;

                        model.disabled = false;
                        learners.splice(0, learners.length);

                        pendingInvites.push(model);

                        notificationSvc.success(str.successRequest);
                    }

                    function errorSave() {
                        vars.disabled = false;
                    }

                    vars.disabled = true;

                    var model: IQuizzClassInviteRequestModel = {
                        id: 0,
                        message: vars.message,
                        quizzClassId: quizzClass.id,
                        userId: selected.userId
                    };

                    resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassInviteRequest, model, successCreate, errorSave);
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
                        learners.splice(0, learners.length);

                        data.forEach(function (item: ISearchModel) {
                            if (item.isTeacher || item.isMember || item.isRequestSent)
                                return;
                            learners.push(item);
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
                        resourceSvc.getResourceManyAlt(enums.ResourceTypeEnum.QuizzClassInviteRequest, quizzClass.id, 0, 0, vars.searchString, "", "", successSearch, errorSearch);
                    }, 1000);
                }

                return {
                    vars: vars,
                    learners: learners,
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

            var pendingInviteControl = (function () {

                function errorSave() {
                    notificationSvc.error(str.errorSave);
                }

                function cancelRequest(idx: number, item: IQuizzClassInviteRequestModel) {

                    function successCancel() {
                        pendingInvites.splice(idx, 1);

                        notificationSvc.success(str.successCancelRequest);
                    }

                    dialogSvc.confirm(str.confirmCancelRequest, function (result: boolean) {
                        if (result) {
                            item.disabled = true;

                            var model: IQuizzClassInviteRequestModel = {
                                quizzClassId: item.id
                            };

                            resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzClassInviteRequest, item, successCancel, errorSave);
                        }
                    });
                }

                return {
                    cancelRequest: cancelRequest,
                }
            })();

            init();

            $scope.page = page;
            $scope.isOwner = isOwner;

            $scope.pendingInvites = pendingInvites;
            $scope.pendingRequests = pendingRequests;
            $scope.pendingRequestControl = pendingRequestControl;

            $scope.students = students;
            $scope.studentControl = studentControl;

            $scope.findLearnersControl = findLearnersControl;
            $scope.pendingInviteControl = pendingInviteControl;
        }
        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/classrooms/students.html",
            replace: true,
            scope: {
                qcId: "@",
                quizzClass: "=",
                isTeacher: "@?"
            },
            controller: controller
        }
    }
})();