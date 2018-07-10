module quizzClassLessonDiscussions {
    export interface IScope {
        user: IUserModel;
        depId: number;

        page: IPage;
        isReadyOnly: boolean;
        quizzClassLesson: IQuizzClassLessonModel;
        lessonCommentCtrl: any;
        lessonComments: IQuizzClassLessonCommentModel[];
    }
}

(function () {
    l2lApp.directive("quizzClassLessonDiscussions", quizzClassLessonDiscussions);

    function quizzClassLessonDiscussions() {

        controller.$inject = ["$scope", "$state", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

        function controller(
            $scope: quizzClassLessonDiscussions.IScope,
            $state: ng.ui.IStateService,
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
            var user: IUserModel = currentUser.getUserData();
            var quizzClassLesson: IQuizzClassLessonModel = {};

            function errorSave(response) {
                lessonCommentCtrl.vars.disabled = false;
                lessonCommentCtrl.vars.disableAll = false;

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

                function successClassComments(data: IQuizzClassLessonCommentModel[]) {
                    data.forEach(function (item: IQuizzClassLessonCommentModel) {
                        item.isOpen = false;
                        lessonComments.push(item);
                    });
                    updateIsReady();
                }

                $scope.depId = util.getNumber($scope.depId);
                $scope.isReadyOnly = $scope.depId !== -1;
                quizzClassLesson = $scope.quizzClassLesson;
                if (quizzClassLesson.isTeacher)
                    lessonCommentCtrl.vars.canEditAll = true;
                resourceSvc.getResourceManyAlt(enums.ResourceTypeEnum.QuizzClassLessonComment, 0, quizzClassLesson.id, 4, "", "", "", successClassComments, errorLoad);
            }

            var lessonComments: IQuizzClassLessonCommentModel[] = new Array<IQuizzClassLessonCommentModel>();
            var lessonCommentCtrl = (function () {
                var numItemsToGet: number = 25;
                var vars = {
                    message: "",
                    isSaveEnabled: false,
                    disabled: false,
                    disableAll: false,
                    canEditAll: false
                };

                function enableAll() {
                    vars.disableAll = false;
                }

                function disableAll() {
                    vars.disableAll = true;
                }

                function changed() {
                    if (vars.message.trim() === "")
                        vars.isSaveEnabled = false;
                    else
                        vars.isSaveEnabled = true;
                }

                function create() {

                    function successCreate(model: IQuizzClassLessonCommentModel) {
                        vars.disabled = false;
                        vars.disableAll = false;
                        lessonComments.push(model);

                        vars.message = "";
                        vars.isSaveEnabled = false;
                        quizzClassLesson.numComments++;

                        notificationSvc.success(str.createSuccess);
                    }

                    vars.disabled = true;

                    var model: IQuizzClassLessonCommentModel = {
                        id: 0,
                        comment: vars.message,
                        quizzClassLessonId: quizzClassLesson.id
                    };

                    resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassLessonComment, model, successCreate, errorSave);
                }

                function edit(item: IQuizzClassLessonCommentModel) {
                    vars.disableAll = true;
                    item.isOpen = true;

                    vars.message = item.comment;
                }

                function editSave(item: IQuizzClassLessonCommentModel) {

                    function successUpdate() {
                        vars.disabled = false;
                        vars.disableAll = false;

                        item.isOpen = false;
                        item.comment = vars.message;
                        vars.message = "";
                        notificationSvc.success(str.updateSuccess);
                    }

                    item.comment = vars.message;
                    resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzClassLessonComment, item, successUpdate, errorSave);
                }

                function editCancel(item: IQuizzClassLessonCommentModel) {
                    item.isOpen = false;
                    vars.message = "";
                    vars.disableAll = false;
                }

                function remove(idx: number, item: IQuizzClassLessonCommentModel) {
                    function successRemove() {
                        lessonComments.splice(idx, 1);
                        quizzClassLesson.numComments--;
                        notificationSvc.success(str.deleteSuccess);
                    }

                    dialogSvc.confirm(str.confirmDelete, function (result: boolean) {
                        if (result) {
                            resourceSvc.deleteResource(enums.ResourceTypeEnum.QuizzClassLessonComment, item.id, successRemove, errorSave);
                        }
                    });
                }

                var isLoading: boolean = false;
                function loadMore() {
                    if (isLoading)
                        return;

                    function successQuizzClassAnnouncement(data: IQuizzClassLessonCommentModel[]) {
                        data.forEach(function (item: IQuizzClassLessonCommentModel) {
                            item.isOpen = false;
                            lessonComments.unshift(item);
                        });
                        isLoading = false;
                    }

                    var firstItem: IQuizzClassCommentModel = lessonComments[0];

                    isLoading = true;
                    resourceSvc.getResourceManyAlt(enums.ResourceTypeEnum.QuizzClassLessonComment, 1, quizzClassLesson.id, numItemsToGet, firstItem.postedDate.toString(), "", "", successQuizzClassAnnouncement, errorLoad);
                }

                return {
                    vars: vars,
                    changed: changed,
                    create: create,

                    edit: edit,
                    editSave: editSave,
                    editCancel: editCancel,

                    remove: remove,

                    loadMore: loadMore,
                    isLoading: isLoading,
                }
            })();

            init();

            $scope.page = page;
            $scope.user = user;
            $scope.lessonComments = lessonComments;
            $scope.lessonCommentCtrl = lessonCommentCtrl;
        }
        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/classrooms/lessons/lessonDiscussions.html",
            replace: true,
            scope: {
                quizzClassLesson: "=",
                depId: "@"
            },
            controller: controller
        }
    }
})();