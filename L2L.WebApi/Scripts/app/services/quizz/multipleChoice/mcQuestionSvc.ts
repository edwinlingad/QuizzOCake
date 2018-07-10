interface IMCQuestionSvc {
    getQuestion: (id: number, success?: Function, error?: Function) => IMCQuestionModel;
    updateQuestion: (model: IMCQuestionModel, success?: Function, error?: Function) => void;
}

module mcQuestionSvc {
    export interface IResource {
        question: any;
    }
}

(function () {
    l2lApp.service('mcQuestionSvc', mcQuestionSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): mcQuestionSvc.IResource {
        var questionResource = $resource(setting.serverUrl() + '/api/mcQuestion', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        return {
            question: questionResource
        }
    }

    mcQuestionSvc.$inject = ["$resource", "currentUser"];
    function mcQuestionSvc($resource, currentUser: ICurrentUser): IMCQuestionSvc {
        function getQuestion(id: number, success?: Function, error?: Function): IMCQuestionModel {
            return <IMCQuestionModel>resource($resource, currentUser).question.get({ id: id }, success, error);
        }
        function updateQuestion(model: IMCQuestionModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).question.patch(model, success, error);
        }

        return {
            getQuestion: getQuestion,
            updateQuestion: updateQuestion
        }
    }
})();