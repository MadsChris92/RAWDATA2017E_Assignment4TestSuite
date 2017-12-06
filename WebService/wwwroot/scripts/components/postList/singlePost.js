define(['knockout', 'dataservice'], function (ko, dat) {
    return function (params) {
        var self = this;
        var postResult = ko.observable("");

        var getPost = function (postListItem) {

            //var callback = function (sr, self) {
            //    self.postResult(sr);
            //    console.log(JSON.stringify(self.postResult));
            //}
        }
        return {
            getPost
        };

    }
});