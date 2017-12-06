requirejs.config({
    baseUrl: 'lib',
    paths: {
        knockout: 'knockout/dist/knockout',
        bootstrap: 'bootstrap/dist/js/bootstrap',
        jquery: "../lib/jquery/dist/jquery",
        text: "../lib/text/text",
        jqcloud: "../lib/jqcloud2/dist/jqcloud",
        dataservice: "../scripts/services/dataservice"
    }
})

require(["knockout", "jquery", "dataservice"], function (ko, $, dat) {
    var vm = (function() {
        var self = this;
        var searchWord = ko.observable("");
        var test = ko.observable("GSEDGKWSD");
        var postListArray = ko.observableArray([]);
        var resultArray = ko.observableArray([]);
        var searchResult = ko.observable(null);
        var getPostList = function () {
            $.ajax({
                url: "http://localhost:5001/api/posts/title/" + vm.searchWord,
                method: "GET",
                dataType: "json",
                success: function (data) {

                    vm.postListArray(data);
                    vm.resultArray(data.results);
                    console.log(data);
                    console.log(vm.searchWord);
                }
            })
        };

        var tagSearch = function(param) {

        };

        var datGetList = function() {
            var callback = function(sr, self) {
                console.log(JSON.stringify(self.resultArray()));
                self.resultArray(sr.posts());
                console.log(JSON.stringify(self.resultArray()));
                self.searchResult(sr);
            }
            dat.getPosts(vm.searchWord(), callback, vm);
        };

        var goToNext = function() {
            
            if (searchResult) {
                console.log("next");
                searchResult().gotoNext();
            }
        };

        var goToPrev = function() {
            if (searchResult) {
                searchResult().gotoPrev();
            }
		};

		var answerCountString = function (param) {
			return answerCount + " answers"; 
		}

        return {
            searchWord,
            test,
            postListArray,
            resultArray,
            getPostList,
            goToNext,
            goToPrev,
            datGetList,
			searchResult,
			answerCountString
        };
    })();

    ko.applyBindings(vm);
});