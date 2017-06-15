var $noteTitle = $('#note-title');
var $noteContent = $('#note-content');
var $colorButton = $('#color-button');
var $colorButtonText = $('#color-button-text');
var panelColorClass = 'panel-success';


function WebNotesViewModel() {
    //Properties
    self = this;
    self.emailVisible = ko.observable(false);
    //Functions
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
    }
    self.makeNotePanel = function (noteToMake) {
        //generate html for note panel
        var $noteHtmlTemplate = $($('#to-do-item-template').html());

        $noteHtmlTemplate.find('.note-title').text(noteToMake.title);
        $noteHtmlTemplate.find('#panel-note-content').text(noteToMake.content);
        $noteHtmlTemplate.find('.delete-note-button').data('id', noteToMake.id);
        $noteHtmlTemplate.find('#panel-note-content').data('id', noteToMake.id);
        $noteHtmlTemplate.find('#note-panel').data('id', noteToMake.id);
        $noteHtmlTemplate.find('#note-panel').data('orderIndex', noteToMake.orderIndex);
        $noteHtmlTemplate.addClass(panelColorClass);

        $('#notes-div').prepend($noteHtmlTemplate);
    }
    self.toggleAddNoteForm = function () {
        $('#new-note-form').toggleClass('hidden');
    }
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
    }
    self.showEmailForm = function () {
        switch (self.emailVisible()) {
            case false:
                self.emailVisible(true);
                break;

            case true:
                self.emailVisible(false);
                break;
        }
    }
    }

//Knockout apply bindings
ko.applyBindings(new WebNotesViewModel());
    



    
