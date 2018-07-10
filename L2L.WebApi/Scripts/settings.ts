interface IAppState {
    isMobileSize: boolean;
    isMobileApp: boolean;

    hasHeaderTitle: boolean;
    headerTitle: string;
    hasBackButton: boolean;
    hasTabs: boolean;
    hasSearch: boolean;
};

var setting = (function () {

    function isMobileApp(): boolean {
        return false;
    }

    function serverUrl(): string {
        return "";
    }

    var appState: IAppState = {
        isMobileSize: false,
        isMobileApp: false,
        headerTitle: "",
        hasHeaderTitle: false,
        hasBackButton: false,
        hasTabs: true,
        hasSearch: false
    };

    function init() {
        $(window).on('resize', function () {
            var isMobileSizeTmp: boolean = Modernizr.mq("(max-width: 749px)");
            if (isMobileSizeTmp != appState.isMobileSize) {
                appState.isMobileSize = isMobileSizeTmp;
            }
        });

        appState.isMobileSize = Modernizr.mq("(max-width: 749px)");
    }

    function resetMobileAppState() {
        appState.headerTitle = "";
        appState.hasHeaderTitle = false;
        appState.hasBackButton = false;
        appState.hasTabs = true;
    }

    init();

    return {
        isMobileApp: isMobileApp,
        serverUrl: serverUrl,
        appState: appState,
        resetMobileAppState: resetMobileAppState
    }
})();