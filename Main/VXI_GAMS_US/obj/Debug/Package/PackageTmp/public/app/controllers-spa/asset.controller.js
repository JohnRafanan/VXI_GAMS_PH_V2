app.controller("AssetController", assetCtrlr);
assetCtrlr.$inject = ["$rootScope", "$scope", "$http", "DTOptionsBuilder", "DTColumnBuilder", "ApiFactory", "$timeout"];
function assetCtrlr($rootScope, $scope, $http, DTOptionsBuilder, DTColumnBuilder, ApiFactory, $timeout) {
    $scope.isSaveOngoing = false;
    var defaultGuid = "00000000-0000-0000-0000-000000000000";

    $scope.Categories = [];
    $scope.SubCategories = [];
    $scope.Statuses = [];
    $scope.Manufacturers = [];

    $scope.requestData = [];

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
    $scope.WorkTypes = ["All", "WAS", "WAH"];
    $scope.Template = {
        Code: "",
        Url: ""
    };
    $scope.Templates = [
        {
            Code: "NEW ASSET (FARM IN)",
            Url: `${url}template/File?name=NewAsset(FarmIn).xlsx`
        },

        {
            Code: "FARM OUT",
            Url: `${url}template/File?name=FarmOut.xlsx`
        },

        {
            Code: "ASSET DETAILS",
            Url: `${url}template/File?name=AssetDetails.xlsx`
        },

        {
            Code: "ASSET ASSIGNMENT",
            Url: `${url}template/File?name=AssetAssignment.xlsx`
        },

        {
            Code: "ASSET UPDATE",
            Url: `${url}template/File?name=AssetUpdate.xlsx`
        },

        {
            Code: "ASSET REPLACEMENT",
            Url: `${url}template/File?name=AssetReplacement.xlsx`
        },

        //{
        //    Code: "NEW ASSET",
        //    Url: `${url}template/File?name=NewAsset.xlsx`
        //},
        //{
        //    Code: "ASSET ASSIGNMENT",
        //    Url: `${url}template/File?name=AssetAssign.xlsx`
        //},
        //{
        //    Code: "ASSET UPDATE",
        //    Url: `${url}template/File?name=AssetStatusUpdate.xlsx`
        //},
        //{
        //    Code: "ASSET TRANSFER",
        //    Url: `${url}template/File?name=AssetTransfer.xlsx`
        //},
        //{
        //    Code: "ASSET REPLACEMENT",
        //    Url: `${url}template/File?name=AssetReplace.xlsx`
        //},
        //{
        //    Code: "ASSET CHANGE",
        //    Url: `${url}template/File?name=AssetChange.xlsx`
        //},
        //{
        //    Code: "ASSET ASSIGNMENT REMOVAL",
        //    Url: `${url}template/File?name=AssetEmployeeAssignmentRemoval.xlsx`
        //},
        //{
        //    Code: "EMPLOYEE VALIDATION",
        //    Url: `${url}template/File?name=EmployeeValidation.xlsx`
        //}
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
            } catch (er) {
                try {
                    new ToastService().error(e.Message);
                } catch (err) {
                    try {
                        new ToastService().error(err.Message);
                    } catch (e) {
                        new ToastService().error(err);
                    }
                }
            }
        } else {
            new ToastService().error(e);
        }
    };

    $scope.UploadingFile = {
        File: null
    };
    $scope.setFile = function (event) {
        $scope.UploadingFile.File = event.target.files[0];
        $scope.apply();
        $scope.uploadTemplateData();
    };

    // ReSharper disable once UseOfImplicitGlobalInFunctionScope
    $scope.dtOptions_asset = DTOptionsBuilder.newOptions()
        .withOption("ajax", {
            url: globalApiBaseUrl.concat("AssetApi/", defaultGuid, "/table?site=All"),
            type: "POST",
            reloadData: function () {
                this.reload = true;
                return this;
            },
            complete: function (xhr) {
                $scope.requestData = xhr.responseJSON.data || [];
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
        .withOption("language", {
            "processing": "Loading... Please wait...",
            "search": "SEARCH FOR [<strong>REF#, MODEL, SERIAL, NAME, FLOOR, AREA WORK STATION, ADDRESS, CONTACT, EMAIL, TRACKING, TICKET</strong>]:"
        })
        .withOption("info", true);

    $scope.dtColumns_asset = [
        DTColumnBuilder.newColumn("Id").withTitle("ID").withClass("td-id").withOption("visible", false),
        DTColumnBuilder.newColumn("Code").withTitle("REF. #").withClass("td-reqno"),
        DTColumnBuilder.newColumn("Model").withTitle("MODEL").withClass("td-subject"),
        DTColumnBuilder.newColumn("SerialNo").withTitle("SERIAL").withClass("td-serial"),
        DTColumnBuilder.newColumn("ItemDescription").withTitle("ITEM DESCRIPTION").withClass("td-serial"), /*New*/
        DTColumnBuilder.newColumn("Category").withTitle("CATEGORY").withClass("td-category"),
        DTColumnBuilder.newColumn("SubCategory").withTitle("SUBCATEGORY").withClass("td-category"),
        DTColumnBuilder.newColumn("Brand").withTitle("BRAND").withClass("td-manufacturer"),
        DTColumnBuilder.newColumn("Quantity").withTitle("QUANTITY").withClass("td-manufacturer"), /*New*/
        DTColumnBuilder.newColumn("Currency").withTitle("CURRENCY").withClass("td-manufacturer"), /*New*/
        DTColumnBuilder.newColumn("UnitPrice").withTitle("UNIT PRICE").withClass("td-manufacturer"), /*New*/
        DTColumnBuilder.newColumn("TotalValue").withTitle("TOTAL VALUE").withClass("td-manufacturer"), /*New*/
        DTColumnBuilder.newColumn("Vendor").withTitle("VENDOR").withClass("td-manufacturer"),
        DTColumnBuilder.newColumn("VendorAddress").withTitle("VENDOR ADDRESS").withClass("td-manufacturer"), /*New*/
        DTColumnBuilder.newColumn("PurchaseOrder").withTitle("P.O #").withClass("td-manufacturer"),
        DTColumnBuilder.newColumn("PurchaseOrderDate").withTitle("P.O DATE").withClass("td-manufacturer"), /*New*/
        DTColumnBuilder.newColumn("CostValue").withTitle("COST/VALUE").withClass("td-manufacturer"),
        DTColumnBuilder.newColumn("Ram").withTitle("RAM").withClass("td-ram"),
        //DTColumnBuilder.newColumn("HdCapacity").withTitle("HD CAPACITY").withClass("td-hdcapacity"),
        DTColumnBuilder.newColumn("MonitorSize").withTitle("SCREEN SIZE").withClass("td-monitorsize"),
        //DTColumnBuilder.newColumn("YearModel").withTitle("YEAR MODEL").withClass("td-yearmodel"),
        DTColumnBuilder.newColumn("Classification").withTitle("CLASSIFICATION").withClass("td-classification"),
        DTColumnBuilder.newColumn("Status").withTitle("STATUS").withClass("td-status"),
        DTColumnBuilder.newColumn("Site").withTitle("SITE").withClass("td-site"),
        DTColumnBuilder.newColumn("WorkType").withTitle("WORK TYPE").withClass("td-work"),
        DTColumnBuilder.newColumn("EmployeeName").withTitle("NAME").withClass("td-requestor"),
        DTColumnBuilder.newColumn("EmployeeStatus").withTitle("EMPLOYEE STATUS").withClass("td-empstat"),
        DTColumnBuilder.newColumn("EmployeeTitle").withTitle("EMPLOYEE TITLE").withClass("td-emptitle"),
        DTColumnBuilder.newColumn("Floors").withTitle("FLOOR").withClass("td-floor"),
        DTColumnBuilder.newColumn("AreaWorkStation").withTitle("AREA WORK STATION").withClass("td-area"),
        DTColumnBuilder.newColumn("ContactNumber").withTitle("CONTACT #").withClass("td-contact"),
        DTColumnBuilder.newColumn("Address").withTitle("ADDRESS").withClass("td-address"),
        DTColumnBuilder.newColumn("Email").withTitle("EMAIL").withClass("td-address"),
        DTColumnBuilder.newColumn("TrackingNumber").withTitle("TRACKING #").withClass("td-address"),
        DTColumnBuilder.newColumn("ItTicketNumber").withTitle("IT TICKET #").withClass("td-address"),
        DTColumnBuilder.newColumn("InvoiceNumber").withTitle("INVOICE NUMBER").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("InvoiceDate").withTitle("INVOICE DATE").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("DeliveryReceiptNumber").withTitle("DELIVERY RECEIPT NUMBER").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("DeliveryReceivedDate").withTitle("DELIVERY RECEIVED DATE").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("ReceivedBy").withTitle("RECEIVED BY").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("WarrantyStartDate").withTitle("WARRANTY START DATE").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("WarrantyEndDate").withTitle("WARRANTY END DATE").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("PezaForm8105Number").withTitle("PEZA FORM 8105 NUMBER").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("PezaPermitNumber").withTitle("PEZA PERMIT NUMBER").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("PezaApprovalDate").withTitle("PEZA APPROVAL DATE").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("ImportationPermitNumber").withTitle("IMPORTATION PERMIT NUMBER").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("ValidUntil").withTitle("VALID UNTIL").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("BillOfLandingNumber").withTitle("BILL OF LANDING NUMBER").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("TaxesPaid").withTitle("TAXES PAID").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("BondIssuer").withTitle("BOND ISSUER").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("SuretyBondPolicyNumber").withTitle("SURETY BOND POLICY NUMBER").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("ValidityPeriod").withTitle("VALIDITY PERIOD").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("SuretyBondOfficialReceiept").withTitle("SURETY BOND OFFICIAL RECEIEPT").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("AmountPaid").withTitle("AMOUNT PAID").withClass("td-address"), /*New*/
        DTColumnBuilder.newColumn("Remarks").withTitle("REMARKS").withClass("td-manufacturer"), /*New*/
        DTColumnBuilder.newColumn("DeployedDate").withTitle("DEPLOYED DATE").withClass("td-manufacturer"),
        DTColumnBuilder.newColumn("DateCreated").withTitle("DATE CREATED").withClass("td-manufacturer")
            .renderWith(function (data, type, full) {
                if (data) {
                    var date = new Date(data);
                    var day = ("0" + date.getDate()).slice(-2);
                    var month = ("0" + (date.getMonth() + 1)).slice(-2);
                    var year = date.getFullYear();

                    var hours = date.getHours();
                    var minutes = ("0" + date.getMinutes()).slice(-2);
                    var seconds = ("0" + date.getSeconds()).slice(-2);

                    var ampm = hours >= 12 ? 'PM' : 'AM';
                    hours = hours % 12;
                    hours = hours ? hours : 12; // the hour '0' should be '12'

                    var formattedTime = hours + ':' + minutes + ':' + seconds + ' ' + ampm;

                    var formattedDate = month + '/' + day + '/' + year;

                    return formattedDate + ' ' + formattedTime;
                }
                return "";
            })
        //DTColumnBuilder.newColumn("RetrievedDate").withTitle("RETRIEVED DATE").withClass("td-manufacturer"),
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
        try {
            if ($scope.Template.Url.length < 1) {
                throw "Select a Template first";
            }
            window.open($scope.Template.Url);
        } catch (e) {
            new ToastService().error(e);
        }
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

    $scope.refreshData = function () {
        $scope.requestData = [];
        $scope.isSaveOngoing = false;
        $scope.Site = $scope.Site || {};
        $scope.Status = $scope.Status || {};
        $scope.Category = $scope.Category || {};
        $scope.SubCategory = $scope.SubCategory || {};
        $scope.Manufacturer = $scope.Manufacturer || {};
        $scope.WorkType = $scope.WorkType || "";
        $scope.dtOptions_asset.ajax.url = "";
        $scope.dtOptions_asset.ajax.url = globalApiBaseUrl.concat("AssetApi/", defaultGuid, "/table" +
            "?site=", ($scope.Site.LocationDesc || "All"),
            "&?status=", ($scope.Status.Code || "All"),
            "&?category=", ($scope.Category.Code || "All"),
            "&?subcategory=", ($scope.SubCategory.Code || "All"),
            "&?manufacturer=", ($scope.Manufacturer.Code || "All"),
            "&?worktype=", ($scope.WorkType || "All"),
            "&?_=", new Date().getTime()
        );
        $scope.dtOptions_asset.ajax.reloadData();
    };

    $scope.exportData = function () {
        $scope.Site = $scope.Site || {};
        $scope.Status = $scope.Status || {};
        $scope.Category = $scope.Category || {};
        $scope.SubCategory = $scope.SubCategory || {};
        $scope.Manufacturer = $scope.Manufacturer || {};
        $scope.WorkType = $scope.WorkType || "NULL";
        var json = {
            Filter: "NULL",
            Site: $scope.Site.LocationDesc || "NULL",
            Status: $scope.Status.Code || "NULL",
            Category: $scope.Category.Code || "NULL",
            SubCategory: $scope.SubCategory.Code || "NULL",
            Manufacturer: $scope.Manufacturer.Code || "NULL",
            WorkType: $scope.WorkType || "NULL"
        }
        var exportUrl = url + "home/raw?filters=" + JSON.stringify(json);
        window.open(exportUrl);
    };

    $scope.exportHistory = function () {
        var exportUrl = url + "home/RawHistory";
        window.open(exportUrl);
    };

    $scope.uploadTemplateData = function () {
        try {
            $scope.isSaveOngoing = true;
            if ($scope.Template.Url.length < 1) {
                throw "Select a Template first";
            }
            if ($scope.UploadingFile.File === null) {
                throw "Select a downloaded template to upload";
            }
            var arrLen = $scope.Template.Url.split("/").length - 1;
            var fileName = $scope.Template.Url.split("/")[arrLen];
            fileName = fileName.split(".")[0];
            if (fileName.indexOf("=") > -1) {
                fileName = fileName.split("=")[1];
            }
            var isInvalid = $scope.UploadingFile.File.name.indexOf(fileName) < 0;
            if (isInvalid) {
                throw "Kindly use the proper template file [" + fileName + "]";
            }
            var finalUrl = "";
            if (fileName.indexOf("NewAsset") > -1) {
                finalUrl = "AssetApi/" + defaultGuid + "/bulk";
            }
            else if (fileName.indexOf("FarmOut") > -1) {
                finalUrl = 'FarmOut';
            }
            else if (fileName.indexOf("AssetDetails") > -1) {
                finalUrl = 'AssetDetails';
            }
            else if (fileName.indexOf("AssetAssignment") > -1) {
                finalUrl = 'AssetAssignment';
            }
            else if (fileName.indexOf("AssetUpdate") > -1) {
                finalUrl = 'AssetUpdate';
            }
            else if (fileName.indexOf("AssetReplacement") > -1) {
                finalUrl = 'AssetReplacement';
            }
            //else if (fileName.indexOf("AssetAssign") > -1) {
            //    finalUrl = "DeployApi";
            //}
            //else if (fileName.indexOf("AssetReplace") > -1) {
            //    finalUrl = "ReplaceApi";
            //}
            //else if (fileName.indexOf("AssetStatusUpdate") > -1) {
            //    finalUrl = "AssetStatusApi";
            //}
            //else if (fileName.indexOf("AssetTransfer") > -1) {
            //    finalUrl = "TransferApi";
            //}
            //else if (fileName.indexOf("AssetEmployeeAssignmentRemoval") > -1) {
            //    finalUrl = "RetrieveApi";
            //}
            //else if (fileName.indexOf("AssetChange") > -1) {
            //    finalUrl = "ChangeApi";
            //}
            //else if (fileName.indexOf("EmployeeValidation") > -1) {
            //    finalUrl = "ValidateApi";
            //}
            else throw "Kindly make sure that template being uploaded is downloaded from this tool";
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
            $scope.factory.uploadData(finalUrl, formData)
                .then(function () {
                    $scope.resetUploading();
                    $scope.refreshData();
                    $scope.getSites();
                    if (fileName.indexOf("EmployeeValidation") > -1) {
                        new ToastService().success("No Invalid HRID");
                    } else {
                        new ToastService().success("File has been uploaded");
                    }

                    

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
                // console.log("getCategories:", res.data);
                $scope.Categories = angular.copy(res.data);
                $scope.Categories.unshift({ Code: "All", CreatedBy: null, DateCreated: null, DateUpdated: null, Id: null, IsActive: false, UpdatedBy: null });
            }, $scope.error);
    };
    $scope.Sites = [{ Region: "All", Location: "All", LocationDesc: "All" }];
    $scope.getSites = function () {
        $scope.factory.getData("siteapi")
            .then(function (res) {
                // console.log("getSites:", res.data);
                $scope.Sites = angular.copy(res.data);
                $scope.Sites.unshift({ Region: "All", Location: "All", LocationDesc: "All" });
                //$scope.apply();
            }, $scope.error);
    };

    $scope.getSubCategories = function () {
        $scope.factory.getData("subcategoryapi")
            .then(function (res) {
                // console.log("subcategoryapi:", res.data);
                $scope.SubCategories = angular.copy(res.data);
                $scope.SubCategories.unshift({ Code: "All", CategoryId: null, CreatedBy: null, DateCreated: null, DateUpdated: null, Id: null, IsActive: false, UpdatedBy: null });
            }, $scope.error);
    };

    $scope.getStatus = function () {
        $scope.factory.getData("statusapi")
            .then(function (res) {
                // console.log("statusapi:", res.data);
                $scope.Statuses = angular.copy(res.data);
                $scope.Statuses.unshift({ Code: "All", CreatedBy: null, DateCreated: null, DateUpdated: null, Description: null, Id: null, IsActive: false, Sort: 0, UpdatedBy: null });
            }, $scope.error);
    };

    $scope.getManufacturer = function () {
        $scope.factory.getData("manufacturerapi")
            .then(function (res) {
                // console.log("manufacturerapi:", res.data);
                $scope.Manufacturers = angular.copy(res.data);
                $scope.Manufacturers.unshift({ Code: "All", CreatedBy: null, DateCreated: null, DateUpdated: null, Description: null, Id: null, IsActive: false, Month: 0, Year: 0 });
            }, $scope.error);
    };

    $scope.Histories = [];
    $scope.loadingHistory = false;
    $scope.getHistory = function (id) {
        $scope.Histories = [];
        $scope.GeoId = id;
        $scope.loadingHistory = true;
        try {
            $scope.factory.getData(`AssetApi/${id}/history`)
                .then(function (res) {
                    $scope.loadingHistory = false;
                    $scope.Modified = [];
                    for (var i = 0; i < res.data.length; i++) {
                        $scope.Modified.push(res.data[i]);
                        if ($scope.Modified[i].EmployeeStatus == true) {
                            $scope.Modified[i].EmployeeStatus = "ACTIVE";
                        }
                        else {
                            $scope.Modified[i].EmployeeStatus = "INACTIVE";
                        }
                    }
                    $scope.Histories = angular.copy($scope.Modified);
                }, $scope.error);
        } catch (e) {
            new ToastService().error("Something went wrong");
        } finally {
            $scope.loadingHistory = false;
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

    $scope.load = function () {
        try {
            scanner.stop();
        } catch (e) {
            //ignore
        }
        setTimeout(function () { $('.page-loader-wrapper').fadeOut(); }, 50);
        $scope.getSites();
        $scope.getCategories();
        $scope.getSubCategories();
        $scope.getStatus();
        $scope.getManufacturer();
        try {
            $scope.Site = $scope.Sites[0];
        } catch (e) {
            $scope.Site = { Region: "All", Location: "All", LocationDesc: "All" };
        }
        try {
            $scope.Status = $scope.Statuses[0];
        } catch (e) {
            $scope.Status = { Code: "All", CreatedBy: null, DateCreated: null, DateUpdated: null, Description: null, Id: null, IsActive: false, Sort: 0, UpdatedBy: null };
        }
        try {
            $scope.Category = $scope.Categories[0];
        } catch (e) {
            $scope.Category = { Code: "All", CreatedBy: null, DateCreated: null, DateUpdated: null, Id: null, IsActive: false, UpdatedBy: null };
        }
        try {
            $scope.SubCategory = $scope.SubCategories[0];
        } catch (e) {
            $scope.SubCategory = { Code: "All", CategoryId: null, CreatedBy: null, DateCreated: null, DateUpdated: null, Id: null, IsActive: false, UpdatedBy: null };
        }
        try {
            $scope.Manufacturer = { Code: "All", CreatedBy: null, DateCreated: null, DateUpdated: null, Description: null, Id: null, IsActive: false, Month: 0, Year: 0 };
        } catch (e) {
            $scope.Manufacturer = $scope.Manufacturers[0];
        }
        $scope.WorkType = "All";
        $scope.apply();

        $(document).on("click", "td.td-reqno", function (e) {
            e.preventDefault();
            e.stopImmediatePropagation();
            var $this = $(this).text();
            if ($this.length > 0) {
                var tmp = $scope.requestData.filter((x) => x.Code === $this);
                if (Array.isArray(tmp) && tmp.length > 0) {
                    $scope.Asset = window.angular.copy(tmp[0]);
                    $scope.getHistory(tmp[0].Id || defaultGuid);
                    $("#assetHistoryModal").modal("show");
                }
            }
        });
    };
    $scope.load();

    $scope.clear = function () {
        $scope.AssetVm = window.angular.copy($scope.AssetRaw);
    };
}