module quizzmateMsgCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;

        msgThread: IQuizzmateMsgThreadModel;
        msgMembers: IQuizzmateMsgThreadMemberModel[];
        messages: IQZConnectMessage[];
        message: IMessage;
        send(): void;

        quizzmates: IQuizzerModel[];

        loadMore(): void;
        $on: any;

        msgThreadGroupCtrl: any;

    }

    export interface IMessage {
        content: string;
    }

    export interface IStateParams {
        threadId: string;
        userId: number;
        depId: number;
    }
}

(function () {
    l2lControllers.controller('quizzmateMsgCtrl', quizzmateMsgCtrl);

    quizzmateMsgCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "hubSvc"];

    function quizzmateMsgCtrl(
        $scope: quizzmateMsgCtrl.IScope,
        $stateParams: quizzmateMsgCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        resourceSvc: IResourceSvc,
        $timeout: ng.ITimeoutService,
        dialogSvc: IDialogSvc,
        hubSvc: IHubSvc
    ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 1,
            disabled: false,
            isNotFoundOrAuthorized: false
        };
        var user: IUserModel = currentUser.getUserData();
        var messages: IQZConnectMessage[] = new Array<IQZConnectMessage>();
        var chat: any;
        var msgThread: IQuizzmateMsgThreadModel = {};
        var msgMembers: IQuizzmateMsgThreadMemberModel[];
        var lastSenderId: number = 0;
        var quizzmates: IQuizzerModel[] = new Array<IQuizzerModel>();
        var depId: number = 0;
        var $newMessageTextBox: JQuery = undefined;

        function errorSave() {
            notificationSvc.error(str.errorAddMember);
            msgThreadGroupCtrl.reset();
        }

        function init() {

            function errorLoad(response) {
                notificationSvc.error(str.errorLoad);
                page.isReady = true;
                page.isNotFoundOrAuthorized = true;
            }

            function updateIsReady() {
                page.numResourceToWait--;
                page.isReady = page.numResourceToWait <= 0;
            }

            function successQuizzmates(data: IQuizzerModel[]) {
                $scope.quizzmates = quizzmates = data;
                updateIsReady();
            }

            
            function initChat() {
                function connectKeyPress() {
                    $newMessageTextBox = $("#new-message");
                    var $messageEnd = $("#message-end");
                    if ($newMessageTextBox === undefined || $newMessageTextBox === null || $messageEnd === undefined || $messageEnd === null) {
                        $timeout(function () {
                            connectKeyPress();
                        }, 1000);

                        return;
                    }

                    $timeout(function () {
                        $(".qz-connect-messages").scrollTo($messageEnd);
                    }, 1000);

                    $newMessageTextBox.keypress(function (e) {
                        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                            $('#send-button').click();
                            return false;
                        } else {
                            return true;
                        }
                    });
                }

                hubSvc.registerQuizzmateMsgListener(msgThread.id, receiveMsg);
                hubSvc.joinQuizzmatemsgGroup(msgThread.signalRGroupName, msgThread.id, user.id);

                connectKeyPress();
            }

            function successMsgThread(data: IQuizzmateMsgThreadModel) {
                function updatePageTitle() {
                    if (depId !== -1) {
                        var isFound: boolean = false;
                        msgThread.msgThreadMembers.forEach(function (item: IQuizzmateMsgThreadMemberModel) {
                            if (item.userId === depId)
                                isFound = true;
                        });
                        if (isFound === false)
                            page.isNotFoundOrAuthorized = true;

                        page.title = "Quizzling Messages";
                        page.subTitle = data.groupMessageName;
                    } else {
                        page.title = data.groupMessageName;
                    }
                }

                $scope.msgThread = msgThread = data;
                $scope.msgMembers = msgMembers = msgThread.msgThreadMembers;

                if (msgThread.isGroupMsg) {
                    page.numResourceToWait++;
                    resourceSvc.getResourceMany(enums.ResourceTypeEnum.Quizzmates, 0, 0, 0, 0, 0, successQuizzmates, errorLoad);
                }

                updateIsReady();

                updatePageTitle();

                data.messages.forEach(function (item: IQuizzmateMsgModel) {
                    var msg: IQZConnectMessage = {
                        isSelf: item.authorId == user.id,
                        isSameAsLastSender: item.authorId === lastSenderId,
                        userName: item.userName,
                        profileImageUrl: item.profileImageUrl,
                        message: item.message,
                        dateTime: ""
                    };
                    messages.push(msg);
                    lastSenderId = item.authorId;
                });

                initChat();
            }

            $anchorScroll("top");
            depId = util.getNumber($stateParams.depId);
            depId = depId == 0 ? -1 : depId;
            var idx: number = util.getNumber($stateParams.userId);
            if ($stateParams.threadId !== undefined && $stateParams.threadId !== "")
                msgThread = resourceSvc.getResourceAlt(enums.ResourceTypeEnum.QuizzmateMsgThread, $stateParams.threadId, successMsgThread, errorLoad);
            else if (idx !== -1 || idx !== 0)
                msgThread = resourceSvc.getResource(enums.ResourceTypeEnum.QuizzmateMsgThread, idx, successMsgThread, errorLoad);

            //$(document).ready(function () {
            //    $("#new-message").keypress(function (e) {
            //        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            //            $('#send-button').click();
            //            return false;
            //        } else {
            //            return true;
            //        }
            //    });
            //});

            $scope.$on("$destroy", destroy);
        }

        function send(): void {
            if ($scope.message.content.trim() !== "")
                hubSvc.sendQuizzmateMsg(msgThread.id, msgThread.signalRGroupName, user.id, $scope.message.content);
            $scope.message.content = "";
        }

        function receiveMsg(userId, message) {
            var sender: IQuizzmateMsgThreadMemberModel;
            msgMembers.forEach(function (item: IQuizzmateMsgThreadMemberModel) {
                if (item.userId == userId)
                    sender = item;
            });
            var msg: IQZConnectMessage = {
                isSelf: sender.userId == user.id,
                isSameAsLastSender: sender.userId === lastSenderId,
                userName: sender.userName,
                profileImageUrl: sender.profileImageUrl,
                message: message,
                dateTime: ""
            };

            lastSenderId = userId;

            $timeout(function () {
                messages.push(msg);

                $timeout(function () {
                    var $messageEnd = $("#message-end");
                    $(".qz-connect-messages").scrollTo($messageEnd);
                }, 200);
            }, 100);
        };

        function loadMore(): void {
            //alert("near top");
        }

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        function destroy() {
            hubSvc.leaveQuizzmateMsgGroup(msgThread.signalRGroupName, msgThread.id, user.id);
            hubSvc.unRegisterQuizzmateMsgListener();

            if ($newMessageTextBox !== undefined)
                $newMessageTextBox.unbind('keypress');

        }

        var msgThreadGroupCtrl = (function () {
            var vars = {
                isOpen: false,
                selectedUserId: 0,
                isAddEnabled: false,
                disabled: false,

            };

            function reset() {
                vars.disabled = false;
                vars.isOpen = false
                vars.selectedUserId = 0
            }

            function successCreate(data: IQuizzmateMsgThreadMemberModel) {
                reset();
            }

            function open() {
                vars.isOpen = true;
            }

            function close() {
                vars.isOpen = false;
            }

            function addMember() {
                vars.disabled = true;
                var member: IQuizzmateMsgThreadMemberModel = {
                    id: 0,
                    quizzmateMsgThreadId: msgThread.id,
                    userId: vars.selectedUserId
                };

                resourceSvc.createResource(enums.ResourceTypeEnum.QuizzmateMsgThreadMember, member, successCreate, errorSave);

            }

            function leaveGroup() {
                dialogSvc.confirm(str.leaveChatGroup, function (result: boolean) {
                    function successLeaveGroup() {
                        vars.disabled = false;
                        notificationSvc.success("Successfully left " + msgThread.groupMessageName);
                        $state.go("si.allMessage");
                    }

                    if (result == true) {
                        msgMembers.forEach(function (item: IQuizzmateMsgThreadMemberModel) {
                            if (item.userId == user.id) {
                                vars.disabled = true;
                                resourceSvc.deleteResource(enums.ResourceTypeEnum.QuizzmateMsgThreadMember, item.id, successLeaveGroup, errorSave);
                            }
                        });
                    }
                });
            }

            function selectedChanged() {
                vars.isAddEnabled = true;
            }

            return {
                vars: vars,
                open: open,
                close: close,
                addMember: addMember,
                leaveGroup: leaveGroup,
                selectedChanged: selectedChanged,
                reset: reset
            }
        })();

        init();

        $scope.user = user;
        $scope.page = page;
        $scope.msgThread = msgThread;
        $scope.msgMembers = msgMembers;
        $scope.messages = messages;
        $scope.message = {
            content: ""
        };
        $scope.send = send;
        $scope.quizzmates = quizzmates;
        $scope.loadMore = loadMore;

        $scope.msgThreadGroupCtrl = msgThreadGroupCtrl;
    }
})();