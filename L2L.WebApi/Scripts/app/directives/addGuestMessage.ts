module addGuestMessage {
    export interface IScope {
        openLoginModal: () => void;
        openSignUpModal: () => void;
    }
}

(function () {
    l2lApp.directive("addGuestMessage", addGuestMessage);

    function addGuestMessage() {

        controller.$inject = ["$scope", "loginModalSvc"];

        function controller(
            $scope: addGuestMessage.IScope,
            loginModalSvc: ILoginModalSvc
            ) {

            function openLoginModal(): void {
                loginModalSvc.openLoginModal(false);
            }

            function openSignUpModal(): void {
                loginModalSvc.openSignUpModal();
            }

            $scope.openLoginModal = openLoginModal;
            $scope.openSignUpModal = openSignUpModal;
        }

        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/add-guest-message.html",
            replace: true,
            scope: {
                msg: "@",
                isGuest: "="
            },
            controller: controller
        }
    }            
})();