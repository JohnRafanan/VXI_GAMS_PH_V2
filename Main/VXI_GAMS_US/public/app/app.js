// ReSharper disable all StringLiteralTypo
var app = angular.module("applicationModule", ["datatables", "datatables.columnfilter", "ngIdle", "ng-fusioncharts", "ngRoute", "ngSanitize"]);
app.factory("IdleTimeout", function ($timeout, $document) {

    return function (delay, onIdle, playOnce) {

        var idleTimeout = function (delay, onIdle) {
            var $this = this;
            $this.idleTime = delay;
            $this.goneIdle = function () {
                console.log("Gone Idle");
                onIdle();
                $timeout.cancel($this.timeout);
            };
            return {
                cancelCountdown: function () {
                    console.log("cancelTimeout");
                    return $timeout.cancel($this.timeout);
                },
                startCountdown: function (event) {
                    console.log("startTimeout", $this.idleTime);
                    $this.timeout = $timeout(function () {
                        $this.goneIdle();
                    },
                        $this.idleTime);
                }
            };
        };

        var events = [
            "keydown", "keyup", "click", "mousemove", "DOMMouseScroll", "mousewheel", "mousedown", "touchstart",
            "touchmove", "touchend", "mouseover", "scroll", "mouseup"
        ];
        var $body = angular.element($document);
        var reset = function () {
            idleTimer.cancelCountdown();
            idleTimer.startCountdown();
        };
        var idleTimer = idleTimeout(delay, onIdle);


        return {
            active: true,
            cancelCountdown: function () {
                idleTimer.cancelCountdown();
                angular.forEach(events,
                    function (event) {
                        $body.off(event, reset);
                    });
            },
            startCountdown: function () {
                idleTimer.startCountdown();
                angular.forEach(events,
                    function (event) {
                        $body.on(event, reset);
                    });
            }
        };
    };
});
app.factory('templateInterceptor', function ($injector, $q) {
    return {
        request: function (config) {
            if (config.url.indexOf('views') !== -1) {
                config.url = config.url + '?t=' + new Date().getTime();
            }
            return config;
        },
        responseError: function (response) {
            //if (response.status === 404 && /\.html$/.test(response.config.url)) {
            if (response.status === 404 && /\.html/.test(response.config.url)) {
                response.config.url = root + "/public/app/views/PageNotFound.html";
                return $injector.get("$http")(response.config);
            }
            return $q.reject(response);
        }
    }
});
app.directive("timeoutTest", ["IdleTimeout", function (IdleTimeout) {
    return {
        restrict: "AC",
        controller: function ($scope) {
            $scope.msg = "";
            $scope.timer = null;
            $scope.active = false;
            $scope.startCountdown = function (timer) {
                //alert("timer is running");
                $scope.timer = new IdleTimeout(30000, $scope.cancelCountdown);
                $scope.timer.startCountdown();
                $scope.msg = "Timer is running";
                $scope.active = true;
            };
            $scope.cancelCountdown = function () {
                //alert("timer is stopped");
                $scope.showBanner();
                console.log("app has gone idle");

                $scope.timer.cancelCountdown();
                $scope.msg = "Timer has stopped.";
                $scope.active = false;
            };
        },
        link: function ($scope, $el, $attrs) {
            $scope.startCountdown();
        }
    };
}]);
app.directive("editorDirective", ["$rootScope", function ($rootScope) {
    return function (scope, element, attrs) {
        if (scope.$last) {
            setTimeout(function () {
                $("#select-violation, #select-resolution").selectpicker("refresh");
                setTimeout(function () {
                    $("#defaultModal li[data-original-index='0'], #defaultModal .bootstrap-select.btn-group .dropdown-menu li.divider:first").remove();
                }, 500);
            }, 1000);
        }
    };
}]);
app.directive("fileUploadOnChange", function () {
    return {
        restrict: "A",
        link: function (scope, element, attrs) {
            var onChangeHandler = scope.$eval(attrs.fileUploadOnChange);
            element.bind("change", onChangeHandler);
        }
    };
});
app.directive("triggerOnChange", function () {
    return {
        restrict: "A",
        link: function (scope, element, attrs) {
            var onChangeHandler = scope.$eval(attrs.triggerOnChange);
            element.bind("change", onChangeHandler);
        }
    };
});
app.directive("myselect", function ($parse) {
    return {
        restrict: "A",
        require: "select",
        link: function (scope, element, attrs) {
            var ngModel = $parse(attrs.ngModel);

            $(element).selectmenu({
                "change": function (event, ui) {
                    scope.$apply(function (scope) {
                        // Change binded variable
                        ngModel.assign(scope, $(element).val());
                    });
                },
                "width": 200
            });

            var selectSource = jQuery.trim(attrs.ngOptions.split("|")[0]).split(" ").pop();
            var scopeCollection = $parse(selectSource);

            scope.$watchCollection(scopeCollection, function (newVal) {
                $(element).selectmenu("refresh");
            });

            $("#defaultModal li[data-original-index='0'], #defaultModal .bootstrap-select.btn-group .dropdown-menu li.divider:first").remove();
        }
    };
});
app.directive("momentInput", [
    function () {
        var dateFormat = "DD/MM/YYYY HH:mm";
        return {
            require: "ngModel",
            link: function (scope, element, attrs, ngModelController) {
                ngModelController.$parsers.push(function (data) {
                    //convert data from view format to model format
                    if (data != null) {
                        data = moment(data, dateFormat).format();
                    }
                    return data;
                });

                ngModelController.$formatters.push(function (data) {
                    //convert data from model format to view format
                    if (data != null) {
                        data = moment(data).format(dateFormat);
                    }
                    return data;
                });
            }
        };
    }
]);
app.directive("ngExcelSheetImport", function () {
    return {
        scope: { opts: "=" },
        link: function (scope, elm, attr) {
            elm.on("change", function (changeEvent) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    /* read workbook */
                    var bstr = e.target.result;
                    var workbook = XLSX.read(bstr, { type: "binary" });

                    /* DO SOMETHING WITH workbook HERE */
                    var sheetName = workbook.SheetNames[0];
                    var data = XLSX.utils.sheet_to_json(workbook.Sheets[sheetName]);
                    scope.$emit(attr.ngExcelSheetImport, { rows: data });
                };

                reader.readAsBinaryString(changeEvent.target.files[0]);
            });
        }
    };
});

app.directive("numbersOnly", function () {
    return {
        require: "ngModel",
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9]/g, "");

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
}); 
app.directive("decimalOnly", function () {
    return {
        restrict: "A",
        link: function ($scope, $element, $attributes) {
            var limit = $attributes.ngDecimal;
            function caret(node) {
                if (node.selectionStart) {
                    return node.selectionStart;
                }
                else if (!document.selection) {
                    return 0;
                }

                var c = "\001";
                var sel = document.selection.createRange();
                var txt = sel.text;
                var dul = sel.duplicate();
                var len = 0;
                try { dul.moveToElementText(node); } catch (e) { return 0; }
                sel.text = txt + c;
                len = (dul.text.indexOf(c));
                sel.moveStart("character", -1);
                sel.text = "";
                return len;
            }

            $element.bind("keypress", function (event) {
                var charCode = (event.which) ? event.which : event.keyCode;
                if (charCode == 45) {
                    event.preventDefault();
                    return false;
                }
                if (charCode == 46) {
                    if ($element.val().length > limit - 1) {
                        event.preventDefault();
                        return false;
                    }
                    if ($element.val().indexOf(".") != -1) {
                        event.preventDefault();
                        return false;
                    }
                    return true;
                }
                if (charCode != 45 && charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                    event.preventDefault();
                    return false;
                }
                if ($element.val().length > limit - 1) {
                    event.preventDefault();
                    return false;
                }
                return true;
            });
        }
    };
}); 
app.directive("alphaOnly", function () {
    return {
        require: "ngModel",
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                var transformedInput = text.replace(/[^a-zA-Z]/g, "");

                if (transformedInput !== text) {
                    ngModelCtrl.$setViewValue(transformedInput);
                    ngModelCtrl.$render();
                }
                return transformedInput;
            }
            ngModelCtrl.$parsers.push(fromUser);
            
            $element.bind("keypress", function (event) {
                var charCode = (event.which) ? event.which : event.keyCode;
                console.log("charCode: ", charCode);
                //if (charCode == 45) {
                //    event.preventDefault();
                //    return false;
                //}
                //if (charCode == 46) {
                //    if ($element.val().length > limit - 1) {
                //        event.preventDefault();
                //        return false;
                //    }
                //    if ($element.val().indexOf(".") != -1) {
                //        event.preventDefault();
                //        return false;
                //    }
                //    return true;
                //}
                //if (charCode != 45 && charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                //    event.preventDefault();
                //    return false;
                //}
                //if ($element.val().length > limit - 1) {
                //    event.preventDefault();
                //    return false;
                //}
                //return true;
            });
        }
    };
}); 
app.directive("alphaNumbers", function () {
    return {
        require: "ngModel",
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                text = (text || "").toUpperCase();
                var transformedInput = text.replace(/[^a-zA-Z0-9]/g, "");

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
}); 
app.directive("alphaNumbersWithSpaces", function () {
    return {
        require: "ngModel",
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                var transformedInput = text.replace(/[^a-zA-Z 0-9]/g, "");
                if (transformedInput !== text) {
                    ngModelCtrl.$setViewValue(transformedInput);
                    ngModelCtrl.$render();
                }
                return transformedInput;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
}); 
app.directive("alphaSpecialOnly", function () {
    return {
        require: "ngModel",
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^a-zA-Z _\\\/.’'-]/g, "");

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});
app.constant("Settings", {
    Options: {
        Upload: {
            withCredentials: false,
            headers: { "Content-Type": undefined },
            transformRequest: angular.identity
        }
    }
});
app.filter("secondsToDateTime", [
    function () {
        return function (seconds) {
            return new Date(1970, 0, 1).setSeconds(seconds);
        };
    }
]);
app.config([
    "$compileProvider", "$routeProvider", "$locationProvider",
    function ($compileProvider, $routeProvider, $locationProvider) {
        $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|javascript):/);
        $routeProvider
            .when("/",
                {
                    redirectTo: "/App/Index/Main"
                }
            )
            .when("/App/Index/Main",
                {
                    templateUrl: root + "/public/app/views/Assets.html",
                    controller: "AssetController"
                })
            .when("/App/Index/Print",
                {
                    templateUrl: root + "/public/app/views/Print.html",
                    controller: "PrinterController"
                })
            .when("/App/Index/Scan",
                {
                    templateUrl: root + "/public/app/views/Scanner.html",
                    controller: "ScannerController"
                })
            .when("/App/Index/Manage",
                {
                    templateUrl: root + "/public/app/views/Manage.html",
                    controller: "ManageController"
                })
            .otherwise({
                templateUrl: root + "/public/app/views/PageNotFound.html",
            });
        //// configure html5 to get links working on jsfiddle
        //$locationProvider
        //    .html5Mode({
        //        enabled: true,
        //        requireBase: false
        //    });
    }
]);
app.config(function (IdleProvider, KeepaliveProvider, $httpProvider) {
    //this config handles dynamic pages in SPA in $routeProvider using factory -> template404Interceptor
    $httpProvider.interceptors.push('templateInterceptor');

    //this handles idle message
    KeepaliveProvider.interval(10);
    IdleProvider.windowInterrupt("focus");
});
app.run();