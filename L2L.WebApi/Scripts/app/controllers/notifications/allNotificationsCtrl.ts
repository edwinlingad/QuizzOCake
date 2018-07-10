module allNotificationsCtrl {
    export interface IScope {
        user: IUserModel;
        notificationItem: IItemToWait;
        notifications: INewNotificationModel[];

        helpControl: any;
    }
}

(function () {
    l2lControllers.controller("allNotificationsCtrl", allNotificationsCtrl);

    allNotificationsCtrl.$inject = ["$scope", "notificationSvc", "userNotificationSvc", "$anchorScroll", "currentUser"];
    function allNotificationsCtrl(
        $scope: allNotificationsCtrl.IScope,
        notificationSvc: INotificationSvc,
        userNotificationSvc: IUserNotificationSvc,
        $anchorScroll: ng.IAnchorScrollService,
        currentUser: ICurrentUser
        ) {

        var user: IUserModel = currentUser.getUserData();
        var notificationItem: IItemToWait = { isReady: true };
        var notifications: INewNotificationModel[] = new Array<INewNotificationModel>();
        var count: number = 0;
        var currPage: number = 1;
        var numPerPage: number = 20;

        function init() {
            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function getNotifications(data: INewNotificationModel[]) {
                data.forEach(function (item) {
                    item.isNewOrigValue = item.isNew;
                    notifications.push(item);
                });

                count -= numPerPage;
                currPage++;

                if(count >0) {
                    userNotificationSvc.getAllNotifications(
                        currPage, numPerPage, getNotifications, errorLoad);
                } else {
                    notificationItem.isReady = true;
                }
            }

            function successCount(data: any) {
                count = data.count;

                count = count > 50 ? 50 : count;

                userNotificationSvc.getAllNotifications(
                    currPage, numPerPage, getNotifications, errorLoad);
            }

            $anchorScroll("top");
            notificationItem.isReady = false;
            count = userNotificationSvc.getAllNotificationsCount(successCount, errorLoad);
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

        init();

        $scope.user = user;
        $scope.notifications = notifications;
        $scope.notificationItem = notificationItem;

        $scope.helpControl = helpControl;
    }
})();