app.controller("ApprovalController", approvalCtrlr);
approvalCtrlr.$inject = ["$rootScope", "$scope", "$http", "DTOptionsBuilder", "DTColumnBuilder", "ApiFactory", "$timeout"];
function approvalCtrlr($rootScope, $scope, $http, DTOptionsBuilder, DTColumnBuilder, ApiFactory, $timeout) {
    $scope.isSaveOngoing = false;
    $scope.NoImageFile = window.url + "/public/image/img_placeholder.png";
    $rootScope.checkUrl();
    $scope.resizeWindow = function() {
        var height = $("#spa-view-container").height() + $("#spa-menus").height() + 150;
        $("#spa-container").css("height", height + "px");
        setTimeout(function () { $(".page-loader-wrapper").fadeOut(); }, 50);
    };
    $scope.resizeWindow();
    window.onresize = $scope.resizeWindow;
}