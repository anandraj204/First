angular.module('AskMJaneAngularApp').controller('StoreCtrl', StoreCtrl);

StoreCtrl.$inject = ['$scope', '$http'];

function StoreCtrl($scope, $http) {
    $scope.alerts = [];
    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
    $scope.model = {
        CurrentDispensary: null
    }
    $scope.dispensaries = [];
    $scope.selectedDispensary = 0;
    $scope.user = null;
    $scope.productCategories = [];
    $scope.effects = [];
    $scope.dispensaryProducts = [];
    $scope.products = [];
    $scope.symptoms = [];
    $scope.filter = {
        'productCategories': [],
        'symptoms': [],
        'effects': []
    };
    $scope.loading = true;

    $scope.getParameterByName = function (name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }

    $scope.GetProducts = function () {

        var zip;
        if ($scope.user)
             zip = $scope.user.zipcode;

        if ($scope.getParameterByName('zip'))
            zip = $scope.getParameterByName('zip');

        var filter = {
            'productCategories': [],
            'symptoms': [],
            'effects': [],
            'zip': zip
        };

        for (var j = 0; j < $scope.filter.productCategories.length; j++) {
            filter.productCategories[j] = $scope.filter.productCategories[j].id;
        }
        for (var j = 0; j < $scope.filter.symptoms.length; j++) {
            filter.symptoms[j] = $scope.filter.symptoms[j].id;
        }
        for (var j = 0; j < $scope.filter.effects.length; j++) {
            filter.effects[j] = $scope.filter.effects[j].id;
        }

        $http.get($scope.ApiAddress + "/DispensaryProducts/getProducts", {
            params: filter
        }).then(function (response, data) {
            if (typeof (response) == "undefined") {
            } else {
                $scope.products = response.data;
                console.log(response.data);
                for (var i = 0; i < $scope.products.length; i++) {
                    $scope.products[i].selectedQuantity = 0;
                    if ($scope.products[i].masterVariant.photos != null && $scope.products[i].masterVariant.photos.length > 0) {
                        $scope.products[i].photoUrl = $scope.ApiAddress + "/images/" + $scope.products[i].masterVariant.photos[0].id;
                    } else {
                        $scope.products[i].photoUrl = "/Content/images/IMG_0101.jpg";
                    }
                }
            }
            $scope.loading = false;
        }, function (response) {
            console.log(response);
            $scope.loading = false;
        });
    };


    $scope.GetUser = function () {
        var usertoken = Cookies.get("usertoken");
        $http.get($scope.ApiAddress + "/account/getbyusertoken?usertoken=" + usertoken)
          .then(function (response, data) {
              if (typeof (response) == "undefined") {
              } else {
                  $scope.user = response.data;
                  $scope.GetProducts();
                  console.log(response.data);
              }
          }, function (response) {
              console.log(response);
          });
        console.log(usertoken);
    }

    $scope.GetProductCategories = function () {
        $http.get($scope.ApiAddress + "/productcategories")
          .then(function (response) {
              if (typeof (response) == "undefined") {

              } else {
                  console.log(response.data);
                  $scope.productCategories = response.data;
              }
          }, function (response) {
              console.log(response);
          });
    }

    $scope.GetSymptoms = function () {
        $http.get($scope.ApiAddress + "/symptoms")
          .then(function (response) {
              if (typeof (response) == "undefined") {

              } else {
                  console.log(response.data);
                  $scope.symptoms = response.data;
              }
          }, function (response) {
              console.log(response);
          });
    }

    $scope.GetEffects = function () {
        $http.get($scope.ApiAddress + "/effects")
          .then(function (response) {
              if (typeof (response) == "undefined") {

              } else {
                  console.log(response.data);
                  $scope.effects = response.data;
              }
          }, function (response) {
              console.log(response);
          });
    }

    $scope.addToCart = function (product) {
        var name = product.name;
        
        $http.post($scope.ApiAddress + "/cart/addtocart", { 'UserId': $scope.user.id, 'VariantId': product.masterVariant.id, 'VariantPriceId': product.selectedQuantity })
           .then(function (response) {
               if (typeof (response) == "undefined") {
               }
               else {
                   $scope.BadgeCounter();

                   $scope.SmallModal.class = "added";
                   $scope.SmallModal.header = "Item Added";
                   $scope.SmallModal.text = name + " added to cart";

                   $scope.CloseSmallModalWithDelay();
               }
           }, function (response) {
               console.log(response);
           });
    }

    $scope.displayPrice = function (price, isweight) {
        var s = "$" + price.price + " / " + price.displayWeight;
        return s;
    }

    $scope.GetUser();
    $scope.GetProductCategories();
    $scope.GetEffects();
    $scope.GetSymptoms();
}