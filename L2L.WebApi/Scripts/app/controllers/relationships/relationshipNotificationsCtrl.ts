module relationshipNotificationsCtrl {
    export interface IScope {
        page: IPage;
        user: IUserModel;
        rNotificationItem: IItemToWait
        rNotifications: IRelationshipNotificationModel[];

        pNotificationItem: IItemToWait
        pNotifications: IRelationshipNotificationModel[];

        processRequest(idx: number, item: IRelationshipNotificationModel, response: number): void;

        quizzmatesItem: IItemToWait;
        quizzmates: IQuizzerModel[];

        helpControl: any;
        quizzmateControl: any;
    }

    export interface IStateParams {
        quizzId: number;
    }
}

(function () {
    l2lControllers.controller('relationshipNotificationsCtrl', relationshipNotificationsCtrl);

    relationshipNotificationsCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc"];

    function relationshipNotificationsCtrl(
        $scope: relationshipNotificationsCtrl.IScope,
        $stateParams: relationshipNotificationsCtrl.IStateParams,
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
            numResourceToWait: 2
        }
        var user: IUserModel = currentUser.getUserData();
        var rNotificationItem: IItemToWait = { isReady: false };
        var rNotifications: IRelationshipNotificationModel[] = new Array<IRelationshipNotificationModel>();
        var rCount: number = 0;
        var rCurrPage: number = 1;
        var rNumPerPage: number = 10;

        var pNotificationItem: IItemToWait = { isReady: false };
        var pNotifications: IRelationshipNotificationModel[] = new Array<IRelationshipNotificationModel>();
        var pCount: number = 0;
        var pCurrPage: number = 1;
        var pNumPerPage: number = 10;

        var quizzmatesItem: IItemToWait = { isReady: true };
        var quizzmates: IQuizzerModel[] = new Array<IQuizzerModel>();

        function updateIsReady() {
            page.numResourceToWait--;
            page.isReady = page.numResourceToWait <= 0;
        }

        function errorLoad(response) {
            notificationSvc.error(str.errorLoad);
        }

        function init() {
            
            function callGetRNotificationsResource() {
                rNotificationItem.isReady = false;
                resourceSvc.getResourceMany(enums.ResourceTypeEnum.RelationshipNotification, rCurrPage, rNumPerPage, 0, 0, 0, getRNotifications, errorLoad);
            }

            function callGetPNotificationsResource() {
                pNotificationItem.isReady = false;
                resourceSvc.getResourceMany(enums.ResourceTypeEnum.RelationshipNotification, rCurrPage, rNumPerPage, 1, 0, 0, getPNotifications, errorLoad);
            }

            function getRNotifications(data: IRelationshipNotificationModel[]) {
                data.forEach(function (item: IRelationshipNotificationModel) {
                    item.disabled = false;
                    item.acceptProcessing = { isReady: true };
                    item.rejectProcessing = { isReady: true };
                    item.resendProcessing = { isReady: true };
                    item.cancelProcessing = { isReady: true };
                    rNotifications.push(item);
                });

                rCount -= rNumPerPage;
                rCurrPage++;
                rNotificationItem.isReady = true;

                if (rCount > 0)
                    callGetRNotificationsResource();
            }

            function getPNotifications(data: IRelationshipNotificationModel[]) {
                data.forEach(function (item: IRelationshipNotificationModel) {
                    item.disabled = false;
                    item.acceptProcessing = { isReady: true };
                    item.rejectProcessing = { isReady: true };
                    item.resendProcessing = { isReady: true };
                    item.cancelProcessing = { isReady: true };
                    pNotifications.push(item);
                });

                pCount -= pNumPerPage;
                pCurrPage++;
                pNotificationItem.isReady = true;

                if (rCount > 0)
                    callGetPNotificationsResource();
            }

            function successRCount(data: any) {
                rCount = data.count;
                updateIsReady();
                rNotificationItem.isReady = true;

                rCount = rCount > 50 ? 50 : rCount;
                if (rCount > 0)
                    callGetRNotificationsResource();
            }

            function successPCount(data: any) {
                pCount = data.count;
                updateIsReady();
                pNotificationItem.isReady = true;

                pCount = pCount > 50 ? 50 : pCount;
                if (pCount > 0)
                    callGetPNotificationsResource();
            }

            $anchorScroll("top");
            rCount = resourceSvc.getResource(enums.ResourceTypeEnum.RelationshipNotification, 0, successRCount, errorLoad);
            pCount = resourceSvc.getResource(enums.ResourceTypeEnum.RelationshipNotification, 1, successPCount, errorLoad);
            //getQuizzmates();

        }

        //function getQuizzmates() {
        //    var count: number = 0;
        //    var currPage: number = 1;
        //    var numPerPage: number = 10;

        //    function callGetQuizzmatesResource() {
        //        resourceSvc.getResourceMany(enums.ResourceTypeEnum.Quizzmates, 0, currPage, numPerPage, 0, 0, getQuizzmates, errorLoad);
        //    }

        //    function getQuizzmates(data: IQuizzerModel[]) {
        //        data.forEach(function (item: IQuizzerModel) {
        //            quizzmates.push(item);
        //        });

        //        count -= numPerPage;
        //        currPage++;
        //        quizzmatesItem.isReady = true;

        //        if (count > 0)
        //            callGetQuizzmatesResource();
        //    }

        //    function successQuizzmateCount(data: any) {
        //        count = data.count;
        //        updateIsReady();

        //        if (count > 0)
        //            callGetQuizzmatesResource();
        //    }

        //    count = resourceSvc.getResource(enums.ResourceTypeEnum.Quizzmates, 0, successQuizzmateCount, errorLoad);
        //}

        function reloadQuizzmates() {
            //quizzmates.splice(0, quizzmates.length);
            //getQuizzmates();
            quizzmateControl.reload();
        }

        function processRequest(idx: number, item: IRelationshipNotificationModel, response: number): void {
            function errorSave() {
                resetItemState();
                notificationSvc.error(str.errorSave);
            }

            function resetItemState() {
                $timeout(function () {
                    item.disabled = false;
                    item.acceptProcessing.isReady = true;
                    item.rejectProcessing.isReady = true;
                    item.resendProcessing.isReady = true;
                    item.cancelProcessing.isReady = true;
                }, 200);
            }

            function acceptRequest() {
                item.response = 0;
                item.acceptProcessing.isReady = false;

                resourceSvc.updateResource(enums.ResourceTypeEnum.RelationshipNotification, item,
                    function () {
                        resetItemState();
                        rNotifications.splice(idx, 1);
                        reloadQuizzmates();
                        notificationSvc.success(str.relationshipRequestAcceptSuccess);
                    }, errorSave);
            }

            function rejectRequest() {

                dialogSvc.confirm(str.confirmQuizzmateReject, function (result) {
                    if (result == true) {
                        item.response = 1;
                        item.rejectProcessing.isReady = false;

                        resourceSvc.updateResource(enums.ResourceTypeEnum.RelationshipNotification, item,
                            function () {
                                resetItemState();
                                rNotifications.splice(idx, 1);
                                notificationSvc.success(str.relationshipRequestRejectSuccess);
                            }, errorSave);
                    }
                    else {
                        resetItemState();
                    }
                });
            }

            function resendRequest() {
                item.response = 2;
                item.resendProcessing.isReady = false;

                resourceSvc.updateResource(enums.ResourceTypeEnum.RelationshipNotification, item,
                    function () {
                        resetItemState();
                        notificationSvc.success(str.quizzmateRequestSendSuccess);
                    }, errorSave);
            }

            function cancelRequest() {

                dialogSvc.confirm(str.confirmQuizzmateReject, function (result) {
                    if (result == true) {
                        item.response = 3;
                        item.cancelProcessing.isReady = false;

                        resourceSvc.updateResource(enums.ResourceTypeEnum.RelationshipNotification, item,
                            function () {
                                resetItemState();
                                pNotifications.splice(idx, 1);
                                notificationSvc.success(str.quizzmateRequestCancelSuccess);
                            }, errorSave);
                    }
                    else {
                        resetItemState();
                    }
                });
            }

            item.disabled = true;
            var val: number = util.getNumber(response);

            switch (val) {
                case 0:
                    rejectRequest();
                    break;
                case 1:
                    acceptRequest();
                    break;
                case 2:
                    resendRequest();
                    break;
                case 3:
                    cancelRequest();
                    break;
            }
        }

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

        var quizzmateControl = {
            reload: function () {
            }
        };

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.processRequest = processRequest;

        $scope.rNotifications = rNotifications;
        $scope.rNotificationItem = rNotificationItem;

        $scope.pNotifications = pNotifications;
        $scope.pNotificationItem = pNotificationItem;

        $scope.quizzmatesItem = quizzmatesItem;
        $scope.quizzmates = quizzmates;

        $scope.helpControl = helpControl;
        $scope.quizzmateControl = quizzmateControl;
    }
})();