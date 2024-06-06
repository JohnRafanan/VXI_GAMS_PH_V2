app.service("ToastService", ToastService);
function ToastService() {
    this.info = function (message) {
        return swal("Information", message, "info");
    };
    
    this.error = function (message) {
        return swal("Error", message, "error");
    };

    this.success = function (message) {
        return swal("Success", message, "success");
    };

    this.warning = function (message) {
        return swal("Warning", message, "warning");
    };
    

    this.info2 = function (title, message) {
        return swal(title, message, "info");
    };

    this.error2 = function (title, message) {
        return swal(title, message, "error");
    };

    this.success2 = function (title, message) {
        return swal(title, message, "success");
    };

    this.warning2 = function (title, message) {
        return swal(title, message, "warning");
    };
}