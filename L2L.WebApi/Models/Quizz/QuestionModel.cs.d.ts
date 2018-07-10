declare module server {
	interface QuestionModel {
		id: number;
		questionId: number;
		questionType: any;
		order: number;
		isFlashCard: boolean;
		testId: number;
		question: string;
		questionViewType: any;
	}
}
