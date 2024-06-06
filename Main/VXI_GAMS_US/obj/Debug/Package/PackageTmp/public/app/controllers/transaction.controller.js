app.controller("transactionCtrl", transactionCtrl);
transactionCtrl.$inject = ["$rootScope", "$scope", "$http", "DTOptionsBuilder", "DTColumnBuilder", "ApiFactory", "$timeout"];
function transactionCtrl($rootScope, $scope, $http, DTOptionsBuilder, DTColumnBuilder, ApiFactory, $timeout) {
    var vm = this;

    vm.AssetByRole = vm.AssetByRole || "";

    function error(err) {

        try {
            new ToastService().error(err.data.Message);
        } catch (e) {
            new ToastService().error("An error occured");
        }
    }


    //$scope.transactSave = {

    //    Group: {
    //        Id: '',
    //        ItemId:'',
    //        ItemName:''
    //    },
    //    Site: '',
    //    Status: {
    //        Id: '',
    //        ItemId:'',
    //        ItemName: ''
    //    },
    //    FarmOut: {
    //        Id: '',
    //        ItemId: '',
    //        ItemName:''
    //    },
    //    Floor: '',
    //    Area: '',
    //    Remarks: '',
    //    CreatedBy: '',
    //    CreatedDate:'',
    //    Region: '',
    //    Asset: [
    //        {
    //            Id: '',
    //            Code: '',
    //            Description:''

    //        }]
    //}
    //$scope.disposal = {

    //    Group: {
    //        Id: '',
    //        ItemId: '',
    //        ItemName: ''
    //    },

    //    Status: {
    //        Id: '',
    //        ItemId: '',
    //        ItemName: ''
    //    },
    //    Method: {
    //            Id: '',
    //            ItemId: '',
    //            ItemName: ''
    //    },
    //    FarmOut: {
    //        Id: '',
    //        ItemId: '',
    //        ItemName: ''
    //    },
    //    Vendor: '',
    //    Remarks: '',
    //    CreatedBy: '',
    //    CreatedDate: '',
    //    Region: '',
    //    Asset: []
    //}
    //$scope.replacementSave = {

    //    Group: {
    //        Id: '',
    //        ItemId: '',
    //        ItemName: ''
    //    },
    //    Site: '',
    //    Status: {
    //        Id: '',
    //        ItemId: '',
    //        ItemName: ''
    //    },
    //    FarmIn: {
    //        Id: '',
    //        ItemId: '',
    //        ItemName: ''
    //    },
    //    Floor: '',
    //    Area: '',
    //    Remarks: '',
    //    CreatedBy: '',
    //    CreatedDate: '',
    //    Region: '',
    //    Site: '',
    //    Asset: []
    //}
    //$scope.loanSave = {

    //    Group: {
    //        Id: '',
    //        ItemId: '',
    //        ItemName: ''
    //    },
    //    Site: '',
    //    Status: {
    //        Id: '',
    //        ItemId: '',
    //        ItemName: ''
    //    },
    //    FarmIn: {
    //        Id: '',
    //        ItemId: '',
    //        ItemName: ''
    //    },
    //    Employee: '',
    //    Remarks: '',
    //    CreatedBy: '',
    //    CreatedDate: '',
    //    Region: '',
    //    LoanStartDate: '',
    //    LoanEndDate: '',
    //    CheckedOutBy: '',
    //    CheckedInBy: '',
    //    Asset: []
    //}

    $scope.dtInstance_1 = {};
    $scope.dtOptions_1 = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("asset?group=TEST"))
        .withColumnFilter(
            {
                aoColumns:
                    [
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                    ]
            }
        )
        .withOption("autoWidth", true)
        .withOption("scrollX", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        .withOption("processing", true)
        .withOption("language", { 'processing': 'Loading...' })
        //.withOption("paging", false)
        //.withOption("scrollY", "40vh")
        //.withOption("deferLoading", 10)
        //.withOption("dom", "Bfrtip")
        //.withOption("buttons", ["excelHtml5"])
        .withOption("bSort", false);
    function actionsHtml(data) {
        return "<button class=\"btn btn-warning\" onclick=\"angular.element('#transaction-tab').scope().".concat("getAssetById", "('", data.Id, "')\">EDIT</button>");
    }

    function lookupAction(data) {
        data = data || {};
        return data.ItemName || "";
    }
    $scope.dtColumns_1 = [
        DTColumnBuilder.newColumn(null).withTitle("OPTION").withClass("td-option").renderWith(actionsHtml),
        DTColumnBuilder.newColumn("ControlNo").withTitle("CONTROL NO").withClass("td-small"),
        DTColumnBuilder.newColumn("Description").withTitle("DESCRIPTION").withClass("td-small1"),
        DTColumnBuilder.newColumn("SerialNo").withTitle("SERIAL NO.").withClass("td-small"),
        DTColumnBuilder.newColumn("GrNo").withTitle("GR NO.").withClass("td-small"),
        DTColumnBuilder.newColumn("Category").withTitle("CATEGORY").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("SubCategory").withTitle("SUBCATEGORY").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("Site").withTitle("SITE").withClass("td-small"),
        DTColumnBuilder.newColumn("LobOwner").withTitle("LOB").withClass("td-small"),
    ];
    $scope.Table = {
        dtOptions: DTOptionsBuilder
            .newOptions()
            .withPaginationType("full_numbers")
            .withColumnFilter(
                {
                    aoColumns:
                        [
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                        ]
                }
            )
            .withOption("dom", "<'H'Bfr>t<'F'ip>")
            .withOption("buttons", []),
        dtColumnDefs: [
            DTColumnBuilder.newColumn(0),
            DTColumnBuilder.newColumn(1),
            DTColumnBuilder.newColumn(2)
        ]
    };
    function actionHtml2(data) {
        var final = "";
        if (data.Status.ItemName === "New" || data.Status.ItemName === "Pending") {
            final = "<button class=\"btn btn-warning\" onclick=\"angular.element('#transaction-tab').scope().".concat("getLocTrasnferById", "('", data.Id, "')\">EDIT</button>");
        }
        return final;
    }
    $scope.dtInstance_2 = {};
    $scope.dtOptions_2 = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("locationtransfer"))
        .withColumnFilter(
            {
                aoColumns:
                    [
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                    ]
            }
        )
        .withOption("autoWidth", true)
        .withOption("scrollX", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        .withOption("bSort", false);
    $scope.dtColumns_2 = [
        DTColumnBuilder.newColumn(null).withTitle("OPTION").withClass("td-option").renderWith(actionHtml2),
        DTColumnBuilder.newColumn("ControlNo").withTitle("CONTROL NO").withClass("td-small"),
        DTColumnBuilder.newColumn("Status").withTitle("STATUS").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("FarmOut").withTitle("FARM OUT").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("Site").withTitle("SITE").withClass("td-small"),
        DTColumnBuilder.newColumn("Floor").withTitle("FLOOR").withClass("td-small"),
        DTColumnBuilder.newColumn("Group").withTitle("GROUP").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("DateCreated").withTitle("DATE CREATED").withClass("td-small"),
        DTColumnBuilder.newColumn("CreatedBy").withTitle("CREATED BY").withClass("td-small"),
    ];
    function actionsHtml3(data) {
        return "<button class=\"btn btn-warning\" onclick=\"angular.element('#transaction-tab').scope().".concat("getAssetByIdDisposal", "('", data.Id, "')\">EDIT</button>");
    }
    $scope.dtInstance_3 = {};
    $scope.dtOptions_3 = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("asset?group=TEST"))
        .withColumnFilter(
            {
                aoColumns:
                    [
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                    ]
            }
        )
        .withOption("autoWidth", true)
        .withOption("scrollX", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        .withOption("processing", true)
        .withOption("language", { 'processing': 'Loading...' })
        .withOption("bSort", false);
    $scope.dtColumns_3 = [
        DTColumnBuilder.newColumn(null).withTitle("OPTION").withClass("td-option").renderWith(actionsHtml3),
        DTColumnBuilder.newColumn("ControlNo").withTitle("CONTROL NO").withClass("td-small"),
        DTColumnBuilder.newColumn("Description").withTitle("DESCRIPTION").withClass("td-small1"),
        DTColumnBuilder.newColumn("SerialNo").withTitle("SERIAL NO.").withClass("td-small"),
        DTColumnBuilder.newColumn("GrNo").withTitle("GR NO.").withClass("td-small"),
        DTColumnBuilder.newColumn("Category").withTitle("CATEGORY").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("SubCategory").withTitle("SUBCATEGORY").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("Site").withTitle("SITE").withClass("td-small"),
        DTColumnBuilder.newColumn("LobOwner").withTitle("LOB").withClass("td-small"),
    ];

    function actionHtml4(data) {
        var final = "";
        if (data.Status.ItemName === "New") {
            final = "<button class=\"btn btn-warning\" onclick=\"angular.element('#transaction-tab').scope().".concat("getDisposalById", "('", data.Id, "')\">EDIT</button>");
        }
        return final;
    }
    $scope.dtInstance_4 = {};
    $scope.dtOptions_4 = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("disposal"))
        .withColumnFilter(
            {
                aoColumns:
                    [
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                    ]
            }
        )
        .withOption("autoWidth", true)
        .withOption("scrollX", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        .withOption("bSort", false);
    $scope.dtColumns_4 = [
        DTColumnBuilder.newColumn(null).withTitle("OPTION").withClass("td-option").renderWith(actionHtml4),
        DTColumnBuilder.newColumn("ControlNo").withTitle("CONTROL NO").withClass("td-small"),
        DTColumnBuilder.newColumn("Status").withTitle("STATUS").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("FarmOut").withTitle("FARM OUT").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("Method").withTitle("METHOD").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("Group").withTitle("GROUP").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("Vendor").withTitle("VENDOR").withClass("td-small"),
        DTColumnBuilder.newColumn("DateCreated").withTitle("DATE CREATED").withClass("td-small"),
        DTColumnBuilder.newColumn("CreatedBy").withTitle("CREATED BY").withClass("td-small"),
    ];

    function actionsHtml5(data) {
        return "<button class=\"btn btn-warning\" onclick=\"angular.element('#transaction-tab').scope().".concat("getAssetByIdReplacement", "('", data.Id, "')\">EDIT</button>");
    }
    $scope.dtInstance_5 = {};
    $scope.dtOptions_5 = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("asset?group=TEST"))
        .withColumnFilter(
            {
                aoColumns:
                    [
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                    ]
            }
        )
        .withOption("autoWidth", true)
        .withOption("scrollX", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        .withOption("processing", true)
        .withOption("language", { 'processing': 'Loading...' })
        .withOption("bSort", false);
    $scope.dtColumns_5 = [
        DTColumnBuilder.newColumn(null).withTitle("OPTION").withClass("td-option").renderWith(actionsHtml5),
        DTColumnBuilder.newColumn("ControlNo").withTitle("CONTROL NO").withClass("td-small"),
        DTColumnBuilder.newColumn("Description").withTitle("DESCRIPTION").withClass("td-small1"),
        DTColumnBuilder.newColumn("SerialNo").withTitle("SERIAL NO.").withClass("td-small"),
        DTColumnBuilder.newColumn("GrNo").withTitle("GR NO.").withClass("td-small"),
        DTColumnBuilder.newColumn("Category").withTitle("CATEGORY").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("SubCategory").withTitle("SUBCATEGORY").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("Site").withTitle("SITE").withClass("td-small"),
        DTColumnBuilder.newColumn("LobOwner").withTitle("LOB").withClass("td-small"),
    ];

    function actionHtml6(data) {
        var final = "";
        if (data.Status.ItemName === "New") {
            final = "<button class=\"btn btn-warning\" onclick=\"angular.element('#transaction-tab').scope().".concat("getReplacementById", "('", data.Id, "')\">EDIT</button>");
        }
        return final;
    }
    $scope.dtInstance_6 = {};
    $scope.dtOptions_6 = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("replacement"))
        .withColumnFilter(
            {
                aoColumns:
                    [
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                    ]
            }
        )
        .withOption("autoWidth", true)
        .withOption("scrollX", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        .withOption("bSort", false);
    $scope.dtColumns_6 = [
        DTColumnBuilder.newColumn(null).withTitle("OPTION").withClass("td-option").renderWith(actionHtml6),
        DTColumnBuilder.newColumn("ControlNo").withTitle("CONTROL NO").withClass("td-small"),
        DTColumnBuilder.newColumn("Status").withTitle("STATUS").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("FarmIn").withTitle("FARM OUT").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("Group").withTitle("GROUP").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("DateCreated").withTitle("DATE CREATED").withClass("td-small"),
        DTColumnBuilder.newColumn("CreatedBy").withTitle("CREATED BY").withClass("td-small"),
    ];


    function actionsHtml7(data) {
        return "<button class=\"btn btn-warning\" onclick=\"angular.element('#transaction-tab').scope().".concat("getAssetByIdLoan", "('", data.Id, "')\">EDIT</button>");
    }
    $scope.dtInstance_7 = {};
    $scope.dtOptions_7 = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("asset?group=TEST"))
        .withColumnFilter(
            {
                aoColumns:
                    [
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                    ]
            }
        )
        .withOption("autoWidth", true)
        .withOption("scrollX", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        .withOption("processing", true)
        .withOption("language", { 'processing': 'Loading...' })
        .withOption("bSort", false);
    $scope.dtColumns_7 = [
        DTColumnBuilder.newColumn(null).withTitle("OPTION").withClass("td-option").renderWith(actionsHtml7),
        DTColumnBuilder.newColumn("ControlNo").withTitle("CONTROL NO").withClass("td-small"),
        DTColumnBuilder.newColumn("Description").withTitle("DESCRIPTION").withClass("td-small1"),
        DTColumnBuilder.newColumn("SerialNo").withTitle("SERIAL NO.").withClass("td-small"),
        DTColumnBuilder.newColumn("GrNo").withTitle("GR NO.").withClass("td-small"),
        DTColumnBuilder.newColumn("Category").withTitle("CATEGORY").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("SubCategory").withTitle("SUBCATEGORY").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("Site").withTitle("SITE").withClass("td-small"),
        DTColumnBuilder.newColumn("LobOwner").withTitle("LOB").withClass("td-small"),
    ];

    function actionHtml8(data) {
        var final = "";
        if (data.Status.ItemName === "New") {
            final = "<button class=\"btn btn-warning\" onclick=\"angular.element('#transaction-tab').scope().".concat("getLoanById", "('", data.Id, "')\">EDIT</button>");
        }
        return final;
    }
    $scope.dtInstance_8 = {};
    $scope.dtOptions_8 = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("loan"))
        .withColumnFilter(
            {
                aoColumns:
                    [
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                    ]
            }
        )
        .withOption("autoWidth", true)
        .withOption("scrollX", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        .withOption("bSort", false);
    $scope.dtColumns_8 = [
        DTColumnBuilder.newColumn(null).withTitle("OPTION").withClass("td-option").renderWith(actionHtml8),
        DTColumnBuilder.newColumn("ControlNo").withTitle("CONTROL NO").withClass("td-small"),
        DTColumnBuilder.newColumn("Status").withTitle("STATUS").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("FarmIn").withTitle("FARM OUT").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("Group").withTitle("GROUP").withClass("td-small").renderWith(lookupAction),
        DTColumnBuilder.newColumn("DateCreated").withTitle("DATE CREATED").withClass("td-small"),
        DTColumnBuilder.newColumn("CreatedBy").withTitle("CREATED BY").withClass("td-small"),
    ];




















    $scope.Table = {
        dtOptions: DTOptionsBuilder
            .newOptions()
            .withPaginationType("full_numbers")
            .withColumnFilter(
                {
                    aoColumns:
                        [
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                            {
                                type: "text"
                            },
                        ]
                }
            )
            .withOption("dom", "<'H'Bfr>t<'F'ip>")
            .withOption("buttons", []),
        dtColumnDefs: [
            DTColumnBuilder.newColumn(0),
            DTColumnBuilder.newColumn(1),
            DTColumnBuilder.newColumn(2)
        ]
    };
    $scope.dtOptions = DTOptionsBuilder
        .newOptions()
        .withPaginationType("full_numbers")
        .withColumnFilter(
            {
                aoColumns:
                    [
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                        {
                            type: "text"
                        },
                    ]
            }
        )
        .withOption("autoWidth", true)
        .withOption("scrollX", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        .withOption("bSort", false);

    vm.apply = function () {
        try {
            $scope.$apply();
        } catch (e) {
            //ignore
        }
    };



    $scope.TransactSites = [];
    vm.factory = new ApiFactory();
    $scope.getSites = function () {
        vm.factory.getData("site-department/site/".concat(globalRegion))
            .then(function (data) {
                $scope.TransactSites = [];
                $scope.TransactSites = data.data;

                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 1000);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                error);
    };
    $scope.getGroups = function () {
        vm.Groups = [];
        vm.apply();
        vm.factory.getData("group")
            .then(function (res) {
                $scope.transactGroups = [];
                $scope.transactGroups = res.data;
                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 1000);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                error);
    };
    $scope.getData = function () {
        vm.factory.getData("locationtransfer")
            .then(function (data) {

                $scope.locTransferRequest = [];
                $scope.locTransferRequest = data.data;
                console.log($scope.locTransferRequest);
                vm.apply();
            },
                function () {
                    new ToastService().error("ERROR WHILE GETTING THE RECORD");
                });
    };
    $scope.getDataDisposal = function () {
        vm.factory.getData("disposal")
            .then(function (data) {

                $scope.disposalRequest = [];
                $scope.disposalRequest = data.data;
                console.log($scope.disposalRequest);
                vm.apply();
            },
                function () {
                    new ToastService().error("ERROR WHILE GETTING THE RECORD");
                });
    };
    $scope.getDataReplacement = function () {
        vm.factory.getData("replacement")
            .then(function (data) {

                $scope.replacementRequest = [];
                $scope.replacementRequest = data.data;
                console.log($scope.replacementRequest);
                vm.apply();
            },
                function () {
                    new ToastService().error("ERROR WHILE GETTING THE RECORD");
                });
    };
    $scope.getDataLoan = function () {
        vm.factory.getData("loan")
            .then(function (data) {

                $scope.loanRequest = [];
                $scope.loanRequest = data.data;
                console.log($scope.loanRequest);
                vm.apply();
            },
                function () {
                    new ToastService().error("ERROR WHILE GETTING THE RECORD");
                });
    };


    $scope.transact = [];
    $scope.getLocTrasnferById = function (id) {
        $("#locTransferModal").modal("hide");
        vm.factory.getData("locationtransfer/".concat(id))
            .then(function (data) {

                $scope.transact = [];
                $scope.transact = data.data;
                //$scope.transact.Site = $scope.transact.Site || "";
                //var site = $scope.TransactSites.filter(function (x) {
                //    return x.ItemName === $scope.transact.Site;
                //});
                //if (site.length > 0) {
                //    try {
                //        $scope.transact.Site = site[0];
                //    } catch (e) {
                //        $scope.transact.Site = null;
                //    }
                //}
                console.log($scope.transact);
                $rootScope.AssetDetails = [];
                $rootScope.AssetDetails = $scope.transact.Asset;
                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 1000);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();

            },
                function () {
                    new ToastService().error("ERROR WHILE GETTING THE RECORD");
                });
    };

    $scope.disposal = {};
    $scope.getDisposalById = function (id) {
        $("#disposalModal").modal("hide");
        vm.factory.getData("disposal/".concat(id))
            .then(function (data) {
                $scope.disposal = {};
                $scope.disposal = data.data;
                console.log($scope.disposal);
                $scope.AssetDetailsDisposal = [];
                $scope.AssetDetailsDisposal = $scope.disposal.Asset;
                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 1000);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                function () {
                    new ToastService().error("ERROR WHILE GETTING THE RECORD");
                });
    };

    $scope.replacement = [];
    $scope.getReplacementById = function (id) {
        $("#replacementModal").modal("hide");
        vm.factory.getData("replacement/".concat(id))
            .then(function (data) {

                $scope.replacement = [];
                $scope.replacement = data.data;
                console.log($scope.replacement);
                $rootScope.AssetDetailsReplacement = [];
                $rootScope.AssetDetailsReplacement = $scope.replacement.Asset;
                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 1000);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                function () {
                    new ToastService().error("ERROR WHILE GETTING THE RECORD");
                });
    };

    $scope.loan = {};
    $scope.getLoanById = function (id) {
        $("#loanModal").modal("hide");
        vm.factory.getData("loan/".concat(id))
            .then(function (data) {
                $scope.loan = {};
                $scope.loan = data.data;
                $scope.loan.LoanEndDate = moment($scope.loan.LoanEndDate, "M/D/YYYY").toDate();
                $scope.loan.LoanStartDate = moment($scope.loan.LoanStartDate, "M/D/YYYY").toDate();
                $rootScope.AssetDetailsLoan = [];
                $rootScope.AssetDetailsLoan = $scope.loan.Asset;
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 1000);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
                vm.apply();
            },
                function () {
                    new ToastService().error("ERROR WHILE GETTING THE RECORD");
                });
    };

    $scope.getAssets = function () {
        $scope.transactAssets = [];
        vm.apply();
        vm.factory.getData("asset?group=".concat(vm.AssetByRole))
            .then(function (data) {
                $scope.transactAssets = [];
                $scope.transactAssets = data.data;
                vm.apply();
            },
                error);
    };

    $rootScope.AssetDetails = [];
    $scope.getAssetById = function (obj) {
        $("#transactAssetModal").modal("hide");
        var id = parseInt(obj);
        if (isNaN(id)) {
            id = 0;
        }
        vm.factory.getData("asset/".concat(id))
            .then(function (data) {
                $scope.asset = data.data;
                $rootScope.AssetDetails.push($scope.asset);
                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 800);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                error);
    };

    $scope.AssetDetailsDisposal = [];
    $scope.getAssetByIdDisposal = function (obj) {
        $("#disposalAssetModal").modal("hide");
        var id = parseInt(obj);
        if (isNaN(id)) {
            id = 0;
        }
        vm.factory.getData("asset/".concat(id))
            .then(function (data) {
                $scope.asset = data.data;
                $scope.AssetDetailsDisposal.push($scope.asset);

                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 800);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                error);
    };

    $rootScope.AssetDetailsReplacement = [];
    $scope.getAssetByIdReplacement = function (obj) {
        $("#replacementAssetModal").modal("hide");
        var id = parseInt(obj);
        if (isNaN(id)) {
            id = 0;
        }
        vm.factory.getData("asset/".concat(id))
            .then(function (data) {
                $scope.asset = data.data;
                $rootScope.AssetDetailsReplacement.push($scope.asset);

                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 800);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                error);
    };

    $rootScope.AssetDetailsLoan = [];
    $scope.getAssetByIdLoan = function (obj) {
        $("#loanAssetModal").modal("hide");
        var id = parseInt(obj);
        if (isNaN(id)) {
            id = 0;
        }
        vm.factory.getData("asset/".concat(id))
            .then(function (data) {
                $scope.asset = data.data;
                $rootScope.AssetDetailsLoan.push($scope.asset);

                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 800);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                error);
    };


    $scope.getFarmInStatus = function () {
        vm.factory.getData("farmstatus")
            .then(function (data) {
                $scope.FarmStatus = [];
                $scope.FarmStatus = data.data;
                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 800);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                error);
    };
    $scope.getFarmControlNo = function () {
        vm.factory.getData("farmout/lookup")
            .then(function (data) {
                $scope.FarmNo = [];
                $scope.FarmNo = data.data;
                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 800);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                error);
    };
    $scope.getDisposalStatus = function () {
        vm.factory.getData("disposalstatus")
            .then(function (data) {
                $scope.disStatus = [];
                $scope.disStatus = data.data;
                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 800);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                error);
    };
    $scope.getDisposalMethod = function () {
        vm.factory.getData("disposalmethod")
            .then(function (data) {
                $scope.disMethod = [];
                $scope.disMethod = data.data;
                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 800);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                error);
    };
    $scope.getLoanStatus = function () {
        vm.factory.getData("loanstatus")
            .then(function (data) {
                $scope.loanStatus = [];
                $scope.loanStatus = data.data;
                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 800);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                error);
    };
    $scope.getRepStatus = function () {
        vm.factory.getData("replacementstatus")
            .then(function (data) {
                $scope.repStatus = [];
                $scope.repStatus = data.data;
                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 800);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                error);
    };

    $scope.getFarmInControlNo = function () {
        vm.factory.getData("farmin/lookup")
            .then(function (data) {
                $scope.FarmInNo = [];
                $scope.FarmInNo = data.data;
                vm.apply();
                //setTimeout(function () {
                //    $("#transaction-tab select").selectpicker("refresh");
                //}, 800);
                (function () {
                    function codeBlock() {
                        $("#transaction-tab select").selectpicker("refresh");
                    }
                    codeBlock();
                })();
            },
                error);
    };

    //$scope.transact = {};
    //$scope.disposal = {};
    //$scope.replacement = {};
    //$scope.loan = {};

    $scope.saveLocTrasfer = function () {
        try {
            $scope.transact = $scope.transact || {};
            $scope.transact.FarmOut = $scope.transact.FarmOut || [];
            $scope.transact.Group = $scope.transact.Group || {};
            if ($scope.transact.Group.Id === undefined) {
                throw "Select a Group";
            }
            if ($scope.transact.FarmOut.length < 1) {
                throw "Select a Farm Control No";
            }
            $scope.transact.Site = $scope.transact.Site || "";
            if ($scope.transact.Site.length < 1) {
                throw "Select a Site";
            }
            $rootScope.AssetDetails = $rootScope.AssetDetails || [];
            if ($rootScope.AssetDetails.length < 1) {
                throw "Select atleast one Asset for this transaction";
            }
            if ($scope.transact.Id > 0) {
                $scope.updateLocTrasfer();
            } else {
                var disData = $scope.transact;
                $scope.transact = {};
                $scope.transact = {
                    Group: disData.Group,
                    Status: disData.Status,
                    Method: disData.Method,
                    FarmOut: disData.FarmOut,
                    Vendor: disData.Vendor,
                    Remarks: disData.Remarks,
                    CreatedBy: username,
                    Region: globalRegion,
                    Site: disData.Site,
                    Asset: $rootScope.AssetDetails,
                    Floor: disData.Floor,
                    Area: disData.Area

                }
                vm.apply();
                vm.factory.setData("locationtransfer/save", $scope.transact)
                    .then(function () {
                        new ToastService().success("RECORD HAS BEEN ADDED");
                        setTimeout(function () {
                            $rootScope.AssetDetails = [];
                            $scope.transact = {};
                            vm.apply();
                            //setTimeout(function () {
                            //    $("#transaction-tab select").selectpicker("refresh");
                            //}, 800);
                            (function () {
                                function codeBlock() {
                                    $("#transaction-tab select").selectpicker("refresh");
                                }
                                codeBlock();
                            })();
                            //$scope.getData();
                            $scope.dtInstance_2.DataTable.ajax.reload();
                        },
                            1000);
                    },
                        function () {
                            new ToastService().error("ERROR WHILE SAVING THE RECORD");
                        });
            }
        } catch (e) {
            new ToastService().error(e);
        }

    };
    $scope.saveDisposal = function () {
        try {
            $scope.disposal = $scope.disposal || {};
            $scope.disposal.FarmOut = $scope.disposal.FarmOut || [];
            $scope.disposal.Group = $scope.disposal.Group || {};
            if ($scope.disposal.Group.Id === undefined) {
                throw "Select a Group";
            }
            if ($scope.disposal.FarmOut.length < 1) {
                throw "Select a Farm Control No";
            }
            $scope.AssetDetailsDisposal = $scope.AssetDetailsDisposal || [];
            if ($scope.AssetDetailsDisposal.length < 1) {
                throw "Select atleast one Asset for this transaction";
            }
            if ($scope.disposal.Id > 0) {
                $scope.updateDisposal();
            } else {

                var disData = $scope.disposal;
                $scope.disposal.Asset = [];
                $scope.disposal = {

                    Group: disData.Group,
                    Status: disData.Status,
                    Method: disData.Method,
                    FarmOut: disData.FarmOut,
                    Vendor: disData.Vendor,
                    Remarks: disData.Remarks,
                    CreatedBy: username,
                    Region: globalRegion,
                    Asset: $scope.AssetDetailsDisposal,


                };
                vm.apply();
                vm.factory.setData("disposal/save", $scope.disposal)
                    .then(function () {
                        new ToastService().success("RECORD HAS BEEN ADDED");
                        setTimeout(function () {
                            $scope.AssetDetailsDisposal = [];
                            $scope.disposal = {};
                            vm.apply();
                            //setTimeout(function () {
                            //    $("#transaction-tab select").selectpicker("refresh");
                            //}, 800);
                            (function () {
                                function codeBlock() {
                                    $("#transaction-tab select").selectpicker("refresh");
                                }
                                codeBlock();
                            })();
                            //$scope.getDataDisposal();
                            $scope.dtInstance_4.DataTable.ajax.reload();
                        },
                            1000);
                    },
                        function () {
                            new ToastService().error("ERROR WHILE SAVING THE RECORD");
                        });
            }
        } catch (e) {
            new ToastService().error(e);
        }
    };
    $scope.saveReplacement = function () {
        try {
            $scope.replacement = $scope.replacement || {};
            $scope.replacement.FarmIn = $scope.replacement.FarmIn || [];
            $scope.replacement.Group = $scope.replacement.Group || {};
            if ($scope.replacement.Group.Id === undefined) {
                throw "Select a Group";
            }
            if ($scope.replacement.FarmIn.length < 1) {
                throw "Select a Farm Control No";
            }
            $rootScope.AssetDetailsReplacement = $rootScope.AssetDetailsReplacement || [];
            if ($rootScope.AssetDetailsReplacement.length < 1) {
                throw "Select atleast one Asset for this transaction";
            }
            if ($scope.replacement.Id > 0) {
                $scope.updateReplacement();
            } else {

                var disData = $scope.replacement;
                $scope.replacement = {};
                $scope.replacement = {
                    Group: disData.Group,
                    Status: disData.Status,
                    Method: disData.Method,
                    FarmIn: disData.FarmIn,
                    Vendor: disData.Vendor,
                    Remarks: disData.Remarks,
                    CreatedBy: username,
                    Region: globalRegion,
                    Site: disData.Site,
                    Asset: $rootScope.AssetDetailsReplacement,
                    Floor: disData.Floor,
                    Area: disData.Area
                }
                vm.apply();
                vm.factory.setData("replacement/save", $scope.replacement)
                    .then(function () {
                        new ToastService().success("RECORD HAS BEEN ADDED");
                        setTimeout(function () {
                            $rootScope.AssetDetailsReplacement = [];
                            $scope.replacement = {};
                            vm.apply();
                            //setTimeout(function () {
                            //    $("#transaction-tab select").selectpicker("refresh");
                            //}, 800);
                            (function () {
                                function codeBlock() {
                                    $("#transaction-tab select").selectpicker("refresh");
                                }
                                codeBlock();
                            })();
                            //$scope.getDataReplacement();
                            $scope.dtInstance_6.DataTable.ajax.reload();
                        },
                            1000);
                    },
                        function () {
                            new ToastService().error("ERROR WHILE SAVING THE RECORD");
                        });
            }
        } catch (e) {
            new ToastService().error(e);
        }
    };
    $scope.saveLoan = function () {
        try {
            $scope.loan = $scope.loan || {};
            $scope.loan.FarmIn = $scope.loan.FarmIn || [];
            $scope.loan.Group = $scope.loan.Group || {};
            if ($scope.loan.Group.Id === undefined) {
                throw "Select a Group";
            }
            if ($scope.loan.FarmIn.length < 1) {
                throw "Select a Farm Control No";
            }
            $rootScope.AssetDetailsLoan = $rootScope.AssetDetailsLoan || [];
            if ($rootScope.AssetDetailsLoan.length < 1) {
                throw "Select atleast one Asset for this transaction";
            }
            if ($scope.loan.Id > 0) {
                $scope.updateLoan();
            } else {
                var disData = $scope.loan;
                $scope.loan = {};
                $scope.loan = {
                    Group: disData.Group,
                    Status: disData.Status,
                    Method: disData.Method,
                    FarmIn: disData.FarmIn,
                    Vendor: disData.Vendor,
                    Remarks: disData.Remarks,
                    CreatedBy: username,
                    Employee: disData.Employee,
                    Region: globalRegion,
                    Site: disData.Site,
                    Asset: $rootScope.AssetDetailsLoan,
                    Floor: disData.Floor,
                    Area: disData.Area,
                    LoanStartDate: disData.LoanStartDate,
                    LoanEndDate: disData.LoanEndDate,
                    CheckedOutBy: disData.CheckedOutBy,
                    CheckedInBy: disData.CheckedInBy
                }
                vm.apply();
                vm.factory.setData("loan/save", $scope.loan)
                    .then(function () {
                        new ToastService().success("RECORD HAS BEEN ADDED");
                        setTimeout(function () {
                            $rootScope.AssetDetailsLoan = [];
                            $scope.loan = {};
                            vm.apply();
                            //setTimeout(function () {
                            //    $("#transaction-tab select").selectpicker("refresh");
                            //}, 800);
                            (function () {
                                function codeBlock() {
                                    $("#transaction-tab select").selectpicker("refresh");
                                }
                                codeBlock();
                            })();
                            //$scope.getDataLoan();
                            $scope.dtInstance_8.DataTable.ajax.reload();

                        },
                            1000);
                    },
                        function () {
                            new ToastService().error("ERROR WHILE SAVING THE RECORD");
                        });
            }
        } catch (e) {
            new ToastService().error(e);
        }
    };

    $scope.updateLocTrasfer = function () {
        $scope.transact.UpdatedBy = username;
        $scope.transact.CreatedBy = username;
        $scope.transact.Region = globalRegion;
        //$scope.transact.Asset = [];
        //$scope.transact.Asset = $rootScope.AssetDetails;

        vm.apply();

        vm.factory.putData("locationtransfer/update", $scope.transact)
            .then(function () {
                new ToastService().success("RECORD HAS BEEN UPDATED");
                setTimeout(function () {
                    $rootScope.AssetDetails = [];
                    $scope.transact = {};
                    vm.apply();
                    //setTimeout(function () {
                    //    $("#transaction-tab select").selectpicker("refresh");
                    //}, 800);
                    (function () {
                        function codeBlock() {
                            $("#transaction-tab select").selectpicker("refresh");
                        }
                        codeBlock();
                    })();
                    //$scope.getData();
                    $scope.dtInstance_2.DataTable.ajax.reload();
                },
                    1000);
            },
                function () {
                    new ToastService().error("ERROR WHILE UPDATING THE RECORD");
                });
    };
    $scope.updateDisposal = function () {
        $scope.disposal.UpdatedBy = username;
        $scope.disposal.CreatedBy = username;
        $scope.disposal.Region = globalRegion;
        //$scope.disposal.Asset = [];
        //$scope.disposal.Asset = $rootScope.AssetDetails;
        vm.apply();

        vm.factory.putData("disposal/update", $scope.disposal)
            .then(function () {
                new ToastService().success("RECORD HAS BEEN UPDATED");
                setTimeout(function () {
                    $scope.AssetDetailsDisposal = [];
                    $scope.disposal = {};
                    vm.apply();
                    //setTimeout(function () {
                    //    $("#transaction-tab select").selectpicker("refresh");
                    //}, 800);
                    (function () {
                        function codeBlock() {
                            $("#transaction-tab select").selectpicker("refresh");
                        }
                        codeBlock();
                    })();
                    //$scope.getDataDisposal();
                    $scope.dtInstance_4.DataTable.ajax.reload();
                },
                    1000);
            },
                function () {
                    new ToastService().error("ERROR WHILE UPDATING THE RECORD");
                });
    };
    $scope.updateReplacement = function () {
        $scope.replacement.UpdatedBy = username;
        $scope.replacement.CreatedBy = username;
        $scope.replacement.Region = globalRegion;
        //$scope.replacement.Asset = [];
        //$scope.replacement.Asset = $rootScope.AssetDetails;
        vm.apply();

        vm.factory.putData("replacement/update", $scope.replacement)
            .then(function () {
                new ToastService().success("RECORD HAS BEEN UPDATED");
                setTimeout(function () {
                    $rootScope.AssetDetailsReplacement = [];
                    $scope.replacement = {};
                    vm.apply();
                    //setTimeout(function () {
                    //    $("#transaction-tab select").selectpicker("refresh");
                    //}, 800);
                    (function () {
                        function codeBlock() {
                            $("#transaction-tab select").selectpicker("refresh");
                        }
                        codeBlock();
                    })();
                    //$scope.getDataReplacement();
                    $scope.dtInstance_6.DataTable.ajax.reload();
                },
                    1000);
            },
                function () {
                    new ToastService().error("ERROR WHILE UPDATING  THE RECORD");
                });
    };
    $scope.updateLoan = function () {
        $scope.loan.UpdatedBy = username;
        $scope.loan.CreatedBy = username;
        $scope.loan.Region = globalRegion;
        //$scope.loan.Asset = [];
        //$scope.loan.Asset = $rootScope.AssetDetails;
        vm.apply();

        vm.factory.putData("loan/update", $scope.loan)
            .then(function () {
                new ToastService().success("RECORD HAS BEEN UPDATED");
                setTimeout(function () {
                    $rootScope.AssetDetailsLoan = [];
                    $scope.loan = {};
                    vm.apply();
                    //setTimeout(function () {
                    //    $("#transaction-tab select").selectpicker("refresh");
                    //}, 800);
                    (function () {
                        function codeBlock() {
                            $("#transaction-tab select").selectpicker("refresh");
                        }
                        codeBlock();
                    })();
                    //$scope.getDataLoan();
                    $scope.dtInstance_8.DataTable.ajax.reload();
                },
                    1000);
            },
                function () {
                    new ToastService().error("ERROR WHILE UPDATING THE RECORD");
                });
    };

    $scope.useSearchedEmployee2 = function (obj) {
        $scope.loan.Employee = JSON.parse(JSON.stringify(obj));
        vm.apply();
        $("#employeeModalForLoan").modal("hide");
    };


    $scope.openModal = function (id) {
        id = id || "";
        var url = "";
        if (id === "#transactAssetModal") {
            url = globalApiBaseUrl.concat("asset?group=".concat(vm.AssetByRole));
            $scope.dtInstance_1.DataTable.clear().draw();
            $scope.dtInstance_1.DataTable.ajax.url(url);
            $scope.dtInstance_1.DataTable.ajax.reload();
        } else if (id === "#disposalAssetModal") {
            url = globalApiBaseUrl.concat("asset?group=".concat(vm.AssetByRole));
            $scope.dtInstance_3.DataTable.clear().draw();
            $scope.dtInstance_3.DataTable.ajax.url(url);
            $scope.dtInstance_3.DataTable.ajax.reload();
        } else if (id === "#replacementAssetModal") {
            url = globalApiBaseUrl.concat("asset?group=".concat(vm.AssetByRole));
            $scope.dtInstance_5.DataTable.clear().draw();
            $scope.dtInstance_5.DataTable.ajax.url(url);
            $scope.dtInstance_5.DataTable.ajax.reload();
        } else if (id === "#loanAssetModal") {
            url = globalApiBaseUrl.concat("asset?group=".concat(vm.AssetByRole));
            $scope.dtInstance_7.DataTable.clear().draw();
            $scope.dtInstance_7.DataTable.ajax.url(url);
            $scope.dtInstance_7.DataTable.ajax.reload();
        }
        $(id).modal("show");
    };

    $scope.$watch("transact.Group", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //setTimeout(function () {
        //    $("#transaction-tab select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                $("#transaction-tab select").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("transact.Status", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        console.log(newValue);
        //setTimeout(function () {
        //    $("#transaction-tab select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                $("#transaction-tab select").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("transact.Site", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //setTimeout(function () {
        //    $("#transaction-tab select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                $("#transaction-tab select").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("disposal.Group", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //setTimeout(function () {
        //    $("#transaction-tab select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                $("#transaction-tab select").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("disposal.Status", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //setTimeout(function () {
        //    $("#transaction-tab select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                $("#transaction-tab select").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("disposal.FarmOut", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //setTimeout(function () {
        //    $("#transaction-tab select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                $("#transaction-tab select").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("disposal.Method", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //setTimeout(function () {
        //    $("#transaction-tab select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                $("#transaction-tab select").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("replacement.Group", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //setTimeout(function () {
        //    $("#transaction-tab select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                $("#transaction-tab select").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("replacement.FarmIn", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //setTimeout(function () {
        //    $("#transaction-tab select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                $("#transaction-tab select").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("replacement.Status", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //setTimeout(function () {
        //    $("#transaction-tab select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                $("#transaction-tab select").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("loan.Group", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //setTimeout(function () {
        //    $("#transaction-tab select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                $("#transaction-tab select").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("loan.FarmIn", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //setTimeout(function () {
        //    $("#transaction-tab select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                $("#transaction-tab select").selectpicker("refresh");
            }
            codeBlock();
        })();
    });
    $scope.$watch("loan.Status", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //setTimeout(function () {
        //    $("#transaction-tab select").selectpicker("refresh");
        //}, 500);
        (function () {
            function codeBlock() {
                $("#transaction-tab select").selectpicker("refresh");
            }
            codeBlock();
        })();
    });

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
        if (it.length > 0 && reaf.length > 0) {
            vm.AssetByRole = "";
        }
        //$scope.getDataLoan();
        //$scope.getDataReplacement();
        //$scope.getDataDisposal();
        $scope.getDisposalStatus();
        $scope.getDisposalMethod();
        $scope.getLoanStatus();
        $scope.getRepStatus();
        $scope.getSites();
        $scope.getGroups();
        //$scope.getData();
        //$scope.getAssets();
        $scope.getFarmInStatus();
        $scope.getFarmControlNo();
        $scope.getFarmInControlNo();
    };
    vm.Load();

    $scope.resetLocationTransfer = function () {
        $rootScope.AssetDetails = [];
        $scope.transact = {};
        $scope.transact.Status = { Id: 1, ItemId: null, ItemName: "New" };
        vm.apply();
        $scope.getGroups();
        $scope.getSites();
        $scope.getFarmControlNo();
        $scope.dtInstance_1.DataTable.ajax.reload();
        //setTimeout(function () { $("#locTransfer select").selectpicker("refresh"); }, 500);
        (function () {
            function codeBlock() {
                $("#locTransfer select").selectpicker("refresh");
            }
            codeBlock();
        })();
    }

    $scope.resetAssetDisposal = function () {
        $scope.AssetDetailsDisposal = [];
        $scope.disposal = {};
        $scope.disposal.Status = { Id: 1, ItemId: null, ItemName: "New" };
        vm.apply();
        $scope.getGroups();
        $scope.getFarmControlNo();
        $scope.dtInstance_3.DataTable.ajax.reload();
        //setTimeout(function () { $("#assetDisposal select").selectpicker("refresh"); }, 500);
        (function () {
            function codeBlock() {
                $("#assetDisposal select").selectpicker("refresh");
            }
            codeBlock();
        })();
    }

    $scope.resetAssetReplacement = function () {
        $rootScope.AssetDetailsReplacement = [];
        $scope.replacement = {};
        $scope.replacement.Status = { Id: 1, ItemId: null, ItemName: "New" };
        vm.apply();
        $scope.getGroups();
        $scope.getFarmInControlNo();
        $scope.dtInstance_5.DataTable.ajax.reload();
        //setTimeout(function () { $("#assetReplacementt select").selectpicker("refresh"); }, 500);
        (function () {
            function codeBlock() {
                $("#assetReplacementt select").selectpicker("refresh");
            }
            codeBlock();
        })();
    }

    $scope.resetLoanAsset = function () {
        $rootScope.AssetDetailsLoan = [];
        $scope.loan = {};
        $scope.loan.Status = { Id: 1, ItemId: null, ItemName: "New" };
        vm.apply();
        $scope.getGroups();
        $scope.getFarmInControlNo();
        $scope.dtInstance_7.DataTable.ajax.reload();
        //setTimeout(function () { $("#loanAsset select").selectpicker("refresh"); }, 500);
        (function () {
            function codeBlock() {
                $("#loanAsset select").selectpicker("refresh");
            }
            codeBlock();
        })();
    }
}