module quizzClassLessonCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        goBack(): void;

        depId: number;
        depName: string;
        view: number;

        lesson: IQuizzClassLessonModel;
        lessonsCtrl: any;
    }

    export interface IStateParams {
        qclId: number;
        depId: number;
        depName: string;
        view: number;
    }
}

(function () {
    l2lControllers.controller('quizzClassLessonCtrl', quizzClassLessonCtrl);

    quizzClassLessonCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "topPanelContentSvc"];

    function quizzClassLessonCtrl(
        $scope: quizzClassLessonCtrl.IScope,
        $stateParams: quizzClassLessonCtrl.IStateParams,
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
        var user: IUserModel = currentUser.getUserData();
        var view: number = 0;
        var lesson: IQuizzClassLessonModel = {};

        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successLesson(data: IQuizzClassLessonModel) {
                $scope.lesson = lesson = data;
                lesson.isOpen = false;
                updateIsReady();

                page.title = lesson.quizzClassName;

                switch (view) {
                    case 0:
                        topPanelContentSvc.subFromNewQuizzClassCount(lesson.newMessageCount);
                        break;
                    case 2:
                        topPanelContentSvc.subFromNewQuizzClassCount(lesson.newCommentCount);
                        break;
                    case 3:
                        topPanelContentSvc.subFromNewQuizzClassCount(lesson.newQuizzCount);
                        break;            
                }
            }

            $anchorScroll("top");
            $scope.depName = $stateParams.depName;
            $scope.depId = util.getNumber($stateParams.depId);
            view = util.getNumber($stateParams.view);
            var cqlId: number = util.getNumber($stateParams.qclId);
            lesson = resourceSvc.getResource(enums.ResourceTypeEnum.QuizzClassLesson, cqlId, successLesson, errorLoad);
        }

        function errorSave(response) {
            lessonsCtrl.vars.disabled = false;
            lessonsCtrl.vars.disableAll = false;

            notificationSvc.error(str.errorSave);
        }

        var lessonsCtrl = (function () {
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

            function remove(item: IQuizzClassLessonModel) {
                function successRemove() {
                    notificationSvc.success(str.deleteSuccess);
                    if (lesson.isTeacher)
                        $state.go("si.myQuizzClass", { qcId: lesson.quizzClassId, view : 3});
                    else
                        $state.go("si.enrolledQuizzClass", { qcId: lesson.quizzClassId, view: 3 });
                }

                dialogSvc.confirm(str.confirmDelete, function (result: boolean) {
                    if (result) {
                        resourceSvc.deleteResource(enums.ResourceTypeEnum.QuizzClassLesson, item.id, successRemove, errorSave);
                    }
                });
            }

            return {
                vars: vars,
                open: open,
                close: close,
                changed: changed,

                edit: edit,
                editSave: editSave,
                editCancel: editCancel,

                remove: remove,
            }
        })();

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.goBack = goBack;

        $scope.view = view;
        $scope.lesson = lesson;
        $scope.lessonsCtrl = lessonsCtrl;
    }
})();