module childIcon {
    export interface IScope {
        age: number;
    }
}

(function () {
    l2lApp.directive("childIcon", childIcon);

    function childIcon() {

        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/child-icon.html",
            replace: true,
            scope: {
                age: "@"
            },
        }
    }
})();