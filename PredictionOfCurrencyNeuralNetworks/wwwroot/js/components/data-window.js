import RequestApi from '../../api/request-api.js';

export default {
    props: ["name", "code"],
    data: function () {
        return {
            show: false,
            wasInit: false,
            graphic: false,
            items: [],
            entry: {
                date: "2018-01-01",
                time: "00:00",
                value: 0.0
            }
        };
    },
    methods: {
        showHideGraphic: function () {
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