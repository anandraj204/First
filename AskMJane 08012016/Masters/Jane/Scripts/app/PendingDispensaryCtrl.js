angular.module('AskMJaneAngularApp').controller('PendingDispensaryCtrl', PendingDispensaryCtrl);

PendingDispensaryCtrl.$inject = ['$window', '$scope', '$http', '$modal', 'Upload'];


function PendingDispensaryCtrl($window, $scope, $http, $modal, Upload) {
    $scope.model = {
        Id: 0,
        Name: '',
        PhoneNumber: '',
        Email: '',
        Type: '',
        Address: {
            Address1: '',
            Address2: '',
            City: '',
            State: '',
            Country: ''
        },
        Website: '',
        Password: '',
        ConfirmPassword: '',
        AddressId: null,
        driversLicenseImageUrl: '',
        recommendationImageUrl: '',
        IdNumber: '',
        ExperationDate: null
    };

    $scope.rxPhoto = null;
    $scope.idPhoto = null;

    $scope.pendingDispensaries = null;
    $scope.alerts = null;
    $scope.dispensary = null;
    $scope.username = null;
    $scope.password = null;
    $scope.hideInfo = true;
    $scope.hidePassword = false;
    $scope.idupload = null;
    $scope.rxupload = null;
    $scope.patientdoctype = 'rx';
    $scope.policy = 'ewogICJleHBpcmF0aW9uIjogIjIwMjAtMDEtMDFUMDA6MDA6MDBaIiwKICAiY29uZGl0aW9ucyI6IFsKICAgIHsiYnVja2V0IjogImFza21qYW5lIn0sCiAgICBbInN0YXJ0cy13aXRoIiwgIiRrZXkiLCAiIl0sCiAgICB7ImFjbCI6ICJwcml2YXRlIn0sCiAgICBbInN0YXJ0cy13aXRoIiwgIiRDb250ZW50LVR5cGUiLCAiIl0sCiAgICBbInN0YXJ0cy13aXRoIiwgIiRmaWxlbmFtZSIsICIiXSwKICAgIFsiY29udGVudC1sZW5ndGgtcmFuZ2UiLCAwLCA1MjQyODgwMDBdCiAgXQp9';
    $scope.signature = 'fWI1T0ypTbk35wwxlzzqeWRkdA8=';
    $scope.loading = false;
    $scope.dispensaryModel = {
        Name: "",
        HoursAndInfo: "",
        PhoneNumber: "",
        EmailAddress: "",
        Description: "",
        PhotoUrl: "",
        HasDelivery: false,
        HasPickup: false,
        HasScheduledDelivery: false,
        HasStorefront: false,
        IsCaregiver: false,
        IsPrivate: false,
        Slug: "",
        LeaflySlug: "",
        Photos: null,
        HoursOfOperation: [],
        DeliveryZipCodes: "",
        ScheduledDeliveryZipCodes: "",
        ApprovalZipCodes: "",
        Type: "",
        Address: {

            Name: "",
            Address1: null,
            Address2: null,
            City: null,
            State: null,
            Zip: null,
            Country: null,
            PhoneNumber: this.PhoneNumber,
            Latitude: null,
            Longitude: null,
            FormattedAddress: ""
        }
    };

    $scope.hideDocuments = true;

    $scope.approve = function (id) {
        bootbox.confirm("Are you sure you want to approve this dispensary?", function (result) {
            if (result) {

                $http.get($scope.ApiAddress + "/PendingDispensary/approve?id=" + id)
         .then(function (response) {
             //console.log(response);
             //$window.location.href = '/admin/PendingDispensaries';
             // alert(id);
             for (var i = 0; i < $scope.pendingDispensaries.length; i++) {
                 if ($scope.pendingDispensaries[i].id === id) {
                     $scope.pendingDispensaries.splice(i, 1);
                     return;
                 }
             }
             bootbox.alert('Dispensary approved successfully');
         }, function (response) {
             console.log(response);
             bootbox.alert("something went wrong while approving");
         });
            }
        });
    }

    $scope.approve = function (id) {
        bootbox.confirm("Are you sure you want to approve this dispensary?", function (result) {
            if (result) {

                $http.get($scope.ApiAddress + "/PendingDispensary/approve?id=" + id)
         .then(function (response) {
             for (var i = 0; i < $scope.pendingDispensaries.length; i++) {
                 if ($scope.pendingDispensaries[i].id === id) {
                     $scope.pendingDispensaries.splice(i, 1);
                     return;
                 }
             }
             bootbox.alert('Dispensary approved successfully');
         }, function (response) {
             console.log(response);
             bootbox.alert("something went wrong while approving");
         });
            }
        });
    }

    $scope.reject = function (id) {
        bootbox.confirm("Are you sure you want to reject this dispensary?", function (result) {
            if (result) {
                $http.get($scope.ApiAddress + "/PendingDispensary/reject?id=" + id)
                  .then(function (response) {
                      for (var i = 0; i < $scope.pendingDispensaries.length; i++) {
                          if ($scope.pendingDispensaries[i].id === id) {
                              $scope.pendingDispensaries.splice(i, 1);
                              return;
                          }
                      }
                      bootbox.alert('Dispensary reject successfully');
                  }, function (response) {
                      console.log(response);
                      bootbox.alert("something went wrong while rejecting");
                  });

            }
        });
    }

    $scope.predicate = 'name';
    $scope.reverse = true;
    $scope.order = function (predicate) {
        $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
        $scope.predicate = predicate;
    };




    $scope.getPendingDispensaries = function () {
        var url = $scope.ApiAddress + '/PendingDispensary/GetPendingDespensaries';

        $http.get(url).
            then(function (response) {
                $scope.pendingDispensaries = response.data;
                console.log(response.data);
            }, function (response) {
                console.log(response);
            });
    }


    $scope.getPendingDispensary = function () {
        var url = $scope.ApiAddress + '/PendingDispensary/GetPendingDispensary';

        $http.get(url).
            then(function (response) {
                console.log(response.data);
                $scope.dispensaryModel.Name = response.data.name;
                $scope.dispensaryModel.PhoneNumber = response.data.phoneNumber;
                $scope.dispensaryModel.EmailAddress = response.data.email;
                $scope.dispensaryModel.Type = response.data.type;
                $scope.dispensaryModel.Address.Address2 = response.data.address.address2;
                console.log(response.data);
            }, function (response) {
                console.log(response);
            });
    }



    $scope.registerDispensary = function () {
        $scope.loading = true;
        var url = $scope.ApiAddress + '/PendingDispensary/AddPendingDispensary';

        $http.post(url, $scope.model).
            then(function (response) {
                $scope.loading = false;
                $scope.alerts = [];
                alert('Request sent successfully, please check your email.');
                $window.location.href = '/';
            }, function (response) {
                $scope.loading = false;
                console.log(response);
                $scope.alerts = [];
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i] != "") {
                        $scope.alerts.push({ type: "danger", msg: response.data[i] });
                    }
                }
            });
    }

    $scope.updateStateInfo = function () {
        var url = $scope.ApiAddress + '/PendingDispensary/AddPendingDispensary';

        $http.post(url, $scope.model).
            then(function (response) {
                $scope.alerts = [];
                alert('Request sent successfully, we will back to you.');
                $window.location.href = '/';
            }, function (response) {
                console.log(response);
                $scope.alerts = [];
                for (var i = 0; i < response.data.length; i++) {
                    $scope.alerts.push({ type: "danger", msg: response.data[i] });
                }
            });
    }



    $scope.ViewDispensaryInfo = function (dispensary) {
        var modalInstance = $modal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'dipensaryInfo.html',
            controller: 'VerifyDispensaryModalCtrl',
            windowClass: 'large-Modal',
            resolve: {
                dispensary: function () {
                    return dispensary;
                }
            }
        });
    }

    $scope.login = function () {
        if (!$scope.username || !$scope.password) {
            return;
        }
        var url = $scope.ApiAddress + '/PendingDispensary/login';
        var data = {
            'Username': $scope.username,
            'Password': $scope.password,
            'Guid': Cookies.get("usertoken"),
            'grant_type': 'password'
        };
        $scope.alerts = null;
        $http.post(url, data).
            then(function (response) {
                console.log(response);
                if (typeof response.data.id != 'undefined') {
                    $scope.model.Id = response.data.id;
                    $scope.model.AddressId = response.data.addressId;
                    $scope.model.Name = response.data.name;
                    $scope.model.Email = response.data.email;
                    $scope.model.PhoneNumber = response.data.phoneNumber;
                    $scope.model.Type = response.data.type;
                    $scope.model.Address.Address1 = response.data.address.address1;
                    $scope.model.Address.Address2 = response.data.address.address2;
                    $scope.model.Address.City = response.data.address.city;
                    $scope.model.Address.State = response.data.address.state;
                    $scope.model.Address.Country = response.data.address.country;
                    $scope.model.Website = response.data.website;
                    $scope.model.ExperationDate = new Date(response.data.experationDate);
                    $scope.model.IdNumber = response.data.idNumber;
                    $scope.model.driversLicenseImageUrl = response.data.driversLicenseImageUrl;
                    $scope.model.recommendationImageUrl = response.data.recommendationImageUrl;

                    $scope.hideInfo = false;
                    $scope.hidePassword = true;
                    $scope.hideDocuments = false;
                    return;
                }

                console.log(data.access_token);
                Cookies.set("usertoken", response.data.usertoken);
                Cookies.set("sessiontoken", response.data.sessiontoken);
                Cookies.set("access_token", response.data.access_token);
                if (response.data.notCompleted) {
                    $window.location.href = '/dispensary/complete';
                }

                if ($scope.redirect === 'undefined' || $scope.redirect == null) {
                    window.location.replace("/menu");
                }
                else {
                    window.location.replace("/" + $scope.redirect);
                }

            }, function (response) {
                console.log(response);
                $scope.alerts = [];
                for (var i = 0; i < response.data.length; i++) {
                    $scope.alerts.push({ type: "danger", msg: response.data[i] });
                }
            });
    };

    $scope.removeHours = function (i) {
        $scope.dispensaryModel.HoursOfOperation.splice(i, 1);
        console.log($scope.dispensaryModel);
    };
    $scope.addHours = function () {
        $scope.dispensaryModel.HoursOfOperation.push({ "Day": $scope.hoursModel.Day, "Start": $scope.hoursModel.Start, "Stop": $scope.hoursModel.Stop });
        console.log($scope.dispensaryModel.HoursOfOperation);
    };


    $scope.UploadRx = function (file) {

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


            $scope.model.recommendationImageUrl = photo.url;
            $scope.rxPhoto = photo;
        }).error(function (data, status, headers, config) {
            bootbox.alert("Error uploading photo");
        });
    };


    $scope.updateDocuments = function () {
        var url = $scope.ApiAddress + '/PendingDispensary/UpdatePendingDispensaryDocs';

        if ($scope.model.driversLicenseImageUrl === '' && $scope.model.recommendationImageUrl === '') {
            alert('Please add documents first');
            return;
        }

        $http.post(url, $scope.model).
            then(function (response) {
                $scope.alerts = [];
                alert('Your dispensary now awaiting for approve.');
            }, function (response) {
                console.log(response);
                $scope.alerts = [];
                for (var i = 0; i < response.data.length; i++) {
                    $scope.alerts.push({ type: "danger", msg: response.data[i] });
                }
            });
    }

    $scope.updateLicense = function () {
        var url = $scope.ApiAddress + '/PendingDispensary/UpdatePendingDispensaryLicense';

        if ($scope.model.IdNumber === '' && $scope.model.ExperationDate === null) {
            alert('Please add lisence data first');
            return;
        }

        $http.post(url, $scope.model).
            then(function (response) {
                $scope.alerts = [];
                alert('data saved successfully');
            }, function (response) {
                console.log(response);
                $scope.alerts = [];
                for (var i = 0; i < response.data.length; i++) {
                    $scope.alerts.push({ type: "danger", msg: response.data[i] });
                }
            });
    }

    $scope.UploadId = function (file) {

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


            $scope.model.driversLicenseImageUrl = photo.url;
            $scope.idPhoto = photo;
        }).error(function (data, status, headers, config) {
            bootbox.alert("Error uploading photo");
        });
    };

    $scope.addDispensary = function (form, valid) {
        if (valid) {
            console.log($scope.dispensaryModel);
            $http.post($scope.ApiAddress + "/PendingDispensary/InsertDispensaryFromPending", JSON.stringify($scope.dispensaryModel))
                .then(function (response) {
                    console.log(response);
                    alert('Dispensary saved successfully');
                    $window.location.href = '/';
                }, function (response) {
                    console.log(response);
                });
        } else {
            alert("One or more fields are invalid");
        }
    };


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
}

app.controller('VerifyDispensaryModalCtrl', VerifyDispensaryModalCtrl);

VerifyDispensaryModalCtrl.$inject = ['$scope', '$http', '$modalInstance', '$filter', 'dispensary', 'ApiAddress', '$window'];

function VerifyDispensaryModalCtrl($scope, $http, $modalInstance, $filter, dispensary, ApiAddress, $window) {
    $scope.dispensary = dispensary;
    $scope.ApiAddress = ApiAddress;


    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.cancelDispensarytData = function () {
        $modalInstance.dismiss('cancel');
    };



}