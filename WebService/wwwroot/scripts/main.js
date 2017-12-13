require(["knockout", "jquery", "dataservice", "bootstrap"], function (ko, $, dat) {
    var vm = (function () {
        var self = this;

        this.currentView = ko.observable("searchList");
        this.isCurrentView = function(arg) {
            return ko.computed(function() {
                    return arg === this.currentView();
                }, self);
        }
            
        this.showSearchView = function() {
            self.currentView("searchList");
            self.searchAction = self.datGetList;
        }
        this.showCloudView = function () {
            self.currentView("wordCloud");
            self.searchAction = function() {
                console.log("cloud");
            };
        }
        this.showForceView = function () {
            self.currentView("forceNetwork");
        }


        this.searchWord = ko.observable("");
        this.searchResult = ko.observable(null);

        this.datGetList = function () {
            dat.getPosts(self.searchWord(), (sr, caller) => {
                caller.searchResult(sr);
            }, self);
        }

        dat.getPostsHighscore((sr, caller) =>
        {
            caller.searchResult(sr);
        }, self);

        return {
            isCurrentView: self.isCurrentView,
            currentView: self.currentView,
            showSearchView: self.showSearchView,
            showCloudView: self.showCloudView,
            showForceView: self.showForceView,
            searchWord: self.searchWord,
            searchResult: self.searchResult,
            searchAction: self.searchAction
        };
    })();

    ko.applyBindings(vm);
});