interface IQAAnswerModel extends ITrackModify {
    id?: number;
    answer?: string;
    questionId?: number;
}

interface IQAQuestionModel extends IEditorControl {
    id?: number;
    question?: string;
    answersInOrder?: boolean;
    isMultiplePoints?: boolean;
    authorId?: number;
    testId?: number;
    answers: IQAAnswerModel[];
}