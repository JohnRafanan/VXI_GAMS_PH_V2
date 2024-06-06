app.controller("qrCtrl", qrCtrl);
qrCtrl.$inject = ["$rootScope", "$scope", "$http", "DTOptionsBuilder", "DTColumnBuilder", "ApiFactory", "$timeout"];
function qrCtrl($rootScope, $scope, $http, DTOptionsBuilder, DTColumnBuilder, ApiFactory, $timeout) {
    var vm = this;

    vm.apply = function () {
        try {
            $scope.$apply();
        } catch (e) {
            //ignore
        }
    };

    function toCurrencyFormat(data) {
        data = data || 0;
        var currency = new Intl.NumberFormat();
        return currency.format(data);
    }

    vm.factory = new ApiFactory();
    vm.Asset = {};
    vm.getQrCodeData = function (content) {
        vm.Asset = {};
        $("#data-loader").removeClass("no-display");
        vm.factory.getData("asset/qr/".concat(content))
            .then(function (res) {
                console.log("res:", res.data);
                vm.Asset = {};
                $("#data-loader").addClass("no-display");
                vm.Asset = JSON.parse(JSON.stringify(res.data));

                vm.Asset.ImageFile = vm.Asset.ImagePicture;
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
                vm.Asset.FarmIn = vm.Asset.FarmIn || {};
                if (vm.Asset.FarmIn.DeliveryDate !== null && vm.Asset.FarmIn.DeliveryDate.length > 0) {
                    vm.Asset.DeliveryDate = moment(vm.Asset.FarmIn.DeliveryDate, "M/D/YYYY").toDate();
                }

                var tmpArrayOfAssetLineItem = [];
                try {
                    tmpArrayOfAssetLineItem = vm.Asset.FarmIn.FarmLineItemViewModel.filter(function (x) {
                        return x.Description === vm.Asset.Description;
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
        vm.Scanner = new window.Instascan.Scanner({ video: document.getElementById("preview") });
        vm.Scanner.addListener("scan",
            function (content) {
                this.stop();
                $("#qrCodeModal").modal("hide");
                new ToastService().info(content);
                vm.getQrCodeData(content);
            });
        window.Instascan.Camera.getCameras().then(function (cameras) {
            if (cameras.length > 0) {
                vm.Scanner.start(cameras[cameras.length - 1]);
            } else {
                console.log("No cameras found.");
                new ToastService().error("No camera is available");
            }
        }).catch(function (e) {
            console.log(e);
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

    vm.Load = function () {
    };
    vm.Load();
}