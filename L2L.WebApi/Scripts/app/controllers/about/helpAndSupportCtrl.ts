module helpAndSupportCtrl {
    export interface IScope {
        gotoHash(hash: string): void;
    }

    export interface IStateParams {
    }
}

(function () {
    l2lControllers.controller('helpAndSupportCtrl', helpAndSupportCtrl);

    helpAndSupportCtrl.$inject = ["$scope", "$anchorScroll"];

    function helpAndSupportCtrl(
        $scope: helpAndSupportCtrl.IScope,
        $anchorScroll: ng.IAnchorScrollService
    ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 1,
            disabled: false
        };

        function init() {
            $anchorScroll("top");
        }

        function gotoHash(hash: string): void {
            $('html, body').stop().animate({
                scrollTop: $('#' + hash).offset().top - 90
            }, 1500, 'easeInOutExpo');
        }

        init();

        $scope.gotoHash = gotoHash;
    }
})();