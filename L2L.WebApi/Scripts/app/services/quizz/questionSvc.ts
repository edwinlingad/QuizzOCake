interface IQuestionSvc {
    createQuestion: (model: ICreateQuestionModel, success?: Function, error?: Function) => IQuestionModel;
    updateQuestion: (model: IQuestionModel, success?: Function, error?: Function) => void;
    deleteQuestion: (id: number, success?: Function, error?: Function) => void;

    includeInFlashCards: (id: number, success?: Function, error?: Function) => void;
    excludeInFlashCards: (id: number, success?: Function, error?: Function) => void;
}

module questionSvc {
    export interface IResource {
        question: any;
        includeInFlashCards: any;
        excludeInFlashCards: any;
    }
}

(function () {
    l2lApp.service('questionSvc', questionSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): questionSvc.IResource {
        var questionResource = $resource(setting.serverUrl() + '/api/question', null,
            {
                'post': { method: 'POST', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'delete': { method: 'DELETE', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        var includeInFlashCards = $resource(setting.serverUrl() + '/api/question/IncludeInFlashCards', null,
            {
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });
        var excludeInFlashCards = $resource(setting.serverUrl() + '/api/question/ExcludeInFlashCards', null,
            {
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        return {
            question: questionResource,
            includeInFlashCards: includeInFlashCards,
            excludeInFlashCards: excludeInFlashCards
        }
    }

    questionSvc.$inject = ["$resource", "currentUser"];
    function questionSvc($resource, currentUser: ICurrentUser): IQuestionSvc {
        function createQuestion(model: ICreateQuestionModel, success?: Function, error?: Function): IQuestionModel {
            return <IQuestionModel>resource($resource, currentUser).question.post(model, success, error);
        }

        function updateQuestion(model: IQuestionModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).question.patch(model, success, error);
        }

        function deleteQuestion(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).question.delete({ id: id }, success, error);
        }

        function includeInFlashCards(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).includeInFlashCards.patch(id, success, error);
        }

        function excludeInFlashCards(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).excludeInFlashCards.patch(id, success, error);
        }

        return {
            createQuestion: createQuestion,
            updateQuestion: updateQuestion,
            deleteQuestion: deleteQuestion,
            includeInFlashCards: includeInFlashCards,
            excludeInFlashCards: excludeInFlashCards
        }
    }
})();