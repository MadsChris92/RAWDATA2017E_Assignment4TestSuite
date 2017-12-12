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

            var callback = function (sr, self) {
                self.singlePost(sr);
            }
            self.activePost(self.index);
            console.log(index);

            dat.getSinglePost(post.url, callback, self);
        };

        return {
            post,
            isActive,
            singlePost: self.singlePost,
            showSinglePost
        };

    }
});