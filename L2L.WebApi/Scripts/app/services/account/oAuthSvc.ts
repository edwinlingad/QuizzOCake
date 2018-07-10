interface IOAuthSvc {
    fbLogin(success: Function, error: Function): void;
    googleLogin(success: Function, error: Function): void;
}

(function () {
    l2lApp.service("oAuthSvc", oAuthSvc);

    oAuthSvc.$inject = ["$resource", "currentUser", "$location", "$window"];
    function oAuthSvc(
        $resource,
        currentUser: ICurrentUser,
        $location: ng.ILocationService,
        $window: ng.IWindowService
    ): IOAuthSvc {

        function externalLogin(company: string, success: Function, error: Function) {

            function CallExternalLogin(url: string) {

                $window.location.href = url;
            }

            function getExternalLogin() {
                //$resource('/api/Account/ExternalLogins?returnUrl=loginRet&generateState=true').query(null,
                $resource('/api/Account/ExternalLogins?returnUrl=%2F&generateState=true').query(null,
                    function (data) {
                        var url: string = undefined;
                        data.forEach(function (item) {
                            if (item.name === company)
                                url = item.url;
                        });

                        if (url !== undefined)
                            CallExternalLogin(url);
                    },
                    function () {
                        console.log("error!");
                    });
            }

            getExternalLogin();
        }

        function fbLogin(success: Function, error: Function): void {
            externalLogin("Facebook", success, error);
        }

        function googleLogin(success: Function, error: Function): void {
            externalLogin("Google", success, error);
        }

        return {
            fbLogin: fbLogin,
            googleLogin: googleLogin
        }
    }
})();