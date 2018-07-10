module quizzClassLessons {
    export interface IScope {
        qcId: number;
        isTeacher: number;
        isViewer: number;
        depId: number;
        depName: string;

        page: IPage;
        isOwner: boolean;
        isViewOnly: boolean;
        lessonsCtrl: any;
        lessons: IQuizzClassLessonModel[];
    }
}

(function () {
    l2lApp.directive("quizzClassLessons", quizzClassLessons);

    function quizzClassLessons() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

        function controller(
            $scope: quizzClassLessons.IScope,
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
            var qcId: number = 0;
            var lessons: IQuizzClassLessonModel[] = new Array<IQuizzClassLessonModel>();

            function errorSave(response) {
                lessonsCtrl.vars.disabled = false;
                lessonsCtrl.vars.disableAll = false;

                notificationSvc.error(str.errorSave);
            }

            function errorLoad(response) {
                lessonsCtrl.isLoading = false;
                notificationSvc.error(str.errorLoad);
            }

            function init() {
                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait <= 0;
                }

                function successQuizzClassLesson(data: IQuizzClassLessonModel[]) {
                    $scope.lessons = lessons = data;
                    lessons.forEach(function (item: IQuizzClassLessonModel) {
                        item.isOpen = false;
                    });
                    updateIsReady();
                }

                $scope.isViewOnly = util.getNumber($scope.isViewer) === 1;

                if ($scope.isViewOnly) {
                    $scope.isOwner = false;
                }
                else {
                    $scope.isOwner = util.getNumber($scope.isTeacher) === 1;
                }

                lessonsCtrl.vars.canEditAll = $scope.isOwner;
                $scope.depId = util.getNumber($scope.depId);
                qcId = util.getNumber($scope.qcId);
                lessons = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClassLesson, qcId, 1, lessonsCtrl.numPerPage, $scope.depId, 0, successQuizzClassLesson, errorLoad);
            }

            var lessonsCtrl = (function () {
                var pageNum: number = 2;
                var numPerPage: number = 25;
                var vars = {
                    isOpen: false,
                    title: "",
                    description: "",
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
                    if (vars.title.trim() === "")
                        vars.isSaveEnabled = false;
                    else
                        vars.isSaveEnabled = true;
                }

                function create() {

                    function successCreate(model: IQuizzClassLessonModel) {
                        vars.isOpen = false;
                        vars.disabled = false;
                        vars.disableAll = false;
                        lessons.push(model);

                        vars.title = "";

                        notificationSvc.success(str.createSuccess);
                    }

                    vars.disabled = true;

                    var model: IQuizzClassLessonModel = {
                        id: 0,
                        title: vars.title,
                        description: vars.description,
                        quizzClassId: qcId
                    };

                    resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassLesson, model, successCreate, errorSave);
                }

                function edit(item: IQuizzClassLessonModel) {
                    vars.disableAll = true;
                    item.isOpen = true;

                    vars.title = item.title;
                    vars.description = item.description;
                }

                function editSave(item: IQuizzClassLessonModel) {

                    function successUpdate() {
                        vars.disabled = false;
                        vars.disableAll = false;

                        item.isOpen = false;
                        item.title = vars.title;
                        item.description = vars.description;

                        vars.title = "";
                        vars.description = "";
                        notificationSvc.success(str.updateSuccess);
                    }

                    var model: IQuizzClassLessonModel = {};
                    util.copy(model, item);

                    model.title = vars.title;
                    model.description = vars.description;

                    resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzClassLesson, model, successUpdate, errorSave);
                }

                function editCancel(item: IQuizzClassLessonModel) {
                    item.isOpen = false;
                    vars.disableAll = false;
                }

                function remove(idx: number, item: IQuizzClassLessonModel) {
                    function successRemove() {
                        lessons.splice(idx, 1);
                        notificationSvc.success(str.deleteSuccess);
                    }

                    dialogSvc.confirm(str.confirmDelete, function (result: boolean) {
                        if (result) {
                            resourceSvc.deleteResource(enums.ResourceTypeEnum.QuizzClassLesson, item.id, successRemove, errorSave);
                        }
                    });
                }

                var isLoading: boolean = false;
                var noMoreItems: boolean = false;
                function loadMore() {
                    if (isLoading || noMoreItems)
                        return;

                    function successQuizzClassLesson(data: IQuizzClassLessonModel[]) {
                        if (data.length === 0) {
                            noMoreItems = true;
                            return;
                        }
                        data.forEach(function (item: IQuizzClassLessonModel) {
                            item.isOpen = false;
                            lessons.push(item);
                        });
                        isLoading = false;
                    }

                    isLoading = true;
                    resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClassLesson, qcId, pageNum, numPerPage, $scope.depId, 0, successQuizzClassLesson, errorLoad);
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

            $scope.page = page;
            // announcements
            $scope.lessons = lessons;
            $scope.lessonsCtrl = lessonsCtrl;
        }
        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/classrooms/lessons.html",
            replace: true,
            scope: {
                qcId: "@",
                isTeacher: "@?",
                isViewer: "@?",
                depId: "@?",
                depName: "@?"
            },
            controller: controller
        }
    }
})();