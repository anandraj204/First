$(".menu-list-wrapper .menu-item").on("click", function (e) {
	if ($(this).hasClass("open")) {
		$(this).removeClass("open");
	}
	else {
		$(".menu-list-wrapper .menu-item").each(function () {
			if ($(this).hasClass("open")) {
				$(this).removeClass("open");
			}
		});

		$(this).addClass("open");
	}

});