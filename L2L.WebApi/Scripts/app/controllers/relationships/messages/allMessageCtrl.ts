module allMessageCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;

        messages: IQuizzConnectMsgThread[];
        newGroupMsgCtrl: any;

        helpControl: any;

        goBack(): void;
    }

    export interface IStateParams {
    }
}

(function () {
    l2lControllers.controller('allMessageCtrl', allMessageCtrl);

    allMessageCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

    function allMessageCtrl(
        $scope: allMessageCtrl.IScope,
        $stateParams: allMessageCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        resourceSvc: IResourceSvc,
        $timeout: ng.ITimeoutService,
        dialogSvc: IDialogSvc
    ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 1,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var messages: IQuizzConnectMsgThread[] = new Array<IQuizzConnectMsgThread>();

        function errorCreate(response) {
            notificationSvc.error(str.errorCreateChat);
        }

        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successQuizzConnectMsgThread(data: IQuizzConnectMsgThread[]) {
                updateIsReady();

                $scope.messages = messages = data;
            }

            $anchorScroll("top");
            messages = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzConnectMsgThread, 0, 0, 0, 0, 0, successQuizzConnectMsgThread, errorLoad);
        }

        var newGroupMsgCtrl = (function () {
            var vars = {
                isOpen: false,
                groupName: "",
                isCreateEnabled: false,
                disabled: false
            };

            function open() {
                vars.isOpen = true;
            }

            function close() {
                vars.isOpen = false;
            }

            function changed() {
                if (vars.groupName.trim() === "")
                    vars.isCreateEnabled = false;
                else
                    vars.isCreateEnabled = true;
            }

            function successCreate(model: IQuizzmateMsgThreadModel) {
                $state.go("si.quizzmateMsg", { threadId: model.id });
            }

            function create() {
                vars.disabled = true;

                var model: IQuizzmateMsgThreadModel = {
                    id: 0,
                    isGroupMsg: true,
                    groupMessageName: vars.groupName
                };

                resourceSvc.createResource(enums.ResourceTypeEnum.QuizzmateMsgThread, model, successCreate, errorCreate);
            }

            return {
                vars: vars,
                open: open,
                close: close,
                changed: changed,
                create: create
            }
        })();

        var helpControl = (function () {
            var vars = {
                isOpen: false
            }

            function open() {
                vars.isOpen = true;
            }

            function close() {
                vars.isOpen = false;
            }

            return {
                vars: vars,
                open: open,
                close: close
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

        $scope.messages = messages;
        $scope.newGroupMsgCtrl = newGroupMsgCtrl;

        $scope.helpControl = helpControl;

        $scope.goBack = goBack;
    }
})();