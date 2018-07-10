declare module server {
	interface QuizzCategory {
		id: number;
		title: string;
		description: string;
		categoryValue: number;
		iconStrValue: string;
		iconColor: string;
		borderColor: string;
		textColor: string;
		isIncludedInDailyReward: boolean;
	}
}
