function ViewModel() {
    var self = this;

    self.UseSameEmailForSecondary = ko.observable(false);
    self.Email = ko.observable();

    self.UseSameEmailForSecondary.subscribe(function (newValue) {
        if (newValue) {
            $("#SecondaryEmail").val(self.Email())
        }
    });

    self.Email.subscribe(function (newValue) {
        if ( self.UseSameEmailForSecondary() ) {
            $("#SecondaryEmail").val(newValue)
        }
    });
}

var vm = new ViewModel();
ko.applyBindings(vm);