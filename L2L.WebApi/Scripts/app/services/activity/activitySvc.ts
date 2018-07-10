interface IActivitySvc {
    getCurrentUserActivities: (pageNum: number, numPerPage: number, skip: number, success?: Function, error?: Function) => IActivityModel[];
    getUserActivities: (id: number, pageNum: number, numPerPage: number, skip: number, success?: Function, error?: Function) => IActivityModel[];
    getQuizzmateActivities: (pageNum: number, numPerPage: number, skip: number, success?: Function, error?: Function) => IActivityModel[];
}

module activitySvc {
    export interface IResource {
        currentUserActivity: any;
        userActivity: any;
        quizzmateActivity: any;
    }
}

(function () {
    l2lApp.service("activitySvc", activitySvc);

    resource.$inject = ["$resource", "currentUser"];

    function resource($resource, currentUser): activitySvc.IResource {
        var currentUserActivity = $resource(setting.serverUrl() + "/api/Activity/GetActivitiesOfCurrentUser", null,
            {
                'query': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });
        var userActivity = $resource(setting.serverUrl() + "/api/Activity/GetActivitiesOfUser", null,
            {
                'query': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });
        var quizzmateActivity = $resource(setting.serverUrl() + "/api/Activity/GetActivitiesOfQuizzmates", null,
            {
                'query': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        return {
            currentUserActivity: currentUserActivity,
            userActivity: userActivity,
            quizzmateActivity: quizzmateActivity
        }
    }

    activitySvc.$inject = ["$resource", "currentUser"];

    function activitySvc($resource, currentUser: ICurrentUser): IActivitySvc {
        function getCurrentUserActivities(pageNum: number, numPerPage: number, skip: number, success?: Function, error?: Function): IActivityModel[] {
            return <IActivityModel[]> resource($resource, currentUser).currentUserActivity.query({ pageNum: pageNum, numPerPage: numPerPage }, success, error);
        }

        function getUserActivities(id: number, pageNum: number, numPerPage: number, skip: number, success?: Function, error?: Function): IActivityModel[]{
            return <IActivityModel[]> resource($resource, currentUser).userActivity.query({ id: id, pageNum: pageNum, numPerPage: numPerPage }, success, error);
        }

        function getQuizzmateActivities(pageNum: number, numPerPage: number, skip: number, success?: Function, error?: Function): IActivityModel[] {
            return <IActivityModel[]> resource($resource, currentUser).currentUserActivity.query({ pageNum: pageNum, numPerPage: numPerPage }, success, error);
        }

        return {
            getCurrentUserActivities: getCurrentUserActivities,
            getUserActivities: getUserActivities,
            getQuizzmateActivities: getQuizzmateActivities
        }
    }
})();