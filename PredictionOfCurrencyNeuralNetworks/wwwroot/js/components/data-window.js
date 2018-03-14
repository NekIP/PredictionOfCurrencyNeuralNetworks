import RequestApi from '../../api/request-api.js';

export default {
    props: ["name", "code"],
    data: function () {
        return {
            show: false,
            wasInit: false,
            graphic: false,
            items: [],
            itemsOnCurrentPage: [],
            chunk: 1000,
            currentPage: undefined,
            countPages: undefined,
            entry: {
                date: "2018-01-01",
                time: "00:00",
                value: 0.0
            }
        };
    },
    computed: {
        graphicWidth: function () {
            let dataManagerCoef = window.innerWidth< 1200 ? 1 : 0.9;
            let width1 = window.innerWidth * dataManagerCoef - 30;
            return width1 * 0.7 - 40;
        }
    },
    methods: {
        showHideGraphic: function () {
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
            console.log(data);
            console.log(minMax);
            let y2 = minMax.minValue;
            let y1 = minMax.maxValue;
            let a = new Date(minMax.minDate).getTime();
            let b = new Date(minMax.maxDate).getTime();
            let i1 = 25;
            let j1 = 25;
            let i2 = $("#" + this.code + '-graphic').width() - 100;
            let j2 = $("#" + this.code + '-graphic').height() - 50;
            let xScreen = x => i1 + Math.trunc((x - a) * (i2 - i1) / (b - a));
            let yScreen = y => j1 + Math.trunc((y - y1) * (j2 - j1) / (y2 - y1));

            //let i = 0;
            var line = d3.line()
                .x(function (d) { return xScreen(d.x); })
                .y(function (d) { return yScreen(d.y); });

            this.graphic = !this.graphic;
            let pathC = [
                { x: 1, y: 3 },
                { x: 2, y: 4 },
                { x: 3, y: 2 },
                { x: 4, y: 6 },
                { x: 5, y: 2 },
                { x: 6, y: 9 },
            ];
            let x = [1, 2, 3, 4, 5, 6];
            let y = [2, 3, 4, 5, 6, 7];

            var axisWidth = $("#" + this.code + '-graphic').width() - 100; 
            var offset = 25;

            var svg = d3.select("#" + this.code + '-graphic').append("svg");
            console.log(axisWidth);

            var scale = d3.scaleTime() // от 1 января 2015 года до текущей даты
                .domain([new Date(minMax.minDate), new Date(minMax.maxDate)])
                .range([25, axisWidth]);

            var axis = d3.axisBottom()
                .scale(scale)
                //.orient('bottom')
                .ticks(12);
                //.tickFormat(d3.timeFormat('%d.%m'));

            svg.append("g")
                //.attr("transform", "translate(" + 2 * xScreen(a) + "," + offset + ")")
                .call(axis);



            var width = 400,
                height = 400;
            /*svg.attr("height", height)
                .attr("width", width);*/

            // добавляем путь
            //let data2 = this.itemsOnCurrentPage.slice(0, 1000);
            svg.append("path").attr("d", line(data));
            /*
            var phonesList = d3.select('.graphic')
                .selectAll('div')
                .data(x)
                .text(function (d) { return d; })
                .enter()
                .append('div')
                .text(function (d) { return d; });
            */
            /*phonesList.enter()
                .append('div')
                .text(function (d) { return d; });*/
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
                this.items = data;
                this.currentPage = 1;
                this.countPages = Math.ceil(this.items.length / this.chunk);
                this.initItemsOnCurrentPage();
                this.wasInit = true;
            }
        },
        leftAvailable: function () {
            return this.currentPage > 1;
        },
        rightAvailable: function () {
            return this.currentPage < this.countPages;
        },
        movePage: function (direction) {
            this.incrementCurrentPage(direction);
            this.initItemsOnCurrentPage();
        },
        initItemsOnCurrentPage: function () {
            this.itemsOnCurrentPage = [];
            let items = this.getItemsOnCurrentPage();
            for (let i = 0; i < items.length; i++) {
                this.itemsOnCurrentPage.push({
                    id: items[i].id,
                    date: items[i].date,
                    value: items[i].value,
                    update: false
                });
            }
        },
        scroll: function (event) {
            if (this.currentPage < this.countPages 
                && event.target.scrollTop + event.target.scrollWidth > event.target.scrollHeight - event.target.scrollWidth) {
                this.incrementCurrentPage(1);
                let forPush = this.getItemsOnCurrentPage();
                for (let i = 0; i < forPush.length; i++) {
                    this.itemsOnCurrentPage.push({
                        id: forPush[i].id,
                        date: forPush[i].date,
                        value: forPush[i].value,
                        update: false
                    });
                }
            }
        },
        incrementCurrentPage: function (value) {
            var computed = +this.currentPage + +value;
            if (computed <= this.countPages && computed > 0) {
                this.currentPage = computed;
            }
        },
        getItemsOnCurrentPage: function () {
            return this.items.slice(this.chunk * (this.currentPage - 1),
                this.chunk * this.currentPage >= this.items.length
                    ? this.items.length
                    : this.chunk * this.currentPage);
        }
    }
}