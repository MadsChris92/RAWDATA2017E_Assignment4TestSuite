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
    var vm = (function () {
        var searchWord = ko.observable("");
        var test = ko.observable("GSEDGKWSD");
        var postListArray = ko.observableArray([]);
        var resultArray = ko.observableArray([]);
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

        var datGetList = function() {
            dat.getPosts(vm.searchWord, function(sr) {
                postListArray = sr.posts;
            });
        };

        return {
            searchWord,
            test,
            postListArray,
            resultArray,
            getPostList,
            datGetList

        };
    })();

    ko.applyBindings(vm);
});