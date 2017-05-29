function AntiForgeryTokenAPI() {

    this.AddAntiForgeryToken = function (data) {
        data.__RequestVerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
        return data;
    };

    this.GetAntiForgeryToken = function (data) {
        return $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
    };

    this.GetAntiForgeryTokenElement = function (data) {
        return $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]');
    };
}

var AntiForgeryToken = new AntiForgeryTokenAPI();

if (!$.custom) {
    $.custom = {};
}

$.custom["AntiForgeryToken"] = AntiForgeryToken;
