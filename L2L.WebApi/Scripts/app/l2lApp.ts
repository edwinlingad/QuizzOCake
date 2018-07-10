/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/angular-ui-router/angular-ui-router.d.ts" />
/// <reference path="../typings/ionic/ionic.d.ts" />

declare var FastClick: any;

var baseTemplateUrl: string = 'scripts/templates/';

function goToTop() {
    document.body.scrollTop = document.documentElement.scrollTop = 0;
}

var l2lApp = angular.module('l2lApp', ['l2lControllers', 'ngRoute', 'ngResource', 'ui.router', 'ui.bootstrap', 'ngAnimate', 'ui.tinymce', 'ngSanitize', 'ngCookies', 'zInfiniteScroll', 'viewhead'])
    .config(l2lAppConfig);

l2lAppConfig.$inject = ["$routeProvider", "$locationProvider", "$stateProvider", "$urlRouterProvider", "$sceDelegateProvider"];

function l2lAppConfig(
    $routeProvider: ng.route.IRouteProvider,
    $locationProvider: ng.ILocationProvider,
    $stateProvider: ng.ui.IStateProvider,
    $urlRouterProvider: ng.ui.IUrlRouterProvider,
    $sceDelegateProvider: ng.ISCEDelegateProvider
) {

    //intializeFastClick();

    initializeSCE($sceDelegateProvider);

    //if (!setting.isMobileApp())
    $urlRouterProvider.otherwise("/n/index");

    $routeProvider
        .when(
        '/', {
            redirectTo: '/n/index'
        })
        .otherwise({
            redirectTo: '/n/index'
        });

    $stateProvider
        .state('resetPassword', {
            url: '^/account/reset-password?userName&code',
            templateUrl: baseTemplateUrl + 'account/management/ResetPassword.html',
            controller: "resetPasswordCtrl",
            onEnter: function () {
                setTimeout(function () {
                    $('.ed-img-fill-screen').css('height', $(window).innerHeight());
                }, 1000);
            }
        })
        .state('confirmEmail', {
            url: '^/account/confirm-email?userId&code',
            controller: "confirmEmailCtrl"
        })
        .state('privacyNotice', {
            url: '^/legal/privacy-notice',
            templateUrl: baseTemplateUrl + 'legal/PrivacyNotice.html',
        })
        .state('termsAndConditions', {
            url: '^/legal/terms-and-conditions',
            templateUrl: baseTemplateUrl + 'legal/TermsAndConditions.html',
        })
        .state('helpAndSupport', {
            url: '^/about/help-and-support',
            templateUrl: baseTemplateUrl + 'about/HelpAndSupport.html',
            controller: "helpAndSupportCtrl"
        })


    $stateProvider
        .state('nsi', {
            abstract: true,
            url: '/n',
            templateUrl: baseTemplateUrl + 'layout/nsiLayout.html',
            controller: "nsiLayoutCtrl"
        })
        .state('nsi.index', {
            url: '/index',
            templateUrl: baseTemplateUrl + 'home/Index.html',
            controller: 'homeIndexCtrl',
            //onEnter: function () {
            //    setTimeout(function () {
            //        $('.ed-img-fill-screen').css('height', $(window).innerHeight());
            //    }, 1000);
            //}
        })
        .state('si', {
            abstract: true,
            url: '/u',
            templateUrl: baseTemplateUrl + 'layout/siLayout.html',
            controller: 'siLayoutCtrl'
        });

    configWelcomeState($stateProvider);
    configQuizzState($stateProvider);
    configDependentState($stateProvider);
    configNotificationsState($stateProvider);
    configAssignmentState($stateProvider);
    configQuizzPoints($stateProvider);
    configRelationshipNotifications($stateProvider);
    configSearchStates($stateProvider);
    configMessageStates($stateProvider);

    //$urlRouterProvider.when('/n', '/n/index');

    $locationProvider.html5Mode(false);
    $locationProvider.hashPrefix('!');

    configQuizzClassrooms($stateProvider);

    $stateProvider
        .state('si.loginRet', {
            url: '^/{acccess_token}',
            controller: 'loginRetCtrl'
        })
}

function initializeSCE($sceDelegateProvider: ng.ISCEDelegateProvider) {
    $sceDelegateProvider.resourceUrlWhitelist([
        "self",
        "http://www.youtube.com/**",
        "https://www.youtube.com/**",
        "http://www.facebook.com/**",
        "https://www.facebook.com/**"
    ]);
}

function configMobileStates($stateProvider: ng.ui.IStateProvider) {
    $stateProvider
        .state('mSignIn', {
            url: '^/mobile/SignIn',
            templateUrl: baseTemplateUrl + 'mobile/account/SigIn.html',
            controller: 'mSignInCtrl',
        })
        .state('mSignUpStandard', {
            url: '^/mobile/SignUpStandard',
            templateUrl: baseTemplateUrl + 'mobile/account/SignUpStandard.html',
            controller: 'registerUserCtrl',
        })
        .state('mSignUpDependent', {
            url: '^/mobile/SignUpDependent',
            templateUrl: baseTemplateUrl + 'mobile/account/SignUpDependent.html',
            controller: 'signUpCtrl',
        })
}

function intializeFastClick() {
    $(function () {
        FastClick.attach(document.body);
    });
}

//function initializeIonic($ionicConfigProvider: ionic.utility.IonicConfigProvider) {
//    $ionicConfigProvider.tabs.position("top");
//}

function configWelcomeState($stateProvider) {
    $stateProvider
        .state('si.welcomeStandard', {
            url: '^/welcome/standard',
            templateUrl: baseTemplateUrl + 'welcome/Standard.html',
        })
        .state('si.welcomeDependent', {
            url: '^/welcome/dependent',
            templateUrl: baseTemplateUrl + 'welcome/Dependent.html',
        });
}

function configAccountState($stateProvider: ng.ui.IStateProvider) {
    var $footer = $('#footer');
    $stateProvider
        .state('nsi.signIn', {
            url: '^/account/sign-in',
            templateUrl: baseTemplateUrl + 'account/SignIn.html',
            onEnter: function () {
                $footer.addClass('fixed-footer');
            },
            onExit: function () {
                $footer.removeClass('fixed-footer');
            }
        })
        .state('nsi.signUp', {
            url: '^/account/sign-up',
            templateUrl: baseTemplateUrl + 'account/SignUp.html',
            onEnter: function () {
                $footer.addClass('fixed-footer');
            },
            onExit: function () {
                $footer.removeClass('fixed-footer');
            }
        })
        .state('nsi.signUpStandard', {
            url: '^/account/sign-up-standard',
            templateUrl: baseTemplateUrl + 'account/SignUpStandard.html',
            controller: 'registerUserCtrl',
            onEnter: function () {
                $footer.addClass('fixed-footer');
            },
            onExit: function () {
                $footer.removeClass('fixed-footer');
            }
        })
        .state('si.signUpDependent', {
            url: '^/account/sign-up-dependent',
            templateUrl: baseTemplateUrl + 'account/SignUpDependent.html',
            controller: 'registerUserCtrl',
            onEnter: function () {
                $footer.addClass('fixed-footer');
            },
            onExit: function () {
                $footer.removeClass('fixed-footer');
            }
        })


    configAccountManagementState($stateProvider);
}

function configAccountManagementState($stateProvider: ng.ui.IStateProvider) {
    $stateProvider
        .state('nsi.resetPassword', {
            url: '^/account/reset-password?email&code',
            templateUrl: baseTemplateUrl + 'account/management/ResetPassword.html',
            controller: "resetPasswordCtrl"
        })
}

function configQuizzState($stateProvider: ng.ui.IStateProvider) {

    $stateProvider
        .state('si.editquizzCategories', {
            url: '^/quizz/editCategories',
            templateUrl: baseTemplateUrl + 'quizz/EditCategories.html',
            controller: 'quizzEditCategoriesCtrl',
        })
        .state('si.quizzCategories', {
            url: '^/quizz/categories',
            templateUrl: baseTemplateUrl + 'quizz/Categories.html',
            controller: 'quizzCategoriesCtrl',
        })
        .state('si.quizzCategory', {
            url: '^/quizz/category/{category}',
            templateUrl: baseTemplateUrl + 'quizz/Category.html',
            controller: 'quizzCategoryCtrl',
        })
        .state('si.gradeLevels', {
            url: '^/quizz/gradeLevels',
            templateUrl: baseTemplateUrl + 'quizz/GradeLevels.html',
            controller: 'gradeLevelsCtrl',
        })
        .state('si.gradeLevel', {
            url: '^/quizz/gradeLevel/{gradeLevel}',
            templateUrl: baseTemplateUrl + 'quizz/GradeLevel.html',
            controller: 'gradeLevelCtrl',
        })
        .state('si.quizzOverviews', {
            url: '^/quizz/overviews?category&pageNum&numPerPage&sortBy&sortType&levelMin&levelMax&userId&searchString',
            templateUrl: baseTemplateUrl + 'quizz/QuizzOverviews.html',
            controller: 'quizzOverviewsCtrl',
        })
        .state('si.editQuizz', {
            url: '^/quizz/edit-quizz/{quizzId}',
            templateUrl: baseTemplateUrl + 'quizz/EditQuizz.html',
            controller: 'editQuizzCtrl',
            onEnter: function () {
                $('html').scrollTop(0);
            }
        })
        .state('si.viewQuizz', {
            url: '^/quizz/view-quizz/{quizzId}?help',
            templateUrl: baseTemplateUrl + 'quizz/ViewQuizz.html',
            controller: 'viewQuizzCtrl',
        })
        .state('si.myQuizzes', {
            url: '^/quizz/myQuizzes/',
            templateUrl: baseTemplateUrl + 'quizz/MyQuizzes.html',
            controller: 'myQuizzesCtrl',
        })
        .state('si.quizzDetail', {
            url: '^/quizz/quizz-detail/{quizzId}?view&help&assId',
            templateUrl: baseTemplateUrl + 'quizz/detail/quizzDetail.html',
            controller: 'quizzDetailCtrl',
        })

    configReviewerState($stateProvider);
    configTestState($stateProvider);
    configbuiltInState($stateProvider);
    configQandAState($stateProvider);
    configMultipleChoiceState($stateProvider);
    configMultiChoiceSameState($stateProvider);
    configTakeTestState($stateProvider);
    configQuizzerState($stateProvider);
}

function configReviewerState($stateProvider: ng.ui.IStateProvider) {

    configQuickNoteState($stateProvider);
    configTextFlashCardsState($stateProvider);
    configViewReviewerState($stateProvider);
}

function configQuickNoteState($stateProvider: ng.ui.IStateProvider) {
    $stateProvider
        .state('si.viewQuickNotes', {
            url: '^/quizz/reviewer/quick-note/view-all/{revId}?quizzId&edit',
            templateUrl: baseTemplateUrl + 'quizz/reviewer/quickNote/ViewQuickNotes.html',
            controller: 'viewQuickNotesCtrl',
        })
        .state('si.editQuickNote', {
            url: '^/quizz/reviewer/quick-note/edit/{qnId}?revId&quizzId',
            templateUrl: baseTemplateUrl + 'quizz/reviewer/quickNote/EditQuickNote.html',
            controller: 'editQuickNoteCtrl',
        })
        .state('si.viewQuickNote', {
            abstract: true,
            url: '^/quizz/reviewer/quick-note/view/{quizzId}',
            templateUrl: baseTemplateUrl + 'quizz/reviewer/quickNote/ViewQuickNote.html',
            controller: 'viewQuickNoteCtrl',
        })
        .state('si.viewQuickNote.quickNoteItem', {
            url: '^/quizz/reviewer/quick-note/view/{quizzId}/note/{idx}?revId',
            templateUrl: baseTemplateUrl + 'quizz/reviewer/quickNote/ViewQuickNoteItem.html',
            controller: 'viewQuickNoteItemCtrl',
        })
}

function configTextFlashCardsState($stateProvider: ng.ui.IStateProvider) {
    $stateProvider
        .state('si.editTextFlashCards', {
            url: '^/quizz/reviewer/text-flash-card/view-all/{revId}?quizzId',
            templateUrl: baseTemplateUrl + 'quizz/reviewer/textFlashCard/EditTextFlashCards.html',
            controller: 'editTextFlashCardsCtrl',
        })
        .state('si.editTextFlashCard', {
            url: '^/quizz/reviewer/text-flash-card/edit/{tfcId}?revId&quizzId',
            templateUrl: baseTemplateUrl + 'quizz/reviewer/textFlashCard/EditTextFlashCard.html',
            controller: 'editTextFlashCardCtrl',
        })
}

function configViewReviewerState($stateProvider: ng.ui.IStateProvider) {
    $stateProvider
        .state('si.viewReviewer', {
            url: '^/quizz/reviewer/view?quizzId',
            templateUrl: baseTemplateUrl + 'quizz/reviewer/ViewReviewer.html',
            controller: 'viewReviewerCtrl',
        })
        .state('si.viewAsFlashCard', {
            abstract: true,
            url: '^/quizz/reviewer/viewAsFlashCard/{quizzId}',
            templateUrl: baseTemplateUrl + 'quizz/reviewer/textFlashCard/ViewAsFlashCard.html',
            controller: 'viewAsFlashCardCtrl',
        })
        .state('si.viewAsFlashCard.flashCardItem', {
            url: '^/quizz/reviewer/viewAsFlashCard/{quizzId}/FlashCard?fcId',
            templateUrl: baseTemplateUrl + 'quizz/reviewer/textFlashCard/ViewAsFlashCardItem.html',
            controller: 'viewAsFlashCardItemCtrl',
        })
        .state('si.viewAsFlashCard.viewMarkedflashCards', {
            url: '^/quizz/reviewer/viewAsFlashCard/{quizzId}/viewMarkedFlashCards',
            templateUrl: baseTemplateUrl + 'quizz/reviewer/textFlashCard/ViewMarkedFlashCards.html',
            controller: 'viewMarkedFlashCardsCtrl',
        })
        .state('si.viewReviewerItem', {
            abstract: true,
            url: '^/quizz/reviewer/view/{revId}?quizzId',
            templateUrl: baseTemplateUrl + 'quizz/reviewer/ViewReviewer.html',
            controller: 'viewReviewerItemCtrl',
        })
        .state('si.viewReviewerItem.quickNote', {
            url: '^/quizz/reviewer/view/{revId}/quick-note/{rId}?rItemId&quizzId',
            templateUrl: baseTemplateUrl + 'quizz/reviewer/quickNote/ViewQuickNoteItem.html',
            controller: 'viewReviewerQuickNoteCtrl',
        })
}

function configTestState($stateProvider: ng.ui.IStateProvider) {
    $stateProvider.state('si.editTest', {
        url: '^/quizz/test/edit/{testId}?quizzId',
        templateUrl: baseTemplateUrl + 'quizz/test/EditTest.html',
        controller: 'editTestCtrl',
    })
}

function configbuiltInState($stateProvider: ng.ui.IStateProvider) {
}

function configQandAState($stateProvider: ng.ui.IStateProvider) {
    $stateProvider.state('si.editQandA', {
        url: '^/quizz/q-and-a/edit/{qId}?testId&quizzId',
        templateUrl: baseTemplateUrl + 'quizz/qandA/EditQandA.html',
        controller: 'editQandACtrl',
    })
}

function configMultipleChoiceState($stateProvider: ng.ui.IStateProvider) {
    $stateProvider.state('si.editMultipleChoice', {
        url: '^/quizz/multi-choice/edit/{qId}?testId&quizzId',
        templateUrl: baseTemplateUrl + 'quizz/multipleChoice/EditMultipleChoice.html',
        controller: 'editMultipleChoiceCtrl',
    })
}

function configMultiChoiceSameState($stateProvider: ng.ui.IStateProvider) {
    $stateProvider
        .state('si.editMultiChoiceSameChoiceGroup', {
            url: '^/quizz/multi-choice-same-choice-group/edit/{cgId}?testId&quizzId',
            templateUrl: baseTemplateUrl + 'quizz/types/multiChoiceSame/EditMultiChoiceSameChoiceGroup.html',
            controller: 'editMultiChoiceSameChoiceGroupCtrl',
        })
        .state('si.editMultiChoiceSameQuestion', {
            url: '^/quizz/multi-choice-same-question/edit/{qId}?cgId&testId&quizzId',
            templateUrl: baseTemplateUrl + 'quizz/types/multiChoiceSame/EditMultiChoiceSameQuestion.html',
            controller: 'editMultiChoiceSameQuestionCtrl',
        })
}

function configTakeTestState($stateProvider: ng.ui.IStateProvider) {
    $stateProvider
        .state('si.prepTest', {
            url: '^/quizz/prepTest/{testId}?quizzId&isPrev&assId',
            templateUrl: baseTemplateUrl + 'quizz/takeTest/PrepTest.html',
            controller: 'prepTestCtrl',
        })
        .state('si.takeTest', {
            abstract: true,
            url: '^/quizz/takeTest/{testId}?quizzId',
            templateUrl: baseTemplateUrl + 'quizz/takeTest/TakeTest.html',
            controller: 'takeTestCtrl',
        })
        .state('si.takeTest.QandA', {
            url: '^/quizz/takeTest/{testId}/q-a-question/{qId}?quizzId',
            templateUrl: baseTemplateUrl + 'quizz/qandA/TakeTestQandA.html',
            controller: 'takeTestQandACtrl',
        })
        .state('si.takeTest.MultiChoice', {
            url: '^/quizz/takeTest/{testId}/m-c-question/{qId}?quizzId',
            templateUrl: baseTemplateUrl + 'quizz/multipleChoice/TakeTestMultiChoice.html',
            controller: 'takeTestMultiChoiceCtrl',
        })
        .state('si.takeTest.MultiChoiceSame', {
            url: '^/quizz/takeTest/{testId}/m-c-s-question/{qId}?quizzId',
            templateUrl: baseTemplateUrl + 'quizz/multipleChoice/TakeTestMultiChoice.html',
            controller: 'takeTestMultiChoiceSameCtrl',
        })
        .state('si.takeTest.Finish', {
            url: '^/quizz/takeTest/{testId}/finish',
            templateUrl: baseTemplateUrl + 'quizz/takeTest/FinishTest.html',
            controller: 'finishTestCtrl',
        })
        .state('si.viewResult', {
            url: '^/quizz/viewResult/{testLogId}?quizzId&testId',
            templateUrl: baseTemplateUrl + 'quizz/takeTest/viewResult.html',
            controller: 'viewResultCtrl',
        })
        .state('si.testResults', {
            url: '^/quizz/testResults',
            templateUrl: baseTemplateUrl + 'quizz/test/TestResults.html',
            controller: 'testResultsCtrl',
        })
        .state('si.loadTest', {
            url: '^/quizz/loadTest/{testSnapId}',
            controller: 'loadTestCtrl',
        })
}

function configDependentState($stateProvider: ng.ui.IStateProvider) {
    $stateProvider.state('si.depActivities', {
        url: '^/dependent/activities?depId&view',
        templateUrl: baseTemplateUrl + 'dependent/DepActivities.html',
        controller: 'depActivitiesCtrl',
    })
}

function configNotificationsState($stateProvider: ng.ui.IStateProvider) {
    $stateProvider.state('si.allNotifications', {
        url: '^/notifications/all',
        templateUrl: baseTemplateUrl + 'notifications/AllNotifications.html',
        controller: 'allNotificationsCtrl',
    })
}

function configQuizzerState($stateProvider: ng.ui.IStateProvider) {
    $stateProvider.state("si.quizzer", {
        url: '^/quizzer/view?quizzerId&view',
        templateUrl: baseTemplateUrl + 'quizzer/ViewQuizzer.html',
        controller: 'viewQuizzerCtrl',
    });
}

function configAssignmentState($stateProvider: ng.ui.IStateProvider) {
    $stateProvider
        .state("si.myAssignments", {
            url: '^/my-assignments?assId',    // assId = -1 - for all assigned
            templateUrl: baseTemplateUrl + 'assignment/MyAssignments.html',
            controller: 'myAssignmentsCtrl',
        })
        .state("si.givenAssignmentGroups", {
            url: '^/assignment-groups-given?assGId',
            templateUrl: baseTemplateUrl + 'assignment/GivenAssignmentGroups.html',
            controller: 'givenAssignmentGroupsCtrl',
        })
        .state("si.editAssignmentGroup", {
            url: '^/assignment-group/edit?assGId&quizzId',
            templateUrl: baseTemplateUrl + 'assignment/EditAssignmentGroup.html',
            controller: 'editAssignmentGroupCtrl',
        })
}

function configQuizzPoints($stateProvider: ng.ui.IStateProvider) {
    $stateProvider
        .state("si.dailyRewards", {
            url: '^/daily-rewards',    // assId = -1 - for all assigned
            templateUrl: baseTemplateUrl + 'QuizzPoints/DailyRewards.html',
            controller: 'dailyRewardsCtrl',
        })
}

function configRelationshipNotifications($stateProvider: ng.ui.IStateProvider) {
    $stateProvider
        .state("si.quizzmates", {
            url: '^/quizzmates',
            templateUrl: baseTemplateUrl + 'relationships/Quizzmates.html',
            controller: 'quizzmatesCtrl',
        })
        .state("si.relationshipNotifications", {
            url: '^/relationship-notifications',    // assId = -1 - for all assigned
            templateUrl: baseTemplateUrl + 'relationships/RelationshipNotifications.html',
            controller: 'relationshipNotificationsCtrl',
        })
}

function configSearchStates($stateProvider: ng.ui.IStateProvider) {
    $stateProvider
        .state("si.searchAll", {
            url: '^/search-all?search',
            templateUrl: baseTemplateUrl + 'search/SearchAll.html',
            controller: 'searchAllCtrl',
        })
}

function configMessageStates($stateProvider: ng.ui.IStateProvider) {
    $stateProvider
        .state("si.quizzmateMsg", {
            url: '^/quizzmateMsg?threadId&userId&depId',
            templateUrl: baseTemplateUrl + 'relationships/messages/QuizzmateMsg.html',
            controller: 'quizzmateMsgCtrl',
        })
        .state("si.allMessage", {
            url: '^/allMessage',
            templateUrl: baseTemplateUrl + 'relationships/messages/AllMessage.html',
            controller: 'allMessageCtrl',
        })
}

function configQuizzClassrooms($stateProvider: ng.ui.IStateProvider) {
    $stateProvider
        .state("si.quizzClassAll", {
            url: '^/QuizzClassAll',
            templateUrl: baseTemplateUrl + 'Classroom/QuizzClassAll.html',
            controller: 'quizzClassAllCtrl',
        })
        .state("si.myQuizzClass", {
            url: '^/MyQuizzClass?qcId&depId&view',
            templateUrl: baseTemplateUrl + 'Classroom/MyQuizzClass.html',
            controller: 'myQuizzClassCtrl',
        })
        .state("si.enrolledQuizzClass", {
            url: '^/EnrolledQuizzClass?qcId&depId&depName&view',
            templateUrl: baseTemplateUrl + 'Classroom/EnrolledQuizzClass.html',
            controller: 'enrolledQuizzClassCtrl',
        })
        .state("si.quizzClass", {
            url: '^/QuizzClass?qcId&view',
            templateUrl: baseTemplateUrl + 'Classroom/QuizzClass.html',
            controller: 'quizzClassCtrl',
        })
        .state("si.quizzClassLesson", {
            url: '^/QuizzClassLesson?qclId&depId&depName&view',
            templateUrl: baseTemplateUrl + 'Classroom/QuizzClassLesson.html',
            controller: 'quizzClassLessonCtrl',
        })
        .state("si.quizzClassMaterial", {
            url: '^/QuizzClassMaterial?qcmId',
            templateUrl: baseTemplateUrl + 'Classroom/QuizzClassMaterial.html',
            controller: 'quizzClassMaterialCtrl',
        })
}