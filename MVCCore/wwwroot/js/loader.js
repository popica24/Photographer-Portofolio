﻿$(window).on("load", function () { $(".tile").css("opacity", 0); var i = $(".loading-screen"), n = $("#container"); setTimeout(function () { i.hide(), n.show() }, 1e3); var t = 0, a = $(".tile"), o = setInterval(function () { a.eq(t).animate({ opacity: 1 }, 1e3), ++t >= a.length && clearInterval(o) }, 500); $(".slide-left-automatic").css("animation", "slideRight 2s forwards") });