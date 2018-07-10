declare module server {
	interface DailyRewardModel {
		dailyRewardId: number;
		dailyRewardPerUserId: number;
		isTaken: boolean;
		points: number;
		availablePoints: number;
	}
}
