module nsiLayoutCtrl {
    export interface IScope {
        page: IPage;
        openLoginModal: (isGuest: boolean) => void;
        openSignUpModal: () => void;
        addNewQuizz: () => void;
        gotoHash(hash: string): void;
        watchOverview(): void;

        slides: nsiLayoutCtrl.ISlide[];
        $on: any;

        promoInfoControl: any;
    }

    export interface ISlide {
        image: string;
        text: string;
    }
}

(function () {
    l2lControllers.controller("nsiLayoutCtrl", nsiLayoutCtrl);

    nsiLayoutCtrl.$inject = ["$scope", "modalSvc", "$state", "accountSvc", "dialogSvc", "currentUser", "loginModalSvc", "$location", "$anchorScroll", "$timeout", "resourceSvc"];
    function nsiLayoutCtrl(
        $scope: nsiLayoutCtrl.IScope,
        modalSvc: IModalSvc,
        $state: ng.ui.IStateService,
        accountSvc: IAccount,
        dialogSvc: IDialogSvc,
        currentUser: ICurrentUser,
        loginModalSvc: ILoginModalSvc,
        $location: ng.ILocationService,
        $anchorScroll: any,
        $timeout: ng.ITimeoutService,
        resourceSvc: IResourceSvc
    ) {
        var page: IPage = {
            isReady: false,
            numResourceToWait: 1,
            disabled: false
        };
        var user: IUserModel = currentUser.getUserData();
        var slides: nsiLayoutCtrl.ISlide[] = [
            {
                image: "/content/images/carousel/01.jpg",
                text: ""
            },
            {
                image: "/content/images/carousel/02.jpg",
                text:""
            },
            {
                image: "/content/images/carousel/03.jpg",
                text: ""
            },
            {
                image: "/content/images/carousel/04.jpg",
                text: ""
            },
            {
                image: "/content/images/carousel/05.jpg",
                text: ""
            },
            {
                image: "/content/images/carousel/06.jpg",
                text: ""
            },
            {
                image: "/content/images/carousel/07.jpg",
                text: ""
            },
            {
                image: "/content/images/carousel/08.jpg",
                text: ""
            },
            {
                image: "/content/images/carousel/09.jpg",
                text: ""
            },
            {
                image: "/content/images/carousel/10.jpg",
                text: ""
            },
            {
                image: "/content/images/carousel/11.jpg",
                text: ""
            },
            {
                image: "/content/images/carousel/12.jpg",
                text: ""
            }
        ];

        function errorLoad() {
        }

        function init() {

            function initScrollTop() {
                var docElem = document.documentElement,
                    $topButton = $('#goto-top-button-div'),
                    didScroll = false,
                    showButtonOn = 350;

                function init() {
                    $(window).scroll(function (event) {
                        if (!didScroll) {
                            didScroll = true;
                            setTimeout(scrollPage, 250);
                        }
                    });

                    $scope.$on('$destroy', function () {
                        $(window).unbind('scroll');
                    });
                }

                function scrollPage() {
                    var sy = scrollY();
                    if (sy >= showButtonOn) {
                        $topButton.css('opacity', 0.7);
                    }
                    else {
                        $topButton.css('opacity', 0);
                    }
                    didScroll = false;
                }

                function scrollY() {
                    return window.pageYOffset || docElem.scrollTop;
                }

                init();
                $timeout(function () {
                    $('.ed-img-fill-screen').css('height', $(window).innerHeight());
                }, 1000);

                $timeout(function () {
                    page.isReady = true;
                }, 2000);

            }

            function successCurrentUser() {
                if (user.id !== 0) {
                    $state.go(util.getDefaultLocation());
                    return;
                }

                initScrollTop();
            }

            currentUser.init(successCurrentUser, errorLoad);
        }

        var handle: any = undefined;
        function openLoginModal(isGuest: boolean): void {
            if (handle !== undefined && !handle.closed)
                return;
            handle = loginModalSvc.openLoginModal(false, false, function () {

            });
        }

        function openSignUpModal(): void {
            loginModalSvc.openSignUpModal();
        }

        function gotoHash(hash: string): void {
            $('html, body').stop().animate({
                scrollTop: $('#' + hash).offset().top
            }, 1500, 'easeInOutExpo');
        }

        function watchOverview() {
            var settings: ng.ui.bootstrap.IModalSettings = {
                templateUrl: "scripts/templates/about/VideoOverview.html",
                controller: "videoOverviewCtrl",
                size: "lg"
            }

            modalSvc.open(settings, function (result: boolean) {
                if (result == true) {
                }
            });
        }

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

        $scope.page = page;
        $scope.openLoginModal = openLoginModal;
        $scope.openSignUpModal = openSignUpModal;
        $scope.gotoHash = gotoHash;
        $scope.slides = slides;
        $scope.watchOverview = watchOverview;

        $scope.promoInfoControl = promoInfoControl;
    }

})();