define(["jquery", "knockout"], function ($, ko) {

    const postApi = "/api/posts";
    const histApi = "/api/history";
    const searchApi = "/api/search"; // Does it make sense to implement a search api?(that is, move the search functionality over to its own controller)
    var pageSize = 10;

    const events = {
        event: "event"  // should we have events?
    }

    const getSinglePost = function (link, callback, caller) {
        $.ajax({
            url: link,
            success: function (result) {
                console.log(result);
            }
        });
    };

    const getPosts = function (searchTerm, callback, caller) {
        $.ajax({
            url: `${postApi}/title/${searchTerm}?pageSize=${pageSize}&firstPage=true`,
            success: function (result) {
                console.log(result);
                callback(new SearchResult(result), caller);// kan ikke returnere fordi den er asyncron... Bruge en event?
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

    const getPost = function (id) {
        $.ajax({
            url: `${postApi}/${id}`,
            success: function (result) {
                console.log(result);
                return result;
            }
        });
    }

    //ideen er at have et objekt der bare kan få besked om at hente den næste/forrige side, uden at bekymre sig om url'er
    function SearchResult(result) {
        var self = this;
        const page = ko.observable(0);
        const next = ko.observable(result.nextPage);
        const prev = ko.observable(result.previousPage);
        const hasNext = ko.computed(function () {
            return next() || false;
        }, this);
        const hasPrev = ko.computed(function () {
            return prev() || false;
        }, this);
        const posts = ko.observableArray(result.results);

        const gotoNext = function () {
            if (hasNext()) {
                posts([]);
                $.ajax({
                    url: next(),
                    success: function (result) {
                        next(result.nextPage);
                        prev(result.previousPage);
                        posts(result.results);
                        //Javascript er fgt joe sproe
                    }
                });
                page(page() + 1);
            }
        }

        const gotoPrev = function () {
            if (hasPrev()) {
                posts([]);
                $.ajax({
                    url: prev(),
                    success: function(result) {
                        next(result.nextPage);
                        prev(result.previousPage);
                        posts(result.results);
                    }
                });
                page(page() - 1);
            }
        }
        return {
            hasNext,
            hasPrev,
            gotoNext,
            gotoPrev,
            posts,
            page,
            getPost
        }
    }

    return {
        events,
        getPosts,
        getPost,
        getPostByTag,
        getSinglePost
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