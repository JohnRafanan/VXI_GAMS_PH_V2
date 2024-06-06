// ReSharper disable all StringLiteralTypo
app.controller("ManageController", manageCtrl);
manageCtrl.$inject = ["$rootScope", "$scope", "$http", "DTOptionsBuilder", "DTColumnBuilder", "ApiFactory", "$timeout"];
function manageCtrl($rootScope, $scope, $http, DTOptionsBuilder, DTColumnBuilder, ApiFactory, $timeout) {
    $scope.isSaveOngoing = false;
    var defaultGuid = "00000000-0000-0000-0000-000000000000";

    $scope.ManageType = "0";
    $scope.Loading = false;

    $scope.Categories = [];
    $scope.SubCategories = [];
    $scope.Statuses = [];
    $scope.Manufacturers = [];
    $scope.Vendors = [];

    $scope.Templates = [
        {
            Code: "NEW ASSET (FARM IN)",
            Url: `public/template/NewAsset(FarmIn).xlsx`
        },

        {
            Code: "FARM OUT",
            Url: `public/template/FarmOut.xlsx`
        },

        {
            Code: "ASSET DETAILS",
            Url: `public/template/AssetDetails.xlsx`
        },

        {
            Code: "ASSET ASSIGNMENT",
            Url: `public/template/AssetAssignment.xlsx`
        },

        {
            Code: "ASSET UPDATE",
            Url: `public/template/AssetUpdate.xlsx`
        },

        {
            Code: "ASSET REPLACEMENT",
            Url: `public/template/AssetReplacement.xlsx`
        },

        //{
        //    Code: "NEW ASSET",
        //    Url: url + "public/template/NewAsset.xlsx"
        //},
        //{
        //    Code: "ASSIGNMENT ASSET",
        //    Url: url + "public/template/AssetAssign.xlsx"
        //},
        //{
        //    Code: "UPDATE ASSET",
        //    Url: url + "public/template/AssetStatusUpdate.xlsx"
        //},
        //{
        //    Code: "TRANSFER ASSET",
        //    Url: url + "public/template/AssetTransfer.xlsx"
        //},
        //{
        //    Code: "REPLACE ASSET",
        //    Url: url + "public/template/AssetReplace.xlsx"
        //}
    ];

    $scope.Files = [];

    $scope.setFile = function (event) {
        $scope.Files = event.target.files;
        $scope.apply();
        $scope.uploadTemplateData();
    };

    $rootScope.checkUrl();
    $scope.factory = new ApiFactory();

    $scope.apply = function () {
        try {
            $scope.$apply();
        } catch (e) {
            //ignore
        }
    };

    $scope.error = function (e) {
        $scope.Loading = false;
        $scope.resetUploading();
        var isObj = typeof e;
        if (isObj === "object") {
            try {
                new ToastService().error(e.data.Message);
            } catch (e) {
                new ToastService().error(e.Message);
            }
        } else {
            new ToastService().error(e);
        }
    };

    $scope.dateToLocal = function (datetime) {
        datetime = datetime || "";
        if (datetime.length > 0) {
            return moment(new Date(moment.utc(datetime).format())).format("MM/DD/YYYY LT");
        } else {
            return datetime;
        }
    };

    $scope.getCategories = function () {
        $scope.Loading = true;
        $scope.Categories = angular.copy([]);
        $scope.factory.getData("categoryapi")
            .then(function (res) {
                $scope.Loading = false;
                $scope.Categories = angular.copy(res.data);
            }, $scope.error);
    };

    $scope.Category = {};
    $scope.saveCategory = function () {
        $scope.factory.setData("categoryapi", $scope.Category)
            .then(function () {
                $scope.Category = {};
                $scope.getCategories();
            }, $scope.error);
    };

    $scope.newData = function () {
        var type = $scope.ManageType;
        if (type === "0") {
            $scope.Category = {};
        }
        if (type === "1") {
            $scope.SubCategory = {};
        }
        if (type === "2") {
            $scope.Manufacturer = {};
        }
        if (type === "5") {
            $scope.Vendors = {};
        }
        $scope.apply();
    };

    $scope.editData = function (id) {
        var type = $scope.ManageType;
        try {
            if (type === "0") {
                $scope.Category = JSON.parse(JSON.stringify($scope.Categories.filter(function (item) { return item.Id === id; })[0]));
            }
            if (type === "1") {
                $scope.SubCategory = JSON.parse(JSON.stringify($scope.SubCategories.filter(function (item) { return item.Id === id; })[0]));
            }
            if (type === "2") {
                $scope.Manufacturer = JSON.parse(JSON.stringify($scope.Manufacturers.filter(function (item) { return item.Id === id; })[0]));
            }
            if (type === "5") {
                $scope.Vendors = JSON.parse(JSON.stringify($scope.Vendors.filter(function (item) { return item.Id === id; })[0]));
            }
        } catch (e) {
            alert("Data not found. Kindly refresh the page.");
        }
    };

    $scope.deleteData = function (id) {
        var type = $scope.ManageType;
        var vm = { Id: defaultGuid };
        var apiUri = "";
        try {
            if (type === "0") {
                vm = JSON.parse(JSON.stringify($scope.Categories.filter(function (item) { return item.Id === id; })[0]));
                apiUri = "categoryapi";
            } else if (type === "1") {
                vm = JSON.parse(JSON.stringify($scope.SubCategories.filter(function (item) { return item.Id === id; })[0]));
                apiUri = "subcategoryapi";
            } else if (type === "2") {
                vm = JSON.parse(
                    JSON.stringify($scope.Manufacturers.filter(function (item) { return item.Id === id; })[0]));
                apiUri = "manufacturerapi";
            } else if (type === "5") {
                vm = JSON.parse(
                    JSON.stringify($scope.Vendors.filter(function (item) { return item.Id === id; })[0]));
                apiUri = "vendorsapi";
            } else {
                throw "Invalid Type";
            }
            $scope.factory.deleteData(apiUri + "/" + vm.Id)
                .then(function () {
                    if (type === "0") {
                        $scope.getCategories();
                    }
                    else if (type === "1") {
                        $scope.getSubCategories(vm.CategoryId);
                    }
                    else if (type === "2") {
                        $scope.getManufacturer();
                    }
                    else if (type === "5") {
                        $scope.getVendors();
                    }
                    $("#defaultModal").modal("hide");
                }, $scope.error);
        } catch (e) {
            alert("Data not found. Kindly refresh the page.");
        }
    };

    $scope.saveData = function () {
        var type = $scope.ManageType;
        var vm = {};
        var apiUri = "";
        try {
            if (type === "0") {
                vm = $scope.Category;
                apiUri = "categoryapi";
            }
            else if (type === "1") {
                vm = $scope.SubCategory;
                vm.CategoryId = $scope.Category.Id;
                apiUri = "subcategoryapi";
            }
            else if (type === "2") {
                vm = $scope.Manufacturer;
                apiUri = "manufacturerapi";
            }
            else if (type === "5") {
                vm = $scope.Vendors;
                apiUri = "vendorsapi";
            }
            else {
                throw "Invalid Type";
            }
            $scope.factory.setData(apiUri, vm)
                .then(function () {
                    if (type === "0") {
                        $scope.getCategories();
                    }
                    else if (type === "1") {
                        $scope.getSubCategories(vm.CategoryId);
                    }
                    else if (type === "2") {
                        $scope.getManufacturer();
                    }
                    else if (type === "5") {
                        $scope.getVendors();
                    }
                    $("#defaultModal").modal("hide");
                }, $scope.error);
        } catch (e) {
            alert(e);
        }
    };

    $scope.getSubCategories = function (id) {
        id = id || defaultGuid;
        if (id == defaultGuid) {
            $scope.Category = {};
        }
        $scope.Loading = true;
        $scope.SubCategories = angular.copy([]);
        $scope.factory.getData("subcategoryapi/" + id)
            .then(function (res) {
                $scope.Loading = false;
                $scope.SubCategories = res.data;
                try {
                    $scope.Category = JSON.parse(JSON.stringify($scope.Categories.filter(function (item) { return item.Id === id; })[0]));
                } catch (e) {
                    //ignore
                }
                $scope.apply();
            }, $scope.error);
    };

    $scope.SubCategory = {};
    $scope.saveSubCategory = function () {
        $scope.factory.setData("subcategoryapi", $scope.SubCategory)
            .then(function () {
                $scope.SubCategory = {};
                $scope.getSubCategories();
            }, $scope.error);
    };

    $scope.getStatus = function () {
        $scope.Statuses = angular.copy([]);
        $scope.factory.getData("statusapi")
            .then(function (res) {
                //console.log(res.data);
                $scope.Statuses = angular.copy(res.data);
            }, $scope.error);
    };

    $scope.getManufacturer = function () {
        $scope.Loading = true;
        $scope.Manufacturers = angular.copy([]);
        $scope.factory.getData("manufacturerapi")
            .then(function (res) {
                $scope.Loading = false;
                $scope.Manufacturers = angular.copy(res.data);
            }, $scope.error);
    };

    $scope.getVendors = function () {
        $scope.Loading = true;
        $scope.Vendors = angular.copy([]);
        $scope.factory.getData("vendorsapi")
            .then(function (res) {
                $scope.Loading = false;
                $scope.Vendors = angular.copy(res.data);
            }, $scope.error);
    };

    $scope.Manufacturer = {};
    $scope.saveManufacturer = function () {
        $scope.factory.getData("manufacturerapi", $scope.Manufacturer)
            .then(function () {
                $scope.Manufacturer = {};
                $scope.getManufacturer();
            }, $scope.error);
    };
    $scope.Vendors = {};
    $scope.saveVendors = function () {
        $scope.factory.getData("vendorsapi", $scope.Manufacturer)
            .then(function () {
                $scope.Vendors = {};
                $scope.getVendors();
            }, $scope.error);
    };

    $scope.load = function () {
        try {
            scanner.stop();
        } catch (e) {
            //ignore
        }
        setTimeout(function () { $('.page-loader-wrapper').fadeOut(); }, 50);
        $scope.getCategories();
        //$scope.getSubCategories();
        //$scope.getStatus();
        //$scope.getManufacturer();
    };
    $scope.load();

    $scope.$watch("Category", function (newValue, oldValue, scope) {
        console.log("newValue: ", newValue);
        console.log("oldValue: ", oldValue);
        console.log("scope: ", scope);
        $scope.apply();
    }, false);

    $scope.resetUploading = function () {
        $("[type=file]").val(null);
        $scope.UploadIsOnGoing = false;
        $scope.Files = [];
        $scope.apply();
    };

    $scope.UploadIsOnGoing = false;
    $scope.uploadTemplateData = function () {
        try {
            $scope.UploadIsOnGoing = true;
            if ($scope.Files.length < 1) {
                throw "Select a file to upload";
            }
            var formData = new FormData();
            angular.forEach($scope.Files,
                function (value, key) {
                    value = value || null;
                    if (value !== null && (value.name !== undefined || value.name !== null || value.name !== "")) {
                        formData.append(value.name, value);
                    }
                });
            $scope.apply();
            $scope.factory.uploadData("templateapi", formData)
                .then(function () {
                    $scope.resetUploading();
                    new ToastService().success("Template upload complete. Kindly inform the other Users to download the updated template.");
                }, $scope.error);
        } catch (e) {
            $scope.resetUploading();
            var isObj = typeof e;
            if (isObj === "object") {
                new ToastService().error(e.message);
            } else {
                new ToastService().error(e);
            }
        }
    };
}