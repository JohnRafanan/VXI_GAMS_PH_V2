// ReSharper disable all IdentifierTypo
function _toConsumableArray(arr) { return _arrayWithoutHoles(arr) || _iterableToArray(arr) || _nonIterableSpread(); }

function _nonIterableSpread() { throw new TypeError("Invalid attempt to spread non-iterable instance"); }

function _iterableToArray(iter) { if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter); }

function _arrayWithoutHoles(arr) { if (Array.isArray(arr)) { for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) { arr2[i] = arr[i]; } return arr2; } }

app.controller("assetManageCtrl", assetManageCtrl);
assetManageCtrl.$inject = ["$rootScope", "$scope", "$http", "DTOptionsBuilder", "DTColumnBuilder", "ApiFactory", "$timeout"];
function assetManageCtrl($rootScope, $scope, $http, DTOptionsBuilder, DTColumnBuilder, ApiFactory, $timeout) {
    var vm = this;
    var btn = $("#btn-click");
    vm.isSaveOngoing = false;

    vm.Notifications = {
        RecordAdded: "RECORD HAS BEEN ADDED",
        RecordFailed: "ERROR WHILE SAVING THE RECORD",
        CategoryRequired: "CATEGORY IS REQUIRED"
    };

    vm.Categories = [];

    vm.CategoryModel = {
        "Id": 0,
        "Title": null,
        "ParentName": null,
        "ParentId": 0,
        "CreatedBy": username,
        "UpdatedBy": username
    };
    vm.SubCategoryModel = {
        "Id": 0,
        "Title": null,
        "ParentName": null,
        "ParentId": 0,
        "CreatedBy": username,
        "UpdatedBy": username,
        "Category": {}
    };
    vm.EmailNotificationModel = {
        "Id": 0,
        "Title": null,
        "EmailTo": null,
        "EmailCc": null,
        "ParentName": null,
        "ParentId": 0,
        "CreatedBy": username,
        "UpdatedBy": username
    };
    vm.VendorModel = {
        "Id": 0,
        "Title": null,
        "ParentName": null,
        "ParentId": 0,
        "CreatedBy": username,
        "UpdatedBy": username
    };
    vm.BrandModel = {
        "Id": 0,
        "Title": null,
        "ParentName": null,
        "ParentId": 0,
        "CreatedBy": username,
        "UpdatedBy": username
    };
    vm.FinancialTreatmentModel = {
        "Id": 0,
        "Title": null,
        "ParentName": null,
        "ParentId": 0,
        "CreatedBy": username,
        "UpdatedBy": username
    };
    vm.GroupModel = {
        "Id": 0,
        "Title": null,
        "ParentName": null,
        "ParentId": 0,
        "CreatedBy": username,
        "UpdatedBy": username
    };
    vm.UserModel = {
        "UserRole": []
    };
    vm.RoleModel = {
        "Id": 0,
        "ItemId": null,
        "ItemName": null
    };
    function error(err) {
        vm.isSearchOngoing = false;
        vm.isSaveOngoing = false;
        vm.IsExtracting = false;
        vm.apply();
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
    function actionsHtml(data, func) {

        return "<button class=\"btn btn-warning\" type=\"button\" onclick=\"angular.element('section').scope().vm.".concat(func, "('", data.Id, "','EDIT')\">", "EDIT", "</button>") +
            "<button class=\"btn btn-warning\" type=\"button\" onclick=\"angular.element('section').scope().vm.".concat(func, "('", data.Id, "','DELETE')\">", "DELETE", "</button>");
    }
    function categoryAction(data) {
        return actionsHtml(data, "updateCategoryById");
    }
    function subCategoryAction(data) {
        return actionsHtml(data, "updateSubCategoryById");
    }
    function emailAction(data) {
        return actionsHtml(data, "updateEmailNotificationById");
    }
    function vendorAction(data) {
        return actionsHtml(data, "updateVendorById");
    }
    function brandAction(data) {
        return actionsHtml(data, "updateBrandById");
    }
    function financialAction(data) {
        return actionsHtml(data, "updateFinancialTreatmentById");
    }
    function groupAction(data) {
        return actionsHtml(data, "updateGroupById");
    }
    function userAction(data) {
        return actionsHtml(data, "updateUserById");
    }

    vm.dtInstance_Category = {};
    vm.dtOptions_Category = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("category"))
        .withOption("autoWidth", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        //.withOption("deferLoading", 10)
        .withOption("bSort", false)
        .withPaginationType("full_numbers");
    //.withOption("scrollX", true);
    vm.dtColumns_Category = [
        DTColumnBuilder.newColumn(null).withTitle("OPTIONS").withClass("td-option").renderWith(categoryAction),
        DTColumnBuilder.newColumn('ItemName').withTitle('CATEGORY')
    ];

    vm.dtInstance_SubCategory = {};
    vm.dtOptions_SubCategory = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("subcategory/parent/0"))
        .withOption("autoWidth", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        //.withOption("deferLoading", 10)
        .withOption("bSort", false)
        .withPaginationType("full_numbers");
    //.withOption("scrollX", true);
    vm.dtColumns_SubCategory = [
        DTColumnBuilder.newColumn(null).withTitle("OPTIONS").withClass("td-option").renderWith(subCategoryAction),
        DTColumnBuilder.newColumn('ItemName').withTitle('SUBCATEGORY')
    ];

    vm.dtInstance_EmailNotification = {};
    vm.dtOptions_EmailNotification = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("email"))
        .withOption("autoWidth", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        //.withOption("deferLoading", 10)
        .withOption("bSort", false)
        .withPaginationType("full_numbers");
    //.withOption("scrollX", true);
    vm.dtColumns_EmailNotification = [
        DTColumnBuilder.newColumn(null).withTitle("OPTIONS").withClass("td-option").renderWith(emailAction),
        DTColumnBuilder.newColumn('ItemName').withTitle('DISTRO')
    ];

    vm.dtInstance_Vendor = {};
    vm.dtOptions_Vendor = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("vendor"))
        .withOption("autoWidth", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        //.withOption("deferLoading", 10)
        .withOption("bSort", false)
        .withPaginationType("full_numbers");
    //.withOption("scrollX", true);
    vm.dtColumns_Vendor = [
        DTColumnBuilder.newColumn(null).withTitle("OPTIONS").withClass("td-option").renderWith(vendorAction),
        DTColumnBuilder.newColumn('ItemName').withTitle('VENDOR')
    ];

    vm.dtInstance_Brand = {};
    vm.dtOptions_Brand = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("brand"))
        .withOption("autoWidth", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        //.withOption("deferLoading", 10)
        .withOption("bSort", false)
        .withPaginationType("full_numbers");
    //.withOption("scrollX", true);
    vm.dtColumns_Brand = [
        DTColumnBuilder.newColumn(null).withTitle("OPTIONS").withClass("td-option").renderWith(brandAction),
        DTColumnBuilder.newColumn('ItemName').withTitle('BRAND')
    ];

    vm.dtInstance_FinancialTreatment = {};
    vm.dtOptions_FinancialTreatment = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("financial-treatment"))
        .withOption("autoWidth", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        //.withOption("deferLoading", 10)
        .withOption("bSort", false)
        .withPaginationType("full_numbers");
    //.withOption("scrollX", true);
    vm.dtColumns_FinancialTreatment = [
        DTColumnBuilder.newColumn(null).withTitle("OPTIONS").withClass("td-option").renderWith(financialAction),
        DTColumnBuilder.newColumn('ItemName').withTitle('FINANCIAL TREATMENT')
    ];

    vm.dtInstance_Group = {};
    vm.dtOptions_Group = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("group"))
        .withOption("autoWidth", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        //.withOption("deferLoading", 10)
        .withOption("bSort", false)
        .withPaginationType("full_numbers");
    //.withOption("scrollX", true);
    vm.dtColumns_Group = [
        DTColumnBuilder.newColumn(null).withTitle("OPTIONS").withClass("td-option").renderWith(groupAction),
        DTColumnBuilder.newColumn('ItemName').withTitle('GROUP')
    ];

    vm.dtInstance_User = {};
    vm.dtOptions_User = DTOptionsBuilder
        .fromSource(globalApiBaseUrl.concat("user"))
        .withOption("autoWidth", true)
        .withOption("bLengthChange", false)
        .withOption("searching", true)
        .withOption("info", false)
        //.withOption("deferLoading", 10)
        .withOption("bSort", false)
        .withPaginationType("full_numbers");
    //.withOption("scrollX", true);
    vm.dtColumns_User = [
        DTColumnBuilder.newColumn(null).withTitle("OPTIONS").withClass("td-option").renderWith(userAction),
        DTColumnBuilder.newColumn("HRID").withTitle('HRID'),
        DTColumnBuilder.newColumn("FirstName").withTitle('FIRST NAME'),
        DTColumnBuilder.newColumn("LastName").withTitle('LAST NAME'),
        DTColumnBuilder.newColumn("LineOfBusiness").withTitle('LOB'),
        //DTColumnBuilder.newColumn("Team").withTitle('TEAM'),
        //DTColumnBuilder.newColumn("BuildingAssignment").withTitle('SITE'),
    ];

    vm.factory = new ApiFactory();

    vm.getCategories = function () {
        vm.factory.getData("category")
            .then(function (res) {
                vm.Categories = JSON.parse(JSON.stringify(res.data));
                vm.apply();
                setTimeout(function () {
                    $("select").selectpicker("refresh");
                }, 500);
            },
                error);
    };

    vm.Roles = [];
    vm.getRoles = function () {
        vm.factory.getData("user/roles")
            .then(function (res) {
                vm.Roles = JSON.parse(JSON.stringify(res.data));
                vm.apply();
                setTimeout(function () {
                    $("select").selectpicker("refresh");
                }, 500);
            },
                error);
    };

    vm.resetCategory = function () {
        vm.CategoryModel = {
            "id": 0,
            "title": null,
            "parentName": null,
            "parentId": 0,
            "createdBy": username,
            "updatedBy": username
        };
        vm.apply();
        setTimeout(vm.dtInstance_Category.DataTable.ajax.reload, 100);
        setTimeout(vm.getCategories, 100);
    };

    vm.resetSubCategory = function () {
        var currentCategory = JSON.parse(JSON.stringify(vm.SubCategoryModel.Category));
        vm.SubCategoryModel = {
            "Id": 0,
            "Title": null,
            "ParentName": null,
            "ParentId": 0,
            "CreatedBy": username,
            "UpdatedBy": username,
            "Category": currentCategory
        };
        vm.apply();
        //function below are being called already in $scope.$watch
        //setTimeout(vm.dtInstance_SubCategory.DataTable.ajax.reload, 100);
    };

    vm.resetEmailNotification = function () {
        vm.EmailNotificationModel = {
            "Id": 0,
            "Title": null,
            "EmailTo": null,
            "EmailCc": null,
            "ParentName": null,
            "ParentId": 0,
            "CreatedBy": username,
            "UpdatedBy": username
        };
        vm.apply();
        setTimeout(vm.dtInstance_EmailNotification.DataTable.ajax.reload, 100);
    };

    vm.resetVendor = function () {
        vm.VendorModel = {
            "Id": 0,
            "Title": null,
            "EmailTo": null,
            "EmailCc": null,
            "ParentName": null,
            "ParentId": 0,
            "CreatedBy": username,
            "UpdatedBy": username
        };
        vm.apply();
        setTimeout(vm.dtInstance_Vendor.DataTable.ajax.reload, 100);
    };

    vm.resetFinancialTreatment = function () {
        vm.FinancialTreatmentModel = {
            "Id": 0,
            "Title": null,
            "ParentName": null,
            "ParentId": 0,
            "CreatedBy": username,
            "UpdatedBy": username
        };
        vm.apply();
        setTimeout(vm.dtInstance_FinancialTreatment.DataTable.ajax.reload, 100);
    };

    vm.resetGroup = function () {
        vm.GroupModel = {
            "Id": 0,
            "Title": null,
            "ParentName": null,
            "ParentId": 0,
            "CreatedBy": username,
            "UpdatedBy": username
        };
        vm.apply();
        setTimeout(vm.dtInstance_Group.DataTable.ajax.reload, 100);
    };

    vm.resetUser = function () {
        vm.UserModel = {
            "Id": 0,
            "Title": null,
            "ParentName": null,
            "ParentId": 0,
            "CreatedBy": username,
            "UpdatedBy": username,
            "Employee": {},
            "Role": []
        };
        vm.apply();
        setTimeout(function () {
            $("select").selectpicker("refresh");
        }, 250);
        setTimeout(vm.dtInstance_User.DataTable.ajax.reload, 100);
    };

    vm.resetBrand = function () {
        vm.BrandModel = {
            "Id": 0,
            "Title": null,
            "ParentName": null,
            "ParentId": 0,
            "CreatedBy": username,
            "UpdatedBy": username
        };
        vm.apply();
        setTimeout(vm.dtInstance_Brand.DataTable.ajax.reload, 100);
    };

    vm.updateCategoryById = function (id, type) {
        try {
            id = id || 0;
            type = type || "EDIT";
            if (id === 0) {
                type = "ADD";
            }
            type = type.toUpperCase();
            if (type === "ADD") {
                //todo
                var itemId = vm.CategoryModel.Id || 0;
                vm.CategoryModel.IsActive = true;
                vm.apply();
                if (vm.CategoryModel.Title === undefined ||
                    vm.CategoryModel.Title === null ||
                    vm.CategoryModel.Title.trim().length < 1) {
                    throw "Enter a Category first";
                }
                if (itemId === 0) {
                    vm.factory.setData("category/manage", vm.CategoryModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN ADDED");
                            vm.resetCategory();
                        },
                            error);
                } else {
                    vm.factory.putData("category/manage", vm.CategoryModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN UPDATED");
                            vm.resetCategory();
                        },
                            error);
                }
            } else if (type === "EDIT") {
                //todo
                vm.factory.getData("category/manage/".concat(id))
                    .then(function (res) {
                        vm.CategoryModel = JSON.parse(JSON.stringify(res.data));
                        vm.apply();
                    },
                        error);
            } else if (type === "DELETE") {
                //todo
                vm.factory.deleteData("category/delete/".concat(id, "/", username))
                    .then(function () {
                        new ToastService().success("RECORD HAS BEEN DELETED");
                        vm.resetCategory();
                    },
                        error);
            }
        } catch (e) {
            error({ data: { Message: e } });
        }
    };

    vm.updateSubCategoryById = function (id, type) {
        try {
            id = id || 0;
            type = type || "EDIT";
            if (id === 0) {
                type = "ADD";
            }
            type = type.toUpperCase();
            if (type === "ADD") {
                //todo
                var itemId = vm.SubCategoryModel.Id || 0;
                vm.SubCategoryModel.IsActive = true;
                vm.apply();
                if (itemId === 0) {
                    if (vm.SubCategoryModel.Category.Id === undefined) {
                        throw "Select a Category first";
                    }
                    if (vm.SubCategoryModel.Title === undefined ||
                        vm.SubCategoryModel.Title === null ||
                        vm.SubCategoryModel.Title.trim().length < 1) {
                        throw "Enter a SubCategory first";
                    }
                    vm.SubCategoryModel.ParentId = vm.SubCategoryModel.Category.Id;
                    vm.factory.setData("subcategory/manage", vm.SubCategoryModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN ADDED");
                            vm.resetSubCategory();
                        },
                            error);
                } else {
                    vm.factory.putData("subcategory/manage", vm.SubCategoryModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN UPDATED");
                            vm.resetSubCategory();
                        },
                            error);
                }
            } else if (type === "EDIT") {
                //todo
                vm.factory.getData("subcategory/manage/".concat(id))
                    .then(function (res) {
                        var subcategory = JSON.parse(JSON.stringify(res.data));
                        vm.SubCategoryModel.Id = subcategory.Id;
                        vm.SubCategoryModel.ParentId = subcategory.ParentId;
                        vm.SubCategoryModel.Title = subcategory.Title;
                        //vm.apply();
                    },
                        error);
            } else if (type === "DELETE") {
                //todo
                vm.factory.deleteData("subcategory/delete/".concat(id, "/", username))
                    .then(function () {
                        new ToastService().success("RECORD HAS BEEN DELETED");
                        vm.resetSubCategory();
                    },
                        error);
            }
        } catch (e) {
            error({ data: { Message: e } });
        }
    };

    vm.updateEmailNotificationById = function (id, type) {
        try {
            id = id || 0;
            type = type || "EDIT";
            if (id === 0) {
                type = "ADD";
            }
            type = type.toUpperCase();
            if (type === "ADD") {
                //todo
                var itemId = vm.EmailNotificationModel.Id || 0;
                vm.EmailNotificationModel.IsActive = true;
                vm.apply();
                if (vm.EmailNotificationModel.Title === undefined ||
                    vm.EmailNotificationModel.Title === null ||
                    vm.EmailNotificationModel.Title.trim().length < 1) {
                    throw "Enter a Group Address Name first";
                }
                if (itemId === 0) {
                    vm.factory.setData("email/manage", vm.EmailNotificationModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN ADDED");
                            vm.resetEmailNotification();
                        },
                            error);
                } else {
                    vm.factory.putData("email/manage", vm.EmailNotificationModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN UPDATED");
                            vm.resetEmailNotification();
                        },
                            error);
                }
            } else if (type === "EDIT") {
                //todo
                vm.factory.getData("email/manage/".concat(id))
                    .then(function (res) {
                        vm.EmailNotificationModel = JSON.parse(JSON.stringify(res.data));
                        vm.apply();
                    },
                        error);
            } else if (type === "DELETE") {
                //todo
                vm.factory.deleteData("email/delete/".concat(id, "/", username))
                    .then(function () {
                        new ToastService().success("RECORD HAS BEEN DELETED");
                        vm.resetEmailNotification();
                    },
                        error);
            }
        } catch (e) {
            error({ data: { Message: e } });
        }
    };

    vm.updateVendorById = function (id, type) {
        try {
            id = id || 0;
            type = type || "EDIT";
            if (id === 0) {
                type = "ADD";
            }
            type = type.toUpperCase();
            if (type === "ADD") {
                //todo
                var itemId = vm.VendorModel.Id || 0;
                vm.VendorModel.IsActive = true;
                vm.apply();
                if (vm.VendorModel.Title === undefined ||
                    vm.VendorModel.Title === null ||
                    vm.VendorModel.Title.trim().length < 1) {
                    throw "Enter a Vendor Name first";
                }
                if (itemId === 0) {
                    vm.factory.setData("vendor/manage", vm.VendorModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN ADDED");
                            vm.resetVendor();
                        },
                            error);
                } else {
                    vm.factory.putData("vendor/manage", vm.VendorModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN UPDATED");
                            vm.resetVendor();
                        },
                            error);
                }
            } else if (type === "EDIT") {
                //todo
                vm.factory.getData("vendor/manage/".concat(id))
                    .then(function (res) {
                        vm.VendorModel = JSON.parse(JSON.stringify(res.data));
                        vm.apply();
                    },
                        error);
            } else if (type === "DELETE") {
                //todo
                vm.factory.deleteData("vendor/delete/".concat(id, "/", username))
                    .then(function () {
                        new ToastService().success("RECORD HAS BEEN DELETED");
                        vm.resetVendor();
                    },
                        error);
            }
        } catch (e) {
            error({ data: { Message: e } });
        }
    };

    vm.updateBrandById = function (id, type) {
        try {
            id = id || 0;
            type = type || "EDIT";
            if (id === 0) {
                type = "ADD";
            }
            type = type.toUpperCase();
            if (type === "ADD") {
                //todo
                var itemId = vm.BrandModel.Id || 0;
                vm.BrandModel.IsActive = true;
                vm.apply();
                if (vm.BrandModel.Title === undefined ||
                    vm.BrandModel.Title === null ||
                    vm.BrandModel.Title.trim().length < 1) {
                    throw "Enter a Brand Name first";
                }
                if (itemId === 0) {
                    vm.factory.setData("brand/manage", vm.BrandModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN ADDED");
                            vm.resetBrand();
                        },
                            error);
                } else {
                    vm.factory.putData("brand/manage", vm.BrandModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN UPDATED");
                            vm.resetBrand();
                        },
                            error);
                }
            } else if (type === "EDIT") {
                //todo
                vm.factory.getData("brand/manage/".concat(id))
                    .then(function (res) {
                        vm.BrandModel = JSON.parse(JSON.stringify(res.data));
                        vm.apply();
                    },
                        error);
            } else if (type === "DELETE") {
                //todo
                vm.factory.deleteData("brand/delete/".concat(id, "/", username))
                    .then(function () {
                        new ToastService().success("RECORD HAS BEEN DELETED");
                        vm.resetBrand();
                    },
                        error);
            }
        } catch (e) {
            error({ data: { Message: e } });
        }
    };

    vm.updateFinancialTreatmentById = function (id, type) {
        try {
            id = id || 0;
            type = type || "EDIT";
            if (id === 0) {
                type = "ADD";
            }
            type = type.toUpperCase();
            if (type === "ADD") {
                //todo
                var itemId = vm.FinancialTreatmentModel.Id || 0;
                vm.FinancialTreatmentModel.IsActive = true;
                vm.apply();
                if (vm.FinancialTreatmentModel.Title === undefined ||
                    vm.FinancialTreatmentModel.Title === null ||
                    vm.FinancialTreatmentModel.Title.trim().length < 1) {
                    throw "Enter a Financial Treatment first";
                }
                if (itemId === 0) {
                    vm.factory.setData("financial-treatment/manage", vm.FinancialTreatmentModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN ADDED");
                            vm.resetFinancialTreatment();
                        },
                            error);
                } else {
                    vm.factory.putData("financial-treatment/manage", vm.FinancialTreatmentModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN UPDATED");
                            vm.resetFinancialTreatment();
                        },
                            error);
                }
            } else if (type === "EDIT") {
                //todo
                vm.factory.getData("financial-treatment/manage/".concat(id))
                    .then(function (res) {
                        vm.FinancialTreatmentModel = JSON.parse(JSON.stringify(res.data));
                        vm.apply();
                    },
                        error);
            } else if (type === "DELETE") {
                //todo
                vm.factory.deleteData("financial-treatment/delete/".concat(id, "/", username))
                    .then(function () {
                        new ToastService().success("RECORD HAS BEEN DELETED");
                        vm.resetFinancialTreatment();
                    },
                        error);
            }
        } catch (e) {
            error({ data: { Message: e } });
        }
    };

    vm.updateGroupById = function (id, type) {
        try {
            id = id || 0;
            type = type || "EDIT";
            if (id === 0) {
                type = "ADD";
            }
            type = type.toUpperCase();
            if (type === "ADD") {
                //todo
                var itemId = vm.GroupModel.Id || 0;
                vm.GroupModel.IsActive = true;
                vm.apply();
                if (vm.GroupModel.Title === undefined ||
                    vm.GroupModel.Title === null ||
                    vm.GroupModel.Title.trim().length < 1) {
                    throw "Enter a Brand Name first";
                }
                if (itemId === 0) {
                    vm.factory.setData("group/manage", vm.GroupModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN ADDED");
                            vm.resetGroup();
                        },
                            error);
                } else {
                    vm.factory.putData("group/manage", vm.GroupModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN UPDATED");
                            vm.resetGroup();
                        },
                            error);
                }
            } else if (type === "EDIT") {
                //todo
                vm.factory.getData("group/manage/".concat(id))
                    .then(function (res) {
                        vm.GroupModel = JSON.parse(JSON.stringify(res.data));
                        vm.apply();
                    },
                        error);
            } else if (type === "DELETE") {
                //todo
                vm.factory.deleteData("group/delete/".concat(id, "/", username))
                    .then(function () {
                        new ToastService().success("RECORD HAS BEEN DELETED");
                        vm.resetGroup();
                    },
                        error);
            }
        } catch (e) {
            error({ data: { Message: e } });
        }
    };

    vm.updateUserById = function (id, type) {
        console.log("vm.UserModel: ", vm.UserModel);
        vm.UserModel.CreatedBy = username;
        vm.UserModel.UpdatedBy = username;
        //vm.UserModel = {
        //    "Id": 0,
        //    "Title": null,
        //    "ParentName": null,
        //    "ParentId": 0,
        //    "CreatedBy": username,
        //    "UpdatedBy": username,
        //    "Employee": {},
        //    "Role": []
        //};
        try {
            id = id || 0;
            type = type || "EDIT";
            if (id === 0) {
                type = "ADD";
            }
            type = type.toUpperCase();
            if (type === "ADD") {
                //todo
                var itemId = vm.UserModel.Id || 0;
                vm.UserModel.IsActive = true;
                vm.apply();
                if (vm.UserModel.HRID === undefined ||
                    vm.UserModel.HRID === null ||
                    vm.UserModel.HRID.trim().length < 1) {
                    throw "Select an Employee first";
                }
                if (itemId === 0) {
                    vm.factory.setData("user/manage", vm.UserModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN ADDED");
                            vm.resetUser();
                        },
                            error);
                } else {
                    vm.factory.putData("user/manage", vm.UserModel)
                        .then(function () {
                            new ToastService().success("RECORD HAS BEEN UPDATED");
                            vm.resetUser();
                        },
                            error);
                }
            } else if (type === "EDIT") {
                //todo
                vm.factory.getData("user/manage/".concat(id))
                    .then(function (res) {
                        vm.UserModel = JSON.parse(JSON.stringify(res.data));
                        vm.apply();
                        setTimeout(function () {
                            $("select").selectpicker("refresh");
                        }, 250);
                    },
                        error);
            } else if (type === "DELETE") {
                //todo
                vm.factory.deleteData("user/delete/".concat(id, "/", username))
                    .then(function () {
                        new ToastService().success("RECORD HAS BEEN DELETED");
                        vm.resetUser();
                    },
                        error);
            }
        } catch (e) {
            error({ data: { Message: e } });
        }
    };

    $scope.$watch("vm.SubCategoryModel.Category", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        $("select").selectpicker("refresh");
        var itemId = newValue.Id || 0;
        vm.SubCategoryModel.Id = 0;
        vm.SubCategoryModel.ParentId = itemId;
        vm.SubCategoryModel.Title = null;
        vm.dtInstance_SubCategory.DataTable.ajax.url(globalApiBaseUrl.concat("subcategory/parent/".concat(itemId)));
        setTimeout(vm.dtInstance_SubCategory.DataTable.ajax.reload, 100);
    });

    $scope.$watch("vm.UserModel.Role", function (newValue, oldValue) {
        if (!newValue || newValue === oldValue) return;
        vm.apply();
        console.log("vm.: ", vm.UserModel);
    });

    vm.TmpEmployee = {};
    vm.SearchEmployeeHrid = "";
    vm.getEmployeeByHrid = function () {
        vm.TmpEmployee = JSON.parse(JSON.stringify({}));
        vm.apply();
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
        vm.UserModel = JSON.parse(JSON.stringify(vm.TmpEmployee));
        vm.UserModel.Id = 0;
        vm.UserModel.UserRole = [];
        vm.TmpEmployee = JSON.parse(JSON.stringify({}));
        vm.apply();
        setTimeout(function () {
            $("select").selectpicker("refresh");
        }, 100);
        $("#employeeModal").modal("hide");
    };

    vm.ExpiryAssets = [];
    vm.getExpiredAssets = function () {
        vm.factory.setData("asset/search", { Site: "", Month: parseInt(moment().format("MM")) })
            .then(function (res) {
                vm.ExpiryAssets = JSON.parse(JSON.stringify(res.data));
                vm.apply();
            },
                function () {
                    new ToastService().error("BAD");
                });
    };
    vm.FarmLineFiler = function (item, a, b, c) {
        return item.Id = vm.ExpiryAssets.FarmLineItemId;
    };
    vm.AssetQr = [{
        ControlNo: '',
        QRCode: '',
        Description: '',
        IsChecked: '',
        Site: '',
    }];
    vm.IsExtracting = true;
    vm.getAssetQR = function () {
        vm.AssetQr = JSON.parse(JSON.stringify([]));
        vm.IsExtracting = true;
        vm.apply();
        vm.factory.getData("asset?withQr=true&group=")
            .then(function (res) {
                for (var i = 0; i < res.data.length; i++) {
                    vm.AssetQr.push({
                        ControlNo: res.data[i].ControlNo,
                        Description: res.data[i].Description,
                        Site: res.data[i].Site,
                        IsChecked: false,
                        QRCode: res.data[i].QRCode,
                    });
                }
                vm.IsExtracting = false;
                vm.apply();
            },
                error);
    };
    vm.SelectedQr = [];

    vm.returnQR = [];
    vm.printQr = function () {
        var selectedQr = vm.AssetQr.filter(function (i) {
            return i.IsChecked === true;
        });
        vm.SelectedQr = JSON.parse(JSON.stringify(selectedQr));
        vm.apply();
        $("#QRCodeModal").modal("show");
        //console.log(vm.SelectedQr);
    }

    vm.closeMOdalQr = function () {
        $("#QRCodeModal").modal("hide");
    }

    vm.Load = function () {
        vm.getCategories();
        vm.getRoles();
        vm.getExpiredAssets();
        vm.getAssetQR();
    };
    vm.Load();
}