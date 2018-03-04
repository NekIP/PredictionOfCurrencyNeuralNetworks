using DataBase;
using DataBase.Entities;
using DataBase.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManager {
    public interface IDataPreparerForNeuralNetwork {
        Task<double[]> GetData(DateTime date, TimeSpan step);
        void ProvideData(DateTime date, TimeSpan step);
    }
    public class DataPreparerForNeuralNetwork : IDataPreparerForNeuralNetwork {
        public List<DataContextFinam> Finams { get; set; }
        public List<DataContextEconomicIndicator> EconomicIndicators { get; set; }

        public DataPreparerForNeuralNetwork(IConfiguration configuration) {
            Finams = new List<DataContextFinam> {
                new DataContextFinam(new UsdToRubCurrencyCollector(new UsdToRubCurrencyRepository(configuration)), IfDataNotExist.Aproximated),
                new DataContextFinam(new CAC40Collector(new CAC40Repository(configuration)), IfDataNotExist.Aproximated),
                new DataContextFinam(new CSI200Collector(new CSI200Repository(configuration)), IfDataNotExist.Aproximated),
                new DataContextFinam(new DowJonesCollector(new DowJonesRepository(configuration)), IfDataNotExist.Aproximated),
                new DataContextFinam(new GoldCollector(new GoldRepository(configuration)), IfDataNotExist.Aproximated),
                new DataContextFinam(new MMVBCollector(new MMVBRepository(configuration)), IfDataNotExist.Aproximated),
                new DataContextFinam(new OliBrentCollector(new OliBrentRepository(configuration)), IfDataNotExist.Aproximated),
                new DataContextFinam(new OliLightCollector(new OliLightRepository(configuration)), IfDataNotExist.Aproximated),
                new DataContextFinam(new RTSCollector(new RTSRepository(configuration)), IfDataNotExist.Aproximated),
                new DataContextFinam(new SAndP500Collector(new SAndPRepository(configuration)), IfDataNotExist.Aproximated)
            };
            EconomicIndicators = new List<DataContextEconomicIndicator> {
                new DataContextEconomicIndicator(new GdpPerCapitaPppCollector(new GdpPerCapitaPppRepository(configuration)), IfDataNotExist.TakeLastNear),
                new DataContextEconomicIndicator(new InflationCollector(new InflationRepository(configuration)), IfDataNotExist.TakeLastNear),
                new DataContextEconomicIndicator(new RefinancingRateCollector(new RefinancingRateRepository(configuration)), IfDataNotExist.TakeLastNear),
                new DataContextEconomicIndicator(new TradeBalanceCollector(new TradeBalanceRepository(configuration)), IfDataNotExist.Aproximated)
            };
        }

        public async Task<double[]> GetData(DateTime date, TimeSpan step) {
            var result = new List<double>();
            foreach (var context in Finams) {
                if (context.Collector.TryGet(date, step, out var data)) {
                    result.Add(context.Converter(data));
                }
                else {
                    switch (context.IfDataNotExist) {
                        case IfDataNotExist.TakeLastNear:
                            var last = (await context.Collector.List(date - TimeSpan.FromDays(365) * 3, date, step))
                                .LastOrDefault();
                            if (last is null) {
                                return null;
                            }
                            result.Add(context.Converter(last));
                            break;
                        case IfDataNotExist.Aproximated:
                            for (var i = step * 2; i < step * 96; i += step * 12) {
                                var values = await context.Collector.List(date - i, date, step);
                                if (values.Count > 2) {
                                    var list = await context.Collector.List(date + step, date + i, step);
                                    if (list.Count > 0) {
                                        values.AddRange(list);
                                        var interpolator = 
                                            new SplineInterpolation(values.ToDictionary(x => (double)x.Date.Ticks, x => context.Converter(x)));
                                        var need = interpolator.GetValue(date.Ticks);
                                        result.Add(need);
                                    }
                                    else {
                                        var extrapolator = new LagrangeApproximation(values
                                            .Select(x => new KeyValuePair<double, double>(x.Date.Ticks, context.Converter(x)))
                                            .ToList()
                                        );
                                        var need = extrapolator.GetValue(date.Ticks);
                                        result.Add(need);
                                    }
                                }
                            }
                            break;
                        case IfDataNotExist.Crush:
                            return null;
                    }
                }
            }
            return result.ToArray();
        }

        public void ProvideData(DateTime date, TimeSpan step) {
            Finams.ForEach(async x => await x.Collector.DownloadMissingData(date, step));
            EconomicIndicators.ForEach(async x => await x.Collector.DownloadMissingData(date, step));
        }

        public class DataContext {
            public IfDataNotExist IfDataNotExist { get; private set; }
            public DataContext(IfDataNotExist ifDataNotExist) {
                IfDataNotExist = ifDataNotExist;
            }
        }

        public class DataContextFinam : DataContext {
            public IDataCollector<Product> Collector { get; private set; }
            public Func<Product, double> Converter { get; private set; } = x => x.Close;
            public DataContextFinam(IDataCollector<Product> collector, 
                IfDataNotExist ifDataNotExist, Func<Product, double> converter) : 
                base(ifDataNotExist) {
                Collector = collector;
                Converter = converter;
            }
            public DataContextFinam(IDataCollector<Product> collector, 
                IfDataNotExist ifDataNotExist) :
                base(ifDataNotExist) {
                Collector = collector;
            }
        }

        public class DataContextEconomicIndicator : DataContext{
            public IDataCollector<EconomicIndicator> Collector { get; private set; }
            public Func<EconomicIndicator, double> Converter { get; private set; } = x => x.Indicator;
            public DataContextEconomicIndicator(IDataCollector<EconomicIndicator> collector, 
                IfDataNotExist ifDataNotExist, Func<EconomicIndicator, double> converter) :
                base(ifDataNotExist) {
                Collector = collector;
                Converter = converter;
            }
            public DataContextEconomicIndicator(IDataCollector<EconomicIndicator> collector, 
                IfDataNotExist ifDataNotExist) :
                base(ifDataNotExist) {
                Collector = collector;
            }
        }

        public enum IfDataNotExist {
            TakeLastNear,
            Aproximated,
            Crush
        }
    }
}
