using DataManager;
using NeuralNetwork;
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
                },
                {
                    "defaultAbsolute8For15Day",
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
                        neuralNetworkName: "defaultAbsolute8For15Day",
                        dataType: DataValueType.Absolute,
                        dataProcessingMethods: DataProcessingMethods.Normalize,
                        dataParameters: new DataParameter(new DateTime(2008, 1, 1), new DateTime(2018, 2, 1), TimeSpan.FromDays(15))
                    )
                },
                {
                    "defaultAbsolute7ForTenDay",
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
                        neuralNetworkName: "defaultAbsolute7ForTenDay",
                        dataType: DataValueType.Absolute,
                        dataProcessingMethods: DataProcessingMethods.Normalize,
                        dataParameters: new DataParameter(new DateTime(2007, 12, 1), new DateTime(2018, 2, 1), TimeSpan.FromDays(10))
                    )
                },
                {
                    "defaultOneLayerOneDay",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            dowJones,
                            gold,
                            mmvb,
                            oliBrent,
                            oliLight,
                            usdToRub
                        },
                        neuralNetworkName: "defaultOneLayerOneDay",
                        dataType: DataValueType.Absolute,
                        dataProcessingMethods: DataProcessingMethods.Normalize,
                        dataParameters: new DataParameter(new DateTime(2007, 12, 3), new DateTime(2018, 2, 1), TimeSpan.FromDays(1)),
                        learnParameters: new LearnParameters {
                            RecurentParameters = new RecurentParameters(1.2, 0.5, 0.1),
                            RecurentCellParameters = new NeuralNetwork.RecurentCellParameters[] {
                                new NeuralNetwork.RecurentCellParameters(6, 1)
                            }
                        }
                    )
                },
                {
                    "defaultOneLayer1OneDay",
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
                        neuralNetworkName: "defaultOneLayer1OneDay",
                        dataType: DataValueType.Absolute,
                        dataProcessingMethods: DataProcessingMethods.Normalize,
                        dataParameters: new DataParameter(new DateTime(2007, 12, 4), new DateTime(2018, 2, 1), TimeSpan.FromDays(1)),
                        learnParameters: new LearnParameters {
                            RecurentParameters = new RecurentParameters(1.2, 0.5, 0.1),
                            RecurentCellParameters = new RecurentCellParameters[] {
                                new RecurentCellParameters(9, 1)
                            }
                        }
                    )
                },
                {
                    "defaultOneLayer1OneDayDataForOneYear",
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
                        neuralNetworkName: "defaultOneLayer1OneDayDataForOneYear",
                        dataType: DataValueType.Absolute,
                        dataProcessingMethods: DataProcessingMethods.Normalize,
                        dataParameters: new DataParameter(new DateTime(2017, 2, 1), new DateTime(2018, 2, 1), TimeSpan.FromDays(1)),
                        learnParameters: new LearnParameters {
                            RecurentParameters = new RecurentParameters(1.2, 0.5, 0.1),
                            RecurentCellParameters = new RecurentCellParameters[] {
                                new RecurentCellParameters(9, 1)
                            }
                        }
                    )
                },
                {
                    "defaultOneYear",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            cac40,
                            dowJones,
                            gdpPerCapitaPpp,
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
                        neuralNetworkName: "defaultOneYear",
                        dataType: DataValueType.Absolute,
                        dataProcessingMethods: DataProcessingMethods.Normalize,
                        dataParameters: new DataParameter(new DateTime(2008, 1, 1), new DateTime(2018, 1, 1), TimeSpan.FromDays(366))
                    )
                },
                {
                    "default1ForOneMonthWithout",
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
                        neuralNetworkName: "default1ForOneMonthWithout",
                        dataParameters: new DataParameter(new DateTime(2008, 4, 1), new DateTime(2018, 2, 1), TimeSpan.FromDays(31))
                    )
                },
                {
                    "allCollectors10Days",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            cac40,
                            dowJones,
                            gdpPerCapitaPpp,
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
                        neuralNetworkName: "allCollectors10Days",
                        dataParameters: new DataParameter(new DateTime(2008, 2, 1), new DateTime(2018, 2, 1), TimeSpan.FromDays(10))
                    )
                }
            };
        }
    }
}
