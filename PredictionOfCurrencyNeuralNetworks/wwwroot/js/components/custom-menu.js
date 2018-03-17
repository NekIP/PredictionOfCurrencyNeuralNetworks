export default {
    data: function () {
        return {
            show: false
        }
    },
    methods: {
        showHide: function () {
            this.show = !this.show;
        }
    }
}