interface IFlashCardSvc {
    getReviewerFromQuestions(id: number, success?: Function, error?: Function);
}

(function () {
    l2lApp.service('flashCardSvc', flashCardSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser) {
        var reviewerFromQuestions = $resource(setting.serverUrl() + '/api/ReviewerFromQuestions', null, {
            'get': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
        });
        return {
            reviewerFromQuestions: reviewerFromQuestions
        };
    }

    flashCardSvc.$inject = ["$resource", "currentUser"];
    function flashCardSvc($resource, currentUser) {
        function getReviewerFromQuestions(id, success, error) {
            return resource($resource, currentUser).reviewerFromQuestions.query({ id: id }, success, error);
        }
        return {
            getReviewerFromQuestions: getReviewerFromQuestions
        };
    }
})();
