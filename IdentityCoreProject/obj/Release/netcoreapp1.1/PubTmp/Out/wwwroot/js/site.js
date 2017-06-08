// Write your Javascript code.
$(document).ready(function () {

    var $noteTitle = $('#note-title');
    var $noteContent = $('#note-content');
    var $colorButton = $('#color-button');
    var $colorButtonText = $('#color-button-text');
    var panelColorClass = 'panel-success';
    var justDeletedNote = localStorage.getItem("justDeletedNote");

    console.log(justDeletedNote);


    function toggleAddNoteForm() {
        $('#new-note-form').toggleClass('hidden');
    }

    function submitNote() {
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
                makeNotePanel(newNote);
                toastr.success("WebNote added!");
            });
            //location.reload();
        }
    }

    function makeNotePanel(noteToMake) {
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

    function deleteNote($this) {
        event.preventDefault();


        //delete the note after a fade-out animation
        var idToDelete = $this.closest('.delete-note-button').data('id');

        $.post("deleteNote", { id: idToDelete }).then(function () {
            $this.closest('#note-panel').fadeOut('fast', function () {
                $this.closest('#note-panel').remove();
                toastr.error('WebNote ' + idToDelete + ' deleted');
            });
        });

    }

    function noteColor(color) {
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


    //edit Note Title
    function editTitle($this) {
        var title = $this.find('.note-title').text();
        $this.find('.note-title').addClass('hidden');
        $this.find('#move-up-symbol').addClass('hidden');
        $this.find('#move-down-symbol').addClass('hidden');
        $this.find('#delete-note-symbol').addClass('hidden');
        $this.find('#edit-title-input').removeClass('hidden');
        $this.find('#edit-title-input').focus();
        $this.find('#edit-title-input').val(title);
    }
    function editTitleFinished($this) {
        var newTitle = $this.find('#edit-title-input').val();
        var idToModifiy = $this.find('.delete-note-button').data('id');
        $this.find('.note-title').removeClass('hidden');
        $this.find('#move-up-symbol').removeClass('hidden');
        $this.find('#move-down-symbol').removeClass('hidden');
        $this.find('#delete-note-symbol').removeClass('hidden');
        $this.find('#edit-title-input').addClass('hidden');
        $this.find('.note-title').text(newTitle);
        $.post('updateNoteTitle', { id: idToModifiy, title: newTitle }).then(function () {
            toastr.success('WebNote title modified!');
        });
    }

    //edit Note Content                                                                                      
    function editContent($this) {
        var content = $this.find('#panel-note-content').text();
        $this.find('#panel-note-content').addClass('hidden');
        $this.parent().find('#move-up-symbol').addClass('hidden');
        $this.parent().find('#move-down-symbol').addClass('hidden');
        $this.parent().find('#delete-note-symbol').addClass('hidden');
        $this.find('#edit-content-input').removeClass('hidden');
        $this.find('#edit-content-input').focus();
        $this.find('#edit-content-input').val(content);
    }
    function editContentFinished($this) {
        var newContent = $this.find('#edit-content-input').val();
        var idToModifiy = $this.data('id');
        $this.find('#panel-note-content').removeClass('hidden');
        $this.parent().find('#move-up-symbol').removeClass('hidden');
        $this.parent().find('#move-down-symbol').removeClass('hidden');
        $this.parent().find('#delete-note-symbol').removeClass('hidden');
        $this.find('#edit-content-input').addClass('hidden');
        $this.find('#panel-note-content').text(newContent);
        $.post('updateNoteContent', { id: idToModifiy, content: newContent }).then(function () {
            toastr.success('WebNote content modified!');
        });
    }


    function moveNoteUp($this) {
        var idOfClickedNote = $this.closest('#note-panel').data('id');
        var idOfAboveNote = $this.closest('#note-panel').prev().data('id');
        $.post('moveNoteUp', { idOfClickedNote: idOfClickedNote, idOfAboveNote: idOfAboveNote }).then(function () {
            location.reload();
        });
    }

    function moveNoteDown($this) {
        var idOfClickedNote = $this.closest('#note-panel').data('id');
        var idOfBelowNote = $this.closest('#note-panel').next().data('id');
        $.post('moveNoteDown', { idOfClickedNote: idOfClickedNote, idOfBelowNote: idOfBelowNote }).then(function () {
            location.reload();
        });
    }

    function sendEmail() {
        var emailToSendTo = $('#email-input').val();
        $.post('sendEmail', { email: emailToSendTo });
    }

    $('#add-note-btn').on('click', toggleAddNoteForm);
    $('#submit-note-btn').on('click', submitNote);
    $noteContent.on('keypress', function () { $noteContent.removeClass('alert-danger'); });

    $('#notes-div').on('click', '#delete-note-symbol', function (evt) {
        evt.stopPropagation();
        deleteNote($(this));
    });

    //Note color picker dropdown options
    $('#selected-green').on('click', function () {
        noteColor('green');
    });
    $('#selected-blue').on('click', function () {
        noteColor('blue');
    });
    $('#selected-orange').on('click', function () {
        noteColor('orange');
    });
    $('#selected-red').on('click', function () {
        noteColor('red');
    });


    $('#notes-div').on('click', '.panel-heading', function () {
        editTitle($(this));
    });
    $('#notes-div').on('focusout', '.panel-heading', function () {
        editTitleFinished($(this));
    });
    $('#notes-div').on('click', '.panel-body', function () {
        editContent($(this));
    });
    $('#notes-div').on('focusout', '.panel-body', function () {
        editContentFinished($(this));
    });

    $('#notes-div').on('click', '#move-up-symbol', function (evt) {
        evt.stopPropagation();
        moveNoteUp($(this));
    });

    $('#notes-div').on('click', '#move-down-symbol', function (evt) {
        evt.stopPropagation();
        moveNoteDown($(this));
    });

    $('#download-notes').on('click', downloadNotes);

});

