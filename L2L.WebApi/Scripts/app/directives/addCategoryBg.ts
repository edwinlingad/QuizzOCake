// add-category-tag
(function () {
    l2lApp.directive("addCategoryBg", addCategoryBg);

    function addCategoryBg() {
        var catClass: string[] = [
            "math-bg-color",
            "science-bg-color",
            "english-bg-color",
            "filipino-bg-color",
            "araling-panlipunan-bg-color",
            "mandarin-bg-color",
            "computer-ed-bg-color",
        ];

        var catClassNoHover: string[] = [
            "math-bg-color-no-hover",
            "science-bg-color-no-hover",
            "english-bg-color-no-hover",
            "filipino-bg-color-no-hover",
            "araling-panlipunan-bg-color-no-hover",
            "mandarin-bg-color-no-hover",
            "computer-ed-bg-color-no-hover",
        ];

        function link(scope, element: ng.IAugmentedJQuery, attr: ng.IAttributes) {
            var idx: number = parseInt(scope.category);
            if (scope.noHover != undefined && scope.noHover == "true")
                element.addClass(catClassNoHover[idx]);
            else
                element.addClass(catClass[idx]);
        }

        return {
            scope: {
                category: "=",
                noHover: "@"
            },
            link: link
        }
    }
})();

(function () {
    l2lApp.directive("addCategoryFontColor", addCategoryBg);

    function addCategoryBg() {
        var catClass: string[] = [
            "math-color",
            "science-color",
            "english-color",
            "filipino-color",
            "araling-panlipunan-color",
            "mandarin-color",
            "computer-ed-color",
        ];

        function link(scope, element: ng.IAugmentedJQuery, attr: ng.IAttributes) {
            var idx: number = parseInt(scope.category);
            element.addClass(catClass[idx]);
        }

        return {
            scope: {
                category: "="
            },
            link: link
        }
    }
})();