app.controller("approverCtrl", approverCtrl);
approverCtrl.$inject = ["$rootScope", "$scope", "$http", "DTOptionsBuilder", "DTColumnBuilder", "ApiFactory", "$timeout"];
function approverCtrl($rootScope, $scope, $http, DTOptionsBuilder, DTColumnBuilder, ApiFactory, $timeout) {


    var vmm = this;
    vmm.factory = new ApiFactory();
    vmm.isSaveOngoing = false;
    vmm.Sites = [];

    vmm.myFilter = {
        Site: ""
    };


    $rootScope.objApproval = {
        ApprovalType: null,
        ApproveRemarks: null,
        ApprovedBy: null,
        Asset: [],
        ControlNo: "",
        CreatedBy: null,
        DateApproved: null,
        DateCreated: null,
        DateUpdated: null,
        FarmOut: {},
        Group: null,
        GroupDescription: null,
        Id: null,
        IsActive: null,
        Method: [],
        Region: null,
        Remarks: null,
        Status: [],
        UpdatedBy: null,
        Vendor: null
    }

    function error(err) {
        vmm.isSearchOngoing = false;
        vmm.isSaveOngoing = false;
        $("#farm-attachment-loader, #asset-attachment-loader").addClass("no-display");
        try {
            new ToastService().error(err.data.Message);
        } catch (e) {
            new ToastService().error("An error occured");
        }
        //btn.removeAttr("disabled");
    }

    vmm.resetFarmForm = function () {
        vmm.isSaveOngoing = false;
        //vmm.Farm = JSON.parse(JSON.stringify(vm.FarmRaw));
        vmm.apply();
        setTimeout(function () {
            $("select").selectpicker("refresh");
        }, 500);
        vmm.getSitesApprover();
    };

    vmm.apply = function () {
        try {
            $scope.$apply();
        } catch (e) {
            //ignore
        }
    };

    //var string = "DISPOSAL-00001";

    //string.split("-")[0]
    vmm.getForUpdate = function (object) {
        var url = "";
        var splitId = object.TransactionControlNo.split("-")[0];
        switch (splitId) {
            case "DISPOSAL":
                url = "disposal/" + object.DisposalId;
                break;
            case "TRANSFER":
                url = "locationtransfer/" + object.LocationTransferId;
                break;
            case "REPLACE":
                url = "replacement/" + object.ReplacementId;
                break;
            case "LOAN":
                url = "loan/" + object.LoanId;
                break;
        }

        vmm.factory.getData(url)
            .then(function (res) {
                $rootScope.objApproval = res.data;
                console.log("DatabyId", $rootScope.objApproval);
            },
                error);
        vmm.apply();
    }

    vmm.saveTransaction = function (id) {
        vmm.isSaveOngoing = true;
        if (id === "APPROVE") {
            $rootScope.objApproval.Status.Id = 3;
            $rootScope.objApproval.Status.ItemName = "Approved";
            $rootScope.objApproval.ApprovedBy = username;
        } else {
            $rootScope.objApproval.Status.Id = 4;
            $rootScope.objApproval.Status.ItemName = "Declined";
            $rootScope.objApproval.ApprovedBy = username;
        }
        vmm.apply();
        var url = "";
        var splitId = $rootScope.objApproval.ControlNo.split("-")[0];
        switch (splitId) {
            case "DISPOSAL":
                url = "disposal/update";
                break;
            case "TRANSFER":
                url = "locationtransfer/update";
                break;
            case "REPLACE":
                url = "replacement/update";
                break;
            case "LOAN":
                url = "loan/update";
                break;
        }

        vmm.factory.putData(url, $rootScope.objApproval)
            .then(function () {
                new ToastService().success("RECORD HAS BEEN UPDATED");
                angular.element(document.querySelector("#ApprovalModal")).modal("hide");
                angular.element("#approver-tab").scope().vmm.searchTransaction();
            },
                function () {
                    error({ data: { Message: "ERROR WHILE UPDATING THE RECORD" } });
                });
    }

    vmm.searchTransaction = function () {
        var siteConcat = '{"site":"' + vmm.myFilter.Site.ItemName + '"}';
        vmm.apply();
        console.log(siteConcat);
        vmm.factory.setData("transaction/search/", siteConcat)
            .then(function (res) {
                console.log("transaction Data", res.data);
                vmm.Table.Transactions = res.data;
                vmm.apply();
                setTimeout(function () {
                    $("select").selectpicker("refresh");
                }, 1000);
            },
                error);
    };



    vmm.getSitesApprover = function () {
        vmm.factory.getData("site-department/site/".concat(globalRegion))
            .then(function (res) {
                vmm.Sites = [];
                vmm.Sites = res.data;

                vmm.apply();
                try {
                    //this happens everytime we load an asset from open records
                    var item = vmm.Sites.filter(function (e) {
                        return e.ItemName === vmm.Asset.Site;
                    });
                    try {
                        if (item.length > 0) {
                            vmm.Asset.Site = item[0];
                        }
                    } catch (e) {
                        vmm.Asset.Site = null;
                    }
                } catch (e) {
                    //ignore
                }
                try {
                    //this happens everytime we load an asset from open records
                    var site = vmm.Sites.filter(function (e) {
                        return e.ItemName === vmm.Farm.Site;
                    });
                    try {
                        if (site.length > 0) {
                            vmm.Farm.Site = site[0];
                        }
                    } catch (e) {
                        vmm.Farm.Site = null;
                    }
                } catch (e) {
                    //ignore
                }
                setTimeout(function () {
                    $("#approver-tabselect").selectpicker("refresh");
                }, 1000);
            },
                error);
    };

    vmm.Load = function () {
        vmm.getSitesApprover();
        setTimeout(function () {
            $(".dataTables_filter").remove();
            $("#approver-tab select").selectpicker("refresh");

        }, 1000);
    };
    vmm.Load();

    vmm.Table = {
        Transactions: [],
        dtInstance: {},
        dtOptions: DTOptionsBuilder
            .newOptions()
            .withPaginationType("full_numbers")
            .withOption("dom", "<'H'Bfr>t<'F'ip>")
            .withOption("buttons", [
                "copyHtml5",
                "csvHtml5",
                "excelHtml5"
            ])
            .withOption("scrollX", true)
            .withOption("lengthMenu", [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]]),
        dtColumnDefs: [
            DTColumnBuilder.newColumn(0),
            DTColumnBuilder.newColumn(1),
            DTColumnBuilder.newColumn(2)
        ]
    };

    vmm.options = {
        aoColumnDefs: [{
            "bSortable": true,
            "aTargets": [0, 1, 2]
        }],
        bJQueryUI: true,
        bDestroy: true,
        aaData: null,
        "paginationType": "full_numbers",
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        dom: '<"H"Bfr>t<"F"ip>',
        buttons: [
            "excelHtml5"
        ],
        scrollX: true
    };
}