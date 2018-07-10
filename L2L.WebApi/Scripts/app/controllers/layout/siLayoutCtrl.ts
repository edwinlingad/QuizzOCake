/// <reference path="../../../typings/modernizr/modernizr.d.ts" />
module siLayoutCtrl {

    export interface IScope {
        user: IUserModel;
        clickLogo(): void;
        layout: ILayoutModel;
        openSignUpDependentModal: () => void;
        // Ongoing test
        goToOngoingTest(item: ILayoutTestSnapshot): void;

        notificationItem: IItemToWait;
        notificationBtn: siLayoutCtrl.INotifyElement;
        friendRequestBtn: siLayoutCtrl.ITopPanelElement;
        messageBtn: siLayoutCtrl.ITopPanelElement;

        topPanelContent: any;
        globalSearchControl: siLayoutCtrl.IGlobalSearch;
        userControl: any;
        signInUpControl: any;
        mainMenuControl: any;

        promoInfoControl: any;

        $on: any;
    }

    export interface IGlobalSearch {
        prevUrl: string;
        searchHandle: any;
        searchText: string;
        search(): void;
    }

    export interface ITopPanelElement {
        newCount?: number;
        isLoaded?: boolean;
        init?: () => void;
        click?: () => void;
        hover?: () => void;
        templateUrl?: string;
        openQuizz?: (item: INewNotificationModel) => void;
    }

    export interface INotifyElement extends ITopPanelElement {
        list: INewNotificationModel[];
    }
}

(function () {
    l2lControllers.controller('siLayoutCtrl', siLayoutCtrl);

    function createEmptyLayoutModel(): ILayoutModel {
        var model: ILayoutModel = {
            topPanel: {

            },
            leftSideBar: {
                testSnapshots: new Array(),
                dependents: new Array(),
                assignments: new Array(),
                assignmentsGiven: new Array(),
                recentQuizzes: new Array(),
                myQuizzes: new Array(),
                bookmarks: new Array(),
                suggestedQuizzes: new Array(),
                groups: new Array()
            }
        }
        return model;
    }

    siLayoutCtrl.$inject = ["$scope", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "dialogSvc", "layoutSvc", "broadcastSvc", "accountSvc", "modalSvc", "userNotificationSvc", "$timeout", "takeTestCtrlSvc", "$location", "$rootScope", "hubSvc", "topPanelContentSvc", "loginModalSvc", "resourceSvc"];
    function siLayoutCtrl(
        $scope: siLayoutCtrl.IScope,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        dialogSvc: IDialogSvc,
        layoutSvc: ILayoutSvc,
        broadcastSvc: IBroadcastSvc,
        accountSvc: IAccount,
        modalSvc: IModalSvc,
        userNotificationSvc: IUserNotificationSvc,
        $timeout: ng.ITimeoutService,
        takeTestCtrlSvc: ITakeTestCtrlSvc,
        $location: ng.ILocationService,
        $rootScope,
        hubSvc: IHubSvc,
        topPanelContentSvc: ITopPanelContentSvc,
        loginModalSvc: ILoginModalSvc,
        resourceSvc: IResourceSvc
    ) {
        var notificationItem: IItemToWait = { isReady: true };
        var notificationBtn: siLayoutCtrl.INotifyElement = {
            newCount: 0,
            isLoaded: false,
            templateUrl: "notification.html",
            init: function () {
                userNotificationSvc.getNewNotificationsCount(function (data) {
                    notificationBtn.newCount = data.count;
                });
            },
            click: function () {
                notificationBtn.newCount = 0;
            },
            openQuizz: function (item: INewNotificationModel) {
                item.isNew = false;
                $state.go("si.quizzDetail", { quizzId: item.quizzId, view: 0 });
            },
            list: new Array<INewNotificationModel>()
        };
        var friendRequestBtn: siLayoutCtrl.ITopPanelElement = {
            newCount: 0,
            click: function () {
                friendRequestBtn.newCount = 0;
            }
        };

        function errorLoad() {
        }

        var messageBtn: siLayoutCtrl.ITopPanelElement = {
            newCount: 0,
            click: function () {
                messageBtn.newCount = 0;
            }
        };
        var user: IUserModel = $scope.user = currentUser.getUserData();
        var layout: ILayoutModel = createEmptyLayoutModel();
        var globalSearchControl: siLayoutCtrl.IGlobalSearch = {
            prevUrl: "",
            searchText: "",
            searchHandle: {},
            search: function () {
                if (globalSearchControl.searchHandle !== undefined)
                    $timeout.cancel(globalSearchControl.searchHandle);

                globalSearchControl.searchHandle = $timeout(function () {
                    if (globalSearchControl.searchText.trim() === "") {
                        if (globalSearchControl.prevUrl.trim() === "")
                            return;

                        //console.log(globalSearchCtrl.prevUrl);
                        $location.url(globalSearchControl.prevUrl);
                        globalSearchControl.prevUrl = "";
                        return;
                    }

                    if (globalSearchControl.prevUrl.trim() == "")
                        globalSearchControl.prevUrl = $location.url();
                    $state.go("si.searchAll", { search: globalSearchControl.searchText });

                }, 1000);
            }
        };

        function clickLogo() {
            if (user.id === 0) {
                $state.go("nsi.index");
                return;
            }

            $state.go(util.getDefaultLocation());
        }

        function init() {
            function updateLayout() {
                //$scope.user = user = currentUser.getUserData();
                layoutSvc.getLayoutModel(
                    function (data) {
                        var model: ILayoutModel = data;
                        $scope.layout = layout = model;
                        notificationBtn.newCount = layout.topPanel.newNotificationCount;
                        messageBtn.newCount = layout.topPanel.newMessageCount;
                        friendRequestBtn.newCount = layout.topPanel.newFriendRequestCount;

                        // new implementation
                        topPanelContentSvc.addToNewQuizzClassCount(layout.topPanel.newQuizzClassNotificationCount);

                        hubSvc.joinLayoutGroup("QuizzOCake-" + user.id);
                    },
                    function () {
                        // Not logged in
                        // notificationSvc.error(str.errorLoad);
                    });
            }

            function errorCurrentUser() {
                notificationSvc.error(str.errorConnection);
            }

            function successCurrentUser() {
                broadcastSvc.addUpdateLayoutListener($scope, function (event) {
                    updateLayout();
                });

                cachedDataSvc.getGradeLevels();
                cachedDataSvc.getQuizzzCategories();
                cachedDataSvc.getNotificationTypes();

                $scope.topPanelContent = topPanelContentSvc.vars;

                if (user.id !== 0) {
                    updateLayout();
                    hubSvc.registerLayouteMsgListener(layoutHubMsgReceiver);
                }
            }

            function initUrlListener() {
                $rootScope.$on("$locationChangeSuccess", function (event, next: string, current) {
                    if (next.indexOf("search-all") == -1) {
                        globalSearchControl.searchText = "";
                    }
                });
            }

            // Init Start
            currentUser.init(successCurrentUser, errorCurrentUser);
            initUrlListener();
        }

        function openSignUpDependentModal() {
            var settings: ng.ui.bootstrap.IModalSettings = {
                templateUrl: "scripts/templates/account/SignUpDependent.html",
                controller: "registerUserCtrl",
                size: "sm",
            }

            modalSvc.open(settings, function () {
            });
        }

        function layoutHubMsgReceiver(sType: number, iParam: number, sParam: string) {
            $timeout(function () {
                switch (sType) {
                    case 0: // quizzmate message
                        var threadId: number = iParam;
                        if (threadId != hubSvc.getCurrQuizzmateThreadId())
                            messageBtn.newCount++;
                        break;
                }
            }, 200);
        }

        function goToOngoingTest(item: ILayoutTestSnapshot): void {
            $state.go("si.loadTest", { testSnapId: item.id });
        }

        var signInUpControl = (function () {
            var handle: any = undefined;
            function openLoginModal(): void {
                if (handle !== undefined && !handle.closed)
                    return;

                handle = loginModalSvc.openLoginModal(false, false, function () {
                });
            }

            function openSignUpModal(): void {
                loginModalSvc.openSignUpModal();
            }

            return {
                openLoginModal: openLoginModal,
                openSignUpModal: openSignUpModal
            }

        })();

        var userControl = (function () {

            function openProfile(): void {
                $state.go("si.quizzer", { quizzerId: user.id, view: 0 });
            }

            function openChangePasswordModal(): void {
                var settings: ng.ui.bootstrap.IModalSettings = {
                    templateUrl: "scripts/templates/account/management/ChangePassword.html",
                    controller: "changePasswordCtrl",
                    size: "sm"
                }

                modalSvc.open(settings, function () {
                });
            }

            function logOut() {
                dialogSvc.confirm(str.confirmLogout,
                    function (result: boolean) {
                        function successLogout() {
                            $state.go("nsi.index");
                        }

                        function errorLogout() {
                            notificationSvc.error(str.errorConnection);
                        }

                        if (result == true) {
                            accountSvc.logOut(successLogout, errorLogout);
                            currentUser.logOff();
                        }
                    });
            }

            return {
                openProfile: openProfile,
                openChangePasswordModal: openChangePasswordModal,
                logOut: logOut
            }
        })();

        var mainMenuControl = (function () {
            var vars = {
                isOpen: false
            }

            function open() {
                vars.isOpen = true;
                //$nlDrawer.show();
            }

            function close() {
                vars.isOpen = false;
            }

            return {
                vars: vars,
                open: open,
                close: close
            }
        })();

        var promoInfoControl = (function () {
            var vars = {
                totalQuestions: 5234
            }

            function successTestLog(data: any) {
                vars.totalQuestions = data.count;
            }

            function init() {
                resourceSvc.getResource(enums.ResourceTypeEnum.TestLog, 0, successTestLog, errorLoad);
            }

            return {
                vars: vars,
                init: init
            }
        })();

        init();
        promoInfoControl.init();

        $scope.layout = layout;
        $scope.user = user;
        $scope.clickLogo = clickLogo;
        $scope.openSignUpDependentModal = openSignUpDependentModal;
        // Ongoing Test
        $scope.goToOngoingTest = goToOngoingTest;

        $scope.notificationItem = notificationItem;
        $scope.notificationBtn = notificationBtn;
        $scope.friendRequestBtn = friendRequestBtn;
        $scope.messageBtn = messageBtn;

        $scope.globalSearchControl = globalSearchControl;
        $scope.signInUpControl = signInUpControl;
        $scope.userControl = userControl;
        $scope.mainMenuControl = mainMenuControl;

        $scope.promoInfoControl = promoInfoControl;
    };
})();