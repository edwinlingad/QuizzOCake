interface IQuickNoteSvc {
    getQuickNotes: (id: number, success?: Function, error?: Function) => IQuickNoteModel[];
    getQuickNote: (id: number, success?: Function, error?: Function) => IQuickNoteModel;
    createNewQuickNote: (model: IQuickNoteModel, success?: Function, error?: Function) => void;
    updateQuickNote: (model: IQuickNoteModel, success?: Function, error?: Function) => void;
    deleteQuickNote: (id: number, success?: Function, error?: Function) => void;
}

module quickNotesSvc {
    export interface IResource {
        quickNotes: any;
        quickNote: any;
    }
}

(function () {
    l2lApp.service('quickNotesSvc', quickNotesSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): quickNotesSvc.IResource {
        var quickNotes = $resource(setting.serverUrl() + setting.serverUrl() + '/api/QuickNotes/GetQuickNotes', null,
            {
                'get': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });
        var quickNote = $resource(setting.serverUrl() + '/api/QuickNotes', null,
            {
                'get': { method: 'GET', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'post': { method: 'POST', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'delete': { method: 'DELETE', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        return {
            quickNotes: quickNotes,
            quickNote: quickNote
        }
    }

    quickNotesSvc.$inject = ["$resource", "currentUser"];
    function quickNotesSvc($resource, currentUser: ICurrentUser): IQuickNoteSvc {
        function getQuickNotes(id: number, success?: Function, error?: Function): IQuickNoteModel[] {
            return <IQuickNoteModel[]>resource($resource, currentUser).quickNotes.query({ id: id }, success, error);
        }

        function getQuickNote(id: number, success?: Function, error?: Function): IQuickNoteModel {
            return <IQuickNoteModel>resource($resource, currentUser).quickNote.get({ id: id }, success, error);
        }

        function createNewQuickNote(model: IQuickNoteModel, success?: Function, error?: Function): void {
            return resource($resource, currentUser).quickNote.post(model, success, error);
        }

        function updateQuickNote(model: IQuickNoteModel, success?: Function, error?: Function): void {
            return resource($resource, currentUser).quickNote.patch(model, success, error);
        }

        function deleteQuickNote(id: number, success?: Function, error?: Function): void {
            return resource($resource, currentUser).quickNote.delete({ id: id }, success, error);
        }

        return {
            getQuickNotes: getQuickNotes,
            getQuickNote: getQuickNote,
            createNewQuickNote: createNewQuickNote,
            updateQuickNote: updateQuickNote,
            deleteQuickNote: deleteQuickNote
        }
    }
})();
