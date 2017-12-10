require(["knockout", "jquery", "dataservice"], function (ko, $, dat) {
    var vm = (function () {
        var self = this;
        var searchWord = ko.observable("");
        var test = ko.observable("GSEDGKWSD");
        var postListArray = ko.observableArray([]);
        var resultArray = ko.observableArray([]);
        var searchResult = ko.observable(null);
        var noResultsFound = ko.computed(function () {
            if (resultArray().length < 1) {
                return true;
            } else {
                return false;
            }
        }, this);

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


        var singlePost = ko.observable({
            title: "john",
            body: "johnjohn",
            answers: []
        });

        var showSinglePost = function (postLink) {
            //dat.getSinglePost(postLink);
            //dat.getPosts(vm.searchWord(), callback, );

            var callback = function (sr, self) {
                console.log("SR: " + sr);
                self.singlePost(sr);
            }

            dat.getSinglePost(postLink, callback, vm);
            console.log(postLink);
        };

        var tagSearch = function (tagTitle) {
            console.log(tagTitle);
            var callback = function (sr, self) {
                console.log(JSON.stringify(self.resultArray()));
                self.resultArray(sr.posts());
                console.log(JSON.stringify(self.resultArray()));
                self.searchResult(sr);
            }
            dat.getPostByTag(tagTitle, callback, vm);
        };

        var datGetList = function () {
            var callback = function (sr, self) {
                //console.log(JSON.stringify(self.resultArray()));
                self.resultArray(sr.posts());
                //console.log(JSON.stringify(self.resultArray()));
                self.searchResult(sr);
            }
            dat.getPosts(vm.searchWord(), callback, vm);
        };

        var goToNext = function () {
            console.log("Next Activated");
            if (searchResult) {
                console.log("next");
                searchResult().gotoNext();
            }
        };

        var goToPrev = function () {
            console.log("Prev Activated");
            if (searchResult) {
                searchResult().gotoPrev();

            }
        };



        return {
            searchWord,
            test,
            postListArray,
            resultArray,
            goToNext,
            goToPrev,
            hasNext,
            hasPrev,
            datGetList,
            searchResult,
            tagSearch,
            showSinglePost,
            singlePost,
            noResultsFound
        };
    })();

    ko.applyBindings(vm);
});