function AppsViewModel(model) {
    var self = this;
    var model = model;
    
    self.Apps = ko.observableArray([]);

    self.List = function () {
        model.List()
            .done(function (data) {
                var apps = $.map(data.Applications, function (app) {
                    return new AppViewModel(app);
                });

                self.Apps(apps);
            });
    };

    self.GenerateToken = function(item){
        model.GenerateToken(item.Name())
            .done(self.List);
    };


    self.List();
}

function AppViewModel(data) {
    var self = this;

    self.Name = ko.observable(data.Name);
    self.Description = ko.observable(data.Description);
    self.Token = ko.observable(data.Token);
}