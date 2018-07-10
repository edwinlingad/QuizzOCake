interface IFacebookSvc {
    login(success: Function, error: Function);
    logout(success: Function, error: Function);

    share(message: string, meta?: IOGMeta): void;
}

interface IOGMeta {
    type: number;   // 0 - quizz
    title?: string;
    description?: string;
    url: string;
}

(function () {
    l2lApp.service("facebookSvc", facebookSvc);

    facebookSvc.$inject = ["currentUser", "$location"];

    function facebookSvc(
        currentUser: ICurrentUser,
        $location: ng.ILocationService
    ): IFacebookSvc {

        function statusChangeCallback(response) {
            //console.log('statusChangeCallback');
            //console.log(response);

            if (response.status === 'connected') {
                FB.api('/me?fields=first_name,last_name,email,age_range,picture,location', function (response) {
                    console.log(JSON.stringify(response));
                });
            } else if (response.status === 'not_authorized') {
                // The person is logged into Facebook, but not your app.

            } else {
                // The person is not logged into Facebook, so we're not sure if
                // they are logged into this app or not.
            }
        }

        function checkLoginState() {
            FB.getLoginStatus(function (response) {
                statusChangeCallback(response);
            });
        }

        window.fbAsyncInit = function () {
            FB.init({
                appId: '567536553426080',
                cookie: true,  // enable cookies to allow the server to access 
                // the session
                xfbml: true,  // parse social plugins on this page
                version: 'v2.5' // use graph api version 2.5
            });

            FB.getLoginStatus(function (response) {
                statusChangeCallback(response);
            });

        };

        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/en_US/sdk.js";
            fjs.parentNode.insertBefore(js, fjs);
        } (document, 'script', 'facebook-jssdk'));

        function fbLogin(success: Function, error: Function) {
            

            FB.login(function (response) {
                function successLogin(data) {
                    var model: IOauthUserInfo = {
                        firstName: data.first_name,
                        lastName: data.last_name,
                        email: data.email,
                        profilePixUrl: data.picture.data.url
                    };

                    if (success !== undefined) {
                        success(model);
                    }
                }

                // handle the response
                //statusChangeCallback(response);

                console.log(JSON.stringify(response));

                if (response.status === 'connected') {
                    FB.api('/me?fields=first_name,last_name,email,age_range,picture,location',
                        function (response) {
                            //console.log(JSON.stringify(response));

                            successLogin(response);
                        });
                } else if (response.status === 'not_authorized') {
                    // The person is logged into Facebook, but not your app.

                } else {
                    // The person is not logged into Facebook, so we're not sure if
                    // they are logged into this app or not.
                }

            }, { scope: 'public_profile,email,user_location' });
        }

        function fbLogout(success: Function, error: Function) {
            FB.logout(function (response) {
                // Person is now logged out
            });
        }

        function share(message: string, meta?: IOGMeta) {
            var url = $location.absUrl();
            if (meta !== undefined)
                url = meta.url;
            FB.ui({
                method: 'share',
                href: url,
                quote: message
            }, function (response) {
            });
        }

        return {
            login: fbLogin,
            logout: fbLogout,
            share: share
        }
    }
})();