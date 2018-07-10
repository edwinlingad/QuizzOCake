interface ITestLogSvc {
    getTestLog: (id: number, success?: Function, error?: Function) => ITestLogModel;
    createNewTestLog: (model: IQAAnswerModel, success?: Function, error?: Function) => ITestLogModel;
    deleteTestLog: (id: number, success?: Function, error?: Function) => void;
    getTestLogGrouped: (id: number, success?: Function, error?: Function) => ITestLogGroupedModel[];

} 

module testLogSvc {
    export interface IResource {
        testLog: any;
        testLogGrouped: any;
    }
}

(function () {
    l2lApp.service("testLogSvc", testLogSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): testLogSvc.IResource {
        var testLog = $resource(setting.serverUrl() + '/api/testLog', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'post': { method: 'POST', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'delete': { method: 'DELETE', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });
        var testLogGrouped = $resource(setting.serverUrl() + '/api/testLog/GetTestLogsGrouped', null,
            {
                'query': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        return {
            testLog: testLog,
            testLogGrouped: testLogGrouped
        }
    }

    testLogSvc.$inject = ["$resource", "currentUser"];
    function testLogSvc($resource, currentUser: ICurrentUser): ITestLogSvc {
        function createNewTestLog(model: ITestLogModel, success?: Function, error?: Function): ITestLogModel {
            return <ITestLogModel>resource($resource, currentUser).testLog.post(model, success, error);
        }

        function getTestLog(id: number, success?: Function, error?: Function): ITestLogModel {
            return <ITestLogModel>resource($resource, currentUser).testLog.get({ id: id }, success, error);
        }

        function deleteTestLog(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).testLog.delete({ id: id }, success, error);
        }

        function getTestLogGrouped(id: number, success?: Function, error?: Function): ITestLogGroupedModel[] {
            return <ITestLogGroupedModel[]>resource($resource, currentUser).testLogGrouped.query({ id: id }, success, error);
        }

        return {
            getTestLog: getTestLog,
            createNewTestLog: createNewTestLog,
            deleteTestLog: deleteTestLog,
            getTestLogGrouped: getTestLogGrouped
        }
    }
})();