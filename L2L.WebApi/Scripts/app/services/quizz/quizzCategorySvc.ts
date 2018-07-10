interface IQuizzCategorySvc {
    getCategories: (success?: Function, error?: Function) => IQuizzCategoryModel[];
}

module quizzCategorySvc {
    export interface IResource {
        quizzCategories: any;
    }
}

(function () {
    l2lApp.service('quizzCategorySvc', quizzCategorySvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): quizzCategorySvc.IResource {
        var quizzCategoriesResource = $resource(setting.serverUrl() + '/api/QuizzCategories', null,
            {
                'get': { headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        return {
            quizzCategories: quizzCategoriesResource
        }
    }

    var categories = undefined;

    quizzCategorySvc.$inject = ["$resource", "currentUser"];
    function quizzCategorySvc(
        $resource,
        currentUser: ICurrentUser
        ): IQuizzCategorySvc {
        function getCategories(success?: Function, error?: Function): IQuizzCategoryModel[]{
            if (categories === undefined) {
                categories = <IQuizzCategoryModel[]>resource($resource, currentUser).quizzCategories.query(null, success, error);
            }
            return categories;
        }

        return {
            getCategories: getCategories
        }
    }
})();