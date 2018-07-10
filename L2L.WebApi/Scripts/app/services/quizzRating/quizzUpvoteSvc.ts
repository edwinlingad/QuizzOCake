interface IQuizzUpvoteSvc {
    upVote: (id: number, success?: Function, error?: Function) => void;
    downVote: (id: number, success?: Function, error?: Function) => void;
}

module quizzUpvoteSvc {
    export interface IResource {
        upVote: any;
        downVote: any;
    }
}

(function () {
    l2lApp.service("quizzUpvoteSvc", quizzUpvoteSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): quizzUpvoteSvc.IResource {
        var upVote = $resource(setting.serverUrl() + '/api/QuizzUpvote/UpVote', null,
            {
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });
        var downVote = $resource(setting.serverUrl() + '/api/QuizzUpvote/DownVote', null,
            {
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        return {
            upVote: upVote,
            downVote: downVote
        }
    }

    quizzUpvoteSvc.$inject = ["$resource", "currentUser"];
    function quizzUpvoteSvc($resource, currentUser: ICurrentUser): IQuizzUpvoteSvc {
        function upVote(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).upVote.patch(id, success, error);
        }

        function downVote(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).downVote.patch(id, success, error);
        }

        return {
            upVote: upVote,
            downVote: downVote
        }
    }
})();