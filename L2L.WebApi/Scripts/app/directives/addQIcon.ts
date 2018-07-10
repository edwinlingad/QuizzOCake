(function () {
    l2lApp.directive("addQIcon", function () {
        var iconMap: string[] = ["Bi", "Qa", "Mc", "Cg"];
        var iconColorMap: string[] = ["q-b-i", "q-q-a", "q-mc","q-mcs-cg1"];
        function link(scope, element: ng.IAugmentedJQuery, attr: ng.IAttributes) {
            
            var idx: number;

            if (scope.question != undefined) {
                idx = parseInt(scope.question.questionType);
            } else {
                idx = parseInt(scope.questionType);
            }

            var htmlElement = '<span class=" q-color ' + iconColorMap[idx] + " " + scope.addClass + '">' + iconMap[idx] + "</span>";
            element.prepend(htmlElement);
        }

        return {
            scope: {
                question: "=",
                questionType: "@",
                addClass: "@"
            },
            link: link
        }
    });
})();