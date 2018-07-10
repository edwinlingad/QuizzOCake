module quizzClassDiscussions {
    export interface IScope {
        quizzClass: IQuizzClassModel;
        depId: number;
        user: IUserModel;

        isReadyOnly: boolean;
        classCommentCtrl: any;
        classComments: IQuizzClassCommentModel[];
    }
}

(function () {
    l2lApp.directive("quizzClassDiscussions", quizzClassDiscussions);

    function quizzClassDiscussions() {

        controller.$inject = ["$scope", "$state", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

        function controller(
            $scope: quizzClassDiscussions.IScope,
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
            var quizzClass: IQuizzClassModel = $scope.quizzClass;

            function errorSave(response) {
                classCommentCtrl.vars.disabled = false;
                classCommentCtrl.vars.disableAll = false;

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

                function successClassComments(data: IQuizzClassCommentModel[]) {
                    data.forEach(function (item: IQuizzClassCommentModel) {
                        item.isOpen = false;
                        classComments.push(item);
                    });
                    updateIsReady();
                }

                $scope.depId = util.getNumber($scope.depId);
                $scope.isReadyOnly = $scope.depId !== -1;
                var depIdStr = $scope.depId === -1 ? "" : $scope.depId.toString();
                resourceSvc.getResourceManyAlt(enums.ResourceTypeEnum.QuizzClassComment, 0, quizzClass.id, 4, depIdStr, "", "", successClassComments, errorLoad);
            }

            var classComments: IQuizzClassCommentModel[] = new Array<IQuizzClassCommentModel>();
            var classCommentCtrl = (function () {
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

                    function successCreate(model: IQuizzClassCommentModel) {
                        vars.disabled = false;
                        vars.disableAll = false;
                        classComments.push(model);

                        vars.message = "";
                        vars.isSaveEnabled = false;
                        quizzClass.numComments++;

                        notificationSvc.success(str.createSuccess);
                    }

                    vars.disabled = true;

                    var model: IQuizzClassCommentModel = {
                        id: 0,
                        comment: vars.message,
                        quizzClassId: quizzClass.id
                    };

                    resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassComment, model, successCreate, errorSave);
                }

                function edit(item: IQuizzClassCommentModel) {
                    vars.disableAll = true;
                    item.isOpen = true;

                    vars.message = item.comment;
                }

                function editSave(item: IQuizzClassCommentModel) {

                    function successUpdate() {
                        vars.disabled = false;
                        vars.disableAll = false;

                        item.isOpen = false;
                        item.comment = vars.message;
                        vars.message = "";
                        notificationSvc.success(str.updateSuccess);
                    }

                    item.comment = vars.message;
                    resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzClassComment, item, successUpdate, errorSave);
                }

                function editCancel(item: IQuizzClassCommentModel) {
                    item.isOpen = false;
                    vars.disableAll = false;
                    vars.message = "";
                }

                function remove(idx: number, item: IQuizzClassCommentModel) {
                    function successRemove() {
                        classComments.splice(idx, 1);
                        quizzClass.numComments--;

                        notificationSvc.success(str.deleteSuccess);
                    }

                    dialogSvc.confirm(str.confirmDelete, function (result: boolean) {
                        if (result) {
                            resourceSvc.deleteResource(enums.ResourceTypeEnum.QuizzClassComment, item.id, successRemove, errorSave);
                        }
                    });
                }

                var isLoading: boolean = false;
                function loadMore() {
                    if (isLoading)
                        return;

                    function successQuizzClassAnnouncement(data: IQuizzClassCommentModel[]) {
                        data.forEach(function (item: IQuizzClassCommentModel) {
                            item.isOpen = false;
                            classComments.unshift(item);
                        });
                        isLoading = false;
                    }

                    var firstItem: IQuizzClassCommentModel = classComments[0];

                    isLoading = true;
                    resourceSvc.getResourceManyAlt(enums.ResourceTypeEnum.QuizzClassComment, 1, quizzClass.id, numItemsToGet, firstItem.postedDate.toString(), "", "", successQuizzClassAnnouncement, errorLoad);
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

            $scope.user = user;
            $scope.classComments = classComments;
            $scope.classCommentCtrl = classCommentCtrl;
        }
        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/classrooms/discussions.html",
            replace: true,
            scope: {
                quizzClass: "=",
                depId: "@"
            },
            controller: controller
        }
    }
})();