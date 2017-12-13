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
        
        return {
            postResult,
            hasComments,
            getId
        };

    }
});