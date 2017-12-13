require(["knockout"], function (ko) {
    ko.components.register("wordCloud",
        {
            viewModel: { require: "../scripts/components/wordCloud/wordCloud" },
            template: { require: "text!../scripts/components/wordCloud/wordCloud.html" }
        }
    );
    ko.components.register("post",
        {
            viewModel: { require: "../scripts/components/singlePost/singlePost" },
            template: { require: "text!../scripts/components/singlePost/singlePost.html" }
        }
    );
    ko.components.register("searchList",
        {
            viewModel: { require: "../scripts/components/searchList/searchList" },
            template: { require: "text!../scripts/components/searchList/searchList.html" }
        }
    );
    ko.components.register("searchListItem",
        {
            viewModel: { require: "../scripts/components/searchListItem/searchListItem" },
            template: { require: "text!../scripts/components/searchListItem/searchListItem.html" }
        }
    );
    //TODO Add force network component
});