module loadTestCtrl {
    export interface IScope {
    }

    export interface IStateParams {
        testSnapId: number;
    }
}

(function () {
    l2lControllers.controller('loadTestCtrl', loadTestCtrl);

    loadTestCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "takeTestCtrlSvc"];
    function loadTestCtrl(
        $scope: loadTestCtrl.IScope,
        $stateParams: loadTestCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        resourceSvc: IResourceSvc,
        $timeout: ng.ITimeoutService,
        dialogSvc: IDialogSvc,
        takeTestCtrlSvc: ITakeTestCtrlSvc
        ) {

        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }            
            
            var idx: number = util.getNumber($stateParams.testSnapId);
            if (idx === -1)
                takeTestCtrlSvc.reloadCurrent();
            else {
                resourceSvc.getResource(enums.ResourceTypeEnum.TestSnapshot, idx,
                    function (data: ITestSnapshot) {
                        takeTestCtrlSvc.load(data);
                    }, errorLoad);
            }

        }

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        init();
    }
})();