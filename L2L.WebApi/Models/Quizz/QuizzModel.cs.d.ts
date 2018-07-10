/// <reference path="../QuizzPoints/QuizzPointsModel.cs.d.ts" />

declare module server {
	interface QuizzModel {
		id: number;
		title: string;
		description: string;
		isLive: boolean;
		visibility: any;
		modifyPermission: number;
		isBuiltIn: boolean;
		builtInType: any;
		mainType: number;
		subType: number;
		subType2: number;
		subType3: number;
		subType4: number;
		subType5: number;
		quizzTags: string;
		ownerId: number;
		reviewerId: number;
		testId: number;
		defaultTestSetting: {
			id: number;
			isOrdered: boolean;
			numberOfQuestions: number;
			timedTypeEnum: any;
			secondsPerQuestion: number;
			secondsForWholeQuiz: number;
			instantFeedback: boolean;
		};
		category: number;
		difficulty: any;
		gradeLevelMin: any;
		gradeLevelMax: any;
		dailyReward: server.DailyRewardModel;
	}
}
