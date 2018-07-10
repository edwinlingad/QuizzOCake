/// <reference path="../../models/accounts/accountbindingmodels.ts" />

module registerUserCtrl {
    export interface IScope {
        page: IPage;
        user: IUserModel;
        showMessage: boolean;
        message: string;
        standardUser: IRegisterStandardModel;
        registerStandard: (form: ng.IFormController) => void;
        dependentUser: IRegisterDependentModel;
        registerDependent: (form: ng.IFormController) => void;
        changed(): void;

        standardsaveEnabled(): boolean;
        dependentUserSaveEnabled(): boolean;
        close(): void;
        termAgree: boolean;
    }
}

(function () {
    l2lControllers.controller('registerUserCtrl', registerUserCtrl);

    registerUserCtrl.$inject = ["$scope", "accountSvc", "currentUser", "$state", "broadcastSvc", "notificationSvc", "$uibModalInstance", "loginModalSvc", "layoutSvc"];
    function registerUserCtrl(
        $scope: registerUserCtrl.IScope,
        accountSvc: IAccount,
        currentUser: ICurrentUser,
        $state: ng.ui.IStateService,
        broadcastSvc: IBroadcastSvc,
        notificationSvc: INotificationSvc,
        $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
        loginModalSvc: ILoginModalSvc,
        layoutSvc: ILayoutSvc
    ) {

        var page: IPage = {
            isReady: false,
            numResourceToWait: 0,
            disabled: false
        };
        var standardUser: IRegisterStandardModel = {
            firstName: "",
            lastName: "",
            userName: "",
            email: "",
            password: "",
            confirmPassword: ""
        };
        var dependentUser: IRegisterDependentModel = {
            firstName: "",
            lastName: "",
            userName: "",
            password: "",
            confirmPassword: ""
        };
        var user: IUserModel = currentUser.getUserData();

        function init() {
            var today: Date = new Date();
            standardUser.birthDate = new Date(today.getFullYear() - 20, today.getMonth(), today.getDate());
            dependentUser.birthDate = new Date(today.getFullYear() - 8, today.getMonth(), today.getDate());
        }

        function registerStandard(form: ng.IFormController) {
            function errorRegister(response) {
                util.enableForm(page);
                $scope.message = response.data.message;
                notificationSvc.error(str.registerQuizzlingError);
            }

            if (form.$valid) {
                var idx = standardUser.userName.indexOf("@");
                if (standardUser.userName.indexOf("@") != -1) {
                    $scope.message = "You cannot use email address as username";
                    return;
                }

                if (standardUser.userName.length < 4) {
                    $scope.message = "Username should have 4 or more characters";
                    return;
                }

                if (standardUser.password !== standardUser.confirmPassword) {
                    $scope.message = "Passwords do not match";
                    return;
                }

                var age: number = Math.abs(util.getDiffDays(standardUser.birthDate) / 365.25);
                if (age < 16) {
                    $scope.message = "User should be less than 16 years old";
                    return;
                }

                util.disableForm(page);
                accountSvc.registerStandardUser($scope.standardUser,
                    function (data) {
                        var loginUser: ILogin = {
                            grant_type: "password",
                            userName: standardUser.userName,
                            password: standardUser.password
                        }

                        accountSvc.loginUser(loginUser,
                            function (data) {
                                util.enableForm(page);
                                currentUser.setAcccessToken(data.access_token);
                                currentUser.accessToken = data.access_token;

                                accountSvc.getUser(function (data) {
                                    currentUser.setUserData(data);
                                    notificationSvc.success("Registration successful");
                                    $uibModalInstance.close(true);
                                });
                            }, errorRegister);
                    }, errorRegister);
            }
        }

        function registerDependent(form: ng.IFormController) {
            function errorRegister(response) {
                util.enableForm(page);
                commonError($scope, response);
                notificationSvc.error(str.registerQuizzlingError);
            }

            if (util.showLoginIfGuest(user, loginModalSvc, function () {
                $uibModalInstance.dismiss();
            }))
                return;

            if (form.$valid) {
                var idx = dependentUser.userName.indexOf("@");
                if (dependentUser.userName.indexOf("@") != -1) {
                    $scope.message = "You cannot use email address as username";
                    return;
                }

                if (dependentUser.userName.length < 4) {
                    $scope.message = "Username should have 4 or more characters";
                    return;
                }

                if (dependentUser.password !== dependentUser.confirmPassword) {
                    $scope.message = "Passwords do not match";
                    return;
                }

                util.disableForm(page);

                accountSvc.registerDependentUser($scope.dependentUser,
                    function (data) {
                        util.enableForm(page);
                        var bcData: IBroadcastData = {
                            eventType: bcEventTypeEnum.newItem,
                            model: data
                        }
                        layoutSvc.updateQuizzlings();

                        notificationSvc.success(str.registerQuizzlingSuccess);
                        $uibModalInstance.close(true);
                    }, errorRegister);
            }
        }

        function changed(): void {
            $scope.message = "";
        }

        function standardsaveEnabled(): boolean {
            if (standardUser.firstName !== undefined && standardUser.firstName.trim().length !== 0 &&
                standardUser.lastName !== undefined && standardUser.lastName.trim().length !== 0 &&
                standardUser.userName !== undefined && standardUser.userName.trim().length !== 0 &&
                standardUser.email !== undefined && standardUser.email.trim().length !== 0 &&
                standardUser.password !== undefined && standardUser.password.trim().length !== 0 &&
                standardUser.confirmPassword !== undefined && standardUser.confirmPassword.trim().length !== 0 &&
                standardUser.birthDate !== undefined && standardUser.birthDate !== null &&
                $scope.termAgree === true
            )
                return true;
            return false;
        }

        function dependentUserSaveEnabled(): boolean {
            if (dependentUser.firstName !== undefined && dependentUser.firstName.trim().length !== 0 &&
                dependentUser.lastName !== undefined && dependentUser.lastName.trim().length !== 0 &&
                dependentUser.userName !== undefined && dependentUser.userName.trim().length !== 0 &&
                dependentUser.password !== undefined && dependentUser.password.trim().length !== 0 &&
                dependentUser.confirmPassword !== undefined && dependentUser.confirmPassword.trim().length !== 0 &&
                dependentUser.birthDate !== undefined && dependentUser.birthDate !== null &&
                $scope.termAgree === true
            )
                return true;

            return false;
        }

        function close(): void {
            $uibModalInstance.dismiss();
        }

        init();

        $scope.page = page;
        $scope.user = user;
        $scope.standardUser = standardUser;
        $scope.dependentUser = dependentUser;
        $scope.message = "";
        $scope.registerStandard = registerStandard;
        $scope.registerDependent = registerDependent;
        $scope.changed = changed;
        $scope.standardsaveEnabled = standardsaveEnabled;
        $scope.dependentUserSaveEnabled = dependentUserSaveEnabled;

        $scope.close = close;
        $scope.termAgree = false;
    }
})(); 