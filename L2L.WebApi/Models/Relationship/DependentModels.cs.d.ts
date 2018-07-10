declare module server {
	interface DependentRequestFromUserModel {
		id: number;
		toChildId: number;
		fromUserId: number;
		isAccepted: boolean;
	}
}
