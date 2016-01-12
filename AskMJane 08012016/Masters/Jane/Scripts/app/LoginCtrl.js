angular.module('AskMJaneAngularApp').controller('LoginCtrl', LoginCtrl);

LoginCtrl.$inject = ['$scope', '$http'];

function LoginCtrl($scope, $http) {
    $scope.username = null;
    $scope.password = null;
    $scope.confirmpassword = null;
    $scope.zipcode = null;
    $scope.alerts = [];
    $scope.redirect = location.search.split('redirectTo=')[1];

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
    $scope.register = function () {
        if (!$scope.username || !$scope.password) {
            return;
        }
        var url = $scope.ApiAddress + '/account/register';
        var data = {
            'Email': $scope.username,
            'Username': $scope.username,
            'Password': $scope.password,
            'ConfirmPassword': $scope.password,
            'Zipcode': $scope.zipcode,
            'Guid': Cookies.get("usertoken")
        };

        $http.post(url, data).
            then(function (response) {
                console.log(response);
                Cookies.set("usertoken", response.data.usertoken);
                Cookies.set("sessiontoken", response.data.sessiontoken);
                Cookies.set("access_token", response.data.access_token);
                if ($scope.redirect == 'undefined' || $scope.redirect == null) {
                    window.location.replace("/patient/applyv2");
                }
                else {
                    window.location.replace("/" + $scope.redirect);
                }
            }, function (response) {
                console.log(response);
                $scope.alerts = [];
                for (var i = 0; i < response.data.length; i++) {
                    $scope.alerts.push({ type: "danger", msg: response.data[i] });
                }


                // errors here, including:
                // invalid_grant_type
                // invalid username/password
            });
    }
    $scope.externalLogin = function () {
        var url = $scope.ApiAddress + '/account/Register';
        var data = {
            'email': $scope.email,
            'Password': $scope.password,
            'Guid': Cookies.get("usertoken"),
            'grant_type': 'password',
            'Redirect': $scope.redirect
        };

        $http.post(url, data).
            then(function (response) {
                console.log(data.access_token);
                Cookies.set("usertoken", response.data.usertoken);
                Cookies.set("sessiontoken", response.data.sessiontoken);
                Cookies.set("access_token", response.data.access_token);

                if ($scope.redirect === 'undefined' || $scope.redirect == null) {
                    window.location.replace("/menu");
                }
                else {
                    window.location.replace("/" + $scope.redirect);
                }

            }, function (response) {
                console.log(response);
                $scope.alerts = [];
                for (var i = 0; i < response.data.length; i++) {
                    $scope.alerts.push({ type: "danger", msg: response.data[i] });
                }

                // errors here, including:
                // invalid_grant_type
                // invalid username/password
            });
    };
    $scope.login = function () {
        if (!$scope.username || !$scope.password) {
            return;
        }

        var url = $scope.ApiAddress + '/account/login';
        var data = {
            'Username': $scope.username,
            'Password': $scope.password,
            'Guid': Cookies.get("usertoken"),
            'grant_type': 'password',
            'Redirect': $scope.redirect
        };

        $http.post(url, data).
            then(function (response) {
                console.log(data.access_token);
                Cookies.set("usertoken", response.data.usertoken);
                Cookies.set("sessiontoken", response.data.sessiontoken);
                Cookies.set("access_token", response.data.access_token);

                if ($scope.redirect === 'undefined' || $scope.redirect == null) {
                    window.location.replace("/menu");
                }
                else {
                    window.location.replace("/" + $scope.redirect);
                }

            }, function (response) {
                console.log(response);
                $scope.alerts = [];
                for (var i = 0; i < response.data.length; i++) {
                    $scope.alerts.push({ type: "danger", msg: response.data[i] });
                }

                // errors here, including:
                // invalid_grant_type
                // invalid username/password
            });
    };

}