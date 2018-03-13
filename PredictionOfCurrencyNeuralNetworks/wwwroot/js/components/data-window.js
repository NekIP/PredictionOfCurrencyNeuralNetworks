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
            countPages: undefined
        };
    },
    methods: {
        showHide: function () {
            this.show = !this.show;
            if (this.show && !this.wasInit) {
                this.downloadData();
            }
        },
        downloadData: function () {
            let loadItems = new RequestApi("DataManager/Load", 'GET');
            loadItems.execute({ code: this.code }, this.addData);
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
            this.itemsOnCurrentPage = this.getItemsOnCurrentPage();
        },
        scroll: function (event) {
            if (this.currentPage < this.countPages 
                && event.target.scrollTop + event.target.scrollWidth > event.target.scrollHeight - event.target.scrollWidth) {
                this.incrementCurrentPage(1);
                let forPush = this.getItemsOnCurrentPage();
                for (var i = 0; i < forPush.length; i++) {
                    this.itemsOnCurrentPage.push(forPush[i]);
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
                    ? this.items.length - 1
                    : this.chunk * this.currentPage);
        }
    }
}