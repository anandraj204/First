angular.module('AskMJaneAngularApp').controller('PatientApplicationCtrl', PatientApplicationCtrl);

PatientApplicationCtrl.$inject = ['$scope', '$http', '$filter', 'Upload'];

function PatientApplicationCtrl($scope, $http, $filter,Upload) {
    $scope.alerts = [];
    $scope.closeAlert = function(index) {
        $scope.alerts.splice(index, 1);
    };
    $scope.confirmmodel = {code : ""};
    $scope.verificationCodeSent = false;
    $scope.verificationfailed = false;
    $scope.user = null;
    $scope.cart = null;
    $scope.idupload = null;
    $scope.rxupload = null;
    $scope.patientdoctype = 'rx';
    $scope.policy = 'ewogICJleHBpcmF0aW9uIjogIjIwMjAtMDEtMDFUMDA6MDA6MDBaIiwKICAiY29uZGl0aW9ucyI6IFsKICAgIHsiYnVja2V0IjogImFza21qYW5lIn0sCiAgICBbInN0YXJ0cy13aXRoIiwgIiRrZXkiLCAiIl0sCiAgICB7ImFjbCI6ICJwcml2YXRlIn0sCiAgICBbInN0YXJ0cy13aXRoIiwgIiRDb250ZW50LVR5cGUiLCAiIl0sCiAgICBbInN0YXJ0cy13aXRoIiwgIiRmaWxlbmFtZSIsICIiXSwKICAgIFsiY29udGVudC1sZW5ndGgtcmFuZ2UiLCAwLCA1MjQyODgwMDBdCiAgXQp9';
    $scope.signature = 'fWI1T0ypTbk35wwxlzzqeWRkdA8=';

    $scope.SaveUserInfo = function (form, valid) {
        if (valid) {
            if ($scope.user.zipcode != '' && $scope.user.phoneNumber != '' &&
                $scope.user.phoneNumberConfirmed != false && $scope.user.patientInfo.driversLicenseImageUrl != ''
                && $scope.user.patientInfo.recommendationImageUrl != '' && $scope.user.patientInfo.driversLicenseImageUrl != null
                && $scope.user.patientInfo.recommendationImageUrl != null && $scope.user.patientInfo.approvalStatus != 2
                && $scope.user.patientInfo.approvalStatus != 4)
            {
                $http.post($scope.ApiAddress + "/patient/submitApplication", {
                    'Id': $scope.user.id,
                    'FirstName': $scope.user.firstName,
                    'LastName': $scope.user.lastName,
                    'Birthday': $filter('date')($scope.user.birthday, 'MM-dd-yyyy'),
                    'Zipcode': $scope.user.zipcode,
                    'PhoneNumber': $scope.user.phoneNumber
                }).then(function (response) {
                    console.log(response);
                    window.location = "/cart";
                }, function (response) {
                    console.log(response);
                });
            }
        }
    }


    $scope.UploadId = function (file) {
        Upload.upload({
            url: 'https://askmjane.s3.amazonaws.com/', //S3 upload url including bucket name
            method: 'POST',
            sendFieldsAs: 'form',
            fields: {
                key:  'users/' + $scope.user.guid + '/patientdocs/id/' + file.name, // the key to store the file on S3, could be file name or customized
                AWSAccessKeyId: 'AKIAIQTPO4IEKH3RUNDQ',
                acl: 'private', // sets the access to the uploaded file in the bucket: private or public
                policy: $scope.policy, // base64-encoded json policy (see article below)
                signature: $scope.signature, // base64-encoded signature based on policy string (see article below)
                "Content-Type": file.type != '' ? file.type : 'application/octet-stream', // content type of the file (NotEmpty)
                filename: file.name // this is needed for Flash polyfill IE8-9
            },
            file: file
        }).success(function (data, status, headers, config) {
            $scope.idupload = file;
            $scope.SetPatientIDImageUrl('users/' + $scope.user.guid + '/patientdocs/id/' + file.name);
        }).error(function (data, status, headers, config) {
        });
    };
    $scope.UploadRx = function (file) {
        Upload.upload({
            url: 'https://askmjane.s3.amazonaws.com/', //S3 upload url including bucket name
            method: 'POST',
            sendFieldsAs: 'form',
            fields: {
                key: 'users/' + $scope.user.guid + '/patientdocs/rx/' + file.name, // the key to store the file on S3, could be file name or customized
                AWSAccessKeyId: 'AKIAIQTPO4IEKH3RUNDQ',
                acl: 'private', // sets the access to the uploaded file in the bucket: private or public
                policy: $scope.policy, // base64-encoded json policy (see article below)
                signature: $scope.signature, // base64-encoded signature based on policy string (see article below)
                "Content-Type": file.type != '' ? file.type : 'application/octet-stream', // content type of the file (NotEmpty)
                filename: file.name // this is needed for Flash polyfill IE8-9
            },
            file: file
        }).success(function(data, status, headers, config) {
            $scope.rxupload = file;
            if ($scope.patientdoctype == 'rx') {
                $scope.SetPatientRxImageUrl('users/' + $scope.user.guid + '/patientdocs/rx/' + file.name);
            } else {
                $scope.SetPatientRxImageUrl('users/' + $scope.user.guid + '/patientdocs/rx/' + file.name);
            }
        }).error(function(data, status, headers, config) {
        });
    };

    $scope.SetPatientIDImageUrl = function(path) {
        var data = {
            Id : $scope.user.patientInfoId,
            Path : path
        }
        $http.post($scope.ApiAddress + "/patient/setidurl", data)
         .then(function (response) {
             if (typeof (response) == "undefined") {
             } else {
                 console.log(response.data);
                 $scope.user.patientInfo.driversLicenseImageUrl = data.Path;
             }
         }, function (response) {
             console.log(response);
         });
    };
    $scope.SetPatientRxImageUrl = function (path) {
        var data = {
            Id : $scope.user.patientInfoId,
            Path : path
        }
        $http.post($scope.ApiAddress + "/patient/setrxurl", data)
         .then(function (response) {
             if (typeof (response) == "undefined") {
             } else {
                 $scope.user.patientInfo.recommendationImageUrl = data.Path;
                 console.log(response.data);
             }
         }, function (response) {
             console.log(response);
         });
    };

    $scope.SendVerificationCode = function (form, valid) {
        if (valid) {
            $scope.user.phoneNumber = form.phonenumber.$modelValue;
            $http.get($scope.ApiAddress + "/patient/GetMobileVerificationCode?phonenumber=" + $scope.user.phoneNumber)
               .then(function (response, data) {
                   if (typeof (response) == "undefined") {
                   } else {
                       $scope.verificationcode = response.data;
                       $scope.verificationCodeSent = true;
                       console.log(response.data);
                   }
               }, function (response) {
                   console.log(response);
               });
        }
    }
    $scope.ConfirmPhoneNumber = function(form, valid) {
        if ($scope.confirmmodel.code == $scope.verificationcode && $scope.user.phoneNumber != '') {
            $http.post($scope.ApiAddress + "/patient/ConfirmPhoneNumber?id=" + $scope.user.id + "&phonenumber=" + $scope.user.phoneNumber)
                .then(function(response, data) {
                    if (typeof (response) == "undefined") {
                    } else {
                        $scope.GetUser();
                        console.log(response.data);
                    }
                }, function(response) {
                    $scope.verificationfailed = true;
                    console.log(response);
                });
        } else {
            $scope.verificationfailed = true;
        }
    }

    $scope.GetUser = function() {
        var usertoken = Cookies.get("usertoken");
        $http.get($scope.ApiAddress + "/account/getbyusertoken?usertoken=" + usertoken)
            .then(function(response, data) {
                if (typeof (response) == "undefined") {
                } else {
                    $scope.user = response.data;
                    if (!($scope.user.birthday === 'undefined') &&  $scope.user.birthday != null && $scope.user.birthday != '') {
                        $scope.user.birthday = new Date($filter('date')($scope.user.birthday, 'MM-dd-yyyy'));
                    }
                    if ($scope.user.patientInfo.approvalStatus == 0 || $scope.user.patientInfo.approvalStatus == 1) {
                        $scope.SaveUserInfo(null, true);
                    }
                    console.log(response.data);
                }
            }, function(response) {
                console.log(response);
            });
        console.log(usertoken);
    }

  

    $scope.GetUser();
}
