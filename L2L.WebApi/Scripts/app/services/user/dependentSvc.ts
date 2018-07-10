interface IDependentSvc {
    getDependentInfo: (id: number, success?: Function, error?: Function) => IDependentModel;
    updatePermissions: (model: IDependentPermissionModel, success?: Function, error?: Function) => void;
    updateNotifications: (model: IDependentNotificationModel, success?: Function, error?: Function) => void;
}

module dependentSvc {
    export interface IResource {
        dependent: any;
        permission: any;
        notification: any;
    }
}

(function () {
    l2lApp.service("dependentSvc", dependentSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): dependentSvc.IResource {
        var dependent = $resource(setting.serverUrl() + '/api/Dependent/GetDependentInfo', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });
        var permission = $resource(setting.serverUrl() + '/api/Dependent/UpdatePermissions', null,
            {
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });
        var notification = $resource(setting.serverUrl() + '/api/Dependent/UpdateNotifications', null,
            {
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        return {
            dependent: dependent,
            permission: permission,
            notification: notification
        }
    }

    dependentSvc.$inject = ["$resource", "currentUser"];
    function dependentSvc(
        $resource,
        currentUser: ICurrentUser
        ): IDependentSvc {

        function getDependentInfo(id: number, success?: Function, error?: Function): IDependentModel {
            return <IDependentModel>resource($resource, currentUser).dependent.get({ id: id }, success, error);
        }

        function updatePermissions(model: IDependentPermissionModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).permission.patch(model, success, error);
        }

        function updateNotifications(model: IDependentNotificationModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).notification.patch(model, success, error);
        }

        return {
            getDependentInfo: getDependentInfo,
            updatePermissions: updatePermissions,
            updateNotifications: updateNotifications
        }
    }
})();