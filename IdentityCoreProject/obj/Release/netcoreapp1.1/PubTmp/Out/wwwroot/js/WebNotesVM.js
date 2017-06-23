﻿$(document).ready(function () {

    var $noteTitle = $('#note-title');
    var $noteContent = $('#note-content');
    var $colorButton = $('#color-button');
    var $colorButtonText = $('#color-button-text');
    var noteColor = '#35d63d';
    var _sortingOption = '';

    function WebNotesViewModel() {
        //Properties
        var self = this;
        self.emailVisible = ko.observable(false);
        self.editVisible = ko.observable(true);
        self.textVisible = ko.observable(true);
        self.emailToSendTo = ko.observable().extend({email: true, required: true});
        self.webNotesData = ko.observable();
        self.noteToEmail = ko.observable();
        self.noteTitle = ko.observable();
        self.noteContent = ko.observable();
        self.sortingOption = ko.observable();
        self.sortedByPriority = ko.observable();
        self.noteToAttachFileTo = ko.observable();
        self.deleteClickedOnce = ko.observable(false);

        self.fileInput = ko.observable();
        self.fileName = ko.observable();
        self.someReader = new FileReader();

        //Functions
        self.initializeMovingArrowsVisibility = function () {

            $.get('getSortingOption', self.sortingOption).then(function () {
                switch (self.sortingOption()) {
                    case 'byPriority':
                        self.sortedByPriority(true);
                        break;
                    case 'byDate':
                        self.sortedByPriority(false);
                        break;
                }
            });
        };
        self.getWebNotesData = function (webnotes) {
            $.get('getWebNotes', { webnotes: webnotes }, function (data) {
                console.clear();
                var observableData = {
                    notes: ko.observableArray(data.notes.map(function (note) {
                        note.isEditable = ko.observable(false);
                        note.deleteClickedOnce = ko.observable(false);
                        console.log(note);
                        return note;
                    }))
                };
                self.webNotesData(observableData);
            });
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
                newNote.color = noteColor;
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
                    noteColor = "#35d63d";
                    break;
                case 'red':
                    $colorButtonText.text('Red');
                    $colorButton.removeClass('btn-success btn-info btn-warning btn-danger');
                    $colorButton.addClass('btn-danger');
                    noteColor = "#FF5858";
                    break;
                case 'orange':
                    $colorButtonText.text('Orange');
                    $colorButton.removeClass('btn-success btn-info btn-warning btn-danger');
                    $colorButton.addClass('btn-warning');
                    noteColor = "#FFA458";
                    break;
                case 'blue':
                    $colorButtonText.text('Blue');
                    $colorButton.removeClass('btn-success btn-info btn-warning btn-danger');
                    $colorButton.addClass('btn-info');
                    noteColor = "#53F1F1";
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
        self.removeNote = function (idToDelete, note) {
            event.preventDefault();
            $('#delete-note-popover').text('?');
            if (note.deleteClickedOnce() === true) {
                $.post("deleteNote", { id: idToDelete }).then(function () {
                    toastr.error('WebNote ' + idToDelete + ' deleted');
                    self.getWebNotesData();
                });
            }
            else {
                note.deleteClickedOnce(true);
                toastr.warning('Click again to confirm deletion');
                setTimeout(function () { note.deleteClickedOnce(false); }, 2000);
            }
        };
        self.setNoteToEmail = function (id) {
            $.get('getSingleWebNote', { id: id }, self.noteToEmail);
        };
        self.setNoteToAttachFileTo = function (id) {
            $.get('getSingleWebNote', { id: id }, self.noteToAttachFileTo);
        };
        self.emailNote = function () {
            $.post('/sendEmail', { email: self.emailToSendTo(), note: self.noteToEmail() }).then(
                location.reload()
            );
        };
        self.EditNote = function (note) {
            self.noteTitle(note.title);
            self.noteContent(note.content);
            note.isEditable(!note.isEditable());
        };
        self.saveNote = function (note) {
            document.body.style.cursor = 'wait';
            $.post('updateNoteContent', { id: note.id, content: self.noteContent() }).then(function () {
                $.post('updateNoteTitle', { id: note.id, title: self.noteTitle() }).then(function () {
                    self.getWebNotesData();
                    document.body.style.cursor = 'default';
                    toastr.success('WebNote successfully updated!');
                });
            });



        };
        self.groupByPriority = function () {
            $.get('getSortingOption', self.sortingOption).then(function () {
                $.post('groupByPriority').then(function () {
                    self.getWebNotesData();
                    switch (self.sortingOption()) {
                        case 'byPriority':
                            toastr.success('Sorting: Manual');
                            self.sortedByPriority(false);
                            break;
                        case 'byDate':
                            toastr.success('Sorting: Priority groups');
                            self.sortedByPriority(true);
                            break;
                        default:
                            toastr.success('Sorting: Priority groups');
                            self.sortedByPriority(true);
                            break;
                    }
                });
            });
        };
        self.moveNoteUp = function (note) {
            $.get('getSortingOption', self.sortingOption).then(function () {
                $.post('moveNoteUp', { idOfClickedNote: note.id, userId: note.userId }).then(function () {
                    self.getWebNotesData();
                }); 
            });
        };
        self.moveNoteDown = function (note) {
            $.post('moveNoteDown', { idOfClickedNote: note.id, userId: note.userId }).then(function () {
                self.getWebNotesData();
            });
        };
        self.uploadFileAttachment = function () {
            $.post('uploadFileAttachment', { noteToAttachTo: self.noteToAttachFileTo(), file: self.fileInput })
                .then(function () { location.reload(); });
        };

        self.initializeMovingArrowsVisibility();
        self.getWebNotesData();
    }

    //Knockout apply bindings
    ko.applyBindings(new WebNotesViewModel());

});



    
