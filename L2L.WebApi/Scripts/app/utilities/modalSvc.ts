interface IModalSvc {
    open(settings: ng.ui.bootstrap.IModalSettings, onExit: (promiseValue: any) => void): ng.ui.bootstrap.IModalServiceInstance;
}

(function () {
    l2lApp.service("modalSvc", modalSvc);

    modalSvc.$inject = ["$uibModal"];

    function modalSvc($uibModal: ng.ui.bootstrap.IModalService): IModalSvc {
        function open(setting: ng.ui.bootstrap.IModalSettings, onExit: (promiseValue: any) => void): ng.ui.bootstrap.IModalServiceInstance {
            setting.backdrop = "static";
            var ret: ng.ui.bootstrap.IModalServiceInstance = $uibModal.open(setting);
            ret.result.then(onExit);

            return ret;
        }

        return {
            open: open
        }
    }
})();