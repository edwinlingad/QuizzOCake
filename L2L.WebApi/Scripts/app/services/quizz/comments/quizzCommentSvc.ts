interface IQuizzCommentSvc {
    getQuizzComments: (id: number, pageNum: number, numPerPage: number, skip: number, sortType: number, success?: Function, error?: Function) => IQuizzCommentModel[];
    createNewComment: (model: IQuizzCommentModel, success?: Function, error?: Function) => IQuizzCommentModel;
    updateComment: (model: IQuizzCommentModel, success?: Function, error?: Function) => void;
    deleteComment: (id: number, success?: Function, error?: Function) => void;
    likeUpVote: (id: number, success?: Function, error?: Function) => void;
    likeDownVote: (id: number, success?: Function, error?: Function) => void;
    flagUpVote: (id: number, success?: Function, error?: Function) => void;
    flagDownVote: (id: number, success?: Function, error?: Function) => void;
}

module quizzCommentSvc {
    export interface IResource {
        quizzComment: any;
        likeUpVote: any;
        likeDownVote: any;
        flagUpVote: any;
        flagDownVote: any;
    }
}

(function () {
    l2lApp.service("quizzCommentSvc", quizzCommentSvc);

    resource.$inject = ["$resource", "currentUser"];
    function resource($resource, currentUser): quizzCommentSvc.IResource {
        var quizzComment = $resource(setting.serverUrl() + '/api/QuizzComment', null,
            {
                'query': { method: 'GET', isArray: true, headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'post': { method: 'POST', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
                'delete': { method: 'DELETE', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });
        var likeUpVote = $resource(setting.serverUrl() + '/api/QuizzComment/LikeUpVote', null,
            {
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });
        var likeDownVote = $resource(setting.serverUrl() + '/api/QuizzComment/LikeDownVote', null,
            {
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        var flagUpVote = $resource(setting.serverUrl() + '/api/QuizzComment/FlagUpVote', null,
            {
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });
        var flagDownVote = $resource(setting.serverUrl() + '/api/QuizzComment/FlagDownVote', null,
            {
                'patch': { method: 'PATCH', headers: { 'Authorization': 'Bearer ' + currentUser.getAccessToken() } },
            });

        return {
            quizzComment: quizzComment,
            likeUpVote: likeUpVote,
            likeDownVote: likeDownVote,
            flagUpVote: flagUpVote,
            flagDownVote: flagDownVote
        }
    }

    quizzCommentSvc.$inject = ["$resource", "currentUser"];
    function quizzCommentSvc(
        $resource,
        currentUser: ICurrentUser
        ): IQuizzCommentSvc {

        function getQuizzComments(id: number, pageNum: number, numPerPage: number, skip: number, sortType: number,success?: Function, error?: Function): IQuizzCommentModel[] {
            return <IQuizzCommentModel[]>resource($resource, currentUser).quizzComment.query({ id: id, pageNum: pageNum, numPerPage: numPerPage, skip: skip, sortType: sortType }, success, error);
        }

        function createNewComment(model: IQuizzCommentModel, success?: Function, error?: Function): IQuizzCommentModel {
            return <IQuizzCommentModel>resource($resource, currentUser).quizzComment.post(model, success, error);
        }

        function updateComment(model: IQuizzCommentModel, success?: Function, error?: Function): void {
            resource($resource, currentUser).quizzComment.patch(model, success, error);
        }

        function deleteComment(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).quizzComment.delete({ id: id }, success, error);
        }

        function likeUpVote(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).likeUpVote.patch(id, success, error);
        }

        function likeDownVote(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).likeDownVote.patch(id, success, error);
        }

        function flagUpVote(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).flagUpVote.patch(id, success, error);
        }

        function flagDownVote(id: number, success?: Function, error?: Function): void {
            resource($resource, currentUser).flagDownVote.patch(id, success, error);
        }

        return {
            getQuizzComments: getQuizzComments,
            createNewComment: createNewComment,
            updateComment: updateComment,
            deleteComment: deleteComment,
            likeUpVote: likeUpVote,
            likeDownVote: likeDownVote,
            flagUpVote: flagUpVote,
            flagDownVote: flagDownVote
        }
    }
})();