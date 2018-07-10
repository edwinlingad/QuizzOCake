module notificationTypeIcon {
    export interface IScope {
        notification: INewNotificationModel;
        faIcon:() => string;
        fgColor:() => string;
    }
}

(function () {
    l2lApp.directive("notificationTypeIcon", function () {
        controller.$inject = ["$scope", "cachedDataSvc"];

        function controller(
            $scope: notificationTypeIcon.IScope,
            cachedDataSvc: ICachedDataSvc
            ) {

            var notificationTypes: INotificationTypeModel[];
            function init() {
                function errorLoad(response) {
                    //notificationSvc.error(str.errorLoad);
                }

                function successNotificationTypes(data: INotificationTypeModel[]) {
                    notificationTypes = data;
                }

                notificationTypes = cachedDataSvc.getNotificationTypes(successNotificationTypes, errorLoad);
            }

            function faIcon(): string {
                return notificationTypes[$scope.notification.notificationType].faIcon;
            }

            function fgColor(): string {
                return notificationTypes[$scope.notification.notificationType].fgColor;
            }

            init();

            $scope.faIcon = faIcon;
            $scope.fgColor = fgColor;
        }

        return {
            scope: {
                notification: "=",
                addClass: "@"
            },
            template: "<i class='{{addClass}} fa {{faIcon()}}' style='color: {{fgColor()}}'></span>",
            controller: controller
        }
    });
})(); 