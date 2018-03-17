import RequestApi from '../../api/request-api.js';

export default {
    props: ["name", "code"],
    data: function () {
        return {
            show: false,
            wasInit: false,
            graphic: false,
            items: [],
            itemsTest: [], ////---------------------
            /*itemsOnCurrentPage: [],
            chunk: 1000,
            currentPage: undefined,
            countPages: undefined,*/
            entry: {
                date: "2018-01-01",
                time: "00:00",
                value: 0.0
            }
        };
    },
    methods: {
        showHideGraphic: function () {
            this.itemsTest.push("test");
            d3.select("#" + this.code + '-graphic').selectAll("svg").remove();
            let minMax = {
                minDate: undefined,
                maxDate: undefined,
                minValue: undefined,
                maxValue: undefined
            };
            let data = [];
            for (var i = 0; i < this.items.length; i++) {
                var item = this.items[i];
                if (!minMax.minDate
                    || new Date(minMax.minDate).getTime() > new Date(item.date).getTime()) {
                    minMax.minDate = item.date;
                }
                if (!minMax.maxDate
                    || new Date(minMax.maxDate).getTime() < new Date(item.date).getTime()) {
                    minMax.maxDate = item.date;
                }
                if (!minMax.minValue || minMax.minValue > item.value) {
                    minMax.minValue = item.value;
                }
                if (!minMax.maxValue || minMax.maxValue < item.value) {
                    minMax.maxValue = item.value;
                }
                data.push({ x: new Date(item.date).getTime(), y: item.value });
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
            svg.append("path").attr("d", line(data));
            this.graphic = !this.graphic;
        },
        graphicWidth: function () {
            let dataManagerCoef = window.innerWidth < 1200 ? 1 : 0.9;
            let width1 = window.innerWidth * dataManagerCoef - 30;
            return width1 * 0.7 - 40;
        },
        showHide: function () {
            this.show = !this.show;
            if (this.show && !this.wasInit) {
                this.downloadData();
            }
        },
        prepareData: function () {
            let prepareData = new RequestApi("DataManager/PrepareData", 'POST');
            this.wasInit = false;
            prepareData.execute({
                code: this.code
            }, this.prepareDataSuccess);
        },
        addEntry: function () {
            let dateStr = this.entry.date + "T" + this.entry.time;
            let addEntry = new RequestApi("DataManager/Add", 'POST');
            this.wasInit = false;
            addEntry.execute({
                code: this.code,
                dateStr: dateStr,
                value: ("" + this.entry.value).replace(".", ",")
            }, this.addEntrySuccess);
        },
        removeEntry: function (id) {
            let removeEntry = new RequestApi("DataManager/Remove", 'POST');
            this.wasInit = false;
            removeEntry.execute({
                code: this.code,
                id: id
            }, this.removeEntrySuccess);
        },
        updateEntry: function (item) {
            if (!item.update) {
                item.update = true;
            }
            else {
                this.wasInit = false;
                item.update = false;
                let updateEntry = new RequestApi("DataManager/Update", 'POST');
                updateEntry.execute({
                    code: this.code,
                    id: item.id,
                    value: ("" + item.value).replace(".", ",")
                }, this.requestSuccess);
            }
        },
        downloadData: function () {
            let loadItems = new RequestApi("DataManager/Load", 'GET');
            loadItems.execute({ code: this.code }, this.addData);
        },
        prepareDataSuccess: function (data) {
            this.downloadData();
        },
        addEntrySuccess: function (data) {
            this.downloadData();
        },
        removeEntrySuccess: function (data) {
            this.downloadData();
        },
        requestSuccess: function (data) {
            this.wasInit = true;
        },
        addData: function (data) {
            if (data && data.length > 0) {
                this.items = [];
                for (let i = 0; i < data.length; i++) {
                    this.items.push({ id: data[i].id, values: [data[i].date, data[i].value] });
                }
                this.wasInit = true;
            }
        }
    }
}