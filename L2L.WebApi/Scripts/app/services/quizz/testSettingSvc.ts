interface ITestSettingSvc {
    updateTestSetting: (model: ITestSettingModel, success?: Function, error?: Function) => void;
}

module testSettingSvc {
    export interface IResource {
        testSetting: any;
    }
}

(function () {
    l2lApp.service("testSettingSvc", testSettingSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): testSettingSvc.IResource {
        var testSetting = $resource(setting.serverUrl() + '/api/testSetting', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'post': { method: 'POST', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'delete': { method: 'DELETE', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                
            });

        return {
            testSetting: testSetting
        }
    }

    testSettingSvc.$inject = ["$resource", "currentUser"];
    function testSettingSvc($resource, currentUser: ICurrentUser): ITestSettingSvc{
        function updateTestSetting(model: IQuestionModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).testSetting.patch(model, success, error);
        }

        return {
            updateTestSetting: updateTestSetting
        }
    }
})();