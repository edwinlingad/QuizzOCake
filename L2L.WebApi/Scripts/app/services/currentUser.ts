/// <reference path="../l2lapp.ts" />
interface ICurrentUser {
    isLoggedIn: () => boolean;
    getUserData: () => IUserModel;
    //getUserDataWithCb(success: Function): void;
    setUserData: (data: IUserModel) => void;
    getAccessToken: () => string;
    setAcccessToken: (token: string) => void;
    logOff: () => void;
    accessToken: string;
    isGuest: () => boolean;
    getUpdatedUserData(): void;

    init(success: Function, error: Function): void;
}

(function () {
    l2lApp.service("currentUser", currentUser);

    currentUser.$inject = ["$cookies", "$state", "$resource", "topPanelContentSvc"];
    function currentUser(
        $cookies: ng.cookies.ICookiesService,
        $state: ng.ui.IStateService,
        $resource,
        topPanelContentSvc: ITopPanelContentSvc
    ): ICurrentUser {
        var loggedIn: boolean = false;
        var tokenKey: string = "quizzinatorToken";

        function isLoggedIn(): boolean {
            if (loggedIn == false) {
                accessToken = $cookies.get(tokenKey);
                loggedIn = !(accessToken === "" || accessToken === undefined);
            }
            return loggedIn;
        }

        var userData: IUserModel = {
            isReady: false,
            id: 0,
            isGuest: true,
            isAdmin: false,
            profile: {
            }
        };
        var clientToday: string = "";

        function getUserData(): IUserModel {
            var clientTodayTmp = util.getClientToday();

            if (clientTodayTmp !== clientToday)
                getUpdatedUserData();

            return userData;
        }

        var isGuestBool: boolean = false;
        function setUserData(data: IUserModel): void {

            userData.id = data.id;
            userData.userType = data.userType;
            userData.userName = data.userName;
            userData.profile.firstName = data.profile.firstName;
            userData.profile.lastName = data.profile.lastName;
            userData.profile.birthDate = new Date(data.profile.birthDate.toString());
            userData.profile.profileImageUrl = setting.serverUrl() + "/" + data.profile.profileImageUrl;
            userData.isAdmin = data.isAdmin;
            userData.asUserDependents = data.asUserDependents;

            userData.points = data.points;
            userData.dailyPoints = data.dailyPoints;
            userData.dailyPointsDate = data.dailyPointsDate;

            userData.totalDailyRewardItems = data.totalDailyRewardItems;

            userData.dailyNormalPointsQuizzSelfIntList = data.dailyNormalPointsQuizzSelfIntList;
            userData.dailyNormalPointsQuizzOthersIntList = data.dailyNormalPointsQuizzOthersIntList;
            userData.dailySpecialPointsQuizzIntList = data.dailySpecialPointsQuizzIntList;
            userData.dailyPointsAllIntList = data.dailyPointsAllIntList;

            isGuestBool = userData.id == 0 || userData.userName == "guest" || userData.userName == "quizzling1" || userData.userName == "quizzling2";
            userData.isGuest = isGuestBool;
            userData.isReady = true;
            userData.isEmailConfirmed = data.isEmailConfirmed;
            userData.isThirdPartyLogin = data.isThirdPartyLogin;
        }

        function logOff(): void {
            loggedIn = false;
            accessToken = "";
            userData.isReady = false;
            $cookies.remove(tokenKey);
            topPanelContentSvc.reset();
        }

        var accessToken: string = "";
        function getAccessToken(): string {
            return accessToken;
        }

        function setAccessToken(token: string): void {
            loggedIn = true;
            accessToken = token;
            var expire: Date = new Date();
            expire.setDate(expire.getDate() + 14);
            $cookies.put(tokenKey, token, { expires: expire });
        }

        function isGuest(): boolean {
            return isGuestBool;
        }

        var isInited: boolean = false;
        function init(success: Function, error: Function) {
            function goToStartPage() {
                error();
                $state.go(util.getDefaultLocation());
            }

            function errorLogin() {
                goToStartPage();
            }

            if (accessToken === "" || accessToken === undefined) {
                accessToken = $cookies.get(tokenKey);
            }

            if (userData.isReady == false) {
                getUserDataFromUser(accessToken,
                    function (data: IUserModel) {
                        setUserData(data);
                        success();
                    }, errorLogin);
            } else {
                success();
            }
        }

        function getUpdatedUserData() {
            getUserDataFromUser(accessToken,
                function (data: IUserModel) {
                    setUserData(data);
                },
                function () {
                });
        }

        function getUserDataFromUser(token: string, success: Function, error: Function) {
            var getUserResource = $resource(setting.serverUrl() + '/api/Account/GetUser', null,
                {
                    'get': { headers: { 'Authorization': 'Bearer ' + token } }
                });

            clientToday = util.getClientToday();

            getUserResource.get({ clientToday: clientToday }, success, error);
        }

        return {
            isLoggedIn: isLoggedIn,
            getUserData: getUserData,
            setUserData: setUserData,
            logOff: logOff,
            getAccessToken: getAccessToken,
            setAcccessToken: setAccessToken,
            accessToken: accessToken,
            isGuest: isGuest,
            getUpdatedUserData: getUpdatedUserData,
            init: init
        }
    }
})();
