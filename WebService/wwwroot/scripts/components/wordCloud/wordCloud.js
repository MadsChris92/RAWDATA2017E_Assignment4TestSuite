define(['knockout'], function (ko) {
    return function (params) {
        var words = ko.computed(() => {
            var _words = [];
            if (params && params.searchCloud()) {
                for (let word of params.searchCloud().posts()) {
                    _words.push({text: word.word, weight: word.ranking});
                }
            } else {
                _words = [
                    { text: "Lorem", weight: 13 },
                    { text: "Ipsum", weight: 10.5 },
                    { text: "Dolor", weight: 9.4 },
                    { text: "Sit", weight: 8 },
                    { text: "Amet", weight: 6.2 },
                    { text: "Consectetur", weight: 5 },
                    { text: "Adipiscing", weight: 5 }
                ];
            }
            console.log(_words);
            return _words;
        });

        

        return {
            words
        };
    }
});