declare module server {
	interface FriendRequestModel {
		id: number;
		message: string;
		isNew: boolean;
		isAccepted: boolean;
		requestToId: number;
		requestFromId: number;
	}
	interface FriendRelationshipModel {
		id: number;
		user1Id: number;
		user2Id: number;
	}
}
