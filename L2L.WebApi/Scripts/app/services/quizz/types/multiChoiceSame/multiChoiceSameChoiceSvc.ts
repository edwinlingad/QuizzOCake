interface IMultiChoiceSameChoiceSvc {
    createChoice: (model: IMultiChoiceSameChoiceModel, success?: Function, error?: Function) => IMultiChoiceSameChoiceModel;
    updateChoice: (model: IMultiChoiceSameChoiceModel, success?: Function, error?: Function) => void;
    deleteChoice: (id: number, success?: Function, error?: Function) => void;
}

module multiChoiceSameChoiceSvc {
    export interface IResource {
        choice: any;
    }
}

(function () {
    l2lApp.service('multiChoiceSameChoiceSvc', multiChoiceSameChoiceSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): multiChoiceSameChoiceSvc.IResource {
        var choiceResource = $resource(setting.serverUrl() + '/api/multiChoiceSameChoice', null,
            {
                'post': { method: 'POST', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'delete': { method: 'DELETE', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        return {
            choice: choiceResource
        }
    }

    multiChoiceSameChoiceSvc.$inject = ["$resource", "currentUser"];
    function multiChoiceSameChoiceSvc($resource, currentUser: ICurrentUser): IMultiChoiceSameChoiceSvc {

        function createChoice(model: IMultiChoiceSameChoiceModel, success?: Function, error?: Function): IMultiChoiceSameChoiceModel {
            return <IMultiChoiceSameChoiceModel>resource($resource, currentUser).choice.post(model, success, error);
        }

        function updateChoice(model: IMultiChoiceSameChoiceModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).choice.patch(model, success, error);
        }

        function deleteChoice(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).choice.delete({ id: id }, success, error);
        }

        return {
            createChoice: createChoice,
            updateChoice: updateChoice,
            deleteChoice: deleteChoice
        }
    }
})();