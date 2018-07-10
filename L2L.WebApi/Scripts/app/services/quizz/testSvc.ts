interface ITestSvc {
    getTest: (id: number, success?: Function, error?: Function) => ITestModel;
    getTakeTestById: (id: number, success?: Function, error?: Function) => ITakeTestModel;
}

module testSvc {
    export interface IResource {
        test: any;
        takeTest: any;
    }
}

(function () {
    l2lApp.service('testSvc', testSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): testSvc.IResource {
        var testResource = $resource(setting.serverUrl() + '/api/test', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });
        var getTakeTestResource = $resource(setting.serverUrl() + '/api/test/getTakeTestModel', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        return {
            test: testResource,
            takeTest: getTakeTestResource
        }
    }
    
    testSvc.$inject = ["$resource", "currentUser"];
    function testSvc($resource, currentUser: ICurrentUser): ITestSvc {
        function getTestById(id: number, success?: Function, error?: Function): ITestModel {
            return <ITestModel>resource($resource, currentUser).test.get({ id: id }, success, error);
        }

        function getTakeTestById(id: number, success?: Function, error?: Function): ITakeTestModel {
            return <ITakeTestModel>resource($resource, currentUser).takeTest.get({ id: id }, success, error);
        }

        return {
            getTest: getTestById,
            getTakeTestById: getTakeTestById
        }
    }
})();

