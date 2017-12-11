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
});