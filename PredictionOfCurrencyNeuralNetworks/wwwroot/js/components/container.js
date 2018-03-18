export default {
    props: ["name", "callback"],
    data: function () {
        return {
            show: false
        };
    },
    methods: {
        showHide: function () {
            this.show = !this.show;
            if (this.show) {
                this.callback();
            }
        }
    }
}