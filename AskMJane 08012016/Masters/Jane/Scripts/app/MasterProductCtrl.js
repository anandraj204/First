angular.module('AskMJaneAngularApp').controller('MasterProductCtrl', MasterProductCtrl);

MasterProductCtrl.$inject = ['$scope', '$http', 'Upload'];

function MasterProductCtrl($scope, $http, Upload) {

    $scope.WeightOptions = [
        {
            Weight: "Gram",
            Value: 1.0
        }, {
            Weight: "Eighth",
            Value: 3.5
        }, {
            Weight: "Quarter Oz",
            Value: 7.0
        }, {
            Weight: "Half Oz",
            Value: 14.0
        }, {
            Weight: "Ounce",
            Value: 28.0
        }
    ];

    $scope.productModel = {
        "Name": "",
        "Slug": "",
        "LeaflySlug": "",
        "Description": "",
        "CategoryId": 1,
        "IsPricedByWeight": true,
        "NoVariants": true,
        "Price": 35.00,
        "Weight": 3.5
    };

    $scope.loading = true;
    $scope.currentPage = 1;
    $scope.productphoto = null;
    $scope.effects = [];
    $scope.symptoms = [];
    $scope.CurrentDispensary = 0;
    $scope.dispensaries = [];
    $scope.prices = [];
    $scope.dispensaryProducts = [];
    $scope.selectedProduct = 0;
    $scope.productcategories = [];
    $scope.selectedVariants = [];
    $scope.Price = '';
    $scope.Weight = '';
    $scope.filterCategory = 0;
    $scope.variant = {
        'Name': "",
        'Slug': '',
        'LeaflySlug': ''
    };
    $scope.editVariants = [];

    $scope.UploadProductPhoto = function (file, p) {

        if (!file)
            return;

        Upload.upload({
            url: $scope.ApiAddress + "/images/upload",
            method: 'POST',
            sendFieldsAs: 'form',
            file: file
        }).success(function (data, status, headers, config) {

            var photo = {
                url: $scope.ApiAddress + "/images/" + data,
                id: data
            };

            if (typeof (p.photos) == "undefined") {
                p.photos = [];
            }
            else if (p.photos == null) {
                p.photos = [];
            }

            p.photos.push(photo);
        }).error(function (data, status, headers, config) {
            alert("Error uploading photo");
        });
    };

    $scope.DeleteImage = function ($index, p) {
        p.photos.splice($index, 1);
    },

    $scope.edit = function (idx) {
        var the_string = idx + '_collapsed';
        if ($scope[the_string] === undefined) {
            $scope[the_string] = true;
        } else {
            $scope[the_string] = !$scope[the_string];
        }
    }
    $scope.IsCollapsed = function (idx) {
        var the_string = idx + '_collapsed';

        if ($scope[the_string] === undefined) {
            $scope[the_string] = true;
        }
        return $scope[the_string];
    }

    $scope.Collapse = function (idx) {
        var the_string = idx + '_collapsed';

        $scope[the_string] = true;
    }

    $scope.GetProductCategories = function () {
        $http.get($scope.ApiAddress + "/productcategories")
          .then(function (response) {
              if (typeof (response) == "undefined") {

              } else {
                  console.log(response.data);
                  $scope.productcategories = response.data;
              }
          }, function (response) {
              console.log(response);
          });
    };
    $scope.GetProductCategories();

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
    };
    $scope.GetEffects();

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
    };
    $scope.GetSymptoms();

    $scope.updateProduct = function (p, idx) {
        if (this['updateProductForm_' + idx].$valid) {

            var prod = jQuery.extend(true, {}, p);

            prod.effects = prod.selectedEffects;
            prod.symptoms = prod.selectedSymptoms;

            delete prod.selectedEffects;
            delete prod.selectedSymptoms;

            $http.post($scope.ApiAddress + "/products/update", JSON.stringify(prod))
                .then(function (response) {
                    console.log(response);
                    alert("Update Successful");
                    location.reload();
                    return response;
                }, function (response) {
                    console.log(response);
                    alert("One or more errors while updating");
                });

        } else {
            alert("One or more fields are missing");
        }
    };
    $scope.validate = function (idx, name) {

    };

    $scope.invalidate = function () {
        $scope.$apply();
    };

    $scope.removeProduct = function (index) {
        var r = confirm("Are you sure you want to permanently remove this product?");
        if (r) {
            //  console.log($scope.CurrentDispensary);
            $http.post($scope.ApiAddress + "/products/delete?id=" + $scope.dispensaryProducts[index].id)
                .then(function (response, data) {
                    if (typeof (response) == "undefined") {
                    } else {
                        $scope.dispensaryProducts.splice(index, 1);
                    }
                }, function (response) {
                    alert('Unable to remove product');
                });

        }
    }
    
    $scope.initReadMore = function () {
        var $dot5 = $('.product-description').filter(function () {
            return $(this).height() >= 33;
        });
        $dot5.append(' <a class="toggle" href="#"><span class="open">Read more</span><span class="close">Read less</span></a>');
        $dot5.addClass('product-description-height');

        function createDots(dot) {
            dot.dotdotdot({
                after: 'a.toggle'
            });
        }
        function destroyDots(dot) {
            dot.trigger('destroy');
        }
        createDots($dot5);

        $dot5.on(
            'click',
            'a.toggle',
            function (e) {
                var dot = $(e.target.parentElement.parentElement);
                dot.toggleClass('opened');

                if (dot.hasClass('opened')) {
                    destroyDots(dot);
                } else {
                    createDots(dot);
                }
                return false;
            }
        );
    };
    
    $scope.addProduct = function (form, valid) {

        if ($scope.productModel.Slug == "") {
            $scope.productModel.Slug = $scope.productModel.Name;
        }
        $scope.productModel.ProductCategoryId = $scope.productModel.CategoryId;

        $http.post($scope.ApiAddress + "/products", JSON.stringify($scope.productModel)).then(
        function (response) {
            console.log(response);
            alert('Inventiry added successfully');
            window.location.reload();
            return response;
        }, function (errors) {
            alert("One or more errors adding product");
            console.log(errors);
        });
    };

    $scope.loadProducts = function () {
        $http.get($scope.ApiAddress + "/products/list", {
            params: {
                categoryId: $scope.filterCategory,
                page: $scope.currentPage
            }
        }).then(function (response, data) {
            if (typeof (response) == "undefined") {
            } else {

                if (response.data) {
                    for (var j = 0; j < response.data.length; j++) {
                        if (response.data[j].photos && response.data[j].photos.length > 0) {
                            for (var k = 0; k < response.data[j].photos.length; k++) {
                                response.data[j].photos[k].url = $scope.ApiAddress + "/images/" + response.data[j].photos[k].id;
                            }
                        }

                        response.data[j].effectsList = $.map($scope.effects, function (obj) {
                            return $.extend(true, {}, obj);
                        });

                        for (var l = 0; l < response.data[j].effects.length; l++) {
                            for (var m = 0; m < response.data[j].effectsList.length; m++) {
                                if (response.data[j].effectsList[m].id === response.data[j].effects[l].id)
                                    response.data[j].effectsList[m].ticked = true;
                            }
                        }

                        response.data[j].symptomsList = $.map($scope.symptoms, function (obj) {
                            return $.extend(true, {}, obj);
                        });
                        for (var l = 0; l < response.data[j].symptoms.length; l++) {
                            for (var m = 0; m < response.data[j].symptomsList.length; m++) {
                                if (response.data[j].symptomsList[m].id === response.data[j].symptoms[l].id)
                                    response.data[j].symptomsList[m].ticked = true;
                            }
                        }
                    }
                }

                $scope.dispensaryProducts = response.data;
            }
            $scope.loading = false;


            setTimeout(function () {
                $scope.initReadMore();
            }, 0);

        }, function (response) {
            console.log(response);
            $scope.loading = false;
        });
    };

    $scope.loadProducts();

    $scope.initReadMore = function () {
        var $dot5 = $('.product-description').filter(function () {
            return $(this).height() >= 33;
        });
        $dot5.append(' <a class="toggle" href="#"><span class="open">Read more</span><span class="close">Read less</span></a>');
        $dot5.addClass('product-description-height');

        function createDots(dot) {
            dot.dotdotdot({
                after: 'a.toggle'
            });
        }
        function destroyDots(dot) {
            dot.trigger('destroy');
        }
        createDots($dot5);

        $dot5.on(
            'click',
            'a.toggle',
            function (e) {
                var dot = $(e.target.parentElement.parentElement);
                dot.toggleClass('opened');

                if (dot.hasClass('opened')) {
                    destroyDots(dot);
                } else {
                    createDots(dot);
                }
                return false;
            }
        );
    };

}
