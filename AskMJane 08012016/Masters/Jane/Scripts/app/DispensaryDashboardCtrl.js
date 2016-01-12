angular.module('AskMJaneAngularApp').controller('DispensaryDashboardCtrl', DispensaryDashboardCtrl);

DispensaryDashboardCtrl.$inject = ['$scope', '$http'];

function DispensaryDashboardCtrl($scope, $http) {
    
    $scope.model = {
        CurrentDispensary: null
    }
    
    $scope.loading = true;

}