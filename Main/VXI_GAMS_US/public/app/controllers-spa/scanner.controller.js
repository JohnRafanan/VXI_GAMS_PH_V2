app.controller("ScannerController", scanCtrl);
scanCtrl.$inject = ["$rootScope", "$scope", "$http", "DTOptionsBuilder", "DTColumnBuilder", "ApiFactory", "$timeout"];
function scanCtrl($rootScope, $scope, $http, DTOptionsBuilder, DTColumnBuilder, ApiFactory, $timeout) {
    $scope.isSaveOngoing = false;
    $scope.NoImageFile = window.url + "/public/image/img_placeholder.png";
    $scope.selectedItems = 0;
    $rootScope.checkUrl();
    $scope.resizeWindow = function () {
        var height = $("#spa-view-container").height() + $("#spa-menus").height() + 150;
        $("#spa-container").css("height", height + "px");
        setTimeout(function () { $(".page-loader-wrapper").fadeOut(); }, 50);
    };
    $scope.factory = new ApiFactory();
    $scope.resizeWindow();
    window.onresize = $scope.resizeWindow;
    $scope.forceApply = function () {
        try {
            $scope.$apply();
        } catch (e) {
            //ignore
        }
    };

    $scope.error = function (e) {
        $scope.loadingHistory = false;
        initCamera(cameraId);
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

    $scope.Categories = [];
    $scope.getCategory = function () {
        $scope.Categories = [];
        $scope.factory.getData("CategoryApi")
            .then(function (res) {
                if (res.data && Array.isArray(res.data)) {
                    for (var i = 0; i < res.data.length; i++) {
                        $scope.Categories.push(res.data[i].Code);
                    }
                }
                $scope.Categories = $scope.Categories.sort();
            }, $scope.error);
    };

    $scope.SubCategories = [];
    $scope.getSubCategory = function () {
        $scope.SubCategories = [];
        $scope.factory.getData("SubCategoryApi")
            .then(function (res) {
                if (res.data && Array.isArray(res.data)) {
                    for (var i = 0; i < res.data.length; i++) {
                        $scope.SubCategories.push(res.data[i].Code);
                    }
                }
                $scope.SubCategories = $scope.SubCategories.sort();
            }, $scope.error);
    };

    $scope.Statuses = [];
    $scope.getStatus = function () {
        $scope.Statuses = [];
        $scope.factory.getData("StatusApi")
            .then(function (res) {
                if (res.data && Array.isArray(res.data)) {
                    for (var i = 0; i < res.data.length; i++) {
                        $scope.Statuses.push(res.data[i].Code);
                    }
                }
                $scope.Statuses = $scope.Statuses.sort();
            }, $scope.error);
    };

    $scope.Manufacturers = [];
    $scope.getManufacturer = function () {
        $scope.Manufacturers = [];
        $scope.factory.getData("ManufacturerApi")
            .then(function (res) {
                if (res.data && Array.isArray(res.data)) {
                    for (var i = 0; i < res.data.length; i++) {
                        $scope.Manufacturers.push(res.data[i].Code);
                    }
                }
                $scope.Manufacturers = $scope.Manufacturers.sort();
            }, $scope.error);
    };

    $scope.Sites = [];
    $scope.WorkTypes = ["WAS", "WAH"].sort();
    $scope.getSite = function () {
        $scope.Sites = [];
        $scope.factory.getData("SiteApi")
            .then(function (res) {
                if (res.data && Array.isArray(res.data)) {
                    for (var i = 0; i < res.data.length; i++) {
                        $scope.Sites.push(res.data[i].Location);
                    }
                }
                $scope.Sites = $scope.Sites.sort();
            }, $scope.error);
    };

    $scope.QrCodeData = "";
    scanStart(function (data) {
        stopCamera();
        return new Promise(function (resolve, reject) {
            try {
                if ($scope.QrCodeData !== data) {
                    $scope.QrCodeData = data;
                }
                $(".page-loader-wrapper").fadeIn();
                $scope.getQrCodeData($scope.QrCodeData);
            } catch (e) {
                //ignored
                setTimeout(function () { $(".page-loader-wrapper").fadeOut(); }, 50);
            }
        });
    });

    $scope.openSearchEmployee = function () {
        $scope.Search.Hrid = null;
        $scope.Search.Result = {};
        $("#employeeModal").modal("show");
    };

    $scope.Search = {
        Hrid: null,
        Result: {}
    };

    $scope.searchEmployee = function (data) {
        $scope.Search.Result = {};
        try {
            data = data || "101";
            $scope.factory.getData(`EmployeeApi/${data}/search`)
                .then(function (res) {
                    $scope.Search.Result = res.data;
                }, $scope.error);
        } catch (e) {
            initCamera(window.cameraId);
            new ToastService().error("Something went wrong");
        }
    };

    $scope.Transfer = {
        Site: null,
        TrackingNo: null,
        TicketNo: null,
    };

    $scope.TrackTicket = {
        TrackingNo: null,
        TicketNo: null,
    };

    $scope.Assign = {
        WorkType: null,
        Hrid: null,
        Email: null,
        ContactNo: null,
        Address: null,
        Floor: null,
        Area: null,
        TrackingNo: null,
        TicketNo: null,
    };

    $scope.checkOpenModals = function () {
        $timeout(() => {
            if ($(".modal-stack").length > 0) {
                $("body").addClass("modal-open");
            }
        }, 500);
    };

    $scope.$watch("Search.Hrid", function (newValue, oldValue, scope) {
        if (newValue !== null) {
            $scope.searchEmployee(newValue);
        }
    }, true);

    $scope.getQrCodeData = function (data) {
        $scope.Asset = {};
        try {
            $scope.factory.setData("scanner", { Item: data })
                .then(function (res) {
                    //DISPLAYING TO HTML
                    $scope.Transfer = window.angular.copy(res.data);
                    $scope.Assign = window.angular.copy(res.data);

                    const tmp = window.angular.copy(res.data);

                    $scope.Asset = window.angular.copy(res.data);

                    
                }, $scope.error);
        } catch (e) {
            new ToastService().error("Something went wrong");
        } finally {
            initCamera(window.cameraId);
            setTimeout(function () { $(".page-loader-wrapper").fadeOut(); }, 50);
        }
    };

    $scope.openHistoryModal = function () {
        if ($scope.Asset.Id && $scope.Asset.Id.length === 36) {
            $scope.getHistory();
            $("#assetHistoryModal").modal("show");
        } else {
            new ToastService().error("Invalid Asset");
        }
    };

    $scope.updateAsset = function () {
        $(".page-loader-wrapper").fadeIn();
        try {
            $scope.factory.setData("AssetApi/" + $scope.Asset.Id, $scope.Asset)
                .then(function () {
                    new ToastService().info("Update Complete");
                    $scope.getQrCodeData($scope.QrCodeData);
                }, $scope.error);
        } catch (e) {
            new ToastService().error("Something went wrong");
        } finally {
            setTimeout(function () { $(".page-loader-wrapper").fadeOut(); }, 50);
        }
    };

    $scope.removeAssignAsset = function () {
        if (confirm("Are you sure?")) {
            $(".page-loader-wrapper").fadeIn();
            try {
                $scope.factory.setData(`AssetApi/${$scope.Asset.Id}/remove`, $scope.TrackTicket)
                    .then(function () {
                        new ToastService().info("Removal Complete");
                        $scope.getQrCodeData($scope.QrCodeData);
                    }, $scope.error);
            } catch (e) {
                new ToastService().error("Something went wrong");
            } finally {
                setTimeout(function () { $(".page-loader-wrapper").fadeOut(); }, 50);
            }
        }
    };

    $scope.useEmployee = function () {
        $scope.Search.Result = $scope.Search.Result || {};
        $scope.Assign.Hrid = $scope.Search.Result.ID || null;
        $scope.Asset.EmployeeName = `${$scope.Search.Result.FirstName} ${$scope.Search.Result.LastName}`;
        $scope.checkOpenModals();
    };

    $scope.assignAssetToEmployee = function () {
        $(".page-loader-wrapper").fadeIn();
        try {
            $scope.factory.setData(`AssetApi/${$scope.Asset.Id}/assign`, $scope.Assign)
                .then(function () {
                    new ToastService().info("Assignment Complete");
                    $scope.getQrCodeData($scope.QrCodeData);
                }, $scope.error);
        } catch (e) {
            initCamera(window.cameraId);
            new ToastService().error("Something went wrong");
        } finally {
            setTimeout(function () { $(".page-loader-wrapper").fadeOut(); }, 50);
        }
    };

    $scope.transferAsset = function () {
        if (confirm("Are you sure?")) {
            $(".page-loader-wrapper").fadeIn();
            try {
                $scope.factory.setData(`AssetApi/${$scope.Asset.Id}/transfer`, $scope.Transfer)
                    .then(function () {
                        new ToastService().info("Transfer Complete");
                        $scope.getQrCodeData($scope.QrCodeData);
                    }, $scope.error);
            } catch (e) {
                initCamera(window.cameraId);
                new ToastService().error("Something went wrong");
            } finally {
                setTimeout(function () { $(".page-loader-wrapper").fadeOut(); }, 50);
            }
        }
    };
    $scope.Histories = [];
    $scope.loadingHistory = false;
    $scope.getHistory = function () {
        $scope.Histories = [];
        $scope.loadingHistory = true;
        try {
            $scope.factory.getData(`AssetApi/${$scope.Asset.Id}/history`)
                .then(function (res) {
                    $scope.loadingHistory = false;
                    $scope.Histories = res.data;
                }, $scope.error);
        } catch (e) {
            new ToastService().error("Something went wrong");
        } finally {
            $scope.loadingHistory = false;
        }
    };

    $scope.load = function () {
        try {
            scanner.stop();
        } catch (e) {
            //ignore
        }
        $scope.getCategory();
        $scope.getSubCategory();
        $scope.getStatus();
        $scope.getManufacturer();
        $scope.getSite();
        ////for testing, remove on production
        //$scope.QrCodeData = "aouhTbfI4UVBIGXcCc_yUiLe3xcgc4alngfwjux24Hwt6e0qRgr21A2";
        //$timeout(() => {
        //    $scope.getQrCodeData("aouhTbfI4UVBIGXcCc_yUiLe3xcgc4alngfwjux24Hwt6e0qRgr21A2");
        //}, 1500);
    };
    $scope.load();
}