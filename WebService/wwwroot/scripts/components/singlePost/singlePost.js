define(['knockout', 'dataservice'], function (ko, dat) {
    return function (params) {
        var self = this;
        
        var postResult = params.name;
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
            } else {
                dat.markPost(postResult().id);
                postResult().marked = true;
            }
        };
        
        this.notes = ko.observableArray([]);
        var getNotes = function () {
            dat.getPostNotes(postResult, (result, caller) => {
                caller.notes(result);
            }, self);
        }
        
        return {
            postResult,
            hasComments,
            getId,
            getNotes,
            notes: this.notes,
            markPost: self.markPost
        };

    }
});