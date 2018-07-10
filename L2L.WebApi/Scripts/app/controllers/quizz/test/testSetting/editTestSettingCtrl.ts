module editTestSettingCtrl {
    export interface IModalSetting {
        numAvailQuestions: number;
        saveToDb: boolean;
        isBuiltIn: boolean;
    }

    export interface IScope {
        user: IUserModel;
        page: IPage;
        modalSetting: IModalSetting;
        setting: ITestSettingModel;
        submit: (form: ng.IFormController) => void;
        cancel: () => void;
        changed: () => void;
        numQCtrl: editTestSettingCtrl.INumQuestionsCtrl;
    }

    export interface INumQuestionsCtrl {
        selected: string;
        numQList: IInputItem[];
        hideCustomValueInput: () => boolean;
        customValue: number;
    }
}

(function () {
    l2lControllers.controller("editTestSettingCtrl", editTestSettingCtrl);

    editTestSettingCtrl.$inject = ["$scope", "$uibModalInstance", "testSettingSvc", "setting", "modalSetting", "currentUser", "notificationSvc", "loginModalSvc"];
    function editTestSettingCtrl(
        $scope: editTestSettingCtrl.IScope,
        $uibModalInstance: ng.ui.bootstrap.IModalServiceInstance,
        testSettingSvc: ITestSettingSvc,
        setting: ITestSettingModel,
        modalSetting: editTestSettingCtrl.IModalSetting,
        currentUser: ICurrentUser,
        notificationSvc: INotificationSvc,
        loginModalSvc: ILoginModalSvc
        ) {

        var page: IPage = {
            isReady: true,
            numResourceToWait: 0,
            disabled: false
        }
        var user: IUserModel = currentUser.getUserData();
        var settingCopy: ITestSettingModel = util.clone(setting);
        var numQCtrl: editTestSettingCtrl.INumQuestionsCtrl = {
            selected: "0",
            numQList: [{ name: "All", value: "0" }],
            customValue: 0,
            hideCustomValueInput: hideCustomValueInput
        };

        function init() {
            if(modalSetting.isBuiltIn) {
                numQCtrl.numQList = [
                    { name: "10", value: "10" },
                    { name: "20", value: "20" },
                    { name: "50", value: "50" },
                    { name: "Custom", value: "custom" }
                ];
            } else if (modalSetting.numAvailQuestions == 0) {
                numQCtrl.numQList = [
                    { name: "All", value: "0" },
                    { name: "10", value: "10" },
                    { name: "20", value: "20" },
                    { name: "50", value: "50" },
                    { name: "Custom", value: "custom" }
                ];
            } else {
                if (modalSetting.numAvailQuestions >= 10) {
                    numQCtrl.numQList.push({ name: "10", value: "10" });
                }
                if (modalSetting.numAvailQuestions >= 20) {
                    numQCtrl.numQList.push({ name: "20", value: "20" });
                }
                if (modalSetting.numAvailQuestions >= 50) {
                    numQCtrl.numQList.push({ name: "50", value: "50" });
                }
                numQCtrl.numQList.push({ name: "Custom", value: "custom" });
            }

            if (settingCopy.numberOfQuestions != 0 &&
                settingCopy.numberOfQuestions != 10 &&
                settingCopy.numberOfQuestions != 20 &&
                settingCopy.numberOfQuestions != 50) {
                numQCtrl.selected = "custom";
                numQCtrl.customValue = settingCopy.numberOfQuestions;
            }
            else
                numQCtrl.selected = settingCopy.numberOfQuestions.toString();
        }

        function submit(form: ng.IFormController): void {
            function copyBackAndExit() {
                page.disabled = false;
                util.copy(setting, settingCopy);
                $uibModalInstance.close();
            }

            if (user.isGuest) {
                loginModalSvc.openLoginModal(false, true, function () {
                    $uibModalInstance.dismiss();
                });
                return;
            }

            if (form.$invalid == false) {
                util.disableForm(page);
                if (numQCtrl.selected == "custom") {

                    if (modalSetting.isBuiltIn) {
                        if (numQCtrl.customValue <= 0)
                            settingCopy.numberOfQuestions = 20;
                        else
                            settingCopy.numberOfQuestions = numQCtrl.customValue;

                    } else if (modalSetting.numAvailQuestions == 0)
                        settingCopy.numberOfQuestions = numQCtrl.customValue;
                    else {
                        if (modalSetting.numAvailQuestions < numQCtrl.customValue || numQCtrl.customValue <= 0)
                            settingCopy.numberOfQuestions = 0;
                        else
                            settingCopy.numberOfQuestions = numQCtrl.customValue;
                    }
                }
                else
                    settingCopy.numberOfQuestions = parseInt(numQCtrl.selected);

                if (modalSetting.saveToDb) {
                    page.disabled = true;
                    testSettingSvc.updateTestSetting(settingCopy,
                        function () {
                            util.enableForm(page);
                            copyBackAndExit();
                        },
                        function () {
                            util.enableForm(page);
                            notificationSvc.error(str.errorSave);
                        });
                }
                else {
                    util.enableForm(page);
                    copyBackAndExit();
                }
            } 
        }

        function cancel() {
            $uibModalInstance.dismiss();
        }

        function hideCustomValueInput() {
            return numQCtrl.selected != "custom";
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.setting = settingCopy;
        $scope.modalSetting = modalSetting;
        $scope.submit = submit;
        $scope.cancel = cancel;
        $scope.numQCtrl = numQCtrl;
    }
})();