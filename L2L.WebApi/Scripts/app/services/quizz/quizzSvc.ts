interface IQuizzSvc {
    getQuizzById: (id: number, success?: Function, error?: Function) => IQuizzModel;
    createNewQuizz: (model: IQuizzModel, success?: Function, error?: Function) => IQuizzModel;
    updateQuizz: (model: IQuizzModel, success?: Function, error?: Function) => void;
    deleteQuizz: (id: number, success?: Function, error?: Function) => void;
    setLive: (model: IToggleElement, success?: Function, error?: Function) => void;
}

module quizzSvc {
    export interface IResource {
        quizz: any;
        live: any;
    }
}

(function () {
    l2lApp.service('quizzSvc', quizzSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser):  quizzSvc.IResource {
        var quizzResource = $resource(setting.serverUrl() + '/api/quizz', null,
            {
                'post': { method: 'POST', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'delete': { method: 'DELETE', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        var liveResource = $resource(setting.serverUrl() + '/api/quizz/live', null,
            {
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        return {
            quizz: quizzResource,
            live: liveResource
        }
    }

    quizzSvc.$inject = ["$resource", "currentUser"];
    function quizzSvc($resource, currentUser: ICurrentUser): IQuizzSvc {
        function getQuizzById(id: number, success?: Function, error?: Function): IQuizzModel  {
            return <IQuizzModel>resource($resource, currentUser).quizz.get({ id: id }, success, error);
        }

        function createNewQuizz(model: IQuizzModel, success?: Function, error?: Function): IQuizzModel {
            return <IQuizzModel>resource($resource, currentUser).quizz.post(model, success, error);
        }

        function updateQuizz(model: IQuizzModel, success?: Function, error?: Function): void {
             resource($resource, currentUser).quizz.patch(model, success, error);
        }

        function deleteQuizz(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).quizz.delete({ id: id }, success, error);
        }

        function setLive(model: IToggleElement, success?: Function, error?: Function): void {
            resource($resource, currentUser).live.patch(model, success, error);
        }

        return {
            getQuizzById: getQuizzById,
            createNewQuizz: createNewQuizz,
            updateQuizz: updateQuizz,
            deleteQuizz: deleteQuizz,
            setLive: setLive
        }
    }
})();