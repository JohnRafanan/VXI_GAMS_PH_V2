app.factory("ManageApiFactory", ManageApiFactory);
ManageApiFactory.$inject = ["$http", "Settings"];
function ManageApiFactory($http, Settings) {
    return function() {
        this.baseUrl = globalBaseUrl;
        
        this.getData = function (api, vm) {
            if (vm === undefined || vm === null) {
                return $http.post(this.baseUrl + api);
            } else {
                return $http.post(this.baseUrl + api, vm);
            }
        };

        this.setData = function (api, vm) {
            if (vm === undefined || vm === null) {
                return $http.post(this.baseUrl + api);
            } else {
                return $http.post(this.baseUrl + api, vm);
            }
        };
    };
}