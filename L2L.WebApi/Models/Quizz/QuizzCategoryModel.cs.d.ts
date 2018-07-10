declare module server {
	interface QuizzCategoryModel {
		id: number;
		quizzCategoryType: number;
		title: string;
		description: string;
		iconStrValue: string;
		iconColor: string;
		borderColor: string;
		textColor: string;
		isIncludedInDailyReward: boolean;
		quizzCount: number;
	}
}
