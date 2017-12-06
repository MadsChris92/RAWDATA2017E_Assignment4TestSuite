requirejs.config({
    baseUrl: 'lib',
    paths: {
        knockout: 'knockout/dist/knockout',
        bootstrap: 'bootstrap/dist/js/bootstrap',
        jquery: "../lib/jquery/dist/jquery",
        text: "../lib/text/text",
        jqcloud: "../lib/jqcloud2/dist/jqcloud"
    }
})

require(["knockout", "jquery"], function (ko, $) {
    var vm = (function () {
        searchWord: ko.observable("")

        return {
            test: ko.observable("GSEDGKWSD"),
            postListArray: ko.observableArray([]),
            resultArray: ko.observableArray([]),
            searchWord: this.searchWord,
            
            getPostList: function () {
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
            }
        };
    })();

    ko.applyBindings(vm);
});