module quizzOverviewDirective {
    export interface IScope {
        page: IPage;
        quizzId: number;
        model: IQuizzOverviewModel;
        index: number;
        help: number;
        goToEditQuizz: () => void;
        goToEditQuickNotes: () => void;
        goToEditTextFlashCards: () => void;
        goToViewReviewer: () => void;
        goToViewQuickNotes: () => void;
        goToEditTest: () => void;
        goToPrepTest: (isPreview: boolean) => void;
        goToQuizz: () => void;
        goToQuizzer: () => void;
        goToAssign(): void;
        deleteQuizz(): void;
        toggleLive: () => void;
        toggleBookmark: () => void;
        upVoteBtn: IElement;
        liveBtn: IButtonElement;
        user: IUserModel;
        quizzPermission: IPermission;
        detailed: string;
        comment: IComment;

        idx: number;
        deleteIdx(idx: number): void;

        //rating: IRating;

        tip: any;
        tipControl: any;
        shareControl: any;
    }

    export interface IComment {
        isOpen: boolean;
        isLoaded: boolean;
        toggleShow: () => void;
    }

    export interface IRating {
        isReady: boolean;
        uiValue: number;
        init: Function;
        numRatings: number;
    }
}

l2lApp.directive('quizzOverview', function () {

    controller.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "resourceSvc", "dialogSvc", "quizzUpvoteSvc", "quizzSvc", "quizzCommentSvc", "loginModalSvc", "$timeout", "facebookSvc", "$location"];

    function controller(
        $scope: quizzOverviewDirective.IScope,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        resourceSvc: IResourceSvc,
        dialogSvc: IDialogSvc,
        quizzUpvoteSvc: IQuizzUpvoteSvc,
        quizzSvc: IQuizzSvc,
        quizzCommentSvc: IQuizzCommentSvc,
        loginModalSvc: ILoginModalSvc,
        $timeout: ng.ITimeoutService,
        facebookSvc: IFacebookSvc,
        $location: ng.ILocationService
    ) {

        var page: IPage = {
            isReady: false,
            numResourceToWait: 1
        };
        var model: IQuizzOverviewModel;
        var upVoteBtn: IElement = {
            isEnabled: true,
            click: upVoteClick
        };
        var liveBtn: IButtonElement = {
            isReady: true,
            click: function () {
                if (util.showLoginIfGuest(user, loginModalSvc))
                    return;

                var onClass: string = "is-live";
                var tmpValue: boolean = !model.isLive;
                liveBtn.isReady = false;
                quizzSvc.setLive({ id: model.id, value: tmpValue },
                    function () {
                        model.isLive = tmpValue;
                        cachedDataSvc.deleteQuizzById(model.id);
                        updateLiveBtn();
                        liveBtn.isReady = true;
                    },
                    function () {
                        liveBtn.isReady = true;
                        notificationSvc.error(str.errorSave);
                    });
            }
        }
        var user: IUserModel = currentUser.getUserData();
        var comment: quizzOverviewDirective.IComment = {
            isOpen: false,
            isLoaded: false,
            toggleShow: function () {
                comment.isOpen = !comment.isOpen;

                $timeout(function () {
                    comment.isLoaded = !comment.isLoaded;
                }, 300);
            },
        };
        //var rating: quizzOverviewDirective.IRating = {
        //    isReady: false,
        //    uiValue: 0,
        //    numRatings: 0,
        //    init: function () {
        //        var quizzUserRating: IQuizzUserRatingModel;

        //        quizzUserRating = resourceSvc.getResource(enums.ResourceTypeEnum.QuizzUserRating, $scope.quizzId,
        //            function (data: IQuizzUserRatingModel) {
        //                rating.uiValue = Math.ceil(data.ratingAvg);
        //                rating.isReady = true;
        //                rating.numRatings = data.numRatings;
        //            },
        //            function () {
        //                rating.isReady = true;
        //            });
        //    }
        //}

        function init() {
            function errorLoad() {
                notificationSvc.error(str.errorLoad);
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait == 0;
            }

            function successQuizzOverview(data: IQuizzOverviewModel) {
                $scope.quizzId = data.id;
                $scope.model = model = data;
                updateIsReady();

                //rating.init();

                updateLiveBtn();
                updateUpvoteBtn();
                if (model.isLiked) {
                    upVoteBtn.addClass = "is-liked";
                    upVoteBtn.name = "Unlike";
                }
                else {
                    upVoteBtn.name = "Like";
                }
            }

            if ($scope.quizzId !== undefined) {
                model = cachedDataSvc.getQuizzOverviewById($scope.quizzId, successQuizzOverview, errorLoad);
            } else {
                successQuizzOverview($scope.model);
            }
        }

        function updateLiveBtn() {
            if (model.numQuestions < 10) {
                liveBtn.tooltip = "There should at least be 10 questions";
                return;
            }

            liveBtn.tooltip = model.isLive ? "Click to hide from other people" : "Click to make other people see this Quizz";
        }

        function updateUpvoteBtn() {
            upVoteBtn.addClass = model.isLiked ? "is-liked" : "";
        }

        function upVoteClick() {
            function successUpDownVote(data) {
                upVoteBtn.isEnabled = true;
                model.isLiked = !model.isLiked;
                if (model.isLiked) {
                    upVoteBtn.addClass = "is-liked";
                    upVoteBtn.name = "Unlike";
                    model.numLikes++;
                }
                else {
                    upVoteBtn.addClass = "";
                    upVoteBtn.name = "Like";
                    model.numLikes--;
                }
            }

            function errorUpDownVote() {
                upVoteBtn.isEnabled = true;
                alert("error");
            }

            if (util.showLoginIfGuest(user, loginModalSvc))
                return;

            if (user.id == model.ownerId)
                return;

            upVoteBtn.isEnabled = false;
            if (model.isLiked == false) {
                quizzUpvoteSvc.upVote(model.id, successUpDownVote, errorUpDownVote);
            } else {
                quizzUpvoteSvc.downVote(model.id, successUpDownVote, errorUpDownVote);
            }

        }

        function closeOptionButtons() {
            //$ionicListDelegate.closeOptionButtons();
        }

        function goToEditQuizz() {
            $state.go("si.editQuizz", { quizzId: $scope.model.id });
        }

        function goToEditQuickNotes() {
            $state.go("si.viewQuickNotes", { quizzId: $scope.model.id, revId: model.reviewerId, edit: true });
        }

        function goToViewQuickNotes() {
            $state.go("si.viewQuickNotes", { quizzId: $scope.model.id, revId: model.reviewerId, edit: false });
        }

        function goToViewReviewer() {
            $state.go("si.viewReviewer", { quizzId: $scope.model.id });
        }

        function goToEditTextFlashCards() {
            $state.go("si.editTextFlashCards", { quizzId: $scope.model.id, revId: $scope.model.reviewerId });
        }

        function goToEditTest() {
            $state.go("si.editTest", { testId: $scope.model.testId, quizzId: $scope.model.id });
        }

        function goToPrepTest(isPreview: boolean) {
            $state.go("si.prepTest", { testId: $scope.model.testId, quizzId: $scope.model.id, isPrev: isPreview });
        }

        function goToQuizzer() {
            $state.go("si.quizzer", { quizzerId: $scope.model.ownerId });
        }

        function goToQuizz() {
            $state.go("si.quizzDetail", { quizzId: $scope.model.id, view: 3 });
        }

        function goToAssign() {
            $state.go("si.editAssignmentGroup", { assGId: -1, quizzId: $scope.model.id });
        }

        function toggleBookmark() {

        }

        function deleteQuizz(): void {
            function successDelete() {
                notificationSvc.success(str.deleteSuccess);

                if ($scope.idx !== undefined && $scope.deleteIdx !== undefined) {
                    $scope.deleteIdx($scope.idx);
                } else {
                    $state.go(util.getDefaultLocation());
                }
            }

            function errorDelete() {
                notificationSvc.error(str.errorSave);
            }

            dialogSvc.confirm(str.confirmDelete, function (result) {
                if (result == true) {
                    quizzSvc.deleteQuizz(model.id, successDelete, errorDelete);

                }
            });
        }

        var tipInternal = $scope.tipControl || {};
        tipInternal.showAll = function () {
            tip.showAll();
        }

        var tip = (function () {
            var vars = {
                isEditQuizzVisible: false,
                isLiveVisible: false
            };

            function hideEditQuizz() {
                vars.isEditQuizzVisible = false;
            }

            function hideLive() {
                vars.isLiveVisible = false;
            }

            function showAll() {
                vars.isEditQuizzVisible = true;
                vars.isLiveVisible = true;
            }

            return {
                vars: vars,
                hideEditQuizz: hideEditQuizz,
                hideLive: hideLive,
                showAll: showAll
            }
        })();

        var shareControl = (function () {
            function share(where: string) {
                if (where === 'facebook') {
                    var message: string = "";
                    if (user.id === model.ownerId)
                        message = "Check out the Quizz that I made on QuizzOCake : " + model.title;
                    else
                        message = "Check out this Quizz on QuizzOCake : " + model.title;
                    var meta: IOGMeta = {
                        type: 0,
                        url: "https://quizzocake.com/#/quizz/quizz-detail/" + model.id + "?view=3"
                    }
                    facebookSvc.share(message, meta);
                    return;
                }
            }

            return {
                share: share
            }
        })();

        init();

        $scope.page = page;
        $scope.user = user;
        $scope.upVoteBtn = upVoteBtn;
        $scope.goToEditQuizz = goToEditQuizz;

        $scope.goToEditQuickNotes = goToEditQuickNotes;
        $scope.goToViewQuickNotes = goToViewQuickNotes;

        $scope.goToViewReviewer = goToViewReviewer;
        $scope.goToEditTextFlashCards = goToEditTextFlashCards;

        $scope.goToEditTest = goToEditTest;
        $scope.goToPrepTest = goToPrepTest;
        $scope.goToQuizz = goToQuizz;
        $scope.goToQuizzer = goToQuizzer;
        $scope.goToAssign = goToAssign;
        $scope.deleteQuizz = deleteQuizz;
        $scope.toggleBookmark = toggleBookmark;
        $scope.liveBtn = liveBtn;
        $scope.shareControl = shareControl;

        $scope.comment = comment;

        //$scope.rating = rating;

        $scope.tip = tip;
    }

    return {
        restrict: 'E',
        templateUrl: 'scripts/templates/directives/quizz-overview.html',
        replace: true,
        scope: {
            model: '=?',
            quizzId: "@",
            detailed: "@",
            idx: "@?",
            deleteIdx: "=?",
            help: "@?",
            tipControl: "=?"
        },
        controller: controller
    }
});