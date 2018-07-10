interface INotificationSvc {
    success: (message: string) => void;
    error: (message: string) => void;
}

(function () {
    l2lApp.service('notificationSvc', notificationSvc);

    function notificationSvc(): INotificationSvc{
        function success(message: string) : void {
            toastr.success(message);
        }

        function error(message: string): void {
            //toastr.error(message);
        }

        return {
            success: success,
            error: error
        }
    }
})();