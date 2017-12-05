define(["jquery"], function($) {
    const postApi = "/api/posts";
    const histApi = "/api/history";
    const searchApi = "/api/search"; // Does it make sense to implement a search api?(that is, move the search functionality over to its own controller)
    var pageSize = 10;

    const events = {
        event: "event"  // should we have events?
    }

    const getPosts = function(searchTerm, page) {
        $.ajax({
            url: `${postApi}/title/${searchTerm}?page=${page}&pageSize=${pageSize}`,
            success: function (result) {
                console.log(result);
                return result;
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

    return {
        events,
        getPosts,
        getPost
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