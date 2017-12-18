define(['knockout', 'dataservice'], function (ko, dat) {
    return function (params) {
        var self = this;
        
		var postResult = params.name;
		var postMarked = ko.observable(postResult().marked);
        var hasComments = ko.computed(function() {
            return typeof postResult().comments !== "undefined";
        }, this);

        var getId = (url) => {
            return url.substring(url.lastIndexOf("/")+1, url.length);
        };

        this.markPost = function () {
            if (postResult().marked === true) {
				dat.unmarkPost(postResult().id);
				postResult().marked = false;
				postMarked(false);
            } else {
				dat.markPost(postResult().id);
				postResult().marked = true;
				postMarked(true);
            }
        };
        
        this.notes = ko.observableArray();
        var getNotes = function () {
            dat.getPostNotes(postResult(), (result, caller) => {
                caller.notes(result);
            }, self);
        }

        this.newNoteText = ko.observable("");
        var createNote = function () {
            let note = { text: self.newNoteText() };
            dat.createNote(postResult(), note, (result, caller) => {
                caller.notes.push(result);
            }, self);
        }

        this.editNote = {
            active: ko.observable(null),
            newText: ko.observable(""),
            Begin: function (note) {
                self.editNote.active(note);
                self.editNote.newText(note.text);
                console.log(note);
            },
            Commit: function (note) {
                note.text = self.editNote.newText();
                dat.updateNote(postResult(), note, (result, caller) => {
                    caller.editNote.active(null);
                }, self);
            },
            Cancel: function() {
                self.editNote.active(null);
            }
        }

        var deleteNote = function (note) {
            dat.deleteNote(postResult(), note, (result, caller) => {
                caller.notes.remove(note);
            }, self);
        }

        var counter = 0;
        
        return {
            postResult,
            hasComments,
            getId,
            getNotes,
            notes: self.notes,
            markPost: self.markPost,
            newNoteText: self.newNoteText,
            createNote,
            editNote: self.editNote,
            deleteNote,
            postMarked,
            counter
        };

    }
});