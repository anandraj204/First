$(document).ready(function () {
	$(".navbar .navbar-right .dropdown-toggle").on("click", function (e) {
		e.preventDefault();

		if ($(this).hasClass("active")) {
			$(this).removeClass("active");
			$(this).parents(".navbar").removeClass("open-slide");
			$(".slide-menu").removeClass("slide");
			$("div.overlay").hide();
		}
		else {
			$(".slide-menu").addClass("slide");
			$("div.overlay").show();
			$(this).addClass("active");
			$(this).parents(".navbar").addClass("open-slide");

		}
	});

	$(".overlay").on("click", function () {
		$(".navbar .navbar-right .dropdown-toggle").removeClass("active");
		$(".navbar").removeClass("open-slide");
		$(".slide-menu").removeClass("slide");
		$(this).hide();
	});

	$(document).keyup(function (e) {
		if (e.keyCode == 27) {
			if($(".navbar .navbar-right .dropdown-toggle").hasClass("active")){
				$(".navbar .navbar-right .dropdown-toggle").removeClass("active");
				$(".navbar").removeClass("open-slide");
				$(".slide-menu").removeClass("open");
				$("div.overlay").hide();
			}

		}
	});

	$(".desktop-navbar .search").on("click", function () {
		$(this).addClass("hidden");
		$(".navbar .navbar-form").removeClass("hidden");
	});

	$(".mobile-navbar .search").on("click", function () {
		$(this).addClass("hidden");
		$(".navbar-header").addClass("hidden");
		$(".navbar .navbar-form").removeClass("hidden");
	});

	$(".desktop-navbar .navbar-right .fa-times").on("click", function () {
		if ($(".desktop-navbar .search-wrapper input.search-input").val().length == 0) {
			$(".navbar .search").removeClass("hidden");
			$(".navbar .navbar-form").addClass("hidden");
		}
		else {
			$(".desktop-navbar .search-wrapper .search-input").val("");
		}

	});
	$(".mobile-navbar .navbar-right .fa-times").on("click", function () {
		if ($(".mobile-navbar .search-wrapper input.search-input").val().length == 0) {
			$(".mobile-navbar .search").removeClass("hidden");
			$(".navbar-header").removeClass("hidden");
			$(".mobile-navbar .navbar-form").addClass("hidden");
		}
		else {
			$(".mobile-navbar .search-wrapper input.search-input").val("");
		}

	});

});