interface ITextFlashCardSvc {
    getTextFlashCards: (id: number, success?: Function, error?: Function) => ITextFlashCardModel[];
    getTextFlashCard: (id: number, success?: Function, error?: Function) => ITextFlashCardModel;
    createNewTextFlashCard: (model: ITextFlashCardModel, success?: Function, error?: Function) => void;
    updateTextFlashCard: (model: ITextFlashCardModel, success?: Function, error?: Function) => void;
    deleteTextFlashCard: (id: number, success?: Function, error?: Function) => void;
}

module textFlashCardsSvc {
    export interface IResource {
        textFlashCards: any;
        textFlashCard: any;
    }
}

(function () {
    l2lApp.service('textFlashCardsSvc', textFlashCardsSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): textFlashCardsSvc.IResource {
        var textFlashCards = $resource(setting.serverUrl() + '/api/TextFlashCards/GetTextFlashCards', null,
            {
                'get': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });
        var textFlashCard = $resource(setting.serverUrl() + '/api/TextFlashCards', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'post': { method: 'POST', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'delete': { method: 'DELETE', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        return {
            textFlashCards: textFlashCards,
            textFlashCard: textFlashCard
        }
    }

    textFlashCardsSvc.$inject = ["$resource", "currentUser"];
    function textFlashCardsSvc($resource, currentUser: ICurrentUser): ITextFlashCardSvc {
        function getTextFlashCards(id: number, success?: Function, error?: Function): ITextFlashCardModel[] {
            return <ITextFlashCardModel[]>resource($resource, currentUser).textFlashCards.query({ id: id }, success, error);
        }

        function getTextFlashCard(id: number, success?: Function, error?: Function): ITextFlashCardModel {
            return <ITextFlashCardModel>resource($resource, currentUser).textFlashCard.get({ id: id }, success, error);
        }

        function createNewTextFlashCard(model: ITextFlashCardModel, success?: Function, error?: Function): void {
            return resource($resource, currentUser).textFlashCard.post(model, success, error);
        }

        function updateTextFlashCard(model: ITextFlashCardModel, success?: Function, error?: Function): void {
            return resource($resource, currentUser).textFlashCard.patch(model, success, error);
        }

        function deleteTextFlashCard(id: number, success?: Function, error?: Function): void {
            return resource($resource, currentUser).textFlashCard.delete({ id: id }, success, error);
        }

        return {
            getTextFlashCards: getTextFlashCards,
            getTextFlashCard: getTextFlashCard,
            createNewTextFlashCard: createNewTextFlashCard,
            updateTextFlashCard: updateTextFlashCard,
            deleteTextFlashCard: deleteTextFlashCard
        }
    }
})();
