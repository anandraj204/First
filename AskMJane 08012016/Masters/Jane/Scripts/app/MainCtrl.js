angular.module('AskMJaneAngularApp').controller('MainCtrl', MainCtrl);

MainCtrl.$inject = ['$scope', '$http','ApiAddress'];

function MainCtrl($scope, $http, ApiAddress) {
	$scope.alerts = [];
	$scope.badge = 0;
    $scope.ApiAddress = ApiAddress;
	/*Small modal variables*/
	$scope.ShowSmallModal = false;
	$scope.SmallModal = {
		"header": "",
		"text": "",
		"class": "hide"
	}

	$scope.GetUser = function () {
	    var usertoken = Cookies.get("usertoken");
	    $http.get($scope.ApiAddress + "/account/getbyusertoken?usertoken=" + usertoken)
          .then(function (response, data) {
              if (typeof (response) == "undefined") {
              } else {
                  $scope.user = response.data;
                  console.log(response.data);
              }
          }, function (response) {
              console.log(response);
          });
	    console.log(usertoken);
	}

	$scope.GetUser();

	$("#tbSearch").autocomplete({
	    source: function (request, response) {
	        $.ajax({
	            url: $scope.ApiAddress + "/DispensaryProducts/search",
	            dataType: "json",
	            data: {
	                term: request.term,
	                "zip": $scope.user.zipcode
	            },
	            success: function (data) {
	                response(data);
	            }
	        });
	    },
	    minLength: 3,
	    select: function (event, ui) {
	        window.location.href = "/product/details/" + ui.item.value;
	        this.value = ui.item.label;
	        return false;
	    }
	});

    $scope.closeAlert = function(index) {
        $scope.alerts.splice(index, 1);
    };


    $scope.Logout = function() {
        $http.post($scope.ApiAddress + "/account/logout?usertoken=" + Cookies.get('usertoken') + '&sessiontoken=' + Cookies.get('sessiontoken')).
            then(function (response) {
                console.log(response);

                Cookies.remove("sessiontoken");
                Cookies.remove("access_token");
                window.location.replace("/");
            }, function (response) {
                console.log(response);
                // errors here, including:
                // invalid_grant_type
                // invalid username/password
                Cookies.remove("sessiontoken");
                Cookies.remove("access_token");
                window.location.replace("/");
            });
        
    }

    $scope.BadgeCounter = function () {
        var token = Cookies.get('usertoken');
        if (token != null) {
            $http.get($scope.ApiAddress + "/cart/getcountbyusertoken?usertoken=" + Cookies.get('usertoken'))
            .then(function (response) {

                if (response.data != null) {
                    $scope.badge = response.data;
                }
              
            },
            function (error) {
                console.log(error);
            });
        }
    }
 
    $scope.BadgeCounter();

    
    $scope.CloseSmallModalWithDelay = function () {
    	window.setTimeout(function () {
    		$scope.$apply(function () {
    			$scope.SmallModal.class = "hide";
    			$scope.SmallModal.header = "";
    			$scope.SmallModal.text = "";
    		});
    	}, 3000);
    }
}