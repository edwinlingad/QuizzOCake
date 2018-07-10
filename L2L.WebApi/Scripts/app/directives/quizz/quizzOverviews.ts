module quizzOverviewsDirective {
    export interface IScope {
        page: IPage;
        message: string;
        user: IUserModel;
        quizzOverviews: IQuizzOverviewModel[];
        goToPage: (num: number) => void;
        gradeLevelChanged: (value: string) => void;
        search: () => void;

        sortByOption: ISearchOption;
        sortTypeOption: ISearchOption;
        categoryOption: ISearchOption;
        minGradeLevelOption: ISearchOption;
        maxGradeLevelOption: ISearchOption;

        pager: IPager;

        searchParams: ISearchParam;
        hideCategory: boolean;
        hideGradeLevel: boolean;

        searchCondition: ISearchParam;

        loadMore(): void;
        $broadcast: any;

        deleteIdx(idx: number): void;
        searchChanged(): void;

        shareControl: any;
    }

    export interface ISearchParam {
        category?: string;
        levelMin?: string;
        levelMax?: string;
        searchString?: string;
        pageNum?: number;
        numPerPage?: number;
        sortBy?: number;
        sortType?: number;
        userId?: any;
        availOnly?: any;
        dailyRewardsOnly?: any;
    }

    export interface IEnumInputType {
        name: string;
        value: any;
    }

    export interface ISearchOption {
        selected?: any;
        options: IEnumInputType[];
    }
}

(function () {
    l2lApp.directive("quizzOverviews", function () {

        controller.$inject = ["$scope", "currentUser", "quizzOverviewSvc", "notificationSvc", "cachedDataSvc", "$timeout"];

        function controller(
            $scope: quizzOverviewsDirective.IScope,
            currentUser: ICurrentUser,
            quizzOverviewSvc: IQuizzOverviewSvc,
            notificationSvc: INotificationSvc,
            cachedDataSvc: ICachedDataSvc,
            $timeout: ng.ITimeoutService

        ) {
            var appState: IAppState = setting.appState;
            var page: IPage = {
                isReady: false,
                numResourceToWait: 2
            }
            var categories: IQuizzCategoryModel[] = new Array<IQuizzCategoryModel>();
            var gradeLevels: IGradeLevelModel[] = new Array<IGradeLevelModel>();
            var user: IUserModel = currentUser.getUserData();
            var searchCondition: quizzOverviewsDirective.ISearchParam = util.clone($scope.searchParams);
            var qoList: IQuizzOverviewModel[] = new Array<IQuizzOverviewModel>();
            var qoListResult: IQuizzOverviewListResult;
            var sortByOption: quizzOverviewsDirective.ISearchOption = { options: new Array<quizzOverviewsDirective.IEnumInputType>() };
            var sortTypeOption: quizzOverviewsDirective.ISearchOption = { options: new Array<quizzOverviewsDirective.IEnumInputType>() };
            var categoryOption: quizzOverviewsDirective.ISearchOption = { options: new Array<quizzOverviewsDirective.IEnumInputType>() };
            var minGradeLevelOption: quizzOverviewsDirective.ISearchOption = { options: new Array<quizzOverviewsDirective.IEnumInputType>() };
            var maxGradeLevelOption: quizzOverviewsDirective.ISearchOption = { options: new Array<quizzOverviewsDirective.IEnumInputType>() };

            var pager: IPager = {
                currentPage: 1,
                totalItems: 1,
                pageChanged: function () {
                    goToPage(pager.currentPage);
                }
            };

            function errorLoad() {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait == 0;
            }

            function init() {
                function initSearchCondition() {
                    if (searchCondition.pageNum === undefined)
                        searchCondition.pageNum = 1;
                    if (searchCondition.numPerPage === undefined)
                        searchCondition.numPerPage = 10;
                    if (searchCondition.sortBy === undefined)
                        searchCondition.sortBy = enums.SortByEnum.DateCreated;
                    if (searchCondition.sortType === undefined)
                        searchCondition.sortType = enums.SortTypeEnum.Descending;
                    if (searchCondition.category === undefined)
                        searchCondition.category = enums.QuizzCategoryEnum.Unassigned.toString();
                    if (searchCondition.levelMin === undefined)
                        searchCondition.levelMin = "-1";
                    if (searchCondition.levelMax === undefined)
                        searchCondition.levelMax = "-1";
                    if (searchCondition.availOnly === undefined)
                        searchCondition.availOnly = 1;
                }

                function initFilterOptions() {
                    initSortByOptions();
                    initSortTypeOptions();
                    initCategoryOptions();
                    initGradeLevelOptions();
                }

                function initSortByOptions() {
                    $scope.sortByOption = sortByOption = {
                        selected: searchCondition.sortBy.toString(),
                        options: [
                            { name: "Date Created", value: enums.SortByEnum.DateCreated },
                            { name: "Date Modified", value: enums.SortByEnum.DateModified },
                            { name: "Number of Likes", value: enums.SortByEnum.NumLikes },
                            { name: "Number of Questions", value: enums.SortByEnum.NumQuestions },
                            { name: "Creator", value: enums.SortByEnum.Creator }
                        ]
                    };
                }

                function initSortTypeOptions() {
                    $scope.sortTypeOption = sortTypeOption = {
                        selected: searchCondition.sortType.toString(),
                        options: [
                            { name: "Ascending", value: enums.SortTypeEnum.Ascending },
                            { name: "Descending", value: enums.SortTypeEnum.Descending }
                        ]
                    };
                }

                function initCategoryOptions() {
                    $scope.categoryOption = categoryOption = {
                        selected: searchCondition.category,
                        options: [
                            { name: "Any", value: enums.QuizzCategoryEnum.Unassigned }
                        ]
                    };

                    categories.forEach(function (item: IQuizzCategoryModel) {
                        categoryOption.options.push({
                            name: item.title, value: item.quizzCategoryType
                        });
                    });
                }

                function initGradeLevelOptions() {
                    $scope.minGradeLevelOption = minGradeLevelOption = {
                        selected: searchCondition.levelMin,
                        options: [
                            { name: "Any", value: "-1" }
                        ]
                    }

                    gradeLevels.forEach(function (item) {
                        minGradeLevelOption.options.push({
                            name: item.name, value: item.gradeLevel
                        });
                    });

                    $scope.maxGradeLevelOption = maxGradeLevelOption = {
                        selected: searchCondition.levelMax,
                        options: new Array<quizzOverviewsDirective.IEnumInputType>()
                    }
                }

                function successGradeLevel(data) {
                    gradeLevels = data;
                    updateIsReady();

                    initSearchCondition();
                    initFilterOptions();
                    updateQuizzes(true);
                }

                function successCategories(data) {
                    categories = data;
                    updateIsReady();

                    gradeLevels = cachedDataSvc.getGradeLevels(successGradeLevel, errorLoad);
                }

                // init start
                categories = cachedDataSvc.getQuizzzCategories(successCategories, errorLoad);

                if ($scope.hideCategory === undefined)
                    $scope.hideCategory = false;
                if ($scope.hideGradeLevel === undefined)
                    $scope.hideGradeLevel = false;
            }

            function updateQuizzes(clearList: boolean) {

                function successQoListResult(data: IQuizzOverviewListResult) {
                    qoListResult = data;
                    updateIsReady();

                    pager.totalItems = qoListResult.totalCount;

                    qoListResult.list.forEach(function (item) {
                        qoList.push(item);
                    });

                    pager.currentPage = searchCondition.pageNum;

                    $scope.$broadcast('scroll.infiniteScrollComplete');
                }

                page.numResourceToWait = 1;
                page.isReady = false;

                if (clearList)
                    qoList.splice(0, qoList.length);
                qoListResult = quizzOverviewSvc.getQuizzOverviewListResult(searchCondition, successQoListResult, errorLoad);
            }

            function goToPage(num: number) {
                if (num === undefined)
                    return;

                searchCondition.pageNum = num;
                updateQuizzes(true);
            }

            function gradeLevelChanged(value: string): void {
                var min = parseInt(minGradeLevelOption.selected);
                var max = parseInt(maxGradeLevelOption.selected);
                if (max < min) {
                    if (value == "min")
                        maxGradeLevelOption.selected = minGradeLevelOption.selected;
                    else
                        minGradeLevelOption.selected = maxGradeLevelOption.selected;
                }
            }

            function search() {
                searchCondition.sortBy = sortByOption.selected;
                searchCondition.sortType = sortTypeOption.selected;
                searchCondition.category = categoryOption.selected;
                searchCondition.levelMin = minGradeLevelOption.selected;
                searchCondition.levelMax = maxGradeLevelOption.selected;
                searchCondition.pageNum = 1;
                updateQuizzes(true);
            }

            var timeOutHandle;
            function searchChanged(): void {
                if (timeOutHandle !== undefined)
                    $timeout.cancel(timeOutHandle);
                timeOutHandle = $timeout(function () {
                    search();
                }, 1000);
            }

            function loadMore(): void {
                searchCondition.pageNum = pager.currentPage + 1;
                updateQuizzes(false);
            }

            function deleteIdx(idx: number): void {
                $timeout(function () {
                    qoList.splice(idx, 1);
                }, 200);
            }

            init();

            $scope.page = page;
            $scope.user = user;
            $scope.quizzOverviews = qoList;
            $scope.goToPage = goToPage;

            $scope.sortByOption = sortByOption;
            $scope.sortTypeOption = sortTypeOption;
            $scope.categoryOption = categoryOption;
            $scope.minGradeLevelOption = minGradeLevelOption;
            $scope.maxGradeLevelOption = maxGradeLevelOption;

            $scope.gradeLevelChanged = gradeLevelChanged;

            $scope.search = search;
            $scope.pager = pager;

            $scope.searchCondition = searchCondition;

            $scope.loadMore = loadMore;
            $scope.deleteIdx = deleteIdx;

            $scope.searchChanged = searchChanged;
        }

        return {
            restrict: 'E',
            templateUrl: 'scripts/templates/directives/quizz-overviews.html',
            replace: true,
            scope: {
                searchParams: "=",
                hideCategory: "@",
                hideGradeLevel: "@",
                hideSearch: "@?"
            },
            controller: controller
        }
    });
})();