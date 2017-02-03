function AppsModel() {
    var self = this;
    var rootUrl = "/Notifications/Dashboard/Apps";

    self.List = function (page, pageSize) {
        return $.get(rootUrl);
    };

    self.GenerateToken = function (appName) {
        return $.ajax({
            url: rootUrl + "/" + appName + "/Token",
            method: "PUT"
        });
    };
}