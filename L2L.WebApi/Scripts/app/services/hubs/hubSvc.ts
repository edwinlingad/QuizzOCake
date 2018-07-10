interface IHubSvc {
    joinLayoutGroup(groupName: string): void;

    joinQuizzmatemsgGroup(groupName: string, aqmThreadId: number, userId: number): void;
    leaveQuizzmateMsgGroup(groupName: string, aqmThreadId: number, userId: number): void;
    registerLayouteMsgListener(cb): void;
    sendQuizzmateMsg(tId: number, tgName: string, userId: number, message: string);

    registerQuizzmateMsgListener(tId: number, cb): void;
    unRegisterQuizzmateMsgListener(): void;
    getCurrQuizzmateThreadId(): number;

    reconnectHub(): void;
}

(function () {
    l2lApp.service("hubSvc", hubSvc);

    hubSvc.$inject = ["$resource", "currentUser", "notificationSvc"];
    function hubSvc(
        $resource,
        currentUser: ICurrentUser,
        notificationSvc: INotificationSvc): IHubSvc {

        var hub: any;
        var cbQuizzmateMsg: any;
        var cbLayoutMsgReceiver: any;
        var curQmThreadId: number = 0;

        function init() {
            hub = $.connection.cakeHub;

            hub.client.broadcastQuizzmateMsg = quizzmateMsgReceiver;
            hub.client.layoutMsg = layoutMsgReceiver;
            $.connection.hub.start().done(successInitHub)
                .fail(function () {
                    notificationSvc.error(str.errorLoad);
                });

            //$scope.$on("$destroy", destroy);
        }

        function layoutMsgReceiver(sType: number, iParam: number, sParam: string) {
            if (cbLayoutMsgReceiver !== undefined)
                cbLayoutMsgReceiver(sType, iParam, sParam);
        }

        function registerLayouteMsgListener(cb) {
            cbLayoutMsgReceiver = cb;
        }

        function quizzmateMsgReceiver(userId, message) {
            if (cbQuizzmateMsg !== undefined)
                cbQuizzmateMsg(userId, message);
        }

        function sendQuizzmateMsg(tId: number, tgName: string, userId: number, message: string) {
            hub.server.sendQuizzmateMsg(tId, tgName, userId, message);
        }

        function registerQuizzmateMsgListener(tId: number, cb) {
            curQmThreadId = tId;
            cbQuizzmateMsg = cb;
        }

        function unRegisterQuizzmateMsgListener() {
            curQmThreadId = 0;
            cbQuizzmateMsg = undefined;
        }

        function getCurrQuizzmateThreadId(): number {
            return curQmThreadId;
        }

        function joinQuizzmatemsgGroup(groupName: string, aqmThreadId: number, userId: number) {
            hub.server.joinQuizzmatemsgGroup(groupName, aqmThreadId, userId);
        }

        function leaveJoinQuizzmatemsgGroup(groupName: string, aqmThreadId: number, userId: number) {
            hub.server.leaveJoinQuizzmatemsgGroup(groupName, aqmThreadId, userId);
        }

        function joinLayoutGroup(groupName: string): void {

            reconnectHub(function () {
                hub.server.joinLayoutGroup(groupName);
            });
        }

        function destroy() {
            $.connection.hub.stop();
        }

        function successInitHub() {
        }

        function reconnectHub(success?: Function) {
            $.connection.hub.stop();
            var cb: any = success === null ? successInitHub : success;
            $.connection.hub.start().done(cb)
                .fail(function () {
                    notificationSvc.error(str.errorLoad);
                });
        }

        init();

        return {
            joinQuizzmatemsgGroup: joinQuizzmatemsgGroup,
            leaveQuizzmateMsgGroup: leaveJoinQuizzmatemsgGroup,
            registerLayouteMsgListener: registerLayouteMsgListener,
            sendQuizzmateMsg: sendQuizzmateMsg,
            registerQuizzmateMsgListener: registerQuizzmateMsgListener,
            unRegisterQuizzmateMsgListener: unRegisterQuizzmateMsgListener,
            getCurrQuizzmateThreadId: getCurrQuizzmateThreadId,
            joinLayoutGroup: joinLayoutGroup,
            reconnectHub: reconnectHub
        }
    }
})();