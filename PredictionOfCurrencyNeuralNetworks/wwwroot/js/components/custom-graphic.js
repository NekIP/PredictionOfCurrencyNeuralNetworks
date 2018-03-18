export default {
    props: ["code", "data", "graphicWidth", "graphic"],
    data: function () {
        return {
            colors: [
                "#00a47f",
                "#fea62b",
                "#FE602B",
                "#FE2B48",
                "#FE2B88",
                "#C12BFE",
                "#3A2BFE",
                "#2B89FE",
                "#2BFEED",
                "#CEFE2B",
                "#2BFE72",
                "#2BFAFE",
                "#FE372B"
            ],
            minMax: {
                minDate: undefined,
                maxDate: undefined,
                minValue: undefined,
                maxValue: undefined
            },
            left: 0,
            right: 0
        }
    },
    watch: {
        graphic: function (val) {
            this.showHideGraphic();
        },
        left: function (val) {
            if (val < 0) {
                this.left = 0;
            }
            if (val > 100) {
                this.left = 100;
            }
            if (100 - val < Math.abs(this.right)) {
                this.right = -(100 - val);
            }
            this.showHideGraphic();
        },
        right: function (val) {
            if (val < -100) {
                this.right = -100;
            }
            if (val > 0) {
                this.right = 0;
            }
            if (100 - Math.abs(+val) < this.left) {
                this.left = 100 - Math.abs(+val);
            }
            this.showHideGraphic();
        }
    },
    methods: {
        showHideGraphic: function () {
            d3.select("#" + this.code + '-graphic').selectAll("svg").remove();
            if (this.graphic) {
                this.minMax = {
                    minDate: undefined,
                    maxDate: undefined,
                    minValue: undefined,
                    maxValue: undefined
                }
                let converted = [];
                for (let i = Math.trunc(this.data.length * (this.left / 100)); i < Math.trunc(this.data.length * (1 - Math.abs(this.right / 100))); i++) {
                    let item = {
                        id: this.data[i].id,
                        date: this.data[i].values[0],
                        values: this.data[i].values.slice(1)
                    };
                    if (!this.minMax.minDate
                        || new Date(this.minMax.minDate).getTime() > new Date(item.date).getTime()) {
                        this.minMax.minDate = item.date;
                    }
                    if (!this.minMax.maxDate
                        || new Date(this.minMax.maxDate).getTime() < new Date(item.date).getTime()) {
                        this.minMax.maxDate = item.date;
                    }
                    for (let j = 0; j < item.values.length; j++) {
                        if (!this.minMax.minValue || this.minMax.minValue > item.values[j]) {
                            this.minMax.minValue = +item.values[j];
                        }
                        if (!this.minMax.maxValue || this.minMax.maxValue < item.values[j]) {
                            this.minMax.maxValue = +item.values[j];
                        }
                    }
                    for (let j = 0; j < item.values.length; j++) {
                        if (j + 1 > converted.length) {
                            converted.push([]);
                        }
                        converted[j].push({ x: new Date(item.date).getTime(), y: item.values[j] });
                    }
                }
                console.log(converted);
                let y2 = this.minMax.minValue;
                let y1 = this.minMax.maxValue;
                console.log(y2 + "_" + y1);
                let a = new Date(this.minMax.minDate).getTime();
                let b = new Date(this.minMax.maxDate).getTime();
                let i1 = 50;
                let j1 = 25;
                let i2 = $("#" + this.code + '-graphic').width() - 30;
                let j2 = $("#" + this.code + '-graphic').height() - 50;
                let xScreen = x => i1 + Math.trunc((x - a) * (i2 - i1) / (b - a));
                let yScreen = y => j1 + Math.trunc((y - y1) * (j2 - j1) / (y2 - y1));
                let line = d3.line()
                    .x(function (d) { return xScreen(d.x); })
                    .y(function (d) { return yScreen(d.y); });
                let svg = d3.select("#" + this.code + '-graphic').append("svg");
                let scaleHorizontal = d3.scaleTime()
                    .domain([new Date(this.minMax.minDate), new Date(this.minMax.maxDate)])
                    .range([i1, i2]);
                let scaleVertical = d3.scaleLinear()
                    .domain([y2, y1])
                    .range([j2, j1]);
                let axisHorizontal = d3.axisBottom()
                    .scale(scaleHorizontal)
                    .ticks(12);
                let axisVertical = d3.axisLeft()
                    .scale(scaleVertical)
                    .ticks(24);
                let xG = svg.append("g")
                    .attr("class", "x-axis")
                    .attr("transform", "translate(" + 0 + "," + j1 + ")")
                    .call(axisHorizontal);
                let yG = svg.append("g")
                    .attr("class", "y-axis")
                    .attr("transform", "translate(" + i1 + "," + 0 + ")")
                    .call(axisVertical);

                xG.selectAll("g.x-axis g.tick")
                    .append("line")
                    .classed("grid-line", true)
                    .attr("x1", 0)
                    .attr("y1", 0)
                    .attr("x2", 0)
                    .attr("y2", j2 - j1);

                yG.selectAll("g.y-axis g.tick")
                    .append("line")
                    .classed("grid-line", true)
                    .attr("x1", 0)
                    .attr("y1", 0)
                    .attr("x2", i2 - i1)
                    .attr("y2", 0);

                for (let i = 0; i < converted.length; i++) {
                    svg.append("path")
                        .attr("d", line(converted[i]))
                        .style("stroke", this.colors[i % this.colors.length]);
                    if (converted[i].length < 2000) {
                        svg.selectAll(".dot")
                            .data(converted[i])
                            .enter().append("circle")
                            .style("stroke", this.colors[i % this.colors.length])
                            .style("fill", "white")
                            .attr("class", "dot")
                            .attr("r", 3.5)
                            .attr("cx", function (d) { return scaleHorizontal(d.x); })
                            .attr("cy", function (d) { return scaleVertical(d.y); });
                    }
                }
            }
        },
        wheel: function (event) {
            console.log(event);
            this.left += -(event.deltaY / 100) * 0.8;
            this.right -= -(event.deltaY / 100) * 0.8;
        }
    }
}