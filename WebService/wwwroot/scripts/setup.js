requirejs.config({
    baseUrl: "scripts",
    paths: {
        knockout: "../lib/knockout/dist/knockout",
        bootstrap: "../lib/bootstrap/dist/js/bootstrap.bundle",
        jquery: "../lib/jquery/dist/jquery",
        popper: "../lib/popper.js/dist/umd/popper",
        text: "../lib/text/text",
        jqcloud: "../lib/jqcloud2/dist/jqcloud",
        dataservice: "../scripts/services/dataservice",
        sugarDate: "../lib/sugar-date/dist/sugar-date",
        d3js: "../lib/d3js/d3"
    },
    shim: {
        jqcloud: {
            deps: ["jquery"]
        },
        bootstrap: {
            deps: ["jquery"]
        }
    }
});