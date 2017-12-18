define(['knockout', 'dataservice'], function (ko, dat) {
    return function (params) {
        var self = this;
        this.searchHistory = params.searchHistory;

        this.totalPosts = ko.computed(function () {
            if (this.searchHistory() !== null) {

                return searchHistory().totalResults();
            }
            return -1;
        }, this);

        this.noResultsFound = ko.computed(function () {
            return this.totalPosts() === 0;
        }, this);

        var postsShowing = ko.computed(function () {
            if (this.searchHistory() === null) return "";
            return this.searchHistory().page() * this.searchHistory().pageSize() + 1 + "-" + (this.searchHistory().page() + 1) * this.searchHistory().pageSize() + " out of " + this.totalPosts();
        }, this);


        var hasNext = ko.computed(function () {
            if (self.searchHistory() !== null) {
                return self.searchHistory().hasNext();
            }
            return false;
        }, this);

        var hasPrev = ko.computed(function () {
            if (self.searchHistory() !== null) {
                return self.searchHistory().hasPrev();
            }
            return false;
        }, this);

        var goToNext = function () {
            console.log("Next Activated");
            if (self.searchHistory) {
                self.searchHistory().gotoNext();
            }
        };

        var goToPrev = function () {
            console.log("Prev Activated");
            if (self.searchHistory) {
                self.searchHistory().gotoPrev();

            }
        };

        var clearHistory = function () {
            dat.clearHistory();
            console.log("Clearing");
            self.searchHistory().posts(null);
        } 

        return {
            searchHistory: self.searchHistory,
            goToNext,
            goToPrev,
            hasNext,
            hasPrev,
            noResultsFound: self.noResultsFound,
            postsShowing,
            clearHistory
        };
    }
});