module editTextFlashCardCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage,
        quizz: IQuizzModel;
        textFlashCard: ITextFlashCardModel;

        saveEditBtn: IButtonElement;
        changed: () => void;

        goBack: () => void;
    }

    export interface IStateParams {
        quizzId: number;
        tfcId: any;
        revId: any;
    }
}

(function () {
    l2lControllers.controller('editTextFlashCardCtrl', editTextFlashCardCtrl);

    editTextFlashCardCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "textFlashCardsSvc"];
    function editTextFlashCardCtrl(
        $scope: editTextFlashCardCtrl.IScope,
        $stateParams: editTextFlashCardCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        textFlashCardsSvc: ITextFlashCardSvc
        ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 1,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var quizz: IQuizzModel = {};
        var textFlashCards: ITextFlashCardModel[] = new Array<ITextFlashCardModel>();
        var textFlashCard: ITextFlashCardModel = {};
        var saveEditBtn: IButtonElement = {
            click: function (form: ng.IFormController) {
                if (form.$valid) {
                    util.disableForm(page);
                    if (textFlashCard.id != 0) {
                        textFlashCardsSvc.updateTextFlashCard(textFlashCard,
                            function () {
                                util.enableForm(page);
                                textFlashCards.forEach(function (item: ITextFlashCardModel) {
                                    if (item.id == textFlashCard.id) {
                                        item.title = textFlashCard.title;
                                    }
                                });
                                goBack();
                            }, errorSave);
                        return;
                    }

                    if (textFlashCard.id == 0) {
                        textFlashCardsSvc.createNewTextFlashCard(textFlashCard,
                            function (data: IQuickNoteModel) {
                                data.notes = "";
                                textFlashCards.push(data);
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

            function successTextFlashCard(data: ITextFlashCardModel) {
                $scope.textFlashCard = textFlashCard = data;
                updateIsReady();
            }

            function successQuizz(data: IQuizzModel) {
                $scope.quizz = quizz = data;
                updateIsReady();

                if (util.isNumberEqual($stateParams.tfcId, -1)) {
                    var idx: number = util.getNumber($stateParams.revId);
                    var tfc: ITextFlashCardModel = {
                        id: 0,
                        ownerId: user.id,
                        reviewerId: idx
                    };
                    successTextFlashCard(tfc);
                    page.title = "Next Flash Card";
                    saveEditBtn.name = "Create";
                } else {
                    page.title = "Edit Flash Card";
                    saveEditBtn.name = "Save";
                    var idx: number = util.getNumber($stateParams.tfcId);
                    textFlashCard = cachedDataSvc.getTextFlashCardById(idx, successTextFlashCard, errorLoad);
                }
            }

            $anchorScroll("top");
            quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, successQuizz, errorLoad);

            var idx: number = util.getNumber($stateParams.revId);
            textFlashCards = cachedDataSvc.getTextFlashCardsByRevId(idx);
        }

        function changed(): void {
            if (textFlashCard.id == 0) {
                saveEditBtn.disabled = false;
            } else {
                saveEditBtn.disabled = textFlashCard.title.trim().length == 0 || textFlashCard.title.trim().length == 0;
            }
        }

        function goBack() {
            history.back();
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.quizz = quizz;

        $scope.textFlashCard = textFlashCard;
        $scope.saveEditBtn = saveEditBtn;
        $scope.changed = changed;

        $scope.goBack = goBack;
    }
})();