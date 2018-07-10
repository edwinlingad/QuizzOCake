module viewQuickNotesCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage,
        quizz: IQuizzModel;
        isEditMode: boolean;
        quickNotes: IQuickNoteModel[];

        createNew: () => void;

        openEdit: (item: IQuickNoteModel) => void;
        deleteItem: (idx: number, item: IQuickNoteModel) => void;

        openNotes: (idx: number) => void;

        goBack: () => void;
    }

    export interface IStateParams {
        quizzId: any;
        revId: any;
        edit: any;
    }
}

(function () {
    l2lControllers.controller('viewQuickNotesCtrl', viewQuickNotesCtrl);

    viewQuickNotesCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "dialogSvc", "quickNotesSvc"];
    function viewQuickNotesCtrl(
        $scope: viewQuickNotesCtrl.IScope,
        $stateParams: viewQuickNotesCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,

        $anchorScroll: ng.IAnchorScrollService,
        dialogSvc: IDialogSvc,
        quickNotesSvc: IQuickNoteSvc
        ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 2,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var quizz: IQuizzModel = {};
        var quickNotes: IQuickNoteModel[] = new Array<IQuickNoteModel>();
        function errorSave(response) {
            notificationSvc.error(str.errorSave);
        }

        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successQuickNotes(data: IQuickNoteModel[]) {
                $scope.quickNotes = quickNotes = data;
                updateIsReady();

                quickNotes.forEach(function (item: IQuickNoteModel) {
                    item.isEditOpen = false;
                    item.isViewOpen = false;
                    item.canEdit = user.id == item.ownerId;
                });
            }

            function successQuizz(data: IQuizzModel) {
                $scope.quizz = quizz = data;
                updateIsReady();

                quickNotes = cachedDataSvc.getQuickNotesByRevId($stateParams.revId, successQuickNotes, errorLoad);
            }

            $anchorScroll("top");
            quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, successQuizz, errorLoad);
            $scope.isEditMode = $stateParams.edit != undefined && $stateParams.edit == "true";
            page.title = $scope.isEditMode ? "Edit Notes" : "Notes Reviewer";

        }

        function createNew() {
            $state.go("si.editQuickNote", { qnId: -1, revId: $stateParams.revId, quizzId: $stateParams.quizzId });
        }

        function openEdit(item: IQuickNoteModel) {
            $state.go("si.editQuickNote", { qnId: item.id, revId: $stateParams.revId, quizzId: $stateParams.quizzId });
        }

        function deleteItem(idx: number, item: IQuickNoteModel) {
            dialogSvc.confirm(str.confirmDelete, function (result) {
                if (result == true) {
                    quickNotesSvc.deleteQuickNote(item.id, function (data) {
                        quickNotes.splice(idx, 1);
                        notificationSvc.success(str.deleteSuccess);
                    }, errorSave);
                }
            });
        }

        function openNotes(idx: number): void {
            $state.go("si.viewQuickNote.quickNoteItem", { quizzId: $stateParams.quizzId, revId: $stateParams.revId, idx: idx });
        }

        function goBack() {
            $state.go("si.quizzDetail", { quizzId: $stateParams.quizzId, view: 1});
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.quizz = quizz;
        $scope.quickNotes = quickNotes;

        $scope.createNew = createNew;
        $scope.openEdit = openEdit;
        $scope.deleteItem = deleteItem;

        $scope.openNotes = openNotes;

        $scope.goBack = goBack;
    }
})();