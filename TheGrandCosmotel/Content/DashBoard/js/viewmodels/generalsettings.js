function ViewModel() {

    var vm = {
        Games: ko.observableArray([]),
        RefreshGames: RefreshGames,
        UpdateGame: UpdateGame,
    };

    // General Game Settings
    function RefreshGames() {
        $.custom.Server["SendRequest"]("POST", "/Dashboard/GetGameSettings", {},
            function (res) { //success
                // debugger
                if (res.success) {
                    var data = ko.mapping.fromJS((res.data || []));
                    vm.Games(data());
                }
            },
            function (error, hrx, code) { });
    };

    function UpdateGame(Game) {
        if (!Game.Multiplier) {
            alert('Ο πολλαπλασιαστεις πρέπει να είναι πραγματικός αριθμός μεγαλύτερος του.');
            return;
        }
        var model = ko.mapping.toJS(Game);
        $.custom.Server["SendRequest"]("POST", "/Dashboard/SetGameSettings", { model: model },
            function (res) { //success

                if (res.success) {
                    $.custom['Logger'].Success("Το παιχνίδι αποθηκεύτηκε.", "");
                }
                else {
                    $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "");
                }
            },
            function (error, hrx, code) { $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "") });
    };

    return vm;
};

var newView = new ViewModel();

// Init
newView.RefreshGames();


ko.applyBindings(newView);