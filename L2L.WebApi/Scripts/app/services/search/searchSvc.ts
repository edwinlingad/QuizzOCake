interface ISearchSvc {
    search: (search: string, success?: Function, error?: Function) => ISearchModel[];
}

module searchSvc {
    export interface IResource {
        searchResource: any;
    }
}

(function () {
    l2lApp.service("searchSvc", searchSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): searchSvc.IResource {
        var searchResource = $resource('/api/search', null,
            {
                'query': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        return {
            searchResource: searchResource
        }
    }

    searchSvc.$inject = ["$resource", "currentUser"];
    function searchSvc($resource, currentUser: ICurrentUser): ISearchSvc {
        function search(search: string, success?: Function, error?: Function): ISearchModel[] {
            return <ISearchModel[]>resource($resource, currentUser).searchResource.query({ search: search }, success, error);
        }

        return {
            search: search
        }
    }
})();