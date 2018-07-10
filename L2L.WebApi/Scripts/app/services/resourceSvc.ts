interface IResourceSvc {
    getResourceMany(rType: enums.ResourceTypeEnum, id: number, id2: number, id3: number, id4: number, id5: number, success?: Function, error?: Function): any[];
    getResourceManyAlt(rType: enums.ResourceTypeEnum, id: number, id2: number, id3: number, str: string, str2: string, str3: string, success?: Function, error?: Function): any[];
    getResource(rType: enums.ResourceTypeEnum, id: number, success?: Function, error?: Function): any;
    getResourceAlt(rType: enums.ResourceTypeEnum, str: string, success?: Function, error?: Function): any;
    createResource(rType: enums.ResourceTypeEnum, model: any, success?: Function, error?: Function): any;
    updateResource(rType: enums.ResourceTypeEnum, model: any, success?: Function, error?: Function): void;
    deleteResource(rType: enums.ResourceTypeEnum, id: number, success?: Function, error?: Function): void;
}

module resourceSvc {
    export interface IResource {
        resourceRsc: any;
        resourceRscAlt: any;
    }
}

(function () {
    l2lApp.service("resourceSvc", resourceSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): resourceSvc.IResource {
        var resourceResource = $resource(setting.serverUrl() + '/api/resource', null,
            {
                'query': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'save': { method: 'POST', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'delete': { method: 'DELETE', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        var resourceResourceAlt = $resource(setting.serverUrl() + '/api/resource/GetAlt', null,
            {
                'query': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        return {
            resourceRsc: resourceResource,
            resourceRscAlt: resourceResourceAlt
        }
    }

    resourceSvc.$inject = ["$resource", "currentUser"];
    function resourceSvc($resource, currentUser: ICurrentUser): IResourceSvc {
        function getResourceMany(rType: enums.ResourceTypeEnum, id: number, id2: number, id3: number, id4: number, id5: number, success?: Function, error?: Function): any[] {
            return <any[]> resource($resource, currentUser).resourceRsc.query({ rType: rType, id: id, id2: id2, id3: id3, id4: id4, id5: id5 }, success, error);
        }

        function getResourceManyAlt(rType: enums.ResourceTypeEnum, id: number, id2: number, id3: number, str: string, str2: string, str3: string, success?: Function, error?: Function): any[] {
            return <any[]>resource($resource, currentUser).resourceRscAlt.query({ rType: rType, id: id, id2: id2, id3: id3, str: str, str2: str2, str3: str3 }, success, error);
        }

        function getResource(rType: enums.ResourceTypeEnum, id: number, success?: Function, error?: Function): any {
            return <any> resource($resource, currentUser).resourceRsc.get({ rType: rType, id: id }, success, error);
        }

        function getResourceAlt(rType: enums.ResourceTypeEnum, str: string, success?: Function, error?: Function): any {
            return <any>resource($resource, currentUser).resourceRscAlt.get({ rType: rType, str: str }, success, error);
        }

        function createResource(rType: enums.ResourceTypeEnum, model: any, success?: Function, error?: Function): any {
            return <any>resource($resource, currentUser).resourceRsc.save({ rType: rType }, model, success, error);
        }

        function updateResource(rType: enums.ResourceTypeEnum, model: any, success?: Function, error?: Function): void {
            resource($resource, currentUser).resourceRsc.patch({ rType: rType } , model, success, error);
        }

        function deleteResource(rType: enums.ResourceTypeEnum, id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).resourceRsc.delete({ rType: rType, id: id }, success, error);
        }

        return {
            getResourceMany: getResourceMany,
            getResourceManyAlt: getResourceManyAlt,
            getResource: getResource,
            getResourceAlt: getResourceAlt,
            createResource: createResource,
            updateResource: updateResource,
            deleteResource: deleteResource
        }
    }
})();