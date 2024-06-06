app.factory("ApiFactory", ApiFactory);
ApiFactory.$inject = ["$http", "Settings"];
function ApiFactory($http, Settings) {
    return function () {
        this.baseUrl = globalApiBaseUrl;


        this.getData = function (api) {
            //return $http.get(this.baseUrl + api + "?_=" + new Date().getTime());
            return $http.get(this.baseUrl + api);
        };


        this.setData = function (api, vm) {
            if (vm === undefined || vm === null) {
                return $http.post(this.baseUrl + api);
            } else {
                return $http.post(this.baseUrl + api, vm);
            }
        };

        this.uploadData = function (api, vm) {
            return $http.post(this.baseUrl + api, vm, Settings.Options.Upload);
        };

        this.uploadDataUpdate = function (api, vm) {
            return $http.put(this.baseUrl + api, vm, Settings.Options.Upload);
        };

        this.putData = function (api, vm) {
            if (vm === undefined || vm === null) {
                return $http.put(this.baseUrl + api);
            } else {
                return $http.put(this.baseUrl + api, vm);
            }
        };

        this.deleteData = function (api, vm) {
            if (vm === undefined || vm === null) {
                return $http.delete(this.baseUrl + api);
            } else {
                return $http.delete(this.baseUrl + api, vm, { "Content-Type": "application/json"});
        }
    };
};
}