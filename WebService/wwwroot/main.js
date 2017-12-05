requirejs.config({
    baseUrl: 'lib',
    paths: {
        knockout: 'knockout/dist/knockout',
        bootstrap: 'bootstrap/dist/js/bootstrap'
    }
})

require(['knockout'], function (ko) {
    var vm = (function () {
    

        return {
            test: ko.observable("GSEDGKWSD"),
            postListArray: ko.observableArray([]),
            resultArray: ko.observableArray([]),
            searchWord: ko.observable(""),

            getPostList: function () {
                $.ajax({
                    url: "http://localhost:5001/api/posts/title/" + vm.searchWord,
                    method: "GET",
                    dataType: "json",
                    success: function (data) {
                        vm.postListArray(data);
                        vm.resultArray(data.results);
                        console.log(data);
                    }
                })
            }
        };
    })();

    ko.applyBindings(vm);
});