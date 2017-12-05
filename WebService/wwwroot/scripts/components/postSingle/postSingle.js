define(['knockout', 'jquery'], function (ko, $) {
    return function (params) {
        var self = this;
        this.post = params.post;
        this.answers = ko.observableArray();
        this.getAnswers = function () {
            console.log(`test: ${self.post().link}`);
            $.ajax({
                url: `${self.post().link}/answers`,
                success: function (result) {
                    self.answers(result);
                }
            });
        }
        this.back = function() {
            self.post(null);
        }
        this.getAnswers();
        return {
            post: self.post,
            answers: self.answers,
            back: self.back
        };

    }
});