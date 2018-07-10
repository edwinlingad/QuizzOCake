enum bcEventTypeEnum {
    newItem,
    modifyItem
}

interface IBroadcastData {
    eventType: bcEventTypeEnum;
    model: any;
}

interface IBroadcastSvc {
    //postTestLogEvent: (bcData: IBroadcastData) => void;
    //addTestLogListener: ($scope, listener: (event, bcData: IBroadcastData) => void) => void;

    //postMyQuizzEvent: (bcData: IBroadcastData) => void;
    //addMyQuizzListener: ($scope, listener: (event, bcData: IBroadcastData) => void) => void;

    //postMyDependentEvent: (bcData: IBroadcastData) => void;
    //addMyDependentListener: ($scope, listener: (event, bcData: IBroadcastData) => void) => void;

    postUpdateLayoutEvent: () => void;
    addUpdateLayoutListener: ($scope, listener: (event) => void) => void;
}


(function () {
    l2lApp.service("broadcastSvc", broadcastSvc);

    broadcastSvc.$inject = ["$rootScope"];
    function broadcastSvc($rootScope): IBroadcastSvc {
        //function postTestLogEvent(bcData: IBroadcastData): void {
        //    $rootScope.$broadcast("testLog", bcData);
        //}

        //function addTestLogListener($scope, listener: (event, bcData: IBroadcastData) => void): void {
        //    var cleanUpFunc = $rootScope.$on("testLog", listener);
        //    $scope.$on("$destroy",
        //        function () {
        //            cleanUpFunc();
        //        });
        //}

        //function postMyQuizzEvent(bcData: IBroadcastData): void {
        //    $rootScope.$broadcast("myQuizz", bcData);
        //}

        //function addMyQuizzListener($scope, listener: (event, bcData: IBroadcastData) => void): void {
        //    var cleanUpFunc = $rootScope.$on("myQuizz", listener);
        //    $scope.$on("$destroy",
        //        function () {
        //            cleanUpFunc();
        //        });
        //}

        //function postMyDependentEvent(bcData: IBroadcastData): void {
        //    $rootScope.$broadcast("myDependent", bcData);
        //}

        //function addMyDependentListener($scope, listener: (event, bcData: IBroadcastData) => void): void {
        //    var cleanUpFunc = $rootScope.$on("myDependent", listener);
        //    $scope.$on("$destroy",
        //        function () {
        //            cleanUpFunc();
        //        });
        //}

        function postUpdateLayoutEvent(): void {
            $rootScope.$broadcast("updateLayout");
        }

        function addUpdateLayoutListener($scope, listener: (event) => void): void {
            var cleanUpFunc = $rootScope.$on("updateLayout", listener);
            $scope.$on("$destroy",
                function () {
                    cleanUpFunc();
                });
        }

        return {
            //postTestLogEvent: postTestLogEvent,
            //addTestLogListener: addTestLogListener,
            //postMyQuizzEvent: postMyQuizzEvent,
            //addMyQuizzListener: addMyQuizzListener,
            //postMyDependentEvent: postMyDependentEvent,
            //addMyDependentListener: addMyDependentListener,
            postUpdateLayoutEvent: postUpdateLayoutEvent,
            addUpdateLayoutListener: addUpdateLayoutListener
        }
    }

})();