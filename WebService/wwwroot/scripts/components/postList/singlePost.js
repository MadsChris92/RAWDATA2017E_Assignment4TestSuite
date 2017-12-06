define(['knockout', 'dataservice'], function (ko, dat) {
    return function (params) {
        var self = this;
        
        var postResult = params;
        
        console.log(postResult.title);
        

        var getPost = function (postListItem) {
            
        }
        return {
            getPost,
            postResult
        };

    }
});