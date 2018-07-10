interface ITextFlashCardModel extends IEditorControl {
    id?: number;
    //title?: string;
    answer?: string;
    reviewerId?: number;
    ownerId?: number;

    // for view
    canEdit?: boolean;
    isEditOpen?: boolean;
    isViewOpen?: boolean;
}

interface IQuickNoteModel extends IEditorControl {
    id?: number;
    //title?: string;
    notes?: string;
    reviewerId?: number;
    ownerId?: number;

    // for view
    canEdit?: boolean;
    isEditOpen?: boolean;
    isViewOpen?: boolean;
}

interface IReviewerQuestionAnswer {
    answer: string;
}

interface IReviewerFromQuestionModel extends IEditorControl {
    questionId: number;
    question: string;
    questionViewType: number;
    answers: IReviewerQuestionAnswer[];

    isAnswerShown: boolean;
    isMarked: boolean;
} 
