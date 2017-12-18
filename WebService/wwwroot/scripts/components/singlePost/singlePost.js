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

        var editNote = {
            active: ko.observable(-1),
            oldText: "",
            Begin: function (note) {
                editNote.active = ko.observable(note);
            },
            Commit: function(note) {
                dat.editNote(self.postResult(), note, (result, caller) => {
                    caller.
                }, self);
            },
            Cancel: function() {
                
            }
        }

        var deleteNote = function (note) {
            dat.deleteNote(postResult(), note, (result, caller) => {
                caller.notes.remove(note);
            }, self);
        } 
        
        return {
            postResult,
            hasComments,
            getId,
            getNotes,
            notes: self.notes,
            markPost: self.markPost,
            newNoteText: self.newNoteText,
            createNote,
            editNote,
            deleteNote,
			postMarked
        };

    }
});