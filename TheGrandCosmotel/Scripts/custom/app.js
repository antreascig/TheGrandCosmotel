function ViewModel() {
    var self = this;

    var UserDetailToken = 'userDetailToken';

    self.result = ko.observable();
    self.user = ko.observable();

    self.AntiForgeToken = 
    self.RegisterModel = {
        UserName: ko.observable(),
        Password: ko.observable(),
        ConfirmPassword: ko.observable(),
        FullName: ko.observable(),
        Email: ko.observable(),
        SecondaryEmail: ko.observable(),
        Shop: ko.observable(),
        MaritalStatus: ko.observable(),
        Hobby: ko.observable(),
        Avatar: ko.observable()
    };

    self.LoginModel = {
        UserName: ko.observable(),
        Password: ko.observable(),
        RememberMe: ko.observable()
    };

    self.loginPassword = ko.observable();
    self.errors = ko.observableArray([]);

    function showError(jqXHR) {

        self.result(jqXHR.status + ': ' + jqXHR.statusText);

        var response = jqXHR.responseJSON;
        if (response) {
            if (response.Message) self.errors.push(response.Message);
            if (response.ModelState) {
                var modelState = response.ModelState;
                for (var prop in modelState) {
                    if (modelState.hasOwnProperty(prop)) {
                        var msgArr = modelState[prop]; // expect array here
                        if (msgArr.length) {
                            for (var i = 0; i < msgArr.length; ++i) self.errors.push(msgArr[i]);
                        }
                    }
                }
            }
            if (response.error) self.errors.push(response.error);
            if (response.error_description) self.errors.push(response.error_description);
        }
    }

    //self.register = function () {
    //    self.result('');
    //    self.errors.removeAll();

    //    var data = ko.toJS(self.RegisterModel);

    //    $.custom.Server["SendRequest"]('POST', '/account/Register', data,
    //        function (res)  { // success
    //            self.user(data.UserDetails.UserName);
    //            localStorage.setItem(UserDetailToken, data.UserDetails);
    //        }, showError);
    //};

    //self.login = function () {
    //    self.result('');
    //    self.errors.removeAll();

    //    var data = ko.toJS(self.LoginModel);

    //    $.custom.Server["SendRequest"]('POST', '/account/login', data,
    //        function (res)  { // success
    //            console.log(res);
    //            //self.user(data.UserDetails.UserName);
    //            //localStorage.setItem(UserDetailToken, data.UserDetails);
    //        }, showError,true);
    //};

    //self.logout = function () {
    //    var logoutData = { id: "logoutForm" };

    //    $.custom.Server["SendRequest"]('POST', '/Account/LogOff', logoutData,
    //        function (res)  { // success
    //        }, showError, true);
    //};
}

var app = new ViewModel();
ko.applyBindings(app);