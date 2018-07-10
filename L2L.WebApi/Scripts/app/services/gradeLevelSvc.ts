interface IGradeLevelSvc {
    getGradeLevels: (success?: Function, error?: Function) => IGradeLevelModel[];
} 

module gradeLevelSvc {
    export interface IResource {
        gradeLevel: any;
    }
}

(function () {
    l2lApp.service("gradeLevelSvc", gradeLevelSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): gradeLevelSvc.IResource {
        var gradeLevel = $resource(setting.serverUrl() + '/api/GradeLevel', null,
            {
                'get': { headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } }
            });

        return {
            gradeLevel: gradeLevel
        }
    }

    gradeLevelSvc.$inject = ["$resource", "currentUser"];
    function gradeLevelSvc(
        $resource,
        currentUser: ICurrentUser
        ): IGradeLevelSvc {

        function getGradeLevels(success?: Function, error?: Function): IGradeLevelModel[] {
            return <IGradeLevelModel[]>resource($resource, currentUser).gradeLevel.query(null, success, error);
        }

        return {
            getGradeLevels: getGradeLevels
        }
    }
    
})();