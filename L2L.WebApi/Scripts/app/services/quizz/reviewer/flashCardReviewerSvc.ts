interface IFlashCardReviewerSvc {
    init: (id: number, reviewers: IReviewerFromQuestionModel[]) => void;
    start: (randomize: boolean) => void;
    counter: ICounter;

    getCurrentReviewer: () => IReviewerFromQuestionModel;
    getReviewerAtIdx: (id: number) => IReviewerFromQuestionModel;

    canGoToPrevReviewer: () => boolean;
    goToPrevReviewer: () => void;

    canGoToNextReviewer: () => boolean;
    goToNextReviewer: () => void;

    goToReviewerIdx: (idx: number) => void;

    toggleMark: () => void;
    getMarkedReviewers: () => IReviewerFromQuestionModel[]
}

(function () {
    l2lApp.service("flashCardReviewerSvc", flashCardReviewerSvc);

    flashCardReviewerSvc.$inject = ["$state"];
    function flashCardReviewerSvc(
        $state: ng.ui.IStateService
        ): IFlashCardReviewerSvc {

        var origReviewers: IReviewerFromQuestionModel[];
        var reviewers = new Array<IReviewerFromQuestionModel>();
        var counter: ICounter = {
            current: 0,
            total: 0
        }
        var quizzId: number;

        function init(id: number, reviewers: IReviewerFromQuestionModel[]) {
            quizzId = id;
            origReviewers = reviewers;
            counter.total = reviewers.length;
        }

        function start(randomize: boolean): void {
            reviewers.splice(0, reviewers.length);
            counter.current = 0;
            if (randomize == true) {
                var reviewersCopy: IReviewerFromQuestionModel[] = new Array<IReviewerFromQuestionModel>();
                origReviewers.forEach(function (item: IReviewerFromQuestionModel) {
                    item.isMarked = false;
                    reviewersCopy.push(item);
                });
                var count: number = reviewersCopy.length;

                while (count > 0) {
                    var idx: number = Math.floor(Math.random() * count);
                    var reviewer: IReviewerFromQuestionModel = reviewersCopy[idx];
                    reviewers.push(reviewer);
                    reviewersCopy.splice(idx, 1);

                    count--;
                }

            } else {
                origReviewers.forEach(function (item: IReviewerFromQuestionModel) {
                    item.isMarked = false;
                    reviewers.push(item);
                });
            }
        }

        function getCurrentReviewer(): IReviewerFromQuestionModel {
            return reviewers[counter.current];
        }

        function getReviewerAtIdx(idx: number): IReviewerFromQuestionModel {
            
            if (idx >= 0 && idx < counter.total) {
                counter.current = idx;
                return reviewers[idx];
            }

            return null;
        }

        function canGoToPrevReviewer(): boolean {
            return counter.current > 0;
        }

        function goToPrevReviewer(): void {
            if (counter.current <= 0)
                return;
            counter.current--;

            goToCurrentIdx();
        }

        function canGoToNextReviewer(): boolean {
            return counter.current < counter.total - 1;
        }

        function goToNextReviewer(): void {
            if (counter.current >= counter.total - 1)
                return;
            counter.current++;

            goToCurrentIdx();
        }

        function toggleMark(): void {
            reviewers[counter.current].isMarked = !reviewers[counter.current].isMarked;
        }

        function getMarkedReviewers(): IReviewerFromQuestionModel[] {
            var markedReviewers: IReviewerFromQuestionModel[] = new Array<IReviewerFromQuestionModel>();

            reviewers.forEach(function (item: IReviewerFromQuestionModel) {
                if (item.isMarked == true)
                    markedReviewers.push(item);
            });

            return markedReviewers;
        }

        function goToReviewerIdx(idx: number): void {
            if (idx >= 0 && idx < counter.total) {
                counter.current = idx;
                goToCurrentIdx();
            }
        }

        function goToCurrentIdx() {
            $state.go("si.viewAsFlashCard.flashCardItem", { quizzId: quizzId, fcId: counter.current });
        }

        return {
            counter: counter,
            init: init,
            start: start,
            getCurrentReviewer: getCurrentReviewer,
            getReviewerAtIdx: getReviewerAtIdx,

            canGoToPrevReviewer: canGoToPrevReviewer,
            goToPrevReviewer: goToPrevReviewer,

            canGoToNextReviewer: canGoToNextReviewer,
            goToNextReviewer: goToNextReviewer,

            toggleMark: toggleMark,
            getMarkedReviewers: getMarkedReviewers,

            goToReviewerIdx: goToReviewerIdx
        }
    }

})();