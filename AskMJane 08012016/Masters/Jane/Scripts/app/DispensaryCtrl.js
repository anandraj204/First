angular.module('AskMJaneAngularApp').controller('DispensaryCtrl', DispensaryCtrl);

DispensaryCtrl.$inject = ['$scope', '$http', '$geolocation', '$cookies', '$modal'];

function DispensaryCtrl($scope, $http, $geolocation, $cookies, $modal) {
    $scope.hoursModel = {
        Day: "0",
        Start: "0 9.0",
        Stop: "0 17.0"
    }
    $scope.model = {
        filterState: '',
        filterName: ''
    };

    $scope.grid = null;
    $scope.dispensaries = [];

    $scope.states = [
        { value: 'AL', name: 'Alabama' },
        { value: 'AK', name: 'Alaska' },
        { value: 'AZ', name: 'Arizona' },
        { value: 'AR', name: 'Arkansas' },
        { value: 'CA', name: 'California' },
        { value: 'CO', name: 'Colorado' },
        { value: 'CT', name: 'Connecticut' },
        { value: 'DE', name: 'Delaware' },
        { value: 'FL', name: 'Florida' },
        { value: 'GA', name: 'Georgia' },
        { value: 'HI', name: 'Hawaii' },
        { value: 'ID', name: 'Idaho' },
        { value: 'IL', name: 'Illinois' },
        { value: 'IN', name: 'Indiana' },
        { value: 'IA', name: 'Iowa' },
        { value: 'KS', name: 'Kansas' },
        { value: 'KY', name: 'Kentucky' },
        { value: 'LA', name: 'Louisiana' },
        { value: 'ME', name: 'Maine' },
        { value: 'MD', name: 'Maryland' },
        { value: 'MA', name: 'Massachusetts' },
        { value: 'MI', name: 'Michigan' },
        { value: 'MN', name: 'Minnesota' },
        { value: 'MS', name: 'Mississippi' },
        { value: 'MO', name: 'Missouri' },
        { value: 'MT', name: 'Montana' },
        { value: 'NE', name: 'Nebraska' },
        { value: 'NV', name: 'Nevada' },
        { value: 'NH', name: 'New Hampshire' },
        { value: 'NJ', name: 'New Jersey' },
        { value: 'NM', name: 'New Mexico' },
        { value: 'NY', name: 'New York' },
        { value: 'NC', name: 'North Carolina' },
        { value: 'ND', name: 'North Dakota' },
        { value: 'OH', name: 'Ohio' },
        { value: 'OK', name: 'Oklahoma' },
        { value: 'OR', name: 'Oregon' },
        { value: 'PA', name: 'Pennsylvania' },
        { value: 'RI', name: 'Rhode Island' },
        { value: 'SC', name: 'South Carolina' },
        { value: 'SD', name: 'South Dakota' },
        { value: 'TN', name: 'Tennessee' },
        { value: 'TX', name: 'Texas' },
        { value: 'UT', name: 'Utah' },
        { value: 'VT', name: 'Vermont' },
        { value: 'VA', name: 'Virginia' },
        { value: 'WA', name: 'Washington' },
        { value: 'WV', name: 'West Virginia' },
        { value: 'WI', name: 'Wisconsin' },
        { value: 'WY', name: 'Wyoming' }
    ];

    $scope.updateDispensaries = function () {
        $scope.grid.ajax.reload();
    }

    $scope.initDispensaries = function () {
        var token = $cookies.get("access_token");
        $scope.grid = $('#dispensaryGrid').on('draw.dt', function () {
        }).DataTable({
            ajax: {},
            "processing": true,
            "serverSide": true,
            "bServerSide": true,
            "bAutoWidth": false,
            "bFilter": false,
            "sAjaxSource": $scope.ApiAddress + "/dispensaries/get",
            "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
                aoData.push({ name: "state", value: $scope.model.filterState });
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
                                        $('#editDispensary_' + id).click(function () {
                                            $scope.edit(id);
                                        });
                                        $('#deleteDispensary_' + id).click(function () {
                                            $scope.delete(id);
                                        });
                                        $('#hideDispensary_' + id).click(function () {
                                            $scope.hideDispensary(id);
                                        });
                                        $('#showDispensary_' + id).click(function () {
                                            $scope.showDispensary(id);
                                        });
                                    })(id);
                                }
                            }
                        }, 100);

                        $scope.dispensaries = items;
                        fnCallback(data);
                    },
                    beforeSend: function (request) {
                        request.setRequestHeader('Authorization', 'Bearer ' + token);
                    }
                });
            },
            "aoColumns": [
                { "width": "35%", "sName": "Name", "mData": "name" },
                {
                    "width": "10%", "sName": "State", "mData": function (item) {

                        for (var i = 0; i < $scope.states.length; i++) {
                            if ($scope.states[i].value == item.address.state) {
                                return $scope.states[i].name;
                            }
                        }
                        return item.address.state;
                    }
                },
                { "width": "10%", "sName": "Type", "mData": "type" },
                { "width": "25%", "sName": "Email", "mData": "emailAddress" },
                { "width": "15%", "sName": "Phone", "mData": "phoneNumber" },
                {
                    "width": "5%", "orderable": false, "mData": function (item) {
                        var btns = '<div class="dropdown">\
                                                    <button id="dispensaryActionsButton' + item.id + '" class="btn btn-info" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">\
                                                        Actions\
                                                        <span class="caret"></span>\
                                                    </button>\
                                                    <ul class="dropdown-menu" aria-labelledby="dispensaryActionsButton' + item.id + '">';

                        btns += '<li><a id="editDispensary_' + item.id + '">Edit</a></li>';

                        if (!item.isHidden)
                            btns += '<li><a id="hideDispensary_' + item.id + '">Hide</a></li>';
                        else
                            btns += '<li><a id="showDispensary_' + item.id + '">Show</a></li>';

                        btns += '<li><a id="deleteDispensary_' + item.id + '">Delete</a></li>\
                                                    </ul>\
                                            </div>';

                        return btns;
                    }
                }
            ]
        }
        );
    };
    $(document).ready(function() {
        $scope.initDispensaries();
    });

    $scope.edit = function (id) {

        var dispensary;
        for (var i = 0; i < $scope.dispensaries.length; i++) {
            if ($scope.dispensaries[i].id === id) {
                dispensary = $scope.dispensaries[i];

                console.log(dispensary);

                if (dispensary.address == null)
                {
                    dispensary.address = $scope.dispensaryModel.address;
                }
            }
        }

        var modalInstance = $modal.open({
            templateUrl: 'editDispensary.html',
            controller: "EditDispensaryModalCtrl",
            size: "lg",
            resolve: {
                params: function () {
                    return {
                        dispensary: dispensary
                    }

                }
            }
        });
    }


    $scope.addNewDispensary = function () {
        var modalInstance = $modal.open({
            templateUrl: 'createDispensary.html',
            controller: "EditDispensaryModalCtrl",
            size: "lg",
            resolve: {
                params: function () {
                    return {
                        dispensary: $scope.dispensaryModel
                    }
                }
            }
        });
    },

    $scope.delete = function (id) {
        bootbox.confirm("Are you sure you want to permanently remove this dispensary?", function (result) {
            if (result) {
                var url = $scope.ApiAddress + '/dispensaries/delete?id=' + id;
                $http.get(url).
                    then(function (response) {
                        for (var i = 0; i < $scope.dispensaries.length; i++) {
                            if ($scope.dispensaries[i].id === id) {
                                $scope.dispensaries.splice(i, 1);
                                break;
                            }
                        }

                        $scope.updateDispensaries();
                        bootbox.alert('Dispensary deleted successfully');
                    }, function (response) {
                        bootbox.alert('Error occured while deleting Dispensary');
                    });
            }
        });
    }

    $scope.hideDispensary = function (id) {
        var url = $scope.ApiAddress + '/dispensaries/hideDispensary?id=' + id;
        $http.get(url).
            then(function (response) {
                $scope.updateDispensaries();
                bootbox.alert('Dispensary saved successfully');

            }, function (response) {
                bootbox.alert('Error occured while hiding Dispensary');
            });
    }

    $scope.showDispensary = function (id) {
        var url = $scope.ApiAddress + '/dispensaries/showDispensary?id=' + id;
        $http.get(url).
            then(function (response) {
                $scope.updateDispensaries();
                bootbox.alert('Dispensary saved successfully');
            }, function (response) {
                bootbox.alert('Error occured while hiding Dispensary');
            });
    }

    $scope.invalidDeliveryAddress = true;

    $scope.dispensaryModel = {
        name: "",
        hoursAndInfo: "",
        phoneNumber: "",
        emailAddress: "",
        description: "",
        photoUrl: "",
        hasDelivery: false,
        hasPickup: false,
        hasScheduledDelivery: false,
        hasStorefront: false,
        isCaregiver: false,
        isPrivate: false,
        slug: "",
        photos: null,
        hoursOfOperation: [],
        deliveryZipCodes: "",
        scheduledDeliveryZipCodes: "",
        approvalZipCodes: "",
        type: "",
        address: {

            name: "",
            address1: null,
            address2: null,
            city: null,
            state: null,
            zip: null,
            country: null,
            phoneNumber: this.phoneNumber,
            latitude: null,
            longitude: null,
            formattedAddress: ""
        }
    };

};


app.controller('EditDispensaryModalCtrl', EditDispensaryModalCtrl);

EditDispensaryModalCtrl.$inject = ['$scope', '$http', '$modalInstance', '$filter', 'Upload', 'params', 'ApiAddress'];

function EditDispensaryModalCtrl($scope, $http, $modalInstance, $filter, Upload, params, ApiAddress) {
    $scope.ApiAddress = ApiAddress;
    $scope.dispensary = params.dispensary;

    $scope.autocompleteAddresses = [];

    $scope.initAddress = function () {
        var options = {
            types: ['geocode'],
            componentRestrictions: { country: "us" }
        };
        var inputFrom = document.getElementById('InputAddressDispensary');
        var autocompleteFrom = new google.maps.places.Autocomplete(inputFrom, options);
        google.maps.event.addListener(autocompleteFrom, 'place_changed', function () {
            var place = autocompleteFrom.getPlace();
            var components = place.address_components;
            for (var i = 0; i < components.length; i++) {
                for (var j = 0; j < components[i].types.length; j++) {
                    if ("street_number" == components[i].types[j]) {
                        $scope.dispensary.address.Address1 = components[i].long_name;
                    }
                    if ("route" == components[i].types[j]) {
                        $scope.dispensary.address.Address1 = $scope.dispensary.Address.Address1 + " " + components[i].long_name;
                    }
                    if ("locality" == components[i].types[j]) {
                        $scope.dispensary.address.City = components[i].long_name;
                    }
                    if ("administrative_area_level_1" == components[i].types[j]) {
                        $scope.dispensary.address.State = components[i].short_name;
                    }
                    if ("country" == components[i].types[j]) {
                        $scope.dispensary.address.Country = components[i].short_name;
                    }
                    if ("postal_code" == components[i].types[j]) {
                        $scope.dispensary.address.Zip = components[i].short_name;
                    }
                }
            }
            $scope.dispensary.address.latitude = place.geometry.location.lat();
            $scope.dispensary.address.longitude = place.geometry.location.lng();
            $scope.dispensary.address.formattedAddress = place.formatted_address;
            $scope.invalidDeliveryAddress = false;
            $scope.showInvalidAddress = false;

            $scope.$apply();

        });
    };
    setTimeout($scope.initAddress, 100);

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };


    $scope.removeHoursEdit = function (d, i) {
        d.hoursOfOperation.splice(i, 1);

    }
    $scope.addHoursEdit = function (d) {
        d.hoursOfOperation.push({ "day": d.day, "start": d.start, "stop": d.stop });

    }

    $scope.removeHours = function (i) {
        $scope.dispensary.HoursOfOperation.splice(i, 1);
    };
    $scope.addHours = function () {
        $scope.dispensary.HoursOfOperation.push({ "Day": $scope.hoursModel.Day, "Start": $scope.hoursModel.Start, "Stop": $scope.hoursModel.Stop });
    };
    $scope.addDispensary = function (form, valid) {
        if (valid) {
            $http.post($scope.ApiAddress + "/dispensaries", JSON.stringify($scope.dispensary))
                .then(function (response) {
                    bootbox.alert('Dispensary added successfully');
                    location.reload();
                    return response;
                }, function (response) {
                    console.log(response);
                });
        }
    };

    $scope.updateDispensary = function (form, valid) {
        if (valid) {
            $http.post($scope.ApiAddress + "/dispensaries/update", JSON.stringify($scope.dispensary))
                .then(function (response) {
                    bootbox.alert("Update Successful");
                    location.reload();
                    return response;
                }, function (response) {
                    console.log(response);
                    bootbox.alert("One or more errors while updating");
                });
        }
    }


    $scope.Days = { "0": "Mon", "1": "Tues", "2": "Wed", "3": "Thurs", "4": "Fri", "5": "Sat", "6": "Sun" };
    $scope.Starts = {
        "0 0.0": "12:00 am (midnight)",

        "0 0.5": "12:30 am",

        "0 1.0": "1:00 am",

        "0 1.5": "1:30 am",

        "0 2.0": "2:00 am",

        "0 2.5": "2:30 am",

        "0 3.0": "3:00 am",

        "0 3.5": "3:30 am",

        "0 4.0": "4:00 am",

        "0 4.5": "4:30 am",

        "0 5.0": "5:00 am",

        "0 5.5": "5:30 am",

        "0 6.0": "6:00 am",

        "0 6.5": "6:30 am",

        "0 7.0": "7:00 am",

        "0 7.5": "7:30 am",

        "0 8.0": "8:00 am",

        "0 8.5": "8:30 am",

        "0 9.0": "9:00 am",

        "0 9.5": "9:30 am",

        "0 10.0": "10:00 am",

        "0 10.5": "10:30 am",

        "0 11.0": "11:00 am",

        "0 11.5": "11:30 am",

        "0 12.0": "12:00 pm (noon)",

        "0 12.5": "12:30 pm ",

        "0 13.0": "1:00 pm",

        "0 13.5": "1:30 pm",

        "0 14.0": "2:00 pm",

        "0 14.5": "2:30 pm",

        "0 15.0": "3:00 pm",

        "0 15.5": "3:30 pm",

        "0 16.0": "4:00 pm",

        "0 16.5": "4:30 pm",

        "0 17.0": "5:00 pm",

        "0 17.5": "5:30 pm",

        "0 18.0": "6:00 pm",

        "0 18.5": "6:30 pm",

        "0 19.0": "7:00 pm",

        "0 19.5": "7:30 pm",

        "0 20.0": "8:00 pm",

        "0 20.5": "8:30 pm",

        "0 21.0": "9:00 pm",

        "0 21.5": "9:30 pm",

        "0 22.0": "10:00 pm",

        "0 22.5": "10:30 pm",

        "0 23.0": "11:00 pm",
        "0 23.5": "11:30 pm"
    };
    $scope.Stops = {

        "0 0.5": "12:30 am",

        "0 1.0": "1:00 am",

        "0 1.5": "1:30 am",

        "0 2.0": "2:00 am",

        "0 2.5": "2:30 am",

        "0 3.0": "3:00 am",

        "0 3.5": "3:30 am",

        "0 4.0": "4:00 am",

        "0 4.5": "4:30 am",

        "0 5.0": "5:00 am",

        "0 5.5": "5:30 am",

        "0 6.0": "6:00 am",

        "0 6.5": "6:30 am",

        "0 7.0": "7:00 am",

        "0 7.5": "7:30 am",

        "0 8.0": "8:00 am",

        "0 8.5": "8:30 am",

        "0 9.0": "9:00 am",

        "0 9.5": "9:30 am",

        "0 10.0": "10:00 am",

        "0 10.5": "10:30 am",

        "0 11.0": "11:00 am",

        "0 11.5": "11:30 am",

        "0 12.0": "12:00 pm (noon)",

        "0 12.5": "12:30 pm ",

        "0 13.0": "1:00 pm",

        "0 13.5": "1:30 pm",

        "0 14.0": "2:00 pm",

        "0 14.5": "2:30 pm",

        "0 15.0": "3:00 pm",

        "0 15.5": "3:30 pm",

        "0 16.0": "4:00 pm",

        "0 16.5": "4:30 pm",

        "0 17.0": "5:00 pm",

        "0 17.5": "5:30 pm",

        "0 18.0": "6:00 pm",

        "0 18.5": "6:30 pm",

        "0 19.0": "7:00 pm",

        "0 19.5": "7:30 pm",

        "0 20.0": "8:00 pm",

        "0 20.5": "8:30 pm",

        "0 21.0": "9:00 pm",

        "0 21.5": "9:30 pm",

        "0 22.0": "10:00 pm",

        "0 22.5": "10:30 pm",

        "0 23.0": "11:00 pm",
        "0 23.5": "11:30 pm"
    };

};