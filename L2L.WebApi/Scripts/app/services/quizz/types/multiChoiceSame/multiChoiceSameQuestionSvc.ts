interface IMultiChoiceSameQuestionSvc {
    getQuestion: (id: number, success?: Function, error?: Function) => IMultiChoiceSameQuestionModel;
    updateQuestion: (model: IMultiChoiceSameQuestionModel, success?: Function, error?: Function) => void;
}

module multiChoiceSameQuestionSvc {
    export interface IResource {
        question: any;
    }
}

(function () {
    l2lApp.service('multiChoiceSameQuestionSvc', multiChoiceSameQuestionSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): multiChoiceSameQuestionSvc.IResource {
        var questionResource = $resource(setting.serverUrl() + '/api/multiChoiceSameQuestion', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        return {
            question: questionResource
        }
    }

    multiChoiceSameQuestionSvc.$inject = ["$resource", "currentUser"];
    function multiChoiceSameQuestionSvc($resource, currentUser: ICurrentUser): IMultiChoiceSameQuestionSvc {
        function getQuestion(id: number, success?: Function, error?: Function): IMultiChoiceSameQuestionModel {
            return <IMultiChoiceSameQuestionModel>resource($resource, currentUser).question.get({ id: id }, success, error);
        }
        function updateQuestion(model: IMultiChoiceSameQuestionModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).question.patch(model, success, error);
        }

        return {
            getQuestion: getQuestion,
            updateQuestion: updateQuestion
        }
    }
})();