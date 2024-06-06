app.factory("SignalRFactory", SignalRFactory);
function SignalRFactory() {
    return function() {
        this.hub = "";

        this.url = function(url) {
            $.connection.hub.url = url;
        };

        this.setHubName = function(hubName) {
            this.hub = hubName;
        };

        this.connectToHub = function() {
            return $.connection[this.hub];
        };

        this.client = function() {
            var hub = this.connectToHub();
            return hub.client;
        };

        this.server = function() {
            var hub = this.connectToHub();
            return hub.server;
        };

        this.start = function(fn) {
            return $.connection.hub.start({ transport: "longPolling" }).done(fn);
        };

        this.disconnected = function(fn) {
            return $.connection.hub.disconnected(fn);
        };

        this.reconnecting = function(fn) {
            return $.connection.hub.reconnecting(fn);
        };

        this.reconnected = function(fn) {
            return $.connection.hub.reconnected(fn);
        };
    };
}