import RequestApi from '../../api/request-api.js';

export default {
    props: ["name", "code"],
    data: function () {
        return {
            show: false,
            wasInit: false,
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
    methods: {
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