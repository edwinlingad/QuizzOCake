interface IMCQuestionModel extends IEditorControl {
    id?: number;
    question?: string;
    isMultiplePoints?: boolean;
    shuffleChoices?: boolean;
    authorId?: number;
    testId?: number;
    choices: IMCChoiceModel[];
}

interface IMCChoiceModel extends ITrackModify {
    id?: number;
    value?: string;
    order?: number;
    questionId?: number;
    isAnswer?: boolean;
}

interface IActualAnswer {
    value: string;
    isCorrect: boolean;
}

interface IMultiChoiceAnswer extends IActualAnswer {
    id?: number;
    isAnswer: boolean;
    isSelected: boolean;
}