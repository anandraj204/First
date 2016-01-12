var app = angular.module('AskMJaneAngularApp', ["isteven-multi-select", 'ngFileUpload', 'ui.bootstrap', 'ngGeolocation', 'ngPlacesAutocomplete', 'ngCookies', 'ui.directives', 'ui.filters']);


angular.module('AskMJaneAngularApp').directive('zipcodesvalidator', zipcodesvalidator);

function zipcodesvalidator() {
    return {

        // limit usage to argument only
        restrict: 'A',

        // require NgModelController, i.e. require a controller of ngModel directive
        require: 'ngModel',

        // create linking function and pass in our NgModelController as a 4th argument
        link: function (scope, element, attr, ctrl) {
            // please note you can name your function & argument anything you like
            function customValidator(ngModelValue) {
                var codes = ngModelValue.split(',');
                if (codes.length == 0) {
                    ctrl.$setValidity('zipcodesEmptyValidator', false);
                } else {
                    ctrl.$setValidity('zipcodesEmptyValidator', true);
                }
                var valid = true;
                for (var i = 0; i < codes.length; i++) {
                    codes[i] = codes[i].trim();
                    if (codes[i].length != 5) {
                        valid = false;
                    }
                    if (!(/^[0-9]{5,5}$/.test(codes[i]))) {
                        valid = false;
                    }
                }
                ctrl.$setValidity('zipcodeValidator', valid);

                // we need to return our ngModelValue, to be displayed to the user(value of the input)
                return ngModelValue;
            }

            // we need to add our customValidator function to an array of other(build-in or custom) functions
            // I have not notice any performance issues, but it would be worth investigating how much
            // effect does this have on the performance of the app
            ctrl.$parsers.push(customValidator);
        }
    }
};

angular.module('AskMJaneAngularApp').directive("phoneFormat", phoneFormat);
phoneFormat.$inject = ['$filter', '$browser'];
function phoneFormat($filter, $browser) {
    return {
        require: 'ngModel',
        link: function ($scope, $element, $attrs, ngModelCtrl) {
            var listener = function () {
                var value = $element.val().replace(/[^0-9]/g, '');
                $element.val($filter('tel')(value, false));
            };

            // This runs when we update the text field
            ngModelCtrl.$parsers.push(function (viewValue) {
                return viewValue.replace(/[^0-9]/g, '').slice(0, 10);
            });

            // This runs when the model gets updated on the scope directly and keeps our view in sync
            ngModelCtrl.$render = function () {
                $element.val($filter('tel')(ngModelCtrl.$viewValue, false));
            };

            $element.bind('change', listener);
            $element.bind('keydown', function (event) {
                var key = event.keyCode;
                // If the keys include the CTRL, SHIFT, ALT, or META keys, or the arrow keys, do nothing.
                // This lets us support copy and paste too
                if (key == 91 || (15 < key && key < 19) || (37 <= key && key <= 40)) {
                    return;
                }
                $browser.defer(listener); // Have to do this or changes don't get picked up properly
            });

            $element.bind('paste cut', function () {
                $browser.defer(listener);
            });
        }
    };
};

angular.module('AskMJaneAngularApp').filter('tel', function () {
    return function (tel) {
        console.log(tel);
        if (!tel) { return ''; }

        var value = tel.toString().trim().replace(/^\+/, '');
        if (value.match(/[^0-9]/)) {
            return tel;
        }
        var country, city, number;
        switch (value.length) {
            case 1:
            case 2:
            case 3:
                city = value;
                break;

            default:
                city = value.slice(0, 3);
                number = value.slice(3);
        }

        if (number) {
            if (number.length > 3) {
                number = number.slice(0, 3) + '-' + number.slice(3, 7);
            }
            else {
                number = number;
            }

            return ("(" + city + ") " + number).trim();
        }
        else {
            return "(" + city;
        }

    };
});

angular.module('AskMJaneAngularApp').factory('httpRequestInterceptor', httpRequestInterceptor);

httpRequestInterceptor.$inject = ['$cookies', '$q', 'ApiAddress'];

function httpRequestInterceptor($cookies, $q, ApiAddress) {
    return {
        request: function (config) {
            if (config.url.indexOf(ApiAddress) != -1) {
                var token = $cookies.get("access_token");
                config.headers = {
                    'Authorization': 'Bearer ' + token
                };

                //  fix for all multipart-form uploaders
                if (typeof (config.headers['Content-Type']) == 'undefined' && config.url.indexOf('upload') === -1)
                    config.headers['Content-Type'] = 'application/json';
            }
            return config;
        },
        response: function (response) {
            return response || $q.when(response);
            // return response;

        },
        responseError: function (rejection) {
            // your error handler
            //return rejection;
            return $q.reject(rejection);

        }
    };
}



angular.module('AskMJaneAngularApp').config(function ($httpProvider) {
  $httpProvider.interceptors.push('httpRequestInterceptor');
});