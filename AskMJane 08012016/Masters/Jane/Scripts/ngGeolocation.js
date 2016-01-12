﻿"use strict"; angular.module("ngGeolocation", []).factory("$geolocation", ["$rootScope", "$window", "$q", function (a, b, c) { function d() { return "geolocation" in b.navigator } var e = { getCurrentPosition: function (f) { var g = c.defer(); return d() ? b.navigator.geolocation.getCurrentPosition(function (b) { a.$apply(function () { e.position.coords = b.coords, e.position.timestamp = b.timestamp, g.resolve(b) }) }, function (b) { a.$apply(function () { g.reject({ error: b }) }) }, f) : g.reject({ error: { code: 2, message: "This web browser does not support HTML5 Geolocation" } }), g.promise }, watchPosition: function (c) { d() ? this.watchId || (this.watchId = b.navigator.geolocation.watchPosition(function (b) { a.$apply(function () { e.position.coords = b.coords, e.position.timestamp = b.timestamp, delete e.position.error, a.$broadcast("$geolocation.position.changed", b) }) }, function (b) { a.$apply(function () { e.position.error = b, delete e.position.coords, delete e.position.timestamp, a.$broadcast("$geolocation.position.error", b) }) }, c)) : e.position = { error: { code: 2, message: "This web browser does not support HTML5 Geolocation" } } }, clearWatch: function () { this.watchId && (b.navigator.geolocation.clearWatch(this.watchId), delete this.watchId) }, position: {} }; return e }]);