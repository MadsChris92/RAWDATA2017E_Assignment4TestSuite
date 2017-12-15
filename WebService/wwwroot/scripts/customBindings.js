require(['knockout', 'jquery', 'd3js', 'jqcloud', 'sugarDate'], function (ko, $, d3) {
    ko.bindingHandlers.cloud = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            // This will be called when the binding is first applied to an element
            // Set up any initial state, event handlers, etc. here
            console.log("in init: cloud");
            var words = ko.unwrap(valueAccessor()).words;
            $(element).jQCloud(ko.unwrap(words));
        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            console.log("in update: cloud");
            var words = ko.unwrap(ko.unwrap(valueAccessor()).words) || [];
            $(element).jQCloud('update', ko.unwrap(words));
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
        }
    };

    ko.bindingHandlers.forceGraph = {

        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            "use strict";
            console.log("in init: force");
            // This will be called when the binding is first applied to an element
            // Set up any initial state, event handlers, etc. here
            var graph = ko.unwrap(valueAccessor());
            if (graph && ko.isObservable(graph)) {
                graph.subscribe(function () {
                    console.log("whenever");
                });
            }


            viewModel.graphObject = (function () {
                var width, height;
                var chartWidth, chartHeight;
                var margin;
                var svg = d3.select("#graph").append("svg");
                var chartLayer = svg.append("g").classed("chartLayer", true);

                var setData = function (newData) {
                    console.log(newData);
                    svg.selectAll("*").remove();
                    setSize(newData);
                    drawChart(newData);
                }

                function setSize(data) {
                    width = document.querySelector("#graph").clientWidth;
                    height = document.querySelector("#graph").clientHeight;

                    margin = { top: 0, left: 0, bottom: 0, right: 0 };

                    chartWidth = width - (margin.left + margin.right);
                    chartHeight = height - (margin.top + margin.bottom);

                    svg.attr("width", width).attr("height", height);

                    chartLayer
                        .attr("width", chartWidth)
                        .attr("height", chartHeight)
                        .attr("transform", `translate(${[margin.left, margin.top]})`);
                }

                function drawChart(data) {

                    var simulation = d3.forceSimulation()
                        .force("link", d3.forceLink().id(function (d) { return d.index }))
                        .force("collide", d3.forceCollide(function (d) { return d.r*3 + 5 }).iterations(16))
                        .force("charge", d3.forceManyBody())
                        .force("center", d3.forceCenter(chartWidth / 2, chartHeight / 2))
                        .force("y", d3.forceY(0))
                        .force("x", d3.forceX(0));

                    var link = svg.append("g")
                        .attr("class", "links")
                        .selectAll("line")
                        .data(data.links)
                        .enter()
                        .append("line")
                        .attr("stroke", "black")
                        .attr("stroke-width", function (d) { return Math.log(1+d.value); });

                    var node = svg.selectAll(".node")
                        .data(data.nodes)
                        .enter().append("g")
                        .attr("class", "node")
                        .call(d3.drag()
                            .on("start", dragstarted)
                            .on("drag", dragged)
                            .on("end", dragended));

                    node.append("circle")
                        .attr("r", function (d) { return d.r })
                        .attr("fill", function (d) { return d.color });

                    node.append("text")
                        .attr("text-anchor", "middle")
                        .text(function (d) {
                            console.log(d);
                            return d.label;
                        });

                    var ticked = function () {
                        link
                            .attr("x1", function (d) { return d.source.x; })
                            .attr("y1", function (d) { return d.source.y; })
                            .attr("x2", function (d) { return d.target.x; })
                            .attr("y2", function (d) { return d.target.y; });

                        node
                            .attr("cx", function (d) { return d.x; })
                            .attr("cy", function (d) { return d.y; });

                        node.attr("transform",
                            function (d) {
                                return "translate(" + d.x + "," + d.y + ")";
                            });
                    };



                    simulation
                        .nodes(data.nodes)
                        .on("tick", ticked);

                    simulation.force("link")
                        .links(data.links);



                    function dragstarted(d) {
                        if (!d3.event.active) simulation.alphaTarget(0.3).restart();
                        d.fx = d.x;
                        d.fy = d.y;
                    }

                    function dragged(d) {
                        d.fx = d3.event.x;
                        d.fy = d3.event.y;
                    }

                    function dragended(d) {
                        if (!d3.event.active) simulation.alphaTarget(0);
                        d.fx = null;
                        d.fy = null;
                    }

                }

                return {
                    setData
                }
            })();
            
        },
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            console.log("in update: ");
            // This will be called once when the binding is first applied to an element,
            // and again whenever any observables/computeds that are accessed change
            // Update the DOM element based on the supplied values here.
            
            viewModel.graphObject.setData(ko.unwrap(valueAccessor()));
        }
    };
});