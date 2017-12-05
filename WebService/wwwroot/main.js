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
            test: ko.observable("GSEDGKWSD")
        };
    })();

    ko.applyBindings(vm);
});