define(['knockout', 'dataservice'], function (ko, dat) {
    return function (params) {
        var self = this;

        var post = params.post;
        var index = params.index;
        this.activePost = params.activePost;
        this.index = params.index;
        this.parent = params.searchList;

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


        this.tagSearch = function (tagTitle) {
            console.log(tagTitle);
            var callback = function (sr, self) {
                console.log('Tagsearch: ');
                //console.log(JSON.stringify(self.resultArray()));
                //self.resultArray(sr.posts());
                //console.log(JSON.stringify(self.resultArray()));
                self.searchResult(sr);
            }
            dat.getPostByTag(tagTitle, callback, self.parent);
        };


        return {
            post,
            isActive,
            singlePost: self.singlePost,
            showSinglePost,
            tagSearch: self.tagSearch
        };

    }
});