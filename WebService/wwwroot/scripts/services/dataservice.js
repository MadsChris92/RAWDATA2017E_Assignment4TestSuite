define(["jquery", "knockout"], function ($, ko) {

    const postApi = "/api/posts";
    const histApi = "/api/history";
    const searchApi = "/api/search"; // Does it make sense to implement a search api?(that is, move the search functionality over to its own controller)
    var defaultPageSize = 10;

    const events = {
        event: "event"  // should we have events?
    }

    const getSinglePost = function (link, callback, caller) {
        $.ajax({
            url: link,
            success: function (result) {
                //console.log(result);
                callback(result, caller);
            }
        });
    };

    const getPosts = function (searchTerm, callback, caller) {
        $.ajax({
            url: `${postApi}/title/${searchTerm}?pageSize=${callback.pageSize || defaultPageSize}&firstPage=true`,
            success: function (result) {
                console.log(result);
                callback(new SearchResult(result), caller);// kan ikke returnere fordi den er asyncron... Bruge en event?
            }
        });
    }

    const getPostNotes = function (post, callback, caller) {
        $.ajax({
            url: post.notesUrl,
            success: function (result) {
                console.log(post.notesUrl);
                callback(result, caller);// kan ikke returnere fordi den er asyncron... Bruge en event?
            }
        });
    }

    const getPostByTag = function(tagTitle, callback, caller) {
        $.ajax({
        url: `${postApi}/tag/${tagTitle}`,
            success: function (result) {
                console.log(result);
                callback(new SearchResult(result), caller);// kan ikke returnere fordi den er asyncron... Bruge en event?
            }
    });
    }

    const getPostsHighscore = function (callback, caller) {
        $.ajax({
            url: `${searchApi}/?pageSize=${callback.pageSize || defaultPageSize}`,
            success: function (result) {
                //console.log(result);
                callback(new SearchResult(result), caller);// kan ikke returnere fordi den er asyncron... Bruge en event?
            }
        });
    }

    const getRelatedWords = function (searchString, callback, caller) {
        if (searchString) {
            $.ajax({
                url: `${searchApi}/words/related/${searchString}?pageSize=${callback.pageSize || defaultPageSize}`,
                success: function (result) {
                    callback(result.results, caller);
                }
            });
        }
    }

    const getRankedWords = function (searchString, callback, caller) {
        if (searchString) {
            $.ajax({
                url: `${searchApi}/words/ranked/${searchString}?pageSize=${callback.pageSize || defaultPageSize}`,
                success: function (result) {
                    callback(new SearchResult(result), caller);
                }
            });
        }
    }

    const getHistory = function(callback, caller) {
        $.ajax({
            url: `${histApi}/`,
            success: function (result) {
                callback(new SearchResult(result), caller);
                console.log(result);
                
            }
        });
    }

    const clearHistory = function () {
        $.ajax({
            url: `${histApi}/`,
            type: 'DELETE', 
            success: function (result) {
                console.log(result);
            }
        });
    }

    const createNote = function (post, note, callback, caller) {
        $.ajax({
            url: `${post.url}/note`,
            type: "POST",
            data: JSON.stringify(note),
            contentType: "application/json",
            success: function(result) {
                console.log(`created ${result}`);
                callback(result, caller);
            },
            error: function(result) {
                console.log("failed to create note");
            }
        });
    }

    const updateNote = function (post, note, callback, caller) {
        $.ajax({
            url: `${note.url}`,
            type: "PUT",
            data: JSON.stringify(note),
            contentType: "application/json",
            success: function (result) {
                console.log(`created ${result}`);
                callback(result, caller);
            },
            error: function (result) {
                console.log("failed to create note");
            }
        });
    }

    const deleteNote = function (post, note, callback, caller) {
        $.ajax({
            url: `${note.url}`,
            type: "DELETE",
            success: function (result) {
                console.log(`deleted note ${result}`);
                callback(result, caller);
            },
            error: function (result) {
                console.log("failed to delete note");
            }
        });
    }

    const markPost = function(id) {
        $.ajax({
            url: `${postApi}/mark/${id}`,
            success: function (result) {
                
                console.log("Marked");
            }
        });
    }

    const unmarkPost = function (id) {
        $.ajax({
            url: `${postApi}/mark/${id}`,
            type: 'DELETE',
            success: function (result) {

                console.log("Unmarked");
            }
        });
    }



    //ideen er at have et objekt der bare kan få besked om at hente den næste/forrige side, uden at bekymre sig om url'er
    function SearchResult(result) {
        const self = this;
        this.next = ko.observable(result.nextPage);
        this.prev = ko.observable(result.previousPage);
        this.hasNext = ko.computed(() => {
            return this.next() || false;
        }, this);
        this.hasPrev = ko.computed(() => {
            return this.prev() || false;
        }, this);
        this.posts = ko.observableArray(result.results);
        this.showingResults = ko.observable(result.showingResults);
        this.totalResults = ko.observable(result.totalResults);

        this.page = ko.observable(0);
        this.pageSize = ko.observable(defaultPageSize); // TODO Page size i searchresult vil altid være default, selv hvis en anden er brugt
        this.totalPages = ko.computed(() => {
            return this.totalResults / this.pageSize;
        }, this);


        this.gotoNext = () => {
            if (this.hasNext()) {
                this.posts([]);
                $.ajax({
                    url: this.next(),
                    success: function (result) {
                        self.next(result.nextPage);
                        self.prev(result.previousPage);
                        self.posts(result.results);
                    }
                });
                this.page(this.page() + 1);
            }
        }

        this.gotoPrev = () => {
            if (this.hasPrev()) {
                this.posts([]);
                $.ajax({
                    url: this.prev(),
                    success: function(result) {
                        self.next(result.nextPage);
                        self.prev(result.previousPage);
                        self.posts(result.results);
                    }
                });
                this.page(this.page() - 1);
            }
        }
        return {
            hasNext: self.hasNext,
            hasPrev: self.hasPrev,
            gotoNext: self.gotoNext,
            gotoPrev: self.gotoPrev,
            posts: self.posts,
            page: self.page,
            pageSize: self.pageSize,
            totalPages: self.totalPages,
            showingResults: self.showingResults,
            totalResults: self.totalResults
        }
    }

    return {
        events,
        getPosts,
        getPostNotes,
        getPostByTag,
        getPostsHighscore,
        getSinglePost,
        getRelatedWords,
        getRankedWords,
        getHistory,
        createNote,
        updateNote,
        deleteNote,
        markPost,
        unmarkPost
    }
});


// postman example
/*
define([], function() {

    var subscribers = [];

    var subscribe = (event, callback) => {
        var subscriber = { event, callback };
        subscribers.push(subscriber);

        return function() {
            var index = subscribers.indexOf(subscriber);
            subscribers.slice(index, 1);
        };

    };

    var publish = (event, payload) =>
    {
        subscribers.forEach(s => {
            if (s.event === event) s.callback(payload);
        });
    }

    var events =
    {
        changeView: "changeView"
    };

    return {
        subscribe,
        publish,
        events
    };

});
*/