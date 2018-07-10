module quizzClassLessonQuizzes {
    export interface IScope {
        quizzClassLesson: IQuizzClassLessonModel;

        page: IPage;
        $on: any;

        assignedQuizzes: IQuizzOverviewModel[];
        findQuizzesControl: any;

        remove(idx: number, item: IQuizzOverviewModel);
    }
}

(function () {
    l2lApp.directive("quizzClassLessonQuizzes", quizzClassLessonQuizzes);

    function quizzClassLessonQuizzes() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

        function controller(
            $scope: quizzClassLessonQuizzes.IScope,
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
            var assignedQuizzes: IQuizzOverviewModel[] = new Array<IQuizzOverviewModel>();

            function init() {
                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait <= 0;
                }

                function errorLoad() {
                    notificationSvc.error(str.errorLoad);
                }

                function successQuizzes(data: IQuizzOverviewModel[]) {
                    $scope.assignedQuizzes = assignedQuizzes = data;
                    updateIsReady();
                }

                $scope.$on('$destroy', destroy);
                quizzClassLesson = $scope.quizzClassLesson;

                assignedQuizzes = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClassLessonQuizz, quizzClassLesson.id, 0, 0, 0, 0, successQuizzes, errorLoad);
            }

            function destroy() {
            }

            var findQuizzesControl = (function () {
                var vars = {
                    isOpen: false,
                    searchString: "",
                    disabled: false,
                };

                var quizzes: ISearchModel[] = new Array<ISearchModel>();

                function open() {
                    vars.isOpen = true;
                }

                function close() {
                    vars.isOpen = false;
                }

                function submitRequest(item: ISearchModel) {
                    function successCreate(model: IQuizzOverviewModel) {
                        vars.disabled = false;
                        vars.isOpen = false;
                        vars.searchString = "";

                        model.disabled = false;
                        quizzes.splice(0, quizzes.length);

                        assignedQuizzes.push(model);

                        notificationSvc.success(str.successRequest);
                    }

                    function errorSave() {
                        vars.disabled = false;
                    }

                    vars.disabled = true;

                    var model: IQuizzClassLessonQuizzModel = {
                        id: 0,
                        quizzClassId: quizzClassLesson.quizzClassId,
                        quizzClassLessonId: quizzClassLesson.id,
                        quizzId: item.quizzId
                    };

                    resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassLessonQuizz, model, successCreate, errorSave);
                }

                var searchHandle: any = undefined;
                function search() {

                    function successSearch(data: ISearchModel[]) {
                        quizzes.splice(0, quizzes.length);

                        data.forEach(function (item: ISearchModel) {
                            quizzes.push(item);
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
                        resourceSvc.getResourceManyAlt(enums.ResourceTypeEnum.QuizzClassLessonQuizz, quizzClassLesson.id, 0, 0, vars.searchString, "", "", successSearch, errorSearch);
                    }, 1000);
                }

                return {
                    vars: vars,
                    quizzes: quizzes,
                    open: open,
                    close: close,
                    submitRequest: submitRequest,
                    search: search,
                }
            })();

            function remove(idx: number, item: IQuizzOverviewModel) {

                function successRemove() {
                    notificationSvc.success(str.deleteSuccess);

                    assignedQuizzes.splice(idx, 1);
                }

                function errorSave() {
                    notificationSvc.error(str.errorSave);
                }

                dialogSvc.confirm(str.confirmDelete, function (result: boolean) {
                    if (result) {
                        var model: IQuizzClassLessonQuizzModel = {
                            id: 0,
                            quizzId: item.id,
                            quizzClassLessonId: quizzClassLesson.id
                        };
                        resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzClassLessonQuizz, model, successRemove, errorSave);
                    }
                });
            }

            init();

            $scope.page = page;

            $scope.assignedQuizzes = assignedQuizzes;
            $scope.findQuizzesControl = findQuizzesControl;
            $scope.remove = remove;
        }

        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/classrooms/lessons/LessonQuizzes.html",
            replace: true,
            scope: {
                quizzClassLesson: "=",
            },
            controller: controller
        }
    }
})();