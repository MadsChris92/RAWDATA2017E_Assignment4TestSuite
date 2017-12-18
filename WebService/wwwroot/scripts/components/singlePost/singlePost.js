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
        
        return {
            postResult,
            hasComments,
            getId,
			markPost: self.markPost,
			postMarked
        };

    }
});