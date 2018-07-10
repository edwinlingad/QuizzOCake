interface IQuizzUserRatingSvc {
    getQuizzUserRating: (id: number, success?: Function, error?: Function) => IQuizzUserRatingModel;
    updateQuizzUserRating: (model: IQuizzUserRatingUpdateRateModel, success?: Function, error?: Function) => void;
}

module quizzUserRatingSvc {
    export interface IResource {
        quizzUserRatingResource: any;
        quizzRatingResource: any;
    }
}

(function () {
    l2lApp.service("quizzUserRatingSvc", quizzUserRatingSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): quizzUserRatingSvc.IResource {
        var quizzUserRatingResource = $resource(setting.serverUrl() + '/api/UserRating', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        var quizzRatingResource = $resource(setting.serverUrl() + '/api/Notification/GetNewNotifications', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        return {
            quizzUserRatingResource: quizzUserRatingResource,
            quizzRatingResource: quizzRatingResource
        }
    }

    quizzUserRatingSvc.$inject = ["$resource", "currentUser"];
    function quizzUserRatingSvc($resource, currentUser: ICurrentUser): IQuizzUserRatingSvc {
    
        function getQuizzUserRating(id: number, success?: Function, error?: Function): IQuizzUserRatingModel {
            return <IQuizzUserRatingModel> resource($resource, currentUser).quizzUserRatingResource.get(id, success, error);
        }

        function updateQuizzUserRating(model: IQuizzUserRatingUpdateRateModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).quizzUserRatingResource.patch(model, success, error);
        }

        return {
            getQuizzUserRating: getQuizzUserRating,
            updateQuizzUserRating: updateQuizzUserRating,
        }
    }
})();