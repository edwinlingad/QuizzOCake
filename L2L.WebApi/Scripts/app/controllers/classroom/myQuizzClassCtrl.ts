module myQuizzClassCtrl {
    export interface IScope {
        user: IUserModel;
        page: IPage;
        depId: number;
        view: number;

        quizzClass: IQuizzClassModel;
        quizzClassControl: any;
        imageControl: IImageEditorControl;

        goBack(): void;
    }

    export interface IStateParams {
        qcId: number;
        depId: number;
        view: number;
    }
}

(function () {
    l2lControllers.controller('myQuizzClassCtrl', myQuizzClassCtrl);

    myQuizzClassCtrl.$inject = ["$scope", "$stateParams", "$state", "cachedDataSvc", "notificationSvc", "currentUser", "$anchorScroll", "$location", "resourceSvc", "$timeout", "dialogSvc", "topPanelContentSvc"];

    function myQuizzClassCtrl(
        $scope: myQuizzClassCtrl.IScope,
        $stateParams: myQuizzClassCtrl.IStateParams,
        $state: ng.ui.IStateService,
        cachedDataSvc: ICachedDataSvc,
        notificationSvc: INotificationSvc,
        currentUser: ICurrentUser,
        $anchorScroll: ng.IAnchorScrollService,
        $location: ng.ILocationService,
        resourceSvc: IResourceSvc,
        $timeout: ng.ITimeoutService,
        dialogSvc: IDialogSvc,
        topPanelContentSvc: ITopPanelContentSvc
    ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 1,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var quizzClass: IQuizzClassModel = {};
        var imageControl: IImageEditorControl = {
        };

        function errorSave(response) {
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

            function successQuizzClass(data: IQuizzClassModel) {
                $scope.quizzClass = quizzClass = data;
                page.title = quizzClass.className;
                updateIsReady();

                switch ($scope.view) {
                    case 0: // summary
                        break;
                    case 1: // announement

                        break;
                    case 2: // materials
                        break;
                    case 3: // lessons
                        break;
                    case 4: // discussions
                        topPanelContentSvc.subFromNewQuizzClassCount(quizzClass.member.newClassCommentCount);
                        break;
                    case 5: // learners
                        break;
                    case 6: // quizzes
                        topPanelContentSvc.subFromNewQuizzClassCount(quizzClass.member.newClassQuizzCount);
                        break;
                }
            }

            $anchorScroll("top");
            var qcId: number = util.getNumber($stateParams.qcId);
            $scope.depId = util.getNumber($stateParams.depId);
            $scope.view = util.getNumber($stateParams.view);

            if ($scope.depId === -1) 
                quizzClass = resourceSvc.getResource(enums.ResourceTypeEnum.QuizzClass, qcId, successQuizzClass, errorLoad);
            else
                quizzClass = resourceSvc.getResourceAlt(enums.ResourceTypeEnum.QuizzClass, qcId.toString() + "," + $scope.depId.toString(), successQuizzClass, errorLoad);
        }

        var quizzClassControl = (function () {
            var vars = {
                isOpen: false,
                className: "",
                description: "",
                tags: "",
                isSaveEnabled: false,
                disabled: false,
                disableAll: false
            };

            function changed() {
                if (vars.className.trim() === "")
                    vars.isSaveEnabled = false;
                else
                    vars.isSaveEnabled = true;
            }

            function edit() {
                vars.isOpen = true;

                vars.className = quizzClass.className;
                vars.description = quizzClass.description;
                vars.tags = quizzClass.tags;

                util.imagetEditorLoad(imageControl, quizzClass, changed);
            }

            function editSave() {

                function successUpdate() {
                    vars.disabled = false;

                    vars.isOpen = false;
                    quizzClass.className = vars.className;
                    quizzClass.description = vars.description;
                    quizzClass.tags = vars.tags;

                    if (imageControl.isImageChanged) {

                        var newFileName = imageControl.newImageFileName;
                        var oldImageUrl = quizzClass.imageUrl;
                        util.editorLoad(quizzClass, imageControl);

                        if (oldImageUrl === "Content/images/classroom/class-default.jpg") {
                            quizzClass.imageUrl = "Content/images/QuizzClass/QuizzClass_" + quizzClass.id.toString() + "/" + newFileName;
                        }
                        else {
                            var lastIdx: number = oldImageUrl.lastIndexOf("/");
                            quizzClass.imageUrl =
                                oldImageUrl.substr(0, lastIdx + 1) + newFileName;
                        }
                    }

                    vars.className = "";
                    vars.description = "";
                    notificationSvc.success(str.updateSuccess);
                }

                var model: IQuizzClassModel = {};
                util.copy(model, quizzClass);

                model.className = vars.className;
                model.description = vars.description;
                model.tags = vars.tags;

                util.imagetEditorLoad(model, imageControl);

                resourceSvc.updateResource(enums.ResourceTypeEnum.QuizzClass, model, successUpdate, errorSave);
            }

            function editCancel() {
                vars.isOpen = false;
            }

            function remove() {
                function successRemove() {
                    notificationSvc.success(str.deleteSuccess);
                    $state.go("si.quizzClassAll");
                }

                dialogSvc.confirm(str.confirmDelete, function (result: boolean) {
                    if (result) {
                        resourceSvc.deleteResource(enums.ResourceTypeEnum.QuizzClass, quizzClass.id, successRemove, errorSave);
                    }
                });
            }

            return {
                vars: vars,
                changed: changed,

                edit: edit,
                editSave: editSave,
                editCancel: editCancel,

                remove: remove,
            }
        })();

        function goBack() {
            $timeout(function () {
                history.back();
            }, 100);
        }

        init();

        $scope.user = user;
        $scope.page = page;

        $scope.quizzClass = quizzClass;
        $scope.quizzClassControl = quizzClassControl;
        $scope.imageControl = imageControl;

        $scope.goBack = goBack;
    }
})();