angular.module('AskMJaneAngularApp').controller('CartCtrl', CartCtrl);

CartCtrl.$inject = ['$scope', '$http'];

function CartCtrl($scope, $http) {
    $scope.alerts = [];
    $scope.RegularPrice = 0.00;
    $scope.TotalPrice = 0.00;
    $scope.Discount = 0.00;
    $scope.Credit = 0.00;
    $scope.photoUrl = "";
    $scope.CurrentDispensary = {};
    $scope.invalidDeliveryAddress = true;

    $scope.useCredits = false;
    $scope.payFromCredit = 0;
    $scope.fundAmount = null;

    $scope.expressDelivery = true;
    $scope.cashPayment = false;

    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
    $scope.user = {};
    $scope.cart = {};
    $scope.quantity = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];

    $scope.DeliveryTypeEnum = [
    {
        'Id': 1,
        'Type': 'Immediate Delivery'
    },
    {
        'Id': 2,
        'Type': 'Express Lane Pickup'
    }
    ];
    $scope.PaymentTypeEnum = [
    {
        'Id': 1,
        'Type': 'Cash'
    },
    {
        'Id': 2,
        'Type': 'Credit/Debit'
    }
    ];
    $scope.SelectedDeliveryType = $scope.DeliveryTypeEnum[1];
    $scope.SelectDeliveryType = function () {
        $scope.SelectedDeliveryType = $scope.DeliveryTypeEnum[$scope.expressDelivery ? 1 : 0];
    }
    $scope.SelectedPaymentType = $scope.PaymentTypeEnum[1];
    $scope.SelectPaymentType = function (item) {
        $scope.SelectedPaymentType = $scope.PaymentTypeEnum[$scope.cashPayment ? 0 : 1];
    }


    $scope.DeliveryAddress = {
        'GoogleAddress': '',
        'Address1': '',
        'Address2': '',
        'City': '',
        'State': '',
        'Zip': '',
        'Country': ''
    };
    $scope.GoogleAddress = "";

    $scope.CanCheckout = false;

    var checkoutHandler = StripeCheckout.configure({
        key: 'pk_test_SIm9LDJwNaIz1mSqI4DqtpeI',
        locale: 'auto',
        token: function(token) {

            //hit api for checkout
            var checkoutData = {
                "UserId": $scope.user.id,
                "WalletId": $scope.user.walletId,
                "OrderId": $scope.cart.id,
                "DeliveryTypeId": $scope.SelectedDeliveryType.Id,
                "PaymentTypeId": $scope.SelectedPaymentType.Id,
                "DeliveryAddress": $scope.user.deliveryAddress,
                "TransactionAmount": ($scope.TotalPrice * 100),
                "StripeToken": token.id,
                "UseCredits": $scope.useCredits
            };

            $http.post($scope.ApiAddress + "/cart/checkout", checkoutData)
                .then(function(response) {
                    if (typeof (response) == "undefined") {
                    } else {
                        alert("Order completed.");
                        window.location.replace("/menu");
                    }
                }, function(response) {
                    console.log(response);
                    alert("Unable to process order at this time.");
                });
        }
    });

    $scope.Checkout = function (form, valid) {
        if (valid) {
            if ($scope.SelectedDeliveryType.Id == 1 && $scope.invalidDeliveryAddress) {
                return;
            }

            if ($scope.TotalPrice > 0) {
                checkoutHandler.open({
                    name: 'Checkout',
                    description: 'Checkout for $' + $scope.TotalPrice,
                    amount: $scope.TotalPrice * 100
                });
            } else {
                var checkoutData = {
                    "UserId": $scope.user.id,
                    "OrderId": $scope.cart.id,
                    "DeliveryTypeId": $scope.SelectedDeliveryType.Id,
                    "PaymentTypeId": $scope.SelectedPaymentType.Id,
                    "DeliveryAddress": $scope.user.deliveryAddress,
                    "UseCredits": $scope.useCredits
                }

                $http.post($scope.ApiAddress + "/cart/checkout", checkoutData)
                     .then(function (response) {
                         if (typeof (response) == "undefined") {
                         } else {
                             alert("Order completed.");
                             window.location.replace("/menu");
                         }
                     }, function (response) {
                         console.log(response);
                         alert("Unable to process order at this time.");
                     });
            }
        }
    }


    $scope.GetUser = function () {
        var usertoken = Cookies.get("usertoken");
        $http.get($scope.ApiAddress + "/account/getbyusertoken?usertoken=" + usertoken)
          .then(function (response, data) {
              if (typeof (response) == "undefined") {
              } else {
                  $scope.user = response.data;
                  $scope.CanCheckout = $scope.user != null && $scope.user.patientInfo != null && $scope.user.patientInfo.isApproved;

                  if ($scope.user.deliveryAddress == null) {
                      $scope.user.deliveryAddress = $scope.DeliveryAddress;
                  }
                  $scope.GetCartByUserId($scope.user.id);
                  console.log(response.data);
                  $scope.CanCheckout = $scope.user != null && $scope.user.patientInfo != null && $scope.user.patientInfo.isApproved;

              }
          }, function (response) {
              console.log(response);
          });
        console.log(usertoken);
    }

    $scope.GetCartByUserId = function (id) {
        $http.get($scope.ApiAddress + "/cart/getbyuserid?userid=" + id)
          .then(function (response, data) {
              if (typeof (response) == "undefined" || !response || !response.data) {
              } else {
                  $scope.MultipleDispensaries = false;
                  $scope.RegularPrice = 0;

                  $scope.cart = response.data;
                  if ($scope.cart.dispensaryProductVariantOrders.length > 0) {

                      $scope.CurrentDispensary = $scope.cart.dispensaryProductVariantOrders[0].dispensaryProductVariant.dispensaryProduct.dispensary;
                      angular.forEach(response.data.dispensaryProductVariantOrders, function (value, key) {
                          $scope.RegularPrice = $scope.RegularPrice + (value.quantity * value.unitPrice);
                          if (value.dispensaryProductVariant.dispensaryProduct.dispensary.id != $scope.CurrentDispensary.id) {
                              $scope.MultipleDispensaries = true;
                          }
                      });
                  }

                  $scope.RecalcTotal();
              }
          }, function (response) {
              console.log(response);
          });
    }

    $scope.GetUser();

    $scope.RecalcTotal = function () {
        var amount = $scope.RegularPrice;

        if ($scope.useCredits) {
            if (amount >= $scope.user.wallet.credit) {
                $scope.payFromCredit = $scope.user.wallet.credit;
                amount -= $scope.user.wallet.credit;
            } else {
                $scope.payFromCredit = amount;
                amount = 0;
            }
        }

        if (amount < 0)
            amount = 0;

        $scope.TotalPrice = amount;
    };


    $scope.UpdateQuantity = function (item) {
        $http.post($scope.ApiAddress + "/cart/updatequantity?id=" + item.id + "&qty=" + item.quantity)
       .then(function (response, data) {
           if (typeof (response) == "undefined") {
           } else {
               $scope.GetCartByUserId($scope.user.id);

               $scope.SmallModal.class = "updated";
               $scope.SmallModal.header = "Updated Item";
               $scope.SmallModal.text = "Item " + item.dispensaryProductVariant.name + " updated, quantity " + item.quantity;

               $scope.CloseSmallModalWithDelay();

           }
       }, function (response) {
           console.log(response);
       });
    }

    $scope.RemoveItem = function (item) {
        $http.post($scope.ApiAddress + "/cart/RemoveFromCart?id=" + item.id)
       .then(function (response, data) {
           if (typeof (response) == "undefined") {
           } else {
               $scope.BadgeCounter();
               $scope.GetCartByUserId($scope.user.id);
               $scope.RecalcTotal();

               $scope.SmallModal.class = "removed";
               $scope.SmallModal.header = "Removed Item";
               $scope.SmallModal.text = "Item " + item.dispensaryProductVariant.name + " removed from cart";

               $scope.CloseSmallModalWithDelay();
           }
       }, function (response) {
           console.log(response);
       });
    }

    var options = {
        componentRestrictions: { country: "us" }
    };
    $(document).ready(function () {
        var inputFrom = document.getElementById('DeliveryAddress');
        var autocompleteFrom = new google.maps.places.Autocomplete(inputFrom, { componentRestrictions: { country: "us" }, types: ["address"] });

        google.maps.event.addListener(autocompleteFrom, 'place_changed', function () {
            var place = autocompleteFrom.getPlace();
            var components = place.address_components;
            for (var i = 0; i < components.length; i++) {
                for (var j = 0; j < components[i].types.length; j++) {
                    if ("street_number" == components[i].types[j]) {
                        $scope.user.deliveryAddress.Address1 = components[i].long_name;
                    }
                    if ("route" == components[i].types[j]) {
                        $scope.user.deliveryAddress.Address1 = $scope.user.deliveryAddress.Address1 + " " + components[i].long_name;
                    }
                    if ("locality" == components[i].types[j]) {
                        $scope.user.deliveryAddress.City = components[i].long_name;
                    }
                    if ("administrative_area_level_1" == components[i].types[j]) {
                        $scope.user.deliveryAddress.State = components[i].short_name;
                    }
                    if ("country" == components[i].types[j]) {
                        $scope.user.deliveryAddress.Country = components[i].short_name;
                    }
                    if ("postal_code" == components[i].types[j]) {
                        $scope.user.deliveryAddress.Zip = components[i].short_name;
                    }
                }
            }
            $scope.user.deliveryAddress.Latitude = place.geometry.location.lat();
            $scope.user.deliveryAddress.Longitude = place.geometry.location.lng();
            $scope.user.deliveryAddress.FormattedAddress = place.formatted_address;
            $scope.invalidDeliveryAddress = false;
            $scope.showInvalidAddress = false;

            if (!$scope.user.deliveryAddress.FormattedAddress || !$scope.user.deliveryAddress.Address1) {
                $scope.invalidDeliveryAddress = $scope.showInvalidAddress = true;

            }

            $scope.$apply();
        });
    });

    // Close Checkout on page navigation
    $(window).on('popstate', function () {
        handler.close();
    });

    $(function () {
        $("[name=billing_same]").change(function () {
            if ($(this).is(":checked")) {
                $(".hidden").hide();
            } else {
                $(".hidden").show();
            }
        });
    });

}