interface IQAQuestionSvc {
    getQuestion: (id: number, success?: Function, error?: Function) => IQAQuestionModel;
    updateQuestion : (model: IQAQuestionModel, success?: Function, error?: Function) => void;
}

module qaQuestionSvc {
    export interface IResource {
        question: any;
    }
}

(function () {
    l2lApp.service('qaQuestionSvc', qaQuestionSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): qaQuestionSvc.IResource {
        var questionResource = $resource(setting.serverUrl() + '/api/qaQuestion', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        return {
            question: questionResource
        }
    }

    qaQuestionSvc.$inject = ["$resource", "currentUser"];
    function qaQuestionSvc($resource, currentUser: ICurrentUser): IQAQuestionSvc {
        function getQuestion(id: number, success?: Function, error?: Function): IQAQuestionModel {
            return <IQAQuestionModel>resource($resource, currentUser).question.get({ id: id }, success, error);
        }
        function updateQuestion(model: IQAQuestionModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).question.patch(model, success, error);
        }

        return {
            getQuestion: getQuestion,
            updateQuestion: updateQuestion
        }
    }
})();