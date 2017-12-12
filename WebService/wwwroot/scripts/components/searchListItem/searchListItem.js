﻿define(['knockout', 'dataservice'], function (ko, dat) {
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

            dat.getSinglePost(post.url, callback, self);
        };


        var tagSearch = function (tagTitle) {
            console.log(tagTitle);
            var callback = function (sr, self) {
                console.log('Tagsearch: ');
                //console.log(JSON.stringify(self.resultArray()));
                self.resultArray(sr.posts());
                //console.log(JSON.stringify(self.resultArray()));
                self.searchResult(sr);
            }
            dat.getPostByTag(tagTitle, callback, vm);
        };


        return {
            post,
            isActive,
            singlePost: self.singlePost,
            showSinglePost,
            tagSearch
        };

    }
});