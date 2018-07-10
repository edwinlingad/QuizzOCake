(function () {
    l2lApp.directive("loadWait", function () {
        return {
            scope: {
                page: "=",
            },
            replace: true,
            template: " <span ng-hide='page.isReady'><i class='fa fa-spinner fa-pulse'></i> </span>",
        }
    });
})(); 

(function () {
    l2lApp.directive("saveWait", function () {
        return {
            scope: {
                item: "=",
            },
            replace: true,
            template: "<span ng-hide='item.disabled == false'><i class='fa fa-spinner fa-pulse'></i></span>",
        }
    });
})(); 

(function () {
    l2lApp.directive("saveWaitIsReady", function () {
        return {
            scope: {
                item: "=",
            },
            replace: true,
            template: "<span ng-show='!item.isReady'><i class='fa fa-spinner fa-pulse'></i></span>",
        }
    });
})(); 

(function () {
    l2lApp.directive("itemLoadWait", function () {
        return {
            scope: {
                item: "=",
                addClass: "@?"
            },
            replace: true,
            template: "<div class='item-wait {{addClass}}'><span><i class='fa fa-spinner fa-pulse' ng-show='!item.isReady'></i></span></div>",
        }
    });
})(); 

(function () {
    l2lApp.directive("itemLoadWaitWithSpace", function () {
        return {
            scope: {
                item: "=",
            },
            replace: true,
            //template: "<i class='fa fa-spinner fa-pulse' ng-show='!item.isReady'></i>",
            template: "<i class='fa fa-spinner fa-pulse' ng-class='{invisible: item.isReady}'></i>",
        }
    });
})(); 

(function () {
    l2lApp.directive("pageLoadWait", function () {
        return {
            scope: {
                page: "=",
            },
            replace: true,
            template:   "<div class='page-load-wait ng-anim-fade' ng-hide='page.isReady'>" +
                            "<i class='fa fa-spinner fa-pulse'>" +
                            "</i>" +
                        "</div>"
        }
    });
})(); 

(function () {
    l2lApp.directive("pageNotFound", function () {
        return {
            scope: {
                page: "=",
            },
            replace: true,
            template: "<div class='page-not-found ng-anim-fade' ng-if='page.isNotFoundOrAuthorized'>" +
                            "<span>" +
                                "Page not Found or You are not Authorized to view Page" +
                            "</span>" +
                        "</div>"
        }
    });
})(); 