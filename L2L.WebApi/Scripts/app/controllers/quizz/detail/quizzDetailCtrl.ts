module quizzDetailCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        view: number;
        assId: number;

        help: number;
        tipControl: any;
        isAssignment: boolean;

        quizz: IQuizzModel;
        quizzOverview: IQuizzOverviewModel;
        categories: IQuizzCategoryModel[];
        quizzGradeLevelList: IGradeLevelModel[];

        categoryIconControl: any;
        quizzControl: any;
        //upvoteControl: any;
        //ratingControl: any;
        liveControl: any;
        shareControl: any;

        goBack: () => void;
    }

    export interface IStateParams {
        quizzId: number;
        view: number;
        assId: number;
        help: number;
    }
}

(function () {
    l2lControllers.controller('quizzDetailCtrl', quizzDetailCtrl);

    quizzDetailCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "currentUser", "$anchorScroll", "notificationSvc", "resourceSvc", "quizzUpvoteSvc", "quizzSvc", "broadcastSvc", "$timeout", "dialogSvc", "layoutSvc", "facebookSvc", "$location"];
    function quizzDetailCtrl(
        $scope: quizzDetailCtrl.IScope,
        $stateParams: quizzDetailCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        notificationSvc: INotificationSvc,
        resourceSvc: IResourceSvc,
        quizzUpvoteSvc: IQuizzUpvoteSvc,
        quizzSvc: IQuizzSvc,
        broadcastSvc: IBroadcastSvc,
        $timeout: ng.ITimeoutService,
        dialogSvc: IDialogSvc,
        layoutSvc: ILayoutSvc,
        facebookSvc: IFacebookSvc,
        $location: ng.ILocationService
    ) {

        var user: IUserModel = currentUser.getUserData();
        var page: IPage = {
            numResourceToWait: 4,
            isReady: false,
            disabled: false
        }
        var quizzId: number = 0;
        var quizz: IQuizzModel = {};
        var quizzOverview: IQuizzOverviewModel = {};
        var categories: IQuizzCategoryModel[] = new Array<IQuizzCategoryModel>();
        var quizzGradeLevelList: IGradeLevelModel[] = new Array<IGradeLevelModel>();

        function init() {

            function errorLoad() {
                notificationSvc.error(str.errorLoad);
                console.log("quizzDetail error");
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successQuizz(data: IQuizzModel) {
                $scope.quizz = quizz = data;
                quizz.isModified = false;
                updateIsReady();
            }

            function successQuizzOverview(data: IQuizzOverviewModel) {
                $scope.quizzOverview = quizzOverview = data;
                updateIsReady();

                //ratingControl.init();
                liveControl.init();

                quizzOverview.updateQuestions = updateQuestions;

                //if (quizzOverview.isLive && quizzOverview.numQuestions < 10) {
                //    liveControl.toggle();
                //}
            }

            function successCategories(data) {
                $scope.categories = categories = data;
                updateIsReady();
            }

            function successGradeLevels(data: IGradeLevelModel[]) {
                $scope.quizzGradeLevelList = quizzGradeLevelList = data;
                updateIsReady();
            }

            function processHelp(stateParam: any, scope: any) {
                var idx: number = util.getNumber(stateParam.help);
                scope.help = idx;
            }

            $anchorScroll("top");

            quizzId = util.getNumber($stateParams.quizzId);
            $scope.view = util.getNumber($stateParams.view);
            $scope.assId = util.getNumber($stateParams.assId);
            $scope.isAssignment = $scope.assId !== -1;

            quizzOverview = cachedDataSvc.getQuizzOverviewById(quizzId, successQuizzOverview, errorLoad);
            quizz = cachedDataSvc.getQuizzById(quizzId, successQuizz, errorLoad);
            categories = cachedDataSvc.getQuizzzCategories(successCategories, errorLoad);
            quizzGradeLevelList = cachedDataSvc.getGradeLevels(successGradeLevels, errorLoad);

            processHelp($stateParams, $scope);
        }

        function updateQuestions() {
            if (quizzOverview.numQuestions < 10)
                liveControl.toggle();
        }

        var categoryIconControl: any = {};
        var quizzControl = (function () {
            var vars = {
                isOpen: false,
                className: "",
                description: "",
                tags: "",
                isSaveEnabled: false,
                disabled: false,
                disableAll: false
            };

            function errorSave() {
                notificationSvc.error(str.errorSave);
            }

            function gradeLevelChanged(value: string): void {
                quizz.isModified = true;
                var min = parseInt(quizz.gradeLevelMin);
                var max = parseInt(quizz.gradeLevelMax);
                if (max < min) {
                    if (value == "min")
                        quizz.gradeLevelMax = quizz.gradeLevelMin;
                    else
                        quizz.gradeLevelMin = quizz.gradeLevelMax;
                }
            }

            function changed(model: ITrackModify): void {
                model.isModified = true;
            }

            function edit() {
                vars.isOpen = true;
            }

            function editSave(form: ng.IFormController) {

                function notifyChanged(eventType: bcEventTypeEnum, data) {
                    var bcData: IBroadcastData = {
                        eventType: eventType,
                        model: data
                    }
                    //broadcastSvc.postMyQuizzEvent(bcData);
                }

                function saveEditQuizz() {
                    if (quizz.isModified == true)
                        quizzSvc.updateQuizz(quizz,
                            function (data) {
                                cachedDataSvc.deleteQuizzOverviewById(data.id);
                                util.enableForm(page);
                                notifyChanged(bcEventTypeEnum.modifyItem, quizz);
                                notificationSvc.success(str.updateSuccess);

                                quizzOverview.title = quizz.title;
                                quizzOverview.description = quizz.description;
                                quizzOverview.category = quizz.category;
                                quizzOverview.gradeLevelMin = quizz.gradeLevelMin;
                                quizzOverview.gradeLevelMax = quizz.gradeLevelMax;
                                quizzOverview.quizzTags = quizz.quizzTags;
                                $timeout(function () {
                                    $scope.categoryIconControl.update();
                                }, 200);

                                vars.isOpen = false;

                                layoutSvc.updateMyQuizzes();

                            }, errorSave);
                }

                quizz.gradeLevelMin = parseInt(quizz.gradeLevelMin);
                quizz.gradeLevelMax = parseInt(quizz.gradeLevelMax);

                if (form.$valid) {
                    util.disableForm(page);
                    saveEditQuizz();
                }
            }

            function editCancel() {
                vars.isOpen = false;
            }

            function remove() {

                function successDelete() {
                    layoutSvc.updateMyQuizzes();

                    notificationSvc.success(str.deleteSuccess);
                    $state.go(util.getDefaultLocation());
                }

                function errorDelete() {
                    notificationSvc.error(str.errorSave);
                }

                dialogSvc.confirm(str.confirmDelete, function (result) {
                    if (result == true) {
                        quizzSvc.deleteQuizz(quizz.id, successDelete, errorDelete);

                    }
                });
            }

            return {
                vars: vars,
                changed: changed,

                edit: edit,
                editSave: editSave,
                editCancel: editCancel,

                gradeLevelChanged: gradeLevelChanged,
                remove: remove
            }
        })();

        //var ratingControl = (function () {
        //    var vars = {
        //        isReady: false,
        //        uiValue: 0,
        //        numRatings: 0,
        //    };

        //    function init() {
        //        var quizzUserRating: IQuizzUserRatingModel;

        //        quizzUserRating = resourceSvc.getResource(enums.ResourceTypeEnum.QuizzUserRating, quizzId,
        //            function (data: IQuizzUserRatingModel) {
        //                vars.uiValue = Math.ceil(data.ratingAvg);
        //                vars.isReady = true;
        //                vars.numRatings = data.numRatings;
        //            },
        //            function () {
        //                vars.isReady = true;
        //            });
        //    }

        //    return {
        //        vars: vars,
        //        init: init
        //    }
        //})();

        //var upvoteControl = (function () {
        //    var vars = {
        //        isReady: true,
        //    };

        //    function upvote() {
        //        function successUpDownVote(data) {
        //            vars.isReady = true;
        //            quizzOverview.isLiked = !quizzOverview.isLiked;
        //            if (quizzOverview.isLiked)
        //                quizzOverview.numLikes++;
        //            else
        //                quizzOverview.numLikes--;
        //        }

        //        function errorUpDownVote() {
        //            vars.isReady = true;
        //            notificationSvc.error(str.errorSave);
        //        }

        //        if (user.id == quizzOverview.ownerId || user.id === 0)
        //            return;

        //        vars.isReady = false;
        //        if (quizzOverview.isLiked == false) {
        //            quizzUpvoteSvc.upVote(quizzOverview.id, successUpDownVote, errorUpDownVote);
        //        } else {
        //            quizzUpvoteSvc.downVote(quizzOverview.id, successUpDownVote, errorUpDownVote);
        //        }
        //    }

        //    return {
        //        vars: vars,
        //        upvote: upvote
        //    }
        //})();

        var liveControl = (function () {
            var vars = {
                isReady: true,
                title: "",
                tooltip: ""
            };

            function updateLiveBtn() {
                vars.title = quizzOverview.isLive ? "Make Private" : "Publish";

                if (quizzOverview.numQuestions < 10) {
                    vars.tooltip = "There should at least be 10 questions";

                    
                }
            }

            function toggle() {
                vars.isReady = false;
                var tmpValue: boolean = !quizzOverview.isLive;
                quizzSvc.setLive({ id: quizzOverview.id, value: tmpValue },
                    function () {
                        quizzOverview.isLive = tmpValue;
                        cachedDataSvc.deleteQuizzById(quizzOverview.id);
                        updateLiveBtn();
                        vars.isReady = true;
                    },
                    function () {
                        vars.isReady = true;
                        notificationSvc.error(str.errorSave);
                    });
            }

            function init() {
                updateLiveBtn();
            }

            return {
                vars: vars,
                init: init,
                toggle: toggle
            }
        })();

        var shareControl = (function () {
            function share(where: string) {
                if (where === 'facebook') {
                    var message: string = "";
                    if (user.id === quizz.ownerId)
                        message = "Check out the Quizz that I made on QuizzOCake : " + quizz.title;
                    else
                        message = "Check out this Quizz on QuizzOCake : " + quizz.title;
                    var meta: IOGMeta = {
                        type: 0,
                        url: $location.absUrl()
                    }
                    facebookSvc.share(message, meta);
                    return;
                }
            }

            return {
                share: share
            }
        })();

        function goBack(): void {
            history.back();
        }

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.goBack = goBack;

        $scope.tipControl = {};

        $scope.quizz = quizz;
        $scope.categoryIconControl = categoryIconControl;
        $scope.quizzControl = quizzControl;
        //$scope.ratingControl = ratingControl;
        //$scope.upvoteControl = upvoteControl;
        $scope.liveControl = liveControl;
        $scope.shareControl = shareControl;
    }
})();