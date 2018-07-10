module quizzFlashCards {
    export interface IScope {
        $on: any;

        quizz: IQuizzModel;
        quizzOverview: IQuizzOverviewModel;

        page: IPage;
        flashCardControl: any;
        flashCards: ITextFlashCardModel[];
        editorControl: IEditorControl;
        viewControl: any;

        startCardControl: any;
    }
}

(function () {
    l2lApp.directive("quizzFlashCards", quizzFlashCards);

    function quizzFlashCards() {

        controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "builtInQuestionsSvc", "flashCardSvc", "flashCardReviewerSvc"];

        function controller(
            $scope: quizzFlashCards.IScope,
            $state: ng.ui.IStateService,
            cachedDataSvc: ICachedDataSvc,
            notificationSvc: INotificationSvc,
            currentUser: ICurrentUser,
            $anchorScroll: ng.IAnchorScrollService,
            $location: ng.ILocationService,
            resourceSvc: IResourceSvc,
            $timeout: ng.ITimeoutService,
            dialogSvc: IDialogSvc,
            builtInQuestionsSvc: IBuiltInQuestionsSvc,
            flashCardSvc: IFlashCardSvc,
            flashCardReviewerSvc: IFlashCardReviewerSvc
        ) {
            var page: IPage = {
                isReady: false,
                numResourceToWait: 2,
                disabled: false
            };
            var user: IUserModel = currentUser.getUserData();
            var quizz: IQuizzModel = {};
            var quizzOverview: IQuizzOverviewModel = {};
            var flashCards: ITextFlashCardModel[] = new Array<ITextFlashCardModel>();
            var editorControl: IEditorControl = {
                title: "",
                textContent: "",
                addContentType: 0,
                imageUrl: "",
                imageContent: "",
                drawingContent: undefined
            };

            function errorSave(response) {
                flashCardControl.vars.disabled = false;
                flashCardControl.vars.disableAll = false;
                util.editorClear(editorControl);

                notificationSvc.error(str.errorSave);
            }

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
            }

            function init() {
                function updateIsReady() {
                    page.numResourceToWait--;
                    page.isReady = page.numResourceToWait <= 0;
                }

                function successQuizzQuickNote(data: ITextFlashCardModel[]) {
                    $scope.flashCards = flashCards = data;
                    flashCards.forEach(function (item: ITextFlashCardModel) {
                        item.isOpen = false;
                        item.isViewOpen = false;
                        item.isOpenIf = false;
                        item.isViewOpenIf = false;
                    });
                    updateIsReady();
                }

                function successCardQuestion(data: any) {
                    updateIsReady();

                    flashCardControl.vars.numQuestionCards = data.count;
                }

                quizz = $scope.quizz;
                quizzOverview = $scope.quizzOverview;
                flashCards = resourceSvc.getResourceManyAlt(enums.ResourceTypeEnum.FlashCards, quizzOverview.reviewerId, 0, 0, "", "", "", successQuizzQuickNote, errorLoad);

                if (quizz.isBuiltIn) {
                    updateIsReady();
                    flashCardControl.vars.numQuestionCards = 20;
                } else {
                    resourceSvc.getResource(enums.ResourceTypeEnum.ReviewerFromQuestions, quizz.testId, successCardQuestion, errorLoad);
                }

                $scope.$on("$destroy", destroy);
            }

            var flashCardControl = (function () {
                var pageNum: number = 2;
                var numPerPage: number = 25;
                var vars = {
                    isOpen: false,
                    isOpenIf: false,
                    answer: "",
                    answerIsModified: false,
                    disabled: false,
                    disableAll: false,
                    canEditAll: false,
                    numQuestionCards: 0
                };


                function open() {

                    vars.isOpen = true;
                    vars.isOpenIf = true;
                    vars.disableAll = true;
                    vars.answer = "";

                    util.editorClear(editorControl);
                }

                function close() {

                    vars.isOpen = false;
                    vars.disableAll = false;
                    $timeout(function () {
                        vars.isOpenIf = false;
                    }, 1000);
                }

                function changed() {
                    vars.answerIsModified = true;
                }

                function create() {

                    function successCreate(model: ITextFlashCardModel) {
                        vars.isOpen = false;
                        vars.isOpenIf = false;
                        vars.disabled = false;
                        vars.disableAll = false;

                        model.isOpen = false;
                        model.isOpenIf = false;
                        model.isViewOpen = false;
                        model.isViewOpenIf = false;

                        flashCards.push(model);
                        util.editorClear(editorControl);

                        notificationSvc.success(str.createSuccess);
                    }

                    vars.disabled = true;

                    var model: ITextFlashCardModel = {
                        id: 0,
                        reviewerId: quizzOverview.reviewerId,
                        answer: vars.answer
                    };

                    editorControl.callSave();
                    util.editorLoad(model, editorControl);

                    resourceSvc.createResource(enums.ResourceTypeEnum.FlashCards, model, successCreate, errorSave);
                }

                function edit(item: ITextFlashCardModel) {
                    vars.disableAll = true;
                    item.isOpen = true;
                    item.isOpenIf = true;

                    util.editorLoad(editorControl, item);
                    vars.answer = item.answer;
                    editorControl.isImageChanged = false;
                }

                function closeEditor(item: ITextFlashCardModel) {
                    item.isOpen = false;
                    $timeout(function () {
                        item.isOpenIf = false;
                    }, 500);
                }

                function editSave(item: ITextFlashCardModel) {
                    function successUpdate() {
                        vars.disabled = false;
                        vars.disableAll = false;
                        vars.answerIsModified = false;

                        closeEditor(item);

                        item.answer = vars.answer;
                        vars.answer = "";

                        util.editorLoad(item, editorControl);

                        if (editorControl.isImageChanged) {
                            var newFileName = editorControl.newImageFileName;
                            var oldImageUrl = item.imageUrl;
                            util.editorLoad(item, editorControl);

                            var lastIdx: number = oldImageUrl.lastIndexOf("/");
                            item.imageUrl =
                                oldImageUrl.substr(0, lastIdx + 1) + newFileName;
                        }

                        util.editorClear(editorControl);
                        notificationSvc.success(str.updateSuccess);
                    }

                    var model: ITextFlashCardModel = {};
                    model = util.clone(item);
                    editorControl.callSave();
                    util.editorLoad(model, editorControl);
                    model.reviewerId = quizzOverview.reviewerId;
                    model.answer = vars.answer;

                    resourceSvc.updateResource(enums.ResourceTypeEnum.FlashCards, model, successUpdate, errorSave);
                }

                function editCancel(item: ITextFlashCardModel) {
                    closeEditor(item);
                    vars.disableAll = false;
                    vars.answerIsModified = false;
                }

                function editSaveEnabled() {
                    if ((vars.answerIsModified || editorControl.isModified) && (vars.answer.trim().length !== 0 && editorControl.textContent !== undefined)) {
                        return true;
                    }

                    return false;
                }

                function remove(idx: number, item: ITextFlashCardModel) {
                    function successRemove() {
                        flashCards.splice(idx, 1);
                        notificationSvc.success(str.deleteSuccess);
                    }

                    dialogSvc.confirm(str.confirmDelete, function (result: boolean) {
                        if (result) {
                            resourceSvc.deleteResource(enums.ResourceTypeEnum.FlashCards, item.id, successRemove, errorSave);
                        }
                    });
                }

                return {
                    numPerPage: numPerPage,
                    vars: vars,
                    open: open,
                    close: close,
                    changed: changed,
                    create: create,

                    edit: edit,
                    editSave: editSave,
                    editCancel: editCancel,
                    editSaveEnabled: editSaveEnabled,

                    remove: remove,
                }
            })();

            var viewControl = (function () {
                function open(item: ITextFlashCardModel) {
                    item.isViewOpen = true;
                    item.isViewOpenIf = true;
                    flashCardControl.vars.disableAll = true;

                    util.editorLoad(editorControl, item, undefined, undefined, close);
                }

                function close(item: ITextFlashCardModel) {
                    item.isViewOpen = false;
                    $timeout(function () {
                        item.isViewOpenIf = false;
                    }, 500);
                    flashCardControl.vars.disableAll = false;
                }

                return {
                    open: open,
                    close: close
                }
            })();

            var startCardControl = (function () {
                var vars = {
                    isReady: true,
                };
                var randomBtn: IItemToWait = {
                    isReady: true
                };
                var orderedBtn: IItemToWait = {
                    isReady: true
                };

                var reviewers: IReviewerFromQuestionModel[] = new Array<IReviewerFromQuestionModel>();

                function initAndStart(fType: string) {
                    function successReviewer(data: IReviewerFromQuestionModel[]) {
                        reviewers = data;

                        if (user.id === 0) {
                            var startIdx: number = reviewers.length > 5 ? 5 : reviewers.length;

                            reviewers.splice(startIdx, reviewers.length - startIdx);
                        }

                        var isRandomized: boolean = false;
                        flashCardReviewerSvc.init(quizz.id, reviewers);

                        if (fType === "random") {
                            isRandomized = true;
                        }
                        flashCardReviewerSvc.start(isRandomized);

                        $state.go("si.viewAsFlashCard.flashCardItem", { quizzId: quizz.id, fcId: 0 });
                    }

                    if (quizz.isBuiltIn)
                        reviewers = builtInQuestionsSvc.getFlashCards(quizz.id, 20, successReviewer, errorLoad);
                    else
                        reviewers = flashCardSvc.getReviewerFromQuestions(quizz.testId, successReviewer, errorLoad);
                }

                function startOrdered() {
                    vars.isReady = false;
                    orderedBtn.isReady = false;

                    initAndStart("ordered");
                }

                function startRandom() {
                    vars.isReady = false;
                    randomBtn.isReady = false;

                    initAndStart("random");
                }

                return {
                    vars: vars,
                    randomBtn: randomBtn,
                    orderedBtn: orderedBtn,
                    init: init,
                    startRandom: startRandom,
                    startOrdered: startOrdered
                }
            })();

            function destroy() {
            }

            init();

            $scope.page = page;

            $scope.flashCards = flashCards;
            $scope.flashCardControl = flashCardControl;
            $scope.editorControl = editorControl;
            $scope.viewControl = viewControl;
            $scope.startCardControl = startCardControl;
        }
        return {
            restrict: "E",
            templateUrl: "scripts/templates/directives/quizz/reviewers/flashCards.html",
            replace: true,
            scope: {
                quizz: "=",
                quizzOverview: "="
            },
            controller: controller
        }
    }
})();