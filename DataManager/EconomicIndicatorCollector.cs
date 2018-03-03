using DataBase;
using DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManager {
    public interface IEconomicIndicatorCollector : IDataCollector<EconomicIndicator> { }
    public abstract class EconomicIndicatorCollector : DataCollector<EconomicIndicator>, IEconomicIndicatorCollector {
        public EconomicIndicatorCollector(string source, IRepository<EconomicIndicator> repository) :
            base(source, repository) { }

        public override bool TryGet(DateTime date, TimeSpan step, out EconomicIndicator result) {
            CheckConditionOnException(step < TimeSpan.FromDays(365),
                "Time step should be more than year");
            return base.TryGet(date, step, out result);
        }

        public override Task<List<EconomicIndicator>> List(DateTime from, DateTime to, TimeSpan step) {
            CheckConditionOnException(step < TimeSpan.FromDays(365),
                "Time step should be more than year");
            CheckConditionOnException(to - from < TimeSpan.FromDays(365),
                "The difference in time should be more than year");
            return base.List(from, to, step);
        }

        public override async Task DownloadMissingData(DateTime before, TimeSpan step) {
            var minDeff = Repository.Table().Min(x => new TimeSpan(Math.Abs(before.Ticks - x.Date.Ticks)));
            if (minDeff < step) {
                var serializer = new Serializer();
                var newData = (await serializer.ReadFromTxt(Source, x => new EconomicIndicator() {
                    Date = DateTime.Parse(x.Split(";")[0]),
                    Value = double.Parse(x.Split(";")[1])
                })).ToList();
                Repository.Table().AddRange(newData);
                Repository.SaveChanges();
                DeleteDuplicateEntries(new EconomicIndicatorComparer());
            }
        }

        class EconomicIndicatorComparer : IEqualityComparer<EconomicIndicator> {
            public bool Equals(EconomicIndicator x, EconomicIndicator y) => x.Date == y.Date;
            public int GetHashCode(EconomicIndicator obj) => obj.Date.GetHashCode();
        }
    }
}
