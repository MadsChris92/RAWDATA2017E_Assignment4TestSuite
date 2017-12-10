require(['knockout', 'jquery', 'jqcloud', 'sugarDate'], function (ko, $) {
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

    ko.bindingHandlers.date = {
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            // This will be called once when the binding is first applied to an element,
            // and again whenever any observables/computeds that are accessed change
            // Update the DOM element based on the supplied values here.
            var date = new Sugar.Date(ko.unwrap(valueAccessor()));
            element.innerText = date.relative().raw;
            element.title = date.raw;
            console.log(ko.unwrap(valueAccessor()));
        }
    };
});