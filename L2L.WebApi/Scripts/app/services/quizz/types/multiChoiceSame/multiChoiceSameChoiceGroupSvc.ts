interface IMultiChoiceSameChoiceGroupSvc {
    createChoiceGroup: (model: IMultiChoiceSameChoiceGroupModel, success?: Function, error?: Function) => IMultiChoiceSameChoiceGroupModel;
    updateChoiceGroup: (model: IMultiChoiceSameChoiceGroupModel, success?: Function, error?: Function) => void;
    deleteChoiceGroup: (id: number, success?: Function, error?: Function) => void;
}

module multiChoiceSameChoiceGroupSvc {
    export interface IResource {
        choiceGroup: any;
    }
}

(function () {
    l2lApp.service('multiChoiceSameChoiceGroupSvc', multiChoiceSameChoiceGroupSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): multiChoiceSameChoiceGroupSvc.IResource {
        var choiceGroupResource = $resource(setting.serverUrl() + '/api/multiChoiceSameChoiceGroup', null,
            {
                'post': { method: 'POST', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'delete': { method: 'DELETE', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        return {
            choiceGroup: choiceGroupResource
        }
    }

    multiChoiceSameChoiceGroupSvc.$inject = ["$resource", "currentUser"];
    function multiChoiceSameChoiceGroupSvc($resource, currentUser: ICurrentUser): IMultiChoiceSameChoiceGroupSvc {

        function createChoiceGroup(model: IMultiChoiceSameChoiceGroupModel, success?: Function, error?: Function): IMultiChoiceSameChoiceGroupModel {
            return <IMultiChoiceSameChoiceGroupModel>resource($resource, currentUser).choiceGroup.post(model, success, error);
        }

        function updateChoiceGroup(model: IMultiChoiceSameChoiceGroupModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).choiceGroup.patch(model, success, error);
        }

        function deleteChoiceGroup(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).choiceGroup.delete({ id: id }, success, error);
        }

        return {
            createChoiceGroup: createChoiceGroup,
            updateChoiceGroup: updateChoiceGroup,
            deleteChoiceGroup: deleteChoiceGroup
        }
    }
})();