// To replace: 
//  - IBuiltInQuestionsSvc
//  - builtInQuestionsSvc
//  - questionResource
//  - PatternModel

interface IBuiltInQuestionsSvc {
    getTest: (id: number, numQuestions: number, success?: Function, error?: Function) => ITakeTestModel;
    getFlashCards: (id: number, numQuestions: number, success?: Function, error?: Function) => IReviewerFromQuestionModel[];
}

module builtInQuestionsSvc {
    export interface IResource {
        questionResource: any;
        flashCardResource: any;
    }
}

(function () {
    l2lApp.service("builtInQuestionsSvc", builtInQuestionsSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): builtInQuestionsSvc.IResource {
        var questionResource = $resource(setting.serverUrl() + '/api/BuiltInQuestions', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });
        var flashCardResource = $resource(setting.serverUrl() + '/api/BuiltInQuestions/GetFlashCards', null,
            {
                'get': { method: 'GET', isArray: true,  headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        return {
            questionResource: questionResource,
            flashCardResource: flashCardResource
        }
    }

    builtInQuestionsSvc.$inject = ["$resource", "currentUser"];
    function builtInQuestionsSvc($resource, currentUser: ICurrentUser): IBuiltInQuestionsSvc {
        function getTest(id: number, numQuestions: number, success?: Function, error?: Function): ITakeTestModel {
            return <ITakeTestModel> resource($resource, currentUser).questionResource.get({ id: id, numQuestions: numQuestions }, success, error);
        }

        function getFlashCards(id: number, numQuestions: number, success?: Function, error?: Function): IReviewerFromQuestionModel[] {
            return <IReviewerFromQuestionModel[]> resource($resource, currentUser).flashCardResource.query({ id: id, numQuestions: numQuestions }, success, error);
        }

        return {
            getTest: getTest,
            getFlashCards: getFlashCards
        }
    }
})();