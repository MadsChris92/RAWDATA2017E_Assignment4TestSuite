define(['knockout', 'dataservice'], function (ko, dat) {
    return function (params) {
        var self = this;
        
        var postResult = params.name;
        var hasComments = ko.computed(function() {
            return typeof postResult().comments !== "undefined" && postResult().comments.length > 0;
        }, this);
        //var postResult = ko.observable(params.name || '');

        
        console.log(postResult);
        return {
            postResult,
            hasComments
        };

    }
});