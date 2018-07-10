(function () {
    l2lApp.directive("testSettingSummary", testSettingSummary);

    function testSettingSummary() {
        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/test-setting-summary.html",
            scope: {
                settings: "=",
                totalQs: "=?"
            }
        }
    }

})(); 