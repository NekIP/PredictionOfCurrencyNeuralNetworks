using DataBase.Repositories;
using DataManager;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace Experiment {
    public class UsdToRubCurrencyExperiment : Experiment {
        public override async void Run() {
            var configuration = GetConfiguration();
            var repository = new UsdToRubCurrencyRepository(configuration);
            var collector = new UsdToRubCurrencyCollector(repository);
            //collector.GlobalFrom = DateTime.Now - TimeSpan.FromDays(20);
            var client = new WebClient();
            await collector.DownloadMissingData(DateTime.Now, TimeSpan.FromHours(1));
            var res1 = collector.List(DateTime.Now - TimeSpan.FromHours(96), DateTime.Now, TimeSpan.FromHours(1));
            var res2 = collector.List(DateTime.Now - TimeSpan.FromHours(96), DateTime.Now, TimeSpan.FromHours(2));
            var res24 = collector.List(DateTime.Now - TimeSpan.FromHours(96), DateTime.Now, TimeSpan.FromHours(24));
            // var y = collector.TryGet(new DateTime(2018, 1, 31, 14, 0, 0), TimeSpan.FromHours(1), out var result);
            // var t = result;
        }

        private IConfiguration GetConfiguration() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            return builder.Build();
        }
    }
}
