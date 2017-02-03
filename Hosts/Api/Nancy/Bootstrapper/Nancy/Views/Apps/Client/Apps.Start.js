$(document).ready(function () {
    ko.applyBindings(new AppsViewModel(new AppsModel()), $("#Apps")[0]);
});