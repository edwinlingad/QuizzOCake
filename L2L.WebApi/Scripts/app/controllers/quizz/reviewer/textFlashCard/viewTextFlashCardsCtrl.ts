module viewTextFlashCardsCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage,
        isEdit: boolean;
        quizz: IQuizzModel;
        quickNotes: IQuickNoteModel[];
        newQuickNote: IQuickNoteModel;

        openEdit: (item: IQuickNoteModel) => void;
        deleteItem: (idx: number, item: IQuickNoteModel) => void;

        saveEditBtn: IButtonElement;
        cancelEdit: (item: IQuickNoteModel) => void;

        toggleOpenView: (item: IQuickNoteModel) => void;
        openAllView: () => void;
        closeAllView: () => void;
    }

    export interface IStateParams {
        quizzId: any;
        revId: any;
        edit: any;
    }
}

(function () {
    l2lControllers.controller('viewTextFlashCardsCtrl', viewTextFlashCardsCtrl);

    viewTextFlashCardsCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "quickNotesSvc", "dialogSvc"];
    function viewTextFlashCardsCtrl(
        $scope: viewTextFlashCardsCtrl.IScope,
        $stateParams: viewTextFlashCardsCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        quickNotesSvc: IQuickNoteSvc,
        dialogSvc: IDialogSvc
        ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 2,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var quizz: IQuizzModel = {};
        var quickNotes: IQuickNoteModel[] = new Array<IQuickNoteModel>();
        var newQuickNote: IQuickNoteModel = {
            id: 0,
            title: "",
            notes: "",
            reviewerId: $stateParams.revId,
            ownerId: user.id
        }
        var saveEditBtn: IButtonElement = {
            click: function (item: IQuickNoteModel) {
                if (item.id != 0) {
                    quickNotesSvc.updateQuickNote(item);
                    return;
                }

                if (item.id == 0) {
                    quickNotesSvc.createNewQuickNote(newQuickNote,
                        function (data: IQuickNoteModel) {
                            var qNote: IQuickNoteModel = {
                                id: data.id,
                                title: data.title,
                                notes: data.notes,
                                reviewerId: data.reviewerId,
                                ownerId: data.ownerId,

                                canEdit: true,
                                isEditOpen: false,
                                isViewOpen: false
                            };
                            quickNotes.push(qNote);
                            newQuickNote.isEditOpen = false;
                            newQuickNote.isViewOpen = false;
                        }, errorSave);
                }
            }
        }

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

                quickNotes = quickNotesSvc.getQuickNotes($stateParams.revId, successQuickNotes, errorLoad);
            }

            $scope.isEdit = $stateParams.edit !== undefined && $stateParams.edit === "true";
            quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, successQuizz, errorLoad);
        }

        function openEdit(item: IQuickNoteModel) {
            item.isEditOpen = true;
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

        function cancelEdit(item: IQuickNoteModel) {
            item.isEditOpen = false;
        }

        function toggleOpenView(item: IQuickNoteModel) {
            item.isViewOpen = !item.isViewOpen;
        }

        function openAllView() {
            quickNotes.forEach(function (item: IQuickNoteModel) {
                item.isViewOpen = true;
            });
        }

        function closeAllView() {
            quickNotes.forEach(function (item: IQuickNoteModel) {
                item.isViewOpen = false;
            });
        }

        function goBack() {
            history.back();
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.quizz = quizz;
        $scope.quickNotes = quickNotes;
        $scope.newQuickNote = newQuickNote;

        $scope.saveEditBtn = saveEditBtn;

        $scope.openEdit = openEdit;
        $scope.deleteItem = deleteItem;

        $scope.toggleOpenView = toggleOpenView;
        $scope.openAllView = openAllView;
        $scope.closeAllView = closeAllView;
    }
})();