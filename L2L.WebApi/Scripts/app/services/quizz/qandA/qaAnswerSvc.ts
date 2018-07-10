interface IQaAnswerSvc {
    createNewQaAnswer: (model: IQAAnswerModel, success?: Function, error?: Function) => IQAAnswerModel;
    updateQaAnswer: (model: IQAAnswerModel, success?: Function, error?: Function) => void;
    deleteQaAnswer: (id: number, success?: Function, error?: Function) => void;
}

module qaAnswerSvc {
    export interface IResource {
        qaAnswer: any;
    }
}

(function () {
    l2lApp.service('qaAnswerSvc', qaAnswerSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): qaAnswerSvc.IResource {
        var qaAnswerResource = $resource(setting.serverUrl() + '/api/qaAnswer', null,
            {
                'post': { method: 'POST', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'delete': { method: 'DELETE', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        return {
            qaAnswer: qaAnswerResource
        }
    }

    qaAnswerSvc.$inject = ["$resource", "currentUser"];
    function qaAnswerSvc($resource, currentUser: ICurrentUser): IQaAnswerSvc {

        function createNewQaAnswer(model: IQAAnswerModel, success?: Function, error?: Function): IQAAnswerModel {
            return <IQAAnswerModel>resource($resource, currentUser).qaAnswer.post(model, success, error);
        }

        function updateQaAnswer(model: IQAAnswerModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).qaAnswer.patch(model, success, error);
        }

        function deleteQaAnswer(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).qaAnswer.delete({ id: id }, success, error);
        }

        return {
            createNewQaAnswer: createNewQaAnswer,
            updateQaAnswer: updateQaAnswer,
            deleteQaAnswer: deleteQaAnswer
        }
    }
})();