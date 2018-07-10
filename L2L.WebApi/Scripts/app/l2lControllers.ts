var l2lControllers = angular.module('l2lControllers', []);

function commonError($scope, response) {
    $scope.message = response.statusText + "\r\n";
    if (response.data.exceptionMessage)
        $scope.message += response.data.exceptionMessage;
    if (response.data.modelState) {
        for (var key in response.data.modelStatte) {
            $scope.message += response.data.modelState[key] + "\r\n";
        }
    }
}

function attachEnterPress() {
    $("form input").on("keypress", function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('button[type=submit] .default').click();
            return false;
        } else {
            return true;
        }
    });
}
