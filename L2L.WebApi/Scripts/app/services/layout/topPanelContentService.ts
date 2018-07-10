
interface ITopPanelContentSvc {
    vars: any;

    reset();

    addToNewQuizzClassCount(count: number);
    subFromNewQuizzClassCount(count: number);

    addToNewFriendRequestCount(count: number);
    subFromNewFriendRequestCount(count: number);

    addToNewMessageCount(count: number);
    subFromNewMessageCount(count: number);

    addToNewNotificationCount(count: number);
    subFromNewNotificationCount(count: number);
}

(function () {
    l2lApp.service("topPanelContentSvc", topPanelContentSvc);

    topPanelContentSvc.$inject = [];
    function topPanelContentSvc(): ITopPanelContentSvc {
        
        var vars = {
            newQuizzClassNotificationCount: 0,
            newFriendRequestCount: 0,
            newMessageCount: 0,
            newNotificationCount: 0
        };

        function reset() {
            vars.newQuizzClassNotificationCount = 0;
            vars.newFriendRequestCount = 0;
            vars.newMessageCount = 0;
            vars.newNotificationCount = 0;
        }

        function addToNewQuizzClassCount(count: number) {
            vars.newQuizzClassNotificationCount += count;
        }

        function subFromNewQuizzClassCount(count: number) {
            if (vars.newQuizzClassNotificationCount <= 0)
                return;
            vars.newQuizzClassNotificationCount -= count;
        }

        function addToNewFriendRequestCount(count: number) {
            vars.newFriendRequestCount += count;
        }

        function subFromNewFriendRequestCount(count: number) {
            if (vars.newFriendRequestCount <= 0)
                return;
            vars.newFriendRequestCount -= count;
        }

        function addToNewMessageCount(count: number) {
            vars.newMessageCount += count;
        }

        function subFromNewMessageCount(count: number) {
            if (vars.newMessageCount <= 0)
                return;
            vars.newMessageCount -= count;
        }

        function addToNewNotificationCount(count: number) {
            vars.newNotificationCount += count;
        }

        function subFromNewNotificationCount(count: number) {
            if (vars.newNotificationCount <= 0)
                return;
            vars.newNotificationCount -= count;
        }

        return {
            vars: vars,
            reset: reset,

            addToNewQuizzClassCount: addToNewQuizzClassCount,
            subFromNewQuizzClassCount: subFromNewQuizzClassCount,

            addToNewFriendRequestCount: addToNewFriendRequestCount,
            subFromNewFriendRequestCount: subFromNewFriendRequestCount,

            addToNewMessageCount: addToNewMessageCount,
            subFromNewMessageCount: subFromNewMessageCount,

            addToNewNotificationCount: addToNewNotificationCount,
            subFromNewNotificationCount: subFromNewNotificationCount
        }
    }
})();