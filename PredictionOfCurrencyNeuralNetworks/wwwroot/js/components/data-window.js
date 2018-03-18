import RequestApi from '../../api/request-api.js';

export default {
    props: ["name", "code"],
    data: function () {
        return {
            wasInit: false,
            graphic: false,
            items: [],
            entry: {
                date: "2018-01-01",
                time: "00:00",
                value: 0.0
            },
            info: {
                from: undefined,
                to: undefined,
                count: 0,
                expectedValue: 0,
                dispersion: 0
            }
        };
    },
    methods: {
        showHideGraphic: function () {
            this.graphic = !this.graphic;
        },
        graphicWidth: function () {
            let dataManagerCoef = window.innerWidth < 1200 ? 1 : 0.9;
            let dataManagerCoef1 = window.innerWidth < 1300 ? 0.52 : 0.51;
            let width1 = window.innerWidth * dataManagerCoef - 30;
            return width1 * dataManagerCoef1 - 40;
        },
        callback: function () {
            if (!this.wasInit) {
                this.downloadData();
            }
        },
        prepareData: function () {
            if (confirm("Данная операция загружает со строннего источника все недостающие данные. Для некоторых таблиц она занимаеть долгое время. Вы хотите продолжить?")) {
                let prepareData = new RequestApi("DataManager/PrepareData", 'POST');
                this.wasInit = false;
                prepareData.execute({
                    code: this.code
                }, this.prepareDataSuccess);
            }
        },
        addEntry: function () {
            if (confirm("Добавить запись?")) {
                let dateStr = this.entry.date + "T" + this.entry.time;
                let addEntry = new RequestApi("DataManager/Add", 'POST');
                this.wasInit = false;
                addEntry.execute({
                    code: this.code,
                    dateStr: dateStr,
                    value: ("" + this.entry.value).replace(".", ",")
                }, this.addEntrySuccess);
            }
        },
        removeEntry: function (id) {
            if (confirm("Удалить запись?")) {
                let removeEntry = new RequestApi("DataManager/Remove", 'POST');
                this.wasInit = false;
                removeEntry.execute({
                    code: this.code,
                    id: id
                }, this.removeEntrySuccess);
            }
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
            if (data && data.values.length > 0) {
                this.items = [];
                this.info.from = undefined;
                this.info.to = undefined;
                this.info.count = data.values.length;
                this.info.expectedValue = data.expectedValue;
                this.info.dispersion = data.dispersion;
                for (let i = 0; i < data.values.length; i++) {
                    this.items.push({ id: data.values[i].id, values: [data.values[i].date, data.values[i].value] });
                    if (!this.info.from
                        || new Date(this.info.from).getTime() > new Date(data.values[i].date).getTime()) {
                        this.info.from = data.values[i].date;
                    }
                    if (!this.info.to
                        || new Date(this.info.to).getTime() < new Date(data.values[i].date).getTime()) {
                        this.info.to = data.values[i].date;
                    }
                }
                this.wasInit = true;
            }
        }
    }
}