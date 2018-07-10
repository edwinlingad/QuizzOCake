interface ICachedDataSvc {
    getQuizzzCategories(success?: Function, error?: Function): IQuizzCategoryModel[];
    getGradeLevels(success?: Function, error?: Function): IGradeLevelModel[];
    getBadgeDescriptions(success?: Function, error?: Function): string[];

    getQuizzById(id: number, success?: Function, error?: Function): IQuizzModel;
    deleteQuizzById(id: number);
    getLastQuizz(): IQuizzModel;
    getQuizzOverviewById(id: number, success?: Function, error?: Function): IQuizzOverviewModel;
    deleteQuizzOverviewById(id: number);
    getTestById(id: number, success?: Function, error?: Function): ITestModel;

    getQandAQuestionById(id: number, success?: Function, error?: Function): IQAQuestionModel;
    deleteQandAQuestionById(id: number): void;

    getMultiChoiceQuestionById(id: number, success?: Function, error?: Function): IMCQuestionModel;
    deleteMulitChoiceQuestionById(id: number): void;

    getTakeTestModelById(id: number, success?: Function, error?: Function): ITakeTestModel;
    getTestLogById(id: number, success?: Function, error?: Function): ITestLogModel;

    getMultiChoiceSameChoiceGroupById(id: number, testId: number, success?: Function, error?: Function): IMultiChoiceSameChoiceGroupModel;
    getMultiChoiceSameQuestionById(id: number, success?: Function, error?: Function): IMultiChoiceSameQuestionModel;

    getTestLogGroupedById(id: number, success?: Function, error?: Function): ITestLogGroupedModel[];
    getNotificationTypes(success?: Function, error?: Function): INotificationTypeModel[];

    getQuickNotesByRevId(id: number, success?: Function, error?: Function): IQuickNoteModel[];
    getQuickNoteById(id: number, success?: Function, error?: Function): IQuickNoteModel;

    getTextFlashCardsByRevId(id: number, success?: Function, error?: Function): ITextFlashCardModel[];
    getTextFlashCardById(id: number, success?: Function, error?: Function): ITextFlashCardModel;

    getAssignmentGroupById(id: number, success?: Function, error?: Function): IAssignmentGroupModel;
    deleteAssignmentGroupdById(id: number);
    getAssignmentById(id: number, success?: Function, error?: Function): IAssignmentModel;
    deleteAssignmentById(id: number);

    getAssignmentGroups(pageNum: number, completed: boolean, success?: Function, error?: Function): IAssignmentGroupModel[];
    getAssignments(dependentId: number, pageNum: number, completed: boolean, success?: Function, error?: Function): IAssignmentModel[];

    getQuizzerInfo(quizzerId: number, success?: Function, error?: Function): IQuizzerModel;
}

(function () {
    l2lApp.service("cachedDataSvc", cachedDataSvc);

    cachedDataSvc.$inject = ["quizzSvc", "quizzOverviewSvc", "testSvc", "qaQuestionSvc", "mcQuestionSvc", "testLogSvc", "flashCardSvc", "multiChoiceSameChoiceGroupSvc", "multiChoiceSameQuestionSvc", "quizzCategorySvc", "gradeLevelSvc", "userNotificationSvc", "quickNotesSvc", "textFlashCardsSvc", "resourceSvc", ];
    function cachedDataSvc(
        quizzSvc: IQuizzSvc,
        quizzOverviewSvc: IQuizzOverviewSvc,
        testSvc: ITestSvc,
        qaQuestionSvc: IQAQuestionSvc,
        mcQuestionSvc: IMCQuestionSvc,
        testLogSvc: ITestLogSvc,
        flashCardSvc: IFlashCardSvc,
        multiChoiceSameChoiceGroupSvc: IMultiChoiceSameChoiceSvc,
        multiChoiceSameQuestionSvc: IMultiChoiceSameQuestionSvc,
        quizzCategorySvc: IQuizzCategorySvc,
        gradeLevelSvc: IGradeLevelSvc,
        userNotificationSvc: IUserNotificationSvc,
        quickNotesSvc: IQuickNoteSvc,
        textFlashCardsSvc: ITextFlashCardSvc,
        resourceSvc: IResourceSvc

    ): ICachedDataSvc {
        function error(response) {
            //commonError($scope, response);
        }

        var quizzCategories: IQuizzCategoryModel[];
        function getQuizzzCategories(success?: Function, error?: Function): IQuizzCategoryModel[] {
            if (quizzCategories === undefined || quizzCategories.$resolved == false) {
                //quizzCategories = quizzCategorySvc.getCategories(success, error);
                quizzCategories = resourceSvc.getResourceMany(enums.ResourceTypeEnum.QuizzCategories, 0, 0, 0, 0, 0, success, error);
            } else {
                if (success != undefined)
                    success(quizzCategories);
            }
            return quizzCategories;
        }

        var gradeLevels: IGradeLevelModel[];
        function getGradeLevels(success?: Function, error?: Function): IGradeLevelModel[] {
            if (gradeLevels === undefined || gradeLevels.$resolved == false) {
                gradeLevels = gradeLevelSvc.getGradeLevels(success, error);
            } else {
                if (success != undefined)
                    success(gradeLevels);
            }
            return gradeLevels;
        }

        var badgeDescriptions: string[];
        function getBadgeDescriptions(success?: Function, error?: Function): string[] {
            if (badgeDescriptions === undefined || badgeDescriptions.$resolved == false)
                badgeDescriptions = resourceSvc.getResourceMany(enums.ResourceTypeEnum.BadgeList, 0, 0, 0, 0, 0, success, error);
            else {
                if (success != undefined)
                    success(badgeDescriptions);
            }

            return badgeDescriptions;
        }

        var notificationTypeModels: INotificationTypeModel[];
        function getNotificationTypes(success?: Function, error?: Function): INotificationTypeModel[] {
            if (notificationTypeModels === undefined) {
                notificationTypeModels = userNotificationSvc.getNotificationTypes(success, error);
            } else {
                if (success != undefined)
                    success(notificationTypeModels);
            }
            return notificationTypeModels;
        }

        var quizzes: DataCache = new DataCache(5);
        function getQuizzById(id: number, success?: Function, error?: Function): IQuizzModel {

            var model = quizzSvc.getQuizzById(id, function (data) {
                quizzes.addEntry(data);
                if (success != undefined)
                    success(data);
            }, error);

            //var model: IQuizzModel = quizzes.getEntry(id);
            //if (model === undefined) {
            //    model = quizzSvc.getQuizzById(id, function (data) {
            //        quizzes.addEntry(data);
            //        if (success != undefined)
            //            success(data);
            //    }, error);
            //}
            //else {
            //    if (success != undefined)
            //        success(model);
            //}

            return model;
        }

        function deleteQuizzById(id: number) {
            quizzes.deleteEntry(id);
        }

        function getLastQuizz(): IQuizzModel {
            return quizzes.getLastEntry();
        }

        var quizzOverviews: DataCache = new DataCache(5);
        function getQuizzOverviewById(id: number, success?: Function, error?: Function): IQuizzOverviewModel {

            var model = quizzOverviewSvc.getQuizzOverview(id,
                function (data) {
                    quizzOverviews.addEntry(data);
                    if (success != undefined)
                        success(data);
                }, error);

            //var model: IQuizzOverviewModel = quizzOverviews.getEntry(id);
            //if (model === undefined) {
            //    model = quizzOverviewSvc.getQuizzOverview(id, function (data) {
            //        quizzOverviews.addEntry(data);
            //        if (success != undefined)
            //            success(data);
            //    }, error);
            //}
            //else {
            //    if (success != undefined)
            //        success(model);
            //}

            return model;
        }

        function deleteQuizzOverviewById(id: number) {
            quizzOverviews.deleteEntry(id);
        }

        var tests: DataCache = new DataCache(3);
        function getTestById(id: number, success?: Function, error?: Function): ITestModel {

            var model: ITestModel = tests.getEntry(id);
            if (model === undefined) {
                model = testSvc.getTest(id, function (data) {
                    tests.addEntry(data);
                    if (success != undefined)
                        success(data);
                }, error);
            }
            else {
                if (success != undefined)
                    success(model);
            }

            return model;
        }

        //var qandAQuestions: DataCache = new DataCache(20);
        function getQandAQuestionById(id: number, success?: Function, error?: Function): IQAQuestionModel {
            var model: IQAQuestionModel = qaQuestionSvc.getQuestion(id, function (data) {
                if (success != undefined)
                    success(data);
            }, error);

            //var model: IQAQuestionModel = qandAQuestions.getEntry(id);
            //if (model === undefined) {
            //    model = qaQuestionSvc.getQuestion(id, function (data) {
            //        qandAQuestions.addEntry(data);
            //        if (success != undefined)
            //            success(data);
            //    }, error);
            //}
            //else {
            //    if (success != undefined)
            //        success(model);
            //}

            return model;
        }

        function deleteQandAQuestionById(id: number) {
            //qandAQuestions.deleteEntry(id);
        }

        //var multiChoiceQuestions: DataCache = new DataCache(20);
        function getMultiChoiceQuestionById(id: number, success?: Function, error?: Function): IMCQuestionModel {
            var model: IMCQuestionModel = mcQuestionSvc.getQuestion(id, function (data) {
                if (success != undefined)
                    success(data);
            }, error);

            //var model: IMCQuestionModel = multiChoiceQuestions.getEntry(id);
            //if (model === undefined) {
            //    model = mcQuestionSvc.getQuestion(id, function (data) {
            //        multiChoiceQuestions.addEntry(data);
            //        if (success != undefined)
            //            success(data);
            //    }, error);
            //}
            //else {
            //    if (success != undefined)
            //        success(model);
            //}

            return model;
        }

        function deleteMulitChoiceQuestionById(id: number): void {
            //multiChoiceQuestions.deleteEntry(id);
        }

        var takeTestModels: DataCache = new DataCache(5);
        function getTakeTestModelById(id: number, success?: Function, error?: Function): ITakeTestModel {

            //var model: ITakeTestModel = takeTestModels.getEntry(id);
            var model: ITakeTestModel = undefined;
            if (model === undefined) {
                model = testSvc.getTakeTestById(id, function (data) {
                    takeTestModels.addEntry(data);
                    if (success != undefined)
                        success(data);
                }, error);
            }
            else {
                if (success != undefined)
                    success(model);
            }

            return model;
        }

        var testLogModels: DataCache = new DataCache(5);
        function getTestLogById(id: number, success?: Function, error?: Function): ITestLogModel {

            var model: ITestLogModel = testLogModels.getEntry(id);
            if (model === undefined) {
                model = testLogSvc.getTestLog(id, function (data) {
                    testLogModels.addEntry(data);
                    if (success != undefined)
                        success(data);
                }, error);
            }
            else {
                if (success != undefined)
                    success(model);
            }

            return model;
        }

        var multiChoiceSameChoiceGroupModels: DataCache = new DataCache(10);
        function getMultiChoiceSameChoiceGroupById(id: number, testId: number, success?: Function, error?: Function): IMultiChoiceSameChoiceGroupModel {

            var model: IMultiChoiceSameChoiceGroupModel = multiChoiceSameChoiceGroupModels.getEntry(id);
            if (model === undefined) {
                var test: ITestModel = tests.getEntry(testId);
                for (var i = 0; i < test.multiChoiceSameChoiceGroups.length; i++) {
                    if (test.multiChoiceSameChoiceGroups[i].id == id) {
                        model = test.multiChoiceSameChoiceGroups[i];
                        break;
                    }
                }

                multiChoiceSameChoiceGroupModels.addEntry(model);
                if (success != undefined)
                    success(model);
            }
            else {
                if (success != undefined)
                    success(model);
            }

            return model;
        }

        var multiChoiceSameChoiceModels: DataCache = new DataCache(20);
        function getMultiChoiceSameQuestionById(id: number, success?: Function, error?: Function): IMultiChoiceSameQuestionModel {

            var model: IMultiChoiceSameQuestionModel = multiChoiceSameChoiceModels.getEntry(id);
            if (model === undefined) {
                model = multiChoiceSameQuestionSvc.getQuestion(id, function (data) {
                    multiChoiceSameChoiceModels.addEntry(data);
                    if (success != undefined)
                        success(data);
                }, error);
            }
            else {
                if (success != undefined)
                    success(model);
            }

            return model;
        }

        var testLogId: number = -1;
        var testLogGroupedModels: ITestLogGroupedModel[];
        function getTestLogGroupedById(id: number, success?: Function, error?: Function): ITestLogGroupedModel[] {

            testLogGroupedModels = testLogSvc.getTestLogGrouped(id, function (data) {
                if (success != undefined)
                    success(data);
            }, error);

            //if (id != testLogId) {
            //    testLogGroupedModels = testLogSvc.getTestLogGrouped(id, function (data) {
            //        testLogId = id;
            //        if (success != undefined)
            //            success(data);
            //    }, error);
            //}
            //else {
            //    if (success != undefined)
            //        success(testLogGroupedModels);
            //}

            return testLogGroupedModels;
        }

        //var quickNoteRevId: number = -1;
        var quickNoteListModels: IQuickNoteModel[];
        function getQuickNotesByRevId(id: number, success?: Function, error?: Function): IQuickNoteModel[] {

            quickNoteListModels = quickNotesSvc.getQuickNotes(id, function (data) {
                //quickNoteRevId = id;
                if (success != undefined)
                    success(data);
            }, error);

            //if (id != quickNoteRevId) {
            //    quickNoteListModels = quickNotesSvc.getQuickNotes(id, function (data) {
            //        quickNoteRevId = id;
            //        if (success != undefined)
            //            success(data);
            //    }, error);
            //}
            //else {
            //    if (success != undefined)
            //        success(quickNoteListModels);
            //}

            return quickNoteListModels;
        }

        //var quickNoteModels: DataCache = new DataCache(20);
        function getQuickNoteById(id: number, success?: Function, error?: Function): IQuickNoteModel {

            var model = quickNotesSvc.getQuickNote(id, function (data) {
                if (success != undefined)
                    success(data);
            }, error);

            //var model: IQuickNoteModel = quickNoteModels.getEntry(id);
            //if (model === undefined) {
            //    model = quickNotesSvc.getQuickNote(id, function (data) {
            //        quickNoteModels.addEntry(data);
            //        if (success != undefined)
            //            success(data);
            //    }, error);
            //}
            //else {
            //    if (success != undefined)
            //        success(model);
            //}

            return model;
        }

        //var textFlashCardRevId: number = -1;
        var textFlashCardListModels: ITextFlashCardModel[];
        function getTextFlashCardsByRevId(id: number, success?: Function, error?: Function): ITextFlashCardModel[] {

            textFlashCardListModels = textFlashCardsSvc.getTextFlashCards(id, function (data) {
                if (success != undefined)
                    success(data);
            }, error);

            //if (id != textFlashCardRevId) {
            //    textFlashCardListModels = textFlashCardsSvc.getTextFlashCards(id, function (data) {
            //        textFlashCardRevId = id;
            //        if (success != undefined)
            //            success(data);
            //    }, error);
            //}
            //else {
            //    if (success != undefined)
            //        success(textFlashCardListModels);
            //}

            return textFlashCardListModels;
        }

        //var textFlashCardModels: DataCache = new DataCache(20);
        function getTextFlashCardById(id: number, success?: Function, error?: Function): ITextFlashCardModel {

            var model = textFlashCardsSvc.getTextFlashCard(id, function (data) {
                if (success != undefined)
                    success(data);
            }, error);

            //var model: ITextFlashCardModel = textFlashCardModels.getEntry(id);
            //if (model === undefined) {
            //    model = textFlashCardsSvc.getTextFlashCard(id, function (data) {
            //        textFlashCardModels.addEntry(data);
            //        if (success != undefined)
            //            success(data);
            //    }, error);
            //}
            //else {
            //    if (success != undefined)
            //        success(model);
            //}

            return model;
        }

        var assignmentGroupModels: DataCache = new DataCache(20);
        function getAssignmentGroupById(id: number, success?: Function, error?: Function): IAssignmentGroupModel {
            var model = resourceSvc.getResource(enums.ResourceTypeEnum.AssignmentGroup, id, success, error);
            return model;

            //var model: IAssignmentGroupModel = assignmentGroupModels.getEntry(id);
            //if (model === undefined) {
            //    model = resourceSvc.getResource(enums.ResourceTypeEnum.AssignmentGroup, id, function (data) {
            //        assignmentGroupModels.addEntry(data);
            //        if (success != undefined)
            //            success(data);
            //    }, error);
            //}
            //else {
            //    if (success != undefined)
            //        success(model);
            //}

            //return model;
        }

        function deleteAssignmentGroupById(id: number) {
            assignmentGroupModels.deleteEntry(id);
        }

        var assignmentModels: DataCache = new DataCache(20);
        function getAssignmentById(id: number, success?: Function, error?: Function): IAssignmentModel {
            var model = resourceSvc.getResource(enums.ResourceTypeEnum.Assignment, id, success, error);
            return model;

            //var model: IAssignmentModel = assignmentModels.getEntry(id);
            //if (model === undefined) {
            //    model = resourceSvc.getResource(enums.ResourceTypeEnum.Assignment, id, function (data) {
            //        assignmentModels.addEntry(data);
            //        if (success != undefined)
            //            success(data);
            //    }, error);
            //}
            //else {
            //    if (success != undefined)
            //        success(model);
            //}

            //return model;
        }

        function deleteAssignmentById(id: number) {
            assignmentModels.deleteEntry(id);
        }

        var assignmentGroups: IAssignmentGroupModel[];
        function getAssignmentGroups(pageNum: number, completed: boolean, success?: Function, error?: Function): IAssignmentGroupModel[] {
            var completedParam: number = completed ? 1 : 0;
            assignmentGroups = resourceSvc.getResourceMany(enums.ResourceTypeEnum.AssignmentGroup, pageNum, completedParam, 0, 0, 0, success, error);
            return assignmentGroups;

            //if (assignmentGroups === undefined) {
            //    assignmentGroups = resourceSvc.getResourceMany(enums.ResourceTypeEnum.AssignmentGroup, 0, 0, 0, 0, 0, success, error);
            //} else {
            //    if (success != undefined)
            //        success(assignmentGroups);
            //}
            //return assignmentGroups;
        }

        function getAssignments(dependentId: number, pageNum: number, completed: boolean, success?: Function, error?: Function): IAssignmentModel[] {
            var completedParam: number = completed ? 1 : 0;
            var assignments = resourceSvc.getResourceMany(enums.ResourceTypeEnum.Assignment, dependentId, pageNum, completedParam, 0, 0, success, error);
            return assignments;
        }


        // Quizzer
        function getQuizzerInfo(quizzerId: number, success?: Function, error?: Function): IQuizzerModel {
            var quizzer: IQuizzerModel = resourceSvc.getResource(enums.ResourceTypeEnum.QuizzerInfo, quizzerId, success, error);
            return quizzer;
        }

        return {
            getQuizzzCategories: getQuizzzCategories,
            getGradeLevels: getGradeLevels,
            getBadgeDescriptions: getBadgeDescriptions,

            getQuizzById: getQuizzById,
            deleteQuizzById: deleteQuizzById,
            getLastQuizz: getLastQuizz,
            getQuizzOverviewById: getQuizzOverviewById,
            deleteQuizzOverviewById: deleteQuizzOverviewById,
            getTestById: getTestById,

            getQandAQuestionById: getQandAQuestionById,
            deleteQandAQuestionById: deleteQandAQuestionById,

            getMultiChoiceQuestionById: getMultiChoiceQuestionById,
            deleteMulitChoiceQuestionById: deleteMulitChoiceQuestionById,

            getTakeTestModelById: getTakeTestModelById,
            getTestLogById: getTestLogById,
            getMultiChoiceSameChoiceGroupById: getMultiChoiceSameChoiceGroupById,
            getMultiChoiceSameQuestionById: getMultiChoiceSameQuestionById,
            getTestLogGroupedById: getTestLogGroupedById,
            getNotificationTypes: getNotificationTypes,

            getQuickNotesByRevId: getQuickNotesByRevId,
            getQuickNoteById: getQuickNoteById,

            getTextFlashCardsByRevId: getTextFlashCardsByRevId,
            getTextFlashCardById: getTextFlashCardById,

            // Assignments
            getAssignmentGroupById: getAssignmentGroupById,
            deleteAssignmentGroupdById: deleteAssignmentGroupById,
            getAssignmentById: getAssignmentById,
            deleteAssignmentById: deleteAssignmentById,
            getAssignmentGroups: getAssignmentGroups,
            getAssignments: getAssignments,

            // Quizzer
            getQuizzerInfo: getQuizzerInfo
        }
    }
})();