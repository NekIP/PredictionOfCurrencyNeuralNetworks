using DataManager;
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
                { "default",
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
                { "defaultRelative",
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
                { "defaultRelativePercentage",
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
                }
            };
        }
    }
}
