requirejs.config({
    baseUrl: 'lib',
    paths: {
        knockout: 'knockout/dist/knockout',
        bootstrap: 'bootstrap/dist/js/bootstrap',
        jquery: "../lib/jquery/dist/jquery",
        text: "../lib/text/text",
        jqcloud: "../lib/jqcloud2/dist/jqcloud",
        dataservice: "../scripts/services/dataservice"
    },
    shim: {
        jqcloud: {
            deps:['jquery']
        }
    }
});

require(['knockout', 'jquery', 'jqcloud'], function (ko, $) {
    ko.bindingHandlers.cloud = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            // This will be called when the binding is first applied to an element
            // Set up any initial state, event handlers, etc. here
            var words = allBindings.get('cloud').words;
            if (words && ko.isObservable(words)) {
                words.subscribe(function () {
                    $(element).jQCloud('update', ko.unwrap(words));
                });
            }
            console.log("in init: ");
        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            console.log("in update: ");
            // This will be called once when the binding is first applied to an element,
            // and again whenever any observables/computeds that are accessed change
            // Update the DOM element based on the supplied values here.
            var words = ko.unwrap(allBindings.get('cloud').words) || [];
            $(element).jQCloud(words);
        }
    };
});


require(["knockout"], function (ko) {
    ko.components.register("wordCloud",
        {
            viewModel: { require: "../scripts/components/wordCloud/wordCloud" },
            template: { require: "text!../scripts/components/wordCloud/wordCloud.html" }
        }
    );
});

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

        var tagSearch = function(tagTitle) {
            console.log(tagTitle);
            var callback = function(sr, self) {
                console.log(JSON.stringify(self.resultArray()));
                self.resultArray(sr.posts());
                console.log(JSON.stringify(self.resultArray()));
                self.searchResult(sr);
            }
            dat.getPostByTag(tagTitle, callback,vm);
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
			answerCountString,
            tagSearch
        };
    })();

    ko.applyBindings(vm);
});