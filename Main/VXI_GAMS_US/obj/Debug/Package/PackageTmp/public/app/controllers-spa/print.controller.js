app.controller("PrinterController", printCtrl);
printCtrl.$inject = ["$rootScope", "$scope", "$http", "DTOptionsBuilder", "DTColumnBuilder", "ApiFactory", "$timeout"];
function printCtrl($rootScope, $scope, $http, DTOptionsBuilder, DTColumnBuilder, ApiFactory, $timeout) {
    $scope.isSaveOngoing = false;
    var defaultGuid = "00000000-0000-0000-0000-000000000000";
    $scope.Assets = [];
    $scope.PrintView = [];
    $scope.Categories = [];
    $scope.SubCategories = [];
    $scope.Statuses = [];
    $scope.Manufacturers = [];

    $scope.NoImageFile = window.url + "/public/image/img_placeholder.png";
    $rootScope.checkUrl();
    $scope.factory = new ApiFactory();
    $scope.AssetVm = {
        Id: null,
        Code: null,
        QrCode: null,
        Description: null,
        SerialNo: null,
        Category: null,
        SubCategory: null,
        Manufacturer: null,
        Status: null,
        IsActive: null,
        CreatedBy: null,
        DateCreated: null,
        UpdatedBy: null,
        DateUpdated: null
    };
    $scope.AssetRaw = {
        Id: null,
        Code: null,
        QrCode: null,
        Description: null,
        SerialNo: null,
        Category: null,
        SubCategory: null,
        Manufacturer: null,
        Status: null,
        IsActive: null,
        CreatedBy: null,
        DateCreated: null,
        UpdatedBy: null,
        DateUpdated: null
    };
    $scope.apply = function () {
        try {
            $scope.$apply();
        } catch (e) {
            //ignore
        }
    };
    $scope.Site = "All";
    $scope.Sites = ["All"];
    $scope.WorkTypes = ["WAH", "WAS"];
    $scope.Template = {
        Code: "",
        Url: ""
    };
    $scope.Templates = [
        {
            Code: "NEW ASSET",
            Url: url + "public/template/NewAsset.xlsx"
        },
        {
            Code: "ASSIGNMENT ASSET",
            Url: url + "public/template/AssetAssign.xlsx"
        },
        {
            Code: "UPDATE ASSET",
            Url: url + "public/template/AssetStatusUpdate.xlsx"
        },
        {
            Code: "TRANSFER ASSET",
            Url: url + "public/template/AssetTransfer.xlsx"
        },
        {
            Code: "REPLACE ASSET",
            Url: url + "public/template/AssetReplace.xlsx"
        }
    ];
    $scope.resizeWindow = function () {
        //setTimeout(function () { $(".page-loader-wrapper").fadeIn(); }, 1);
        //setTimeout(function () {
        //    var height = $("#spa-view-container").height() + $("#spa-menus").height() + 50 /*+ 150*/;
        //    $("#spa-container").css("height", height + "px");
        //    setTimeout(function () { $(".page-loader-wrapper").fadeOut(); }, 1);
        //}, 100);
        setTimeout(function () { $(".page-loader-wrapper").fadeOut(); }, 1);
    };
    $scope.resizeWindow();
    window.onresize = $scope.resizeWindow;
    $scope.error = function (e) {
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
    function lookupHtml(data) {
        data = data || {};
        return data.Code || "";
    }

    $scope.UploadingFile = {
        File: null
    };
    $scope.setFile = function (event) {
        $scope.UploadingFile.File = event.target.files[0];
        $scope.apply();
        $scope.uploadTemplateData();
    };
    $scope.dtOptions = DTOptionsBuilder
        .newOptions()
        .withColumnFilter({
            sPlaceHolder: "head:before",
            aoColumns:[]
        })
        .withDOM("<lf<t>ip>");
    // ReSharper disable once UseOfImplicitGlobalInFunctionScope
    $scope.dtOptions_asset = DTOptionsBuilder.newOptions()
        .withOption("ajax",
            {
                url: globalApiBaseUrl.concat("AssetApi/", defaultGuid, "/table"),
                type: "POST",
                reloadData: function () {
                    this.reload = true;
                    return this;
                },
                complete: function (xhr) {
                    //vm.requestData = xhr.responseJSON.data || [];
                    $scope.resizeWindow();
                }
            })
        .withDataProp("data")
        .withOption("processing", true)
        .withOption("language", { "processing": "Loading... Please wait..." })
        .withOption("serverSide", true)
        .withPaginationType("full_numbers")
        .withOption("bSort", false)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        //.withColumnFilter({
        //    aoColumns: [
        //        {
        //            type: "text"
        //        },
        //        {
        //            type: "text"
        //        },
        //        {
        //            type: "text"
        //        },
        //        {
        //            type: "text"
        //        },
        //        {
        //            type: "text"
        //        },
        //        {
        //            type: "text"
        //        }]
        //})
        .withOption("info", false);
    $scope.dtColumns_asset = [
        DTColumnBuilder.newColumn("Code").withTitle("REF.NO #").withClass("td-reqno"),
        DTColumnBuilder.newColumn("Description").withTitle("DESCRIPTION").withClass("td-subject"),
        DTColumnBuilder.newColumn("SerialNo").withTitle("SERIAL").withClass("td-requestor"),
        DTColumnBuilder.newColumn("Category").withTitle("CATEGORY").withClass("td-subject").renderWith(lookupHtml),
        DTColumnBuilder.newColumn("SubCategory").withTitle("SUBCATEGORY").withClass("td-subject").renderWith(lookupHtml),
        DTColumnBuilder.newColumn("Manufacturer").withTitle("MANUFACTURER").withClass("td-subject").renderWith(lookupHtml)
    ];

    $scope.save = function () {
        if ($scope.AssetVm.Id === null) {
            $scope.factory.setData("assetapi", $scope.AssetVm)
                .then(function () {
                    new ToastService().success("Record has been saved");
                }, $scope.error);
        } else {
            $scope.factory.putData("assetapi", $scope.AssetVm)
                .then(function () {
                    new ToastService().success("Record has been updated");
                }, $scope.error);
        }
    };

    $scope.downloadTemplate = function () {
        window.open(url + "public/template/PrintSerials.xlsx");
    };

    $scope.openUploader = function () {
        $("#asset-bulk-container").trigger("click");
    };

    $scope.resetUploading = function () {
        $scope.isSaveOngoing = false;
        $scope.UploadIsOnGoing = false;
        $scope.UploadingFile.File = null;
        $scope.apply();
        $("[type=file]").val(null);
    };

    $scope.uploadTemplateData = function () {
        try {
            $scope.isSaveOngoing = true;
            if ($scope.UploadingFile.File === null) {
                throw "Select a file to upload";
            }
            var formData = new FormData();
            angular.forEach($scope.UploadingFile,
                function (value, key) {
                    value = value || null;
                    if (key === "File") {
                        if (value !== null) {
                            formData.append(key, value);
                        }
                    }
                });
            $scope.apply();
            $scope.factory.uploadData("assetapi/".concat(defaultGuid, "/printserial"), formData)
                .then(function (res) {
                    $scope.Assets = angular.copy(res.data);
                    $scope.resetUploading();
                    new ToastService().success("Serial No. has been uploaded successfuly");
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

    $scope.getCategories = function () {
        $scope.factory.getData("categoryapi")
            .then(function (res) {
                console.log(res.data);
                $scope.Categories = angular.copy(res.data);
            }, $scope.error);
    };

    $scope.getSubCategories = function () {
        $scope.factory.getData("subcategoryapi")
            .then(function (res) {
                console.log(res.data);
                $scope.SubCategories = angular.copy(res.data);
            }, $scope.error);
    };

    $scope.getStatus = function () {
        $scope.factory.getData("statusapi")
            .then(function (res) {
                console.log(res.data);
                $scope.Statuses = angular.copy(res.data);
            }, $scope.error);
    };

    $scope.getManufacturer = function () {
        $scope.factory.getData("manufacturerapi")
            .then(function (res) {
                console.log(res.data);
                $scope.Manufacturers = angular.copy(res.data);
            }, $scope.error);
    };

    $scope.getSites = function () {
        $scope.factory.setData("assetapi/".concat(defaultGuid, "/sites"))
            .then(function (res) {
                $scope.Sites = angular.copy(res.data);
                $scope.Sites.unshift("All");
                //$scope.apply();
            }, $scope.error);
    };

    $scope.getAssets = function () {
        $scope.isSaveOngoing = true;
        $scope.PrintView = [];
        $scope.factory.setData("assetapi/".concat(defaultGuid, "/print"), { Site: $scope.Site })
            .then(function (res) {
                $scope.isSaveOngoing = false;
                $scope.Assets = angular.copy(res.data);
                //$scope.apply();
                //$scope.PrintPage();
            }, $scope.error);
    };

    $scope.getImagePath = function (imgId) {
        var final = url.concat("/qrcodes/", imgId, ".png");
        return final;
    };

    $scope.load = function () {
        try {
            scanner.stop();
        } catch (e) {
            //ignore
        }
        setTimeout(function () { $('.page-loader-wrapper').fadeOut(); }, 50);
        //$scope.getCategories();
        //$scope.getSubCategories();
        //$scope.getStatus();
        //$scope.getManufacturer();
        $scope.getSites();
    };
    $scope.load();

    $scope.clear = function () {
        $scope.AssetVm = window.angular.copy($scope.AssetRaw);
    };


    $scope.printQrCodes = function () {
        //readyToPrint(document.getElementById("printThis"));
        //$("#print").modal("hide");
        //setTimeout(window.print(), 5000);
    };

    $scope.AddToPrintPage = function () {
        $scope.PrintView = [];
        //$scope.TmpSchedules = [];
        //$scope.TmpSelected = [];
        var size = 1;
        //for (var k = 0; k < $scope.ListOfSchedules.length; k++) {
        //    for (var l = 0; l < $scope.ListOfSchedules[k].vehicles.length; l++) {
        //        for (var m = 0; m < $scope.ListOfSchedules[k].vehicles[l].schedules.length; m++) {
        //            var isSelected = $scope.ListOfSchedules[k].vehicles[l].schedules[m].selected;
        //            if (isSelected) {
        //                $scope.TmpSelected.push(angular.copy($scope.ListOfSchedules[k].vehicles[l].schedules[m]));
        //            } else {
        //                $scope.TmpSchedules.push(angular.copy($scope.ListOfSchedules[k].vehicles[l].schedules[m]));
        //            }
        //        }
        //    }
        //}
        //if ($scope.TmpSelected.length > 0) {
        //    $scope.TmpSchedules = window.angular.copy($scope.TmpSelected);
        //}
        //while ($scope.TmpSchedules.length > 0) {
        //    $scope.PrintView.push($scope.TmpSchedules.splice(0, size));
        //}
        var selectedAssets = angular.copy($scope.Assets.filter(function (x) { return x.Selected === true; }));
        if (selectedAssets.length < 1) {
            selectedAssets = angular.copy($scope.Assets);
        }
        while (selectedAssets.length > 0) {
            $scope.PrintView.push(selectedAssets.splice(0, size));
        }
        $scope.apply();
        //readyToPrint(document.getElementById("printThis"));
        //$("#print").modal("show");
        //window.print();
    }
    $scope.PrintPage = function () {
        if ($scope.PrintView.length < 1) {
            $scope.AddToPrintPage();
        }
        window.print();
    }
    $scope.Printables = 0;
    $scope.$watch("Assets", function () {
        $scope.Printables = $scope.Assets.filter(function (x) { return x.Selected === true; }).length;
        $scope.AddToPrintPage();
    }, true);
}