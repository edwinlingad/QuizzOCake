module editTextFlashCardsCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage,
        quizz: IQuizzModel;
        textFlashCards: ITextFlashCardModel[];

        createNew: () => void;

        openEdit: (item: ITextFlashCardModel) => void;
        deleteItem: (idx: number, item: ITextFlashCardModel) => void;

        goBack: () => void;
    }

    export interface IStateParams {
        quizzId: number;
        revId: any;
    }
}

(function () {
    l2lControllers.controller('editTextFlashCardsCtrl', editTextFlashCardsCtrl);

    editTextFlashCardsCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "dialogSvc", "textFlashCardsSvc"];
    function editTextFlashCardsCtrl(
        $scope: editTextFlashCardsCtrl.IScope,
        $stateParams: editTextFlashCardsCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        dialogSvc: IDialogSvc,
        textFlashCardsSvc: ITextFlashCardSvc
        ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 2,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var quizz: IQuizzModel = {};
        var textFlashCards: ITextFlashCardModel[] = new Array<ITextFlashCardModel>();

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

            function successTextFlashCards(data: ITextFlashCardModel[]) {
                $scope.textFlashCards = textFlashCards = data;
                updateIsReady();
            }

            function successQuizz(data: IQuizzModel) {
                $scope.quizz = quizz = data;
                updateIsReady();

                var idx: number = util.getNumber($stateParams.revId);
                textFlashCards = cachedDataSvc.getTextFlashCardsByRevId(idx, successTextFlashCards, errorLoad);
            }

            $anchorScroll("top");
            quizz = cachedDataSvc.getQuizzById($stateParams.quizzId, successQuizz, errorLoad);
        }

        function createNew() {
            $state.go("si.editTextFlashCard", { tfcId: -1, revId: $stateParams.revId, quizzId: $stateParams.quizzId });
        }

        function openEdit(item: ITextFlashCardModel) {
            $state.go("si.editTextFlashCard", { tfcId: item.id, revId: $stateParams.revId, quizzId: $stateParams.quizzId });
        }

        function deleteItem(idx: number, item: ITextFlashCardModel) {
            dialogSvc.confirm(str.confirmDelete, function (result) {
                if (result == true) {
                    textFlashCardsSvc.deleteTextFlashCard(item.id, function (data) {
                        textFlashCards.splice(idx, 1);
                        notificationSvc.success(str.deleteSuccess);
                    }, errorSave);
                }
            });
        }

        function goBack() {
            $state.go("si.quizzDetail", { quizzId: $stateParams.quizzId, view: 2 });
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.quizz = quizz;
        $scope.textFlashCards = textFlashCards;

        $scope.createNew = createNew;
        $scope.openEdit = openEdit;
        $scope.deleteItem = deleteItem;

        $scope.goBack = goBack;
    }
})();