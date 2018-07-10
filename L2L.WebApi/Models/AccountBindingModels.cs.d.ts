declare module server {
	interface AddExternalLoginBindingModel {
		externalAccessToken: string;
	}
	interface ChangePasswordBindingModel {
		oldPassword: string;
		newPassword: string;
		confirmPassword: string;
	}
	interface RegisterBindingModel {
		email: string;
		password: string;
		confirmPassword: string;
	}
	interface RegisterExternalBindingModel extends RegisterUserModelBase {
		email: string;
	}
	interface RemoveLoginBindingModel {
		loginProvider: string;
		providerKey: string;
	}
	interface SetPasswordBindingModel {
		newPassword: string;
		confirmPassword: string;
	}
	interface ResetPasswordModel {
		userName: string;
		password: string;
		confirmPassword: string;
		code: string;
	}
	interface ForgotPasswordModel {
		emailOrUserName: string;
	}
	interface RegisterUserModelBase {
		userName: string;
		firstName: string;
		lastName: string;
		password: string;
		confirmPassword: string;
		birthDate: Date;
		country: string;
	}
	interface RegisterDependentModel extends RegisterUserModelBase {
	}
	interface RegisterStandardModel extends RegisterUserModelBase {
		email: string;
	}
}
