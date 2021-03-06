﻿using DataManager;
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
                        learnParameters: new LearnParameters { Parameters = new NeuralNetwork.RecurentParameters(2, 2, 0.3) }
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
                            Parameters = new RecurentParameters(1.2, 0.5, 0.1),
                            CellParameters = new NeuralNetwork.RecurentCellParameters[] {
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
                            Parameters = new RecurentParameters(1.2, 0.5, 0.1),
                            CellParameters = new RecurentCellParameters[] {
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
                            Parameters = new RecurentParameters(1.2, 0.5, 0.1),
                            CellParameters = new RecurentCellParameters[] {
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
                },
                {
                    "allCollectors1Day",
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
                        neuralNetworkName: "allCollectors1Day",
                        dataParameters: new DataParameter(new DateTime(2007, 12, 3), new DateTime(2018, 1, 3), TimeSpan.FromDays(1))
                    )
                },
                {
                    "test1Day",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            //cac40,
                            //dowJones,
                            gdpPerCapitaPpp,
                            gold,
                            inflation,
                            mmvb,
                            oliBrent,
                            //oliLight,
                            refinancingRate,
                            //rts,
                            //sAndP,
                            tradeBalance,
                            usdToRub
                        },
                        neuralNetworkName: "test1Day",
                        dataParameters: new DataParameter(new DateTime(2007, 12, 2), new DateTime(2018, 1, 3), TimeSpan.FromDays(1))
                    )
                },
                {
                    "test4Day",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            //cac40,
                            dowJones,
                            gdpPerCapitaPpp,
                            gold,
                            inflation,
                            mmvb,
                            oliBrent,
                            //oliLight,
                            refinancingRate,
                            rts,
                            //sAndP,
                            tradeBalance,
                            usdToRub
                        },
                        neuralNetworkName: "test4Day",
                        dataParameters: new DataParameter(new DateTime(2007, 12, 8), new DateTime(2018, 1, 4), TimeSpan.FromDays(1)),
                        learnParameters: new LearnParameters {
                            Parameters = new RecurentParameters(1.2, 0.5, 0.1),
                            CellParameters = new RecurentCellParameters[] {
                                new RecurentCellParameters(10, 1)
                            }
                        }
                    )
                },
                {
                    "test6Day",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            //cac40,
                            //dowJones,
                            //gdpPerCapitaPpp,
                            //gold,
                            inflation,
                            //mmvb,
                            oliBrent,
                            //oliLight,
                            //refinancingRate,
                            //rts,
                            //sAndP,
                            tradeBalance,
                            usdToRub
                        },
                        neuralNetworkName: "test6Day",
                        dataParameters: new DataParameter(new DateTime(2007, 12, 12), new DateTime(2014, 1, 3), TimeSpan.FromDays(1))
                        /*learnParameters: new LearnParameters {
                            RecurentParameters = new RecurentParameters(1.2, 0.5, 0.1),
                            RecurentCellParameters = new RecurentCellParameters[] {
                                new RecurentCellParameters(3, 1)
                            }
                        },*/
                    )
                },
                {
                    "testSimpleNeuralNetwork",
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
                        neuralNetworkName: "testSimpleNeuralNetwork",
                        dataParameters: new DataParameter(new DateTime(2007, 12, 13), new DateTime(2014, 1, 3), TimeSpan.FromDays(1)),
                        learnParameters: new LearnParameters {
                            Parameters = new RecurentParameters(1, 0.5, 0.1),
                            CellParameters = new RecurentCellParameters[] {
                                new RecurentCellParameters(13, 13),
                                new RecurentCellParameters(13, 1),
                            }
                        },
                        usingNeuralNetwork: UsingNeuralNetwork.SimpleNeuralNetwork
                    )
                },
                {
                    "lstmNetworkChunk2OneDay",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            //cac40,
                            //dowJones,
                            //gdpPerCapitaPpp,
                            //gold,
                            inflation,
                            //mmvb,
                            oliBrent,
                            //oliLight,
                            //refinancingRate,
                            //rts,
                            //sAndP,
                            //tradeBalance,
                            usdToRub
                        },
                        neuralNetworkName: "lstmNetworkChunk2OneDay",
                        dataParameters: new DataParameter(new DateTime(2007, 12, 14), new DateTime(2014, 1, 3), TimeSpan.FromDays(1)),
                        learnParameters: new LearnParameters {
                            Parameters = new RecurentParameters(1, 0.5, 0.1),
                            CellParameters = new RecurentCellParameters[] {
                                new RecurentCellParameters(3, 1)
                            }
                        },
                        chunk: 2
                    )
                },
                {
                    "lstmNetworkChunk10000OneDay",
                    new PredictionOfCurrency(
                        collectors: new List<IDataCollector>{
                            //cac40,
                            //dowJones,
                            gdpPerCapitaPpp,
                            gold,
                            inflation,
                            //mmvb,
                            oliBrent,
                            //oliLight,
                            refinancingRate,
                            //rts,
                            //sAndP,
                            tradeBalance,
                            usdToRub
                        },
                        neuralNetworkName: "lstmNetworkChunk10000OneDay",
                        dataParameters: new DataParameter(new DateTime(2007, 12, 15), new DateTime(2014, 1, 3), TimeSpan.FromDays(1)),
                        learnParameters: new LearnParameters {
                            Parameters = new RecurentParameters(0.3, 0.005, 0.1),
                            CellParameters = new RecurentCellParameters[] {
                                new RecurentCellParameters(7, 1)
                            }
                        },
                        chunk: 1000000
                    )
                },
                {
                    "testSimpleNeuralNetwork40",
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
                        neuralNetworkName: "testSimpleNeuralNetwork40",
                        dataParameters: new DataParameter(new DateTime(2007, 12, 13), new DateTime(2014, 1, 3), TimeSpan.FromDays(1)),
                        learnParameters: new LearnParameters {
                            Parameters = new RecurentParameters(60, 0.01, 0.7),
                            CellParameters = new RecurentCellParameters[] {
                                new RecurentCellParameters(13, 13),
                                new RecurentCellParameters(13, 1)
                            }
                        },
                        usingNeuralNetwork: UsingNeuralNetwork.SimpleNeuralNetwork,
                        loadNetwork: false
                    )
                }
            };
        }
    }
}
