app.controller("expiryCtrl", expiryCtrl);
expiryCtrl.$inject = ["$rootScope", "$scope", "$http", "DTOptionsBuilder", "DTColumnBuilder", "ApiFactory", "$timeout"];
function expiryCtrl($rootScope, $scope, $http, DTOptionsBuilder, DTColumnBuilder, ApiFactory, $timeout) {
    var vmmm = this;
    vmmm.factory = new ApiFactory();
}