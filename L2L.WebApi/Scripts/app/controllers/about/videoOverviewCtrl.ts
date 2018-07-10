module videoOverviewCtrl {
    export interface IScope {
        close: () => void;
    }
}

(function () {
    l2lControllers.controller('videoOverviewCtrl', videoOverviewCtrl);

    videoOverviewCtrl.$inject = ["$scope", "$uibModalInstance"];

    function videoOverviewCtrl(
        $scope: videoOverviewCtrl.IScope,
        $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance
    ) {
        function init() {

        }

        function close(): void {
            $uibModalInstance.dismiss();
        }

        init();

        $scope.close = close;
    }
})();