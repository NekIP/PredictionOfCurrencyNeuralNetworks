using DataAssistants;
using DataBase;
using DataBase.Entities;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DataManager {
    public interface IEconomicIndicatorCollector : IDataCollector<EconomicIndicator> { }
    public abstract class EconomicIndicatorCollector : DataCollector<EconomicIndicator>, IEconomicIndicatorCollector {
        public EconomicIndicatorCollector(string source, IRepository<EconomicIndicator> repository, bool isCache = true) :
            base(source, repository, isCache) { }

        public override async Task DownloadMissingData(DateTime before, TimeSpan step) {
            if (Repository.Table().Count() == 0) {
                ReadAndSaveData();
            }
            var minDeff = Repository.Table().Min(x => new TimeSpan(Math.Abs(before.Ticks - x.Date.Ticks)));
            if (minDeff > step) {
                ReadAndSaveData();
            }
        }

        protected void ReadAndSaveData() {
            var serializer = new Serializer();
            var newData = serializer.ReadFromTxt(Source, x => new EconomicIndicator() {
                Date = DateTime.ParseExact(x.Split(";")[0], "dd.MM.yyyyTHH:mm:ss", CultureInfo.InvariantCulture),
                Indicator = double.Parse(x.Split(";")[1])
            }).ToList();
            Repository.Table().AddRange(newData);
            Repository.SaveChanges();
            DeleteDuplicateEntries();
        }
    }
}
