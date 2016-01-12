angular.module('AskMJaneAngularApp').controller('AdminOrderCtrl', AdminOrderCtrl);

AdminOrderCtrl.$inject = ['$scope', '$http', '$modal'];

function AdminOrderCtrl($scope, $http, $modal) {
    $scope.orders = [];
    $scope.dispensaries = [];
    $scope.markets = [];
    $scope.model = {
        CurrentDispensary: 0,
        CurrentMarket: ''
    };
    $scope.loading = true;

    $scope.getDispensaries = function () {
        $http.get($scope.ApiAddress + "/dispensaries").
            then(function (response) {
                $scope.dispensaries = response.data;;
                if ($scope.dispensaries.length > 0) {
                    //$scope.model.CurrentDispensary = $scope.dispensaries[0].id;
                    $scope.updateCurrentDispensary();
                }
            }, function (response) {
                //console.log(response)
            });
    }

    $scope.getMarkets = function () {
        $http.get($scope.ApiAddress + "/dispensaries/getMarkets").
            then(function (response) {
                $scope.markets = response.data;;
                if ($scope.markets.length > 0) {
                    //$scope.model.CurrentDispensary = $scope.dispensaries[0].id;
                    $scope.updateCurrentMarket();
                }
            }, function (response) {
                //console.log(response)
            });
    }

    $scope.predicate = 'name';
    $scope.reverse = true;
    $scope.order = function (predicate) {
        $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
        $scope.predicate = predicate;
    };


    $scope.getDispensaries();
    $scope.getMarkets();
    $scope.updateCurrentDispensary = function () {
        //  console.log($scope.CurrentDispensary);
        $http.get($scope.ApiAddress + "/orders/dispensary/" + $scope.model.CurrentDispensary)
            .then(function (response, data) {
                if (typeof (response) == "undefined") {
                } else {
                    $scope.orders = response.data;
                    console.log(response.data);
                }
                $scope.loading = false;
            }, function (response) {
                console.log(response);
                $scope.loading = false;
            });
    }

    $scope.updateCurrentMarket=function() {
        $http.get($scope.ApiAddress + "/orders/GetByMarket?stateName=" + ($scope.model.CurrentMarket ? $scope.model.CurrentMarket : ''))
           .then(function (response, data) {
               if (typeof (response) == "undefined") {
               } else {
                   $scope.orders = response.data;
                   console.log(response.data);
               }
               $scope.loading = false;
           }, function (response) {
               console.log(response);
               $scope.loading = false;
           });
    }

    $scope.GetDispensaryOrders = function () {

    }
    $scope.openMoreInfoModal = function (order) {
        var modalInstance = $modal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'orderInfoModal.html',
            size: "lg",
            controller: "AdminOrderInfoModalCtrl",
            resolve: {
                order: function () {
                    return order;
                }
            }
        });
    }
}



app.controller('AdminOrderInfoModalCtrl', AdminOrderInfoModalCtrl);

AdminOrderInfoModalCtrl.$inject = ['$scope', '$http', '$modalInstance', '$filter', 'order', 'ApiAddress'];

function AdminOrderInfoModalCtrl($scope, $http, $modalInstance, $filter, order, ApiAddress) {
    $scope.o = order;
    $scope.ApiAddress = ApiAddress;

    $scope.approve = function (form, valid) {
        if (valid) {
            $scope.patient.birthday = new Date($filter('date')($scope.patient.birthday, 'MM-dd-yyyy'));
            $scope.patient.patientInfo.approvalStatus = 4; //accepted
            $http.post($scope.ApiAddress + "/patient/SetApproval", $scope.patient)
              .then(function (response) {
                  if (typeof (response) == "undefined") {
                  }
                  else {
                      console.log(response);
                      $modalInstance.close();
                  }
              }, function (response) {
                  console.log(response);
                  alert("something went wrong while approving");
              });
        }
    };
    $scope.reject = function () {
        $scope.patient.birthday = new Date($filter('date')($scope.patient.birthday, 'MM-dd-yyyy'));
        $scope.patient.patientInfo.approvalStatus = 3; //rejected
        $http.post($scope.ApiAddress + "/patient/SetApproval", $scope.patient)
          .then(function (response) {
              if (typeof (response) == "undefined") {
              }
              else {
                  console.log(response);
                  $modalInstance.close();
              }
          }, function (response) {
              console.log(response);
              alert("something went wrong while rejecting");
          });

    }

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

}