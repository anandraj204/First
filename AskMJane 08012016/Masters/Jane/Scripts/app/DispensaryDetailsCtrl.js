angular
 .module("AskMJaneAngularApp")
 .controller("DispensaryDetailsCtrl", DispensaryDetailsCtrl);

DispensaryDetailsCtrl.$inject = ["$scope"];

function DispensaryDetailsCtrl($scope) {

    $scope.dispensaryProducts = [];
    $scope.loading = true;

    $scope.init = function (dispensary) {
        $scope.dispensary = dispensary;
        console.log(dispensary);
    };
};