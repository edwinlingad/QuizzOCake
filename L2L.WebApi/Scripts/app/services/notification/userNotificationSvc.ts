interface IUserNotificationSvc {
    getNotificationTypes: (success?: Function, error?: Function) => INotificationTypeModel[];
    getNewNotificationsCount: (success?: Function, error?: Function) => number;
    getNewNotifications: (success?: Function, error?: Function) => INewNotificationModel[];
    getAllNotificationsCount: (success?: Function, error?: Function) => number;
    getAllNotifications: (pageNum: number, numPerPage: number, success?: Function, error?: Function) => INewNotificationModel[];
}

module userNotificationSvc {
    export interface IResource {
        notificationTypes: any;
        newNotificationsCount: any;
        newNotifications: any;
        allNotificationsCount: any;
        allNotifications: any;
    }
}

(function () {
    l2lApp.service("userNotificationSvc", userNotificationSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): userNotificationSvc.IResource {
        var notificationTypes = $resource(setting.serverUrl() + '/api/NotificationTypes', null,
            {
                'query': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });
        var newNotificationCount = $resource(setting.serverUrl() + '/api/Notification/GetNewNotificationsCount', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });
        var newNotification = $resource(setting.serverUrl() + '/api/Notification/GetNewNotifications', null,
            {
                'query': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });
        var allNotificationsCount = $resource(setting.serverUrl() + '/api/Notification/GetAllNotificationsCount', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });
        var allNotifications = $resource(setting.serverUrl() + '/api/Notification/GetNotifications', null,
            {
                'query': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        return {
            notificationTypes: notificationTypes,
            newNotificationsCount: newNotificationCount,
            newNotifications: newNotification,
            allNotificationsCount: allNotificationsCount,
            allNotifications: allNotifications
        }
    }

    userNotificationSvc.$inject = ["$resource", "currentUser"];
    function userNotificationSvc($resource, currentUser: ICurrentUser): IUserNotificationSvc {

        function getNewNotificationsCount(success?: Function, error?: Function): number {
            return <number>resource($resource, currentUser).newNotificationsCount.get(null, success, error);
        }

        function getNewNotifications(success?: Function, error?: Function): INewNotificationModel[] {
            return <INewNotificationModel[]> resource($resource, currentUser).newNotifications.query(null, success, error);
        }

        function getAllNotificationsCount(success?: Function, error?: Function): number {
            return <number>resource($resource, currentUser).allNotificationsCount.get(null, success, error);
        }

        function getAllNotifications(pageNum: number, numPerPage: number, success?: Function, error?: Function): INewNotificationModel[] {
            return <INewNotificationModel[]> resource($resource, currentUser).allNotifications.query({ pageNum: pageNum, numPerPage: numPerPage }, success, error);
        }

        function getNotificationTypes(success?: Function, error?: Function): INotificationTypeModel[] {
            return <INotificationTypeModel[]> resource($resource, currentUser).notificationTypes.query(null, success, error);
        }

        return {
            getNotificationTypes: getNotificationTypes,
            getNewNotificationsCount: getNewNotificationsCount,
            getNewNotifications: getNewNotifications,
            getAllNotificationsCount: getAllNotificationsCount,
            getAllNotifications: getAllNotifications
        }
    }
})();