using DataAssistants.Structs;
using DataBase;
using DataBase.Entities;
using DataBase.Repositories;
using DataManager;
using Microsoft.Extensions.Configuration;
using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Experiment {
    public class DataPreparer : Experiment {
        public override async void Run() {
            var config = GetConfiguration();
            var dataPreparer = new DataForNeuralNetworkCollector(
                new DataForNeuralNetworkRepository(config),
                new CAC40Collector(new CAC40Repository(config)),
                new DowJonesCollector(new DowJonesRepository(config)),
                new GdpPerCapitaPppCollector(new GdpPerCapitaPppRepository(config)),
                new GoldCollector(new GoldRepository(config)),
                new InflationCollector(new InflationRepository(config)),
                new MMVBCollector(new MMVBRepository(config)),
                new OliBrentCollector(new OliBrentRepository(config)),
                new OliLightCollector(new OliLightRepository(config)),
                new RefinancingRateCollector(new RefinancingRateRepository(config)),
                new RTSCollector(new RTSRepository(config)),
                new SAndP500Collector(new SAndPRepository(config)),
                new TradeBalanceCollector(new TradeBalanceRepository(config)),
                new UsdToRubCurrencyCollector(new UsdToRubCurrencyRepository(config)));

            //await dataPreparer.DownloadMissingData(DateTime.Now, TimeSpan.FromHours(1));

            // await dataPreparer.LearnExtrapolators(0.000007);

            //await dataPreparer.ProvideData(DateTime.Now, TimeSpan.FromHours(1));
            /*
            var usdToRubRepository = new UsdToRubCurrencyRepository(config);
            var extrapolate = new MultilayerPerceptron(new PerceptronParameters { LearningSpeed = 0.7, Moment = 0.1 }, 
                new SigmoidActivation(), 1, 3, 3, 1);
            //extrapolate.Save("extrapolate");
            extrapolate.Load("extrapolate");
            //var serializer = new Serializer();
            var elems = usdToRubRepository.Table().ToList();
            for (var h = 0; h < 100000; h++) {
                var error = 0.0;
                for (var i = 2; i < elems.Count; i++) {
                    var input = (Vector)new double[] { (elems[i - 1].Close - elems[i - 2].Close) / elems[i - 2].Close };
                    var ideal = (Vector)new double[] { (elems[i].Close - elems[i - 1].Close) / elems[i - 1].Close };
                    var learn = extrapolate.Learn(new NeuralNetworkData(input), new NeuralNetworkData(ideal));
                    error += learn.Error[0, 0, 0];
                }
                Console.WriteLine(error / (elems.Count - 2));
                extrapolate.Save("extrapolate");
                //serializer.Serialize(extrapolate, "test.json");
            }

            */

            Vector expectedValues = new double[] {
                dataPreparer.GetExpectedValue(x => x.D1),
                dataPreparer.GetExpectedValue(x => x.D2),
                dataPreparer.GetExpectedValue(x => x.D3),
                dataPreparer.GetExpectedValue(x => x.D4),
                dataPreparer.GetExpectedValue(x => x.D5),
                dataPreparer.GetExpectedValue(x => x.D6),
                dataPreparer.GetExpectedValue(x => x.D7),
                dataPreparer.GetExpectedValue(x => x.D8),
                dataPreparer.GetExpectedValue(x => x.D9),
                dataPreparer.GetExpectedValue(x => x.D10),
                dataPreparer.GetExpectedValue(x => x.D11),
                dataPreparer.GetExpectedValue(x => x.D12),
                dataPreparer.GetExpectedValue(x => x.D13)
            };

            Vector dispersions = new double[]  {
                dataPreparer.GetDispersion(expectedValues[0], x => x.D1),
                dataPreparer.GetDispersion(expectedValues[1], x => x.D2),
                dataPreparer.GetDispersion(expectedValues[2], x => x.D3),
                dataPreparer.GetDispersion(expectedValues[3], x => x.D4),
                dataPreparer.GetDispersion(expectedValues[4], x => x.D5),
                dataPreparer.GetDispersion(expectedValues[5], x => x.D6),
                dataPreparer.GetDispersion(expectedValues[6], x => x.D7),
                dataPreparer.GetDispersion(expectedValues[7], x => x.D8),
                dataPreparer.GetDispersion(expectedValues[8], x => x.D9),
                dataPreparer.GetDispersion(expectedValues[9], x => x.D10),
                dataPreparer.GetDispersion(expectedValues[10], x => x.D11),
                dataPreparer.GetDispersion(expectedValues[11], x => x.D12),
                dataPreparer.GetDispersion(expectedValues[12], x => x.D13)
            };

            Vector normalize(Vector x, Vector expectedValue, Vector dispersion) => (x - expectedValue) / Vector.Convert(dispersion, Math.Sqrt);
            Vector denormalize(Vector y, Vector expectedValue, Vector dispersion) => y * Vector.Convert(dispersion, Math.Sqrt) + expectedValue;

            Vector scaling(Vector x, Vector min, Vector max) => (x - min) * (1 - (-1)) / (max - min) + (-1);
            Vector descaling(Vector y, Vector min, Vector max) => ((max - min) * (y - (-1))) / (1 - (-1)) + min;

            var all = (await dataPreparer.List()).Select(x => x as DataForNeuralNetwork).ToList();
            var normalized = new List<DataForNeuralNetwork>();
            for (var i = 0; i < all.Count; i++) {
                normalized.Add(new DataForNeuralNetwork(all[i].Count) {
                    Date = all[i].Date,
                    Data = normalize(all[i].Data, expectedValues, dispersions)
                });
            }

            Vector mins = new double[] {
                normalized.Min(x => x.D1),
                normalized.Min(x => x.D2),
                normalized.Min(x => x.D3),
                normalized.Min(x => x.D4),
                normalized.Min(x => x.D5),
                normalized.Min(x => x.D6),
                normalized.Min(x => x.D7),
                normalized.Min(x => x.D8),
                normalized.Min(x => x.D9),
                normalized.Min(x => x.D10),
                normalized.Min(x => x.D11),
                normalized.Min(x => x.D12),
                normalized.Min(x => x.D13)
            };

            Vector maxs = new double[] {
                normalized.Max(x => x.D1),
                normalized.Max(x => x.D2),
                normalized.Max(x => x.D3),
                normalized.Max(x => x.D4),
                normalized.Max(x => x.D5),
                normalized.Max(x => x.D6),
                normalized.Max(x => x.D7),
                normalized.Max(x => x.D8),
                normalized.Max(x => x.D9),
                normalized.Max(x => x.D10),
                normalized.Max(x => x.D11),
                normalized.Max(x => x.D12),
                normalized.Max(x => x.D13)
            };

            var trainingSet = new List<DataForNeuralNetwork>();
            var testSet = new List<DataForNeuralNetwork>();
            var testSetChunk = 7;   // 1/7 * 100%
            var countItemsInARow = 4;

            for (var i = 0; i < normalized.Count; i++) {
                var v = i % (testSetChunk * countItemsInARow);
                if (v >= (testSetChunk - 1) * countItemsInARow &&
                    v < countItemsInARow * testSetChunk) {
                    testSet.Add(new DataForNeuralNetwork(normalized[i].Count) {
                        Date = normalized[i].Date,
                        Data = scaling(normalized[i].Data, mins, maxs)
                    });
                }
                else {
                    trainingSet.Add(new DataForNeuralNetwork(normalized[i].Count) {
                        Date = normalized[i].Date,
                        Data = scaling(normalized[i].Data, mins, maxs)
                    });
                }
            }

            var lstm = new Lstm(trainingSet.First().Count, 1, new RecurentParameters(0.5, 1, 0.5),
                new RecurentCellParameters(trainingSet.First().Count, trainingSet.First().Count),
                new RecurentCellParameters(trainingSet.First().Count, 1));
            for (var i = 0; i < trainingSet.Count - 1; i++) {
                if (trainingSet[i + 1].Date - trainingSet[i].Date == TimeSpan.FromHours(1)) {
                    var input = new Vector[] { trainingSet[i].Data };
                    var ideal = new Vector[] { new double[] { trainingSet[i + 1].Data.Last() } };
                    var learnResult = lstm.Learn(input, ideal);
                    Console.WriteLine(learnResult.errors[0]);
                }
            }



            //var item = await dataPreparer.GetData(new DateTime(2007, 10, 13, 16, 0, 0), TimeSpan.FromHours(1), true);
            /*var result = new List<double[]>();
            for (var i = new DateTime(2007, 10, 11); i < DateTime.Now; i = i.AddDays(3)) {
                var item1Task = Method(dataPreparer, i, result);
                var item2Task = Method(dataPreparer, i.AddDays(1), result);
                var item3Task = Method(dataPreparer, i.AddDays(2), result);
                await Task.WhenAll(item1Task, item2Task, item3Task);
            }*/

                /*var result = new List<double[]>();
                // new DateTime(2007, 10, 14)
                // 25.02.2012 01:00:00
                for (var i = new DateTime(2017, 6, 30); i < DateTime.Now; i = i.AddHours(1)) {
                    var item = await dataPreparer.GetData(i, TimeSpan.FromHours(1), true);
                    Console.WriteLine(i.ToString("dd.MM.yyyy HH:mm:ss") + "\t"
                        + string.Join("\t", item.Select(x => x == 0 ? " 0.00000000 " : " " + string.Format("{0:0.########}", x) + " ")));
                    result.Add(item);
                }*/

                /*var result = new List<double[]>();
                for (var i = new DateTime(2007, 10, 14); i < DateTime.Now; i = i.AddHours(3)) {
                    var item1Task = Method(dataPreparer, i, result);
                    var item2Task = Method(dataPreparer, i.AddHours(1), result);
                    var item3Task = Method(dataPreparer, i.AddHours(2), result);
                    await Task.WhenAll(item1Task, item2Task, item3Task);
                }*/
        }

        private async Task Method(DataForNeuralNetworkCollector dataPreparer, DateTime i, List<double[]> outer) {
            /*var item = await dataPreparer.GetData(i, TimeSpan.FromHours(1), true);
            Console.WriteLine(i.ToString("dd.MM.yyyy HH:mm:ss") + "\t"
                + string.Join("\t", item.Select(x => x == 0 ? " 0.00000000 " : " " + string.Format("{0:0.########}", x) + " ")));
            outer.Add(item);*/
        }

        private IConfiguration GetConfiguration() {
            var builder = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            return builder.Build();
        }
    }
}
