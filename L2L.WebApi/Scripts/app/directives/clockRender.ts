module clockRender {
    export interface IScope {
        time: string;
        cId: string;
        addClass?: string;
    }
}

(function () {
    l2lApp.directive("clockRender", clockRender);

    function clockRender() {

        controller.$inject = ["$scope", "$timeout"];

        function controller(
            $scope: clockRender.IScope,
            $timeout: ng.ITimeoutService
            ) {

            var time: string[] = $scope.time.split(":");

            $timeout(function () {
                clockRenderer($scope.cId).drawClock(parseInt(time[0]), parseInt(time[1]));
            }, 500);
        }

        return {
            restrict: "E",
            template: "<canvas id='{{cId}}' class='{{addClass}}'> </canvas>",
            replace: true,
            scope: {
                time: "@",
                cId: "@",
                addClass: "@?"
            },
            controller: controller
        }
    }
})();