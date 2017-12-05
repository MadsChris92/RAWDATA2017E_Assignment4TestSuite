requirejs.config({
    baseUrl: "scripts",
    paths: {
        knockout: "../lib/knockout/dist/knockout",
        bootstrap: "../lib/bootstrap/dist/js/bootstrap",
        jquery: "../lib/jquery/dist/jquery",
        text: "../lib/text/text",
        jqcloud: "../lib/jqcloud2/dist/jqcloud"
    }
});



require(["knockout"], function (ko) {
    ko.components.register("postList",
        {
            viewModel: { require: "components/postList/postList" },
            template: { require: "text!components/postList/postList.html" }
        }
    );
});

require(["knockout", "jquery"], function (ko, $) {
    var vm = (function () {

        var test = ko.observable("Men ikke lige så meget som Mads");
        var postListArray = ko.observableArray([]);
        var resultArray = ko.observableArray([]);
        var getPostList = function () {
            $.ajax({
                url: "http://localhost:5001/api/posts/title/sql",
                method: "GET",
                dataType: "json",
                success: function(data) {
                    console.log(JSON.stringify(vm.postListArray));
                    vm.postListArray(data);
                    vm.resultArray(data.results);
                }
            });
        };

        getPostList();
        return {
            test,
            postListArray,
            resultArray,
            getPostList

        };
    })();

    ko.applyBindings(vm);
});