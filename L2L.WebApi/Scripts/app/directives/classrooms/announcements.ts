module quizzClassAnnouncements {
    export interface IScope {
        qcId: number;
        isTeacher: number;

        isOwner: boolean;
        addEditAnnouncement: any;
        announcements: IQuizzClassAnnouncementModel[];
    }
}

(function () {
    l2lApp.directive("quizzClassAnnouncements", quizzClassAnnouncements);

    function quizzClassAnnouncements() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "topPanelContentSvc"];

        function controller(
            $scope: quizzClassAnnouncements.IScope,
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
            var announcements: IQuizzClassAnnouncementModel[] = new Array<IQuizzClassAnnouncementModel>();

            function errorSave(response) {
                addEditAnnouncement.vars.disabled = false;
                addEditAnnouncement.vars.disableAll = false;

                notificationSvc.error(str.errorSave);
            }

            function errorLoad(response) {
                addEditAnnouncement.isLoading = false;
                notificationSvc.error(str.errorLoad);
            }

            function init() {
                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait <= 0;
                }

                function successQuizzClassAnnouncement(data: IQuizzClassAnnouncementModel[]) {
                    $scope.announcements = announcements = data;
                    announcements.forEach(function (item: IQuizzClassAnnouncementModel) {
                        item.isOpen = false;
                    });
                    updateIsReady();
                }

                var idx: number = util.getNumber($scope.isTeacher);
                if (idx == 1)
                    addEditAnnouncement.vars.canEditAll = true;

                $scope.isOwner = util.getNumber($scope.isTeacher) === 1;

                qcId = util.getNumber($scope.qcId);
                announcements = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClassAnnouncement, qcId, 1, addEditAnnouncement.numPerPage, 0, 0, successQuizzClassAnnouncement, errorLoad);
            }

            var addEditAnnouncement = (function () {
                var pageNum: number = 2;
                var numPerPage: number = 25;
                var vars = {
                    isOpen: false,
                    message: "",
                    isSaveEnabled: false,
                    disabled: false,
                    disableAll: false,
                    canEditAll: false
                };

                function enableAll() {
                    vars.isOpen = false;
                    vars.disableAll = false;
                }

                function disableAll() {
                    vars.isOpen = true;
                    vars.disableAll = true;
                }

                function open() {
                    disableAll();
                }

                function close() {
                    enableAll();
                }

                function changed() {
                    if (vars.message.trim() === "")
                        vars.isSaveEnabled = false;
                    else
                        vars.isSaveEnabled = true;
                }

                function create() {

                    function successCreate(model: IQuizzClassAnnouncementModel) {
                        vars.isOpen = false;
                        vars.disabled = false;
                        vars.disableAll = false;
                        announcements.unshift(model);

                        vars.message = "";

                        notificationSvc.success(str.createSuccess);
                    }

                    vars.disabled = true;

                    var model: IQuizzClassAnnouncementModel = {
                        id: 0,
                        announcement: vars.message,
                        quizzClassId: qcId
                    };

                    resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassAnnouncement, model, successCreate, errorSave);
                }

                function edit(item: IQuizzClassAnnouncementModel) {
                    vars.disableAll = true;
                    item.isOpen = true;

                    vars.message = item.announcement;
                }

                function editSave(item: IQuizzClassAnnouncementModel) {

                    function successUpdate() {
                        vars.disabled = false;
                        vars.disableAll = false;

                        item.isOpen = false;
                        item.announcement = vars.message;
                        vars.message = "";
                        notificationSvc.success(str.updateSuccess);
                    }

                    item.announcement = vars.message;
                    resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzClassAnnouncement, item, successUpdate, errorSave);
                }

                function editCancel(item: IQuizzClassAnnouncementModel) {
                    item.isOpen = false;
                    vars.disableAll = false;
                }

                function remove(idx: number, item: IQuizzClassAnnouncementModel) {
                    function successRemove() {
                        announcements.splice(idx, 1);
                        notificationSvc.success(str.deleteSuccess);
                    }

                    dialogSvc.confirm(str.confirmDelete, function (result: boolean) {
                        if (result) {
                            resourceSvc.deleteResource(enums.ResourceTypeEnum.QuizzClassAnnouncement, item.id, successRemove, errorSave);
                        }
                    });
                }

                var isLoading: boolean = false;
                var noMoreItems: boolean = false;
                function loadMore() {
                    if (isLoading || noMoreItems)
                        return;

                    function successQuizzClassAnnouncement(data: IQuizzClassAnnouncementModel[]) {
                        if (data.length === 0) {
                            noMoreItems = true;
                            return;
                        }
                        data.forEach(function (item: IQuizzClassAnnouncementModel) {
                            item.isOpen = false;
                            announcements.push(item);
                        });
                        isLoading = false;
                    }

                    isLoading = true;
                    resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClassAnnouncement, qcId, pageNum, numPerPage, 0, 0, successQuizzClassAnnouncement, errorLoad);
                    pageNum++;
                }

                return {
                    numPerPage: numPerPage,
                    vars: vars,
                    open: open,
                    close: close,
                    changed: changed,
                    create: create,

                    edit: edit,
                    editSave: editSave,
                    editCancel: editCancel,

                    remove: remove,

                    loadMore: loadMore,
                    isLoading: isLoading
                }
            })();

            init();

            // announcements
            $scope.announcements = announcements;
            $scope.addEditAnnouncement = addEditAnnouncement;
        }
        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/classrooms/announcements.html",
            replace: true,
            scope: {
                qcId: "@",
                isTeacher: "@?"
            },
            controller: controller
        }
    }
})();