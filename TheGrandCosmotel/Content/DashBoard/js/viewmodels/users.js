function ViewModel() {

    var vm = {
        RefreshUsers: RefreshUsers,
        EditPlayer: EditPlayer,
        InitDatable: InitDatable,
        SelectedUser: ko.observable(),
        CancelEditing: CancelEditing,
        SaveUser: SaveUser,
        ResetTokens: ResetTokens,
        resetTime: resetTime,
        reSendConfirmation: reSendConfirmation
    };

    var columns = [
        { "title": "Id", "visible": false, "searchable": false, "target": 0 },
        { "title": "Όνομα", "searchable": true, "target": 1 },
        { "title": "Email", "searchable": true, "target": 2 },
        { "title": "UserName", "searchable": true, "target": 3 },
        { "title": "Κατάστημα", "searchable": true, "target": 4 },
        {
            "title": "Ρυθμίσεις", "searchable": false, "target": 5, createdCell: function (td, cellData, rowData, row, col) {
                // debugger;
                var html = '<button type="button" class="btn btn-warning" onclick="newView.EditPlayer(' + row + ')">Edit</button>';
                $(td).html(html);
            }
        }
    ];
    var table;

    function InitDatable() {
        table = $('#datatable-users').DataTable({
            // "ajax": "/Dashboard/" + view.url,
            "ajax": "/Dashboard/GetUsers",
            "columns": columns
        });
    }

    // General Game Settings
    function RefreshUsers() {
        table.ajax.reload();
    }

    function EditPlayer(rowIndex) {
        var data = table.row(rowIndex).data();
        var id = data[0];
        var name = data[1];
        $('#modalUserName').text(name);

        $.custom.Server["SendRequest"]("GET", "/Dashboard/GetUserDetails", { userId: id },
            function (res) { //success
                // debugger
                if (res && res.success) {
                    var gs = [];
                    for (var key in res.Games) {
                        res.Games[key].Key = key;
                        res.Games[key].Default = res.Games[key].Tokens;
                        var obs = ko.mapping.fromJS(res.Games[key]);
                        gs.push(obs);
                    }

                    var PlayTimeToday = parseInt(res.PlayTimeToday / 60) + ' λεπτά ' + (res.PlayTimeToday % 60) + ' δευτερόλεπτα';

                    var isConfirmed = res.isConfirmed || false;

                    vm.SelectedUser({
                        UserId: id,
                        GameScores: gs,
                        PlayTimeToday: ko.observable(PlayTimeToday),
                        isConfirmed: ko.observable(isConfirmed)
                    });
                }
            },
            function (error, hrx, code) { });
        $('#userEditorModal').modal();
    }

    function CancelEditing() {
        vm.SelectedUser(null);
    }

    function ResetTokens(GameData) {
        GameData.Tokens(GameData.Default());
    }

    function SaveUser() {
        var User = vm.SelectedUser();
        var gameTokens = {};
        for (var i = 0; i < User.GameScores.length; i++) {
            var gameData = User.GameScores[i];
            gameTokens[gameData.Key()] = gameData.Tokens();
        }
        $.custom.Server["SendRequest"]("POST", "/Dashboard/SaveUserDetails",
            {
                userId: User.UserId,
                gameTokensJSON: JSON.stringify(gameTokens),
                isConfirmed: User.isConfirmed()
        },
            function (res) { //success
                // debugger
                if (res.success) {
                    $.custom['Logger'].Success("Τα Tokens Αποθηκεύτηκαν", "");
                }
                else {
                    $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "");
                }
            },
            function (error, hrx, code) { $.custom['Logger'].Error("Κατι δεν πηγε σωστά", ""); });
        vm.SelectedUser(null);

    }

    function resetTime() {
        var User = vm.SelectedUser();
        $.custom.Server["SendRequest"]("POST", "/Dashboard/ResetGameTime", { userId: User.UserId },
            function (res) { //success
                // debugger
                if (res.success) {
                    $.custom['Logger'].Success("Ο χρόνος διαγράφηκε", "");
                    User.PlayTimeToday('0 λεπτά 0 δευτερόλεπτα');
                }
                else {
                    $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "");
                }
            },
            function (error, hrx, code) { $.custom['Logger'].Error("Κατι δεν πηγε σωστά", ""); });
    }

    function reSendConfirmation() {

    }

    return vm;
}

var newView = new ViewModel();
// Init
$(document).ready(function () {
    newView.InitDatable();
    ko.applyBindings(newView);
});
