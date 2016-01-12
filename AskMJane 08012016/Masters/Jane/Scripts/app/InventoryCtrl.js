angular.module('AskMJaneAngularApp').controller('InventoryCtrl', InventoryCtrl);

InventoryCtrl.$inject = ['$scope', '$http', 'Upload', '$cookies', '$modal'];

function InventoryCtrl($scope, $http, Upload, $cookies, $modal) {
    $scope.model = {
        CurrentDispensary: 0,
        filterCategory: 0,
        filterName: ''
    };

    $scope.productModel = {
        "Name": "",
        "Slug": "",
        "LeaflySlug": "",
        "Description": "",
        "IsDiscounted": false,
        "IsAvailable": true,
        "IsPopular": false,
        "DispensaryId": 0,
        "DispensaryProductVariants": [],
        "ProductId": 0,
        "CategoryId": 1,
        "IsPricedByWeight": true,
        "NoVariants": true,
        "masteradded": false,
        "Price": 35.00,
        "Weight": 3.5
    };

    $scope.grid = null;
    $scope.productphoto = null;
    $scope.effects = [];
    $scope.symptoms = [];
    $scope.CurrentDispensary = 0;
    $scope.dispensaries = [];
    $scope.prices = [];
    $scope.products = [{ "id": 0, "name": "Create New" }];
    $scope.dispensaryProducts = [];
    $scope.selectedProduct = 0;
    $scope.productcategories = [];
    $scope.selectedVariants = [];
    $scope.Price = '';
    $scope.Weight = '';
    $scope.variant = {
        'Name': "",
        'Slug': '',
        'LeaflySlug': '',
        'IsMasterVariant': false
    };
    $scope.editVariants = [];

    $scope.Types = ['Medicinal', 'Recreational'];
    $scope.addNewInventory = function () {
        var modalInstance = $modal.open({
            templateUrl: 'createInventory.html',
            controller: "EditInventoryModalCtrl",
            size: "lg",
            resolve: {
                params: function () {
                    return {
                        inventory: $scope.productModel,
                        categories: $scope.productcategories,
                        dispensaries: $scope.dispensaries
                    }

                }
            }
        });
    },

        $scope.edit = function (id) {

            var inventory;
            for (var i = 0; i < $scope.dispensaryProducts.length; i++) {
                if ($scope.dispensaryProducts[i].id === id)
                    inventory = $scope.dispensaryProducts[i];
            }

            if (inventory.masterVariant && inventory.masterVariant.photos && inventory.masterVariant.photos.length > 0) {
                for (var k = 0; k < inventory.masterVariant.photos.length; k++) {
                    inventory.masterVariant.photos[k].url = $scope.ApiAddress + "/images/" + inventory.masterVariant.photos[k].id;
                }
            }

            inventory.effectsList = $.map($scope.effects, function (obj) {
                return $.extend(true, {}, obj);
            });

            for (var l = 0; l < inventory.effects.length; l++) {
                for (var m = 0; m < inventory.effectsList.length; m++) {
                    if (inventory.effectsList[m].id === inventory.effects[l].id)
                        inventory.effectsList[m].ticked = true;
                }
            }

            inventory.symptomsList = $.map($scope.symptoms, function (obj) {
                return $.extend(true, {}, obj);
            });
            for (var l = 0; l < inventory.symptoms.length; l++) {
                for (var m = 0; m < inventory.symptomsList.length; m++) {
                    if (inventory.symptomsList[m].id === inventory.symptoms[l].id)
                        inventory.symptomsList[m].ticked = true;
                }
            }

            var modalInstance = $modal.open({
                templateUrl: 'editInventory.html',
                controller: "EditInventoryModalCtrl",
                size: "lg",
                resolve: {
                    params: function () {
                        return {
                            inventory: inventory,
                            categories: $scope.productcategories,
                            dispensaries: $scope.dispensaries
                        }

                    }
                }
            });
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
                  $scope.effects = response.data;
                  $scope.productModel.effectsList = $.map(response.data, function (obj) {
                      return $.extend(true, {}, obj);
                  });
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
                  $scope.symptoms = response.data;
                  $scope.productModel.symptomsList = $.map(response.data, function (obj) {
                      return $.extend(true, {}, obj);
                  });
              }
          }, function (response) {
              console.log(response);
          });
    };
    $scope.GetSymptoms();
    $scope.invalidate = function () {
        $scope.$apply();
    };

    $scope.removeProduct = function (id) {
        bootbox.confirm("Are you sure you want to permanently remove this product?", function (result) {
            if (result) {
                $http.post($scope.ApiAddress + "/dispensaryproducts/delete?id=" + id)
                    .then(function (response, data) {
                        if (typeof (response) == "undefined") {
                        } else {
                            $scope.grid.ajax.reload();
                        }
                    }, function (response) {
                        bootbox.alert('Unable to remove product');
                    });
            }
        });
    }

    $scope.updateCurrentDispensary = function () {

        $scope.grid.ajax.reload();
    }

    $scope.initCurrentDispensary = function () {
        var token = $cookies.get("access_token");
        $scope.grid = $('#inventoryGrid').on('draw.dt', function () {
            setTimeout(function () {
                $scope.$apply();
                $scope.initReadMore();
            }, 0);
        }).DataTable({
            ajax: {},
            "processing": true,
            "serverSide": true,
            "bServerSide": true,
            "bAutoWidth": false,
            "bFilter": false,
            "sAjaxSource": $scope.ApiAddress + "/dispensaryproducts/getByDispensary",
            "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
                aoData.push({ name: "dispensaryId", value: $scope.model.CurrentDispensary });
                aoData.push({ name: "categoryId", value: $scope.model.filterCategory });
                aoData.push({ name: "name", value: $scope.model.filterName });

                oSettings.jqXHR = $.ajax({
                    "dataType": 'json',
                    "type": "POST",
                    "url": sSource,
                    "data": aoData,
                    "success": function (data) {
                        var items = data.aaData;
                        setTimeout(function () {

                            if (items) {
                                for (var j = 0; j < items.length; j++) {
                                    var id = items[j].id;

                                    (function (id) {
                                        $('#editInventory_' + id).click(function () {
                                            $scope.edit(id);
                                        });
                                        $('#deleteInventory_' + id).click(function () {
                                            $scope.removeProduct(id);
                                        });
                                    })(id);
                                }
                            }

                            $scope.editVariants = [];
                            for (var i = 0; i < $scope.dispensaryProducts.length; i++) {
                                $scope.editVariants.push({
                                    'Name': "",
                                    'Slug': '',
                                    'LeaflySlug': ''
                                });
                            }
                        }, 100);

                        $scope.dispensaryProducts = items;
                        fnCallback(data);
                    },
                    beforeSend: function (request) {
                        request.setRequestHeader('Authorization', 'Bearer ' + token);
                    }
                });
            },
            "aoColumns": [
                { "width": "20%", "sName": "Name", "mData": "name" },
                { "width": "10%", "sName": "Category", "mData": "productCategory.name" },
                { "width": "65%", "sName": "Description", "mData": function (item) { return '<p class="product-description">' + item.description + '</p>'; } },
                {
                    "width": "5%", "orderable": false, "mData": function (item) {
                        return '<div class="dropdown"><button id="inventoryActionsButton' + item.id + '" class="btn btn-info dropdown-toggle" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Actions<span class="caret"></span></button><ul class="dropdown-menu" aria-labelledby="inventoryActionsButton' + item.id + '"><li><a id="deleteInventory_' + item.id + '">Remove</a></li><li><a id="editInventory_' + item.id + '">Edit</a></li></ul></div>';
                    }
                }
            ]
        }
        );
    };

    $scope.initReadMore = function () {
        var $dot5 = $('p.product-description').filter(function () {
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



    $scope.getDispensaries = function () {
        $http.get($scope.ApiAddress + "/dispensaries/getShortDispensaries").
            then(function (response) {
                $scope.dispensaries = response.data;
                if ($scope.dispensaries.length > 0) {
                    $scope.model.CurrentDispensary = $scope.dispensaries[0].id;
                    $scope.initCurrentDispensary();
                }

                $('a[tab-heading-transclude]').click(function (e) {
                    $scope.initReadMore();
                });
            }, function (response) {
            });
    }
    $scope.getDispensaries();
}


app.controller('EditInventoryModalCtrl', EditInventoryModalCtrl);

EditInventoryModalCtrl.$inject = ['$scope', '$http', '$modalInstance', '$filter', 'Upload', 'params', 'ApiAddress'];

function EditInventoryModalCtrl($scope, $http, $modalInstance, $filter, Upload, params, ApiAddress) {
    $scope.inventory = params.inventory;
    $scope.productcategories = params.categories;
    $scope.ApiAddress = ApiAddress;
    $scope.dispensaries = params.dispensaries;

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


    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

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

            if (typeof (p.masterVariant) == "undefined") {
                p.masterVariant = { photos: [] };
            }
            else if (p.masterVariant.photos == null) {
                p.masterVariant.photos = [];
            }

            p.masterVariant.photos.push(photo);
        }).error(function (data, status, headers, config) {
            bootbox.alert("Error uploading photo");
        });
    };
    $scope.UpdateProductPhotos = function (variant, photos, doneCallback) {
        var data = {
            Id: variant.id,
            Photos: photos
        };
        $http.post($scope.ApiAddress + "/dispensaryproductvariants/updatephotos", data)
            .then(function (response) {
                if (doneCallback)
                    doneCallback();
            }, function (repsonse) {
                bootbox.alert("upload failed");
            });
    }

    $scope.DeleteImage = function ($index, p) {
        p.masterVariant.photos.splice($index, 1);
    },

    $scope.GetProducts = function () {

        var url = $scope.ApiAddress + "/products";
        if ($scope.inventory && $scope.inventory.CategoryId) {
            url += "?categoryId=" + $scope.inventory.CategoryId;
        }

        $http.get(url)
                 .then(function (response, data) {
                     if (typeof (response) == "undefined") {

                     } else {
                         $scope.products = response.data;
                         $scope.products.push({ "id": 0, "name": "Create New" });
                     }
                 }, function (response) {
                     console.log(response);
                 });
    };
    $scope.GetProducts();


    $scope.updateProduct = function (p) {
        if (this['updateProductForm_'].$valid) {

            var prod = jQuery.extend(true, {}, p);

            for (var i = 0; i < prod.dispensaryProductVariants.length; i++) {
                if (prod.dispensaryProductVariants[i].isMasterVariant) {
                    prod.dispensaryProductVariants[i].photos = prod.masterVariant.photos;
                    prod.dispensaryProductVariants[i].variantPricing = prod.masterVariant.variantPricing;
                }
            }

            prod.masterVariant = null;
            prod.effects = prod.selectedEffects;
            prod.symptoms = prod.selectedSymptoms;

            delete prod.selectedEffects;
            delete prod.selectedSymptoms;

            $http.post($scope.ApiAddress + "/dispensaryProducts/update", JSON.stringify(prod))
                .then(function (response) {
                    $scope.cancel();
                    bootbox.alert("Update Successful");
                    location.reload();
                    return response;
                }, function (response) {
                    console.log(response);
                    $scope.cancel();
                    bootbox.alert("One or more errors while updating");
                });

        } else {
            bootbox.alert("One or more fields are missing");
        }
    };
    $scope.validate = function (idx, name) {

    };
    $scope.addPricing = function () {
        if ($scope.inventory.Price == "" || $scope.inventory.Price === undefined || $scope.inventory.Weight == "" || $scope.inventory.Weight === undefined) {
            return;
        }
        $scope.prices.push({ 'Price': $scope.inventory.Price, 'Weight': $scope.inventory.Weight });
        $scope.inventory.Price = '';
        $scope.inventory.Weight = '';
    };
    $scope.addPricingEdit = function (p) {
        if (p.masterVariant.addprice == "" || p.masterVariant.addprice === undefined || p.masterVariant.weight == "" || p.masterVariant.weight === undefined) {
            return;
        }
        if (p.masterVariant.variantPricing == null) {
            p.masterVariant.variantPricing = [];
        }
        p.masterVariant.variantPricing.push({ 'price': p.masterVariant.addprice, 'weight': p.masterVariant.weight });

        p.masterVariant.addprice = '';
        p.masterVariant.weight = '';
    };
    $scope.removePrice = function (index) {
        $scope.prices.splice(index, 1);
    };
    $scope.removePriceEdit = function (p, index) {
        p.masterVariant.variantPricing.splice(index, 1);
    };

    $scope.addVariantEdit = function (idx) {
        if ($scope.editVariants[idx].name == "" || $scope.editVariants[idx].name === undefined) {
            return;
        }
        $scope.dispensaryProducts[idx].masterVariant.push($scope.editVariants[idx]);
        $scope.editVariants[idx] = {
            'name': "",
            'slug': '',
            'leaflySlug': ''
        };
    };
    $scope.removeVariantEdit = function (p, idx) {
        p.masterVariant.splice(idx, 1);
    };
    $scope.addVariant = function () {
        if ($scope.variant.Name == "" || $scope.variant.Name === undefined) {
            return;
        }
        $scope.inventory.DispensaryProductVariants.push($scope.variant);
        $scope.variant = {
            'Name': "",
            'Slug': '',
            'LeaflySlug': ''
        };
        $scope.selectedVariants.push(false);
    };
    $scope.removeVariant = function (index) {
        $scope.inventory.DispensaryProductVariants.splice(index, 1);
        $scope.selectedVariants.splice(index, 1);
    };

    $scope.addProduct = function (form, valid) {
        if (this['createProductForm_'].$valid) {

            if (!$scope.inventory.DispensaryId)
                return;

            if ($scope.inventory.Slug == "") {
                $scope.inventory.Slug = $scope.inventory.Name;
            }
            if (!$scope.inventory.masteradded) {
                $scope.inventory.DispensaryProductVariants.push({
                    'Name': $scope.inventory.name,
                    'Slug': $scope.inventory.slug || $scope.inventory.name,
                    'IsPricedByWeight': $scope.inventory.isPricedByWeight,
                    'VariantPricing': $scope.inventory.masterVariant.variantPricing,
                    "IsMasterVariant": true,
                    'Photos': $scope.inventory.masterVariant ? $scope.inventory.masterVariant.photos : []
                });

                $scope.inventory.masterVariant = null;
                $scope.inventory.masteradded = true;
            }

            $http.post($scope.ApiAddress + "/dispensaryProducts/create", JSON.stringify($scope.inventory)).then(
                function (response) {
                    console.log(response);
                    bootbox.alert('Inventiry added successfully');
                    window.location.reload();
                    return response;
                }, function (errors) {
                    bootbox.alert("One or more errors adding product");
                    console.log(errors);
                });
        }
    };
    $scope.populateDispensaryProductForm = function () {
        var result = $.grep($scope.products, function (e) { return e.id == $scope.inventory.ProductId; });
        if (result.length == 0) {
            // not found
        } else if (result.length == 1) {
            $http.get($scope.ApiAddress + "/products/" + result[0].id).
                then(function (response) {
                    $scope.inventory.name = response.data.name;
                    $scope.inventory.slug = response.data.slug;
                    $scope.inventory.youTubeVideoUrl = response.data.youTubeVideoUrl;
                    $scope.inventory.description = response.data.description;
                    $scope.inventory.productCategoryId = response.data.productCategoryId;

                    for (var l = 0; l < response.data.effects.length; l++) {
                        for (var m = 0; m < $scope.inventory.effectsList.length; m++) {
                            if ($scope.inventory.effectsList[m].id === response.data.effects[l].id)
                                $scope.inventory.effectsList[m].ticked = true;
                        }
                    }

                    for (var l = 0; l < response.data.symptoms.length; l++) {
                        for (var m = 0; m < $scope.inventory.symptomsList.length; m++) {
                            if ($scope.inventory.symptomsList[m].id === response.data.symptoms[l].id)
                                $scope.inventory.symptomsList[m].ticked = true;
                        }
                    }

                    if (typeof ($scope.inventory.masterVariant) == "undefined") {
                        $scope.inventory.masterVariant = { photos: response.data.photos };
                    }
                    else if ($scope.inventory.masterVariant.photos == null) {
                        $scope.inventory.masterVariant.photos = response.data.photos;
                    }

                    for (var k = 0; k < $scope.inventory.masterVariant.photos.length; k++) {
                        $scope.inventory.masterVariant.photos[k].url = $scope.ApiAddress + "/images/" + $scope.inventory.masterVariant.photos[k].id;
                    }
                });
        } else {
            // multiple items found
        }
    }

}