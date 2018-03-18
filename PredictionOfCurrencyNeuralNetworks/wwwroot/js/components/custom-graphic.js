export default {
    props: ["code", "data", "graphicWidth", "graphic"],
    data: {
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
        ]
    },
    watch: {
        graphic: function (val) {
            this.showHideGraphic();
        }
    },
    methods: {
        showHideGraphic: function () {
            d3.select("#" + this.code + '-graphic').selectAll("svg").remove();
            if (this.graphic) {
                let minMax = {
                    minDate: undefined,
                    maxDate: undefined,
                    minValue: undefined,
                    maxValue: undefined
                };
                let converted = [];
                for (let i = 0; i < this.data.length; i++) {
                    let item = {
                        id: this.data[i].id,
                        date: this.data[i].values[0],
                        values: this.data[i].values.slice(1)
                    };
                    if (!minMax.minDate
                        || new Date(minMax.minDate).getTime() > new Date(item.date).getTime()) {
                        minMax.minDate = item.date;
                    }
                    if (!minMax.maxDate
                        || new Date(minMax.maxDate).getTime() < new Date(item.date).getTime()) {
                        minMax.maxDate = item.date;
                    }
                    for (let j = 0; j < item.values.length; j++) {
                        if (!minMax.minValue || minMax.minValue > item.values[j]) {
                            minMax.minValue = item.values[j];
                        }
                        if (!minMax.maxValue || minMax.maxValue < item.values[j]) {
                            minMax.maxValue = item.values[j];
                        }
                    }
                    for (let j = 0; j < item.values.length; j++) {
                        if (j + 1 > converted.length) {
                            converted.push([]);
                        }
                        converted[j].push({ x: new Date(item.date).getTime(), y: item.values[j] });
                    }
                }
                let y2 = minMax.minValue;
                let y1 = minMax.maxValue;
                let a = new Date(minMax.minDate).getTime();
                let b = new Date(minMax.maxDate).getTime();
                let i1 = 50;
                let j1 = 25;
                let i2 = $("#" + this.code + '-graphic').width() - 100;
                let j2 = $("#" + this.code + '-graphic').height() - 50;
                let xScreen = x => i1 + Math.trunc((x - a) * (i2 - i1) / (b - a));
                let yScreen = y => j1 + Math.trunc((y - y1) * (j2 - j1) / (y2 - y1));
                let line = d3.line()
                    .x(function (d) { return xScreen(d.x); })
                    .y(function (d) { return yScreen(d.y); });
                let svg = d3.select("#" + this.code + '-graphic').append("svg");
                let scaleHorizontal = d3.scaleTime()
                    .domain([new Date(minMax.minDate), new Date(minMax.maxDate)])
                    .range([i1, i2]);
                let scaleVertical = d3.scaleLinear()
                    .domain([minMax.minValue, minMax.maxValue])
                    .range([j2, j1]);
                let axisHorizontal = d3.axisBottom()
                    .scale(scaleHorizontal)
                    .ticks(12);
                let axisVertical = d3.axisLeft()
                    .scale(scaleVertical)
                    .ticks(24);
                svg.append("g")
                    .attr("transform", "translate(" + 0 + "," + j1 + ")")
                    .call(axisHorizontal);
                svg.append("g")
                    .attr("transform", "translate(" + i1 + "," + 0 + ")")
                    .call(axisVertical);
                for (let i = 0; i < converted.length; i++) {
                    svg.append("path")
                        .attr("d", line(converted[i]))
                        .style("stroke", this.colors[i % this.color.length]);
                }
            }
        }
    }
}