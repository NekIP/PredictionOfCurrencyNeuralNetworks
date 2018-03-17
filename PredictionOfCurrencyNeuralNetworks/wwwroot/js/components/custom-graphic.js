export default {
    props: ["code", "data", "graphicWidth", "graphic"],
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
                for (var i = 0; i < this.data.length; i++) {
                    var item = {
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
                    if (!minMax.minValue || minMax.minValue > item.values[0]) {
                        minMax.minValue = item.values[0];
                    }
                    if (!minMax.maxValue || minMax.maxValue < item.values[0]) {
                        minMax.maxValue = item.values[0];
                    }
                    converted.push({ x: new Date(item.date).getTime(), y: item.values[0] });
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
                var line = d3.line()
                    .x(function (d) { return xScreen(d.x); })
                    .y(function (d) { return yScreen(d.y); });
                var svg = d3.select("#" + this.code + '-graphic').append("svg");
                var scaleHorizontal = d3.scaleTime()
                    .domain([new Date(minMax.minDate), new Date(minMax.maxDate)])
                    .range([i1, i2]);
                var scaleVertical = d3.scaleLinear()
                    .domain([minMax.minValue, minMax.maxValue])
                    .range([j2, j1]);
                var axisHorizontal = d3.axisBottom()
                    .scale(scaleHorizontal)
                    .ticks(12);
                var axisVertical = d3.axisLeft()
                    .scale(scaleVertical)
                    .ticks(24);
                svg.append("g")
                    .attr("transform", "translate(" + 0 + "," + j1 + ")")
                    .call(axisHorizontal);
                svg.append("g")
                    .attr("transform", "translate(" + i1 + "," + 0 + ")")
                    .call(axisVertical);
                svg.append("path").attr("d", line(converted));
            }
        }
    }
}