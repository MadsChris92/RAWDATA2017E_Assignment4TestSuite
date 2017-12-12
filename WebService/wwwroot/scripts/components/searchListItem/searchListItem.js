define(['knockout', 'dataservice'], function (ko, dat) {
    return function (params) {
        var self = this;

        var post = params.post;
        var index = params.index;
        this.activePost = params.activePost;
        this.index = params.index;

        var isActive = ko.computed(function () {
            return this.activePost() == this.index && this.singlePost() != null;
        }, this)




        this.singlePost = ko.observable(null);

        var showSinglePost = function () {

            if (self.activePost() === self.index) {
                self.activePost(-1);
            } else {
                if (self.singlePost() === null) {
                    dat.getSinglePost(post.url, (sr, self) => {
                        self.singlePost(sr);
                    }, self);
                }

                self.activePost(self.index);
            }
        };

        return {
            post,
            isActive,
            singlePost: self.singlePost,
            showSinglePost
        };

    }
});