app.service("PopupService", PopupService);
function PopupService() {
    this.show = function (link) {
        var kind = "_blank";
        var params = ["height=".concat(screen.height), "width=".concat(screen.width), "location=no", "fullscreen=yes" // only works in IE, but here for completeness
        ].join(",");
        var popup = window.open(link, kind, params);
        popup.moveTo(0, 0);
    };
}