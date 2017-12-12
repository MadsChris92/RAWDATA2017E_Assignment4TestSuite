define(['knockout', 'dataservice'], function (ko, dat) {
    return function (params) {
        var self = this;

        this.searchWord = ko.observable("");
        this.searchResult = ko.observable(null);
        var noResultsFound = ko.computed(function () {
            if (self.searchResult() && self.searchResult().posts.length > 1) {
                return false;
            } else {
                return true;
            }
        }, this);



        var minPostShowing = ko.observable(1);
        var totalPosts = ko.observable(10);
        var postsShowingAmount = ko.observable(10);
        var postsShowing = ko.computed(function () {
            return minPostShowing() + "-" + (minPostShowing() + postsShowingAmount() - 1) + " out of " + totalPosts();
        }, this);

        var tagSearch = function (tagTitle) {
            console.log(tagTitle);
            var callback = function (sr, self) {
                //console.log(JSON.stringify(self.resultArray()));
                //console.log(JSON.stringify(self.resultArray()));
                self.searchResult(sr);
            }
            dat.getPostByTag(tagTitle, callback, self);
        };

        var datGetList = function () {
            var callback = function (sr, self) {
                //console.log(JSON.stringify(self.resultArray()));
                //self.resultsShowing(sr.showingResults());
                totalPosts(sr.totalResults());
                self.searchResult(sr);
            }
            dat.getPosts(self.searchWord(), callback, self);
        };


        var hasNext = ko.computed(function () {
            if (self.searchResult() != null) {
                return self.searchResult().hasNext();
            }
            return false;
        }, this);

        var hasPrev = ko.computed(function () {
            if (self.searchResult() != null) {
                return self.searchResult().hasPrev();
            }
            return false;
        }, this);

        var goToNext = function () {
            console.log("Next Activated");
            if (self.searchResult) {
                self.searchResult().gotoNext();
            }

            minPostShowing(minPostShowing() + postsShowingAmount());
        };

        var goToPrev = function () {
            console.log("Prev Activated");
            if (self.searchResult) {
                self.searchResult().gotoPrev();

            }
            minPostShowing(minPostShowing() - postsShowingAmount());
        };

        var isActive = function (index) {
            console.log(index);
            return activePost() == index;
        };

        return {
            searchWord: self.searchWord,
            searchResult: self.searchResult,
            goToNext,
            goToPrev,
            hasNext,
            hasPrev,
            datGetList,
            tagSearch,
            noResultsFound,
            activePost,
            postsShowing
        };
    }
});