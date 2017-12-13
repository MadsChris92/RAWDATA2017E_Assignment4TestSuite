define(['knockout', 'd3js'], function (ko, d3) {
    return function (params) {
        var graph = params && params.graph ||
            ko.observable(
                {
                    "nodes": [
                        { "name": "No" },
                        { "name": "words" },
                        { "name": "were" },
                        { "name": "given" },
                        { "name": "to" },
                        { "name": "show" }
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




        var width = 960,
            height = 500

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
        (function () {
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
                .style("fill", "red")

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


        return {
            graph
        };
    }
});