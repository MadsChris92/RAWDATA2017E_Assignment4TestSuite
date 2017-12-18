define(['knockout', 'dataservice'], function (ko, dat) {
    return function (params) {
        var self = this;
        this.searchResult = params.searchResult;

        this.totalPosts = ko.computed(function () {
            if (this.searchResult() !== null) {

                return searchResult().totalResults();
            }
            return -1;
        }, this);

        this.noResultsFound = ko.computed(function () {
            return this.totalPosts() === 0;
        }, this);

        var postsShowing = ko.computed(function () {
            if (this.searchResult() === null) return "";
            return this.searchResult().page() * this.searchResult().pageSize() + 1 + "-" + (this.searchResult().page() + 1) * this.searchResult().pageSize() + " out of " + this.totalPosts();
        }, this);


        var hasNext = ko.computed(function () {
            if (self.searchResult() !== null) {
                return self.searchResult().hasNext();
            }
            return false;
        }, this);

        var hasPrev = ko.computed(function () {
            if (self.searchResult() !== null) {
                return self.searchResult().hasPrev();
            }
            return false;
        }, this);

        var goToNext = function () {
            console.log("Next Activated");
            if (self.searchResult) {
                self.searchResult().gotoNext();
            }
        };

        var goToPrev = function () {
            console.log("Prev Activated");
            if (self.searchResult) {
                self.searchResult().gotoPrev();

            }
        };

        return {
            searchResult: self.searchResult,
            goToNext,
            goToPrev,
            hasNext,
            hasPrev,
            noResultsFound: self.noResultsFound,
            postsShowing,
        };
    }
});