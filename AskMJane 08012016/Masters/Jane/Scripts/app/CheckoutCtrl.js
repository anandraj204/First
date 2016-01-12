angular.module('AskMJaneAngularApp').controller('CheckoutCtrl', CheckoutCtrl);

CheckoutCtrl.$inject = ['$scope', '$http', '$geolocation'];

function CheckoutCtrl($scope, $http, $geolocation) {
    $scope.DeliveryTypeEnum = [
        {
            'Id': 1,
            'Type': 'Delivery'
        },
        {
            'Id': 2,
            'Type': 'Pickup'
        }
    ];
    $scope.SelectedDeliveryType = $scope.DeliveryTypeEnum[0];
    $scope.SelectDeliveryType = function (item) {
        $scope.SelectedDeliveryType = item;
    }

    $scope.PaymentTypeEnum = [
      {
          'Id': 1,
          'Type': 'Cash'
      },
      {
          'Id': 2,
          'Type': 'Credit'
      }
    ];
    $scope.DeliveryAddress = {
        'GoogleAddress' : '',
        'Address1': '',
        'Address2': '',
        'City': '',
        'State': '',
        'Zip': '',
        'Country': ''
    };
    $scope.GoogleAddress = "";
    $scope.GetUser = function () {
        var usertoken = Cookies.get("usertoken");
        $http.get($scope.ApiAddress + "/account/getbyusertoken?usertoken=" + usertoken)
          .then(function (response, data) {
              if (typeof (response) == "undefined") {
              } else {
                  $scope.user = response.data;
                  if ($scope.user.deliveryAddress == null) {
                      $scope.user.deliveryAddress = $scope.DeliveryAddress;
                  }
                  $scope.GetCartByUserId($scope.user.id);
                  console.log(response.data);
              }
          }, function (response) {
              console.log(response);
          });
        console.log(usertoken);
    }

    $scope.GetCartByUserId = function (id) {
        $http.get("http://local.api.askmjane.com/api/cart/getbyuserid?userid=" + id)
          .then(function (response, data) {
              if (typeof (response) == "undefined") {
              } else {
                  $scope.cart = response.data;
                  console.log(response.data);
              }
          }, function (response) {
              console.log(response);
          });
    }

    $scope.GetUser();



    var options = {
        types: [],
        componentRestrictions: { country: "us" }
    };
    $(document).ready(function() {
        var inputFrom = document.getElementById('DeliveryAddress');
        var autocompleteFrom = new google.maps.places.Autocomplete(inputFrom, options);
        google.maps.event.addListener(autocompleteFrom, 'place_changed', function() {
            var place = autocompleteFrom.getPlace();
            var components = place.address_components;
            for (var i = 0; i < components.length; i++) {
                for (var j = 0; j < components[i].types.length; j++) {
                    if ("street_number" == components[i].types[j]) {
                        $scope.user.DeliveryAddress.Address1 = components[i].long_name;
                    }
                    if ("route" == components[i].types[j]) {
                        $scope.user.DeliveryAddress.Address1 = $scope.dispensaryModel.Address.Address1 + " " + components[i].long_name;
                    }
                    if ("locality" == components[i].types[j]) {
                        $scope.user.DeliveryAddress.City = components[i].long_name;
                    }
                    if ("administrative_area_level_1" == components[i].types[j]) {
                        $scope.user.DeliveryAddress.State = components[i].short_name;
                    }
                    if ("country" == components[i].types[j]) {
                        $scope.user.DeliveryAddress.Country = components[i].short_name;
                    }
                    if ("postal_code" == components[i].types[j]) {
                        $scope.user.DeliveryAddress.Zip = components[i].short_name;
                    }
                }
            }
        });
    });


}