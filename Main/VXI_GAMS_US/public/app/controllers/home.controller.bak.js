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

    vm.factory = new ApiFactory();

    //new property
    vm.Site = { Sites: "All", Sort: 0 };
    vm.StatusFilter = { "Id": 0, "Status": "All" };
    vm.RequestorStatusFilter = { "Id": 0, "Status": "All" };
    vm.ReqIssueSite = { "Id": 0, "Site": "All", "Sort": 0 };
    vm.ReqIssueLob = { "Id": 0, "SiteId": "", "Lob": "All" };
    vm.Site = { "Id": 0, "Site": "All", "Sort": 0 };
    vm.Lob = { "Id": 0, "SiteId": "", "Lob": "All" };

    vm.forReview = 0;
    vm.forApproval = 0;
    vm.isSearchOngoing = false;
    vm.isSaveOngoing = false;

    vm.modalData = {
        Title: "",
        Editable: false,
        Type: "",
        ReadOnly: false,
        Data: {}
    };
    vm.claimCountDownElement = $(".help-info-left");
    vm.claimCountdownMinutes = 0;
    vm.keyPressedCount = 0;
    $scope.onTimeout = function () {
        if (vm.claimCountdownMinutes > 0) {
            vm.claimCountdownMinutes--;
            vm.claimCountDownElement.css("display", "block");
        } else {
            vm.claimCountDownElement.css("display", "none");
            $("#claim").attr("disabled", "disabled");
        }
        vm.claimTimeout = $timeout($scope.onTimeout, 1000);
    };
    vm.startCountdown = function () {
        if (vm.keyPressedCount > 0) return;
        vm.keyPressedCount = 1;
        vm.claimCountdownMinutes = 1200;
        vm.claimTimeout = $timeout($scope.onTimeout, 1000);
    };
    vm.setModalData = function (title, data, type, editable) {
        data = data || {};
        vm.modalData = {
            Title: title || "",
            Ticket: data.ReferenceNo,
            Editable: editable || false,
            Type: type,
            Status: null,
            ReadOnly: false,
            Data: data
        };
        data.RequestorId = data.RequestorId || "";
        vm.apply();
        switch (type) {
            case "forRequest":
                vm.modalData.Status = data.Status;
                vm.modalData.ReadOnly = data.RequestorId.toLowerCase() === requestorId;
                break;
            case "forReview":
                vm.modalData.Status = data.Status;
                break;
            case "forApproval":
                vm.modalData.Status = data.Status;
                break;
            default:
                vm.modalData.Status = data.Status;
                break;
        }
        vm.apply();
        //console.log("boom", !(vm.UserAttuid === vm.modalData.Data.RequestorTlAttuid && vm.modalData.Type === "forReview") || vm.modalData.Data.RequestorTlApproved === true)
        vm.getIssueDetails();
        //$("#defaultModal").modal("show");
    };

    function error(err) {
        vm.isSearchOngoing = false;
        vm.isSaveOngoing = false;
        try {
            new ToastService().error(err.data.Message);
        } catch (e) {
            new ToastService().error("An error occured");
        }
        btn.removeAttr("disabled");
    }

    function severityHtml(data) {
        data = data || "";
        var labelColor = data.toLowerCase();
        if (data.length < 1) {
            return data;
        } else {
            return "<span class=\"sla-label-" + labelColor + "\">" + data + "</span>";
        }
    }

    function dateHtml(data) {
        var momentDate = moment(data);
        var isValid = momentDate.isValid();
        return !isValid ? "" : moment(data).format("MM/DD/YYYY hh:mm:ss A");
    }

    function statusHtml(data) {
        data = data || "";
        //var labelColor = "default";
        //if (data.indexOf("Pending") > -1) {
        //    labelColor = "primary";
        //}
        //if (data.indexOf("Closed") > -1) {
        //    labelColor = "success";
        //}
        //return "<span class=\"label label-" + labelColor + "\">" + data + "</span>";
        return "<span class=\"label-status-default\">" + data + "</span>";
    }

    function descriptionHtml(data, type, full, meta) {
        var labelColor = "default";
        if (data.Rank === 1) {
            labelColor = "primary";
        }
        if (data.Rank === 2) {
            labelColor = "success";
        }
        return "<span class=\"label label-" + labelColor + "\">" + (data.Description || "") + "</span>";
    }

    function approverOptionHtml(data, type, full, meta) {
        var labelColor = "default";
        if (data.Rank === 1) {
            labelColor = "warning";
        } else if (data.Rank === 2) {
            labelColor = "success";
        } else if (data.Rank === 3) {
            labelColor = "success";
        }
        return "<span class=\"label label-" + labelColor + "\">" + (data.Description || "") + "</span>";
    }

    function actionsHtml(data, type, full, meta, actionType) {
        vm.person = data;
        return "<button class=\"btn btn-warning\" onclick=\"angular.element('section').scope().vm.viewData('".concat(data.ReferenceNo, "','", actionType, "'", ")\">VIEW</button>");
    }

    function requestOption(data, type, full, meta) {
        return actionsHtml(data, type, full, meta, "forRequest");
    }

    function reviewOption(data, type, full, meta) {
        return actionsHtml(data, type, full, meta, "forReview");
    }

    function approveOption(data, type, full, meta) {
        return actionsHtml(data, type, full, meta, "forApproval");
    }

    vm.viewData = function (id, type) {
        try {
            vm.modalData.Title = "";
            vm.modalData.Ticket = "";
            var data;
            switch (type) {
                case "forRequest":
                    var requestData = vm.requestData.filter(function (x) {
                        return x.ReferenceNo === id;
                    });
                    vm.apply();
                    if (requestData.length > 0) {
                        data = requestData[0];
                        vm.apply();
                        vm.setModalData("".concat(data.IssueItemDescription), data, "forRequest", false);
                    }
                    else {
                        throw "Item is either invalid or unreadable try refreshing the page to fix the issue";
                    }
                    break;
                case "forReview":
                    var reviewData = vm.reviewData.filter(function (x) {
                        return x.ReferenceNo === id;
                    });
                    vm.apply();
                    if (reviewData.length > 0) {
                        data = reviewData[0];
                        vm.apply();
                        vm.setModalData("".concat(data.IssueItemDescription), data, "forReview", true);
                    }
                    else {
                        throw "Item is either invalid or unreadable try refreshing the page to fix the issue";
                    }
                    break;
                case "forApproval":
                    var approvalData = vm.approvalData.filter(function (x) {
                        return x.ReferenceNo === id;
                    });
                    vm.apply();
                    if (approvalData.length > 0) {
                        data = approvalData[0];
                        vm.apply();
                        vm.setModalData("".concat(data.IssueItemDescription), data, "forApproval", true);
                    }
                    else {
                        throw "Item is either invalid or unreadable try refreshing the page to fix the issue";
                    }
                    break;
                default:
                    break;
            }
        } catch (e) {
            new ToastService().error(e);
        }
    };
    vm.reportedSite = {
        Id: 0,
        Site: "All"
    };

    // ReSharper disable once UseOfImplicitGlobalInFunctionScope
    vm.dtOptions_request = DTOptionsBuilder.newOptions()
        .withOption("ajax",
            {
                url: "".concat(url,
                    "api/RequestApi/",
                    defaultGuid,
                    "/issuerequest/",
                    vm.RequestorStatusFilter.Id,
                    "/",
                    encodeURIComponent("".concat(vm.ReqIssueSite.Site)),
                    "/",
                    encodeURIComponent("".concat(vm.ReqIssueLob.Lob))),
                type: "POST",
                reloadData: function () {
                    this.reload = true;
                    return this;
                },
                complete: function (jqXHR, textStatus) {
                    vm.requestData = jqXHR.responseJSON.data || [];
                    vm.forRequest = jqXHR.responseJSON.data.length || 0;
                    vm.apply();
                    $("select").selectpicker("refresh");
                    $(".dataTables_length").remove();
                }
            })
        .withDataProp("data")
        .withOption("processing", true)
        .withOption("language",
            {
                "processing": "Loading... Please wait...",
                "search": "SEARCH <strong>REF.NO</strong>:"
            })
        .withOption("serverSide", true)
        .withPaginationType("full_numbers")
        .withOption("bSort", false);
    //.withOption("order", [[1, "asc"]]);
    vm.dtColumns_request = [
        DTColumnBuilder.newColumn("ReferenceNo").withTitle("REF.NO #").withClass("td-reqno"),
        DTColumnBuilder.newColumn("DateRequested").withTitle("DATE REQUESTED").withClass("td-attuid").renderWith(dateHtml),
        DTColumnBuilder.newColumn("RequestedBy").withTitle("REQUESTOR").withClass("td-requestor"),
        DTColumnBuilder.newColumn("IssueLob").withTitle("LOB").withClass("td-lob"),
        DTColumnBuilder.newColumn("IssueItemDescription").withTitle("SUBJECT").withClass("td-subject"),
        DTColumnBuilder.newColumn("Mode").withTitle("MODE").withClass("td-sla display-block").renderWith(severityHtml),
        DTColumnBuilder.newColumn("SlaStatus").withTitle("SLA").withClass("td-sla display-block").renderWith(severityHtml),
        DTColumnBuilder.newColumn("DateClosed").withTitle("DATE CLOSED").withClass("td-attuid").renderWith(dateHtml),
        DTColumnBuilder.newColumn("ClosedBy").withTitle("CLOSED BY").withClass("td-requestor"),
        DTColumnBuilder.newColumn("Status").withTitle("STATUS").withClass("td-status display-block").renderWith(statusHtml),
        //DTColumnBuilder.newColumn("RecipientLocation").withTitle("RESOLUTION").withClass("td-location"),
        //DTColumnBuilder.newColumn("RequestorTlStatus").withTitle("STATUS").withClass("td-location").renderWith(descriptionHtml),
        DTColumnBuilder.newColumn(null).withTitle("DETAILS").notSortable().withClass("td-option for-request").renderWith(requestOption)
    ];
    // ReSharper disable once UseOfImplicitGlobalInFunctionScope
    vm.forReviewInitialLength = -1;
    vm.dtOptions_review = DTOptionsBuilder.newOptions()
        .withOption("ajax",
            {
                url: "".concat(url, "api/RequestApi/", defaultGuid, "/issuereview/", vm.StatusFilter.Id, "/", encodeURIComponent("".concat(vm.Site.Site)), "/", encodeURIComponent("".concat(vm.Lob.Lob))),
                type: "POST",
                reloadData: function () {
                    this.reload = true;
                    return this;
                },
                complete: function (jqXHR, textStatus) {
                    vm.reviewData = jqXHR.responseJSON.data || [];
                    vm.forReview = jqXHR.responseJSON.data.length || 0;
                    if (vm.forReviewInitialLength === -1) {
                        vm.forReviewInitialLength = vm.forReview;
                    }
                    vm.apply();
                    $("select").selectpicker("refresh");
                    $(".dataTables_length").remove();
                }
            })
        .withDataProp("data")
        .withOption("processing", true)
        .withOption("serverSide", true)
        .withOption("language",
            {
                "processing": "Loading... Please wait...",
                "search": "SEARCH <strong>REF.NO</strong>:"
            })
        .withPaginationType("full_numbers")
        .withOption("bSort", false);
    //.withOption("order", [[1, "asc"]]);
    vm.dtColumns_review = [
        DTColumnBuilder.newColumn("ReferenceNo").withTitle("REF.NO #").withClass("td-reqno"),
        DTColumnBuilder.newColumn("DateRequested").withTitle("DATE REQUESTED").withClass("td-attuid").renderWith(dateHtml),
        DTColumnBuilder.newColumn("RequestedBy").withTitle("REQUESTOR").withClass("td-requestor"),
        DTColumnBuilder.newColumn("IssueLob").withTitle("LOB").withClass("td-lob"),
        DTColumnBuilder.newColumn("IssueItemDescription").withTitle("SUBJECT").withClass("td-subject"),
        DTColumnBuilder.newColumn("Mode").withTitle("MODE").withClass("td-sla display-block").renderWith(severityHtml),
        DTColumnBuilder.newColumn("SlaStatus").withTitle("SLA").withClass("td-sla display-block").renderWith(severityHtml),
        DTColumnBuilder.newColumn("DateClosed").withTitle("DATE CLOSED").withClass("td-attuid").renderWith(dateHtml),
        DTColumnBuilder.newColumn("ClosedBy").withTitle("CLOSED BY").withClass("td-requestor"),
        DTColumnBuilder.newColumn("Status").withTitle("STATUS").withClass("td-status display-block").renderWith(statusHtml),
        //DTColumnBuilder.newColumn("RecipientLocation").withTitle("RESOLUTION").withClass("td-location"),
        //DTColumnBuilder.newColumn("RequestorTlStatus").withTitle("STATUS").withClass("td-location").renderWith(descriptionHtml),
        DTColumnBuilder.newColumn(null).withTitle("DETAILS").notSortable().withClass("td-option for-request").renderWith(reviewOption)
    ];

    vm.apply = function () {
        try {
            $scope.$apply();
        } catch (e) {
            //ignore
        }
    };





























    /////////////////////////////////////start new js here

    vm.request = {
        IssueItem: null,
        IssueDetails: null
    };

    vm.ItIssue = null;
    vm.LoginIssue = null;
    vm.SupCallIssue = null;

    vm.getIssueItems = function () {
        // ReSharper disable once UseOfImplicitGlobalInFunctionScope
        vm.factory.getData("RequestApi/".concat(defaultGuid, "/issueitems"))
            .then(function (res) {
                vm.ItIssues = [];
                vm.LoginIssues = [];
                vm.SupCallIssues = [];
                vm.ItIssues = res.data;
                vm.apply();
            }, error);
    };

    vm.IssueStatus = [];
    vm.getIssueStatus = function () {
        vm.factory.getData("RequestApi/".concat(defaultGuid, "/issuestatus"))
            .then(function (res) {
                vm.IssueStatus = [];
                vm.IssueStatus = res.data;
                vm.apply();
                setTimeout(function () {
                    $("select").selectpicker("refresh");
                }, 500);
            }, error);
    };

    vm.IssueResolution = [];
    vm.getIssueResolution = function () {
        vm.factory.getData("RequestApi/".concat(defaultGuid, "/issueresolution"))
            .then(function (res) {
                vm.IssueResolution = [];
                vm.IssueResolution = res.data;
                vm.apply();
                setTimeout(function () {
                    $("select").selectpicker("refresh");
                }, 500);
            }, error);
    };

    vm.IssueSites = [];
    vm.getIssueSites = function () {
        vm.factory.getData("RequestApi/".concat(defaultGuid, "/issuesites"))
            .then(function (res) {
                vm.IssueSites = [];
                vm.IssueSites = res.data;
                vm.apply();
                setTimeout(function () {
                    $("select").selectpicker("refresh");
                }, 500);
            }, error);
    };

    vm.IssueLobs = [];
    vm.HistoryLobs = [];
    vm.GroupLobs = [];
    vm.getLobs = function (siteId, container) {
        siteId = siteId || 0;
        vm.factory.getData("RequestApi/".concat(defaultGuid, "/issuelobs/", siteId))
            .then(function (res) {
                if (container === "vm.IssueLobs") {
                    vm.IssueLobs = [];
                    vm.IssueLobs = res.data;
                    setTimeout(function () {
                        $("select").selectpicker("refresh");
                    },
                        500);
                } else if (container === "vm.HistoryLobs") {
                    vm.HistoryLobs = [];
                    vm.HistoryLobs = res.data;
                    setTimeout(function () {
                        $("select").selectpicker("refresh");
                    },
                        500);
                } else if (container === "vm.GroupLobs") {
                    vm.GroupLobs = [];
                    vm.GroupLobs = res.data;
                    setTimeout(function () {
                        $("select").selectpicker("refresh");
                    },
                        500);
                }
                vm.apply();
                setTimeout(function () {
                    $("select").selectpicker("refresh");
                },
                    1500);
            },
                error);
    };

    //vm.getIssueLobs = function (siteId) {
    //    siteId = siteId || 0;
    //    //vm.factory.getData("RequestApi/".concat(defaultGuid, "/issuelobs/", siteId))
    //    //    .then(function (res) {
    //    //        vm.IssueLobs = [];
    //    //        vm.IssueLobs = res.data;
    //    //        vm.apply();
    //    //        setTimeout(function () {
    //    //            $("select").selectpicker("refresh");
    //    //        }, 1500);
    //    //    }, error);
    //};

    vm.IssueCategories = [];
    vm.getIssueCategories = function () {
        vm.factory.getData("RequestApi/".concat(defaultGuid, "/issuecategories"))
            .then(function (res) {
                vm.IssueCategories = [];
                vm.IssueCategories = res.data;
                vm.apply();
                setTimeout(function () {
                    $("select").selectpicker("refresh");
                }, 500);
            }, error);
    };

    vm.TmpIssueDetails = "";
    vm.getIssueDetails = function () {
        vm.TmpIssueDetails = "";
        vm.apply();
        vm.factory.getData("RequestApi/".concat(vm.modalData.Data.Id, "/issuedetails"))
            .then(function (res) {
                vm.TmpIssueDetails = "";
                try {
                    vm.TmpIssueDetails = res.data.Tmp;
                    vm.modalData.Data.IssueResolution = res.data.IssueResolution;
                    vm.modalData.Data.IssueStatus = res.data.IssueStatus;
                    vm.modalData.Data.Notes = res.data.Notes;
                    vm.apply();
                    setTimeout(function () {
                        $("#select-status, #select-resolution").selectpicker("refresh");
                        $("#defaultModal").modal("show");
                    }, 500);
                } catch (e) {
                    error(e);
                }
            }, error);
    };

    vm.clearCreateRequest = function (e) {
        vm.ItIssue = null;
        vm.LoginIssue = null;
        vm.SupCallIssue = null;
        vm.request = {
            IssueItem: null,
            IssueDetails: null
        };
        vm.apply();
    };
    vm.clearCreateRequest();

    vm.sendRequest = function (issueCategory) {
        try {
            if (vm.ItIssueSite === undefined
                || vm.ItIssueLob === undefined
                || vm.ItIssueCategory === undefined
                || vm.ItIssue === undefined
                || vm.ItIssueSite === null
                || vm.ItIssueLob === null
                || vm.ItIssueCategory === null
                || vm.ItIssue === null
            )
                throw "Site, LOB, Category, Issue and Details are required";
            if (vm.ItIssueSite.Id === undefined
                || vm.ItIssueLob.Id === undefined
                || vm.ItIssueCategory.Id === undefined
                || vm.ItIssue.Id === undefined
            )
                throw "Site, LOB, Category, Issue and Details are required";
            var siteId = vm.ItIssueSite.Id || 0;
            var lobId = vm.ItIssueLob.Id || 0;

            vm.request = {
                IssueItem: vm.ItIssue,
                IssueDetails: vm.IssueDetailsForIt || "",
                IssueSiteId: siteId,
                IssueLobId: lobId
            };

            vm.IssueDetailsForIt = vm.IssueDetailsForIt || "";

            vm.apply();//apply changes

            vm.isSaveOngoing = true;
            vm.apply();
            if (vm.request.IssueItem === null) {
                throw "Kindly select an Issue first";
            }
            vm.request.IssueItem.Item = vm.request.IssueItem.Item || "";
            if (vm.request.IssueItem.Item.trim().length < 1) {
                throw "Kindly specify the issue first";
            }
            if (vm.request.IssueDetails === null || vm.request.IssueDetails.trim().length < 1) {
                throw "Details is Required";
            }

            // ReSharper disable once UseOfImplicitGlobalInFunctionScope
            vm.factory.getData("RequestApi/".concat(defaultGuid, "/submitissue"), vm.request)
                .then(function (res) {
                    new ToastService().success("Request has been submitted")
                        .then(function (result) {
                            window.location.reload();
                        });
                }, error);
        } catch (e) {
            vm.isSaveOngoing = false;
            var asdasdas = e;
            //console.log("e: ", e);
            new ToastService().error(asdasdas);
        }
    };

    vm.ItIssueTmp = {};
    $scope.$watch("vm.ItIssue", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.ItIssueTmp = JSON.parse(JSON.stringify(newValue));
        //console.log("vm.ItIssueTmp: ", vm.ItIssueTmp);
        try {
            if (newValue.Item.indexOf("Other") > -1) {
                newValue.Item = null;
            }
        } catch (e) {
            //ignore
        }
        vm.apply();
        $("select").selectpicker("refresh");
        //console.log("vm.ItIssue: ", vm.ItIssue);
    });
    $scope.$watch("vm.LoginIssue", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        $("#select-login-issues").selectpicker("refresh");
    });
    $scope.$watch("vm.SupCallIssue", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        $("#select-supcall-issue").selectpicker("refresh");
    });
    $scope.$watch("vm.Site", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        $("#select-site-filter").selectpicker("refresh");
        vm.getLobs(newValue.Id, "vm.GroupLobs");
        vm.dtOptions_review.ajax.url = "".concat(url, "api/RequestApi/", defaultGuid, "/issuereview/", vm.StatusFilter.Id, "/", encodeURIComponent("".concat(vm.Site.Site)), "/", encodeURIComponent("".concat(vm.Lob.Lob)));
        vm.dtOptions_review.ajax.reloadData();
    });
    $scope.$watch("vm.StatusFilter", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.Lob.Lob = "All";
        vm.apply();
        $("#select-status-filter").selectpicker("refresh");
        vm.dtOptions_review.ajax.url = "".concat(url, "api/RequestApi/", defaultGuid, "/issuereview/", vm.StatusFilter.Id, "/", encodeURIComponent("".concat(vm.Site.Site)), "/", encodeURIComponent("".concat(vm.Lob.Lob)));
        vm.dtOptions_review.ajax.reloadData();
    });
    $scope.$watch("vm.Lob", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        $("#select-lob-filter").selectpicker("refresh");
        vm.dtOptions_review.ajax.url = "".concat(url, "api/RequestApi/", defaultGuid, "/issuereview/", vm.StatusFilter.Id, "/", encodeURIComponent("".concat(vm.Site.Site)), "/", encodeURIComponent("".concat(vm.Lob.Lob)));
        vm.dtOptions_review.ajax.reloadData();
    });
    $scope.$watch("vm.RequestorStatusFilter", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        $("#select-requestor-status-filter").selectpicker("refresh");
        vm.dtOptions_request.ajax.url = "".concat(url, "api/RequestApi/", defaultGuid, "/issuerequest/", vm.RequestorStatusFilter.Id, "/", encodeURIComponent("".concat(vm.ReqIssueSite.Site)), "/", encodeURIComponent("".concat(vm.ReqIssueLob.Lob)));
        vm.dtOptions_request.ajax.url = "".concat(url, "api/RequestApi/", defaultGuid, "/issuerequest/", vm.RequestorStatusFilter.Id, "/", encodeURIComponent("".concat(vm.ReqIssueSite.Site)), "/", encodeURIComponent("".concat(vm.ReqIssueLob.Lob)));
        vm.dtOptions_request.ajax.reloadData();
    });

    $scope.$watch("vm.ItIssueSite", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        vm.getLobs(newValue.Id, "vm.IssueLobs");
    });

    $scope.$watch("vm.ReqIssueSite", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.ReqIssueLob.Lob = "All";
        vm.apply();
        $("#select-req-issue-site").selectpicker("refresh");
        vm.getLobs(newValue.Id, "vm.HistoryLobs");
        vm.dtOptions_request.ajax.url = "".concat(url, "api/RequestApi/", defaultGuid, "/issuerequest/", vm.RequestorStatusFilter.Id, "/", encodeURIComponent("".concat(vm.ReqIssueSite.Site)), "/", encodeURIComponent("".concat(vm.ReqIssueLob.Lob)));
        vm.dtOptions_request.ajax.reloadData();
    });

    $scope.$watch("vm.ReqIssueLob", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        $("#select-req-issue-lob").selectpicker("refresh");
        vm.dtOptions_request.ajax.url = "".concat(url, "api/RequestApi/", defaultGuid, "/issuerequest/", vm.RequestorStatusFilter.Id, "/", encodeURIComponent("".concat(vm.ReqIssueSite.Site)), "/", encodeURIComponent("".concat(vm.ReqIssueLob.Lob)));
        vm.dtOptions_request.ajax.reloadData();
    });

    $scope.$watch("vm.modalData.Data.IssueResolution", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        $("#select-resolution").selectpicker("refresh");
    });

    $scope.$watch("vm.modalData.Data.IssueStatus", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //console.log("vm.modalData.Data.IssueStatus: ", vm.modalData.Data.IssueStatus);
        $("#select-status").selectpicker("refresh");
    });

    $scope.$watch("vm.ItIssueCategory", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        //console.log("ItIssueCategory: ", vm.ItIssueCategory);
        setTimeout(function () {
            $("#select-it-issue").selectpicker("refresh");
            $("#select-issue-categories").selectpicker("refresh");
        },
            100);
    });

    $scope.$watch("vm.reportedSite", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        $("#select-download-site-filter").selectpicker("refresh");
    });

    vm.setStatus = function ($event, id) {
        try {
            $($($event.currentTarget).parents()[1]).find("button.btn-block").removeClass("active");
            $($event.currentTarget).addClass("active");
        } catch (e) {
            //ignore
        }
        var status = vm.IssueStatus.filter(function (x) {
            return x.Id === id;
        });
        if (status.length > 0) {
            vm.RequestorStatusFilter = status[0];
        }
    };

    vm.setIssueStatus = function ($event, id) {
        try {
            $($($event.currentTarget).parents()[1]).find("button.btn-block").removeClass("active");
            $($event.currentTarget).addClass("active");
        } catch (e) {
            //ignore
        }
        var status = vm.IssueStatus.filter(function (x) {
            return x.Id === id;
        });
        if (status.length > 0) {
            vm.StatusFilter = status[0];
        }
    };

    vm.processReview = function () {
        vm.isSaveOngoing = true;
        try {
            if (vm.modalData.Data.Id === null) {
                throw "Invalid Item";
            }

            // ReSharper disable once UseOfImplicitGlobalInFunctionScope
            vm.factory.getData("RequestApi/".concat(defaultGuid, "/processissue"), vm.modalData.Data)
                .then(function (res) {
                    new ToastService().success("Ticket has been updated")
                        .then(function (result) {
                            window.location.reload();
                        });
                }, error);
        } catch (e) {
            vm.isSaveOngoing = false;
            new ToastService().error(e);
        }
    };


    vm.downloadReport = function () {
        vm.reportedFrom = vm.reportedFrom || "";
        vm.reportedTo = vm.reportedTo || "";
        //console.log("reportedFrom:", vm.reportedFrom);
        //console.log("reportedTo:", vm.reportedTo);
        //console.log("reportedSite:", vm.reportedSite);
        try {
            if (vm.reportedSite === undefined) {
                throw "Select a Site first";
            }
            if (vm.reportedFrom !== "") {
                vm.reportedFrom = moment(vm.reportedFrom).format("MM/DD/YYYY");
            }

            if (vm.reportedTo !== "") {
                vm.reportedTo = moment(vm.reportedTo).format("MM/DD/YYYY");
            }

            window.open("".concat(url, "Report/GetReport?f=", vm.reportedFrom, "&t=", vm.reportedTo, "&l=", vm.reportedSite.Id));
        } catch (e) {
            error({ data: { Message: e } });
        }
    };

    vm.onLoad = function () {
        var cookie = Cookies.get("popup");
        if (cookie === undefined) {
            $("#notifyModal").modal("show");
            setTimeout(function () {
                $("#notifyModal").modal("hide");
            }, 10000);
        }
        vm.getIssueStatus();
        vm.getIssueSites();
        vm.getIssueItems();
        vm.getIssueCategories();
        vm.getIssueResolution();
        setTimeout(function () {
            $("select").selectpicker("refresh");
            vm.apply();
            if ($("[name='from']")[0].type !== "date")
                $("[name='from']").datepicker();
            if ($("[name='to']")[0].type !== "date")
                $("[name='to']").datepicker();
        }, 1000);
    };
    vm.onLoad();
}