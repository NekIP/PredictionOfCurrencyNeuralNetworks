using DataManager;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Experiment {
    public class UsdToRubCurrencyExperiment : Experiment {
        public override async void Run() {
            var configuration = GetConfiguration();
            var collector = new UsdToRubCurrencyCollector(configuration);
            await collector.DownloadMissingData(DateTime.Now, TimeSpan.FromHours(1));
        }

        private IConfiguration GetConfiguration() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            return builder.Build();
        }
    }
}
