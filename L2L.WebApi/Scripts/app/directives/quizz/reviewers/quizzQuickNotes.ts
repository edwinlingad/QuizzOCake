module quizzQuickNotes {
    export interface IScope {
        $on: any;

        quizz: IQuizzModel;
        quizzOverview: IQuizzOverviewModel;

        page: IPage;
        quickNoteControl: any;
        quickNotes: IQuickNoteModel[];
        editorControl: IEditorControl;

        viewControl: any;
    }
}

(function () {
    l2lApp.directive("quizzQuickNotes", quizzQuickNotes);

    function quizzQuickNotes() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

        function controller(
            $scope: quizzQuickNotes.IScope,
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
            var quizzOverview: IQuizzOverviewModel = {};
            var quickNotes: IQuickNoteModel[] = new Array<IQuickNoteModel>();
            var editorControl: IEditorControl = {
                title: "",
                textContent: "",
                addContentType: 0,
                imageUrl: "",
                imageContent: "",
                drawingContent: undefined
            };

            function errorSave(response) {
                quickNoteControl.vars.disabled = false;
                quickNoteControl.vars.disableAll = false;
                util.editorClear(editorControl);

                notificationSvc.error(str.errorSave);
            }

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function init() {
                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait <= 0;
                }

                function successQuizzQuickNote(data: IQuickNoteModel[]) {
                    $scope.quickNotes = quickNotes = data;
                    quickNotes.forEach(function (item: IQuickNoteModel) {
                        item.isOpen = false;
                        item.isViewOpen = false;
                        item.isOpenIf = false;
                        item.isViewOpenIf = false;
                    });
                    updateIsReady();
                }

                quizzOverview = $scope.quizzOverview;
                quickNotes = resourceSvc.getResourceManyAlt(enums.ResourceTypeEnum.QuizzQuickNote, quizzOverview.reviewerId, 0, 0, "", "", "", successQuizzQuickNote, errorLoad);

                $scope.$on("$destroy", destroy);
            }

            var quickNoteControl = (function () {
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

                    function successCreate(model: IQuickNoteModel) {
                        vars.isOpen = false;
                        vars.isOpenIf = false;
                        vars.disabled = false;
                        vars.disableAll = false;

                        model.isOpen = false;
                        model.isOpenIf = false;
                        model.isViewOpen = false;
                        model.isViewOpenIf = false;

                        quickNotes.push(model);
                        util.editorClear(editorControl);

                        notificationSvc.success(str.createSuccess);
                    }

                    vars.disabled = true;

                    var model: IQuickNoteModel = {
                        id: 0,
                        reviewerId: quizzOverview.reviewerId
                    };

                    util.editorLoad(model, editorControl);

                    resourceSvc.createResource(enums.ResourceTypeEnum.QuizzQuickNote, model, successCreate, errorSave);
                }

                function edit(item: IQuickNoteModel) {
                    vars.disableAll = true;
                    item.isOpen = true;
                    item.isOpenIf = true;

                    util.editorLoad(editorControl, item, editSave, editCancel);
                    editorControl.isImageChanged = false;
                }

                function closeEditor(item: IQuickNoteModel) {
                    item.isOpen = false;
                    $timeout(function () {
                        item.isOpenIf = false;
                    }, 500);
                }

                function editSave(item: IQuickNoteModel) {
                    function successUpdate() {
                        vars.disabled = false;
                        vars.disableAll = false;

                        closeEditor(item);

                        item.title = vars.message;
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

                    var model: IQuickNoteModel = {};
                    model = util.clone(item);
                    util.editorLoad(model, editorControl);
                    model.reviewerId = quizzOverview.reviewerId;

                    resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzQuickNote, model, successUpdate, errorSave);
                }

                function editCancel(item: IQuickNoteModel) {
                    closeEditor(item);
                    vars.disableAll = false;
                }

                function remove(idx: number, item: IQuickNoteModel) {
                    function successRemove() {
                        quickNotes.splice(idx, 1);
                        notificationSvc.success(str.deleteSuccess);
                    }

                    dialogSvc.confirm(str.confirmDelete, function (result: boolean) {
                        if (result) {
                            resourceSvc.deleteResource(enums.ResourceTypeEnum.QuizzQuickNote, item.id, successRemove, errorSave);
                        }
                    });
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
                }
            })();

            var viewControl = (function () {
                function open(item: IQuickNoteModel) {
                    item.isViewOpen = true;
                    item.isViewOpenIf = true;
                    quickNoteControl.vars.disableAll = true;

                    util.editorLoad(editorControl, item, undefined, undefined, close);
                }

                function close(item: IQuickNoteModel) {
                    item.isViewOpen = false;
                    $timeout(function () {
                        item.isViewOpenIf = false;
                    }, 500);
                    quickNoteControl.vars.disableAll = false;
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
            $scope.quickNotes = quickNotes;
            $scope.quickNoteControl = quickNoteControl;
            $scope.editorControl = editorControl;
            $scope.viewControl = viewControl;
        }
        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/quizz/reviewers/quizzQuickNotes.html",
            replace: true,
            scope: {
                quizz: "=",
                quizzOverview: "="
            },
            controller: controller
        }
    }
})();