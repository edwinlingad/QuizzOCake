module signUpCtrl {
    export interface IScope {
        signUpStandard(): void;
        close(): void;
    }
} 

(function () {
    l2lControllers.controller("signUpCtrl", signUpCtrl);

    signUpCtrl.$inject = ["$scope", "$uibModalInstance"];
    function signUpCtrl($scope: signUpCtrl.IScope, $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance) {
        function signUpStandard(): void {
            $uibModalInstance.close(UserTypeEnum.Standard);
        }

        function close(): void {
            $uibModalInstance.dismiss();
        }

        $scope.close = close;
        $scope.signUpStandard = signUpStandard;
    }
})();