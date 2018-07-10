module editQuickNoteCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage,
        quizz: IQuizzModel;
        quickNote: IQuickNoteModel;

        saveEditBtn: IButtonElement;
        changed: () => void;

        goBack: () => void;

        tinymceOptions: any;
    }

    export interface IStateParams {
        quizzId: number;
        qnId: any;
        revId: any;
    }
}

(function () {
    l2lControllers.controller('editQuickNoteCtrl', editQuickNoteCtrl);

    editQuickNoteCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "quickNotesSvc"];
    function editQuickNoteCtrl(
        $scope: editQuickNoteCtrl.IScope,
        $stateParams: editQuickNoteCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
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
        var quickNote: IQuickNoteModel = {};
        var saveEditBtn: IButtonElement = {
            click: function (form: ng.IFormController) {
                if (form.$valid) {
                    util.disableForm(page);
                    if (quickNote.id != 0) {
                        quickNotesSvc.updateQuickNote(quickNote,
                            function () {
                                util.enableForm(page);
                                quickNotes.forEach(function (item: IQuickNoteModel) {
                                    if (item.id == quickNote.id) {
                                        item.title = quickNote.title;
                                    }
                                });
                                goBack();
                            }, errorSave);
                        return;
                    }

                    if (quickNote.id == 0) {
                        quickNotesSvc.createNewQuickNote(quickNote,
                            function (data: IQuickNoteModel) {
                                data.notes = "";
                                quickNotes.push(data);
                                util.enableForm(page);
                                goBack();
                            }, errorSave);
                    }
                }
            }
        };

        function errorSave(response) {
            util.enableForm(page);
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

            function successQuickNote(data: IQuickNoteModel) {
                $scope.quickNote = quickNote = data;
                updateIsReady();
            }

            function successQuizz(data: IQuizzModel) {
                $scope.quizz = quizz = data;
                updateIsReady();

                if ($stateParams.qnId != undefined && ($stateParams.qnId == "-1" || $stateParams.qnId == -1)) {
                    var qNote: IQuickNoteModel = {
                        id: 0,
                        ownerId: user.id,
                        reviewerId: $stateParams.revId
                    };
                    successQuickNote(qNote);
                    page.title = "New Note";
                    saveEditBtn.name = "Create";
                } else {
                    page.title = "Edit Note";
                    saveEditBtn.name = "Save";
                    quickNote = cachedDataSvc.getQuickNoteById($stateParams.qnId, successQuickNote, errorLoad);
                }
            }

            $anchorScroll("top");
            quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, successQuizz, errorLoad);
            quickNotes = cachedDataSvc.getQuickNotesByRevId($stateParams.revId);
        }

        function changed(): void {
            if(quickNote.id == 0) {
                saveEditBtn.disabled = false;
            } else {
                saveEditBtn.disabled = quickNote.title.trim().length == 0 || quickNote.title.trim().length == 0;
            }
        }

        function goBack() {
            history.back();
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.quizz = quizz;
        $scope.tinymceOptions = util.getTinymceOptions();

        $scope.quickNote = quickNote;
        $scope.saveEditBtn = saveEditBtn;
        $scope.changed = changed;

        $scope.goBack = goBack;

    }
})();