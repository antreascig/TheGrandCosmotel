function ViewModel() {

    var vm = {
        GameQuestions: ko.observableArray([]),
        GetGameQuestions: GetGameQuestions,
        EditQuestion: EditQuestion,
        RemoveQuestion: RemoveQuestion,
        SelectedQuestion: ko.observable(),
        AddQuestion: AddQuestion,
        AddOption: AddOption,
        RemoveOption: RemoveOption,
        SaveQuestion: SaveQuestion,
        SaveQuestions: SaveQuestions,
        CancelQuestion: CancelQuestion,
        InitDatable: InitDatable,
        RefreshGames: RefreshGames
    };

    // Game Question Settings
    function GetGameQuestions() {
        $.custom.Server["SendRequest"]("POST", "/Dashboard/GetGameQuestions", {},
            function (res) { //success
                if (res.success) {
                    var data = ko.mapping.fromJS((res.data || {}));
                    vm.GameQuestions(data());
                    if (table == null) {
                        vm.InitDatable();
                    } else {
                        table.ajax.reload();
                    }
                }
            },
            function (error, hrx, code) { });
    };

    function EditQuestion(QuestionId) {
        var question = null;
        var tmp = vm.GameQuestions() || [];
        var newList = [];
        for (var i = 0; i < tmp.length; i++) {
            if (tmp[i].QuestionId() == QuestionId)
                question = tmp[i];
        }

        if (!question) return;

        // debugger
        var tmpQ = ko.mapping.fromJS(ko.toJS(question));
        vm.SelectedQuestion(tmpQ);
        vm.SelectedQuestion.Question = question;
    };

    function RemoveQuestion(QuestionId, rowIndex) {
        var tmp = vm.GameQuestions() || [];
        var newList = [];
        for (var i = 0; i < tmp.length; i++) {
            if (tmp[i].QuestionId() != QuestionId)
                newList.push(tmp[i]);
        }
        vm.GameQuestions(newList);

        //debugger;
        //table.draw();

        //table.row(rowIndex).remove().draw();
        vm.RefreshGames();
    };

    function AddQuestion() {
         //debugger
        var id = null;
        var UniqueId = false;
        var questions = vm.GameQuestions() || [];
        do {
            id = GetRandomNumber(1, 1000);
            UniqueId = true;
            for (var i = 0; i < questions.length; i++) {
                if (id == questions[i].QuestionId()) {
                    UniqueId = false;
                    break;
                }
            }
        } while (!UniqueId)

        var newQuestion = { QuestionId: ko.observable(id), QuestionText: ko.observable(''), Options: ko.observableArray([]), AnswerIndex: ko.observable(1), Active: ko.observable(true) };
        questions.push(newQuestion);
        vm.GameQuestions(questions);

        vm.EditQuestion(id);

        vm.RefreshGames();

    };

    function SaveQuestion() {
        // debugger
        var sq = vm.SelectedQuestion();
        var model = vm.SelectedQuestion.Question;
        for (var prop in sq) {
            if (model[prop]) {
                model[prop](sq[prop]());
            }
        }
        $('#questionEditorModal').modal('hide');
        vm.SelectedQuestion(null);
        var tmp = vm.GameQuestions();
        for (var i = 0; i < tmp.length; i++) {
            if (tmp[i].QuestionId() == model.QuestionId()) {
                return;
            }
        }
        tmp.push(model);
        vm.GameQuestions(tmp);

        vm.RefreshGames();
    };

    function CancelQuestion() {
        vm.SelectedQuestion(null);
    };

    function SaveQuestions() {
        var questions = ko.mapping.toJS(vm.GameQuestions);

        $.custom.Server["SendRequest"]("POST", "/Dashboard/SaveGame5Questions", { questions: questions },
            function (res) { //success
                if (res.success) {
                    $.custom['Logger'].Success("Οι ερωτήσεις αποθηκεύτηκαν.", "");
                }
                else {
                    $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "");
                }
            },
            function (error, hrx, code) { $.custom['Logger'].Error("Κατι δεν πηγε σωστά", "") });
    };

    function AddOption() {
        // debugger;
        var options = vm.SelectedQuestion().Options();
        options.push('Option' + (options.length + 1));
        vm.SelectedQuestion().Options(options);
    };

    function RemoveOption(index) {
        var options = vm.SelectedQuestion().Options();

        options.splice(index, 1);

        var selQ = vm.SelectedQuestion();
        if ((index + 1) == selQ.AnswerIndex()) {
            selQ.AnswerIndex(1);
        }
        selQ.Options(options);
    };

    var columns = [
        { "data": "QuestionId", "title": "ID", "searchable": false },
        { "data": "QuestionText", "title": "Ερώτηση", "searchable": true },
        {
            "data": "Active", "title": "Ρυθμίσεις", "searchable": false, createdCell: function (td, cellData, rowData, row, col) {
                 //debugger;
                var QuestionId = rowData["QuestionId"]();
                var html = '<button type="button" class="btn btn-warning" data-toggle="modal" data-target="#questionEditorModal" onclick="newView.EditQuestion(' + QuestionId + ')">Αλλαγή</button>';
                html += '<button type= "button" class="btn btn-danger" onclick="newView.RemoveQuestion(' + QuestionId + ', ' + row + ')" > Διαγραφή</button>';
                $(td).html(html)
            }
        },
    ];
    var table;

    function RefreshGames() {
        table.ajax.reload();
    };

    function InitDatable() {
        table = $('#datatable-questions').DataTable({
            // "ajax": "/Dashboard/" + view.url,
            //"data": vm.GameQuestions(),
            "ajax" : function (data, callback, settings) {
                callback({ data: vm.GameQuestions() });
            },
            "columns": columns
        });
    };

    return vm;
};

var newView = new ViewModel();

// Init
newView.GetGameQuestions();
//newView.InitDatable();

ko.applyBindings(newView);