// ReSharper disable all IdentifierTypo
function _toConsumableArray(arr) { return _arrayWithoutHoles(arr) || _iterableToArray(arr) || _nonIterableSpread(); }

function _nonIterableSpread() { throw new TypeError("Invalid attempt to spread non-iterable instance"); }

function _iterableToArray(iter) { if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter); }

function _arrayWithoutHoles(arr) { if (Array.isArray(arr)) { for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) { arr2[i] = arr[i]; } return arr2; } }

app.controller("homeCtrl", homeCtrl);
homeCtrl.$inject = ["$rootScope", "$scope", "$http", "DTOptionsBuilder", "DTColumnBuilder", "ApiFactory", "$timeout"];
function homeCtrl($rootScope, $scope, $http, DTOptionsBuilder, DTColumnBuilder, ApiFactory, $timeout) {
    var vm = this;
    var defaultGuid = "00000000-0000-0000-0000-000000000000";
    var btn = $("#btn-click");
    vm.isSaveOngoing = false;
    $.fn.dataTable.ext.errMode = 'none';
    vm.Notifications = {
        RecordAdded: "RECORD HAS BEEN ADDED",
        RecordFailed: "ERROR WHILE SAVING THE RECORD",
        CategoryRequired: "CATEGORY IS REQUIRED"
    };
    vm.useBulkUpload = false;
    vm.FarmTypes = ["IN", "OUT"];
    vm.WorkTypes = ["WAS", "WAH"];
    vm.TransactionTypes = [];
    vm.FinancialTreatments = [];
    vm.Regions = [];
    vm.Categories = [];
    vm.SubCategories = [];
    vm.Sites = [];
    vm.Lobs = [];
    vm.Brands = [];
    vm.Vendors = [];
    vm.Employees = [];
    vm.Groups = [];
    vm.Currency = ["PHP", "USD"];
    vm.PezaTransactionTypes = [];
    vm.AssetStatus = [];
    vm.AssetBulkItems = {
        CreatedBy: username,
        Asset: []
    }
    $scope.$on("newAssetBulkItems", function (event, data) {
        vm.AssetBulkItems.Asset = data.rows;
        //console.log("vm.AssetBulkItems.Asset:", vm.AssetBulkItems.Asset);
        //console.log("vm.AssetBulkItems.Asset STRING:", JSON.stringify(vm.AssetBulkItems.Asset));
        vm.apply();
    });
    function actionsHtml(data, func, btnName) {
        btnName = btnName || "EDIT";
        btnName = btnName.toUpperCase();
        return "<button class=\"btn btn-warning\" onclick=\"angular.element('section').scope().vm.".concat(func, "('", data.Id, "','", data.ControlNo, "')\">", btnName, "</button>");
    }
    function farmAction(data) {
        return actionsHtml(data, "getFarmInById");
    }
    function assetAction(data) {
        return actionsHtml(data, "getAssetById");
    }
    function farmLineItemAction(data) {
        return actionsHtml(data, "removeFarmLineItem", "del");
    }
    function lookupAction(data) {
        data = data || {};
        return data.ItemName || "";
    }
    function assetFilterAction(data) {
        //return actionsHtml(data, "getFilteredFarmLineItemData", "USE");
        return "<button class=\"btn btn-warning\" onclick=\"angular.element('section').scope().vm.".concat("getFilteredFarmLineItemData", "('", data.ParentId, "','", data.Id, "')\">", "USE", "</button>");

    }
    function moneyFormatAction(data, a, b, c) {
        data = data || 0;
        var currency = new Intl.NumberFormat();
        //if (b.Currency === "USD") {
        //    currency = new Intl.NumberFormat("en-US", { style: "currency", currency: "USD", });
        //}else if (b.Currency === "PHP") {
        //    currency = new Intl.NumberFormat("en-PH", { style: "currency", currency: "PHP", });
        //}
        return currency.format(data);
    }
    function groupAction(data) {
        data = data || {};
        return data.ItemName || "";
    }

    vm.dtInstance = {};
    vm.dtOptions = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("asset?group=".concat("TEST")))
        .withColumnFilter(
            {
                aoColumns:
                    [
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                    ]
            }
        )
        .withOption("autoWidth", true)
        .withOption("scrollX", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        //.withOption("paging", false)
        //.withOption("scrollY", "40vh")
        //.withOption("deferLoading", 10)
        //.withOption("dom", "Bfrtip")
        //.withOption("buttons", ["excelHtml5"])
        .withOption("bSort", false);
    vm.dtColumns = [
        DTColumnBuilder.newColumn(null).withTitle("OPTION").withClass("td-option").renderWith(assetAction),
        DTColumnBuilder.newColumn('ControlNo').withTitle('CONTROL NO').withClass('td-small'),
        DTColumnBuilder.newColumn('Description').withTitle('DESCRIPTION').withClass('td-small'),
        DTColumnBuilder.newColumn('SerialNo').withTitle('SERIAL NO.').withClass('td-small'),
        DTColumnBuilder.newColumn('GrNo').withTitle('GR NO.').withClass('td-small'),
        //DTColumnBuilder.newColumn('PezaFormNo').withTitle('PEZA FORM NO.').withClass('td-small'),
        //DTColumnBuilder.newColumn('PezaPermitNo').withTitle('PEZA PERMIT NO.').withClass('td-small'),
        DTColumnBuilder.newColumn('Category').withTitle('CATEGORY').withClass('td-small').renderWith(lookupAction),
        DTColumnBuilder.newColumn('SubCategory').withTitle('SUBCATEGORY').withClass('td-small').renderWith(lookupAction),
        DTColumnBuilder.newColumn('Site').withTitle('SITE').withClass('td-small'),
        DTColumnBuilder.newColumn('LobOwner').withTitle('LOB').withClass('td-small'),
        DTColumnBuilder.newColumn('Brand').withTitle('BRAND').withClass('td-small').renderWith(lookupAction),
        DTColumnBuilder.newColumn('Vendor').withTitle('VENDOR').withClass('td-small').renderWith(lookupAction),
        DTColumnBuilder.newColumn('PuchaseOrderNo').withTitle('PURCHASE ORDER NO').withClass('td-small'),
        DTColumnBuilder.newColumn('DeliveryNo').withTitle('DELIVERY NO').withClass('td-small'),
        DTColumnBuilder.newColumn('TagDate').withTitle('TAG DATE').withClass('td-small'),
        DTColumnBuilder.newColumn('InvoiceNo').withTitle('INVOICE NO').withClass('td-small')
    ];
    vm.dtColumns = [
        DTColumnBuilder.newColumn(null).withTitle("OPTION").withClass("td-option").renderWith(assetAction),
        DTColumnBuilder.newColumn('ControlNo').withTitle('CONTROL NO').withClass('td-small'),
        DTColumnBuilder.newColumn('Description').withTitle('DESCRIPTION').withClass('td-small'),
        DTColumnBuilder.newColumn('SerialNo').withTitle('SERIAL NO.').withClass('td-small'),
        DTColumnBuilder.newColumn('GrNo').withTitle('GR NO.').withClass('td-small'),
        //DTColumnBuilder.newColumn('PezaFormNo').withTitle('PEZA FORM NO.').withClass('td-small'),
        //DTColumnBuilder.newColumn('PezaPermitNo').withTitle('PEZA PERMIT NO.').withClass('td-small'),
        DTColumnBuilder.newColumn('Category').withTitle('CATEGORY').withClass('td-small').renderWith(lookupAction),
        DTColumnBuilder.newColumn('SubCategory').withTitle('SUBCATEGORY').withClass('td-small').renderWith(lookupAction),
        DTColumnBuilder.newColumn('Site').withTitle('SITE').withClass('td-small'),
        DTColumnBuilder.newColumn('LobOwner').withTitle('LOB').withClass('td-small'),
        DTColumnBuilder.newColumn('Brand').withTitle('BRAND').withClass('td-small').renderWith(lookupAction),
        DTColumnBuilder.newColumn('Vendor').withTitle('VENDOR').withClass('td-small').renderWith(lookupAction),
        DTColumnBuilder.newColumn('PuchaseOrderNo').withTitle('PURCHASE ORDER NO').withClass('td-small'),
        DTColumnBuilder.newColumn('DeliveryNo').withTitle('DELIVERY NO').withClass('td-small'),
        DTColumnBuilder.newColumn('TagDate').withTitle('TAG DATE').withClass('td-small'),
        DTColumnBuilder.newColumn('InvoiceNo').withTitle('INVOICE NO').withClass('td-small')
    ];
    vm.dtInstance_farmIn = {};
    vm.dtOptions_farmIn = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("farmin"))
        .withColumnFilter(
            {
                aoColumns:
                    [
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                    ]
            }
        )
        .withOption("autoWidth", true)
        .withOption("scrollX", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        //.withOption("paging", false)
        //.withOption("scrollY", "40vh")
        //.withOption("deferLoading", 10)
        //.withOption("dom", "Bfrtip")
        //.withOption("buttons", ["excelHtml5"])
        .withOption("bSort", false);
    vm.dtColumns_farmIn = [
        DTColumnBuilder.newColumn(null).withTitle("OPTION").withClass("td-option").renderWith(farmAction),
        DTColumnBuilder.newColumn('ControlNo').withTitle('CONTROL NO').withClass('td-small'),
        DTColumnBuilder.newColumn('PurchaseOrderNo').withTitle('PURCHASE ORDER NO.').withClass('td-small'),
        DTColumnBuilder.newColumn('PurchaseOrderDate').withTitle('PURCHASE ORDER DATE').withClass('td-small'),
        DTColumnBuilder.newColumn('Site').withTitle('SITE').withClass('td-small'),
        DTColumnBuilder.newColumn('Vendor').withTitle('VENDOR').withClass('td-small').renderWith(lookupAction),
        DTColumnBuilder.newColumn('ProformaInvoiceNo').withTitle('PROFORMA INVOICE NO.').withClass('td-small'),
        DTColumnBuilder.newColumn('ProformaInvoiceDate').withTitle('PROFORMA INVOICE DATE').withClass('td-small'),
        DTColumnBuilder.newColumn('FinalInvoiceNo').withTitle('FINAL INVOICE NO.').withClass('td-small'),
        DTColumnBuilder.newColumn('FinalInvoiceDate').withTitle('FINAL INVOICE DATE').withClass('td-small'),
        DTColumnBuilder.newColumn('FarmStatus').withTitle('STATUS').withClass('td-small').renderWith(lookupAction),
        DTColumnBuilder.newColumn('Remarks').withTitle('REMARKS').withClass('td-small')
    ];
    vm.dtInstance_farmLineItems = {};
    vm.dtOptions_farmLineItems = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("farmin-lineitem/parent/", 0))
        .withColumnFilter(
            {
                aoColumns:
                    [
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                    ]
            }
        )
        .withOption("autoWidth", true)
        .withOption("scrollX", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        //.withOption("paging", false)
        //.withOption("scrollY", "40vh")
        //.withOption("deferLoading", 10)
        //.withOption("dom", "Bfrtip")
        //.withOption("buttons", ["excelHtml5"])
        .withOption("bSort", false);
    vm.dtColumns_farmLineItems = [
        DTColumnBuilder.newColumn(null).withTitle("OPTION").withClass("td-option").renderWith(farmLineItemAction),
        DTColumnBuilder.newColumn('ControlNo').withTitle('CONTROL NO').withClass('td-small2'),
        DTColumnBuilder.newColumn('Description').withTitle('DESCRIPTION').withClass('td-small2'),
        DTColumnBuilder.newColumn('Quantity').withTitle('QTY').withClass('td-small2'),
        DTColumnBuilder.newColumn('Price').withTitle('PRICE').withClass('td-small2').renderWith(moneyFormatAction),
        DTColumnBuilder.newColumn('Currency').withTitle('CURRENCY').withClass('td-small2'),
        DTColumnBuilder.newColumn('Group').withTitle('GROUP').withClass('td-small2').renderWith(groupAction),
        DTColumnBuilder.newColumn('WarrantyStartDate').withTitle('WARRANTY START DATE').withClass('td-small2'),
        DTColumnBuilder.newColumn('WarrantyEndDate').withTitle('WARRANTY END DATE').withClass('td-small2'),
    ];
    vm.dtInstance_farmLineItemsByFilter = {};
    vm.dtOptions_farmLineItemsByFilter = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("farmin-lineitem/parent/", 0))
        .withColumnFilter(
            {
                aoColumns:
                    [
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                        {
                            type: 'text'
                        },
                    ]
            }
        )
        .withOption("autoWidth", true)
        .withOption("scrollX", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        //.withOption("paging", false)
        //.withOption("scrollY", "40vh")
        //.withOption("deferLoading", 10)
        //.withOption("dom", "Bfrtip")
        //.withOption("buttons", ["excelHtml5"])
        .withOption("bSort", false);
    vm.dtColumns_farmLineItemsByFilter = [
        DTColumnBuilder.newColumn(null).withTitle("OPTION").withClass("td-option").renderWith(assetFilterAction),
        DTColumnBuilder.newColumn('ControlNo').withTitle('CONTROL NO').withClass('td-small2'),
        DTColumnBuilder.newColumn('Description').withTitle('DESCRIPTION').withClass('td-small2'),
        DTColumnBuilder.newColumn('Quantity').withTitle('QTY').withClass('td-small2'),
        DTColumnBuilder.newColumn('Price').withTitle('PRICE').withClass('td-small2').renderWith(moneyFormatAction),
        DTColumnBuilder.newColumn('Currency').withTitle('CURRENCY').withClass('td-small2'),
        DTColumnBuilder.newColumn('Group').withTitle('GROUP').withClass('td-small2').renderWith(groupAction),
        DTColumnBuilder.newColumn('WarrantyStartDate').withTitle('WARRANTY START DATE').withClass('td-small2'),
        DTColumnBuilder.newColumn('WarrantyEndDate').withTitle('WARRANTY END DATE').withClass('td-small2'),
    ];
    vm.Assets = [];
    vm.Asset =
    {
        "Code": null,
        "Name": null,
        "Description": null,
        "Category": null,
        "SubCategory": null,
        "SerialNo": null,
        "Brand": null,
        "Model": null,
        "Site": null,
        "FLoor": null,
        "AreaWorkStation": null,
        "LobOwner": null,
        "PezaTransactionType": null,
        "PezaFarmInDate": null,
        "PezaFormNo": null,
        "PezaPermitNo": null,
        "IsVatTdPaid": true,
        "FinancialTreatment": null,
        "GrNo": null,
        "SalesInvoice": null,
        "ImageFileName": null,
        "ImagePicture": null,
        "ImageFile": null,
        "DeliveryDate": null,
        "DeliveryNo": null,
        "TagDate": null,
        "InvoiceNo": null,
        "PuchaseOrderNo": null,
        "Vendor": "",
        "IsImportedConstructiveImportation": false,
        "ImportedPermitNo": null,
        "PBONo": null,
        "CEWENo": null,
        "BoarNoteNo": null,
        "BOCCertificate": null,
        "QRFileName": null,
        "QRCode": null,
        "Region": null,
        "History": [],
        "Replacement": [],
        "Id": 0,
        "IsActive": false,
        "WorkType": "WAS",
        "HomeAddress": null,
        "CreatedBy": null,
        "UpdatedBy": null,
        "DateCreated": null,
        "DateUpdated": null
    };

    vm.AssetRaw =
    {
        "Id": 0,
        "Code": null,
        "Name": null,
        "Description": null,
        "Category": null,
        "SubCategory": null,
        "SerialNo": null,
        "Brand": null,
        "Model": null,
        "Site": null,
        "FLoor": null,
        "AreaWorkStation": null,
        "LobOwner": null,
        "PezaTransactionType": null,
        "PezaFarmInDate": null,
        "PezaFormNo": null,
        "PezaPermitNo": null,
        "IsVatTdPaid": true,
        "FinancialTreatment": null,
        "GrNo": null,
        "SalesInvoice": null,
        "ImageFileName": null,
        "ImagePicture": null,
        "ImageFile": null,
        "DeliveryDate": null,
        "DeliveryNo": null,
        "TagDate": null,
        "InvoiceNo": null,
        "PuchaseOrderNo": null,
        "Vendor": "",
        "IsImportedConstructiveImportation": false,
        "ImportedPermitNo": null,
        "PBONo": null,
        "CEWENo": null,
        "BoarNoteNo": null,
        "BOCCertificate": null,
        "QRFileName": null,
        "QRCode": null,
        "Region": null,
        "History": [],
        "Replacement": [],
        "IsActive": false,
        "WorkType": "WAS",
        "HomeAddress": null,
        "CreatedBy": null,
        "UpdatedBy": null,
        "DateCreated": null,
        "DateUpdated": null
    };

    vm.Farm = {
        Id: -1,
        FarmType: null,
        ControlNo: null,
        PurchaseOrderNo: null,
        PurchaseOrderDate: null,
        Site: null,
        VendorId: null,
        ProformaInvoiceNo: null,
        ProformaInvoiceDate: null,
        FinalInvoiceNo: null,
        FinalInvoiceDate: null,
        FarmStatusId: null,
        PezaPermitNo: null,
        PezaPermitStatus: null,
        PezaApprovalNo: null,
        PezaFormNo: null,
        PezaVatPaid: null,
        DeliveryDate: null,
        DeliveryNo: null,
        Remarks: null,
        Email: null,
        Region: globalRegion,
        AirwayDeliveryNo: null,
        AirwayDeliveryDate: null,
        Cargo: null,
        ConsigneeName: null,
        ShipperName: null,
        ReceivedBy: null,
        DateReceived: null,
        CreatedBy: null,
        DateCreated: null,
        UpdatedBy: null,
        DateUpdated: null,
        FarmLineItemViewModel: []
    }

    vm.FarmRaw = {
        Id: -1,
        FarmType: null,
        ControlNo: null,
        PurchaseOrderNo: null,
        PurchaseOrderDate: null,
        Site: null,
        VendorId: null,
        ProformaInvoiceNo: null,
        ProformaInvoiceDate: null,
        FinalInvoiceNo: null,
        FinalInvoiceDate: null,
        FarmStatusId: null,
        PezaPermitNo: null,
        PezaPermitStatus: null,
        PezaApprovalNo: null,
        PezaFormNo: null,
        PezaVatPaid: null,
        DeliveryDate: null,
        DeliveryNo: null,
        Remarks: null,
        Email: null,
        Region: globalRegion,
        AirwayDeliveryNo: null,
        AirwayDeliveryDate: null,
        Cargo: null,
        ConsigneeName: null,
        ShipperName: null,
        ReceivedBy: null,
        DateReceived: null,
        CreatedBy: null,
        DateCreated: null,
        UpdatedBy: null,
        DateUpdated: null,
        FarmLineItemViewModel: []
    }


    vm.FarmLineItem = {
        Id: null,
        ControlNo: null,
        ParentId: null,
        Description: null,
        Quantity: null,
        Price: null,
        Currency: null,
        Group: null,
        WarrantyStartDate: null,
        WarrantyEndDate: null,
        CreatedBy: null,
        UpdatedBy: null,
    };

    vm.FarmLineItemRaw = {
        Id: null,
        ControlNo: null,
        ParentId: null,
        Description: null,
        Quantity: null,
        Price: null,
        Currency: null,
        Group: null,
        WarrantyStartDate: null,
        WarrantyEndDate: null,
        CreatedBy: null,
        UpdatedBy: null,
    }

    vm.FarmAttachment = {
        ParentId: 0,
        DocumentFile: null,
        FileName: null,
        FileType: null,
        FileBase64: null,
        CreatedBy: null,
        UpdatedBy: null,
    }

    vm.FarmAttachmentRaw = {
        ParentId: 0,
        DocumentFile: null,
        FileName: null,
        FileType: null,
        FileBase64: null,
        CreatedBy: null,
        UpdatedBy: null,
    }

    function error(err) {
        vm.isSearchOngoing = false;
        vm.isSaveOngoing = false;
        $("#farm-attachment-loader, #asset-attachment-loader").addClass("no-display");
        try {
            new ToastService().error(err.data.Message);
        } catch (e) {
            new ToastService().error("An error occured");
        }
        btn.removeAttr("disabled");
    }

    vm.apply = function () {
        try {
            (function () {
                function codeBlock() {
                    $scope.$apply();
                }
                codeBlock();
            })();
        } catch (e) {
            //ignore
        }
    };

    vm.factory = new ApiFactory();
    $scope.setImage = function (event) {
        vm.UploadingFile.Image = event.target.files[0] || "";
        vm.apply();
        var reader = new FileReader();
        reader.onload = $scope.imageIsLoaded;
        reader.readAsDataURL(vm.UploadingFile.Image);
    };
    $scope.imageIsLoaded = function (e) {
        $scope.$apply(function () {
            vm.Asset.ImageFile = e.target.result;
        });
    }

    $scope.setImageOpenRecords = function (event) {
        var file = event.target.files[0] || "";
        var reader = new FileReader();
        reader.onload = function (e) {
            $scope.$apply(function () {
                vm.OpenRecord.ImagePicture = null;
                vm.OpenRecord.ImageFile = e.target.result;
            });
        };
        reader.readAsDataURL(file);
    };

    //$scope.setFarmLineItems = function (event) {
    //    vm.UploadingFile.FarmLineItems = event.target.files;
    //    vm.apply();
    //   //console.log("vm.UploadingFile.FarmLineItems:", vm.UploadingFile.FarmLineItems);
    //};

    vm.UploadingFile = {
        Image: null,
        FarmAttachments: [],
        AssetAttachments: [],
    };

    $scope.setFarmAttachments = function (event) {
        vm.UploadingFile.FarmAttachments = event.target.files;
        vm.apply();
        //console.log("vm.UploadingFile.FarmAttachments:", vm.UploadingFile.FarmAttachments);
    };

    $scope.setAssetAttachments = function (event) {
        vm.UploadingFile.AssetAttachments = event.target.files;
        vm.apply();
        //console.log("vm.UploadingFile.AssetAttachments:", vm.UploadingFile.AssetAttachments);
    };

    vm.resetNewAsset = function () {
        vm.SearchPurchaseOrderNo = "";
        vm.UploadingFile.Image = null;
        vm.Asset = JSON.parse(JSON.stringify(vm.AssetRaw));
        vm.apply();
        $("#new-asset-img").val(null);
        vm.resetAssetAttachments();
        //setTimeout(function () {
        //    $("#home select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                //$("#home select").selectpicker("refresh");
            }
            codeBlock();
        })();
    };
    vm.resetFarmForm = function () {
        vm.isSaveOngoing = false;
        vm.Farm = JSON.parse(JSON.stringify(vm.FarmRaw));
        vm.apply();
        vm.getFarmLineItems();
        vm.getFarmAttachments();
        //setTimeout(function () {
        //    $("#farm-form select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                //$("#home select").selectpicker("refresh");
            }
            codeBlock();
        })();
    };
    vm.resetFarmAttachments = function () {
        vm.UploadingFile.FarmAttachments = JSON.parse(JSON.stringify([]));
        vm.apply();
        (function () {
            function codeBlock() {
                //$("#farm-tab select").selectpicker("refresh");
            }
            codeBlock();
        })();
        vm.getFarmAttachments();
    };
    vm.resetAssetAttachments = function () {
        vm.UploadingFile.AssetAttachments = JSON.parse(JSON.stringify([]));
        vm.apply();
        (function () {
            function codeBlock() {
                //$("#home select").selectpicker("refresh");
            }
            codeBlock();
        })();
        vm.getAssetAttachments();
    };
    vm.ongoingUpload = false;

    vm.saveFarm = function () {
        try {
            vm.isSaveOngoing = true;
            var model = JSON.parse(JSON.stringify(vm.Farm));
            vm.apply();
            model.Id = model.Id || 0;
            try {
                model.Site = "".concat(vm.Farm.Site.ItemName);
            } catch (e) {
                model.Site = null;
            }

            model.FarmType = model.FarmType || "";
            if (model.FarmType.length < 1) {
                throw "Farm Type is required.";
            }

            model.PurchaseOrderNo = model.PurchaseOrderNo || "";
            if (model.PurchaseOrderNo.length < 1) {
                throw "Purchase Order is required.";
            }

            model.Site = model.Site || "";
            if (model.Site.length < 1) {
                throw "Site is required.";
            }

            model.Vendor = model.Vendor || {};
            if (model.Vendor.Id === undefined) {
                throw "Vendor is required.";
            }

            model.FarmStatus = model.FarmStatus || {};
            if (model.FarmStatus.Id === undefined) {
                throw "Status is required.";
            }

            model.Email = model.Email || {};
            if (model.Email.Id === undefined) {
                throw "Email Distro is required.";
            }

            model.CreatedBy = username;
            model.UpdatedBy = username;

            //if (vm.Farm.FarmType === "IN") {
            //}else if (vm.Farm.FarmType === "OUT") {
            //}
            vm.apply();
            //console.log("model:", model);
            if (model.Id < 1 || model.Id === null || model.Id === undefined) {
                if (vm.Farm.FarmType === "IN") {
                    vm.factory.setData("farmin/save", model)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN ADDED");
                            vm.resetFarmForm();
                        },
                            function () {
                                error({ data: { Message: "ERROR WHILE SAVING THE RECORD" } });
                            });
                } else if (vm.Farm.FarmType === "OUT") {
                    vm.factory.setData("farmout/save", model)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN ADDED");
                            vm.resetFarmForm();
                        },
                            function () {
                                error({ data: { Message: "ERROR WHILE SAVING THE RECORD" } });
                            });
                } else {
                    throw "Please select a Farm Type first";
                }
            } else {
                if (vm.Farm.FarmType === "IN") {
                    vm.factory.putData("farmin/update", model)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN ADDED");
                            vm.resetFarmForm();
                        },
                            function () {
                                error({ data: { Message: "ERROR WHILE SAVING THE RECORD" } });
                            });
                } else if (vm.Farm.FarmType === "OUT") {
                    vm.factory.putData("farmout/update", model)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN ADDED");
                            vm.resetFarmForm();
                        },
                            function () {
                                error({ data: { Message: "ERROR WHILE SAVING THE RECORD" } });
                            });
                } else {
                    throw "Please select a Farm Type first";
                }
            }
        } catch (e) {
            vm.isSaveOngoing = false;
            vm.apply();
            new ToastService().error(e);
        }
    };

    vm.saveFarmAttachments = function () {
        vm.isSaveOngoing = true;
        var total = vm.UploadingFile.FarmAttachments.length;
        var currentSuccess = 0;
        var currentTotal = 0;
        var totalError = 0;
        vm.apply();
        angular.forEach(vm.UploadingFile.FarmAttachments,
            function (value, key) {
                currentTotal++;
                var formData = new FormData();
                formData.append("DocumentFile", value);
                var model = JSON.parse(JSON.stringify(vm.FarmAttachmentRaw));
                model.ParentId = vm.Farm.Id;
                model.CreatedBy = username;
                model.UpdatedBy = username;
                vm.apply();
                formData.append("metaData", JSON.stringify(model));
                var url = "farmin-attachment/save";
                if (vm.Farm.FarmType === "IN" || vm.Farm.FarmType === "OUT") {
                    if (vm.Farm.FarmType === "OUT") {
                        url = "farmout-attachment/save";
                    }
                    vm.factory.uploadData(url, formData)
                        .then(function () {
                            currentSuccess++;
                        },
                            function () {
                                //error({ data: { Message: vm.Notifications.RecordFailed } });
                                totalError++;
                            });
                }
            }
        );
        //non ui blocking loop
        (function () {
            var ctr = currentSuccess + totalError;
            var condition = ctr < total;
            function codeBlock() {
                if (condition === true) {
                    ctr = currentSuccess + totalError;
                    condition = ctr < total;
                    setTimeout(codeBlock, 0);
                } else {
                    //done execution, add code here
                    vm.isSaveOngoing = false;
                    if (totalError > 0) {
                        var msg = "".concat(totalError, "/", total, " failed to upload");
                        new ToastService().error(msg);
                    } else {
                        vm.UploadingFile.FarmAttachments = [];
                        new ToastService().success("Files has been uploaded");
                    }
                    vm.apply();

                    setTimeout(function () {
                        vm.getFarmAttachments();
                    }, 1000);
                }
            }
            codeBlock();
        })();
    };

    vm.saveAssetAttachments = function () {
        vm.isSaveOngoing = true;
        var total = vm.UploadingFile.AssetAttachments.length;
        var currentSuccess = 0;
        var currentTotal = 0;
        var totalError = 0;
        vm.apply();
        angular.forEach(vm.UploadingFile.AssetAttachments,
            function (value, key) {
                currentTotal++;
                var formData = new FormData();
                formData.append("DocumentFile", value);
                var model = JSON.parse(JSON.stringify(vm.FarmAttachmentRaw));
                model.ParentId = vm.Asset.Id;
                model.CreatedBy = username;
                model.UpdatedBy = username;
                vm.apply();
                formData.append("metaData", JSON.stringify(model));
                vm.factory.uploadData("asset-attachment/save", formData)
                    .then(function () {
                        currentSuccess++;
                    },
                        function () {
                            //error({ data: { Message: vm.Notifications.RecordFailed } });
                            totalError++;
                        });
            }
        );
        //non ui blocking loop
        (function () {
            var ctr = currentSuccess + totalError;
            var condition = ctr < total;
            function codeBlock() {
                if (condition === true) {
                    ctr = currentSuccess + totalError;
                    condition = ctr < total;
                    setTimeout(codeBlock, 0);
                } else {
                    //done execution, add code here
                    vm.isSaveOngoing = false;
                    if (totalError > 0) {
                        var msg = "".concat(totalError, "/", total, " failed to upload");
                        new ToastService().error(msg);
                    } else {
                        new ToastService().success("Files has been uploaded");
                    }
                    vm.UploadingFile.AssetAttachments = [];
                    vm.apply();

                    setTimeout(function () {
                        vm.getAssetAttachments();
                    }, 1000);
                }
            }
            codeBlock();
        })();
    };

    vm.showNewFarmLineItemModal = function () {
        vm.Farm.Id = vm.Farm.Id || -1;
        if (vm.Farm.Id > 0) {
            $("#farmInLineItemModal").modal("show");
            vm.resetFarmLineItem();
        }
    };
    vm.resetFarmLineItem = function () {
        vm.FarmLineItem = JSON.parse(JSON.stringify(vm.FarmLineItemRaw));
        vm.apply();
        setTimeout(function () {
            (function () {
                function codeBlock() {
                    //$("#farmInLineItemModal select").selectpicker("refresh");
                }
                codeBlock();
            })();
        }, 500);
    };
    vm.saveFarmLineItem = function () {
        try {
            vm.FarmLineItem = vm.FarmLineItem || {};
            vm.FarmLineItem.ParentId = vm.Farm.Id;
            vm.FarmLineItem.CreatedBy = username;
            vm.FarmLineItem.UpdatedBy = username;

            vm.FarmLineItem.Description = vm.FarmLineItem.Description || "";
            if (vm.FarmLineItem.Description.length < 1)
                throw "Item Description is Required";

            vm.FarmLineItem.Quantity = vm.FarmLineItem.Quantity || "0";
            if (parseFloat(vm.FarmLineItem.Quantity) < 1)
                throw "Item Qty is Required";

            vm.FarmLineItem.Price = vm.FarmLineItem.Price || "0";
            if (parseFloat(vm.FarmLineItem.Price) < 1)
                throw "Item Price is Required";

            vm.FarmLineItem.Currency = vm.FarmLineItem.Currency || "";
            if (vm.FarmLineItem.Currency.length < 1)
                throw "Item Currency is Required";

            vm.FarmLineItem.Group = vm.FarmLineItem.Group || {};
            if (vm.FarmLineItem.Group.Id < 1)
                throw "Item Group is Required";

            //if (Object.prototype.toString.call(vm.FarmLineItem.WarrantyStartDate) !== '[object Date]')
            //    throw "Warranty Start Date is invalid";

            //if (Object.prototype.toString.call(vm.FarmLineItem.WarrantyEndDate) !== '[object Date]')
            //    throw "Warranty End Date is invalid";

            console.log("vm.FarmLineItem:", vm.FarmLineItem);

            var url = "farmin-lineitem/save";
            if (vm.Farm.FarmType === "IN" || vm.Farm.FarmType === "OUT") {
                if (vm.Farm.FarmType === "OUT") {
                    url = "farmout-lineitem/save";
                }
                vm.factory.setData(url, vm.FarmLineItem)
                    .then(function () {
                        vm.resetFarmLineItem();
                        vm.getFarmLineItems();
                        new ToastService().success("RECORD HAS BEEN ADDED");
                    },
                        function () {
                            error({ data: { Message: "ERROR WHILE SAVING THE RECORD" } });
                        });
            }
        } catch (e) {
            error({ data: { Message: e } });
        }
    };
    vm.uploadAssetBulkData = function () {
        vm.isSaveOngoing = true;
        vm.apply();
        vm.factory.setData("asset/bulk", vm.AssetBulkItems)
            .then(function (res) {
                vm.isSaveOngoing = false;
                $("#asset-bulk-container").val(null);
                vm.AssetBulkItems.Asset = JSON.parse(JSON.stringify([]));
                var i = JSON.parse(JSON.stringify(res.data));
                if (i.Item2.length > 0) {
                    var errorMessage = "";
                    for (var j = 0; j < i.Item2.length; j++) {
                        var e = i.Item2[j];
                        errorMessage += "Error in No. ".concat(e.No, " :: ", e.ErrorMessage, "\r\n");
                    }
                    new ToastService().warning2("Some items are not uploaded", errorMessage);
                } else {
                    vm.apply();
                    new ToastService().success("RECORDS HAS BEEN UPLOADED");
                }
            },
                function () {
                    vm.isSaveOngoing = false;
                    vm.apply();
                    error({ data: { Message: "ERROR WHILE UPLOADING THE RECORDS" } });
                });

    };
    vm.saveNewAsset = function () {
        try {
            var formData = new FormData();
            angular.forEach(vm.UploadingFile,
                function (value, key) {
                    if (key === "Image") {
                        var val = value;
                        if (val !== null) {
                            formData.append("ImageFile", val);
                        }
                    }
                });
            //console.log("vm.Asset: ", vm.Asset);
            var model = JSON.parse(JSON.stringify(vm.Asset));
            vm.apply();
            model.Id = model.Id || 0;
            model.Region = globalRegion;
            model.ImageFile = null;
            try {
                model.Region = globalRegion;
            } catch (e) {
                //ignore
            }
            try {
                model.Site = "".concat(vm.Asset.Site.ItemName);
            } catch (e) {
                model.Site = null;
            }
            try {
                model.LobOwner = "".concat(vm.Asset.LobOwner.ItemName);
            } catch (e) {
                model.LobOwner = null;
            }
            try {
                model.CreatedBy = username;
                model.UpdatedBy = username;
            } catch (e) {
                model.CreatedBy = null;
                model.UpdatedBy = null;
            }
            vm.apply();
            formData.append("metaData", JSON.stringify(model));
            if (model.Id > 0) {
                vm.factory.uploadDataUpdate("asset/update", formData)
                    .then(function (res) {
                        var i = res.data;
                        if (i.Item1 > 0) {
                            error({ data: { Message: i.Item2 } });
                        } else {
                            new ToastService().success(vm.Notifications.RecordAdded);
                            vm.resetNewAsset();
                        }
                    },
                        function () {
                            error({ data: { Message: vm.Notifications.RecordFailed } });
                        });
            } else {

                vm.factory.uploadData("asset/save", formData)
                    .then(function (res) {
                        var i = res.data;
                        if (i.Item1 > 0) {
                            error({ data: { Message: i.Item2 } });
                        } else {
                            new ToastService().success(vm.Notifications.RecordAdded);
                            vm.resetNewAsset();
                        }
                    },
                        function () {
                            error({ data: { Message: vm.Notifications.RecordFailed } });
                        });
            }
        } catch (e) {
            new ToastService().error(e);
        }
    };

    vm.getTransactionType = function () {
        vm.factory.getData("peza-transaction-type")
            .then(function (res) {
                vm.TransactionTypes = [];
                vm.TransactionTypes = res.data;
                vm.apply();
                //setTimeout(function () {
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                (function () {
                    function codeBlock() {
                        setTimeout(function () {
                            //$("#select-peza-transaction-type").selectpicker("refresh");
                        }, 1000);
                    }
                    codeBlock();
                })();
            },
                error);
    };

    $scope.$watch("vm.Asset.Vendor", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //$("#select-vendor").selectpicker("refresh");
    });
    $scope.$watch("vm.Asset.WorkType", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        (function () {
            function codeBlock() {
                setTimeout(function () {
                    //$("#asset-work-type").selectpicker("refresh");
                }, 1000);
            }
            codeBlock();
        })();
    });
    $scope.$watch("vm.FarmLineItem.Group", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        (function () {
            function codeBlock() {
                //$("#group1").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("vm.FarmLineItem.Currency", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        (function () {
            function codeBlock() {
                //$("#selectCurrency").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("vm.Asset.Site", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //$("#select-site").selectpicker("refresh");
        vm.getLobs();
    });
    $scope.$watch("vm.Asset.Brand", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        (function () {
            function codeBlock() {
                //$("#select-openrecords-brand").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("vm.Asset.AssetStatus", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        (function () {
            function codeBlock() {
                //$("#select-openrecords-status").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("vm.Asset.FinancialTreatment", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        (function () {
            function codeBlock() {
                //$("#select-openrecords-financialtreatment").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("vm.Asset.Category", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //$("#select-category").selectpicker("refresh");
        try {
            vm.getSubCategories(vm.Asset.Category.Id);
        } catch (e) {
            //ignore
        }
    });
    $scope.$watch("vm.OpenRecord.Category", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //$("#select-openrecords-category").selectpicker("refresh");
        try {
            vm.getSubCategories(vm.Asset.Category.Id);
        } catch (e) {
            //ignore
        }
    });
    $scope.$watch("vm.Farm.FarmType", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //$("#select-farm-type").selectpicker("refresh");
    });
    $scope.$watch("vm.Asset.SubCategory", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //$("#select-subcategory").selectpicker("refresh");
    });
    $scope.$watch("vm.OpenRecord.SubCategory", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //$("#select-openrecords-subcategory").selectpicker("refresh");
    });
    $scope.$watch("vm.Asset.Site", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //$("#select-site").selectpicker("refresh");
    });
    $scope.$watch("vm.Asset.LobOwner", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //$("#lobOwner").selectpicker("refresh");
    });
    $scope.$watch("vm.Asset.PezaTransactionType", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //$("#select-peza-transaction-type").selectpicker("refresh");
    });
    $scope.$watch("vm.Farm.Site", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //$("#select-farmin-site").selectpicker("refresh");
    });
    //$scope.$watch("vm.Farm.FarmPermitStatus", function (newValue, oldValue) {
    //    if (!newValue || newValue === oldValue) return;
    //    vm.apply();
    //    (function () {
    //        function codeBlock() {
    //            $("select").selectpicker("refresh");
    //        }
    //        codeBlock();
    //    })();
    //});
    $scope.$watch("vm.Farm.FarmType", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //$("#select-farm-type").selectpicker("refresh");
    });
    $scope.$watch("vm.Farm.Vendor", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //$("#select-farmin-vendor").selectpicker("refresh");
    });
    $scope.$watch("vm.Farm.Email", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        (function () {
            function codeBlock() {
                //$("#select-farm-email").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("vm.Farm.FarmStatus", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        (function () {
            function codeBlock() {
                //$("#select-farmin-status").selectpicker("refresh");
            }
            codeBlock();
        })();
    });

    //$scope.$watch("vm.Farm.PezaPermitStatus", function (newValue, oldValue) {
    //    if (!newValue || newValue === oldValue) return;
    //    vm.apply();
    //    (function () {
    //        function codeBlock() {
    //            $("select").selectpicker("refresh");
    //        }
    //        codeBlock();
    //    })();
    //});

    vm.getSites = function () {
        vm.factory.getData("site-department/site/".concat(globalRegion))
            .then(function (res) {
                vm.Sites = [];
                vm.Sites = res.data;
                vm.apply();
                try {
                    //this happens everytime we load an asset from open records
                    var item = vm.Sites.filter(function (e) {
                        return e.ItemName === vm.Asset.Site;
                    });
                    try {
                        if (item.length > 0) {
                            vm.Asset.Site = item[0];
                        }
                    } catch (e) {
                        vm.Asset.Site = null;
                    }
                } catch (e) {
                    //ignore
                }
                try {
                    //this happens everytime we load an asset from open records
                    var site = vm.Sites.filter(function (e) {
                        return e.ItemName === vm.Farm.Site;
                    });
                    try {
                        if (site.length > 0) {
                            vm.Farm.Site = site[0];
                        }
                    } catch (e) {
                        vm.Farm.Site = null;
                    }
                } catch (e) {
                    //ignore
                }
                //setTimeout(function () {
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                (function () {
                    function codeBlock() {
                        //$("#select-openrecords-site, #select-farmin-site").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                error);
    };

    vm.getLobs = function () {
        vm.factory.getData("site-department/department/".concat(globalRegion))
            .then(function (res) {
                vm.Lobs = [];
                vm.Lobs = res.data;
                vm.apply();
                //this happens everytime we load an asset from open records
                var item = vm.Lobs.filter(function (e) {
                    return e.ItemName === vm.Asset.LobOwner;
                });
                try {
                    if (item.length > 0) {
                        vm.Asset.LobOwner = item[0];
                    }
                } catch (e) {
                    vm.Asset.LobOwner = null;
                }
                //setTimeout(function () {
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                (function () {
                    function codeBlock() {
                        setTimeout(function () {
                            //$("lobOwner").selectpicker("refresh");
                        }, 1000);
                    }
                    codeBlock();
                })();
            },
                error);
    };

    vm.getSubCategories = function (id) {
        vm.factory.getData("subcategory/parent/".concat(id))
            .then(function (res) {
                vm.SubCategories = [];
                vm.SubCategories = res.data;
                vm.apply();
                //setTimeout(function () {
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                (function () {
                    function codeBlock() {
                        setTimeout(function () {
                            //$("#select-openrecords-subcategory").selectpicker("refresh");
                        }, 1000);
                    }
                    codeBlock();
                })();
            },
                error);
    };

    vm.getCategories = function () {
        vm.Categories = [];
        vm.SubCategories = [];
        vm.apply();
        vm.factory.getData("category")
            .then(function (res) {
                vm.Categories = [];
                vm.Categories = res.data;
                vm.apply();
                //setTimeout(function () {
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                (function () {
                    function codeBlock() {
                        setTimeout(function () {
                            //$("#select-openrecords-category").selectpicker("refresh");
                        }, 1000);
                    }
                    codeBlock();
                })();
            },
                error);
    };

    vm.getGroups = function () {
        vm.Groups = [];
        vm.apply();
        vm.factory.getData("group")
            .then(function (res) {
                vm.Groups = [];
                vm.Groups = res.data;
                vm.apply();
                //setTimeout(function () {
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                (function () {
                    function codeBlock() {
                        setTimeout(function () {
                            //$("#group1,#group2").selectpicker("refresh");
                        }, 1000);
                    }
                    codeBlock();
                })();
            },
                error);
    };

    vm.getVendors = function () {
        vm.factory.getData("vendor")
            .then(function (res) {
                vm.Vendors = [];
                vm.Vendors = res.data;
                vm.apply();
                //setTimeout(function () {
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                (function () {
                    function codeBlock() {
                        setTimeout(function () {
                            //$("#select-farmin-vendor, #select-openrecords-vendor").selectpicker("refresh");
                        }, 1000);
                    }
                    codeBlock();
                })();
            },
                error);
    };

    vm.getPurchaseOrderNos = function () {
        vm.factory.getData("vendor")
            .then(function (res) {
                vm.PurchaseOrderNos = [];
                vm.PurchaseOrderNos = res.data;
                vm.apply();
                //setTimeout(function () {
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                (function () {
                    function codeBlock() {
                        setTimeout(function () {
                            //$("select").selectpicker("refresh");
                        }, 1000);
                    }
                    codeBlock();
                })();
            },
                error);
    };
    vm.getBrands = function () {
        vm.factory.getData("brand")
            .then(function (res) {
                vm.Brands = [];
                vm.Brands = res.data;
                vm.apply();
                //setTimeout(function () {
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                (function () {
                    function codeBlock() {
                        setTimeout(function () {
                            //$("#select-openrecords-brand").selectpicker("refresh");
                        }, 1000);
                    }
                    codeBlock();
                })();
            },
                error);
    };
    vm.getFinancialTreatments = function () {
        vm.factory.getData("financial-treatment")
            .then(function (res) {
                vm.FinancialTreatments = [];
                vm.FinancialTreatments = res.data;
                vm.apply();
                //setTimeout(function () {
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                (function () {
                    function codeBlock() {
                        setTimeout(function () {
                            //$("#select-openrecords-financialtreatment").selectpicker("refresh");
                        }, 1000);
                    }
                    codeBlock();
                })();
            },
                error);
    };
    vm.getAssets = function () {
        vm.Assets = [];
        vm.apply();
        vm.factory.getData("asset?group=".concat(vm.AssetByRole))
            .then(function (res) {
                vm.Assets = [];
                vm.Assets = res.data;
                vm.apply();
            },
                error);
    };

    vm.getEmails = function () {
        vm.Emails = [];
        vm.apply();
        vm.factory.getData("email")
            .then(function (res) {
                vm.Emails = [];
                vm.Emails = res.data;
                vm.apply();
                //setTimeout(function () {
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                (function () {
                    function codeBlock() {
                        setTimeout(function () {
                            //$("#select-farm-email").selectpicker("refresh");
                        }, 1000);
                    }
                    codeBlock();
                })();
            },
                error);
    };

    vm.getFarmInStatus = function () {
        vm.factory.getData("farmstatus")
            .then(function (res) {
                vm.FarmStatus = [];
                vm.FarmStatus = res.data;
                vm.apply();
                //setTimeout(function () {
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 800);
                (function () {
                    function codeBlock() {
                        setTimeout(function () {
                            //$("#select-farmin-status").selectpicker("refresh");
                        }, 800);
                    }
                    codeBlock();
                })();
            },
                error);
    };
    vm.FarmAttachments = [];
    vm.getFarmAttachments = function () {
        $("#farm-attachment-loader").removeClass("no-display");
        vm.FarmAttachments = JSON.parse(JSON.stringify([]));
        vm.apply();
        vm.Farm.FarmType = vm.Farm.FarmType || "";
        vm.Farm.FarmType = vm.Farm.FarmType.toUpperCase();
        var url = "farmin-attachment/parent/".concat(vm.Farm.Id || -1);
        if (vm.Farm.FarmType === "OUT") {
            url = "farmout-attachment/parent/".concat(vm.Farm.Id || -1);
        }
        vm.factory.getData(url)
            .then(function (res) {
                vm.FarmAttachments = res.data;
                $("#farm-attachment-loader").addClass("no-display");
                vm.apply();
            },
                error);
    };

    vm.AssetAttachments = [];
    vm.getAssetAttachments = function () {
        $("#asset-attachment-loader").removeClass("no-display");
        vm.AssetAttachments = JSON.parse(JSON.stringify([]));
        vm.apply();
        vm.factory.getData("asset-attachment/parent/".concat(vm.Asset.Id || -1))
            .then(function (res) {
                vm.AssetAttachments = res.data;
                $("#asset-attachment-loader").addClass("no-display");
                vm.apply();
            },
                error);
    };

    vm.getFarmLineItems = function () {
        vm.Farm.FarmType = vm.Farm.FarmType || "";
        vm.Farm.FarmType = vm.Farm.FarmType.toUpperCase();
        var url = globalApiBaseUrl.concat("farmin-lineitem/parent/", vm.Farm.Id || -1);
        if (vm.Farm.FarmType === "OUT") {
            url = globalApiBaseUrl.concat("farmout-lineitem/parent/", vm.Farm.Id || -1);
        }
        vm.dtInstance_farmLineItems.DataTable.ajax.reload();
        vm.dtInstance_farmLineItems.DataTable.clear().draw();
        setTimeout(function () {
            vm.dtInstance_farmLineItems.DataTable.ajax.url(url);
            vm.dtInstance_farmLineItems.DataTable.ajax.reload();
            setTimeout(function () {
                vm.dtInstance_farmLineItems.DataTable
                    .columns
                    .adjust()
                    .responsive
                    .recalc()
                    .scroller
                    .measure()
                    .draw();
            }, 500);
        }, 500);
        setTimeout(function () {
        }, 800);
    };

    vm.openAssetWindow = function () {
        vm.resetNewAsset();
        vm.dtInstance.DataTable.clear().draw();
        var url = globalApiBaseUrl.concat("asset?group=", vm.AssetByRole);
        vm.dtInstance.DataTable.ajax.url(url);
        vm.dtInstance.DataTable.ajax.reload();
        setTimeout(function () {
            vm.dtInstance.DataTable
                .columns
                .adjust()
                .responsive
                .recalc()
                .scroller
                .measure()
                .draw();
        }, 800);
        $("#assetModal").modal("show");
    };

    vm.getAssetById = function (obj, ctrlNo) {
        $("#assetModal").modal("hide");
        var id = parseInt(obj);
        if (isNaN(id)) {
            id = 0;
        }
        vm.factory.getData("asset/".concat(id))
            .then(function (res) {
                vm.Asset = {};
                vm.Asset = JSON.parse(JSON.stringify(res.data));
                vm.Asset.ImageFile = vm.Asset.ImagePicture;
                if (vm.Asset.Category !== null && vm.Asset.Category !== undefined) {
                    vm.getSubCategories(vm.Asset.Category.Id);
                }
                vm.getSites();
                vm.getLobs();
                if (vm.Asset.DateDepreciated !== null && vm.Asset.DateDepreciated.length > 0) {
                    vm.Asset.DateDepreciated = moment(vm.Asset.DateDepreciated, "M/D/YYYY").toDate();
                }
                if (vm.Asset.TagDate !== null && vm.Asset.TagDate.length > 0) {
                    vm.Asset.TagDate = moment(vm.Asset.TagDate, "M/D/YYYY").toDate();
                }
                if (vm.Asset.Contract_ContractEndDate !== null && vm.Asset.Contract_ContractEndDate.length > 0) {
                    vm.Asset.Contract_ContractEndDate = moment(vm.Asset.Contract_ContractEndDate, "M/D/YYYY").toDate();
                }
                if (vm.Asset.Contract_ContractStartDate !== null && vm.Asset.Contract_ContractStartDate.length > 0) {
                    vm.Asset.Contract_ContractStartDate = moment(vm.Asset.Contract_ContractStartDate, "M/D/YYYY").toDate();
                }
                if (vm.Asset.DateDepreciated !== null && vm.Asset.DateDepreciated.length > 0) {
                    vm.Asset.DateDepreciated = moment(vm.Asset.DateDepreciated, "M/D/YYYY").toDate();
                }
                if (vm.Asset.ServiceDate !== null && vm.Asset.ServiceDate.length > 0) {
                    vm.Asset.ServiceDate = moment(vm.Asset.ServiceDate, "M/D/YYYY").toDate();
                }
                vm.Asset.FarmIn = vm.Asset.FarmIn || {};
                if (vm.Asset.FarmIn.DeliveryDate !== null && vm.Asset.FarmIn.DeliveryDate.length > 0) {
                    vm.Asset.DeliveryDate = moment(vm.Asset.FarmIn.DeliveryDate, "M/D/YYYY").toDate();
                }

                var tmpArrayOfAssetLineItem = [];
                try {
                    tmpArrayOfAssetLineItem = vm.Asset.FarmIn.FarmLineItemViewModel.filter(function (x) {
                        return x.Id === parseInt(vm.Asset.FarmLineItemId);
                    });
                } catch (e) {
                    tmpArrayOfAssetLineItem = [];
                }
                if (tmpArrayOfAssetLineItem.length > 0) {
                    var lineItem = tmpArrayOfAssetLineItem[0];
                    if (lineItem.WarrantyEndDate !== null && lineItem.WarrantyEndDate.length > 0) {
                        vm.Asset.WarrantyEndDate = moment(lineItem.WarrantyEndDate, "M/D/YYYY").toDate();
                    }
                    if (lineItem.WarrantyStartDate !== null && lineItem.WarrantyStartDate.length > 0) {
                        vm.Asset.WarrantyStartDate = moment(lineItem.WarrantyStartDate, "M/D/YYYY").toDate();
                    }
                    vm.Asset.PurchasePrice = toCurrencyFormat(lineItem.Price);
                    vm.Asset.Group = lineItem.Group;
                }
                vm.Asset.PezaFarmInControlNo = vm.Asset.FarmIn.ControlNo;
                vm.Asset.PezaFarmInPermitStatus = vm.Asset.FarmIn.PezaPermitStatus;
                vm.Asset.PezaFarmInFormNo = vm.Asset.FarmIn.PezaFormNo;
                vm.Asset.PezaFarmInApprovalNo = vm.Asset.FarmIn.PezaApprovalNo;

                vm.Asset.FarmOut = vm.Asset.FarmOut || {};
                vm.Asset.PezaFarmOutControlNo = vm.Asset.FarmOut.ControlNo;
                vm.Asset.PezaFarmOutPermitStatus = vm.Asset.FarmOut.PezaPermitStatus;
                vm.Asset.PezaFarmOutFormNo = vm.Asset.FarmOut.PezaFormNo;
                vm.Asset.PezaFarmOutApprovalNo = vm.Asset.FarmOut.PezaApprovalNo;

                if (vm.Asset.Maintenance_MaintenanceEndDate !== null && vm.Asset.Maintenance_MaintenanceEndDate.length > 0) {
                    vm.Asset.Maintenance_MaintenanceEndDate = moment(vm.Asset.Maintenance_MaintenanceEndDate, "M/D/YYYY").toDate();
                }
                if (vm.Asset.Maintenance_MaintenanceStartDate !== null && vm.Asset.Maintenance_MaintenanceStartDate.length > 0) {
                    vm.Asset.Maintenance_MaintenanceStartDate = moment(vm.Asset.Maintenance_MaintenanceStartDate, "M/D/YYYY").toDate();
                }
                if (vm.Asset.FarmIn.DateCreated !== null && vm.Asset.FarmIn.DateCreated.length > 0) {
                    vm.Asset.PezaFarmInDate = moment(vm.Asset.FarmIn.DateCreated, "M/D/YYYY").toDate();
                }
                if (vm.Asset.FarmIn.PurchaseOrderDate !== null && vm.Asset.FarmIn.PurchaseOrderDate.length > 0) {
                    vm.Asset.PurchaseDate = moment(vm.Asset.FarmIn.PurchaseOrderDate, "M/D/YYYY").toDate();
                }
                vm.apply();
                //setTimeout(function () {
                //    vm.getAssetAttachments();
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                //(function () {
                //    function codeBlock() {
                //        $("select").selectpicker("refresh");
                //    }
                //    codeBlock();
                //})();
            },
                error);
    };

    vm.getAssetStatus = function () {
        vm.factory.getData("assetstatus")
            .then(function (res) {
                vm.AssetStatus = [];
                vm.AssetStatus = JSON.parse(JSON.stringify(res.data));
                //setTimeout(function () {
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                (function () {
                    function codeBlock() {
                        setTimeout(function () {
                            //$("#select-openrecords-status").selectpicker("refresh");
                        }, 1000);
                    }
                    codeBlock();
                })();
            },
                error);
    };

    vm.getFarmInById = function (objId, objCtrlNo) {
        $("#farmInModal").modal("hide");
        var farmType = "IN";
        if (objCtrlNo.indexOf("FARMOUT") > -1) {
            farmType = "OUT";
        }
        var id = parseInt(objId);
        if (isNaN(id)) {
            id = 0;
        }
        if (farmType === "IN" || farmType === "OUT") {
            var url = "farmin/".concat(id);
            if (farmType === "OUT") {
                url = "farmout/".concat(id);
            }
            vm.factory.getData(url)
                .then(function (res) {
                    vm.Farm = JSON.parse(JSON.stringify(vm.FarmRaw));
                    vm.Farm = res.data;
                    vm.Farm.FarmType = farmType;

                    if (vm.Farm.AirwayDeliveryDate !== "") {
                        vm.Farm.AirwayDeliveryDate = moment(vm.Farm.AirwayDeliveryDate, "M/D/YYYY").toDate();
                    }
                    if (vm.Farm.DeliveryDate !== "") {
                        vm.Farm.DeliveryDate = moment(vm.Farm.DeliveryDate, "M/D/YYYY").toDate();
                    }
                    if (vm.Farm.PurchaseOrderDate !== "") {
                        vm.Farm.PurchaseOrderDate = moment(vm.Farm.PurchaseOrderDate, "M/D/YYYY").toDate();
                    }
                    if (vm.Farm.ProformaInvoiceDate !== "") {
                        vm.Farm.ProformaInvoiceDate = moment(vm.Farm.ProformaInvoiceDate, "M/D/YYYY").toDate();
                    }
                    if (vm.Farm.FinalInvoiceDate !== "") {
                        vm.Farm.FinalInvoiceDate = moment(vm.Farm.FinalInvoiceDate, "M/D/YYYY").toDate();
                    }
                    if (vm.Farm.DateReceived !== "") {
                        vm.Farm.DateReceived = moment(vm.Farm.DateReceived, "M/D/YYYY").toDate();
                    }

                    vm.apply();
                    vm.getSites();
                    vm.getFarmLineItems();
                    vm.getFarmAttachments();
                    //setTimeout(function () {
                    //    (function () {
                    //        function codeBlock() {
                    //            $("select").selectpicker("refresh");
                    //        }
                    //        codeBlock();
                    //    })();
                    //}, 1000);
                    //(function () {
                    //    function codeBlock() {
                    //        $("select").selectpicker("refresh");
                    //    }
                    //    codeBlock();
                    //})();
                },
                    error);
        }
    };


    function s2ab(s) {
        var buf = new ArrayBuffer(s.length);
        var view = new Uint8Array(buf);
        for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
        return buf;
    }

    vm.getFarmAttachmentById = function (Id, FileName) {
        Id = Id || 0;
        FileName = FileName || "";
        FileName = FileName.toLowerCase();
        var farmType = "IN";
        if (FileName.indexOf("farmout") > -1) {
            farmType = "OUT";
        }
        if (farmType === "IN" || farmType === "OUT") {
            var url = "farmin-attachment/".concat(Id);
            if (farmType === "OUT") {
                url = "farmout-attachment/".concat(Id);
            }
            vm.factory.getData(url)
                .then(function (res) {
                    var item = res.data;
                    vm.downloadAttachment(item.FileBase64, item.FileName);
                },
                    error);
        }
    };

    vm.getAssetAttachmentById = function (Id, FileName) {
        Id = Id || 0;
        vm.factory.getData("asset-attachment/".concat(Id))
            .then(function (res) {
                var item = res.data;
                vm.downloadAttachment(item.FileBase64, item.FileName);
            },
                error);
    };

    vm.removeFarmLineItem = function (Id, FileName) {
        Id = Id || 0;
        FileName = FileName || "";
        FileName = FileName.toLowerCase();
        var farmType = "IN";
        if (FileName.indexOf("farmout") > -1) {
            farmType = "OUT";
        }
        if (farmType === "IN" || farmType === "OUT") {
            var url = "farmin-lineitem/delete/".concat(Id, "/", username);
            if (farmType === "OUT") {
                url = "farmout-lineitem/delete/".concat(Id, "/", username);
            }
            vm.factory.deleteData(url)
                .then(function () {
                    vm.getFarmLineItems();
                    new ToastService().success("RECORD HAS BEEN DELETED");
                },
                    error);
        }
    };

    vm.downloadAttachment = function (base64FileString, fileName) {
        try {
            var arr = base64FileString.split(",");
            var contentType = arr[0].split(":")[1].split(";")[0];
            var data = arr[1];
            var blob = new Blob([s2ab(atob(data))], {
                type: contentType
            });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = fileName;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        } catch (e) {
            //console.log("Error in vm.downloadAttachment: ", e);
            error({ data: { Message: "Cannot download attachment" } });
        }
    };

    vm.openFarmSearch = function (farmType) {
        farmType = farmType || "";
        farmType = farmType.toUpperCase();
        if (farmType === "IN" || farmType === "OUT") {
            var url = globalApiBaseUrl.concat("farmin");
            if (farmType === "OUT") {
                url = globalApiBaseUrl.concat("farmout");
            }
            $("#farmInModal").modal("show");
            vm.dtInstance_farmIn.DataTable.clear().draw();
            setTimeout(function () {
                vm.dtInstance_farmIn.DataTable.ajax.url(url);
                vm.dtInstance_farmIn.DataTable.ajax.reload();
                setTimeout(function () {
                    vm.dtInstance_farmIn.DataTable.columns.adjust().draw();
                }, 500);
            }, 500);
        }
    };
    vm.SearchPurchaseOrderNo = "";
    vm.openAssetModal = function () {
        vm.resetNewAsset();
        $("#newAssetModal").modal("show");
        vm.dtInstance_farmLineItemsByFilter.DataTable.clear().draw();
        //vm.dtInstance_farmLineItemsByFilter.DataTable.ajax.url(globalApiBaseUrl.concat("farmin"));
        //vm.dtInstance_farmLineItemsByFilter.DataTable.ajax.reload();
        //setTimeout(function () {
        //    vm.dtInstance_farmLineItemsByFilter.DataTable.columns.adjust().draw();
        //}, 100);
    };

    vm.getFarmLineItemsByPurchaseOrderNo = function () {
        var purchaseOrderNo = vm.SearchPurchaseOrderNo || "";
        vm.dtInstance_farmLineItemsByFilter.DataTable.clear().draw();
        if (purchaseOrderNo.trim().length > 0) {
            vm.factory.getData("farmin/search/".concat(purchaseOrderNo))
                .then(function (res) {
                    var item = res.data;
                    if (item === null) {
                        new ToastService().error("NO RECORD FOUND");
                    } else if (item.FarmLineItemViewModel.length < 1) {
                        new ToastService().error("NO LINE ITEM FOUND");
                    } else {
                        vm.dtInstance_farmLineItemsByFilter.DataTable.ajax.url(globalApiBaseUrl.concat("farmin-lineitem/parent/", item.Id));
                        vm.dtInstance_farmLineItemsByFilter.DataTable.ajax.reload();
                    }
                },
                    error);
        }
    };

    function toCurrencyFormat(data) {
        data = data || 0;
        var currency = new Intl.NumberFormat();
        return currency.format(data);
    }
    //vm.getFilteredFarmLineItemData = function (id, description, warrantyStartDate, warrantyEndDate) {
    vm.getFilteredFarmLineItemData = function (id, lineId) {
        id = id || 0;
        id = parseInt(id);
        if (isNaN(id)) {
            id = 0;
        }
        var lineItem = {};
        vm.factory.getData("farmin-lineitem/".concat(lineId))
            .then(function (res) {
                lineItem = res.data;
                vm.apply();
            },
                error);
        lineItem.WarrantyStartDate = lineItem.WarrantyStartDate || "";
        lineItem.WarrantyEndDate = lineItem.WarrantyEndDate || "";
        vm.factory.getData("farmin/".concat(id))
            .then(function (res) {
                var item = res.data;
                //console.log(item);
                vm.Asset.Description = lineItem.Description;
                vm.Asset.Vendor = item.Vendor;
                vm.Asset.FarmLineItemId = parseInt(lineId);
                if (lineItem.WarrantyStartDate.length > 0) {
                    try {
                        vm.Asset.WarrantyStartDate = moment(lineItem.WarrantyStartDate, "M/D/YYYY").toDate();
                    } catch (e) {
                        vm.Asset.WarrantyStartDate = null;
                    }
                }
                if (lineItem.WarrantyEndDate.length > 0) {
                    try {
                        vm.Asset.WarrantyEndDate = moment(lineItem.WarrantyEndDate, "M/D/YYYY").toDate();
                    } catch (e) {
                        vm.Asset.WarrantyEndDate = null;
                    }
                }
                if (item.DeliveryDate !== null && item.DeliveryDate.length > 0) {
                    try {
                        vm.Asset.DeliveryDate = moment(item.DeliveryDate, "M/D/YYYY").toDate();
                    } catch (e) {
                        vm.Asset.DeliveryDate = null;
                    }
                }
                vm.Asset.DeliveryNo = item.DeliveryNo;
                if (item.FinalInvoiceDate !== null && item.FinalInvoiceDate.length > 0) {
                    try {
                        vm.Asset.InvoiceDate = moment(item.FinalInvoiceDate, "M/D/YYYY").toDate();
                    } catch (e) {
                        vm.Asset.InvoiceDate = null;
                    }
                }
                if (item.PurchaseOrderDate !== null && item.PurchaseOrderDate.length > 0) {
                    try {
                        vm.Asset.PurchaseDate = moment(item.PurchaseOrderDate, "M/D/YYYY").toDate();
                    } catch (e) {
                        vm.Asset.PurchaseDate = null;
                    }
                }
                vm.Asset.InvoiceNo = item.FinalInvoiceNo;
                vm.Asset.PezaFarmInApprovalNo = item.PezaApprovalNo;
                vm.Asset.PezaFarmInFormNo = item.PezaFormNo;
                vm.Asset.PezaFarmInPermitNo = item.PezaPermitNo;
                vm.Asset.PezaFarmInPermitStatus = item.PezaPermitStatus;
                vm.Asset.PezaFarmInControlNo = item.ControlNo;
                vm.Asset.PuchaseOrderNo = item.PurchaseOrderNo;
                vm.Asset.PurchasePrice = toCurrencyFormat(lineItem.Price);
                vm.Asset.Group = lineItem.Group;
                try {
                    vm.Asset.Site = vm.Sites.filter(function (x) {
                        return x.ItemName === item.Site;
                    })[0];
                } catch (e) {
                    vm.Asset.Site = null;
                }
                vm.apply();
                //setTimeout(function () {
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                //(function () {
                //    function codeBlock() {
                //        $("select").selectpicker("refresh");
                //    }
                //    codeBlock();
                //})();
                $("#newAssetModal").modal("hide");
            },
                error);
    };

    vm.FarmPermitStatus = ["YES", "NO", "N/A"];

    vm.getQrCodeData = function (content) {
        vm.Asset = {};
        $("#data-loader").removeClass("no-display");
        vm.factory.getData("asset/qr/".concat(content))
            .then(function (res) {
                vm.Asset = {};
                vm.Asset = JSON.parse(JSON.stringify(res.data));
                vm.Asset.ImageFile = vm.Asset.ImagePicture;
                if (vm.Asset.Category !== null && vm.Asset.Category !== undefined) {
                    vm.getSubCategories(vm.Asset.Category.Id);
                }
                vm.getSites();
                vm.getLobs();
                if (vm.Asset.DateDepreciated !== null && vm.Asset.DateDepreciated.length > 0) {
                    vm.Asset.DateDepreciated = moment(vm.Asset.DateDepreciated, "M/D/YYYY").toDate();
                }
                if (vm.Asset.TagDate !== null && vm.Asset.TagDate.length > 0) {
                    vm.Asset.TagDate = moment(vm.Asset.TagDate, "M/D/YYYY").toDate();
                }
                if (vm.Asset.Contract_ContractEndDate !== null && vm.Asset.Contract_ContractEndDate.length > 0) {
                    vm.Asset.Contract_ContractEndDate = moment(vm.Asset.Contract_ContractEndDate, "M/D/YYYY").toDate();
                }
                if (vm.Asset.Contract_ContractStartDate !== null && vm.Asset.Contract_ContractStartDate.length > 0) {
                    vm.Asset.Contract_ContractStartDate = moment(vm.Asset.Contract_ContractStartDate, "M/D/YYYY").toDate();
                }
                if (vm.Asset.DateDepreciated !== null && vm.Asset.DateDepreciated.length > 0) {
                    vm.Asset.DateDepreciated = moment(vm.Asset.DateDepreciated, "M/D/YYYY").toDate();
                }
                if (vm.Asset.ServiceDate !== null && vm.Asset.ServiceDate.length > 0) {
                    vm.Asset.ServiceDate = moment(vm.Asset.ServiceDate, "M/D/YYYY").toDate();
                }
                vm.Asset.FarmIn = vm.Asset.FarmIn || {};
                if (vm.Asset.FarmIn.DeliveryDate !== null && vm.Asset.FarmIn.DeliveryDate.length > 0) {
                    vm.Asset.DeliveryDate = moment(vm.Asset.FarmIn.DeliveryDate, "M/D/YYYY").toDate();
                }

                var tmpArrayOfAssetLineItem = [];
                try {
                    tmpArrayOfAssetLineItem = vm.Asset.FarmIn.FarmLineItemViewModel.filter(function (x) {
                        return x.Id === parseInt(vm.Asset.FarmLineItemId);
                    });
                } catch (e) {
                    tmpArrayOfAssetLineItem = [];
                }
                if (tmpArrayOfAssetLineItem.length > 0) {
                    var lineItem = tmpArrayOfAssetLineItem[0];
                    if (lineItem.WarrantyEndDate !== null && lineItem.WarrantyEndDate.length > 0) {
                        vm.Asset.WarrantyEndDate = moment(lineItem.WarrantyEndDate, "M/D/YYYY").toDate();
                    }
                    if (lineItem.WarrantyStartDate !== null && lineItem.WarrantyStartDate.length > 0) {
                        vm.Asset.WarrantyStartDate = moment(lineItem.WarrantyStartDate, "M/D/YYYY").toDate();
                    }
                    vm.Asset.PurchasePrice = toCurrencyFormat(lineItem.Price);
                }
                vm.Asset.PezaFarmInControlNo = vm.Asset.FarmIn.ControlNo;
                vm.Asset.PezaFarmInPermitStatus = vm.Asset.FarmIn.PezaPermitStatus;
                vm.Asset.PezaFarmInFormNo = vm.Asset.FarmIn.PezaFormNo;
                vm.Asset.PezaFarmInApprovalNo = vm.Asset.FarmIn.PezaApprovalNo;

                vm.Asset.FarmOut = vm.Asset.FarmOut || {};
                vm.Asset.PezaFarmOutControlNo = vm.Asset.FarmOut.ControlNo;
                vm.Asset.PezaFarmOutPermitStatus = vm.Asset.FarmOut.PezaPermitStatus;
                vm.Asset.PezaFarmOutFormNo = vm.Asset.FarmOut.PezaFormNo;
                vm.Asset.PezaFarmOutApprovalNo = vm.Asset.FarmOut.PezaApprovalNo;

                if (vm.Asset.Maintenance_MaintenanceEndDate !== null && vm.Asset.Maintenance_MaintenanceEndDate.length > 0) {
                    vm.Asset.Maintenance_MaintenanceEndDate = moment(vm.Asset.Maintenance_MaintenanceEndDate, "M/D/YYYY").toDate();
                }
                if (vm.Asset.Maintenance_MaintenanceStartDate !== null && vm.Asset.Maintenance_MaintenanceStartDate.length > 0) {
                    vm.Asset.Maintenance_MaintenanceStartDate = moment(vm.Asset.Maintenance_MaintenanceStartDate, "M/D/YYYY").toDate();
                }
                if (vm.Asset.FarmIn.DateCreated !== null && vm.Asset.FarmIn.DateCreated.length > 0) {
                    vm.Asset.PezaFarmInDate = moment(vm.Asset.FarmIn.DateCreated, "M/D/YYYY").toDate();
                }
                if (vm.Asset.FarmIn.PurchaseOrderDate !== null && vm.Asset.FarmIn.PurchaseOrderDate.length > 0) {
                    vm.Asset.PurchaseDate = moment(vm.Asset.FarmIn.PurchaseOrderDate, "M/D/YYYY").toDate();
                }
                vm.apply();
                //setTimeout(function () {
                //    vm.getAssetAttachments();
                //    (function () {
                //        function codeBlock() {
                //            $("select").selectpicker("refresh");
                //        }
                //        codeBlock();
                //    })();
                //}, 1000);
                //(function () {
                //    function codeBlock() {
                //        $("select").selectpicker("refresh");
                //    }
                //    codeBlock();
                //})();
            },
                function () {
                    $("#data-loader").addClass("no-display");
                    new ToastService().error("ERROR WHILE SAVING THE RECORD");
                });
    };
    vm.Scanner = {};
    vm.openQrCodeModal = function () {
        $("#preview-container").html('<video id="preview"></video>');
        if (vm.Scanner !== null && vm.Scanner !== undefined) {
            try {
                vm.Scanner.stop();
            } catch (e) {
                //ignore
            }
        }
        vm.Scanner = new window.Instascan.Scanner({
            video: document.getElementById("preview"),
            continuous: true,
            scanPeriod: 1
        });
        vm.Scanner.addListener("scan",
            function (content) {
                this.stop();
                $("#qrCodeModal").modal("hide");
                vm.getQrCodeData(content);
            });
        window.Instascan.Camera.getCameras().then(function (cameras) {
            if (cameras.length > 0) {
                vm.Scanner.start(cameras[cameras.length - 1]);
            } else {
                //console.log("No cameras found.");
                new ToastService().error("No camera is available");
            }
        }).catch(function (e) {
            //console.log(e);
            new ToastService().error("Cannot initialized the camera. Check if camera is being used by other application");
        });
        $("#qrCodeModal").modal("show");
    };
    vm.stopCamera = function () {
        if (vm.Scanner !== null && vm.Scanner !== undefined) {
            try {
                vm.Scanner.stop();
            } catch (e) {
                //ignore
            }
        }
    };
    vm.openEmployeeWindow = function () {
        $("#employeeModal").modal("show");
    };
    vm.TmpEmployee = {};
    vm.getEmployeeByHrid = function () {
        vm.TmpEmployee = JSON.parse(JSON.stringify({}));
        vm.apply();
        //vm.SearchEmployeeHrid = "";
        if (vm.SearchEmployeeHrid.length > 0) {
            vm.factory.getData("employee/".concat(vm.SearchEmployeeHrid))
                .then(function (res) {
                    vm.TmpEmployee = JSON.parse(JSON.stringify(res.data));
                    vm.apply();
                },
                    error);
        }
    };
    vm.useSearchedEmployee = function () {
        vm.Asset.Employee = JSON.parse(JSON.stringify(vm.TmpEmployee));
        //console.log("vm.Asset.Employee: ", vm.Asset.Employee);
        vm.TmpEmployee = JSON.parse(JSON.stringify({}));
        $("#employeeModal").modal("hide");
    };
    vm.AssetByRole = "";
    vm.Load = function () {
        var it = globalRoles.filter(function (x) {
            return x.toUpperCase() === "IT";
        });
        var reaf = globalRoles.filter(function (x) {
            return x.toUpperCase() === "REAF";
        });
        if (it.length > 0) {
            vm.AssetByRole = "IT";
        }
        if (reaf.length > 0) {
            vm.AssetByRole = "REAF";
        }
        var administrator = globalRoles.filter(function (x) {
            return x.toUpperCase() === "ADMINISTRATOR";
        });
        if ((it.length > 0 && reaf.length > 0) || administrator.length > 0) {
            vm.AssetByRole = "";
        }
        vm.apply();
        vm.Asset.Region = (globalRegion || "").toUpperCase();
        vm.OpenRecord = JSON.parse(JSON.stringify(vm.AssetRaw));
        vm.getSites();
        vm.getTransactionType();
        vm.getFinancialTreatments();
        vm.getBrands();
        vm.getVendors();
        vm.getCategories();
        //vm.getPurchaseOrderNos();
        vm.getEmails();
        vm.getFarmInStatus();
        vm.getGroups();
        vm.getAssetStatus();
        //setTimeout(function () {
        //    $(".dataTables_filter").remove();
        //    (function () {
        //        function codeBlock() {
        //            $("select").selectpicker("refresh");
        //        }
        //        codeBlock();
        //    })();
        //    //console.log("vm.dtInstance_farmLineItems: ", vm.dtInstance_farmLineItems);
        //}, 1000);
        //(function () {
        //    function codeBlock() {
        //        $(".dataTables_filter").remove();
        //        setTimeout(function () {
        //            $("#farm-tab select").selectpicker("refresh");
        //        }, 500);
        //        setTimeout(function () {
        //            $("#home select").selectpicker("refresh");
        //        }, 500);
        //    }
        //    codeBlock();
        //})();
    };
    vm.Load();
}