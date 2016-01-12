angular.module('AskMJaneAngularApp').controller('MyAccountCtrl', MyAccountCtrl);

MyAccountCtrl.$inject = ['$window', '$scope', '$http', 'Upload'];



function MyAccountCtrl($window, $scope, $http, Upload) {
    $scope.username = null;
    $scope.password = null;
    $scope.confirmpassword = null;
    $scope.zipcode = null;
    $scope.alerts = [];
    $scope.passwordAlerts = [];
    $scope.phoneNumber = null;
    $scope.firstName = null;
    $scope.lastName = null;
    $scope.birthday = null;

    $scope.addressLine1 = null;
    $scope.unitNo = null;
    $scope.city = null;
    $scope.state = null;

    $scope.redirect = location.search.split('redirectTo=')[1];
    $scope.oldPassword = null;
    $scope.newPassword = null;
    $scope.confirmPassword = null;
    $scope.phoneNumberConfirmed = null;
    $scope.phoneNumberError = null;
    $scope.verifyCode = null;
    $scope.emailConfirmed = null;
    $scope.forgetEmail = null;
    $scope.idNumber = null;
    $scope.experationDate = null;

    $scope.driversLicenseImageUrl = null;
    $scope.recommendationImageUrl = null;
    $scope.idupload = null;
    $scope.rxupload = null;



    $scope.GetUser = function () {
        var usertoken = Cookies.get("usertoken");
        $http.get($scope.ApiAddress + "/account/GetLoggedInUser")
          .then(function (response, data) {
              if (typeof (response) == "undefined") {
              } else {
                  $scope.username = response.data.username;
                  $scope.zipcode = response.data.zipcode;
                  $scope.phoneNumber = response.data.phoneNumber;
                  $scope.firstName = response.data.firstName;
                  $scope.lastName = response.data.lastName;
                  $scope.birthday = new Date(response.data.birthday);
                  $scope.phoneNumberConfirmed = response.data.phoneNumberConfirmed;
                  $scope.emailConfirmed = response.data.emailConfirmed;
                  $scope.idNumber = response.data.patientInfo.medicalCardNumber;
                  $scope.experationDate = new Date(response.data.patientInfo.medicalCardExpirationDate);
                  $scope.addressLine1 = response.data.address.address1;
                  $scope.unitNo = response.data.address.address2;
                  $scope.city = response.data.address.city;
                  $scope.state = response.data.address.state;
                  $scope.driversLicenseImageUrl = response.data.patientInfo.driversLicenseImageUrl;

                  $scope.recommendationImageUrl = response.data.patientInfo.recommendationImageUrl;
                  console.log(response.data);
              }
          }, function (response) {
              console.log(response);
          });
        console.log(usertoken);
    }

    $scope.GetUser();

    $scope.update = function () {
        var url = $scope.ApiAddress + '/account/update';
        var data = {
            'Email': $scope.username,
            'Username': $scope.username,
            'FirstName': $scope.firstName,
            'LastName': $scope.lastName,
            'PhoneNumber': $scope.phoneNumber,
            'Zipcode': $scope.zipcode,
            'Birthday': $scope.birthday,
            'Guid': Cookies.get("usertoken")
        };

        $http.post(url, data).
            then(function (response) {
                $scope.alerts = [];
                $scope.phoneNumberConfirmed = response.data.phoneNumberConfirmed;
                $scope.emailConfirmed = response.data.emailConfirmed;
                alert('data saved successfully');
            }, function (response) {
                console.log(response);
                $scope.alerts = [];
                for (var i = 0; i < response.data.length; i++) {
                    $scope.alerts.push({ type: "danger", msg: response.data[i] });
                }
            });
    }

    $scope.changePassword = function () {
        var url = $scope.ApiAddress + '/account/ChangePassword';
        var data = {
            'OldPassword': $scope.oldPassword,
            'NewPassword': $scope.newPassword,
            'ConfirmPassword': $scope.confirmPassword
        };

        $http.post(url, data).
            then(function (response) {
                $scope.passwordAlerts = [];
                $scope.newPassword = null;
                $scope.oldPassword = null;
                $scope.confirmPassword = null;
                alert('Password changed successfully');
            }, function (response) {
                console.log(response);
                $scope.passwordAlerts = [];
                for (var i = 0; i < response.data.length; i++) {
                    $scope.passwordAlerts.push({ type: "danger", msg: response.data[i] });
                }
            });
    }


    $scope.sendCode = function () {
        $scope.phoneNumberError = null;
        var url = $scope.ApiAddress + '/account/SendCode?phoneNumber=' + $scope.phoneNumber;
        if ($scope.phoneNumber === "" || $scope.phoneNumber == null) {
            $scope.phoneNumberError = "Please add phone number first";
            return;
        }
        $http.get(url).
            then(function (response) {
                $window.location.href = '/verifyPhoneNumber';
            }, function (response) {
                console.log(response);
                $scope.phoneNumberError = "Faild to send code to your number";
            });

    }

    $scope.verifyPhoneNumber = function () {
        var url = $scope.ApiAddress + '/account/verifyPhoneNumber?code=' + $scope.verifyCode;
        $http.get(url).
            then(function (response) {
                alert("phone number Verified successfulyy");
                $window.location.href = '/myaccount';
            }, function (response) {
                $scope.phoneNumberError = "Invalid code";
            });

    }

    $scope.sendMailVerification = function () {
        var url = $scope.ApiAddress + '/account/sendMailVerification';
        $http.get(url).
            then(function (response) {
                alert("A verifiaction mail sent successfully");
            }, function (response) {

            });

    }



    $scope.sendPasswordToUser = function () {
        $scope.alerts = [];
        var url = $scope.ApiAddress + '/account/ForgotPassword?email=' + $scope.forgetEmail;
        if ($scope.forgetEmail === "" || $scope.forgetEmail == null) {
            $scope.alerts.push({ type: "danger", msg: "Please enter your email" });
            return;
        }
        $http.get(url).
            then(function (response) {
                alert("Reset password email sent to you successfully");
                $window.location.href = '/login';
            }, function (response) {
                console.log(response);
                $scope.alerts = [];
                for (var i = 0; i < response.data.length; i++) {
                    $scope.alerts.push({ type: "danger", msg: response.data[i] });
                }
            });

    }


    $scope.resetUserPassword = function () {
        $scope.alerts = [];
        var url = $scope.ApiAddress + '/account/resetpassword';

        var data = {
            'NewPassword': $scope.newPassword,
            'ConfirmPassword': $scope.confirmPassword,
            'SentPassword': $scope.oldPassword,
            'Email': $scope.forgetEmail
        };

        $http.post(url, data).
            then(function (response) {
                alert("Password reset successfully");
                $window.location.href = '/login';
            }, function (response) {
                console.log(response);
                $scope.alerts = [];
                for (var i = 0; i < response.data.length; i++) {
                    $scope.alerts.push({ type: "danger", msg: response.data[i] });
                }
            });

    }

    $scope.updateStateInfo = function () {
        $scope.alerts = [];
        var url = $scope.ApiAddress + '/account/updateStateInfo';

        var data = {
            'MedicalCardNumber': $scope.idNumber,
            'MedicalCardExpirationDate': $scope.experationDate
        };

        $http.post(url, data).
            then(function (response) {
                alert("State info updated successfully");
            }, function (response) {
                console.log(response);
                $scope.alerts = [];
                for (var i = 0; i < response.data.length; i++) {
                    $scope.alerts.push({ type: "danger", msg: response.data[i] });
                }
            });
    }


    $scope.updateDocumentsInfo = function () {
        $scope.alerts = [];
        var url = $scope.ApiAddress + '/account/UpdateUserDocuments';

        var data = {
            'DriversLicenseImageUrl': $scope.driversLicenseImageUrl,
            'RecommendationImageUrl': $scope.recommendationImageUrl
        };

        $http.post(url, data).
            then(function (response) {
                alert("Documents info updated successfully");
            }, function (response) {
                console.log(response);
                $scope.alerts = [];
                for (var i = 0; i < response.data.length; i++) {
                    $scope.alerts.push({ type: "danger", msg: response.data[i] });
                }
            });
    }



    $scope.updateAddressInfo = function () {
        $scope.alerts = [];
        var url = $scope.ApiAddress + '/account/updateAddressInfo';

        var data = {
            'Address1': $scope.addressLine1,
            'Address2': $scope.unitNo,
            'City': $scope.city,
            'State': $scope.state
        };

        $http.post(url, data).
            then(function (response) {
                alert("Address info updated successfully");
            }, function (response) {
                console.log(response);
                alert("Error has occured while updating your address");
            });
    }


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


            $scope.recommendationImageUrl = photo.url;
            $scope.rxPhoto = photo;
        }).error(function (data, status, headers, config) {
            bootbox.alert("Error uploading photo");
        });
    };

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


            $scope.driversLicenseImageUrl = photo.url;
            $scope.idPhoto = photo;
        }).error(function (data, status, headers, config) {
            bootbox.alert("Error uploading photo");
        });
    };
}