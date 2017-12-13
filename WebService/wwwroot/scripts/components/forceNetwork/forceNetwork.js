define(['knockout', 'd3js'], function (ko, d3) {
    return function (params) {
        var graph = params && params.graph ||
            ko.observable(
                {
                    "nodes": [
                        { "label": "No", "r": 5 },
                        { "label": "words", "r": 100 },
                        { "label": "were", "r": 3 },
                        { "label": "given", "r": 30 },
                        { "label": "to", "r": 15 },
                        { "label": "show", "r": 15 }
                    ],

                    "links": [
                        { "source": 0, "target": 1, "value": 1 },
                        { "source": 1, "target": 2, "value": 1 },
                        { "source": 2, "target": 3, "value": 1 },
                        { "source": 3, "target": 4, "value": 1 },
                        { "source": 4, "target": 5, "value": 1 },
                        { "source": 5, "target": 0, "value": 1 }
                    ]
                });



        /*
        var width = 960,
            height = 500;

        var svg = d3.select("#forceNetwork").append("svg")
            .attr("width", width)
            .attr("height", height);

        var force = d3.force()
            .gravity(0.05)
            .distance(100)
            .charge(-200)
            .size([width, height]);

        //var color = d3.scaleOrdinal(d3.schemeCategory20);
        var color = d3.scale.category20c();

        //d3.json("graph.json", function(error, json) {
 
        !(function () {
            //if (error) throw error;

            force
                .nodes(graph.nodes)
                .links(graph.links)
                .start();

            var link = svg.selectAll(".link")
                .data(graph.links)
                .enter().append("line")
                .attr("class", "link")
                .attr("stroke-width", function (d) { return Math.sqrt(d.value); });

            var node = svg.selectAll(".node")
                .data(graph.nodes)
                .enter().append("g")
                .attr("class", "node")
                .call(force.drag);
            node.append("circle")
                .attr("r", function (d) { return 5; })
                .style("fill", "red");

            node.append("text")
                .attr("dx", function (d) { return -(d.name.length * 3) })
                .attr("dy", ".65em")
                .text(function (d) { return d.name });

            force.on("tick", function () {
                link.attr("x1", function (d) { return d.source.x; })
                    .attr("y1", function (d) { return d.source.y; })
                    .attr("x2", function (d) { return d.target.x; })
                    .attr("y2", function (d) { return d.target.y; });

                node.attr("transform", function (d) { return "translate(" + d.x + "," + d.y + ")"; });
            });
        })();
        */

        let boolean = !(function () {
            "use strict";

            var width, height;
            var chartWidth, chartHeight;
            var margin;
            var svg = d3.select("#graph").append("svg");
            var chartLayer = svg.append("g").classed("chartLayer", true);

            main();

            function main() {
                var range = 100;
                var data = ko.unwrap(graph);
                /*
                var data = {
                    nodes: d3.range(0, range).map(function (d) { return { label: "l" + d, r: ~~d3.randomUniform(8, 28)() } }),
                    links: d3.range(0, range).map(function () { return { source: ~~d3.randomUniform(range)(), target: ~~d3.randomUniform(range)() } })
                };*/

                setSize(data);
                drawChart(data);
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
                    .attr("transform", "translate(" + [margin.left, margin.top] + ")");


            }

            function drawChart(data) {

                var simulation = d3.forceSimulation()
                    .force("link", d3.forceLink().id(function (d) { return d.index }))
                    .force("collide", d3.forceCollide(function (d) { return d.r + 8 }).iterations(16))
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
                    .attr("stroke", "black");

                var node = svg.selectAll(".node")
                    .data(data.nodes)
                    .enter().append("g")
                    .attr("class", "node")
                    .call(d3.drag()
                        .on("start", dragstarted)
                        .on("drag", dragged)
                        .on("end", dragended));

                /*var node = svg.append("g")
                    .attr("class", "nodes")
                    .selectAll("circle")
                    .data(data.nodes)
                    .enter();*/

                node.append("circle")
                    .attr("r", function (d) { return d.r })
                    .attr("fill", "antiquepaper" );

                node.append("text")
                    .attr("dx", "6")
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
                        function(d) {
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
        }());

        return {
            graph
        };
    };
});