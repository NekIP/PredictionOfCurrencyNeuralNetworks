import dataManagerApi from "./data-manager-api.js";
import RequestApi from '../../api/request-api.js';

new Vue({
    el: ".data-manager",
    data: function () {
        return {
            wasInit: false,
            wasInitRawData: false,
            wasInitNormalizedData: false,
            graphicRawData: false,
            graphicNormalizedData: false,
            rawData: [],
            normalizedData: [],
            info: {
                from: undefined,
                to: undefined,
                count: 0,
                expectedValues: [],
                dispersions: [],
                mins: [],
                maxs: [],
                fieldsNames: []
            }
        }
    },
    methods: {
        showHideGraphicRawData: function () {
            this.graphicRawData = !this.graphicRawData;
        },
        showHideGraphicNormalizedData: function () {
            this.graphicNormalizedData = !this.graphicNormalizedData;
        },
        gWidth: function () {
            let dataManagerCoef = window.innerWidth < 1200 ? 0.97 : 0.89;
            return window.innerWidth * dataManagerCoef;
        },
        downloadInfo: function () {
            if (!this.wasInit) {
                let downloadInfo = new RequestApi("/DataManager/GetDataForNeuralNetworkInformation", "GET");
                downloadInfo.execute({ }, this.downloadInfoSuccess);
            }
        },
        downloadRawData: function () {
            if (!this.wasInitRawData) {
                let downloadRawData = new RequestApi("/DataManager/GetDataForNeuralNetwork", "GET");
                downloadRawData.execute({ }, this.downloadRawDataSuccess);
            }
        },
        downloadNormalizedData: function () {
            if (!this.wasInitNormalizedData) {
                let downloadNormalizedData = new RequestApi("/DataManager/GetDataForNeuralNetworkNormalized", "GET");
                downloadNormalizedData.execute({ }, this.downloadNormalizedDataSuccess);
            }
        },
        downloadInfoSuccess: function (data) {
            if (data) {
                this.info = data;
            }
            this.wasInit = true;
        },
        downloadRawDataSuccess: function (data) {
            if (data && data.length > 0) {
                this.rawData = [];
                for (let i = 0; i < data.length; i++) {
                    this.rawData.push({
                        id: data[i].id,
                        values: [data[i].date].concat(data[i].data.map((x, y, z) => x.toFixed(8)))
                    });
                }
            }
            this.wasInitRawData = true;
        },
        downloadNormalizedDataSuccess: function (data) {
            if (data && data.length > 0) {
                this.normalizedData = [];
                for (let i = 0; i < data.length; i++) {
                    this.normalizedData.push({
                        id: data[i].id,
                        values: [data[i].date].concat(data[i].data.map((x, y, z) => x.toFixed(8)))
                    });
                }
            }
            this.wasInitNormalizedData = true;
        }
    }
})