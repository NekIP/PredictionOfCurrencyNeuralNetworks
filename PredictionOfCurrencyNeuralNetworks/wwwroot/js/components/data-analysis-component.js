import RequestApi from '../../api/request-api.js';

export default {
    props: ["systemName"],
    data: function () {
        return {
            wasInit: false,
            wasInitGraphic: false,
            data: {
                meanErrorForLearnSet: 0,
                meanErrorForTestSet: 0,
                efficiencyForLearnSet: 0,
				efficiencyTailForLearnSet: 0,
				efficiencyTailForTestSet: 0,
                efficiencyForTestSet: 0,
                inputData: [],
                descriptionSystem: [],
                learnResult: [],
                learnResultWithoutInput: [],
                learnResultErrors: [],
				testResult: [],
				testResultWithoutInput: []
            }
        }
    },
    methods: {
        info: function () {
            if (!this.wasInit) {
                this.wasInit = false;
                this.wasInitGraphic = false;
                let info = new RequestApi("/DataAnalysis/Info", "GET");
                info.execute({ systemName: this.systemName }, this.infoSuccess);
            }
        },
        infoSuccess: function (data) {
            if (data) {
                this.data.meanErrorForLearnSet = data.meanErrorForLearnSet;
                this.data.meanErrorForTestSet = data.meanErrorForTestSet;
                this.data.efficiencyForLearnSet = data.efficiencyForLearnSet;
                this.data.efficiencyForTestSet = data.efficiencyForTestSet;
                this.data.descriptionSystem = data.descriptionSystem;
				this.data.efficiencyTailForLearnSet = data.efficiencyTailForLearnSet;
				this.data.efficiencyTailForTestSet = data.efficiencyTailForTestSet;
                for (let i = 0; i < data.learnResult.length; i++) {
                    let item = data.learnResult[i];
                    this.data.learnResult.push({
                        id: i,
                        values: [item.date, item.input[item.input.length - 1], item.output[0], item.ideal[0]]
                    });
                    this.data.learnResultWithoutInput.push({
                        id: i,
                        values: [item.date, item.output[0], item.ideal[0]]
                    });
                    this.data.learnResultErrors.push({
                        id: i,
                        values: [item.date, item.error[0]]
                    });
				}
				for (let i = 1; i < data.testResult.length; i++) {
					let item = data.testResult[i];
					this.data.testResult.push({
						id: i,
						values: [item.date, item.input[item.input.length - 1], item.output[0], item.ideal[0]]
					});
					this.data.testResultWithoutInput.push({
						id: i,
						values: [item.date, data.testResult[i - 1].output[0], item.ideal[0]]
					});
				}
                for (let i = 0; i < data.inputData.length; i++) {
                    let item = data.inputData[i];
                    this.data.inputData.push({
                        id: i,
                        values: [item.date].concat(item.data)
                    });
                }
                this.wasInit = true;
                this.wasInitGraphic = true;
            }
        },
        gWidth: function () {
            let dataManagerCoef = window.innerWidth < 1200 ? 0.97 : 0.89;
            return window.innerWidth * dataManagerCoef;
        }
    }
}