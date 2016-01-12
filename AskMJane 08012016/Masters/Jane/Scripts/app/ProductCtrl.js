angular
    .module("AskMJaneAngularApp")
    .controller("ProductCtrl", ProductCtrl)
        .directive('onLastRepeat', function () {
            return function (scope, element, attrs) {
                if (scope.$last)
                    setTimeout(function () {
                        scope.InitGallery();
                    }, 1);
            };
        });

ProductCtrl.$inject = ["$scope", "$http", "$sce"];

function ProductCtrl($scope, $http, $sce) {

    $scope.init = function (prod) {

        if (prod && prod.MasterVariant && prod.MasterVariant && prod.MasterVariant.Photos) {
            for (var i = 0; i < prod.MasterVariant.Photos.length; i++) {
                prod.MasterVariant.Photos[i].Url = $scope.ApiAddress + "/images/" + prod.MasterVariant.Photos[i].Id;
            }
        }
        prod.SelectedQuantity = 0;

        $scope.product = prod;
    };

    $scope.trustSrc = function (src) {
     return $sce.trustAsResourceUrl(src);
    }

    $scope.InitGallery = function() {
        $('.fotorama').fotorama();
    }

    $scope.displayPrice = function (price, isweight) {
        var s = "$" + price.Price + " / " + price.DisplayWeight;
        return s;
    }

    $scope.addToCart = function (product) {
        var name = product.Name;

        $http.post($scope.ApiAddress + "/cart/addtocart", { 'UserId': $scope.user.id, 'VariantId': product.MasterVariant.Id, 'VariantPriceId': product.SelectedQuantity })
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
}