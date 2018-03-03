using DataBase.Repositories;

namespace DataManager {
    public interface IInflationCollector : IEconomicIndicatorCollector { }
    public class InflationCollector : EconomicIndicatorCollector, IInflationCollector {
        public InflationCollector(IInflationRepository repository) : 
            base("Data/inflation.csv", repository) { }
    }
}
