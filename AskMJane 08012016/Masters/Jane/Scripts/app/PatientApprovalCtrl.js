angular.module('AskMJaneAngularApp').controller('PatientApprovalCtrl', PatientApprovalCtrl);

PatientApprovalCtrl.$inject = ['$scope', '$http', '$modal'];
var unknowPatients = [];
var knownPatients = [];
var allPatients = [];

function PatientApprovalCtrl($scope, $http, $modal) {

    $scope.patients = [];
    $scope.hideUnknown = false;
    $scope.patient = null;
    $scope.loading = true;

    $scope.searchText = '';

    $scope.getNonPatients = function () {
        $http.get($scope.ApiAddress + "/patient/GetNonPatients").then(function (response) {
            console.log(response);
            allPatients = response.data;
            for (var i = 0; i < response.data.length; i++) {
                var email = response.data[i].email;
                if (email.indexOf("@none.com") > -1) {
                    unknowPatients.push(response.data[i]);
                } else {
                    knownPatients.push(response.data[i]);
                }
            }

            $scope.patients = allPatients;
            $scope.loading = false;
        }, function (response) {
            console.log(response);
            $scope.loading = false;
        });
    }

    $scope.predicate = 'firstName';
    $scope.reverse = true;
    $scope.order = function (predicate) {
        $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
        $scope.predicate = predicate;
    };


    $scope.getApprovalStatus = function (approvalid) {
        if (approvalid == 1) {
            return "Incomplete";
        }
        if (approvalid == 2) {
            return "Awaiting Approval";
        }
        if (approvalid == 3) {
            return "Rejected";
        }
        if (approvalid == 4) {
            return "Approved";
        }
    };



    $scope.open = function (patient) {
        var modalInstance = $modal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'myModalContent.html',
            size: "lg",
            controller: "VerifyPatientModalCtrl",
            resolve: {
                patient: function () {
                    patient.patientInfo.medicalCardExpirationDate = new Date(patient.patientInfo.medicalCardExpirationDate);
                    return patient;
                }
            }
        });
    }

    $scope.editCredits = function (patient) {
        var modalInstance = $modal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'editCredits.html',
            size: "sm",
            controller: "EditCreditsModalCtrl",
            resolve: {
                patient: function () {
                    return patient;
                }
            }
        });
    }


    $scope.viewPatientData = function (patient) {
        var modalInstance = $modal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'patientInfo.html',
            controller: "VerifyPatientModalCtrl",
            size: "lg",
            resolve: {
                patient: function () {
                    patient.birthday = new Date(patient.birthday);
                    patient.patientInfo.medicalCardExpirationDate = new Date(patient.patientInfo.medicalCardExpirationDate);
                    return patient;
                }
            }
        });
    }

    $scope.deletePatient = function (patient) {
            var r = confirm("Are you sure you want to delete this patient?");
            if (r) {
                var url = $scope.ApiAddress + '/account/deleteUser?id=' + patient.id;

                $http.get(url).
                    then(function (response) {
                        var deletedPatient = allPatients.indexOf(patient);
                        if (deletedPatient > -1) {
                            allPatients.splice(deletedPatient, 1);
                        }

                        deletedPatient = knownPatients.indexOf(patient);
                        if (deletedPatient > -1) {
                            knownPatients.splice(deletedPatient, 1);
                        }

                        deletedPatient = unknowPatients.indexOf(patient);
                        if (deletedPatient > -1) {
                            unknowPatients.splice(deletedPatient, 1);
                        }

                        deletedPatient = $scope.patients.indexOf(patient);
                        if (deletedPatient > -1) {
                            $scope.patients.splice(deletedPatient, 1);
                        }

                        alert('Patient deleted successfully');
                    }, function (response) {
                        console.log(response);
                        alert("Error occured while deleting");
                    });
            }
    }

    $scope.hideUnknownPatients = function () {
        if ($scope.hideUnknown) {
            $scope.patients = knownPatients;
        } else {
            $scope.patients = allPatients;
        }
    }


    $scope.getNonPatients();
};
app.controller('VerifyPatientModalCtrl', VerifyPatientModalCtrl);

VerifyPatientModalCtrl.$inject = ['$scope', '$http', '$modalInstance', '$filter', 'patient', 'ApiAddress'];

function VerifyPatientModalCtrl($scope, $http, $modalInstance, $filter, patient, ApiAddress) {
    $scope.patient = patient;
    $scope.ApiAddress = ApiAddress;
    if (!($scope.patient.birthday === 'undefined') && $scope.patient.birthday != null && $scope.patient.birthday != '') {
        $scope.patient.birthday = new Date($filter('date')($scope.patient.birthday, 'MM-dd-yyyy'));
    }
    $scope.approve = function (form, valid) {
        if (valid) {
            $scope.patient.birthday = new Date($filter('date')($scope.patient.birthday, 'MM-dd-yyyy'));
            $scope.patient.patientInfo.approvalStatus = 4; //accepted
            $http.post($scope.ApiAddress + "/patient/SetApproval", $scope.patient)
              .then(function (response) {
                  if (typeof (response) == "undefined") {
                  }
                  else {
                      console.log(response);
                      $modalInstance.close();
                  }
              }, function (response) {
                  console.log(response);
                  alert("something went wrong while approving");
              });
        }
    };
    $scope.reject = function () {
        $scope.patient.birthday = new Date($filter('date')($scope.patient.birthday, 'MM-dd-yyyy'));
        $scope.patient.patientInfo.approvalStatus = 3; //rejected
        $http.post($scope.ApiAddress + "/patient/SetApproval", $scope.patient)
          .then(function (response) {
              if (typeof (response) == "undefined") {
              }
              else {
                  console.log(response);
                  $modalInstance.close();
              }
          }, function (response) {
              console.log(response);
              alert("something went wrong while rejecting");
          });

    }

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.cancelPatientData = function () {
        $modalInstance.dismiss('cancel');
    };
}

app.controller('EditCreditsModalCtrl', EditCreditsModalCtrl);

EditCreditsModalCtrl.$inject = ['$scope', '$http', '$modalInstance', '$filter', 'patient', 'ApiAddress'];

function EditCreditsModalCtrl($scope, $http, $modalInstance, $filter, patient, ApiAddress) {
    $scope.patient = patient;
    $scope.ApiAddress = ApiAddress;

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.save = function () {
        var data = { userId: $scope.patient.id, amount: $scope.patient.wallet.credit };
        $http.post($scope.ApiAddress + "/wallet/EditCredits", data)
            .then(function(response) {
                if (typeof (response) != "undefined") {
                    $modalInstance.close();
                    alert("User's credits has been updated");
                }
            }, function(response) {
                console.log(response);
            });

    }

}