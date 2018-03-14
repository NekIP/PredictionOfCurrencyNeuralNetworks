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
            //var item = await dataPreparer.GetData(new DateTime(2007, 10, 13, 16, 0, 0), TimeSpan.FromHours(1), true);
            /*var result = new List<double[]>();
            for (var i = new DateTime(2007, 10, 11); i < DateTime.Now; i = i.AddDays(3)) {
                var item1Task = Method(dataPreparer, i, result);
                var item2Task = Method(dataPreparer, i.AddDays(1), result);
                var item3Task = Method(dataPreparer, i.AddDays(2), result);
                await Task.WhenAll(item1Task, item2Task, item3Task);
            }*/
            
            var result = new List<double[]>();
            // new DateTime(2007, 10, 14)
            // 25.02.2012 01:00:00
            for (var i = new DateTime(2017, 9, 5); i < DateTime.Now; i = i.AddHours(1)) {
                var item = await dataPreparer.GetData(i, TimeSpan.FromHours(1), true);
                Console.WriteLine(i.ToString("dd.MM.yyyy HH:mm:ss") + "\t"
                    + string.Join("\t", item.Select(x => x == 0 ? " 0.00000000 " : " " + string.Format("{0:0.########}", x) + " ")));
                result.Add(item);
            }

            /*var result = new List<double[]>();
            for (var i = new DateTime(2007, 10, 14); i < DateTime.Now; i = i.AddHours(3)) {
                var item1Task = Method(dataPreparer, i, result);
                var item2Task = Method(dataPreparer, i.AddHours(1), result);
                var item3Task = Method(dataPreparer, i.AddHours(2), result);
                await Task.WhenAll(item1Task, item2Task, item3Task);
            }*/
        }

        private async Task Method(DataForNeuralNetworkCollector dataPreparer, DateTime i, List<double[]> outer) {
            var item = await dataPreparer.GetData(i, TimeSpan.FromHours(1), true);
            Console.WriteLine(i.ToString("dd.MM.yyyy HH:mm:ss") + "\t"
                + string.Join("\t", item.Select(x => x == 0 ? " 0.00000000 " : " " + string.Format("{0:0.########}", x) + " ")));
            outer.Add(item);
        }

        private IConfiguration GetConfiguration() {
            var builder = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            return builder.Build();
        }
    }
}
