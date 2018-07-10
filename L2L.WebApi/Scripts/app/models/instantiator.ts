function newCtrlState(numResourceToWait: number): ICtrlState {
    return {
        isReady: false, 
        numResourceToWait: numResourceToWait
    }
} 