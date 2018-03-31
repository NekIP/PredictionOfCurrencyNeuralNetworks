using DataManager;
using System;
using System.Collections.Generic;

namespace Business {
    public interface IPredictionOfCurrencyManager {
        Dictionary<string, PredictionOfCurrency> PredictionOfCurrency { get; set; }
    }
    public class PredictionOfCurrencyManager : IPredictionOfCurrencyManager {
        public Dictionary<string, PredictionOfCurrency> PredictionOfCurrency { get; set; }
        public PredictionOfCurrencyManager(ICAC40Collector cac40,
            IDowJonesCollector dowJones,
            IGdpPerCapitaPppCollector gdpPerCapitaPpp,
            IGoldCollector gold,
            IInflationCollector inflation,
            IMMVBCollector mmvb,
            IOliBrentCollector oliBrent,
            IOliLightCollector oliLight,
            IRefinancingRateCollector refinancingRate,
            IRTSCollector rts,
            ISAndP500Collector sAndP,
            ITradeBalanceCollector tradeBalance,
            IUsdToRubCurrencyCollector usdToRub) {
            PredictionOfCurrency = new Dictionary<string, PredictionOfCurrency> {
                {
                    "default",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            dowJones, 
                            gold,
                            mmvb,
                            oliBrent,
                            oliLight,
                            usdToRub
                        }
                    )
                },
                {
                    "defaultRelativeForOneDay",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            dowJones,
                            gold,
                            mmvb,
                            oliBrent,
                            oliLight,
                            usdToRub
                        },
                        neuralNetworkName: "relativeForOneDay",
                        dataType: DataValueType.Relative
                    )
                },
                {
                    "defaultAbsolute5ForOneDay",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            dowJones,
                            gold,
                            mmvb,
                            oliBrent,
                            oliLight,
                            usdToRub
                        },
                        neuralNetworkName: "absoluteFor6OneDay",
                        dataType: DataValueType.Relative,
                        dataProcessingMethods: DataProcessingMethods.Normalize,
                        learnParameters: new LearnParameters { RecurentParameters = new NeuralNetwork.RecurentParameters(2, 2, 0.3) }
                    )
                },
                {
                    "defaultRelativePercentageForOneDay",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            dowJones,
                            gold,
                            mmvb,
                            oliBrent,
                            oliLight,
                            usdToRub
                        },
                        neuralNetworkName: "relativePercentageForOneDay",
                        dataType: DataValueType.RelativePercentage
                    )
                },
                {
                    "defaultRelative1ForOneDay",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            //dowJones,
                            gold,
                            //mmvb,
                            oliBrent,
                            oliLight,
                            //rts,
                            usdToRub
                        },
                        neuralNetworkName: "relative1ForOneDay",
                        dataType: DataValueType.Relative,
                        dataParameters: new DataParameter(new DateTime(2014, 1, 1), new DateTime(2018, 3, 10), TimeSpan.FromDays(1))
                    )
                },
                {
                    "default1ForOneMonth",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            cac40,
                            dowJones,
                            gold,
                            inflation,
                            mmvb,
                            oliBrent,
                            oliLight,
                            refinancingRate,
                            rts,
                            sAndP,
                            tradeBalance,
                            usdToRub
                        },
                        neuralNetworkName: "default1ForOneMonth",
                        dataParameters: new DataParameter(new DateTime(2008, 1, 1), new DateTime(2018, 2, 1), TimeSpan.FromDays(31))
                    )
                },
                {
                    "defaultRelative6ForOneDay",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            cac40,
                            dowJones,
                            gold,
                            mmvb,
                            oliBrent,
                            oliLight,
                            rts,
                            sAndP,
                            usdToRub
                        },
                        neuralNetworkName: "defaultRelative6ForOneDay",
                        dataType: DataValueType.Relative,
                        dataProcessingMethods: DataProcessingMethods.Normalize,
                        dataParameters: new DataParameter(new DateTime(2008, 1, 1), new DateTime(2018, 2, 1), TimeSpan.FromDays(1))
                    )
                }
            };
        }
    }
}
