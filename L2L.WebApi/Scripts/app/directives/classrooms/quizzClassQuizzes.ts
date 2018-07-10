module quizzClassQuizzes {
    export interface IScope {
        quizzClass: IQuizzClassModel;

        page: IPage;
        $on: any;

        assignedQuizzes: IQuizzOverviewModel[];
        findQuizzesControl: any;

        remove(idx: number, item: IQuizzOverviewModel);
    }
}

(function () {
    l2lApp.directive("quizzClassQuizzes", quizzClassQuizzes);

    function quizzClassQuizzes() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

        function controller(
            $scope: quizzClassQuizzes.IScope,
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
            var quizzClass: IQuizzClassModel = {};
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
                quizzClass = $scope.quizzClass;

                assignedQuizzes = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzClassQuizz, quizzClass.id, 0, 0, 0, 0, successQuizzes, errorLoad);
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

                    var model: IQuizzClassQuizzModel = {
                        id: 0,
                        quizzClassId: quizzClass.id,
                        quizzId: item.quizzId
                    };

                    resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassQuizz, model, successCreate, errorSave);
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
                        resourceSvc.getResourceManyAlt(enums.ResourceTypeEnum.QuizzClassQuizz, quizzClass.id, 0, 0, vars.searchString, "", "", successSearch, errorSearch);
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
                        var model: IQuizzClassQuizzModel = {
                            id: 0,
                            quizzId: item.id,
                            quizzClassId: quizzClass.id
                        };
                        resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzClassQuizz, model, successRemove, errorSave);
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
            templateUrl: "scripts/templates/directives/classrooms/classQuizzes.html",
            replace: true,
            scope: {
                quizzClass: "=",
            },
            controller: controller
        }
    }
})();