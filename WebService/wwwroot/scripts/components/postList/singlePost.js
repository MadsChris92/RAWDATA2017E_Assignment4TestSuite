define(['knockout', 'dataservice'], function (ko, dat) {
    return function (params) {
        var self = this;
        
        //var postResult = params.name;
        var postResult = ko.observable(params.name || '');

        
        console.log(postResult);
        return {
            postResult
        };

    }
});