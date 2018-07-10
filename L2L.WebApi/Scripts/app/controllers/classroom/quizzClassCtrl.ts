module quizzClassCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        view: number;

        quizzClass: IQuizzClassModel;
        classRequestControl: any;

        goBack(): void;
    }

    export interface IStateParams {
        qcId: number;
        view: number;
    }
}

(function () {
    l2lControllers.controller('quizzClassCtrl', quizzClassCtrl);

    quizzClassCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "topPanelContentSvc"];

    function quizzClassCtrl(
        $scope: quizzClassCtrl.IScope,
        $stateParams: quizzClassCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        resourceSvc: IResourceSvc,
        $timeout: ng.ITimeoutService,
        dialogSvc: IDialogSvc,
        topPanelContentSvc: ITopPanelContentSvc
    ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 1,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var quizzClass: IQuizzClassModel = {};

        function errorSave(response) {
            notificationSvc.error(str.errorSave);
        }

        function errorLoad(response) {
            notificationSvc.error(str.errorLoad);
        }

        function init() {

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successQuizzClass(data: IQuizzClassModel) {
                $scope.quizzClass = quizzClass = data;
                quizzClass.disabled = false;
                quizzClass.cancelRequestItem = {
                    isReady: true
                };
                quizzClass.acceptRequestItem = {
                    isReady: true
                };
                quizzClass.rejectRequestItem = {
                    isReady: true
                };
                updateIsReady();
            }

            $anchorScroll("top");
            var idx: number = util.getNumber($stateParams.qcId);
            $scope.view = util.getNumber($stateParams.view);
            quizzClass = resourceSvc.getResource(enums.ResourceTypeEnum.QuizzClass, idx, successQuizzClass, errorLoad);
        }

        var classRequestControl = (function () {
            var vars = {
                message: "",
                isSaveEnabled: false,
                disableAll: false,
            };

            function open() {
                quizzClass.isOpen = true;
                vars.disableAll = true;
                vars.isSaveEnabled = true;
                vars.message = "Hi " + quizzClass.teacherName + ", please add me as your learner";
            }

            function close() {
                quizzClass.isOpen = false;
                vars.disableAll = false;
            }

            function changed() {
                if (vars.message.trim() === "")
                    vars.isSaveEnabled = false;
                else
                    vars.isSaveEnabled = true;
            }

            function submitRequest() {

                function successCreate(model: IQuizzClassJoinRequestModel) {
                    quizzClass.isRequestSent = true;
                    quizzClass.isOpen = false;
                    quizzClass.disabled = false;
                    vars.disableAll = false;

                    vars.message = "";

                    $state.go("si.quizzClassAll");

                    notificationSvc.success(str.successRequest);
                }

                vars.disableAll = true;
                quizzClass.disabled = true;

                var model: IQuizzClassJoinRequestModel = {
                    id: 0,
                    message: vars.message,
                    quizzClassId: quizzClass.id
                };

                resourceSvc.createResource(enums.ResourceTypeEnum.QuizzClassJoinRequest, model, successCreate, errorSave);
            }

            function cancelRequest() {

                function successCancel() {
                    quizzClass.disabled = false;
                    quizzClass.cancelRequestItem.isReady = true;
                    quizzClass.isRequestSent = false;

                    notificationSvc.success(str.successCancelRequest);
                }

                dialogSvc.confirm(str.confirmCancelRequest, function (result: boolean) {
                    if (result) {
                        quizzClass.disabled = true;
                        quizzClass.cancelRequestItem.isReady = false;

                        var model: IQuizzClassJoinRequestModel = {
                            quizzClassId: quizzClass.id
                        };

                        resourceSvc.deleteResource(enums.ResourceTypeEnum.QuizzClassJoinRequest, quizzClass.id, successCancel, errorSave);
                    }
                });
            }

            function accept() {
                function successAccept(data: IQuizzClassModel) {
                    quizzClass.disabled = false;
                    quizzClass.acceptRequestItem.isReady = true;

                    topPanelContentSvc.subFromNewQuizzClassCount(1);
                    notificationSvc.success(str.successAcceptRequest);
                    $state.go("si.quizzClassAll");
                }

                quizzClass.disabled = true;
                quizzClass.acceptRequestItem.isReady = false;

                var invite: IQuizzClassInviteRequestModel = {
                    quizzClassId: quizzClass.id,
                    isAccepted: true
                };

                resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzClassMemberInvite, invite, successAccept, errorSave);
            }

            function reject() {
                function successReject() {
                    quizzClass.disabled = false;
                    quizzClass.rejectRequestItem.isReady = true;

                    topPanelContentSvc.subFromNewQuizzClassCount(1);
                    notificationSvc.success(str.successRejectRequest);
                    $state.go("si.quizzClassAll");
                }

                dialogSvc.confirm(str.confirmRejectRequest, function (result: boolean) {
                    if (result) {

                        quizzClass.disabled = true;
                        quizzClass.rejectRequestItem.isReady = false;

                        var invite: IQuizzClassInviteRequestModel = {
                            quizzClassId: quizzClass.id,
                            isAccepted: false
                        };
                        resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzClassMemberInvite, invite, successReject, errorSave);
                    }
                });
            }

            return {
                vars: vars,
                open: open,
                close: close,
                changed: changed,
                submitRequest: submitRequest,
                cancelRequest: cancelRequest,
                accept: accept,
                reject: reject
            }
        })();

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.quizzClass = quizzClass;
        $scope.classRequestControl = classRequestControl;

        $scope.goBack = goBack;
    }
})();