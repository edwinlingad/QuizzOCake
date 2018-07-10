/// <reference path="../../models/accounts/accountbindingmodels.ts" />
interface IAccount {
    registerStandardUser: (user: IRegisterStandardModel, success?: Function, error?: Function) => IUserModel;
    loginUser: (user: ILogin, success?: Function, error?: Function) => void;
    loginEmail: (user: ILogin, success?: Function, error?: Function) => void;
    registerDependentUser: (user: IRegisterDependentModel, success?: Function, error?: Function) => IUserModel;
    getUser: (success?: Function, error?: Function) => void;
    logOut: (success?: Function, error?: Function) => void;
    forgotPassword: (model: IForgotPasswordModel, success?: Function, error?: Function) => void;
    resetPassword: (model: IResetPasswordModel, success?: Function, error?: Function) => void;
    changePassword: (model: IChangePasswordBindingModel, success?: Function, error?: Function) => void;
    confirmEmail(userId: string, code: string, success?: Function, error?: Function): void;
    isUserNameAvailable(userName: string, success?: Function, error?: Function): void;
    changeUserName(model: IQuizzerModel, success?: Function, error?: Function): void;

    userInfo(success?: Function, error?: Function): void;
    registerExternal(model: IRegisterExternalBindingModel,success?: Function, error?: Function): void;
}

module accountSvcResource {
    export interface IResource {
        userData: any;
        registerStandard: any;
        login: any;
        registerDependent: any;
        logout: any;
        forgotPassword: any;
        resetPassword: any;
        changePassword: any;
        confirmEmail: any;
        isUserNameAvailable: any;
        changeUserName: any;
        userInfo: any;
        registerExternal: any;
    }
}

(function () {
    l2lApp.factory('accountSvc', accountSvc);

    resource.$inject = ["$resource", "currentUser"];

    function resource($resource, currentUser: ICurrentUser): accountSvcResource.IResource {
        var getUserResource = $resource(setting.serverUrl() + '/api/Account/GetUser', null,
            {
                'get': { headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });
        var registerStandardResource = $resource(setting.serverUrl() + '/Api/Account/RegisterStandard', null,
            {
                'registerStandardUser': { method: 'POST' }
            });
        var registerDependentResource = $resource(setting.serverUrl() + '/Api/Account/RegisterDependent', null,
            {
                'registerDependentUser': { method: 'POST', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });
        var loginResource = $resource(setting.serverUrl() + "/Token", null,
            {
                'loginUser': {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                    transformRequest: function (data, headersGetter) {
                        var str = [];
                        for (var d in data)
                            str.push(encodeURIComponent(d) + "=" +
                                encodeURIComponent(data[d]));
                        return str.join("&");
                    }
                }
            });
        var logOutResource = $resource(setting.serverUrl() + '/api/Account/Logout', null,
            {
                'post': { method: "POST", headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });
        var forgotPasswordResource = $resource(setting.serverUrl() + '/api/Account/ForgotPassword');
        var resetPasswordResource = $resource(setting.serverUrl() + '/api/Account/ResetPassword');
        var changePasswordResource = $resource(setting.serverUrl() + '/api/Account/ChangePassword', null,
            {
                'post': { method: "POST", headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });
        var confirmEmailResource = $resource(setting.serverUrl() + '/api/Account/ConfirmEmail');
        var isUserNameAvailableResource = $resource(setting.serverUrl() + '/api/Account/IsUserNameAvailable', null,
            {
                'get': { method: "GET", headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });
        var changeUserNameResource = $resource(setting.serverUrl() + '/api/Account/ChangeUserName', null,
            {
                'post': { method: "POST", headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });
        var userInfoResource = $resource(setting.serverUrl() + '/api/Account/UserInfo', null,
            {
                'get': { method: "GET", headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });
        var registerExternalResource = $resource(setting.serverUrl() + '/api/Account/RegisterExternal', null,
            {
                'post': { method: "POST", headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        return {
            userData: getUserResource,
            registerStandard: registerStandardResource,
            registerDependent: registerDependentResource,
            login: loginResource,
            logout: logOutResource,
            forgotPassword: forgotPasswordResource,
            resetPassword: resetPasswordResource,
            changePassword: changePasswordResource,
            confirmEmail: confirmEmailResource,
            isUserNameAvailable: isUserNameAvailableResource,
            changeUserName: changeUserNameResource,
            userInfo: userInfoResource,
            registerExternal: registerExternalResource
        }
    }

    accountSvc.$inject = ["$resource", "currentUser"];

    function accountSvc($resource, currentUser: ICurrentUser): IAccount {

        function registerStandardUser(user: IRegisterStandardModel, success?: Function, error?: Function): IUserModel {
            return <IUserModel>resource($resource, currentUser).registerStandard.registerStandardUser(user, success, error);
        }

        function registerDependentUser(user: IRegisterDependentModel, success?: Function, error?: Function): IUserModel {
            return <IUserModel>resource($resource, currentUser).registerDependent.registerDependentUser(user, success, error);
        }

        function loginUser(user: ILogin, success?: Function, error?: Function) {
            resource($resource, currentUser).login.loginUser(user, success, error);
        }

        function loginEmail(user: ILogin, success?: Function, error?: Function) {
            var login: ILogin = {
                email: user.userName,
                password: user.password,
                grant_type: user.grant_type
            }
            resource($resource, currentUser).login.loginUser(login, success, error);
        }

        function getUser(success?: Function, error?: Function) {
            var clientToday: string = util.getClientToday();

            resource($resource, currentUser).userData.get({ clientToday: clientToday }, success, error);
        }

        function logOut(success?: Function, error?: Function) {
            resource($resource, currentUser).logout.post(null, success, error);
        }

        function forgotPassword(model: IForgotPasswordModel, success?: Function, error?: Function) {
            resource($resource, currentUser).forgotPassword.save(model, success, error);
        }

        function resetPassword(model: IResetPasswordModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).resetPassword.save(model, success, error);
        }

        function changePassword(model: IChangePasswordBindingModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).changePassword.post(model, success, error);
        }

        function confirmEmail(userId: string, code: string, success?: Function, error?: Function): void {
            resource($resource, currentUser).confirmEmail.get({ userId: userId, code: code }, success, error);
        }

        function isUserNameAvailable(userName: string, success?: Function, error?: Function): void {
            resource($resource, currentUser).isUserNameAvailable.get({ userName: userName }, success, error);
        }

        function changeUserName(model: IQuizzerModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).changeUserName.post(model, success, error);
        }

        function userInfo(success?: Function, error?: Function): void {
            resource($resource, currentUser).userInfo.get(null, success, error);
        }

        function registerExternal(model: IRegisterExternalBindingModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).registerExternal.post(model, success, error);
        }

        return {
            registerStandardUser: registerStandardUser,
            registerDependentUser: registerDependentUser,
            loginUser: loginUser,
            loginEmail: loginEmail,
            getUser: getUser,
            logOut: logOut,
            forgotPassword: forgotPassword,
            resetPassword: resetPassword,
            changePassword: changePassword,
            confirmEmail: confirmEmail,
            isUserNameAvailable: isUserNameAvailable,
            changeUserName: changeUserName,
            userInfo: userInfo,
            registerExternal: registerExternal
        }
    }
})();