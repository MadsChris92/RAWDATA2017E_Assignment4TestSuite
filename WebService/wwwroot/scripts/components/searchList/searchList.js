define(['knockout', 'dataservice'], function (ko, dat) {
    return function (params) {
        var self = this;

        var searchWord = ko.observable("");
        var noResultsFound = ko.computed(function () {
            if (resultArray().length < 1) {
                return true;
            } else {
                return false;
            }
        }, this);



        var minPostShowing = ko.observable(1);
        var totalPosts = ko.observable(10);
        var postsShowingAmount = ko.observable(10);
        var postsShowing = ko.computed(function () {
            return minPostShowing() + "-" + (minPostShowing() + postsShowingAmount() - 1) + " out of " + totalPosts();
        }, this);


        var singlePost = ko.observable();

        var showSinglePost = function (postLink, index) {

            var callback = function (sr, self) {
                self.singlePost(sr);
            }
            self.activePost(index());
            console.log(index());

            dat.getSinglePost(postLink, callback, vm);
        };

        var tagSearch = function (tagTitle) {
            console.log(tagTitle);
            var callback = function (sr, self) {
                //console.log(JSON.stringify(self.resultArray()));
                self.resultArray(sr.posts());
                //console.log(JSON.stringify(self.resultArray()));
                self.searchResult(sr);
            }
            dat.getPostByTag(tagTitle, callback, vm);
        };

        var datGetList = function () {
            var callback = function (sr, self) {
                //console.log(JSON.stringify(self.resultArray()));
                //self.resultsShowing(sr.showingResults());
                totalPosts(sr.totalResults());
                self.resultArray(sr.posts());
                self.searchResult(sr);
            }
            dat.getPosts(vm.searchWord(), callback, vm);
        };


        var hasNext = ko.computed(function () {
            if (searchResult() != null) {
                return searchResult().hasNext();
            }
            return false;
        }, this);

        var hasPrev = ko.computed(function () {
            if (searchResult() != null) {
                return searchResult().hasPrev();
            }
            return false;
        }, this);

        var goToNext = function () {
            console.log("Next Activated");
            if (searchResult) {
                searchResult().gotoNext();
            }

            minPostShowing(minPostShowing() + postsShowingAmount());
        };

        var goToPrev = function () {
            console.log("Prev Activated");
            if (searchResult) {
                searchResult().gotoPrev();

            }
            minPostShowing(minPostShowing() - postsShowingAmount());
        };

        var isActive = function (index) {
            console.log(index);
            return activePost() == index;
        };

        return {
            searchWord,
            goToNext,
            goToPrev,
            hasNext,
            hasPrev,
            datGetList,
            searchResult,
            tagSearch,
            showSinglePost,
            singlePost,
            noResultsFound,
            activePost,
            postsShowing,
            isActive
        };

    }
});