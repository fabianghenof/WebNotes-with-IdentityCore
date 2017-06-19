$(document).ready(function () {

    var $noteTitle = $('#note-title');
    var $noteContent = $('#note-content');
    var $colorButton = $('#color-button');
    var $colorButtonText = $('#color-button-text');
    var panelColorClass = 'panel-success';

    function WebNotesViewModel() {
        //Properties
        var self = this;
        self.emailVisible = ko.observable(false);
        self.emailToSendTo = ko.observable();
        self.webNotesData = ko.observable();
        self.noteToEmail = ko.observable();

        //Functions
        self.getWebNotesData = function (webnotes)
        {
            $.get('getWebNotes', { webnotes: webnotes }, self.webNotesData);
        };
        self.submitNote = function () {
            var newNote = new Note();

            if (!$noteContent.val()) {
                $noteContent.addClass('alert-danger');
                toastr.warning("WebNote content empty!");
            }
            else {
                if (!$noteTitle.val()) {
                    $noteTitle.val('WebNote');
                }
                $noteContent.removeClass('alert-danger');
                newNote.title = $noteTitle.val();
                newNote.content = $noteContent.val();
                newNote.color = panelColorClass;
                $noteTitle.val('');
                $noteContent.val('');
                $.post("saveNote", newNote).then(function (response) {
                    newNote.id = response.id;
                    self.makeNotePanel(newNote);
                    toastr.success("WebNote added!");
                });
                //location.reload();
            }
        };
        self.makeNotePanel = function (noteToMake) {
            self.getWebNotesData();
        };
        self.toggleAddNoteForm = function () {
            $('#new-note-form').toggleClass('hidden');
        };
        self.noteColor = function (color) {
            switch (color) {
                case 'green':
                    $colorButtonText.text('Green');
                    $colorButton.removeClass('btn-success btn-info btn-warning btn-danger');
                    $colorButton.addClass('btn-success');
                    panelColorClass = "panel-success";
                    break;
                case 'red':
                    $colorButtonText.text('Red');
                    $colorButton.removeClass('btn-success btn-info btn-warning btn-danger');
                    $colorButton.addClass('btn-danger');
                    panelColorClass = "panel-danger";
                    break;
                case 'orange':
                    $colorButtonText.text('Orange');
                    $colorButton.removeClass('btn-success btn-info btn-warning btn-danger');
                    $colorButton.addClass('btn-warning');
                    panelColorClass = "panel-warning";
                    break;
                case 'blue':
                    $colorButtonText.text('Blue');
                    $colorButton.removeClass('btn-success btn-info btn-warning btn-danger');
                    $colorButton.addClass('btn-info');
                    panelColorClass = "panel-info";
                    break;
            }
        };
        self.showEmailForm = function () {
            switch (self.emailVisible()) {
                case false:
                    self.emailVisible(true);
                    break;

                case true:
                    self.emailVisible(false);
                    break;
            }
        };
        self.removeNote = function (idToDelete) {
            //alert('test');
            event.preventDefault();
            //delete the note after a fade-out animation
            $.post("deleteNote", { id: idToDelete }).then(function () {
                toastr.error('WebNote ' + idToDelete + ' deleted');
                self.getWebNotesData();
            });
        };
        self.setNoteToEmail = function (id) {
            $.get('getSingleWebNote', { id: id }, self.noteToEmail);
        };
        self.emailNote = function () {
            $.post('/sendEmail', { email: self.emailToSendTo(), note: self.noteToEmail() }).then(
                location.reload()
            );
        };

        self.getWebNotesData();
    }

    //Knockout apply bindings
    ko.applyBindings(new WebNotesViewModel());

});



    
