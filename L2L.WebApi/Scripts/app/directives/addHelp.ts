module addHelp {
    export interface IScope {
    }
}

(function () {
    l2lApp.directive("addHelp", addHelp);

    function addHelp() {

        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/add-help.html",
            replace: true,
            scope: {
                msg: "@",
            },
        }
    }
})();