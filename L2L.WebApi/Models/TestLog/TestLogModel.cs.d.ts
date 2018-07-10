declare module server {
	interface TestLogModel {
		id: number;
		score: number;
		total: number;
		duration: number;
		dateTaken: Date;
		comment: string;
		resultBlob: string;
		quizzName: string;
		category: number;
		userId: number;
		testSettingId: number;
		quizzId: number;
		assignmentId: number;
		testSetting: {
			id: number;
			isOrdered: boolean;
			numberOfQuestions: number;
			timedTypeEnum: any;
			secondsPerQuestion: number;
			secondsForWholeQuiz: number;
			instantFeedback: boolean;
		};
		isQuizzmate: boolean;
		isAuthor: boolean;
		authorUserName: string;
		authorFullName: string;
		isPassed: boolean;
		authorName: string;
	}
	interface TestLogGroup {
		quizzSummary: server.QuizzSummary;
		testLogs: server.TestLogSummary[];
	}
	interface QuizzSummary {
		id: number;
		title: string;
		description: string;
		scoreSum: number;
		totalSum: number;
		category: number;
	}
	interface TestLogSummary {
		id: number;
		score: number;
		total: number;
		dateTaken: Date;
	}
}
