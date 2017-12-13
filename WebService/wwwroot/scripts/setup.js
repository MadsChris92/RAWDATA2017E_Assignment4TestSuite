requirejs.config({
    baseUrl: "scripts",
    paths: {
        knockout: "../lib/knockout/dist/knockout",
        bootstrap: "../lib/bootstrap/dist/js/bootstrap",
        jquery: "../lib/jquery/dist/jquery",
        text: "../lib/text/text",
        jqcloud: "../lib/jqcloud2/dist/jqcloud",
        dataservice: "../scripts/services/dataservice",
        sugarDate: "../lib/sugar-date/dist/sugar-date"
    },
    shim: {
        jqcloud: {
            deps: ["jquery"]
        }
    }
});