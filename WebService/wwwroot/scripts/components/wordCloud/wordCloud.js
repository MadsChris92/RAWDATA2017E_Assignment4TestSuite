define(['knockout'], function (ko) {
    return function (params) {
        var words = params && params.words || ko.observableArray([
            { text: "Lorem", weight: 13 },
            { text: "Ipsum", weight: 10.5 },
            { text: "Dolor", weight: 9.4 },
            { text: "Sit", weight: 8 },
            { text: "Amet", weight: 6.2 },
            { text: "Consectetur", weight: 5 },
            { text: "Adipiscing", weight: 5 }
        ]);

        var test = params.test;

        console.log("why");
        return {
            words
        };
    }
});