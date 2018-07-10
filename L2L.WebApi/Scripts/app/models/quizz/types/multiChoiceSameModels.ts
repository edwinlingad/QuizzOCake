interface IMultiChoiceSameQuestionModel extends IEditorControl {
    id?: number;
    question?: string;
    answers?: string;
    isMultiplePoints?: boolean;

    testId?: number;
    choiceGroupId?: number;
    choiceGroup?: IMultiChoiceSameChoiceGroupModel;
}

interface IMultiChoiceSameChoiceGroupModel extends ITrackModify {
    id: number;
    name: string;
    shuffleChoices: boolean;
    isMultiplePoints: boolean;
    testId: number;
    choices: IMultiChoiceSameChoiceModel[];
}

interface IMultiChoiceSameChoiceModel extends ITrackModify {
    id: number;
    value: string;
    choiceGroupId: number;

    noDelete?: boolean;
}