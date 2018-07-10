declare module server {
	const enum QuizzMessageTypeEnum {
		Quizzmate,
	}
	interface QuizzmateMsgThreadModel {
		id: number;
		isGroupMsg: boolean;
		groupMessageName: string;
		signalRGroupName: string;
		isDeleted: boolean;
		msgThreadMembers: server.QuizzmateMsgThreadMemberModel[];
		messages: server.QuizzmateMsgModel[];
		isViewerOnly: boolean;
	}
	interface QuizzmateMsgThreadMemberModel extends WithAgeModel {
		id: number;
		hasNew: boolean;
		newCount: number;
		quizzmateMsgThreadId: number;
		userId: number;
		userName: string;
		userFullName: string;
		profileImageUrl: string;
	}
	interface ToggleModel {
		id: number;
		value: boolean;
	}
	interface WithAgeModel {
		age: number;
		birthDate: Date;
	}
	interface QuizzmateMsgModel {
		id: number;
		message: string;
		postedDate: Date;
		authorId: number;
		quizzmateMsgThreadId: number;
		isNew: boolean;
		userName: string;
		profileImageUrl: string;
	}
	interface QuizzConnectMsgThread {
		isEmpty: boolean;
		quizzMessageType: server.QuizzMessageTypeEnum;
		id: number;
		newCount: number;
		groupName: string;
		quizzmateMsgThreadId: number;
		lastSenderUserName: string;
		lastSenderFullName: string;
		lastSenderProfileUrl: string;
		lastSenderMsg: string;
		lastMsgPostedDate: Date;
		lastSenderName: string;
	}
}
