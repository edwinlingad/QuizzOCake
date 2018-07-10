
l2lApp.directive('inputText', function () {
    return {
        restrict: 'E',
        templateUrl: 'scripts/templates/directives/input-text.html',
        replace: true,
        scope: {
            name: '@',
            label: '@',
            model: '=',
            formClassAdd: "@"
        }
    }
});


l2lApp.directive('inputPassword', function () {
    return {
        restrict: 'E',
        templateUrl: 'scripts/templates/directives/input-password.html',
        replace: true,
        scope: {
            name: '@',
            label: '@',
            model: '=',
            formClassAdd: "@"
        }
    }
});


/* 
<inp-radio-bool name="my-test" label-true="True" label-false="False" model="myModel" changed="changed(myModel)" > </inp-radio-bool>
 */
l2lApp.directive('inpRadioBool', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: 'scripts/templates/directives/input-radio-bool.html',
        scope: {
            name: '@',
            labelTrue: "@",
            labelFalse: "@",
            model: "=",
            changed: "&"
        }
    }
});

/*

 */

l2lApp.directive('inpRadio', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: 'scripts/templates/directives/input-radio.html',
        scope: {
            groupName: "@",
            name: "@",
            value: "@",
            label: "@",
            checked: "@",
            model: "=",
            changed: "&",
            addClass: "@"
        }
    }
});

l2lApp.directive('datePicker', function () {
    return {
        restrict: 'E',
        replace: true,
        template: "<input type='text' id='{{dId}}' class='{{addClass}}' ng-change='changed()' ng-model='model' value='{{model}}'>",
        scope: {
            dId: "@",
            addClass: "@?",
            changed: "&?",
            minDate: "@",
            maxDate: "@",
            model: "=",
        },
        controller: function (
            $scope,
            $timeout: ng.ITimeoutService
            ) {

            $timeout(function () {
                $('#' + $scope.dId).datepicker({ minDate: $scope.minDate, maxDate: $scope.maxDate,  dateFormat: "DD, d MM, yy" });
            }, 500);

        }
    }
});