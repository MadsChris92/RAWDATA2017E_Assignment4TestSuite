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

require(["knockout"], function (ko) {
    var vm = (function () {

        return {
            test: ko.observable("Men ikke lige så meget som Mads")

        };
    })();

    ko.applyBindings(vm);
});