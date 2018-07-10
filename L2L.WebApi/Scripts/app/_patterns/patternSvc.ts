// To replace: 
//  - IPatternSvc
//  - patternSvc
//  - patternResource
//  - PatternModel

interface IPatternSvc {
    getPatterns: (id: number, success?: Function, error?: Function) => IPatternModel[];
    getPattern: (id: number, success?: Function, error?: Function) => IPatternModel;
    createPattern: (model: IPatternModel, success?: Function, error?: Function) => IPatternModel;
    updatePattern: (model: IPatternModel, success?: Function, error?: Function) => void;
    deletePattern: (id: number, success?: Function, error?: Function) => void;
}

module patternSvc {
    export interface IResource {
        patternResource: any;
    }
}

(function () {
    l2lApp.service("patternSvc", patternSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): patternSvc.IResource {
        var patternResource = $resource('/api/pattern', null,
            {
                'query': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'post': { method: 'POST', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'delete': { method: 'DELETE', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });


        return {
            patternResource: patternResource
        }
    }

    patternSvc.$inject = ["$resource", "currentUser"];
    function patternSvc($resource, currentUser: ICurrentUser): IPatternSvc {
        function getPatterns(id: number, success?: Function, error?: Function): IPatternModel[] {
            return <IPatternModel[]> resource($resource, currentUser).patternResource.query({ id: id }, success, error);
        }

        function getPattern(id: number, success?: Function, error?: Function): IPatternModel {
            return <IPatternModel> resource($resource, currentUser).patternResource.get({ id: id }, success, error);
        }

        function createPattern(model: IPatternModel, success?: Function, error?: Function): IPatternModel {
            return <IPatternModel>resource($resource, currentUser).patternResource.post(model, success, error);
        }

        function updatePattern(model: IPatternModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).patternResource.patch(model, success, error);
        }

        function deletePattern(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).patternResource.delete({ id: id }, success, error);
        }

        return {
            getPatterns: getPatterns,
            getPattern: getPattern,
            createPattern: createPattern,
            updatePattern: updatePattern,
            deletePattern: deletePattern
        }
    }
})();