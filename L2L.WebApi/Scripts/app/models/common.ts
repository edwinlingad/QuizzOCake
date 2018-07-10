interface IPatternModel {

}

interface ICtrlState {
    isReady: boolean;
    numResourceToWait: number;
}

interface IPageOld {
    title?: string;
    btnName?: string;
    disabled?: boolean;
}

interface IElement {
    name?: string;
    isEnabled?: boolean;
    click?: (value: any) => void;
    isDisabledFn?: () => void;
    addClass?: string;
    disabled?: boolean;
    tooltip?: string;
}

interface IInputItem {
    name?: string;
    value: string;
}

interface IPage {
    isReady: boolean;
    title?: string,
    subTitle?: string,
    numResourceToWait: number;
    disabled?: boolean;
    isModified?: boolean;
    isNotFoundOrAuthorized?: boolean;
}

interface IToggleElement {
    name?: string;
    id?: number;
    value?: boolean;
}


interface IGradeLevelModel {
    name: string;
    gradeLevel: number;

    fgColor: string;
    bgColor: string;

    numQuizz: number;
}

interface IPager {
    currentPage: number;
    totalItems: number;
    pageChanged(): void;
}


interface IPermission {
    canCreate?: boolean;
    canEdit?: boolean;
    canDelete?: boolean;
}

interface IItemToWait {
    isReady: boolean;
}

interface IActionElement {
    isReady: boolean;
    click?: Function;
}

interface IButtonElement {
    name?: string;
    isReady?: boolean;
    click?: Function;
    disabled?: boolean;
    visible?: boolean;
    tooltip?: string;
    showSubControl?: boolean;
}

interface IToggleButton extends IToggleElement {
    isReady?: boolean;
    click?: Function;
    tooltip?: string;
}

interface ICounter {
    current: number;
    total: number;
}

interface X {
}

interface IOauthUserInfo {
    firstName: string;
    lastName: string;
    email: string;
    profilePixUrl?: string;
}
