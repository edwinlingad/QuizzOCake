
// add-category-tag

// TODO: merge
(function () {
    l2lApp.directive("addCategoryIconDyn", function () {
        controller.$inject = ["$scope", "cachedDataSvc", "$timeout"];

        function controller(
            $scope: any,
            cachedDataSvc: ICachedDataSvc,
            $timeout: ng.ITimeoutService
        ) {

            var categories: IQuizzCategoryModel[];
            var catModel: IQuizzCategoryModel = {
                iconStrValue: "",
                iconColor: "",
                textColor: "",
                borderColor: "",
            };

            function init() {
                function getCatModel() {
                    if ($scope.category === undefined) {
                        $timeout(function () {
                            getCatModel();
                        }, 500);

                        return;
                    }

                    categories.forEach(function (item: IQuizzCategoryModel) {
                        if (item.quizzCategoryType === $scope.category)
                            $scope.catModel = catModel = item;
                    });
                }

                function successCategories(data: IQuizzCategoryModel[]) {
                    categories = data;
                    getCatModel();
                }

                categories = cachedDataSvc.getQuizzzCategories(successCategories);

                if ($scope.control !== undefined && $scope.control !== null) {
                    $scope.control.update = function () {
                        categories.forEach(function (item: IQuizzCategoryModel) {
                            if (item.quizzCategoryType.toString() === $scope.category || item.quizzCategoryType === $scope.category)
                                $scope.catModel = catModel = item;
                        });
                    }
                }
            }

            init();
            $scope.catModel = catModel;
        }

        return {
            scope: {
                category: "=",
                addClass: "@",
                control: "=?"
            },
            template: "<span class='{{addClass}}' style='background-color:{{catModel.iconColor}};border-color:{{catModel.borderColor}};color: {{catModel.textColor}}' uib-tooltip='{{catModel.title}} ' tooltip-placement='right'>{{catModel.iconStrValue}}</span>",
            controller: controller
        }
    });
})();

(function () {
    l2lApp.directive("addCategoryIcon", function (
        cachedDataSvc: ICachedDataSvc
    ) {

        var catModel: IQuizzCategoryModel;
        var categories: IQuizzCategoryModel[];

        function link(scope, element: ng.IAugmentedJQuery, attr: ng.IAttributes) {
            function getCatModel() {
                for (var i: number = 0; i < categories.length; i++) {
                    if (categories[i].quizzCategoryType === scope.category) {
                        catModel = categories[i];
                        console.log(categories[i].iconStrValue);
                    }
                }
            }

            function successCategories(data: IQuizzCategoryModel[]) {
                
                categories = data;
                getCatModel();

                var htmlElement = "<span class='" + scope.addClass + "'" +
                    "style='background-color:" + catModel.iconColor + ";" +
                    "border-color:" + catModel.borderColor + ";" +
                    "color:" + catModel.textColor + ";' " +
                    "uib-tooltip=" + catModel.title + " " +
                    "tooltip-placement='right'" +
                    '">' + catModel.iconStrValue + "</span>";
                element.prepend(htmlElement);
            }

            categories = cachedDataSvc.getQuizzzCategories(successCategories);
        }

        return {
            scope: {
                category: "=",
                addClass: "@"
            },
            link: link
        }
    });
})();

