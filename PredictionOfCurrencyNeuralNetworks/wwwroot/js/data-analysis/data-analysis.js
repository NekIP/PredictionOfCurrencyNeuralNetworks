import RequestApi from '../../api/request-api.js';

new Vue({
    el: ".data-analysis",
    data: function () {
        return {
            wasInitFit: false,
            wasInitGraphic: false,
            data: [],
            dataWithoutInput: [],
            errors: []
        }
    },
    methods: {
        fit: function () {
            this.wasInitFit = false;
            this.wasInitGraphic = false;
            let fit = new RequestApi("/DataAnalysis/Fit", "GET");
            fit.execute({}, this.fitSuccess);
        },
        fitSuccess: function (data) {
            if (data && data.length) {
                for (let i = 0; i < data.length; i++) {
                    this.data.push({
                        id: i,
                        values: [data[i].date, data[i].input[7], data[i].output[0], data[i].ideal[0]]
                    });
                    this.dataWithoutInput.push({
                        id: i,
                        values: [data[i].date, data[i].output[0], data[i].ideal[0]]
                    });
                    this.errors.push({
                        id: i,
                        values: [data[i].date, data[i].error[0]]
                    });
                }
                this.wasInitFit = true;
                this.wasInitGraphic = true;
            }
        },
        gWidth: function () {
            let dataManagerCoef = window.innerWidth < 1200 ? 0.97 : 0.89;
            return window.innerWidth * dataManagerCoef;
        }
    }
})