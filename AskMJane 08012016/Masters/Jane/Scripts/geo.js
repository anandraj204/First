$(document).ready(function () {


	if (navigator.geolocation && $('.zip-input').length) {

		navigator.geolocation.getCurrentPosition(function (position) {
			var pos = new google.maps.LatLng(position.coords.latitude,
											 position.coords.longitude);

			$.get('https://maps.googleapis.com/maps/api/geocode/json?latlng=' + position.coords.latitude + ',' + position.coords.longitude + '&sensor=true').then(function (res) {
				var components = res.results[0].address_components;
				for (var i = 0; i < components.length; i++) {
					for (var j = 0; j < components[i].types.length; j++) {
						if ("postal_code" == components[i].types[j]) {
							zip = components[i].long_name;
							$('.zip-input')[0].value = zip

						}
					}
				}
			});
		}, function () {
			//handleNoGeolocation(true);
		});
	} else {
		// Browser doesn't support Geolocation
		//handleNoGeolocation(false);
	}

})
