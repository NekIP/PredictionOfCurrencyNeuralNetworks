using DataManager;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Experiment {
    public class DataPreparer : Experiment {
        public override void Run() {
            var config = GetConfiguration();
            var dataPreparer = new DataPreparerForNeuralNetwork(config);
            dataPreparer.ProvideData(DateTime.Now, TimeSpan.FromHours(1));
        }

        private IConfiguration GetConfiguration() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            return builder.Build();
        }
    }
}
