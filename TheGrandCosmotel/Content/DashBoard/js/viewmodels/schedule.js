

function ViewModel() {

    var vm = {
        init: init,
        RefreshSchedule: RefreshSchedule,
        Games: ko.observableArray([]),
        ScheduleDays: ko.observableArray(),
        SaveSchedule: SaveSchedule,
        AddDay: AddDay,
        EditDay: EditDay,
        RemoveDay: RemoveDay,
        SelectedDay: ko.observable(),
        CancelDay: CancelDay,
        SaveDay: SaveDay,
    };

    // General Game Settings
    function init() {
        vm.RefreshSchedule();
        GetGames();
    };

    function RefreshSchedule() {
        $.custom.Server["SendRequest"]("POST", "/Dashboard/GetSchedule", {},
            function (res) { //success
                // debugger

                if (res.success) {
                    var data = ko.mapping.fromJS((res.data || {}));
                    vm.ScheduleDays(data());
                }
            },
            function (error, hrx, code) { });
    };

    function GetGames() {
        $.custom.Server["SendRequest"]("POST", "/Dashboard/GetGameSettings", {},
            function (res) { //success
                // debugger
                if (res.success) {
                    // debugger
                    var data = [];
                    for (var i = 0; i < res.data.length; i++) {
                        var tag = { id: res.data[i].GameKey, text: res.data[i].Name };
                        data.push(tag);
                    }
                    vm.Games(data);
                }
            },
            function (error, hrx, code) { });
    };

    function SaveSchedule() {
        var schedule = ko.mapping.toJS(vm.ScheduleDays);
        $.custom.Server["SendRequest"]("POST", "/Dashboard/SaveSchedule", { scheduleJSON: encodeURIComponent( JSON.stringify(schedule) ) },
            function (res) { //success

                if (res.success) {
                    $.custom['Logger'].Success("Το πρόγραμμα παιχνιδιών αποθηκεύτηκε.", "");
                }
                else {
                    $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "");
                }
            },
            function (error, hrx, code) { $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "") });
    };

    function AddDay() {
        var newDay = {
            Day: ko.observable(),
            GameKey: ko.observable(),
            SuccessMessage: ko.observable(),
            FailMesssage: ko.observable(),
            OutOfTimeMessage: ko.observable(),
        };
        EditDay(newDay, true);
    };

    function EditDay(ScheduleDay, isNew) {
        var toEdit = ScheduleDay;
        if (!isNew) {
            toEdit = ko.mapping.fromJS(ko.toJS(ScheduleDay));
            toEdit.Existing = ScheduleDay;
            ScheduleDay.Editing = true;
        }

        vm.SelectedDay(toEdit);

        var day = moment();
        if (toEdit.Day()) {
            var stringDay = toEdit.Day();
            day = moment(stringDay);
            $('#single_cal4').val(stringDay);
        }

        $('#single_cal4').daterangepicker({
            startDate: day,
            singleDatePicker: true,
            calender_style: "picker_4"
        }, function (start, end, label) {
            var value = start.format("YYYY-MM-DD");
            var tmp = vm.SelectedDay();
            tmp.Day(value);
            console.log('Day: ', value);
        });
    };

    function RemoveDay(Day) {
        var tmp = vm.ScheduleDays() || [];
        var newList = [];
        for (var i = 0; i < tmp.length; i++) {
            if (tmp[i].Day != Day)
                newList.push(tmp[i]);
        }
        vm.ScheduleDays(newList);
    };

    function CancelDay() {
        vm.SelectedDay(null);
    };

    function SaveDay() {
        var value = vm.SelectedDay();
        value.GameName = ko.observable("");

        if (!value.Day()) {
            alert("Πρέπει να διαλέξεις ημερομηνία")
            return;
        }

        if (!value.GameKey()) {
            alert("Πρέπει να διαλέξεις παιχνίδι")
            return;
        }

        var counter = 0;
        var tmp = vm.ScheduleDays();
        for (var i = 0; i < tmp.length; i++) {
            if (!tmp[i].Editing && tmp[i].Day() == value.Day()) counter++;
        }

        if (counter > 0) {
            alert("H επιλεγμένη ημερομηνία υπάρχει ήδη. Παρακαλώ διάλεξε άλλη ημερομηνία ή διάγραψε την υπάρχουσα πρώτα.")
            return;
        }

        $('#scheduleEditor').modal('hide');
        vm.SelectedDay(null);

        var gameList = vm.Games();

        for (var i = 0; i < gameList.length; i++) {
            if (gameList[i].id == value.GameKey()) {
                value.GameName(gameList[i].text);
                break;
            }
        }
        if (value.Existing) {
            var model = ko.mapping.toJS(value);
            for (var prop in model) {
                if (prop == "Editing") continue;
                value.Existing[prop](model[prop]);
            }
            value.Existing.Editing = false;
        }
        else {

            tmp.push(value);
            vm.ScheduleDays(tmp);
        }
    };

    return vm;
};

var newView = new ViewModel();

// Init
newView.init();


ko.applyBindings(newView);