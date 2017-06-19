$(document).ready(function () {

    var $noteTitle = $('#note-title');
    var $noteContent = $('#note-content');
    var $colorButton = $('#color-button');
    var $colorButtonText = $('#color-button-text');
    var panelColorClass = '#55F855';
    var currentTitle = "";
    var currentContent = "";

    function WebNotesViewModel() {
        //Properties
        var self = this;
        self.emailVisible = ko.observable(false);
        self.editVisible = ko.observable(false);
        self.textVisible = ko.observable(true);
        self.emailToSendTo = ko.observable();
        self.webNotesData = ko.observable();
        self.noteToEmail = ko.observable();
        self.noteTitle = ko.observable();
        self.noteContent = ko.observable();

        //Functions
        self.getWebNotesData = function (webnotes) {
            $.get('getWebNotes', { webnotes: webnotes }, function (data) {
                console.log(data);
                var observableData = {
                    notes: ko.observableArray(data.notes.map(function (note) {
                        note.isEditable = ko.observable(false);
                        console.log(note);
                        return note;
                    }))
                };
                self.webNotesData(observableData);
            });
        }
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
                    panelColorClass = "#55F855";
                    break;
                case 'red':;
                    panelColorClass = "#FF5858";
                    break;
                case 'orange':
                    panelColorClass = "#FFA458";
                    break;
                case 'blue':
                    panelColorClass = "#53F1F1";
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
        self.EditNote = function (note) {
            console.log(this);
            currentTitle = self.noteTitle();
            currentContent = self.noteContent();
            self.noteTitle(note.title);
            self.noteContent(note.content);
            note.isEditable(!note.isEditable());
        }
        self.saveNote = function (note) {
            $.post('updateNoteContent', { id: note.id, content: self.noteContent() }).then(function () {
                $.post('updateNoteTitle', { id: note.id, title: self.noteTitle() }).then(function () {
                    self.getWebNotesData();
                    toastr.success('WebNote successfully updated!');
                });
            });
            


        }

        self.getWebNotesData();
    }

    //Knockout apply bindings
    ko.applyBindings(new WebNotesViewModel());

});



    
