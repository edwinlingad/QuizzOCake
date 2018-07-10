module viewQuickNoteItemCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage,
        quizz: IQuizzModel;
        quickNote: IQuickNoteModel;
        prevBtn: IButtonElement;
        nextBtn: IButtonElement;
        finishBtn: IButtonElement;
        counterBtn: IButtonElement;
        idx: any;
        goBack: () => void;
        goToReviewerIdxBtn: IButtonElement;
        cancelGoTo: () => void;
        counter: ICounter;

        editorControl: IEditorControl;
    }

    export interface IStateParams {
        quizzId: number;
        revId: any;
        idx: any;
    }
}

(function () {
    l2lControllers.controller('viewQuickNoteItemCtrl', viewQuickNoteItemCtrl);

    viewQuickNoteItemCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc"];
    function viewQuickNoteItemCtrl(
        $scope: viewQuickNoteItemCtrl.IScope,
        $stateParams: viewQuickNoteItemCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        resourceSvc: IResourceSvc
        ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 3,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var quizz: IQuizzModel = {};
        var quickNotes: IQuickNoteModel[] = new Array<IQuickNoteModel>();
        var quickNote: IQuickNoteModel;
        var curIdx: number;
        var prevBtn: IButtonElement = {
            disabled: false,
            click: function () {
                if (curIdx > 0)
                    goToNoteIdx(curIdx - 1);
            }
        };
        var nextBtn: IButtonElement = {
            visible: true,
            click: function () {
                if (curIdx < quickNotes.length)
                    goToNoteIdx(curIdx + 1);
            }
        };
        var finishBtn: IButtonElement = {
            visible: false,
            click: function () {
                goBack();
            }
        };
        var counterBtn: IButtonElement = {
            visible: true,
            click: function () {
                counterBtn.visible = false;
                $scope.idx = curIdx + 1;
            }
        };
        var goToReviewerIdxBtn: IButtonElement = {
            disabled: false,
            click: function () {
                if (goToReviewerIdxBtn.disabled == true)
                    return;
                var idx: number = util.getNumber($scope.idx);
                idx = idx - 1;
                if (idx < 0 || idx >= counter.total)
                    return;

                goToNoteIdx(idx);
            }
        }
        var counter: ICounter = {
            current: 0,
            total: 0
        };
        var editorControl: IEditorControl = {
            title: "",
            textContent: "",
            addContentType: 0,
            imageUrl: "",
            imageContent: "",
            drawingContent: undefined
        };

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

                util.editorLoad(editorControl, quickNote, undefined, undefined, close);
            }

            function successQuickNoteList(data: IQuickNoteModel[]) {
                quickNotes = data;
                updateIsReady();

                counter.current = curIdx;
                counter.total = quickNotes.length;

                prevBtn.disabled = curIdx <= 0;
                nextBtn.visible = curIdx < counter.total - 1;
                finishBtn.visible = !nextBtn.visible;

                var qNoteIdx: number = quickNotes[curIdx].id;
                quickNote = cachedDataSvc.getQuickNoteById(qNoteIdx, successQuickNote, errorLoad);
            }

            function successQuizz(data: IQuizzModel) {
                $scope.quizz = quizz = data;
                updateIsReady();

                var revId: number = util.getNumber($stateParams.revId);
                quickNotes = resourceSvc.getResourceManyAlt(enums.ResourceTypeEnum.QuizzQuickNote, quizz.reviewerId, 0, 0, "", "", "", successQuickNoteList, errorLoad);
                //quickNote = cachedDataSvc.getQuickNotesByRevId(revId, successQuickNoteList, errorLoad);
            }

            $location.replace();
            $anchorScroll("top");
            curIdx = util.getNumber($stateParams.idx);
            var quizzId: number = util.getNumber($stateParams.quizzId);
            quizz = cachedDataSvc.getQuizzById(quizzId, successQuizz, errorLoad);

        }

        function goToNoteIdx(idx: number) {
            $state.go("si.viewQuickNote.quickNoteItem", { quizzId: $stateParams.quizzId, revId: $stateParams.revId, idx: idx });
        }

        function goBack() {
            $state.go("si.viewQuickNotes", { quizzId: $stateParams.quizzId, revId: $stateParams.revId, edit: false });
        }

        function cancelGoTo() {
            counterBtn.visible = true;
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.quizz = quizz;
        $scope.goBack = goBack;
        $scope.prevBtn = prevBtn;
        $scope.nextBtn = nextBtn;
        $scope.finishBtn = finishBtn;
        $scope.counterBtn = counterBtn;
        $scope.goToReviewerIdxBtn = goToReviewerIdxBtn;
        $scope.counter = counter;
        $scope.cancelGoTo = cancelGoTo;

        $scope.editorControl = editorControl;
    }
})();