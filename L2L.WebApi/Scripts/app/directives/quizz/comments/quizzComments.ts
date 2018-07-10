module quizzComments {
    export interface IScope {
        page: IPage;
        $on: any;

        isSummary: any;
        quizz: IQuizzModel;
        quizzOverview: IQuizzOverviewModel;

        isSimple: boolean;
        user: IUserModel;
        comments: IQuizzCommentModel[];
        quizzCommentControl: any;
    }
}

(function () {
    l2lApp.directive("quizzComments", quizzComments);

    function quizzComments() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "quizzCommentSvc"];

        function controller(
            $scope: quizzComments.IScope,
            $state: ng.ui.IStateService,
            cachedDataSvc: ICachedDataSvc,
            notificationSvc: INotificationSvc,
            currentUser: ICurrentUser,
            $anchorScroll: ng.IAnchorScrollService,
            $location: ng.ILocationService,
            resourceSvc: IResourceSvc,
            $timeout: ng.ITimeoutService,
            dialogSvc: IDialogSvc,
            quizzCommentSvc: IQuizzCommentSvc
        ) {
            var page: IPage = {
                isReady: false,
                numResourceToWait: 1,
                disabled: false
            };
            var numGetInitial: number = 4;
            var comments: IQuizzCommentModel[] = new Array<IQuizzCommentModel>();
            var quizz: IQuizzModel;
            var quizzOverview: IQuizzOverviewModel;
            var user = $scope.user = currentUser.getUserData();

            function errorLoad() {
                notificationSvc.error(str.errorLoad);
            }

            function errorSave(response) {
                quizzCommentControl.vars.disabled = false;
                quizzCommentControl.vars.disableAll = false;

                notificationSvc.error(str.errorSave);
            }

            function updateItem(item: IQuizzCommentModel) {
                item.like = { isReady: true };
                item.flag = { isReady: true };
            }

            function init() {
                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait <= 0;
                }

                $scope.$on('$destroy', destroy);

                function successQuizzComment(data: IQuizzCommentModel[]) {
                    data.forEach(function (item: IQuizzCommentModel) {
                        updateItem(item);
                        comments.push(item);
                    });
                    updateIsReady();
                }

                $scope.isSimple = util.getNumber($scope.isSummary) === 1;
                if ($scope.isSimple)
                    numGetInitial = 2;
                quizz = $scope.quizz;
                quizzOverview = $scope.quizzOverview;
                resourceSvc.getResourceManyAlt(enums.ResourceTypeEnum.QuizzComment, 0, quizz.id, numGetInitial, "", "", "", successQuizzComment, errorLoad);
            }

            function destroy() {
            }

            var quizzCommentControl = (function () {
                var pageNum: number = 2;
                var numItemsToGet: number = 10;
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

                    function successCreate(model: IQuizzCommentModel) {
                        vars.disabled = false;
                        vars.disableAll = false;

                        updateItem(model);
                        comments.push(model);
                        quizzOverview.numComments++;

                        vars.message = "";
                        vars.isSaveEnabled = false;

                        notificationSvc.success(str.createSuccess);
                    }

                    vars.disabled = true;

                    var model: IQuizzCommentModel = {
                        id: 0,
                        comment: vars.message,
                        quizzId: quizz.id
                    };

                    resourceSvc.createResource(enums.ResourceTypeEnum.QuizzComment, model, successCreate, errorSave);
                }

                function edit(item: IQuizzCommentModel) {
                    vars.disableAll = true;
                    item.isOpen = true;

                    vars.message = item.comment;
                }

                function editSave(item: IQuizzCommentModel) {

                    function successUpdate() {
                        vars.disabled = false;
                        vars.disableAll = false;

                        item.isOpen = false;
                        item.comment = vars.message;
                        vars.message = "";
                        quizzOverview.numComments++;

                        notificationSvc.success(str.updateSuccess);
                    }

                    item.comment = vars.message;

                    quizzCommentSvc.updateComment(item, successUpdate, errorSave);
                }

                function editCancel(item: IQuizzCommentModel) {
                    item.isOpen = false;
                    vars.disableAll = false;
                    vars.message = "";
                }

                function remove(idx: number, item: IQuizzCommentModel) {
                    function successRemove() {
                        comments.splice(idx, 1);
                        quizzOverview.numComments--;

                        notificationSvc.success(str.deleteSuccess);
                    }

                    dialogSvc.confirm(str.confirmDelete, function (result: boolean) {
                        if (result) {
                            quizzCommentSvc.deleteComment(item.id, successRemove, errorSave);
                        }
                    });
                }

                var isLoading: boolean = false;
                function loadMore() {
                    if (isLoading)
                        return;

                    function successQuizzComment(data: IQuizzCommentModel[]) {
                        data.forEach(function (item: IQuizzCommentModel) {
                            item.isOpen = false;
                            updateItem(item);

                            comments.unshift(item);
                        });
                        isLoading = false;
                    }

                    var firstItem: IQuizzCommentModel = comments[0];

                    isLoading = true;
                    resourceSvc.getResourceManyAlt(enums.ResourceTypeEnum.QuizzComment, 1, quizz.id, numItemsToGet, firstItem.postedDate.toString() , "", "", successQuizzComment, errorLoad);
                    pageNum++;
                }

                function report(item: IQuizzCommentModel) {
                    if (item.isAuthor)
                        return;

                    if (item.flag.isReady == false)
                        return;

                    item.flag.isReady = false;
                    if (item.isFlagged) {
                        quizzCommentSvc.flagDownVote(item.id, function () {
                            item.isFlagged = false;
                            item.flag.isReady = true;
                            item.numFlags--;
                        }, errorSave);
                    }
                    else {
                        quizzCommentSvc.flagUpVote(item.id, function () {
                            item.isFlagged = true;
                            item.flag.isReady = true;
                            item.numFlags++;
                        }, errorSave);
                    }
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

                    report: report
                }
            })();

            init();

            $scope.page = page;
            $scope.comments = comments;
            $scope.quizzCommentControl = quizzCommentControl;
        }

        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/quizz/comments/quizzComments.html",
            replace: true,
            scope: {
                isSummary: "@?",
                quizz: "=",
                quizzOverview: "="
            },
            controller: controller
        }
    }
})();