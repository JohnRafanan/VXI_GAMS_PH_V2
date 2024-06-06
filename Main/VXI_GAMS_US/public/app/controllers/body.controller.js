app.controller("bodyCtrl", bodyCtrl);
bodyCtrl.$inject = ["$rootScope", "$scope", "$http", "DTOptionsBuilder", "DTColumnBuilder", "ApiFactory", "$timeout", "Idle", "SignalRFactory"];
function bodyCtrl($rootScope, $scope, $http, DTOptionsBuilder, DTColumnBuilder, ApiFactory, $timeout, Idle, SignalRFactory) {
    var vm = this;
    var hub = new SignalRFactory();
    hub.setHubName("globalHub");
    $scope.idle = 5;
    $scope.timeout = 1200;//1200->20min

    $scope.$watch("idle", function (value) {
        if (value !== null) Idle.setIdle(value);
    });

    $scope.$watch("timeout", function (value) {
        if (value !== null) Idle.setTimeout(value);
    });
    
    function removeDuplicates(ar) {
        var j = {};

        ar.forEach( function(v) {
            j[v+ '::' + typeof v] = v;
        });

        return Object.keys(j).map(function(v){
            return j[v];
        });
    } 

    $scope.$on("IdleTimeout", function () {
        swal({
            closeOnEsc: false,
            closeOnClickOutside: false,
            title: "You have been Idle for too long",
            text: "The page will be redirected to logon page.",
            buttons: false,
            dangerMode: false,
            timer: 5000
        }).then(function () {
            $("#logoutForm")[0].submit();
        });
    });

    vm.apply = function () {
        try {
            $scope.$apply();
        } catch (e) {
            //ignore
        }
    };

    try {
        hub.client().setCount = function (arr) {
            arr = arr || [];
            console.log("Users:", removeDuplicates(arr));
        };
    } catch (setCountError) {
        console.log("hub.client().setCount: ", setCountError);
    }

    $scope.getCount = function () {
        try {
            hub.server().getCount();
        } catch (getCountError) {
            console.log("hub.server().getCount(): ", getCountError);
        }
    };

    $scope.reloadOtherUsers = function () {
        try {
            hub.server().reloadPage();
        } catch (reloadPage) {
            console.log("hub.server().reloadPage(): ", reloadPage);
        }
    };

    vm.getVersion = function () {
        try {
            hub.server().getVersion();
        } catch (reloadPage) {
            console.log("hub.server().reloadPage(): ", reloadPage);
        }
    };

    try {
        hub.client().reloadPages = function () {
            swal({
                closeOnEsc: false,
                closeOnClickOutside: false,
                title: "Refreshing",
                text: "Please Wait...",
                buttons: false,
                dangerMode: false,
                timer: 2500
            }).then(function () {
                javascript: document.getElementById("logoutForm").submit();
            });
        };
    } catch (reloadPages) {
        console.log("hub.client().reloadPages(): ", reloadPages);
    }

    try {
        hub.client().getVersion = function (v) {
            //console.log("matching client version to server version");
            if (iv !== v) {
                console.log("version mismatched");
                swal({
                    closeOnEsc: false,
                    closeOnClickOutside: false,
                    title: "The tool has been updated",
                    text: "The page will be refreshed in 3 seconds",
                    buttons: false,
                    dangerMode: false,
                    timer: 3000
                }).then(function () {
                    javascript: document.getElementById("logoutForm").submit();
                });
            } else {
                //console.log("version matched");
                setTimeout(function () {
                    try {
                        vm.getVersion();
                    } catch (e) {
                        console.log("hub.server().getVersion(): ", e);
                    }
                }, 10000);
            }
        };
    } catch (getVersion) {
        console.log("hub.client().getVersion(): ", getVersion);
    }

    var tryingToReconnect = false;
    try {

        hub.reconnecting(function () {
            tryingToReconnect = true;
        });

        hub.reconnected(function () {
            tryingToReconnect = false;
            try {
                vm.getVersion();
            } catch (e) {
                console.log("hub.server().getVersion(): ", e);
            }
        });
    } catch (e) {
        console.log("reconnecting to hub server error: ", e);
    }

    try {
        hub.disconnected(function () {
            if (tryingToReconnect) {
                console.log("disconnected to hub server");
                setTimeout(function () {
                    console.log("connecting again to hub server");
                    vm.startHub();
                }, 5000);
            }
        });
    } catch (ee) {
        console.log("hub.disconnected: ", ee);
    }

    vm.startHub = function () {
        try {
            hub.start(function () {
                console.log("connected to hub server");
                console.log("get server version");
                try {
                    vm.getVersion();
                } catch (e) {
                    console.log("hub.server().getVersion(): ", e);
                }
            });
        } catch (e) {
            console.log("hub.start: ", e);
        }
    };

    $rootScope.checkUrl = function() {
        if (window.location.pathname.indexOf("/Home/Assets/assets") > -1) {
            $rootScope.pageSelected = "assets";
        } else if (window.location.pathname.indexOf("/Home/Assets/transactions") > -1) {
            $rootScope.pageSelected = "transactions";
        } else if (window.location.pathname.indexOf("/Home/Assets/approvals") > -1) {
            $rootScope.pageSelected = "approvals";
        }
    };

    $rootScope.pageSelected = "";
    vm.startIdleTimeout = function () {
        vm.startHub();
        Idle.watch();
        $rootScope.checkUrl();
        console.log("watching...");
    };
    vm.startIdleTimeout();
}