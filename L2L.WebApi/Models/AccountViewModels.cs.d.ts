declare module server {
	interface ExternalLoginViewModel {
		name: string;
		url: string;
		state: string;
	}
	interface ManageInfoViewModel {
		localLoginProvider: string;
		email: string;
		logins: server.UserLoginInfoViewModel[];
		externalLoginProviders: server.ExternalLoginViewModel[];
	}
	interface UserInfoViewModel {
		email: string;
		hasRegistered: boolean;
		loginProvider: string;
		isEmailUsed: boolean;
	}
	interface UserLoginInfoViewModel {
		loginProvider: string;
		providerKey: string;
	}
}
