require(["knockout", "jquery", "dataservice", "bootstrap"], function (ko, $, dat) {
    var vm = (function () {
        var self = this;

        this.currentView = ko.observable("searchList");
        this.isCurrentView = function(arg) {
            return ko.computed(function() {
                    return arg === this.currentView();
                }, self);
        }


        this.showSearchView = function () {
            if (self.currentView() !== "searchList") {
                self.currentView("searchList");
                self.searchAction(self.datGetList);
                self.searchResult(null);
                self.searchAction()(); 
            }
        }
        this.showCloudView = function () {
            if (self.currentView() !== "wordCloud") {
                self.currentView("wordCloud");
                self.searchAction(self.datGetWords);
                self.searchResult(null);
                self.searchAction()();
            }
        }
        this.showForceView = function () {
            if (self.currentView() !== "forceNetwork") {
                self.currentView("forceNetwork");
                self.searchAction(self.datGetOccurences);
                self.searchResult(null);
                self.searchAction()();
            }
        }


        this.searchWord = ko.observable("");

        this.searchResult = ko.observable(null);
        this.datGetList = function () {
            let callback = (sr, caller) => {
                caller.searchResult(sr);
            };
            dat.getPosts(self.searchWord(), callback, self);
        }

        this.searchCloud = ko.observable(null);
        this.datGetWords = function () {
            let callback = (sr, caller) => {
                caller.searchCloud(sr);
            };
            callback.pageSize = 50;
            dat.getRankedWords(self.searchWord(), callback, self);
        }

        this.searchGraph = ko.observable(null);
        this.datGetOccurences = function () {
            let callback = (sr, caller) => {
                caller.searchGraph(sr);
            };
            callback.pageSize = 25;
            dat.getRelatedWords(self.searchWord(), callback, self);
        }


        dat.getPostsHighscore((sr, caller) =>
        {
            caller.searchResult(sr);
        }, self);

        this.searchAction = ko.observable(this.datGetList);

        this.searchMethod = function() {
            self.searchAction()();
        }

        return {
            isCurrentView: self.isCurrentView,
            currentView: self.currentView,
            showSearchView: self.showSearchView,
            showCloudView: self.showCloudView,
            showForceView: self.showForceView,
            searchWord: self.searchWord,
            searchResult: self.searchResult,
            searchMethod: self.searchMethod,
            searchGraph: self.searchGraph,
            searchCloud: self.searchCloud
        };
    })();

    ko.applyBindings(vm);
});