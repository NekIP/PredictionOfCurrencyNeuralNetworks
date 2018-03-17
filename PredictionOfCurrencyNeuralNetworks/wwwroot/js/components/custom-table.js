import RequestApi from '../../api/request-api.js';

export default {
    props: ["code", "data", "fieldsNames", "removeEntry"],
    data: function () {
        return {
            itemsOnCurrentPage: [],
            chunk: 1000,
            currentPage: undefined,
            countPages: undefined
        };
    },
    watch: {
        data: function (val) {
            this.currentPage = 1;
            this.countPages = Math.ceil(this.data.length / this.chunk);
            this.initItemsOnCurrentPage();
        }
    },
    methods: {
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
                    values: items[i].values,
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
                        values: forPush[i].values,
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
            return this.data.slice(this.chunk * (this.currentPage - 1),
                this.chunk * this.currentPage >= this.data.length
                    ? this.data.length
                    : this.chunk * this.currentPage);
        }
    }
}