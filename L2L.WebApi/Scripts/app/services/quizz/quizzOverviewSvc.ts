//interface IQuizzSearchCondition {
//    category?: string;
//    pageNum?: number;
//    numPerPage?: number;
//    sortBy?: number;
//    sortType?: number;
//}

interface IQuizzOverviewSvc {
    getQuizzOverview: (id: number, success?: Function, error?: Function) => IQuizzOverviewModel;
    getQuizzOverviews: (search: any, success?: Function, error?: Function) => IQuizzOverviewModel[];
    getQuizzOverviewListResult: (search: any, success?: Function, error?: Function) => IQuizzOverviewListResult;
}

module quizzOverviewSvc {
    export interface IResource {
        quizzOverview: any;
        getOverviews: any;
    }
}

(function () {
    l2lApp.service('quizzOverviewSvc', quizzOverviewSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): quizzOverviewSvc.IResource {
        var quizzOverview = $resource(setting.serverUrl() + '/api/quizzOverview', null,
            {
                'query': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });
        var getOverviews = $resource(setting.serverUrl() + '/api/quizzOverview/GetOverviews', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        return {
            quizzOverview: quizzOverview,
            getOverviews: getOverviews
        }
    }

    quizzOverviewSvc.$inject = ["$resource", "currentUser"];
    function quizzOverviewSvc($resource, currentUser: ICurrentUser): IQuizzOverviewSvc {
        function getQuizzOverviews(search: any, success?: Function, error?: Function): IQuizzOverviewModel[] {
            return <IQuizzOverviewModel[]>resource($resource, currentUser).quizzOverview.query(search, success, error);
        }

        function getQuizzOverview(id: number, success?: Function, error?: Function): IQuizzOverviewModel {
            return <IQuizzOverviewModel>resource($resource, currentUser).quizzOverview.get({ id }, success, error);
        }

        function getQuizzOverviewListResult(search: any, success?: Function, error?: Function): IQuizzOverviewListResult {
            return <IQuizzOverviewListResult>resource($resource, currentUser).getOverviews.get(search, success, error);
        }

        return {
            getQuizzOverviews: getQuizzOverviews,
            getQuizzOverview: getQuizzOverview,
            getQuizzOverviewListResult: getQuizzOverviewListResult
        }
    }
})();