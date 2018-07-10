interface IMcChoiceSvc {
    createNewMcChoice: (model: IMCChoiceModel, success?: Function, error?: Function) => IMCChoiceModel;
    updateMcChoice: (model: IMCChoiceModel, success?: Function, error?: Function) => void;
    deleteMcChoice: (id: number, success?: Function, error?: Function) => void;
}

module mcChoiceSvc {
    export interface IResource {
        mcChoice: any;
    }
}

(function () {
    l2lApp.service('mcChoiceSvc', mcChoiceSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): mcChoiceSvc.IResource {
        var qaAnswerResource = $resource(setting.serverUrl() + '/api/mcChoice', null,
            {
                'post': { method: 'POST', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'delete': { method: 'DELETE', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        return {
            mcChoice: qaAnswerResource
        }
    }

    mcChoiceSvc.$inject = ["$resource", "currentUser"];
    function mcChoiceSvc($resource, currentUser: ICurrentUser): IMcChoiceSvc {

        function createNewMcChoice(model: IMCChoiceModel, success?: Function, error?: Function): IMCChoiceModel {
            return <IMCChoiceModel>resource($resource, currentUser).mcChoice.post(model, success, error);
        }

        function updateMcChoice(model: IMCChoiceModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).mcChoice.patch(model, success, error);
        }

        function deleteMcChoice(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).mcChoice.delete({ id: id }, success, error);
        }

        return {
            createNewMcChoice: createNewMcChoice,
            updateMcChoice: updateMcChoice,
            deleteMcChoice: deleteMcChoice
        }
    }
})();