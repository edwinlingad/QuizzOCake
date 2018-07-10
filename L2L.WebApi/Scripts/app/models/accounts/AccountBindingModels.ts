interface IRegisterUserModelBase {
    userName?: string;
    firstName?: string;
    lastName?: string;
    password?: string;
    confirmPassword?: string;
    birthDate?: Date;
    country?: string;
} 

interface IRegisterDependentModel extends IRegisterUserModelBase {
}

interface IRegisterStandardModel extends IRegisterUserModelBase {
    email?: string;
}

interface IRegisterExternalBindingModel extends IRegisterUserModelBase {
    email?: string;
}

interface ILogin {
    userName?: string;
    email?: string;
    password?: string;
    grant_type?: string;
}

interface IProfile {
    isLoggedIn: boolean;
    username?: string;
    token?: string;
    userType: UserTypeEnum;
}

interface IForgotPasswordModel {
    emailOrUserName: string;
}

interface IResetPasswordModel {
    userName: string;
    password?: string;
    confirmPassword?: string;
    code: string;
}

interface IChangePasswordBindingModel {
    oldPassword?: string;
    newPassword?: string;
    confirmPassword?: string;
}

interface IExternalLoginViewModel {
    name: string;
    url: string;
    state: string;
}
interface IManageInfoViewModel {
    localLoginProvider: string;
    email: string;
    logins: server.UserLoginInfoViewModel[];
    externalLoginProviders: server.ExternalLoginViewModel[];
}
interface IUserInfoViewModel {
    email: string;
    hasRegistered: boolean;
    loginProvider: string;
    isEmailUsed: boolean;
}
interface IUserLoginInfoViewModel {
    loginProvider: string;
    providerKey: string;
}