angular.module('AskMJaneAngularApp').controller('RolesManagerCtrl', RolesManagerCtrl);

RolesManagerCtrl.$inject = ['$scope', '$http', '$modal'];

function RolesManagerCtrl($scope, $http, $modal) {

    $scope.people = [];
    $scope.loading = true;
    $scope.roles = [];

    $scope.getRoles = function () {
        $http.get($scope.ApiAddress + "/roles/GetAllRoles").then(function (response) {

            $scope.roles = response.data;
            $scope.getNonPatients();
        }, function (response) {
            console.log(response);
        });
    }
    $scope.getRoles();

    $scope.getNonPatients = function () {
        $http.get($scope.ApiAddress + "/roles/getUsers").then(function (response) {

            for (var j = 0; j < response.data.length; j++) {
                response.data[j].roles = $.map($scope.roles, function (obj) {
                    return $.extend(true, {}, obj);
                });
                for (var l = 0; l < response.data[j].rolesList.length; l++) {
                    for (var m = 0; m < response.data[j].roles.length; m++) {
                        if (response.data[j].roles[m].id === response.data[j].rolesList[l].id)
                            response.data[j].roles[m].ticked = true;
                    }
                }
            }

            $scope.people = response.data;
            $scope.loading = false;
        }, function (response) {
            console.log(response);
            $scope.loading = false;
        });
    }

    $scope.save = function(user) {
        $http.post($scope.ApiAddress + "/roles/updateUserRoles", JSON.stringify(user)).then(function (response) {

        }, function (response) {
            console.log(response);
        });

    }
};