module quizzClassLessonAnnouncements {
    export interface IScope {
        $on: any;
        quizzClassLesson: IQuizzClassLessonModel;

        page: IPage;
        addEditAnnouncement: any;
        announcements: IQuizzClassLessonMessageModel[];
        editorControl: IEditorControl;

        viewControl: any;
    }
}

(function () {
    l2lApp.directive("quizzClassLessonAnnouncements", quizzClassLessonAnnouncements);

    function quizzClassLessonAnnouncements() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

        function controller(
            $scope: quizzClassLessonAnnouncements.IScope,
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
            var quizzClassLesson: IQuizzClassLessonModel = {};
            var announcements: IQuizzClassLessonMessageModel[] = new Array<IQuizzClassLessonMessageModel>();
            var editorControl: IEditorControl = {
                title: "",
                textContent: "",
                addContentType: 0,
                imageUrl: "",
                imageContent: "",
                drawingContent: undefined
            };

            function errorSave(response) {
                addEditAnnouncement.vars.disabled = false;
                addEditAnnouncement.vars.disableAll = false;
                util.editorClear(editorControl);

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

                function successQuizzClassLessonMessage(data: IQuizzClassLessonMessageModel[]) {
                    $scope.announcements = announcements = data;
                    announcements.forEach(function (item: IQuizzClassLessonMessageModel) {
                        item.isOpen = false;
                        item.isViewOpen = false;
                        item.isOpenIf = false;
                        item.isViewOpenIf = false;
                    });
                    updateIsReady();
                }

                var idx: number = util.getNumber($scope.quizzClassLesson.isTeacher);
                if (idx == 1)
                    addEditAnnouncement.vars.canEditAll = true;

                quizzClassLesson = $scope.quizzClassLesson;
                announcements = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClassLessonMessage, quizzClassLesson.id, 1, addEditAnnouncement.numPerPage, 0, 0, successQuizzClassLessonMessage, errorLoad);

                $scope.$on("$destroy", destroy);
            }

            var addEditAnnouncement = (function () {
                var pageNum: number = 2;
                var numPerPage: number = 25;
                var vars = {
                    isOpen: false,
                    isOpenIf: false,
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

                function open() {

                    vars.isOpen = true;
                    vars.isOpenIf = true;
                    vars.disableAll = true;

                    util.editorClear(editorControl);
                    editorControl.save = create;
                    editorControl.cancel = close;
                }

                function close() {
                    enableAll();

                    vars.isOpen = false;
                    vars.disableAll = false;
                    $timeout(function () {
                        vars.isOpenIf = false;
                    }, 1000);
                }

                function changed() {
                    if (vars.message.trim() === "")
                        vars.isSaveEnabled = false;
                    else
                        vars.isSaveEnabled = true;
                }

                function create() {

                    function successCreate(model: IQuizzClassLessonMessageModel) {
                        vars.isOpen = false;
                        vars.isOpenIf = false;
                        vars.disabled = false;
                        vars.disableAll = false;

                        model.isOpen = false;
                        model.isOpenIf = false;
                        model.isViewOpen = false;
                        model.isViewOpenIf = false;

                        announcements.push(model);
                        util.editorClear(editorControl);

                        notificationSvc.success(str.createSuccess);
                    }

                    vars.disabled = true;

                    var model: IQuizzClassLessonMessageModel = {
                        id: 0,
                        quizzClassLessonId: quizzClassLesson.id,
                        quizzClassId: quizzClassLesson.quizzClassId
                    };

                    util.editorLoad(model, editorControl);

                    resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassLessonMessage, model, successCreate, errorSave);
                }

                function edit(item: IQuizzClassLessonMessageModel) {
                    vars.disableAll = true;
                    item.isOpen = true;
                    item.isOpenIf = true;

                    util.editorLoad(editorControl, item, editSave, editCancel);
                    editorControl.isImageChanged = false;
                }

                function closeEditor(item: IQuizzClassLessonMessageModel) {
                    item.isOpen = false;
                    $timeout(function () {
                        item.isOpenIf = false;
                    }, 500);
                }

                function editSave(item: IQuizzClassLessonMessageModel) {
                    function successUpdate() {
                        vars.disabled = false;
                        vars.disableAll = false;

                        closeEditor(item);

                        item.message = vars.message;
                        vars.message = "";

                        util.editorLoad(item, editorControl);

                        if (editorControl.isImageChanged) {
                            var newFileName = editorControl.newImageFileName;
                            var oldImageUrl = item.imageUrl;
                            util.editorLoad(item, editorControl);

                            var lastIdx: number = oldImageUrl.lastIndexOf("/");
                            item.imageUrl =
                                oldImageUrl.substr(0, lastIdx + 1) + newFileName;
                        }

                        util.editorClear(editorControl);
                        notificationSvc.success(str.updateSuccess);
                    }

                    var model: IQuizzClassLessonMessageModel = {};
                    model = util.clone(item);
                    util.editorLoad(model, editorControl);
                    model.quizzClassId = quizzClassLesson.quizzClassId;
                    
                    resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzClassLessonMessage, model, successUpdate, errorSave);
                }

                function editCancel(item: IQuizzClassLessonMessageModel) {
                    closeEditor(item);
                    vars.disableAll = false;
                }

                function remove(idx: number, item: IQuizzClassLessonMessageModel) {
                    function successRemove() {
                        announcements.splice(idx, 1);
                        notificationSvc.success(str.deleteSuccess);
                    }

                    dialogSvc.confirm(str.confirmDelete, function (result: boolean) {
                        if (result) {
                            resourceSvc.deleteResource(enums.ResourceTypeEnum.QuizzClassLessonMessage, item.id, successRemove, errorSave);
                        }
                    });
                }

                var isLoading: boolean = false;
                var noMoreItems: boolean = false;
                function loadMore() {
                    if (isLoading || noMoreItems)
                        return;

                    function successQuizzClassLessonMessage(data: IQuizzClassLessonMessageModel[]) {
                        if (data.length === 0) {
                            noMoreItems = true;
                            return;
                        }
                        data.forEach(function (item: IQuizzClassLessonMessageModel) {
                            item.isOpen = false;
                            announcements.push(item);
                        });
                        isLoading = false;
                    }

                    isLoading = true;
                    resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClassLessonMessage, quizzClassLesson.id, pageNum, numPerPage, 0, 0, successQuizzClassLessonMessage, errorLoad);
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

            var viewControl = (function () {
                function open(item: IQuizzClassLessonMessageModel) {
                    item.isViewOpen = true;
                    item.isViewOpenIf = true;
                    addEditAnnouncement.vars.disableAll = true;

                    util.editorLoad(editorControl, item, undefined, undefined, close);
                }

                function close(item: IQuizzClassLessonMessageModel) {
                    item.isViewOpen = false;
                    $timeout(function () {
                        item.isViewOpenIf = false;
                    }, 500);
                    addEditAnnouncement.vars.disableAll = false;
                }

                return {
                    open: open
                }
            })();

            function destroy() {
            }

            init();

            $scope.page = page;

            // announcements
            $scope.announcements = announcements;
            $scope.addEditAnnouncement = addEditAnnouncement;
            $scope.editorControl = editorControl;
            $scope.viewControl = viewControl;
        }
        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/classrooms/lessons/LessonAnnouncements.html",
            replace: true,
            scope: {
                quizzClassLesson: "="
            },
            controller: controller
        }
    }
})();