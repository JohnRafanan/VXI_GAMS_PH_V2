function _toConsumableArray(arr) { return _arrayWithoutHoles(arr) || _iterableToArray(arr) || _nonIterableSpread(); }

function _nonIterableSpread() { throw new TypeError("Invalid attempt to spread non-iterable instance"); }

function _iterableToArray(iter) { if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter); }

function _arrayWithoutHoles(arr) { if (Array.isArray(arr)) { for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) { arr2[i] = arr[i]; } return arr2; } }

app.controller("manageCtrl", manageCtrl);
manageCtrl.$inject = ["$rootScope", "$scope", "$http", "DTOptionsBuilder", "DTColumnBuilder", "ApiFactory", "$timeout"];
function manageCtrl($rootScope, $scope, $http, DTOptionsBuilder, DTColumnBuilder, ApiFactory, $timeout) {
    var vm = this;
    var defaultGuid = "00000000-0000-0000-0000-000000000000";
    var btn = $("#btn-click,#btn-click2");
    vm.factory = new ApiFactory();
    vm.isSaveOngoing = false;

    vm.Categories = [];
    vm.Sites = [];
    vm.Lobs = [];

    vm.AddUser = {
        CategoryId: 0,
        SiteId: 0,
        LobId: 0,
        NtAccounts: "",
        InEmailCc: false,
        WithDownloadTab: false
    };

    vm.SelectedCategory = {
        Id: 0
    };
    vm.SelectedSite = {
        Id: 0
    };
    vm.SelectedLob = {
        Id: 0
    };
    vm.NtAccounts = "";

    vm.InEmailCc = false;
    vm.WithDownloadTab = false;
    vm.SelectedRole = {
        Id: undefined
    };

    function error(err) {
        vm.isSaveOngoing = false;
        try {
            new ToastService().error(err.data.Message);
        } catch (e) {
            new ToastService().error("An error occured");
        }
        btn.removeAttr("disabled");
    }

    vm.apply = function () {
        try {
            $scope.$apply();
        } catch (e) {
            //ignore
        }
    };

    vm.getCategories = function () {
        vm.factory.getData("ManageApi/".concat(defaultGuid, "/getcategories"))
            .then(function (res) {
                vm.Categories = [];
                vm.Categories = JSON.parse(JSON.stringify(res.data));
                vm.apply();
                setTimeout(function () {
                    $("#select-category").selectpicker("refresh");
                }, 1000);
            }, error);
    };

    vm.getSurveys = function () {
        vm.factory.getData("ManageApi/".concat(defaultGuid, "/getsurveys"))
            .then(function (res) {
                vm.Surveys = [];
                vm.Surveys = JSON.parse(JSON.stringify(res.data));
                $scope.example1data = JSON.parse(JSON.stringify(res.data));
                vm.apply();
                setTimeout(function () {
                    $("#select-survey").selectpicker("refresh");
                }, 1000);
            }, error);
    };

    vm.getSites = function () {
        vm.factory.getData("ManageApi/".concat(defaultGuid, "/getsites"))
            .then(function (res) {
                vm.Sites = [];
                vm.Sites = JSON.parse(JSON.stringify(res.data));
                vm.apply();
                setTimeout(function () {
                    $("#select-site").selectpicker("refresh");
                }, 1000);
            }, error);
    };

    vm.getLobs = function () {
        vm.factory.getData("ManageApi/".concat(defaultGuid, "/getlobs/", vm.SelectedSite.Id))
            .then(function (res) {
                vm.Lobs = [];
                vm.Lobs = JSON.parse(JSON.stringify(res.data));
                vm.apply();
                setTimeout(function () {
                    $("#select-lob").selectpicker("refresh");
                }, 1000);
            }, error);
    };

    vm.getRoles = function () {
        vm.factory.getData("ManageApi/".concat(defaultGuid, "/getroles"))
            .then(function (res) {
                vm.Roles = [];
                vm.Roles = JSON.parse(JSON.stringify(res.data));
                vm.apply();
                setTimeout(function () {
                    $("#select-role").selectpicker("refresh");
                }, 1000);
            }, error);
    };

    $scope.$watch("vm.SelectedSurvey", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        $("#select-survey").selectpicker("refresh");
    });

    $scope.$watch("vm.SelectedCategory", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        $("#select-category").selectpicker("refresh");
    });
    $scope.$watch("vm.SelectedRole", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        $("#select-role").selectpicker("refresh");
    });
    $scope.$watch("vm.SelectedSite", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        $("#select-site").selectpicker("refresh");
        vm.SelectedLob.Id = 0;
        vm.apply();
        vm.getLobs();
    });
    $scope.$watch("vm.SelectedLob", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        $("#select-lob").selectpicker("refresh");
    });
    $scope.$watch("vm.WithDownloadTab", function (newValue, oldValue) {
        vm.apply();
        setTimeout(function () {
            $("#select-role").selectpicker("refresh");
        }, 500);
    });

    vm.saveAccounts = function (type) {
        vm.isSaveOngoing = true;
        vm.NtAccounts = vm.NtAccounts || "";
        try {
            if(vm.SelectedSurvey === undefined)
                throw "Select a Survey";
            if (vm.NtAccounts.length < 1)
                throw "Add NT Account First";
            console.log("vm.SelectedSurvey: ", vm.SelectedSurvey);
            vm.AddUser = {
                Nt: vm.NtAccounts,
                Type: type,
                Surveys: vm.SelectedSurvey
            };
            vm.ErrorLogs = "";
            vm.apply();
            vm.factory.setData("ManageApi/".concat(defaultGuid, "/save"), vm.AddUser)
                .then(function (res) {
                    vm.isSaveOngoing = false;
                    console.log("res.data: ", res.data);
                    if (res.data !== undefined && res.data.length > 0) {
                        for (var i = 0; i < res.data.length; i++) {
                            vm.ErrorLogs += res.data[i] + "\r\n";
                        }
                        vm.ErrorLogs = vm.ErrorLogs.replace(/undefinedError/gi, "Error");
                        vm.apply();
                        console.log("vm.ErrorLogs:", vm.ErrorLogs);
                        new ToastService().error(vm.ErrorLogs);
                        return;
                    }
                    if (res.data.length < 1) {
                        new ToastService().info("Surveys have been added to Users.");
                    }
                }, error);
        } catch (e) {
            vm.isSaveOngoing = false;
            new ToastService().error(e);
        }
    };

    $scope.example1model = []; 
    $scope.example1data = [ {id: 1, label: "David"}, {id: 2, label: "Jhon"}, {id: 3, label: "Danny"} ];

    vm.onLoad = function () {
        vm.getSurveys();
    };
    vm.onLoad();
}